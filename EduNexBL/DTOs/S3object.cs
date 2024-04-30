using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs
{
    public class S3object
    {
    public string Name { get; set; } = null!;
    public MemoryStream InputStream { get; set; } = null!;
    public string BucketName { get; set; } = null!;

    }
}
