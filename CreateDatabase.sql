-- =============================================
-- Script tạo Database cho Hệ thống Quản lý Đăng ký Học tín chỉ
-- Database: QLDT_DangKiHocPhan
-- =============================================

-- Tạo Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QLDT_DangKiHocPhan')
BEGIN
    CREATE DATABASE QLDT_DangKiHocPhan
END
GO

USE QLDT_DangKiHocPhan
GO

-- =============================================
-- TẠO BẢNG
-- =============================================

-- Bảng: Khoa viện
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'KhoaVien')
BEGIN
    CREATE TABLE KhoaVien (
        MaKV NVARCHAR(10) PRIMARY KEY,
        TenKhoaVien NVARCHAR(100) NOT NULL
    )
END
GO

-- Bảng: Chương trình đào tạo
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChuongTrinhDaoTao')
BEGIN
    CREATE TABLE ChuongTrinhDaoTao (
        MaCTDT NVARCHAR(10) PRIMARY KEY,
        TenCTDT NVARCHAR(100) NOT NULL,
        MaKhoaVien NVARCHAR(10) NOT NULL,
        FOREIGN KEY (MaKhoaVien) REFERENCES KhoaVien(MaKV)
    )
END
GO

-- Bảng: Sinh viên
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SinhVien')
BEGIN
    CREATE TABLE SinhVien (
        MaSinhVien NVARCHAR(10) PRIMARY KEY,
        HoTenSinhVien NVARCHAR(100) NOT NULL,
        NgaySinh DATE NOT NULL,
        GioiTinh NVARCHAR(10),
        SDT NVARCHAR(20),
        Email NVARCHAR(100),
        DiaChi NVARCHAR(200),
        MaCTDT NVARCHAR(10) NOT NULL,
        Password NVARCHAR(50) NOT NULL,
        FOREIGN KEY (MaCTDT) REFERENCES ChuongTrinhDaoTao(MaCTDT)
    )
END
GO

-- Bảng: Giảng viên
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'GiangVien')
BEGIN
    CREATE TABLE GiangVien (
        MaGiangVien NVARCHAR(10) PRIMARY KEY,
        HoTenGiangVien NVARCHAR(100) NOT NULL,
        GioiTinh NVARCHAR(10),
        DiaChi NVARCHAR(200),
        Email NVARCHAR(100),
        DienThoai NVARCHAR(20),
        HocVi NVARCHAR(50),
        MaKV NVARCHAR(10) NOT NULL,
        Password NVARCHAR(50) NOT NULL,
        FOREIGN KEY (MaKV) REFERENCES KhoaVien(MaKV)
    )
END
GO

-- Bảng: Học kỳ
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HocKi')
BEGIN
    CREATE TABLE HocKi (
        MaHocKi NVARCHAR(10) PRIMARY KEY,
        TenHocKi NVARCHAR(50) NOT NULL,
        NamHoc NVARCHAR(50) NOT NULL,
        NgayBatDau DATE NOT NULL,
        NgayKetThuc DATE NOT NULL
    )
END
GO

-- Bảng: Học phần
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HocPhan')
BEGIN
    CREATE TABLE HocPhan (
        MaHocPhan NVARCHAR(10) PRIMARY KEY,
        TenHocPhan NVARCHAR(100) NOT NULL,
        SoTinChi INT NOT NULL,
        MaHocPhanTienQuyet NVARCHAR(10) NULL,
        MaHocKi NVARCHAR(10) NOT NULL,
        FOREIGN KEY (MaHocPhanTienQuyet) REFERENCES HocPhan(MaHocPhan),
        FOREIGN KEY (MaHocKi) REFERENCES HocKi(MaHocKi)
    )
END
GO

-- Bảng: Lớp học phần
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LopHocPhan')
BEGIN
    CREATE TABLE LopHocPhan (
        MaLopHocPhan NVARCHAR(10) PRIMARY KEY,
        TenLop NVARCHAR(50) NOT NULL,
        SiSo INT NOT NULL,
        SoLuongDangKy INT DEFAULT 0,
        LoaiMonHoc NVARCHAR(50),
        LichHoc NVARCHAR(100),
        MaHocPhan NVARCHAR(10) NOT NULL,
        MaGiangVien NVARCHAR(10) NOT NULL,
        FOREIGN KEY (MaHocPhan) REFERENCES HocPhan(MaHocPhan),
        FOREIGN KEY (MaGiangVien) REFERENCES GiangVien(MaGiangVien)
    )
