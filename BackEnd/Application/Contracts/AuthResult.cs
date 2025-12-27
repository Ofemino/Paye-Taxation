namespace Paye.Application.Contracts
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
    }

    public class Result
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
