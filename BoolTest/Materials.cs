using BoolTest.DataBase;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class Materials : Form
    {
        public Materials()
        {
            InitializeComponent();
            populate();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookShopDb;Integrated Security=True");
        ModelKeyBTcontext db;

        private void populate()
        {
            using (db)
            {
                // Создаём объект нашего контекста
                db = new ModelKeyBTcontext();
                // Загружаем данные из таблицы в кэш
                db.BookTbls.Load();
                var categoryBox = db.Serias.ToList();
                foreach(var item in categoryBox)
                {
                    CatCbSearchCb.Items.Add(item.Name);
                    BcatCb.Items.Add(item.Name);
                }

                // Привязываем данные к dataGridView
                BookDGV.DataSource = db.BookTbls.Local.ToBindingList();
                txtID.Enabled = false;

                // Заполнение шапки таблицы
                BookDGV.Columns[0].Visible = false;
                BookDGV.Columns[1].HeaderText = "Название";
                BookDGV.Columns[2].HeaderText = "Автор";
                BookDGV.Columns[3].HeaderText = "Категория";
                BookDGV.Columns[4].HeaderText = "Количество";
                BookDGV.Columns[5].HeaderText = "Цена";

                CatCbSearchCb.SelectedIndex = -1;
            }
        }

        // Метод фильр по категориям
        private void Filter()
        {
            Con.Open();
            string quertyFilter = "SELECT * FROM BookTbl WHERE BCat=N'" + CatCbSearchCb.SelectedItem.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(quertyFilter, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        // Добваить запись в таблицу
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (BTitleTb.Text == "" || BautTb.Text == "" || QtyTb.Text == "" || PriceTb.Text == "" || BcatCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "INSERT INTO BookTbl values (N'" + BTitleTb.Text + "', N'" + BautTb.Text + "', N'" + BcatCb.SelectedItem.ToString() + "'," +
                        "N'" + QtyTb.Text + "', N'" + PriceTb.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Save book Good");
                    Con.Close();
                    populate();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        // Закрыть программу
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Действия при выборе категории (поиск по категории)
        private void CatCbSearchCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter();
        }

        // Обновить данные в таблице
        private void button5_Click(object sender, EventArgs e)
        {
            populate();
            CatCbSearchCb.SelectedIndex = -1;
        }

        // метод очистки текстовых полей
        private void Reset()
        {
            BTitleTb.Text = "";
            BautTb.Text = "";
            BcatCb.SelectedIndex = -1;
            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        // Очистить текствоые поля
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // Удалить запись из таблицы
        private void DeleleBtn_Click(object sender, EventArgs e)
        {
            if (BookDGV.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string id = BookDGV.SelectedRows[0].Cells[0].Value.ToString();
                    Con.Open();
                    SqlCommand cmd = new SqlCommand(@"DELETE FROM BookTbl WHERE Bid = @id", Con);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    populate();
                }
            }
        }

        // Изменить запись в таблице
        private void EditBtn_Click(object sender, EventArgs e)
        {
            try
            {

                // Проверяем, что выбрана запись
                if (txtID.Text == String.Empty)
                {
                    MessageBox.Show("Не выбрана запись!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                string id = BookDGV.SelectedRows[0].Cells[0].Value.ToString();
                Con.Open();
                SqlCommand query = new SqlCommand("UPDATE BookTbl SET BTitle = @BTitle, BAuthor=@BAuthor, BCat=@BCat, BQty=@BQty, BPrice=@BPrice WHERE Bid = @id", Con);

                query.Parameters.AddWithValue("@BTitle", BTitleTb.Text);
                query.Parameters.AddWithValue("@BAuthor", BautTb.Text);
                query.Parameters.AddWithValue("@BCat", BcatCb.Text);
                query.Parameters.AddWithValue("@BQty", QtyTb.Text);
                query.Parameters.AddWithValue("@BPrice", PriceTb.Text);
                query.Parameters.AddWithValue("id", id);

                query.ExecuteNonQuery();
                Con.Close();
                populate();
                Reset();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }


        public int BookId;
        private void BookDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверка выборки строк
            // Если строка не выбрана, то дальше ничего не происходит
            if (BookDGV.CurrentRow == null) return;

            // Получаем выделенную строку и приводим её у типу Countries
            DataBase.Materials location = BookDGV.CurrentRow.DataBoundItem as DataBase.Materials;

            // Если мы щёлкаем по пустой строке, то ничего дальше не делаем
            if (location == null) return;

            txtID.Text = location.BId.ToString();
            BookId = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value);
            BTitleTb.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            BautTb.Text = BookDGV.SelectedRows[0].Cells[2].Value.ToString();
            BcatCb.SelectedItem = BookDGV.SelectedRows[0].Cells[3].Value.ToString();
            QtyTb.Text = BookDGV.SelectedRows[0].Cells[4].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Login flogin = new Login();
            flogin.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Users flogin = new Users();
            flogin.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            DashBoard flogin = new DashBoard();
            flogin.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            category fCat = new category();
            fCat.Show();
            this.Hide();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            db = new ModelKeyBTcontext();
            // Загружаем данные из таблицы в кэш
            db.BookTbls.Load();
            // Привязываем данные к dataGridView
            BindingList<DataBase.Materials> list = db.BookTbls.Local.ToBindingList();

            BookDGV.DataSource = list;
            BookDGV.RowHeadersVisible = false;

            // Запрос поиска 
            BookDGV.DataSource = db.BookTbls.Where(x => x.BId.ToString().Contains(txtSearch.Text)
                || x.BTitle.ToString().Contains(txtSearch.Text)
                || x.BAuthor.Contains(txtSearch.Text)).ToList();
        }

        
    }
}
