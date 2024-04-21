using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;

namespace AuthenticationMechanism.Services
{
    public class CloudinaryService:ICloudinaryService
    {

        private readonly Cloudinary _cloudinary;


        public CloudinaryService(Cloudinary cloudinary)

        {

            _cloudinary = cloudinary;

        }


        public async Task<ImageUploadResult> UploadAsync(IFormFile file)

        {

            using (var stream = new MemoryStream())

            {

                await file.CopyToAsync(stream);

                var uploadParams = new ImageUploadParams

                {

                    File = new FileDescription(file.Name, stream),

                    Transformation = new Transformation().Width(150).Height(150).Crop("thumb")

                };


                return await _cloudinary.UploadAsync(uploadParams);

            }

        }
    }
}
