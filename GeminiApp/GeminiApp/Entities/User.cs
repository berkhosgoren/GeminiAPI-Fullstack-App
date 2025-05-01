namespace GeminiApp.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class ContentRequest
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string GeneratedResponse { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
