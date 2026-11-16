using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NganhNghe> NganhNghes => Set<NganhNghe>();
    public DbSet<ViTriTuyenDung> ViTriTuyenDungs => Set<ViTriTuyenDung>();
    public DbSet<KienThuc> KienThucs => Set<KienThuc>();
    public DbSet<KyNang> KyNangs => Set<KyNang>();
    public DbSet<ChungChi> ChungChis => Set<ChungChi>();
    public DbSet<BangCap> BangCaps => Set<BangCap>();
    public DbSet<DoanhNghiep> DoanhNghieps => Set<DoanhNghiep>();
    public DbSet<HoSoTuyenDung> HoSoTuyenDungs => Set<HoSoTuyenDung>();
    public DbSet<ChiTietTuyenDung> ChiTietTuyenDungs => Set<ChiTietTuyenDung>();
    public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mã danh mục là duy nhất trong từng bảng danh mục
        modelBuilder.Entity<NganhNghe>().HasIndex(x => x.Ma).IsUnique();
        modelBuilder.Entity<ViTriTuyenDung>().HasIndex(x => x.Ma).IsUnique();
        modelBuilder.Entity<KienThuc>().HasIndex(x => x.Ma).IsUnique();
        modelBuilder.Entity<KyNang>().HasIndex(x => x.Ma).IsUnique();
        modelBuilder.Entity<ChungChi>().HasIndex(x => x.Ma).IsUnique();
        modelBuilder.Entity<BangCap>().HasIndex(x => x.Ma).IsUnique();

        modelBuilder.Entity<DoanhNghiep>(e =>
        {
            e.HasIndex(x => x.MaSoThue).IsUnique();   // tránh trùng lặp doanh nghiệp
            e.Property(x => x.MaSoThue).HasMaxLength(14);
        });

        modelBuilder.Entity<HoSoTuyenDung>(e =>
        {
            e.HasIndex(x => x.MaHoSo).IsUnique();
            e.HasOne(x => x.DoanhNghiep)
             .WithMany(d => d.HoSoTuyenDungs)
             .HasForeignKey(x => x.DoanhNghiepId)
             .OnDelete(DeleteBehavior.Restrict);      // không xóa doanh nghiệp đã có hồ sơ
        });

        modelBuilder.Entity<ChiTietTuyenDung>(e =>
        {
            e.HasOne(x => x.HoSoTuyenDung).WithMany(h => h.ChiTiets)
             .HasForeignKey(x => x.HoSoTuyenDungId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.NganhNghe).WithMany()
             .HasForeignKey(x => x.NganhNgheId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ViTriTuyenDung).WithMany()
             .HasForeignKey(x => x.ViTriTuyenDungId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.BangCap).WithMany()
             .HasForeignKey(x => x.BangCapId).OnDelete(DeleteBehavior.Restrict);
        });

        CauHinhNhieuNhieu<KienThuc>(modelBuilder, c => c.KienThucs, "ChiTiet_KienThuc");
        CauHinhNhieuNhieu<KyNang>(modelBuilder, c => c.KyNangs, "ChiTiet_KyNang");
        CauHinhNhieuNhieu<ChungChi>(modelBuilder, c => c.ChungChis, "ChiTiet_ChungChi");

        modelBuilder.Entity<NguoiDung>().HasIndex(x => x.TenDangNhap).IsUnique();
    }

    /// <summary>
    /// Bảng trung gian giữa chi tiết tuyển dụng và danh mục yêu cầu.
    /// Xóa chi tiết thì xóa liên kết; xóa danh mục đang được dùng thì bị chặn.
    /// </summary>
    private static void CauHinhNhieuNhieu<T>(
        ModelBuilder modelBuilder,
        Expression<Func<ChiTietTuyenDung, IEnumerable<T>?>> navigation,
        string tenBang) where T : DanhMucBase
    {
        var fk = typeof(T).Name + "Id";
        modelBuilder.Entity<ChiTietTuyenDung>()
            .HasMany(navigation)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                tenBang,
                r => r.HasOne<T>().WithMany().HasForeignKey(fk).OnDelete(DeleteBehavior.Restrict),
                l => l.HasOne<ChiTietTuyenDung>().WithMany().HasForeignKey("ChiTietTuyenDungId").OnDelete(DeleteBehavior.Cascade));
    }
}
