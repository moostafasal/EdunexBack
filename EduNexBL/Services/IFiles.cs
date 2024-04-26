using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace AuthenticationMechanism.Services
{
    public interface IFiles
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<string> UploadRawAsync(IFormFile file);
        Task<string> UploadVideoAsync(IFormFile file);
    }
}
