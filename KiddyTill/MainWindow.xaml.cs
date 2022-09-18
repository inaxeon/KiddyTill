using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KiddyTill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LstBasket.ItemsSource = new List<BasketItem> { new BasketItem { ProductDescription = "Product 1" } };
        }

        private void CmdCaptureProducts_Click(object sender, RoutedEventArgs e)
        {
            var captureDialog = new ProductCapture();
            captureDialog.ShowDialog();
        }
    }
}
