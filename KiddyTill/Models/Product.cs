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
        public string Description { get; set; }
        public decimal Price { get; set; }

        [XmlIgnore]
        public bool AddedOrModified { get; set; }

        [XmlIgnore]
        public BitmapImage Image { get; set; }

        public void SetBitmap(Bitmap src)
        {
            var ms = new MemoryStream();
            src.Save(ms, ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            Image = image;
        }
    }
}
