using System.ComponentModel.DataAnnotations;

namespace QuanLyTuyenDung.Web.Models.Entities;

/// <summary>Một vị trí tuyển dụng trong hồ sơ, kèm các yêu cầu về bằng cấp, kiến thức, kỹ năng, chứng chỉ.</summary>
public class ChiTietTuyenDung
{
    public int Id { get; set; }

    public int HoSoTuyenDungId { get; set; }
    public HoSoTuyenDung? HoSoTuyenDung { get; set; }

    [Display(Name = "Ngành nghề")]
    public int NganhNgheId { get; set; }
    public NganhNghe? NganhNghe { get; set; }

    [Display(Name = "Vị trí")]
    public int ViTriTuyenDungId { get; set; }
    public ViTriTuyenDung? ViTriTuyenDung { get; set; }

    [Display(Name = "Bằng cấp tối thiểu")]
    public int? BangCapId { get; set; }
    public BangCap? BangCap { get; set; }

    [Display(Name = "Số lượng")]
    [Range(1, 10000)]
    public int SoLuong { get; set; } = 1;

    [Display(Name = "Mức lương")]
    [StringLength(100)]
    public string? MucLuong { get; set; }

    [Display(Name = "Mô tả công việc")]
    [StringLength(2000)]
    public string? MoTaCongViec { get; set; }

    public ICollection<KienThuc> KienThucs { get; set; } = new List<KienThuc>();
    public ICollection<KyNang> KyNangs { get; set; } = new List<KyNang>();
    public ICollection<ChungChi> ChungChis { get; set; } = new List<ChungChi>();
}
