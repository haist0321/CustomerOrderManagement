using CustomerOrderManagement.Models;
using System.Linq;
using System.Windows;
using ClosedXML.Excel;
using Microsoft.Win32;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace CustomerOrderManagement.Views
{
    public partial class OrderStatisticsWindow : Window
    {
        private readonly CustomerOrderDbContext _context;

        public ISeries[] PieSeries { get; set; } = System.Array.Empty<ISeries>();

        public OrderStatisticsWindow()
        {
            InitializeComponent();
            _context = new CustomerOrderDbContext();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var orderTypeStats = _context.Orders
                .GroupBy(o => o.OrderType)
                .Select(g => new
                {
                    Type = g.Key,
                    Total = (double)g.Sum(o => o.FinalAmount)
                })
                .ToList();

            PieSeries = orderTypeStats
                .Select(x => new PieSeries<double>
                {
                    Name = x.Type,
                    Values = new[] { x.Total }
                })
                .ToArray();

            DataContext = this;

            var customerStats = _context.Orders
                .GroupBy(o => o.Customer.Name)
                .Select(g => new
                {
                    KhachHang = g.Key,
                    SoLuong = g.Count(),
                    TongTien = g.Sum(o => o.FinalAmount)
                })
                .ToList();

            customerStatsGrid.ItemsSource = customerStats;
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (customerStatsGrid.ItemsSource == null)
            {
                MessageBox.Show("Không có dữ liệu để xuất.");
                return;
            }

            var stats = customerStatsGrid.ItemsSource.Cast<object>().ToList();

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "ThongKeDonHang.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("ThongKeTheoKhach");

                ws.Cell(1, 1).Value = "Khách hàng";
                ws.Cell(1, 2).Value = "Số lượng đơn";
                ws.Cell(1, 3).Value = "Tổng tiền";

                int row = 2;
                foreach (dynamic item in stats)
                {
                    ws.Cell(row, 1).Value = item.KhachHang;
                    ws.Cell(row, 2).Value = item.SoLuong;
                    ws.Cell(row, 3).Value = item.TongTien;
                    row++;
                }

                workbook.SaveAs(dialog.FileName);
                MessageBox.Show("Đã xuất thành công!");
            }
        }
    }
}