namespace AuthExample.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public bool? IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
    }
}
