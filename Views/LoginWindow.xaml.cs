using System.Windows;
using CustomerOrderManagement.Helpers;

namespace CustomerOrderManagement.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string usernameInput = txtUsername.Text.Trim();
            string passwordInput = txtPassword.Password.Trim();

            string configuredUsername = AppConfig.GetUsername();
            string configuredPassword = AppConfig.GetPassword();

            if (usernameInput == configuredUsername && passwordInput == configuredPassword)
            {
                MainWindow main = new();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
