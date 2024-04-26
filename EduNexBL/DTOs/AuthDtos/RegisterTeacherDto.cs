using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.AuthDtos
{
    public class RegisterTeacherDto
    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]

        [Display(Name = "LastName")]
        public string LastName { get; set; }


        [Required]
        public string Email { get; set; }



       // [Display(Name = "Profile Photo")]

      //  public IFormFile ProfilePhoto { get; set; }


        [Required]

        [Display(Name = "Phone Number")]

        public string PhoneNumber { get; set; }


        [Required]

        [Display(Name = "Facebook Account")]

        public string FacebookAccount { get; set; }


        //[Required]

        //[Display(Name = "Religion")]

        //public string Religion { get; set; }


        [Required]

        [Display(Name = "Date of Birth")]

        public DateTime DateOfBirth { get; set; }


        //[Required]

        //public int CityId { get; set; }

        [Required]
        public string gender { get; set; }
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

        public IFormFile file { get; set; }
    }
}
