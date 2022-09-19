using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace KiddyTill.Models
{
    public class Product
    {
        public string BarCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

        [XmlIgnore]
        public Bitmap Image { get; set; }
    }
}
