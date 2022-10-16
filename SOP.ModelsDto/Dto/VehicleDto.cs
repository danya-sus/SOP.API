using Microsoft.AspNetCore.Mvc;
using SOP.API.Filters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SOP.ModelsDto.Dto
{
    public class VehicleDto
    {
		[HiddenInput(DisplayValue = false)]
		public string ModelCode { get; set; }

		private string registration;

		private static string NormalizeRegistration(string reg)
		{
			return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
		}

		[Required]
		[DisplayName("Registration Plate")]
		public string Registration
		{
			get => NormalizeRegistration(registration);
			set => registration = value;
		}

		[Required]
		[DisplayName("Year of first registration")]
        [Age(70)]
		public int Year { get; set; }

		[Required]
		[DisplayName("Color")]
		public string Color { get; set; }
	}
}
