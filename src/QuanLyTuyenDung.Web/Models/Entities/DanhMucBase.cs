using System.ComponentModel.DataAnnotations;

namespace QuanLyTuyenDung.Web.Models.Entities;

/// <summary>
/// Lớp cơ sở cho các danh mục dùng chung (ngành nghề, vị trí, kiến thức, kỹ năng, chứng chỉ, bằng cấp).
/// Mỗi lớp con được ánh xạ thành một bảng riêng trong SQL Server.
/// </summary>
public abstract class DanhMucBase
{
    public int Id { get; set; }

    [Display(Name = "Mã")]
    [Required(ErrorMessage = "Vui lòng nhập mã")]
    [StringLength(20, ErrorMessage = "Mã tối đa 20 ký tự")]
    public string Ma { get; set; } = string.Empty;

    [Display(Name = "Tên")]
    [Required(ErrorMessage = "Vui lòng nhập tên")]
    [StringLength(200, ErrorMessage = "Tên tối đa 200 ký tự")]
    public string Ten { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    [StringLength(500)]
    public string? MoTa { get; set; }

    [Display(Name = "Đang sử dụng")]
    public bool ConSuDung { get; set; } = true;
}
