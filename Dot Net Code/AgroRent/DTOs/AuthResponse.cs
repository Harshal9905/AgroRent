namespace AgroRent.DTOs
{
    public class AuthResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public AuthResponse() { }

        public AuthResponse(string message, string token)
        {
            Message = message;
            Token = token;
        }
    }
}
