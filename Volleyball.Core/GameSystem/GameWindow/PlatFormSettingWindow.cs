using HZH_Controls.Forms;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class PlatFormSettingWindow : Form
    {
        public PlatFormSettingWindow()
        {
            InitializeComponent();
        }
        #region 参数
        public IFreeSql fsql = DbFreesql.Sqlite;
        public Dictionary<string, string> localValues;

        string MachineCode = String.Empty;
        string ExamId = String.Empty;
        string Platform = String.Empty;
        string Platforms = String.Empty;
        #endregion
        private void PlatFormSettingWindow_Load(object sender, EventArgs e)
        {
            List<LocalInfos> localInfos = fsql.Select<LocalInfos>().ToList();
            localValues = new Dictionary<string, string>();
            foreach (var li in localInfos)
            {
                localValues.Add(li.key, li.value);
                switch (li.key)
                {
                    case "MachineCode":
                        MachineCode = li.value;
                        break;
                    case "ExamId":
                        ExamId = li.value;
                        break;
                    case "Platform":
                        Platform = li.value;
                        break;
                    case "Platforms":
                        Platforms = li.value;
                        break;
                }
            }
            if (string.IsNullOrEmpty(MachineCode))
            {
                UIMessageBox.ShowWarning( "设备码为空");
            }
            else
            {
                comboBox1.Text = MachineCode;
            }

            if (string.IsNullOrEmpty(ExamId))
            {
                UIMessageBox.ShowWarning( "考试id为空");
            }
            else
            {
                comboBox3.Text = ExamId;
            }
            if (string.IsNullOrEmpty(Platforms))
            {
                UIMessageBox.ShowWarning( "平台码为空");
            }
            else
            {
                string[] Platformss = Platforms.Split(';');
                comboBox2.Items.Clear();
                foreach (var item in Platformss)
                {
                    comboBox2.Items.Add(item);
                }

            }
            if (string.IsNullOrEmpty(Platform))
            {
                UIMessageBox.ShowWarning( "平台码为空");
            }
            else
            {
                comboBox2.Text = Platform;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox3.Items.Clear();
                string url = comboBox2.Text;
                if (string.IsNullOrEmpty(url))
                {
                    FrmTips.ShowTipsError(this, "网址为空!");
                    return;
                }
                url += RequestUrl.GetExamListUrl;
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });
                var httpUpload = new HttpUpload();
                string result = String.Empty;
                try
                {
                    result = HttpUpload.PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    throw new Exception("请检查网络");
                }
                GetExamList upload_Result = JsonConvert.DeserializeObject<GetExamList>(result);

                if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.Count == 0)
                {
                    string error = string.Empty;
                    try
                    {
                        error = upload_Result.Error;

                    }
                    catch (Exception)
                    {

                        error = string.Empty;
                    }
                    UIMessageBox.ShowWarning( $"提交错误,错误码:[{error}]");
                    return;
                }

                foreach (var item in upload_Result.Results)
                {
                    string str = $"{item.title}_{item.exam_id}";
                    comboBox3.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Items.Clear();
                string examId = comboBox3.Text;
                if (string.IsNullOrEmpty(examId))
                {
                    UIMessageBox.ShowWarning("考试id为空!");
                    return;
                }
                if (examId.IndexOf('_') != -1)
                {
                    examId = examId.Substring(examId.IndexOf('_') + 1);
                }
                string url = comboBox2.Text;
                if (string.IsNullOrEmpty(url))
                {
                    UIMessageBox.ShowWarning("网址为空!");
                    return;
                }
                url += RequestUrl.GetMachineCodeListUrl;
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                RequestParameter.ExamId = examId;
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });
                var httpUpload = new HttpUpload();
                string result = String.Empty;
                try
                {
                    result = HttpUpload.PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    throw new Exception("请检查网络");
                }
                GetMachineCodeList upload_Result = JsonConvert.DeserializeObject<GetMachineCodeList>(result);
                if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.Count == 0)
                {
                    string error = string.Empty;
                    try
                    {
                        error = upload_Result.Error;
                    }
                    catch (Exception)
                    {
                        error = string.Empty;
                    }
                    UIMessageBox.ShowWarning($"提交错误,错误码:[{error}]");
                    return;
                }

                foreach (var item in upload_Result.Results)
                {
                    string str = $"{item.title}_{item.MachineCode}";
                    comboBox1.Items.Add(str);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string Platform = comboBox2.Text;
                string ExamId = comboBox3.Text;
                string MachineCode = comboBox1.Text;
                int sum = 0;
                int result = fsql.Update<LocalInfos>().Set(a => a.value == Platform).Where(a => a.key == "Platform").ExecuteAffrows();
                sum += result;
                result = fsql.Update<LocalInfos>().Set(a => a.value == ExamId).Where(a => a.key == "ExamId").ExecuteAffrows();
                sum += result;
                result = fsql.Update<LocalInfos>().Set(a => a.value == MachineCode).Where(a => a.key == "MachineCode").ExecuteAffrows();
                sum += result;
                if (sum == 3)
                {
                    UIMessageBox.ShowSuccess( "保存成功");
                    this.Close();
                }
                else
                {
                    UIMessageBox.ShowWarning( "更新失败");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                UIMessageBox.ShowWarning("更新失败");
            }
        }
    }
}
