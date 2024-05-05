using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SuperMarketE_Mart.Login;
using System.Data.SqlClient;

namespace SuperMarketE_Mart
{

    public partial class MainForm : Form
    {
        private const string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";

        public MainForm()
        {
            InitializeComponent();
            CustomizeSetting();
            CustomizeInfor();
            CustomizeChangePass();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private object displayname;
        private object vaitro;
        private object maNV;
        private object email;
        private object soDT;
        private object joindate;

        public void DisplayUserInfo(string displayname, string vaitro, string maNV, string email, string soDT, string joindate)
        {
            this.displayname = displayname;
            this.vaitro = vaitro;
            this.maNV = maNV;
            this.email = email;
            this.soDT = soDT;
            this.joindate = joindate;
            lblUser.Text = displayname;
            lblVaiTro.Text = vaitro;

            txtIdNhanVien.Text = maNV;
            txtTenNhanVien.Text = displayname;
            txtEmail.Text = email;
            txtSDT.Text = soDT;
            txtJoinDate.Text = joindate;
            txtRole.Text = vaitro;
        }


        private Form currentFormchild;

        private void showChildFrom(Form childForm)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
            currentFormchild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlBody.Controls.Add(childForm);
            pnlBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            lblUser.Text = "?";
            lblVaiTro.Text = "?";
            buttonLog.Visible = true;

            btnLogout.Enabled = false;
            btnQLHang.Enabled = false;
            btnKiemSoat.Enabled = false;
            btnNCC.Enabled = false;
            btnThongKe.Enabled = false;
            btnVanChuyen.Enabled = false;
            picIndividual.Enabled = false;

            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }

            CustomizeSetting();
            CustomizeInfor();
            CustomizeChangePass();
        }

        private void picBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();

            btnLogout.Enabled = true;
            btnQLHang.Enabled = true;
            btnKiemSoat.Enabled = true;
            btnNCC.Enabled = true;
            btnThongKe.Enabled = true;
            btnVanChuyen.Enabled = true;
            picIndividual.Enabled = true;

            if (btnLogout.Enabled == true)
            {
                buttonLog.Visible = false;
            }
            else
            {
                buttonLog.Visible = true;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }

            btnHome.BackColor = Color.Teal;
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            showChildFrom(new QL_NhaCC());
            lblGeneral.Text = btnNCC.Text;

