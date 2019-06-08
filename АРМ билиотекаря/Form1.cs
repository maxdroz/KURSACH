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
            updateDebtors();
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

        public void updateDebtors()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(6));
        }

        public void updateReadersGrid()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(2));

            
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

        public void updateReaderBooks(int id)
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
            public int debitId;
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
            public bool readerClear;
            public int id;
            public Res(int argg, DataTable tablee)
            {
                arg = argg;
                table = tablee;
            }
        }
        private void disableEnableBookButtons()
        {
            if (dataGridView1.RowCount != 0)
            {
                button2.Enabled = true;
                button12.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button12.Enabled = false;
            }
        }

        //1  - Поиск всех книг
        //2  - Поиск по читателям
        //3  - Добавление читателя
        //4  - Берем книги определенного читателя
        //5  - Изменение информации о читателе
        //6  - Получить всех должников
        //7  - Удалить Читателя
        //8  - Проверить есть ли у читателя книги
        //9  - Обновить дату выдачи
        //10 - Вернуть книгу
        public void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (syncLock)
            {
                int id = ((Args)e.Argument).id;
                DataTable result = null;
                Res res = new Res(id, result);
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
                    case 6:
                        result = adapter.getDebtors();
                        break;
                    case 7:
                        result = adapter.deleteReaderAndGetFilteredReaders(new Reader(textBox5.Text,
                            textBox1.Text,
                            textBox2.Text,
                            textBox3.Text,
                            dateTimePicker1.Value,
                            textBox6.Text,
                            textBox10.Text), checkBox1.Checked, ((Args)e.Argument).user_id);
                            id = 2;
                        break;
                    case 8:
                        res.readerClear = !adapter.areThereBookAtReader(((Args)e.Argument).user_id);
                        res.id = ((Args)e.Argument).user_id;
                        break;
                    case 9:
                        adapter.expandIssueDate(((Args)e.Argument).debitId, ((Args)e.Argument).reader.birthday);
                        res.id = ((Args)e.Argument).user_id;
                        break;
                    case 10:
                        adapter.returnBook(((Args)e.Argument).debitId);
                        res.id = ((Args)e.Argument).user_id;
                        id = 9;
                        break;
                }
                res.arg = id;
                res.table = result;
                e.Result = res;
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
                    if (dataGridView2.RowCount <= 1)
                    {
                        if (empty == null)
                        {
                            empty = adapter.getReaderBooks(-1);
                        }
                        dataGridView1.DataSource = empty;
                        //dataGridView1.DataSource = adapter.getReaderBooks(-1);
                    }
                    updateEditAndDeleteButton();
                    disableEnableBookButtons();
                    break;
                case 4:
                    dataGridView1.DataSource = r.table;
                    disableEnableBookButtons();
                    break;
                case 6:
                    dataGridView4.DataSource = r.table;
                    break;
                case 8:
                    if (r.readerClear)
                    {
                        deleteAndUpdateReaders(r.id);
                    }
                    else
                    {
                        MessageBox.Show("У данного читателя есть не сданные книги\nСначала пометьте сданными все книги данного читателя");
                    }
                    break;
                case 9:
                    updateReaderBooks(r.id);
                    break;
            }
        }

        public void expandReturnDate(int userId, DateTime newDate, int debitId)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            Args a = new Args(9, userId);
            a.debitId = debitId;
            a.reader = new Reader("", "", "", "", newDate, "", "");
            worker.RunWorkerAsync(a);
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            AddEditReader reader = new AddEditReader();
            reader.setForm(this);
            reader.ShowDialog();
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
            reader.ShowDialog();
        }

        private void updateEditAndDeleteButton()
        {
            if (dataGridView2.RowCount == 0)
            {
                button5.Enabled = false;
                button6.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
                button6.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            BookIssue issue = new BookIssue(this, id);
            issue.ShowDialog();
        }

        private void deleteAndUpdateReaders(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(7, id));
        }

        private void checkForBooksAndDeleteReader(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(8, id));
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            checkForBooksAndDeleteReader(id);
        }

        void returnBook(int debitId, int readerId)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            Args a = new Args(10, readerId);
            a.debitId = debitId;
            worker.RunWorkerAsync(a);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            returnBook(debitId, readerId);
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 2)
            {
                updateDebtors();
            }
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            ExpandIssueDate exp = new ExpandIssueDate(this, debitId, readerId);
            exp.ShowDialog();
        }
    }
}
