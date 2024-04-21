using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Validations
{
    internal class DurationWithinTimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult(ErrorMessage);
            if (value is int)
            {
                var startTime = (DateTime)validationContext.ObjectType.GetProperty("StartDateTime")?.GetValue(validationContext.ObjectInstance, null);
                var endTime = (DateTime)validationContext.ObjectType.GetProperty("EndDateTime")?.GetValue(validationContext.ObjectInstance, null);
                var duration = (int)value;

                var timeDifference = endTime - startTime;
                var maxDuration = (int)timeDifference.TotalMinutes;

                if (duration > maxDuration)
                {
                    return new ValidationResult($"Duration cannot be greater than the time difference between start and end times ({maxDuration} minutes).");
                }

                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
