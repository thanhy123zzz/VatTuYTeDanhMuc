using Microsoft.AspNetCore.Mvc;
using System;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NhomHangHoaController : Controller
    {
        public IActionResult table()
        {
            
            return View("TableNhomHangHoa");  
        }
        // hiển thị view Insert
        public IActionResult ViewInsertNHH()
        {
            return View();
        }
        // Insert nhóm hàng hóa 
        public IActionResult InsertNHH(NhomHangHoa nhh) 
        {
            webContext context = new webContext();
            nhh.Nvtao = 3;
            nhh.NgayTao = DateTime.Now;

            context.NhomHangHoa.Add(nhh);
            context.SaveChanges();
            return RedirectToAction("table"); 
        }

        // xóa nhóm hàng hóa
        [Route("/NhomHangHoa/Delete/{id}")]
        public IActionResult DeleteNHH(int id )
        {
            webContext context = new webContext();
            NhomHangHoa nhh = context.NhomHangHoa.Find(id);
            nhh.Active = false;

            context.NhomHangHoa.Update(nhh);
            context.SaveChanges();

            return RedirectToAction("table");
        }
        // trả về view update 
        [Route("/NhomHangHoa/updateNHH/{id}")]
        public IActionResult ViewUpdateNHH(int id )
        {
            webContext context = new webContext();
            NhomHangHoa a = context.NhomHangHoa.Find(id);

            return View(a);
        }

        // update nhóm hàng hóa 
        public IActionResult UpdateNHH(NhomHangHoa nhh)
        {
            webContext context = new webContext();
            NhomHangHoa n = context.NhomHangHoa.Find(nhh.Id);
            n.Nvsua = 3;
            n.NgayTao = DateTime.Now;
            n.TenNhh = nhh.TenNhh;
            n.MaNhh = nhh.MaNhh;
            n.Idcn = nhh.Idcn;
            context.NhomHangHoa.Update(n);
            context.SaveChanges();
            return RedirectToAction("table");
        }
    }
}
