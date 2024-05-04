using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class StudentCourse
    {
        public int Id { set; get; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime Enrolled { get; set; }

        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }


    }
}
