using CustomerOrderManagement.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomerOrderManagement.Views
{
    public partial class CustomerWindow : Window
    {
        private CustomerOrderDbContext _context;
        private Customer? selectedCustomer = null;

        public CustomerWindow()
        {
            InitializeComponent();
            _context = new CustomerOrderDbContext();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            customerDataGrid.ItemsSource = _context.Customers.ToList();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Tên khách hàng không được để trống.");
                return;
            }

            // Nếu người dùng nhập email, kiểm tra email đã tồn tại
            if (!string.IsNullOrWhiteSpace(email))
            {
                bool emailExists = _context.Customers.Any(c => c.Email == email);
                if (emailExists)
                {
                    MessageBox.Show("Email này đã tồn tại. Vui lòng nhập email khác.", "Lỗi trùng email", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            var customer = new Customer
            {
                Name = name,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
            LoadCustomers();
            ClearForm();
            MessageBox.Show("Đã thêm khách hàng mới.");
        }


        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để sửa.");
                return;
            }

            selectedCustomer.Name = txtName.Text.Trim();
            selectedCustomer.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            selectedCustomer.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();

            _context.SaveChanges();
            LoadCustomers();
            ClearForm();
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa.");
                return;
            }

            // Kiểm tra từ DB: khách hàng này có đơn hàng không?
            bool hasOrders = _context.Orders.Any(o => o.CustomerId == selectedCustomer.CustomerId);
            if (hasOrders)
            {
                MessageBox.Show("Không thể xóa khách hàng này vì đã có đơn hàng.",
                                "Xóa thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng \"{selectedCustomer.Name}\"?",
                "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _context.Customers.Remove(selectedCustomer);
                    _context.SaveChanges();
                    LoadCustomers();
                    ClearForm();
                    MessageBox.Show("Đã xóa khách hàng.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message);
                }
            }
        }


        private void customerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customerDataGrid.SelectedItem is Customer customer)
            {
                selectedCustomer = customer;
                txtName.Text = customer.Name;
                txtEmail.Text = customer.Email;
                txtPhone.Text = customer.Phone;
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchCustomer_Click(sender, e);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadCustomers(); // Tự động reload nếu tìm kiếm bị xoá
            }
        }

        private void SearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            var result = _context.Customers
                .Where(c =>
                    (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(keyword)) ||
                    (!string.IsNullOrEmpty(c.Phone) && c.Phone.Contains(keyword)))
                .ToList();

            customerDataGrid.ItemsSource = result;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            selectedCustomer = null;
            customerDataGrid.SelectedItem = null;
        }
    }
}
