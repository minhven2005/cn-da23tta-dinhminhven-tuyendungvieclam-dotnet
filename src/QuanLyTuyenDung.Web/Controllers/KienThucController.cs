using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class KienThucController(AppDbContext db) : DanhMucController<KienThuc>(db)
{
    protected override string TieuDe => "Yêu cầu kiến thức";
    protected override string BieuTuong => "bi-book";
}
