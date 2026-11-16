using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Helpers;
using QuanLyTuyenDung.Web.Models.Entities;
using QuanLyTuyenDung.Web.Models.Enums;

namespace QuanLyTuyenDung.Web.Controllers;

public class DoanhNghiepController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? tuKhoa, string? maSoThue, TinhTrangHoatDong? tinhTrang, string sapXep = "ten")
    {
        var query = db.DoanhNghieps.Include(d => d.HoSoTuyenDungs).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            query = query.Where(d => d.TenDoanhNghiep.Contains(tuKhoa) || d.TinhThanh!.Contains(tuKhoa)
                                     || d.NguoiLienHe!.Contains(tuKhoa));
        }
        if (tinhTrang.HasValue)
            query = query.Where(d => d.TinhTrang == tinhTrang);

        var ds = await query.ToListAsync();

        // Tra cứu chính xác theo mã số thuế: sắp xếp theo MST rồi tìm kiếm nhị phân
        if (!string.IsNullOrWhiteSpace(maSoThue))
        {
            ThuatToan.QuickSort(ds, (a, b) => string.CompareOrdinal(a.MaSoThue, b.MaSoThue));
            var viTri = ThuatToan.TimKiemNhiPhan(ds, maSoThue.Trim(), d => d.MaSoThue);
            ds = viTri >= 0 ? [ds[viTri]] : [];
        }

        // Sắp xếp danh sách hiển thị bằng Quick Sort
        Comparison<DoanhNghiep> soSanh = sapXep switch
        {
            "mst" => (a, b) => string.CompareOrdinal(a.MaSoThue, b.MaSoThue),
            "tinh" => (a, b) => string.Compare(a.TinhThanh, b.TinhThanh, StringComparison.CurrentCulture),
            "moi" => (a, b) => b.NgayTao.CompareTo(a.NgayTao),
            _ => (a, b) => string.Compare(a.TenDoanhNghiep, b.TenDoanhNghiep, StringComparison.CurrentCulture)
        };
        ThuatToan.QuickSort(ds, soSanh);

        ViewData["TuKhoa"] = tuKhoa;
        ViewData["MaSoThue"] = maSoThue;
        ViewData["TinhTrang"] = tinhTrang;
        ViewData["SapXep"] = sapXep;
        return View(ds);
    }

    public async Task<IActionResult> Details(int id)
    {
        var dn = await db.DoanhNghieps
            .Include(d => d.HoSoTuyenDungs).ThenInclude(h => h.ChiTiets)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);
        return dn is null ? NotFound() : View(dn);
    }

    public IActionResult Create() => View("Form", new DoanhNghiep());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DoanhNghiep model)
    {
        await KiemTraTrungMaSoThue(model);
        if (!ModelState.IsValid) return View("Form", model);

        model.NgayTao = DateTime.Now;
        db.DoanhNghieps.Add(model);
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã thêm doanh nghiệp \"{model.TenDoanhNghiep}\".";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var dn = await db.DoanhNghieps.FindAsync(id);
        return dn is null ? NotFound() : View("Form", dn);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DoanhNghiep model)
    {
        if (id != model.Id) return BadRequest();

        await KiemTraTrungMaSoThue(model);
        if (!ModelState.IsValid) return View("Form", model);

        db.Entry(model).State = EntityState.Modified;
        db.Entry(model).Property(d => d.NgayTao).IsModified = false;
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã cập nhật doanh nghiệp \"{model.TenDoanhNghiep}\".";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var dn = await db.DoanhNghieps.Include(d => d.HoSoTuyenDungs).FirstOrDefaultAsync(d => d.Id == id);
        if (dn is null) return NotFound();

        if (dn.HoSoTuyenDungs.Count > 0)
        {
            TempData["Loi"] = $"Không thể xóa \"{dn.TenDoanhNghiep}\" vì đã có {dn.HoSoTuyenDungs.Count} hồ sơ tuyển dụng. "
                              + "Hãy chuyển tình trạng sang \"Ngừng hoạt động\".";
            return RedirectToAction(nameof(Index));
        }

        db.DoanhNghieps.Remove(dn);
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã xóa doanh nghiệp \"{dn.TenDoanhNghiep}\".";
        return RedirectToAction(nameof(Index));
    }

    private async Task KiemTraTrungMaSoThue(DoanhNghiep model)
    {
        if (await db.DoanhNghieps.AnyAsync(d => d.MaSoThue == model.MaSoThue && d.Id != model.Id))
            ModelState.AddModelError(nameof(DoanhNghiep.MaSoThue), "Mã số thuế đã tồn tại – doanh nghiệp này đã có trong hệ thống.");
    }
}
