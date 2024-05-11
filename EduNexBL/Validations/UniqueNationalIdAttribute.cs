using EduNexDB.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Validations
{
    public class UniqueNationalIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (EduNexContext)validationContext.GetService(typeof(EduNexContext));
            var nationalId = (string)value;

            if (dbContext.Students.Any(s => s.NationalId == nationalId))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
