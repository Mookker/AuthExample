namespace AuthExample.Infrastructure.Exceptions
{
    public class UserNotFoundException : ArgumentException
    {
        public UserNotFoundException(): base("User not found")
        {

        }
    }
}
