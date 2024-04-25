using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SuperMarketE_Mart.Login;

namespace SuperMarketE_Mart
{
   
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private object displayname;
        private object vaitro;

        public void DisplayUserInfo(string displayname, string vaitro)
        {
            this.displayname = displayname;
            this.vaitro = vaitro;
            lblUser.Text = displayname;
            lblVaiTro.Text = vaitro;
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

        private void button2_Click(object sender, EventArgs e)
        {
            
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
            button1.Enabled = false;
            button2.Enabled = false;

            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
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
                button1.Enabled = true;
                button2.Enabled = true;

                if (btnLogout.Enabled == true)
                {
                    buttonLog.Visible = false;
                }
                else
                {
                    buttonLog.Visible = true;
                } 
            
        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            showChildFrom(new QL_NhaCC());
            label2.Text = btnNCC.Text;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (currentFormchild != null)
            {
                currentFormchild.Close();
            }
        }

        private void btnQLHang_Click(object sender, EventArgs e)
        {
            showChildFrom(new QL_Hang());
            label2.Text = btnQLHang.Text;
        }
    }
}
