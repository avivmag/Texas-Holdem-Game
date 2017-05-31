namespace ClassLibrary1
{
    partial class Test
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
            this.listSystemUsers = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listSystemUsers
            // 
            this.listSystemUsers.FormattingEnabled = true;
            this.listSystemUsers.ItemHeight = 16;
            this.listSystemUsers.Location = new System.Drawing.Point(30, 30);
            this.listSystemUsers.Name = "listSystemUsers";
            this.listSystemUsers.Size = new System.Drawing.Size(148, 132);
            this.listSystemUsers.TabIndex = 0;
            this.listSystemUsers.SelectedIndexChanged += new System.EventHandler(this.listSystemUsers_SelectedIndexChanged);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 400);
            this.Controls.Add(this.listSystemUsers);
            this.Name = "Test";
            this.Text = "TestExample";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listSystemUsers;
    }
}