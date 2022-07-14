namespace AuthExample.Domain.Entities
{
    public class Car : BaseEntity
    {
        public string? Maker { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public int? Milage { get; set; }
    }
}
