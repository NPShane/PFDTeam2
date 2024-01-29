using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace PFDTeam2.Controllers
{
	public class FaceDetectionController : Controller
	{
		private readonly HttpClient _httpClient;

		public FaceDetectionController()
		{
			_httpClient = new HttpClient();
		}

		[HttpGet]
		public async Task<IActionResult> StartFaceDetection()
		{
			try
			{
				var result = await _httpClient.GetStringAsync("https://ecee-2406-3003-206b-223-4597-6120-aef2-ad51.ngrok-free.app/video_feed");
				// Process the result or display it in your C# web application

				return View("~/Views/FaceDetection/Index.cshtml");
			}
			catch (HttpRequestException ex)
			{
				// Handle exceptions (e.g., server not reachable)
				return View("~/Views/Shared/Error.cshtml", $"Error: {ex.Message}");
			}
		}
	}
}
