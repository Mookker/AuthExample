namespace AuthExample.API.Requests
{
    public record BlockUserRequest
    {
        public bool IsBlocked { get; init; }
    }
}