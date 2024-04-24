using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.AuthDtos
{
    public class LoginUserDto
    {
        [Required]

        [EmailAddress]

        public string Email { get; set; }


        [Required]

        public string Password { get; set; }

        [Display(Name = "Remember me?")]

        public bool RememberMe { get; set; }=false;
    }
}
