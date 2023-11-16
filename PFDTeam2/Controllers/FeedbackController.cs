using Microsoft.AspNetCore.Mvc;
using PFDTeam2.DAL;

namespace PFDTeam2.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly StaffDAL staffDAL;
        public FeedbackController()
        {
            staffDAL = new StaffDAL();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessFeedback(string feedback)
        {
            // logic for relevance
            bool isRelevant = ContainsAnyKeyword(feedback, new[] { "improvement", "onboarding", "goals" });

            staffDAL.AddFeedback(feedback, isRelevant, false);

            return RedirectToAction("Main", "Staff");
        }

        private bool ContainsAnyKeyword(string text, string[] keywords)
        {
            // Check if any of the keywords are present in the text (case-insensitive)
            return keywords.Any(keyword => text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
