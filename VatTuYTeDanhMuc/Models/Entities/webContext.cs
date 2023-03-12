using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace VatTuYTeDanhMuc.Models.Entities
{
    public partial class webContext : DbContext
    {
        public webContext()
        {
        }

        public webContext(DbContextOptions<webContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaoHanh> BaoHanh { get; set; }
        public virtual DbSet<CachXuat> CachXuat { get; set; }
        public virtual DbSet<ChiNhanh> ChiNhanh { get; set; }
        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
        public virtual DbSet<ChiTietPhieuNhapTam> ChiTietPhieuNhapTam { get; set; }
        public virtual DbSet<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
        public virtual DbSet<ChiTietPhieuXuatTam> ChiTietPhieuXuatTam { get; set; }
        public virtual DbSet<ChiTietThuNo> ChiTietThuNo { get; set; }
        public virtual DbSet<ChiTietTraNo> ChiTietTraNo { get; set; }
        public virtual DbSet<ChucNang> ChucNang { get; set; }
        public virtual DbSet<Dvt> Dvt { get; set; }
        public virtual DbSet<Dvvc> Dvvc { get; set; }
        public virtual DbSet<GiaTheoKhachHang> GiaTheoKhachHang { get; set; }
        public virtual DbSet<GiaTheoNhomHangHoa> GiaTheoNhomHangHoa { get; set; }
        public virtual DbSet<HangHoa> HangHoa { get; set; }
        public virtual DbSet<HangSanXuat> HangSanXuat { get; set; }
        public virtual DbSet<HhDvt> HhDvt { get; set; }
        public virtual DbSet<HinhAnhHangHoa> HinhAnhHangHoa { get; set; }
        public virtual DbSet<HinhAnhNhanVien> HinhAnhNhanVien { get; set; }
        public virtual DbSet<Httt> Httt { get; set; }
        public virtual DbSet<KhachHang> KhachHang { get; set; }
        public virtual DbSet<KhachHangNganHang> KhachHangNganHang { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<NccNganHang> NccNganHang { get; set; }
        public virtual DbSet<NganHang> NganHang { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCap { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<NhomHangHoa> NhomHangHoa { get; set; }
        public virtual DbSet<NhomNhanVien> NhomNhanVien { get; set; }
        public virtual DbSet<NuocSanXuat> NuocSanXuat { get; set; }
        public virtual DbSet<Phan> Phan { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyen { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhap { get; set; }
        public virtual DbSet<PhieuThuNo> PhieuThuNo { get; set; }
        public virtual DbSet<PhieuTraNo> PhieuTraNo { get; set; }
        public virtual DbSet<PhieuXuat> PhieuXuat { get; set; }
        public virtual DbSet<QuyDinhMa> QuyDinhMa { get; set; }
        public virtual DbSet<SoLuongHhcon> SoLuongHhcon { get; set; }
        public virtual DbSet<SoThuTu> SoThuTu { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<TrangThai> TrangThai { get; set; }
        public virtual DbSet<TtdoanhNghiep> TtdoanhNghiep { get; set; }
        public virtual DbSet<VaiTro> VaiTro { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=vppphucuong.ddns.net;Initial Catalog=WebBanhang;User ID=saBanhang;Password=@1234@");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaoHanh>(entity =>
            {
                entity.HasIndex(e => e.MaBh)
                    .HasName("unique_MaBH")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.LoaiTg)
                    .HasColumnName("LoaiTG")
                    .HasMaxLength(500);

                entity.Property(e => e.MaBh)
                    .HasColumnName("MaBH")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Slqd).HasColumnName("SLQD");
            });

            modelBuilder.Entity<CachXuat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TheoHsd).HasColumnName("TheoHSD");

                entity.Property(e => e.TheoTgnhap).HasColumnName("TheoTGNhap");
            });

            modelBuilder.Entity<ChiNhanh>(entity =>
            {
                entity.HasIndex(e => e.MaCn)
                    .HasName("unique_MaCN")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.MaCn)
                    .HasColumnName("MaCN")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(50);

                entity.Property(e => e.TenCn)
                    .HasColumnName("TenCN")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Cktm).HasColumnName("CKTM");

                entity.Property(e => e.GhiChu).HasMaxLength(2000);

                entity.Property(e => e.Hsd)
                    .HasColumnName("HSD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idbh).HasColumnName("IDBH");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nsx)
                    .HasColumnName("NSX")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Slg).HasColumnName("SLG");

                entity.Property(e => e.SoLo).HasMaxLength(50);

                entity.Property(e => e.Tgbh).HasColumnName("TGBH");

                entity.HasOne(d => d.IdbhNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idbh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_BaoHanh");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_HangHoa");

                entity.HasOne(d => d.IdpnNavigation)
                    .WithMany(p => p.ChiTietPhieuNhap)
                    .HasForeignKey(d => d.Idpn)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuNhap_PhieuNhap");
            });

            modelBuilder.Entity<ChiTietPhieuNhapTam>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Cktm).HasColumnName("CKTM");

                entity.Property(e => e.GhiChu).HasMaxLength(2000);

                entity.Property(e => e.Host).HasMaxLength(50);

                entity.Property(e => e.Hsd)
                    .HasColumnName("HSD")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idbh).HasColumnName("IDBH");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nsx)
                    .HasColumnName("NSX")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Slg).HasColumnName("SLG");

                entity.Property(e => e.SoLo).HasMaxLength(50);

                entity.Property(e => e.Tgbh).HasColumnName("TGBH");
            });

            modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Cktm).HasColumnName("CKTM");

                entity.Property(e => e.Idctpn).HasColumnName("IDCTPN");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpx).HasColumnName("IDPX");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Slg).HasColumnName("SLG");

                entity.HasOne(d => d.IdctpnNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Idctpn)
                    .HasConstraintName("FK__ChiTietPh__IDCTP__764C846B");

                entity.HasOne(d => d.IddvtNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Iddvt)
                    .HasConstraintName("FK__ChiTietPh__IDDVT__6B79F03D");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Idhh)
                    .HasConstraintName("fk_ChiTietPhieuXuat");

                entity.HasOne(d => d.IdpxNavigation)
                    .WithMany(p => p.ChiTietPhieuXuat)
                    .HasForeignKey(d => d.Idpx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChiTietPhieuXuat_PhieuXuat");
            });

            modelBuilder.Entity<ChiTietPhieuXuatTam>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cktm).HasColumnName("CKTM");

                entity.Property(e => e.Host).HasMaxLength(50);

                entity.Property(e => e.Idctpn).HasColumnName("IDCTPN");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idpx).HasColumnName("IDPX");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Slg).HasColumnName("SLG");
            });

            modelBuilder.Entity<ChiTietThuNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idptn).HasColumnName("IDPTN");

                entity.Property(e => e.Idpx).HasColumnName("IDPX");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.HasOne(d => d.IdptnNavigation)
                    .WithMany(p => p.ChiTietThuNo)
                    .HasForeignKey(d => d.Idptn)
                    .HasConstraintName("FK__ChiTietTh__IDPTN__6991A7CB");

                entity.HasOne(d => d.IdpxNavigation)
                    .WithMany(p => p.ChiTietThuNo)
                    .HasForeignKey(d => d.Idpx)
                    .HasConstraintName("fk_ChiTietThuNo");
            });

            modelBuilder.Entity<ChiTietTraNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idpn).HasColumnName("IDPN");

                entity.Property(e => e.Idptn).HasColumnName("IDPTN");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.HasOne(d => d.IdpnNavigation)
                    .WithMany(p => p.ChiTietTraNo)
                    .HasForeignKey(d => d.Idpn)
                    .HasConstraintName("fk_ChiTietTraNo");

                entity.HasOne(d => d.IdptnNavigation)
                    .WithMany(p => p.ChiTietTraNo)
                    .HasForeignKey(d => d.Idptn)
                    .HasConstraintName("FK__ChiTietTr__IDPTN__6D6238AF");
            });

            modelBuilder.Entity<ChucNang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idmenu).HasColumnName("IDMenu");

                entity.Property(e => e.Idvt).HasColumnName("IDVT");

                entity.Property(e => e.MaCnang)
                    .HasColumnName("MaCNang")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.HasOne(d => d.IdmenuNavigation)
                    .WithMany(p => p.ChucNang)
                    .HasForeignKey(d => d.Idmenu)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ChucNang_Menu");

                entity.HasOne(d => d.IdvtNavigation)
                    .WithMany(p => p.ChucNang)
                    .HasForeignKey(d => d.Idvt)
                    .HasConstraintName("FK__ChucNang__IDVT__50C5FA01");
            });

            modelBuilder.Entity<Dvt>(entity =>
            {
                entity.ToTable("DVT");

                entity.HasIndex(e => e.MaDvt)
                    .HasName("unique_MaDVT")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaDvt)
                    .HasColumnName("MaDVT")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenDvt)
                    .HasColumnName("TenDVT")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Dvvc>(entity =>
            {
                entity.ToTable("DVVC");

                entity.HasIndex(e => e.MaDvvc)
                    .HasName("unique_MaDVVC")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaDvvc)
                    .HasColumnName("MaDVVC")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenDvvc)
                    .HasColumnName("TenDVVC")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<GiaTheoKhachHang>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.HasOne(d => d.IddvtNavigation)
                    .WithMany(p => p.GiaTheoKhachHang)
                    .HasForeignKey(d => d.Iddvt)
                    .HasConstraintName("FK_GiaTheoKhachHang_DVT");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.GiaTheoKhachHang)
                    .HasForeignKey(d => d.Idhh)
                    .HasConstraintName("FK_GiaTheoKhachHang_HangHoa");

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.GiaTheoKhachHang)
                    .HasForeignKey(d => d.Idkh)
                    .HasConstraintName("FK_GiaTheoKhachHang_KhachHang");
            });

            modelBuilder.Entity<GiaTheoNhomHangHoa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idnhh).HasColumnName("IDNHH");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.HasOne(d => d.IdnhhNavigation)
                    .WithMany(p => p.GiaTheoNhomHangHoa)
                    .HasForeignKey(d => d.Idnhh)
                    .HasConstraintName("FK__GiaTheoNh__IDNHH__605D434C");
            });

            modelBuilder.Entity<HangHoa>(entity =>
            {
                entity.HasIndex(e => e.MaHh)
                    .HasName("unique_MaHH")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Iddvtchinh).HasColumnName("IDDVTChinh");

                entity.Property(e => e.Idhsx).HasColumnName("IDHSX");

                entity.Property(e => e.Idnhh).HasColumnName("IDNHH");

                entity.Property(e => e.Idnsx).HasColumnName("IDNSX");

                entity.Property(e => e.MaHh)
                    .HasColumnName("MaHH")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenHh)
                    .HasColumnName("TenHH")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IddvtchinhNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Iddvtchinh)
                    .HasConstraintName("FK__HangHoa__IDDVTCh__4AD81681");

                entity.HasOne(d => d.IdhsxNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idhsx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HangHoa_HangSanXuat");

                entity.HasOne(d => d.IdnhhNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idnhh)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NhomHangHoa_HangHoa");

                entity.HasOne(d => d.IdnsxNavigation)
                    .WithMany(p => p.HangHoa)
                    .HasForeignKey(d => d.Idnsx)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_HangHoa_NuocSanXuat");
            });

            modelBuilder.Entity<HangSanXuat>(entity =>
            {
                entity.HasIndex(e => e.MaHsx)
                    .HasName("unique_MaHSX")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaHsx)
                    .HasColumnName("MaHSX")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenHsx)
                    .HasColumnName("TenHSX")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<HhDvt>(entity =>
            {
                entity.ToTable("HH_DVT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Iddvt).HasColumnName("IDDVT");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.SlquyDoi).HasColumnName("SLQuyDoi");

                entity.HasOne(d => d.IddvtNavigation)
                    .WithMany(p => p.HhDvt)
                    .HasForeignKey(d => d.Iddvt)
                    .HasConstraintName("FK__HH_DVT__IDDVT__67A95F59");

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.HhDvt)
                    .HasForeignKey(d => d.Idhh)
                    .HasConstraintName("fk_HH_DVT");
            });

            modelBuilder.Entity<HinhAnhHangHoa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhh).HasColumnName("IDHH");

                entity.Property(e => e.Loai).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenHinh).HasMaxLength(250);

                entity.HasOne(d => d.IdhhNavigation)
                    .WithMany(p => p.HinhAnhHangHoa)
                    .HasForeignKey(d => d.Idhh)
                    .HasConstraintName("FK_HinhAnhHangHoa_HangHoa");
            });

            modelBuilder.Entity<HinhAnhNhanVien>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.Loai).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenHinh).HasMaxLength(250);

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.HinhAnhNhanVien)
                    .HasForeignKey(d => d.Idnv)
                    .HasConstraintName("FK_HinhAnhNhanVien_NhanVien");
            });

            modelBuilder.Entity<Httt>(entity =>
            {
                entity.ToTable("HTTT");

                entity.HasIndex(e => e.MaHttt)
                    .HasName("unique_MaHTTT")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaHttt)
                    .HasColumnName("MaHTTT")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenHttt)
                    .HasColumnName("TenHTTT")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasIndex(e => e.MaKh)
                    .HasName("unique_MaKH")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.LoaiKh).HasColumnName("LoaiKH");

                entity.Property(e => e.MaKh)
                    .HasColumnName("MaKH")
                    .HasMaxLength(50);

                entity.Property(e => e.MaSoThue).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsale).HasColumnName("NVSale");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(50);

                entity.Property(e => e.TenKh)
                    .HasColumnName("TenKH")
                    .HasMaxLength(500);

                entity.Property(e => e.TenTaiKhoan).HasMaxLength(50);

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.KhachHang)
                    .HasForeignKey(d => d.Idnv)
                    .HasConstraintName("FK__KhachHang__IDNV__7CA47C3F");

                entity.HasOne(d => d.TenTaiKhoanNavigation)
                    .WithMany(p => p.KhachHang)
                    .HasPrincipalKey(p => p.TenTaiKhoan)
                    .HasForeignKey(d => d.TenTaiKhoan)
                    .HasConstraintName("fk_KH_TenTk");
            });

            modelBuilder.Entity<KhachHangNganHang>(entity =>
            {
                entity.ToTable("KhachHang_NganHang");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idnh).HasColumnName("IDNH");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Stk)
                    .HasColumnName("STK")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.KhachHangNganHang)
                    .HasForeignKey(d => d.Idkh)
                    .HasConstraintName("fk_KhachHang_NganHang");

                entity.HasOne(d => d.IdnhNavigation)
                    .WithMany(p => p.KhachHangNganHang)
                    .HasForeignKey(d => d.Idnh)
                    .HasConstraintName("FK__KhachHang___IDNH__6F4A8121");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idphan).HasColumnName("IDPhan");

                entity.Property(e => e.MaMenu).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenMenu).HasMaxLength(100);

                entity.HasOne(d => d.IdphanNavigation)
                    .WithMany(p => p.Menu)
                    .HasForeignKey(d => d.Idphan)
                    .HasConstraintName("FK__Menu__IDPhan__11F49EE0");
            });

            modelBuilder.Entity<NccNganHang>(entity =>
            {
                entity.ToTable("NCC_NganHang");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idncc).HasColumnName("IDNCC");

                entity.Property(e => e.Idnh).HasColumnName("IDNH");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Stk)
                    .HasColumnName("STK")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdnccNavigation)
                    .WithMany(p => p.NccNganHang)
                    .HasForeignKey(d => d.Idncc)
                    .HasConstraintName("fk_NCC_NganHang");

                entity.HasOne(d => d.IdnhNavigation)
                    .WithMany(p => p.NccNganHang)
                    .HasForeignKey(d => d.Idnh)
                    .HasConstraintName("FK__NCC_NganHa__IDNH__7132C993");
            });

            modelBuilder.Entity<NganHang>(entity =>
            {
                entity.HasIndex(e => e.MaNh)
                    .HasName("unique_MaNH")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");

                entity.Property(e => e.MaNh)
                    .HasColumnName("MaNH")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenNh)
                    .HasColumnName("TenNH")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IdhtttNavigation)
                    .WithMany(p => p.NganHang)
                    .HasForeignKey(d => d.Idhttt)
                    .HasConstraintName("FK__NganHang__IDHTTT__7F80E8EA");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasIndex(e => e.MaNcc)
                    .HasName("unique_MaNCC")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.GhiChu).HasMaxLength(500);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNcc)
                    .HasColumnName("MaNCC")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(50);

                entity.Property(e => e.TenNcc)
                    .HasColumnName("TenNCC")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasIndex(e => e.MaNv)
                    .HasName("unique_MaNV")
                    .IsUnique();

                entity.HasIndex(e => e.TenTaiKhoan)
                    .HasName("unique_NV_tenTk")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.Cccd)
                    .HasColumnName("CCCD")
                    .HasMaxLength(50);

                entity.Property(e => e.DiaChi).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idnnv).HasColumnName("IDNNV");

                entity.Property(e => e.MaNv)
                    .HasColumnName("MaNV")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySinh).HasColumnType("datetime");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.QueQuan).HasMaxLength(500);

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(50);

                entity.Property(e => e.TenNv)
                    .HasColumnName("TenNV")
                    .HasMaxLength(500);

                entity.Property(e => e.TenTaiKhoan).HasMaxLength(50);

                entity.HasOne(d => d.IdnnvNavigation)
                    .WithMany(p => p.NhanVien)
                    .HasForeignKey(d => d.Idnnv)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NhanVien_NhanVien");

                entity.HasOne(d => d.TenTaiKhoanNavigation)
                    .WithOne(p => p.NhanVien)
                    .HasPrincipalKey<TaiKhoan>(p => p.TenTaiKhoan)
                    .HasForeignKey<NhanVien>(d => d.TenTaiKhoan)
                    .HasConstraintName("fk_NV_TenTk");
            });

            modelBuilder.Entity<NhomHangHoa>(entity =>
            {
                entity.HasIndex(e => e.MaNhh)
                    .HasName("unique_MaNHH")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNhh)
                    .HasColumnName("MaNHH")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenNhh)
                    .HasColumnName("TenNHH")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NhomNhanVien>(entity =>
            {
                entity.HasIndex(e => e.MaNnv)
                    .HasName("unique_MaNNV")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNnv)
                    .HasColumnName("MaNNV")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenNnv)
                    .HasColumnName("TenNNV")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<NuocSanXuat>(entity =>
            {
                entity.HasIndex(e => e.MaNsx)
                    .HasName("unique_MaNSX")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.MaNsx)
                    .HasColumnName("MaNSX")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenNsx)
                    .HasColumnName("TenNSX")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Phan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TenPhan).HasMaxLength(50);
            });

            modelBuilder.Entity<PhanQuyen>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idtk).HasColumnName("IDTK");

                entity.Property(e => e.Idvt).HasColumnName("IDVT");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");
            });

            modelBuilder.Entity<PhieuNhap>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.GhiChu).HasMaxLength(2000);

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idncc).HasColumnName("IDNCC");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.NgayHd)
                    .HasColumnName("NgayHD")
                    .HasColumnType("date");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.SoHd)
                    .HasColumnName("SoHD")
                    .HasMaxLength(10);

                entity.Property(e => e.SoPn)
                    .HasColumnName("SoPN")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdnccNavigation)
                    .WithMany(p => p.PhieuNhap)
                    .HasForeignKey(d => d.Idncc)
                    .HasConstraintName("FK__PhieuNhap__IDNCC__60FC61CA");

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.PhieuNhap)
                    .HasForeignKey(d => d.Idnv)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PhieuNhap_NhanVien");
            });

            modelBuilder.Entity<PhieuThuNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NgayTra).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.SoPhieu).HasMaxLength(50);

                entity.HasOne(d => d.IdhtttNavigation)
                    .WithMany(p => p.PhieuThuNo)
                    .HasForeignKey(d => d.Idhttt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PhieuThuNo_HTTT");

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.PhieuThuNo)
                    .HasForeignKey(d => d.Idnv)
                    .HasConstraintName("FK__PhieuThuNo__IDNV__4460231C");
            });

            modelBuilder.Entity<PhieuTraNo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idhttt).HasColumnName("IDHTTT");

                entity.Property(e => e.Idncc).HasColumnName("IDNCC");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NgayTra).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.SoPhieu).HasMaxLength(50);

                entity.HasOne(d => d.IdhtttNavigation)
                    .WithMany(p => p.PhieuTraNo)
                    .HasForeignKey(d => d.Idhttt)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PhieuTraNo_HTTT");

                entity.HasOne(d => d.IdnccNavigation)
                    .WithMany(p => p.PhieuTraNo)
                    .HasForeignKey(d => d.Idncc)
                    .HasConstraintName("FK__PhieuTraN__IDNCC__731B1205");

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.PhieuTraNo)
                    .HasForeignKey(d => d.Idnv)
                    .HasConstraintName("FK__PhieuTraNo__IDNV__473C8FC7");
            });

            modelBuilder.Entity<PhieuXuat>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.GhiChu)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idnv).HasColumnName("IDNV");

                entity.Property(e => e.Idtt).HasColumnName("IDTT");

                entity.Property(e => e.NgayHd)
                    .HasColumnName("NgayHD")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.SoHd)
                    .HasColumnName("SoHD")
                    .HasMaxLength(50);

                entity.Property(e => e.SoPx)
                    .HasColumnName("SoPX")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.PhieuXuat)
                    .HasForeignKey(d => d.Idkh)
                    .HasConstraintName("FK__PhieuXuat__IDKH__7ABC33CD");

                entity.HasOne(d => d.IdnvNavigation)
                    .WithMany(p => p.PhieuXuat)
                    .HasForeignKey(d => d.Idnv)
                    .HasConstraintName("fk_PhieuXuat");

                entity.HasOne(d => d.IdttNavigation)
                    .WithMany(p => p.PhieuXuat)
                    .HasForeignKey(d => d.Idtt)
                    .HasConstraintName("FK__PhieuXuat__IDTT__79C80F94");
            });

            modelBuilder.Entity<QuyDinhMa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.TiepDauNgu).HasMaxLength(1);
            });

            modelBuilder.Entity<SoLuongHhcon>(entity =>
            {
                entity.ToTable("SoLuongHHCon");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cktm).HasColumnName("CKTM");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Idctpn).HasColumnName("IDCTPN");

                entity.Property(e => e.NgayNhap).HasColumnType("datetime");

                entity.Property(e => e.Slcon).HasColumnName("SLCon");

                entity.HasOne(d => d.IdctpnNavigation)
                    .WithMany(p => p.SoLuongHhcon)
                    .HasForeignKey(d => d.Idctpn)
                    .HasConstraintName("FK__SoLuongHH__IDCTP__24485945");
            });

            modelBuilder.Entity<SoThuTu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Loai)
                    .HasColumnName("loai")
                    .HasMaxLength(50);

                entity.Property(e => e.Ngay)
                    .HasColumnName("ngay")
                    .HasColumnType("datetime");

                entity.Property(e => e.Stt).HasColumnName("stt");
            });

            modelBuilder.Entity<TaiKhoan>(entity =>
            {
                entity.HasIndex(e => e.TenTaiKhoan)
                    .HasName("unique_TenTK")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.MatKhau).HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenTaiKhoan)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TrangThai>(entity =>
            {
                entity.HasIndex(e => e.MaTt)
                    .HasName("unique_MaTT")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.Idcn).HasColumnName("IDCN");

                entity.Property(e => e.Iddvvc).HasColumnName("IDDVVC");

                entity.Property(e => e.Idnvtt).HasColumnName("IDNVTT");

                entity.Property(e => e.Idnvxk).HasColumnName("IDNVXK");

                entity.Property(e => e.MaTt)
                    .HasColumnName("MaTT")
                    .HasMaxLength(50);

                entity.Property(e => e.NgayBdvc)
                    .HasColumnName("NgayBDVC")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayGiao).HasColumnType("datetime");

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.NgayTt)
                    .HasColumnName("NgayTT")
                    .HasColumnType("datetime");

                entity.Property(e => e.NgayXk)
                    .HasColumnName("NgayXK")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.Sdtnvgh)
                    .HasColumnName("SDTNVGH")
                    .HasMaxLength(50);

                entity.Property(e => e.TenNvgh)
                    .HasColumnName("TenNVGH")
                    .HasMaxLength(500);

                entity.Property(e => e.TenTt)
                    .HasColumnName("TenTT")
                    .HasMaxLength(500);

                entity.HasOne(d => d.IddvvcNavigation)
                    .WithMany(p => p.TrangThai)
                    .HasForeignKey(d => d.Iddvvc)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TrangThai_DVVC");
            });

            modelBuilder.Entity<TtdoanhNghiep>(entity =>
            {
                entity.ToTable("TTDoanhNghiep");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChuTk)
                    .HasColumnName("ChuTK")
                    .HasMaxLength(100);

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.MaSoThue).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasColumnName("SDT")
                    .HasMaxLength(20);

                entity.Property(e => e.Stk)
                    .HasColumnName("STK")
                    .HasMaxLength(20);

                entity.Property(e => e.Ten).HasMaxLength(200);

                entity.Property(e => e.TenNh)
                    .HasColumnName("TenNH")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<VaiTro>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Active).HasDefaultValueSql("((1))");

                entity.Property(e => e.MaVt)
                    .HasColumnName("MaVT")
                    .HasMaxLength(50);

                entity.Property(e => e.NgaySua).HasColumnType("datetime");

                entity.Property(e => e.NgayTao).HasColumnType("datetime");

                entity.Property(e => e.Nvsua).HasColumnName("NVSua");

                entity.Property(e => e.Nvtao).HasColumnName("NVTao");

                entity.Property(e => e.TenVt)
                    .HasColumnName("TenVT")
                    .HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
