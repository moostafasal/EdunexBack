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
        public string address { get; set; }//1
        public DateTime birthDate { get; set; }//2
        public string? city { get; set; }//6
        public string FirstName { get; set; }//5
        public string PhoneNumber { get; set; }//9

        public string LastName { get; set; }//7
        public string ParentPhoneNumber { get; set; }//4
        public string Religion { get; set; }//8
        public int? LevelId { get; set; }//3
        public string? Gender { get; set; }//9

    }
}
