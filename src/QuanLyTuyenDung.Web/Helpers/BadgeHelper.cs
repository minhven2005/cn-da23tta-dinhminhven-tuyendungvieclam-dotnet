using QuanLyTuyenDung.Web.Models.Enums;

namespace QuanLyTuyenDung.Web.Helpers;

/// <summary>Chọn lớp CSS cho nhãn trạng thái trên giao diện.</summary>
public static class BadgeHelper
{
    public static string Css(TinhTrangHoatDong tinhTrang) => tinhTrang switch
    {
        TinhTrangHoatDong.DangHoatDong => "nhan nhan-xanh",
        TinhTrangHoatDong.TamNgung => "nhan nhan-vang",
        _ => "nhan nhan-do"
    };

    public static string Css(TrangThaiHoSo trangThai) => trangThai switch
    {
        TrangThaiHoSo.MoiTiepNhan => "nhan nhan-lam",
        TrangThaiHoSo.DaDuyet => "nhan nhan-tim",
        TrangThaiHoSo.DaDangThongBao => "nhan nhan-xanh",
        _ => "nhan nhan-do"
    };
}
