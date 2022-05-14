using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static АРМ_билиотекаря.DatabaseAdapter;

namespace АРМ_билиотекаря
{
    public partial class DBChooser : Form
    {
        private Form1 form = null;
        public DBChooser()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            String addressString = address.Text;
            String databaseNameString = name.Text;
            String dbUser = username.Text;
            String dbPasswordString = pswd.Text;
            if (form == null)
            {
                form = new Form1(this);
            }
            try { 
                form.updateConnection(addressString, databaseNameString, dbUser, dbPasswordString);
            } 
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string userUsername = user_name.Text;
            string surname = user_surname.Text;
            string password = user_password.Text;

            var res = form.getUserId(userUsername, surname, password);

            if(res is Success)
            {
                form.updateUser(((Success)res).userId, ((Success)res).isAdmin);
            } 
            else if(res is UserNotFound) 
            {
                MessageBox.Show("Пользователь с такими данными не найден", "Неверное имя или фамилия", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 
            else if(res is WrongPassword)
            {

                MessageBox.Show("Проверьте правильность введенного пароля", "Неверный пароль", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            form.Show();
            Hide();
        }

        private void DBChooser_Load(object sender, EventArgs e)
        {
            //button1_Click_1(null, null);
        }
    }
}
