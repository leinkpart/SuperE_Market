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
    public partial class CTXuat : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public CTXuat()
        {
            InitializeComponent();
        }

        private void CTXuat_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"SELECT  pro.TenSanPham,  ctx.SoLuong, (XH.SoLuongXuat * pro.GiaBanRa) AS 'ThanhTien', XH.NgayNhap, AC.DisplayName   
                                        FROM CT_XuatHang ctx 
                                        JOIN XuatHang XH ON ctx.IdXuatHang = XH.IdXuatHang
                                        JOIN Products pro ON XH.IdSanPham = pro.IdSanPham
                                        JOIN ACCOUNT AC ON XH.IdNguoiXuat = AC.ID";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvCTXuat.DataSource = dataTable;

                    dgvCTXuat.Columns[0].HeaderText = "Tên Sản Phẩm";
                    dgvCTXuat.Columns[1].HeaderText = "Số Lượng Xuất";
                    dgvCTXuat.Columns[2].HeaderText = "Thành Tiền";
                    dgvCTXuat.Columns[3].HeaderText = "Ngày Xuất";
                    dgvCTXuat.Columns[4].HeaderText = "Người Xuất";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }
    }
}
