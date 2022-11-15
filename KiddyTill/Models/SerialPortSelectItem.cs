using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyTill.Models
{
    public class SerialPortSelectItem
    {
        private string _desc;
        private string _port;

        public SerialPortSelectItem(string desc, string port)
        {
            _desc = desc;
            _port = port;
        }

        public override string ToString()
        {
            return _desc;
        }

        public string Port { get { return _port; } }
    }
}
