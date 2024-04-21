using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace AuthenticationMechanism.Services
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadAsync(IFormFile file);
    }
}
