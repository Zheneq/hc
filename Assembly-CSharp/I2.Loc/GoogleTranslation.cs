using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace I2.Loc
{
	public static class GoogleTranslation
	{
		public static void Translate(string text, string LanguageCodeFrom, string LanguageCodeTo, Action<string> OnTranslationReady)
		{
			WWW translationWWW = GetTranslationWWW(text, LanguageCodeFrom, LanguageCodeTo);
			CoroutineManager.pInstance.StartCoroutine(WaitForTranslation(translationWWW, OnTranslationReady, text));
		}

		private static IEnumerator WaitForTranslation(WWW www, Action<string> OnTranslationReady, string OriginalText)
		{
			yield return www;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			WWW translationWWW = GetTranslationWWW(text, LanguageCodeFrom, LanguageCodeTo);
			while (!translationWWW.isDone)
			{
			}
			if (!string.IsNullOrEmpty(translationWWW.error))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						Debug.LogError("-- " + translationWWW.error);
						using (Dictionary<string, string>.Enumerator enumerator = translationWWW.responseHeaders.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> current = enumerator.Current;
								Debug.Log(current.Value + "=" + current.Key);
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						return string.Empty;
					}
					}
				}
			}
			return ParseTranslationResult(translationWWW.text, text);
		}

		private static WWW GetTranslationWWW(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			LanguageCodeFrom = GoogleLanguages.GetGoogleLanguageCode(LanguageCodeFrom);
			LanguageCodeTo = GoogleLanguages.GetGoogleLanguageCode(LanguageCodeTo);
			text = text.ToLower();
			string url = $"http://www.google.com/translate_t?hl=en&vi=c&ie=UTF8&oe=UTF8&submit=Translate&langpair={LanguageCodeFrom}|{LanguageCodeTo}&text={Uri.EscapeUriString(text)}";
			return new WWW(url);
		}

		private static string ParseTranslationResult(string html, string OriginalText)
		{
			try
			{
				int num = html.IndexOf("TRANSLATED_TEXT") + "TRANSLATED_TEXT='".Length;
				int num2 = html.IndexOf("';INPUT_TOOL_PATH", num);
				string text = html.Substring(num, num2 - num);
				string input = text;
				if (_003C_003Ef__am_0024cache0 == null)
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
					_003C_003Ef__am_0024cache0 = ((Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)));
				}
				text = Regex.Replace(input, "\\\\x([a-fA-F0-9]{2})", _003C_003Ef__am_0024cache0);
				string input2 = text;
				if (_003C_003Ef__am_0024cache1 == null)
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
					_003C_003Ef__am_0024cache1 = ((Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value)));
				}
				text = Regex.Replace(input2, "&#(\\d+);", _003C_003Ef__am_0024cache1);
				text = text.Replace("<br>", "\n");
				if (OriginalText.ToUpper() == OriginalText)
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
					text = text.ToUpper();
				}
				else if (UppercaseFirst(OriginalText) == OriginalText)
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
					text = UppercaseFirst(text);
				}
				else if (TitleCase(OriginalText) == OriginalText)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					text = TitleCase(text);
				}
				return text;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				return string.Empty;
			}
		}

		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToLower().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		public static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				while (true)
				{
					switch (4)
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
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}
	}
}
