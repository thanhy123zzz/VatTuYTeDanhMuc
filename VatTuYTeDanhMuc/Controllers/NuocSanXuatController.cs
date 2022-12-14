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
            ViewData["Title"] = "Danh mục nước sản xuất";
            webContext context = new webContext();
            ViewBag.nuocsx = context.NuocSanXuat.ToList();
            return View("TableNuocSX");
        }

        //hiển thị view thêm nhà sản xuất
       public IActionResult ViewInsertNSX()
        {
            ViewData["Title"] = "Thêm danh mục đơn vị tính";

            return View();
        }

        // thêm nhà sản xuất
        [HttpPost]
       public IActionResult InsertNSX(NuocSanXuat nsx)
        {
            webContext context = new webContext();
            nsx.Nvtao = 3;
            nsx.NgayTao = DateTime.Now;
            context.Add(nsx);
            context.SaveChanges();
            return RedirectToAction("table");

        }
        //xóa nhà sản xuất
        [Route("/NuocSanXuat/delete/{id}")]
        public IActionResult Delete(int id)
        {
            webContext context = new webContext();
            NuocSanXuat nsx = context.NuocSanXuat.Find(id);
            nsx.Active = false;

            context.NuocSanXuat.Update(nsx);
            context.SaveChanges();
            return RedirectToAction("table");
        }

        // dẫn tới view update
        [Route("/NuocSanXuat/Update/{id}")]
        public IActionResult ViewUpdate(int id)
        {
            webContext context = new webContext();
            NuocSanXuat nsx = context.NuocSanXuat.Find(id);
            return View(nsx);
        }

        //update nhà sản xuất
        public IActionResult Update(NuocSanXuat nsx)
        {
            webContext context = new webContext();
            NuocSanXuat n = context.NuocSanXuat.Find(nsx.Id);

            n.MaNsx = nsx.MaNsx;
            n.TenNsx = nsx.TenNsx;
            n.Idcn = nsx.Idcn;
            n.Nvsua = 3;
            n.NgaySua = DateTime.Now;

            context.NuocSanXuat.Update(n);
            context.SaveChanges();
            return RedirectToAction("table");
        }

    }
}
