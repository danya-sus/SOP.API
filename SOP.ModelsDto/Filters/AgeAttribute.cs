using System;
using System.ComponentModel.DataAnnotations;

namespace SOP.API.Filters
{
    public class AgeAttribute : ValidationAttribute
    {
        public int MaxAge { get; }
        public string GetMaxAgeErrorMessage() => "Date of registration from the future!";
        public string GetMinAgeErrorMessage() => "The vehicle is too old!";

        public AgeAttribute(int maxAge)
        {
            this.MaxAge = maxAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var age = DateTime.Now.Year - (int)value;

            if (age < 0) return new ValidationResult(GetMinAgeErrorMessage());
            else if (age > MaxAge) return new ValidationResult(GetMaxAgeErrorMessage());

            return ValidationResult.Success;
        }
    }
}
