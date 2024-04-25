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
    public partial class QL_NhaCC : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public QL_NhaCC()
        {
            InitializeComponent();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void QL_NhaCC_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = "SELECT * FROM TB_NhaCungCap";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dtgvSupplier.DataSource = dataTable;

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtSupplierID.Text == "" && txtSupplierName.Text == "" && txtAddress.Text == "" && txtPhoneNumb.Text == "" && txtSupplyGoods.Text == "")
            {
                MessageBox.Show("Please enter Information follow the Fields.", "Information", MessageBoxButtons.OK);
                txtSupplierID.Focus();
                return;
            }

            if (txtSupplierID.Text == "")
            {
                MessageBox.Show("Supplier ID has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtSupplierID.Focus();
                return;
            }

            if (txtSupplierName.Text == "")
            {
                MessageBox.Show("Supplier Name has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtSupplierName.Focus();
                return;
            }

            if (txtAddress.Text == "")
            {
                MessageBox.Show("Supplier Address has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtAddress.Focus();
                return;
            }


            if (txtPhoneNumb.Text == "")
            {
                MessageBox.Show("Supplier PhoneNumber has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtPhoneNumb.Focus();
                return;
            }

            if (txtSupplyGoods.Text == "")
            {
                MessageBox.Show("Supply Goods has no Information yet!!", "Information", MessageBoxButtons.OK);
                txtSupplyGoods.Focus();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string AddSupplier = "INSERT INTO TB_NhaCungCap VALUES (@IdNCC , @TenNCC , @DiaChi , @DienThoai , @HangCungCap , @DanhMucSP )";

                    SqlCommand command = new SqlCommand(AddSupplier, conn);

                    command.Parameters.AddWithValue("@IdNCC", txtSupplierID.Text);
                    command.Parameters.AddWithValue("@TenNCC", txtSupplierName.Text);
                    command.Parameters.AddWithValue("@DiaChi", txtAddress.Text);
                    command.Parameters.AddWithValue("@DienThoai", txtPhoneNumb.Text);
                    command.Parameters.AddWithValue("@HangCungCap", txtSupplyGoods.Text);
                    command.Parameters.AddWithValue("@DanhMucSP", cbG_Categorry.SelectedItem);
                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_NhaCC_Load(sender, e);

                    MessageBox.Show("Added Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnReset_Click(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string EditSupplier = "UPDATE TB_NhaCungCap SET IdNCC = @IdNCC , TenNCC = @TenNCC , DiaChi = @DiaChi , DienThoai = @DienThoai , HangCungCap = @HangCungCap , DanhMucSP = @DanhMucSP WHERE IdNCC = @IdNCC";

                    SqlCommand command = new SqlCommand(EditSupplier, conn);

                    command.Parameters.AddWithValue("@IdNCC", txtSupplierID.Text);
                    command.Parameters.AddWithValue("@TenNCC", txtSupplierName.Text);
                    command.Parameters.AddWithValue("@DiaChi", txtAddress.Text);
                    command.Parameters.AddWithValue("@DienThoai", txtPhoneNumb.Text);
                    command.Parameters.AddWithValue("@HangCungCap", txtSupplyGoods.Text);
                    command.Parameters.AddWithValue("@DanhMucSP", cbG_Categorry.SelectedItem);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_NhaCC_Load(sender, e);

                    MessageBox.Show("Edited Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnReset_Click(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string DeleteSupplier = "DELETE FROM TB_NhaCungCap WHERE IdNCC = '" + txtSupplierID.Text + "'";

                    SqlCommand command = new SqlCommand(DeleteSupplier, conn);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    QL_NhaCC_Load(sender, e);

                    MessageBox.Show("Added Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnReset_Click(sender, e);
        }

        private void dtgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dtgvSupplier.Rows.Count)
            {
                DataGridViewRow dataGridView = dtgvSupplier.Rows[e.RowIndex];

                txtSupplierID.Text = dataGridView.Cells["IdNCC"].Value.ToString();
                txtSupplierName.Text = dataGridView.Cells["TenNCC"].Value.ToString();
                txtAddress.Text = dataGridView.Cells["DiaChi"].Value.ToString();
                txtPhoneNumb.Text = dataGridView.Cells["DienThoai"].Value.ToString();
                txtSupplyGoods.Text = dataGridView.Cells["HangCungCap"].Value.ToString();
                cbG_Categorry.Text = dataGridView.Cells["DanhMucSP"].Value.ToString();
            }
        }



        private void txtSearch_Enter(object sender, EventArgs e)
        { 
            if (txtSearch.Text == "Enter Supplier Name..")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        { 
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Enter Supplier Name..";
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtSupplierID.Text = string.Empty;
            txtSupplierName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhoneNumb.Text = string.Empty;
            txtSupplyGoods.Text = string.Empty;
            cbG_Categorry.Text = string.Empty;
        }
    }
}