END
GO

-- Bảng: Đăng ký
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DangKi')
BEGIN
    CREATE TABLE DangKi (
        MaSinhVien NVARCHAR(10) NOT NULL,
        MaLopHocPhan NVARCHAR(10) NOT NULL,
        HinhThucDangKi NVARCHAR(50),
        PRIMARY KEY (MaSinhVien, MaLopHocPhan),
        FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
        FOREIGN KEY (MaLopHocPhan) REFERENCES LopHocPhan(MaLopHocPhan)
    )
END
GO

-- Bảng: Điểm
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Diem')
BEGIN
    CREATE TABLE Diem (
        MaSinhVien NVARCHAR(10) NOT NULL,
        MaLopHocPhan NVARCHAR(10) NOT NULL,
        DiemChuyenCan FLOAT NULL,
        DiemGiuaKy FLOAT NULL,
        DiemThi FLOAT NULL,
        DiemTongKet FLOAT NULL,
        PRIMARY KEY (MaSinhVien, MaLopHocPhan),
        FOREIGN KEY (MaSinhVien) REFERENCES SinhVien(MaSinhVien),
        FOREIGN KEY (MaLopHocPhan) REFERENCES LopHocPhan(MaLopHocPhan)
    )
END
GO

-- =============================================
-- THÊM DỮ LIỆU MẪU
-- =============================================

-- Khoa viện
IF NOT EXISTS (SELECT * FROM KhoaVien WHERE MaKV = 'KV01')
    INSERT INTO KhoaVien VALUES ('KV01', N'Khoa Công nghệ thông tin')
IF NOT EXISTS (SELECT * FROM KhoaVien WHERE MaKV = 'KV02')
    INSERT INTO KhoaVien VALUES ('KV02', N'Khoa Kinh tế')
IF NOT EXISTS (SELECT * FROM KhoaVien WHERE MaKV = 'KV03')
    INSERT INTO KhoaVien VALUES ('KV03', N'Khoa Quản trị kinh doanh')

-- Chương trình đào tạo
IF NOT EXISTS (SELECT * FROM ChuongTrinhDaoTao WHERE MaCTDT = 'CTDT01')
    INSERT INTO ChuongTrinhDaoTao VALUES ('CTDT01', N'Kỹ thuật phần mềm', 'KV01')
IF NOT EXISTS (SELECT * FROM ChuongTrinhDaoTao WHERE MaCTDT = 'CTDT02')
    INSERT INTO ChuongTrinhDaoTao VALUES ('CTDT02', N'Kinh tế số', 'KV02')
IF NOT EXISTS (SELECT * FROM ChuongTrinhDaoTao WHERE MaCTDT = 'CTDT03')
    INSERT INTO ChuongTrinhDaoTao VALUES ('CTDT03', N'Quản trị kinh doanh', 'KV03')

-- Giảng viên
IF NOT EXISTS (SELECT * FROM GiangVien WHERE MaGiangVien = 'GV01')
    INSERT INTO GiangVien VALUES ('GV01', N'Nguyễn Văn A', 'Nam', 'Hà Nội', 'gv01@neue.edu.vn', '0912345678', N'Tiến sĩ', 'KV01', 'password')
IF NOT EXISTS (SELECT * FROM GiangVien WHERE MaGiangVien = 'GV02')
    INSERT INTO GiangVien VALUES ('GV02', N'Trần Thị B', 'Nữ', 'Hà Nội', 'gv02@neue.edu.vn', '0912345679', N'Thạc sĩ', 'KV01', 'password')
IF NOT EXISTS (SELECT * FROM GiangVien WHERE MaGiangVien = 'GV03')
    INSERT INTO GiangVien VALUES ('GV03', N'Lê Văn C', 'Nam', 'Hà Nội', 'gv03@neue.edu.vn', '0912345680', N'Tiến sĩ', 'KV02', 'password')

-- Sinh viên
IF NOT EXISTS (SELECT * FROM SinhVien WHERE MaSinhVien = 'SV001')
    INSERT INTO SinhVien VALUES ('SV001', N'Nguyễn Văn Nam', '2003-01-15', 'Nam', '0911111111', 'sv001@neue.edu.vn', 'Hà Nội', 'CTDT01', 'password')
