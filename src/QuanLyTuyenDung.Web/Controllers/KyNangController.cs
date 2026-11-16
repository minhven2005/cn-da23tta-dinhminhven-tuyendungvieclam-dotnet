using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class KyNangController(AppDbContext db) : DanhMucController<KyNang>(db)
{
    protected override string TieuDe => "Yêu cầu kỹ năng";
    protected override string BieuTuong => "bi-lightning";
}
