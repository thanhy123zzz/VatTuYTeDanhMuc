using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VatTuYTeDanhMuc.Models.Entities;

namespace VatTuYTeDanhMuc.Controllers
{
    public class PhieuXuatKhoController : Controller
    {
        [Route("/PhieuXuatKho")]
        public IActionResult Index()
        {
            return View("PhieuXuatKho");
        }

        [HttpPost("/loadListDVT")]
        public IActionResult loadListDVT(int idHH, int idKH)
        {
            webContext context = new webContext();
            double? GiaBan;
            KhachHang kh = context.KhachHang.Find(idKH);

            HangHoa hh = context.HangHoa.Include(x=>x.IddvtchinhNavigation).FirstOrDefault(x=>x.Id == idHH);

            var dvt = context.HhDvt.Include(x => x.IddvtNavigation).Where(x => x.Idhh == idHH).ToList();

            //Lấy các đơn vị tính của Hàng hoá
            string options = "<option selected value = '" + hh.Iddvtchinh + "'>" + hh.IddvtchinhNavigation.TenDvt + "</option>";
            foreach(HhDvt d in dvt)
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
                });
            }


            //nếu giá theo khách hàng không tồn tại thì xét tiếp
            if (GiaTheoKH == null || (GiaTheoKH.TiLeLe==0 && GiaTheoKH.TiLeSi ==0 && GiaTheoKH.GiaBanSi == 0 && GiaTheoKH.GiaBanLe == 0))
            {
                //Xét tiếp tới giá theo nhóm hàng hoá
                var listGTNHH = context.GiaTheoNhomHangHoa.Where(x => x.Idnhh == hh.Idnhh).ToList();

                //Nếu giá theo nhóm hàng hoá có tồn tại
                if (listGTNHH.Count > 0)
                {
                    //xét nhiều khoản min max khác nhau
                    foreach (var h in listGTNHH)
                    {
                        if(GiaHangTon.IdctpnNavigation.DonGia >= h.Min && GiaHangTon.IdctpnNavigation.DonGia <= h.Max)
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
                        });
                    }
                }
            }
            else //Nếu giá theo khách hàng tồn tại
            {
                //Nếu loại khách hàng là lẻ
                if (kh.LoaiKh == true) {
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
                    });
                }
            }
        }

        [HttpPost("/loadTienDVT")]
        public double? loadTienDVT(int idHH, int idKH,int idDVT)
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
                    return 0;
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
                                return GiaBan;
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
                            return GiaBan;
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
                            return GiaBan;
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
                            return GiaBan;
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
                            return GiaBan;
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
                        return GiaBan;
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
                        return GiaBan;
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
                    return 0;
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
                                return GiaBan;
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
                                return GiaBan;
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
                            return GiaBan;
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
                            return GiaBan;
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
                            return GiaBan;
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
                        return GiaBan;
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
                        return GiaBan;
                    }
                }
            }
        }


        [HttpPost("/checkSLC")]
        public double? checkSLC(int idHH, int idDVT)
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
        double? getTiLe(double? tl)
        {
            double a = (tl.Value / 100) + 1;
            return Math.Round(a, 2);
        }
    }
}
