//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

    [Authorize(Roles = "NV")]
    public IActionResult ViewSelector()
    {
      //ViewBag.TaiKhoan = TempData["TaiKhoan"];
      //ViewBag.ChiNhanh = TempData["ChiNhanh"];
      //return View();

      string user = User.Claims.ElementAt(0).Value;
      ViewBag.TaiKhoan = user;

      webContext context = new webContext();
      int idtk = context.TaiKhoan.FirstOrDefault(k => k.Active == true && k.TenTaiKhoan == user).Id;
      ViewBag.ChiNhanh = context.PhanQuyen.Where(aa => aa.Idtk.Equals(idtk) && aa.Active == true).Select(aa => aa.Idcn).Distinct().ToList();

      return View();

    }





    [HttpPost]
    [Authorize(Roles = "NV")]
    public IActionResult Selector(PhanQuyen pq)
    {
      webContext context = new webContext();

      var identity = new ClaimsIdentity(User.Identity);
      identity.AddClaim(new Claim("VaiTro", pq.Idvt.ToString()));
      identity.AddClaim(new Claim("ChiNhanh", pq.Idcn.ToString()));

      var claims = identity.Claims.ToList();

      var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

      return RedirectToAction("index","Home");
    }


    [HttpPost("/loadQuyen")]
    [Authorize(Roles = "NV")]
    public IActionResult loadVaiTro(string tk, int idcn)
    {
      webContext context = new webContext();

      int idtk = context.TaiKhoan.FirstOrDefault(k => k.Active == true && k.TenTaiKhoan == tk).Id;

      if (idcn != 0)
      {
        var listvt = context.PhanQuyen.Where(a => a.Idcn == idcn && a.Idtk == idtk && a.Active == true).Select(j => j.Idvt).Distinct().ToList();
        ViewBag.ListPQCN = listvt;
      }
      else
      {
        var listvt = context.PhanQuyen.Where(a => a.Idtk.Equals(idtk) && a.Active == true).Select(j => j.Idvt).Distinct().ToList();
        ViewBag.ListPQCN = listvt;
      }    
      return PartialView();
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
          // PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id) && c.Active == true);

          //var listpq = context.PhanQuyen.Where(j => j.Idtk.Equals(acc.Id) && j.Active == true).Select(j => j.Id).Distinct().ToList();
          //TempData["TaiKhoan"] = user;
          //TempData["ChiNhanh"] = listpq;

          if (acc.Loai == true)
          {
            NhanVien nv = context.NhanVien.FirstOrDefault(b => b.TenTaiKhoan.Equals(acc.TenTaiKhoan));

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.TenTaiKhoan),
                    new Claim(ClaimTypes.Role, "NV"),
                    //new Claim("User", nv.Idcn.ToString()),
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
              return RedirectToAction("ViewSelector");
              //return RedirectToAction("Index", "Home");
            }
          }
          else
          {
            KhachHang kh = context.KhachHang.FirstOrDefault(s => s.TenTaiKhoan.Equals(acc.TenTaiKhoan));
            PhanQuyen vaitro = context.PhanQuyen.FirstOrDefault(c => c.Idtk.Equals(acc.Id) && c.Active == true);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.TenTaiKhoan),
                    new Claim(ClaimTypes.Role, "KH"),                 
                    new Claim(kh.Id.ToString(), kh.TenKh),
                    new Claim("VaiTro", vaitro.Idvt.ToString()),
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
