using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SOP.ModelsDto.Dto
{
    public class OwnerDto
    {
        [HiddenInput(DisplayValue = false)]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string VehicleRegistration { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required]
        [DisplayName("Birthday")]
        public string Birthday { get; set; }       
    }
}
