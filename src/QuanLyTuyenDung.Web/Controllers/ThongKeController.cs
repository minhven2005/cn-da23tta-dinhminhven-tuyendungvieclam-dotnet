using Microsoft.AspNetCore.Mvc;
using QuanLyTuyenDung.Web.Services;

namespace QuanLyTuyenDung.Web.Controllers;

public class ThongKeController(IThongKeService thongKeService) : Controller
{
    public async Task<IActionResult> Index(int? nam)
        => View(await thongKeService.ThongKeAsync(nam ?? DateTime.Today.Year));
}
