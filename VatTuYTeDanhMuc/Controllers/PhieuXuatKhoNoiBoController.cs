using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using VatTuYTeDanhMuc.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using Microsoft.Extensions.Options;
using SelectPdf;

namespace VatTuYTeDanhMuc.Controllers
{
	public class PhieuXuatKhoNoiBoController : Controller
	{
        private ICompositeViewEngine _viewEngine;
        public PhieuXuatKhoNoiBoController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        [Route("/PhieuXuatKhoNoiBo")]

        public IActionResult Index()
        {
            return View("PhieuXuatKhoNoiBo");
        }
        [HttpPost("/them-phieu-xuat-NB")]
        public IActionResult themPhieuXuatNB(PhieuXuatNoiBo phieuXuat, string NgayHd, string NgayTao)
        {
            webContext context = new webContext();
            var cachXuat = context.CachXuat.Find(1);
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            List<SoLuongHhcon> soLuongHhcon;
            if (cachXuat.TheoTgnhap == true)
            {
                soLuongHhcon = context.SoLuongHhcon
                .Include(x => x.IdctpnNavigation)
                .OrderBy(x => x.NgayNhap).ToList();
            }
            else
            {
                soLuongHhcon = context.SoLuongHhcon
               .Include(x => x.IdctpnNavigation)
               .OrderBy(x => x.IdctpnNavigation.Hsd).ToList();
            }

            string Host = GetLocalIPAddress();
            List<ChiTietPhieuXuatTamNoiBo> ListCTPNT = context.ChiTietPhieuXuatTamNoiBo.Where(x => x.Host == Host).OrderByDescending(x => x.Id).ToList();
            var tran = context.Database.BeginTransaction();
            try
            {
                phieuXuat.NgayHd = DateTime.ParseExact(NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                phieuXuat.NgayTao = DateTime.ParseExact(NgayTao, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                phieuXuat.Active = true;
                phieuXuat.Idcn = int.Parse(User.Claims.ElementAt(4).Value);
                phieuXuat.Idnv = idUser;
                phieuXuat.SoPxnb = getSoPhieu();
                context.PhieuXuatNoiBo.Add(phieuXuat);
                context.SaveChanges();

                foreach (ChiTietPhieuXuatTamNoiBo t in ListCTPNT)
                {
                    double slq = 0;
                    foreach (SoLuongHhcon slhhc in soLuongHhcon.Where(x => x.IdctpnNavigation.Idhh == t.Idhh))
                    {
                        ChiTietPhieuXuatNoiBo ct = new ChiTietPhieuXuatNoiBo();
                        ct.Idhh = t.Idhh;
                        ct.Idpxnb = phieuXuat.Id;
                        ct.DonGia = t.DonGia;
                        ct.Iddvt = t.Iddvt;
                        ct.Idctpn = slhhc.Idctpn;                      
                        ct.Nvtao = phieuXuat.Idnv;
                        ct.NgayTao = phieuXuat.NgayTao;
                        ct.Active = true;
                        HangHoa hangHoa = context.HangHoa.Find(t.Idhh);
                        double? slquydoi = 0;
                        //nếu mà đơn vị tính là đơn vị tính chính
                        if (t.Iddvt == hangHoa.Iddvtchinh)
                        {
                            slquydoi = t.Slg;
                        }
                        else
                        {
                            slquydoi = t.Slg * context.HhDvt.Where(x => x.Idhh == t.Idhh && x.Iddvt == t.Iddvt).FirstOrDefault().SlquyDoi;
                        }
                        if (slq == 0)
                        {
                            slq = Math.Round((double)slquydoi, 2);
                        }
                        //nếu mà trong kho còn nhiều hơn số xuất
                        if (slhhc.Slcon > slq)
                        {
                            ct.Slg = t.Slg;
                            slhhc.Slcon -= slq;
                            context.SoLuongHhcon.Update(slhhc);
                            context.ChiTietPhieuXuatNoiBo.Add(ct);
                            context.SaveChanges();
                            break;
                        }
                        //nếu mà trong kho ngang với số cần xuất
                        if (slhhc.Slcon == slq)
                        {
                            ct.Slg = t.Slg;
                            context.SoLuongHhcon.Remove(slhhc);
                            context.ChiTietPhieuXuatNoiBo.Add(ct);
                            context.SaveChanges();
                            break;
                        }
                        //nếu trong kho còn ít hơn số cần xuất
                        if (slhhc.Slcon < slq)
                        {
                            ct.Slg = slhhc.Slcon;
                            slq = (double)(slq - slhhc.Slcon);
                            t.Slg = slq;
                            context.SoLuongHhcon.Remove(slhhc);
                            context.ChiTietPhieuXuatNoiBo.Add(ct);
                            context.SaveChanges();
                        }
                    }
                    context.ChiTietPhieuXuatTamNoiBo.Remove(t);
                    context.SaveChanges();
                }
                var stt = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'XuatKho'").FirstOrDefault();
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

            return RedirectToAction("Index");
        }
        [Route("/download/phieuxuatkhoNB/{id:int}")]
        public IActionResult downloadPhieuXuatNB(int id)
        {
            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1280;
            fullView.Options.PdfPageSize = PdfPageSize.A4;
            fullView.Options.MarginTop = 20;
            fullView.Options.MarginBottom = 20;
            fullView.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var pdf = fullView.ConvertUrl(currentUrl + "/phieuXuatKhoNBPDF/" + id);

            var pdfBytes = pdf.Save();
            return File(pdfBytes, "application/pdf", "PhieuXuatNoiBo.pdf");
        }
        [Route("/phieuXuatKhoNBPDF/{id:int}")]
        public IActionResult viewPDFNB(int id)
        {
            webContext context = new webContext();
            var phieu = context.PhieuXuatNoiBo
                .Include(x => x.IdkNavigation)
                .Include(x => x.ChiTietPhieuXuatNoiBo)
                .Where(x => x.Id == id).FirstOrDefault();
            return View("PhieuxuatkhoNBPDF", phieu);
        }
        [HttpPost("/loadListDVT_NB")]
        public IActionResult loadListDVT_NB(int idHH)
        {
            webContext context = new webContext();
            double? GiaBan;

            HangHoa hh = context.HangHoa.Include(x => x.IddvtchinhNavigation).FirstOrDefault(x => x.Id == idHH);

            var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

            //Lấy các đơn vị tính của Hàng hoá
            string options = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
            foreach (HhDvt d in dvt)
            {
                options += "<option value = '" + d.Iddvt + "'>" + d.IddvtNavigation.TenDvt + "</option>";
            }

            //lấy giá theo khách hàng


            // lấy giá nhập của hàng hoá
            SoLuongHhcon GiaHangTon;
            var cachXuat = context.CachXuat.Find(1);
            if (cachXuat.TheoTgnhap == true)
            {
                GiaHangTon = context.SoLuongHhcon
                    .Include(x => x.IdctpnNavigation)
                    .Where(x => x.IdctpnNavigation.Idhh == idHH)
                    .OrderBy(x => x.NgayNhap).
                    FirstOrDefault();
            }
            else
            {
                GiaHangTon = context.SoLuongHhcon
                    .Include(x => x.IdctpnNavigation)
                    .Where(x => x.IdctpnNavigation.Idhh == idHH)
                    .OrderBy(x => x.IdctpnNavigation.Hsd)
                    .FirstOrDefault();
            }
            if (GiaHangTon == null)
            {
                return Json(new
                {
                    options = options,
                    giaBan = 0,
                    slCon = getSLCon(idHH, (int)hh.Iddvtchinh),
                    hinhAnh = hh.Avatar
                });
            }



            return Json(new
            {
                options = options,
                giaBan  = getGiaBan(GiaHangTon.GiaNhap),
                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                hinhAnh = hh.Avatar
            });

        }
        [HttpPost("/add-Chi-Tiet-Phieu-Xuat-Tam-NB")]
        public IActionResult addChiTietPhieuXuatTamNB(int idHH, float SL, float DonGia,
            int idDVT)
        {
            int idUser = int.Parse(User.Claims.ElementAt(2).Type);
            webContext context = new webContext();
            ChiTietPhieuXuatTamNoiBo ct = new ChiTietPhieuXuatTamNoiBo();
            ct.Idhh = idHH;
            ct.Iddvt = idDVT;
            ct.Slg = Math.Round(SL, 2);
            ct.DonGia = Math.Round(DonGia, 2);
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = idUser;
            ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            context.ChiTietPhieuXuatTamNoiBo.Add(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTamNoiBo> ListCTPNT = context.ChiTietPhieuXuatTamNoiBo.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienThanhToan = TienHang ;
            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuatNB");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,               
                tienThanhToan = TienThanhToan
            });
        }
        [HttpPost("/edit-Chi-Tiet-Phieu-Xuat-Tam-NB")]
        public IActionResult addChiTietPhieuXuatTamNB(int idHH, float SL, float DonGia,
             int idDVT, int id)
        {
            webContext context = new webContext();
            ChiTietPhieuXuatTamNoiBo ct = context.ChiTietPhieuXuatTamNoiBo.Find(id);
            ct.Idhh = idHH;
            ct.Iddvt = idDVT;
            ct.Slg = Math.Round(SL, 2);
            ct.DonGia = Math.Round(DonGia, 2);
            context.ChiTietPhieuXuatTamNoiBo.Update(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTamNoiBo> ListCTPNT = context.ChiTietPhieuXuatTamNoiBo.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienThanhToan = TienHang;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuatNB");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,
           
                tienThanhToan = TienThanhToan
            });
        }
        [HttpPost("/editChitietPhieuXuatTamNB")]
        public IActionResult editChitietPhieuXuatTamNB(int id)
        {
            webContext context = new webContext();
            if (id > 0)
            {
                return PartialView("GroupChiTietPhieuXuatTamNB", context.ChiTietPhieuXuatTamNoiBo.Find(id));
            }
            else
            {
                return PartialView("GroupChiTietPhieuXuatTamNB");
            }
        }
        [HttpPost("/delete-Chi-Tiet-Phieu-Xuat-NB")]
        public IActionResult deletePhieuNhapTamNB(int id)
        {
            webContext context = new webContext();
            ChiTietPhieuXuatTamNoiBo ch = context.ChiTietPhieuXuatTamNoiBo.Find(id);
            context.ChiTietPhieuXuatTamNoiBo.Remove(ch);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTamNoiBo> ListCTPNT = context.ChiTietPhieuXuatTamNoiBo.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienThanhToan = TienHang;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuatNB");

            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            return Json(new
            {
                table = viewContent,
                tienHang = TienHang,
               
                tienThanhToan = TienThanhToan
            });
        }
        [HttpPost("/loadTableLichSuXuatNB")]
        public IActionResult loadTableLichSuXuat(string fromDay, string toDay, string soPhieuLS, string soHDLS, int kLS, int hhLS)
        {
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            webContext context = new webContext();
            List<PhieuXuatNoiBo> listPhieu = context.PhieuXuatNoiBo
            .Where(x => x.NgayTao.Value.Date >= FromDay
                && x.NgayTao.Value.Date <= ToDay
                && x.Active == true)
            .Include(x => x.IdkNavigation)
            .Include(x => x.IdnvNavigation)
            .Include(x => x.ChiTietPhieuXuatNoiBo)
            .OrderByDescending(x => x.Id)
            .ToList();
            if (kLS == 0 && hhLS == 0)
            {
                ViewBag.ListPhieuXuat = listPhieu.Where(x => (soHDLS == null ? true : (x.SoHd?.ToLower().Contains(soHDLS.ToLower()) ?? false))
                && (soPhieuLS == null ? true : x.SoPxnb.Contains(soPhieuLS.ToUpper())));
            }
            else
            {
                ViewBag.ListPhieuXuat = listPhieu.Where(x => (hhLS == 0 ? true : (x.ChiTietPhieuXuatNoiBo.Where(y => y.Idhh == hhLS).Count() > 0 ? true : false))
                && (kLS == 0 ? true : x.Idk == kLS)
                && (soPhieuLS == null ? true : x.SoPxnb.Contains(soPhieuLS.ToUpper()))
                && (soHDLS == null ? true : (x.SoHd?.Contains(soHDLS.ToUpper()) ?? false)));
            }

            return PartialView("TableLichSuXuatNB");
        }
        [HttpPost("/ViewThongTinPhieuXuatNB")]
        public IActionResult ViewThongTinPhieuXuatNB(int idPX)
        {
            webContext context = new webContext();
            var phieu = context.PhieuXuatNoiBo.Include(x => x.ChiTietPhieuXuatNoiBo)
                .Include(x => x.IdkNavigation)
                .Include(x => x.IdnvNavigation)
                .FirstOrDefault(x => x.Id == idPX);
            return PartialView(phieu);
        }
        [HttpPost("/checkSLC_NB")]
        public double? checkSLC_NB(int idHH, int idDVT)
        {
            return getSLCon(idHH, idDVT);
        }
        [HttpPost("/loadTienDVTNB")]
        public IActionResult loadTienDVTNB(int idHH, int idDVT)
        {
            webContext context = new webContext();
            double? GiaBan;
            HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);
            if (hh.Iddvtchinh == idDVT)
            {
                var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

               

                // lấy giá nhập của hàng hoá
                SoLuongHhcon GiaHangTon;
                var cachXuat = context.CachXuat.Find(1);
                if (cachXuat.TheoTgnhap == true)
                {
                    GiaHangTon = context.SoLuongHhcon
                        .Include(x => x.IdctpnNavigation)
                        .Where(x => x.IdctpnNavigation.Idhh == idHH)
                        .OrderBy(x => x.NgayNhap).
                        FirstOrDefault();
                }
                else
                {
                    GiaHangTon = context.SoLuongHhcon
                        .Include(x => x.IdctpnNavigation)
                        .Where(x => x.IdctpnNavigation.Idhh == idHH)
                        .OrderBy(x => x.IdctpnNavigation.Hsd)
                        .FirstOrDefault();
                }
                if (GiaHangTon == null)
                {
                    return Json(new
                    {
                        giaBan = 0,
                        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                    });
                }
                return Json(new
                {

                    giaBan = getGiaBan(GiaHangTon.GiaNhap),
                    slCon = getSLCon(idHH, idDVT),

                });

            }
            else
            {
                var dvt = context.HhDvt
                    .Where(x => x.Idhh == idHH).ToList();

              

                // lấy giá nhập của hàng hoá
                SoLuongHhcon GiaHangTon;
                var cachXuat = context.CachXuat.Find(1);
                if (cachXuat.TheoTgnhap == true)
                {
                    GiaHangTon = context.SoLuongHhcon
                        .Include(x => x.IdctpnNavigation)
                        .Where(x => x.IdctpnNavigation.Idhh == idHH)
                        .OrderBy(x => x.NgayNhap).
                        FirstOrDefault();
                }
                else
                {
                    GiaHangTon = context.SoLuongHhcon
                        .Include(x => x.IdctpnNavigation)
                        .Where(x => x.IdctpnNavigation.Idhh == idHH)
                        .OrderBy(x => x.IdctpnNavigation.Hsd)
                        .FirstOrDefault();
                }
                if (GiaHangTon == null)
                {
                    return Json(new
                    {
                        giaBan = 0,
                        slCon = getSLCon(idHH, idDVT)
                    });
                }

                return Json(new
                {
                   
                    giaBan = getGiaBan(GiaHangTon.GiaNhap),
                    slCon = getSLCon(idHH, idDVT),
                  
                });

            }
        }
        double? getSLCon(int idHH, int idDVT)
        {
            webContext context = new webContext();
            HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);

