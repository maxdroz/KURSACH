using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АРМ_билиотекаря
{
    public partial class Users : Form
    {
        private readonly Form1 form;

        public Users(Form1 form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void Users_Load(object sender, EventArgs e)
        {
            dataGridView12.DataSource = form.adapter.getAllUsers();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            int active = dataGridView12.CurrentCellAddress.Y;
            int id = Int32.Parse(dataGridView12["id", active].Value.ToString());
            string name = dataGridView12["name", active].Value.ToString();
            string surname = dataGridView12["surname", active].Value.ToString();
            string password = dataGridView12["password_hash", active].Value.ToString();
            bool isAdmin = dataGridView12["is_admin", active].Value.ToString() == "1";
            new AddEditUsers("Изменение пользователя", "Изменить", true, (name1, surname1, password1, isAdmin1) =>
            {
                if (name1 != name || surname1 != surname || password1 != password)
                {
                    var passwordUpdate = name1 == name && surname1 == surname;
                    form.updateUser(isAdmin, id, name1, surname1, password1, dataGridView12, button36, button35, passwordUpdate);
                }
            }, isAdmin, name, surname, password)
                .ShowDialog();
            //AddEditAuthors form = new AddEditAuthors((a, b, c) => {
            //    editAuthor(a, b, c, Int32.Parse(dataGridView5[0, active].Value.ToString()));
            //}, true, dataGridView5[1, active].Value.ToString(), dataGridView5[2, active].Value.ToString(), dataGridView5[3, active].Value.ToString());
            //form.ShowDialog();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            new AddEditUsers("Добавление пользователя", "Добавить", false, (name1, surname1, password1, isAdmin1) =>
            {
                    form.createUser(isAdmin1, name1, surname1, password1, form.librarianId, dataGridView12, button36, button35);  
            })
                .ShowDialog();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            int active = dataGridView12.CurrentCellAddress.Y;
            int id = Int32.Parse(dataGridView12["id", active].Value.ToString());

            if(id == form.librarianId)
            {
                MessageBox.Show("Вы не можете удалить самого себя");
                return;
            }

            bool isAdmin = dataGridView12["is_admin", active].Value.ToString() == "1";
            form.deleteUser(isAdmin, id, dataGridView12, button36, button35);
        }
    }
}
