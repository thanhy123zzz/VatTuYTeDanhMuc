using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class NhomHangHoaController : Controller
    {
        public IActionResult table()
        {
            ViewData["Title"] = "Danh mục nhóm hàng hóa";

            return View("TableNhomHangHoa");  
        }
        // hiển thị view Insert
        public IActionResult ViewInsertNHH()
        {
            ViewData["Title"] = "Thêm nhóm hàng hóa";
            return View();
        }
        // Insert nhóm hàng hóa 
        public IActionResult InsertNHH(NhomHangHoa nhh) 
        {
            webContext context = new webContext();
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            nhh.Nvtao = idUser;
            nhh.NgayTao = DateTime.Now;
            nhh.Active = true;
            nhh.Idcn = 1;

            context.NhomHangHoa.Add(nhh);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("table"); 
        }

        // xóa nhóm hàng hóa
        [Route("/NhomHangHoa/Delete/{id}")]
        public IActionResult DeleteNHH(int id )
        {
            webContext context = new webContext();
            NhomHangHoa nhh = context.NhomHangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            nhh.NgaySua = DateTime.Now;
            nhh.Nvsua = idUser;
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
            ViewData["Title"] = "Thêm nhóm hàng hóa";

            return View(a);
        }

        // update nhóm hàng hóa 
        public IActionResult UpdateNHH(NhomHangHoa nhh)
        {
            webContext context = new webContext();
            NhomHangHoa n = context.NhomHangHoa.Find(nhh.Id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            n.Nvsua = idUser;
            n.NgayTao = DateTime.Now;
            n.TenNhh = nhh.TenNhh;
            n.MaNhh = nhh.MaNhh;
            n.Idcn = nhh.Idcn;
            context.NhomHangHoa.Update(n);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("table");
        }
        [HttpPost("/loadTableNHH")]
        public IActionResult loadTableNHH(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.NHH = context.NhomHangHoa.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.NHH = context.NhomHangHoa.ToList();
            }
            return PartialView();
        }
        [Route("/NhomHangHoa/khoiphuc/{id}")]
        public IActionResult khoiphucNHH(int id)
        {
            webContext context = new webContext();
            NhomHangHoa hsx = context.NhomHangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            hsx.NgaySua = DateTime.Now;
            hsx.Nvsua = idUser;
            hsx.Active = true;

            context.NhomHangHoa.Update(hsx);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }
    }
}
