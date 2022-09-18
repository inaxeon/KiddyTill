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
        private Camera CurrentCamera
        {
            get
            {
                return cmbCameras.SelectedItem as Camera;
            }
        }

        public ProductCapture()
        {
            InitializeComponent();
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
        }

        private void btnStartCamera_Click(object sender, EventArgs e)
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
        }

        private void btnStopCamera_Click(object sender, EventArgs e)
        {
            DisposeCamera();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
