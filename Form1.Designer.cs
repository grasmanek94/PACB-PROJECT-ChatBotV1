namespace ChatBot
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
            this.btn_AddBot = new System.Windows.Forms.Button();
            this.browsers = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // btn_AddBot
            // 
            this.btn_AddBot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_AddBot.Location = new System.Drawing.Point(0, 501);
            this.btn_AddBot.Name = "btn_AddBot";
            this.btn_AddBot.Size = new System.Drawing.Size(985, 21);
            this.btn_AddBot.TabIndex = 0;
            this.btn_AddBot.Text = "New Bot";
            this.btn_AddBot.UseVisualStyleBackColor = true;
            this.btn_AddBot.Click += new System.EventHandler(this.btn_AddBot_Click);
            // 
            // browsers
            // 
            this.browsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browsers.Location = new System.Drawing.Point(12, 12);
            this.browsers.Name = "browsers";
            this.browsers.SelectedIndex = 0;
            this.browsers.Size = new System.Drawing.Size(961, 483);
            this.browsers.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 522);
            this.Controls.Add(this.browsers);
            this.Controls.Add(this.btn_AddBot);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_AddBot;
        private System.Windows.Forms.TabControl browsers;
    }
}

