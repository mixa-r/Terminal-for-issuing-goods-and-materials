using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookShopDb;Integrated Security=True");

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static string UserName = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string querty = "SELECT COUNT (*) FROM UserTbl where Uname=N'" + UnameTb.Text + "' and UPass=N'" + UPassTb.Text + "'";
            Con.Open();

            SqlDataAdapter sda = new SqlDataAdapter(querty, Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                UserName = UnameTb.Text;
                Billing obj = new Billing();
                obj.Show();
                this.Hide();
                Con.Close();
            }
            else
            {
                MessageBox.Show("Wrong your password");
            }

            Con.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            AdminLoginForm admFrom = new AdminLoginForm();
            admFrom.Show();
            this.Hide();
        }
    }
}
