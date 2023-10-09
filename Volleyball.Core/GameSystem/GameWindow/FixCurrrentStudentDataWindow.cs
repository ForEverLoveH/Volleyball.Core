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
using System.Threading.Tasks;
using System.Windows.Forms;
using Volleyball.Core.GameSystem.GameHelper;
using Volleyball.Core.GameSystem.GameModel;

namespace Volleyball.Core.GameSystem.GameWindow
{
    public partial class FixCurrrentStudentDataWindow : Form
    {
        public FixCurrrentStudentDataWindow()
        {
            InitializeComponent();
        }

        //mode 0 轮次清空
        //mode 1 轮次修改成绩
        public int mode = -1;

        public IFreeSql fsql = DbFreesql.Sqlite;
        public SportProjectInfos sportProjectInfos = null;
        public DbPersonInfos dbPersonInfos = null;
        public string _idnumber = string.Empty;

        private void FixCurrrentStudentDataWindow_Load(object sender, EventArgs e)
        {
            if (mode == 0) this.uiTitlePanel1.Text = "轮次清空";
            else if (mode == 1) this.uiTitlePanel1.Text = "修正成绩";
            if (!string.IsNullOrEmpty(_idnumber))
            {
                dbPersonInfos = fsql.Select<DbPersonInfos>().Where(a => a.IdNumber == _idnumber).ToOne();
            }
            sportProjectInfos = fsql.Select<SportProjectInfos>().ToOne();
            if (sportProjectInfos != null)
            {
                int roundTotal = sportProjectInfos.RoundCount;
                for (int i = 0; i < roundTotal; i++)
                {
                    comboBox1.Items.Add($"第{i + 1}轮");
                }
                if (roundTotal > 0) comboBox1.SelectedIndex = 0;
            }
            if (dbPersonInfos != null)
            {
                textBox1.Text = dbPersonInfos.IdNumber;
                textBox2.Text = dbPersonInfos.Name;
            }
        }

        private bool isNoExam = false;
        private string oldScore = "";

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int roundid = comboBox1.SelectedIndex + 1;
            if (roundid <= 0) return;
            List<ResultInfos> resultInfos = fsql.Select<ResultInfos>()
               .Where(a => a.PersonIdNumber == _idnumber)
               .Where(a => a.RoundId == roundid)
               .Where(a => a.IsRemoved == 0)
               .ToList();
            isNoExam = false;
            textBox3.Text = "";
            if (resultInfos.Count == 0)
            {
                isNoExam = true;
                MessageBox.Show("该学生本轮未参加考试");
                return;
            }
            foreach (var ri in resultInfos)
            {
                if (ri.IsRemoved == 0)
                {
                    textBox3.Text = ri.Result.ToString();
                    oldScore = ri.Result.ToString();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixCurrrentStudentDataWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (base.DialogResult == DialogResult.OK)
            {
                int roundid = comboBox1.SelectedIndex + 1;
                if (mode == 0)
                {
                    int result = fsql.Delete<ResultInfos>()
                       .Where(a => a.PersonIdNumber == _idnumber)
                       .Where(a => a.RoundId == roundid)
                       .ExecuteAffrows();
                    if (result == 1) Debug.WriteLine("删除成功");
                    if (result > 0)
                    {
                        string scoreContent = string.Format("时间:{0,-20},项目:{1,-20},组别:{2,-10},准考证号:{3,-20},姓名{4,-5},第{5}次成绩:{6,-5}, 状态:{7,-5}",
                                       DateTime.Now.ToString("yyyy年MM月dd日HH:mm:ss"),
                                       "排球垫球",
                                       dbPersonInfos.GroupName,
                                       dbPersonInfos.IdNumber,
                                       dbPersonInfos.Name,
                                       roundid,
                                       0,
                                       $"清空成绩{oldScore}");
                        File.AppendAllText(@"./操作日志.txt", scoreContent + "\n");
                    }
                }
                else if (mode == 1)
                {
                    double.TryParse(textBox3.Text, out double fhl);
                    int result = 0;

                    if (isNoExam)
                    {
                        List<ResultInfos> insertResults = new List<ResultInfos>();
                        fsql.Select<ResultInfos>().Aggregate(x => x.Max(x.Key.SortId), out int maxSortId);
                        maxSortId++;
                        ResultInfos rinfo = new ResultInfos();
                        rinfo.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        rinfo.SortId = maxSortId;
                        rinfo.PersonId = dbPersonInfos.Id.ToString();
                        rinfo.SportItemType = 0;
                        rinfo.PersonName = dbPersonInfos.Name;
                        rinfo.PersonIdNumber = dbPersonInfos.IdNumber;
                        rinfo.RoundId = roundid;
                        rinfo.State = 1;
                        rinfo.IsRemoved = 0;
                        rinfo.Result = fhl;
                        insertResults.Add(rinfo);

                        result = fsql.InsertOrUpdate<ResultInfos>().SetSource(insertResults).IfExistsDoNothing().ExecuteAffrows();
                        if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                    }
                    else
                    {
                        result = fsql.Update<ResultInfos>().Set(a => a.Result == fhl).Where(a => a.PersonIdNumber == _idnumber).Where(a => a.RoundId == roundid).Where(a => a.IsRemoved == 0).ExecuteAffrows();
                        if (result == 1) UIMessageBox.ShowSuccess("修改成功");
                    }

                    if (result == 1)
                    {
                        string scoreContent = string.Format("时间:{0,-20},项目:{1,-20},组别:{2,-10},准考证号:{3,-20},姓名{4,-5},第{5}次成绩:{6,-5}, 状态:{7,-5}",
                                       DateTime.Now.ToString("yyyy年MM月dd日HH:mm:ss"),
                                       "排球垫球",
                                       dbPersonInfos.GroupName,
                                       dbPersonInfos.IdNumber,
                                       dbPersonInfos.Name,
                                       roundid,
                                       fhl,
                                       $"修正成绩{oldScore}->{fhl}");
                        File.AppendAllText(@"./操作日志.txt", scoreContent + "\n");
                    }
                }
            }
        }
    }
}