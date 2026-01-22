using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace citycorporation
{
    public partial class Form3 : Form
    {
        
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Reset Password";

                
                txt_newPassword.PasswordChar = '●';
                txt_confirmPassword.PasswordChar = '●';

                
                txt_nid.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            string nid = txt_nid.Text.Trim();
            string newPassword = txt_newPassword.Text;
            string confirmPassword = txt_confirmPassword.Text;

            
            if (string.IsNullOrEmpty(nid))
            {
                MessageBox.Show("Please enter your NID number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_nid.Focus();
                return;
            }

            if (nid.Length < 10)
            {
                MessageBox.Show("NID must be at least 10 digits", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_nid.Focus();
                return;
            }

            
            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please enter a new password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_newPassword.Focus();
                return;
            }

            if (newPassword.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_newPassword.Focus();
                return;
            }

            if (!Regex.IsMatch(newPassword, @"[A-Z]"))
            {
                MessageBox.Show("Password must contain at least one uppercase letter", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_newPassword.Focus();
                return;
            }

            if (!Regex.IsMatch(newPassword, @"[a-z]"))
            {
                MessageBox.Show("Password must contain at least one lowercase letter", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_newPassword.Focus();
                return;
            }

            if (!Regex.IsMatch(newPassword, @"[0-9]"))
            {
                MessageBox.Show("Password must contain at least one number", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_newPassword.Focus();
                return;
            }

            
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_confirmPassword.Focus();
                return;
            }

            
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    
                    string checkQuery = "SELECT COUNT(*), UserID, FullName FROM Users WHERE NID = @NID GROUP BY UserID, FullName";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@NID", nid);

                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show(
                                    "NID not found in the system.\n\nPlease check your NID or register a new account.",
                                    "NID Not Found",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                                txt_nid.Clear();
                                txt_nid.Focus();
                                return;
                            }

                            string userName = reader.GetString(2);
                            reader.Close();

                            
                            string newSalt = GenerateSalt();
                            string newHash = HashPassword(newPassword, newSalt);

                            
                            string updateQuery = @"
                                UPDATE Users 
                                SET PasswordHash = @Hash, 
                                    Salt = @Salt 
                                WHERE NID = @NID";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@Hash", newHash);
                                updateCmd.Parameters.AddWithValue("@Salt", newSalt);
                                updateCmd.Parameters.AddWithValue("@NID", nid);

                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show(
                                        $"Password reset successful, {userName}!\n\nYou can now login with your new password.",
                                        "Success",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information
                                    );

                                    
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show(
                                        "Password reset failed. Please try again.",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                    );
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show(
                    "Database error!\n\n" +
                    "Error: " + sqlEx.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}