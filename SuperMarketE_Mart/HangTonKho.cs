using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketE_Mart
{
    public partial class HangTonKho : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public HangTonKho()
        {
            InitializeComponent();
        }

        private void HangTonKho_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"SELECT pro.IdSanPham, pro.TenSanPham, DM.TenDanhMuc, pro.NgaySX, NCC.TenNCC, pro.SoLuong
                                    FROM Products pro
                                    JOIN TB_DanhMucSP DM ON pro.IdDanhMucSP = DM.IdDanhMuc
                                    Join TB_NhaCungCap NCC ON pro.IdNhaSX = NCC.IdNCC";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvHangTonKho.DataSource = dataTable;

                    dgvHangTonKho.Columns[0].HeaderText = "Mã Sản Phẩm";
                    dgvHangTonKho.Columns[1].HeaderText = "Tên Sản Phẩm";
                    dgvHangTonKho.Columns[2].HeaderText = "Danh Mục";
                    dgvHangTonKho.Columns[3].HeaderText = "Ngày Sản Xuất";
                    dgvHangTonKho.Columns[4].HeaderText = "Nhà Sản Xuất";
                    dgvHangTonKho.Columns[5].HeaderText = "Số Lượng Tồn";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim().ToLower();

                if (keyword == "")
                {
                    HangTonKho_Load(sender, e);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string query = @"SELECT pro.IdSanPham, pro.TenSanPham, DM.TenDanhMuc, pro.NgaySX, NCC.TenNCC, pro.SoLuong
                                    FROM Products pro
                                    JOIN TB_DanhMucSP DM ON pro.IdDanhMucSP = DM.IdDanhMuc
                                    Join TB_NhaCungCap NCC ON pro.IdNhaSX = NCC.IdNCC
                                    WHERE TenSanPham LIKE '%" + keyword + "%'";
                    SqlCommand command = new SqlCommand(query, conn);


                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    dgvHangTonKho.DataSource = dataTable;

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }
    }
}
