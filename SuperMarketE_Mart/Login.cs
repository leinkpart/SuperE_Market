using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SuperMarketE_Mart
{
    public partial class Login : Form
    {


        private const string connectionString = "Data Source=LAPTOP-NK68S4GM\\SQLEXPRESS01;Initial Catalog=SieuThiE_Market;Integrated Security=True";
        private const int MAX_LOGIN_ATTEMPTS = 5;
        private int loginAttempts = 0;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = textuser.Text;
            string password = textpass.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //Gọi thủ tục để kiểm tra đăng nhập.
                SqlCommand command = new SqlCommand("CheckedLogin", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                string result = command.ExecuteScalar() as string;

                if (result == "Login successful")
                {
                    MessageBox.Show("Đăng nhập thành công!");

                    //Lấy tên hiển thị của tài khoản đăng nhập vào.
                    string query = "SELECT DisplayName FROM ACCOUNT WHERE Username = @User";
                    SqlCommand sqlcommand = new SqlCommand(query, connection);

                    sqlcommand.Parameters.AddWithValue("@User", username);

                    string displayname = sqlcommand.ExecuteScalar().ToString();

                    //Lấy vai trò của tài khoản đăng nhập vào.
                    query = "SELECT VaiTro FROM ACCOUNT WHERE Username = @User";
                    sqlcommand = new SqlCommand(query, connection);

                    sqlcommand.Parameters.AddWithValue("@User", username);

                    
                    string vaitro = sqlcommand.ExecuteScalar().ToString();

                    this.Close();

                    MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
                    if (mainForm != null)
                    {
                        mainForm.DisplayUserInfo(displayname, vaitro);
                        mainForm.Show();

                    }
                    else
                    {
                        loginAttempts++;

                        if (textuser.Text.Length == 0 && textpass.Text.Length == 0)
                        {
                            MessageBox.Show("Please Enter the field.");
                        }
                        else if (loginAttempts == 1 || loginAttempts == 2)
                        {
                            MessageBox.Show("Sai mật khẩu/tên người dùng");
                        }

                        if (loginAttempts == 4 || loginAttempts == 3)
                        {
                            MessageBox.Show("Bạn đã nhập sai mật khẩu/tên người dùng " + loginAttempts + " lần. Lưu ý: Còn " + (MAX_LOGIN_ATTEMPTS - loginAttempts) + " lần thử!");
                        }

                        if (loginAttempts == MAX_LOGIN_ATTEMPTS)
                        {
                            MessageBox.Show("Bạn đã nhập sai mật khẩu/tên người dùng " + MAX_LOGIN_ATTEMPTS + " lần. From này sẽ tự đóng.");
                            Application.Exit();
                        }
                    }
                    connection.Close();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
