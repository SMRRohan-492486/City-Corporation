namespace citycorporation
{
    partial class Form2
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btn_register = new System.Windows.Forms.Button();
            this.txt_confirm = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_phone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_area = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_city = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_nid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_fullname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.btn_register);
            this.panel1.Controls.Add(this.txt_confirm);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txt_password);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txt_phone);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cmb_area);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmb_city);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txt_nid);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_fullname);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(50, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 540);
            this.panel1.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.linkLabel1.Location = new System.Drawing.Point(65, 515);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(235, 13);
            this.linkLabel1.TabIndex = 16;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Already have an account? Login here";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // btn_register
            // 
            this.btn_register.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(71)))));
            this.btn_register.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_register.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_register.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btn_register.ForeColor = System.Drawing.Color.White;
            this.btn_register.Location = new System.Drawing.Point(50, 465);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(300, 38);
            this.btn_register.TabIndex = 15;
            this.btn_register.Text = "REGISTER";
            this.btn_register.UseVisualStyleBackColor = false;
            this.btn_register.Click += new System.EventHandler(this.btn_register_Click);
            // 
            // txt_confirm
            // 
            this.txt_confirm.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_confirm.Location = new System.Drawing.Point(50, 425);
            this.txt_confirm.Name = "txt_confirm";
            this.txt_confirm.PasswordChar = '●';
            this.txt_confirm.Size = new System.Drawing.Size(300, 23);
            this.txt_confirm.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label8.Location = new System.Drawing.Point(50, 405);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 15);
            this.label8.TabIndex = 13;
            this.label8.Text = "Confirm Password:";
            // 
            // txt_password
            // 
            this.txt_password.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_password.Location = new System.Drawing.Point(50, 370);
            this.txt_password.Name = "txt_password";
            this.txt_password.PasswordChar = '●';
            this.txt_password.Size = new System.Drawing.Size(300, 23);
            this.txt_password.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.Location = new System.Drawing.Point(50, 350);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Password:";
            // 
            // txt_phone
            // 
            this.txt_phone.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_phone.Location = new System.Drawing.Point(50, 315);
            this.txt_phone.MaxLength = 11;
            this.txt_phone.Name = "txt_phone";
            this.txt_phone.Size = new System.Drawing.Size(300, 23);
            this.txt_phone.TabIndex = 10;
            this.txt_phone.TextChanged += new System.EventHandler(this.txt_phone_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.Location = new System.Drawing.Point(50, 295);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Phone Number:";
            // 
            // cmb_area
            // 
            this.cmb_area.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_area.Enabled = false;
            this.cmb_area.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_area.FormattingEnabled = true;
            this.cmb_area.Location = new System.Drawing.Point(50, 260);
            this.cmb_area.Name = "cmb_area";
            this.cmb_area.Size = new System.Drawing.Size(300, 23);
            this.cmb_area.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.Location = new System.Drawing.Point(50, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Area:";
            // 
            // cmb_city
            // 
            this.cmb_city.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_city.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmb_city.FormattingEnabled = true;
            this.cmb_city.Location = new System.Drawing.Point(50, 205);
            this.cmb_city.Name = "cmb_city";
            this.cmb_city.Size = new System.Drawing.Size(300, 23);
            this.cmb_city.TabIndex = 6;
            this.cmb_city.SelectedIndexChanged += new System.EventHandler(this.cmb_city_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.Location = new System.Drawing.Point(50, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "City:";
            // 
            // txt_nid
            // 
            this.txt_nid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_nid.Location = new System.Drawing.Point(50, 150);
            this.txt_nid.MaxLength = 17;
            this.txt_nid.Name = "txt_nid";
            this.txt_nid.Size = new System.Drawing.Size(300, 23);
            this.txt_nid.TabIndex = 4;
            this.txt_nid.TextChanged += new System.EventHandler(this.txt_nid_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.Location = new System.Drawing.Point(50, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "NID Number:";
            // 
            // txt_fullname
            // 
            this.txt_fullname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_fullname.Location = new System.Drawing.Point(50, 95);
            this.txt_fullname.Name = "txt_fullname";
            this.txt_fullname.Size = new System.Drawing.Size(300, 23);
            this.txt_fullname.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.Location = new System.Drawing.Point(50, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Full Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(71)))));
            this.label1.Location = new System.Drawing.Point(75, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Register New Account";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "City Corporation - Registration";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_fullname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_nid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_city;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_area;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_phone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_confirm;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_register;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}