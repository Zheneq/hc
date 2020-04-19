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
			string text = Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length);
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			StringReader stringReader = new StringReader(text);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string line;
			while ((line = stringReader.ReadLine()) != null)
			{
				string text2;
				string value;
				string text3;
				string text4;
				string text5;
				if (LocalizationReader.TextAsset_ReadLine(line, out text2, out value, out text3, out text4, out text5))
				{
					if (!string.IsNullOrEmpty(text2))
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.ReadTextAsset(TextAsset)).MethodHandle;
						}
						if (!string.IsNullOrEmpty(value))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							dictionary[text2] = value;
						}
					}
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return dictionary;
		}

		public unsafe static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//");
			if (num >= 0)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.TextAsset_ReadLine(string, string*, string*, string*, string*, string*)).MethodHandle;
				}
				comment = line.Substring(num + 2).Trim();
				comment = LocalizationReader.DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num2 = line.IndexOf("=");
			if (num2 < 0)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return false;
			}
			key = line.Substring(0, num2).Trim();
			value = line.Substring(num2 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = LocalizationReader.DecodeString(value);
			if (key.Length > 2)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (key[0] == '[')
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					int num3 = key.IndexOf(']');
					if (num3 >= 0)
					{
						termType = key.Substring(1, num3 - 1);
						key = key.Substring(num3 + 1);
					}
				}
			}
			LocalizationReader.ValidateFullTerm(ref key);
			return true;
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.ReadCSVfile(string)).MethodHandle;
					}
					((IDisposable)streamReader).Dispose();
				}
			}
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			return text;
		}

		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int i = 0;
			List<string[]> list = new List<string[]>();
			while (i < Text.Length)
			{
				string[] array = LocalizationReader.ParseCSVline(Text, ref i, Separator);
				if (array == null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.ReadCSV(string, char)).MethodHandle;
					}
					return list;
				}
				list.Add(array);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return list;
			}
		}

		private unsafe static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int num = iStart;
			bool flag = false;
			while (iStart < length)
			{
				char c = Line[iStart];
				if (flag)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.ParseCSVline(string, int*, char)).MethodHandle;
					}
					if (c == '"')
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (iStart + 1 < length)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (Line[iStart + 1] == '"')
							{
								if (iStart + 2 < length)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (Line[iStart + 2] == '"')
									{
										flag = false;
										iStart += 2;
										goto IL_A0;
									}
								}
								iStart++;
								goto IL_A0;
							}
						}
						flag = false;
					}
					IL_A0:;
				}
				else
				{
					if (c != '\n')
					{
						if (c == Separator)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else
						{
							if (c == '"')
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = true;
								goto IL_F0;
							}
							goto IL_F0;
						}
					}
					LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
					if (c == '\n')
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						iStart++;
						IL_108:
						if (iStart > num)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
						}
						return list.ToArray();
					}
				}
				IL_F0:
				iStart++;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_108;
			}
		}

		private unsafe static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.AddCSVtoken(List<string>*, string*, int, int*)).MethodHandle;
				}
				if (text[0] == '"' && text[text.Length - 1] == '"')
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					text = text.Substring(1, text.Length - 2);
				}
			}
			list.Add(text);
		}

		public unsafe static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.ValidateFullTerm(string*)).MethodHandle;
				}
				return;
			}
			int startIndex;
			while ((startIndex = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(startIndex, 1);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationReader.DecodeString(string)).MethodHandle;
				}
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
