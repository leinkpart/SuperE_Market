using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SuperMarketE_Mart
{
    public partial class QL_Hang : Form
    {
        private  string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public QL_Hang()
        {
            InitializeComponent();
        }

        public DataTable getNhaSXList()
        {
            string getListNSX = "SELECT * FROM TB_NhaCungCap";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(getListNSX, connectionString);
            DataSet dataSet = new DataSet();
            try
            {
                sqlDataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch
            {
                return null;
            }
        }

        public DataTable getDanhMuc() 
        {
            string getDanhMuc = "SELECT * FROM TB_DanhMucSP";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(getDanhMuc, connectionString);
            DataSet dataSet = new DataSet();
            try
            {
                sqlDataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch { return null; }
        }

        private void QL_Hang_Load(object sender, EventArgs e)
        {            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed) 
                        connection.Open();
                    string query = "SELECT  pro.IdSanPham,  pro.TenSanPham,  pro.SoLuong,  pro.NgaySX,  DM.TenDanhMuc,  NCC.TenNCC,   FORMAT(pro.GiaNhapVao, 'N0', 'vi-VN') AS GiaNhapVao,   FORMAT(pro.GiaBanRa, 'N0', 'vi-VN') AS GiaBanRa   FROM   Products pro  JOIN  TB_DanhMucSP DM ON pro.IdDanhMucSP = DM.IdDanhMuc  JOIN   TB_NhaCungCap NCC ON pro.IdNhaSX = NCC.IdNCC";
                    SqlCommand cmd = new SqlCommand(query, connection);                   

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dtgvGoodsList.DataSource = dataTable;
                    dtgvGoodsList.Columns[0].HeaderText = "Mã Sản Phẩm";
                    dtgvGoodsList.Columns[1].HeaderText = "Tên Sản Phẩm";
                    dtgvGoodsList.Columns[2].HeaderText = "Số Lượng";
                    dtgvGoodsList.Columns[3].HeaderText = "Ngày Sản Xuất";
                    dtgvGoodsList.Columns[4].HeaderText = "Danh Mục";
                    dtgvGoodsList.Columns[5].HeaderText = "Nhà Sản Xuất";
                    dtgvGoodsList.Columns[6].HeaderText = "Giá Nhập Vào";                   
                    dtgvGoodsList.Columns[7].HeaderText = "Giá Bán Ra";


                    DataTable dtTable = getNhaSXList();
                    cmbNhaSX.DataSource = dtTable;
                    cmbNhaSX.DisplayMember = "TenNCC";
                    cmbNhaSX.ValueMember = "IdNCC";

                    DataTable dataTB = getDanhMuc();
                    cbDanhMuc.DataSource = dataTB;
                    cbDanhMuc.DisplayMember = "TenDanhMuc";
                    cbDanhMuc.ValueMember = "IdDanhMuc";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }   
        }

        private void dtgvGoodsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dtgvGoodsList.Rows.Count)
            {
                DataGridViewRow dataGridView = dtgvGoodsList.Rows[e.RowIndex];
                
                txtIDSP.Text = dataGridView.Cells["IdSanPham"].Value.ToString();
                txtTenSP.Text = dataGridView.Cells["TenSanPham"].Value.ToString();
                txtInputPrice.Text = dataGridView.Cells["GiaNhapVao"].Value.ToString();
                numSoLuong.Text = dataGridView.Cells["SoLuong"].Value.ToString();
                cmbNhaSX.Text = dataGridView.Cells["TenNCC"].Value.ToString();
                dtNgaySX.Text = dataGridView.Cells["NgaySX"].Value.ToString();
                cbDanhMuc.Text = dataGridView.Cells["TenDanhMuc"].Value.ToString();
                txtOutputPrice.Text = dataGridView.Cells["GiaBanRa"].Value.ToString();
            }
            btnAdd.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTenSP.Text == "" && txtInputPrice.Text == "" && cmbNhaSX.Text == "" && cbDanhMuc.Text == "")
            {
                MessageBox.Show("Please enter Information follow the Fields.", "Information", MessageBoxButtons.OK);
                txtTenSP.Focus();
                return;
            }
            if (txtTenSP.Text == "")
            {
                MessageBox.Show("Tên SP has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtTenSP.Focus();
                return;
            }

            if (txtInputPrice.Text == "")
            {
                MessageBox.Show("Giá Tiền has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtInputPrice.Focus();
                return;
            }


            if (cmbNhaSX.Text == "")
            {
                MessageBox.Show("Nhà SX has no Information yet!!", "Information", MessageBoxButtons.OK);
                cmbNhaSX.Focus();
                return;
            }

            DateTime dateTime = dtNgaySX.Value;
            if (dateTime > DateTime.Now)
            {
                MessageBox.Show("Please double check the Date entered.", "Information", MessageBoxButtons.OK);
                dtNgaySX.Focus();
                return;
            }

            if (cbDanhMuc.Text == "")
            {
                MessageBox.Show("Danh Mục has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbDanhMuc.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Gọi thủ tục(Proc) để thêm một dữ liệu mới vào bảng
                    SqlCommand command = new SqlCommand("AddProduct", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("@TenSanPham", txtTenSP.Text);
                    command.Parameters.AddWithValue("@SoLuong", numSoLuong.Text);
                    command.Parameters.AddWithValue("@NgaySX", dtNgaySX.Value.ToString());
                    command.Parameters.AddWithValue("@TenDanhMuc", cbDanhMuc.Text);
                    command.Parameters.AddWithValue("@TenNhaSX", cmbNhaSX.Text);
                    command.Parameters.AddWithValue("@GiaNhapVao", txtInputPrice.Text);
                    command.Parameters.AddWithValue("@GiaBanRa", txtOutputPrice.Text);
                   
                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_Hang_Load(sender, e);

                    MessageBox.Show("Added Successfully");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }


        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDSP.Text = string.Empty;
            txtTenSP.Text = string.Empty;
            txtInputPrice.Text = string.Empty;
            numSoLuong.Text = "1";
            cmbNhaSX.Text = string.Empty;
            dtNgaySX.Text = string.Empty;
            cbDanhMuc.Text = string.Empty;
            txtOutputPrice.Text = string.Empty;

            btnAdd.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtIDSP.Text == "")
            {
                MessageBox.Show("Please choose one data from list to Edit", "Information", MessageBoxButtons.OK);
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    //Gọi thủ tục để cập nhật hàng hóa sau khi sửa.
                    SqlCommand command = new SqlCommand("UpdateProduct", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdSanPham", txtIDSP.Text);
                    command.Parameters.AddWithValue("@TenSanPham", txtTenSP.Text);
                    command.Parameters.AddWithValue("@SoLuong", numSoLuong.Text);
                    command.Parameters.AddWithValue("@NgaySX", dtNgaySX.Value.ToString());
                    command.Parameters.AddWithValue("@TenDanhMuc", cbDanhMuc.Text);
                    command.Parameters.AddWithValue("@TenNhaSX", cmbNhaSX.Text);
                    command.Parameters.AddWithValue("@GiaNhapVao", txtInputPrice.Text);
                    command.Parameters.AddWithValue("@GiaBanRa", txtOutputPrice.Text);
                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_Hang_Load(sender, e);

                    MessageBox.Show("Edited Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtIDSP.Text == "")
            {
                MessageBox.Show("Please choose one data from list to Delete", "Information", MessageBoxButtons.OK);
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();


                    SqlCommand command = new SqlCommand("DeleteGoods", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdSanPham", txtIDSP.Text);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_Hang_Load(sender, e);

                    MessageBox.Show("Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnNew_Click(sender, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {          

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim().ToLower();

                if (keyword == "")
                {
                    QL_Hang_Load(sender, e);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string query = "SELECT * FROM Products WHERE TenSanPham LIKE '%" + keyword + "%'";
                    SqlCommand command = new SqlCommand(query, conn);
                    

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    dtgvGoodsList.DataSource = dataTable;


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
