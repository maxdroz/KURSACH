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
    public partial class BookIssue : Form
    {
        private bool inOperation = false;
        private bool isResetButton = false;
        private DatabaseAdapter adapter;
        private int readerId;
        private Form1 form;
        public BookIssue(Form1 f, int readerId)
        {
            form = f;
            InitializeComponent();
            this.readerId = readerId;
            adapter = DatabaseAdapter.getInstance();
        }

        private void textBox_TextEdit(object sender, EventArgs e)
        {
            if (!isResetButton)
                updateBooksGrid();
        }

        private void updateBooksGrid()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = adapter.getFilteredBooksNotTaken(new Book(textBox7.Text, textBox4.Text, textBox8.Text, textBox9.Text), textBox11.Text);
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridView3.DataSource = e.Result;
            if(dataGridView3.RowCount == 0)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
            inOperation = false;
        }
        private void AddBook_DoWork(object sender, DoWorkEventArgs e)
        {
            int bookId = Convert.ToInt32(dataGridView3[0, dataGridView3.CurrentCellAddress.Y].Value);
            adapter.issueBookToReader(readerId, bookId, dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void AddBook_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            form.updateReaderBooks(readerId);
            updateBooksGrid();
        }

        private void BookIssue_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = dateTimePicker1.Value.AddDays(30);

            updateBooksGrid();

        }

        private void Button11_Click(object sender, EventArgs e)
        {
            isResetButton = true;
            textBox4.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
            isResetButton = false;
            updateBooksGrid();
        }

        private void addBookToReader()
        {
            inOperation = true;
            var worker = new BackgroundWorker();
            worker.DoWork += AddBook_DoWork;
            worker.RunWorkerCompleted += AddBook_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(!inOperation)
                addBookToReader();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
            dateTimePicker1.MaxDate = dateTimePicker2.Value;
        }
    }
}
