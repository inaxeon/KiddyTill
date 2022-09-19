using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Threading;
using System.Xml.Serialization;

namespace KiddyTill
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> _products;
        private ObservableCollection<Product> _basket;
        private DispatcherTimer _keyTimer;
        private List<char> _keyCodes;

        public MainWindow()
        {
            InitializeComponent();
            _products = new List<Product>();
            _basket = new ObservableCollection<Product>();
            _keyTimer = new DispatcherTimer();
            _keyTimer.Tick += new EventHandler(BarcodeScannerInputEentTimer);
            _keyTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            _keyCodes = new List<char>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialise();
            LstBasket.ItemsSource = _basket;
        }

        private void Initialise()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.ProductsDirectory))
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var subFolderPath = Path.Combine(path, "KiddyTill Products");
                Properties.Settings.Default.ProductsDirectory = subFolderPath;
                Properties.Settings.Default.Save();
            }

            Directory.CreateDirectory(Properties.Settings.Default.ProductsDirectory);

            LoadProducts();

        }

        private void LoadProducts()
        {
            var files = Directory.EnumerateFiles(Properties.Settings.Default.ProductsDirectory, "*.xml", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var serializer = new XmlSerializer(typeof(Product));
                using (var myFileStream = new FileStream(file, FileMode.Open))
                {
                    var product = (Product)serializer.Deserialize(myFileStream);
                    using (Stream BitmapStream = File.Open(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".png"), FileMode.Open))
                    {
                        var img = System.Drawing.Image.FromStream(BitmapStream);
                        product.Image = new Bitmap(img);
                    }

                    _products.Add(product);
                }
            }
        }

        private void SaveProducts()
        {
            foreach (var product in _products)
            {
                var writer = new XmlSerializer(typeof(Product));

                var path = Properties.Settings.Default.ProductsDirectory + "//" + product.BarCode;
                var file = File.Create(path + ".xml");

                writer.Serialize(file, product);
                file.Close();

                product.Image.Save(path + ".png", ImageFormat.Png);
            }
        }

        private void CmdCaptureProducts_Click(object sender, RoutedEventArgs e)
        {
            var captureDialog = new ProductCapture(_products);
            captureDialog.ShowDialog();
            SaveProducts();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var key = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            if (!(char.IsDigit(key) || char.IsLetter(key)))
            {
                e.Handled = false;
                return;
            }

            _keyCodes.Add(key);

            e.Handled = true;

            if (_keyTimer.IsEnabled)
                _keyTimer.Stop();

            _keyTimer.Start();
        }

        private void BarcodeScannerInputEentTimer(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();

            if (_keyCodes.Count > 1)
                AddProductToBasket(new string(_keyCodes.ToArray()));

            _keyCodes.Clear();
        }

        private void AddProductToBasket(string barcode)
        {
            var product = _products.FirstOrDefault(p => p.BarCode == barcode);

            if (product == null)
            {
                LblProductDescription.Content = "Product not found";
                LblPrice.Content = null;
                ImgProduct.Source = null;
                return;
            }

            LblProductDescription.Content = product.ProductDescription;
            LblPrice.Content = product.PriceFormatted;
            ImgProduct.Source = product.ImageObject;

            _basket.Add(product);

            LblBasketTotal.Content = _basket.Select(b => b.Price).Aggregate((a, b) => a + b).ToString("C");
        }
    }
}
