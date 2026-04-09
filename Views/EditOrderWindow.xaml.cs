using CustomerOrderManagement.Models;
using System;
using System.Linq;
using System.Windows;

namespace CustomerOrderManagement.Views
{
    public partial class EditOrderWindow : Window
    {
        private CustomerOrderDbContext _context;
        private Order _order;

        public EditOrderWindow(int orderId)
        {
            InitializeComponent();
            _context = new CustomerOrderDbContext();
            _order = _context.Orders.First(o => o.OrderId == orderId);
            LoadData();
        }

        private void LoadData()
        {
            txtTotal.Text = _order.TotalAmount.ToString();
            txtDiscount.Text = _order.Discount.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtTotal.Text, out decimal total) || total < 0 ||
                !decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0)
            {
                MessageBox.Show("Vui lòng nhập đúng số hợp lệ.");
                return;
            }

            _order.TotalAmount = total;
            _order.Discount = discount;
            _order.FinalAmount = total - discount;

            _context.SaveChanges();
            MessageBox.Show("Cập nhật thành công.");
            this.Close();
        }
    }
}
