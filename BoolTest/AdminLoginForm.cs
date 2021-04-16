using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class AdminLoginForm : Form
    {
        public AdminLoginForm()
        {
            InitializeComponent();
        }

        // Завершение работы программы
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Открыть форму авторизации Пользователя
        private void label4_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }


        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookShopDb;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "admin" && UPassTb.Text == "123")
            {
                string querty = "SELECT COUNT (*) FROM UserTbl where Uname=N'" + UnameTb.Text + "' and UPass=N'" + UPassTb.Text + "'";

                Con.Open();

                SqlDataAdapter sda = new SqlDataAdapter(querty, Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    Materials obj = new Materials();
                    obj.Show();
                    this.Hide();
                    Con.Close();
                }
            }
            else
            {
                MessageBox.Show("Wrong your password");
            }

            Con.Close();
        }
    }
}
