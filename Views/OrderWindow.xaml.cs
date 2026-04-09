using CustomerOrderManagement.Models;
using CustomerOrderManagement.Factories;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CustomerOrderManagement.Views
{
    public partial class OrderWindow : Window
    {
        private CustomerOrderDbContext _context;

        public OrderWindow()
        {
            InitializeComponent();
            _context = new CustomerOrderDbContext();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            cbCustomers.ItemsSource = _context.Customers.ToList();
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng.");
                return;
            }

            if (!decimal.TryParse(txtTotal.Text, out decimal total) || total < 0)
            {
                MessageBox.Show("Tổng tiền không hợp lệ.");
                return;
            }

            if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0)
            {
                MessageBox.Show("Giảm giá không hợp lệ.");
                return;
            }

            string orderType = ((ComboBoxItem)cbOrderType.SelectedItem)?.Content?.ToString() ?? "Online";
            int customerId = (int)cbCustomers.SelectedValue;

            OrderFactory factory;

            switch (orderType)
            {
                case "In-Store":
                    factory = new InStoreOrderFactory();
                    break;
                case "Wholesale":
                    factory = new WholesaleOrderFactory();
                    break;
                default:
                    factory = new OnlineOrderFactory();
                    break;
            }

            Order newOrder = factory.CreateOrder(total, discount, customerId);

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            MessageBox.Show("Đơn hàng đã được tạo thành công.");
            ClearForm();
        }

        private void ClearForm()
        {
            txtTotal.Clear();
            txtDiscount.Clear();
            cbOrderType.SelectedIndex = 0;
        }
    }
}
