using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SOP.Models.Entities
{
    public class Owner
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly Birthday { get; set; }
        public string? VehicleRegistration { get; set; }

        [JsonIgnore]
        public virtual Vehicle? Vehicle { get; set; }
    }
}
