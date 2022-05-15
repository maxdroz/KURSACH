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
using static АРМ_билиотекаря.DatabaseAdapter;
using Excel = Microsoft.Office.Interop.Excel;

namespace АРМ_билиотекаря
{
    public partial class Form1 : Form
    {
        private DataTable empty = null;

        private DBChooser chooser;
        public int librarianId = -1;
        private bool isAdmin = false;
        private readonly object syncLock = new object();
        private bool isResetButton = false;
        public DatabaseAdapter adapter;
        private bool mouseDown = false;
        private Point startPos;
         
        public void updateConnection(String address, String databaseName, String username, String password)
        {
            adapter.setBDPath(address, databaseName, username, password);
            adapter.createTables();
            updateDebtors();
        }

        public void updateUser(int librarianId, bool isAdmin)
        {
            this.librarianId = librarianId;
            this.isAdmin = isAdmin;

            пользователиToolStripMenuItem.Visible = isAdmin;
            запросыToolStripMenuItem.Visible = isAdmin;
        }

        public Form1(DBChooser chooser)
        {
            InitializeComponent();
            this.chooser = chooser;
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

        public UserResolution getUserId(string name, string surname, string password)
        {
            return adapter.resolveUser(name, surname, password);
        }

        public class Args
        {
            public int author2;
            public int language;
            public int genre;
            public int ph;
            public int cover;
            public string title;
            public Book book;
            public String message;
            public int id;
            public int user_id;
            public int debitId;
            public Reader reader = null;
            public DataGridView dataGridView = null;
            public string tableName = null;
            public string columnName = null;
            public Button deleteButton;
            public Button editButton;
            public string data;
            public int dataId;
            public string errorMessage = "";
            public int era;
            public int type;
            public int font;
            public int size;

            public bool isAdmin;
            public string name;
            public string surname;
            public string password;
            public int userAuthorId;
            public bool isPasswordUpdate;

            public Args(int id, int user_id) : this(id)
            {
                this.user_id = user_id;
            }

            public Args(int id, Reader reader) : this(id)
            {
                this.reader = reader;
            }

            public Args(int id, DataGridView table, string tableName, Button deleteButton, Button editButton, string data)
            {
                this.id = id;
                this.dataGridView = table;
                this.tableName = tableName;
                this.deleteButton = deleteButton;
                this.editButton = editButton;
                this.data = data;
            }

            public Args(int id, DataGridView table, string tableName, Button deleteButton, Button editButton)
            {
                this.id = id;
                this.dataGridView = table;
                this.tableName = tableName;
                this.deleteButton = deleteButton;
                this.editButton = editButton;
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
            public bool readerError = false;
            public bool bookError = false;
            public int id;
            public DataGridView dataGridView;
            public Button deleteButton;
            public Button editButton;
            public bool error = false;
            public string message = "";
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
        private void deleteAuthor(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView5, "author", button14, button15)
            {
                user_id = id,
                errorMessage = "Чтобы удалить автора, необходимо избавиться от его использования во всех книгах."
            });
        }
        private void deleteLanguage(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView6, "language", button17, button18)
            {
                user_id = id,
                errorMessage = "Чтобы удалить язык, необходимо избавиться от его использования во всех книгах."
            });
        }

