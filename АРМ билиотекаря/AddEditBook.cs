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
        public AddEditBook(Form1 form)
        {
            f = form;
            InitializeComponent();
        }

        public void setBook(Book book, int id)
        {
            this.id = id;
            textBox1.Text = book.author;
            textBox2.Text = book.title;
            textBox3.Text = book.language;
            edit = true;
            button1.Text = "Изменить";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!edit)
            {
                f.addBook(new Book(textBox2.Text, textBox1.Text, textBox3.Text));
                textBox1.Text = textBox2.Text = textBox3.Text = "";
            }
            else
            {
                //TODO: Разабраться с изменением кода
                //if(prevLocation != textBox6.Text)

                f.editBook(new Book(textBox2.Text, textBox1.Text, textBox3.Text), id);
                Close();
            }
        }
    }
}
