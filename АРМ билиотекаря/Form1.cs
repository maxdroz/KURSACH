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
        private bool isResetButton = false;
        private DatabaseAdapter adapter;
        private bool mouseDown = false;
        private Point startPos;
        public Form1()
        {
            InitializeComponent();
            adapter = DatabaseAdapter.getInstance();
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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bDDataSet.books". При необходимости она может быть перемещена или удалена.
            this.booksTableAdapter.Fill(this.bDDataSet.books);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bDDataSet.readers". При необходимости она может быть перемещена или удалена.
            this.readersTableAdapter.Fill(this.bDDataSet.readers);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isResetButton)
                updateReadersGrid();
        }

        private void updateReadersGrid()
        {
            dataGridView2.DataSource = adapter.getFilteredReaders(textBox1.Text,
                textBox2.Text,
                textBox3.Text, 
                textBox6.Text,
                checkBox1.Checked, dateTimePicker1.Value,
                textBox5.Text);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(!isResetButton)
                updateReadersGrid();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (!isResetButton)
                updateReadersGrid();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            isResetButton = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            checkBox1.Checked = false;
            isResetButton = false;
            updateReadersGrid();
        }

        private void DataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int reader_id = Convert.ToInt32(dataGridView2[0, e.RowIndex].Value);
            dataGridView1.DataSource = adapter.getReaderBooks(reader_id);
        }

        private void updateBooksGrid()
        {
            dataGridView3.DataSource = adapter.getFilteredBooks(textBox4.Text, textBox7.Text, textBox8.Text, textBox9.Text);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            isResetButton = true;
            textBox4.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            isResetButton = false;
            updateBooksGrid();
        }

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if(!isResetButton)
                updateBooksGrid();
        }
    }
}
