using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using VatTuYTeDanhMuc.Models.Entities;

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
            return View("PhieuNhapKho");
        }
        [HttpPost("/them-phieu-nhap")]
        public IActionResult ThemPhieuNhap(PhieuNhap phieuNhap, string NgayHd, string NgayTao)
        {

            webContext context = new webContext();
            SoLuongHhcon soLuongHhcon = new SoLuongHhcon();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            string h = GetLocalIPAddress();
            List<ChiTietPhieuNhapTam> ListCTPNT = context.ChiTietPhieuNhapTam.Where(x => x.Host == h).OrderByDescending(x => x.Id).ToList();
            var tran = context.Database.BeginTransaction();
            try
            {
                phieuNhap.NgayHd = DateTime.ParseExact(NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                phieuNhap.NgayTao = DateTime.ParseExact(NgayTao, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                phieuNhap.Active = true;
                phieuNhap.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
                phieuNhap.Idnv = idUser;
                phieuNhap.SoPn = getSoPhieu();
                context.PhieuNhap.Add(phieuNhap);
                context.SaveChanges();
                foreach (ChiTietPhieuNhapTam t in ListCTPNT)
                {
                    ChiTietPhieuNhap ct = new ChiTietPhieuNhap();
                    ct.Idhh = t.Idhh;
                    ct.Idbh = t.Idbh;
                    ct.Thue = t.Thue;
                    ct.Idpn = phieuNhap.Id;
                    ct.Slg = t.Slg;
                    ct.Tgbh = t.Tgbh;
                    ct.DonGia = t.DonGia;
                    ct.Cktm = t.Cktm;
                    ct.Nsx = t.Nsx;
                    ct.Hsd = t.Hsd;
                    ct.SoLo = t.SoLo;
                    ct.GhiChu = t.GhiChu;
                    ct.Active = true;
                    ct.Nvtao = idUser;
                    ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    context.ChiTietPhieuNhap.Add(ct);
                    context.ChiTietPhieuNhapTam.Remove(t);
                    context.SaveChanges();

                    SoLuongHhcon sl = new SoLuongHhcon();
                    sl.Idctpn = ct.Id;
                    sl.Slcon = Math.Round((double)ct.Slg, 2);
                    sl.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
                    sl.NgayNhap = phieuNhap.NgayTao;
                    sl.Thue = ct.Thue;
                    sl.Cktm = ct.Cktm;
                    sl.GiaNhap = ct.DonGia;
                    context.SoLuongHhcon.Add(sl);
                    context.SaveChanges();
                }
                var stt = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'NhapKho'").FirstOrDefault();
                stt.Stt += 1;
                context.SoThuTu.Update(stt);
                context.SaveChanges();
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                TempData["ThongBao"] = e.Message;
                return RedirectToAction("Index");
            }
            TempData["ThongBao"] = "Thêm thành công";
            return RedirectToAction("Index");
        }

        [HttpPost("/getDonViTinh")]
        public JsonResult getTenDVT(int idHH)
        {
            webContext context = new webContext();
            HangHoa hh = context.HangHoa.Find(idHH);
            Dvt dvt = context.Dvt.Find(hh.Iddvtchinh);
            return Json(
                new
                {
                    tenDVT = dvt.TenDvt,
                    GiaBan = hh.GiaBanSi
                });
        }

        [HttpPost("/add-Chi-Tiet-Phieu")]
        public JsonResult addChiTietPhieu(int idHH, string SoLo, float ThueXuat, float SL, float DonGia,
            float ChietKhau, string HanDung, string NgaySX)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            webContext context = new webContext();
            ChiTietPhieuNhapTam ct = new ChiTietPhieuNhapTam();
            ct.Active = true;
            ct.Idhh = idHH;
            ct.SoLo = SoLo;
            ct.Thue = Math.Round(ThueXuat, 2);
            ct.Slg = Math.Round(SL, 2);
            ct.DonGia = Math.Round(DonGia, 2);
            ct.Cktm = Math.Round(ChietKhau, 2);
            ct.Hsd = DateTime.ParseExact(HanDung, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ct.Nsx = DateTime.ParseExact(NgaySX, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = idUser;
            ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            context.ChiTietPhieuNhapTam.Add(ct);
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

        [HttpPost("/edit-Chi-Tiet-Phieu")]
        public IActionResult editChiTietPhieu(int idHH, string SoLo, float ThueXuat, int SL, float DonGia,
            float ThanhTien, float ChietKhau, string HanDung, string NgaySX, int id)
        {
            webContext context = new webContext();
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            ChiTietPhieuNhapTam ct = context.ChiTietPhieuNhapTam.Find(id);
            ct.Active = true;
            ct.Idhh = idHH;
            ct.SoLo = SoLo;
            ct.Thue = ThueXuat;
            ct.Slg = SL;
            ct.DonGia = DonGia;
            ct.Cktm = ChietKhau;
            ct.Hsd = DateTime.ParseExact(HanDung, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ct.Nsx = DateTime.ParseExact(NgaySX, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = idUser;
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
        public IActionResult loadTableChitiet()
        {
            return PartialView("TableChiTietPhieuNhap");
        }

        [HttpPost("/editChitietPhieuNhapTam")]
        public IActionResult editChitietPhieuNhapTam(int id)
        {
            webContext context = new webContext();
            if (id > 0)
            {
                return PartialView("GroupChitietPhieuNhapTam", context.ChiTietPhieuNhapTam.Find(id));
            }
            else
            {
                return PartialView("GroupChitietPhieuNhapTam");
            }
        }

        [HttpPost("/ViewThongTinPhieuNhap")]
        public IActionResult ViewThongTinPhieuNhap(int idPN)
        {
            webContext context = new webContext();
            var phieu = context.PhieuNhap.Include(x => x.ChiTietPhieuNhap).Include(x => x.IdnccNavigation).Include(x => x.IdnvNavigation).FirstOrDefault(x => x.Id == idPN);
            return PartialView(phieu);
        }

        [HttpPost("/loadTableLichSuNhap")]
        public IActionResult loadTableLichSuNhap(string fromDay, string toDay, string soPhieuLS, string soHDLS, int nhaCC, int hhLS)
        {
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            webContext context = new webContext();
            List<PhieuNhap> listPhieu = context.PhieuNhap
            .Where(x => x.NgayTao.Value.Date >= FromDay
            && x.NgayTao.Value.Date <= ToDay
            && x.Active == true)
            .Include(x => x.IdnccNavigation)
            .Include(x => x.IdnvNavigation)
            .Include(x => x.ChiTietPhieuNhap)
            .OrderByDescending(x => x.Id)
            .ToList();
            if (nhaCC == 0 && hhLS == 0)
            {
                ViewBag.ListPhieuNhap = listPhieu.Where(x => (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS) ?? false))
                && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper())));
            }
            else
            {

                ViewBag.ListPhieuNhap = listPhieu.Where(x => (hhLS == 0 ? true : (x.ChiTietPhieuNhap.Where(y => y.Idhh == hhLS).Count() > 0 ? true : false))
                && (nhaCC == 0 ? true : x.Idncc == nhaCC)
                && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper()))
                && (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS.ToUpper()) ?? false)));
            }

            return PartialView("TableLichSuNhap");
        }
        [Route("/download/phieunhapkho/{id:int}")]
        public IActionResult downloadPhieuNhap(int id)
        {
            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1280;
            fullView.Options.PdfPageSize = PdfPageSize.A4;
            fullView.Options.MarginTop = 20;
            fullView.Options.MarginBottom = 20;
            fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var pdf = fullView.ConvertUrl(currentUrl + "/phieuNhapKhoPDF/" + id);

            var pdfBytes = pdf.Save();
            return File(pdfBytes, "application/pdf", "PhieuNhap.pdf");
        }

        [Route("/phieuNhapKhoPDF/{id:int}")]
        public IActionResult viewPDF(int id)
        {
            webContext context = new webContext();
            var phieu = context.PhieuNhap
                .Include(x => x.IdnccNavigation)
                .Include(x => x.ChiTietPhieuNhap)
                .Where(x => x.Id == id).FirstOrDefault();
            return View("PhieunhapkhoPDF", phieu);
        }

        [HttpPost("/download/BaoCaoPhieuNhap")]
        public IActionResult downloadBaoCaoPhieuNhap(string fromDay, string toDay, string soPhieuLS, string soHDLS, int nhaCC, int hhLS)
        {
            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1280;
            fullView.Options.PdfPageSize = PdfPageSize.A4;
            fullView.Options.MarginTop = 20;
            fullView.Options.MarginBottom = 20;
            fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var url = Url.Action("viewBaoCaoPhieuNhapPDF", "PhieuNhapKho", new { fromDay = fromDay, toDay = toDay, soPhieuLS = soPhieuLS, soHDLS = soHDLS, nhaCC = nhaCC, hhLS = hhLS });

            var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + url;

            var pdf = fullView.ConvertUrl(currentUrl);

            var pdfBytes = pdf.Save();
            return File(pdfBytes, "application/pdf", "BaoCaoPhieuNhap.pdf");
        }
        public IActionResult viewBaoCaoPhieuNhapPDF(string fromDay, string toDay, string soPhieuLS, string soHDLS, int nhaCC, int hhLS)
        {
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ViewBag.tuNgay = fromDay;
            ViewBag.denNgay = toDay;
            webContext context = new webContext();
            List<PhieuNhap> listPhieu = context.PhieuNhap
            .Where(x => x.NgayTao.Value.Date >= FromDay
            && x.NgayTao.Value.Date <= ToDay
            && x.Active == true)
            .Include(x => x.IdnccNavigation)
            .Include(x => x.IdnvNavigation)
            .Include(x => x.ChiTietPhieuNhap)
            .OrderByDescending(x => x.Id)
            .ToList();
            if (nhaCC == 0 && hhLS == 0)
            {

                return View("BaocaophieunhapPDF", listPhieu.Where(x => (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS) ?? false))
                && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper()))).ToList());
            }
            else
            {
                return View("BaocaophieunhapPDF", listPhieu.Where(x => (hhLS == 0 ? true : (x.ChiTietPhieuNhap.Where(y => y.Idhh == hhLS).Count() > 0 ? true : false))
                && (nhaCC == 0 ? true : x.Idncc == nhaCC)
                && (soPhieuLS == null ? true : x.SoPn.Contains(soPhieuLS.ToUpper()))
                && (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS.ToUpper()) ?? false))).ToList());
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
        string getSoPhieu()
        {
            webContext context = new webContext();
            QuyDinhMa qd = context.QuyDinhMa.Find(1);
            //ID chi nhánh
            string cn = "1";

            DateTime d = DateTime.Now;
            string ngayThangNam = d.ToString("yyMMdd");
            string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
            var list = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'NhapKho'").FirstOrDefault();
            int stt;
            if (list == null)
            {
                SoThuTu sttt = new SoThuTu();
                sttt.Ngay = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                sttt.Stt = 0;
                sttt.Loai = "NhapKho";
                context.SoThuTu.Add(sttt);
                context.SaveChanges();
                stt = 1;
            }
            else
            {
                stt = (Int32)list.Stt + 1;
            }
            SoPhieu += stt;
            while (true)
            {
                if (qd.DoDai == SoPhieu.Length) break;
                SoPhieu = SoPhieu.Insert(SoPhieu.Length - stt.ToString().Length, "0");
            }
            return SoPhieu;
        }
    }
}