            double? a = context.SoLuongHhcon.Include(x => x.IdctpnNavigation)
                .Where(x => x.IdctpnNavigation.Idhh == idHH)
                .Sum(x => x.Slcon);

            if (hh.Iddvtchinh == idDVT)
            {
                return a;
            }
            else
            {
                var sl = (double)context.HhDvt.Where(x => x.Idhh == idHH && x.Iddvt == idDVT).FirstOrDefault().SlquyDoi;
                return Math.Round(a.Value / sl, 2);
            }
        }
        double? getGiaBan(double? GiaNhap)
        {
            
            return GiaNhap;
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
            QuyDinhMa qd = context.QuyDinhMa.Find(2);
            //ID chi nhánh
            string cn = "01";

            DateTime d = DateTime.Now;
            string ngayThangNam = d.ToString("yyMMdd");
            string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam + "NB";
            var list = context.SoThuTu.FromSqlRaw("select*from SoThuTu where '" + DateTime.Now.ToString("yyyy-MM-dd") + "' = Convert(date,ngay) and Loai = 'XuatKho'").FirstOrDefault();
            int stt;
            if (list == null)
            {
                SoThuTu sttt = new SoThuTu();
                sttt.Ngay = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                sttt.Stt = 0;
                sttt.Loai = "XuatKho";
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
