using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VatTuYTeDanhMuc.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;


namespace VatTuYTeDanhMuc.Controllers
{
    public class NuocSanXuatController : Controller
    {
        [Route("/NuocSanXuat")]
        public IActionResult Table()
        {
            webContext context = new webContext();
            ViewBag.nuocsx = context.NuocSanXuat.ToList();
            return View("TableNuocSX");
        }
       //[ Route("/Them")]
        public IActionResult Add()
        {
                return View();
        }
        public IActionResult Add1()
        {
            return View();
        }
        public IActionResult InsertNuocSX(NuocSanXuat nuocSanXuat)
        {
            webContext context = new webContext();
            nuocSanXuat.Nvtao = 3;
            nuocSanXuat.Nvsua = 3;
            context.Add(nuocSanXuat);
            context.SaveChanges();

            return RedirectToAction("Table");
            
        }
        public IActionResult InsertHangSX(HangSanXuat h)
        {
            webContext context = new webContext();
            
            context.Add(h);
            context.SaveChanges();

            return RedirectToAction("Table");

        }
        public IActionResult ViewUpdate(int id)
        {
            webContext context = new webContext();
            HangSanXuat nsx = context.HangSanXuat.Find(id);
            return View(nsx);
        }
        public IActionResult ViewUpdateDVT(int id)
        {
            ViewData["Title"] = "Sửa đơn vị tính";
            webContext context = new webContext();
            Dvt dvt = context.Dvt.Find(id);
            return View(dvt);
        }



    }
}
