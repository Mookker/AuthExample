namespace AuthExample.API.Responses
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
        public Guid TraceId { get; set; }

        public int Code { get; set; }
    }
}
