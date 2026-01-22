using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace citycorporation
{
    public partial class Citizen : Form
    {
        // CONNECTION STRING - UPDATE THIS!
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        // User information
        private int userId;
        private string fullName;
        private string userRole;
        private string area;
        private string nid;

        // Constructor with user data from login
        public Citizen(int userId, string fullName, string userRole, string area, string nid)
        {
            InitializeComponent();

            this.userId = userId;
            this.fullName = fullName;
            this.userRole = userRole;
            this.area = area;
            this.nid = nid;
        }

        // ============================================
        // FORM LOAD
        // ============================================
        private void Form4_Load(object sender, EventArgs e)
        {
            // Set welcome message
            label1.Text = $"Welcome, {fullName}!";
            label2.Text = $"Citizen - {area} Area";

            // Set colors
            panel1.BackColor = ColorTranslator.FromHtml("#006747"); // Green header
            panel2.BackColor = ColorTranslator.FromHtml("#F8F9FA"); // Light gray menu
            panel3.BackColor = Color.White;

            // Show initial dashboard
            ShowDashboard();
        }

        // ============================================
        // BUTTON 1 - DASHBOARD
        // ============================================
        private void button1_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "Welcome to City Corporation Portal",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Get user's complaint statistics - FIXED WITH ISNULL
                    string query = @"
                        SELECT 
                            ISNULL(COUNT(*), 0) as Total,
                            ISNULL(SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END), 0) as Pending,
                            ISNULL(SUM(CASE WHEN Status = 'In Progress' THEN 1 ELSE 0 END), 0) as InProgress,
                            ISNULL(SUM(CASE WHEN Status = 'Resolved' THEN 1 ELSE 0 END), 0) as Resolved
                        FROM Complaints
                        WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        int total = 0;
                        int pending = 0;
                        int inProgress = 0;
                        int resolved = 0;

                        if (reader.Read())
                        {
                            total = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            pending = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            inProgress = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            resolved = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        }

                        reader.Close();

                        System.Windows.Forms.Label lblInfo = new System.Windows.Forms.Label
                        {
                            Text = $"Hello {fullName}!\n\n" +
                                   $"Your NID: {nid}\n" +
                                   $"Area: {area}\n\n" +
                                   $"Your Complaints:\n" +
                                   $"📊 Total: {total}\n" +
                                   $"⏳ Pending: {pending}\n" +
                                   $"🔄 In Progress: {inProgress}\n" +
                                   $"✅ Resolved: {resolved}\n\n" +
                                   $"Use the menu to:\n" +
                                   $"• Submit new complaints\n" +
                                   $"• Request services\n" +
                                   $"• Track your requests",
                            Font = new Font("Segoe UI", 12),
                            Location = new Point(50, 90),
                            Size = new Size(600, 450)
                        };
                        panel3.Controls.Add(lblInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // BUTTON 2 - MY PROFILE
        // ============================================
        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "My Profile",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            System.Windows.Forms.Label lblProfile = new System.Windows.Forms.Label
            {
                Text = $"Full Name: {fullName}\n\n" +
                       $"NID: {nid}\n\n" +
                       $"Role: {userRole}\n\n" +
                       $"Area: {area}\n\n" +
                       $"User ID: {userId}",
                Font = new Font("Segoe UI", 12),
                Location = new Point(50, 80),
                Size = new Size(500, 300)
            };
            panel3.Controls.Add(lblProfile);
        }

        // ============================================
        // BUTTON 3 - SUBMIT COMPLAINT
        // ============================================
        private void button3_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "Submit New Complaint",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            // Complaint Type
            System.Windows.Forms.Label lblType = new System.Windows.Forms.Label
            {
                Text = "Complaint Type:",
                Location = new Point(50, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(lblType);

            ComboBox cmbType = new ComboBox
            {
                Location = new Point(50, 110),
                Size = new Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbType.Items.AddRange(new string[] {
                "Road Damage",
                "Street Light",
                "Drainage Issue",
                "Waste Collection",
                "Water Supply",
                "Illegal Construction",
                "Noise Pollution",
                "Other"
            });
            panel3.Controls.Add(cmbType);

            // Priority
            System.Windows.Forms.Label lblPriority = new System.Windows.Forms.Label
            {
                Text = "Priority:",
                Location = new Point(50, 160),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(lblPriority);

            ComboBox cmbPriority = new ComboBox
            {
                Location = new Point(50, 190),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbPriority.Items.AddRange(new string[] { "Low", "Normal", "High", "Urgent" });
            cmbPriority.SelectedIndex = 1; // Default: Normal
            panel3.Controls.Add(cmbPriority);

            // Description
            System.Windows.Forms.Label lblDesc = new System.Windows.Forms.Label
            {
                Text = "Description:",
                Location = new Point(50, 240),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(lblDesc);

            TextBox txtDesc = new TextBox
            {
                Location = new Point(50, 270),
                Size = new Size(400, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(txtDesc);

            // Submit button
            Button btnSubmit = new Button
            {
                Text = "SUBMIT COMPLAINT",
                Location = new Point(50, 390),
                Size = new Size(200, 40),
                BackColor = ColorTranslator.FromHtml("#006747"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSubmit.Click += (s, ev) => {
                if (cmbType.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select complaint type", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtDesc.Text))
                {
                    MessageBox.Show("Please enter description", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // SAVE TO DATABASE
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"
                            INSERT INTO Complaints (UserID, ComplaintType, Description, Status, Priority, CreatedDate, Area)
                            VALUES (@UserID, @Type, @Description, 'Pending', @Priority, @CreatedDate, @Area);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@Type", cmbType.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Description", txtDesc.Text.Trim());
                            cmd.Parameters.AddWithValue("@Priority", cmbPriority.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Area", area);

                            int complaintId = (int)cmd.ExecuteScalar();

                            MessageBox.Show(
                                $"Complaint submitted successfully!\n\n" +
                                $"Complaint ID: {complaintId}\n" +
                                $"Type: {cmbType.SelectedItem}\n" +
                                $"Priority: {cmbPriority.SelectedItem}\n" +
                                $"Status: Pending\n\n" +
                                $"You will be notified when status changes.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            // Clear form
                            cmbType.SelectedIndex = -1;
                            cmbPriority.SelectedIndex = 1;
                            txtDesc.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error submitting complaint: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel3.Controls.Add(btnSubmit);
        }

        // ============================================
        // BUTTON 4 - MY COMPLAINTS
        // ============================================
        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "My Complaints",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            // DataGridView to show complaints
            DataGridView dgv = new DataGridView
            {
                Location = new Point(50, 80),
                Size = new Size(700, 450),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            panel3.Controls.Add(dgv);

            // Load complaints from database
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            ComplaintID as 'ID',
                            ComplaintType as 'Type',
                            Description,
                            Status,
                            Priority,
                            CreatedDate as 'Submitted On',
                            Remarks
                        FROM Complaints
                        WHERE UserID = @UserID
                        ORDER BY CreatedDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgv.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            System.Windows.Forms.Label lblNoData = new System.Windows.Forms.Label
                            {
                                Text = "No complaints submitted yet.\n\nClick 'Submit Complaint' to create one.",
                                Location = new Point(50, 200),
                                AutoSize = true,
                                Font = new Font("Segoe UI", 11)
                            };
                            panel3.Controls.Add(lblNoData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading complaints: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // BUTTON 5 - REQUEST SERVICE
        // ============================================
        private void button5_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "Request Service",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            // Service Selection
            System.Windows.Forms.Label lblService = new System.Windows.Forms.Label
            {
                Text = "Select Service:",
                Location = new Point(50, 80),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(lblService);

            ComboBox cmbService = new ComboBox
            {
                Location = new Point(50, 110),
                Size = new Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(cmbService);

            // Load services from database
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ServiceID, ServiceName FROM Services WHERE IsActive = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbService.Items.Add(new { ID = reader.GetInt32(0), Name = reader.GetString(1) });
                        }
                        cmbService.DisplayMember = "Name";
                        cmbService.ValueMember = "ID";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading services: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Remarks
            System.Windows.Forms.Label lblRemarks = new System.Windows.Forms.Label
            {
                Text = "Additional Information:",
                Location = new Point(50, 160),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(lblRemarks);

            TextBox txtRemarks = new TextBox
            {
                Location = new Point(50, 190),
                Size = new Size(400, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10)
            };
            panel3.Controls.Add(txtRemarks);

            // Submit button
            Button btnRequest = new Button
            {
                Text = "REQUEST SERVICE",
                Location = new Point(50, 290),
                Size = new Size(200, 40),
                BackColor = ColorTranslator.FromHtml("#006747"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRequest.Click += (s, ev) => {
                if (cmbService.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a service", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        dynamic selectedService = cmbService.SelectedItem;
                        int serviceId = selectedService.ID;

                        string query = @"
                            INSERT INTO ServiceRequests (UserID, ServiceID, RequestDate, Status, Remarks)
                            VALUES (@UserID, @ServiceID, @RequestDate, 'Pending', @Remarks);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@ServiceID", serviceId);
                            cmd.Parameters.AddWithValue("@RequestDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());

                            int requestId = (int)cmd.ExecuteScalar();

                            MessageBox.Show(
                                $"Service request submitted successfully!\n\n" +
                                $"Request ID: {requestId}\n" +
                                $"Service: {selectedService.Name}\n" +
                                $"Status: Pending\n\n" +
                                $"You will be notified when processed.",
                                "Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            cmbService.SelectedIndex = -1;
                            txtRemarks.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error submitting request: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panel3.Controls.Add(btnRequest);
        }

        // ============================================
        // BUTTON 6 - MY REQUESTS
        // ============================================
        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "My Service Requests",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            // DataGridView
            DataGridView dgv = new DataGridView
            {
                Location = new Point(50, 80),
                Size = new Size(700, 450),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            panel3.Controls.Add(dgv);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            SR.RequestID as 'ID',
                            S.ServiceName as 'Service',
                            SR.RequestDate as 'Requested On',
                            SR.Status,
                            SR.CompletionDate as 'Completed On',
                            SR.Remarks
                        FROM ServiceRequests SR
                        INNER JOIN Services S ON SR.ServiceID = S.ServiceID
                        WHERE SR.UserID = @UserID
                        ORDER BY SR.RequestDate DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgv.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            System.Windows.Forms.Label lblNoData = new System.Windows.Forms.Label
                            {
                                Text = "No service requests yet.\n\nClick 'Request Service' to create one.",
                                Location = new Point(50, 200),
                                AutoSize = true,
                                Font = new Font("Segoe UI", 11)
                            };
                            panel3.Controls.Add(lblNoData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading requests: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================
        // BUTTON 7 - LOGOUT
        // ============================================
        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Logged out successfully!", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}