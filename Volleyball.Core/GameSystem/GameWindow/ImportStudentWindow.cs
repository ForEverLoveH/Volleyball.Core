using HZH_Controls;
using HZH_Controls.Forms;
using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class ImportStudentWindow : Form
    {
        public ImportStudentWindow()
        {
            InitializeComponent();
        }

        public Dictionary<string, string> localValues = new Dictionary<string, string>();
        public IFreeSql fsql = DbFreesql.Sqlite;

        private AutoSizeWindow AutoSizeWindow = null;

        private string importpath = string.Empty;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportStudentWindow_Load(object sender, EventArgs e)
        {
            InitListViewHead();
            AutoSizeWindow = new AutoSizeWindow(this.Width, this.Height);
            AutoSizeWindow.SetTag(this);
        }

        /// <summary>
        ///
        /// </summary>
        private void InitListViewHead()
        {
            listView1.View = View.Details;
            ColumnHeader[] Header = new ColumnHeader[100];
            int sp = 0;
            Header[sp] = new ColumnHeader();
            Header[sp].Text = "序号";
            Header[sp].Width = 40;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "时间";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "学校";
            Header[sp].Width = 220;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "年级";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "班级";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "准考证号";
            Header[sp].Width = 200;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "姓名";
            Header[sp].Width = 150;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "性别";
            Header[sp].Width = 80;
            sp++;

            Header[sp] = new ColumnHeader();
            Header[sp].Text = "组别名称";
            Header[sp].Width = 150;
            sp++;

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
        private void uiButton1_Click(object sender, EventArgs e)
        {
            PlatFormSettingWindow platFormSettingWindow = new PlatFormSettingWindow();
            platFormSettingWindow.ShowDialog();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton2_Click(object sender, EventArgs e)
        {
            string num = uiTextBox1.Text.Trim();
            if (string.IsNullOrEmpty(num))
            {
                UIMessageBox.ShowWarning("请输入你需要导入的数目！！");
                return;
            }
            downloadData(num);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupNums"></param>
        private void downloadData(string groupNums)
        {
            TxProcessRollForm txProcess = new TxProcessRollForm();

            try
            {
                new Thread((ThreadStart)delegate
                {
                    txProcess.ShowDialog();
                }).Start();
                List<LocalInfos> localInfos = fsql.Select<LocalInfos>().ToList();
                localValues = new Dictionary<string, string>();
                foreach (var li in localInfos)
                {
                    localValues.Add(li.key, li.value);
                }
                RequestParameter RequestParameter = new RequestParameter();
                RequestParameter.AdminUserName = localValues["AdminUserName"];
                RequestParameter.TestManUserName = localValues["TestManUserName"];
                RequestParameter.TestManPassword = localValues["TestManPassword"];
                string ExamId0 = localValues["ExamId"];
                ExamId0 = ExamId0.Substring(ExamId0.IndexOf('_') + 1);
                string MachineCode0 = localValues["MachineCode"];
                MachineCode0 = MachineCode0.Substring(MachineCode0.IndexOf('_') + 1);
                RequestParameter.ExamId = ExamId0;
                RequestParameter.MachineCode = MachineCode0;
                RequestParameter.GroupNums = groupNums + "";
                //序列化
                string JsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(RequestParameter);
                string url = localValues["Platform"] + RequestUrl.GetGroupStudentUrl;
                var formDatas = new List<FormItemModel>();
                //添加其他字段
                formDatas.Add(new FormItemModel()
                {
                    Key = "data",
                    Value = JsonStr
                });
                var httpUpload = new HttpUpload();
                string result = string.Empty;
                try
                {
                    result = HttpUpload.PostForm(url, formDatas);
                }
                catch (Exception ex)
                {
                    FrmTips.ShowTipsError(this, "请检查网络");
                }
                //GetGroupStudent upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                string[] strs = GetGroupStudent.CheckJson(result);
                GetGroupStudent upload_Result = null;
                if (strs[0] == "1")
                {
                    upload_Result = JsonConvert.DeserializeObject<GetGroupStudent>(result);
                }
                else
                {
                    upload_Result = new GetGroupStudent();
                    upload_Result.Error = strs[1];
                }
                bool bFlag = false;
                if (upload_Result == null || upload_Result.Results == null || upload_Result.Results.groups.Count == 0)
                {
                    string error = string.Empty;
                    try
                    { error = upload_Result.Error; }
                    catch (Exception)
                    { error = string.Empty; }
                    FrmTips.ShowTipsError(this, $"提交错误,错误码:[{error}]");
                }
                else
                {
                    bFlag = true;
                }

                if (bFlag)
                {
                    downlistOutputExcel1(upload_Result);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
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
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="upload_Result"></param>
        private void downlistOutputExcel1(GetGroupStudent upload_Result)
        {
            List<GroupsItem> Groups = upload_Result.Results.groups;
            List<InputData> doc = new List<InputData>();
            int step = 1;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            //序号	学校	 年级	班级 	姓名	 性别	准考证号	 组别名称
            foreach (var Group in Groups)
            {
                string groupId = Group.GroupId;
                string groupName = Group.GroupName;
                foreach (var StudentInfo in Group.StudentInfos)
                {
                    InputData idata = new InputData();
                    idata.Id = step;
                    idata.examTime = StudentInfo.examTime;
                    idata.School = StudentInfo.SchoolName;
                    idata.GradeName = StudentInfo.GradeName;
                    idata.ClassName = StudentInfo.ClassName;
                    idata.Name = StudentInfo.Name;
                    idata.Sex = StudentInfo.Sex;
                    idata.IdNumber = StudentInfo.IdNumber;
                    idata.GroupName = groupId;

                    ListViewItem li = new ListViewItem();
                    li.Text = step.ToString();
                    li.SubItems.Add(StudentInfo.examTime);
                    li.SubItems.Add(StudentInfo.SchoolName);
                    li.SubItems.Add(StudentInfo.GradeName);
                    li.SubItems.Add(StudentInfo.ClassName);
                    li.SubItems.Add(StudentInfo.IdNumber);
                    li.SubItems.Add(StudentInfo.Name);
                    li.SubItems.Add(StudentInfo.Sex);
                    li.SubItems.Add(groupId);
                    listView1.Items.Insert(listView1.Items.Count, li);
                    doc.Add(idata);
                    step++;
                }
            }
            listView1.EndUpdate();
            importpath = Application.StartupPath + $"\\模板\\下载名单\\";
            if (!Directory.Exists(importpath))
            {
                Directory.CreateDirectory(importpath);
            }
            importpath = Path.Combine(importpath, $"downList{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
            MiniExcel.SaveAs(importpath, doc);
            //MessageBox.Show("下载完成");
            ControlHelper.ThreadInvokerControl(this, () =>
            {
                msglabel.Text = $"下载名单成功,共{step - 1}人";
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton3_Click(object sender, EventArgs e)
        {
            uiButton3.Enabled = false;
            string path = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;      //该值确定是否可以选择多个文件
            dialog.Title = "请选择文件";     //弹窗的标题
            dialog.InitialDirectory = Application.StartupPath + "\\";    //默认打开的文件夹的位置
            dialog.Filter = "MicroSoft Excel文件(*.xlsx)|*.xlsx";       //筛选文件
            dialog.ShowHelp = true;     //是否显示“帮助”按钮
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
            }
            uiTextBox2.Text = path;
            importpath = path;
            if (!string.IsNullOrWhiteSpace(uiTextBox2.Text.Trim()))
            {
                var sl = MiniExcel.GetSheetNames(path);
                uiComboBox1.Items.Clear();
                foreach (var s in sl)
                {
                    uiComboBox1.Items.Add(s);
                }
                if (uiComboBox1.Items.Count > 0)
                {
                    uiComboBox1.SelectedIndex = 0;
                }
            }
            var rows = MiniExcel.Query<InputData>(path, uiComboBox1.Text.Trim()).ToList();
            if (rows.Count > 0)
            {
                listView1.BeginUpdate();
                listView1.Items.Clear();
                int step = 1;
                foreach (var row in rows)
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = step.ToString();
                    li.SubItems.Add(row.examTime);
                    li.SubItems.Add(row.School);
                    li.SubItems.Add(row.GradeName);
                    li.SubItems.Add(row.ClassName);
                    li.SubItems.Add(row.IdNumber);
                    li.SubItems.Add(row.Name);
                    li.SubItems.Add(row.Sex);
                    li.SubItems.Add(row.GroupName);
                    listView1.Items.Insert(listView1.Items.Count, li);

                    step++;
                }
                listView1.EndUpdate();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Filter = "xls files(*.xls)|*.xls|xlsx file(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            saveImageDialog.RestoreDirectory = true;
            saveImageDialog.FileName = $"导入模板{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            string path = Application.StartupPath + "\\excel\\output.xlsx";

            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                path = saveImageDialog.FileName;
                File.Copy(@"./模板/导入名单模板.xlsx", path);
                FrmTips.ShowTipsSuccess(this, "导出成功");
            }
        }

        private bool isDeleteBeforeImport = false;

        private void uiButton5_Click(object sender, EventArgs e)
        {
            isDeleteBeforeImport = true;
            if (string.IsNullOrEmpty(importpath))
            {
                MessageBox.Show("路径错误");
                return;
            }
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ExcelListInput);
            Thread t = new Thread(ParStart);
            t.IsBackground = true;
            t.Start(importpath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton6_Click(object sender, EventArgs e)
        {
            isDeleteBeforeImport = false;
            if (string.IsNullOrEmpty(importpath))
            {
                MessageBox.Show("路径错误");
                return;
            }
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ExcelListInput);
            Thread t = new Thread(ParStart);
            t.IsBackground = true;
            t.Start(importpath);
        }

        /// <summary>
        /// 本地xlsx导入数据
        /// </summary>
        /// <param name="obj"></param>
        private void ExcelListInput(object obj)
        {
            bool IsResult = false;
            TxProcessRollForm txProcess = new TxProcessRollForm();

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //全新导入
                if (isDeleteBeforeImport)
                {
                    string[] deleteTableNames = new string[] { "DbGroupInfos", "DbPersonInfos", "ResultInfos", "LogInfos" };
                    int result1 = fsql.Delete<DbGroupInfos>().Where("1=1").ExecuteAffrows();
                    int result2 = fsql.Delete<DbPersonInfos>().Where("1=1").ExecuteAffrows();
                    int result3 = fsql.Delete<ResultInfos>().Where("1=1").ExecuteAffrows();
                    int result4 = fsql.Update<LogInfos>().Set(a => a.State == -404).Where("1=1").ExecuteAffrows();
                }
                string path = obj as string;
                if (!string.IsNullOrEmpty(path))
                {
                    new Thread((ThreadStart)delegate
                    {
                        txProcess.ShowDialog();
                    }).Start();
                    SportProjectInfos sportProjectInfo = fsql.Select<SportProjectInfos>().ToOne();
                    var rows = MiniExcel.Query<InputData>(path).ToList();
                    ///序号 学校 年纪 班级 姓名 性别 准考证号 组别名称
                    HashSet<String> set = new HashSet<String>();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        string[] examTime = rows[i].examTime.Split(' ');
                        set.Add(rows[i].GroupName + "#" + examTime[0]);
                    }
                    List<String> rolesMarketList = new List<string>();
                    rolesMarketList.AddRange(set);
                    fsql.Select<DbGroupInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                    //var repo = fsql.GetRepository<DbGroupInfos>();
                    List<DbGroupInfos> insertDbGroupInfosList = new List<DbGroupInfos>();
                    for (int i = 0; i < rolesMarketList.Count; i++)
                    {
                        maxSortId++;
                        string role = rolesMarketList[i];
                        string[] roles = role.Split('#');
                        string GroupName = roles[0];
                        string examTime = roles[1];
                        DbGroupInfos dbGroup = new DbGroupInfos();
                        dbGroup.CreateTime = examTime;
                        dbGroup.SortId = maxSortId;
                        dbGroup.IsRemoved = 0;
                        dbGroup.ProjectId = "0";
                        dbGroup.Name = GroupName;
                        dbGroup.IsAllTested = 0;
                        insertDbGroupInfosList.Add(dbGroup);
                        /* repo.InsertOrUpdate(new DbGroupInfos()
                         {
                            CreateTime = examTime,
                            SortId = maxSortId,
                            IsRemoved = 0,
                            ProjectId = "0",
                            Name = GroupName,
                            IsAllTested = 0
                         });*/
                    }
                    //实际插入数
                    int result0 = fsql.InsertOrUpdate<DbGroupInfos>()
                        .SetSource(insertDbGroupInfosList)
                        .IfExistsDoNothing()
                        .ExecuteAffrows();
                    fsql.Select<DbPersonInfos>().Aggregate(x => x.Max(x.Key.SortId), out maxSortId);
                    List<DbPersonInfos> insertDbPersonInfosList = new List<DbPersonInfos>();
                    foreach (var row in rows)
                    {
                        maxSortId++;
                        string PersonIdNumber = row.IdNumber;
                        string name = row.Name;
                        int Sex = row.Sex == "男" ? 0 : 1;
                        string SchoolName = row.School;
                        string GradeName = row.GradeName;
                        string classNumber = row.ClassName;
                        string GroupName = row.GroupName;
                        string[] examTimes = row.examTime.Split(' ');
                        string examTime = examTimes[0];
                        DbPersonInfos dbPersonInfos = new DbPersonInfos();
                        dbPersonInfos.CreateTime = examTime;
                        dbPersonInfos.SortId = maxSortId;
                        dbPersonInfos.ProjectId = "0";
                        dbPersonInfos.SchoolName = SchoolName;
                        dbPersonInfos.GradeName = GradeName;
                        dbPersonInfos.ClassNumber = classNumber;
                        dbPersonInfos.GroupName = GroupName;
                        dbPersonInfos.Name = name;
                        dbPersonInfos.IdNumber = PersonIdNumber;
                        dbPersonInfos.Sex = Sex;
                        dbPersonInfos.State = 0;
                        dbPersonInfos.FinalScore = -1;
                        dbPersonInfos.uploadState = 0;
                        insertDbPersonInfosList.Add(dbPersonInfos);
                    }
                    //实际插入数
                    int result = fsql.InsertOrUpdate<DbPersonInfos>()
                        .SetSource(insertDbPersonInfosList)
                        .IfExistsDoNothing()
                        .ExecuteAffrows();

                    if (result == 0) IsResult = false;
                    else IsResult = true;
                    sw.Stop();
                    string time = (sw.ElapsedMilliseconds / 1000).ToString("0.000") + "秒";
                    ControlHelper.ThreadInvokerControl(this, () =>
                    {
                        msglabel.Text = $"耗时：{time},实际插入:{result},重复：{rows.Count - result}";
                    });
                    try
                    {
                        txProcess.Invoke((EventHandler)delegate { txProcess.Close(); });
                    }
                    catch (Exception)
                    { }
                    MessageBox.Show(msglabel.Text);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
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
            }
            if (IsResult)
            {
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    //FrmTips.ShowTipsSuccess(this, "导入成功");
                    DialogResult = DialogResult.OK;
                });
            }
            else
            {
                ControlHelper.ThreadInvokerControl(this, () =>
                {
                    //FrmTips.ShowTipsSuccess(this, "导入失败");
                    DialogResult = DialogResult.Cancel;
                });
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportStudentWindow_Resize(object sender, EventArgs e)
        {
            AutoSizeWindow?.ReWinformLayout(this);
        }
    }
}