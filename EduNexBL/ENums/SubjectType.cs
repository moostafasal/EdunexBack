using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.ENums
{
    public enum SubjectType
    {
        [Display(Name = "Literature")]
        Literature,
        [Display(Name = "Scientific")]
        Scientific,
        [Display(Name = "General")]
        General
    }
}
