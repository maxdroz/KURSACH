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

        public AddEditBook(Form1 form, DataTable authors, string authorsCol, DataTable language, string languageCol, DataTable genre, string genreCol, DataTable publishingHouse, string publishingCol, DataTable cover, string coverCol)
        {
            f = form;
            InitializeComponent();
            this.authors = authors;
            this.language = language;
            this.genre = genre;
            this.publishingHouse = publishingHouse;
            this.cover = cover;
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
        }

        public void setBook(int id, string title, int authorsIndex, int languageIndex, int genreIndex, int publishingHouseIndex, int coverIndex)
        {
            edit = true;
            this.id = id;
            comboBox1.SelectedIndex = authorsIndex;
            comboBox2.SelectedIndex = languageIndex;
            comboBox3.SelectedIndex = genreIndex;
            comboBox4.SelectedIndex = publishingHouseIndex;
            comboBox5.SelectedIndex = coverIndex;
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
            if (!edit)
            {
                int authorsIndex = (int)authors.Rows[comboBox1.SelectedIndex][0];
                int languageIndex = (int)language.Rows[comboBox2.SelectedIndex][0];
                int genreIndex = (int)genre.Rows[comboBox3.SelectedIndex][0];
                int publishingHouseIndex = (int)publishingHouse.Rows[comboBox4.SelectedIndex][0];
                int coverIndex = (int)cover.Rows[comboBox5.SelectedIndex][0];
                f.addBook(textBox2.Text, authorsIndex, languageIndex, genreIndex, publishingHouseIndex, coverIndex);
            }
            else
            {
                int authorsIndex = (int)authors.Rows[comboBox1.SelectedIndex][0];
                int languageIndex = (int)language.Rows[comboBox2.SelectedIndex][0];
                int genreIndex = (int)genre.Rows[comboBox3.SelectedIndex][0];
                int publishingHouseIndex = (int)publishingHouse.Rows[comboBox4.SelectedIndex][0];
                int coverIndex = (int)cover.Rows[comboBox5.SelectedIndex][0];
                f.editBook(id, textBox2.Text, authorsIndex, languageIndex, genreIndex, publishingHouseIndex, coverIndex);
            }
            Close();
        }
    }
}
