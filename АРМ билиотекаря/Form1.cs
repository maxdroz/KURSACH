using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace АРМ_билиотекаря
{
    public partial class Form1 : Form
    {
        private bool mouseDown = false;
        private Point startPos;
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            startPos = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (dataGridView2.Height + e.Y - startPos.Y < 20)
                    return;
                if (dataGridView1.Height - e.Y + startPos.Y < 20)
                    return;
                groupBox3.Top += e.Y - startPos.Y;
                dataGridView2.Height += e.Y - startPos.Y;
                dataGridView1.Height -= e.Y - startPos.Y;
                dataGridView1.Top += e.Y - startPos.Y;
                dataGridView2.Refresh();
                groupBox3.Refresh();
                dataGridView1.Refresh();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bDDataSet.readers". При необходимости она может быть перемещена или удалена.
            this.readersTableAdapter.Fill(this.bDDataSet.readers);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateReadersGrid();
        }

        private void updateReadersGrid()
        {
            
            OleDbConnection connection = new OleDbConnection(DatabaseAdapter.connectionString);
            connection.Open();
            String query = "SELECT * FROM readers WHERE name LIKE '%" + textBox1.Text + "%'";
            OleDbDataAdapter da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            connection.Close();
        }
    }
}
