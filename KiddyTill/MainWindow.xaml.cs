using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        private List<Product> _products;

        public MainWindow()
        {
            InitializeComponent();
            _products = new List<Product>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LstBasket.ItemsSource = new List<Product> { new Product { ProductDescription = "Product 1" } };
            Initialise();
        }

        private void Initialise()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ProductsDirectory))
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var subFolderPath = System.IO.Path.Combine(path, "KiddyTill Products");
                Properties.Settings.Default.ProductsDirectory = subFolderPath;
                Properties.Settings.Default.Save();
            }

            System.IO.Directory.CreateDirectory(Properties.Settings.Default.ProductsDirectory);

            LoadProducts();
        }

        private void LoadProducts()
        {
            var files = Directory.EnumerateFiles(Properties.Settings.Default.ProductsDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                        
            //foreach (var f in files)
            //{
            //    Console.WriteLine($"{f.File}\t{f.Line}");
            //}
        }

        private void SaveProducts()
        {
            foreach (var product in _products)
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Product));

                var path = Properties.Settings.Default.ProductsDirectory + "//" + product.BarCode + ".xml";
                System.IO.FileStream file = System.IO.File.Create(path);

                writer.Serialize(file, product);
                file.Close();
            }
        }

        private void CmdCaptureProducts_Click(object sender, RoutedEventArgs e)
        {
            var captureDialog = new ProductCapture(_products);
            captureDialog.ShowDialog();
            SaveProducts();
        }
    }
}
