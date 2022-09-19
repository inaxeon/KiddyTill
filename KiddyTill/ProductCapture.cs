﻿using KiddyTill.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Touchless.Vision.Camera;

namespace KiddyTill
{
    public partial class ProductCapture : Form
    {
        private CameraFrameSource _frameSource;
        private static Bitmap _latestFrame;
        private List<char> _keyCodes;
        private Timer _keyTimer;
        private List<Product> _products;

        private Camera CurrentCamera
        {
            get
            {
                return cmbCameras.SelectedItem as Camera;
            }
        }

        public ProductCapture(List<Product> products)
        {
            InitializeComponent();
            _keyCodes = new List<char>();
            _keyTimer = new Timer();
            _keyTimer.Interval = 20;
            _keyTimer.Tick += new EventHandler(TimerEventProcessor);
            _products = products;
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

            KeyPreview = true;
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
        {
            StartCamera();
        }

        private void StartCamera()
        {
            // Early return if we've selected the current camera
            if (_frameSource != null && _frameSource.Camera == cmbCameras.SelectedItem)
                return;

            DisposeCamera();
            StartCapturing();
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
                // Draw the latest image from the active camera
                e.Graphics.DrawImage(_latestFrame, 0, 0, _latestFrame.Width, _latestFrame.Height);
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
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            DisposeCamera();
        }

        private void ProductCapture_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '\r')
            {
                e.Handled = false;
                return;
            }

            _keyCodes.Add(e.KeyChar);

            e.Handled = true;

            if (_keyTimer.Enabled)
                _keyTimer.Stop();

            _keyTimer.Start();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                if (_keyTimer.Enabled)
                    return true; // Reading from barcode scanner
            }
            if (keyData == Keys.Space)
            {
                if (_frameSource == null)
                    StartCamera();
                else
                    DisposeCamera();

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        void TimerEventProcessor(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();

            if (_keyCodes.Count == 1)
            {
                KeyPreview = false;
                SendKeys.Send(_keyCodes[0].ToString());
            }
            else
            {
                SaveProduct(new string(_keyCodes.ToArray()));
            }

            _keyCodes.Clear();

            Timer timer = new Timer();
            timer.Tick += new EventHandler((object o, EventArgs s) =>
            {
                ((Timer)o).Stop();
                KeyPreview = true;
            });
            timer.Interval = 5;
            timer.Start();
        }

        private void SaveProduct(string barCode)
        {
            decimal price;
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
                ProductDescription = txtProductDescription.Text,
                Image = (Bitmap)pctDisplay.Image,
                BarCode = barCode
            };

            _products.RemoveAll(p => p.BarCode == barCode);

            _products.Add(newProduct);

            pctDisplay.Image = null;
            txtProductDescription.Text = "";
            txtPrice.Text = "";
            txtProductDescription.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}