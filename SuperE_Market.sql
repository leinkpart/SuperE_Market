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
    GiaNhapVao  DECIMAL(18,0),
    SoLuong INT NOT NULL,
    IdNhaSX NVARCHAR(50),
    NgaySX DATE,
    IdDanhMucSP VARCHAR(10),
	GiaBanRa DECIMAL(18,0)
);
GO


CREATE TABLE TB_NhaCungCap (
	IdNCC VARCHAR(20) PRIMARY KEY,
	TenNCC NVARCHAR(25),
	DiaChi NVARCHAR(40),
	DienThoai INT, 
);
GO

CREATE TABLE NhapHang(
	IdNhapHang VARCHAR(15) PRIMARY KEY,
    IdSanPham INT,
    SoLuongNhap INT,
	NhaCC VARCHAR(20),
	IdDanhMucSP VARCHAR(10),
    NgayNhap DATE,
	IdTaiKhoan Varchar(15)
);
GO


CREATE TABLE CT_NhapHang
(
	IdSanPam INT PRIMARY KEY,
	IdNhapHang VARCHAR(15),
	SoLuong INT,
	ThanhTien DECIMAL(18,0)
);
GO



CREATE TABLE XuatHang (
	IdXuatHang VARCHAR(15) PRIMARY KEY,
    IdSanPham INT,
    SoLuongXuat INT,
	IdDanhMucSP VARCHAR(10),
    NgayNhap DATE,
	IdNguoiXuat VARCHAR(15)
);
GO

CREATE TABLE CT_XuatHang
(
	IdSanPam INT PRIMARY KEY,
	IdXuatHang VARCHAR(15),
	SoLuong INT,
	ThanhTien DECIMAL(18,0)
);
GO


CREATE TABLE TB_DanhMucSP
(
	IdDanhMuc varchar(10) PRIMARY KEY,
	TenDanhMuc nvarchar(30)
);
GO

SELECT 
    pro.IdSanPham,
    pro.TenSanPham,
    pro.SoLuong,
    pro.NgaySX,
    DM.TenDanhMuc,
    NCC.TenNCC,
    FORMAT(pro.GiaNhapVao, 'N0', 'vi-VN') AS GiaNhapVao,
    FORMAT(pro.GiaBanRa, 'N0', 'vi-VN') AS GiaBanRa
FROM 
    Products pro
JOIN 
    TB_DanhMucSP DM ON pro.IdDanhMucSP = DM.IdDanhMuc
JOIN 
    TB_NhaCungCap NCC ON pro.IdNhaSX = NCC.IdNCC;
GO


-- Thủ Tục Kiểm tra đăng nhập bằng Username và Password
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

--Pro thêm hàng hóa vào bảng Products
CREATE PROCEDURE AddProduct
    @TenSanPham NVARCHAR(100),
    @SoLuong INT,
    @NgaySX DATE,
    @TenDanhMuc NVARCHAR(30),
    @TenNhaSX NVARCHAR(25),
    @GiaNhapVao DECIMAL(18, 0),
    @GiaBanRa DECIMAL(18, 0)
AS
BEGIN
    DECLARE @IdDanhMuc varchar(10)
    DECLARE @IdNhaSX varchar(20)

    -- Lấy ID của Danh mục từ tên
    SELECT @IdDanhMuc = IdDanhMuc FROM TB_DanhMucSP WHERE TenDanhMuc = @TenDanhMuc

    -- Lấy ID của Nhà cung cấp từ tên
    SELECT @IdNhaSX = IdNCC FROM TB_NhaCungCap WHERE TenNCC = @TenNhaSX

    -- Thêm dữ liệu vào bảng HangHoa
    INSERT INTO Products(TenSanPham, SoLuong, NgaySX, IdDanhMucSP, IdNhaSX, GiaNhapVao, GiaBanRa)
    VALUES (@TenSanPham, @SoLuong, @NgaySX, @IdDanhMuc, @IdNhaSX, @GiaNhapVao, @GiaBanRa)
END
GO

--Proc Update Hàng hóa sau khi đã chỉnh sửa
CREATE PROCEDURE UpdateProduct
    @IdSanPham INT,
    @TenSanPham NVARCHAR(50),
    @SoLuong INT,
    @NgaySX DATE,
    @TenDanhMuc NVARCHAR(30),
    @TenNhaSX NVARCHAR(25),
    @GiaNhapVao DECIMAL(18, 0),
    @GiaBanRa DECIMAL(18, 0)
