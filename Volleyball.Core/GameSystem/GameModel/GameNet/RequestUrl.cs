using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameModel
{
    /// <summary>
    /// 表单数据项
    /// </summary>
    public class FormItemModel
    {
        /// <summary>
        /// 表单键，request["key"]
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        /// 表单值,上传文件时忽略，request["key"].value
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// 是否是文件
        /// </summary>
        public bool IsFile
        {
            get
            {
                if (FileContent == null || FileContent.Length == 0)
                    return false;

                if (FileContent != null && FileContent.Length > 0 && string.IsNullOrWhiteSpace(FileName))
                    throw new Exception("上传文件时 FileName 属性值不能为空");
                return true;
            }
        }

        /// <summary>
        /// 上传的文件名
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// 上传的文件内容
        /// </summary>
        public Stream FileContent { set; get; }
    }

    public class RequestUrl
    {
        /// <summary>
        /// 设备端 获取体测学校列表
        /// </summary>
        public static string GetExamListUrl = "api/GetExamList/";

        //GetExamList
        /// <summary>
        /// 设备端 获取机器码
        /// </summary>
        public static string GetMachineCodeListUrl = "api/GetMachineCodeList/";

        /// <summary>
        /// 设备端 获取组列表
        /// </summary>
        public static string GetGroupsUrl = "api/GetGroup/";

        /// <summary>
        /// 设备端 获取学生列表
        /// </summary>
        public static string FetchStudentsUrl = "api/FetchStudents/";

        /// <summary>
        /// 设备端 按组数获取学生
        /// </summary>
        public static string GetGroupStudentUrl = "api/GetGroupStudent/";

        /// <summary>
        /// 上报成绩接口
        /// </summary>

        public static string UploadResults = "api/UploadResults/";
    }

    /// <summary>
    /// 获取机器码映射类
    /// </summary>
    public class GetMachineCodePojo
    {
        /// <summary>
        /// 考试id
        /// </summary>
        public string ExamId { get; set; }

        /// <summary>
        /// 管理员账号
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// 裁判员账号
        /// </summary>
        public string TestManUserName { get; set; }

        /// <summary>
        /// 裁判员密码
        /// </summary>
        public string TestManPassword { get; set; }

        public string IdNumber { get; set; }
    }

    public class FetchGroupListSend
    {
        public string MachineCode { get; set; }         //注册软件生成的机器码
        public string AdminUserName { get; set; }       //管理员账号
        public string TestManUserName { get; set; }     //裁判员账号
        public string TestManPassword { get; set; }     //裁判员密码
        public int GroupId { get; set; }                //拉取的组ID，从1到N的整数，取决于编排有多少组

        public int Page { get; set; } //页号
        public int PageSize { get; set; } //页内容数量
        public int All { get; set; }
    }

    public class UploadAttachmentSend
    {
        public string MachineCode { get; set; }             //注册软件生成的机器码
        public string AdminUserName { get; set; }           //管理员账号名称
        public string TestManUserName { get; set; }         //裁判员账号名称
        public string TestManPassword { get; set; }         //裁判员账号密码

        public string GroupNo { get; set; }                 //组名目录
    }

    /// <summary>
    /// 成绩上传的对象
    /// </summary>
    public class UploadResultsSend
    {
        public string MachineCode { get; set; }             //注册软件生成的机器码
        public string AdminUserName { get; set; }           //管理员账号名称
        public string TestManUserName { get; set; }         //裁判员账号名称
        public string TestManPassword { get; set; }         //裁判员账号密码

        public List<SingelStudentResult> Sudents { get; set; }      //学生成绩列表
    }

    /// <summary>
    /// 单个学生成绩
    /// </summary>
    public class SingelStudentResult
    {
        public string SchoolName { get; set; }              //学校名称
        public string GradeName { get; set; }               //年级名称
        public int ClassNumber { get; set; }                //班级名称
        public string Name { get; set; }                    //姓名
        public string IdNumber { get; set; }                //身份证号码
        public List<SingleRound> Rounds { get; set; }       //多轮次的成绩信息
    }

    /// <summary>
    /// 单轮成绩
    /// </summary>
    public class SingleRound
    {
        public int RoundId { get; set; }                    //轮次 ID
        public double Result { get; set; }                  //成绩数值 单位已经在服务器上面配置好了
        public string GroupNo { get; set; }
        public Dictionary<string, string> Text { get; set; }
        public Dictionary<string, string> Images { get; set; }
        public Dictionary<string, string> Videos { get; set; }
    }

    public class StudentInfos
    {
        /// <summary>
        /// 罗天勇
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 男
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 华远中学
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 初三
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 三（5）班
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassNumber { get; set; }

        public string groupId { get; set; }

        public string GroupIndex { get; set; }

        public string project { get; set; }

        public List<string> Projects { get; set; }

        public string dateTime { get; set; }

        public string groupName { get; set; }

        public string groupNumber { get; set; }

        public string Remark { get; set; }

        public int maxGroup { get; set; }

        //"编号", "姓名" , "性别", "单位", "地区", "年级", "班级", "班级号数", "组别", "项目", "日期", "组号", "顺序", "备注"
        /// <summary>
        /// 根据id获取值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string getValue(int index)
        {
            string result = "";
            switch (index)
            {
                case 0:
                    //编号
                    result = IdNumber;
                    break;

                case 1:
                    //姓名
                    result = Name;
                    break;

                case 2:
                    //性别
                    result = Sex;
                    break;

                case 3:
                    //单位
                    result = SchoolName;
                    break;

                case 4:
                    //地区
                    result = SchoolName;
                    break;

                case 5:
                    //年级
                    result = GradeName;
                    break;

                case 6:
                    //班级
                    result = ClassName;
                    break;

                case 7:
                    //班级号数
                    result = ClassNumber;
                    break;

                case 8:
                    //组别
                    groupName = string.IsNullOrEmpty(groupName) ? "考试组" : groupName;
                    result = groupName;
                    break;

                case 9:
                    //项目
                    if (Projects != null)
                    {
                        /*if (Projects.Contains(Common.sportTypeStringzh.Substring(0, Common.sportTypeStringzh.Length - 2)))
                        {
                            project = Common.sportTypeStringzh.Substring(0, Common.sportTypeStringzh.Length - 2);
                        }
                        else
                        {
                            project = "";
                        }*/
                    }
                    else
                    {
                        project = "";
                    }
                    result = project;
                    break;

                case 10:
                    //日期
                    dateTime = string.IsNullOrEmpty(dateTime) ? DateTime.Now.ToString("yyyy/MM/dd") : dateTime;
                    result = dateTime;
                    break;

                case 11:
                    //组号
                    result = (Convert.ToInt32(groupId) + maxGroup + 1).ToString();
                    break;

                case 12:
                    //顺序
                    //result = groupNumber;
                    result = GroupIndex;
                    break;

                case 13:
                    //备注
                    result = Remark;
                    if (Projects != null)
                    {
                        string result1 = "";
                        foreach (var item in Projects)
                        {
                            result1 = result1 + item + ",";
                        }
                        result = result1.Substring(0, result1.Length - 1);
                    }
                    break;
            }

            return result;
        }
    }

    /// <summary>
    /// 获取学生数据返回json实体类
    /// </summary>
    public class student_result
    {
        public int maxGroup { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<StudentInfos> StudentInfos { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Error { get; set; }
    }

    /// <summary>
    /// 上传成绩返回json实体类
    /// </summary>
    public class upload_Result
    {
        /// <summary>
        ///
        /// </summary>
        public List<Dictionary<string, int>> Result { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Error { get; set; }
    }

    public class uploadResult
    {
        public static string Match(int index)
        {
            string result = "";
            switch (index)
            {
                case 1:
                    result = "成功";
                    break;

                case -1:
                    result = "学生数据有误";
                    break;

                case -2:
                    result = "报项数据有误";
                    break;

                case -3:
                    result = "轮次数据有误";
                    break;

                case -4:
                    result = "轮次已经上报过了";
                    break;

                case -5:
                    result = "未检录";
                    break;

                default:
                    result = "未解析错误";
                    break;
            }
            return result;
        }
    }

    #region 请求提交参数

    /// <summary>
    /// 请求提交参数
    /// </summary>
    public class RequestParameter
    {
        /// <summary>
        /// 注册软件生成的机器码
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 管理员账号
        /// </summary>
        public string AdminUserName { get; set; }

        /// <summary>
        /// 裁判员账号
        /// </summary>
        public string TestManUserName { get; set; }

        /// <summary>
        /// 裁判员密码
        /// </summary>
        public string TestManPassword { get; set; }

        /// <summary>
        /// 考试id
        /// </summary>
        public string ExamId { get; set; }

        /// <summary>
        /// 组id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 准考证号
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 要下载的组数
        /// </summary>
        public string GroupNums { get; set; }
    }

    #region 体测学校列表

    /// <summary>
    /// 体测学校列表
    /// </summary>
    public class GetExamList
    {
        public List<GetExamListResults> Results { get; set; }

        public String Error { get; set; }
    }

    public class GetExamListResults
    {
        public String exam_id;

        public String title;
    }

    #endregion 体测学校列表

    #region 机器码列表

    /// <summary>
    /// 机器码列表
    /// </summary>
    public class GetMachineCodeList
    {
        public List<GetMachineCodeListResults> Results;

        public String Error;
    }

    public class GetMachineCodeListResults
    {
        public String title;

        public String MachineCode;
    }

    #endregion 机器码列表

    #region 获取组列表

    /// <summary>
    /// 获取组列表
    /// </summary>
    public class GetGroup
    {
        public GetGroupResults Results { get; set; }

        public String Error { get; set; }
    }

    public class GetGroupResults
    {
        public List<String> groups { get; set; }
    }

    #endregion 获取组列表

    #region 获取组学生列表

    /// <summary>
    /// 获取组学生列表
    /// </summary>
    public class FetchStudents
    {
        public List<FetchStudentsStudentInfos> StudentInfos;

        public String Error;
    }

    public class FetchStudentsStudentInfos
    {
        public String Name;

        public String IdNumber;

        public String Sex;

        public String GradeName;

        public String ClassName;

        public String GroupId;

        public String GroupIndex;

        public List<String> Projects;
    }

    #endregion 获取组学生列表

    #region 按组数获取学生

    /// <summary>
    /// 按组数获取学生
    /// </summary>
    public class GetGroupStudent
    {
        /// <summary>
        ///
        /// </summary>
        public Results Results { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Error { get; set; }

        public static string[] CheckJson(string json)
        {
            string[] strs = new string[2];
            string ResultISNull = "0";
            string ResultError = "";
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var jsObject = JObject.Parse(json);
                    foreach (JToken child in jsObject.Children())
                    {
                        var property1 = child as JProperty;
                        if (property1.Name == "Error")
                        {
                            if (string.IsNullOrEmpty(property1.Value.ToString()))
                            {
                                ResultISNull = "1";
                            }
                            else
                            {
                                ResultISNull = "0";
                                ResultError = property1.Value.ToString();
                            }
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    ResultISNull = "0";
                    ResultError = "";
                }
            }
            strs[0] = ResultISNull;
            strs[1] = ResultError;
            return strs;
        }
    }

    public class Results
    {
        /// <summary>
        ///
        /// </summary>
        public List<GroupsItem> groups { get; set; }
    }

    public class GroupsItem
    {
        /// <summary>
        ///
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<StudentInfosItem> StudentInfos { get; set; }
    }

    public class StudentInfosItem
    {
        /// <summary>
        /// 高小雅
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// 女
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 淮安市文通中学
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 初三
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 1班
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ClassNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string GroupIndex { get; set; }

        public string examTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<string> Projects { get; set; }
    }

    #endregion 按组数获取学生

    #endregion 请求提交参数
}