using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
  public class PhieuTraNoController : Controller
  {

    private ICompositeViewEngine _viewEngine;
    public PhieuTraNoController(ICompositeViewEngine viewEngine)
    {
      _viewEngine = viewEngine;
    }

    [Route("/PhieuTraNo")]
    public IActionResult Index()
    {
      return View("PhieuTraNo");
    }


    [HttpPost("/getNganHangNCC")]
    public IActionResult loadNHNCC(int idncc)
    {
      webContext context = new webContext();
    //  List<NccNganHang> listnhncc = context.NccNganHang.Where(a => a.Idncc == idncc && a.Active == true).ToList();
      var listnhncc = context.NccNganHang.Where(a => a.Idncc == idncc && a.Active == true).Include(x => x.IdnccNavigation).Include(x => x.IdnhNavigation).ToList();
      //  ViewBag.ListNHNCC = listnhncc;
      //get taikhoan(idtk)

      int i = 0;
      string options = null;// = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
      foreach (NccNganHang d in listnhncc)
      {
        if (i == 0)
        {
          options = "<option selected value = '" + d.Idnh + "'>" + d.IdnhNavigation.TenNh + "</option>";
        }
        else 
        { 
          options += "<option value = '" + d.Idnh + "'>" + d.IdnhNavigation.TenNh + "</option>";
        }
        i++;
      }

      return Json(new
      {
        options = options
      });

      //return PartialView();
    }






  }
}