AS
BEGIN
    DECLARE @IdDanhMuc Varchar(10)
    DECLARE @IdNhaCC Varchar(20)

    -- Lấy ID của Danh mục từ tên
    SELECT @IdDanhMuc = IdDanhMuc FROM TB_DanhMucSP WHERE TenDanhMuc = @TenDanhMuc

    -- Lấy ID của Nhà cung cấp từ tên
    SELECT @IdNhaCC = IdNCC FROM TB_NhaCungCap WHERE TenNCC = @TenNhaSX

    -- Cập nhật thông tin hàng hóa
    UPDATE Products
    SET 
        TenSanPham = @TenSanPham,
        SoLuong = @SoLuong,
        NgaySX = @NgaySX,
        IdDanhMucSP = @IdDanhMuc,
        IdNhaSX = @IdNhaCC,
        GiaNhapVao = @GiaNhapVao,
        GiaBanRa = @GiaBanRa
    WHERE
        IdSanPham = @IdSanPham
END
GO


-- Proc Xóa một hàng hóa
CREATE PROCEDURE DeleteGoods
    @IdSanPham INT
AS
BEGIN
    DELETE FROM Products WHERE IdSanPham = @IdSanPham;
END
GO

-- Proc Nhập Hàng vào bảng NhapHang
CREATE PROCEDURE NhapHangProc
	@IdNhapHang Varchar(15),
    @TenSanPham NVARCHAR(35),
    @TenNhaCC NVARCHAR(20),
    @SoLuongNhap INT,
    @TenDanhMuc NVARCHAR(25),
    @NgayNhap DATE,
    @TenNguoiNhap NVARCHAR(55)
AS
BEGIN
    DECLARE @IdSanPham INT 
	DECLARE @IdNhaCC Varchar(20)
	DECLARE @IdDanhMuc Varchar(10)
	DECLARE @IdTaiKhoan Varchar(15);

    -- Lấy Id của sản phẩm từ bảng Products dựa trên tên sản phẩm
    SELECT @IdSanPham = IdSanPham
    FROM Products
    WHERE TenSanPham = @TenSanPham;

    -- Lấy Id của nhà cung cấp từ bảng NhaCungCap dựa trên tên nhà cung cấp
    SELECT @IdNhaCC = IdNCC
    FROM TB_NhaCungCap
    WHERE TenNCC = @TenNhaCC;

    -- Lấy Id của danh mục sản phẩm từ bảng DanhMucSP dựa trên tên danh mục
    SELECT @IdDanhMuc = IdDanhMuc
    FROM TB_DanhMucSP
    WHERE TenDanhMuc = @TenDanhMuc;

    -- Lấy Id của người nhập từ bảng TaiKhoan dựa trên tên người nhập
    SELECT @IdTaiKhoan = ID
    FROM ACCOUNT
    WHERE DisplayName = @TenNguoiNhap;

    -- Thêm thông tin nhập hàng vào bảng NhapHang
    INSERT INTO NhapHang (IdNhapHang, IdSanPham, NhaCC, SoLuongNhap, IdDanhMucSP, NgayNhap, IdTaiKhoan)
    VALUES (@IDNhapHang, @IdSanPham, @IdNhaCC, @SoLuongNhap, @IdDanhMuc, @NgayNhap, @IdTaiKhoan);

    -- Cập nhật số lượng sản phẩm trong kho
    UPDATE Products
    SET SoLuong = SoLuong + @SoLuongNhap
    WHERE IdSanPham = @IdSanPham;
END
GO

-- Dùng để xuất hàng vào bảng XuatHang
CREATE PROC XuatHangProc
	@IdXuatHang Varchar(15),
	@TenSanPham Nvarchar(35),
	@SoLuongXuat Int,
	@TenDanhMuc Nvarchar(25),
	@NgayXuat Date,
	@TenNguoiXuat Nvarchar(55)
AS
BEGIN
	DECLARE @IdSanPham INT 
	DECLARE @IdDanhMuc Varchar(10)
	DECLARE @IdTaiKhoan Varchar(15);

	SELECT @IdSanPham = IdSanPham
    FROM Products
    WHERE TenSanPham = @TenSanPham;

	SELECT @IdDanhMuc = IdDanhMuc
    FROM TB_DanhMucSP
    WHERE TenDanhMuc = @TenDanhMuc;

	SELECT @IdTaiKhoan = ID
    FROM ACCOUNT
    WHERE DisplayName = @TenNguoiXuat;

	INSERT INTO XuatHang (IdXuatHang, IdSanPham, SoLuongXuat, IdDanhMucSP, NgayNhap, IdNguoiXuat)
    VALUES (@IdXuatHang, @IdSanPham, @SoLuongXuat, @IdDanhMuc, @NgayXuat, @IdTaiKhoan);

	 UPDATE Products
    SET SoLuong = SoLuong - @SoLuongXuat
    WHERE IdSanPham = @IdSanPham;
END
GO



