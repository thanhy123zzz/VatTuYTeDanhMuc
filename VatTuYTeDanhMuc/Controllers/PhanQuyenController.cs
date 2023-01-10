using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTe.Controllers
{
  public class PhanQuyenController : Controller
  {

    public IActionResult Table()
    {
      listPQ();
      ViewData["Title"] = "Phân Quyền";
      return View("TablePQ");
    }

    //0 yy
    public void listPQ()
    {
      webContext context = new webContext();
      List<PhanQuyen> listvt = context.PhanQuyen.Where(q => q.Active == true).ToList();
      List<PhanQuyen> list = listvt;
      foreach (var pq in listvt)
      {
        int i = 0;
        foreach (var item in list)
        {
          if (item.Idtk == pq.Idtk)
          {
            i++;
          }
        }
        if (i > 1)
        {
          list = list.Where(mn => mn.Id != pq.Id).ToList();
        }
      }
      ViewBag.ListPQQ = list;
    }



    //0000 yy okk
    //Xóa (ẩn) phân quyền
    [HttpPost("/removePQ")]
    public string removePQ(int id)
    {
      webContext context = new webContext();
      PhanQuyen pq = context.PhanQuyen.Find(id);
      pq.Active = false;
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      pq.Nvsua = idUser;
      pq.NgaySua = DateTime.Now;
      context.Update(pq);
      context.SaveChanges();
      return "Xoá thành công!";
    }


    //ok yy
    [HttpPost("/loadPQCN")]
    public IActionResult loadPQCN(int idtk, int idcn, int idpq)
    {
      webContext context = new webContext();
      List<PhanQuyen> listpq = new List<PhanQuyen>();
      if (idcn != 0)
      {
        listpq = context.PhanQuyen.Where(a => a.Idcn == idcn && a.Idtk == idtk && a.Active == true).ToList();
      }
      else
      {
        listpq = context.PhanQuyen.Where(a => a.Idtk == idtk && a.Active == true).ToList();
      }
      
      List<VaiTro> listvt = context.VaiTro.Where(d => d.Active == true).ToList();
      foreach ( var item in listvt)
      {
        foreach (var pq in listpq)
        {
          if (item.Id == pq.Idvt)
          {
            listvt = listvt.Where(cnn => cnn.Id != pq.Idvt).ToList();
          }
        }
      }

      if (idpq != 0)
      {
        ViewBag.PhanQuyen = context.PhanQuyen.FirstOrDefault(k => k.Active == true && k.Id == idpq);
      }

      ViewBag.ListPQCN = listvt;
      //get taikhoan(idtk)
      return PartialView();
    }

    //yy
    [HttpPost("/AddPQ")]
    public string AddPQ(int idtk, int idvt, int idcn)
    {
      webContext context = new webContext();
      PhanQuyen pq = new PhanQuyen();

      if (idvt ==0 || idcn ==0)
      {
        if (idcn == 0)
        {
          return "Hãy chọn chi nhánh!";
        }
        if (idvt == 0)
        {
          return "Hãy chọn vai trò!";
        }      
        return "Lỗi!";
      }
      else
      {
        int idUser = int.Parse(User.Claims.ElementAt(3).Type);
        pq.Idtk = idtk;
        pq.Idvt = idvt;
        pq.Idcn = idcn;
        pq.Nvtao = idUser;
        pq.Nvsua = idUser;
        pq.NgayTao = DateTime.Now;
        pq.NgaySua = DateTime.Now;
        pq.Active = true;
        context.PhanQuyen.Add(pq);
        context.SaveChanges();
        return "Thêm thành công";
      }    
    }


    //Mới

    //Load table phân quyền okk
    [HttpPost("/loadTablePQ")]
    public IActionResult loadTablePQ(int idtk)
    {
      webContext context = new webContext();
      List<PhanQuyen> listpq = context.PhanQuyen.Where(p => p.Idtk == idtk && p.Active == true).OrderByDescending(p => p.Idcn).ToList();
      ViewBag.ListPQ = listpq;
      ViewBag.IDTK = idtk;
      return PartialView();
    }

    ///search nhan vien okk
    [HttpPost("/searchTableNV")]
    public IActionResult searchTableNV(string key)
    {
      webContext context = new webContext();
      if (key == "" || key == null)
      {
        ViewBag.loadTableNV = context.NhanVien.Where(x => x.Active == true).ToList();
        return PartialView("LoadTableNV");
      }
      else
      {
        ViewBag.loadTableNV = context.NhanVien.FromSqlRaw("SELECT * FROM NhanVien WHERE concat_ws(' ',TenNV,MaNV) LIKE N'%" + key + "%';").ToList();
        return PartialView("LoadTableNV");
      }
    }



    //Tạo dòng enable-edit okk
    [HttpPost("/addNewRowPQ")]
    public IActionResult addNewRowPQ(int idpq)
    {
      webContext context = new webContext();
      if (idpq == 0)
      {
        return PartialView();
      }
      else
      {
        ViewBag.PhanQuyen = context.PhanQuyen.FirstOrDefault(h => h.Active == true && h.Id == idpq);
        return PartialView();
      }
    }

    //cập nhật vai trò
    [HttpPost("/updatepq")]
    public string updatepq(int idvt, int idpq, int idcn)
    {
      webContext context = new webContext();
      PhanQuyen pq = context.PhanQuyen.Find(idpq);
      int idUser = int.Parse(User.Claims.ElementAt(3).Type);
      pq.Idvt = idvt;
      pq.Idcn = idcn;
      pq.Nvsua = idUser;
      pq.NgaySua = DateTime.Now;
      pq.Active = true;
      context.PhanQuyen.Update(pq);
      context.SaveChanges();
      return "Sửa thành công";
    }

  }
}
