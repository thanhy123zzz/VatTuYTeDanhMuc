using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class HH_DVTController : Controller
    {
        public IActionResult Table()
        {
            ViewData["Title"] = "Danh mục Hàng hoá - Đơn vị tính";

            webContext context = new webContext();
            TempData["Menu"] = context.Menu.Where(x => x.MaMenu == "HH_DVT").FirstOrDefault().Id;
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
            TempData["ThongBao"] = "Thêm thành công!";
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
            TempData["ThongBao"] = "Sửa thành công!";
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

        [HttpPost("/loadTableHangHoa")]
        public IActionResult loadTableHangHoa(int nhomHH)
        {
            webContext context = new webContext();
            ViewBag.HangHoas = context.HangHoa.Where(x => x.Idnhh == nhomHH);
            return PartialView();
        }
        [HttpPost("/searchTableHH")]
        public IActionResult searchTableHH(int nhomHH, string key)
        {
            webContext context = new webContext();
            if (nhomHH == 0)
            {
                ViewBag.HangHoas = context.HangHoa.FromSqlRaw("select*from HangHoa where concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%';").ToList();
            }
            else
            {
                ViewBag.HangHoas = context.HangHoa.FromSqlRaw("select*from HangHoa where IdNhh = '" + nhomHH + "' and concat_ws(' ',MaHH,TenHH) LIKE N'%" + key + "%';").ToList();
            }
            return PartialView("loadTableHangHoa");
        }

        [HttpPost("/loadTableHH_DVT")]
        public IActionResult loadTableHH_DVT(int idHH)
        {
            webContext context = new webContext();
            ViewBag.HH_DVT = context.HhDvt.Where(x => x.Idhh == idHH && x.Active == true);
            HangHoa hh = context.HangHoa.Find(idHH);
            ViewBag.DVC = context.Dvt.FirstOrDefault(x => x.Id == hh.Iddvtchinh);
            return PartialView();
        }

        [HttpPost("/addHH_DVT")]
        public string addHH_DVT(int idHH, int sl, int idDVT)
        {
            webContext context = new webContext();
            HhDvt h = new HhDvt();
            h.Active = true;
            h.Nvtao = 3;
            h.NgayTao = DateTime.Now;
            h.Idhh = idHH;
            h.SlquyDoi = sl;
            h.Iddvt = idDVT;
            context.HhDvt.Add(h);
            context.SaveChanges();
            return "Thêm thành công";
        }

        [HttpPost("/updateHH_DVT")]
        public string updateHH_DVT(int idHH, int sl, int idDVT, int id)
        {
            webContext context = new webContext();
            HhDvt h = context.HhDvt.Find(id);
            h.Active = true;
            h.Nvsua = 3;
            h.NgaySua = DateTime.Now;
            h.Idhh = idHH;
            h.SlquyDoi = sl;
            h.Iddvt = idDVT;
            context.HhDvt.Update(h);
            context.SaveChanges();
            return "Sửa thành công";
        }

        [HttpPost("/addNewRowHH_DVT")]
        public IActionResult addNewRowHH_DVT(int idDVT, int sl, int id)
        {
            if (idDVT == 0)
                ViewBag.idDVT = null;
            else
            {
                ViewBag.idDVT = idDVT;
            }
            ViewBag.ID = id;
            ViewBag.SL = sl;
            return PartialView();
        }
        [HttpPost("/removeHH_DVT")]
        public string removeHH_DVT(int id)
        {
            webContext context = new webContext();
            HhDvt h = context.HhDvt.Find(id);
            h.Active = false;
            context.Update(h);
            context.SaveChanges();
            return "Xoá thành công!";
        }
    }
}
