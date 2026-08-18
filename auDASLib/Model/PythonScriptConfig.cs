using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace auDASLib
{
	/// <summary>
	/// Python 脚本配置文件根对象。
	/// </summary>
	[XmlRoot("PythonScripts")]
	public class PythonScriptConfig
	{
		#region [单例者]
		private static PythonScriptConfig instance = new PythonScriptConfig();

		static PythonScriptConfig()
		{
			if (instance == null)
				instance = new PythonScriptConfig();
		}

		public static PythonScriptConfig Instance
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
		#endregion [单例者]

		/// <summary>
		/// Python 脚本配置文件路径。
		/// </summary>
		public static string ScriptPathDefine
		{
			get
			{
				if (OSChecker.IsWindows())
				{
					return @"C:\Users\Public\Documents\" + @"Config\hiTopoPythonScriptDef.xml";
				}
				else
				{
					return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Config/hiTopoPythonScriptDef.xml";
				}
			}
		}

		public string version { get; set; } = "1.0";

		public string description { get; set; } = "";

		public List<PythonScriptItem> Scripts { get; set; } = new List<PythonScriptItem>();

		public DateTime last_modify_time { get; set; } = DateTime.Now;
	}

	/// <summary>
	/// 单个 Python 脚本配置。
	/// </summary>
	public class PythonScriptItem
	{
		public string ScriptName { get; set; } = "";

		public string Description { get; set; } = "";

		public bool bActive { get; set; } = true;

		public PythonScriptTrigger Trigger { get; set; } = new PythonScriptTrigger();

		public string ScriptText { get; set; } = "";

		public DateTime last_modify_time { get; set; } = DateTime.Now;
	}

	/// <summary>
	/// Python 脚本触发条件。
	/// </summary>
	public class PythonScriptTrigger
	{
		public string TagName { get; set; } = "";

		public bool bUseExpression { get; set; } = false;

		public string ExpressionText { get; set; } = "";

		public List<DBTag> ExpressionTags { get; set; } = new List<DBTag>();
	}
}
