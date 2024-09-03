# CapstoneAuctioneer
# Hướng dẫn cập nhật cơ sở dữ liệu với `Update-Database`

## 1. Mở Visual Studio

- Khởi động Visual Studio và mở dự án mà bạn muốn cập nhật cơ sở dữ liệu.
## 2. Cập nhật chuỗi kết nối cơ sở dữ liệu

- Trước khi chạy lệnh `Update-Database`, bạn cần đảm bảo rằng chuỗi kết nối cơ sở dữ liệu trong file `appsettings.json` đã được cấu hình đúng.

### Bước 1: Mở file `appsettings.json`

- Tìm và mở file `appsettings.json` trong dự án của bạn.

### Bước 2: Thay đổi chuỗi kết nối

- Tìm phần cấu hình `"ConnectionStrings"` và cập nhật chuỗi kết nối cho cơ sở dữ liệu của bạn. Ví dụ:

  ```json
  {
    "ConnectionStrings": {
      "Capstone": "Data Source=your_server_name;Initial Catalog=CapstoneAuctioneer;Integrated Security=True; TrustServerCertificate=True"
    }
  }

## 3. Mở Package Manager Console

- Để mở **Package Manager Console**, bạn thực hiện các bước sau:
  1. Trong Visual Studio, chọn `Tools` trên thanh công cụ.
  2. Chọn `NuGet Package Manager`.
  3. Chọn `Package Manager Console`.

  Hoặc bạn có thể dùng phím tắt:
  - Nhấn tổ hợp phím `Ctrl+Q` để mở thanh tìm kiếm.
  - Gõ `Package Manager Console` và chọn kết quả tương ứng.

## 4. Chạy câu lệnh `Update-Database`

- Trong **Package Manager Console**, gõ lệnh sau và nhấn `Enter`:
  ```powershell
  Update-Database

