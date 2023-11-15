using Microsoft.AspNetCore.Mvc;
using Python.Runtime;

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
            using (Py.GIL())
            {
                dynamic feedbackProcessor = Py.Import("process_feedback");
                bool isRelevant = feedbackProcessor.process_feedback(feedback);
            }

            return RedirectToAction("Index");
        }
    }
}
