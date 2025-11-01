# ✅ Checklist Chức năng Hệ thống (Không bao gồm Điểm)

## 📋 Chức năng Sinh viên

### ✅ 1. Đăng nhập
- [x] LoginForm.cs - Có form đăng nhập
- [x] DatabaseHelper.LoginStudent() - Kiểm tra MaSV và password
- [x] Hiển thị form StudentMainForm khi đăng nhập thành công

**Test**: Nhập MaSV (ví dụ: SV001) + password = "password"

### ✅ 2. Xem thông tin cá nhân
- [x] StudentMainForm.LoadStudentInfo() - Load thông tin từ database
- [x] Hiển thị: MaSV, TenSV, Chương trình, Ngày sinh, etc.

**Test**: Sau khi đăng nhập → Xem thông tin ở góc trên

### ✅ 3. Xem danh sách lớp đã đăng ký
- [x] StudentMainForm.LoadRegisteredCourses()
- [x] DatabaseHelper.GetRegisteredSections() - Query từ bảng DangKi
- [x] Hiển thị trong tab "Lớp đã đăng ký"

**Test**: Đăng nhập → Tab "Lớp đã đăng ký" → Xem danh sách

### ✅ 4. Xem danh sách lớp có thể đăng ký
- [x] StudentMainForm.LoadAvailableCourses()
- [x] DatabaseHelper.GetCoursesBySemester() - Lấy môn học theo học kỳ
- [x] DatabaseHelper.GetAvailableCourseSections() - Lấy lớp chưa đầy
- [x] Hiển thị trong tab "Lớp có thể đăng ký"

**Test**: Đăng nhập → Tab "Lớp có thể đăng ký" → Xem danh sách

### ✅ 5. Đăng ký lớp học phần
- [x] StudentMainForm.btnRegister_Click()
- [x] DatabaseHelper.RegisterCourseSection() - Insert vào bảng DangKi
- [x] Kiểm tra lớp đã đầy chưa
- [x] Kiểm tra đã đăng ký chưa

**Test**: Chọn lớp → Click "Đăng ký" → Kiểm tra hiện trong "Lớp đã đăng ký"

### ✅ 6. Hủy đăng ký lớp học phần
- [x] StudentMainForm.btnCancel_Click()
- [x] DatabaseHelper.CancelRegistration() - Delete từ bảng DangKi
- [x] Cập nhật lại danh sách

**Test**: Chọn lớp đã đăng ký → Click "Hủy đăng ký" → Kiểm tra đã bị xóa

### ✅ 7. Xem lịch học
- [x] StudentMainForm.btnSchedule_Click()
- [x] Hiển thị lịch học các lớp đã đăng ký

**Test**: Click "Xem lịch học" → Xem lịch hiển thị

### ❌ 8. Xem điểm (KHÔNG CẦN)
- [ ] Đã comment/disabled

---

## 📋 Chức năng Giảng viên

### ✅ 1. Đăng nhập
- [x] LoginForm.cs - Xử lý đăng nhập giảng viên
- [x] DatabaseHelper.LoginInstructor() - Kiểm tra MaGV và password
- [x] Hiển thị form InstructorMainForm khi đăng nhập thành công

**Test**: Nhập MaGV (ví dụ: GV01) + password = "password"

### ✅ 2. Xem thông tin cá nhân
- [x] InstructorMainForm.LoadInstructorInfo() - Load từ database
- [x] Hiển thị: MaGV, TenGV, Khoa, Học vị, etc.

**Test**: Sau khi đăng nhập → Xem thông tin

### ✅ 3. Xem danh sách lớp đang dạy
- [x] InstructorMainForm.LoadMyCourses()
- [x] DatabaseHelper.GetInstructorSections() - Query từ LopHocPhan
- [x] Hiển thị: MaLHP, TenHocPhan, TenLop, LichHoc, SiSo

