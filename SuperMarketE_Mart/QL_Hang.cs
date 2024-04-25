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

        private void QL_Hang_Load(object sender, EventArgs e)
        {            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed) 
                        connection.Open();
                    string query = "SELECT * FROM Products";
                    SqlCommand cmd = new SqlCommand(query, connection);                   

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dtgvGoodsList.DataSource = dataTable;

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
                txtPrice.Text = dataGridView.Cells["GiaTien"].Value.ToString();
                numSoLuong.Text = dataGridView.Cells["SoLuong"].Value.ToString();
                txtNhaSX.Text = dataGridView.Cells["NhaSX"].Value.ToString();
                dtNgaySX.Text = dataGridView.Cells["NgaySX"].Value.ToString();
                cbDanhMuc.Text = dataGridView.Cells["DanhMucSP"].Value.ToString();
                
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTenSP.Text == "" && txtPrice.Text == "" && txtNhaSX.Text == "" && cbDanhMuc.Text == "")
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

            if (txtPrice.Text == "")
            {
                MessageBox.Show("Giá Tiền has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtPrice.Focus();
                return;
            }


            if (txtNhaSX.Text == "")
            {
                MessageBox.Show("Nhà SX has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtNhaSX.Focus();
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
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string AddGoods = "INSERT INTO Products VALUES (@TenSanPham , @GiaTien , @SoLuong , @NhaSX , @NgaySX , @DanhMucSP )";
                   
                    SqlCommand command = new SqlCommand(AddGoods, conn);
                    
                    command.Parameters.AddWithValue("@TenSanPham", txtTenSP.Text);
                    command.Parameters.AddWithValue("@GiaTien", txtPrice.Text);
                    command.Parameters.AddWithValue("@SoLuong", numSoLuong.Text);
                    command.Parameters.AddWithValue("@NhaSX", txtNhaSX.Text);
                    command.Parameters.AddWithValue("@NgaySX", dtNgaySX.Value.ToString());
                    command.Parameters.AddWithValue("@DanhMucSP", cbDanhMuc.SelectedItem);
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
            txtPrice.Text = string.Empty;
            numSoLuong.Text = "1";
            txtNhaSX.Text = string.Empty;
            dtNgaySX.Text = string.Empty;
            cbDanhMuc.Text = string.Empty;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string EditGoods = "UPDATE Products SET TenSanPham = @TenSanPham ,GiaTien = @GiaTien ,SoLuong = @SoLuong ,NhaSX = @NhaSX ,NgaySX = @NgaySX , DanhMucSP = @DanhMucSP WHERE IdSanPham = @IdSanPham";

                    SqlCommand command = new SqlCommand(EditGoods, conn);

                    command.Parameters.AddWithValue("@IdSanPham", txtIDSP.Text);
                    command.Parameters.AddWithValue("@TenSanPham", txtTenSP.Text);
                    command.Parameters.AddWithValue("@GiaTien", txtPrice.Text);
                    command.Parameters.AddWithValue("@SoLuong", numSoLuong.Text);
                    command.Parameters.AddWithValue("@NhaSX", txtNhaSX.Text);
                    command.Parameters.AddWithValue("@NgaySX", dtNgaySX.Value.ToString());
                    command.Parameters.AddWithValue("@DanhMucSP", cbDanhMuc.SelectedItem);
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
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string DeleteGoods = "DELETE FROM Products WHERE IdSanPham = '" + txtIDSP.Text + "'";

                    SqlCommand command = new SqlCommand(DeleteGoods, conn);

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
