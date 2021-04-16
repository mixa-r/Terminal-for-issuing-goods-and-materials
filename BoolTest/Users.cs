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
    public partial class Users : Form
    {
        ModelKeyBTcontext db;

        public Users()
        {
            InitializeComponent();
            populate();
        }

        // Закрыть программу
        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // добавить данные в таблицу
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (UnameTb.Text == "" || PhoneTb.Text == "" || AddTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    // Создаём экземпляр класса Справочник_сотрудники,
                    // т.е получаем данные о нашем сотруднике из текстовых полей
                    UserTbl user = new UserTbl();
                    {
                        user.UName = UnameTb.Text;
                        user.UPhone = PhoneTb.Text;
                        user.UAdd = AddTb.Text;
                        user.UPass = PassTb.Text;
                    }

                    // Заносим данные в нашу таблицу
                    db.UserTbls.Add(user);
                    // Обязательно сохраняем изменения
                    db.SaveChanges();
                    // Обновляем наш dataGridView, чтобы в нём отобразилась новая страна
                    UserDGV.Refresh();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        // изменить данные в таблице
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
                UserTbl user = db.UserTbls.Find(id);
                if (user == null) return;
                user.UName = UnameTb.Text;
                user.UPhone = PhoneTb.Text;
                user.UAdd = AddTb.Text;
                user.UPass = PassTb.Text;
                // Добавляем или обновляем запись
                db.UserTbls.AddOrUpdate(user);
                // Сохраняем в бузу данных
                db.SaveChanges();
                // Обновляем таблицу с данными
                UserDGV.Refresh();
                Reset();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        // Удалить данные из таблицы
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
                UserTbl user = db.UserTbls.Find(id);
                if (user == null) return;
                //Если запись выбрана, вносим значения в поля
                user.UName = UnameTb.Text;
                user.UPhone = PhoneTb.Text;
                user.UAdd = AddTb.Text;
                user.UPass = PassTb.Text;
                // Удаляем запись из базы данных
                db.UserTbls.Remove(user);
                // Сохраняем в бузу данных
                db.SaveChanges();
                // Обновляем таблицу с данными
                UserDGV.Refresh();
                Reset();
            }
        }

        // Кнопка очистить
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        // Метод очистки текстовых полей
        private void Reset()
        {
            UnameTb.Text = "";
            PhoneTb.Text = "";
            AddTb.Text = "";
            PassTb.Text = "";
        }

        //Метод загрузки данных в таблицу 
        private void populate()
        {

            // Открываем и закрываем соединение с базой данных с использованием очисти памяти
            using (db)
            {
                // Создаём объект нашего контекста
                db = new ModelKeyBTcontext();
                // Загружаем данные из таблицы в кэш
                db.UserTbls.Load();
                // Привязываем данные к dataGridView
                UserDGV.DataSource = db.UserTbls.Local.ToBindingList();

                UserDGV.Columns[0].Visible = false;
                UserDGV.Columns[1].HeaderText = "ФИО";
                UserDGV.Columns[2].HeaderText = "Телефон";
                UserDGV.Columns[3].HeaderText = "Адрес";
                UserDGV.Columns[4].HeaderText = "Пароль";
            }
        }

        // Действия при выборе ячейки в таблице
        private void UserDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {   // Проверка выборки строк
            // Если строка не выбрана, то дальше ничего не происходит
            if (UserDGV.CurrentRow == null) return;

            // Получаем выделенную строку и приводим её у типу Countries
            UserTbl location = UserDGV.CurrentRow.DataBoundItem as UserTbl;

            // Если мы щёлкаем по пустой строке, то ничего дальше не делаем
            if (location == null) return;

            txtID.Text = location.UId.ToString();
            UnameTb.Text = UserDGV.SelectedRows[0].Cells[1].Value.ToString();
            PhoneTb.Text = UserDGV.SelectedRows[0].Cells[2].Value.ToString();
            AddTb.Text = UserDGV.SelectedRows[0].Cells[3].Value.ToString();
            PassTb.Text = UserDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        // Открыть форму авторизации
        private void label11_Click(object sender, EventArgs e)
        {
            Login flogin = new Login();
            flogin.Show();
            this.Hide();
        }

        // Открыть форму справочника материалы
        private void label7_Click(object sender, EventArgs e)
        {
            Materials flogin = new Materials();
            flogin.Show();
            this.Hide();
        }

        // Открыть форму справочника статистика
        private void label10_Click(object sender, EventArgs e)
        {
            DashBoard flogin = new DashBoard();
            flogin.Show();
            this.Hide();
        }

        // Открыть форму справочника категории
        private void label14_Click(object sender, EventArgs e)
        {
            category fCat = new category();
            fCat.Show();
            this.Hide();
        }

        // Строка поиска
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            db = new ModelKeyBTcontext();
            // Загружаем данные из таблицы в кэш
            db.UserTbls.Load();
            // Привязываем данные к dataGridView
            BindingList<DataBase.Materials> list = db.BookTbls.Local.ToBindingList();

            UserDGV.DataSource = list;
            UserDGV.RowHeadersVisible = false;

            // Запрос поиска 
            UserDGV.DataSource = db.UserTbls.Where(x => x.UId.ToString().Contains(txtSearch.Text)
                || x.UName.ToString().Contains(txtSearch.Text)
                || x.UAdd.ToString().Contains(txtSearch.Text)
                || x.UPhone.Contains(txtSearch.Text)).ToList();
        }
    }
}
