using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
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
    public class PhieuXuatKhoController : Controller
    {
        private ICompositeViewEngine _viewEngine;
        public PhieuXuatKhoController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
        [Route("/PhieuXuatKho")]
        public IActionResult Index()
        {
            return View("PhieuXuatKho");
        }

        [HttpPost("/them-phieu-xuat")]
        public IActionResult themPhieuXuat(PhieuXuat phieuXuat, string NgayHd, string NgayTao)
        {
            webContext context = new webContext();
            var cachXuat = context.CachXuat.Find(1);
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
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
            List<ChiTietPhieuXuatTam> ListCTPNT = context.ChiTietPhieuXuatTam.Where(x => x.Host == Host).OrderByDescending(x => x.Id).ToList();

            var tran = context.Database.BeginTransaction();
            try
            {
                phieuXuat.NgayHd = DateTime.ParseExact(NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                phieuXuat.NgayTao = DateTime.ParseExact(NgayTao, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                phieuXuat.Active = true;
                phieuXuat.Idcn = 1;
                phieuXuat.Idnv = idUser;
                phieuXuat.SoPx = getSoPhieu();
                context.PhieuXuat.Add(phieuXuat);
                context.SaveChanges();

                foreach (ChiTietPhieuXuatTam t in ListCTPNT)
                {
                    double slq = 0;
                    foreach (SoLuongHhcon slhhc in soLuongHhcon.Where(x => x.IdctpnNavigation.Idhh == t.Idhh))
                    {
                        ChiTietPhieuXuat ct = new ChiTietPhieuXuat();
                        ct.Idhh = t.Idhh;
                        ct.Idpx = phieuXuat.Id;
                        ct.DonGia = t.DonGia;
                        ct.Iddvt = t.Iddvt;
                        ct.Idctpn = slhhc.Idctpn;
                        ct.Cktm = t.Cktm;
                        ct.Thue = t.Thue;
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
                            context.ChiTietPhieuXuat.Add(ct);
                            context.SaveChanges();
                            break;
                        }
                        //nếu mà trong kho ngang với số cần xuất
                        if (slhhc.Slcon == slq)
                        {
                            ct.Slg = t.Slg;
                            context.SoLuongHhcon.Remove(slhhc);
                            context.ChiTietPhieuXuat.Add(ct);
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
                            context.ChiTietPhieuXuat.Add(ct);
                            context.SaveChanges();
                        }
                    }
                    context.ChiTietPhieuXuatTam.Remove(t);
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

        [HttpPost("/loadListDVT")]
        public IActionResult loadListDVT(int idHH, int idKH)
        {
            webContext context = new webContext();
            double? GiaBan;
            KhachHang kh = context.KhachHang.Find(idKH);

            HangHoa hh = context.HangHoa.Include(x => x.IddvtchinhNavigation).FirstOrDefault(x => x.Id == idHH);

            var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

            //Lấy các đơn vị tính của Hàng hoá
            string options = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
            foreach (HhDvt d in dvt)
            {
                options += "<option value = '" + d.Iddvt + "'>" + d.IddvtNavigation.TenDvt + "</option>";
            }

            //lấy giá theo khách hàng
            var GiaTheoKH = context.GiaTheoKhachHang
                .Where(x => x.Idkh == idKH && x.Idhh == idHH && x.Iddvt == hh.Iddvtchinh)
                .FirstOrDefault();

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
                    slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                    hinhAnh = hh.Avatar
                });
            }


            //nếu giá theo khách hàng không tồn tại thì xét tiếp
            if (GiaTheoKH == null || (GiaTheoKH.TiLeLe == 0 && GiaTheoKH.TiLeSi == 0 && GiaTheoKH.GiaBanSi == 0 && GiaTheoKH.GiaBanLe == 0))
            {
                //Xét tiếp tới giá theo nhóm hàng hoá
                var listGTNHH = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == hh.Idnhh).ToList();

                //Nếu giá theo nhóm hàng hoá có tồn tại
                if (listGTNHH.Count > 0)
                {
                    //xét nhiều khoản min max khác nhau
                    foreach (var h in listGTNHH)
                    {
                        if (GiaHangTon.IdctpnNavigation.DonGia >= h.Min && GiaHangTon.IdctpnNavigation.DonGia <= h.Max)
                        {
                            if (kh.LoaiKh == true)
                            {
                                GiaBan = getTiLe(h.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                            }
                            else // nếu loại khách hàng là sĩ
                            {
                                GiaBan = getTiLe(h.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                            }
                            return Json(new
                            {
                                options = options,
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                                hinhAnh = hh.Avatar
                            });
                        }
                    }
                    // nếu sau khi xét trong list Giá theo nhóm nhà hoá và vẫn kh return thì xét xuống giá theo hàng hoá
                    if (kh.LoaiKh == true)
                    {
                        if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                        {
                            GiaBan = hh.GiaBanLe; // lấy giá bán
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                        }
                        return Json(new
                        {
                            options = options,
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                            hinhAnh = hh.Avatar
                        });
                    }
                    else // nếu loại khách hàng là sĩ
                    {
                        if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                        {
                            GiaBan = hh.GiaBanSi;
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                        }
                        return Json(new
                        {
                            options = options,
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                            hinhAnh = hh.Avatar
                        });
                    }
                }
                else  // xét tiếp giá theo hàng hoá
                {
                    //Nếu loại khách hàng là lẻ
                    if (kh.LoaiKh == true)
                    {
                        if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                        {
                            GiaBan = hh.GiaBanLe; // lấy giá bán
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                        }
                        return Json(new
                        {
                            options = options,
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                            hinhAnh = hh.Avatar
                        });
                    }
                    else // nếu loại khách hàng là sĩ
                    {
                        if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                        {
                            GiaBan = hh.GiaBanSi;
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                        }
                        return Json(new
                        {
                            options = options,
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                            hinhAnh = hh.Avatar
                        });
                    }
                }
            }
            else //Nếu giá theo khách hàng tồn tại
            {
                //Nếu loại khách hàng là lẻ
                if (kh.LoaiKh == true)
                {
                    if (GiaTheoKH.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                    {
                        GiaBan = GiaTheoKH.GiaBanLe; // lấy giá bán
                    }
                    else // xét tỉ lẹ lẻ
                    {
                        GiaBan = getTiLe(GiaTheoKH.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                    }
                    return Json(new
                    {
                        options = options,
                        giaBan = GiaBan,
                        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                        hinhAnh = hh.Avatar
                    });
                }
                else // nếu loại khách hàng là sĩ
                {
                    if (GiaTheoKH.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                    {
                        GiaBan = GiaTheoKH.GiaBanSi;
                    }
                    else // xét tỉ lẹ lẻ
                    {
                        GiaBan = getTiLe(GiaTheoKH.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                    }
                    return Json(new
                    {
                        options = options,
                        giaBan = GiaBan,
                        slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh),
                        hinhAnh = hh.Avatar
                    });
                }
            }
        }

        [HttpPost("/loadTienDVT")]
        public IActionResult loadTienDVT(int idHH, int idKH, int idDVT)
        {
            webContext context = new webContext();
            double? GiaBan;
            KhachHang kh = context.KhachHang.Find(idKH);
            HangHoa hh = context.HangHoa.FirstOrDefault(x => x.Id == idHH);
            if (hh.Iddvtchinh == idDVT)
            {
                var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

                //lấy giá theo khách hàng
                var GiaTheoKH = context.GiaTheoKhachHang
                    .Where(x => x.Idkh == idKH && x.Idhh == idHH && x.Iddvt == hh.Iddvtchinh)
                    .FirstOrDefault();

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

                //nếu giá theo khách hàng không tồn tại thì xét tiếp
                if (GiaTheoKH == null || (GiaTheoKH.TiLeLe == 0 && GiaTheoKH.TiLeSi == 0 && GiaTheoKH.GiaBanSi == 0 && GiaTheoKH.GiaBanLe == 0))
                {
                    //Xét tiếp tới giá theo nhóm hàng hoá
                    var listGTNHH = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == hh.Idnhh).ToList();

                    //Nếu giá theo nhóm hàng hoá có tồn tại
                    if (listGTNHH.Count > 0)
                    {
                        //xét nhiều khoản min max khác nhau
                        foreach (var h in listGTNHH)
                        {
                            if (GiaHangTon.IdctpnNavigation.DonGia >= h.Min && GiaHangTon.IdctpnNavigation.DonGia <= h.Max)
                            {
                                if (kh.LoaiKh == true)
                                {
                                    GiaBan = getTiLe(h.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                                }
                                else // nếu loại khách hàng là sĩ
                                {
                                    GiaBan = getTiLe(h.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                                }
                                return Json(new
                                {
                                    giaBan = GiaBan,
                                    slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                                });
                            }
                        }
                        // nếu sau khi xét trong list Giá theo nhóm nhà hoá và vẫn kh return thì xét xuống giá theo hàng hoá
                        if (kh.LoaiKh == true)
                        {
                            if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                            {
                                GiaBan = hh.GiaBanLe; // lấy giá bán
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                            });
                        }
                        else // nếu loại khách hàng là sĩ
                        {
                            if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                            {
                                GiaBan = hh.GiaBanSi;
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                            });
                        }
                    }
                    else  // xét tiếp giá theo hàng hoá
                    {
                        //Nếu loại khách hàng là lẻ
                        if (kh.LoaiKh == true)
                        {
                            if (hh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                            {
                                GiaBan = hh.GiaBanLe; // lấy giá bán
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(hh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                            });
                        }
                        else // nếu loại khách hàng là sĩ
                        {
                            if (hh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                            {
                                GiaBan = hh.GiaBanSi;
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(hh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                            });
                        }
                    }
                }
                else //Nếu giá theo khách hàng tồn tại
                {
                    //Nếu loại khách hàng là lẻ
                    if (kh.LoaiKh == true)
                    {
                        if (GiaTheoKH.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                        {
                            GiaBan = GiaTheoKH.GiaBanLe; // lấy giá bán
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(GiaTheoKH.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                        }
                        return Json(new
                        {
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                        });
                    }
                    else // nếu loại khách hàng là sĩ
                    {
                        if (GiaTheoKH.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                        {
                            GiaBan = GiaTheoKH.GiaBanSi;
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(GiaTheoKH.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                        }
                        return Json(new
                        {
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, (Int32)hh.Iddvtchinh)
                        });
                    }
                }
            }
            else
            {
                var dvt = context.HhDvt
                    .Where(x => x.Idhh == idHH).ToList();

                //lấy giá theo khách hàng
                GiaTheoKhachHang GiaTheoKH = context.GiaTheoKhachHang
                        .Where(x => x.Idkh == idKH && x.Idhh == idHH && x.Iddvt == idDVT)
                        .FirstOrDefault();

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

                //nếu giá theo khách hàng không tồn tại thì xét tiếp
                if (GiaTheoKH == null || (GiaTheoKH.TiLeLe == 0 && GiaTheoKH.TiLeSi == 0 && GiaTheoKH.GiaBanSi == 0 && GiaTheoKH.GiaBanLe == 0))
                {
                    //Xét tiếp tới giá theo nhóm hàng hoá
                    var listGTNHH = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == hh.Idnhh).ToList();
                    HhDvt dvthh = context.HhDvt.Where(x => x.Idhh == idHH && x.Iddvt == idDVT).FirstOrDefault();
                    //Nếu giá theo nhóm hàng hoá có tồn tại
                    if (listGTNHH.Count > 0)
                    {
                        //xét nhiều khoản min max khác nhau
                        foreach (var h in listGTNHH)
                        {
                            if (GiaHangTon.IdctpnNavigation.DonGia >= h.Min && GiaHangTon.IdctpnNavigation.DonGia <= h.Max)
                            {
                                if (kh.LoaiKh == true)
                                {
                                    GiaBan = getTiLe(h.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                                }
                                else // nếu loại khách hàng là sĩ
                                {
                                    GiaBan = getTiLe(h.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                                }
                                return Json(new
                                {
                                    giaBan = GiaBan,
                                    slCon = getSLCon(idHH, idDVT)
                                });
                            }
                        }
                        // nếu sau khi xét trong list Giá theo nhóm nhà hoá và vẫn kh return thì xét xuống giá theo hàng hoá
                        if (kh.LoaiKh == true)
                        {
                            if (dvthh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                            {
                                GiaBan = dvthh.GiaBanLe; // lấy giá bán
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(dvthh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, idDVT)
                            });
                        }
                        else // nếu loại khách hàng là sĩ
                        {
                            if (dvthh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                            {
                                GiaBan = dvthh.GiaBanSi;
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(dvthh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, idDVT)
                            });
                        }
                    }
                    else  // xét tiếp giá theo hàng hoá
                    {
                        //Nếu loại khách hàng là lẻ
                        if (kh.LoaiKh == true)
                        {
                            if (dvthh.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                            {
                                GiaBan = dvthh.GiaBanLe; // lấy giá bán
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(dvthh.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, idDVT)
                            });
                        }
                        else // nếu loại khách hàng là sĩ
                        {
                            if (dvthh.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                            {
                                GiaBan = dvthh.GiaBanSi;
                            }
                            else // xét tỉ lẹ lẻ
                            {
                                GiaBan = getTiLe(dvthh.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                            }
                            return Json(new
                            {
                                giaBan = GiaBan,
                                slCon = getSLCon(idHH, idDVT)
                            });
                        }
                    }
                }
                else //Nếu giá theo khách hàng tồn tại
                {
                    //Nếu loại khách hàng là lẻ
                    if (kh.LoaiKh == true)
                    {
                        if (GiaTheoKH.GiaBanLe != 0) //Nếu giá bán lẻ tồn tại
                        {
                            GiaBan = GiaTheoKH.GiaBanLe; // lấy giá bán
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(GiaTheoKH.TiLeLe) * GiaHangTon.IdctpnNavigation.DonGia; // giá bán nhân tỉ lệ
                        }
                        return Json(new
                        {
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, idDVT)
                        });
                    }
                    else // nếu loại khách hàng là sĩ
                    {
                        if (GiaTheoKH.GiaBanSi != 0) //Nếu giá bán sĩ tồn tại
                        {
                            GiaBan = GiaTheoKH.GiaBanSi;
                        }
                        else // xét tỉ lẹ lẻ
                        {
                            GiaBan = getTiLe(GiaTheoKH.TiLeSi) * GiaHangTon.IdctpnNavigation.DonGia;
                        }
                        return Json(new
                        {
                            giaBan = GiaBan,
                            slCon = getSLCon(idHH, idDVT)
                        });
                    }
                }
            }
        }

        [HttpPost("/checkSLC")]
        public double? checkSLC(int idHH, int idDVT)
        {
            return getSLCon(idHH, idDVT);
        }

        [HttpPost("/add-Chi-Tiet-Phieu-Xuat-Tam")]
        public IActionResult addChiTietPhieuXuatTam(int idHH, float ThueXuat, float SL, float DonGia,
            float ChietKhau, int idDVT)
        {
            int idUser = int.Parse(User.Claims.ElementAt(3).Type);
            webContext context = new webContext();
            ChiTietPhieuXuatTam ct = new ChiTietPhieuXuatTam();
            ct.Idhh = idHH;
            ct.Iddvt = idDVT;
            ct.Thue = Math.Round(ThueXuat, 2);
            ct.Slg = Math.Round(SL, 2);
            ct.DonGia = Math.Round(DonGia, 2);
            ct.Cktm = Math.Round(ChietKhau, 2);
            ct.Host = GetLocalIPAddress();
            ct.Nvtao = idUser;
            ct.NgayTao = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            context.ChiTietPhieuXuatTam.Add(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTam> ListCTPNT = context.ChiTietPhieuXuatTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuat");

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

        [HttpPost("/edit-Chi-Tiet-Phieu-Xuat-Tam")]
        public IActionResult addChiTietPhieuXuatTam(int idHH, float ThueXuat, float SL, float DonGia,
            float ChietKhau, int idDVT, int id)
        {
            webContext context = new webContext();
            ChiTietPhieuXuatTam ct = context.ChiTietPhieuXuatTam.Find(id);
            ct.Idhh = idHH;
            ct.Iddvt = idDVT;
            ct.Thue = Math.Round(ThueXuat, 2);
            ct.Slg = Math.Round(SL, 2);
            ct.DonGia = Math.Round(DonGia, 2);
            ct.Cktm = Math.Round(ChietKhau, 2);
            context.ChiTietPhieuXuatTam.Update(ct);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTam> ListCTPNT = context.ChiTietPhieuXuatTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuat");

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

        [HttpPost("/editChitietPhieuXuatTam")]
        public IActionResult editChitietPhieuXuatTam(int id)
        {
            webContext context = new webContext();
            if (id > 0)
            {
                return PartialView("GroupChitietPhieuXuatTam", context.ChiTietPhieuXuatTam.Find(id));
            }
            else
            {
                return PartialView("GroupChitietPhieuXuatTam");
            }
        }

        [HttpPost("/delete-Chi-Tiet-Phieu-Xuat")]
        public IActionResult deletePhieuNhapTam(int id)
        {
            webContext context = new webContext();
            ChiTietPhieuXuatTam ch = context.ChiTietPhieuXuatTam.Find(id);
            context.ChiTietPhieuXuatTam.Remove(ch);
            context.SaveChanges();

            string ht = GetLocalIPAddress();
            List<ChiTietPhieuXuatTam> ListCTPNT = context.ChiTietPhieuXuatTam.Where(x => x.Host == ht).OrderByDescending(x => x.Id).ToList();
            var TienHang = ListCTPNT.Sum(x => x.Slg * x.DonGia);
            var TienCK = ListCTPNT.Sum(x => (x.Slg * x.DonGia * x.Cktm) / 100);
            var TienThue = ListCTPNT.Sum(x => (((x.Slg * x.DonGia) - ((x.Slg * x.DonGia * x.Cktm) / 100)) * x.Thue) / 100);
            var TienThanhToan = TienHang - TienCK + TienThue;

            PartialViewResult partialViewResult = PartialView("TableChiTietPhieuXuat");

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

        [HttpPost("/ViewThongTinPhieuXuat")]
        public IActionResult ViewThongTinPhieuXuat(int idPX)
        {
            webContext context = new webContext();
            var phieu = context.PhieuXuat.Include(x => x.ChiTietPhieuXuat)
                .Include(x => x.IdkhNavigation)
                .Include(x => x.IdnvNavigation)
                .FirstOrDefault(x => x.Id == idPX);
            return PartialView(phieu);
        }

        [HttpPost("/loadTableLichSuXuat")]
        public IActionResult loadTableLichSuXuat(string fromDay, string toDay)
        {
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            webContext context = new webContext();
            ViewBag.ListPhieuXuat = context.PhieuXuat
                                                    .FromSqlRaw("select*from PhieuXuat where CONVERT(date,NgayTao) >= '" + FromDay.ToString("yyyy-MM-dd") + "' and CONVERT(date,NgayTao) <= '" + ToDay.ToString("yyyy-MM-dd") + "' and Active = 1")
                                                    .Include(x => x.IdkhNavigation)
                                                    .Include(x => x.IdnvNavigation)
                                                    .OrderByDescending(x => x.Id)
                                                    .ToList();
            return PartialView("TableLichSuXuat");
        }

        double? getTiLe(double? tl)
        {
            double a = (tl.Value / 100) + 1;
            return Math.Round(a, 2);
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
            QuyDinhMa qd = context.QuyDinhMa.Find(2);
            //ID chi nhánh
            string cn = "01";

            DateTime d = DateTime.Now;
            string ngayThangNam = d.ToString("yyMMdd");
            string SoPhieu = cn + "_" + qd.TiepDauNgu + ngayThangNam;
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

        List<SoLuongHhcon> soLuongHhcon()
        {
            webContext context = new webContext();
            var cachXuat = context.CachXuat.Find(1);
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
            return soLuongHhcon;
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
                return a * sl;
            }
        }
    }
}
