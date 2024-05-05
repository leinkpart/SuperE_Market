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
    public partial class CategoryForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public CategoryForm()
        {
            InitializeComponent();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = "SELECT * FROM TB_DanhMucSP";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvDanhMuc.DataSource = dataTable;
                    dgvDanhMuc.Columns[0].HeaderText = "Mã Danh Mục";
                    dgvDanhMuc.Columns[1].HeaderText = "Tên Danh Mục";                  

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập tên danh mục...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Nhập tên danh mục...";
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtSearch.Text.Trim().ToLower();

                if (keyword == "")
                {
                    CategoryForm_Load(sender, e);
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string query = "SELECT * FROM TB_DanhMucSP WHERE TenDanhMuc LIKE '%" + keyword + "%'";
                    SqlCommand command = new SqlCommand(query, conn);


                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    dgvDanhMuc.DataSource = dataTable;


                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void dgvDanhMuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDanhMuc.Rows.Count)
            {
                DataGridViewRow dataGridView = dgvDanhMuc.Rows[e.RowIndex];

                txtIdDanhMuc.Text = dataGridView.Cells["IdDanhMuc"].Value.ToString();
                txtTenDanhMuc.Text = dataGridView.Cells["TenDanhMuc"].Value.ToString();
            }
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtIdDanhMuc.Text == "" && txtTenDanhMuc.Text == "" )
            {
                MessageBox.Show("Please enter Information follow the Fields.", "Information", MessageBoxButtons.OK);
                txtIdDanhMuc.Focus();
                return;
            }

            if (txtIdDanhMuc.Text == "")
            {
                MessageBox.Show("ID Danh Muc has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtIdDanhMuc.Focus();
                return;
            }

            if (txtTenDanhMuc.Text == "")
            {
                MessageBox.Show("Ten Danh Muc has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtTenDanhMuc.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string AddCategory = "INSERT INTO TB_DanhMucSP VALUES (@IdDanhMuc , @TenDanhMuc )";

                    SqlCommand command = new SqlCommand(AddCategory, conn);

                    command.Parameters.AddWithValue("@IdDanhMuc", txtIdDanhMuc.Text);
                    command.Parameters.AddWithValue("@TenDanhMuc", txtTenDanhMuc.Text);
                    
                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    CategoryForm_Load(sender, e);

                    MessageBox.Show("Saved Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnRefresh_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdDanhMuc.Text == "" && txtTenDanhMuc.Text == "" )
                {
                    MessageBox.Show("Please choose one data to Delete", "Information", MessageBoxButtons.OK);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string DeleteCategory = "DELETE FROM TB_DanhMucSP WHERE IdDanhMuc = '" + txtIdDanhMuc.Text + "'";

                    SqlCommand command = new SqlCommand(DeleteCategory, conn);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    CategoryForm_Load(sender, e);

                    MessageBox.Show("Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnRefresh_Click(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdDanhMuc.Text == "" && txtTenDanhMuc.Text == "")
                {
                    MessageBox.Show("Please choose one data to Edit", "Information", MessageBoxButtons.OK);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string EditCategory = "UPDATE TB_DanhMucSP SET IdDanhMuc = @IdDanhMuc , TenDanhMuc = @TenDanhMuc WHERE IdDanhMuc = @IdDanhMuc";

                    SqlCommand command = new SqlCommand(EditCategory, conn);

                    command.Parameters.AddWithValue("@IdDanhMuc", txtIdDanhMuc.Text);
                    command.Parameters.AddWithValue("@TenDanhMuc", txtTenDanhMuc.Text);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    CategoryForm_Load(sender, e);

                    MessageBox.Show("Edited Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnRefresh_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtIdDanhMuc.Text = string.Empty;
            txtTenDanhMuc.Text = string.Empty;

            btnSave.Enabled = true;
        }
    }
}
