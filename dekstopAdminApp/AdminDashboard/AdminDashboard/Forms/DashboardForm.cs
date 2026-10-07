using System.Runtime.Versioning;
using AdminDashboard.Models;
using AdminDashboard.Services;

namespace AdminDashboard.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class DashboardForm : Form
    {
        private readonly AdminService _adminService;

        private TabControl? tabControl;
        private TabPage? postsTab;
        private TabPage? usersTab;
        private TabPage? commentsTab;
        private DataGridView? postsGrid;
        private DataGridView? usersGrid;
        private DataGridView? commentsGrid;

        public DashboardForm()
        {
            InitializeComponent();
            string token = Properties.Settings.Default.AuthToken ?? "";
            _adminService = new AdminService(token);
            InitializeDashboardControls();
            _ = LoadDataAsync();
        }

        private void InitializeDashboardControls()
        {
            tabControl = new TabControl { Dock = DockStyle.Fill };
            postsTab = new TabPage { Text = "Pending Posts" };
            usersTab = new TabPage { Text = "Users" };
            commentsTab = new TabPage { Text = "Comments" };

            postsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            usersGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            commentsGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Text = "Admin Dashboard";
            Size = new Size(1024, 768);
            StartPosition = FormStartPosition.CenterScreen;

            if (postsTab != null && postsGrid != null)
                postsTab.Controls.Add(postsGrid);

            if (usersTab != null && usersGrid != null)
                usersTab.Controls.Add(usersGrid);

            if (commentsTab != null && commentsGrid != null)
                commentsTab.Controls.Add(commentsGrid);

            if (tabControl != null)
            {
                tabControl.Controls.AddRange(new Control[] { postsTab!, usersTab!, commentsTab! });
                Controls.Add(tabControl);
            }

            AddButtonColumns();

            if (postsGrid != null)
                postsGrid.CellClick += PostsGrid_CellClick;

            if (usersGrid != null)
                usersGrid.CellClick += UsersGrid_CellClick;
        }

        private void AddButtonColumns()
        {
            if (postsGrid == null || usersGrid == null) return;

            var approveButton = new DataGridViewButtonColumn
            {
                Name = "Approve",
                Text = "Approve",
                UseColumnTextForButtonValue = true
            };
            postsGrid.Columns.Add(approveButton);

            var rejectButton = new DataGridViewButtonColumn
            {
                Name = "Reject",
                Text = "Reject",
                UseColumnTextForButtonValue = true
            };
            postsGrid.Columns.Add(rejectButton);

            var banButton = new DataGridViewButtonColumn
            {
                Name = "Ban",
                Text = "Ban",
                UseColumnTextForButtonValue = true
            };
            usersGrid.Columns.Add(banButton);
        }

        private async Task LoadDataAsync()
        {
            if (postsGrid == null || usersGrid == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                try
                {
                    var posts = await _adminService.GetPendingPostsAsync();
                    postsGrid.DataSource = posts;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading posts: {ex.Message}", "Posts Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                try
                {
                    var users = await _adminService.GetAllUsersAsync();
                    usersGrid.DataSource = users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading users: {ex.Message}", "Users Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void PostsGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (postsGrid == null || e.RowIndex < 0) return;

            try
            {
                var post = (Post)postsGrid.Rows[e.RowIndex].DataBoundItem;

                if (e.ColumnIndex == postsGrid.Columns["Approve"].Index)
                {
                    await _adminService.ApprovePostAsync(post.Id);
                    await LoadDataAsync();
                }
                else if (e.ColumnIndex == postsGrid.Columns["Reject"].Index)
                {
                    await _adminService.RejectPostAsync(post.Id);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing post: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UsersGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (usersGrid == null || e.RowIndex < 0) return;

            try
            {
                if (e.ColumnIndex == usersGrid.Columns["Ban"].Index)
                {
                    var user = (User)usersGrid.Rows[e.RowIndex].DataBoundItem;
                    await _adminService.BanUserAsync(user.Id);
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error banning user: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}