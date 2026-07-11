//=============================================================================
// Austar Group - AAS
// (c)Copyright (2026) All Rights Reserved
// Description: Python script runtime operator.
//-----------------------------------------------------------------------------
// Author: Codex
// Date: 2026-07-10
// Version: 1.0
//=============================================================================
#nullable enable

using StdLog4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace auDASLib
{
	/// <summary>
	/// Loads Python script configuration and starts script monitor jobs.
	/// </summary>
	public sealed class PythonScriptOperater
	{
		#region Singleton

		public static PythonScriptOperater Instance => _instance;

		private static readonly PythonScriptOperater _instance = new PythonScriptOperater();

		#endregion

		private readonly object _syncRoot = new object();
		private readonly List<PythonScriptJob> _jobs = new List<PythonScriptJob>();
		private string _lastErrorMessage = "";

		private PythonScriptOperater()
		{
		}

		/// <summary>
		/// Starts all active Python script monitor jobs.
		/// </summary>
		public void Start()
		{
			lock (_syncRoot)
			{
				StopJobs();
				_jobs.Clear();

				PythonScriptConfig config = LoadConfig();
				if (config.Scripts == null || config.Scripts.Count == 0)
					return;

				foreach (PythonScriptItem script in config.Scripts)
				{
					if (script == null || !script.bActive)
						continue;

					PythonScriptJob job = new PythonScriptJob(script);
					if (job.Start())
						_jobs.Add(job);
				}
			}
		}

		/// <summary>
		/// Stops all Python script monitor jobs.
		/// </summary>
		public void Stop()
		{
			lock (_syncRoot)
			{
				StopJobs();
				_jobs.Clear();
			}
		}

		private PythonScriptConfig LoadConfig()
		{
			try
			{
				if (!File.Exists(PythonScriptConfig.ScriptPathDefine))
					return new PythonScriptConfig();

				PythonScriptConfig config = XmlHelper.XmlDeserializeFromFile<PythonScriptConfig>(
					PythonScriptConfig.ScriptPathDefine,
					Encoding.UTF8) ?? new PythonScriptConfig();

				if (config.Scripts == null)
					config.Scripts = new List<PythonScriptItem>();

				PythonScriptConfig.Instance = config;
				return config;
			}
			catch (Exception ex)
			{
				LogError("[Python Script] Load script configuration failed.", ex);
				return new PythonScriptConfig();
			}
		}

		private void StopJobs()
		{
			foreach (PythonScriptJob job in _jobs)
			{
				try
				{
					job.Stop();
				}
				catch (Exception ex)
				{
					LogError("[Python Script] Stop script job failed.", ex);
				}
			}
		}

		private void LogError(string message, Exception? ex = null)
		{
			if (string.IsNullOrWhiteSpace(message))
				return;

			string errorMessage = ex == null ? message : $"{message} {ex.Message}";
			if (string.Equals(_lastErrorMessage, errorMessage, StringComparison.Ordinal))
				return;

			if (IsLogEnabled())
			{
				if (ex == null)
					Log4Helper.Instance.Error(message);
				else
					Log4Helper.Instance.Error(message, ex);
			}

			_lastErrorMessage = errorMessage;
		}

		private static bool IsLogEnabled()
		{
			try
			{
				return ServerCfg.Instance != null && ServerCfg.Instance.EnableLog4;
			}
			catch
			{
				return false;
			}
		}
	}

	internal sealed class PythonScriptJob
	{
		private const int PollIntervalMs = 250;
		private readonly PythonScriptItem _script;
		private CancellationTokenSource? _cancellationToken;
		private Task? _monitorTask;
		private string _scriptFilePath = "";
		private string _lastErrorMessage = "";
		private string _lastTriggerName = "";
		private string _lastTriggerValue = "";
		private DateTime _lastTriggerTime = DateTime.MinValue;
		private bool _lastConditionValue;
		private int _isExecuting;

		public PythonScriptJob(PythonScriptItem script)
		{
			_script = script;
		}

		/// <summary>
		/// Generates the .py file and starts the monitor loop.
		/// </summary>
		public bool Start()
		{
			if (!PrepareScriptFile())
				return false;

			_cancellationToken = new CancellationTokenSource();
			_monitorTask = Task.Run(() => MonitorAsync(_cancellationToken.Token));
			return true;
		}

		/// <summary>
		/// Stops the monitor loop and cancels any running script process.
		/// </summary>
		public void Stop()
		{
			if (_cancellationToken == null)
				return;

			_cancellationToken.Cancel();

			try
			{
				_monitorTask?.Wait(TimeSpan.FromSeconds(3));
			}
			catch (AggregateException ex) when (ex.InnerExceptions.All(
				it => it is TaskCanceledException || it is OperationCanceledException))
			{
				Debug.WriteLine(ex.Message);
			}

			_cancellationToken.Dispose();
			_cancellationToken = null;
			_monitorTask = null;
		}

		private async Task MonitorAsync(CancellationToken cancellationToken)
		{
			SetThreadName("PythonScript_" + GetSafeScriptName());
			await Task.Delay(2000, cancellationToken);

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					if (IsTriggerRisingEdge())
						BeginExecute(cancellationToken);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				catch (Exception ex)
				{
					LogErrorOnce("Monitor script trigger failed.", ex);
				}

				await Task.Delay(PollIntervalMs, cancellationToken);
			}
		}

		private bool IsTriggerRisingEdge()
		{
			PythonScriptTrigger trigger = _script.Trigger ?? new PythonScriptTrigger();
			bool conditionValue = trigger.bUseExpression
				? GetExpressionCondition(trigger)
				: GetBoolTagCondition(trigger.TagName);

			bool isRisingEdge = conditionValue && !_lastConditionValue;
			_lastConditionValue = conditionValue;
			return isRisingEdge;
		}

		private bool GetExpressionCondition(PythonScriptTrigger trigger)
		{
			if (string.IsNullOrWhiteSpace(trigger.ExpressionText))
				return false;

			Dictionary<string, dynamic> parameters = new Dictionary<string, dynamic>();
			List<DBTag> expressionTags = trigger.ExpressionTags ?? new List<DBTag>();

			foreach (DBTag tag in expressionTags)
			{
				string parameterName = tag.TagName ?? "";
				if (string.IsNullOrWhiteSpace(parameterName))
					continue;

				string valueKey = GetExpressionValueKey(tag);
				if (PythonScriptReadResolver.TryGetRealtimeValue(valueKey, out RealTimeValue value)
					&& value != null
					&& value.Quality == 192)
				{
					parameters[parameterName] = value.RealValue;
				}
				else
				{
					parameters[parameterName] = 0;
				}
			}

			bool conditionValue = FleeOperator.ConditionIsOK(trigger.ExpressionText, parameters);
			if (conditionValue)
			{
				_lastTriggerName = "Expression";
				_lastTriggerValue = "true";
				_lastTriggerTime = DateTime.Now;
			}

			return conditionValue;
		}

		private bool GetBoolTagCondition(string tagName)
		{
			if (string.IsNullOrWhiteSpace(tagName))
				return false;

			if (!PythonScriptReadResolver.TryGetRealtimeValue(tagName, out RealTimeValue value)
				|| value == null
				|| value.Quality != 192)
			{
				return false;
			}

			bool conditionValue = ConvertToBool(value.RealValue);
			if (conditionValue)
			{
				_lastTriggerName = tagName;
				_lastTriggerValue = Convert.ToString(value.RealValue, CultureInfo.InvariantCulture) ?? "";
				_lastTriggerTime = DateTime.Now;
			}

			return conditionValue;
		}

		private void BeginExecute(CancellationToken cancellationToken)
		{
			if (Interlocked.CompareExchange(ref _isExecuting, 1, 0) != 0)
				return;

			_ = Task.Run(async () =>
			{
				try
				{
					bool succeeded = await ExecuteScriptProcessAsync(cancellationToken);
					if (succeeded)
						_lastErrorMessage = "";
				}
				catch (Exception ex)
				{
					LogErrorOnce("Execute script process failed.", ex);
				}
				finally
				{
					Interlocked.Exchange(ref _isExecuting, 0);
				}
			});
		}

		/// <summary>
		/// Starts the Python process and returns whether the script exits successfully.
		/// </summary>
		/// <returns>True when the process exits with code 0; otherwise false.</returns>
		private async Task<bool> ExecuteScriptProcessAsync(CancellationToken cancellationToken)
		{
			if (!TryPrepareExecutionScript())
				return false;

			string pythonExecutable = ResolvePythonExecutablePath();
			string? workDirectory = Path.GetDirectoryName(_scriptFilePath);
			if (string.IsNullOrWhiteSpace(workDirectory))
				workDirectory = AppContext.BaseDirectory;

			ProcessStartInfo startInfo = new ProcessStartInfo()
			{
				FileName = pythonExecutable,
				Arguments = QuoteArgument(_scriptFilePath),
				WorkingDirectory = workDirectory,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8
			};

			startInfo.Environment["HITOPO_SCRIPT_NAME"] = _script.ScriptName ?? "";
			startInfo.Environment["HITOPO_TRIGGER_NAME"] = _lastTriggerName;
			startInfo.Environment["HITOPO_TRIGGER_VALUE"] = _lastTriggerValue;
			startInfo.Environment["HITOPO_TRIGGER_TIME"] = _lastTriggerTime == DateTime.MinValue
				? ""
				: _lastTriggerTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

			using Process process = new Process();
			process.StartInfo = startInfo;

			try
			{
				if (!process.Start())
				{
					LogErrorOnce($"Start python process failed: {pythonExecutable}");
					return false;
				}

				Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
				Task<string> errorTask = process.StandardError.ReadToEndAsync();

				await process.WaitForExitAsync(cancellationToken);

				_ = await outputTask;
				string error = await errorTask;

				if (process.ExitCode != 0)
				{
					string message = $"Script exit code: {process.ExitCode}.";
					if (!string.IsNullOrWhiteSpace(error))
						message += " " + error.Trim();

					LogErrorOnce(message);
					return false;
				}

				if (!string.IsNullOrWhiteSpace(error))
				{
					LogErrorOnce(error.Trim());
				}

				return true;
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				KillProcess(process);
				return false;
			}
		}

		private bool TryPrepareExecutionScript()
		{
			try
			{
				string scriptContent = ReadScriptContent();
				if (!PythonScriptReadResolver.TryResolve(
					scriptContent,
					out string resolvedScript,
					out string errorMessage))
				{
					LogErrorOnce(errorMessage);
					return false;
				}

				File.WriteAllText(_scriptFilePath, resolvedScript, new UTF8Encoding(false));
				return true;
			}
			catch (Exception ex)
			{
				LogErrorOnce("Resolve realtime read expressions failed.", ex);
				return false;
			}
		}

		private bool PrepareScriptFile()
		{
			try
			{
				string scriptContent = ReadScriptContent();
				if (string.IsNullOrWhiteSpace(scriptContent))
				{
					LogErrorOnce("Script content is empty.");
					return false;
				}

				string folder = Path.Combine(pubDefine.Folder, "PythonScripts");
				Directory.CreateDirectory(folder);

				string fileName = GetSafeScriptName() + ".py";
				_scriptFilePath = Path.Combine(folder, fileName);
				File.WriteAllText(_scriptFilePath, scriptContent, new UTF8Encoding(false));
				return true;
			}
			catch (Exception ex)
			{
				LogErrorOnce("Generate .py script file failed.", ex);
				return false;
			}
		}

		private string ReadScriptContent()
		{
			string scriptText = _script.ScriptText ?? "";
			string text = scriptText.Trim();
			string pathText = text.Trim('"');

			if (!text.Contains('\r')
				&& !text.Contains('\n')
				&& pathText.EndsWith(".py", StringComparison.OrdinalIgnoreCase))
			{
				string? sourcePath = ResolveScriptPath(pathText);
				if (!string.IsNullOrWhiteSpace(sourcePath))
					return File.ReadAllText(sourcePath, Encoding.UTF8);
			}

			return scriptText;
		}

		private static string? ResolveScriptPath(string scriptPath)
		{
			if (string.IsNullOrWhiteSpace(scriptPath))
				return null;

			List<string> candidates = new List<string>();
			if (Path.IsPathRooted(scriptPath))
			{
				candidates.Add(scriptPath);
			}
			else
			{
				candidates.Add(Path.Combine(AppContext.BaseDirectory, scriptPath));
				candidates.Add(Path.Combine(pubDefine.Folder, scriptPath));
			}

			foreach (string candidate in candidates)
			{
				if (File.Exists(candidate))
					return candidate;
			}

			return null;
		}

		private static string GetExpressionValueKey(DBTag tag)
		{
			if (!string.IsNullOrWhiteSpace(tag.Address))
				return tag.Address;

			if (!string.IsNullOrWhiteSpace(tag.TagId))
				return tag.TagId;

			return tag.TagName ?? "";
		}

		private static bool ConvertToBool(object value)
		{
			if (value == null)
				return false;

			if (value is bool boolValue)
				return boolValue;

			if (value is string stringValue)
			{
				if (bool.TryParse(stringValue, out bool parsedBoolean))
					return parsedBoolean;

				if (double.TryParse(
					stringValue,
					NumberStyles.Any,
					CultureInfo.InvariantCulture,
					out double parsedNumber))
				{
					return Math.Abs(parsedNumber) > double.Epsilon;
				}

				return false;
			}

			if (value is IConvertible)
			{
				try
				{
					return Math.Abs(Convert.ToDouble(value, CultureInfo.InvariantCulture)) > double.Epsilon;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}

		private static string ResolvePythonExecutablePath()
		{
			string? environmentPath = Environment.GetEnvironmentVariable("HITOPO_PYTHON_EXE");
			if (!string.IsNullOrWhiteSpace(environmentPath))
				return environmentPath.Trim('"');

			string applicationPath = Path.Combine(AppContext.BaseDirectory, "python", "python.exe");
			if (File.Exists(applicationPath))
				return applicationPath;

			return OSChecker.IsWindows() ? "python.exe" : "python3";
		}

		private string GetSafeScriptName()
		{
			string scriptName = string.IsNullOrWhiteSpace(_script.ScriptName)
				? "PythonScript"
				: _script.ScriptName.Trim();

			foreach (char item in Path.GetInvalidFileNameChars())
			{
				scriptName = scriptName.Replace(item, '_');
			}

			return scriptName;
		}

		private static string QuoteArgument(string value)
		{
			return "\"" + value.Replace("\"", "\\\"") + "\"";
		}

		private static void SetThreadName(string name)
		{
			if (Thread.CurrentThread.Name == null)
				Thread.CurrentThread.Name = name;
		}

		private static void KillProcess(Process process)
		{
			try
			{
				if (!process.HasExited)
					process.Kill(entireProcessTree: true);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		private void LogErrorOnce(string message, Exception? ex = null)
		{
			string errorMessage = ex == null ? message : $"{message} {ex.Message}";
			if (string.Equals(_lastErrorMessage, errorMessage, StringComparison.Ordinal))
				return;

			string logMessage = $"[Python Script][{_script.ScriptName}] {message}";
			if (ex == null)
				Log4Helper.Instance.Fatal(logMessage);
			else
				Log4Helper.Instance.Fatal(logMessage, ex);

			_lastErrorMessage = errorMessage;
		}
	}
}
