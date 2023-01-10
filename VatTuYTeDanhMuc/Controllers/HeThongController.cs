using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTe.Controllers
{
  [Authorize]
  public class HeThongController : Controller
  {
    
    [Route("/HeThong")]
    public IActionResult Index()
    {
      return View("ViewHeThong");
    }
        //Thông tin hồ sơ
    public IActionResult HoSo(int id)
    {
      webContext context = new webContext();
      ViewBag.idUser = id;
      NhanVien nv = context.NhanVien.FirstOrDefault(a => a.Id.Equals(id));
      return View(nv);
    }

    [HttpPost("/change-pass")]
    public string ChangePassword(string passWord,string newPassWord)
    {
      webContext context = new webContext();
      TaiKhoan tk = context.TaiKhoan.Where(x=>x.TenTaiKhoan==User.Identity.Name).FirstOrDefault();
        if (tk.MatKhau != passWord)
       {
                return "<div class='invalid-feedback trangThai' style='display:block;'>Mật khẩu không đúng</div>";
            }
            else
            {
                tk.MatKhau = newPassWord;
                context.TaiKhoan.Update(tk);
                context.SaveChanges();
                return "<div class='valid-feedback trangThai' style='display:block;'>Đổi mật khẩu thành công</div>";
            }
    }




  }
}
