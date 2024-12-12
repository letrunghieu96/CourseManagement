# Quản lý khóa học



## Mô tả source

Source chính bao gồm: Web, Domain, Sql
- Web: Chứa view, view model, controller, service hỗ trợ controller
- Domain: Chứa model, repository thao tác dữ liệu
- Sql: Chứa script sql tạo bảng và các câu truy vấn

[Tải source tại đây](https://github.com/letrunghieu96/CourseManagement)

## Tool
- Visual studio 2022
- Sql server

## Chuẩn bị
- Sau khi tải source thành công, mở bằng Visual studio 2022 sau đó Build kiểm tra có phát sinh lỗi hoặc cảnh báo nào không
- Tạo database SQL server từ các file script có trong dự án, theo tứ tự sau:
1. CreateDatabase.sql
2. CreateTables.sql
3. CreateSeedData.sql

***
# Các chức năng cơ bản của đồ án:

## Đăng ký tài khoản
- Người dùng có thể tạo tài khoản mới bằng cách nhập thông tin như tên, email, và mật khẩu.

## Đăng nhập
- Người dùng đăng nhập vào hệ thống bằng email và mật khẩu.

## Trang chủ
- Hiển thị danh sách các khóa học nổi bật và danh mục khóa học cơ bản.  

## Xem chi tiết khóa học
- Người dùng có thể xem thông tin chi tiết của từng khóa học như tên khóa học, giảng viên, và nội dung chính. 

## Tìm kiếm khóa học
- Tìm kiếm khóa học bằng thanh tìm kiếm theo từ khóa. 

## Đăng ký khóa học
- Người dùng đăng ký tham gia khóa học.

## Quản lý tài khoản cá nhân
- Người dùng có thể cập nhật thông tin cá nhân như tên và mật khẩu.

## Quản trị khóa học (dành cho quản trị viên)
- Thêm, chỉnh sửa hoặc xóa khóa học.