        private void deleteGenre(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView7, "genre", button20, button21)
            {
                user_id = id,
                errorMessage = "Чтобы удалить жанр, необходимо избавиться от его использования во всех книгах."
            });
        }


        private void deletePublishingHouse(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(22, dataGridView8, "publishing_house", button23, button24)
            {
                user_id = id,
                errorMessage = "Чтобы удалить издательство, необходимо избавиться от его использования во всех книгах."
            });
        }

        private void deleteCity(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView9, "city", button26, button27)
            {
                user_id = id,
                errorMessage = "Чтобы удалить город, необходимо избавиться от его использования во всех издательствах."
            });
        }

        private void deleteCover(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView10, "cover", button29, button30)
            {
                user_id = id,
                errorMessage = "Чтобы удалить обложку, необходимо избавиться от ее использования во всех книгах."
            });
        }
        private void deleteEra(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView11, "era", button33, button32)
            {
                user_id = id,
                errorMessage = "Чтобы удалить эру, необходимо избавиться от ее использования во всех книгах."
            });
        }
        private void deleteType(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView12, "type_of_literature", button36, button35)
            {
                user_id = id,
                errorMessage = "Чтобы удалить национальность, необходимо избавиться от ее использования во всех книгах."
            });
        }
        private void deleteFont(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView13, "font_size", button39, button38)
            {
                user_id = id,
                errorMessage = "Чтобы удалить шрифт, необходимо избавиться от ее использования во всех книгах."
            });
        }
        private void deleteSize(int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(18, dataGridView14, "book_size", button42, button41)
            {
                user_id = id,
                errorMessage = "Чтобы удалить размер книги, необходимо избавиться от ее использования во всех книгах."
            });
        }

        private void editPublishingHouse(string text, int id_city, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(21, dataGridView8, "publishing_house", button23, button24, String.Format("{0}', title = '{1}", id_city, text))
            {
                dataId = id,
                columnName = "id_city"
            });
        }

        private void editAuthor(string name, string surname, string patronymic, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView5, "author", button14, button15, String.Format("{0}', surname = '{1}', patronymic = '{2}", name, surname, patronymic))
            {
                dataId = id,
                columnName = "name"
            });
        }

        private void editLanguage(string language, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView6, "language", button17, button18, language)
            {
                dataId = id,
                columnName = "language"
            });
        }

        private void editGenre(string genre, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView7, "genre", button20, button21, genre)
            {
                dataId = id,
                columnName = "genre"
            });
        }

        private void editCity(string cityName, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView9, "city", button26, button27, cityName)
            {
                dataId = id,
                columnName = "city_name"
            });
        }

        private void editCovers(string coverName, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView10, "cover", button29, button30, coverName)
            {
                dataId = id,
                columnName = "cover_description"
            });
        }
        private void editEra(string eraName, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView11, "era", button33, button32, eraName)
            {
                dataId = id,
                columnName = "era"
            });
        }

        private void editType(string name, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView12, "type_of_literature", button36, button35, name)
            {
                dataId = id,
                columnName = "type_of_literature"
            });
        }
        private void editFont(string name, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView13, "font_size", button39, button38, name)
            {
                dataId = id,
                columnName = "name"
            });
        }
        private void editSize(string name, int id)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(17, dataGridView14, "book_size", button42, button41, name)
            {
                dataId = id,
                columnName = "name"
            });
        }

        private void addPublishingHouse(string text, int id_city)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(20, dataGridView8, "publishing_house", button23, button24, String.Format("{0}', '{1}", id_city, text)));
        }

        private void addAuthor(string name, string surname, string patronymic)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView5, "author", button14, button15, String.Format("{0}', '{1}', '{2}", name, surname, patronymic)));
        }

        private void addLanguage(string language)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView6, "language", button17, button18, language));
        }
        private void addGenre(string genre)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView7, "genre", button20, button21, genre));
        }
        private void addCity(string cityName)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView9, "city", button26, button26, cityName));
        }

        private void addCover(string coverName)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView10, "cover", button29, button30, coverName));
        }
        private void addEra(string eraName)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView11, "era", button33, button32, eraName));
        }
        private void addType(string name)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView12, "type_of_literature", button36, button35, name));
        }
        private void addFont(string name)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView13, "font_size", button39, button38, name));
        }
        private void addSize(string name)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(16, dataGridView14, "book_size", button42, button41, name));
        }

        private void updateAuthors()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView5, "author", button14, button15));
        }
        private void updatePublishers()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(19, dataGridView8, null, button23, button24));
        }
        private void updateLanguages()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView6, "language", button17, button18));
        }
 
        private void updateGenres()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView7, "genre", button20, button21));
        }

        private void updateCovers()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView10, "cover", button29, button30));
        }
        private void updateCities()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView9, "city", button27, button26));
        }
        private void updateEras()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView11, "era", button33, button32));
        }
        private void updateType()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView12, "type_of_literature", button36, button35));
        }
        private void updateFont()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView13, "font_size", button39, button38));
        }
        private void updateSize()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(15, dataGridView14, "book_size", button42, button41));
        }

        public void updateUser(bool isAdmin, int id, string name, string surname, string password, DataGridView grid, Button edit, Button delete, bool isPasswordUpdate)
        {

            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(27, grid, "aaaa", edit, delete)
            {
                isAdmin = isAdmin,
                user_id = id,
                name = name,
                surname = surname,
                password = password,
                isPasswordUpdate = isPasswordUpdate
            });
        }
        public void deleteUser(bool isAdmin, int id, DataGridView grid, Button edit, Button delete)
        {

            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(28, grid, "aaaa", edit, delete)
            {
                isAdmin = isAdmin,
                user_id = id,
            });
        }
        
        public void createUser(bool isAdmin, string name, string surname, string password, int authorId, DataGridView grid, Button edit, Button delete)
        {

            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(29, grid, "aaaa", edit, delete)
            {
                isAdmin = isAdmin,
                name = name,
                surname = surname,
                password = password,
                userAuthorId = authorId,
            });
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
        //15 - Получить (обющая)
        //16 - Добавить (обющая)
        //17 - Изменить (обющая)
        //18 - Удалить (обющая)
        //19 - Получить издателей
        //20 - Добавить издателя
        //21 - Изменить издателя
        //22 - Удалить издателя
        //24 - Добавить автора
        //25 - Изменить автора
        //26 - Удалить  автора
        //27 - Изменить пользователя
        //28 - Удалить пользователя

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
                        result = adapter.getFilteredBooks(new Book(textBox7.Text, textBox4.Text, textBox8.Text), textBox11.Text);
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
                        result = adapter.deleteReaderAndGetFilteredReaders(
                            new Reader(
                                textBox5.Text,
                                textBox1.Text,
                                textBox2.Text,
                                textBox3.Text,
                                dateTimePicker1.Value,
                                textBox6.Text,
                                textBox10.Text
                            ),
                            checkBox1.Checked,
                            ((Args)e.Argument).user_id);
                        id = 2;
                        res.readerError = result == null;
                     
                        break;
                    //case 8: 
                    //    res.readerClear = !adapter.isThereBookAtReader(((Args)e.Argument).user_id);
                    //    res.id = ((Args)e.Argument).user_id;
                    //    break;
                    case 9:
                        adapter.expandIssueDate(((Args)e.Argument).debitId, ((Args)e.Argument).reader.birthday);
                        res.id = ((Args)e.Argument).user_id;
                        break;
                    case 10:
                        adapter.returnBook(((Args)e.Argument).debitId);
                        res.id = ((Args)e.Argument).user_id;
                        id = 9;
                        break;
                    case 11:
                        adapter.addBook(
                            ((Args)e.Argument).title,
                            ((Args)e.Argument).author2,
                            ((Args)e.Argument).language,
                            ((Args)e.Argument).genre,
                            ((Args)e.Argument).ph,
                            ((Args)e.Argument).cover,
                            ((Args)e.Argument).era,
                            ((Args)e.Argument).type,
                            ((Args)e.Argument).font,
                            ((Args)e.Argument).size
                        );
                        break;
                    case 12:
                        adapter.editBook(
                            ((Args)e.Argument).title,
                            ((Args)e.Argument).author2,
                            ((Args)e.Argument).language,
                            ((Args)e.Argument).genre,
                            ((Args)e.Argument).ph,
                            ((Args)e.Argument).cover,
                            ((Args)e.Argument).era,
                            ((Args)e.Argument).type,
                            ((Args)e.Argument).font,
                            ((Args)e.Argument).size,
                            ((Args)e.Argument).user_id);
                        id = 11;
                        break;
                    case 13:
                        res.bookError = adapter.deleteBook(((Args)e.Argument).user_id);
                        id = 11;
                        break;
                    case 15:
                        result = adapter.getCommonData(((Args)e.Argument).tableName);
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        break;
                    case 16:
                        result = adapter.addCommon(((Args)e.Argument).tableName, ((Args)e.Argument).data);
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 17:
                        result = adapter.editCommon(((Args)e.Argument).tableName, ((Args)e.Argument).columnName, ((Args)e.Argument).data, ((Args)e.Argument).dataId);
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 18:
                        result = adapter.deleteCommon(((Args)e.Argument).tableName, ((Args)e.Argument).user_id);
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        res.message = ((Args)e.Argument).errorMessage;
                        res.error = result == null;
                        id = 15;
                        break;
                    case 19:
                        result = adapter.getPublishers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 20:
                         adapter.addCommon(((Args)e.Argument).tableName, ((Args)e.Argument).data);
                        break;
                    case 21:
                        adapter.editCommon(((Args)e.Argument).tableName, ((Args)e.Argument).columnName, ((Args)e.Argument).data, ((Args)e.Argument).dataId);
                        result = adapter.getPublishers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 22:
                        res.error = adapter.deleteCommon(((Args)e.Argument).tableName, ((Args)e.Argument).user_id) == null;
                        result = adapter.getPublishers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        res.message = ((Args)e.Argument).errorMessage;
                        id = 15;
                        break;
                    case 24:
                        break;
                    case 25:
                        break;
                    case 26:
                        break;
                    case 27:
                        res.error = !adapter.editUser(
                            ((Args)e.Argument).isAdmin,
                            ((Args)e.Argument).user_id,
                            ((Args)e.Argument).name,
                            ((Args)e.Argument).surname,
                            ((Args)e.Argument).password,
                            ((Args)e.Argument).isPasswordUpdate
                        );
                        res.message = "Пользователь с данными данными уже существует";
                        result = adapter.getAllUsers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 28:
                        adapter.deleteUser(
                            ((Args)e.Argument).isAdmin,
                            ((Args)e.Argument).user_id
                        );
                        result = adapter.getAllUsers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
                        break;
                    case 29:
                        res.error = !adapter.addUser(
                            ((Args)e.Argument).isAdmin,
                            ((Args)e.Argument).name,
                            ((Args)e.Argument).surname,
                            ((Args)e.Argument).password,
                            ((Args)e.Argument).userAuthorId
                        );
                        res.message = "Пользователь с данными данными уже существует";
                        result = adapter.getAllUsers();
                        res.dataGridView = ((Args)e.Argument).dataGridView;
                        res.deleteButton = ((Args)e.Argument).deleteButton;
                        res.editButton = ((Args)e.Argument).editButton;
                        id = 15;
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
        private void updateEditAndDeleteButtons(DataGridView dataGridView, Button edit, Button delete)
        {
            if (dataGridView.RowCount == 0)
            {
                edit.Enabled = false;
                delete.Enabled = false;
            }
            else
            {
                edit.Enabled = true;
                delete.Enabled = true;
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
                    if (r.readerError)
                    {
                        MessageBox.Show("У данного читателя есть не сданные книги\nСначала пометьте сданными все книги данного читателя");
                    }
                    else
                    {
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
                    }
                    break;
                case 4:
                    dataGridView1.DataSource = r.table;
                    disableEnableBookButtons();
                    break;
                case 6:
                    dataGridView4.DataSource = r.table;
                    updateEditAndDeleteDebtorsButtons();
                    break;
                case 9:
                    updateReaderBooks(r.id);
                    updateDebtors();
                    break;
                case 11:
                    if (r.bookError)
                    {
                        MessageBox.Show("Данная книга выдана.\nСначала верните ее");
                    }
                    else
                    {
                        updateBooksGrid();
                    }
                    break;
                case 15:
                    if(r.error)
                    {
                        MessageBox.Show(r.message);
                    }
                    else
                    {
                        r.dataGridView.DataSource = r.table;
                        updateEditAndDeleteButtons(r.dataGridView, r.editButton, r.deleteButton);
                    }
                    break;
                case 20:
                    updatePublishers();
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
            deleteAndUpdateReaders(id);
        }

        public void returnBook(int debitId, int readerId)
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
            int debitId = Convert.ToInt32(dataGridView1["record_id", dataGridView1.CurrentCellAddress.Y].Value);
            int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
            returnBook(debitId, readerId);
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    updateReadersGrid();
                    if (dataGridView2.CurrentCellAddress.Y != -1)
                    {
                        int readerId = Convert.ToInt32(dataGridView2[0, dataGridView2.CurrentCellAddress.Y].Value);
                        updateReaderBooks(readerId);
                    }
                    break;
                case 1:
                    updateDebtors();
                    break;
                case 2:
                    updateBooksGrid();
                    break;
                case 3:
                    updateAuthors();
                    break;
                case 4:
                    updateLanguages();
                    break;
                case 5:
                    updateGenres();
                    break;
                case 6:
                    updatePublishers();
                    break;
                case 7:
                    updateCities();
                    break;
                case 8:
                    updateCovers();
                    break;
                case 9:
                    updateEras();
                    break;
                case 10:
                    updateType();
                    break;
                case 11:
                    updateFont();
                    break;
                case 12:
                    updateSize();
                    break;
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

        public void addBook(string title, int author, int language, int genre, int ph, int cover, int era, int type, int size, int font)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(11)
            {
                author2 = author,
                language = language,
                genre = genre,
                ph = ph,
                cover = cover,
                title = title,
                era = era,
                type = type,
                size = size,
                font = font
            });
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            DataTable authors = adapter.authorFullName();
            DataTable languages = adapter.getCommonData("language");
            DataTable genre = adapter.getCommonData("genre");
            DataTable publishingHouse = adapter.getCommonData("publishing_house");
            DataTable cover = adapter.getCommonData("cover");
            DataTable era = adapter.getCommonData("era");
            DataTable type = adapter.getCommonData("type_of_literature");
            DataTable font = adapter.getCommonData("font_size");
            DataTable size = adapter.getCommonData("book_size");
            AddEditBook addEditBook = new AddEditBook(this, 
                authors, "name", 
                languages, "language", 
                genre, "genre", 
                publishingHouse, "title", 
                cover, "cover_description",
                era, "era",
                type, "type_of_literature",
                size, "name",
                font, "name"
                );
            addEditBook.ShowDialog();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int rowId = dataGridView3.CurrentCellAddress.Y;
            DataTable authors = adapter.authorFullName();
            DataTable languages = adapter.getCommonData("language");
            DataTable genre = adapter.getCommonData("genre");
            DataTable publishingHouse = adapter.getCommonData("publishing_house");
            DataTable cover = adapter.getCommonData("cover");
            DataTable era = adapter.getCommonData("era");
            DataTable type = adapter.getCommonData("type_of_literature");
            DataTable font = adapter.getCommonData("font_size");
            DataTable size = adapter.getCommonData("book_size");
            AddEditBook addEditBook = new AddEditBook(this,
                authors, "name",
                languages, "language",
                genre, "genre",
                publishingHouse, "title",
                cover, "cover_description",
                era, "era",
                type, "type_of_literature",
                size, "name",
                font, "name"
            ); 
            int authorId = Int32.Parse(dataGridView3["author_id", rowId].Value.ToString());
            int languageId = Int32.Parse(dataGridView3["language_id", rowId].Value.ToString());
            int genreId = Int32.Parse(dataGridView3["genre_id", rowId].Value.ToString());
            int publishingHouseId = Int32.Parse(dataGridView3["publishing_house_id", rowId].Value.ToString());
            int coverId = Int32.Parse(dataGridView3["cover_id", rowId].Value.ToString());
            int eraId = Int32.Parse(dataGridView3["era_id", rowId].Value.ToString());
            int typeId = Int32.Parse(dataGridView3["type_of_literature_id", rowId].Value.ToString());
            int fontId = Int32.Parse(dataGridView3["font_size_id", rowId].Value.ToString());
            int sizeId = Int32.Parse(dataGridView3["book_size_id", rowId].Value.ToString());
            DataRow ans = authors.Select("id = '" + authorId + "'")[0];
            authorId = authors.Rows.IndexOf(ans);
            ans = languages.Select("id = '" + languageId + "'")[0];
            languageId = languages.Rows.IndexOf(ans);
            ans = genre.Select("id = '" + genreId + "'")[0];
            genreId = genre.Rows.IndexOf(ans);
            ans = publishingHouse.Select("id = '" + publishingHouseId + "'")[0];
            publishingHouseId = publishingHouse.Rows.IndexOf(ans);
            ans = cover.Select("id = '" + coverId + "'")[0];
            coverId = cover.Rows.IndexOf(ans);
            ans = era.Select("id = '" + eraId + "'")[0];
            eraId = era.Rows.IndexOf(ans);
            ans = type.Select("id = '" + typeId + "'")[0];
            typeId = type.Rows.IndexOf(ans);
            ans = font.Select("id = '" + fontId + "'")[0];
            fontId = font.Rows.IndexOf(ans);
            ans = size.Select("id = '" + sizeId + "'")[0];
            sizeId = size.Rows.IndexOf(ans);
            addEditBook.setBook(
                Int32.Parse(dataGridView3[0, rowId].Value.ToString()), 
                dataGridView3[1, rowId].Value.ToString(), 
                authorId, 
                languageId, 
                genreId, 
                publishingHouseId, 
                coverId,
                eraId,
                typeId,
                sizeId,
                fontId
                );
            addEditBook.ShowDialog();
        }

        public void editBook(int id, string title, int author, int language, int genre, int ph, int cover, int era, int type, int size, int font)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += BackgroundWorker1_DoWork;
            worker.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            worker.RunWorkerAsync(new Args(12)
            {
                author2 = author,
                language = language,
                genre = genre,
                ph = ph,
                cover = cover,
                title = title,
                user_id = id,
                era = era,
                type = type,
                size = size,
                font = font
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
            int debitId = dataGridView4["Name1", dataGridView4.CurrentCellAddress.Y].Value.ToString().ToInt();
            int readerId = dataGridView4["Column10", dataGridView4.CurrentCellAddress.Y].Value.ToString().ToInt();
            //BookReturn br = new BookReturn(debitId, readerId, this);
            //br.ShowDialog();
            returnBook(debitId, readerId);
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
            deleteAndUpdateBook(bookId);
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

        private void button31_Click(object sender, EventArgs e)
        {
            AddEditSimpleCommon form = new AddEditSimpleCommon("Обложка", "Добавление обложки", false, (text) =>
            {
                addCover(text);
            });
            form.ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            AddEditSimpleCommon form = new AddEditSimpleCommon("Город", "Добавление города", false, (text) =>
            {
                addCity(text);
            });
            form.ShowDialog();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            AddEditSimpleCommon form = new AddEditSimpleCommon("Жанр", "Добавление жанра", false, (text) =>
            {
                addGenre(text);
            });
            form.ShowDialog();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            AddEditSimpleCommon form = new AddEditSimpleCommon("Язык", "Добавление языка", false, (text) =>
            {
                addLanguage(text);
            });
            form.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            AddEditAuthors form = new AddEditAuthors((name, surname, patronymic) =>
            {
                addAuthor(name, surname, patronymic);
            });
            form.ShowDialog();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            AddEditPublishingHouses form = new AddEditPublishingHouses(adapter.getCommonData("city"), "city_name", false, (name, id_city) =>
            {
                addPublishingHouse(name, id_city);
            });
            form.ShowDialog();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            int active = dataGridView10.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Обложка", "Изменение обложки", true, (text) =>
            {
                editCovers(text, Int32.Parse(dataGridView10[0, active].Value.ToString()));
            }, dataGridView10[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button27_Click(object sender, EventArgs e)
        { 
            int active = dataGridView9.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Город", "Изменение города", true, (text) =>
            {
                editCity(text, Int32.Parse(dataGridView9[0, active].Value.ToString()));
            }, dataGridView9[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int active = dataGridView7.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Жанр", "Изменение жанра", true, (text) =>
            {
                editGenre(text, Int32.Parse(dataGridView7[0, active].Value.ToString()));
            }, dataGridView7[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int active = dataGridView6.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Язык", "Изменение языка", true, (text) =>
            {
                editLanguage(text, Int32.Parse(dataGridView6[0, active].Value.ToString()));
            }, dataGridView6[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int active = dataGridView5.CurrentCellAddress.Y;
            AddEditAuthors form = new AddEditAuthors((a, b, c) =>{
                editAuthor(a, b, c, Int32.Parse(dataGridView5[0, active].Value.ToString()));
            }, true, dataGridView5[1, active].Value.ToString(), dataGridView5[2, active].Value.ToString(), dataGridView5[3, active].Value.ToString());
            form.ShowDialog();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            int active = dataGridView8.CurrentCellAddress.Y;
            int cityId = Int32.Parse(dataGridView8["city_id", active].Value.ToString());
            DataTable cities = adapter.getCommonData("city");
            DataRow ans = cities.Select("id = '" + cityId + "'")[0];
            cityId = cities.Rows.IndexOf(ans);
            AddEditPublishingHouses form = new AddEditPublishingHouses(cities, "city_name", true, (name, id_city) =>
            {
                editPublishingHouse(name, id_city, Int32.Parse(dataGridView8["idid", active].Value.ToString()));
            }, dataGridView8["dataGridViewTextBoxColumn10", active].Value.ToString(), cityId);
            form.ShowDialog();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            int active = dataGridView10.CurrentCellAddress.Y;
            deleteCover(dataGridView10[0, active].Value.ToString().ToInt());
        }

        private void button26_Click(object sender, EventArgs e)
        {
            int active = dataGridView9.CurrentCellAddress.Y;
            deleteCity(dataGridView9[0, active].Value.ToString().ToInt());
        }

        private void button23_Click(object sender, EventArgs e)
        {
            int active = dataGridView8.CurrentCellAddress.Y;
            deletePublishingHouse(dataGridView8["idid", active].Value.ToString().ToInt());
        }

        private void button20_Click(object sender, EventArgs e)
        {
            int active = dataGridView7.CurrentCellAddress.Y;
            deleteGenre(dataGridView7[0, active].Value.ToString().ToInt());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int active = dataGridView6.CurrentCellAddress.Y;
            deleteLanguage(dataGridView6[0, active].Value.ToString().ToInt());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int active = dataGridView5.CurrentCellAddress.Y;
            deleteAuthor(dataGridView5[0, active].Value.ToString().ToInt());
        }

        private void button34_Click(object sender, EventArgs e)
        {
            AddEditSimpleCommon form = new AddEditSimpleCommon("Эпоха", "Добавление эпохи", false, (text) =>
            {
                addEra(text);
            });
            form.ShowDialog();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            int active = dataGridView11.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Эпоха", "Изменение эпохи", true, (text) =>
            {
                editEra(text, Int32.Parse(dataGridView11[0, active].Value.ToString()));
            }, dataGridView11[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            int active = dataGridView11.CurrentCellAddress.Y;
            deleteEra(dataGridView11[0, active].Value.ToString().ToInt());
        }

        private void button37_Click(object sender, EventArgs e)
        {

            AddEditSimpleCommon form = new AddEditSimpleCommon("Национальность литературы", "Добавить национальность", false, (text) =>
            {
                addType(text);
            });
            form.ShowDialog();
        }

        private void button40_Click(object sender, EventArgs e)
        {

            AddEditSimpleCommon form = new AddEditSimpleCommon("Размер шрифта", "Добавление размера шрифта", false, (text) =>
            {
                addFont(text);
            });
            form.ShowDialog();
        }

        private void button43_Click(object sender, EventArgs e)
        {

            AddEditSimpleCommon form = new AddEditSimpleCommon("Размер книги", "Добавление размера книги", false, (text) =>
            {
                addSize(text);
            });
            form.ShowDialog();
        }

        private void button36_Click(object sender, EventArgs e)
        {

            int active = dataGridView12.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Национальность литературы", "Изменение национальности литературы", true, (text) =>
            {
                editType(text, Int32.Parse(dataGridView12[0, active].Value.ToString()));
            }, dataGridView12[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button39_Click(object sender, EventArgs e)
        {

            int active = dataGridView13.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Размер шрифта", "Изменение размера шрифта", true, (text) =>
            {
                editFont(text, Int32.Parse(dataGridView13[0, active].Value.ToString()));
            }, dataGridView13[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button42_Click(object sender, EventArgs e)
        {

            int active = dataGridView14.CurrentCellAddress.Y;
            AddEditSimpleCommon form = new AddEditSimpleCommon("Размер книги", "Изменение размера книги", true, (text) =>
            {
                editSize(text, Int32.Parse(dataGridView14[0, active].Value.ToString()));
            }, dataGridView14[1, active].Value.ToString());
            form.ShowDialog();
        }

        private void button35_Click(object sender, EventArgs e)
        {

            int active = dataGridView12.CurrentCellAddress.Y;
            deleteType(dataGridView12[0, active].Value.ToString().ToInt());
        }

        private void button38_Click(object sender, EventArgs e)
        {

            int active = dataGridView13.CurrentCellAddress.Y;
            deleteFont (dataGridView13[0, active].Value.ToString().ToInt());
        }

        private void button41_Click(object sender, EventArgs e)
        {

            int active = dataGridView14.CurrentCellAddress.Y;
            deleteSize(dataGridView14[0, active].Value.ToString().ToInt());
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ВыбратьДругуюБДToolStripMenuItem_Click(sender, e);
        }

        private void управлениеПользователямиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Users(this).Show();
        }

        private void произваольныйЗапросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CustomQuery(this).ShowDialog();
        }

        private void экспортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Книги.xlsx";
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; 
            dialog.OverwritePrompt = false;

            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            
            Excel.Application ex = new Excel.Application();
            //ex.Visible = true;

            Excel.Workbook workBook = ex.Workbooks.Add();
            Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);


            sheet.Range["R1", "R1"].EntireColumn.NumberFormat = "@";
            sheet.Range["L1", "M1"].EntireColumn.NumberFormat = "DD/MM/YYYY";
            sheet.Range["Q1", "Q1"].EntireColumn.NumberFormat = "DD/MM/YYYY";

            sheet.Range["A1", "A1"].EntireRow.Font.Bold = true;
            var borders = sheet.Range["A1", "A1"].EntireRow.Borders;
            borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;


            borders = sheet.Range["A1", "A1"].EntireColumn.Borders;
            borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;

            var books = adapter.getExcelBooks();
            for(int i = 0; i < books.Columns.Count; i++)
            {
                string title = books.Columns[i].ColumnName;

                switch(title)
                {
                    case "book_id":
                        title = "Код книги";
                        break;
                    case "title":
                        title = "Название";
                        break;
                    case "author":
                        title = "Автор";
                        break;
                    case "language":
                        title = "Язык";
                        break;
                    case "genre":
                        title = "Жанр";
                        break;
                    case "publishing_house":
                        title = "Издательство";
                        break;
                    case "cover":
                        title = "Обложка";
                        break;
                    case "era":
                        title = "Эра";
                        break;
                    case "type_of_literature":
                        title = "Национальность";
                        break;
                    case "book_size":
                        title = "Размер книги";
                        break;
                    case "font_size":
                        title = "Размер шрифта";
                        break;
                    case "issue_date":
                        title = "Дата выдачи";
                        break;
                    case "return_date":
                        title = "Дата возврата";
                        break;
                    case "name":
                        title = "Имя текущего читателя";
                        break;
                    case "surname":
                        title = "Фамилия";
                        break;
                    case "patronymic":
                        title = "Отчество";
                        break;
                    case "birthday":
                        title = "День рождения";
                        break;
                    case "phone_number":
                        title = "Номер телефона";
                        break;
                    case "address":
                        title = "Адрес";
                        break;
                }

                sheet.Cells[1, i + 1] = title;
                for (int j = 0; j < books.Rows.Count; j++)
                {
                    sheet.Cells[j + 2, i + 1] = books.Rows[j][i].ToString();
                }
            }

            sheet.Name = "Книги";

            sheet.Columns.AutoFit();
            try {
                workBook.SaveAs(dialog.FileName);
            } 
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                workBook.Close();
            }
         }
    }
}

static class Ext
{
    public static int ToInt(this string str)
    {
        return Int32.Parse(str);
    }
}