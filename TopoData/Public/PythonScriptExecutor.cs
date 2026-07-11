using Python.Runtime;
using StdLog4;
using System;
using System.IO;
using System.Text;

namespace auDASLib
{
	/// <summary>
	/// Python 脚本执行服务。
	/// </summary>
	public static class PythonScriptExecutor
	{
		private static readonly object EngineLock = new object();
		private static bool _engineInitialized = false;

		/// <summary>
		/// 执行脚本配置中的 Python 代码或 .py 文件。
		/// </summary>
		/// <param name="script">Python 脚本配置。</param>
		/// <returns>脚本执行结果。</returns>
		public static PythonScriptExecuteResult Execute(PythonScriptItem script)
		{
			if (script == null)
				return PythonScriptExecuteResult.Fail("脚本配置不能为空。");

			if (string.IsNullOrWhiteSpace(script.ScriptText))
				return PythonScriptExecuteResult.Fail("脚本内容不能为空。");

			try
			{
				EnsurePythonEngine();
				string scriptText = ReadScriptText(script.ScriptText);
				if (!PythonScriptReadResolver.TryResolve(
					scriptText,
					out scriptText,
					out string readError))
				{
					Log4Helper.Instance.Error($"[Python Script][{script.ScriptName}] {readError}");
					return PythonScriptExecuteResult.Fail(readError);
				}

				lock (EngineLock)
				{
					using (Py.GIL())
					{
						dynamic sys = Py.Import("sys");
						dynamic io = Py.Import("io");
						dynamic stdout = io.StringIO();
						dynamic stderr = io.StringIO();
						dynamic oldStdout = sys.stdout;
						dynamic oldStderr = sys.stderr;

						try
						{
							sys.stdout = stdout;
							sys.stderr = stderr;

							PythonEngine.Exec(scriptText);

							return new PythonScriptExecuteResult()
							{
								IsSuccess = true,
								Output = Convert.ToString(stdout.getvalue()) ?? "",
								ErrorMessage = Convert.ToString(stderr.getvalue()) ?? ""
							};
						}
						finally
						{
							sys.stdout = oldStdout;
							sys.stderr = oldStderr;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log4Helper.Instance.Error($"[Python Script][{script.ScriptName}] Execute failed.", ex);
				return PythonScriptExecuteResult.Fail(ex.Message);
			}
		}

		private static void EnsurePythonEngine()
		{
			lock (EngineLock)
			{
				if (_engineInitialized)
					return;

				PythonEngine.Initialize();
				_engineInitialized = true;
			}
		}

		private static string ReadScriptText(string scriptTextOrPath)
		{
			string text = scriptTextOrPath.Trim();
			string path = text.Trim('"');

			if (path.EndsWith(".py", StringComparison.OrdinalIgnoreCase) && File.Exists(path))
				return File.ReadAllText(path, Encoding.UTF8);

			return scriptTextOrPath;
		}
	}

	/// <summary>
	/// Python 脚本执行结果。
	/// </summary>
	public class PythonScriptExecuteResult
	{
		public bool IsSuccess { get; set; } = false;

		public string Output { get; set; } = "";

		public string ErrorMessage { get; set; } = "";

		public static PythonScriptExecuteResult Fail(string errorMessage)
		{
			return new PythonScriptExecuteResult()
			{
				IsSuccess = false,
				ErrorMessage = errorMessage
			};
		}
	}
}
