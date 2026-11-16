using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyTuyenDung.Web.Models.Entities;
using QuanLyTuyenDung.Web.Models.Enums;

namespace QuanLyTuyenDung.Web.Data;

/// <summary>Tạo dữ liệu mẫu khi cơ sở dữ liệu còn trống.</summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.NguoiDungs.AnyAsync())
        {
            var admin = new NguoiDung { TenDangNhap = "admin", HoTen = "Cán bộ Trung tâm DVVL", VaiTro = "Admin" };
            admin.MatKhauHash = new PasswordHasher<NguoiDung>().HashPassword(admin, "admin123");
            db.NguoiDungs.Add(admin);
        }

        if (await db.NganhNghes.AnyAsync())
        {
            await db.SaveChangesAsync();
            return;
        }

        var nganh = TaoDanhMuc<NganhNghe>(
            ("NN01", "Công nghệ thông tin"), ("NN02", "Kế toán - Kiểm toán"), ("NN03", "Quản trị kinh doanh"),
            ("NN04", "Kỹ thuật điện - điện tử"), ("NN05", "Nông nghiệp - Thủy sản"), ("NN06", "Du lịch - Nhà hàng - Khách sạn"));
        var viTri = TaoDanhMuc<ViTriTuyenDung>(
            ("VT01", "Lập trình viên .NET"), ("VT02", "Kiểm thử phần mềm"), ("VT03", "Nhân viên kế toán"),
            ("VT04", "Nhân viên kinh doanh"), ("VT05", "Kỹ thuật viên điện"), ("VT06", "Thực tập sinh"));
        var kienThuc = TaoDanhMuc<KienThuc>(
            ("KT01", "Lập trình hướng đối tượng"), ("KT02", "Cơ sở dữ liệu SQL Server"),
            ("KT03", "Nguyên lý kế toán"), ("KT04", "Mạch điện cơ bản"), ("KT05", "Marketing căn bản"));
        var kyNang = TaoDanhMuc<KyNang>(
            ("KN01", "Làm việc nhóm"), ("KN02", "Giao tiếp"), ("KN03", "Tiếng Anh giao tiếp"),
            ("KN04", "Tin học văn phòng"), ("KN05", "Giải quyết vấn đề"));
        var chungChi = TaoDanhMuc<ChungChi>(
            ("CC01", "TOEIC 450+"), ("CC02", "Chứng chỉ Ứng dụng CNTT cơ bản"), ("CC03", "Chứng chỉ kế toán trưởng"), ("CC04", "IELTS 5.5+"));
        var bangCap = TaoDanhMuc<BangCap>(
            ("BC01", "Trung cấp"), ("BC02", "Cao đẳng"), ("BC03", "Đại học"), ("BC04", "Thạc sĩ"));

        db.AddRange(nganh); db.AddRange(viTri); db.AddRange(kienThuc);
        db.AddRange(kyNang); db.AddRange(chungChi); db.AddRange(bangCap);

        // Doanh nghiệp mẫu (tên giả định, dùng cho demo)
        var dn = new List<DoanhNghiep>
        {
            new() { TenDoanhNghiep = "Công ty TNHH Phần mềm Cửu Long", MaSoThue = "1500123456", DiaChiTruSo = "12 Phạm Thái Bường, Phường 4", TinhThanh = "Vĩnh Long", NguoiLienHe = "Nguyễn Văn An", SoDienThoai = "0270 3822 111", Email = "hr@cuulongsoft.vn" },
            new() { TenDoanhNghiep = "Công ty CP Thủy sản Mekong Xanh", MaSoThue = "2100234567", DiaChiTruSo = "Khu công nghiệp Long Đức", TinhThanh = "Trà Vinh", NguoiLienHe = "Trần Thị Bình", SoDienThoai = "0294 3855 222", Email = "tuyendung@mekongxanh.vn" },
            new() { TenDoanhNghiep = "Công ty TNHH Kế toán Phương Nam", MaSoThue = "0301345678", DiaChiTruSo = "45 Nguyễn Đình Chiểu, Quận 3", TinhThanh = "TP. Hồ Chí Minh", NguoiLienHe = "Lê Minh Châu", SoDienThoai = "028 3930 333", Email = "nhansu@phuongnam-acc.vn" },
            new() { TenDoanhNghiep = "Công ty CP Điện lực Tây Đô", MaSoThue = "1800456789", DiaChiTruSo = "88 Trần Hưng Đạo, Ninh Kiều", TinhThanh = "Cần Thơ", NguoiLienHe = "Phạm Quốc Dũng", SoDienThoai = "0292 3811 444", Email = "hr@taydopower.vn" },
            new() { TenDoanhNghiep = "Khách sạn Sông Tiền", MaSoThue = "1200567890", DiaChiTruSo = "5 Lê Lợi, Phường 1", TinhThanh = "Tiền Giang", NguoiLienHe = "Võ Thị Em", SoDienThoai = "0273 3877 555", TinhTrang = TinhTrangHoatDong.TamNgung },
        };
        db.DoanhNghieps.AddRange(dn);

        var nam = DateTime.Today.Year;
        var hoSo = new List<HoSoTuyenDung>
        {
            TaoHoSo("HS001", dn[0], 1, new DateTime(nam, 1, 10), TrangThaiHoSo.DaDangThongBao,
                TaoChiTiet(nganh[0], viTri[0], bangCap[2], 3, "12 - 18 triệu", [kienThuc[0], kienThuc[1]], [kyNang[0], kyNang[2]], [chungChi[0]]),
                TaoChiTiet(nganh[0], viTri[1], bangCap[1], 2, "9 - 12 triệu", [kienThuc[1]], [kyNang[0], kyNang[4]], [])),
            TaoHoSo("HS002", dn[1], 1, new DateTime(nam, 2, 15), TrangThaiHoSo.DaDuyet,
                TaoChiTiet(nganh[4], viTri[5], bangCap[1], 10, "Thỏa thuận", [], [kyNang[0]], []),
                TaoChiTiet(nganh[1], viTri[2], bangCap[2], 1, "8 - 10 triệu", [kienThuc[2]], [kyNang[3]], [chungChi[1]])),
            TaoHoSo("HS003", dn[2], 1, new DateTime(nam, 3, 5), TrangThaiHoSo.DaDangThongBao,
                TaoChiTiet(nganh[1], viTri[2], bangCap[2], 4, "10 - 15 triệu", [kienThuc[2]], [kyNang[3], kyNang[1]], [chungChi[2]])),
            TaoHoSo("HS004", dn[3], 1, new DateTime(nam, 3, 20), TrangThaiHoSo.MoiTiepNhan,
                TaoChiTiet(nganh[3], viTri[4], bangCap[0], 6, "8 - 11 triệu", [kienThuc[3]], [kyNang[0]], [])),
            TaoHoSo("HS005", dn[0], 2, new DateTime(nam, 6, 12), TrangThaiHoSo.DaDuyet,
                TaoChiTiet(nganh[0], viTri[0], bangCap[2], 2, "15 - 20 triệu", [kienThuc[0], kienThuc[1]], [kyNang[2], kyNang[4]], [chungChi[3]])),
            TaoHoSo("HS006", dn[2], 2, new DateTime(nam, 8, 1), TrangThaiHoSo.MoiTiepNhan,
                TaoChiTiet(nganh[2], viTri[3], bangCap[1], 5, "7 triệu + hoa hồng", [kienThuc[4]], [kyNang[1]], [])),
            TaoHoSo("HS007", dn[4], 1, new DateTime(nam - 1, 11, 18), TrangThaiHoSo.DaDangThongBao,
                TaoChiTiet(nganh[5], viTri[5], bangCap[0], 8, "6 - 8 triệu", [], [kyNang[1], kyNang[2]], [chungChi[0]])),
            TaoHoSo("HS008", dn[1], 2, new DateTime(nam - 1, 9, 9), TrangThaiHoSo.TuChoi,
                TaoChiTiet(nganh[4], viTri[3], bangCap[1], 3, "Thỏa thuận", [kienThuc[4]], [kyNang[1]], [])),
        };
        db.HoSoTuyenDungs.AddRange(hoSo);

        await db.SaveChangesAsync();
    }

    private static List<T> TaoDanhMuc<T>(params (string Ma, string Ten)[] items) where T : DanhMucBase, new()
        => items.Select(i => new T { Ma = i.Ma, Ten = i.Ten }).ToList();

    private static HoSoTuyenDung TaoHoSo(string ma, DoanhNghiep dn, int dot, DateTime ngay, TrangThaiHoSo trangThai,
        params ChiTietTuyenDung[] chiTiets)
        => new()
        {
            MaHoSo = ma, DoanhNghiep = dn, DotTuyenDung = dot, Nam = ngay.Year, NgayTiepNhan = ngay,
            HanNopHoSo = ngay.AddDays(30), TrangThai = trangThai, ChiTiets = chiTiets.ToList()
        };

    private static ChiTietTuyenDung TaoChiTiet(NganhNghe nganh, ViTriTuyenDung viTri, BangCap bangCap, int soLuong, string luong,
        List<KienThuc> kienThucs, List<KyNang> kyNangs, List<ChungChi> chungChis)
        => new()
        {
            NganhNghe = nganh, ViTriTuyenDung = viTri, BangCap = bangCap, SoLuong = soLuong, MucLuong = luong,
            KienThucs = kienThucs, KyNangs = kyNangs, ChungChis = chungChis
        };
}
