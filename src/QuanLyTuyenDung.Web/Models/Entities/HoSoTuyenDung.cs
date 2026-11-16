using System.ComponentModel.DataAnnotations;
using QuanLyTuyenDung.Web.Models.Enums;

namespace QuanLyTuyenDung.Web.Models.Entities;

/// <summary>Hồ sơ tuyển dụng mà doanh nghiệp gửi trong một đợt; mỗi hồ sơ có nhiều vị trí (chi tiết).</summary>
public class HoSoTuyenDung
{
    public int Id { get; set; }

    [Display(Name = "Mã hồ sơ")]
    [Required(ErrorMessage = "Vui lòng nhập mã hồ sơ")]
    [StringLength(20)]
    public string MaHoSo { get; set; } = string.Empty;

    [Display(Name = "Doanh nghiệp")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn doanh nghiệp")]
    public int DoanhNghiepId { get; set; }
    public DoanhNghiep? DoanhNghiep { get; set; }

    [Display(Name = "Đợt")]
    [Range(1, 12, ErrorMessage = "Đợt từ 1 đến 12")]
    public int DotTuyenDung { get; set; } = 1;

    [Display(Name = "Năm")]
    [Range(2000, 2100, ErrorMessage = "Năm không hợp lệ")]
    public int Nam { get; set; } = DateTime.Now.Year;

    [Display(Name = "Ngày tiếp nhận")]
    [DataType(DataType.Date)]
    public DateTime NgayTiepNhan { get; set; } = DateTime.Today;

    [Display(Name = "Hạn nộp hồ sơ")]
    [DataType(DataType.Date)]
    public DateTime? HanNopHoSo { get; set; }

    [Display(Name = "Trạng thái")]
    public TrangThaiHoSo TrangThai { get; set; } = TrangThaiHoSo.MoiTiepNhan;

    [Display(Name = "Ghi chú")]
    [StringLength(1000)]
    public string? GhiChu { get; set; }

    public ICollection<ChiTietTuyenDung> ChiTiets { get; set; } = new List<ChiTietTuyenDung>();
}
