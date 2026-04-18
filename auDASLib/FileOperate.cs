using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace auDASLib
{
   public class FileOperate
    {
       #region 获取文件的后缀名
       /// <summary>
        /// 获取文件的后缀名
       /// </summary>
       /// <param name="fullPath">文件名</param>
       /// <returns></returns>
       public static string GetFileSuffix(string fullPath)
        {
            if (!String.IsNullOrEmpty(fullPath))
            {
                return fullPath.Substring(fullPath.LastIndexOf('.') + 1).ToLower();
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion
       #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="FolderPath">具体路径</param>
        public static void CreateFolder(string FolderPath)
        {
            if (!System.IO.Directory.Exists(FolderPath))
            {
                System.IO.Directory.CreateDirectory(FolderPath);
            }
        }
        #endregion
       #region 删除文件夹
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="FolderPath">具体路径</param>
        public static void DeleteFolder(string FolderPath)
        {
            if (System.IO.Directory.Exists(FolderPath))
            {
                System.IO.Directory.Delete(FolderPath);
            }
        }
        #endregion

        #region 复制文件
        public static bool CopyFile(string InputFile,string OutputFile)
        {
            if (File.Exists(InputFile))
            {
                try
                {
                    if (File.Exists(OutputFile))
                        File.Delete(OutputFile);
                    //File.Delete(filename);
                    File.Copy(InputFile,OutputFile);
                    return true;
                }
                catch
                {
                    throw new Exception("复制文件失败！");
                }
            }
            return false;
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filename">文件名</param>
        public static void DeleteFile(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                }
                catch
                {
                    throw new Exception("删除文件失败！");
                }
            }
        }

       /// <summary>
       /// 删除该文件夹下所有文件
       /// </summary>
       /// <param name="dir"></param>
        public void DeleteItemsInFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                string[] strFileSystemEntries = Directory.GetFileSystemEntries(dir);
                foreach (string d in strFileSystemEntries)
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d); //直接删除其中的文件 
                    }

                    else
                    {
                        DirectoryInfo dirf = new DirectoryInfo(d);
                        if (dirf.Exists)
                        {
                            DirectoryInfo[] childs = dirf.GetDirectories();
                            foreach (DirectoryInfo child in childs)
                            {
                                child.Delete(true);
                            }
                            dirf.Delete(true);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 删除该文件夹下所有文件，子文件夹，如果deletbasefolder = true,指定的目录也删掉
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <param name="deletbasefolder">是否删除指定的这个根目录</param>
        public void DeleteItemsInFolder(string dir, bool deletbasefolder = false)
        {
            DirectoryInfo dirf = new DirectoryInfo(dir);
            if (deletbasefolder)
            {
                dirf.Delete(true); return;
            }

            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                string[] strFileSystemEntries = Directory.GetFileSystemEntries(dir);
                foreach (string d in strFileSystemEntries)
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d); //直接删除其中的文件 
                    }
                }

                if (dirf.Exists)
                {
                    DirectoryInfo[] childs = dirf.GetDirectories();
                    foreach (DirectoryInfo child in childs)
                    {
                        child.Delete(true);
                    }
                }


            }
        }

        /// <summary>
        /// 删除文件夹中指定类型的文件,并可删除文件夹，如果存在其它文件则不能完全删除文件夹
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <param name="subnames">要删除的扩展名</param>
        /// <param name="deletbasefolder">是否删除该文件夹</param>
        public void DeleteFolderFiles(string dir, string[] subnames = null, bool deletbasefolder = false)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                string[] strFileSystemEntries = Directory.GetFileSystemEntries(dir);
                foreach (string d in strFileSystemEntries)
                {
                    if (File.Exists(d))
                    {
                        if (subnames == null)
                        {
                            File.Delete(d); //直接删除其中的文件 
                        }
                        else
                        {
                            //判断扩展名,并删除扩展名类型的文件
                            string subname = GetFileSuffix(d);
                            if (!string.IsNullOrEmpty(subname))
                            {
                                foreach (string sb in subnames)
                                {
                                    if (sb.CompareTo(subname) == 0)
                                    {
                                        File.Delete(d); //直接删除其中的文件 
                                    }
                                }
                            }
                        }
                    }

                    else
                        DeleteFolderFiles(d, subnames, deletbasefolder); //递归删除子文件夹 
                }
                if ((deletbasefolder) && (strFileSystemEntries.Length == 0))
                {
                    Directory.Delete(dir); //删除已空文件夹 
                    // Response.Write(dir+" 文件夹删除成功"); 
                }

            }
            // else 
            // Response.Write(dir+" 该文件夹不存在"); //如果文件夹不存在则提示 
        }

        #endregion



    }
}
