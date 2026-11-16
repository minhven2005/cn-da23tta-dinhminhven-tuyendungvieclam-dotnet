using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;
using QuanLyTuyenDung.Web.Models.Enums;
using QuanLyTuyenDung.Web.ViewModels;

namespace QuanLyTuyenDung.Web.Controllers;

public class HoSoTuyenDungController(AppDbContext db) : Controller
{
    // ===== Hồ sơ tuyển dụng =====

    public async Task<IActionResult> Index(string? tuKhoa, int? doanhNghiepId, int? nam, int? dot, TrangThaiHoSo? trangThai)
    {
        var query = db.HoSoTuyenDungs
            .Include(h => h.DoanhNghiep)
            .Include(h => h.ChiTiets)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            query = query.Where(h => h.MaHoSo.Contains(tuKhoa) || h.DoanhNghiep!.TenDoanhNghiep.Contains(tuKhoa)
                                     || h.ChiTiets.Any(c => c.ViTriTuyenDung!.Ten.Contains(tuKhoa)
                                                            || c.NganhNghe!.Ten.Contains(tuKhoa)));
        }
        if (doanhNghiepId.HasValue) query = query.Where(h => h.DoanhNghiepId == doanhNghiepId);
        if (nam.HasValue) query = query.Where(h => h.Nam == nam);
        if (dot.HasValue) query = query.Where(h => h.DotTuyenDung == dot);
        if (trangThai.HasValue) query = query.Where(h => h.TrangThai == trangThai);

        ViewData["TuKhoa"] = tuKhoa;
        ViewData["Nam"] = nam;
        ViewData["Dot"] = dot;
        ViewData["TrangThai"] = trangThai;
        ViewBag.DsDoanhNghiep = new SelectList(await db.DoanhNghieps.OrderBy(d => d.TenDoanhNghiep).ToListAsync(),
            nameof(DoanhNghiep.Id), nameof(DoanhNghiep.TenDoanhNghiep), doanhNghiepId);
        ViewBag.DsNam = await db.HoSoTuyenDungs.Select(h => h.Nam).Distinct().OrderByDescending(n => n).ToListAsync();

