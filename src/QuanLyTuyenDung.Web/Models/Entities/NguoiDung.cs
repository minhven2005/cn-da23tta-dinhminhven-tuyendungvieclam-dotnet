using System.ComponentModel.DataAnnotations;

namespace QuanLyTuyenDung.Web.Models.Entities;

/// <summary>Tài khoản cán bộ trung tâm đăng nhập hệ thống.</summary>
public class NguoiDung
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required]
    public string MatKhauHash { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    [StringLength(20)]
    public string VaiTro { get; set; } = "CanBo";
}
