using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace citycorporation
{
    public partial class Form2 : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadCities();
        }

        private void LoadCities()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CityID, CityName FROM Cities WHERE IsActive = 1 ORDER BY CityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        cmb_city.Items.Clear();

                        while (reader.Read())
                        {
                            cmb_city.Items.Add(new
                            {
                                CityID = reader.GetInt32(0),
                                CityName = reader.GetString(1)
                            });
                        }

                        cmb_city.DisplayMember = "CityName";
                        cmb_city.ValueMember = "CityID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cities: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAreas(int cityId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT AreaID, AreaName FROM Areas WHERE CityID = @CityID AND IsActive = 1 ORDER BY AreaName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CityID", cityId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        cmb_area.Items.Clear();

                        while (reader.Read())
                        {
                            cmb_area.Items.Add(new
                            {
                                AreaID = reader.GetInt32(0),
                                AreaName = reader.GetString(1)
                            });
                        }

                        cmb_area.DisplayMember = "AreaName";
                        cmb_area.ValueMember = "AreaID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading areas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_city_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_city.SelectedItem != null)
            {
                dynamic selectedCity = cmb_city.SelectedItem;
                LoadAreas(selectedCity.CityID);
                cmb_area.Enabled = true;
                cmb_area.SelectedIndex = -1;
            }
        }

        private void txt_nid_TextChanged(object sender, EventArgs e)
        {
            string text = txt_nid.Text;
            string numbersOnly = "";
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    numbersOnly += c;
            }
            if (text != numbersOnly)
            {
                int cursorPosition = txt_nid.SelectionStart;
                txt_nid.Text = numbersOnly;
                txt_nid.SelectionStart = Math.Max(0, cursorPosition - 1);
            }
        }

        private void txt_phone_TextChanged(object sender, EventArgs e)
        {
            string text = txt_phone.Text;
            string numbersOnly = "";
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                    numbersOnly += c;
            }
            if (text != numbersOnly)
            {
                int cursorPosition = txt_phone.SelectionStart;
                txt_phone.Text = numbersOnly;
                txt_phone.SelectionStart = Math.Max(0, cursorPosition - 1);
            }
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_fullname.Text))
            {
                MessageBox.Show("Please enter your full name", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_nid.Text) || txt_nid.Text.Length < 10)
            {
                MessageBox.Show("Please enter a valid NID (at least 10 digits)", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmb_city.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a city", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmb_area.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an area", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dynamic selectedCity = cmb_city.SelectedItem;
            dynamic selectedArea = cmb_area.SelectedItem;
            string city = selectedCity.CityName;
            string area = selectedArea.AreaName;

            if (string.IsNullOrWhiteSpace(txt_phone.Text) || txt_phone.Text.Length != 11)
            {
                MessageBox.Show("Please enter a valid phone number (11 digits)", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_password.Text))
            {
                MessageBox.Show("Please enter a password", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txt_password.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool hasUpper = false, hasLower = false, hasDigit = false;
            foreach (char c in txt_password.Text)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            if (!hasUpper || !hasLower || !hasDigit)
            {
                MessageBox.Show(
                    "Password must contain:\n• At least one uppercase letter\n• At least one lowercase letter\n• At least one number",
                    "Weak Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (txt_password.Text != txt_confirm.Text)
            {
                MessageBox.Show("Passwords do not match", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE NID = @NID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@NID", txt_nid.Text);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "This NID is already registered.\n\nPlease use a different NID or login with existing account.",
                                "NID Already Exists",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return;
                        }
                    }

                    string salt = GenerateSalt();
                    string passwordHash = HashPassword(txt_password.Text, salt);

                    string insertQuery = @"
                        INSERT INTO Users (NID, FullName, PhoneNumber, PasswordHash, Salt, UserRole, IsActive, Area, Address, CreatedDate)
                        VALUES (@NID, @FullName, @Phone, @PasswordHash, @Salt, 'Citizen', 1, @Area, @Address, @CreatedDate)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@NID", txt_nid.Text);
                        cmd.Parameters.AddWithValue("@FullName", txt_fullname.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txt_phone.Text);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Salt", salt);
                        cmd.Parameters.AddWithValue("@Area", area);
                        cmd.Parameters.AddWithValue("@Address", $"{area}, {city}");
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show(
                                $"Registration successful!\n\n" +
                                $"Name: {txt_fullname.Text}\n" +
                                $"NID: {txt_nid.Text}\n" +
                                $"City: {city}\n" +
                                $"Area: {area}\n\n" +
                                $"You can now login with your NID and password.",
                                "Registration Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
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
    }
}