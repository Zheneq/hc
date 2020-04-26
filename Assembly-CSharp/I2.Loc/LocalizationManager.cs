using ArabicSupport;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		public delegate void OnLocalizeCallback();

		private static string mCurrentLanguage;

		private static string mLanguageCode;

		public static bool IsRight2Left = false;

		public static bool mGibberishMode = false;

		public static List<LanguageSource> Sources = new List<LanguageSource>();

		public static string[] GlobalSources = new string[15]
		{
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

		private static string[] LanguagesRTL = new string[20]
		{
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
				while (true)
				{
					if (mCurrentLanguage != supportedLanguage)
					{
						SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage));
					}
					return;
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
				if (!(mLanguageCode != value))
				{
					return;
				}
				string languageFromCode = GetLanguageFromCode(value);
				if (string.IsNullOrEmpty(languageFromCode))
				{
					return;
				}
				while (true)
				{
					SetLanguageAndCode(languageFromCode, value);
					return;
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
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return currentLanguage.Substring(num + 1);
						}
					}
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0)
				{
					if (num != num2)
					{
						return currentLanguage.Substring(num + 1, num2 - num - 1);
					}
				}
				return string.Empty;
			}
			set
			{
				string text = CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0)
				{
					if (num != num2)
					{
						text = text.Substring(num);
					}
				}
				CurrentLanguage = text + "(" + value + ")";
			}
		}

		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				return (num >= 0) ? currentLanguageCode.Substring(num + 1) : string.Empty;
			}
			set
			{
				string text = CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				CurrentLanguageCode = text + "-" + value;
			}
		}

		public static bool GibberishMode
		{
			get
			{
				return mGibberishMode;
			}
			set
			{
				mGibberishMode = value;
				LocalizeAll(true);
			}
		}

		private static OnLocalizeCallback OnLocalizeEventHolder;
		public static event OnLocalizeCallback OnLocalizeEvent
		{
			add
			{
				OnLocalizeCallback onLocalizeCallback = LocalizationManager.OnLocalizeEventHolder;
				OnLocalizeCallback onLocalizeCallback2;
				do
				{
					onLocalizeCallback2 = onLocalizeCallback;
					onLocalizeCallback = Interlocked.CompareExchange(ref LocalizationManager.OnLocalizeEventHolder, (OnLocalizeCallback)Delegate.Combine(onLocalizeCallback2, value), onLocalizeCallback);
				}
				while ((object)onLocalizeCallback != onLocalizeCallback2);
				while (true)
				{
					return;
				}
			}
			remove
			{
				OnLocalizeCallback onLocalizeCallback = LocalizationManager.OnLocalizeEventHolder;
				OnLocalizeCallback onLocalizeCallback2;
				do
				{
					onLocalizeCallback2 = onLocalizeCallback;
					onLocalizeCallback = Interlocked.CompareExchange(ref LocalizationManager.OnLocalizeEventHolder, (OnLocalizeCallback)Delegate.Remove(onLocalizeCallback2, value), onLocalizeCallback);
				}
				while ((object)onLocalizeCallback != onLocalizeCallback2);
			}
		}

		public static void InitializeIfNeeded()
		{
			if (!string.IsNullOrEmpty(mCurrentLanguage))
			{
				return;
			}
			while (true)
			{
				UpdateSources();
				SelectStartupLanguage();
				return;
			}
		}

		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (!(mCurrentLanguage != LanguageName))
			{
				if (!(mLanguageCode != LanguageCode))
				{
					if (!Force)
					{
						return;
					}
				}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						string @string = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
						if (!@string.IsNullOrEmpty())
						{
							CurrentLanguageCode = @string.ToLower();
						}
						else
						{
							Log.Warning("Failed to set custom language");
							CurrentLanguageCode = glyphLanguageCode;
						}
						return;
					}
					}
				}
			}
			CurrentLanguageCode = glyphLanguageCode;
		}

		private static void SelectStartupLanguage()
		{
			string @string = PlayerPrefs.GetString("I2 Language", string.Empty);
			string text = Application.systemLanguage.ToString();
			if (text == "ChineseSimplified")
			{
				text = "Chinese (Simplified)";
			}
			if (text == "ChineseTraditional")
			{
				text = "Chinese (Traditional)";
			}
			if (HasLanguage(@string, true, false))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						CurrentLanguage = @string;
						return;
					}
				}
			}
			string supportedLanguage = GetSupportedLanguage(text);
			if (!string.IsNullOrEmpty(supportedLanguage))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						SetLanguageAndCode(supportedLanguage, GetLanguageCode(supportedLanguage), false);
						return;
					}
				}
			}
			int num = 0;
			int count = Sources.Count;
			while (true)
			{
				if (num < count)
				{
					if (Sources[num].mLanguages.Count > 0)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			SetLanguageAndCode(Sources[num].mLanguages[0].Name, Sources[num].mLanguages[0].Code, false);
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
			if (TryGetTermTranslation(Term, out string Translation, FixForRTL, maxLineLengthForRTL))
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			InitializeIfNeeded();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (!Sources[i].TryGetTermTranslation(Term, out Translation))
				{
					continue;
				}
				while (true)
				{
					if (IsRight2Left)
					{
						if (FixForRTL)
						{
							Translation = ApplyRTLfix(Translation, maxLineLengthForRTL);
						}
					}
					if (mGibberishMode)
					{
						if (Term.IndexOf("@TEXTURE") == -1)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									Translation = Term;
									return true;
								}
							}
						}
					}
					return true;
				}
			}
			while (true)
			{
				return false;
			}
		}

		public static string ApplyRTLfix(string line)
		{
			return ApplyRTLfix(line, 0);
		}

		public static string ApplyRTLfix(string line, int maxCharacters)
		{
			if (maxCharacters <= 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return ArabicFixer.Fix(line);
					}
				}
			}
			Regex regex = new Regex(".{0," + maxCharacters + "}(\\s+|$)", RegexOptions.Multiline);
			line = regex.Replace(line, "$0\n");
			if (line.EndsWith("\n\n"))
			{
				line = line.Substring(0, line.Length - 2);
			}
			string[] array = line.Split('\n');
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				array[i] = ArabicFixer.Fix(array[i]);
			}
			while (true)
			{
				line = string.Join("\n", array);
				return line;
			}
		}

		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0)
		{
			if (IsRight2Left)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return ApplyRTLfix(text, maxCharacters);
					}
				}
			}
			return text;
		}

		internal static void LocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				Localize localize = array[i];
				localize.OnLocalize(Force);
			}
			while (true)
			{
				if (LocalizationManager.OnLocalizeEventHolder != null)
				{
					LocalizationManager.OnLocalizeEventHolder();
				}
				ResourceManager.pInstance.CleanResourceCache();
				return;
			}
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
			for (int num = Sources.Count - 1; num >= 0; num--)
			{
				if (Sources[num] == null)
				{
					RemoveSource(Sources[num]);
				}
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		private static void RegisterSceneSources()
		{
			LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			int i = 0;
			for (int num = array.Length; i < num; i++)
			{
				if (!Sources.Contains(array[i]))
				{
					AddSource(array[i]);
				}
			}
			while (true)
			{
				return;
			}
		}

		private static void RegisterSourceInResources()
		{
			string[] globalSources = GlobalSources;
			foreach (string name in globalSources)
			{
				GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>(name);
				object obj;
				if ((bool)asset)
				{
					obj = asset.GetComponent<LanguageSource>();
				}
				else
				{
					obj = null;
				}
				LanguageSource languageSource = (LanguageSource)obj;
				if (!languageSource)
				{
					continue;
				}
				if (!Sources.Contains(languageSource))
				{
					AddSource(languageSource);
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}

		internal static void AddSource(LanguageSource Source)
		{
			if (Sources.Contains(Source))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
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
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				if (Sources[i].GetLanguageIndex(Language, false) < 0)
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				for (int count2 = Sources.Count; j < count2; j++)
				{
					if (Sources[j].GetLanguageIndex(Language) < 0)
					{
						continue;
					}
					while (true)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string GetSupportedLanguage(string Language)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndex = Sources[i].GetLanguageIndex(Language, false);
				if (languageIndex < 0)
				{
					continue;
				}
				while (true)
				{
					return Sources[i].mLanguages[languageIndex].Name;
				}
			}
			while (true)
			{
				int j = 0;
				for (int count2 = Sources.Count; j < count2; j++)
				{
					int languageIndex2 = Sources[j].GetLanguageIndex(Language);
					if (languageIndex2 < 0)
					{
						continue;
					}
					while (true)
					{
						return Sources[j].mLanguages[languageIndex2].Name;
					}
				}
				while (true)
				{
					return string.Empty;
				}
			}
		}

		public static string GetLanguageCode(string Language)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndex = Sources[i].GetLanguageIndex(Language);
				if (languageIndex < 0)
				{
					continue;
				}
				while (true)
				{
					return Sources[i].mLanguages[languageIndex].Code;
				}
			}
			while (true)
			{
				return string.Empty;
			}
		}

		public static string GetLanguageFromCode(string Code)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int languageIndexFromCode = Sources[i].GetLanguageIndexFromCode(Code);
				if (languageIndexFromCode >= 0)
				{
					return Sources[i].mLanguages[languageIndexFromCode].Name;
				}
			}
			return string.Empty;
		}

		public static List<string> GetAllLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				int j = 0;
				for (int count2 = Sources[i].mLanguages.Count; j < count2; j++)
				{
					if (!list.Contains(Sources[i].mLanguages[j].Name))
					{
						list.Add(Sources[i].mLanguages[j].Name);
					}
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_00a3;
					}
					continue;
					end_IL_00a3:
					break;
				}
			}
			while (true)
			{
				return list;
			}
		}

		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				Sources[i].GetCategories(false, list);
			}
			while (true)
			{
				return list;
			}
		}

		public static List<string> GetTermsList()
		{
			if (Sources.Count == 0)
			{
				UpdateSources();
			}
			if (Sources.Count == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return Sources[0].GetTermsList();
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				hashSet.UnionWith(Sources[i].GetTermsList());
			}
			while (true)
			{
				return new List<string>(hashSet);
			}
		}

		public static TermData GetTermData(string term)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				TermData termData = Sources[i].GetTermData(term);
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
				int i = 0;
				for (int count = Sources.Count; i < count; i++)
				{
					if (Sources[i].GetTermData(term) != null)
					{
						return Sources[i];
					}
				}
			}
			object result;
			if (fallbackToFirst)
			{
				if (Sources.Count > 0)
				{
					result = Sources[0];
					goto IL_009c;
				}
			}
			result = null;
			goto IL_009c;
			IL_009c:
			return (LanguageSource)result;
		}

		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			for (int count = Sources.Count; i < count; i++)
			{
				UnityEngine.Object @object = Sources[i].FindAsset(value);
				if (!@object)
				{
					continue;
				}
				while (true)
				{
					return @object;
				}
			}
			while (true)
			{
				return null;
			}
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
