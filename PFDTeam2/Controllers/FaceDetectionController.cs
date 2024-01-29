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
				var result = await _httpClient.GetStringAsync("http://192.168.112.1:5000/video_feed");
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
