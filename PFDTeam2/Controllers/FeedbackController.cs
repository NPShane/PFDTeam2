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
            bool isRelevant = false;

            staffDAL.AddFeedback(feedback, isRelevant, false);

            return RedirectToAction("Index");
        }
    }
}
