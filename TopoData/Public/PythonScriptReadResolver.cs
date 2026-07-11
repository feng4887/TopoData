//=============================================================================
// Austar Group - AAS
// (c)Copyright (2026) All Rights Reserved
// Description: Resolves HiTopo Read[tag] expressions before Python execution.
//-----------------------------------------------------------------------------
// Author: Codex
// Date: 2026-07-11
// Version: 1.0
//=============================================================================
#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace auDASLib
{
	/// <summary>
	/// Converts Read[channel.tag] expressions to Python literals using realtime values.
	/// </summary>
	internal static class PythonScriptReadResolver
	{
		private static readonly Regex ReadExpressionRegex = new Regex(
			@"(?<![A-Za-z0-9_])Read\s*\[\s*(?<tag>[A-Za-z0-9_.:-]+)\s*\]",
			RegexOptions.Compiled | RegexOptions.CultureInvariant);

		/// <summary>
		/// Resolves all realtime read expressions in a Python script.
		/// </summary>
		public static bool TryResolve(string scriptText, out string resolvedScript, out string errorMessage)
		{
			resolvedScript = scriptText ?? "";
			errorMessage = "";

			MatchCollection matches = ReadExpressionRegex.Matches(resolvedScript);
			if (matches.Count == 0)
				return true;

			StringBuilder result = new StringBuilder(resolvedScript.Length);
			int sourceIndex = 0;
			foreach (Match match in matches)
			{
				string tagName = match.Groups["tag"].Value;
				if (!TryGetRealtimeValue(tagName, out RealTimeValue value))
				{
					errorMessage = $"Realtime tag was not found: {tagName}.";
					return false;
				}

				if (value.Quality != 192)
				{
					errorMessage = $"Realtime tag quality is invalid: {tagName}, quality={value.Quality}.";
					return false;
				}

				result.Append(resolvedScript, sourceIndex, match.Index - sourceIndex);
				result.Append(ToPythonLiteral(value.RealValue));
				sourceIndex = match.Index + match.Length;
			}

			result.Append(resolvedScript, sourceIndex, resolvedScript.Length - sourceIndex);
			resolvedScript = result.ToString();
			return true;
		}

		public static bool TryGetRealtimeValue(string tagName, out RealTimeValue value)
		{
			value = default!;
			if (string.IsNullOrWhiteSpace(tagName))
				return false;

			string key = tagName.Trim();
			if (auDAServer.ItemPool.ValuePool.TryGetValue(key, out RealTimeValue? directValue)
				&& directValue != null)
			{
				value = directValue;
				return true;
			}

			if (auDAServer.ItemPool.TagPool.TryGetValue(key, out DBTag? tag)
				&& tag != null
				&& TryGetRealtimeValueByTag(tag, out value))
			{
				return true;
			}

			foreach (DBTag item in auDAServer.ItemPool.TagPool.Values)
			{
				string fullName = $"{item.CannelID}.{item.TagName}";
				if ((string.Equals(item.TagId, key, StringComparison.OrdinalIgnoreCase)
					|| string.Equals(item.TagName, key, StringComparison.OrdinalIgnoreCase)
					|| string.Equals(fullName, key, StringComparison.OrdinalIgnoreCase))
					&& TryGetRealtimeValueByTag(item, out value))
				{
					return true;
				}
			}

			return false;
		}

		private static bool TryGetRealtimeValueByTag(DBTag tag, out RealTimeValue value)
		{
			value = default!;
			List<string> keys = new List<string>()
			{
				tag.TagId,
				tag.TagName,
				$"{tag.CannelID}.{tag.TagName}"
			};

			foreach (string key in keys
				.Where(it => !string.IsNullOrWhiteSpace(it))
				.Distinct(StringComparer.OrdinalIgnoreCase))
			{
				if (auDAServer.ItemPool.ValuePool.TryGetValue(key, out RealTimeValue? itemValue)
					&& itemValue != null)
				{
					value = itemValue;
					return true;
				}
			}

			return false;
		}

		private static string ToPythonLiteral(object? value)
		{
			if (value == null || value == DBNull.Value)
				return "None";

			if (value is bool boolValue)
				return boolValue ? "True" : "False";

			if (value is float floatValue)
				return FormatFloatingPoint(floatValue);

			if (value is double doubleValue)
				return FormatFloatingPoint(doubleValue);

			TypeCode typeCode = Type.GetTypeCode(value.GetType());
			if (typeCode == TypeCode.Byte
				|| typeCode == TypeCode.SByte
				|| typeCode == TypeCode.Int16
				|| typeCode == TypeCode.UInt16
				|| typeCode == TypeCode.Int32
				|| typeCode == TypeCode.UInt32
				|| typeCode == TypeCode.Int64
				|| typeCode == TypeCode.UInt64
				|| typeCode == TypeCode.Decimal)
			{
				return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "0";
			}

			string text = Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
			return QuotePythonString(text);
		}

		private static string FormatFloatingPoint(double value)
		{
			if (double.IsNaN(value))
				return "float('nan')";
			if (double.IsPositiveInfinity(value))
				return "float('inf')";
			if (double.IsNegativeInfinity(value))
				return "float('-inf')";

			return value.ToString("R", CultureInfo.InvariantCulture);
		}

		private static string QuotePythonString(string value)
		{
			StringBuilder result = new StringBuilder(value.Length + 2);
			result.Append('"');
			foreach (char item in value)
			{
				switch (item)
				{
					case '\\': result.Append("\\\\"); break;
					case '"': result.Append("\\\""); break;
					case '\r': result.Append("\\r"); break;
					case '\n': result.Append("\\n"); break;
					case '\t': result.Append("\\t"); break;
					default:
						if (char.IsControl(item))
							result.Append($"\\u{(int)item:x4}");
						else
							result.Append(item);
						break;
				}
			}

			result.Append('"');
			return result.ToString();
		}
	}
}
