using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class BangCapController(AppDbContext db) : DanhMucController<BangCap>(db)
{
    protected override string TieuDe => "Bằng cấp";
    protected override string BieuTuong => "bi-mortarboard";
}