        return View(await query.OrderByDescending(h => h.NgayTiepNhan).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var hoSo = await db.HoSoTuyenDungs
            .Include(h => h.DoanhNghiep)
            .Include(h => h.ChiTiets).ThenInclude(c => c.NganhNghe)
            .Include(h => h.ChiTiets).ThenInclude(c => c.ViTriTuyenDung)
            .Include(h => h.ChiTiets).ThenInclude(c => c.BangCap)
            .Include(h => h.ChiTiets).ThenInclude(c => c.KienThucs)
            .Include(h => h.ChiTiets).ThenInclude(c => c.KyNangs)
            .Include(h => h.ChiTiets).ThenInclude(c => c.ChungChis)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == id);
        return hoSo is null ? NotFound() : View(hoSo);
    }

    public async Task<IActionResult> Create(int? doanhNghiepId)
    {
        var soLuong = await db.HoSoTuyenDungs.CountAsync();
        var model = new HoSoTuyenDung { MaHoSo = $"HS{soLuong + 1:000}", DoanhNghiepId = doanhNghiepId ?? 0 };
        await NapDanhSachDoanhNghiep();
        return View("Form", model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(HoSoTuyenDung model)
    {
        await KiemTraHoSo(model);
        if (!ModelState.IsValid)
        {
            await NapDanhSachDoanhNghiep();
            return View("Form", model);
        }

        db.HoSoTuyenDungs.Add(model);
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã tạo hồ sơ {model.MaHoSo}. Hãy thêm các vị trí tuyển dụng cho hồ sơ.";
        return RedirectToAction(nameof(Details), new { id = model.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var hoSo = await db.HoSoTuyenDungs.FindAsync(id);
        if (hoSo is null) return NotFound();
        await NapDanhSachDoanhNghiep();
        return View("Form", hoSo);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, HoSoTuyenDung model)
    {
        if (id != model.Id) return BadRequest();

        await KiemTraHoSo(model);
        if (!ModelState.IsValid)
        {
            await NapDanhSachDoanhNghiep();
            return View("Form", model);
        }

        db.Update(model);
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã cập nhật hồ sơ {model.MaHoSo}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var hoSo = await db.HoSoTuyenDungs.FindAsync(id);
        if (hoSo is null) return NotFound();

        db.HoSoTuyenDungs.Remove(hoSo);   // các chi tiết bị xóa theo (Cascade)
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã xóa hồ sơ {hoSo.MaHoSo}.";
        return RedirectToAction(nameof(Index));
    }

    // ===== Vị trí tuyển dụng trong hồ sơ (chi tiết) =====

    public async Task<IActionResult> ThemViTri(int hoSoId)
    {
        var hoSo = await db.HoSoTuyenDungs.Include(h => h.DoanhNghiep).FirstOrDefaultAsync(h => h.Id == hoSoId);
        if (hoSo is null) return NotFound();

        var vm = new ChiTietTuyenDungFormViewModel { HoSoTuyenDungId = hoSo.Id };
        await NapDuLieuForm(vm);
        return View("ViTriForm", vm);
    }

    public async Task<IActionResult> SuaViTri(int id)
    {
        var ct = await db.ChiTietTuyenDungs
            .Include(c => c.KienThucs).Include(c => c.KyNangs).Include(c => c.ChungChis)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (ct is null) return NotFound();

        var vm = new ChiTietTuyenDungFormViewModel
        {
            Id = ct.Id, HoSoTuyenDungId = ct.HoSoTuyenDungId,
            NganhNgheId = ct.NganhNgheId, ViTriTuyenDungId = ct.ViTriTuyenDungId, BangCapId = ct.BangCapId,
            SoLuong = ct.SoLuong, MucLuong = ct.MucLuong, MoTaCongViec = ct.MoTaCongViec,
            KienThucIds = ct.KienThucs.Select(x => x.Id).ToList(),
            KyNangIds = ct.KyNangs.Select(x => x.Id).ToList(),
            ChungChiIds = ct.ChungChis.Select(x => x.Id).ToList()
        };
        await NapDuLieuForm(vm);
        return View("ViTriForm", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> LuuViTri(ChiTietTuyenDungFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await NapDuLieuForm(vm);
            return View("ViTriForm", vm);
        }

        ChiTietTuyenDung ct;
        if (vm.Id == 0)
        {
            ct = new ChiTietTuyenDung { HoSoTuyenDungId = vm.HoSoTuyenDungId };
            db.ChiTietTuyenDungs.Add(ct);
        }
        else
        {
            var tonTai = await db.ChiTietTuyenDungs
                .Include(c => c.KienThucs).Include(c => c.KyNangs).Include(c => c.ChungChis)
                .FirstOrDefaultAsync(c => c.Id == vm.Id);
            if (tonTai is null) return NotFound();
            ct = tonTai;
        }

        ct.NganhNgheId = vm.NganhNgheId;
        ct.ViTriTuyenDungId = vm.ViTriTuyenDungId;
        ct.BangCapId = vm.BangCapId;
        ct.SoLuong = vm.SoLuong;
        ct.MucLuong = vm.MucLuong;
        ct.MoTaCongViec = vm.MoTaCongViec;
        ct.KienThucs = await db.KienThucs.Where(x => vm.KienThucIds.Contains(x.Id)).ToListAsync();
        ct.KyNangs = await db.KyNangs.Where(x => vm.KyNangIds.Contains(x.Id)).ToListAsync();
        ct.ChungChis = await db.ChungChis.Where(x => vm.ChungChiIds.Contains(x.Id)).ToListAsync();

        await db.SaveChangesAsync();
        TempData["ThanhCong"] = vm.Id == 0 ? "Đã thêm vị trí tuyển dụng." : "Đã cập nhật vị trí tuyển dụng.";
        return RedirectToAction(nameof(Details), new { id = ct.HoSoTuyenDungId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> XoaViTri(int id)
    {
        var ct = await db.ChiTietTuyenDungs.FindAsync(id);
        if (ct is null) return NotFound();

        db.ChiTietTuyenDungs.Remove(ct);
        await db.SaveChangesAsync();
        TempData["ThanhCong"] = "Đã xóa vị trí tuyển dụng khỏi hồ sơ.";
        return RedirectToAction(nameof(Details), new { id = ct.HoSoTuyenDungId });
    }

    // ===== Hàm hỗ trợ =====

    private async Task KiemTraHoSo(HoSoTuyenDung model)
    {
        if (await db.HoSoTuyenDungs.AnyAsync(h => h.MaHoSo == model.MaHoSo && h.Id != model.Id))
            ModelState.AddModelError(nameof(HoSoTuyenDung.MaHoSo), "Mã hồ sơ đã tồn tại.");

        // Chỉ kiểm tra tình trạng doanh nghiệp khi tạo mới hoặc đổi doanh nghiệp của hồ sơ
        var doanhNghiepCu = model.Id == 0 ? (int?)null
            : await db.HoSoTuyenDungs.Where(h => h.Id == model.Id).Select(h => (int?)h.DoanhNghiepId).FirstOrDefaultAsync();
        var dn = await db.DoanhNghieps.AsNoTracking().FirstOrDefaultAsync(d => d.Id == model.DoanhNghiepId);
        if (dn is not null && doanhNghiepCu != dn.Id && dn.TinhTrang != TinhTrangHoatDong.DangHoatDong)
            ModelState.AddModelError(nameof(HoSoTuyenDung.DoanhNghiepId),
                "Doanh nghiệp không ở trạng thái đang hoạt động – không thể tiếp nhận hồ sơ.");

        if (model.HanNopHoSo.HasValue && model.HanNopHoSo < model.NgayTiepNhan)
            ModelState.AddModelError(nameof(HoSoTuyenDung.HanNopHoSo), "Hạn nộp phải sau ngày tiếp nhận.");
    }

    private async Task NapDanhSachDoanhNghiep()
    {
        ViewBag.DsDoanhNghiep = (await db.DoanhNghieps.OrderBy(d => d.TenDoanhNghiep).AsNoTracking().ToListAsync())
            .Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.TinhTrang == TinhTrangHoatDong.DangHoatDong ? d.TenDoanhNghiep : $"{d.TenDoanhNghiep} (không hoạt động)"
            }).ToList();
    }

    private async Task NapDuLieuForm(ChiTietTuyenDungFormViewModel vm)
    {
        var hoSo = await db.HoSoTuyenDungs.Include(h => h.DoanhNghiep).AsNoTracking()
            .FirstAsync(h => h.Id == vm.HoSoTuyenDungId);
        vm.MaHoSo = hoSo.MaHoSo;
        vm.TenDoanhNghiep = hoSo.DoanhNghiep?.TenDoanhNghiep;

        vm.DsNganhNghe = await DanhSachChon(db.NganhNghes);
        vm.DsViTri = await DanhSachChon(db.ViTriTuyenDungs);
        vm.DsBangCap = await DanhSachChon(db.BangCaps);
        vm.DsKienThuc = await DanhSachChon(db.KienThucs);
        vm.DsKyNang = await DanhSachChon(db.KyNangs);
        vm.DsChungChi = await DanhSachChon(db.ChungChis);
    }

    private static async Task<List<SelectListItem>> DanhSachChon<T>(IQueryable<T> nguon) where T : DanhMucBase
        => await nguon.Where(x => x.ConSuDung).OrderBy(x => x.Ten)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Ten })
            .ToListAsync();
}
