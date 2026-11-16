using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyTuyenDung.Web.Services;
using QuanLyTuyenDung.Web.ViewModels;

namespace QuanLyTuyenDung.Web.Controllers;

public class HomeController(IThongKeService thongKeService) : Controller
{
    public async Task<IActionResult> Index() => View(await thongKeService.TongQuanAsync());

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
