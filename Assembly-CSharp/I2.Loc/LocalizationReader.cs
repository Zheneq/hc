using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationReader
	{
		public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
		{
			string loc = Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length);
			loc = loc.Replace("\r\n", "\n").Replace("\r", "\n");
			StringReader stringReader = new StringReader(loc);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string line;
			while ((line = stringReader.ReadLine()) != null)
			{
				string key;
				string value;
				string foo;
				string foo1;
				string foo2;
				if (TextAsset_ReadLine(line, out key, out value, out foo, out foo1, out foo2)
				    && !string.IsNullOrEmpty(key)
				    && !string.IsNullOrEmpty(value))
				{
					dictionary[key] = value;
				}
			}
			return dictionary;
		}

		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int slashPos = line.LastIndexOf("//");
			if (slashPos >= 0)
			{
				comment = line.Substring(slashPos + 2).Trim();
				comment = DecodeString(comment);
				line = line.Substring(0, slashPos);
			}
			int equalsPos = line.IndexOf("=");
			if (equalsPos >= 0)
			{
				key = line.Substring(0, equalsPos).Trim();
				value = line.Substring(equalsPos + 1).Trim();
				value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
				value = DecodeString(value);
				if (key.Length > 2 && key[0] == '[')
				{
					int endPos = key.IndexOf(']');
					if (endPos >= 0)
					{
						termType = key.Substring(1, endPos - 1);
						key = key.Substring(endPos + 1);
					}
				}

				ValidateFullTerm(ref key);
				return true;
			}

			return false;
		}

		public static string ReadCSVfile(string Path)
		{
			string text = string.Empty;
			StreamReader streamReader = File.OpenText(Path);
			try
			{
				text = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					((IDisposable)streamReader).Dispose();
				}
			}
			return text.Replace("\r\n", "\n").Replace("\r", "\n");
		}

		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int iStart = 0;
			List<string[]> list = new List<string[]>();
			while (true)
			{
				if (iStart >= Text.Length)
				{
					break;
				}
				string[] array = ParseCSVline(Text, ref iStart, Separator);
				if (array == null)
				{
					break;
				}
				list.Add(array);
			}
			return list;
		}

		private static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int iWordStart = iStart;
			bool isInQuotes = false;
			for (; iStart < length; iStart++)
			{
				char c = Line[iStart];
				if (isInQuotes)
				{
					if (c != '"')
					{
						continue;
					}

					if (iStart + 1 >= length || Line[iStart + 1] != '"')
					{
						isInQuotes = false;
					}
					else if (iStart + 2 < length && Line[iStart + 2] == '"')
					{
						isInQuotes = false;
						iStart += 2;
					}
					else
					{
						iStart++;
					}
				}
				else
				{
					if (c == '\n' || c == Separator)
					{
						AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
						if (c == '\n')
						{
							iStart++;
							break;
						}
					}
					else if (c == '"')
					{
						isInQuotes = true;
					}
				}
			}
			if (iStart > iWordStart)
			{
				AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
			}
			return list.ToArray();
		}

		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1
			    && text[0] == '"'
			    && text[text.Length - 1] == '"')
			{
				text = text.Substring(1, text.Length - 2);
			}
			list.Add(text);
		}

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				return;
			}
			int startIndex;
			while ((startIndex = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(startIndex, 1);
			}
		}

		public static string EncodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
		}

		public static string DecodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
