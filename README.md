# Hệ thống Quản lý Đăng ký Học tín chỉ

## 📋 Mục lục

1. [Tổng quan](#tổng-quan)
2. [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
3. [Cấu trúc dự án](#cấu-trúc-dự-án)
4. [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
5. [Cài đặt và cấu hình](#cài-đặt-và-cấu-hình)
6. [Hướng dẫn sử dụng](#hướng-dẫn-sử-dụng)
7. [Cơ sở dữ liệu](#cơ-sở-dữ-liệu)
8. [Chi tiết các thành phần](#chi-tiết-các-thành-phần)
9. [Luồng hoạt động](#luồng-hoạt-động)
10. [Tác giả và giấy phép](#tác-giả-và-giấy-phép)

---

## 🎯 Tổng quan

**Hệ thống Quản lý Đăng ký Học tín chỉ** là một ứng dụng Windows Forms được xây dựng trên .NET Framework 4.7.2, phục vụ quản lý quy trình đăng ký học phần, quản lý lớp học và quản lý thông tin sinh viên, giảng viên trong môi trường đại học.

### Mục đích

Hệ thống được thiết kế để:
- **Sinh viên**: Đăng ký, hủy đăng ký học phần, xem lịch học, quản lý thông tin cá nhân
- **Giảng viên**: Quản lý lớp học phần, xem danh sách sinh viên trong lớp
- **Quản trị viên**: Quản lý toàn bộ hệ thống (sinh viên, giảng viên, môn học, lớp học phần, khoa)

### Đặc điểm nổi bật

- ✅ Giao diện hiện đại với theme thống nhất
- ✅ Kiểm tra môn tiên quyết tự động
- ✅ Kiểm tra sĩ số lớp học phần
- ✅ Phân quyền rõ ràng (Sinh viên, Giảng viên, Admin)
- ✅ Kết nối SQL Server với xử lý lỗi chi tiết
- ✅ Singleton pattern cho DatabaseHelper
- ✅ Responsive layout với sidebar và header

---

## 🏗️ Kiến trúc hệ thống

### Mô hình 3 lớp (3-Tier Architecture)

```
┌─────────────────────────────────────┐
│   PRESENTATION LAYER (Forms)        │
│   - LoginForm                        │
│   - StudentMainForm                  │
│   - InstructorMainForm               │
│   - AdminMainForm                    │
└─────────────────────────────────────┘
              ↕
┌─────────────────────────────────────┐
│   BUSINESS LOGIC LAYER              │
│   - DatabaseHelper (Singleton)      │
│   - Models (Data Transfer Objects) │
│   - ThemeHelper (UI Styling)        │
└─────────────────────────────────────┘
              ↕
┌─────────────────────────────────────┐
│   DATA ACCESS LAYER                 │
│   - SQL Server Database             │
│   - ADO.NET (SqlConnection)         │
└─────────────────────────────────────┘
```

### Design Patterns sử dụng

1. **Singleton Pattern**: `DatabaseHelper` - Đảm bảo chỉ có một instance kết nối database
2. **Repository Pattern**: `DatabaseHelper` đóng vai trò repository cho tất cả các thao tác database
3. **MVC-like Pattern**: Tách biệt Models, Views (Forms), và Controllers (DatabaseHelper)

---

## 📁 Cấu trúc dự án

```
WindowsFormsApp1/
├── Models/                          # Các lớp dữ liệu (DTO)
│   ├── Student.cs                   # Model sinh viên
│   ├── Instructor.cs                # Model giảng viên
│   ├── Course.cs                    # Model môn học
│   ├── CourseSection.cs             # Model lớp học phần
│   ├── Department.cs                # Model khoa viện
│   ├── AcademicProgram.cs           # Model chương trình đào tạo
│   ├── Semester.cs                  # Model học kỳ
│   └── Registration.cs              # Model đăng ký học phần
│
├── Database/                        # Lớp quản lý database
│   └── DatabaseHelper.cs            # Singleton class quản lý kết nối và truy vấn SQL
│
├── Forms/                           # Các form giao diện
│   ├── LoginForm.cs                 # Form đăng nhập
│   ├── StudentMainForm.cs            # Form chính của sinh viên
│   ├── InstructorMainForm.cs       # Form chính của giảng viên
│   ├── AdminMainForm.cs             # Form chính của admin
│   ├── AddEditStudentForm.cs        # Form thêm/sửa sinh viên
│   ├── AddEditInstructorForm.cs     # Form thêm/sửa giảng viên
│   ├── AddEditCourseForm.cs         # Form thêm/sửa môn học
│   ├── AddEditSectionForm.cs        # Form thêm/sửa lớp học phần
│   ├── AddEditDepartmentForm.cs     # Form thêm/sửa khoa
│   ├── StudentsListForm.cs           # Form xem danh sách sinh viên
│   └── TestConnectionForm.cs        # Form test kết nối database
│
├── UI/                              # Quản lý giao diện
│   └── ThemeHelper.cs               # Class quản lý theme, màu sắc, fonts
│
├── Properties/                      # Cấu hình assembly
│   ├── AssemblyInfo.cs
│   ├── Resources.resx
│   └── Settings.settings
│
├── Resources/                       # Tài nguyên (logo, hình ảnh)
│   └── neu.png                      # Logo trường (nếu có)
│
├── App.config                       # Cấu hình connection string
├── Program.cs                       # Entry point của ứng dụng
└── WindowsFormsApp1.csproj          # File project
```

---

## 💻 Yêu cầu hệ thống

### Phần mềm

- **Hệ điều hành**: Windows 7 trở lên
- **.NET Framework**: 4.7.2 hoặc cao hơn
- **SQL Server**: 
  - SQL Server Express 2012 trở lên, hoặc
  - SQL Server LocalDB (MSSQLLocalDB)
- **Visual Studio**: 2017 trở lên (để phát triển)

### Phần cứng

- **RAM**: Tối thiểu 2GB
- **Ổ cứng**: 500MB trống
- **Màn hình**: Độ phân giải tối thiểu 1024x768

---

## ⚙️ Cài đặt và cấu hình

### Bước 1: Cài đặt SQL Server

1. Tải và cài đặt **SQL Server Express** hoặc **SQL Server LocalDB**
2. Đảm bảo SQL Server Service đang chạy

### Bước 2: Tạo Database

Mở **SQL Server Management Studio (SSMS)** và tạo database:


### Bước 3: Cấu hình Connection String

Mở file `App.config` và chỉnh sửa connection string:

```xml
<connectionStrings>
    <add name="DefaultConnection" 
         connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=tinchi;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" />
</connectionStrings>
```

**Lưu ý**: 
- Thay `localhost\SQLEXPRESS` bằng tên server SQL Server của bạn
- Nếu dùng LocalDB, dùng: `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=tinchi;Integrated Security=True;`

### Bước 4: Build và chạy

1. Mở solution `WindowsFormsApp1.sln` trong Visual Studio
2. Build solution (F6 hoặc Build > Build Solution)
3. Chạy ứng dụng (F5 hoặc Debug > Start Debugging)

---

## 📖 Hướng dẫn sử dụng

### Đăng nhập hệ thống

#### Sinh viên
- **Mã đăng nhập**: Mã sinh viên (ví dụ: SV001, SV002)
- **Mật khẩu**: Mật khẩu được lưu trong cột `Password` của bảng `SinhVien`

#### Giảng viên
- **Mã đăng nhập**: Mã giảng viên (ví dụ: GV01, GV02)
- **Mật khẩu**: Mật khẩu được lưu trong cột `Password` của bảng `GiangVien`

#### Quản trị viên
- **Mã đăng nhập**: `admin`
- **Mật khẩu**: `admin`
- Hoặc sử dụng tài khoản từ bảng `Admin` (nếu có)

### Chức năng dành cho Sinh viên

#### 1. Trang chủ
- Hiển thị thông tin cá nhân trong card đẹp
- Logo trường (nếu có)

#### 2. Đăng ký học phần
- Xem danh sách lớp có thể đăng ký
- Kiểm tra môn tiên quyết tự động
- Kiểm tra sĩ số lớp
- Đăng ký lớp học phần

#### 3. Hủy đăng ký
- Xem danh sách lớp đã đăng ký
- Hủy đăng ký lớp học phần

#### 4. Xem lịch học
- Xem lịch học của các lớp đã đăng ký (đã bỏ)

### Chức năng dành cho Giảng viên

#### 1. Trang chủ
- Hiển thị thông tin cá nhân trong card đẹp
- Logo trường (nếu có)

#### 2. Lớp của tôi
- Xem danh sách các lớp đang giảng dạy
- Thông tin: Mã lớp, Tên học phần, Lớp, Lịch học, Sĩ số

#### 3. Xem danh sách sinh viên
- Chọn lớp học phần
- Xem danh sách sinh viên đã đăng ký lớp đó

### Chức năng dành cho Admin

#### 1. Quản lý Sinh viên
- **Thêm**: Thêm sinh viên mới
- **Sửa**: Cập nhật thông tin sinh viên
- **Xóa**: Xóa sinh viên
- **Tải lại**: Refresh danh sách

#### 2. Quản lý Giảng viên
- **Thêm**: Thêm giảng viên mới
- **Sửa**: Cập nhật thông tin giảng viên
- **Xóa**: Xóa giảng viên
- **Tải lại**: Refresh danh sách

#### 3. Quản lý Môn học
- **Thêm**: Thêm môn học mới
- **Sửa**: Cập nhật thông tin môn học
- **Xóa**: Xóa môn học
- **Tải lại**: Refresh danh sách

#### 4. Quản lý Lớp học phần
- **Thêm**: Tạo lớp học phần mới
- **Sửa**: Cập nhật thông tin lớp học phần
- **Xóa**: Xóa lớp học phần
- **Tải lại**: Refresh danh sách

#### 5. Quản lý Khoa
- **Thêm**: Thêm khoa mới
- **Sửa**: Cập nhật tên khoa
- **Xóa**: Xóa khoa
- **Tải lại**: Refresh danh sách

---

## 🗄️ Cơ sở dữ liệu

### Sơ đồ quan hệ

```
Khoa (1) ──< (N) CTDT
Khoa (1) ──< (N) GiangVien
CTDT (1) ──< (N) SinhVien
HocKi (1) ──< (N) MonHoc
MonHoc (1) ──< (N) MonHoc (Môn tiên quyết)
MonHoc (1) ──< (N) LopHocPhan
GiangVien (1) ──< (N) LopHocPhan
SinhVien (N) ──< (N) LopHocPhan (qua DangKi)
```

### Các bảng chính

#### 1. Khoa
- `MaKhoa` (PK): Mã khoa
- `TenKhoa`: Tên khoa

#### 2. CTDT (Chương trình đào tạo)
- `MaCTDT` (PK): Mã chương trình đào tạo
- `TenCTDT`: Tên chương trình đào tạo
- `MaKhoa` (FK): Mã khoa

#### 3. SinhVien
- `MaSV` (PK): Mã sinh viên
- `TenSV`: Tên sinh viên
- `NgaySinh`: Ngày sinh
- `GioiTinh`: Giới tính
- `SDT`: Số điện thoại
- `Email`: Email
- `DiaChi`: Địa chỉ
- `MaCTDT` (FK): Mã chương trình đào tạo
- `Password`: Mật khẩu đăng nhập

#### 4. GiangVien
- `MaGV` (PK): Mã giảng viên
- `TenGV`: Tên giảng viên
- `GioiTinh`: Giới tính
- `DiaChi`: Địa chỉ
- `Email`: Email
- `SDT`: Số điện thoại
- `HocVi`: Học vị
- `MaKV` (FK): Mã khoa viện
- `Password`: Mật khẩu đăng nhập

#### 5. HocKi
- `MaHocKi` (PK): Mã học kỳ
- `TenHocKi`: Tên học kỳ
- `NamHoc`: Năm học
- `NgayBatDau`: Ngày bắt đầu
- `NgayKetThuc`: Ngày kết thúc

#### 6. MonHoc
- `MaMH` (PK): Mã môn học
- `TenHocPhan`: Tên học phần
- `SoTC`: Số tín chỉ
- `MaHocPhanTienQuyet` (FK): Mã môn học tiên quyết (nullable)
- `MaHocKi` (FK): Mã học kỳ

#### 7. LopHocPhan
- `MaLHP` (PK): Mã lớp học phần
- `TenLop`: Tên lớp
- `MaHP` (FK): Mã học phần
- `MaGV` (FK): Mã giảng viên
- `SiSo`: Sĩ số tối đa
- `LichHoc`: Lịch học

#### 8. DangKi
- `MaSV` (FK, PK): Mã sinh viên
- `MaLHP` (FK, PK): Mã lớp học phần
- `HinhThuc`: Hình thức đăng ký (Kế hoạch/Học vượt)

---

## 🔧 Chi tiết các thành phần

### 1. Program.cs - Entry Point

**Mục đích**: Điểm khởi đầu của ứng dụng

**Chức năng**:
- Khởi tạo Windows Forms application
- Bật visual styles
- Mở `LoginForm` làm form đầu tiên

**Code chính**:
```csharp
[STAThread]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new LoginForm());
}
```

### 2. DatabaseHelper.cs - Quản lý Database

**Mục đích**: Singleton class quản lý tất cả thao tác với database

**Đặc điểm**:
- **Singleton Pattern**: Chỉ có một instance duy nhất
- **Connection Management**: Tự động quản lý kết nối
- **Error Handling**: Xử lý lỗi chi tiết với thông báo rõ ràng

**Các nhóm method chính**:

#### Authentication Methods
- `LoginStudent(maSV, password)`: Đăng nhập sinh viên
- `LoginInstructor(maGV, password)`: Đăng nhập giảng viên
- `LoginAdmin(username, password)`: Đăng nhập admin

#### CRUD Methods - Sinh viên
- `GetAllStudents()`: Lấy tất cả sinh viên
- `InsertStudent(student)`: Thêm sinh viên mới
- `UpdateStudent(student)`: Cập nhật thông tin sinh viên
- `DeleteStudent(maSV)`: Xóa sinh viên

#### CRUD Methods - Giảng viên
- `GetAllInstructors()`: Lấy tất cả giảng viên
- `InsertInstructor(instructor)`: Thêm giảng viên mới
- `UpdateInstructor(instructor)`: Cập nhật thông tin giảng viên
- `DeleteInstructor(maGV)`: Xóa giảng viên

#### CRUD Methods - Môn học
- `GetAllCourses()`: Lấy tất cả môn học
- `GetCoursesBySemester(maHocKi)`: Lấy môn học theo học kỳ
- `InsertCourse(course)`: Thêm môn học mới
- `UpdateCourse(course)`: Cập nhật môn học
- `DeleteCourse(maMH)`: Xóa môn học

#### CRUD Methods - Lớp học phần
- `GetAllSections()`: Lấy tất cả lớp học phần
- `GetAvailableCourseSections(maHP)`: Lấy lớp học phần còn chỗ
- `GetRegisteredSections(maSV)`: Lấy lớp đã đăng ký của sinh viên
- `GetInstructorSections(maGV)`: Lấy lớp đang dạy của giảng viên
- `InsertSection(section)`: Thêm lớp học phần mới
- `UpdateSection(section)`: Cập nhật lớp học phần
- `DeleteSection(maLHP)`: Xóa lớp học phần

#### CRUD Methods - Khoa
- `GetAllDepartments()`: Lấy tất cả khoa
- `InsertDepartment(maKhoa, tenKhoa)`: Thêm khoa mới
- `UpdateDepartment(maKhoa, tenKhoa)`: Cập nhật khoa
- `DeleteDepartment(maKhoa)`: Xóa khoa

#### Registration Methods
- `RegisterCourseSection(maSV, maLHP)`: Đăng ký lớp học phần
  - Kiểm tra đã đăng ký chưa
  - Kiểm tra sĩ số lớp
  - Kiểm tra môn tiên quyết (có thể thêm)
- `CancelRegistration(maSV, maLHP)`: Hủy đăng ký

#### Utility Methods
- `TestConnection(out message)`: Kiểm tra kết nối database
- `GetProgramName(maCTDT)`: Lấy tên chương trình đào tạo
- `GetDepartmentName(maKV)`: Lấy tên khoa
- `GetCurrentSemester()`: Lấy học kỳ hiện tại
- `GetStudentsInSection(maLHP)`: Lấy danh sách sinh viên trong lớp

### 3. ThemeHelper.cs - Quản lý Giao diện

**Mục đích**: Quản lý theme, màu sắc, fonts cho toàn bộ ứng dụng

**Các thành phần**:

#### Màu sắc
- `PrimaryBlue`: #2196F3 - Màu chủ đạo
- `HeaderBlue`: #1976D2 - Màu header
- `BackgroundLight`: #F5F5F5 - Nền nhạt
- `SuccessGreen`: #4CAF50 - Màu thành công
- `DangerRed`: #F44336 - Màu cảnh báo
- `SidebarBackground`: #F0F2F5 - Nền sidebar

#### Fonts
- `TitleFont`: Segoe UI, 20pt, Bold
- `HeaderFont`: Segoe UI, 16pt, Bold
- `NormalFont`: Segoe UI, 11pt, Regular
- `LabelFont`: Segoe UI, 10pt, Regular

#### Methods
- `ApplyButtonStyle()`: Áp dụng style cho button
- `ApplyDataGridViewStyle()`: Áp dụng style cho DataGridView
- `ApplyTextBoxStyle()`: Áp dụng style cho TextBox
- `CreateRoundedPanel()`: Tạo panel với góc bo tròn
- `CreateHeaderBar()`: Tạo header bar
- `CreateSidebar()`: Tạo sidebar

### 4. LoginForm.cs - Form Đăng nhập

**Chức năng**:
- Xác thực 3 loại tài khoản: Sinh viên, Giảng viên, Admin
- Hiển thị thông báo lỗi rõ ràng
- Chuyển hướng đến form tương ứng sau khi đăng nhập thành công

**Luồng xử lý**:
1. Người dùng nhập mã đăng nhập và mật khẩu
2. Thử đăng nhập Admin trước
3. Nếu không phải Admin, thử đăng nhập Sinh viên
4. Nếu không phải Sinh viên, thử đăng nhập Giảng viên
5. Nếu không thành công, hiển thị thông báo lỗi

### 5. StudentMainForm.cs - Form Chính Sinh viên

**Layout**:
- **Header**: Thanh header màu xanh với tiêu đề
- **Sidebar**: 
  - Card thông tin sinh viên (có logo)
  - Menu: Trang chủ, Đăng ký học phần
  - Nút đăng xuất
- **Content**: 
  - Card thông tin chi tiết
  - TabControl với 2 tab:
    - **Lớp đã đăng ký**: Danh sách lớp đã đăng ký
    - **Lớp có thể đăng ký**: Danh sách lớp có thể đăng ký
  - Buttons: Đăng ký, Hủy đăng ký

**Chức năng**:
- `LoadRegisteredCourses()`: Load lớp đã đăng ký
- `LoadAvailableCourses()`: Load lớp có thể đăng ký
- `btnRegister_Click()`: Xử lý đăng ký
  - Kiểm tra môn tiên quyết
  - Kiểm tra sĩ số
  - Kiểm tra trùng lặp
- `btnCancel_Click()`: Xử lý hủy đăng ký

### 6. InstructorMainForm.cs - Form Chính Giảng viên

**Layout**:
- **Header**: Thanh header màu xanh với tiêu đề
- **Sidebar**: 
  - Card thông tin giảng viên (có logo)
  - Menu: Trang chủ, Lớp của tôi
  - Nút đăng xuất
- **Content**: 
  - Card thông tin chi tiết
  - TabControl với 1 tab:
    - **Lớp của tôi**: Danh sách lớp đang giảng dạy
  - Button: Xem danh sách sinh viên

**Chức năng**:
- `LoadMyCourses()`: Load lớp đang giảng dạy
- `btnViewStudents_Click()`: Xem danh sách sinh viên trong lớp

### 7. AdminMainForm.cs - Form Chính Admin

**Layout**:
- **Header**: Thanh header với tiêu đề và nút đăng xuất
- **Sidebar**: 
  - Logo (nếu có)
  - Menu điều hướng:
    - Quản lý sinh viên
    - Quản lý giảng viên
    - Quản lý môn học
    - Lớp học phần
    - Khoa
- **Content**: 
  - TabControl với 5 tab tương ứng
  - Mỗi tab có:
    - ToolStrip với các nút: Thêm, Sửa, Xóa, Tải lại
    - DataGridView hiển thị dữ liệu

**Chức năng**:
- CRUD đầy đủ cho tất cả entities
- Validation dữ liệu
- Xác nhận trước khi xóa

### 8. Các Form Add/Edit

#### AddEditStudentForm.cs
- Form thêm/sửa sinh viên
- Validation: Mã SV, Tên, Ngày sinh, MaCTDT là bắt buộc
- Mã SV không thể sửa khi edit

#### AddEditInstructorForm.cs
- Form thêm/sửa giảng viên
- Validation: Mã GV, Tên, MaKV là bắt buộc

#### AddEditCourseForm.cs
- Form thêm/sửa môn học
- Validation: Mã MH, Tên, Số TC, Mã HK là bắt buộc
- Mã MH không thể sửa khi edit

#### AddEditSectionForm.cs
- Form thêm/sửa lớp học phần
- Validation: Mã LHP, Tên lớp, Mã HP, Mã GV, Sĩ số là bắt buộc

#### AddEditDepartmentForm.cs
- Form thêm/sửa khoa
- Validation: Mã khoa, Tên khoa là bắt buộc

### 9. StudentsListForm.cs

**Mục đích**: Hiển thị danh sách sinh viên trong một lớp học phần

**Chức năng**:
- Hiển thị thông tin: Mã SV, Tên SV, Ngày sinh, Giới tính, Email, SĐT
- DataGridView với style đẹp

---

## 🔄 Luồng hoạt động

### 1. Luồng đăng nhập

```
User nhập thông tin
    ↓
LoginForm.btnLogin_Click()
    ↓
DatabaseHelper.LoginXXX()
    ↓
Kiểm tra trong database
    ↓
Thành công? → Mở form tương ứng
    ↓
Thất bại? → Hiển thị thông báo lỗi
```

### 2. Luồng đăng ký học phần

```
Sinh viên chọn lớp
    ↓
Click "Đăng ký"
    ↓
Kiểm tra môn tiên quyết
    ↓
Kiểm tra sĩ số
    ↓
Kiểm tra trùng lặp
    ↓
DatabaseHelper.RegisterCourseSection()
    ↓
Thành công → Refresh danh sách
```

### 3. Luồng quản lý (Admin)

```
Admin chọn entity (Sinh viên/Giảng viên/...)
    ↓
Click "Thêm" → Mở AddEditForm
    ↓
Nhập thông tin → Click "Lưu"
    ↓
DatabaseHelper.InsertXXX()
    ↓
Thành công → Refresh DataGridView
```

---

## 🎨 Giao diện

### Theme chính

- **Màu chủ đạo**: Xanh dương (#2196F3)
- **Header**: Xanh đậm (#1976D2)
- **Sidebar**: Xám nhạt (#F0F2F5)
- **Background**: Trắng/Xám nhạt (#F5F5F5)

### Layout

- **Header Bar**: 60px chiều cao, màu xanh đậm
- **Sidebar**: 260px chiều rộng, màu xám nhạt
- **Content Panel**: Phần còn lại, màu trắng

### Responsive

- Form tự động resize khi thay đổi kích thước
- ContentPanel tự động điều chỉnh để không bị sidebar che
- DataGridView tự động resize theo kích thước form

---

## 🔒 Bảo mật

### Hiện tại

- Mật khẩu lưu dạng plain text trong database
- Xác thực đơn giản qua username/password

### Khuyến nghị cải thiện

- Mã hóa mật khẩu (BCrypt, SHA256)
- Session management
- Logging các thao tác quan trọng
- Input validation và sanitization
- SQL injection protection (đã dùng parameterized queries)

---

## 🐛 Xử lý lỗi

### Database Connection Errors

- Hiển thị thông báo lỗi chi tiết
- Hướng dẫn khắc phục cụ thể
- Test connection trước khi sử dụng

### Validation Errors

- Kiểm tra dữ liệu đầu vào
- Hiển thị thông báo lỗi rõ ràng
- Không cho phép submit nếu dữ liệu không hợp lệ

### Exception Handling

- Try-catch cho tất cả thao tác database
- Hiển thị MessageBox với thông báo lỗi
- Logging (có thể thêm sau)

---

## 📝 Ghi chú kỹ thuật

### Singleton Pattern trong DatabaseHelper

```csharp
private static DatabaseHelper _instance;
public static DatabaseHelper Instance
{
    get
    {
        if (_instance == null)
            _instance = new DatabaseHelper();
        return _instance;
    }
}
```

**Lợi ích**:
- Đảm bảo chỉ có một kết nối database
- Tiết kiệm tài nguyên
- Dễ quản lý connection string

### Parameterized Queries

Tất cả các truy vấn SQL đều sử dụng parameters để tránh SQL injection:

```csharp
command.Parameters.AddWithValue("@MaSV", maSV);
```

### Using Statements

Sử dụng `using` để tự động dispose resources:

```csharp
using (var connection = new SqlConnection(connectionString))
{
    // Code
} // Tự động đóng connection
```

---

## 🚀 Tính năng nâng cao (Có thể phát triển)

1. **Xuất báo cáo**: Export danh sách sinh viên, lớp học phần ra Excel/PDF
2. **Thống kê**: Thống kê số lượng đăng ký, tỷ lệ đầy lớp
3. **Tìm kiếm và lọc**: Tìm kiếm sinh viên, lớp học phần
4. **Phân quyền chi tiết**: Phân quyền theo chức năng cụ thể
5. **Backup/Restore**: Sao lưu và khôi phục dữ liệu
6. **Audit Log**: Ghi log các thao tác quan trọng
7. **Email notifications**: Gửi email thông báo
8. **Mobile app**: Ứng dụng di động cho sinh viên

---

## 📚 Tài liệu tham khảo

- [.NET Framework Documentation](https://docs.microsoft.com/en-us/dotnet/framework/)
- [Windows Forms Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/winforms/)
- [ADO.NET Documentation](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)

---

## 👥 Tác giả và giấy phép

**Dự án**: Hệ thống Quản lý Đăng ký Học tín chỉ

**Ngành**: Kỹ thuật phần mềm

**Năm học**: 2024-2025

**Môn học**: Tin học ứng dụng

**Phiên bản**: 1.0

---

## 📞 Hỗ trợ

Nếu gặp vấn đề, vui lòng kiểm tra:

1. SQL Server có đang chạy không
2. Database `tinchi` đã được tạo chưa
3. Connection string trong `App.config` có đúng không
4. Windows Authentication có quyền truy cập không

---

## ✅ Checklist trước khi chạy

- [ ] SQL Server đã được cài đặt và đang chạy
- [ ] Database `tinchi` đã được tạo
- [ ] Các bảng đã được tạo với đầy đủ quan hệ
- [ ] Connection string trong `App.config` đã được cấu hình đúng
- [ ] Đã có dữ liệu mẫu (sinh viên, giảng viên, môn học...)
- [ ] Visual Studio đã cài đặt .NET Framework 4.7.2

---

**Chúc bạn sử dụng hệ thống thành công!** 🎉

