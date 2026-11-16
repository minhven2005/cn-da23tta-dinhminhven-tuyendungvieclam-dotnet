using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;
using QuanLyTuyenDung.Web.Models.Enums;
using QuanLyTuyenDung.Web.ViewModels;

namespace QuanLyTuyenDung.Web.Services;

/// <summary>Nghiệp vụ thống kê nhà tuyển dụng và công việc tuyển dụng theo tháng, năm (theo ngày tiếp nhận hồ sơ).</summary>
public class ThongKeService(AppDbContext db) : IThongKeService
{
    public async Task<ThongKeViewModel> ThongKeAsync(int nam)
    {
        var danhSachNam = await db.HoSoTuyenDungs
            .Select(h => h.NgayTiepNhan.Year).Distinct().OrderByDescending(n => n).ToListAsync();
        if (!danhSachNam.Contains(nam)) danhSachNam.Insert(0, nam);

        var tatCa = await LayHoSoAsync();
        var trongNam = tatCa.Where(h => h.NgayTiepNhan.Year == nam).ToList();

        return new ThongKeViewModel
        {
            Nam = nam,
            DanhSachNam = danhSachNam,
            TheoThang = TheoThang(trongNam),
            TheoNam = tatCa.GroupBy(h => h.NgayTiepNhan.Year)
                .OrderByDescending(g => g.Key)
                .Select(g => new ThongKeNamItem
                {
                    Nam = g.Key,
                    SoNhaTuyenDung = g.Select(h => h.DoanhNghiepId).Distinct().Count(),
                    SoHoSo = g.Count(),
                    SoViTri = g.Sum(h => h.ChiTiets.Count),
                    TongSoLuong = g.Sum(h => h.ChiTiets.Sum(c => c.SoLuong))
                }).ToList(),
            TheoNganh = trongNam.SelectMany(h => h.ChiTiets)
                .GroupBy(c => c.NganhNghe?.Ten ?? "(Không rõ)")
                .Select(g => new ThongKeNganhItem { TenNganh = g.Key, SoViTri = g.Count(), TongSoLuong = g.Sum(c => c.SoLuong) })
                .OrderByDescending(x => x.TongSoLuong).ToList(),
            NhaTuyenDung = trongNam.GroupBy(h => h.DoanhNghiepId)
                .Select(g => new NhaTuyenDungItem
                {
                    DoanhNghiepId = g.Key,
                    TenDoanhNghiep = g.First().DoanhNghiep!.TenDoanhNghiep,
                    TinhThanh = g.First().DoanhNghiep!.TinhThanh,
                    SoHoSo = g.Count(),
                    SoViTri = g.Sum(h => h.ChiTiets.Count),
                    TongSoLuong = g.Sum(h => h.ChiTiets.Sum(c => c.SoLuong))
                })
                .OrderByDescending(x => x.TongSoLuong).ToList()
        };
    }

    public async Task<DashboardViewModel> TongQuanAsync()
    {
        var nam = DateTime.Today.Year;
        var trongNam = (await LayHoSoAsync()).Where(h => h.NgayTiepNhan.Year == nam).ToList();

        return new DashboardViewModel
        {
            SoDoanhNghiep = await db.DoanhNghieps.CountAsync(),
            SoDoanhNghiepHoatDong = await db.DoanhNghieps.CountAsync(d => d.TinhTrang == TinhTrangHoatDong.DangHoatDong),
            SoHoSoTrongNam = trongNam.Count,
            SoViTriTrongNam = trongNam.Sum(h => h.ChiTiets.Count),
            TongNhuCauTrongNam = trongNam.Sum(h => h.ChiTiets.Sum(c => c.SoLuong)),
            SoHoSoChoDuyet = await db.HoSoTuyenDungs.CountAsync(h => h.TrangThai == TrangThaiHoSo.MoiTiepNhan),
            HoSoMoiNhat = await db.HoSoTuyenDungs.Include(h => h.DoanhNghiep)
                .OrderByDescending(h => h.NgayTiepNhan).Take(5).AsNoTracking().ToListAsync(),
            TheoThang = TheoThang(trongNam)
        };
    }

    private Task<List<HoSoTuyenDung>> LayHoSoAsync()
        => db.HoSoTuyenDungs
            .Include(h => h.DoanhNghiep)
            .Include(h => h.ChiTiets).ThenInclude(c => c.NganhNghe)
            .Where(h => h.TrangThai != TrangThaiHoSo.TuChoi)
            .AsNoTracking()
            .ToListAsync();

    private static List<ThongKeThangItem> TheoThang(List<HoSoTuyenDung> hoSo)
        => Enumerable.Range(1, 12).Select(thang =>
        {
            var trongThang = hoSo.Where(h => h.NgayTiepNhan.Month == thang).ToList();
            return new ThongKeThangItem
            {
                Thang = thang,
                SoNhaTuyenDung = trongThang.Select(h => h.DoanhNghiepId).Distinct().Count(),
                SoHoSo = trongThang.Count,
                SoViTri = trongThang.Sum(h => h.ChiTiets.Count),
                TongSoLuong = trongThang.Sum(h => h.ChiTiets.Sum(c => c.SoLuong))
            };
        }).ToList();
}
