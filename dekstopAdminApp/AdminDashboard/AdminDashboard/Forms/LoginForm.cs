using System.Runtime.Versioning;
using AdminDashboard.Services;

namespace AdminDashboard.Forms
{
    [SupportedOSPlatform("windows")]
    public partial class LoginForm : Form
    {
        private readonly AuthenticationService _authService;

        private TextBox? txtEmail;
        private TextBox? txtPassword;
        private Button? btnLogin;
        private Label? lblEmail;
        private Label? lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
            InitializeLoginControls();
        }

        private void InitializeLoginControls()
        {
            txtEmail = new TextBox
            {
                Location = new Point(20, 40),
                Size = new Size(240, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(20, 90),
                Size = new Size(240, 20),
                PasswordChar = '*'
            };

            btnLogin = new Button
            {
                Location = new Point(20, 120),
                Size = new Size(240, 30),
                Text = "Login"
            };

            lblEmail = new Label
            {
                Location = new Point(20, 20),
                AutoSize = true,
                Text = "Email:"
            };

            lblPassword = new Label
            {
                Location = new Point(20, 70),
                AutoSize = true,
                Text = "Password:"
            };

            Text = "Admin Login";
            Size = new Size(300, 200);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            if (btnLogin != null)
                btnLogin.Click += btnLogin_Click;

            Controls.AddRange(new Control[] {
                lblEmail!,
                txtEmail,
                lblPassword!,
                txtPassword,
                btnLogin
            });
        }

        private async void btnLogin_Click(object? sender, EventArgs e)
        {
            if (txtEmail == null || txtPassword == null || btnLogin == null) return;

            try
            {
                btnLogin.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var response = await _authService.LoginAsync(txtEmail.Text, txtPassword.Text);

                if (!string.IsNullOrEmpty(response?.Token))
                {
                    Console.WriteLine($"Token received: {response.Token}");
                    Properties.Settings.Default.AuthToken = response.Token;
                    Properties.Settings.Default.Save();

                    var dashboardForm = new DashboardForm();
                    Hide();
                    dashboardForm.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("No token received from server", "Authentication Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}