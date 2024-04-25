CREATE DATABASE SieuThiE_Market
GO

USE SieuThiE_Market
GO

CREATE TABLE ACCOUNT (
	ID VARCHAR(15) PRIMARY KEY,
	Username VARCHAR(20),
	DisplayName NVARCHAR(40),
	Password varchar(15),
	Email varchar(50),
	PhoneNum varchar(10),
	JoinDate DATE,
	VaiTro NVARCHAR(20)
);
GO

CREATE TABLE Products (
    IdSanPham INT PRIMARY KEY IDENTITY,
    TenSanPham NVARCHAR(50) NOT NULL,
    GiaTien  MONEY,
    SoLuong INT NOT NULL,
    NhaSX NVARCHAR(50),
    NgaySX DATE,
    DanhMucSP NVARCHAR(20),
);
GO 

CREATE TABLE TB_NhaCungCap (
	IdNCC VARCHAR(20) PRIMARY KEY,
	TenNCC NVARCHAR(25),
	DiaChi NVARCHAR(40),
	DienThoai INT, 
	HangCungCap NVARCHAR(25)
);
GO

CREATE TABLE NhapHang(
	IdNhapHang VARCHAR(15) PRIMARY KEY,
    IdSanPham INT,
    SoLuongNhap INT,
	NhaCC VARCHAR(20),
	DanhMucSP NVARCHAR(20),
    NgayNhap DATE,
);
GO

CREATE TABLE XuatHang (
	IdXuatHang VARCHAR(15) PRIMARY KEY,
    IdSanPham INT,
    SoLuongXuat INT,
	DanhMucSP NVARCHAR(20),
    NgayNhap DATE,
	NguoiXuat VARCHAR(15)
);
GO

--Kiểm tra đăng nhập bằng Username và Password
CREATE PROCEDURE CheckedLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM ACCOUNT WHERE Username = @Username AND Password = @Password)
        SELECT 'Login successful' AS Result
    ELSE
        SELECT 'Invalid username or password' AS Result
END
GO



