using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduNexDB.Entites;

namespace EduNexBL.DTOs
{
    public class TeacherDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfilePhoto { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalId { get; set; }
        public Gender gender { get; set; }
        public string? Address { get; set; }
        //public string? nationalId { get; set; }
        public string? AboutMe { get; set; }
        public string? AccountNote { get; set; }
        public string Age { get; set; }


    }
}
