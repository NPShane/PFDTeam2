using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PFDTeam2.DAL;

namespace PFDTeam2.Controllers
{
    public class SupervisorController : Controller
    {

        private SupervisorDAL supervisorContext = new SupervisorDAL();

        // GET: SupervisorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SupervisorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SupervisorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupervisorController/Create
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

        // GET: SupervisorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SupervisorController/Edit/5
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

        // GET: SupervisorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SupervisorController/Delete/5
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
        public ActionResult SupervisorMain()
        {
            return View("~/Views/Supervisor/SupervisorMain.cshtml");
        }

        /*-------------------------------------- L O G I N --------------------------------------------*/

        [HttpPost]
        public IActionResult SupervisorLogin(string email, string password)
        {
            if (supervisorContext.Login(email, password))
            {
                // Store Login ID in session with the key "Email"
                HttpContext.Session.SetString("Email", email);
                // Store user role "Member" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", "Supervisor");

                int supervisorId = supervisorContext.GetSupervisorIdByEmail(email);

                // Store the MemberId in session
                HttpContext.Session.SetInt32("SupervisorId", supervisorId);
                // Redirect user to the "MemberMain" view through an action
                return RedirectToAction("SupervisorMain");
            }
            else
            {
                // Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("SupervisorLogin", "Home");
            }
        }



    }
}
