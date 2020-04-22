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
			string @string = Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length);
			@string = @string.Replace("\r\n", "\n");
			@string = @string.Replace("\r", "\n");
			StringReader stringReader = new StringReader(@string);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string line;
			while ((line = stringReader.ReadLine()) != null)
			{
				if (!TextAsset_ReadLine(line, out string key, out string value, out string _, out string _, out string _) || string.IsNullOrEmpty(key))
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!string.IsNullOrEmpty(value))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					dictionary[key] = value;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return dictionary;
			}
		}

		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//");
			if (num >= 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				comment = line.Substring(num + 2).Trim();
				comment = DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num2 = line.IndexOf("=");
			if (num2 < 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			key = line.Substring(0, num2).Trim();
			value = line.Substring(num2 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = DecodeString(value);
			if (key.Length > 2)
			{
				while (true)
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
					while (true)
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
			ValidateFullTerm(ref key);
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
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							((IDisposable)streamReader).Dispose();
							goto end_IL_0018;
						}
					}
				}
				end_IL_0018:;
			}
			text = text.Replace("\r\n", "\n");
			return text.Replace("\r", "\n");
		}

		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int iStart = 0;
			List<string[]> list = new List<string[]>();
			while (true)
			{
				if (iStart < Text.Length)
				{
					string[] array = ParseCSVline(Text, ref iStart, Separator);
					if (array == null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						break;
					}
					list.Add(array);
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			return list;
		}

		private static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int iWordStart = iStart;
			bool flag = false;
			while (true)
			{
				if (iStart < length)
				{
					char c = Line[iStart];
					if (flag)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (c == '"')
						{
							while (true)
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
								while (true)
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
										while (true)
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
											goto IL_00f0;
										}
									}
									iStart++;
									goto IL_00f0;
								}
							}
							flag = false;
						}
					}
					else
					{
						if (c != '\n')
						{
							if (c != Separator)
							{
								if (c == '"')
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									flag = true;
								}
								goto IL_00f0;
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
						if (c == '\n')
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							iStart++;
							break;
						}
					}
					goto IL_00f0;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
				IL_00f0:
				iStart++;
			}
			if (iStart > iWordStart)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
			}
			return list.ToArray();
		}

		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (text[0] == '"' && text[text.Length - 1] == '"')
				{
					while (true)
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

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			int startIndex;
			while ((startIndex = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(startIndex, 1);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
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
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return string.Empty;
					}
				}
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
