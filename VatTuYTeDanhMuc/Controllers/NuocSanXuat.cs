using Microsoft.AspNetCore.Mvc;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NuocSanXuat : Controller
    {
        public IActionResult Table()
        {
            return View("TableNuocSX");
        }
    }
}
