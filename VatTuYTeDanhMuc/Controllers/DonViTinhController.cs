using Microsoft.AspNetCore.Mvc;

namespace VatTuYTeDanhMuc.Controllers
{
    public class DonViTinhController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục đơn vị tính";
            return View("TableDonViTinh");
        }
    }
}
