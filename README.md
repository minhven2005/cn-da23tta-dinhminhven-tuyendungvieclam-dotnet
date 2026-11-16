# Website quản lý doanh nghiệp tuyển dụng – Trung tâm Dịch vụ việc làm

Đồ án chuyên ngành: ASP.NET Core MVC (.NET 9) + Entity Framework Core + SQL Server.

## Chạy ứng dụng

1. Kiểm tra chuỗi kết nối trong `src/QuanLyTuyenDung.Web/appsettings.json`
   (mặc định: SQL Server cục bộ `Server=.`, CSDL `QuanLyTuyenDungDB`, đăng nhập Windows).
   Nếu dùng LocalDB, đổi thành `Server=(localdb)\\MSSQLLocalDB`.
2. (Tùy chọn) Tạo cấu trúc CSDL trước bằng cách chạy `sql/QuanLyTuyenDungDB.sql`
   trong SQL Server Management Studio. Nếu bỏ qua bước này, ứng dụng tự tạo CSDL khi khởi chạy.
3. Chạy:
   ```
   cd src/QuanLyTuyenDung.Web
   dotnet run
   ```
   Hoặc mở `QuanLyTuyenDung.sln` bằng Visual Studio và nhấn F5.
4. Lần chạy đầu, ứng dụng tự tạo dữ liệu mẫu và tài khoản: `admin` / `admin123`.

## Cấu trúc thư mục (mô hình MVC)

```
QuanLyTuyenDung.sln
src/QuanLyTuyenDung.Web/
├── Controllers/            Controller (C trong MVC) – nhận yêu cầu, gọi dữ liệu, trả View
│   ├── Base/DanhMucController.cs   Controller dùng chung cho 6 danh mục (Thêm/Sửa/Xóa/Tìm)
│   ├── NganhNghe, ViTriTuyenDung, KienThuc, KyNang, ChungChi, BangCap Controller
│   ├── DoanhNghiepController.cs    Quản lý doanh nghiệp (trụ sở, tình trạng hoạt động)
│   ├── HoSoTuyenDungController.cs  Hồ sơ theo đợt + vị trí và yêu cầu trong hồ sơ
│   ├── ThongKeController.cs        Thống kê theo tháng, năm
│   ├── AccountController.cs        Đăng nhập / đăng xuất
│   └── HomeController.cs           Trang tổng quan
├── Models/                 Model (M trong MVC) – lớp thực thể ánh xạ bảng CSDL
│   ├── Entities/           DoanhNghiep, HoSoTuyenDung, ChiTietTuyenDung, các danh mục, NguoiDung
│   └── Enums/              TinhTrangHoatDong, TrangThaiHoSo
├── ViewModels/             Dữ liệu riêng cho từng màn hình (form, thống kê, dashboard)
├── Views/                  View (V trong MVC) – giao diện Razor (.cshtml)
│   ├── Shared/             _Layout, _Sidebar, _BieuDoThang, DanhMuc/ (view dùng chung)
│   └── <TênController>/    View của từng controller
├── Data/                   AppDbContext (EF Core) và DbSeeder (dữ liệu mẫu)
├── Services/               Tầng nghiệp vụ (ThongKeService)
├── Helpers/                Giải thuật sắp xếp/tìm kiếm (ThuatToan), tiện ích hiển thị
├── wwwroot/                Tài nguyên tĩnh: css, js, thư viện Bootstrap/jQuery
├── appsettings.json        Cấu hình, chuỗi kết nối
└── Program.cs              Khởi tạo ứng dụng, đăng ký dịch vụ, định tuyến
```

## Ràng buộc nghiệp vụ đã cài đặt

- Mã số thuế doanh nghiệp là duy nhất (tránh trùng doanh nghiệp); mã danh mục, mã hồ sơ là duy nhất.
- Không xóa được doanh nghiệp đã có hồ sơ, danh mục đang được hồ sơ sử dụng (khóa ngoại `Restrict`).
- Chỉ tiếp nhận hồ sơ mới của doanh nghiệp đang hoạt động.
- Xóa hồ sơ sẽ xóa luôn các vị trí trong hồ sơ (`Cascade`).

## Ghi chú

CSDL được tạo bằng `EnsureCreated()` cho gọn. Khi cần thay đổi cấu trúc bảng về sau, nên chuyển
sang EF Core Migrations (`dotnet ef migrations add KhoiTao`) và bỏ lệnh `EnsureCreatedAsync()` trong `Program.cs`.
