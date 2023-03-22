using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
      var listptn = context.PhieuTraNo.Include(a=> a.ChiTietTraNo).Where(a => a.IdnccNavigation.Id == idncc && a.Active == true).ToList();
        //    var phieu = context.PhieuNhap.Include(x => x.ChiTietPhieuNhap).Include(x => x.IdnccNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPN);
      
      string list = null;
      foreach (PhieuTraNo item in listptn)
      {
        double? conlai = item.TongTien - item.ChiTietTraNo.Sum(x => x.SoTien);
        
        list += "<tr ><td class='text-start'>" + item.SoPhieu +"</td>" +
                      "<td class='text-center'>" + item.NgayTra?.ToString("dd/MM/yyyy")+"</td>" +
                      "<td class='text-end'>" + item.TongTien + "</td>" +
                      "<td class='text-end'>" + item.ChiTietTraNo.Sum(x=>x.SoTien)+"</td>"+
                      "<td class='text-end' class='remaining-amount'>" + conlai + "</td>" +
                      "<td class='text-end'><input type='text' style='text-align: right;' class='payment-input input-text qty text form-control' onkeypress='return isMoney(event)' onchange='updateTongTien()' oninput='inputTongTien()' /></td>" +
                 "</tr>";
      }

      string listempty = "<tr><td>...</td><td>...</td><td>...</td><td>...</td><td>...</td>" +
                 "<td><input type='text' disabled/></td></tr>";

      return Json(new
      {
        list = list,
        listempty = listempty
      });

      //ViewBag.ListPTN = listptn;
     // return PartialView();
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
