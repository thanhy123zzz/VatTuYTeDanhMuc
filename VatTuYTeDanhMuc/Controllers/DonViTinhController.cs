using Microsoft.AspNetCore.Mvc;

namespace VatTuYTeDanhMuc.Controllers
{
    public class DonViTinhController : Controller
    {
        public IActionResult Table()
        {
            return View("TableDonViTinh");
        }
    }
}
