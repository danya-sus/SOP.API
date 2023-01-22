
namespace SOP.Messages.Messages
{
    public class NewOwnerMessage
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }
        public string? VehicleRegistration { get; set; }
        public DateTime ListedAtUtc { get; set; }
    }

    public class NewOwnerAgeMessage : NewOwnerMessage
    {
        public uint Years { get; set; }
        public uint Months { get; set; }
        public uint Days { get; set; }

        public NewOwnerAgeMessage() { }

        public NewOwnerAgeMessage(NewOwnerMessage message, uint years, uint months, uint days)
        {
            this.Email = message.Email;
            this.Name = message.Name;
            this.Surname = message.Surname;
            this.Birthday = message.Birthday;
            this.VehicleRegistration = message.VehicleRegistration;
            this.ListedAtUtc = message.ListedAtUtc;
            this.Years = years;
            this.Months = months;
            this.Days = days;
        }
    }
}
