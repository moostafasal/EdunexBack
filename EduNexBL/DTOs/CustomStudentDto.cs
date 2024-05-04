using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs
{
    public class CustomStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ParentPhoneNumber { get; set; }
        public string Religion { get; set; }
        public int? LevelId { get; set; }
        public Gender ?Gender { get; set; }

    }
}
