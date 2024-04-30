using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs
{
    public class S3respons
    {

        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "";
        public string FileUrl { get; set; } = ""; // New property to hold the URL of the uploaded file
    }
}
