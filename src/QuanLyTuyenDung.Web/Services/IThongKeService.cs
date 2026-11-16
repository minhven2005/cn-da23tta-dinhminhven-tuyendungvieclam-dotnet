using QuanLyTuyenDung.Web.ViewModels;

namespace QuanLyTuyenDung.Web.Services;

public interface IThongKeService
{
    Task<ThongKeViewModel> ThongKeAsync(int nam);
    Task<DashboardViewModel> TongQuanAsync();
}
