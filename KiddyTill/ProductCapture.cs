using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Touchless.Vision.Camera;

namespace KiddyTill
{
    public partial class ProductCapture : Form
    {
        private BarcodeScanner _scanner;
        private CameraFrameSource _frameSource;
        private static Bitmap _latestFrame;
        private List<Product> _products;

        private Camera CurrentCamera
        {
            get
            {
                return cmbCameras.SelectedItem as Camera;
            }
        }

        public ProductCapture(BarcodeScanner scanner, List<Product> products)
        {
            InitializeComponent();
            _products = products;
            _scanner = scanner;
        }

        private void ProductCapture_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Refresh the list of available cameras
                cmbCameras.Items.Clear();
                foreach (Camera cam in CameraService.AvailableCameras)
                    cmbCameras.Items.Add(cam);

                if (cmbCameras.Items.Count > 0)
                    cmbCameras.SelectedIndex = 0;
            }

            chkMirrorImage.Checked = Properties.Settings.Default.FlipCameraPreview;

            KeyPreview = true;
            txtProductDescription.Focus();
            StartCamera();

            if (_scanner != null)
                _scanner.BarcodeScanned += Scanner_BarcodeScanned;
        }

        private void Scanner_BarcodeScanned(string barcode)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => { SaveProduct(barcode); }));
            }
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            if (_frameSource == null)
            {
                StartCamera();
            }
            else
            {
                DisposeCamera();
            }
        }

        private void StartCamera()
        {
            // Early return if we've selected the current camera
            if (_frameSource != null && _frameSource.Camera == cmbCameras.SelectedItem)
                return;

            DisposeCamera();
            _latestFrame = null;
            StartCapturing();
            btnStartCamera.Text = "Stop Camera";
        }

        private void StartCapturing()
        {
            try
            {
                Camera c = (Camera)cmbCameras.SelectedItem;
                SetFrameSource(new CameraFrameSource(c));
                _frameSource.Camera.CaptureWidth = 640;
                _frameSource.Camera.CaptureHeight = 480;
                _frameSource.Camera.Fps = 50;
                _frameSource.NewFrame += OnImageCaptured;

                pctDisplay.Paint += new PaintEventHandler(DrawLatestImage);
                _frameSource.StartFrameCapture();
            }
            catch (Exception ex)
            {
                cmbCameras.Text = "Select A Camera";
                MessageBox.Show(ex.Message);
            }
        }

        private void DrawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                var thisFrame = new Bitmap(_latestFrame);

                if (chkMirrorImage.Checked)
                    thisFrame.RotateFlip(RotateFlipType.Rotate180FlipY);

                e.Graphics.DrawImage(thisFrame, 0, 0, thisFrame.Width, thisFrame.Height);
            }
        }

        public void OnImageCaptured(Touchless.Vision.Contracts.IFrameSource frameSource, Touchless.Vision.Contracts.Frame frame, double fps)
        {
            _latestFrame = frame.Image;
            pctDisplay.Invalidate();
        }

        private void SetFrameSource(CameraFrameSource cameraFrameSource)
        {
            if (_frameSource == cameraFrameSource)
                return;

            _frameSource = cameraFrameSource;
        }

        private void DisposeCamera()
        {
            // Trash the old camera
            if (_frameSource != null)
            {
                _frameSource.NewFrame -= OnImageCaptured;
                _frameSource.Camera.Dispose();
                SetFrameSource(null);
                pctDisplay.Paint -= new PaintEventHandler(DrawLatestImage);
            }

            if (_latestFrame != null)
                pctDisplay.Image = _latestFrame;

            btnStartCamera.Text = "Start Camera";
        }

        private void SaveProduct(string barCode)
        {
            decimal price;

            barCode = barCode.ToUpperInvariant();

            if (string.IsNullOrEmpty(txtProductDescription.Text))
            {
                MessageBox.Show("No product description entered. Product not saved.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBox.Show("No product price entered. Product not saved.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Invalid price. Product not saved.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var newProduct = new Product
            {
                Price = price,
                Description = txtProductDescription.Text,
                BarCode = barCode,
                AddedOrModified = true,
            };

            newProduct.SetBitmap(_latestFrame);

            _products.RemoveAll(p => p.BarCode == barCode);
            _products.Add(newProduct);
            pctDisplay.Image = null;
            txtProductDescription.Text = "";
            txtPrice.Text = "";
            txtBarCode.Text = "";
            txtProductDescription.Focus();
            StartCamera();
        }

        private void btnSaveProduct_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"[a-zA-Z0-9]");

            if (!regex.IsMatch(txtBarCode.Text))
            {
                MessageBox.Show("Invalid barcode. Product not saved.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveProduct(txtBarCode.Text);
        }

        private void ProductCapture_FormClosed(object sender, FormClosedEventArgs e)
        {
            DisposeCamera();

            if (DialogResult == DialogResult.OK)
            {
                Properties.Settings.Default.FlipCameraPreview = chkMirrorImage.Checked;
                Properties.Settings.Default.Save();
            }
        }

        private void txtBarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                SaveProduct(txtBarCode.Text);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.PageDown || keyData == Keys.PageUp)
            {
                if (_frameSource == null)
                    StartCamera();
                else
                    DisposeCamera();

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
