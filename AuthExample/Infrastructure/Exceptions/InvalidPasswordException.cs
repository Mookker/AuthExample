namespace AuthExample.Infrastructure.Exceptions
{
    public class InvalidPasswordException: ArgumentException
    {
        public InvalidPasswordException():base("Invalid password provided")
        {

        }
    }
}
