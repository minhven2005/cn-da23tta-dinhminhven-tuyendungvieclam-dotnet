using System.ComponentModel.DataAnnotations;

namespace QuanLyTuyenDung.Web.Models.Enums;

public enum TrangThaiHoSo
{
    [Display(Name = "Mới tiếp nhận")] MoiTiepNhan = 1,
    [Display(Name = "Đã duyệt")] DaDuyet = 2,
    [Display(Name = "Đã đăng thông báo")] DaDangThongBao = 3,
    [Display(Name = "Từ chối")] TuChoi = 4
}
