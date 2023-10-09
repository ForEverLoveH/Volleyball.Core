using HZH_Controls;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class OutPutExcelScoreForm : Form
    {
        public OutPutExcelScoreForm()
        {
            InitializeComponent();
        }

        private bool isAllTest = true;
        private bool isOnlyGroup = false;
        public string _GroupName = string.Empty;
        public IFreeSql fsql = DbFreesql.Sqlite;
        private SportProjectInfos sportProjectInfos = null;

        private void OutPutExcelScoreForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_GroupName))
            {
                checkBox2.Checked = false;
                checkBox2.Visible = false;
                uiTextBox1.Text = "未选择组别";
            }
            else
            {
                checkBox2.Checked = true;
                checkBox2.Visible = true;
                uiTextBox1.Text = _GroupName;
            }
            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            isAllTest = checkBox1.Checked;
            isOnlyGroup = checkBox2.Checked;
            bool result = OutPutScore();
            if (result) DialogResult = DialogResult.OK;
            else DialogResult = DialogResult.No;
        }

        private bool OutPutScore()
        {
            bool result = false;
            string btn01Txt = string.Empty;
            TxProcessRollForm txProcess = new TxProcessRollForm();

            ControlHelper.ThreadInvokerControl(this, () =>
            {
                uiButton1.Enabled = false;
            });
            try
            {
                if (sportProjectInfos == null)
                {
                    MessageBox.Show("项目设置有误");
                    return false;
                }
                SaveFileDialog saveImageDialog = new SaveFileDialog();
                saveImageDialog.Title = "导出成绩";
                saveImageDialog.Filter = "xlsx file(*.xlsx)|*.xlsx";
                saveImageDialog.RestoreDirectory = true;
                string path = Application.StartupPath + $"\\excel\\output{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                //saveImageDialog.FileName = $"output_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                if (isOnlyGroup)
                { saveImageDialog.FileName = $"{sportProjectInfos.Name}_{_GroupName}组成绩.xlsx"; }
                else
                { saveImageDialog.FileName = $"{sportProjectInfos.Name}_成绩.xlsx"; }
                if (saveImageDialog.ShowDialog() == DialogResult.OK)
                {
                    new Thread((ThreadStart)delegate
                    {
                        txProcess.ShowDialog();
                    }).Start();
                    path = saveImageDialog.FileName;
                    if (File.Exists(path)) File.Delete(path);
                    List<Dictionary<string, string>> ldic = new List<Dictionary<string, string>>();
                    //序号 项目名称    组别名称 姓名  准考证号 考试状态    第1轮 第2轮 最好成绩
                    List<DbPersonInfos> dbPersonInfos = new List<DbPersonInfos>();
                    if (isOnlyGroup)
                    {
                        dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.GroupName == _GroupName).ToList();
                    }
                    else
                    {
                        dbPersonInfos = fsql.Select<DbPersonInfos>().ToList();
                    }
                    List<outPutExcelData> outPutExcelDataList = new List<outPutExcelData>();
                    int step = 1;
                    bool isBestScore = false;
                    if (sportProjectInfos.BestScoreMode == 0) isBestScore = true;
                    foreach (var dpInfo in dbPersonInfos)
                    {
                        List<ResultInfos> resultInfos = fsql.Select<ResultInfos>().Where(a => a.PersonId == dpInfo.Id.ToString() && a.IsRemoved == 0).ToList();
                        if (resultInfos.Count == 0)
                        {
                            if (isAllTest) continue;
                        }
                        outPutExcelData opd = new outPutExcelData();
                        opd.Id = step;
                        opd.examTime = dpInfo.CreateTime;
                        opd.School = dpInfo.SchoolName;
                        opd.GradeName = dpInfo.GradeName;
                        opd.ClassName = dpInfo.ClassNumber;
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
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
            finally
            {
                try
                {
                    //importpath=string.Empty;
                    txProcess.Invoke((EventHandler)delegate { txProcess.Close(); });
                    txProcess.Dispose();
                }
                catch (Exception)
                { }
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    uiButton1.Enabled = true;
                });
            }
        }
    }
}