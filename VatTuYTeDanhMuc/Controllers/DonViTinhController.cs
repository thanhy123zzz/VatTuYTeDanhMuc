using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class DonViTinhController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục đơn vị tính";
            return View("TableDonViTinh");
        }

        //Trả về view thêm đơn vị tính
        public IActionResult ViewInsertDVT()
        {
            ViewData["Title"] = "Thêm đơn vị tính";
            return View();
        }
        [HttpPost("/loadTableDVT")]
        public IActionResult loadTableDVT(bool active)
        {
            webContext context = new webContext();
            if (active) {
                ViewBag.DVT = context.Dvt.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.DVT = context.Dvt.ToList();
            }
            return PartialView();
        }

        //Trả về view sửa đơn vị tính
        [Route("/DonViTinh/ViewUpdateDVT/{id}")]
        public IActionResult ViewUpdateDVT(int id)
        {
            ViewData["Title"] = "Sửa đơn vị tính";
            webContext context = new webContext();
            Dvt dvt = context.Dvt.Find(id);
            return View(dvt);
        }

        //insert đơn vị tính
        [HttpPost]
        public IActionResult insertDonViTinh(Dvt dvt)
        {
            webContext context = new webContext();
            dvt.Nvtao =3;
            dvt.Active = true;
            dvt.NgayTao = DateTime.Now;
            context.Dvt.Add(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        //update đơn vị tính
        [HttpPost]
        public IActionResult updateDVt(Dvt dvt)
        {
            webContext context = new webContext();
            Dvt dv = context.Dvt.Find(dvt.Id);

            dv.Nvsua = 3;
            dv.NgaySua = DateTime.Now;
            dv.TenDvt = dvt.TenDvt;
            dv.MaDvt = dvt.MaDvt;

            context.Dvt.Update(dv);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("Table");
        }

        //Xoá đơn vị tính
        [Route("/DonViTinh/xoa/{id}")]
        public IActionResult deleteDonViTinh(int id)
        {
            webContext context = new webContext();
            Dvt dvt = context.Dvt.Find(id);
            dvt.Active=false;

            context.Dvt.Update(dvt);
            context.SaveChanges();
            return RedirectToAction("Table");
        }
        [Route("/DonViTinh/khoiphuc/{id}")]
        public IActionResult khoiphucDVT(int id)
        {
            webContext context = new webContext();
            Dvt dvt = context.Dvt.Find(id);
            dvt.Active = true;

            context.Dvt.Update(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
