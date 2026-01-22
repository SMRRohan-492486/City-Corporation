namespace citycorporation
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_nid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_newPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_confirmPassword = new System.Windows.Forms.TextBox();
            this.btn_reset = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reset Password";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Enter your NID number";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txt_nid
            // 
            this.txt_nid.Location = new System.Drawing.Point(44, 88);
            this.txt_nid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_nid.MaxLength = 17;
            this.txt_nid.Name = "txt_nid";
            this.txt_nid.Size = new System.Drawing.Size(267, 22);
            this.txt_nid.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Enter new password";
            // 
            // txt_newPassword
            // 
            this.txt_newPassword.Location = new System.Drawing.Point(44, 152);
            this.txt_newPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_newPassword.Name = "txt_newPassword";
            this.txt_newPassword.Size = new System.Drawing.Size(267, 22);
            this.txt_newPassword.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Confirm new password";
            // 
            // txt_confirmPassword
            // 
            this.txt_confirmPassword.Location = new System.Drawing.Point(44, 216);
            this.txt_confirmPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_confirmPassword.Name = "txt_confirmPassword";
            this.txt_confirmPassword.Size = new System.Drawing.Size(267, 22);
            this.txt_confirmPassword.TabIndex = 6;
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(44, 264);
            this.btn_reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(267, 32);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "RESET PASSWORD";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(44, 312);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(88, 16);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Back to Login";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 400);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.txt_confirmPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_newPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_nid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reset Password";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_nid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_newPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_confirmPassword;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}