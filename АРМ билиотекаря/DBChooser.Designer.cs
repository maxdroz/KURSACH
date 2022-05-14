namespace АРМ_билиотекаря
{
    partial class DBChooser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBChooser));
            this.label1 = new System.Windows.Forms.Label();
            this.address = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pswd = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.user_password = new System.Windows.Forms.TextBox();
            this.user_surname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.user_name = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Адрес БД";
            // 
            // address
            // 
            this.address.Location = new System.Drawing.Point(13, 34);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(196, 22);
            this.address.TabIndex = 1;
            this.address.Text = "192.168.0.21";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Имя пользователя БД";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(13, 125);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(193, 22);
            this.username.TabIndex = 5;
            this.username.Text = "root";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(196, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Пароль от пользователя БД";
            // 
            // pswd
            // 
            this.pswd.Location = new System.Drawing.Point(13, 174);
            this.pswd.Name = "pswd";
            this.pswd.Size = new System.Drawing.Size(193, 22);
            this.pswd.TabIndex = 7;
            this.pswd.Text = "pswd";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 203);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(489, 46);
            this.button1.TabIndex = 8;
            this.button1.Text = "Подключится";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(13, 79);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(196, 22);
            this.name.TabIndex = 9;
            this.name.Text = "library";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Название БД";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(270, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Пароль";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // user_password
            // 
            this.user_password.Location = new System.Drawing.Point(270, 145);
            this.user_password.Name = "user_password";
            this.user_password.Size = new System.Drawing.Size(196, 22);
            this.user_password.TabIndex = 13;
            this.user_password.Text = "admin";
            this.user_password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // user_surname
            // 
            this.user_surname.Location = new System.Drawing.Point(270, 100);
            this.user_surname.Name = "user_surname";
            this.user_surname.Size = new System.Drawing.Size(196, 22);
            this.user_surname.TabIndex = 12;
            this.user_surname.Text = "admin";
            this.user_surname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(273, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Фамилия";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // user_name
            // 
            this.user_name.Location = new System.Drawing.Point(270, 55);
            this.user_name.Name = "user_name";
            this.user_name.Size = new System.Drawing.Size(196, 22);
            this.user_name.TabIndex = 16;
            this.user_name.Text = "admin";
            this.user_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(273, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(193, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Имя";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DBChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 261);
            this.Controls.Add(this.user_name);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.user_password);
            this.Controls.Add(this.user_surname);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.name);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pswd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.address);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DBChooser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Учет книг бибилиотеки";
            this.Activated += new System.EventHandler(this.DBChooser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox address;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pswd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox user_password;
        private System.Windows.Forms.TextBox user_surname;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox user_name;
        private System.Windows.Forms.Label label7;
    }
}