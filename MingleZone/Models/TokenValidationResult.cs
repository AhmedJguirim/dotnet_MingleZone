namespace MingleZone.Models
{
    public class TokenValidationResult
    {
        public int UserId { get; set; }
        public bool IsTokenValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
