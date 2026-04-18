using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auDASLib
{
    /// <summary>
    /// 配方管理类
    /// 杜金旺
    /// 2026-02-01
    /// </summary>
    public class master_recipe
    {
        #region [单例者]
        private static master_recipe instance = null;
        static master_recipe()
        {
            if (instance == null)
                instance = new master_recipe();
        }
        public static master_recipe Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
            }
        }
        #endregion //[单例者]

        /// <summary>
        /// Gets the full file path to the hiTopoTagDef.xml configuration file, based on the current operating system.
        /// </summary>
        /// <remarks>Use this property to retrieve the correct configuration file path regardless of the
        /// operating system. The returned path is suitable for reading or writing the hiTopoTagDef.xml file as required
        /// by the application.</remarks>
        public static string RecipePathDefine
        {
            get
            {
                if (OSChecker.IsWindows())
                {
                    return @"C:\Users\Public\Documents\" + @"Config\hiTopoRecipeDef.xml"; ;
                }
                else
                {
                    return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/hiTopoRecipeDef.xml";
                }
            }
        }

        public string recipe_name { get; set; } = "";
        public string recipe_id { get; set; } = "";
        public string version { get; set; } = "";
        public string description { get; set; } = "";

        public RecipeType Type { get; set; } = RecipeType.Parameter;
        public List<equipment_recipe> equipment_recipes = new List<equipment_recipe>();
        public DateTime last_modify_time { get; set; } = DateTime.Now;

        /// <summary>
        /// 配方状态
        /// </summary>
        public RecipeStatus status = RecipeStatus.InUse;
    }


    /// <summary>
    /// 单设备配方信息
    /// </summary>
    public class equipment_recipe
    {
        public equipment_recipe() { } 
        public string equipment_id { get; set; } = "";
        public string equipment_name { get; set; } = "";

        public RecipeStatus recipe_status { get; set; } = RecipeStatus.Unknown;
        public string recipe_type { get; set;} = "";
        public string description { get; set; } = "";

        public List<recipe_data> recipe_data = new List<recipe_data>();
    }

    public class recipe_data
    {
        /// <summary>
        /// 配方参数名
        /// </summary>
        public string parameter_name { get; set; } = "";

        /// <summary>
        /// 设备标签名
        /// </summary>
        public string tag_name { get; set; } = "";
        public string data_type { get; set; } 

        public string value { get; set; } = "";
        public string description { get; set; } = "";

        public string uom { get; set; } = "";
    }




    public enum RecipeType
    {
        Unknown,
        Main,
        Sub,
        Parameter,
        Program,
    }

    public enum RecipeSource
    {
        Unknown,
        Equipment,
        MES,
        Manual,
    }

    public enum RecipeStatus
    {
        Unknown,
        InUse,
        Archived,
        Obsolete,
    }









}
