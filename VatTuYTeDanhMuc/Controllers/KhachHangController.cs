﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class KhachHangController : Controller
    {
       
        public IActionResult table()
        {
           
            return View("TableKhachHang");
        }
        //giao diện thêm khách hàng
        public IActionResult ViewInsertKH()
        {
            return View();
        }
        //Thêm khách hàng
        public IActionResult InsertKH(KhachHang kh)
        {
            webContext context = new webContext();
            kh.Nvtao = 3;
            kh.NgayTao = DateTime.Now;

            context.KhachHang.Add(kh);
            context.SaveChanges();
            return RedirectToAction("table");
        }
        // giao diện khách hàng
        [Route("/KhachHang/Update/{id}")]
        public IActionResult ViewUpdateKH(int id)
        {
            webContext context = new webContext();
            KhachHang kh = context.KhachHang.Find(id);
            return View(kh);
        }
        // sửa khách hàng
        public IActionResult UpdateKH(KhachHang kh)
        {
            webContext context = new webContext();
            KhachHang k = context.KhachHang.Find(kh.Id);
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
            k.Nvsua = 3;
            k.NgaySua = DateTime.Now;

            context.KhachHang.Update(k);
            context.SaveChanges();
            return RedirectToAction("table");
        }
        [Route("/KhachHang/Delete/{id}")]
        public IActionResult DeleteKH(int id)
        {
            webContext context = new webContext();
            KhachHang kh = context.KhachHang.Find(id);
            kh.Active = false;

            context.KhachHang.Update(kh);
            context.SaveChanges();
            return RedirectToAction("table");
        }
    }
}