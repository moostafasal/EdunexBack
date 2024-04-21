using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class City:BaseEntity
    {
        [Required]
        public int GovernorateId { get; set; }

        [ForeignKey("GovernorateId")]
        public Governorate Governorate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string CityNameAr { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string CityNameEn { get; set; }


        public ICollection<ApplicationUser> Appusers { get; set; }

    }
}
