using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiddyTill
{
    public class BarcodeScanner
    {
        private string _serialPort;
        private SerialPort _port;

        public delegate void BarcodeScannedCallback(string barcode);
        public event BarcodeScannedCallback BarcodeScanned;

        public BarcodeScanner(string serialPort)
        {
            _serialPort = serialPort;
        }

        public void Open()
        {
            _port = new SerialPort();

            _port.PortName = _serialPort;
            _port.BaudRate = 9600;
            _port.Handshake = Handshake.None;
            _port.DataBits = 8;
            _port.StopBits = StopBits.One;
            _port.ReadTimeout = 1000;
            _port.DataReceived += Port_DataReceived;

            _port.Open();
        }

        public void Close()
        {
            _port.Close();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var code = ReadBarcode();
            BarcodeScanned?.Invoke(code);
        }

        private string ReadBarcode()
        {
            byte[] buffer = new byte[256];
            bool reading = true;
            int total = 0;
            while (true)
            {
                try
                {
                    int read = _port.Read(buffer, total, 256);
                    total += read;

                    if (buffer[total - 1] == '\n' || buffer[total - 1] == '\r')
                        break;
                }
                catch (IOException)
                {
                    return null;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
            }

            return Encoding.ASCII.GetString(buffer.Take(total - 2).ToArray());
        }
    }
}
