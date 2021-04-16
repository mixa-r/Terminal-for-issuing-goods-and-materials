using BoolTest.DataBase;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Forms;

namespace BoolTest
{
    public partial class category : Form
    {
        public category()
        {
            InitializeComponent();
        }

        ModelKeyBTcontext db;

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (txtNameCategoty.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    // Создаём экземпляр класса Справочник_сотрудники,
                    // т.е получаем данные о нашем сотруднике из текстовых полей
                    Seria cat = new Seria();
                    {
                        cat.Name = txtNameCategoty.Text;
                    }

                    // Заносим данные в нашу таблицу
                    db.Serias.Add(cat);
                    // Обязательно сохраняем изменения
                    db.SaveChanges();
                    // Обновляем наш dataGridView, чтобы в нём отобразилась новая страна
                    dvgCategory.Refresh();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        void Reset()
        {
            txtNameCategoty.Text = "";
            txtID.Text = "";
            txtSearch.Text = "";
        }

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
                // Получаем id из текстового поля
                int id = Convert.ToInt32(txtID.Text);
                // Находим запись по этому id с помощью метода Find()
                Seria cat = db.Serias.Find(id);
                if (cat == null) return;
                cat.Name = txtNameCategoty.Text;
                // Добавляем или обновляем запись
                db.Serias.AddOrUpdate(cat);
                // Сохраняем в бузу данных
                db.SaveChanges();
                // Обновляем таблицу с данными
                dvgCategory.Refresh();
                Reset();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбрана запись
            if (txtID.Text == String.Empty)
            {
                MessageBox.Show("Не выбрана запись!", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //
            if (MessageBox.Show("Удалить запись?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Получаем id из текстового поля
                int id = Convert.ToInt32(txtID.Text);
                // Находим запись по этому id с помощью метода Find()
                Seria cat = db.Serias.Find(id);
                if (cat == null) return;
                //Если запись выбрана, вносим значения в поля
                cat.Name = txtNameCategoty.Text;
                // Удаляем запись из базы данных
                db.Serias.Remove(cat);
                // Сохраняем в бузу данных
                db.SaveChanges();
                // Обновляем таблицу с данными
                dvgCategory.Refresh();
                Reset();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            db = new ModelKeyBTcontext();
            // Загружаем данные из таблицы в кэш
            db.Serias.Load();
            // Привязываем данные к dataGridView
            BindingList<DataBase.Materials> list = db.BookTbls.Local.ToBindingList();

            dvgCategory.DataSource = list;
            dvgCategory.RowHeadersVisible = false;

            // Запрос поиска 
            dvgCategory.DataSource = db.Serias.Where(x => x.Id.ToString().Contains(txtSearch.Text)
                || x.Name.Contains(txtSearch.Text)).ToList();
        }

        private void dvgCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверка выборки строк
            // Если строка не выбрана, то дальше ничего не происходит
            if (dvgCategory.CurrentRow == null) return;

            // Получаем выделенную строку и приводим её у типу Countries
            Seria location = dvgCategory.CurrentRow.DataBoundItem as Seria;

            // Если мы щёлкаем по пустой строке, то ничего дальше не делаем
            if (location == null) return;

            txtID.Text = location.Id.ToString();
            txtNameCategoty.Text = dvgCategory.SelectedRows[0].Cells[1].Value.ToString();
            
        }

        private void category_Load(object sender, EventArgs e)
        {
            using (db)
            {
                // Создаём объект нашего контекста
                db = new ModelKeyBTcontext();
                // Загружаем данные из таблицы в кэш
                db.Serias.Load();
                // Привязываем данные к dataGridView
                dvgCategory.DataSource = db.Serias.Local.ToBindingList();

                dvgCategory.Columns[0].Width = 70;
                dvgCategory.Columns[0].HeaderText = "ID";
                dvgCategory.Columns[1].HeaderText = "Категория";
                
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Materials flogin = new Materials();
            flogin.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Users userForm = new Users();
            userForm.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            DashBoard flogin = new DashBoard();
            flogin.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Login flogin = new Login();
            flogin.Show();
            this.Hide();
        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
