using EduNexBL.Validations;
using EduNexDB.Entites;
using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.AuthDtos
{
    public class StudentRegisterDto
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }

        [DataType(DataType.Password)]

        [Display(Name = "Confirm password")]

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }

       

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]

        [EmailAddress]

        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }


        [Required]
        public string gender { get; set; }
        

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        //[Required]

        //public string NationalId { get; set; }

        [Required]

        public string ParentPhoneNumber { get; set; }


        [Required]

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]

        [DataType(DataType.Password)]

        [Display(Name = "Password")]

        public string Password { get; set; }


        [Required]

        public string PhoneNumber { get; set; }

        [Required]
        public string Religion { get; set; }


        //[Required]

        [Required(ErrorMessage = "National ID is required")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be 14 digits")]
        [UniqueNationalId(ErrorMessage = "National ID must be unique")]
        public string NationalId { get; set; }
        //public int CityId { get; set; }
        public int LevelId { get; set; }
    }
}
