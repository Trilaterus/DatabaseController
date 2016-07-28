namespace EmployeeDatabase
{
    partial class Form1
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
            this.sqlDatabaseController1 = new EmployeeDatabase.SQLDatabaseController();
            this.SuspendLayout();
            // 
            // sqlDatabaseController1
            // 
            this.sqlDatabaseController1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sqlDatabaseController1.Location = new System.Drawing.Point(12, 12);
            this.sqlDatabaseController1.Name = "sqlDatabaseController1";
            this.sqlDatabaseController1.Size = new System.Drawing.Size(1340, 610);
            this.sqlDatabaseController1.TabIndex = 6;
            this.sqlDatabaseController1.Load += new System.EventHandler(this.sqlDatabaseController1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 634);
            this.Controls.Add(this.sqlDatabaseController1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private SQLDatabaseController sqlDatabaseController1;

    }
}

