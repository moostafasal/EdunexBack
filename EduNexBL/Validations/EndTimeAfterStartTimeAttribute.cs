using System.ComponentModel.DataAnnotations;

namespace EduNexBL.Validations
{
    internal class EndTimeAfterStartTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult(ErrorMessage);
            if (value is DateTime)
            {
                var endDateTime = (DateTime)value;
                var startDateTime = (DateTime)validationContext.ObjectType.GetProperty("StartDateTime")?.GetValue(validationContext.ObjectInstance, null);

                if (endDateTime <= startDateTime)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}