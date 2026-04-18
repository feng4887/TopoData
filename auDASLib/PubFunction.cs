using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace auDASLib
{
    public class PubFunction
    {
        /// <summary>
        /// 检测串值是否为合法的URI格式
        /// </summary>
        /// <param name="strValue">要检测的String值</param>
        /// <returns>成功返回true 失败返回false</returns>
        public static bool IsURI( string strValue)
        {
            string strRegex = @"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            if (strValue != null && strValue.Trim() != string.Empty)
            {
                Regex re = new Regex(strRegex);
                if (re.IsMatch(strValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public static bool IsValidIP(string ip)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                {
                    string[] ips = ip.Split('.');
                    if (ips.Length == 4 || ips.Length == 6)
                    {
                        if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;

                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified string begins with a special character or a digit.
        /// </summary>
        /// <param name="input">The string to evaluate. If <paramref name="input"/> is <see langword="null"/> or empty, the method returns
        /// <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if the first character of <paramref name="input"/> is a digit or a character that is
        /// neither a letter nor an underscore; otherwise, <see langword="false"/>.</returns>
        public static bool StartsWithSpecialCharOrDigit(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // 定义特殊字符集合
            char[] specialChars = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')',
                       '-', '_', '=', '+', '[', ']', '{', '}', ';', ':',
                       '\'', '"', '\\', '|', ',', '.', '<', '>', '/', '?' };

            char firstChar = input[0];

            // 判断是否以自定义的特殊字符开头
            bool startsWithSpecialChar = specialChars.Contains(firstChar);

            if (startsWithSpecialChar)
                return true;

            // 如果是数字，返回true
            if (char.IsDigit(firstChar))
                return true;

            // 如果不是字母或数字，且不是下划线，认为是特殊字符
            if (!char.IsLetter(firstChar) && firstChar != '_')
                return true;

            return false;
        }

        /// <summary>
        /// 一个list拆分多个list
        /// </summary>
        /// <param name="list">要拆分的集合</param>
        /// <param name="num">拆分数</param>
        /// <returns></returns>
        static public Dictionary<String, List<string>> SplitList(List<string> list, int num)
        {


            int listSize = list.Count; // 长度

            Dictionary<String, List<string>> contractItemDic =
                new Dictionary<String, List<string>>(); //用户封装返回的多个list
            List<string> contractItemList = new List<string>();
            ; //用于承装每个等分list

            for (int i = 0; i < listSize; i++)
            {
                //for循环依次放入每个list中
                contractItemList.Add(list[i]); //先将对象放入list,以防止最后一个没有放入
                if (((i + 1) % num == 0) || (i + 1 == listSize))
                {
                    //如果l+1 除以 要分的份数 为整除,或者是最后一份,为结束循环.那就算作一份list,
                    contractItemDic.Add("ContractItem" + i, contractItemList); //将这一份放入Map中.
                    contractItemList = new List<string>(); //新建一个list,用于继续存储对象
                }
            }

            return contractItemDic;
        }

        /// <summary>
        /// 一个list拆分多个list
        /// </summary>
        /// <param name="list">要拆分的集合</param>
        /// <param name="num">拆分数</param>
        /// <returns></returns>
        static public List<List<string>> SplitListToList(List<string> list, int num)
        {


            int listSize = list.Count; // 长度

            List<List<string>> contractItemDic =
               new List<List<string>>(); //用户封装返回的多个list
            List<string> contractItemList = new List<string>();
            ; //用于承装每个等分list

            for (int i = 0; i < listSize; i++)
            {
                //for循环依次放入每个list中
                contractItemList.Add(list[i]); //先将对象放入list,以防止最后一个没有放入
                if (((i + 1) % num == 0) || (i + 1 == listSize))
                {
                    //如果l+1 除以 要分的份数 为整除,或者是最后一份,为结束循环.那就算作一份list,
                    contractItemDic.Add(contractItemList); //将这一份放入Map中.
                    contractItemList = new List<string>(); //新建一个list,用于继续存储对象
                }
            }

            return contractItemDic;
        }

        /// <summary>
        /// 将List转换为DataTable
        /// </summary>
        /// <param name="list">请求数据</param>
        /// <returns></returns>
        static public DataTable ListToDataTable<T>(List<T> list)
        {
            //创建一个名为"tableName"的空表
            DataTable dt = new DataTable("tableName");

            if (list.Count == 0)
                return dt;

            //创建传入对象名称的列
            foreach (var item in list.FirstOrDefault().GetType().GetProperties())
            {
                dt.Columns.Add(item.Name);
            }
            //循环存储
            foreach (var item in list)
            {
                //新加行
                DataRow value = dt.NewRow();
                //根据DataTable中的值，进行对应的赋值
                foreach (DataColumn dtColumn in dt.Columns)
                {
                    int i = dt.Columns.IndexOf(dtColumn);
                    //基元元素，直接复制，对象类型等，进行序列化
                    if (value.GetType().IsPrimitive)
                    {
                        value[i] = item.GetType().GetProperty(dtColumn.ColumnName).GetValue(item);
                    }
                    else
                    {
                        value[i] = JsonConvert.SerializeObject(item.GetType().GetProperty(dtColumn.ColumnName).GetValue(item));
                        value[i] = value[i].ToString().Replace("\\\"", @"""");
                        value[i] = value[i].ToString().Substring(0, value[i].ToString().Length - 1);
                        value[i] = value[i].ToString().Substring(1);
                    }
                }
                dt.Rows.Add(value);
            }

            return dt;
        }


        /// <summary>
        /// 重启App
        /// </summary>
        /// <param name="fileNameWithoutExtension"></param>
        /// <param name="WindowStyle">Normal = 0,Hidden = 1,Minimized = 2,Maximized = 3</param>
        public static  void RestartApp(string fileNameWithoutExtension, int WindowStyle = 0)
        {
            KillApp(fileNameWithoutExtension);
            StartApp(fileNameWithoutExtension, WindowStyle);
        }

        /// <summary>
        /// Kill App
        /// </summary>
        /// <param name="appname"></param>
        public static  void KillApp(string fileNameWithoutExtension)
        {
            System.Diagnostics.Process[] qqs = System.Diagnostics.Process.GetProcessesByName(fileNameWithoutExtension);

            foreach (Process p in qqs)
            {
                p.Kill();//停止进程
            }
        }

        /// <summary>
        /// StartApp
        /// </summary>
        /// <param name="fileNameWithoutExtension">软件路径及名称</param>
        /// <param name="starttype"> Normal = 0,Hidden = 1,Minimized = 2,Maximized = 3</param>
        public static  void StartApp(string fileNameWithoutExtension, int starttype)
        {

            // Restart and run as admin
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo(fileNameWithoutExtension);
            startInfo.Verb = "runas";
            startInfo.UseShellExecute = true;
            startInfo.Arguments = "restart";
            startInfo.WindowStyle = (ProcessWindowStyle)starttype;
            Process.Start(startInfo);

        }
    }
}
