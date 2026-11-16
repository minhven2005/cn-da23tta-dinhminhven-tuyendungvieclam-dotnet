using QuanLyTuyenDung.Web.Controllers.Base;
using QuanLyTuyenDung.Web.Data;
using QuanLyTuyenDung.Web.Models.Entities;

namespace QuanLyTuyenDung.Web.Controllers;

public class ViTriTuyenDungController(AppDbContext db) : DanhMucController<ViTriTuyenDung>(db)
{
    protected override string TieuDe => "Vị trí tuyển dụng";
    protected override string BieuTuong => "bi-person-badge";
}
