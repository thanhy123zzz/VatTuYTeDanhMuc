using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NhaCungCapController : Controller
    {
       
        public IActionResult table()
        {
            ViewData["Title"] = "Danh mục nhà cung cấp";
            return View("TableNhaCungCap");
        }
        //hiển thị view insert
        public IActionResult ViewInsertNCC()
        {
            ViewData["Title"] = "Thêm nhà cung cấp";
            return View();
        }
        //thêm nhà cung cấp
        public IActionResult Insert(NhaCungCap ncc)
        {
            webContext context = new webContext();

            ncc.Nvtao = 3;
            ncc.NgayTao = DateTime.Now;
            ncc.Idcn = 1;
            ncc.Active = true;

            context.NhaCungCap.Add(ncc);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("table");
        }
        //HIển thị view update
        [Route("/NhaCungCap/ViewUpdate/{id}")]
        public IActionResult ViewUpdateNCC(int id)

        {
            webContext context = new webContext();
            NhaCungCap ncc = context.NhaCungCap.Find(id);
            ViewData["Title"] = "Sửa nhà cung cấp";
            return View(ncc);
        }
        public IActionResult UpdateNCC(NhaCungCap ncc)
        {
            webContext context = new webContext();
            NhaCungCap n = context.NhaCungCap.Find(ncc.Id);
            n.MaNcc = ncc.MaNcc;
            n.TenNcc = ncc.TenNcc;
            n.DiaChi = ncc.DiaChi;
            n.Sdt = ncc.Sdt;
            n.Email = ncc.Email;
            n.GhiChu = ncc.GhiChu;
            n.Idcn = ncc.Idcn;
            n.Nvsua = 3; 
            n.NgaySua = DateTime.Now;   

            context.NhaCungCap.Update(n);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("table");
        }
        //xóa nhà cung cấp
        [Route("/NhaCungCap/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            webContext context = new webContext();
            NhaCungCap n =context.NhaCungCap.Find(id);
            n.Active = false;

            context.NhaCungCap.Update(n);
            context.SaveChanges();
            return RedirectToAction("table");
        }
        [HttpPost("/loadTableNCC")]
        public IActionResult loadTableNCC(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.NCC = context.NhaCungCap.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.NCC = context.NhaCungCap.ToList();
            }
            return PartialView();
        }
        [Route("/NhaCungCap/khoiphuc/{id}")]
        public IActionResult khoiphucNCC(int id)
        {
            webContext context = new webContext();
            NhaCungCap ncc = context.NhaCungCap.Find(id);
            ncc.Active = true;

            context.NhaCungCap.Update(ncc);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
