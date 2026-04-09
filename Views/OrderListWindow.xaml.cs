using CustomerOrderManagement.Models;
using System.Linq;
using System.Windows;

namespace CustomerOrderManagement.Views
{
    public partial class OrderListWindow : Window
    {
        private CustomerOrderDbContext _context;

        public OrderListWindow()
        {
            InitializeComponent();
            _context = new CustomerOrderDbContext();
            LoadOrders();
        }

        private void LoadOrders()
        {
            var orders = _context.Orders.ToList();
            foreach (var o in orders)
            {
                o.Customer = _context.Customers.FirstOrDefault(c => c.CustomerId == o.CustomerId);
            }
            orderDataGrid.ItemsSource = orders;
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataGrid.SelectedItem is not Order selectedOrder)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để sửa.");
                return;
            }

            var editWindow = new EditOrderWindow(selectedOrder.OrderId);
            editWindow.ShowDialog();
            LoadOrders();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataGrid.SelectedItem is not Order selectedOrder)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để xoá.");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xoá đơn hàng này?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                _context.Orders.Remove(selectedOrder);
                _context.SaveChanges();
                LoadOrders();
            }
        }
    }
}
