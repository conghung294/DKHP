# Hệ thống Quản lý Đăng ký Học tín chỉ

Hệ thống quản lý đăng ký học tín chỉ cho trường đại học - Xây dựng trên Windows Forms (.NET Framework 4.7.2)

## Tổng quan

Hệ thống cho phép:
- Sinh viên đăng ký, hủy đăng ký học phần, xem lịch học, xem điểm
- Giảng viên quản lý lớp học phần, xem danh sách sinh viên, nhập điểm
- Quản lý học kỳ, học phần, lớp học phần, đăng ký và điểm số

## Cấu trúc dự án

```
WindowsFormsApp1/
├── Models/              # Các model classes (Student, Instructor, Course, etc.)
├── Database/            # DatabaseHelper để quản lý dữ liệu
├── Forms/               # Các form giao diện
│   ├── LoginForm.cs
│   ├── StudentMainForm.cs
│   ├── InstructorMainForm.cs
│   ├── GradesViewForm.cs
│   ├── StudentsListForm.cs
│   └── EnterGradesForm.cs
├── Properties/
└── Program.cs
```

## Thông tin đăng nhập

### Sinh viên
- **Mã đăng nhập**: SV001, SV002, SV003
- **Mật khẩu**: password

### Giảng viên
- **Mã đăng nhập**: GV01, GV02, GV03
- **Mật khẩu**: password

## Các chức năng chính

### Dành cho Sinh viên
1. **Đăng nhập** - Xác thực tài khoản sinh viên
2. **Đăng ký học phần** - Đăng ký các lớp học phần có sẵn
3. **Hủy đăng ký** - Hủy đăng ký các lớp đã đăng ký
4. **Xem lịch học** - Xem lịch học của các lớp đã đăng ký
5. **Xem điểm** - Xem điểm các môn đã học

### Dành cho Giảng viên
1. **Đăng nhập** - Xác thực tài khoản giảng viên
2. **Xem lớp đang dạy** - Danh sách các lớp học phần đang phụ trách
3. **Xem danh sách sinh viên** - Xem sinh viên trong từng lớp
4. **Nhập điểm** - Nhập điểm chuyên cần, giữa kỳ và cuối kỳ cho sinh viên
5. **Xem lịch dạy** - Xem lịch giảng dạy của mình

## Đặc điểm hệ thống

- **Kiểm tra môn tiên quyết**: Sinh viên chỉ có thể đăng ký môn có môn tiên quyết đã hoàn thành
- **Giới hạn sĩ số**: Mỗi lớp có sĩ số tối đa, hệ thống tự động kiểm tra
- **Tính điểm tự động**: Điểm tổng kết được tính tự động theo công thức:
  - 10% Điểm chuyên cần
  - 30% Điểm giữa kỳ  
  - 60% Điểm thi
- **Phân loại lớp học**: Hệ đại trà (60-80 SV) và các hệ khác (30-50 SV)

## Cơ sở dữ liệu

Hệ thống sử dụng in-memory database với các bảng:
- Khoa viện (Department)
- Chương trình đào tạo (Academic Program)
- Sinh viên (Student)
- Giảng viên (Instructor)
- Học kỳ (Semester)
- Học phần (Course)
- Lớp học phần (Course Section)
- Đăng ký (Registration)
- Điểm (Grade)

## Yêu cầu hệ thống

- Windows 7 trở lên
- .NET Framework 4.7.2
- Visual Studio 2017 trở lên (để phát triển)

## Hướng dẫn chạy

1. Mở solution trong Visual Studio
2. Build solution (F6)
3. Run (F5)
4. Đăng nhập với thông tin ở trên
5. Sử dụng hệ thống theo nhu cầu

## Lưu ý

- Hệ thống hiện sử dụng in-memory database, dữ liệu sẽ mất khi đóng ứng dụng
- Để persist dữ liệu, cần tích hợp SQL Server hoặc SQLite database
- Các chức năng quản trị hệ thống (CRUD Khoa viện, Chương trình đào tạo...) có thể được phát triển thêm

## Tác giả

Hệ thống được xây dựng theo yêu cầu bài tập lớn môn Tin học ứng dụng
Ngành: Kỹ thuật phần mềm
Năm học: 2024-2025



