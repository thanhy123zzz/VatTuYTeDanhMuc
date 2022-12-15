using Microsoft.AspNetCore.Mvc;
using System;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class HH_DVTController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục Hàng hoá - Đơn vị tính";
            return View("TableHH_DVT");
        }

        //Trả về view thêm đơn vị tính
        public IActionResult ViewInsertHH_DVT()
        {
            ViewData["Title"] = "Thêm đơn hàng hoá - đơn vị tính";
            return View();
        }
        //Trả về view sửa đơn vị tính
        [Route("/HH_DVT/ViewUpdateHH_DVT/{id}")]
        public IActionResult ViewUpdateHH_DVT(int id)
        {
            ViewData["Title"] = "Sửa đơn hàng hoá - đơn vị tính";
            webContext context = new webContext();
            HhDvt dvt = context.HhDvt.Find(id);
            return View(dvt);
        }

        //insert đơn vị tính
        [HttpPost]
        public IActionResult insertHH_DVT(HhDvt dvt)
        {
            webContext context = new webContext();
            dvt.Nvtao = 3;
            dvt.Active = true;
            dvt.NgayTao = DateTime.Now;
            context.HhDvt.Add(dvt);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        //update đơn vị tính
        [HttpPost]
        public IActionResult updateHH_DVT(HhDvt dvt)
        {
            webContext context = new webContext();
            HhDvt dv = context.HhDvt.Find(dvt.Id);

            dv.Nvsua = 3;
            dv.NgaySua = DateTime.Now;
            dv.SlquyDoi = dvt.SlquyDoi;
            dv.Iddvt = dvt.Iddvt;
            dv.Idhh = dvt.Idhh;

            context.HhDvt.Update(dv);
            context.SaveChanges();
            return RedirectToAction("Table");
        }

        //Xoá đơn vị tính
        [Route("/HH_DVT/xoa/{id}")]
        public IActionResult deleteHH_DVT(int id)
        {
            webContext context = new webContext();
            HhDvt dvt = context.HhDvt.Find(id);
            dvt.Active = false;

            context.HhDvt.Update(dvt);
            context.SaveChanges();
            return RedirectToAction("Table");
        }
    }
}
