using EduNexBL.DTOs;
using EduNexBL.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AWSController : Controller
    {
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly ILogger<AWSController> _logger;

        public AWSController(ILogger<AWSController> logger,
        IConfiguration config,
        IStorageService storageService)
        {
            _logger = logger;
            _config = config;
            _storageService = storageService;

        }

        [HttpPost(Name = "UploadFile")]

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            // Process file
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileExt = Path.GetExtension(file.FileName);
            var docName = $"{Guid.NewGuid}.{fileExt}";
            // call server

            var s3Obj = new S3object()
            {
                BucketName = "edunex-profile-photo",
                InputStream = memoryStream,
                Name = docName
            };

            var cred = new AwsCredentials()
            {
                AwsKey = _config["AwsConfiguration:AWSAccessKey"],
                AwsSecretKey = _config["AwsConfiguration:AWSSecretKey"]
            };

            var result = await _storageService.UploadFileAsync(s3Obj, cred);
            // 
            return Ok(result);

        }

   
    }
}
