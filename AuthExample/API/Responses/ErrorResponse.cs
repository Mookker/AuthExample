namespace AuthExample.API.Responses
{
    public record ErrorResponse
    {
        public ErrorResponse()
        {
            TraceId = Guid.NewGuid();
        }
        public string ErrorMessage { get; init; }
        public Guid TraceId { get; private set; }

        public int Code { get; init; }
    }
}
