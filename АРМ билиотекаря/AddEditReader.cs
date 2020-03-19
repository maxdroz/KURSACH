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
    public partial class AddEditReader : Form
    {
        private Form1 parent;
        private bool edit = false;
        private string id;
        public AddEditReader()
        {
            InitializeComponent();
        }

        public void setForm(Form1 f)
        {
            parent = f;
        }

        public void setEdit(Reader reader)
        {
            edit = true;
            this.id = reader.id;
            button1.Text = "Изменить";
            textBox1.Text = reader.name;
            textBox2.Text = reader.surname;
            textBox3.Text = reader.patronymic;
            dateTimePicker1.Value = reader.birthday;
            textBox5.Text = reader.phone_number;
            textBox6.Text = reader.address;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Имя не должно быть пустым");
                return;
            }
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("Фамилия не должна быть пустой");
                return;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Отчество не должно быть пустым");
                return;
            }
            if (textBox5.Text.Trim() == "")
            {
                MessageBox.Show("Номер телефона не должен быть пустым");
                return;
            }
            if (textBox6.Text.Trim() == "")
            {
                MessageBox.Show("Адрес не должен быть пустым");
                return;
            }
            if (edit)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += parent.BackgroundWorker1_DoWork;
                worker.RunWorkerAsync(new Form1.Args(5, new Reader(id, textBox1.Text, textBox2.Text, textBox3.Text, dateTimePicker1.Value, textBox5.Text, textBox6.Text)));
                parent.updateReadersGrid();
                Close();
            }
            else
            {
                var worker = new BackgroundWorker();
                worker.DoWork += parent.BackgroundWorker1_DoWork;
                worker.RunWorkerAsync(new Form1.Args(3, new Reader("", textBox1.Text, textBox2.Text, textBox3.Text, dateTimePicker1.Value, textBox5.Text, textBox6.Text)));
                parent.updateReadersGrid();
                textBox1.Text = textBox2.Text = textBox3.Text = textBox5.Text = textBox6.Text = "";
            }
        }
    }
}
