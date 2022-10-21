
namespace SOP.Messages.Messages
{
    public class NewOwnerMessage
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly Birthday { get; set; }
        public string? VehicleRegistration { get; set; }
        public DateTime ListedAtUtc { get; set; }
    }
}
