using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class CameraSettingWindow : Form
    {
        public CameraSettingWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 视频输入设备信息
        /// </summary>
        public FilterInfoCollection filterInfoCollection;

        public string cameraName = string.Empty;

        public int maxFps = 0;

        public int Fps = 0;

        public int _width = 1280;
        public int _height = 720;
        private void CameraSettingWindow_Load(object sender, EventArgs e)
        {
            this.comboBox2.SelectedIndex = 0;
            ReloadCameraList();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ReloadCameraList()
        {
            comboBox_camera.Items.Clear();
            //设置视频来源
            try
            {
                // 枚举所有视频输入设备
                filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (filterInfoCollection.Count == 0)
                    throw new ApplicationException();   //没有找到摄像头设备
                int index = -1;
                int step = 0;
                foreach (FilterInfo device in filterInfoCollection)
                {
                    if (device.Name.Contains("Web"))
                    {
                        continue;
                    }
                    if (device.Name.Contains("YSYS"))
                    {
                        index = step;
                    }
                    comboBox_camera.Items.Add(device.Name);
                    step++;
                }
                if (comboBox_camera.Items.Count > 0)
                {
                    if (index == -1)
                    {
                        comboBox_camera.SelectedIndex = comboBox_camera.Items.Count - 1;
                    }
                    else
                    {
                        comboBox_camera.SelectedIndex = index;
                        base.DialogResult = DialogResult.OK;
                    }

                }
                else
                {
                    MessageBox.Show("未找到摄像头");
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (ApplicationException ex)
            {
                filterInfoCollection = null;
                comboBox_camera.Items.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        List<string> FpsList = new List<string>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void ChooseCamera(string name)
        {
            FpsList.Clear();
            foreach (FilterInfo device in filterInfoCollection)
            {
                if (device.Name == name)
                {
                    VideoCaptureDevice rgbDeviceVideo = new VideoCaptureDevice(device.MonikerString);
                    for (int i = 0; i < rgbDeviceVideo.VideoCapabilities.Length; i++)
                    {
                        if (rgbDeviceVideo.VideoCapabilities[i].FrameSize.Width == _width
                            && rgbDeviceVideo.VideoCapabilities[i].FrameSize.Height == _height)
                        {
                            //rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[i];
                            string fps = rgbDeviceVideo.VideoCapabilities[i].AverageFrameRate + "";
                            if (!FpsList.Contains(fps))
                                FpsList.Add(fps);
                            break;
                        }
                    }
                    break;
                }
            }
            if (FpsList.Count == 0)
            {
                foreach (FilterInfo device in filterInfoCollection)
                {
                    if (device.Name == name)
                    {
                        VideoCaptureDevice rgbDeviceVideo = new VideoCaptureDevice(device.MonikerString);
                        for (int i = 0; i < rgbDeviceVideo.VideoCapabilities.Length; i++)
                        {
                            if (rgbDeviceVideo.VideoCapabilities[i].FrameSize.Width == 1920
                                && rgbDeviceVideo.VideoCapabilities[i].FrameSize.Height == 1080)
                            {
                                //rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[i];
                                string fps = rgbDeviceVideo.VideoCapabilities[i].AverageFrameRate + "";
                                if (!FpsList.Contains(fps))
                                    FpsList.Add(fps);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            comboBox1.Items.Clear();
            foreach (var item in FpsList)
            {
                int.TryParse(item, out int fps);
                maxFps = fps;
                fps /= 2;
                while (fps >= 30)
                {
                    if (fps >= 30)
                        comboBox1.Items.Add(fps + "fps");
                    fps /= 2;
                }
                comboBox1.Items.Add(maxFps + "fps");
                break;
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }

        private void comboBox_camera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox_camera.Text))
            {
                cameraName = comboBox_camera.Text;
                ChooseCamera(cameraName);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                string cb1 = comboBox1.Text;
                string cb2 = cb1.Substring(0, cb1.IndexOf("fps"));
                int.TryParse(cb2, out Fps);
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
