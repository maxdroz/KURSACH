﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АРМ_билиотекаря
{
    public partial class DBChooser : Form
    {
        private Form1 form = null;
        public DBChooser()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            String addressString = address.Text;
            String databaseNameString = name.Text;
            String user = username.Text;
            String passwordString = pswd.Text;
            if (form == null)
            {
                form = new Form1(this);
            }
            try { 
                form.updateConnection(addressString, databaseNameString, user, passwordString);
                form.Show();
                Hide();
            } 
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DBChooser_Load(object sender, EventArgs e)
        {
            button1_Click_1(null, null);
            Hide();
        }
    }
}
