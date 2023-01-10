using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTe.Controllers
{
    public class LoginController : Controller
    {
      private readonly ILogger<LoginController> _logger;

      public LoginController(ILogger<LoginController> logger)
      {
          _logger = logger;
      }
    
      private webContext context = new webContext();
      public IActionResult Login()
      {
          return View();
      }

      [Route("/Login")]
      public IActionResult LogIn(string returnUrl)
      {
        TempData["returnUrl"] = returnUrl;
        return View();
      }

      [HttpPost]
      public ActionResult LogIn(TaiKhoan account, string returnUrl)
      {
        if (ModelState.IsValid)
        {
          returnUrl = (string)TempData["returnUrl"];
          string user = account.TenTaiKhoan;
          string pass = account.MatKhau;
          var acc = context.TaiKhoan.FirstOrDefault(s => s.TenTaiKhoan.Equals(user) && s.MatKhau.Equals(pass));
          if (acc != null)
          {
            PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id));
            if (acc.Loai == true)
            {           
              NhanVien nv = context.NhanVien.FirstOrDefault(b => b.TenTaiKhoan.Equals(acc.TenTaiKhoan));                          
              //int vt = vaitro.Id;

              var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.TenTaiKhoan),
                    new Claim(ClaimTypes.Role, "NV"),
                    new Claim("VaiTro", vaitro.Idvt.ToString()),
                    new Claim(nv.Id.ToString(), nv.TenNv),
                };
              var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
              HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

              if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
              {
                return Redirect(returnUrl);
              }
              else
              {
                return RedirectToAction("Index", "Home");
              }
            }
            else
            {
              KhachHang kh = context.KhachHang.FirstOrDefault(s => s.TenTaiKhoan.Equals(acc.TenTaiKhoan));
              var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.TenTaiKhoan),
                    new Claim(ClaimTypes.Role, "KH"),
                    new Claim("VaiTro", vaitro.Idvt.ToString()),
                    new Claim(kh.Id.ToString(), kh.TenKh),
                };
              var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
              HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

              if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
              {
                return Redirect(returnUrl);
              }
              else
              {
                return RedirectToAction("Index", "Home");
              }
            }
          }
          else
          {
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác!");
          }
        }
        return View();
      }
      [Authorize]
      public ActionResult LogOut()
      {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
      }
  }
}
