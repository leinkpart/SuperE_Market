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
    public partial class NhapHangForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public NhapHangForm()
        {
            InitializeComponent();
        }

        private Form currentFormchild;

        private void showFormCon(Form FormCon)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
            currentFormchild = FormCon;
            FormCon.TopLevel = false;
            FormCon.FormBorderStyle = FormBorderStyle.None;
            FormCon.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(FormCon);
            pnlMain.Tag = FormCon;
            FormCon.BringToFront();
            FormCon.Show();
        }

        public DataTable getGoodsName()
        {
            string getName = "SELECT * FROM Products";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(getName, connectionString);
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

        public DataTable getNameImporter()
        {
            string getNameImporter = "SELECT * FROM ACCOUNT";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(getNameImporter, connectionString);
            DataSet dataSet = new DataSet();
            try
            {
                sqlDataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch { return null; }
        }

        private void NhapHangForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"SELECT  NH.IdNhapHang,  pro.TenSanPham,  NH.SoLuongNhap,  NCC.TenNCC,  DM.TenDanhMuc, NH.NgayNhap, AC.DisplayName   
                                    FROM NhapHang NH, Products pro, TB_DanhMucSP DM, TB_NhaCungCap NCC,ACCOUNT AC   
                                    WHERE pro.IdSanPham = NH.IdSanPham AND NH.NhaCC = NCC.IdNCC 
                                            AND NH.IdDanhMucSP = DM.IdDanhMuc AND NH.IdTaiKhoan = AC.ID";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvNhapHang.DataSource = dataTable;

                    dgvNhapHang.Columns[0].HeaderText = "Mã Nhập Hàng";
                    dgvNhapHang.Columns[1].HeaderText = "Tên Sản Phẩm";
                    dgvNhapHang.Columns[2].HeaderText = "Số Lượng Nhập";
                    dgvNhapHang.Columns[3].HeaderText = "Nhà Sản Xuất";
                    dgvNhapHang.Columns[4].HeaderText = "Danh Muc";
                    dgvNhapHang.Columns[5].HeaderText = "Ngày Nhập";
                    dgvNhapHang.Columns[6].HeaderText = "Tên Người Nhập";

                    DataTable data = getGoodsName();
                    cbTenHang.DataSource = data;
                    cbTenHang.DisplayMember = "TenSanPham";
                    cbTenHang.ValueMember = "IdSanPham";

                    DataTable dtTable = getNhaSXList();
                    cbNhaCC.DataSource = dtTable;
                    cbNhaCC.DisplayMember = "TenNCC";
                    cbNhaCC.ValueMember = "IdNCC";

                    DataTable dataTB = getDanhMuc();
                    cbDanhMuc.DataSource = dataTB;
                    cbDanhMuc.DisplayMember = "TenDanhMuc";
                    cbDanhMuc.ValueMember = "IdDanhMuc";

                    DataTable table = getNameImporter();
                    cbNguoiNhap.DataSource = table;
                    cbNguoiNhap.DisplayMember = "DisplayName";
                    cbNguoiNhap.ValueMember = "ID";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (txtMaNhap.Text == "" && cbTenHang.Text == "" && cbNhaCC.Text == "" && cbDanhMuc.Text == "")
            {
                MessageBox.Show("Please enter Information follow the Fields.", "Information", MessageBoxButtons.OK);
                txtMaNhap.Focus();
                return;
            }
            if (txtMaNhap.Text == "")
            {
                MessageBox.Show("Please enter Ma Nhap Hang!!", "Information", MessageBoxButtons.OK);
                txtMaNhap.Focus();
                return;
            }

            if (cbTenHang.Text == "")
            {
                MessageBox.Show("Ten Hang has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbTenHang.Focus();
                return;
            }


            if (cbNhaCC.Text == "")
            {
                MessageBox.Show("Nhà SX has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbNhaCC.Focus();
                return;
            }

            
            if (cbDanhMuc.Text == "")
            {
                MessageBox.Show("Danh Mục has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbDanhMuc.Focus();
                return;
            }

            if (cbNguoiNhap.Text == "")
            {
                MessageBox.Show("Please enter Importer!!", "Information", MessageBoxButtons.OK);
                cbNguoiNhap.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    
                    SqlCommand command = new SqlCommand("NhapHangProc", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdNhapHang", txtMaNhap.Text);
                    command.Parameters.AddWithValue("@TenSanPham", cbTenHang.Text);
                    command.Parameters.AddWithValue("@TenNhaCC", cbNhaCC.Text);
                    command.Parameters.AddWithValue("@SoLuongNhap", nmSoLuong.Text);
                    command.Parameters.AddWithValue("@TenDanhMuc", cbDanhMuc.Text);
                    command.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                    command.Parameters.AddWithValue("@TenNguoiNhap", cbNguoiNhap.Text);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    NhapHangForm_Load(sender, e);

                    MessageBox.Show("Added Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNhap.Text == "" )
                {
                    MessageBox.Show("Please choose one data to Delete", "Information", MessageBoxButtons.OK);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string DeleteSupplier = "DELETE FROM NhapHang WHERE IdNhapHang = '" + txtMaNhap.Text + "'";

                    SqlCommand command = new SqlCommand(DeleteSupplier, conn);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    NhapHangForm_Load(sender, e);

                    MessageBox.Show("Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnRefresh_Click(sender, e);
        }

        private void dgvNhapHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhapHang.Rows.Count)
            {
                DataGridViewRow dataGridView = dgvNhapHang.Rows[e.RowIndex];

                txtMaNhap.Text = dataGridView.Cells["IdNhapHang"].Value.ToString();
                cbTenHang.Text = dataGridView.Cells["TenSanPham"].Value.ToString();
                cbDanhMuc.Text = dataGridView.Cells["TenDanhMuc"].Value.ToString();
                nmSoLuong.Text = dataGridView.Cells["SoLuongNhap"].Value.ToString();
                cbNhaCC.Text = dataGridView.Cells["TenNCC"].Value.ToString();
                dtNgayNhap.Text = dataGridView.Cells["NgayNhap"].Value.ToString();
                cbNguoiNhap.Text = dataGridView.Cells["DisplayName"].Value.ToString();

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMaNhap.Text = string.Empty;
            cbTenHang.Text = string.Empty;  
            cbDanhMuc.Text = string.Empty;
            nmSoLuong.Text = "1";
            cbNhaCC.Text = string.Empty;
            dtNgayNhap.Text = DateTime.Now.ToString();
            cbNguoiNhap.Text = string.Empty;
        }

        private void btnCTNhap_Click(object sender, EventArgs e)
        {
            showFormCon(new CTNhap());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"INSERT INTO CT_NhapHang (IdSanPam, IdNhapHang, SoLuong, ThanhTien)
                                     SELECT  pro.IdSanPham,  IdNhapHang, NH.SoLuongNhap, (NH.SoLuongNhap * pro.GiaNhapVao) AS ThanhTien  
                                     FROM NhapHang NH 
                                     JOIN Products pro ON NH.IdSanPham = pro.IdSanPham";

                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

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
