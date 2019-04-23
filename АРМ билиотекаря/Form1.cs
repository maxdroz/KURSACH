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
        private DataTable empty = null;
        

        private readonly object syncLock = new object();
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
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bDDataSet.books". При необходимости она может быть перемещена или удалена.
            this.booksTableAdapter.Fill(this.bDDataSet.books);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bDDataSet.readers". При необходимости она может быть перемещена или удалена.
            this.readersTableAdapter.Fill(this.bDDataSet.readers);
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

        public void updateReadersGrid()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(2));

            if (dataGridView2.RowCount <= 1)
            {
                if (empty == null)
                {
                    empty = adapter.getReaderBooks(-1);
                }
                dataGridView1.DataSource = empty;
                //dataGridView1.DataSource = adapter.getReaderBooks(-1);
            }
            //dataGridView2.DataSource = adapter.getFilteredReaders(textBox1.Text,
            //    textBox2.Text,
            //    textBox3.Text, 
            //    textBox6.Text,
            //    checkBox1.Checked, dateTimePicker1.Value,
            //    textBox5.Text,
            //    textBox10.Text);
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
            textBox10.Text = "";
            checkBox1.Checked = false;
            isResetButton = false;
            updateReadersGrid();
        }

        private void DataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            int reader_id = Convert.ToInt32(dataGridView2[0, e.RowIndex].Value);
            updateReaderBooks(reader_id);
        }

        private void updateBooksGrid()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(1));
            //dataGridView3.DataSource = adapter.getFilteredBooks(textBox4.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox11.Text);
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

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {
            if (!isResetButton)
                updateBooksGrid();

        }

        private void updateReaderBooks(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(4, id));
        }

        private void DataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        public class Args
        {
            public int id;
            public int user_id;
            public Reader reader = null;

            public Args(int id, int user_id) : this(id)
            {
                this.user_id = user_id;
            }

            public Args(int id, Reader reader) : this(id)
            {
                this.reader = reader;
            }

            public Args(int id)
            {
                this.id = id;
            }
        }

        class Res
        {
            public int arg;
            public DataTable table;
            public Res(int argg, DataTable tablee)
            {
                arg = argg;
                table = tablee;
            }
        }

        //1 - Поиск всех книг
        //2 - Поиск по читателям
        //3 - Добавление читателя
        //4 - Берем книги определенного читателя
        //5 - Изменение информации о читателе
        public void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (syncLock)
            {
                int id = ((Args)e.Argument).id;
                DataTable result = null;
                switch (id)
                {
                    case 1:
                        result = adapter.getFilteredBooks(textBox4.Text, textBox7.Text, textBox8.Text, textBox9.Text, textBox11.Text);
                        break;
                    case 2:
                        result = adapter.getFilteredReaders(new Reader(textBox5.Text,
                            textBox1.Text,
                            textBox2.Text,
                            textBox3.Text,
                            dateTimePicker1.Value,
                            textBox6.Text,
                            textBox10.Text), checkBox1.Checked);
                        break;
                    case 3:
                        adapter.addReader(((Args)e.Argument).reader);
                        break;
                    case 4:
                        result = adapter.getReaderBooks(((Args)e.Argument).user_id);
                        break;
                    case 5:
                        adapter.editReader(((Args)e.Argument).reader);
                        break;
                }
                e.Result = new Res(id, result);
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Res r = (Res)e.Result;
            switch (r.arg)
            {
                case 1:
                    dataGridView3.DataSource = r.table;
                    break;
                case 2:
                    dataGridView2.DataSource = r.table;
                    updateEditButton();
                    break;
                case 4:
                    dataGridView1.DataSource = r.table;
                    break;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            AddEditReader reader = new AddEditReader();
            reader.setForm(this);
            reader.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            AddEditReader reader = new AddEditReader();
            reader.setForm(this);
            int active = dataGridView2.CurrentCellAddress.Y;
            Reader editReader = new Reader(dataGridView2["id", active].Value.ToString(),
                dataGridView2["name", active].Value.ToString(),
                dataGridView2["surname", active].Value.ToString(),
                dataGridView2["patronymic", active].Value.ToString(),
                Convert.ToDateTime(dataGridView2["birthday", active].Value),
                dataGridView2["phone_number", active].Value.ToString(),
                dataGridView2["adress", active].Value.ToString());

            reader.setEdit(editReader);
            reader.Show();
        }

        private void updateEditButton()
        {
            if (dataGridView2.CurrentCellAddress.Y == -1)
                button5.Enabled = false;
            else
                button5.Enabled = true;
        }
    }
}
