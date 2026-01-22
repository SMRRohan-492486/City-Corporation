using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace citycorporation
{
    public partial class Mayor1 : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        private int userId;
        private string fullName;

        // ============================================
        // CONSTRUCTOR
        // ============================================
        public Mayor1(int userId, string fullName, string role, string area, string nid)
        {
            InitializeComponent();
            this.userId = userId;
            this.fullName = fullName;
            this.Load += Mayor1_Load;
        }

        // ============================================
        // FORM LOAD
        // ============================================
        private void Mayor1_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome, {fullName}";
            label2.Text = "Mayor - City Corporation";
            label3.Visible = false;
            ShowDashboard();
        }

        // ============================================
        // COMMON RESET
        // ============================================
        private void ResetPanel()
        {
            panel3.Controls.Clear();
            panel3.BringToFront();
            panel3.Refresh();
        }

        // ============================================
        // DASHBOARD
        // ============================================
        private void button1_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            ResetPanel();

            // Title
            Label titleLabel = new Label
            {
                Text = "Dashboard Overview",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 102),
                Location = new Point(30, 20),
                AutoSize = true
            };
            panel3.Controls.Add(titleLabel);

            // Section: Complaints Statistics
            Label sectionLabel = new Label
            {
                Text = "Complaints in City Corporation:",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(30, 80),
                AutoSize = true
            };
            panel3.Controls.Add(sectionLabel);

            // Get statistics
            int totalComplaints = ExecuteScalar("SELECT COUNT(*) FROM Complaints");
            int pending = ExecuteScalar("SELECT COUNT(*) FROM Complaints WHERE Status='Pending'");
            int inProgress = ExecuteScalar("SELECT COUNT(*) FROM Complaints WHERE Status='In Progress'");
            int resolved = ExecuteScalar("SELECT COUNT(*) FROM Complaints WHERE Status='Resolved'");
            int rejected = ExecuteScalar("SELECT COUNT(*) FROM Complaints WHERE Status='Rejected'");

            // Calculate resolution rate
            double resolutionRate = totalComplaints > 0 ? (resolved * 100.0 / totalComplaints) : 0;

            // Stats with icons
            int yPos = 130;
            AddStatLine("📊", $"Total Complaints: {totalComplaints}", yPos, new Font("Segoe UI", 12, FontStyle.Bold));
            yPos += 50;
            AddStatLine("⏳", $"Pending: {pending}", yPos, new Font("Segoe UI", 11, FontStyle.Regular));
            yPos += 40;
            AddStatLine("🔄", $"In Progress: {inProgress}", yPos, new Font("Segoe UI", 11, FontStyle.Regular));
            yPos += 40;
            AddStatLine("✅", $"Resolved: {resolved}", yPos, new Font("Segoe UI", 11, FontStyle.Regular));
            yPos += 40;
            AddStatLine("❌", $"Rejected: {rejected}", yPos, new Font("Segoe UI", 11, FontStyle.Regular));
            yPos += 60;

            // Resolution Rate
            Label resolutionLabel = new Label
            {
                Text = $"Resolution Rate: {resolutionRate:F1}%",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 102),
                Location = new Point(30, yPos),
                AutoSize = true
            };
            panel3.Controls.Add(resolutionLabel);
        }

        private void AddStatLine(string icon, string text, int y, Font font)
        {
            Label statLabel = new Label
            {
                Text = $"{icon}  {text}",
                Font = font,
                ForeColor = Color.FromArgb(64, 64, 64),
                Location = new Point(30, y),
                AutoSize = true
            };
            panel3.Controls.Add(statLabel);
        }

        // ============================================
        // ALL COMPLAINTS
        // ============================================
        private void button2_Click(object sender, EventArgs e)
        {
            ResetPanel();

            DataGridView dgv = CreateGrid();
            panel3.Controls.Add(dgv);

            LoadGrid(dgv,
                "SELECT ComplaintID, ComplaintType, Area, Priority, Status FROM Complaints");

            AddApproveRejectButtons(dgv);
        }

        // ============================================
        // AREA-WISE REPORT
        // ============================================
        private void button3_Click(object sender, EventArgs e)
        {
            ResetPanel();

            Label lbl = new Label
            {
                Text = "Area-wise Complaint Report",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            panel3.Controls.Add(lbl);

            DataGridView dgv = CreateGrid();
            dgv.Location = new Point(30, 70);
            panel3.Controls.Add(dgv);

            LoadGrid(dgv,
                @"SELECT Area,
                         COUNT(*) AS TotalComplaints,
                         SUM(CASE WHEN Status='Pending' THEN 1 ELSE 0 END) AS Pending,
                         SUM(CASE WHEN Status='Resolved' THEN 1 ELSE 0 END) AS Resolved
                  FROM Complaints
                  GROUP BY Area");
        }

        // ============================================
        // COUNSELOR PERFORMANCE
        // ============================================
        private void button4_Click(object sender, EventArgs e)
        {
            ResetPanel();

            Label lbl = new Label
            {
                Text = "Counselor Performance",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            panel3.Controls.Add(lbl);

            DataGridView dgv = CreateGrid();
            dgv.Location = new Point(30, 70);
            panel3.Controls.Add(dgv);

            // Query using Users table filtered by Counselor role
            LoadGridSafe(dgv,
                @"SELECT u.FullName AS CounselorName, 
                         u.Area,
                         u.PhoneNumber,
                         COUNT(c.ComplaintID) AS TotalHandled,
                         SUM(CASE WHEN c.Status='Resolved' THEN 1 ELSE 0 END) AS Resolved,
                         SUM(CASE WHEN c.Status='Pending' THEN 1 ELSE 0 END) AS Pending,
                         SUM(CASE WHEN c.Status='In Progress' THEN 1 ELSE 0 END) AS InProgress,
                         SUM(CASE WHEN c.Status='Rejected' THEN 1 ELSE 0 END) AS Rejected
                  FROM Users u
                  LEFT JOIN Complaints c ON u.Area = c.Area
                  WHERE u.UserRole = 'Counselor'
                  GROUP BY u.FullName, u.Area, u.PhoneNumber
                  ORDER BY TotalHandled DESC");
        }

        // ============================================
        // SERVICE REQUESTS
        // ============================================
        private void button5_Click(object sender, EventArgs e)
        {
            ResetPanel();

            Label lbl = new Label
            {
                Text = "Service Requests",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            panel3.Controls.Add(lbl);

            DataGridView dgv = CreateGrid();
            dgv.Location = new Point(30, 70);
            panel3.Controls.Add(dgv);

            // Query matching the actual ServiceRequests table schema
            LoadGridSafe(dgv,
                @"SELECT sr.RequestID, 
                         s.ServiceName AS ServiceType,
                         u.Area,
                         sr.Status,
                         sr.RequestDate AS CreatedAt,
                         u.FullName AS CitizenName,
                         sr.Remarks
                  FROM ServiceRequests sr
                  INNER JOIN Users u ON sr.UserID = u.UserID
                  INNER JOIN Services s ON sr.ServiceID = s.ServiceID
                  ORDER BY sr.RequestDate DESC");
        }

        // ============================================
        // HELPER METHODS
        // ============================================
        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Location = new Point(30, 30),
                Size = new Size(900, 400),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
        }

        private void LoadGrid(DataGridView dgv, string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter ad = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                dgv.DataSource = dt;
            }
        }

        // Safe version with error handling
        private void LoadGridSafe(DataGridView dgv, string query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter ad = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    dgv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Create empty table to prevent crash
                DataTable emptyDt = new DataTable();
                emptyDt.Columns.Add("Message");
                emptyDt.Rows.Add("No data available");
                dgv.DataSource = emptyDt;
            }
        }

        private int ExecuteScalar(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Safe version that returns 0 on error
        private int ExecuteScalarSafe(string query)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private void AddApproveRejectButtons(DataGridView dgv)
        {
            Button approve = new Button
            {
                Text = "Approve",
                Location = new Point(30, 450),
                Size = new Size(150, 40),
                BackColor = Color.Green,
                ForeColor = Color.White
            };

            Button reject = new Button
            {
                Text = "Reject",
                Location = new Point(200, 450),
                Size = new Size(150, 40),
                BackColor = Color.Red,
                ForeColor = Color.White
            };

            approve.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["ComplaintID"].Value);
                UpdateStatus(id, "In Progress", "Approved by Mayor");
                button2_Click(null, null);
            };

            reject.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["ComplaintID"].Value);
                UpdateStatus(id, "Rejected", "Rejected by Mayor");
                button2_Click(null, null);
            };

            panel3.Controls.Add(approve);
            panel3.Controls.Add(reject);
        }

        private void UpdateStatus(int id, string status, string remarks)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Complaints SET Status=@S, Remarks=@R WHERE ComplaintID=@ID", con);
                cmd.Parameters.AddWithValue("@S", status);
                cmd.Parameters.AddWithValue("@R", remarks);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }

        // ============================================
        // LOGOUT
        // ============================================
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            new Form1().Show();
        }
    }
}