using System.Windows;
using CustomerOrderManagement.Views;

namespace CustomerOrderManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ManageCustomers_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().ShowDialog();
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow().ShowDialog();
        }

        private void ViewOrders_Click(object sender, RoutedEventArgs e)
        {
            new OrderListWindow().ShowDialog();
        }

        private void ViewStatistics_Click(object sender, RoutedEventArgs e)
        {
            new OrderStatisticsWindow().ShowDialog();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Đóng MainWindow và quay lại LoginWindow
            new LoginWindow().Show();
            this.Close();
        }
    }
}
