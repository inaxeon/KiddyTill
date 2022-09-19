using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace KiddyTill.Models
{
    public class Product
    {
        public string BarCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

        [XmlIgnore]
        public string PriceFormatted { get { return Price.ToString("C"); } }

        [XmlIgnore]
        public Bitmap Image { get; set; }

        [XmlIgnore]
        public BitmapImage ImageObject { get { return Convert(Image); } }

        private BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
    }
}
