using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{

    public enum Gender
    {
        Male, Female
    }
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        

        [Required]
        public string PhoneNumber { get; set; }

        

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender gender { get; set; } 

        public string ?Address { get; set; }

        public string? NationalId { get; set; }
        //[ForeignKey("city")]
        //public int? CityId { get; set; }
        //public City? city { get; set; }

        public string? City { get; set; }
        
       
    }
}

