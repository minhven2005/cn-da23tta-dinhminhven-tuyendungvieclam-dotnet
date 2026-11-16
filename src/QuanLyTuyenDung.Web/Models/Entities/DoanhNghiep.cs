using System.ComponentModel.DataAnnotations;
using QuanLyTuyenDung.Web.Models.Enums;

namespace QuanLyTuyenDung.Web.Models.Entities;

public class DoanhNghiep
{
    public int Id { get; set; }

    [Display(Name = "Tên doanh nghiệp")]
    [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp")]
    [StringLength(200)]
    public string TenDoanhNghiep { get; set; } = string.Empty;

    [Display(Name = "Mã số thuế")]
    [Required(ErrorMessage = "Vui lòng nhập mã số thuế")]
    [RegularExpression(@"^\d{10}(-\d{3})?$", ErrorMessage = "Mã số thuế gồm 10 chữ số (hoặc 10 số + '-' + 3 số)")]
    public string MaSoThue { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ trụ sở")]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ trụ sở")]
    [StringLength(300)]
    public string DiaChiTruSo { get; set; } = string.Empty;

    [Display(Name = "Tỉnh/Thành phố")]
    [StringLength(100)]
    public string? TinhThanh { get; set; }

    [Display(Name = "Người liên hệ")]
    [StringLength(100)]
    public string? NguoiLienHe { get; set; }

    [Display(Name = "Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Display(Name = "Website")]
    [StringLength(200)]
    public string? Website { get; set; }

    [Display(Name = "Tình trạng hoạt động")]
    public TinhTrangHoatDong TinhTrang { get; set; } = TinhTrangHoatDong.DangHoatDong;

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;

    public ICollection<HoSoTuyenDung> HoSoTuyenDungs { get; set; } = new List<HoSoTuyenDung>();
}
