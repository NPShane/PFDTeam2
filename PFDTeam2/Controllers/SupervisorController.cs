using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PFDTeam2.Controllers
{
    public class SupervisorController : Controller
    {
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
    }
}