            btnHome.BackColor = Color.FromArgb(20, 30, 30);
            btnNCC.BackColor = Color.Teal;
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }


        private bool isPanelCollapse = false;
        private void btnQLHang_Click(object sender, EventArgs e)
        {
            timer1.Start();

            btnHome.BackColor = Color.FromArgb(20, 30, 30);
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.Teal;
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnKiemSoat_Click(object sender, EventArgs e)
        {
            btnHome.BackColor = Color.FromArgb(20, 30, 30);
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.Teal;
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnVanChuyen_Click(object sender, EventArgs e)
        {
            btnHome.BackColor = Color.FromArgb(20, 30, 30);
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.Teal;
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            btnHome.BackColor = Color.FromArgb(20, 30, 30);
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.Teal;
        }

        private void CustomizeSetting()
        {
            pnlSetting.Visible = false;
        }

        private void CustomizeInfor()
        {
            pnlInfor.Visible = false;
        }

        private void CustomizeChangePass()
        {
            pnlChangePass.Visible = false;
        }

        private void HideSetting()
        {
            if (pnlSetting.Visible == true)
            {
                pnlSetting.Visible = false;
            }
        }

        private void HidePanelInfor()
        {
            if (pnlInfor.Visible == true)
            {
                pnlInfor.Visible = false;
            }
        }

        private void HidePnlChangePass()
        {
            if (pnlChangePass.Visible == true) 
            { 
                pnlChangePass.Visible = false;
            }
        }

        private void ShowSetting(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                HideSetting();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;    
            }
        }

        private void ShowPanelInfor(Panel panelInfor)
        {
            if (panelInfor.Visible == false)
            {
                HidePanelInfor();
                panelInfor.Visible = true;
            }
            else
            {
                panelInfor.Visible = false;
            }
        }

        private void ShowPnlChangePass(Panel panelPass)
        {
            if (panelPass.Visible == false)
            {
                HidePnlChangePass();
                panelPass.Visible = true;
            }
            else
            {
                panelPass.Visible = false;
            }
        }

        private void picIndividual_Click(object sender, EventArgs e)
        {
            ShowSetting(pnlSetting);
        }

        private void btnPersonalInfo_Click(object sender, EventArgs e)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
            HideSetting();
            ShowPanelInfor(pnlInfor);
            HidePnlChangePass() ;

            btnHome.BackColor = Color.Teal;
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
            HideSetting();
            ShowPnlChangePass(pnlChangePass);
            HidePanelInfor();

            btnHome.BackColor = Color.Teal;
            btnNCC.BackColor = Color.FromArgb(20, 30, 30);
            btnQLHang.BackColor = Color.FromArgb(20, 30, 30);
            btnKiemSoat.BackColor = Color.FromArgb(20, 30, 30);
            btnVanChuyen.BackColor = Color.FromArgb(20, 30, 30);
            btnThongKe.BackColor = Color.FromArgb(20, 30, 30);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            HidePanelInfor();
            ShowSetting(pnlSetting);
        }

        private void btnPassCancel_Click(object sender, EventArgs e)
        {
            HidePnlChangePass();
            ShowSetting(pnlSetting);
        }

        private void btnPassOk_Click(object sender, EventArgs e)
        {
            string userName = txtCPUsername.Text;
            string currentPassword = txtCurrentPass.Text;
            string newPassword = txtNewPass.Text;
            string confirmPassword = txtConfirmPass.Text;

            if (userName == "" && currentPassword == "" && newPassword == "" && confirmPassword == "")
            {
                MessageBox.Show("Please Fill in the Fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New Password & Confirm Password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (!VerifyCurrentPassword(userName, currentPassword))
            {
                MessageBox.Show("Current Password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtNewPass.Text.Length >= 8)
            {
                ChangePassword(userName, currentPassword);
                MessageBox.Show("Changed Password successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The Length must be 8 characters or more!!!.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            HidePnlChangePass();
        }

        // Kiểm tra mật khẩu hiện tại.
        private bool VerifyCurrentPassword(string userName, string currentPassword)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ACCOUNT WHERE Username = @username AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", txtCPUsername.Text);
                command.Parameters.AddWithValue("@Password", txtCurrentPass.Text);

                int count = (int)command.ExecuteScalar();
                connection.Close();

                return count > 0;
            }
        }

        //Thực hiện thay đổi.
        private void ChangePassword(string userName, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE ACCOUNT SET Password = @NewPassword WHERE Username = @username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", txtCPUsername.Text);
                command.Parameters.AddWithValue("@NewPassword", txtNewPass.Text);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private bool isCollapsed;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                pnlDropdown.Height += 15;
                if (pnlDropdown.Size == pnlDropdown.MaximumSize)
                {
                    timer1.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                pnlDropdown.Height -= 15;
                if (pnlDropdown.Size == pnlDropdown.MinimumSize)
                {
                    timer1.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void btnHangHoa_Click(object sender, EventArgs e)
        {
            showChildFrom(new QL_Hang());
            lblGeneral.Text = btnQLHang.Text;
            timer1.Start();
        }

        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            timer1.Start();

            showChildFrom(new NhapHangForm());
            lblGeneral.Text = btnNhapHang.Text;
        }

        private void btnXuatHang_Click(object sender, EventArgs e)
        {
            timer1.Start();

            showChildFrom(new XuatHangForm());
            lblGeneral.Text= btnXuatHang.Text;
        }

        private void btnQLDanhMuc_Click(object sender, EventArgs e)
        {
            timer1.Start();
            showChildFrom(new CategoryForm());
            lblGeneral.Text = btnQLDanhMuc.Text;
        }

        private void btnHangTon_Click(object sender, EventArgs e)
        {
            timer1.Start();
            showChildFrom(new HangTonKho());
            lblGeneral.Text = btnHangTon.Text;
        }
    }
}
