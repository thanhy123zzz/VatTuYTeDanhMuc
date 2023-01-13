using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            webContext context = new webContext();
            TempData["Menu"] = context.Menu.FirstOrDefault(menu => EF.Functions.Like(menu.TenMenu, "%Danh mục nhóm nhân viên%") && menu.Active == true).Id;
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            dvt.Idcn = idCN;
            dvt.Nvtao = idUser;
            dvt.Active = true;
            dvt.NgayTao = DateTime.Now;
            dvt.NgaySua = DateTime.Now;
            dvt.Nvsua = idUser;
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);

            dv.Nvsua = idUser;
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvt.NgaySua = DateTime.Now;
            dvt.Nvsua = idUser;
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            hsx.NgaySua = DateTime.Now;
            hsx.Nvsua = idUser;
            hsx.Active = true;

            context.NhomNhanVien.Update(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
