using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace citycorporation
{
    public partial class Counselor2 : Form
    {
        private readonly string connectionString =
            @"Data Source=.\SQLEXPRESS;Initial Catalog=CityCorporationDB;Integrated Security=True;TrustServerCertificate=True;";

        private int userId;
        private string fullName;
        private string userRole;
        private string area;
        private string nid;

        public Counselor2(int userId, string fullName, string userRole, string area, string nid)
        {
            InitializeComponent();

            this.userId = userId;
            this.fullName = fullName;
            this.userRole = userRole;
            this.area = area;
            this.nid = nid;
        }

        private void Counselor2_Load(object sender, EventArgs e)
        {
            label1.Text = $"Welcome, {fullName}!";
            label2.Text = $"Counselor - {area} Area";

            panel1.BackColor = ColorTranslator.FromHtml("#006747");
            panel2.BackColor = ColorTranslator.FromHtml("#F8F9FA");
            panel3.BackColor = Color.White;

            ShowDashboard();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            panel3.Controls.Clear();

            Label lblTitle = new Label
            {
                Text = "Dashboard Overview",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
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

                    string query = @"
                        SELECT 
                            COUNT(*),
                            SUM(CASE WHEN Status = 'Pending' THEN 1 ELSE 0 END),
                            SUM(CASE WHEN Status = 'In Progress' THEN 1 ELSE 0 END),
                            SUM(CASE WHEN Status = 'Resolved' THEN 1 ELSE 0 END),
                            SUM(CASE WHEN Status = 'Rejected' THEN 1 ELSE 0 END)
                        FROM Complaints
                        WHERE Area = @Area";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Area", area);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int total = reader.GetInt32(0);
                            int pending = reader.GetInt32(1);
                            int inProgress = reader.GetInt32(2);
                            int resolved = reader.GetInt32(3);
                            int rejected = reader.GetInt32(4);

                            Label lblStats = new Label
                            {
                                Text =
                                    $"Complaints in {area} Area:\n\n" +
                                    $"📊 Total Complaints: {total}\n\n" +
                                    $"⏳ Pending: {pending}\n" +
                                    $"🔄 In Progress: {inProgress}\n" +
                                    $"✅ Resolved: {resolved}\n" +
                                    $"❌ Rejected: {rejected}\n\n" +
                                    $"Resolution Rate: {(total > 0 ? (resolved * 100.0 / total).ToString("F1") : "0")}%",
                                Font = new Font("Segoe UI", 14),
                                Location = new Point(50, 80),
                                Size = new Size(600, 400)
                            };
                            panel3.Controls.Add(lblStats);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message, "Error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Controls.Clear();

            Label lblTitle = new Label
            {
                Text = $"Complaints in {area} Area",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#006747"),
                Location = new Point(50, 30),
                AutoSize = true
            };
            panel3.Controls.Add(lblTitle);

            Label lblFilter = new Label
            {
                Text = "Filter:",
                Location = new Point(50, 70),
                AutoSize = true
            };
            panel3.Controls.Add(lblFilter);

            ComboBox cmbStatus = new ComboBox
            {
                Location = new Point(110, 67),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new string[] { "All", "Pending", "In Progress", "Resolved", "Rejected" });
            cmbStatus.SelectedIndex = 0;
            panel3.Controls.Add(cmbStatus);

            DataGridView dgv = new DataGridView
            {
                Location = new Point(50, 110),
                Size = new Size(700, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            panel3.Controls.Add(dgv);

            Button btnLoad = new Button
            {
                Text = "Load",
                Location = new Point(280, 67),
                Size = new Size(80, 30),
                BackColor = ColorTranslator.FromHtml("#006747"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLoad.Click += (s, ev) => LoadComplaintsGrid(dgv, cmbStatus.SelectedItem.ToString());
            panel3.Controls.Add(btnLoad);

            Button btnUpdate = new Button
            {
                Text = "Update Selected Complaint",
                Location = new Point(50, 540),
                Size = new Size(220, 35),
                BackColor = ColorTranslator.FromHtml("#006747"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.Click += (s, ev) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    int complaintId = Convert.ToInt32(dgv.SelectedRows[0].Cells["ID"].Value);
                    UpdateComplaintStatus(complaintId);
                    LoadComplaintsGrid(dgv, cmbStatus.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("Please select a complaint first");
                }
            };
            panel3.Controls.Add(btnUpdate);

            LoadComplaintsGrid(dgv, "All");
        }

        private void LoadComplaintsGrid(DataGridView dgv, string statusFilter)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        C.ComplaintID as 'ID',
                        U.FullName as 'Citizen Name',
                        C.ComplaintType as 'Type',
                        C.Description,
                        C.Status,
                        C.Priority,
                        C.CreatedDate as 'Submitted Date',
                        C.Remarks
                    FROM Complaints C
                    INNER JOIN Users U ON C.UserID = U.UserID
                    WHERE C.Area = @Area";

                if (statusFilter != "All")
                {
                    query += " AND C.Status = @Status";
                }

                query += " ORDER BY C.CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Area", area);
                    if (statusFilter != "All")
                        cmd.Parameters.AddWithValue("@Status", statusFilter);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void UpdateComplaintStatus(int complaintId)
        {
            Form updateForm = new Form
            {
                Text = "Update Complaint Status",
                Size = new Size(450, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            Label lblStatus = new Label { Text = "New Status:", Location = new Point(30, 30) };
            updateForm.Controls.Add(lblStatus);

            ComboBox cmbNewStatus = new ComboBox
            {
                Location = new Point(30, 60),
                Size = new Size(370, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNewStatus.Items.AddRange(new string[] { "Pending", "In Progress", "Resolved", "Rejected" });
            cmbNewStatus.SelectedIndex = 1;
            updateForm.Controls.Add(cmbNewStatus);

            TextBox txtRemarks = new TextBox
            {
                Location = new Point(30, 120),
                Size = new Size(370, 80),
                Multiline = true
            };
            updateForm.Controls.Add(txtRemarks);

            Button btnSave = new Button
            {
                Text = "UPDATE STATUS",
                Location = new Point(130, 220),
                Size = new Size(180, 40),
                BackColor = ColorTranslator.FromHtml("#006747"),
                ForeColor = Color.White
            };

            btnSave.Click += (s, ev) =>
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        UPDATE Complaints
                        SET Status = @Status,
                            Remarks = @Remarks,
                            AssignedTo = @CounselorID,
                            ResolvedDate = CASE WHEN @Status = 'Resolved' THEN GETDATE() ELSE NULL END
                        WHERE ComplaintID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", cmbNewStatus.Text);
                        cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
                        cmd.Parameters.AddWithValue("@CounselorID", userId);
                        cmd.Parameters.AddWithValue("@ID", complaintId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Complaint updated successfully");
                updateForm.Close();
            };

            updateForm.Controls.Add(btnSave);
            updateForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
