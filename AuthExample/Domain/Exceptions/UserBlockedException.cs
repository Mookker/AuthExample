namespace AuthExample.Domain.Exceptions
{
    public class UserBlockedException : ArgumentException
    {
        public UserBlockedException() : base("User is blocked") { }
    }
}
