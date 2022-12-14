using Microsoft.AspNetCore.Mvc;
using System;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class HangSanXuatController : Controller
    {
        public IActionResult table()
        {
            ViewData["Title"] = "Danh mục hãng sản xuất";
            return View("TableHangSanXuat");
        }
        public IActionResult ViewInsert()
        {
            return View();  
        }

        public IActionResult Insert(HangSanXuat hsx)
        {
            webContext context = new webContext();
            
            hsx.Nvtao = 3;
            hsx.NgayTao = DateTime.Now;
            context.HangSanXuat.Add(hsx);
            context.SaveChanges();

            return RedirectToAction("table");
        }
        [Route("/HangSanXuat/Delete/{id}")]
        public IActionResult DeleteHSX(int id)

        {
            webContext context = new webContext();
            HangSanXuat hsx = context.HangSanXuat.Find(id);
            hsx.Active = false;

            context.HangSanXuat.Update(hsx);
            context.SaveChanges();

            
            return RedirectToAction("table");
        }
        [Route("/HangSanXuat/Update/{id}")]
        public IActionResult ViewUpdateHSX(int id)
        {
            webContext context = new webContext();
            HangSanXuat hsx = context.HangSanXuat.Find(id);

            return View(hsx);
        }
        public IActionResult UpdateHSX(HangSanXuat hsx)
        {
            webContext context = new webContext();
            HangSanXuat h = context.HangSanXuat.Find(hsx.Id);
            h.MaHsx = hsx.MaHsx;
            h.TenHsx = hsx.TenHsx;
            h.Idcn = hsx.Idcn;
            h.Nvsua = 3;
            h.NgaySua = DateTime.Now;
            context.HangSanXuat.Update(h);
            context.SaveChanges();
            return RedirectToAction("table");
        }
    }
}
