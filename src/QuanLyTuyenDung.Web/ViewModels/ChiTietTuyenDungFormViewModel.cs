using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyTuyenDung.Web.ViewModels;

/// <summary>Form thêm/sửa một vị trí tuyển dụng trong hồ sơ.</summary>
public class ChiTietTuyenDungFormViewModel
{
    public int Id { get; set; }
    public int HoSoTuyenDungId { get; set; }
    public string? MaHoSo { get; set; }
    public string? TenDoanhNghiep { get; set; }

    [Display(Name = "Ngành nghề")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ngành nghề")]
    public int NganhNgheId { get; set; }

    [Display(Name = "Vị trí tuyển dụng")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vị trí")]
    public int ViTriTuyenDungId { get; set; }

    [Display(Name = "Bằng cấp tối thiểu")]
    public int? BangCapId { get; set; }

    [Display(Name = "Số lượng")]
    [Range(1, 10000, ErrorMessage = "Số lượng từ 1 đến 10000")]
    public int SoLuong { get; set; } = 1;

    [Display(Name = "Mức lương")]
    [StringLength(100)]
    public string? MucLuong { get; set; }

    [Display(Name = "Mô tả công việc")]
    [StringLength(2000)]
    public string? MoTaCongViec { get; set; }

    public List<int> KienThucIds { get; set; } = [];
    public List<int> KyNangIds { get; set; } = [];
    public List<int> ChungChiIds { get; set; } = [];

    // Dữ liệu cho các ô chọn trên form
    public IEnumerable<SelectListItem> DsNganhNghe { get; set; } = [];
    public IEnumerable<SelectListItem> DsViTri { get; set; } = [];
    public IEnumerable<SelectListItem> DsBangCap { get; set; } = [];
    public IEnumerable<SelectListItem> DsKienThuc { get; set; } = [];
    public IEnumerable<SelectListItem> DsKyNang { get; set; } = [];
    public IEnumerable<SelectListItem> DsChungChi { get; set; } = [];
}
