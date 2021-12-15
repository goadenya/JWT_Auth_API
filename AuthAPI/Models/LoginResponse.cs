namespace AuthAPI.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public bool LoginIsSuccess { get; set; }
        public string ResponseMessage { get; set; }
    }
}
