using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        [Route("/PhieuNhapKho")]
        public IActionResult Index()
        {
            webContext context = new webContext();
            List<NhaCungCap> listNhaCupCap = context.NhaCungCap.ToList();



            return View();
        }
    }
}
