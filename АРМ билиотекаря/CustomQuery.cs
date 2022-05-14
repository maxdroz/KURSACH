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
    public partial class CustomQuery : Form
    {
        private readonly Form1 form;

        public CustomQuery(Form1 form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView12.DataSource = form.adapter.formDataTable(textBox1.Text);
            } 
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