IF NOT EXISTS (SELECT * FROM SinhVien WHERE MaSinhVien = 'SV002')
    INSERT INTO SinhVien VALUES ('SV002', N'Trần Thị Hoa', '2003-03-20', 'Nữ', '0911111112', 'sv002@neue.edu.vn', 'Hà Nội', 'CTDT01', 'password')
IF NOT EXISTS (SELECT * FROM SinhVien WHERE MaSinhVien = 'SV003')
    INSERT INTO SinhVien VALUES ('SV003', N'Lê Văn Tuấn', '2003-05-10', 'Nam', '0911111113', 'sv003@neue.edu.vn', 'Hà Nội', 'CTDT02', 'password')

-- Học kỳ
IF NOT EXISTS (SELECT * FROM HocKi WHERE MaHocKi = 'HK20241')
    INSERT INTO HocKi VALUES ('HK20241', N'Học kỳ 1', '2024-2025', '2024-09-01', '2024-12-31')
IF NOT EXISTS (SELECT * FROM HocKi WHERE MaHocKi = 'HK20242')
    INSERT INTO HocKi VALUES ('HK20242', N'Học kỳ 2', '2024-2025', '2025-01-01', '2025-04-30')

-- Học phần
IF NOT EXISTS (SELECT * FROM HocPhan WHERE MaHocPhan = 'HP001')
    INSERT INTO HocPhan VALUES ('HP001', N'Lập trình hướng đối tượng', 3, NULL, 'HK20241')
IF NOT EXISTS (SELECT * FROM HocPhan WHERE MaHocPhan = 'HP002')
    INSERT INTO HocPhan VALUES ('HP002', N'Cơ sở dữ liệu', 3, NULL, 'HK20241')
IF NOT EXISTS (SELECT * FROM HocPhan WHERE MaHocPhan = 'HP003')
    INSERT INTO HocPhan VALUES ('HP003', N'Thiết kế giao diện', 2, 'HP001', 'HK20241')
IF NOT EXISTS (SELECT * FROM HocPhan WHERE MaHocPhan = 'HP004')
    INSERT INTO HocPhan VALUES ('HP004', N'Kinh tế vi mô', 3, NULL, 'HK20241')
IF NOT EXISTS (SELECT * FROM HocPhan WHERE MaHocPhan = 'HP005')
    INSERT INTO HocPhan VALUES ('HP005', N'Quản trị học', 2, NULL, 'HK20241')

-- Lớp học phần
IF NOT EXISTS (SELECT * FROM LopHocPhan WHERE MaLopHocPhan = 'LHP001')
    INSERT INTO LopHocPhan VALUES ('LHP001', 'LHP01', 80, 0, N'Bắt buộc', 'Thứ 2-4-6, 07:30-09:00', 'HP001', 'GV01')
IF NOT EXISTS (SELECT * FROM LopHocPhan WHERE MaLopHocPhan = 'LHP002')
    INSERT INTO LopHocPhan VALUES ('LHP002', 'LHP02', 50, 0, N'Tiếng Anh', 'Thứ 3-5, 09:00-10:30', 'HP001', 'GV02')
IF NOT EXISTS (SELECT * FROM LopHocPhan WHERE MaLopHocPhan = 'LHP003')
    INSERT INTO LopHocPhan VALUES ('LHP003', 'LHP01', 80, 0, N'Bắt buộc', 'Thứ 2-4-6, 09:00-10:30', 'HP002', 'GV01')
IF NOT EXISTS (SELECT * FROM LopHocPhan WHERE MaLopHocPhan = 'LHP004')
    INSERT INTO LopHocPhan VALUES ('LHP004', 'LHP02', 80, 0, N'Bắt buộc', 'Thứ 3-5, 07:30-09:00', 'HP002', 'GV02')
IF NOT EXISTS (SELECT * FROM LopHocPhan WHERE MaLopHocPhan = 'LHP005')
    INSERT INTO LopHocPhan VALUES ('LHP005', 'LHP01', 80, 0, N'Tự chọn', 'Thứ 2-4, 14:00-15:30', 'HP003', 'GV01')

PRINT N'Database và dữ liệu mẫu đã được tạo thành công!'


