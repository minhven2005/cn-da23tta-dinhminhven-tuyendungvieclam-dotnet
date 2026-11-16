using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class ChungChiController(AppDbContext db) : DanhMucController<ChungChi>(db)
{
    protected override string TieuDe => "Chứng chỉ";
    protected override string BieuTuong => "bi-patch-check";
}
