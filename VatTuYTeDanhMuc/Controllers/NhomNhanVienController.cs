using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NhomNhanVienController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục nhóm nhân viên";
            return View("TableNhomNhanVien");
        }

        //Trả về view thêm nhóm nhân viên
        public IActionResult ViewInsertNhomNV()
        {
            ViewData["Title"] = "Thêm nhóm nhân viên";
            return View();
        }
        //Trả về view sửa nhóm nhân viên
        [Route("/NhomNhanVien/ViewUpdateNhomNV/{id}")]
        public IActionResult ViewUpdateNhomNV(int id)
        {
            ViewData["Title"] = "Sửa nhóm nhân viên";
            webContext context = new webContext();
            NhomNhanVien dvt = context.NhomNhanVien.Find(id);
            return View(dvt);
        }

        //insert nhóm nhân viên
        [HttpPost]
        public IActionResult insertNhomNV(NhomNhanVien dvt)
        {
            webContext context = new webContext();
            dvt.Nvtao = 3;
            dvt.Active = true;
            dvt.NgayTao = DateTime.Now;
            context.NhomNhanVien.Add(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }

        //update nhóm nhân viên
        [HttpPost]
        public IActionResult updateNhomNV(NhomNhanVien dvt)
        {
            webContext context = new webContext();
            NhomNhanVien dv = context.NhomNhanVien.Find(dvt.Id);

            dv.Nvsua = 3;
            dv.NgaySua = DateTime.Now;
            dv.TenNnv = dvt.TenNnv;
            dv.MaNnv = dvt.MaNnv;

            context.NhomNhanVien.Update(dv);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("Table");
        }

        //Xoá nhóm nhân viên
        [Route("/NhomNhanVien/xoa/{id}")]
        public IActionResult deleteNhomNhanVien(int id)
        {
            webContext context = new webContext();
            NhomNhanVien dvt = context.NhomNhanVien.Find(id);
            dvt.Active = false;

            context.NhomNhanVien.Update(dvt);
            context.SaveChanges();
            return RedirectToAction("Table");
        }
        [HttpPost("/loadTableNNV")]
        public IActionResult loadTableNNV(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.NNV = context.NhomNhanVien.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.NNV = context.NhomNhanVien.ToList();
            }
            return PartialView();
        }
        [Route("/NhomNhanVien/khoiphuc/{id}")]
        public IActionResult khoiphucNNV(int id)
        {
            webContext context = new webContext();
            NhomNhanVien hsx = context.NhomNhanVien.Find(id);
            hsx.Active = true;

            context.NhomNhanVien.Update(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
