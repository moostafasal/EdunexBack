using EduNexDB.Entites;
using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.AuthDtos
{
    public class RegisterAdminDto
    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Email")]

        public string Email { get; set; }

        [Required]

        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }


        [Required]

        [Display(Name = "Religion")]

        public string Religion { get; set; }


        [Required]

        [Display(Name = "Date of Birth")]

        public DateTime DateOfBirth { get; set; }


        [Required]

        [Display(Name = "City")]

        public int CityId { get; set; }


        [Required]

        [Display(Name = "Address")]

        public string Address { get; set; }


        [Required]

        [Display(Name = "National ID")]

        public string NationalId { get; set; }


        [Required]

        [Display(Name = "Password")]

        public string Password { get; set; }


        [Required]

        [Display(Name = "Confirm Password")]

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }
    }
}
