﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
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
            webContext context = new webContext();
            TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "HangHoa").FirstOrDefault().Id;
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            hh.Idcn = idCN;
            hh.Avatar = UploadedFile(hh, Avt);
            hh.Nvtao = idUser;
            hh.Nvsua = idUser;
            hh.Active = true;
            hh.NgayTao = DateTime.Now;
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
                uniqueFileName = model.MaHh + ".jpg";
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
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            int idCN = int.Parse(User.Claims.ElementAt(4).Value);
            dv.Idcn = idCN;
            dv.Nvsua = idUser;
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

        [Route("/HangHoa/xoa/{id}")]
        public IActionResult deleteHangHoa(int id)
        {
            webContext context = new webContext();
            HangHoa dvt = context.HangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvt.NgaySua = DateTime.Now;
            dvt.Nvsua = idUser;
            dvt.Active = false;

            context.HangHoa.Update(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Xoá thành công!";
            return RedirectToAction("Table");
        }

        [Route("/HangHoa/khoiphuc/{id}")]
        public IActionResult khoiphucHH(int id)
        {
            webContext context = new webContext();
            HangHoa dvt = context.HangHoa.Find(id);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            dvt.NgaySua = DateTime.Now;
            dvt.Nvsua = idUser;
            dvt.Active = true;

            context.HangHoa.Update(dvt);
            context.SaveChanges();
            TempData["ThongBao"] = "Khôi phục thành công!";
            return RedirectToAction("Table");
        }

        [HttpPost("/loadTableHH")]
        public IActionResult loadTable(bool active, int nhomHH, int SL)
        {
            webContext context = new webContext();
            if (active)
            {
                if (nhomHH != 0)
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Active == active && x.Idnhh == nhomHH).ToList();
                }
                else
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Active == active).ToList();
                }
            }
            else
            {
                if (nhomHH != 0)
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Idnhh == nhomHH).ToList();
                }
                else
                {
                    ViewBag.HangHoas = context.HangHoa.ToList();
                }
            }
            return PartialView("_viewTableHH");
        }
        [HttpPost("/loadMoreTableHH")]
        public IActionResult loadMoreTableHH(bool active, int nhomHH, int SL)
        {
            webContext context = new webContext();
            if (active)
            {
                if (nhomHH != 0)
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Active == active && x.Idnhh == nhomHH).Take(SL + 9).ToList();
                }
                else
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Active == active).Take(SL + 9).ToList();
                }
            }
            else
            {
                if (nhomHH != 0)
                {
                    ViewBag.HangHoas = context.HangHoa.Where(x => x.Idnhh == nhomHH).Take(SL + 9).ToList();
                }
                else
                {
                    ViewBag.HangHoas = context.HangHoa.Take(SL + 9).ToList();
                }
            }
            return PartialView("_viewTableHH");
        }
        [HttpPost("/searchHH")]
        public IActionResult searchHH(string key)
        {
            webContext context = new webContext();
            ViewBag.HangHoas = context.HangHoa.FromSqlRaw("select*from HangHoa where concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%';").ToList();
            return PartialView("_viewTableHH");
        }
        /*[HttpPost("/loadTableHH")]
        public IActionResult loadTableHH(bool active)
        {
            webContext context = new webContext();
            if (active)
            {
                ViewBag.HH = context.HangHoa.Where(x => x.Active == true).ToList();
            }
            else
            {
                ViewBag.HH = context.HangHoa.ToList();
            }
            return PartialView();
        }*/


    }
}
