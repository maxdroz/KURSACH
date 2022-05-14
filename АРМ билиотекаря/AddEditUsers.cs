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
    public partial class AddEditUsers : Form
    {
        private readonly string title;
        private readonly string buttonText;
        private readonly bool isEdit;
        private readonly Action<string, string, string, bool> onClick;
        private readonly bool isAdmin;
        private readonly string name;
        private readonly string surname;
        private readonly string password;

        public AddEditUsers(string title, string buttonText, bool isEdit, Action<string, string, string, bool> onClick, bool isAdmin = false, string name = "", string surname = "", string password = "")
        {
            InitializeComponent();
            this.title = title;
            this.buttonText = buttonText;
            this.isEdit = isEdit;
            this.onClick = onClick;
            this.isAdmin = isAdmin;
            this.name = name;
            this.surname = surname;
            this.password = password;

            checkBox1.Enabled = !isEdit;
            checkBox1.Checked = isAdmin;
            textBox1.Text = name;
            textBox2.Text = surname;
            textBox3.Text = password;

            this.Text = title;
            button1.Text = buttonText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (textBox2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите фамилию");
                return;
            }
            if (textBox3.Text.Trim().Length == 0)
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            onClick.Invoke(
                textBox1.Text,
                textBox2.Text,
                textBox3.Text,
                checkBox1.Checked
               );
            Hide();
        }
    }
}
