using Azure.Core;
using DocumentFormat.OpenXml.Vml.Office;
using GeminiApp.Data;
using GeminiApp.Entities;
using GeminiApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using GeminiApp.DTOs;
using static GeminiApp.Controllers.ContentController;
using System.Diagnostics;

namespace GeminiApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly GeminiService _geminiService;
        private readonly GeminiDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public ContentController(GeminiService geminiService, GeminiDbContext context, JwtTokenHelper jwtTokenHelper)
        {
            _geminiService = geminiService;
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
        }

        // This endpoint takes user input, sends it to Gemini API, saves the interaction, and returns the response
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] ContentRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.InputText))
            {
                return BadRequest("Input text is required.");
            }

            int userId = requestDto.UserId;
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return BadRequest("User does not exist.");
            }

            var previousMessages = await _context.ContentRequests
                .Where(cr => cr.UserId == userId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();

            var context = string.Join("\n", previousMessages.Select(msg => $"{msg.Text}\n{msg.GeneratedResponse}"));
            // Call Gemini API with the provided input wrapped in a string array
            var jsonResponse = await _geminiService.GenerateContentAsync(requestDto.InputText, context);
            // Extract the model's generated answer from the Gemini response
            var answer = _geminiService.ExtractAnswer(jsonResponse);

            // Save the full request-response pair to the database
            var contentRequest = new ContentRequest
            {
                Text = requestDto.InputText,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                GeneratedResponse = answer
            };
            _context.ContentRequests.Add(contentRequest);
            await _context.SaveChangesAsync();

            // Return a structured DTO instead of anonymous object
            var responseDto = new ContentResponseDto
            {
                InputText = requestDto.InputText,
                Answer = answer,
                Timestamp = contentRequest.CreatedAt
            };

            return Ok(responseDto);
        }

        // Returns all past requests made by the current user
        [HttpGet("history")]
        public async Task<IActionResult> GetUserContentHistory()
        {
            // Get the user ID from the token
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            // Fetch all content requests for this user
            var history = await _context.ContentRequests
                .Where(cr => cr.UserId == userId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToListAsync();

            return Ok(history);
        }
    }
}
