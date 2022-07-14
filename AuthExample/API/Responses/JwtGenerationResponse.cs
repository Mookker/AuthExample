namespace AuthExample.API.Responses
{
    public record JwtGenerationResponse
    {
        public string Token { get; init; }
        public DateTime Expiration { get; init; }
    }
}
