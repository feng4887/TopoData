using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//******************************************************************************
//Description: Auto start class by creating shortcut in system startup folder,
//also can create shortcut in desktop if needed.
//Author: 杜金旺 Jim DU
//Data: 2026.11.16
//*******************************************************************************

namespace auDASLib
{
    public class AutoStart
    {
        #region 公开

        /// <summary>
        /// 唯一实例，也可以自定义实例
        /// </summary>
        public static AutoStart Instance { get; private set; } = new AutoStart();

        /// <summary>
        /// 快捷方式描述，默认值是当前的进程名
        /// </summary>
        public string QuickDescribe { get; set; } = Process.GetCurrentProcess().ProcessName;

        /// <summary>
        /// 快捷方式名称，默认值是当前的进程名
        /// </summary>
        public string QuickName { get; set; } = Process.GetCurrentProcess().ProcessName;

        /// <summary>
        /// 自启动窗口类型，默认值是正常窗口
        /// </summary>
        public WshWindowStyle WindowStyle { get; set; } = WshWindowStyle.WshNormalFocus;

        public static bool IsAutoStartViaShortcutEnabled(string ProductName)
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupPath, ProductName + ".lnk");
            return System.IO.File.Exists(shortcutPath);
        }

        /// <summary>
        /// 设置开机自动启动-只需要调用改方法就可以了参数里面的bool变量是控制开机启动的开关的，默认为开启自启启动
        /// </summary>
        /// <param name="onOff">自启开关</param>
        public void SetAutoStart(bool onOff = true)
        {
            if (onOff)//开机启动
            {
                //获取启动路径应用程序快捷方式的路径集合
                List<string> shortcutPaths = GetQuickFromFolder(systemStartPath, appAllPath);
                //存在2个以快捷方式则保留一个快捷方式-避免重复多于
                if (shortcutPaths.Count >= 2)
                {
                    for (int i = 1; i < shortcutPaths.Count; i++)
                    {
                        DeleteFile(shortcutPaths[i]);
                    }
                }
                else if (shortcutPaths.Count < 1)//不存在则创建快捷方式
                {
                    CreateShortcut(systemStartPath, QuickName, appAllPath, QuickDescribe, WindowStyle);
                }
            }
            else//开机不启动
            {
                //获取启动路径应用程序快捷方式的路径集合
                List<string> shortcutPaths = GetQuickFromFolder(systemStartPath, appAllPath);
                //存在快捷方式则遍历全部删除
                if (shortcutPaths.Count > 0)
                {
                    for (int i = 0; i < shortcutPaths.Count; i++)
                    {
                        DeleteFile(shortcutPaths[i]);
                    }
                }
            }
            //创建桌面快捷方式-如果需要可以取消注释
            //CreateDesktopQuick(desktopPath, QuickName, appAllPath);
        }

        /// <summary>
        /// 在桌面上创建快捷方式-如果需要可以调用
        /// </summary>
        public void SetDesktopQuick(bool isCreate)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            List<string> shortcutPaths = GetQuickFromFolder(desktopPath, appAllPath);
            if (isCreate)
            {
                //没有就创建
                if (shortcutPaths.Count < 1)
                {
                    CreateShortcut(desktopPath, QuickName, appAllPath, QuickDescribe, WshWindowStyle.WshNormalFocus);
                }
            }
            else
            {
                //有就删除
                for (int i = 0; i < shortcutPaths.Count; i++)
                {
                    DeleteFile(shortcutPaths[i]);
                }
            }
        }

        #endregion 公开

        #region 私有

        /// <summary>
        /// 自动获取系统自动启动目录
        /// </summary>
        private string systemStartPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        /// <summary>
        /// 自动获取程序完整路径
        /// </summary>
        private string appAllPath = Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// 自动获取桌面目录
        /// </summary>
        private string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        /// <summary>
        ///  向目标路径创建指定文件的快捷方式
        /// </summary>
        /// <param name="directory">目标目录</param>
        /// <param name="shortcutName">快捷方式名字</param>
        /// <param name="targetPath">文件完全路径</param>
        /// <param name="description">描述</param>
        /// <param name="iconLocation">图标地址</param>
        /// <returns>成功或失败</returns>
        private bool CreateShortcut(string directory, string shortcutName, string targetPath, string description, WshWindowStyle windowStyle, string iconLocation = null)
        {
            try
            {
                //目录不存在则创建
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                //合成路径
                string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortcutName));
                //存在则不创建
                if (System.IO.File.Exists(shortcutPath)) return true;
                //添加引用 Com 中搜索 Windows Script Host Object Model
                WshShell shell = new IWshRuntimeLibrary.WshShell();
                //创建快捷方式对象
                IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
                //指定目标路径
                shortcut.TargetPath = targetPath;
                //设置起始位置
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                //设置运行方式，默认为常规窗口
                shortcut.WindowStyle = (int)windowStyle;
                //设置备注
                shortcut.Description = description;
                //设置图标路径
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
                //保存快捷方式
                shortcut.Save();
                return true;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                temp = "";
            }
            return false;
        }

        /// <summary>
        /// 获取指定文件夹下指定应用程序的快捷方式路径集合
        /// </summary>
        /// <param name="directory">文件夹</param>
        /// <param name="targetPath">目标应用程序路径</param>
        /// <returns>目标应用程序的快捷方式</returns>
        private List<string> GetQuickFromFolder(string directory, string targetPath)
        {
            List<string> tempStrs = new List<string>();
            tempStrs.Clear();
            string tempStr = null;
            string[] files = Directory.GetFiles(directory, "*.lnk");
            if (files == null || files.Length < 1)
            {
                return tempStrs;
            }
            for (int i = 0; i < files.Length; i++)
            {
                //files[i] = string.Format("{0}\\{1}", directory, files[i]);
                tempStr = GetAppPathFromQuick(files[i]);
                if (tempStr == targetPath)
                {
                    tempStrs.Add(files[i]);
                }
            }
            return tempStrs;
        }

        /// <summary>
        /// 获取快捷方式的目标文件路径-用于判断是否已经开启了自动启动
        /// </summary>
        /// <param name="shortcutPath"></param>
        /// <returns></returns>
        private string GetAppPathFromQuick(string shortcutPath)
        {
            //快捷方式文件的路径 = @"d:\Test.lnk";
            if (System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortct = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                //快捷方式文件指向的路径.Text = 当前快捷方式文件IWshShortcut类.TargetPath;
                //快捷方式文件指向的目标目录.Text = 当前快捷方式文件IWshShortcut类.WorkingDirectory;
                return shortct.TargetPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据路径删除文件-用于取消自启时从计算机自启目录删除程序的快捷方式
        /// </summary>
        /// <param name="path">路径</param>
        private void DeleteFile(string path)
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                System.IO.File.Delete(path);
            }
        }

        #endregion 私有
    }

}
