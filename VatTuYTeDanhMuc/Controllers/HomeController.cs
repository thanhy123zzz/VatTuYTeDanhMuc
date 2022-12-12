using Microsoft.AspNetCore.Mvc;

namespace VatTuYTe.Controllers
{
    public class HomeController : Controller
    {
        [Route("/TrangChu")]
        [Route("/")]
        public IActionResult Index()
        {
            return View("TrangChu");
        }

        [HttpGet("/TrangChu/{MaMenu}")]
        public IActionResult Index(string MaMenu)
        {
            return RedirectToAction("Table",MaMenu);
        }
    }
}
