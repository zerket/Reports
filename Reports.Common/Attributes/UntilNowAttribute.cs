using System;
using System.ComponentModel.DataAnnotations;

namespace Reports.Attributes
{
    public class UntilNowAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => "Date must be past now";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date, DateTime.Now) < 0)
                return new ValidationResult(GetErrorMessage());
            else 
                return ValidationResult.Success;
        }
    }
}
