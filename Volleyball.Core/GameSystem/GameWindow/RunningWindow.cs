using AForge.Video.DirectShow;
using HZH_Controls;
using HZH_Controls.Forms;
using MiniExcelLibs;
using Newtonsoft.Json;
using SpeechLib;
using Sunny.UI;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class RunningWindow : Form
    {
        internal string _groupName;

        public RunningWindow()
        {
            InitializeComponent();
            AutoSizeWindow = new AutoSizeWindow(this.Width, this.Height);
            AutoSizeWindow.SetTag(this);
            BarCode.BarCodeEvent += new BardCodeHook.BardCodeDeletegate(BarCode_BarCodeEvent);
        }

        private BardCodeHook BarCode = new BardCodeHook();
        private IFreeSql fsql = DbFreesql.Sqlite;
        public SportProjectInfos sportProjectInfos { get; set; }
        private Dictionary<string, string> localInfos = new Dictionary<string, string>();
        public NFC_Helper USBWatcher = new NFC_Helper();
        public int FullMark = 188;//满分成绩
        private int _studentNums = 20;//匹配学生数量
        private string _portNameType = "CH340";//串口类型
        private string _nBaudrate = "115200";//波特率
        private string studentNumsLog = Application.StartupPath + "\\Data\\studentNums.log";
        private string portNameTypeLog = Application.StartupPath + "\\Data\\portNameType.log";
        private string nBaudrateLog = Application.StartupPath + "\\Data\\nBaudrate.log";
        private int _nowRound = 0;//点前轮次
        public static string strMainModule = System.AppDomain.CurrentDomain.BaseDirectory + "data\\";

        //记录球是否落下
        private bool isRightScore = true;

        //记录下落间隔范围
        private int ballFallingInterval = 500;

        private int ballFallingIntervalStep = 0;

        private int threshold = 2000;

        public static string basePath = System.AppDomain.CurrentDomain.BaseDirectory + "data\\";

        //测试模式 0:自动下一位 1:自动下一轮
        public int _TestMethod = 0;

        private List<RaceStudentData> ListViewStudentsData = new List<RaceStudentData>();

        /// <summary>
        ///
        /// </summary>
        private ScreenSerialReader sReader = null;

        private int pBox1Width = 0;
        private int pBox1Height = 0;

        private OpenCvSharp.VideoWriter VideoOutPut;
        private object bmpObj = new object();
        private Bitmap bmpSoure = new Bitmap(1, 1);
        private int _width;
        private int _height;

        /// <summary>
        /// 视频输入设备信息
        /// </summary>
        private FilterInfoCollection filterInfoCollection;

        /// <summary>
        /// RGB摄像头设备
        /// </summary>
        private VideoCaptureDevice rgbDeviceVideo;

        //private FuseImg fuseImg = null;
        private int skipFrameDispR0 = 0;

        private int cameraSkip = 0;

        private bool rgbVideoSourcePaintFlag = false;

        private int GCCounta = 0;
        private int videoSourceRuningR0 = 0;
        private int frameRecSum = 0;//计算帧数

        private string nowFileName = "";//当前文件名
        private string nowTestDir = String.Empty;//当前文件目录
        private int recTimeR0 = 0;//计时时间
        private int frameSum = 0;

        private FuseBitmap fb = null;
        private int tolerance = 60;
        private bool updateBackImgFlag = false;//更新底图
        private Point mousePoint = new Point(); //鼠标移动点
        private int drawFlag = 0;

        private Point leftTopPoint = new Point();//标定区域点
        private Point rightBottomPoint = new Point();
        private Rectangle rectFlag = new Rectangle();
        private int ScoreSum = 0;

        private string cameraName = String.Empty;
        private int maxFps = 0;
        private int Fps = 0;

        private float offsetX = 0;
        private float offsetY = 0;

        private string AutoSaveLog = Application.StartupPath + "\\Data\\AutoSaveLog.log";
        private string AutoNextLog = Application.StartupPath + "\\Data\\AutoNextLog.log";
        private string AutoPrintLog = Application.StartupPath + "\\Data\\AutoPrintLog.log";

        //考试分配信息
        private RaceStudentData raceStudentData = new RaceStudentData();

        private StringBuilder searchStudentSb = new StringBuilder();

        private AutoSizeWindow AutoSizeWindow = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunningWindow_Load(object sender, EventArgs e)
        {
            BarCode.Start();
            CheckForIllegalCrossThreadCalls = false;
            string code = "程序集版本:" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            toolStripStatusLabel1.Text = code;
            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();

            SerialInit();
            int roundCount = sportProjectInfos.RoundCount;
            if (roundCount > 0)
            {
                for (int i = 0; i < roundCount; i++)
                {
                    roundCountCbx.Items.Add($"第{i + 1}轮");
                }
                roundCountCbx.SelectedIndex = 0;
            }
            _TestMethod = sportProjectInfos.TestMethod;
            comboBox1.SelectedIndex = _TestMethod;
            List<LocalInfos> list0 = fsql.Select<LocalInfos>().ToList();
            foreach (var item in list0)
            {
                localInfos.Add(item.key, item.value);
            }
            USBWatcher.AddUSBEventWatcher(USBEventHandler, USBEventHandler, new TimeSpan(0, 0, 1));
            //初始化标题
            InitListViewHead(sportProjectInfos.RoundCount);
            Update_groups(_groupName);
            LoadLocalData();
            CameraInit();
        }

        #region 相机

        private void CameraInit()
        {
            pBox1Width = pictureBox1.Width;
            pBox1Height = pictureBox1.Height;
            _width = 1280;
            _height = 720;
            OpenCameraSetting();
            recTimeTxt.Items.Clear();
            for (int i = 1; i < 10; i++)
            {
                recTimeTxt.Items.Add(i.ToString());
            }
            for (int i = 10; i <= 100; i += 10)
            {
                recTimeTxt.Items.Add(i.ToString());
            }
            int v = recTimeTxt.Items.IndexOf("60");
            recTimeTxt.SelectedIndex = v;
            if (rgbVideoSource != null)
            {
                offsetX = pBox1Width * 1f / 1280;
                offsetY = pBox1Height * 1f / 720;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void OpenCameraSetting()
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            CameraSettingWindow frmc = new CameraSettingWindow();
            frmc.filterInfoCollection = filterInfoCollection;
            if (frmc.ShowDialog() == DialogResult.OK)
            {
                cameraName = frmc.cameraName;
                maxFps = frmc.maxFps;
                Fps = frmc.Fps;
                if (Fps == 0)
                {
                    cameraSkip = maxFps;
                }
                else
                {
                    cameraSkip = maxFps / Fps;
                }

                if (!string.IsNullOrEmpty(cameraName))
                {
                    OpenCamearaFun(cameraName);
                }
            }
        }

        /// <summary>
        /// 打开摄像头方法
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns></returns>
        private bool OpenCamearaFun(string cameraName)
        {
            bool flag = LoadCamera(cameraName);
            if (!flag)
            {
                openCameraBtn.Text = "打开摄像头";
                openCameraBtn.BackColor = Color.White;
                cameraState.Text = "摄像头未开启";
                cameraState.ForeColor = Color.Red;
                FrmTips.ShowTipsError(this, "打开摄像头失败!");
            }
            else
            {
                cameraState.Text = "摄像头开启中";
                cameraState.ForeColor = Color.Green;
                openCameraBtn.Text = "关闭摄像头";
                openCameraBtn.BackColor = Color.Red;
            }
            return flag;
        }

        /// <summary>
        /// 打开写入本地流
        /// </summary>
        /// <param name="outPath"></param>
        public void OpenVideoOutPut(string outPath)
        {
            try
            {
                if (VideoOutPut != null && VideoOutPut.IsOpened())
                {
                    VideoOutPut.Release();
                }
                VideoOutPut = new OpenCvSharp.VideoWriter(outPath, OpenCvSharp.FourCC.XVID, 60, new OpenCvSharp.Size(_width, _height));
                Console.WriteLine(VideoOutPut.IsOpened());
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 释放写入本地流
        /// </summary>
        public void ReleaseVideoOutPut()
        {
            try
            {
                if (VideoOutPut != null && VideoOutPut.IsOpened())
                {
                    VideoOutPut.Release();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool rgbVideoSourceStart()
        {
            bool flag = false;

            if (rgbVideoSource != null)
            {
                rgbVideoSource.Start();
                //rgbVideoSource.Show();
                return true;
            }
            if (string.IsNullOrEmpty(cameraName))
            {
                FrmTips.ShowTipsError(this, "请选择摄像头!");
                return false;
            }
            flag = OpenCamearaFun(cameraName);
            return flag;
        }

        /// <summary>
        ///
        /// </summary>
        public void rgbVideoSourceStop()
        {
            CloseCamera();
        }

        /// <summary>
        ///
        /// </summary>
        private void CloseCamera()
        {
            try
            {
                if (!rgbVideoSource.IsRunning) return;
                if (rgbVideoSource != null && rgbVideoSource.IsRunning)
                {
                    rgbVideoSource.SignalToStop();
                    //rgbVideoSource.Hide();
                }
                openCameraBtn.Text = "打开摄像头";
                cameraState.Text = "摄像头未开启";
                cameraState.ForeColor = Color.Red;
                openCameraBtn.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        /// 刷新视频显示
        /// </summary>
        private void rgbVideoSourceRePaint()
        {
            pictureBox1.Refresh();
            if (!rgbVideoSource.IsRunning)
                rgbVideoSource.Refresh();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        /// <param name="markerTop1"></param>
        private void drawPointCross(Graphics g, System.Drawing.Point markerTop1)
        {
            Pen pen = new Pen(Color.Red, 3);
            g.DrawLine(pen, markerTop1.X - 15, markerTop1.Y, markerTop1.X + 15, markerTop1.Y);
            g.DrawLine(pen, markerTop1.X, markerTop1.Y - 15, markerTop1.X, markerTop1.Y + 15);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void areaSetBtn_Click(object sender, EventArgs e)
        {
            drawFlag = 1;
        }

        /// <summary>
        /// 载入摄像头
        /// </summary>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool LoadCamera(string name)
        {
            bool flag = false;

            try
            {
                if (rgbVideoSource.IsRunning)
                {
                    rgbVideoSource.SignalToStop();
                    //rgbVideoSource.Hide();
                }
                Boolean findIt = false;
                foreach (FilterInfo device in filterInfoCollection)
                {
                    if (device.Name == name)
                    {
                        rgbDeviceVideo = new VideoCaptureDevice(device.MonikerString);
                        if (rgbDeviceVideo.VideoCapabilities.Length == 1)
                        {
                            rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[0];
                            findIt = true;
                        }
                        else
                        {
                            for (int i = 0; i < rgbDeviceVideo.VideoCapabilities.Length; i++)
                            {
                                if (rgbDeviceVideo.VideoCapabilities[i].FrameSize.Width == _width
                                    && rgbDeviceVideo.VideoCapabilities[i].FrameSize.Height == _height
                                    && rgbDeviceVideo.VideoCapabilities[i].AverageFrameRate == maxFps)
                                {
                                    rgbDeviceVideo.VideoResolution = rgbDeviceVideo.VideoCapabilities[i];
                                    findIt = true;
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
                if (findIt)
                {
                    rgbVideoSource.VideoSource = rgbDeviceVideo;
                    rgbVideoSource.Start();
                    rgbVideoSource.SendToBack();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
                LoggerHelper.Debug(ex);
            }
            if (rgbVideoSource.IsRunning)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion 相机

        /// <summary>
        ///
        /// </summary>
        private void LoadLocalData()
        {
            try
            {
                string[] strg = File.ReadAllLines(AutoSaveLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        AutoSaveCheck.Checked = true;
                    }
                    else
                    {
                        AutoSaveCheck.Checked = false;
                    }
                }

                strg = File.ReadAllLines(AutoNextLog);
                if (strg.Length > 0)
                {
                    if (strg[0] == "1")
                    {
                        AutoNextCheck.Checked = true;
                    }
                    else
                    {
                        AutoNextCheck.Checked = false;
                    }
                }

                string[] str = File.ReadAllLines(@"./data/point.dat");
                if (str.Length > 1)
                {
                    string[] s1 = str[0].Split(',');
                    leftTopPoint.X = Str2int(s1[0]);
                    leftTopPoint.Y = Str2int(s1[1]);
                    s1 = str[1].Split(',');
                    rightBottomPoint.X = Str2int(s1[0]);
                    rightBottomPoint.Y = Str2int(s1[1]);
                    int ltpx = leftTopPoint.X;
                    int ltpy = leftTopPoint.Y;
                    int rbpx = rightBottomPoint.X;
                    int rbpy = rightBottomPoint.Y;
                    rectFlag = new Rectangle(ltpx, ltpy,
                        rbpx - ltpx, rbpy - ltpy);
                }
                str = File.ReadAllLines(@"./data/timeOut.dat");
                if (str.Length > 0)
                {
                    timeOutTB.Text = str[0];
                }
                str = File.ReadAllLines(@"./data/tolerance.dat");
                if (str.Length > 0)
                {
                    toleranceTxt.Text = str[0];
                }
                str = File.ReadAllLines(@"./data/threshold.dat");
                if (str.Length > 0)
                {
                    thresholdTxt.Text = str[0];
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int Str2int(string s)
        {
            return Convert.ToInt32(s);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectGroupName"></param>
        private void Update_groups(string selectGroupName = null)
        {
            try
            {
                List<string> list = fsql.Select<DbGroupInfos>().Distinct().ToList(a => a.Name);
                groupsCbx.Items.Clear();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        groupsCbx.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                groupsCbx.Items.Clear();
            }

            try
            {
                if (!string.IsNullOrEmpty(selectGroupName))
                {
                    int index = groupsCbx.Items.IndexOf(selectGroupName);
                    if (index >= 0)
                    {
                        groupsCbx.SelectedIndex = index;
                    }
                    else
                    {
                        groupsCbx.SelectedIndex = 0;
                    }
                }
                else
                {
                    groupsCbx.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rountCount"></param>
        private void InitListViewHead(int rountCount)
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 40;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组号";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 120;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "最好成绩";
            Header[sp].Width = 80;

            sp++;
            for (int i = 1; i <= rountCount; i++)
            {
                Header[sp] = new ColumnHeader();
                Header[sp].Text = $"第{i}轮";
                Header[sp].Width = 80;
                sp++;

                Header[sp] = new ColumnHeader();
                Header[sp].Text = "上传状态";
                Header[sp].Width = 80;
                sp++;
            }

            ColumnHeader[] Header1 = new ColumnHeader[sp];
            listView1.Columns.Clear();
            for (int i = 0; i < Header1.Length; i++)
            {
                Header1[i] = Header[i];
            }
            listView1.Columns.AddRange(Header1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventHandler(object sender, EventArrivedEventArgs e)
        {
            //暂未实现
            var watcher = sender as ManagementEventWatcher;
            watcher.Stop();
            if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
            {
            }
            else if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
            {
                //检测断开,断开提示
                //MessageBox.Show("设备断开请检查");
            }
            watcher.Start();
        }

        #region 扫码枪

        private DateTime _dt = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        private StringBuilder bardCodeSb = new StringBuilder();

        /// <summary>
        ///
        /// </summary>
        /// <param name="barCode"></param>
        private void BarCode_BarCodeEvent(BarCodes barCode)
        {
            ShowInfo(barCode);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="barCode"></param>
        private delegate void ShowInfoDelegate(BarCodes barCode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="barCode"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ShowInfo(BarCodes barCode)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
            }
            else
            {
                string inputCode = barCode.KeyName;
                Console.WriteLine(inputCode);
                if (BarCodeText.Focused) return;
                DateTime tempDt = DateTime.Now;//保存按键按下时刻的时间点
                TimeSpan ts = tempDt.Subtract(_dt);//获取时间间隔
                _dt = tempDt;
                if (ts.Milliseconds > 50) { bardCodeSb.Clear(); }

                if (barCode.IsValid)
                {
                    if (inputCode == "Enter")
                    {
                        //回车处理
                        Console.WriteLine("回车处理");
                        bardCodeSb.Clear();
                        SearchStudentByIdNumber();
                    }
                }
                else
                {
                    if (isNumberic(inputCode))
                    {
                        ///键盘输入
                        bardCodeSb.Append(inputCode);
                        BarCodeText.Text = bardCodeSb.ToString();
                    }
                }
            }
            GC.KeepAlive(BarCode);
        }

        /// <summary>
        ///
        /// </summary>
        private void SearchStudentByIdNumber()
        {
            searchStudentSb.Clear();
            string Idnumber = BarCodeText.Text;
            DbPersonInfos dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == Idnumber).ToOne();
            if (dbPersonInfos == null)
            {
                FrmTips.ShowTipsError(this, "未找到该考生");
                return;
            }
            if (groupsCbx.Text != dbPersonInfos.GroupName)
            {
                int gIndex = groupsCbx.Items.IndexOf(dbPersonInfos.GroupName);

                if (gIndex == -1)
                {
                    FrmTips.ShowTipsError(this, "$未找到组别{dbPersonInfos.GroupName}");
                }
                else
                {
                    searchStudentSb.Append(Idnumber);
                    groupsCbx.SelectedIndex = gIndex;
                }
            }
            else
            {
                if (listView1.Items.Count > 0)
                {
                    searchStudentSb.Append(Idnumber);
                    int vindex = -1;
                    if (searchStudentSb.Length > 0)
                    {
                        vindex = ListViewStudentsData.FindIndex(a => a.idNumber == searchStudentSb.ToString());
                        searchStudentSb.Clear();
                        if (vindex == -1) FrmTips.ShowTipsError(this, "查询失败");
                    }
                    else
                    {
                        vindex = ListViewStudentsData.FindIndex(a => a.idNumber == examStuIdNumber.Text);
                    }
                    if (vindex == -1) vindex = 0;
                    listView1.Items[vindex].Selected = true;
                }
            }

            BarCodeText.Text = "";
            bardCodeSb.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool isNumberic(string message)
        {
            try
            {
                int result = Convert.ToInt32(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion 扫码枪

        #region 串口

        private bool SerialInit()
        {
            bool flag = true;
            try
            {
                sReader = new ScreenSerialReader();
                sReader.AnalyCallback = ScreenAnalyData;
                sReader.ReceiveCallback = ScreenReceiveData;
                sReader.SendCallback = ScreenSendData;
                SerialTool.init();
                ConnectPort();
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="btArySendData"></param>
        private void ScreenSendData(byte[] btArySendData)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="btAryReceiveData"></param>
        private void ScreenReceiveData(byte[] btAryReceiveData)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="btAryAnalyData"></param>
        private void ScreenAnalyData(byte[] btData)
        {
            string v = Encoding.UTF8.GetString(btData, 0, btData.Length);
            richTextBox1.AppendText(v + "\n");
            if (v.Contains("ch="))
            {
                string v1 = v.Replace("ch=", "");
                v1 = v1.Trim();
                textBox1.Text = v1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ConnectPort()
        {
            SendInfoBtn.Enabled = false;
            if (openSerialPortBtn.Text == "断开显示屏")
            {
                if (sReader != null && sReader.IsComOpen())
                {
                    //处理串口断开连接读写器
                    sReader.CloseCom();
                }
                ScreenState.Text = "单人显示屏未连接";
                ScreenState.ForeColor = Color.Red;
                openSerialPortBtn.Text = "连接显示屏";
            }
            else if (openSerialPortBtn.Text == "连接显示屏")
            {
                try
                {
                    string pid = pidtxt.Text;
                    string vid = vidtxt.Text;
                    string portn = portNameSearch.Text;
                    List<string> strPorts = new List<string>();

                    if (portn == "USB Serial Port")
                    {
                        //无线
                        string[] portNames = GetPortDeviceName(portn);
                        if (portNames.Length == 0)
                        {
                            portn = "USB-SERIAL";
                            portNames = GetPortDeviceName(portn);
                        }
                        if (portNames.Length == 0)
                        {
                            portn = "USB-to-Serial";
                            portNames = GetPortDeviceName(portn);
                        }
                        strPorts = portNames.ToList();
                        tb_nBaudrate.Text = "115200";
                    }
                    if (strPorts.Count == 0)
                    {
                        portn = "USB 串行设备";

                        ///有线
                        strPorts = GetScreenCom(portn);
                        if (strPorts.Count > 0)
                        {
                            portNameSearch.Text = portn;
                            tb_nBaudrate.Text = "57600";
                        }
                    }

                    string strPort = string.Empty;
                    if (strPorts.Count > 0)
                    {
                        strPort = strPorts[0];
                    }
                    //string strPort = SerialTool.PortDeviceName2PortName(portNamesList.Text);
                    if (string.IsNullOrEmpty(strPort))
                    {
                        //FrmTips.ShowTipsError(this, "请连接单人显示屏串口");
                        ScreenState.Text = "单人显示屏未连接";
                        ScreenState.ForeColor = Color.Red;
                        return;
                    }

                    int.TryParse(tb_nBaudrate.Text, out int nBaudrate);
                    string strException = string.Empty;
                    int nRet = sReader.OpenCom(strPort, nBaudrate, out strException);
                    if (nRet == -1)
                    {
                        ScreenState.Text = "单人显示屏未连接";
                        ScreenState.ForeColor = Color.Red;
                        openSerialPortBtn.Text = "连接显示屏";
                        //FrmTips.ShowTipsError(this, "打开单人显示屏失败");
                    }
                    else
                    {
                        SendInfoBtn.Enabled = true;
                        ScreenState.Text = "单人显示屏已连接连接";
                        ScreenState.ForeColor = Color.Green;
                        openSerialPortBtn.Text = "断开显示屏";
                        // FrmTips.ShowTipsSuccess(this, "打开单人显示屏串口成功");
                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                }
            }
        }

        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private string[] GetPortDeviceName(string portName = null)
        {
            if (string.IsNullOrEmpty(portName)) portName = _portNameType;
            List<string> strs = new List<string>();
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where Name like '%(COM%'"))
            {
                var hardInfos = searcher.Get();
                foreach (var hardInfo in hardInfos)
                {
                    if (hardInfo.Properties["Name"].Value != null)
                    {
                        string deviceName = hardInfo.Properties["Name"].Value.ToString();
                        if (deviceName.Contains(portName))
                        {
                            int a = deviceName.IndexOf("(COM") + 1;//a会等于1
                            string str = deviceName.Substring(a, deviceName.Length - a);
                            a = str.IndexOf(")");//a会等于1
                            str = str.Substring(0, a);
                            strs.Add(str);
                        }
                    }
                }
            }
            return strs.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pid0"></param>
        /// <param name="vid0"></param>
        /// <returns></returns>
        private List<string> GetScreenCom(string pid0 = "1000", string vid0 = "1122")
        {
            List<string> coms = new List<string>();
            string[] available_spectrometers = SerialPort.GetPortNames();
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = null;
            string commData = string.Empty;
            ManagementObjectSearcher mObjs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM WIN32_PnPEntity");
            try
            {
                enumerator = mObjs.Get().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ManagementObject current = (ManagementObject)enumerator.Current;
                    if (current["Caption"] != null)
                    {
                        string str1 = current["Caption"].ToString();// "(COM173"
                        if (str1.Contains("(COM"))
                        {
                            string strVid = "0";
                            string strPid = "0";

                            string[] str1g = current["DeviceID"].ToString().Split('\\');
                            foreach (string str2 in str1g)
                            {
                                if ((str2.Contains("VID_")) || (str2.Contains("PID_")))
                                {
                                    string[] strbG = str2.Split('&');
                                    foreach (string strb in strbG)
                                    {
                                        if (strb.Contains("VID_"))
                                        {
                                            strVid = strb.Replace("VID_", "");
                                            continue;
                                        }
                                        if (strb.Contains("PID_"))
                                        {
                                            strPid = strb.Replace("PID_", "");
                                            continue;
                                        }
                                    }
                                }
                            }

                            Debug.WriteLine(current["DeviceID"]);
                            if (strPid == pid0 && strVid == vid0)
                            {
                                commData = current["Caption"] + "";
                                commData = SerialTool.PortDeviceName2PortName(commData);
                                coms.Add(commData);
                            }
                        }
                    }
                }
            }
            finally
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                }
            }
            return coms;//返回串口号
        }

        /// <summary>
        /// 发送成绩至小屏
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Score"></param>
        private bool SendScore(string name, string Score, string txt3 = "个")
        {
            if (sReader == null || !sReader.IsComOpen()) return false;
            try
            {
                int c1 = 0;//红
                int c2 = 1;//绿
                int c3 = 2;//蓝
                byte[] result = SerialTool.PushText_BL2(name, c1, Score, c2, txt3, c3);
                Task.Run(() =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        sReader.SendMessage(result);
                        Thread.Sleep(300);
                    }
                });
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            return true;
        }

        #endregion 串口

        #region 页面事件

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="image"></param>
        private void rgbVideoSource_NewFrame(object sender, ref Bitmap image)
        {
            skipFrameDispR0++;
            if (rgbVideoSourcePaintFlag) return;
            rgbVideoSourcePaintFlag = true;
            try
            {
                if (skipFrameDispR0 < cameraSkip)
                {
                    return;
                }
                skipFrameDispR0 = 0;
                // if (rgbVideoSource.IsRunning)
                {
                    //得到当前RGB摄像头下的图片
                    Bitmap bmp = ImageHelper.DeepCopyBitmap(image);
                    if (bmp == null)
                    {
                        return;
                    }
                    Bitmap dstBitmap = null;
                    //if (recTimeR0 > 0)
                    {
                        frameSum++;
                        ///处理录像数据
                        Bitmap backBp = ImageHelper.DeepCopyBitmap(bmp);
                        try
                        {
                            if (FuseBitmap.BackGroundBmp == null || updateBackImgFlag)
                            {
                                FuseBitmap.setBackGround(backBp, tolerance);
                                updateBackImgFlag = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Debug(ex);
                        }
                        finally
                        {
                            updateBackImgFlag = false;
                        }
                        fb = new FuseBitmap(bmp);
                        fb.SetRect(leftTopPoint, rightBottomPoint);
                        fb.FuseColorImg(tolerance, true);
                        fb.Dispose();
                        dstBitmap = fb.dstBitmap;
                        ThresholdLabel.Text = $"Threshold:({fb.unsamePointSum})";
                        if (fb.unsamePointSum >= threshold)
                        {
                            if (isRightScore && ballFallingIntervalStep >= (ballFallingInterval / 10))
                            {
                                ScoreSum++;
                                Task.Run(() =>
                                {
                                    Console.Beep();
                                });
                                ballFallingIntervalStep = 0;
                                ControlHelper.ThreadInvokerControl(this, () =>
                                {
                                    examStuScore.Text = ScoreSum.ToString();
                                });
                                if (!string.IsNullOrEmpty(raceStudentData.idNumber))
                                {
                                    SendScore(raceStudentData.name, ScoreSum.ToString().PadLeft(4, '0'), "");
                                }
                            }
                            isRightScore = false;
                        }
                        else
                        {
                            isRightScore = true;
                        }
                    }
                    frameRecSum++;//计算帧速用
                    if (dstBitmap != null && fb != null)
                    {
                        bmp = ImageHelper.DeepCopyBitmap(dstBitmap);
                    }
                    Graphics g = Graphics.FromImage(bmp);
                    Pen pen = new Pen(Color.MediumSpringGreen, 1);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.DrawRectangle(pen, rectFlag);
                    g.DrawString(ScoreSum.ToString().PadLeft(3, '0'), new Font("Arial", 40), new SolidBrush(Color.Red), 20, 10);
                    //时间
                    g.DrawString($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", new Font("宋体", 20), new SolidBrush(Color.Red), 200, 10);
                    if (!string.IsNullOrEmpty(raceStudentData.idNumber))
                    {
                        //考生姓名和学号
                        g.DrawString($"组号:{groupsCbx.Text} 考号:{raceStudentData.idNumber} 姓名:{raceStudentData.name}", new Font("宋体", 16), new SolidBrush(Color.Red), 600, 15);
                    }
                    if (recTimeR0 > 0)
                    {
                        //是否写入
                        if (VideoOutPut != null && VideoOutPut.IsOpened())
                        {
                            Bitmap bitmap = ImageHelper.DeepCopyBitmap(bmp);
                            OpenCvSharp.Mat mat = ImageHelper.Bitmap2Mat(bitmap);
                            VideoOutPut.Write(mat);
                        }
                    }
                    //显示图片
                    pictureBox1.Image = bmp;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                GCCounta++;
                if (GCCounta > 10)
                {
                    GCCounta = 0;
                    GC.Collect();
                }
                rgbVideoSourcePaintFlag = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePoint.X = e.X;
            mousePoint.Y = e.Y;
            switch (drawFlag)
            {
                case 1:
                    leftTopPoint.X = e.X;
                    leftTopPoint.Y = e.Y;
                    rightBottomPoint.X = e.X;
                    rightBottomPoint.Y = e.Y;
                    break;

                case 2:
                    rightBottomPoint.X = e.X;
                    rightBottomPoint.Y = e.Y;
                    int ltpx = leftTopPoint.X;
                    int ltpy = leftTopPoint.Y;
                    int rbpx = rightBottomPoint.X;
                    int rbpy = rightBottomPoint.Y;
                    rectFlag = new Rectangle(ltpx, ltpy,
                        rbpx - ltpx, rbpy - ltpy);
                    break;

                default:
                    break;
            }
            if (drawFlag != 0)
            {
                pictureBox1.Refresh();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            /* Pen pen = new Pen(Color.MediumSpringGreen, 1);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int ltpx = leftTopPoint.X;
             int ltpy = leftTopPoint.Y;
             int rbpx = rightBottomPoint.X;
             int rbpy = rightBottomPoint.Y;
             g.DrawRectangle(pen, new Rectangle(ltpx, ltpy,
                 rbpx - ltpx, rbpy - ltpy));
             g.DrawString(ScoreSum.ToString().PadLeft(3, '0'), new Font("Arial", 40), new SolidBrush(Color.Red), 20, 10);*/
            drawPointCross(g, mousePoint);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (drawFlag == 1)
            {
                drawFlag = 2;
                leftTopPoint.X = e.X;
                leftTopPoint.Y = e.Y;
                rightBottomPoint.X = e.X;
                rightBottomPoint.Y = e.Y;
            }
            else if (drawFlag == 2)
            {
                drawFlag = 0;
                rightBottomPoint.X = e.X;
                rightBottomPoint.Y = e.Y;

                int ltpx = leftTopPoint.X;
                int ltpy = leftTopPoint.Y;
                int rbpx = rightBottomPoint.X;
                int rbpy = rightBottomPoint.Y;
                rectFlag = new Rectangle(ltpx, ltpy,
                    rbpx - ltpx, rbpy - ltpy);

                File.WriteAllLines(@"./data/point.dat", new string[] {
                    $"{leftTopPoint.X},{leftTopPoint.Y}",
                    $"{rightBottomPoint.X},{rightBottomPoint.Y}" });
            }
            pictureBox1.Refresh();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pBox1Width = pictureBox1.Width;
            pBox1Height = pictureBox1.Height;
            if (rgbVideoSource != null)
            {
                offsetX = pBox1Width * 1f / 1280;
                offsetY = pBox1Height * 1f / 720;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupsCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_listView();
        }

        /// <summary>
        /// 更新视图显示
        /// </summary>
        private List<DbPersonInfos> Update_listView()
        {
            List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
            try
            {
                ListViewStudentsData.Clear();
                listView1.Items.Clear();
                int index = groupsCbx.SelectedIndex;
                string groupName = groupsCbx.Text;
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                if (dbPersonInfos.Count == 0) return dbPersonInfos;
                int step = 1;
                listView1.BeginUpdate();
                Font f = new Font(Control.DefaultFont, FontStyle.Bold);
                bool isBestScore = false;
                foreach (var dbPersonInfo in dbPersonInfos)
                {
                    ListViewItem li = new ListViewItem();
                    li.UseItemStyleForSubItems = false;
                    li.Text = step.ToString();
                    li.SubItems.Add(dbPersonInfo.SchoolName);
                    li.SubItems.Add(dbPersonInfo.GroupName);
                    li.SubItems.Add(dbPersonInfo.IdNumber);
                    li.SubItems.Add(dbPersonInfo.Name);
                    li.SubItems.Add("未测试");
                    ListViewStudentsData.Add(new RaceStudentData
                    {
                        RaceStudentDataId = step,
                        idNumber = dbPersonInfo.IdNumber,
                        name = dbPersonInfo.Name,
                        id = dbPersonInfo.Id.ToString(),
                    });
                    List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
                        .Where(a => a.PersonId == dbPersonInfo.Id.ToString() && a.IsRemoved == 0)
                        .OrderBy(a => a.Id)
                        .ToList();
                    int resultRound = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    bool getScore = false;
                    foreach (var resultInfo in resultInfos)
                    {
                        if (resultInfo.State != 1)
                        {
                            string s_rstate = ResultStateType.Match(resultInfo.State);
                            li.SubItems.Add(s_rstate);
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            getScore = true;
                            li.SubItems.Add(resultInfo.Result.ToString());
                            li.SubItems[li.SubItems.Count - 1].BackColor = Color.MediumSpringGreen;
                            if (isBestScore)
                            {
                                //取最大值
                                if (MaxScore < resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                            else
                            {
                                //取最小值
                                if (MaxScore > resultInfo.Result) MaxScore = resultInfo.Result;
                            }
                        }
                        li.SubItems[li.SubItems.Count - 1].Font = f;
                        if (resultInfo.uploadState == 0)
                        {
                            li.SubItems.Add("未上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.Red;
                        }
                        else if (resultInfo.uploadState == 1)
                        {
                            li.SubItems.Add("已上传");
                            li.SubItems[li.SubItems.Count - 1].ForeColor = Color.MediumSpringGreen;
                            li.SubItems[li.SubItems.Count - 1].Font = f;
                        }
                        resultRound++;
                    }
                    for (int i = resultRound; i < sportProjectInfos.RoundCount; i++)
                    {
                        li.SubItems.Add("未测试");
                        li.SubItems.Add("未上传");
                    }
                    if (getScore)
                    { li.SubItems[5].Text = MaxScore.ToString(); }
                    step++;
                    listView1.Items.Insert(listView1.Items.Count, li);
                }
                ListViewUtils.AutoResizeColumnWidth(listView1);
                listView1.EndUpdate();

                if (listView1.Items.Count > 0)
                {
                    int vindex = -1;
                    if (searchStudentSb.Length > 0)
                    {
                        vindex = ListViewStudentsData.FindIndex(a => a.idNumber == searchStudentSb.ToString());
                        searchStudentSb.Clear();
                        if (vindex == -1) FrmTips.ShowTipsError(this, "查询失败");
                    }
                    else
                    {
                        vindex = ListViewStudentsData.FindIndex(a => a.idNumber == examStuIdNumber.Text);
                    }
                    if (vindex == -1) vindex = 0;
                    listView1.Items[vindex].Selected = true;
                }
            }
            catch (Exception ex)
            {
                listView1.Items.Clear();
                dbPersonInfos.Clear();
                LoggerHelper.Debug(ex);
            }
            return dbPersonInfos;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundCountCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roundCountCbx.SelectedIndex >= 0)
            {
                _nowRound = roundCountCbx.SelectedIndex;
                roundText.Text = (_nowRound + 1).ToString();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Update_groups();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            SearchStudentByIdNumber();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListView lv1 = (ListView)sender;
            ListViewItem lvi1 = lv1.GetItemAt(e.X, e.Y);
            if (lvi1 != null && e.Button == MouseButtons.Right)
            {
                this.cmsListViewItem.Show(lv1, e.X, e.Y);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                string idnumber = listView1.Items[e.ItemIndex].SubItems[3].Text;
                string stuName = listView1.Items[e.ItemIndex].SubItems[4].Text;

                for (int i = 1; i <= sportProjectInfos.RoundCount; i++)
                {
                    ResultInfos resultInfos = fsql.Select<ResultInfos>().Where(a => a.PersonIdNumber == idnumber).Where(a => a.RoundId == i).ToOne();
                    roundCountCbx.SelectedIndex = -1;
                    roundCountCbx.SelectedIndex = i - 1;
                    raceStudentData.idNumber = idnumber;
                    raceStudentData.name = stuName;
                    examStuIdNumber.Text = idnumber;
                    examStuName.Text = stuName;
                    examStuScore.Text = "0";
                    if (resultInfos == null)
                    {
                        examStuState.Text = "未测试";
                        break;
                    }
                    else
                    {
                        examStuState.Text = "已测试";
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 缺考ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetErrorState("缺考");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 中退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetErrorState("中退");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 犯规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetErrorState("犯规");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 弃权ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetErrorState("弃权");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修正成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                FixCurrrentStudentDataWindow fcr = new FixCurrrentStudentDataWindow();
                fcr._idnumber = idnumber;
                fcr.mode = 1;
                fcr.ShowDialog();
                Update_listView();
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 成绩重测ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                FixCurrrentStudentDataWindow fcr = new FixCurrrentStudentDataWindow();
                fcr._idnumber = idnumber;
                fcr.mode = 0;
                fcr.ShowDialog();
                Update_listView();
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 成绩查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;

                DbPersonInfos dpi = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();
                if (dpi != null)
                {
                    string name = dpi.Name;
                    string nowTestDir1 = $"{sportProjectInfos.Name}\\{dpi.GroupName}\\{dpi.IdNumber}_{name}\\第{1}轮\\";
                    nowTestDir1 = strMainModule + nowTestDir1;
                    if (Directory.Exists(nowTestDir1))
                    {
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
                        psi.Arguments = "/e,/select," + nowTestDir1;
                        System.Diagnostics.Process.Start(psi);
                    }
                    else
                    {
                        FrmTips.ShowTipsError(this, "未找到文件夹");
                    }
                }
                else
                {
                    MessageBox.Show("数据异常");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }

        /// <summary>
        /// 设置异常状态
        /// </summary>
        /// <param name="stateStr"></param>
        private void SetErrorState(string stateStr)
        {
            try
            {
                if (listView1.SelectedItems.Count == 0) return;
                string idnumber = listView1.SelectedItems[0].SubItems[3].Text;
                int state = ResultStateType.ResultState2Int(stateStr);
                int result = fsql.Update<ResultInfos>().Set(a => a.State == state)
                    .Where(a => a.PersonIdNumber == idnumber && a.RoundId == _nowRound + 1).ExecuteAffrows();
                DbPersonInfos dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == idnumber).ToOne();

                //更新当前成绩状态没有就插入
                if (result == 0)
                {
                    fsql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    List<ResultInfos> insertResults = new List<ResultInfos>();

                    maxSortId++;
                    ResultInfos rinfo = new ResultInfos();
                    rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    rinfo.SortId = maxSortId;
                    rinfo.IsRemoved = 0;
                    rinfo.PersonId = dbPersonInfos.Id.ToString();
                    rinfo.SportItemType = 0;
                    rinfo.PersonName = dbPersonInfos.Name;
                    rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                    rinfo.RoundId = _nowRound + 1;
                    rinfo.Result = 0;
                    rinfo.State = state;
                    insertResults.Add(rinfo);
                    result = fsql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                }

                if (result > 0)
                {
                    string scoreContent = string.Format("时间:{0,-20},项目:{1,-20},组别:{2,-10},准考证号:{3,-20},姓名{4,-5},第{5}次成绩:{6,-5}, 状态:{7,-5}",
                                      DateTime.Now.ToString("yyyy年MM月dd日HH:mm:ss"),
                                      "排球垫球",
                                      dbPersonInfos.GroupName,
                                      dbPersonInfos.IdNumber,
                                      dbPersonInfos.Name,
                                      _nowRound + 1,
                                      0,
                                      stateStr);
                    File.AppendAllText(@"./操作日志.txt", scoreContent + "\n");
                    Update_listView();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoSaveCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoSaveCheck.Checked)
            {
                File.WriteAllText(AutoSaveLog, "1");
            }
            else
            {
                File.WriteAllText(AutoSaveLog, "0");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveScoreBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(raceStudentData.idNumber))
            {
                UIMessageBox.ShowWarning("请分配考生");
                return;
            }
            WriteScoreIntoDb();
            ClearMatchStudent();
            AutoNext();
        }

        private void AutoNext()
        {
            if (AutoSaveCheck.Checked)
            {
                if (_TestMethod == 0)
                {
                    //自动下一个
                    NextPerson();
                }
                else
                {
                    //自动下一轮
                    NextRound();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void NextRound()
        {
            int RoundIndex = roundCountCbx.SelectedIndex;
            List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
               .Where(a => a.RoundId == RoundIndex + 1)
               .Where(a => a.PersonIdNumber == examStuIdNumber.Text).ToList();
            if (resultInfos.Count > 0)
            {
                RoundIndex++;
                int ncount = roundCountCbx.Items.Count;
                if (RoundIndex >= 0 && RoundIndex < ncount)
                {
                    roundCountCbx.SelectedIndex = RoundIndex;
                }
                else
                {
                    NextPerson();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void NextPerson()
        {
            int GroupselectIndex = groupsCbx.SelectedIndex;
            int Groupround = groupsCbx.Items.Count;
            int ncount = ListViewStudentsData.Count;
            string AdminUserIdNumber = examStuIdNumber.Text;

            int groupIndex = ListViewStudentsData.FindIndex(a => a.idNumber == AdminUserIdNumber);

            if (groupIndex >= 0) groupIndex++;
            else groupIndex = 0;
            if (groupIndex < ncount)
            {
                //不是本组最后一个人
                //索引到下一个人
                listView1.Items[groupIndex].Selected = true;
            }
            else
            {
                //本组最后一个人就下一组
                GroupselectIndex++;
                if (GroupselectIndex >= 0 && GroupselectIndex < Groupround)
                {
                    groupsCbx.SelectedIndex = GroupselectIndex;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ClearMatchStudent()
        {
            raceStudentData.score = 0;
        }

        /// <summary>
        ///
        /// </summary>
        private void WriteScoreIntoDb()
        {
            fsql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
            List<ResultInfos> insertResults = new List<ResultInfos>();
            List<DbPersonInfos> dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.GroupName == groupsCbx.Text).ToList();
            StringBuilder errorsb = new StringBuilder();
            StringBuilder successsb = new StringBuilder();
            if (!string.IsNullOrEmpty(raceStudentData.idNumber))
            {
                raceStudentData.score = ScoreSum;
                string idNumber = raceStudentData.idNumber;
                int score = raceStudentData.score;
                DbPersonInfos df = dbPersonInfos.Find(a => a.IdNumber == idNumber);
                bool t_flag = false;
                //检查轮次
                for (int i = 1; i <= sportProjectInfos.RoundCount; i++)
                {
                    List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
                          .Where(a => a.PersonIdNumber == idNumber
                          && a.IsRemoved == 0
                          && a.RoundId == i)
                          .OrderBy(a => a.Id)
                          .ToList();
                    if (resultInfos.Count == 0)
                    {
                        t_flag = true;
                        maxSortId++;
                        ResultInfos rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.IsRemoved = 0;
                        rinfo.PersonId = df.Id.ToString();
                        rinfo.SportItemType = 0;
                        rinfo.PersonName = df.Name;
                        rinfo.PersonIdNumber = df.IdNumber;
                        rinfo.RoundId = i;
                        rinfo.Result = score;
                        rinfo.State = 1;
                        insertResults.Add(rinfo);
                        successsb.Append($"{df.Name},第{i}轮成绩:{score}");
                        string scoreContent = string.Format("时间:{0,-20},项目:{1,-20},组别:{2,-10},准考证号:{3,-20},姓名{4,-5},第{5}次成绩:{6,-5}, 状态:{7,-5}",
                                       DateTime.Now.ToString("yyyy年MM月dd日HH:mm:ss"),
                                       "排球垫球",
                                       df.GroupName,
                                       df.IdNumber,
                                       df.Name,
                                       i,
                                       score,
                                       "已测试");
                        File.AppendAllText(@"./成绩日志.txt", scoreContent + "\n");

                        break;
                    }
                }
                if (!t_flag)
                {
                    errorsb.AppendLine($"{df.IdNumber},{df.Name}轮次已满");
                }
                if (successsb.Length > 0)
                {
                    voiceOut0(successsb.ToString());
                    SendScore(df.Name, score.ToString().PadLeft(4, '0'), "");
                }
            }

            if (insertResults.Count > 0)
            {
                int result = fsql.InsertOrUpdate<ResultInfos>()
                                       .SetSource(insertResults)
                                       .IfExistsDoNothing()
                                       .ExecuteAffrows();
                if (errorsb.Length != 0) MessageBox.Show(errorsb.ToString());
                if (result > 0) Update_listView();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rate"></param>
        private void voiceOut0(string str, int rate = 2)
        {
            Task.Run(() =>
            {
                SpVoice voice = new SpVoice();
                ISpeechObjectTokens obj = voice.GetVoices();
                voice.Voice = obj.Item(0);
                voice.Rate = rate;
                voice.Speak(str, SpeechVoiceSpeakFlags.SVSFIsXML | SpeechVoiceSpeakFlags.SVSFDefault);
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            if (recTimeR0 > 0)
            {
                UIMessageBox.ShowWarning("考试中请勿进行此操作");
                return;
            }
            OpenCameraSetting();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openCameraBtn_Click(object sender, EventArgs e)
        {
            if (recTimeR0 > 0)
            {
                UIMessageBox.ShowWarning("考试中请勿进行此操作");
                return;
            }
            if (openCameraBtn.Text == "关闭摄像头")
            {
                CloseCamera();
                //openCameraBtn.Text = "打开摄像头";
                return;
            }
            if (string.IsNullOrEmpty(cameraName))
            {
                UIMessageBox.ShowWarning("请选择摄像头!");
                return;
            }
            OpenCamearaFun(cameraName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            PrePerson();
        }

        /// <summary>
        ///
        /// </summary>
        private void PrePerson()
        {
            int GroupselectIndex = groupsCbx.SelectedIndex;
            int Groupround = groupsCbx.Items.Count;
            int ncount = ListViewStudentsData.Count;
            string AdminUserIdNumber = examStuIdNumber.Text;
            int groupIndex = ListViewStudentsData.FindIndex(a => a.idNumber == AdminUserIdNumber);

            if (groupIndex >= 0) groupIndex--;
            else groupIndex = 0;
            if (groupIndex < ncount && groupIndex >= 0)
            {
                //不是本组最后一个人
                //索引到下一个人
                listView1.Items[groupIndex].Selected = true;
            }
            else
            {
                //本组最后一个人就下一组
                GroupselectIndex--;
                if (GroupselectIndex >= 0 && GroupselectIndex < Groupround)
                {
                    groupsCbx.SelectedIndex = GroupselectIndex;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            NextPerson();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PreRound();
        }

        /// <summary>
        ///
        /// </summary>
        private void PreRound()
        {
            int RoundIndex = roundCountCbx.SelectedIndex;
            RoundIndex--;
            int ncount = roundCountCbx.Items.Count;
            if (RoundIndex >= 0 && RoundIndex < ncount)
            {
                roundCountCbx.SelectedIndex = RoundIndex;
            }
            else
            {
                PrePerson();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            NextRound();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            BeginRec();
        }

        /// <summary>
        ///
        /// </summary>
        private int BeginRecTimeOut = 0;

        private void BeginRec()
        {
            if (BeginRecTimeOut > 0) return;
            try
            {
                if (recTimeR0 > 0)//停止录像
                {
                    recTimeR0 = 1;
                    return;
                }
                if (examStuState.Text != "未测试")
                {
                    MessageBox.Show("该考生本轮已测试");
                    return;
                }
                bool flag = BeginTest();

                if (flag)
                {
                    startRec();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                BeginRecTimeOut = 3;
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        private void startRec()
        {
            updateBackImgFlag = true;
            if (!isHaveStudent(true))
            {
                return;
            }
            recTimeR0 = 0;
            updateBackImgFlag = true;
            ControlHelper.ThreadInvokerControl(this, () =>
            {
                pictureBox2.Image = Image.FromFile("./Image/2.png");
                examStuState.Text = "测试中";
                examStuScore.Text = "0";
                timeLabel.Text = "00:00";
            });
            voiceOut0($"{raceStudentData.name}开始考试");
            SendScore($"{raceStudentData.name}", "开始考试", "");
            nowTestDir = $"{sportProjectInfos.Name}\\{groupsCbx.Text}\\{raceStudentData.idNumber}_{raceStudentData.name}\\{roundCountCbx.Text}\\";
            nowTestDir = strMainModule + nowTestDir;
            if (!Directory.Exists(nowTestDir))
            {
                DirectoryInfo dir = new DirectoryInfo(nowTestDir);
                dir.Create();//自行判断一下是否存在。
            }
            string avipath = Path.Combine(nowTestDir,
                $"{raceStudentData.idNumber}_{raceStudentData.name}_{roundCountCbx.Text}.mp4");
            if (File.Exists(avipath))
            {
                File.Delete(avipath);
            }
            OpenVideoOutPut(avipath);

            frameSum = 0;
            recTimeR0 = Str2int(recTimeTxt.Text) * 10;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool isHaveStudent(bool f = false)
        {
            bool flag = true;
            if (raceStudentData == null || string.IsNullOrEmpty(raceStudentData.idNumber)) flag = false;
            if (!flag && f) FrmTips.ShowTipsError(this, "未选择考生");
            return flag;
        }

        private bool BeginTest()
        {
            // SendScore(nowRaceStudentData.name, "准备考试", "");
            ScoreSum = 0;
            bool IhvaStu = isHaveStudent(true);
            if (!IhvaStu)
            {
                return IhvaStu;
            }
            ScoreSum = 0;
            recTimeR0 = 0;
            GC.Collect();
            try
            {
                rgbVideoSourceStart();
                return true;
            }
            catch (Exception ex)
            {
                FrmTips.ShowTipsError(this, "摄像头未开启");
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int testIndex = comboBox1.SelectedIndex;
            if (testIndex >= 0)
            {
                _TestMethod = testIndex;
                AutoNextCheck.Text = comboBox1.Text;
                int v = fsql.Update<SportProjectInfos>().Set(a => a.TestMethod == testIndex).Where("1=1").ExecuteAffrows();
                if (v > 0) sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
            }
        }

        private void AutoNextCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoNextCheck.Checked)
            {
                File.WriteAllText(AutoNextLog, "1");
            }
            else
            {
                File.WriteAllText(AutoNextLog, "0");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            updateBackImgFlag = true;

            int.TryParse(timeOutTB.Text, out ballFallingInterval);
            File.WriteAllText(Path.Combine(basePath, "timeOut.dat"), ballFallingInterval + "");

            int.TryParse(toleranceTxt.Text, out tolerance);
            File.WriteAllText(Path.Combine(basePath, "tolerance.dat"), tolerance + "");

            int.TryParse(thresholdTxt.Text, out threshold);
            File.WriteAllText(Path.Combine(basePath, "threshold.dat"), threshold + "");
            UIMessageBox.ShowSuccess("保存成功");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadGroupsBtn_Click(object sender, EventArgs e)
        {
            UpdateLoadScore();
        }

        private void UpdateLoadScore()
        {
            ControlHelper.ThreadInvokerControl(this, () =>
            {
                UploadGroupsBtn.Text = "上传中";
                UploadGroupsBtn.ForeColor = Color.Red;
            });
            string gn = groupsCbx.Text;
            StartUploadStuGroupScore(gn);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupName"></param>
        private void StartUploadStuGroupScore(string groupName)
        {
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(UploadStuGroupScoreThreadFun);
            Thread t = new Thread(ParStart);
            t.IsBackground = true;
            t.Start(groupName);
        }

        /// <summary>
        /// 上传学生的多线程方法 多人
        /// 先不上传视频
        /// </summary>
        /// <param name="obj"></param>
        public void UploadStuGroupScoreThreadFun(Object obj)
        {
            TxProcessRollForm txProcess = new TxProcessRollForm();

            try
            {
                new Thread((ThreadStart)delegate
                {
                    txProcess.ShowDialog();
                }).Start();
                string cpuid = cpuHelper.GetCpuID();
                List<Dictionary<string, string>> successList = new List<Dictionary<string, string>>();
                List<Dictionary<string, string>> errorList = new List<Dictionary<string, string>>();
                Dictionary<string, string> localInfos = new Dictionary<string, string>();
                List<LocalInfos> list0 = fsql.Select<LocalInfos>().ToList();
                foreach (var item in list0)
                {
                    localInfos.Add(item.key, item.value);
                }
                //组
                string groupName = obj as string;
                SportProjectInfos sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
                List<DbGroupInfos> dbGroupInfos = new List<DbGroupInfos>();
                ///查询本项目已考组
                if (!string.IsNullOrEmpty(groupName))
                {
                    //sql0 += $" AND Name = '{groupName}'";
                    dbGroupInfos = fsql.Select<DbGroupInfos>().Where(a => a.Name == groupName).ToList();
                }

                UploadResultsRequestParameter urrp = new UploadResultsRequestParameter();
                urrp.AdminUserName = localInfos["AdminUserName"];
                urrp.TestManUserName = localInfos["TestManUserName"];
                urrp.TestManPassword = localInfos["TestManPassword"];
                string MachineCode = localInfos["MachineCode"];
                string ExamId = localInfos["ExamId"];
                if (MachineCode.IndexOf('_') != -1)
                {
                    MachineCode = MachineCode.Substring(MachineCode.IndexOf('_') + 1);
                }
                if (ExamId.IndexOf('_') != -1)
                {
                    ExamId = ExamId.Substring(ExamId.IndexOf('_') + 1);
                }
                urrp.MachineCode = MachineCode;
                urrp.ExamId = ExamId;
                StringBuilder messageSb = new StringBuilder();
                StringBuilder logWirte = new StringBuilder();

                ///按组上传
                foreach (var gInfo in dbGroupInfos)
                {
                    List<DbPersonInfos> dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.GroupName == gInfo.Name).ToList();
                    StringBuilder resultSb = new StringBuilder();
                    List<SudentsItem> sudentsItems = new List<SudentsItem>();
                    //IdNumber 对应Id
                    Dictionary<string, string> map = new Dictionary<string, string>();
                    //取值模式
                    bool isBestScore = false;
                    if (sportProjectInfos.BestScoreMode == 0) isBestScore = true;
                    foreach (var stu in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = fsql.Select<ResultInfos>().Where(a => a.PersonIdNumber == stu.IdNumber).ToList();
                        //无成绩的跳过
                        if (resultInfos.Count == 0) continue;
                        int state = -1;
                        string examTime = string.Empty;
                        double MaxScore = 99999;
                        if (isBestScore) MaxScore = 0;
                        int maxScoreRoundId = 1;
                        foreach (var ri in resultInfos)
                        {
                            if (ri.State <= 0) continue;
                            ///异常状态
                            if (ri.State != 1)
                            {
                                if (isBestScore && MaxScore < 0)
                                {
                                    //取最大值
                                    MaxScore = 0;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > 99999)
                                {
                                    //取最小值
                                    MaxScore = 99999;
                                    state = ri.State;
                                }
                            }
                            else
                            {
                                if (isBestScore && MaxScore < ri.Result)
                                {
                                    //取最大值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                                else if (!isBestScore && MaxScore > ri.Result)
                                {
                                    //取最小值
                                    MaxScore = ri.Result;
                                    state = ri.State;
                                }
                            }
                            examTime = ri.CreateTime;
                            maxScoreRoundId = ri.RoundId;
                        }
                        if (state < 0) continue;
                        if (state != 1)
                        {
                            MaxScore = 0;
                        }
                        List<RoundsItem> roundsItems = new List<RoundsItem>();
                        RoundsItem rdi = new RoundsItem();
                        rdi.RoundId = maxScoreRoundId;
                        rdi.State = ResultStateType.Match(state);
                        rdi.Time = examTime;
                        StringBuilder logSb = new StringBuilder();
                        try
                        {
                            List<LogInfos> logInfos = fsql.Select<LogInfos>()
                                .Where(a => a.IdNumber == stu.IdNumber && a.State != -404)
                                .ToList();
                            logInfos.ForEach(item =>
                            {
                                string sbtxt = $"时间：{item.CreateTime},考号:{item.IdNumber},{item.Remark};";
                                logSb.Append(sbtxt);
                            });
                        }
                        catch (Exception)
                        {
                            //logSb.Clear();
                        }
                        rdi.Memo = logSb.ToString();
                        rdi.Ip = cpuid;
                        ///可以处理成绩
                        rdi.Result = MaxScore;
                        //string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);

                        #region 查询文件

                        //成绩根目录
                        Dictionary<string, string> dic_images = new Dictionary<string, string>();
                        Dictionary<string, string> dic_viedos = new Dictionary<string, string>();
                        Dictionary<string, string> dic_texts = new Dictionary<string, string>();
                        string scoreRoot = Application.StartupPath + $"\\Scores\\{sportProjectInfos.Name}\\{stu.GroupName}\\";
                        DateTime.TryParse(examTime, out DateTime examTime_dt);
                        string dateStr = examTime_dt.ToString("yyyyMMdd");
                        string GroupNo = $"{dateStr}_{stu.GroupName}_{stu.IdNumber}_1";

                        if (Directory.Exists(scoreRoot))
                        {
                            List<DirectoryInfo> rootDirs = new DirectoryInfo(scoreRoot).GetDirectories().ToList();
                            string dirEndWith = $"_{stu.IdNumber}_{stu.Name}";
                            DirectoryInfo directoryInfo = rootDirs.Find(a => a.Name.EndsWith(dirEndWith));
                            if (directoryInfo != null)
                            {
                                string stuDir = Path.Combine(scoreRoot, directoryInfo.Name);
                                GroupNo = $"{dateStr}_{stu.GroupName}_{directoryInfo.Name}_1";
                                if (Directory.Exists(stuDir))
                                {
                                    int step = 1;
                                    FileInfo[] files = new DirectoryInfo(stuDir).GetFiles("*.jpg");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_images.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.txt");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_texts.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                    step = 1;
                                    files = new DirectoryInfo(stuDir).GetFiles("*.mp4");
                                    if (files.Length > 0)
                                    {
                                        foreach (var item in files)
                                        {
                                            dic_viedos.Add(step + "", item.Name);
                                            step++;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion 查询文件

                        rdi.GroupNo = GroupNo;
                        rdi.Text = dic_texts;
                        rdi.Images = dic_images;
                        rdi.Videos = dic_viedos;
                        roundsItems.Add(rdi);
                        SudentsItem ssi = new SudentsItem();
                        ssi.SchoolName = stu.SchoolName;
                        ssi.GradeName = stu.GradeName;
                        ssi.ClassNumber = stu.ClassNumber;
                        ssi.Name = stu.Name;
                        ssi.IdNumber = stu.IdNumber;
                        ssi.Rounds = roundsItems;
                        sudentsItems.Add(ssi);
                        map.Add(stu.IdNumber, stu.Id.ToString());
                    }
                    if (sudentsItems.Count == 0) continue;
                    urrp.Sudents = sudentsItems;

                    //序列化json
                    string JsonStr = JsonConvert.SerializeObject(urrp);
                    string url = localInfos["Platform"] + RequestUrl.UploadResults;

                    var httpUpload = new HttpUpload();
                    var formDatas = new List<FormItemModel>();
                    //添加其他字段
                    formDatas.Add(new FormItemModel()
                    {
                        Key = "data",
                        Value = JsonStr
                    });

                    logWirte.AppendLine();
                    logWirte.AppendLine();
                    logWirte.AppendLine(JsonStr);

                    //上传学生成绩
                    string result = HttpUpload.PostForm(url, formDatas);
                    upload_Result upload_Result = JsonConvert.DeserializeObject<upload_Result>(result);
                    string errorStr = "null";
                    List<Dictionary<string, int>> result1 = upload_Result.Result;
                    foreach (var item in sudentsItems)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        //map
                        dic.Add("Id", map[item.IdNumber]);
                        dic.Add("IdNumber", item.IdNumber);
                        dic.Add("Name", item.Name);
                        dic.Add("uploadGroup", item.Rounds[0].GroupNo);
                        var value = 0;
                        result1.Find(a => a.TryGetValue(item.IdNumber, out value));
                        if (value == 1 || value == -4)
                        {
                            successList.Add(dic);
                        }
                        else if (value != 0)
                        {
                            errorStr = uploadResult.Match(value);
                            dic.Add("error", errorStr);
                            errorList.Add(dic);
                            messageSb.AppendLine($"{gInfo.Name}组 考号:{item.IdNumber} 姓名:{item.Name}上传失败,错误内容:{errorStr}");
                        }
                    }

                    #region 失败写入日志

                    string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                    if (!Directory.Exists(txtpath))
                    {
                        Directory.CreateDirectory(txtpath);
                    }
                    StringBuilder errorsb = new StringBuilder();
                    errorsb.AppendLine($"失败:{errorList.Count}");
                    errorsb.AppendLine("****************error***********************");
                    foreach (var item in errorList)
                    {
                        errorsb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]} 错误:{item["error"]}");
                    }
                    errorsb.AppendLine("*****************error**********************");
                    if (errorList.Count != 0)
                    {
                        string txtpath1 = Path.Combine(txtpath, $"error_{gInfo.Name}_upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                        File.WriteAllText(txtpath1, errorsb.ToString());
                        errorList.Clear();
                    }

                    #endregion 失败写入日志
                }

                #region 成功写入日志

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"成功:{successList.Count}");
                sb.AppendLine("******************success*********************");
                foreach (var item in successList)
                {
                    fsql.Update<DbPersonInfos>().Set(a => a.uploadGroup == "1").Where(a => a.Id == Convert.ToInt32(item["Id"])).ExecuteAffrows();
                    fsql.Update<ResultInfos>().Set(a => a.uploadState == 1).Where(a => a.PersonId == item["Id"]).ExecuteAffrows(); ;
                    sb.AppendLine($"考号:{item["IdNumber"]} 姓名:{item["Name"]}");
                }

                sb.AppendLine("*******************success********************");

                if (successList.Count != 0)
                {
                    string txtpath = Application.StartupPath + $"\\Log\\upload\\";
                    txtpath = Path.Combine(txtpath, $"upload_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt");
                    File.WriteAllText(txtpath, sb.ToString());
                    successList.Clear();
                }

                #endregion 成功写入日志

                LoggerHelper.Monitor(logWirte.ToString());
                string outpitMessage = messageSb.ToString();
                if (!string.IsNullOrEmpty(outpitMessage))
                    MessageBox.Show(outpitMessage);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
            finally
            {
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    UploadGroupsBtn.Text = "上传本组";
                    UploadGroupsBtn.ForeColor = Color.Black;
                    Update_listView();
                });

                try
                {
                    //importpath=string.Empty;
                    txProcess.Invoke((EventHandler)delegate { txProcess.Close(); });
                    txProcess.Dispose();
                }
                catch (Exception)
                { }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string groupName = groupsCbx.Text;
            new Thread((ThreadStart)delegate
            {
                printScore(groupName);
            }).Start();
        }

        /// <summary>
        /// 打印函数
        /// </summary>
        /// <param name="groupName"></param>
        private void printScore(string groupName)
        {
            try
            {
                if (string.IsNullOrEmpty(groupName)) throw new Exception("未选择组");
                string path = Application.StartupPath + "\\Data\\PrintExcel\\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                path = Path.Combine(path, $"PrintExcel_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
                List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.GroupName == groupName).ToList();
                List<OutPutPrintExcelData> outPutExcelDataList = new List<OutPutPrintExcelData>();
                int step = 1;
                bool isBestScore = false;
                if (sportProjectInfos.BestScoreMode == 0) isBestScore = true;
                foreach (var dpInfo in dbPersonInfos)
                {
                    List<ResultInfos> resultInfos = fsql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                    OutPutPrintExcelData opd = new OutPutPrintExcelData();
                    opd.Id = step;
                    opd.examTime = dpInfo.CreateTime;
                    opd.School = dpInfo.SchoolName;

                    opd.Name = dpInfo.Name;
                    opd.Sex = dpInfo.Sex == 0 ? "男" : "女";
                    opd.IdNumber = dpInfo.IdNumber;
                    opd.GroupName = dpInfo.GroupName;
                    int state = 0;
                    double MaxScore = 99999;
                    if (isBestScore) MaxScore = 0;
                    foreach (var ri in resultInfos)
                    {
                        ///异常状态
                        if (ri.State != 1)
                        {
                            if (isBestScore && MaxScore <= 0)
                            {
                                //取最大值
                                MaxScore = 0;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore >= 99999)
                            {
                                //取最小值
                                MaxScore = 99999;
                                state = ri.State;
                            }
                        }
                        else if (ri.State > 0)
                        {
                            if (isBestScore && MaxScore < ri.Result)
                            {
                                //取最大值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                            else if (!isBestScore && MaxScore > ri.Result)
                            {
                                //取最小值
                                MaxScore = ri.Result;
                                state = ri.State;
                            }
                        }
                    }
                    if (state < 0) continue;
                    if (state != 1)
                    {
                        MaxScore = 0;
                        opd.Result = ResultStateType.Match(state);
                    }
                    else
                    {
                        opd.Result = MaxScore.ToString();
                    }
                    outPutExcelDataList.Add(opd);
                    step++;
                }
                //result = ExcelUtils.OutPutExcel(ldic, path);
                MiniExcel.SaveAs(path, outPutExcelDataList);
                if (File.Exists(path))
                {
                    try
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.FileName = path;
                        p.StartInfo.Verb = "print";
                        p.Start();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("打印机异常");
                    }
                }
                else
                {
                    throw new Exception("导出失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendInfoBtn_Click(object sender, EventArgs e)
        {
            voiceOut0($"{raceStudentData.name}准备考试");
            SendScore($"{raceStudentData.name}", "准备考试", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeOutTB_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(timeOutTB.Text, out ballFallingInterval);
            if (ballFallingInterval == 0)
            {
                ballFallingInterval = 500;
                timeOutTB.Text = ballFallingInterval + "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toleranceTxt_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(toleranceTxt.Text, out tolerance);
            if (tolerance == 0)
            {
                tolerance = 30;
                toleranceTxt.Text = tolerance + "";
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thresholdTxt_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(thresholdTxt.Text, out threshold);
            if (threshold == 0)
            {
                thresholdTxt.Text = threshold + "";
                threshold = 2000;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (BeginRecTimeOut > 0) BeginRecTimeOut--;
            double v = MemoryTool.GetProcessUsedMemory();
            if (v > 100)
            {
                MemoryTool.ClearMemory();
            }

            frameSpeed_txt.Text = "fps:" + frameRecSum;
            frameRecSum = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            ballFallingIntervalStep++;
            if (recTimeR0 > 0)
            {
                recTimeR0--;
                if (recTimeR0 == 0)
                {
                    try
                    {
                        rgbVideoSourceStop();
                        ControlHelper.ThreadInvokerControl(this, () =>
                        {
                            pictureBox2.Image = Image.FromFile("./Image/1.png");
                            examStuState.Text = "已测试";
                            examStuScore.Text = ScoreSum.ToString();
                            timeLabel.Text = "00:00";
                        });
                        ReleaseVideoOutPut();
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                    }

                    try
                    {
                        if (string.IsNullOrEmpty(raceStudentData.idNumber))
                        {
                            MessageBox.Show("请分配考生");
                            return;
                        }
                        if (AutoSaveCheck.Checked)
                        {
                            WriteScoreIntoDb();
                            ClearMatchStudent();
                            AutoNext();
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.Debug(ex);
                    }
                }
                else
                {
                    int Reci = (recTimeR0 + 10) / 10;
                    recTime.Text = "REC " + Reci;
                    TimeSpan ts = TimeSpan.FromSeconds(Reci);
                    timeLabel.Text = $"{ts.Minutes.ToString().PadLeft(2, '0')}:{ts.Seconds.ToString().PadLeft(2, '0')}";
                }
            }
        }

        private void RunningWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                BeginRec();
                e.Handled = true;
            }
        }

        private void RunningWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            BarCode.Stop();
            CloseCamera();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openSerialPortBtn_Click(object sender, EventArgs e)
        {
            ConnectPort();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pidtxt_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(strMainModule, "pidtxt.dat"), pidtxt.Text);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vidtxt_TextChanged(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(strMainModule, "vidtxt.dat"), vidtxt.Text);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                string code = "readCh?";
                byte[] readCh = Encoding.UTF8.GetBytes(code);
                sReader.SendMessage(readCh);
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                string id = textBox1.Text;
                string code = "setCh" + id;
                byte[] readCh = Encoding.UTF8.GetBytes(code);
                sReader.SendMessage(readCh);
            });
        }

        private void RunningWindow_Resize(object sender, EventArgs e)
        {
            AutoSizeWindow?.ReWinformLayout(this);
        }

        #endregion 页面事件
    }
}