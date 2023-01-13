using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class KhachHangController : Controller
    {
       
        public IActionResult table()
        {
            ViewData["Title"] = "Danh mục khách hàng";
            return View("TableKhachHang");
        }
        //giao diện thêm khách hàng
        public IActionResult ViewInsertKH()
        {
            ViewData["Title"] = "Thêm khách hàng";
            return View();
        }
        //Thêm khách hàng
        public IActionResult InsertKH(KhachHang kh)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            webContext context = new webContext();
            kh.Nvtao = idUser;
            kh.NgayTao = DateTime.Now;
            kh.Active = true;
            kh.Idcn = idCN;
           
            kh.Idnv = idUser;
            kh.Idcn = idCN;

            context.KhachHang.Add(kh);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("table");
        }
        // giao diện khách hàng
        [Route("/KhachHang/Update/{id}")]
        public IActionResult ViewUpdateKH(int id)

        {
            webContext context = new webContext();
            KhachHang kh = context.KhachHang.Find(id);
            ViewData["Title"] = "Sửa khách hàng";
            return View(kh);
        }
        // sửa khách hàng
        public IActionResult UpdateKH(KhachHang kh)
        {
            webContext context = new webContext();
            KhachHang k = context.KhachHang.Find(kh.Id);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            k.MaKh = kh.MaKh;
            k.TenKh = kh.TenKh;
            k.DiaChi = kh.DiaChi;
            k.Sdt = kh.Sdt;
            k.Email = kh.Email;
            k.MaSoThue = kh.MaSoThue;
            k.GhiChu = kh.GhiChu;
            k.Nvsale = kh.Nvsale;
            k.TenTaiKhoan = kh.TenTaiKhoan;
            k.Idcn = kh.Idcn;
            k.Idnv = kh.Idnv;
            k.Nvsua = idUser;
            k.NgaySua = DateTime.Now;

            context.KhachHang.Update(k);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("table");
        }
        [Route("/KhachHang/Delete/{id}")]
        public IActionResult DeleteKH(int id)
        {
            webContext context = new webContext();
            KhachHang kh = context.KhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            kh.NgaySua = DateTime.Now;
            kh.Nvsua = idUser; 
            kh.Active = false;

            context.KhachHang.Update(kh);
            context.SaveChanges();
            return RedirectToAction("table");
        }
        [Route("/KhachHang/Detail/{id}")]
        public IActionResult Detail(int id)
        {
            webContext context = new webContext();
            KhachHang kh = context.KhachHang.Find(id);
            ViewData["Title"] = "Thông tin khách hàng";
            return View(kh);
        }
        [HttpPost("/loadTableKH")]
        public IActionResult loadTableKH(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.KH = context.KhachHang.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.KH = context.KhachHang.ToList();
            }
            return PartialView();
        }
        [Route("/KhachHang/khoiphuc/{id}")]
        public IActionResult khoiphucKH(int id)
        {
            webContext context = new webContext();
            KhachHang hsx = context.KhachHang.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            hsx.NgaySua = DateTime.Now;
            hsx.Nvsua = idUser;
            hsx.Active = true;

            context.KhachHang.Update(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
