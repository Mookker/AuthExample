namespace AuthExample.Domain.Exceptions
{
    public class UserNotFoundException : ArgumentException
    {
        public UserNotFoundException() : base("User not found")
        {

        }
    }
}
