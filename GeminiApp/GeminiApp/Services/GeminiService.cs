using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace GeminiApp.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        // Sends a content-generation request to Gemini API
        public async Task<string> GenerateContentAsync(string[] input)
        {
            // Gemini API endpoint (v1)
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}";

            // Request body structure expected by Gemini
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = input.Select(i => new { text = i }).ToArray()
                    }
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Send the request and get the response body
            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            return responseBody;
        }

        // Extracts the model's generated text from the Gemini JSON response
        public string ExtractAnswer(string jsonResponse)
        {
            dynamic result = JsonConvert.DeserializeObject(jsonResponse);
            try
            {
                return result.candidates[0].content.parts[0].text;
            }
            catch
            {
                return "No answer found.";
            }
        }
    }
}