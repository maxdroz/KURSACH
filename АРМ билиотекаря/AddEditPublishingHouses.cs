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
    public partial class AddEditPublishingHouses : Form
    {
        Action<String, int> action;
        DataTable table;

        public AddEditPublishingHouses(DataTable table, string columnName, bool forEdit, Action<String, int> action, string text = "", int id = 0)
        {
            InitializeComponent();
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = columnName;
            this.table = table;
            if(forEdit)
            {
                textBox1.Text = text;
                comboBox1.SelectedIndex = id;
                Text = "Изменение издательства";
                button1.Text = "Изменить";
            } 
            else
            {
                Text = "Добавление издательства";
                button1.Text = "Добавить";
            }
            this.action = action;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = (int)table.Rows[comboBox1.SelectedIndex][0];
            action.Invoke(textBox1.Text, index);
            Close();
        }
    }
}
