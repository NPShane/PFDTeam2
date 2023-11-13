using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFDTeam2.Models;

namespace PFDTeam2.Controllers
{
    public class OnboardingController : Controller
    {
        // GET: OnboardingController
        public ActionResult Index()
        {
            return View();
        }

        // Inject your CalendarService
        /*private readonly CalendarService _calendarService;

        public OnboardingController(CalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public IActionResult GenerateCalendar()
        {
            // Retrieve the logged-in employee's ID (you need authentication for this)
            var employeeId = User.Identity.Name;

            // Define the date range for the calendar and schedule
            var startDate = DateTime.Now;
            var endDate = startDate.AddMonths(1);

            // Generate the personalized calendar and schedule
            var calendar = _calendarService.GenerateCalendar(employeeId, startDate, endDate);
            var scheduleEvents = _calendarService.GetScheduleEvents(employeeId, startDate, endDate);

            var viewModel = new CalendarViewModel
            {
                Calendar = calendar,
                ScheduleEvents = scheduleEvents
            };

            return View(viewModel);
        }*/

        // GET: OnboardingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OnboardingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnboardingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OnboardingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OnboardingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OnboardingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OnboardingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
