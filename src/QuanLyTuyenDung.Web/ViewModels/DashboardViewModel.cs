using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.ViewModels;

public class DashboardViewModel
{
    public int SoDoanhNghiep { get; set; }
    public int SoDoanhNghiepHoatDong { get; set; }
    public int SoHoSoTrongNam { get; set; }
    public int SoViTriTrongNam { get; set; }
    public int TongNhuCauTrongNam { get; set; }
    public int SoHoSoChoDuyet { get; set; }
    public List<HoSoTuyenDung> HoSoMoiNhat { get; set; } = [];
    public List<ThongKeThangItem> TheoThang { get; set; } = [];
}
