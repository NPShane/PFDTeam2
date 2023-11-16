using Microsoft.AspNetCore.Mvc;
using PFDTeam2.DAL;
using PFDTeam2.Models;
using Python.Runtime;

namespace PFDTeam2.Controllers
{
    public class FeedbackController : Controller
    {
        public readonly StaffDAL staffDAL;
        public FeedbackController()
        {
            staffDAL = new StaffDAL();
        }

        // Index is for posting feedback
        public IActionResult Index()
        {
            return View();
        }

        // POST feedback after processing and redirect to FeedbackPosted
        [HttpPost]
        public IActionResult ProcessFeedback(string feedback)
        {
            var result = CallPythonSentimentAnalysis(feedback);

            // logic for relevance
            bool isRelevant = ContainsAnyKeyword(feedback, new[] { "improvement", "onboarding", "goals" });

            staffDAL.AddFeedback(feedback, isRelevant, result, false, null);

            return RedirectToAction("FeedbackPosted");
        }

        // Show a message after posting feedback
        public IActionResult FeedbackPosted()
        {
            return View();
        }

        // Show all current feedback for supervisor
        public IActionResult ViewAll()
        {
            var allFeedbacks = staffDAL.GetFeedbacks();
            return View(allFeedbacks);
        }

        // GET single feedback info and display page for supervisor response
        [HttpGet]
        public IActionResult Respond(int id)
        {
            var selectedFeedback = staffDAL.GetFeedback(id);
            return View(selectedFeedback);
        }

        // POST the supervisor's response and redirect to ResponsePosted
        [HttpPost]
        public IActionResult Respond(Feedback feedback)
        {
            // Save the response in the database
            staffDAL.Respond(feedback.FeedbackId, feedback.Response);

            // Redirect to the success page
            return RedirectToAction("ResponsePosted");
        }

        // Show a message after posting response to feedback
        public IActionResult ResponsePosted()
        {
            return View();
        }

        // Analyse feedback text for positive/negative sentiment
        private static bool CallPythonSentimentAnalysis(string feedback)
        {
            // This only works on one guy's machine due to a quirk of Python.NET
            Runtime.PythonDLL = @"C:\Users\Shane\AppData\Local\Programs\Python\Python37\python37.dll";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic vaderModule = Py.Import("vaderSentiment.vaderSentiment");
                dynamic analyzer = vaderModule.SentimentIntensityAnalyzer();

                // Call the sentiment analyzer on the feedback
                dynamic sentimentScores = analyzer.polarity_scores(feedback);

                // Analyze sentiment scores and determine if it's positive
                double compoundScore = sentimentScores["compound"];
                bool isPositiveSentiment = compoundScore >= 0;

                return isPositiveSentiment;
            }
        }

        // Check for relevant keywords
        private static bool ContainsAnyKeyword(string text, string[] keywords)
        {
            // Check if any of the keywords are present in the text (case-insensitive)
            return keywords.Any(keyword => text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
