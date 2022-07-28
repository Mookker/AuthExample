namespace AuthExample.Domain.Exceptions
{
    public class InvalidPasswordException : ArgumentException
    {
        public InvalidPasswordException() : base("Invalid password provided")
        {

        }
    }
}
