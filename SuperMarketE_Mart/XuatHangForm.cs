using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarketE_Mart
{
    public partial class XuatHangForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public XuatHangForm()
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
            pnlMainXuat.Controls.Add(FormCon);
            pnlMainXuat.Tag = FormCon;
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

        private void XuatHangForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"SELECT pro.TenSanPham, DM.TenDanhMuc, pro.NgaySX, NCC.TenNCC, pro.SoLuong
                                    FROM Products pro
                                    JOIN TB_DanhMucSP DM ON pro.IdDanhMucSP = DM.IdDanhMuc
                                    Join TB_NhaCungCap NCC ON pro.IdNhaSX = NCC.IdNCC";
                    SqlCommand command = new SqlCommand(query, connection); 

                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgvHangTon.DataSource = dataTable;

                    dgvHangTon.Columns[0].HeaderText = "Tên Sản Phẩm";
                    dgvHangTon.Columns[1].HeaderText = "Danh Mục";
                    dgvHangTon.Columns[2].HeaderText = "Ngày Sản Xuất";
                    dgvHangTon.Columns[3].HeaderText = "Nhà Sản Xuất";
                    dgvHangTon.Columns[4].HeaderText = "Số Lượng Tồn";

                    DataTable data = getGoodsName();
                    cbTenSP.DataSource = data;
                    cbTenSP.DisplayMember = "TenSanPham";
                    cbTenSP.ValueMember = "IdSanPham";

                    DataTable dataTB = getDanhMuc();
                    cbDanhMuc.DataSource = dataTB;
                    cbDanhMuc.DisplayMember = "TenDanhMuc";
                    cbDanhMuc.ValueMember = "IdDanhMuc";

                    DataTable table = getNameImporter();
                    cbNguoiXuat.DataSource = table;
                    cbNguoiXuat.DisplayMember = "DisplayName";
                    cbNguoiXuat.ValueMember = "ID";


                    string XuatHang = @"SELECT  XH.IdXuatHang,  pro.TenSanPham,  XH.SoLuongXuat,  DM.TenDanhMuc, XH.NgayNhap, AC.DisplayName   
                                        FROM XuatHang XH 
                                        JOIN Products pro ON XH.IdSanPham = pro.IdSanPham
                                        JOIN TB_DanhMucSP DM ON XH.IdDanhMucSP = DM.IdDanhMuc 
                                        JOIN ACCOUNT AC ON XH.IdNguoiXuat = AC.ID";
                    SqlCommand cmd = new SqlCommand(XuatHang, connection);

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    DataTable DataTB = new DataTable();
                    DataTB.Load(dataReader);
                    dgvHangXuat.DataSource = DataTB;

                    dgvHangXuat.Columns[0].HeaderText = "Mã Xuất Hàng";
                    dgvHangXuat.Columns[1].HeaderText = "Tên Sản Phẩm";
                    dgvHangXuat.Columns[2].HeaderText = "Số Lượng";
                    dgvHangXuat.Columns[3].HeaderText = "Danh Mục";
                    dgvHangXuat.Columns[4].HeaderText = "Ngày Xuất";
                    dgvHangXuat.Columns[5].HeaderText = "Người Xuất";

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Lỗi kết nối dữ liệu " + ex.Message);
            }
        }

        private void btnThemDSXuat_Click(object sender, EventArgs e)
        {
            if (txtMaXuat.Text == "" && cbTenSP.Text == "" && cbNguoiXuat.Text == "" && cbDanhMuc.Text == "")
            {
                MessageBox.Show("Please enter Information follow the Fields.", "Information", MessageBoxButtons.OK);
                txtMaXuat.Focus();
                return;
            }
            if (txtMaXuat.Text == "")
            {
                MessageBox.Show("Please enter Ma Xuat Hang!!", "Information", MessageBoxButtons.OK);
                txtMaXuat.Focus();
                return;
            }

            if (cbTenSP.Text == "")
            {
                MessageBox.Show("Ten Hang has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbTenSP.Focus();
                return;
            }

            if (cbDanhMuc.Text == "")
            {
                MessageBox.Show("Danh Mục has no Information yet!!", "Information", MessageBoxButtons.OK);
                cbDanhMuc.Focus();
                return;
            }

            if (nmSoLuong.Text == "0")
            {
                MessageBox.Show("Please enter Quantity!!", "Information", MessageBoxButtons.OK);
                nmSoLuong.Focus();
                return;
            }

            if (cbNguoiXuat.Text == "")
            {
                MessageBox.Show("Please enter Importer!!", "Information", MessageBoxButtons.OK);
                cbNguoiXuat.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();


                    SqlCommand command = new SqlCommand("XuatHangProc", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdXuatHang", txtMaXuat.Text);
                    command.Parameters.AddWithValue("@TenSanPham", cbTenSP.Text);
                    command.Parameters.AddWithValue("@SoLuongXuat", nmSoLuong.Text);
                    command.Parameters.AddWithValue("@TenDanhMuc", cbDanhMuc.Text);
                    command.Parameters.AddWithValue("@NgayXuat", DateTime.Now);
                    command.Parameters.AddWithValue("@TenNguoiXuat", cbNguoiXuat.Text);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    XuatHangForm_Load(sender, e);

                    MessageBox.Show("Added Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void dgvHangXuat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRefresh_Click(sender, e);

            if (e.RowIndex >= 0 && e.RowIndex < dgvHangXuat.Rows.Count)
            {
                DataGridViewRow dataGridView = dgvHangXuat.Rows[e.RowIndex];

                txtMaXuat.Text = dataGridView.Cells["IdXuatHang"].Value.ToString();
                cbTenSP.Text = dataGridView.Cells["TenSanPham"].Value.ToString();
                nmSoLuong.Text = dataGridView.Cells["SoLuongXuat"].Value.ToString();
                cbDanhMuc.Text = dataGridView.Cells["TenDanhMuc"].Value.ToString();
                dtNgayXuat.Text = dataGridView.Cells["NgayNhap"].Value.ToString();
                cbNguoiXuat.Text = dataGridView.Cells["DisplayName"].Value.ToString();

            }
        }

        private void dgvHangTon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRefresh_Click(sender, e);

            if (e.RowIndex >= 0 && e.RowIndex < dgvHangTon.Rows.Count)
            {
                DataGridViewRow dataGridView = dgvHangTon.Rows[e.RowIndex];

                cbTenSP.Text = dataGridView.Cells["TenSanPham"].Value.ToString();
                cbDanhMuc.Text = dataGridView.Cells["TenDanhMuc"].Value.ToString();
                nmSoLuong.Text = dataGridView.Cells["SoLuong"].Value.ToString();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Nhập tên sản phẩm...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Nhập tên sản phẩm...";
                txtSearch.ForeColor = Color.DarkGray;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtMaXuat.Text = string.Empty;
            cbTenSP.Text = string.Empty;
            cbDanhMuc.Text = string.Empty;
            cbNguoiXuat.Text = string.Empty;
            dtNgayXuat.Text = string.Empty;
            nmSoLuong.Text = "0";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaXuat.Text == "")
                {
                    MessageBox.Show("Please choose one data from Bảng danh sách Xuất Hàng to Delete", "Information", MessageBoxButtons.OK);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    string DeleteXuatHang = "DELETE FROM XuatHang WHERE IdXuatHang = '" + txtMaXuat.Text + "'";

                    SqlCommand command = new SqlCommand(DeleteXuatHang, conn);

                    command.ExecuteNonQuery();

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    XuatHangForm_Load(sender, e);

                    MessageBox.Show("Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }

            btnRefresh_Click(sender, e);
        }

        private void btnChiTietXuat_Click(object sender, EventArgs e)
        {
            showFormCon(new CTXuat());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    string query = @"INSERT INTO CT_XuatHang (IdSanPam, IdXuatHang, SoLuong, ThanhTien)
                                     SELECT  pro.IdSanPham,  IdXuatHang, XH.SoLuongXuat, (XH.SoLuongXuat * pro.GiaBanRa) AS ThanhTien  
                                     FROM XuatHang XH 
                                     JOIN Products pro ON XH.IdSanPham = pro.IdSanPham";

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
