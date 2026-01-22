using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace citycorporation
{
    public partial class Form1 : Form
    {
        // CONNECTION STRING - UPDATE THIS!
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set form colors
            panel1.BackColor = Color.White;
        }

        // ============================================
        // LOGIN BUTTON CLICK
        // ============================================
        private void button1_Click(object sender, EventArgs e)
        {
            string nid = textBox1.Text.Trim();
            string password = textBox2.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(nid))
            {
                MessageBox.Show("Please enter your NID", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter your password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Authenticate user
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT UserID, FullName, PasswordHash, Salt, UserRole, IsActive, Area 
                        FROM Users 
                        WHERE NID = @NID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NID", nid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Check if user is active
                            bool isActive = reader.GetBoolean(5);
                            if (!isActive)
                            {
                                MessageBox.Show("Your account has been deactivated. Please contact the administrator.", "Account Disabled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Get user data
                            int userId = reader.GetInt32(0);
                            string fullName = reader.GetString(1);
                            string storedHash = reader.GetString(2);
                            string salt = reader.GetString(3);
                            string role = reader.GetString(4);
                            string area = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            // Verify password
                            string passwordHash = HashPassword(password, salt);

                            if (passwordHash == storedHash)
                            {
                                MessageBox.Show($"Welcome, {fullName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Hide login form
                                this.Hide();

                                // Route to appropriate dashboard based on role
                                if (role == "Citizen")
                                {
                                    Citizen citizenDashboard = new Citizen(userId, fullName, role, area, nid);
                                    citizenDashboard.ShowDialog();
                                }
                                else if (role == "Counselor")
                                {
                                    Counselor2 counselorDashboard = new Counselor2(userId, fullName, role, area, nid);
                                    counselorDashboard.ShowDialog();
                                }
                                else if (role == "Secretary")
                                {
                                    MessageBox.Show("Secretary dashboard is under development.", "Coming Soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else if (role == "Mayor")
                                {
                                    Mayor1 mayorDashboard = new Mayor1(userId, fullName, role, area, nid);
                                    mayorDashboard.ShowDialog();
                                }

                                // Show login form again after dashboard closes
                                this.Show();

                                // Clear password field
                                textBox2.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Invalid password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("NID not found. Please check your NID or register a new account.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // SIGN UP LINK CLICKED
        // ============================================
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Hide login form
            this.Hide();

            // Open registration form as modal dialog
            Form2 registrationForm = new Form2();
            registrationForm.ShowDialog();

            // Show login form again after registration closes
            this.Show();
        }

        // ============================================
        // FORGOT PASSWORD LINK CLICKED
        // ============================================
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Hide login form
            this.Hide();

            // Open forgot password form as modal dialog
            Form3 forgotPasswordForm = new Form3();
            forgotPasswordForm.ShowDialog();

            // Show login form again after forgot password closes
            this.Show();
        }

        // ============================================
        // HASH PASSWORD
        // ============================================
        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}