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
    public partial class ExpandIssueDate : Form
    {
        Form1 f;
        int debitId, userId;
        public ExpandIssueDate(Form1 form, int debitId, int userId)
        {
            this.debitId = debitId;
            this.userId = userId;
            f = form;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            f.expandReturnDate(userId, monthCalendar1.SelectionStart, debitId);
            Close();
        }
    }
}
