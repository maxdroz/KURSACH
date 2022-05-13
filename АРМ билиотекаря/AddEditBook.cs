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
    public partial class AddEditBook : Form
    {
        private int id;
        private bool edit = false;
        private Form1 f;
        DataTable authors;
        DataTable language;
        DataTable genre;
        DataTable publishingHouse;
        DataTable cover;
        DataTable era;
        DataTable type;
        DataTable size;
        DataTable font;

        public AddEditBook(
            Form1 form, 
            DataTable authors, string authorsCol, 
            DataTable language, string languageCol, 
            DataTable genre, string genreCol, 
            DataTable publishingHouse, string publishingCol,
            DataTable cover, string coverCol,
            DataTable era, string eraCol,
            DataTable type, string typeCol,
            DataTable size, string sizeCol,
            DataTable font, string fontCol
        ) {
            f = form;
            InitializeComponent();
            this.authors = authors;
            this.language = language;
            this.genre = genre;
            this.publishingHouse = publishingHouse;
            this.cover = cover;
            this.era = era;
            this.type = type;
            this.size = size;
            this.font = font;
            comboBox1.DataSource = authors;
            comboBox1.DisplayMember = authorsCol;
            comboBox2.DataSource = language;
            comboBox2.DisplayMember = languageCol;
            comboBox3.DataSource = genre;
            comboBox3.DisplayMember = genreCol;
            comboBox4.DataSource = publishingHouse;
            comboBox4.DisplayMember = publishingCol;
            comboBox5.DataSource = cover;
            comboBox5.DisplayMember = coverCol;
            comboBox6.DataSource = era;
            comboBox6.DisplayMember = eraCol;
            comboBox7.DataSource = type;
            comboBox7.DisplayMember = typeCol;
            comboBox8.DataSource = size;
            comboBox8.DisplayMember = sizeCol;
            comboBox9.DataSource = font;
            comboBox9.DisplayMember = fontCol;
        }

        public void setBook(
            int id, 
            string title, 
            int authorsIndex, 
            int languageIndex, 
            int genreIndex, 
            int publishingHouseIndex, 
            int coverIndex,
            int eraIndex,
            int typeIndex,
            int sizeIndex,
            int fontIndex
            )
        {
            edit = true;
            this.id = id;
            comboBox1.SelectedIndex = authorsIndex;
            comboBox2.SelectedIndex = languageIndex;
            comboBox3.SelectedIndex = genreIndex;
            comboBox4.SelectedIndex = publishingHouseIndex;
            comboBox5.SelectedIndex = coverIndex;
            comboBox6.SelectedIndex = eraIndex;
            comboBox7.SelectedIndex = typeIndex;
            comboBox8.SelectedIndex = sizeIndex;
            comboBox9.SelectedIndex = fontIndex;
            textBox2.Text = title;
            button1.Text = "Изменить";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") 
            {
                MessageBox.Show("Название не должно быть пустым");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите автора");
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите язык");
                return;
            }
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }
            if (comboBox4.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите издательство");
                return;
            }
            if (comboBox5.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите обложку");
                return;
            }
            if (comboBox6.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите эру");
                return;
            }
            if (comboBox7.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите национальность");
                return;
            }
            if (comboBox8.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите размер книги");
                return;
            }
            if (comboBox9.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите размер шрифта");
                return;
            }
            int authorsIndex = (int)authors.Rows[comboBox1.SelectedIndex][0];
            int languageIndex = (int)language.Rows[comboBox2.SelectedIndex][0];
            int genreIndex = (int)genre.Rows[comboBox3.SelectedIndex][0];
            int publishingHouseIndex = (int)publishingHouse.Rows[comboBox4.SelectedIndex][0];
            int coverIndex = (int)cover.Rows[comboBox5.SelectedIndex][0];
            int eraIndex = (int)era.Rows[comboBox6.SelectedIndex][0];
            int typeIndex = (int)type.Rows[comboBox7.SelectedIndex][0];
            int sizeIndex = (int)size.Rows[comboBox8.SelectedIndex][0];
            int fontIndex = (int)font.Rows[comboBox9.SelectedIndex][0];

            if (!edit)
            {
                f.addBook(textBox2.Text, authorsIndex, languageIndex, genreIndex, publishingHouseIndex, coverIndex,
                    eraIndex,
                    typeIndex,
                    sizeIndex,
                    fontIndex
                    );
            }
            else
            {
                f.editBook(id, textBox2.Text, authorsIndex, languageIndex, genreIndex, publishingHouseIndex, coverIndex,
                    eraIndex,
                    typeIndex,
                    sizeIndex,
                    fontIndex
                    );
            }
            Close();
        }
    }
}
