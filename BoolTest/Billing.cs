using BoolTest.DataBase;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookShopDb;Integrated Security=True");

        // Инициализируем объект нашего контекста
        ModelKeyBTcontext db;

        private void populate()
        {
            using (db)
            {
                // Создаём объект нашего контекста
                db = new ModelKeyBTcontext();
                // Загружаем данные из таблицы в кэш
                db.BookTbls.Load();

                // Привязываем данные к dataGridView
                BookDGV.DataSource = db.BookTbls.Local.ToBindingList();

                // Заполнение шапки таблицы
                BookDGV.RowHeadersVisible = false;
                BookDGV.Columns[0].HeaderText = "ID";
                BookDGV.Columns[1].HeaderText = "Наименование";
                BookDGV.Columns[2].HeaderText = "Производитель";
                BookDGV.Columns[3].HeaderText = "Категория";
                BookDGV.Columns[4].HeaderText = "Количество";
                BookDGV.Columns[5].HeaderText = "Цена";
            }
        }

        private void UpdateBook()
        {
            int newQty = stock - Convert.ToInt32(QtyTb.Text);
            try
            {
                if (BookId > 0)
                {
                    string id = BookDGV.SelectedRows[0].Cells[0].Value.ToString();
                    Con.Open();
                    SqlCommand query = new SqlCommand("UPDATE BookTbl SET BQty=@BQty WHERE Bid = @id", Con);

                    query.Parameters.AddWithValue("@BQty", newQty);
                    query.Parameters.AddWithValue("id", id);

                    query.ExecuteNonQuery();
                    Con.Close();
                    populate();
                    //Reset();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }


        int n = 0;
        int Grdtotal = 0;
        int total = 0;
        private void SaveBtn_Click(object sender, EventArgs e)
        {

            if (QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("Превышено колличество по наличию");
            }
            else
            {
                total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDgv);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = BTitle.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDgv.Rows.Add(newRow);
                n++;
                //****************************************
                UpdateBook();
                Reset();
                Grdtotal = Grdtotal + total;
                TotalLbl.Text = Grdtotal + "Руб.";
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public int BookId;
        public int stock = 0;

        // Действие при выборе ячейки
        private void BookDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BookId = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[0].Value);
            BTitle.Text = BookDGV.SelectedRows[0].Cells[1].Value.ToString();
            PriceTb.Text = BookDGV.SelectedRows[0].Cells[5].Value.ToString();
            stock = Convert.ToInt32(BookDGV.SelectedRows[0].Cells[4].Value);
        }

        // очистка полей
        private void btnClear_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // метод очистки полей
        private void Reset()
        {
            txtSearch.Text = "";
            BTitle.Text = "";
            QtyTb.Text = "";
            PriceTb.Text = "";
        }

        // Печать наряда
        private void button2_Click(object sender, EventArgs e)
        {
            if (ClientNameTb.Text == "" || BTitle.Text == "")
            {
                MessageBox.Show("Не заполнено поле Пользователь");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "INSERT INTO BillTbl values (N'" + UserNameLbl.Text + "', N'" + ClientNameTb.Text + "', " + Grdtotal + ")";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

                // Запуск диалогового окна печати документа
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 600, 600);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }

        }

        int prodid, prodqty, prodprice, tottal, pos = 60, posTabl = 80;

        // Удалить позицию из наряда
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in BillDgv.SelectedRows)
            {
                BillDgv.Rows.RemoveAt(row.Index);
            }

            total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PriceTb.Text);
            Grdtotal = Grdtotal - total;
            TotalLbl.Text = Grdtotal + "Руб.";
            QtyTb.Text = "";
        }

        // Действия при выборе ячейки
        private void BillDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BookId = Convert.ToInt32(BillDgv.SelectedRows[0].Cells[0].Value);
            BTitle.Text = BillDgv.SelectedRows[0].Cells[1].Value.ToString();
            QtyTb.Text = BillDgv.SelectedRows[0].Cells[3].Value.ToString();
            PriceTb.Text = BillDgv.SelectedRows[0].Cells[4].Value.ToString();
            //stock = Convert.ToInt32(BillDgv.SelectedRows[0].Cells[4].Value);
        }

        // Поисковая строка
        private void textBox1_TextChanged(object sender, EventArgs e)
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
                || x.BCat.ToString().Contains(txtSearch.Text)
                || x.BAuthor.Contains(txtSearch.Text)).ToList();
        }

        // Открыть форму авторизации
        private void label10_Click(object sender, EventArgs e)
        {
            Login flogin = new Login();
            flogin.Show();
            this.Hide();
        }

        // Передача параметров в форму
        private void Billing_Load(object sender, EventArgs e)
        {
            UserNameLbl.Text = Login.UserName;
            ClientNameTb.Text = Login.UserName;
        }

        string prodname;
        // Метод формирования и печати наряда
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(DateTime.Now.ToString(), new Font("Centery Gothic", 6, FontStyle.Bold), Brushes.Red, new Point(5, 2));
            e.Graphics.DrawString("Надяр на получении ТМЦ", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(200,10));
            e.Graphics.DrawString("Получатель: " + UserNameLbl.Text, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(26, 30));
            e.Graphics.DrawString("ID", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(26, pos + 20));
            e.Graphics.DrawString("Наименование", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(50, pos + 20));
            e.Graphics.DrawString("Цена", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(300, pos + 20));
            e.Graphics.DrawString("Количество", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(400, pos + 20));
            e.Graphics.DrawString("Сумма", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(500, pos + 20));
            foreach (DataGridViewRow row in BillDgv.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                prodname = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column4"].Value);
                tottal = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(26, posTabl + 20));
                e.Graphics.DrawString("" + prodname, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(50, posTabl + 20));
                e.Graphics.DrawString("" + prodprice, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(300, posTabl + 20));
                e.Graphics.DrawString("" + prodqty, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(400, posTabl + 20));
                e.Graphics.DrawString("" + tottal, new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Blue, new Point(500, posTabl + 20));
                posTabl = posTabl + 20;
            }
            e.Graphics.DrawString("Общая сумма: " + Grdtotal + " Руб.", new Font("Centery Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(26, posTabl + 40));
            e.Graphics.DrawString("Выдал _____________________  / Расшифровка _____________________", new Font("Centery Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, posTabl + 70));
            e.Graphics.DrawString("Принял _____________________ / Расшифровка _____________________", new Font("Centery Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, posTabl + 100));
            BillDgv.Rows.Clear();
            BillDgv.Refresh();
            pos = 100;
            Grdtotal = 0;
        }
    }
}
