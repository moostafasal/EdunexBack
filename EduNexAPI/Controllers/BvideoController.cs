using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class VideoLibraryController : ControllerBase
{
    private readonly string _apiKey = "6bb41be0-4ef0-44df-a2a6dc1cade2-b382-4ec0";
    private readonly string _libraryId = "235821";

    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo(IFormFile videoFile)
    {
        if (videoFile == null || videoFile.Length == 0)
        {
            return BadRequest("No video file provided.");
        }

        // Create a video object in BunnyCDN video library
        var videoId = await CreateVideoObjectAsync(videoFile.FileName);

        // Upload the video file to the created video object
        var uploadUrl = $"https://video.bunnycdn.com/library/{_libraryId}/videos/{videoId}";
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            using (var content = new StreamContent(videoFile.OpenReadStream()))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                using (var response = await httpClient.PutAsync(uploadUrl, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return StatusCode(500, "Failed to upload video.");
                    }
                }
            }
        }

        // Return the video URL
        var videoURL = $"https://{_libraryId}.b-cdn.net/{videoId}";
        return Ok(new { message = "Video uploaded successfully.", url = videoURL });
    }

    private async Task<string> CreateVideoObjectAsync(string title)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            using (var response = await httpClient.PostAsync(
                $"https://video.bunnycdn.com/library/{_libraryId}/videos",
                new StringContent($"{{\"title\":\"{title}\"}}", System.Text.Encoding.UTF8, "application/json")))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to create video object. Response: {errorResponse}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                return ExtractVideoId(responseBody);
            }
        }
    }

    private string ExtractVideoId(string responseBody)
    {
        // Extract the videoId from the response body
        // This is a simplified example, you should use a JSON parser to handle the response properly
        var videoIdStart = responseBody.IndexOf("guid\":\"") + "guid\":\"".Length;
        var videoIdEnd = responseBody.IndexOf("\"", videoIdStart);
        return responseBody.Substring(videoIdStart, videoIdEnd - videoIdStart);
    }
}
