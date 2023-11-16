using Microsoft.AspNetCore.Mvc;

namespace PFDTeam2.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessFeedback(string feedback)
        {
            return RedirectToAction("Index");
        }
    }
}
