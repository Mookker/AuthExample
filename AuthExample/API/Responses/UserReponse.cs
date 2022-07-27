namespace AuthExample.API.Responses
{
    public record UserResponse
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool? IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
    }
}
