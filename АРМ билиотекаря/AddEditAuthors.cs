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
    public partial class AddEditAuthors : Form
    {
        Action<String, String, String> action;
        public AddEditAuthors(Action<String, String, String> action, bool edit = false, string name = "", string surname = "", string patronymic = "")
        {
            InitializeComponent();
            if (edit)
            {
                textBox1.Text = name;
                textBox2.Text = surname;
                textBox3.Text = patronymic;
                Text = "Редактирование автора";
            } else
            {
                Text = "Добавление автора";
            }
            this.action = action;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Поля не должны быть пустыми");
            }
            else
            {
                action.Invoke(textBox1.Text, textBox2.Text, textBox3.Text);
                Close();
            }
        }
    }
}
