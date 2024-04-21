using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Validations
{
    internal class StartTimeAfterNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null) return new ValidationResult(ErrorMessage);
            if (value is DateTime)
            {
                var startDateTime = (DateTime)value;

                if (startDateTime <= DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
