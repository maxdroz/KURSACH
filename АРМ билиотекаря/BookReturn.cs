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
    public partial class BookReturn : Form
    {
        int debitId, readerId;
        Form1 f;

        public BookReturn(int debitId, int readerId, Form1 form)
        {
            f = form;
            this.debitId = debitId;
            this.readerId = readerId;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            f.returnBook(debitId, readerId);
            Close();
        }

    }
}
