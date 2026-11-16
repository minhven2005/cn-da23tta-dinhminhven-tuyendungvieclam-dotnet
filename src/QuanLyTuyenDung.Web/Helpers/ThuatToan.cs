namespace QuanLyTuyenDung.Web.Helpers;

/// <summary>
/// Cài đặt các giải thuật sắp xếp và tìm kiếm dùng trong ứng dụng
/// (vận dụng kiến thức Cấu trúc dữ liệu và giải thuật theo đề cương).
/// </summary>
public static class ThuatToan
{
    /// <summary>Sắp xếp nhanh (Quick Sort) trên danh sách, theo hàm so sánh truyền vào.</summary>
    public static void QuickSort<T>(IList<T> ds, Comparison<T> soSanh)
        => QuickSort(ds, 0, ds.Count - 1, soSanh);

    private static void QuickSort<T>(IList<T> ds, int trai, int phai, Comparison<T> soSanh)
    {
        if (trai >= phai) return;

        var chot = ds[(trai + phai) / 2];
        int i = trai, j = phai;
        while (i <= j)
        {
            while (soSanh(ds[i], chot) < 0) i++;
            while (soSanh(ds[j], chot) > 0) j--;
            if (i <= j)
            {
                (ds[i], ds[j]) = (ds[j], ds[i]);
                i++;
                j--;
            }
        }

        QuickSort(ds, trai, j, soSanh);
        QuickSort(ds, i, phai, soSanh);
    }

    /// <summary>
    /// Tìm kiếm nhị phân trên danh sách đã sắp xếp theo khóa.
    /// Trả về vị trí phần tử, hoặc -1 nếu không tìm thấy.
    /// </summary>
    public static int TimKiemNhiPhan<T>(IList<T> ds, string khoa, Func<T, string> layKhoa)
    {
        int trai = 0, phai = ds.Count - 1;
        while (trai <= phai)
        {
            int giua = (trai + phai) / 2;
            int ss = string.Compare(layKhoa(ds[giua]), khoa, StringComparison.OrdinalIgnoreCase);
            if (ss == 0) return giua;
            if (ss < 0) trai = giua + 1;
            else phai = giua - 1;
        }
        return -1;
    }
}
