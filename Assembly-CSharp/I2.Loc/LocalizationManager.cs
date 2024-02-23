using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using UnityEngine;
using Object = UnityEngine.Object;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		public delegate void OnLocalizeCallback();

		private static string mCurrentLanguage;
		private static string mLanguageCode;
		public static bool IsRight2Left;
		public static bool mGibberishMode;
		public static List<LanguageSource> Sources = new List<LanguageSource>();

		public static string[] GlobalSources = {
			"I2Languages",
			"QuestWideLoc",
			"OptionsLoc",
			"InventoryWideLoc",
			"SeasonWideLoc",
			"GameWideLoc",
			"AbilitiesLoc",
			"SlashCommandsLoc",
			"SceneGlobal",
			"NewFrontEndLoc",
			"TutorialLoc",
			"UIOverconDataLoc",
			"FreelancerStatsLoc",
			"LoreWideLoc",
			"GameSubTypeLoc"
		};

		private static string[] LanguagesRTL = {
			"ar-DZ",
			"ar",
			"ar-BH",
			"ar-EG",
			"ar-IQ",
			"ar-JO",
			"ar-KW",
			"ar-LB",
			"ar-LY",
			"ar-MA",
			"ar-OM",
			"ar-QA",
			"ar-SA",
			"ar-SY",
			"ar-TN",
			"ar-AE",
			"ar-YE",
			"he",
			"ur",
			"ji"
		};

		public static string CurrentLanguage
		{
			get
			{
				InitializeIfNeeded();
				return mCurrentLanguage;
			}
			set
			{
				string supportedLanguage = GetSupportedLanguage(value);
				if (string.IsNullOrEmpty(supportedLanguage))
				{
					return;
				}
				if (mCurrentLanguage != supportedLanguage)
				{
					SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage));
				}
			}
		}

		public static string CurrentLanguageCode
		{
			get
			{
				InitializeIfNeeded();
				return mLanguageCode;
			}
			set
			{
				if (mLanguageCode == value)
				{
					return;
				}
				string languageFromCode = GetLanguageFromCode(value);
				if (!string.IsNullOrEmpty(languageFromCode))
				{
					SetLanguageAndCode(languageFromCode, value);
				}
			}
		}

		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					CurrentLanguage = new StringBuilder().Append(text.Substring(num + 1)).Append(value).ToString();
				}
				else
				{
					num = text.IndexOfAny("[(".ToCharArray());
					int num2 = text.LastIndexOfAny("])".ToCharArray());
					if (num > 0 && num != num2)
					{
						text = text.Substring(num);
					}

					CurrentLanguage = new StringBuilder().Append(text).Append("(").Append(value).Append(")").ToString();
				}
			}
		}

		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				return num >= 0
					? currentLanguageCode.Substring(num + 1)
					: string.Empty;
			}
			set
			{
				string currentLanguageCode = CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					currentLanguageCode = currentLanguageCode.Substring(0, num);
				}

				CurrentLanguageCode = new StringBuilder().Append(currentLanguageCode).Append("-").Append(value).ToString();
			}
		}

		public static bool GibberishMode
		{
			get { return mGibberishMode; }
			set
			{
				mGibberishMode = value;
				LocalizeAll(true);
			}
		}

		public static event OnLocalizeCallback OnLocalizeEvent;

		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(mCurrentLanguage))
			{
				UpdateSources();
				SelectStartupLanguage();
			}
		}

		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (mCurrentLanguage == LanguageName && mLanguageCode == LanguageCode && !Force)
			{
				return;
			}
			if (RememberLanguage)
			{
				PlayerPrefs.SetString("I2 Language", LanguageName);
			}
			mCurrentLanguage = LanguageName;
			mLanguageCode = LanguageCode;
			IsRight2Left = IsRTL(mLanguageCode);
			LocalizeAll(Force);
		}

		public static void SetBootupLanguage(string glyphLanguageCode)
		{
			UpdateSources();
			if (PlayerPrefs.GetInt("OptionsOverrideGlyphLanguage", 0) == 1)
			{
				string langOverride = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
				if (!langOverride.IsNullOrEmpty())
				{
					CurrentLanguageCode = langOverride.ToLower();
				}
				else
				{
					Log.Warning("Failed to set custom language");
					CurrentLanguageCode = glyphLanguageCode;
				}
			}
			else
			{
				CurrentLanguageCode = glyphLanguageCode;
			}
		}

		private static void SelectStartupLanguage()
		{
			string lang = PlayerPrefs.GetString("I2 Language", string.Empty);
			string text = Application.systemLanguage.ToString();
			if (text == "ChineseSimplified")
			{
				text = "Chinese (Simplified)";
			}
			if (text == "ChineseTraditional")
			{
				text = "Chinese (Traditional)";
			}
			if (HasLanguage(lang, true, false))
			{
				CurrentLanguage = lang;
				return;
			}
			string supportedLanguage = GetSupportedLanguage(text);
			if (!string.IsNullOrEmpty(supportedLanguage))
			{
				SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage), false);
				return;
			}
			
			int count = Sources.Count;
			for (int num = 0; num < count; num++) 
			{
				if (Sources[num].mLanguages.Count > 0)
				{
					SetLanguageAndCode(Sources[num].mLanguages[0].Name, Sources[num].mLanguages[0].Code, false);
					return;
				}
			}
		}

		public static string GetTermTranslation(string Term)
		{
			return GetTermTranslation(Term, false, 0);
		}

		public static string GetTermTranslation(string Term, bool FixForRTL)
		{
			return GetTermTranslation(Term, FixForRTL, 0);
		}

		public static string GetTermTranslation(string Term, bool FixForRTL, int maxLineLengthForRTL)
		{
			string Translation;
			if (TryGetTermTranslation(Term, out Translation, FixForRTL, maxLineLengthForRTL))
			{
				return Translation;
			}
			return string.Empty;
		}

		public static bool TryGetTermTranslation(string Term, out string Translation)
		{
			return TryGetTermTranslation(Term, out Translation, false, 0);
		}

		public static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL)
		{
			return TryGetTermTranslation(Term, out Translation, FixForRTL, 0);
		}

		public static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL, int maxLineLengthForRTL)
		{
			Translation = string.Empty;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			InitializeIfNeeded();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (!Sources[i].TryGetTermTranslation(Term, out Translation))
				{
					continue;
				}
				if (IsRight2Left && FixForRTL)
				{
					Translation = ApplyRTLfix(Translation, maxLineLengthForRTL);
				}
				if (mGibberishMode && Term.IndexOf("@TEXTURE") == -1)
				{
					Translation = Term;
				}
				return true;
			}
			return false;
		}

		public static string ApplyRTLfix(string line)
		{
			return ApplyRTLfix(line, 0);
		}

		public static string ApplyRTLfix(string line, int maxCharacters)
		{
			if (maxCharacters <= 0)
			{
				return ArabicFixer.Fix(line);
			}
			Regex regex = new Regex(new StringBuilder().Append(".{0,").Append(maxCharacters).Append("}(\\s+|$)").ToString(), RegexOptions.Multiline);
			line = regex.Replace(line, "$0\n");
			if (line.EndsWith("\n\n"))
			{
				line = line.Substring(0, line.Length - 2);
			}
			string[] array = line.Split('\n');
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ArabicFixer.Fix(array[i]);
			}
			line = string.Join("\n", array);
			return line;
		}

		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0)
		{
			return IsRight2Left
				? ApplyRTLfix(text, maxCharacters)
				: text;
		}

		internal static void LocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			foreach (Localize el in array)
			{
				el.OnLocalize(Force);
			}
			if (OnLocalizeEvent != null) OnLocalizeEvent.Invoke();
			ResourceManager.pInstance.CleanResourceCache();
		}

		public static bool UpdateSources()
		{
			UnregisterDeletededSources();
			RegisterSourceInResources();
			RegisterSceneSources();
			return Sources.Count > 0;
		}

		private static void UnregisterDeletededSources()
		{
			for (int i = Sources.Count - 1; i >= 0; i--)
			{
				if (Sources[i] == null)
				{
					RemoveSource(Sources[i]);
				}
			}
		}

		private static void RegisterSceneSources()
		{
			LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			foreach (LanguageSource src in array)
			{
				if (!Sources.Contains(src))
				{
					AddSource(src);
				}
			}
		}

		private static void RegisterSourceInResources()
		{
			foreach (string name in GlobalSources)
			{
				GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>(name);
				LanguageSource languageSource = asset != null
					? asset.GetComponent<LanguageSource>()
					: null;
				if (languageSource != null && !Sources.Contains(languageSource))
				{
					AddSource(languageSource);
				}
			}
		}

		internal static void AddSource(LanguageSource Source)
		{
			if (Sources.Contains(Source))
			{
				return;
			}
			Sources.Add(Source);
			Source.Import_Google();
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		internal static void RemoveSource(LanguageSource Source)
		{
			Sources.Remove(Source);
		}

		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf(GlobalSources, SourceName) >= 0;
		}

		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true)
		{
			if (Initialize)
			{
				InitializeIfNeeded();
			}

			foreach (LanguageSource src in Sources)
			{
				if (src.GetLanguageIndex(Language, false) >= 0)
				{
					return true;
				}
			}
			
			if (AllowDiscartingRegion)
			{
				foreach (LanguageSource src in Sources)
				{
					if (src.GetLanguageIndex(Language) >= 0)
					{
						return true;
					}
				}
			}
			
			return false;
		}

		public static string GetSupportedLanguage(string Language)
		{
			foreach (LanguageSource src in Sources)
			{
				int idx = src.GetLanguageIndex(Language, false);
				if (idx >= 0)
				{
					return src.mLanguages[idx].Name;
				}
			}

			foreach (LanguageSource src in Sources)
			{
				int idx = src.GetLanguageIndex(Language);
				if (idx >= 0)
				{
					return src.mLanguages[idx].Name;
				}
			}
			
			return string.Empty;
		}

		public static string GetLanguageCode(string Language)
		{
			foreach (LanguageSource src in Sources)
			{
				int idx = src.GetLanguageIndex(Language);
				if (idx >= 0)
				{
					return src.mLanguages[idx].Code;
				}
			}

			return string.Empty;
		}

		public static string GetLanguageFromCode(string Code)
		{
			foreach (LanguageSource src in Sources)
			{
				int idx = src.GetLanguageIndexFromCode(Code);
				if (idx >= 0)
				{
					return src.mLanguages[idx].Name;
				}
			}

			return string.Empty;
		}

		public static List<string> GetAllLanguages()
		{
			List<string> list = new List<string>();
			foreach (LanguageSource src in Sources)
			{
				foreach (LanguageData lang in src.mLanguages)
				{
					if (!list.Contains(lang.Name))
					{
						list.Add(lang.Name);
					}
				}
			}
			return list;
		}

		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			foreach (LanguageSource src in Sources)
			{
				src.GetCategories(false, list);
			}
			return list;
		}

		public static List<string> GetTermsList()
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			if (Sources.Count == 1)
			{
				return Sources[0].GetTermsList();
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (LanguageSource src in Sources)
			{
				hashSet.UnionWith(src.GetTermsList());
			}
			return new List<string>(hashSet);
		}

		public static TermData GetTermData(string term)
		{
			foreach (LanguageSource src in Sources)
			{
				TermData termData = src.GetTermData(term);
				if (termData != null)
				{
					return termData;
				}
			}

			return null;
		}

		public static LanguageSource GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				foreach (LanguageSource src in Sources)
				{
					if (src.GetTermData(term) != null)
					{
						return src;
					}
				}
			}

			return fallbackToFirst && Sources.Count > 0
				? Sources[0]
				: null;
		}

		public static Object FindAsset(string value)
		{
			foreach (LanguageSource src in Sources)
			{
				Object asset = src.FindAsset(value);
				if (asset)
				{
					return asset;
				}
			}

			return null;
		}

		public static string GetVersion()
		{
			return "2.6.5 a2";
		}

		public static int GetRequiredWebServiceVersion()
		{
			return 3;
		}

		private static bool IsRTL(string Code)
		{
			return Array.IndexOf(LanguagesRTL, Code) >= 0;
		}
	}
}
