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
    public partial class AddEditSimpleCommon : Form
    {
        Action<String> action;
        public AddEditSimpleCommon(string labelText, string title, bool buttonEdit, Action<String> action, string value = "")
        {
            InitializeComponent();
            this.action = action;
            label1.Text = labelText;
            Text = title;
            if (buttonEdit)
            {
                textBox1.Text = value;
                button1.Text = "Изменить";
            }
            else
            {
                button1.Text = "Добавить";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Поле не может быть пустым");
            } 
            else
            {
                action.Invoke(textBox1.Text);
                Close();
            }
        }
    }
}
