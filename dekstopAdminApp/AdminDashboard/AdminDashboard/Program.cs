using System.Runtime.Versioning;
using AdminDashboard.Forms;

namespace AdminDashboard
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            if (OperatingSystem.IsWindows())
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new LoginForm());
            }
        }
    }
}