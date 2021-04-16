using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class DashBoard : Form
    {
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookShopDb;Integrated Security=True");

        public DashBoard()
        {
            InitializeComponent();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Login flogin = new Login();
            flogin.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Users flogin = new Users();
            flogin.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Materials flogin = new Materials();
            flogin.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            category fCat = new category();
            fCat.Show();
            this.Hide();
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            Con.Open();

            string querty = "SELECT SUM(BQty) FROM BookTbl";
            SqlDataAdapter sda = new SqlDataAdapter(querty, Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            lbMStock.Text = dt.Rows[0][0].ToString();

            string qAmount = "SELECT SUM(Amount) FROM BillTbl";
            SqlDataAdapter sdAm = new SqlDataAdapter(qAmount, Con);
            DataTable dt1 = new DataTable();
            sdAm.Fill(dt1);
            lbqAmount.Text = dt1.Rows[0][0].ToString();

            string qUsers = "SELECT Count(*) FROM UserTbl";
            SqlDataAdapter sUser = new SqlDataAdapter(qUsers, Con);
            DataTable dt2 = new DataTable();
            sUser.Fill(dt2);
            lbUserStok.Text = dt2.Rows[0][0].ToString();


            string q = "SELECT * FROM BillTbl";
            // Заполнение таблицы
            SqlDataAdapter sdvg = new SqlDataAdapter(q, Con);
            DataSet ds = new System.Data.DataSet();
            sdvg.Fill(ds);
            datagridDash.DataSource = ds.Tables["Table"].DefaultView;

            // Заполнение шапки таблицы
            datagridDash.Columns[0].Visible = false;
            datagridDash.Columns[1].HeaderText = "Пользователь";
            datagridDash.Columns[2].HeaderText = "Клиент";
            datagridDash.Columns[3].HeaderText = "Колличество";

            Con.Close();

        }

        
    }
}
