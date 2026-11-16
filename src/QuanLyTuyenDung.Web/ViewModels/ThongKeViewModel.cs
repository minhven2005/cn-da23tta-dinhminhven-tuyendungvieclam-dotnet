namespace QuanLyTuyenDung.Web.ViewModels;

public class ThongKeViewModel
{
    public int Nam { get; set; }
    public List<int> DanhSachNam { get; set; } = [];
    public List<ThongKeThangItem> TheoThang { get; set; } = [];
    public List<ThongKeNamItem> TheoNam { get; set; } = [];
    public List<ThongKeNganhItem> TheoNganh { get; set; } = [];
    public List<NhaTuyenDungItem> NhaTuyenDung { get; set; } = [];
}

public class ThongKeThangItem
{
    public int Thang { get; set; }
    public int SoNhaTuyenDung { get; set; }
    public int SoHoSo { get; set; }
    public int SoViTri { get; set; }
    public int TongSoLuong { get; set; }
}

public class ThongKeNamItem
{
    public int Nam { get; set; }
    public int SoNhaTuyenDung { get; set; }
    public int SoHoSo { get; set; }
    public int SoViTri { get; set; }
    public int TongSoLuong { get; set; }
}

public class ThongKeNganhItem
{
    public string TenNganh { get; set; } = string.Empty;
    public int SoViTri { get; set; }
    public int TongSoLuong { get; set; }
}

public class NhaTuyenDungItem
{
    public int DoanhNghiepId { get; set; }
    public string TenDoanhNghiep { get; set; } = string.Empty;
    public string? TinhThanh { get; set; }
    public int SoHoSo { get; set; }
    public int SoViTri { get; set; }
    public int TongSoLuong { get; set; }
}
