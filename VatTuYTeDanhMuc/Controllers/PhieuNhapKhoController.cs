using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using VatTuYTeDanhMuc.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace VatTuYTeDanhMuc.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        private ICompositeViewEngine _viewEngine;
        public PhieuNhapKhoController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        [Route("/PhieuNhapKho")]
        public IActionResult Index()
        {
            webContext context = new webContext();
            List<NhaCungCap> listNhaCupCap = context.NhaCungCap.ToList();
            return View("PhieuNhapKho");
        }
        [HttpPost("/them-phieu-nhap")]
        public IActionResult ThemPhieuNhap(PhieuNhap phieuNhap,IFormCollection form)
        {

            return RedirectToAction("Index");
        }

        [HttpPost("/getDonViTinh")]
        public JsonResult getTenDVT(int idHH)
        {
            webContext context = new webContext();
            HangHoa hh = context.HangHoa.Find(idHH);
            Dvt dvt = context.Dvt.Find(hh.Iddvtchinh);
            return Json(
                new { 
                    tenDVT = dvt.TenDvt, 
                    GiaBan = hh.GiaBanSi 
                    });
        }

        [HttpPost("/add-Chi-Tiet-Phieu")]
        public JsonResult addChiTietPhieu(int idHH, string SoLo, float ThueXuat, int SL, float DonGia, 
            float ThanhTien, float ChietKhau, DateTime HanDung, DateTime NgaySX)
        {
            webContext context = new webContext();
            ChiTietPhieuNhapTam ct = new ChiTietPhieuNhapTam();
            ct.Active = true;
            ct.Idhh = idHH;
            ct.SoLo = SoLo;
            ct.Thue = ThueXuat;
            ct.Slg = SL;
            ct.DonGia = DonGia;
            ct.Cktm = ChietKhau;
            ct.Hsd = HanDung;
            ct.Nsx = NgaySX;
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = 3;
            ct.NgayTao = DateTime.Now;
            context.ChiTietPhieuNhapTam.Add(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult , _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,
                tienCK = TienCK,
                tienThue = TienThue,
                tienThanhToan = TienThanhToan
            });
        }

        [HttpPost("/edit-Chi-Tiet-Phieu")]
        public IActionResult editChiTietPhieu(int idHH, string SoLo, float ThueXuat, int SL, float DonGia,
            float ThanhTien, float ChietKhau, DateTime HanDung, DateTime NgaySX, int id)
        {
            webContext context = new webContext();
            ChiTietPhieuNhapTam ct = context.ChiTietPhieuNhapTam.Find(id);
            ct.Active = true;
            ct.Idhh = idHH;
            ct.SoLo = SoLo;
            ct.Thue = ThueXuat;
            ct.Slg = SL;
            ct.DonGia = DonGia;
            ct.Cktm = ChietKhau;
            ct.Hsd = HanDung;
            ct.Nsx = NgaySX;
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = 3;
            ct.NgayTao = DateTime.Now;
            context.ChiTietPhieuNhapTam.Update(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,
                tienCK = TienCK,
                tienThue = TienThue,
                tienThanhToan = TienThanhToan
            });
        }
        [HttpPost("/delete-Chi-Tiet-Phieu")]
        public IActionResult deletePhieuNhapTam(int id)
        {
            webContext context = new webContext();
            ChiTietPhieuNhapTam ch = context.ChiTietPhieuNhapTam.Find(id);
            context.ChiTietPhieuNhapTam.Remove(ch);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuNhap");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,
                tienCK = TienCK,
                tienThue = TienThue,
                tienThanhToan = TienThanhToan
            });
        }

        [HttpPost("/load-table-chitiet")]
        public IActionResult loadTableChitiet ()
        {
            return PartialView("TableChiTietPhieuNhap");
        }
            string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        [HttpPost("/editChitietPhieuNhapTam")]
        public IActionResult editChitietPhieuNhapTam(int id)
        {
            webContext context = new webContext();
            if (id > 0)
            {
                return PartialView("GroupChitietPhieuNhapTam", context.ChiTietPhieuNhapTam.Find(id));
            }
            else{
                return PartialView("GroupChitietPhieuNhapTam");
            }
        }
        public string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

                vResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
