using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;
using VatTuYTeDanhMuc.Models;
using VatTuYTe.Models;

namespace VatTuYTe.Controllers
{
  public class VaiTroController : Controller
  {
    public IActionResult Table()
    {
      ViewData["Title"] = "Danh mục vai trò";
      return View("TableRoles");
    }

    //Load table chức năng
    [HttpPost("/loadTableCN")]
    public IActionResult loadTableCN(int idvt)
    {
      webContext context = new webContext();
      List<ChucNang> listcn = context.ChucNang.Where(x => x.Idvt == idvt && x.Active == true).ToList();
      ViewBag.ListCN = listcn;
      ViewBag.IDVT = idvt;
      return PartialView();
    }

    //update list chucnang
    [HttpPost("/updateRoles")]
    public JsonResult updateRoles([FromBody] IEnumerable <FunctionModel> list)
    {
      webContext context = new webContext();     
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      
      foreach (var item in list)
      {
        ChucNang cn = context.ChucNang.FirstOrDefault(n => n.Idmenu == item.Idmenu && n.Idvt == item.Idvt && n.Active == true);
        if (cn == null)
        {
          ChucNang cnn = new ChucNang();
          cnn.Idmenu = item.Idmenu;
          cnn.Idvt = item.Idvt;
          cnn.Nhap = item.Nhap;
          cnn.Sua = item.Sua;
          cnn.Xoa = item.Xoa;
          cnn.Xuat = item.Xuat;
          cnn.In = item.In;
          cnn.CaNhan = item.CaNhan;
          cnn.Nvtao = idUser;
          cnn.NgayTao = DateTime.Now;
          cnn.Nvsua = idUser;
          cnn.NgaySua = DateTime.Now;
          cnn.Active = true;
          context.ChucNang.Add(cnn);
        }
        else
        {
          cn.Nhap = item.Nhap;
          cn.Sua = item.Sua;
          cn.Xoa = item.Xoa;
          cn.Xuat = item.Xuat;
          cn.In = item.In;
          cn.CaNhan = item.CaNhan;
          cn.Nvsua = idUser;
          cn.NgaySua = DateTime.Now;
          context.ChucNang.Update(cn);
        }       
      }
      context.SaveChanges();
      return Json(data: "Cập nhật thành công!");

    }

    //Tạo dòng enable-edit 
    [HttpPost("/addNewRow")]
    public IActionResult addNewRow(int idvt)
    {
      webContext context = new webContext();
      if (idvt == 0)
      {
        return PartialView();
      }
      else
      {
        ViewBag.VaiTro = context.VaiTro.FirstOrDefault(x => x.Active == true && x.Id == idvt);
        return PartialView();
      }    
    }

    //load table vai trò
    [HttpPost("/loadTableVT")]
    public IActionResult loadTableVT()
    {
      webContext context = new webContext();
      ViewBag.loadTableVT = context.VaiTro.Where(x => x.Active == true).OrderBy(x => x.TenVt).ToList();
      return PartialView();
    }

    //thêm vai trò
    [HttpPost("/addVT")]
    public string addVT(string tenVT)
    {
      webContext context = new webContext();
      VaiTro vt = new VaiTro();
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      vt.TenVt = tenVT;
      vt.Nvtao = idUser;
      vt.Nvsua = idUser;
      vt.NgayTao = DateTime.Now;
      vt.NgaySua = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Add(vt);
      context.SaveChanges();
      return "Thêm thành công";
    }

    //khôi phục vai trò (chưa dùng)
    [Route("/Roles/khoiphuc/{id}")]
    public IActionResult khoiphucRoles(int id)
    {
      webContext context = new webContext();
      VaiTro vt = context.VaiTro.Find(id);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      vt.Nvsua = idUser;
      vt.NgaySua = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Update(vt);
      context.SaveChanges();
      TempData["ThongBao"] = "Khôi phục thành công!";
      return RedirectToAction("Table");
    }


    ///search vai trò
    [HttpPost("/searchTableVT")]
    public IActionResult searchTableVT(string key)
    {
      webContext context = new webContext();
      if (key == "" || key == null)
      {
        ViewBag.loadTableVT = context.VaiTro.Where(x => x.Active == true).ToList();
        return PartialView("LoadTableVT");
      }
      else
      {
        ViewBag.loadTableVT = context.VaiTro.FromSqlRaw("SELECT * FROM VaiTro WHERE concat_ws(' ',TenVT,TenVT) LIKE N'%" + key + "%';").ToList();
        return PartialView("loadTableVT");
      }
    }


    //cập nhật vai trò
    [HttpPost("/updatevaitro")]
    public string updatevaitro(int idvt, string tenvt)
    {
      webContext context = new webContext();
      VaiTro vt = context.VaiTro.Find(idvt);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      vt.TenVt = tenvt;   
      vt.Nvsua = idUser;
      vt.NgaySua = DateTime.Now;
      vt.Active = true;
      context.VaiTro.Update(vt);
      context.SaveChanges();
      return "Sửa thành công";
    }

    //Xóa (ẩn) vai trò
    [HttpPost("/removeVT")]
    public string removeVT(int idvt)
    {
      webContext context = new webContext();
      VaiTro vt = context.VaiTro.Find(idvt);
      vt.Active = false;
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      vt.Nvsua = idUser;
      vt.NgaySua = DateTime.Now;
      context.Update(vt);
      context.SaveChanges();
      return "Xoá thành công!";
    }


  }
}
