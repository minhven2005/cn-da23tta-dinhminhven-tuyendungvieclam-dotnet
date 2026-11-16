using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class NganhNgheController(AppDbContext db) : DanhMucController<NganhNghe>(db)
{
    protected override string TieuDe => "Ngành nghề tuyển dụng";
    protected override string BieuTuong => "bi-diagram-3";
}
