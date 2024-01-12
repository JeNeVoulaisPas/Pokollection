using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PokéService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokéController : Controller
    {
        // GET: /api/poké
        public ActionResult Index()
        {
            return View();
        }

        // GET: /api/poké/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /api/poké/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /api/poké/Create
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

        // GET: /api/poké/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: /api/poké/Edit/5
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

        // GET: /api/poké/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: /api/poké/Delete/5
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
