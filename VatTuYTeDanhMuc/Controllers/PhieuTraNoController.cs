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



    [HttpPost("/getPTN")]
    public IActionResult loadTablePTN(int idncc)
    {
      webContext context = new webContext();
     // var listnhncc = context.ChiTietTraNo.Where(a => a.IdptnNavigation.Idncc == idncc && a.Active == true).Include(x => x.IdpnNavigation.Idncc == idncc).ToList();
      var listptn = context.PhieuTraNo.Where(a => a.IdnccNavigation.Id == idncc && a.Active == true).ToList();
      ViewBag.ListPTN = listptn;
      return PartialView();
    }





    [HttpPost("/getNganHangNCC")]
    public IActionResult loadNHNCC(int idncc, int idhttt)
    {
      webContext context = new webContext();
      string options = null;
        var listnhncc = context.NccNganHang.Where(a => a.Idncc == idncc && a.IdnhNavigation.Idhttt== idhttt && a.Active == true).Include(x => x.IdnccNavigation).Include(x => x.IdnhNavigation).ToList();
        int i = 0;
        // = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
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
    }






  }
}
