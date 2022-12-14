using Microsoft.AspNetCore.Mvc;
using System;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NhomNhanVienController : Controller
    {
        public IActionResult table()
        {
            return View("TableNhomNhanVien");
        }

        // hiển thị view thêm nhóm nhân viên
        public IActionResult ViewInsert()
        {
            return View();
        }
        // thêm nhân viên
        public IActionResult InsertNNV(NhomNhanVien nnv) 
        {
            webContext context = new webContext();
            nnv.Nvtao = 3; 
            nnv.NgayTao= DateTime.Now;

            context.NhomNhanVien.Add(nnv);
            context.SaveChanges();
            return RedirectToAction("table"); 
        }
        // xóa nhóm nhân viên
        [Route("/NhomNhanVien/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            webContext context = new webContext();

            NhomNhanVien a = context.NhomNhanVien.Find(id);
            a.Active = false; 

            context.NhomNhanVien.Update(a);
            context.SaveChanges();
           
            return RedirectToAction("table");
        }
        // hiển thị view update nhóm nhân viên
        public IActionResult ViewUpdateNnv(int id)
        {
            webContext context  = new webContext();
            NhomNhanVien nnv = context.NhomNhanVien.Find(id);
            return View(nnv);
        }
        // update nhóm nhân viên 
        public IActionResult Update(NhomNhanVien nnv)
        {
            webContext context = new webContext();
            NhomNhanVien a = context.NhomNhanVien.Find(nnv.Id);
            a.Nvsua = 3; 
            a.NgaySua = DateTime.Now;
            a.MaNnv = nnv.MaNnv;
            a.TenNnv = nnv.TenNnv;
            a.Idcn = nnv.Idcn;

            context.NhomNhanVien.Update(a);
            context.SaveChanges();
            return RedirectToAction("table");
        }
    }
}
