namespace EmployeeDatabase
{
    partial class ConnectForm
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
            this.tablessTabControl1 = new EmployeeDatabase.TablessTabControl();
            this.Login = new System.Windows.Forms.TabPage();
            this.btNext = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.tbDatabaseName = new System.Windows.Forms.TextBox();
            this.tbUID = new System.Windows.Forms.TextBox();
            this.lbServer = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.cbUID = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.lbUID = new System.Windows.Forms.Label();
            this.cbDatabaseName = new System.Windows.Forms.CheckBox();
            this.cbServer = new System.Windows.Forms.CheckBox();
            this.lbDatabaseName = new System.Windows.Forms.Label();
            this.PrimaryKeySelection = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btGenerate = new System.Windows.Forms.Button();
            this.btBack = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.myTreeView1 = new EmployeeDatabase.MyTreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tablessTabControl1.SuspendLayout();
            this.Login.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PrimaryKeySelection.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablessTabControl1
            // 
            this.tablessTabControl1.Controls.Add(this.Login);
            this.tablessTabControl1.Controls.Add(this.PrimaryKeySelection);
            this.tablessTabControl1.Controls.Add(this.tabPage2);
            this.tablessTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablessTabControl1.Location = new System.Drawing.Point(0, 0);
            this.tablessTabControl1.Name = "tablessTabControl1";
            this.tablessTabControl1.SelectedIndex = 0;
            this.tablessTabControl1.Size = new System.Drawing.Size(487, 631);
            this.tablessTabControl1.TabIndex = 13;
            // 
            // Login
            // 
            this.Login.Controls.Add(this.btNext);
            this.Login.Controls.Add(this.panel1);
            this.Login.Location = new System.Drawing.Point(4, 22);
            this.Login.Name = "Login";
            this.Login.Padding = new System.Windows.Forms.Padding(3);
            this.Login.Size = new System.Drawing.Size(720, 506);
            this.Login.TabIndex = 0;
            this.Login.Text = "Login";
            this.Login.UseVisualStyleBackColor = true;
            // 
            // btNext
            // 
            this.btNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btNext.Location = new System.Drawing.Point(637, 475);
            this.btNext.Name = "btNext";
            this.btNext.Size = new System.Drawing.Size(75, 23);
            this.btNext.TabIndex = 12;
            this.btNext.Text = "Next";
            this.btNext.UseVisualStyleBackColor = true;
            this.btNext.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Controls.Add(this.tbDatabaseName);
            this.panel1.Controls.Add(this.tbUID);
            this.panel1.Controls.Add(this.lbServer);
            this.panel1.Controls.Add(this.lbPassword);
            this.panel1.Controls.Add(this.cbUID);
            this.panel1.Controls.Add(this.tbPassword);
            this.panel1.Controls.Add(this.tbServer);
            this.panel1.Controls.Add(this.lbUID);
            this.panel1.Controls.Add(this.cbDatabaseName);
            this.panel1.Controls.Add(this.cbServer);
            this.panel1.Controls.Add(this.lbDatabaseName);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 466);
            this.panel1.TabIndex = 13;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(5, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(125, 13);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Enter connection values:";
            // 
            // tbDatabaseName
            // 
            this.tbDatabaseName.Location = new System.Drawing.Point(8, 78);
            this.tbDatabaseName.Name = "tbDatabaseName";
            this.tbDatabaseName.Size = new System.Drawing.Size(124, 20);
            this.tbDatabaseName.TabIndex = 6;
            // 
            // tbUID
            // 
            this.tbUID.Location = new System.Drawing.Point(8, 118);
            this.tbUID.Name = "tbUID";
            this.tbUID.Size = new System.Drawing.Size(124, 20);
            this.tbUID.TabIndex = 7;
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(5, 22);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(38, 13);
            this.lbServer.TabIndex = 1;
            this.lbServer.Text = "Server";
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(5, 141);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(53, 13);
            this.lbPassword.TabIndex = 5;
            this.lbPassword.Text = "Password";
            // 
            // cbUID
            // 
            this.cbUID.AutoSize = true;
            this.cbUID.Location = new System.Drawing.Point(138, 120);
            this.cbUID.Name = "cbUID";
            this.cbUID.Size = new System.Drawing.Size(77, 17);
            this.cbUID.TabIndex = 11;
            this.cbUID.Text = "Remember";
            this.cbUID.UseVisualStyleBackColor = true;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(8, 158);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(124, 20);
            this.tbPassword.TabIndex = 8;
            this.tbPassword.Text = "12345yes";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(8, 38);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(124, 20);
            this.tbServer.TabIndex = 2;
            // 
            // lbUID
            // 
            this.lbUID.AutoSize = true;
            this.lbUID.Location = new System.Drawing.Point(5, 101);
            this.lbUID.Name = "lbUID";
            this.lbUID.Size = new System.Drawing.Size(43, 13);
            this.lbUID.TabIndex = 4;
            this.lbUID.Text = "User ID";
            // 
            // cbDatabaseName
            // 
            this.cbDatabaseName.AutoSize = true;
            this.cbDatabaseName.Location = new System.Drawing.Point(138, 80);
            this.cbDatabaseName.Name = "cbDatabaseName";
            this.cbDatabaseName.Size = new System.Drawing.Size(77, 17);
            this.cbDatabaseName.TabIndex = 10;
            this.cbDatabaseName.Text = "Remember";
            this.cbDatabaseName.UseVisualStyleBackColor = true;
            // 
            // cbServer
            // 
            this.cbServer.AutoSize = true;
            this.cbServer.Location = new System.Drawing.Point(138, 40);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(77, 17);
            this.cbServer.TabIndex = 9;
            this.cbServer.Text = "Remember";
            this.cbServer.ThreeState = true;
            this.cbServer.UseVisualStyleBackColor = true;
            // 
            // lbDatabaseName
            // 
            this.lbDatabaseName.AutoSize = true;
            this.lbDatabaseName.Location = new System.Drawing.Point(5, 61);
            this.lbDatabaseName.Name = "lbDatabaseName";
            this.lbDatabaseName.Size = new System.Drawing.Size(84, 13);
            this.lbDatabaseName.TabIndex = 3;
            this.lbDatabaseName.Text = "Database Name";
            // 
            // PrimaryKeySelection
            // 
            this.PrimaryKeySelection.Controls.Add(this.label4);
            this.PrimaryKeySelection.Controls.Add(this.textBox2);
            this.PrimaryKeySelection.Controls.Add(this.button2);
            this.PrimaryKeySelection.Controls.Add(this.button1);
            this.PrimaryKeySelection.Controls.Add(this.label3);
            this.PrimaryKeySelection.Controls.Add(this.panel3);
            this.PrimaryKeySelection.Location = new System.Drawing.Point(4, 22);
            this.PrimaryKeySelection.Name = "PrimaryKeySelection";
            this.PrimaryKeySelection.Padding = new System.Windows.Forms.Padding(3);
            this.PrimaryKeySelection.Size = new System.Drawing.Size(479, 605);
            this.PrimaryKeySelection.TabIndex = 2;
            this.PrimaryKeySelection.Text = "PrimaryKeySelection";
            this.PrimaryKeySelection.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 532);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Selected primary key";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(11, 548);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(460, 20);
            this.textBox2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(396, 574);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Next";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btPKey_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(8, 574);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btBack_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(225, 26);
            this.label3.TabIndex = 1;
            this.label3.Text = "Select a primary key:\r\n(Must be an auto-increment, unsigned column)";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.treeView2);
            this.panel3.Location = new System.Drawing.Point(11, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(460, 500);
            this.panel3.TabIndex = 0;
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(0, 0);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(460, 500);
            this.treeView2.TabIndex = 0;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btGenerate);
            this.tabPage2.Controls.Add(this.btBack);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(479, 605);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ColumnSelection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btGenerate
            // 
            this.btGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btGenerate.Location = new System.Drawing.Point(352, 574);
            this.btGenerate.Name = "btGenerate";
            this.btGenerate.Size = new System.Drawing.Size(119, 23);
            this.btGenerate.TabIndex = 3;
            this.btGenerate.Text = "Generate Database";
            this.btGenerate.UseVisualStyleBackColor = true;
            this.btGenerate.Click += new System.EventHandler(this.btGenerate_Click);
            // 
            // btBack
            // 
            this.btBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btBack.Location = new System.Drawing.Point(8, 574);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 2;
            this.btBack.Text = "Back";
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.myTreeView1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(473, 565);
            this.panel2.TabIndex = 4;
            // 
            // myTreeView1
            // 
            this.myTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myTreeView1.CheckBoxes = true;
            this.myTreeView1.Location = new System.Drawing.Point(8, 16);
            this.myTreeView1.Name = "myTreeView1";
            this.myTreeView1.Size = new System.Drawing.Size(460, 498);
            this.myTreeView1.TabIndex = 8;
            this.myTreeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select table columns to generate";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 517);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current SELECT message:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(8, 537);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(460, 20);
            this.textBox1.TabIndex = 7;
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 631);
            this.Controls.Add(this.tablessTabControl1);
            this.Name = "ConnectForm";
            this.Text = "ConnectForm";
            this.tablessTabControl1.ResumeLayout(false);
            this.Login.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PrimaryKeySelection.ResumeLayout(false);
            this.PrimaryKeySelection.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label lbDatabaseName;
        private System.Windows.Forms.Label lbUID;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbDatabaseName;
        private System.Windows.Forms.TextBox tbUID;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.CheckBox cbServer;
        private System.Windows.Forms.CheckBox cbDatabaseName;
        private System.Windows.Forms.CheckBox cbUID;
        private System.Windows.Forms.Button btNext;
        private TablessTabControl tablessTabControl1;
        private System.Windows.Forms.TabPage Login;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.Button btGenerate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage PrimaryKeySelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private MyTreeView myTreeView1;
    }
}