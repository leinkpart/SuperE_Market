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
    public partial class CTNhap : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public CTNhap()
        {
            InitializeComponent();
        }

        private void CTNhap_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"SELECT  pro.TenSanPham,  ctn.SoLuong, (NH.SoLuongNhap * pro.GiaNhapVao) AS 'ThanhTien', NH.NgayNhap, AC.DisplayName   
                                        FROM CT_NhapHang ctn 
                                        JOIN NhapHang NH ON ctn.IdNhapHang = NH.IdNhapHang
                                        JOIN Products pro ON NH.IdSanPham = pro.IdSanPham
                                        JOIN ACCOUNT AC ON NH.IdTaiKhoan = AC.ID";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvCTNhap.DataSource = dataTable;

                    dgvCTNhap.Columns[0].HeaderText = "Tên Sản Phẩm";
                    dgvCTNhap.Columns[1].HeaderText = "Số Lượng Nhập";
                    dgvCTNhap.Columns[2].HeaderText = "Thành Tiền";
                    dgvCTNhap.Columns[3].HeaderText = "Ngày Nhập";
                    dgvCTNhap.Columns[4].HeaderText = "Người Nhập";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }
    }
}
