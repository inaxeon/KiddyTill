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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Image")]
        public byte[] ImageSerialized
        {
            get
            {
                if (Image == null)
                    return null;

                using (MemoryStream ms = new MemoryStream())
                {
                    Image.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            set
            {
                if (value == null)
                {
                    Image = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        Image = new Bitmap(ms);
                    }
                }
            }
        }
    }
}
