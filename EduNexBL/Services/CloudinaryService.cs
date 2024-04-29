using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AuthenticationMechanism.Services
{
    public class CloudinaryService : IFiles
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("Cloudinary");

            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            return await UploadFileAsync(file, "images");
        }

        public async Task<string> UploadRawAsync(IFormFile file)
        {
            return await UploadFileAsync(file, "raw");
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            return await UploadFileAsync(file, "videos");
        }

        private async Task<string> UploadFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            if (string.IsNullOrWhiteSpace(folder))
                folder = "files"; // Default folder name if not provided

            using (var stream = file.OpenReadStream())
            {
                string uniqueFileName = GenerateUniqueFileName(file.FileName);
               

                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(uniqueFileName, stream),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true,
                    Folder = folder
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    return $"Error uploading file: {uploadResult.Error.Message}";

                return uploadResult.SecureUrl.ToString();
            }
        }

        private string GenerateUniqueFileName(string fileName)
        {
            return $"{DateTime.Now:yyyyMMddHHmmssfff}_{fileName}";
        }
    }
}
