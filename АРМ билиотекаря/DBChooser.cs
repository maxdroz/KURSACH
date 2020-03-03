using System;
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

        //private void Button2_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "Access MDB files (*.mdb)|*.mdb";
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        if (form == null)
        //        {
        //            form = new Form1(this);
        //        }
        //        form.updateConnection(ofd.FileName);
        //        form.Show();
        //        Hide();
        //    }
        //}

        //private void Button1_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    sfd.Filter = "Access MDB files (*.mdb)|*.mdb";
        //    if (sfd.ShowDialog() == DialogResult.OK)
        //    {
        //        if (File.Exists(sfd.FileName))
        //        {
        //            File.Delete(sfd.FileName);
        //        }
        //        var cat = new ADOX.Catalog();
        //        cat.Create(DatabaseAdapter.connectionStringTemplate + sfd.FileName);
        //        if (form == null)
        //        {
        //            form = new Form1(this);
        //        }
        //        form.updateConnection(sfd.FileName);
        //        form.Show();
        //        Hide();
        //    }
            
        //    //FolderBrowserDialog fbd = new FolderBrowserDialog();
        //    //if (fbd.ShowDialog() == DialogResult.OK)
        //    //{
        //    //    String path = fbd.SelectedPath;
        //    //    String 
        //    //    var cat = new ADOX.Catalog();
        //    //    cat.Create();
        //    //}
        //}

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
            } catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
