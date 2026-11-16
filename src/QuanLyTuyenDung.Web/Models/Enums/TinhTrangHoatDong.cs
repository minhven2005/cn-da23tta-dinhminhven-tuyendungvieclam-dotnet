using System.ComponentModel.DataAnnotations;

namespace QuanLyTuyenDung.Web.Models.Enums;

public enum TinhTrangHoatDong
{
    [Display(Name = "Đang hoạt động")] DangHoatDong = 1,
    [Display(Name = "Tạm ngừng hoạt động")] TamNgung = 2,
    [Display(Name = "Ngừng hoạt động")] NgungHoatDong = 3
}
