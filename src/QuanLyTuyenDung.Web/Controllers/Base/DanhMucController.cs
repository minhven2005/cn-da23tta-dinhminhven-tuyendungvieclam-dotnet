using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers.Base;

/// <summary>
/// Controller dùng chung cho các danh mục: Thêm, Sửa, Xóa, Tìm kiếm.
/// Các controller danh mục cụ thể chỉ cần kế thừa và khai báo tiêu đề.
/// </summary>
public abstract class DanhMucController<T>(AppDbContext db) : Controller where T : DanhMucBase, new()
{
    private const string ThuMucView = "~/Views/Shared/DanhMuc/";

    protected AppDbContext Db { get; } = db;
    protected abstract string TieuDe { get; }
    protected abstract string BieuTuong { get; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewData["TieuDe"] = TieuDe;
        ViewData["BieuTuong"] = BieuTuong;
        base.OnActionExecuting(context);
    }

    public async Task<IActionResult> Index(string? tuKhoa)
    {
        var query = Db.Set<T>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            query = query.Where(x => x.Ma.Contains(tuKhoa) || x.Ten.Contains(tuKhoa));
        }

        ViewData["TuKhoa"] = tuKhoa;
        return View(ThuMucView + "Index.cshtml", await query.OrderBy(x => x.Ma).ToListAsync());
    }

    public IActionResult Create() => View(ThuMucView + "Form.cshtml", new T());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(T item)
    {
        await KiemTraTrungMa(item);
        if (!ModelState.IsValid) return View(ThuMucView + "Form.cshtml", item);

        Db.Add(item);
        await Db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã thêm \"{item.Ten}\".";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await Db.Set<T>().FindAsync(id);
        return item is null ? NotFound() : View(ThuMucView + "Form.cshtml", item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, T item)
    {
        if (id != item.Id) return BadRequest();

        await KiemTraTrungMa(item);
        if (!ModelState.IsValid) return View(ThuMucView + "Form.cshtml", item);

        Db.Update(item);
        await Db.SaveChangesAsync();
        TempData["ThanhCong"] = $"Đã cập nhật \"{item.Ten}\".";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await Db.Set<T>().FindAsync(id);
        if (item is null) return NotFound();

        try
        {
            Db.Remove(item);
            await Db.SaveChangesAsync();
            TempData["ThanhCong"] = $"Đã xóa \"{item.Ten}\".";
        }
        catch (DbUpdateException)
        {
            // Khóa ngoại Restrict: danh mục đang được hồ sơ tuyển dụng sử dụng
            TempData["Loi"] = $"Không thể xóa \"{item.Ten}\" vì đang được sử dụng trong hồ sơ tuyển dụng. Hãy bỏ chọn \"Đang sử dụng\" thay vì xóa.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task KiemTraTrungMa(T item)
    {
        if (await Db.Set<T>().AnyAsync(x => x.Ma == item.Ma && x.Id != item.Id))
            ModelState.AddModelError(nameof(DanhMucBase.Ma), $"Mã \"{item.Ma}\" đã tồn tại.");
    }
}
