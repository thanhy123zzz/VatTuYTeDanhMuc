using Microsoft.AspNetCore.Mvc;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class CachXuatController : Controller
    {
        public IActionResult Table()
        {
            webContext context = new webContext();
            CachXuat c = context.CachXuat.Find(1);
            return View("FormCachXuat",c);
        }

        [HttpPost("/CachXuat/UpdateCachXuat")]
        public IActionResult updateCachXuat(int CachXuat)
        {
            webContext context = new webContext();
            CachXuat c = context.CachXuat.Find(1);
            if(CachXuat == 1)
            {
                c.TheoTgnhap = true;
                c.TheoHsd = false;
                context.CachXuat.Update(c);
                context.SaveChanges();
            }
            if(CachXuat == 2)
            {
                c.TheoTgnhap = false;
                c.TheoHsd = true;
                context.CachXuat.Update(c);
                context.SaveChanges();

            }
            return RedirectToAction("Table");
        }
    }
}
