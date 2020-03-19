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
        Form1 form;
        string tableName;
        string paramName;
        DataGridView gridView;

        public AddEditSimpleCommon(Form1 form, string tableName, string paramName, int id, DataGridView gridView)
        {
            InitializeComponent();
            this.form = form;
            this.tableName = tableName;
            this.paramName = paramName;
            this.gridView = gridView;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
