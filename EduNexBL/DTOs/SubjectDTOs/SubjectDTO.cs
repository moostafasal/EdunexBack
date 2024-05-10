using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduNexBL.ENums;

namespace EduNexBL.DTOs.SubjectDTOs
{
    public class SubjectDTO
    {

        [Required]
        public string SubjectName { get; set; }
        [EnumDataType(typeof(SubjectType))]
        public string SubjectType { get; set; }

        [ForeignKey("Level")]
        public int LevelId { get; set; }
        public Level? Level { get; set; }
    }
}
