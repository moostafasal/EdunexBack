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
        public string? NationalId { get; set; }
        public string gender { get; set; }
        public string? Address { get; set; }
        public string? AboutMe { get; set; }
        public string? AccountNote { get; set; }
        public int Age { get; set; }
        public string subject { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? FacebookAccount { get; set; }
        public string? City { get; set; }
        public string? experience { get; set; }


        public String? status { get; set; }



    }
    public class UpdateAllTeacherDto
    {

        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }//2
                                           
        public DateTime DateOfBirth { get; set; }//3
        public string? FacebookAccount { get; set; }//1
        public string? City { get; set; }//7
        public string gender { get; set; }

        public string FirstName { get; set; }//5
        public string LastName { get; set; }//6
    }

    public class UpdatePendingTeacherDto
    {
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        //public string? nationalId { get; set; }
        public string? subject { get; set; }
        public string? experience { get; set; }
        public string? aboutMe { get; set; }

    }
}
