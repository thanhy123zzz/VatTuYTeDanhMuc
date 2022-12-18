using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Runtime.Intrinsics.X86;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public HangHoaController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục hàng hoá";
            return View("TableHangHoa");
        }
        public IActionResult ViewInsertHangHoa()
        {
            ViewData["Title"] = "Thêm hàng hoá";
            return View();
        }

        [HttpPost]
        public IActionResult insertHangHoa(HangHoa hh, IFormFile Avt)
        {
            webContext context = new webContext();
            
            hh.Avatar = UploadedFile(hh, Avt);
            hh.Nvtao = 3;
            hh.Active = true;
            hh.NgayTao = DateTime.Now;
            context.HangHoa.Add(hh);
            context.SaveChanges();
            TempData["ThongBao"] = "Thêm thành công!";
            return RedirectToAction("Table");
        }
        private string UploadedFile(HangHoa model, IFormFile avt)
        {
            string uniqueFileName = null;

            if (avt != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ImagesHangHoa");
                uniqueFileName = model.MaHh+ ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    avt.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        //Trả về view sửa hàng hoá
        [Route("/HangHoa/ViewUpdateHangHoa/{id}")]
        public IActionResult ViewUpdateHangHoa(int id)
        {
            ViewData["Title"] = "Sửa hàng hoá";
            webContext context = new webContext();
            HangHoa dvt = context.HangHoa.Find(id);
            return View(dvt);
        }

        //update đơn vị tính
        [HttpPost]
        public IActionResult updateHangHoa(HangHoa dvt, IFormFile avt)
        {
            webContext context = new webContext();
            HangHoa dv = context.HangHoa.Find(dvt.Id);

            dv.Nvsua = 3;
            dv.NgaySua = DateTime.Now;
            dv.TenHh = dvt.TenHh;
            dv.MaHh = dvt.MaHh;
            dv.Idnhh = dvt.Idnhh;
            dv.Iddvtchinh = dvt.Iddvtchinh;
            dv.Idhsx = dvt.Idhsx;
            dv.Idnsx = dvt.Idnsx;
            dv.Avatar = UploadedFile(dvt, avt);

            context.HangHoa.Update(dv);
            context.SaveChanges();
            TempData["ThongBao"] = "Sửa thành công!";
            return RedirectToAction("Table");
        }
    }
}
