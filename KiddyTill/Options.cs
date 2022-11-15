using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiddyTill
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            txtProductsLocation.Text = Properties.Settings.Default.ProductsDirectory;
            var ports = GetSerialPorts();
            ddlBarcodeScanner.Items.AddRange(ports.ToArray());
            SelectComPort();
        }

        private void btnChoseLocation_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtProductsLocation.Text = fbd.SelectedPath;
                }
            }
        }

        private void Options_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                Properties.Settings.Default.ProductsDirectory = txtProductsLocation.Text;
                Properties.Settings.Default.SerialPort = ddlBarcodeScanner.SelectedItem != null ?
                    ((SerialPortSelectItem)ddlBarcodeScanner.SelectedItem).Port : null;
                Properties.Settings.Default.Save();
            }
        }

        private void SelectComPort()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.SerialPort))
            {
                if (ddlBarcodeScanner.Items.Count == 0)
                    return;

                ddlBarcodeScanner.SelectedIndex = 0;

                Properties.Settings.Default.SerialPort = ((SerialPortSelectItem)ddlBarcodeScanner.SelectedItem).Port;
                Properties.Settings.Default.Save();
            }
            else
            {
                foreach (SerialPortSelectItem item in ddlBarcodeScanner.Items)
                {
                    if (item.Port == Properties.Settings.Default.SerialPort)
                    {
                        ddlBarcodeScanner.SelectedIndex = ddlBarcodeScanner.Items.IndexOf(item);
                        return;
                    }
                }

                if (ddlBarcodeScanner.Items.Count > 0)
                    ddlBarcodeScanner.SelectedIndex = 0;
            }
        }

        private static List<SerialPortSelectItem> GetSerialPorts()
        {
            var names = SerialPort.GetPortNames().ToList();

            names = names.Select(el => el.Length == 4 ? el.Replace("COM", "COM0") : el).ToList();
            names.Sort();
            names = names.Select(el => el.Replace("COM0", "COM")).ToList();

            var descs = new List<string>();
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PnPEntity");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null)
                    {
                        if (queryObj["Caption"].ToString().Contains("(COM"))
                            descs.Add((string)queryObj["Caption"]);
                    }
                }
            }
            catch (ManagementException e)
            {

            }

            var ports = new List<SerialPortSelectItem>();
            foreach (var name in names)
            {
                string bracketed = string.Format(" ({0})", name);
                var desc = descs.Find(el => el.Contains(bracketed));
                if (desc != null)
                {
                    desc = name + " - " + desc.Replace(bracketed, "");
                    ports.Add(new SerialPortSelectItem(desc, name));
                }
                else
                {
                    ports.Add(new SerialPortSelectItem(name, name));
                }
            }

            return ports;
        }

    }
}
