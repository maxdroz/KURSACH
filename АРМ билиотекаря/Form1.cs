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

        private DBChooser chooser;
        private readonly object syncLock = new object();
        private bool isResetButton = false;
        private DatabaseAdapter adapter;
        private bool mouseDown = false;
        private Point startPos;

        public void updateConnection(String pathToDB)
        {
            adapter.setBDPath(pathToDB);
            adapter.createTables();
        }

        public Form1(String pathToDB, DBChooser chooser)
        {
            this.chooser = chooser;
            InitializeComponent();
            adapter = DatabaseAdapter.getInstance();
            updateConnection(pathToDB);
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
            updateReadersGrid();
            updateBooksGrid();
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

        public class Args
        {
            public Book book;
            public String message;
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
            public bool bookClear;
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
        //11 - Добавить книгу
        //12 - Изменить книгу
        //13 - Удалить книгу
        //14 - Проверить, выдана ли книга

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
                        result = adapter.getFilteredBooks(new Book(textBox7.Text, textBox4.Text, textBox8.Text, textBox9.Text), textBox11.Text);
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
                        res.readerClear = !adapter.isThereBookAtReader(((Args)e.Argument).user_id);
                        res.id = ((Args)e.Argument).user_id;
                        break;
                    case 9:
                        adapter.expandIssueDate(((Args)e.Argument).debitId, ((Args)e.Argument).reader.birthday);
                        res.id = ((Args)e.Argument).user_id;
                        break;
                    case 10:
                        adapter.returnBook(((Args)e.Argument).debitId, ((Args)e.Argument).message);
                        res.id = ((Args)e.Argument).user_id;
                        id = 9;
                        break;
                    case 11:
                        adapter.addBook(((Args)e.Argument).book);
                        break;
                    case 12:
                        adapter.editBook(((Args)e.Argument).book, ((Args)e.Argument).user_id);
                        id = 11;
                        break;
                    case 13:
                        adapter.deleteBook(((Args)e.Argument).user_id);
                        id = 11;
                        break;
                    case 14:
                        res.id = ((Args)e.Argument).user_id;
                        res.bookClear = adapter.isBookAtReader(((Args)e.Argument).user_id);
                        break;
                }
                res.arg = id;
                res.table = result;
                e.Result = res;
            }
        }

        private void updateEditAndDeleteBookButtons()
        {
            if (dataGridView3.RowCount == 0)
            {
                button8.Enabled = false;
                button9.Enabled = false;
            }
            else
            {
                button8.Enabled = true;
                button9.Enabled = true;
            }
        }

        private void updateEditAndDeleteDebtorsButtons()
        {
            if (dataGridView4.RowCount == 0)
            {
                button10.Enabled = false;
                button13.Enabled = false;
            }
            else
            {
                button10.Enabled = true;
                button13.Enabled = true;
            }
        }



        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Res r = (Res)e.Result;
            switch (r.arg)
            {
                case 1:
                    dataGridView3.DataSource = r.table;
                    updateEditAndDeleteBookButtons();
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
                    updateEditAndDeleteDebtorsButtons();
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
                    updateDebtors();
                    break;
                case 11:
                    updateBooksGrid();
                    break;
                case 14:
                    if (!r.bookClear)
                    {
                        deleteAndUpdateBook(r.id);
                    }
                    else
                    {
                        MessageBox.Show("Данная книга выдана.\nСначала верните ее");
                    }
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

        public void returnBook(int debitId, int readerId, String message)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            Args a = new Args(10, readerId);
            a.message = message;
            a.debitId = debitId;
            worker.RunWorkerAsync(a);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            BookReturn br = new BookReturn(debitId, readerId, this);
            br.ShowDialog();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 2)
            {
                updateDebtors();
            }
            if (tabControl1.SelectedIndex == 1)
            {
                updateBooksGrid();
            }
            if(tabControl1.SelectedIndex == 0)
            {
                updateReadersGrid();
                if (dataGridView2.CurrentCellAddress.Y != -1)
                {
                    int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
                    updateReaderBooks(readerId);
                }
            }
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView1[0, dataGridView1.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            DateTime date = DateTime.Parse(dataGridView1["issue_date", dataGridView1.CurrentCellAddress.Y].Value.ToString());
            ExpandIssueDate exp = new ExpandIssueDate(this, debitId, readerId, date);
            exp.ShowDialog();
        }

        public void addBook(Book book)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(11)
            {
                book = book
            });
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            AddEditBook addEditBook = new AddEditBook(this);
            addEditBook.ShowDialog();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int rowId = Convert.ToInt32(dataGridView3.CurrentCellAddress.Y);
            AddEditBook addEditBook = new AddEditBook(this);
            addEditBook.setBook(new Book(dataGridView3[2, rowId].Value.ToString(), dataGridView3[1, rowId].Value.ToString(), dataGridView3[3, rowId].Value.ToString(), dataGridView3[4, rowId].Value.ToString()), Convert.ToInt32(dataGridView3[0, rowId].Value.ToString()));
            addEditBook.ShowDialog();
        }

        public void editBook(Book book, int bookId)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(12)
            {
                user_id = bookId,
                book = book
            });
        }

        private void checkBookForReaderAndDelete(int book_id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(14, book_id));
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView4["Name1", dataGridView4.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView4["Column10", dataGridView4.CurrentCellAddress.Y].Value);
            BookReturn br = new BookReturn(debitId, readerId, this);
            br.ShowDialog();
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            int debitId = Convert.ToInt32(dataGridView4["Name1", dataGridView4.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView4["Column10", dataGridView4.CurrentCellAddress.Y].Value);
            DateTime min = DateTime.Parse(dataGridView4["Column5", dataGridView4.CurrentCellAddress.Y].Value.ToString());
            ExpandIssueDate exp = new ExpandIssueDate(this, debitId, readerId, min);
            exp.ShowDialog();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            int bookId = Convert.ToInt32(dataGridView3[0, dataGridView3.CurrentCellAddress.Y].Value);
            checkBookForReaderAndDelete(bookId);
        }

        private void deleteAndUpdateBook(int bookId)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(13, bookId));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ВыбратьДругуюБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chooser.Show();
            Hide();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                TabControl1_SelectedIndexChanged(null, null);
            }
        }
    }
}
