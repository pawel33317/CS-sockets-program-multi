namespace multiClient
{
    partial class Main
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
            this.btnconnect = new System.Windows.Forms.Button();
            this.btnsend = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnconnect
            // 
            this.btnconnect.Location = new System.Drawing.Point(13, 13);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(135, 23);
            this.btnconnect.TabIndex = 0;
            this.btnconnect.Text = "Połącz";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.btnconnect_Click);
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(12, 42);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(135, 23);
            this.btnsend.TabIndex = 1;
            this.btnsend.Text = "Wyślij";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
            // 
            // btnclose
            // 
            this.btnclose.Location = new System.Drawing.Point(12, 97);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(135, 23);
            this.btnclose.TabIndex = 2;
            this.btnclose.Text = "Zamknij";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 71);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(134, 20);
            this.textBox1.TabIndex = 3;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(161, 129);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnclose);
            this.Controls.Add(this.btnsend);
            this.Controls.Add(this.btnconnect);
            this.Name = "Main";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.TextBox textBox1;
    }
}