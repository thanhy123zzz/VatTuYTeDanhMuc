using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
  public class DVVCController : Controller
  {
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục dịch vụ vận chuyển";
      webContext context = new webContext();
      TempData["Menu"] = context.Menu.FirstOrDefault(menu => EF.Functions.Like(menu.TenMenu, "%Danh mục dịch vụ vận chuyển%") && menu.Active == true).Id;
      return View("TableDVVC");
    }

    //Trả về view thêm dịch vụ vận chuyển
    public IActionResult ViewInsertDVVC()
    {
      ViewData["Title"] = "Thêm dịch vụ vận chuyển";
      return View();
    }
    //Trả về view sửa dịch vụ vận chuyển
    [Route("/DVVC/ViewUpdateDVVC/{id}")]
    public IActionResult ViewUpdateDVVC(int id)
    {
      ViewData["Title"] = "Sửa dịch vụ vận chuyển";
      webContext context = new webContext();
      Dvvc dvt = context.Dvvc.Find(id);
      return View(dvt);
    }

    //insert dịch vụ vận chuyển
    [HttpPost]
    public IActionResult insertDVVC(Dvvc dvt)
    {
      webContext context = new webContext();
      dvt.Nvtao = 3;
      dvt.Active = true;
      dvt.NgayTao = DateTime.Now;
      context.Dvvc.Add(dvt);
      context.SaveChanges();
      TempData["ThongBao"] = "Thêm thành công!";
      return RedirectToAction("Table");
    }

    //update dịch vụ vận chuyển
    [HttpPost]
    public IActionResult updateDVVC(Dvvc dvt)
    {
      webContext context = new webContext();
      Dvvc dv = context.Dvvc.Find(dvt.Id);

      dv.Nvsua = 3;
      dv.NgaySua = DateTime.Now;
      dv.TenDvvc = dvt.TenDvvc;
      dv.MaDvvc = dvt.MaDvvc;
      dv.GhiChu = dvt.GhiChu;

      context.Dvvc.Update(dv);
      context.SaveChanges();
      TempData["ThongBao"] = "Sửa thành công!";
      return RedirectToAction("Table");
    }

    //Xoá dịch vụ vận chuyển
    [Route("/DVVC/xoa/{id}")]
    public IActionResult deleteDVVC(int id)
    {
      webContext context = new webContext();
      Dvvc dvt = context.Dvvc.Find(id);
      dvt.Active = false;

      context.Dvvc.Update(dvt);
      context.SaveChanges();
      return RedirectToAction("Table");
    }
    [HttpPost("/loadTableDVVC")]
    public IActionResult loadTableDVVC(bool active)
    {
      webContext context = new webContext();
      if (active)
      {
        ViewBag.DVVC = context.Dvvc.Where(x => x.Active == true).ToList();
      }
      else
      {
        ViewBag.DVVC = context.Dvvc.ToList();
      }
      return PartialView();
    }
    [Route("/DVVC/khoiphuc/{id}")]
    public IActionResult khoiphucDVVC(int id)
    {
      webContext context = new webContext();
      Dvvc hsx = context.Dvvc.Find(id);
      hsx.Active = true;

      context.Dvvc.Update(hsx);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }
  }
}
