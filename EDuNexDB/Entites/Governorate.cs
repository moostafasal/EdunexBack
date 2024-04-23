//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EduNexDB.Entites
//{
//    public class Governorate:BaseEntity
//    {
//        [Required]
//        [Column(TypeName = "nvarchar(50)")]
//        public string GovernorateNameAr { get; set; }

//        [Required]
//        [Column(TypeName = "nvarchar(50)")]
//        public string GovernorateNameEn { get; set; }

//        public ICollection<City> Cities { get; set; }
//    }
//}
