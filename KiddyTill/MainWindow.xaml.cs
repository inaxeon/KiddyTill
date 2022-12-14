using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private BasketItem _noProduct;
        private ObservableCollection<BasketItem> _basket;
        private BarcodeScanner _scanner;
        private decimal _total;

        public MainWindow()
        {
            InitializeComponent();
            _products = new List<Product>();
            _basket = new ObservableCollection<BasketItem>();
            _noProduct = new BasketItem
            {
                Product = new Product
                {
                    Description = "Product not found",
                    Image = new BitmapImage(new Uri(GetResourcePath("NoProduct.png")))
                }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialise();
            LstBasket.ItemsSource = _basket;

            ((INotifyCollectionChanged)LstBasket.ItemsSource).CollectionChanged += (s, evt) =>
            {
                if (evt.Action == NotifyCollectionChangedAction.Add)
                {
                    LstBasket.ScrollIntoView(LstBasket.Items[LstBasket.Items.Count - 1]);
                }
            };
        }

        private void CmdOptions_Click(object sender, RoutedEventArgs e)
        {
            var optionsDialog = new Options();
            if (optionsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _products.Clear();
                LoadProducts();
                OpenScanner();
            }
        }

        private void CmdCaptureProducts_Click(object sender, RoutedEventArgs e)
        {
            var captureDialog = new ProductCapture(_scanner, _products);
            captureDialog.ShowDialog();
            SaveProducts();
        }

        private void CmdExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnNewSale_Click(object sender, RoutedEventArgs e)
        {
            NewSale();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageDown || e.Key == Key.PageUp)
                NewSale();
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

            OpenScanner();
            LoadProducts();
            UpdateDisplay();
        }

        private void OpenScanner()
        {
            CloseScanner();

            if (!string.IsNullOrEmpty(Properties.Settings.Default.SerialPort))
            {
                try
                {
                    _scanner = new BarcodeScanner(Properties.Settings.Default.SerialPort);
                    _scanner.Open();
                    _scanner.BarcodeScanned += Scanner_BarcodeScanned;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening barcode scanner: " + ex.Message, "Error opening scanner", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CloseScanner()
        {
            if (_scanner != null && _scanner.IsOpen)
            {
                _scanner.Close();
                _scanner.BarcodeScanned -= Scanner_BarcodeScanned;
                _scanner = null;
            }
        }

        private void Scanner_BarcodeScanned(string barcode)
        {
            Dispatcher.BeginInvoke((Action)(() => { AddProductToBasket(barcode); }));
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
                    var imageFileName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".jpg");

                    product.Image = LoadImageFromFile(imageFileName);

                    _products.Add(product);
                }
            }
        }

        private BitmapImage LoadImageFromFile(string fileName)
        {
            // What was once simply is apparently now rather difficult.
            // the "new BitmapImage(new Uri(...))" appearoch works with less code but
            // leaves the file handle open which is not very helpful.

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void SaveProducts()
        {
            foreach (var product in _products)
            {
                if (!product.AddedOrModified)
                    continue;

                var writer = new XmlSerializer(typeof(Product));
                var path = Properties.Settings.Default.ProductsDirectory + "//" + product.BarCode;
                var file = File.Create(path + ".xml");

                writer.Serialize(file, product);
                file.Close();

                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(product.Image));

                using (var fileStream = new System.IO.FileStream(path + ".jpg", FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                product.AddedOrModified = false; // Don't serialise again
            }
        }

        private void AddProductToBasket(string barcode)
        {
            var product = _products.FirstOrDefault(p => p.BarCode == barcode);

            if (product == null)
            {
                SetProduct(_noProduct);
                return;
            }

            var basketItem = new BasketItem { Product = product };
            _basket.Add(basketItem);
            _total = _basket.Select(b => b.Product.Price).Aggregate((a, b) => a + b);
            SetProduct(basketItem);
            UpdateDisplay();
        }

        public string GetResourcePath(string fileName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.Combine(Path.GetDirectoryName(path), "Resources", fileName);
        }
        
        private void SetProduct(BasketItem product)
        {
            if (product == null)
            {
                ImgProduct.Source = null;
                LblProductDescription.Content = null;
                LblPrice.Content = null;
                return;
            }

            LblProductDescription.Content = product.Product.Description;
            LblPrice.Content = product.PriceFormatted;
            ImgProduct.Source = product.Product.Image;
        }

        private void UpdateDisplay()
        {
            LblBasketTotal.Content = _total.ToString("C");
        }

        private void NewSale()
        {
            _basket.Clear();
            SetProduct(null);
            _total = 0;
            UpdateDisplay();
        }
    }
}
