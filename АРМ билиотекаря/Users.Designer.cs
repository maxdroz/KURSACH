namespace АРМ_билиотекаря
{
    partial class Users
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Users));
            this.dataGridView12 = new System.Windows.Forms.DataGridView();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.button35 = new System.Windows.Forms.Button();
            this.button36 = new System.Windows.Forms.Button();
            this.button37 = new System.Windows.Forms.Button();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.password_hash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_admin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView12)).BeginInit();
            this.groupBox14.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView12
            // 
            this.dataGridView12.AllowUserToAddRows = false;
            this.dataGridView12.AllowUserToDeleteRows = false;
            this.dataGridView12.AllowUserToResizeColumns = false;
            this.dataGridView12.AllowUserToResizeRows = false;
            this.dataGridView12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView12.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView12.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView12.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView12.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.surname,
            this.password_hash,
            this.author,
            this.is_admin});
            this.dataGridView12.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView12.Location = new System.Drawing.Point(13, 13);
            this.dataGridView12.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView12.MultiSelect = false;
            this.dataGridView12.Name = "dataGridView12";
            this.dataGridView12.RowHeadersVisible = false;
            this.dataGridView12.RowHeadersWidth = 51;
            this.dataGridView12.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView12.Size = new System.Drawing.Size(1022, 495);
            this.dataGridView12.TabIndex = 10;
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.button35);
            this.groupBox14.Controls.Add(this.button36);
            this.groupBox14.Controls.Add(this.button37);
            this.groupBox14.Location = new System.Drawing.Point(13, 516);
            this.groupBox14.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox14.Size = new System.Drawing.Size(1022, 67);
            this.groupBox14.TabIndex = 11;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Операции с пользователями";
            // 
            // button35
            // 
            this.button35.Location = new System.Drawing.Point(355, 23);
            this.button35.Margin = new System.Windows.Forms.Padding(4);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(172, 32);
            this.button35.TabIndex = 2;
            this.button35.Text = "Удалить";
            this.button35.UseVisualStyleBackColor = true;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button36
            // 
            this.button36.Location = new System.Drawing.Point(181, 23);
            this.button36.Margin = new System.Windows.Forms.Padding(4);
            this.button36.Name = "button36";
            this.button36.Size = new System.Drawing.Size(165, 32);
            this.button36.TabIndex = 1;
            this.button36.Text = "Редактировать";
            this.button36.UseVisualStyleBackColor = true;
            this.button36.Click += new System.EventHandler(this.button36_Click);
            // 
            // button37
            // 
            this.button37.Location = new System.Drawing.Point(8, 23);
            this.button37.Margin = new System.Windows.Forms.Padding(4);
            this.button37.Name = "button37";
            this.button37.Size = new System.Drawing.Size(165, 32);
            this.button37.TabIndex = 0;
            this.button37.Text = "Добавить";
            this.button37.UseVisualStyleBackColor = true;
            this.button37.Click += new System.EventHandler(this.button37_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "Номер";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.Width = 80;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Имя";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.Width = 64;
            // 
            // surname
            // 
            this.surname.DataPropertyName = "surname";
            this.surname.HeaderText = "Фамилия";
            this.surname.MinimumWidth = 6;
            this.surname.Name = "surname";
            this.surname.Width = 99;
            // 
            // password_hash
            // 
            this.password_hash.DataPropertyName = "password_hash";
            this.password_hash.HeaderText = "Пароль";
            this.password_hash.MinimumWidth = 6;
            this.password_hash.Name = "password_hash";
            this.password_hash.Width = 86;
            // 
            // author
            // 
            this.author.DataPropertyName = "author";
            this.author.HeaderText = "Создатель";
            this.author.MinimumWidth = 6;
            this.author.Name = "author";
            this.author.Width = 107;
            // 
            // is_admin
            // 
            this.is_admin.DataPropertyName = "is_admin";
            this.is_admin.FalseValue = "0";
            this.is_admin.HeaderText = "Права админа";
            this.is_admin.MinimumWidth = 6;
            this.is_admin.Name = "is_admin";
            this.is_admin.TrueValue = "1";
            this.is_admin.Width = 97;
            // 
            // Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 596);
            this.Controls.Add(this.groupBox14);
            this.Controls.Add(this.dataGridView12);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Users";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Управление пользователями";
            this.Load += new System.EventHandler(this.Users_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView12)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView12;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Button button36;
        private System.Windows.Forms.Button button37;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn surname;
        private System.Windows.Forms.DataGridViewTextBoxColumn password_hash;
        private System.Windows.Forms.DataGridViewTextBoxColumn author;
        private System.Windows.Forms.DataGridViewCheckBoxColumn is_admin;
    }
}