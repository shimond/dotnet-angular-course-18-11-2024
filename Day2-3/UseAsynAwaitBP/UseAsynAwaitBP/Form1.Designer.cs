namespace UseAsynAwaitBP
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            listBoxLogs = new ListBox();
            txtStamTextbox = new TextBox();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(681, 38);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start download";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // listBoxLogs
            // 
            listBoxLogs.FormattingEnabled = true;
            listBoxLogs.ItemHeight = 15;
            listBoxLogs.Location = new Point(27, 35);
            listBoxLogs.Name = "listBoxLogs";
            listBoxLogs.Size = new Size(648, 319);
            listBoxLogs.TabIndex = 1;
            // 
            // txtStamTextbox
            // 
            txtStamTextbox.Location = new Point(27, 374);
            txtStamTextbox.Name = "txtStamTextbox";
            txtStamTextbox.Size = new Size(718, 23);
            txtStamTextbox.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtStamTextbox);
            Controls.Add(listBoxLogs);
            Controls.Add(btnStart);
            Name = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnStart;
        private ListBox listBoxLogs;
        private TextBox txtStamTextbox;
    }
}
