using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class HangSanXuatController : Controller
    {
        public IActionResult table()
        {
            ViewData["Title"] = "Danh mục hãng sản xuất";
            webContext context = new webContext();
            TempData["Menu"] = context.Menu.FirstOrDefault(menu => EF.Functions.Like(menu.TenMenu, "%Danh mục hãng sản xuất%") && menu.Active == true).Id;
            return View("TableHangSanXuat");
        }
        //hiển thị view insert
        public IActionResult ViewInsert()
        {
            ViewData["Title"] = "Thêm hãng sản xuất";
            return View();  
        }
        //thêm hãng sản xuất
        public IActionResult Insert(HangSanXuat hsx)
        {
            webContext context = new webContext();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            hsx.Idcn = idCN;
            hsx.Nvtao = idUser;
            hsx.Nvsua = idUser;
            hsx.NgayTao = DateTime.Now;
            hsx.NgaySua = DateTime.Now;
            hsx.Active = true;
            hsx.Idcn = 1;
            context.HangSanXuat.Add(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";

            return RedirectToAction("table");
        }
        [Route("/HangSanXuat/Delete/{id}")]
        public IActionResult DeleteHSX(int id)

        {
            webContext context = new webContext();
            HangSanXuat hsx = context.HangSanXuat.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            hsx.NgaySua = DateTime.Now;
            hsx.Nvsua = idUser;
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
            ViewData["Title"] = "Sửa hãng sản xuất";

            return View(hsx);
        }
        public IActionResult UpdateHSX(HangSanXuat hsx)
        {
            webContext context = new webContext();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            HangSanXuat h = context.HangSanXuat.Find(hsx.Id);
            h.MaHsx = hsx.MaHsx;
            h.TenHsx = hsx.TenHsx;
            h.Idcn = hsx.Idcn;
            h.Nvsua = idUser;
            h.NgaySua = DateTime.Now;
            context.HangSanXuat.Update(h);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("table");
        }
        [HttpPost("/loadTableHSX")]
        public IActionResult loadTableHSX(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.HSX = context.HangSanXuat.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.HSX = context.HangSanXuat.ToList();
            }
            return PartialView();
        }
        [Route("/HangSanXuat/khoiphuc/{id}")]
        public IActionResult khoiphucHSX(int id)
        {
            webContext context = new webContext();
            HangSanXuat hsx = context.HangSanXuat.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            hsx.NgaySua = DateTime.Now;
            hsx.Nvsua = idUser;
            hsx.Active = true;

            context.HangSanXuat.Update(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
