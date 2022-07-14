namespace AuthExample.API.Requests
{
    public record JwtGenerationRequest
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