**Test**: Đăng nhập → Xem danh sách lớp

### ✅ 4. Xem danh sách sinh viên trong lớp
- [x] InstructorMainForm.btnViewStudents_Click()
- [x] DatabaseHelper.GetStudentsInSection() - Query từ DangKi join SinhVien
- [x] StudentsListForm - Form hiển thị danh sách

**Test**: Chọn lớp → Click "Xem danh sách sinh viên" → Xem form

### ✅ 5. Xem lịch dạy
- [x] InstructorMainForm.btnSchedule_Click()
- [x] Hiển thị lịch dạy các lớp

**Test**: Click "Lịch dạy" → Xem lịch hiển thị

### ❌ 6. Nhập điểm (KHÔNG CẦN)
- [ ] Đã comment/disabled trong btnEnterGrades_Click()

---

## 🔍 Kiểm tra Database Mapping

### ✅ Bảng và Query
- [x] **Khoa** - GetDepartmentName()
- [x] **CTDT** - GetProgramName()
- [x] **SinhVien** - LoginStudent(), GetStudentsInSection()
- [x] **GiangVien** - LoginInstructor()
- [x] **HocKi** - GetCoursesBySemester(), GetCurrentSemester()
- [x] **MonHoc** - GetCoursesBySemester(), GetAvailableCourseSections()
- [x] **LopHocPhan** - GetAvailableCourseSections(), GetRegisteredSections(), GetInstructorSections()
- [x] **DangKi** - RegisterCourseSection(), CancelRegistration(), GetRegisteredSections()

### ✅ Tên cột đã khớp
- [x] MaSV, TenSV (SinhVien)
- [x] MaGV, TenGV (GiangVien)
- [x] MaMH, TenHocPhan, SoTC (MonHoc)
- [x] MaLHP, TenLop, MaHP, MaGV, SiSo, LichHoc (LopHocPhan)
- [x] MaSV, MaLHP, HinhThuc (DangKi)
- [x] MaHocKi (int) (HocKi)

---

## ⚠️ Cần lưu ý

### 1. Authentication
- ✅ Hiện cho phép password = "password" hoặc = username
- ⚠️ Không có cột Password trong DB (đã xử lý)

### 2. SoLuongDangKy
- ✅ Tính động từ COUNT(*) trong DangKi
- ✅ Không cần update thủ công

### 3. LoaiMonHoc
- ⚠️ Không có trong schema → Hiển thị trống (đã xử lý)

### 4. MaHocKi
- ✅ Dùng int (IDENTITY) - đã xử lý đúng
- ✅ GetCurrentSemester() lấy học kỳ mới nhất

### 5. HinhThuc trong DangKi
- ✅ Mặc định = 'Kế hoạch' khi đăng ký (theo schema)

---

## 🎯 Kết luận

### ✅ ĐÃ ĐỦ CHO CÁC CHỨC NĂNG CƠ BẢN:

**Sinh viên:**
- ✅ Đăng nhập
- ✅ Xem thông tin
- ✅ Xem lớp đã đăng ký
- ✅ Xem lớp có thể đăng ký
- ✅ Đăng ký lớp
- ✅ Hủy đăng ký
- ✅ Xem lịch học

**Giảng viên:**
- ✅ Đăng nhập
- ✅ Xem thông tin
- ✅ Xem lớp đang dạy
- ✅ Xem danh sách sinh viên
- ✅ Xem lịch dạy

### ❌ ĐÃ TẮT:
- ❌ Chức năng điểm (theo yêu cầu)

---

## 🚀 Sẵn sàng Test!

Code đã **ĐỦ** cho các chức năng cơ bản. Bạn có thể:
1. ✅ Build project
2. ✅ Run và test đăng nhập
3. ✅ Test đăng ký/hủy đăng ký
4. ✅ Test các chức năng xem

**KHÔNG CẦN BỔ SUNG GÌ THÊM!** 🎉

