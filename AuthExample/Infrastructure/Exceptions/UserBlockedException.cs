namespace AuthExample.Infrastructure.Exceptions
{
    public class UserBlockedException: ArgumentException
    {
        public UserBlockedException(): base("User is blocked") { }
    }
}
