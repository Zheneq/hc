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
			WWW translationWWW = GoogleTranslation.GetTranslationWWW(text, LanguageCodeFrom, LanguageCodeTo);
			CoroutineManager.pInstance.StartCoroutine(GoogleTranslation.WaitForTranslation(translationWWW, OnTranslationReady, text));
		}

		private static IEnumerator WaitForTranslation(WWW www, Action<string> OnTranslationReady, string OriginalText)
		{
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError(www.error);
				OnTranslationReady(string.Empty);
			}
			else
			{
				string obj = GoogleTranslation.ParseTranslationResult(www.text, OriginalText);
				OnTranslationReady(obj);
			}
			yield break;
		}

		public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			WWW translationWWW = GoogleTranslation.GetTranslationWWW(text, LanguageCodeFrom, LanguageCodeTo);
			while (!translationWWW.isDone)
			{
			}
			if (!string.IsNullOrEmpty(translationWWW.error))
			{
				Debug.LogError("-- " + translationWWW.error);
				using (Dictionary<string, string>.Enumerator enumerator = translationWWW.responseHeaders.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> keyValuePair = enumerator.Current;
						Debug.Log(keyValuePair.Value + "=" + keyValuePair.Key);
					}
				}
				return string.Empty;
			}
			return GoogleTranslation.ParseTranslationResult(translationWWW.text, text);
		}

		private static WWW GetTranslationWWW(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			LanguageCodeFrom = GoogleLanguages.GetGoogleLanguageCode(LanguageCodeFrom);
			LanguageCodeTo = GoogleLanguages.GetGoogleLanguageCode(LanguageCodeTo);
			text = text.ToLower();
			string url = string.Format("http://www.google.com/translate_t?hl=en&vi=c&ie=UTF8&oe=UTF8&submit=Translate&langpair={0}|{1}&text={2}", LanguageCodeFrom, LanguageCodeTo, Uri.EscapeUriString(text));
			return new WWW(url);
		}

		private static string ParseTranslationResult(string html, string OriginalText)
		{
			string result;
			try
			{
				int num = html.IndexOf("TRANSLATED_TEXT") + "TRANSLATED_TEXT='".Length;
				int num2 = html.IndexOf("';INPUT_TOOL_PATH", num);
				string text = html.Substring(num, num2 - num);
				string input = text;
				string pattern = "\\\\x([a-fA-F0-9]{2})";
				
				text = Regex.Replace(input, pattern, ((Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber))));
				string input2 = text;
				string pattern2 = "&#(\\d+);";
				
				text = Regex.Replace(input2, pattern2, ((Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value))));
				text = text.Replace("<br>", "\n");
				if (OriginalText.ToUpper() == OriginalText)
				{
					text = text.ToUpper();
				}
				else if (GoogleTranslation.UppercaseFirst(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.UppercaseFirst(text);
				}
				else if (GoogleTranslation.TitleCase(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.TitleCase(text);
				}
				result = text;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				result = string.Empty;
			}
			return result;
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
				return string.Empty;
			}
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}
	}
}
