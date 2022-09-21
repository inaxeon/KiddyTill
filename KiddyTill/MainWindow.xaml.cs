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
        private Product _noProduct;
        private ObservableCollection<BasketItem> _basket;
        private DispatcherTimer _keyTimer;
        private List<char> _keyCodes;
        private decimal _total;

        public MainWindow()
        {
            InitializeComponent();
            _products = new List<Product>();
            _basket = new ObservableCollection<BasketItem>();
            _keyTimer = new DispatcherTimer();
            _keyTimer.Tick += new EventHandler(BarcodeScannerInputEentTimer);
            _keyTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            _keyCodes = new List<char>();
            _noProduct = new Product
            {
                ProductDescription = "Product not found",
                Image = new Bitmap(GetResourcePath("NoProduct.png"))
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

        private void CmdOptions_Click(object sender, RoutedEventArgs e)
        {
            var optionsDialog = new Options();
            if (optionsDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _products.Clear();
                LoadProducts();
            }
        }

        private void CmdCaptureProducts_Click(object sender, RoutedEventArgs e)
        {
            var captureDialog = new ProductCapture(_products);
            captureDialog.ShowDialog();
            SaveProducts();
        }

        private void BtnNewSale_Click(object sender, RoutedEventArgs e)
        {
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

            LoadProducts();
            UpdateDisplay();
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
                    using (Stream BitmapStream = File.Open(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".jpg"), FileMode.Open))
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
                if (!product.AddedOrModified)
                    continue;

                var writer = new XmlSerializer(typeof(Product));
                var path = Properties.Settings.Default.ProductsDirectory + "//" + product.BarCode;
                var file = File.Create(path + ".xml");

                writer.Serialize(file, product);
                file.Close();

                product.Image.Save(path + ".jpg", ImageFormat.Jpeg);
                product.AddedOrModified = false; // Don't serialise again
            }
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
                SetProduct(_noProduct);
                return;
            }

            _basket.Add(new BasketItem { Product = product });
            _total = _basket.Select(b => b.Product.Price).Aggregate((a, b) => a + b);
            SetProduct(product);
            UpdateDisplay();
        }

        public string GetResourcePath(string fileName)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.Combine(Path.GetDirectoryName(path), "Resources", fileName);
        }



        private void SetProduct(Product product)
        {
            if (product == null)
            {
                ImgProduct.Source = null;
                LblProductDescription.Content = null;
                LblPrice.Content = null;
                return;
            }

            LblProductDescription.Content = product.ProductDescription;
            LblPrice.Content = product.PriceFormatted;
            ImgProduct.Source = product.WpfBitmap;
        }

        private void UpdateDisplay()
        {
            LblBasketTotal.Content = _total.ToString("C");
        }

        private void NewSale()
        {
            _basket.Clear();
            SetProduct(null);
            UpdateDisplay();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageDown || e.Key == Key.PageUp)
                NewSale();
        }
    }
}
