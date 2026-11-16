using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuanLyTuyenDung.Web.Helpers;

public static class EnumExtensions
{
    /// <summary>Lấy tên hiển thị (thuộc tính [Display]) của giá trị enum.</summary>
    public static string TenHienThi(this Enum value)
        => value.GetType().GetMember(value.ToString()).FirstOrDefault()
               ?.GetCustomAttribute<DisplayAttribute>()?.GetName()
           ?? value.ToString();
}
