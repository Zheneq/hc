using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using ArabicSupport;
using UnityEngine;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		private static string mCurrentLanguage;

		private static string mLanguageCode;

		public static bool IsRight2Left = false;

		public static bool mGibberishMode = false;

		public static List<LanguageSource> Sources = new List<LanguageSource>();

		public static string[] GlobalSources = new string[]
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

		private static string[] LanguagesRTL = new string[]
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
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value);
				if (!string.IsNullOrEmpty(supportedLanguage))
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.set_CurrentLanguage(string)).MethodHandle;
					}
					if (LocalizationManager.mCurrentLanguage != supportedLanguage)
					{
						LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
					}
				}
			}
		}

		public static string CurrentLanguageCode
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				if (LocalizationManager.mLanguageCode != value)
				{
					string languageFromCode = LocalizationManager.GetLanguageFromCode(value);
					if (!string.IsNullOrEmpty(languageFromCode))
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.set_CurrentLanguageCode(string)).MethodHandle;
						}
						LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
					}
				}
			}
		}

		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = LocalizationManager.CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.get_CurrentRegion()).MethodHandle;
					}
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0)
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
					if (num != num2)
					{
						return currentLanguage.Substring(num + 1, num2 - num - 1);
					}
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.set_CurrentRegion(string)).MethodHandle;
					}
					if (num != num2)
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
						text = text.Substring(num);
					}
				}
				LocalizationManager.CurrentLanguage = text + "(" + value + ")";
			}
		}

		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				return (num >= 0) ? currentLanguageCode.Substring(num + 1) : string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.set_CurrentRegionCode(string)).MethodHandle;
					}
					text = text.Substring(0, num);
				}
				LocalizationManager.CurrentLanguageCode = text + "-" + value;
			}
		}

		public static bool GibberishMode
		{
			get
			{
				return LocalizationManager.mGibberishMode;
			}
			set
			{
				LocalizationManager.mGibberishMode = value;
				LocalizationManager.LocalizeAll(true);
			}
		}

		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.InitializeIfNeeded()).MethodHandle;
				}
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (!(LocalizationManager.mCurrentLanguage != LanguageName))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.SetLanguageAndCode(string, string, bool, bool)).MethodHandle;
				}
				if (!(LocalizationManager.mLanguageCode != LanguageCode))
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
					if (!Force)
					{
						return;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (RememberLanguage)
			{
				PlayerPrefs.SetString("I2 Language", LanguageName);
			}
			LocalizationManager.mCurrentLanguage = LanguageName;
			LocalizationManager.mLanguageCode = LanguageCode;
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.LocalizeAll(Force);
		}

		public static void SetBootupLanguage(string glyphLanguageCode)
		{
			LocalizationManager.UpdateSources();
			if (PlayerPrefs.GetInt("OptionsOverrideGlyphLanguage", 0) == 1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.SetBootupLanguage(string)).MethodHandle;
				}
				string @string = PlayerPrefs.GetString("OverrideGlyphLanguageCode", string.Empty);
				if (!@string.IsNullOrEmpty())
				{
					LocalizationManager.CurrentLanguageCode = @string.ToLower();
				}
				else
				{
					Log.Warning("Failed to set custom language", new object[0]);
					LocalizationManager.CurrentLanguageCode = glyphLanguageCode;
				}
			}
			else
			{
				LocalizationManager.CurrentLanguageCode = glyphLanguageCode;
			}
		}

		private static void SelectStartupLanguage()
		{
			string @string = PlayerPrefs.GetString("I2 Language", string.Empty);
			string text = Application.systemLanguage.ToString();
			if (text == "ChineseSimplified")
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.SelectStartupLanguage()).MethodHandle;
				}
				text = "Chinese (Simplified)";
			}
			if (text == "ChineseTraditional")
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
				text = "Chinese (Traditional)";
			}
			if (LocalizationManager.HasLanguage(@string, true, false))
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
				LocalizationManager.CurrentLanguage = @string;
				return;
			}
			string supportedLanguage = LocalizationManager.GetSupportedLanguage(text);
			if (!string.IsNullOrEmpty(supportedLanguage))
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
				LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
				return;
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[0].Name, LocalizationManager.Sources[i].mLanguages[0].Code, false, false);
					return;
				}
				i++;
			}
		}

		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent
		{
			add
			{
				LocalizationManager.OnLocalizeCallback onLocalizeCallback = LocalizationManager.OnLocalizeEvent;
				LocalizationManager.OnLocalizeCallback onLocalizeCallback2;
				do
				{
					onLocalizeCallback2 = onLocalizeCallback;
					onLocalizeCallback = Interlocked.CompareExchange<LocalizationManager.OnLocalizeCallback>(ref LocalizationManager.OnLocalizeEvent, (LocalizationManager.OnLocalizeCallback)Delegate.Combine(onLocalizeCallback2, value), onLocalizeCallback);
				}
				while (onLocalizeCallback != onLocalizeCallback2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.add_OnLocalizeEvent(LocalizationManager.OnLocalizeCallback)).MethodHandle;
				}
			}
			remove
			{
				LocalizationManager.OnLocalizeCallback onLocalizeCallback = LocalizationManager.OnLocalizeEvent;
				LocalizationManager.OnLocalizeCallback onLocalizeCallback2;
				do
				{
					onLocalizeCallback2 = onLocalizeCallback;
					onLocalizeCallback = Interlocked.CompareExchange<LocalizationManager.OnLocalizeCallback>(ref LocalizationManager.OnLocalizeEvent, (LocalizationManager.OnLocalizeCallback)Delegate.Remove(onLocalizeCallback2, value), onLocalizeCallback);
				}
				while (onLocalizeCallback != onLocalizeCallback2);
			}
		}

		public static string GetTermTranslation(string Term)
		{
			return LocalizationManager.GetTermTranslation(Term, false, 0);
		}

		public static string GetTermTranslation(string Term, bool FixForRTL)
		{
			return LocalizationManager.GetTermTranslation(Term, FixForRTL, 0);
		}

		public static string GetTermTranslation(string Term, bool FixForRTL, int maxLineLengthForRTL)
		{
			string result;
			if (LocalizationManager.TryGetTermTranslation(Term, out result, FixForRTL, maxLineLengthForRTL))
			{
				return result;
			}
			return string.Empty;
		}

		public static bool TryGetTermTranslation(string Term, out string Translation)
		{
			return LocalizationManager.TryGetTermTranslation(Term, out Translation, false, 0);
		}

		public static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL)
		{
			return LocalizationManager.TryGetTermTranslation(Term, out Translation, FixForRTL, 0);
		}

		public unsafe static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL, int maxLineLengthForRTL)
		{
			Translation = string.Empty;
			if (string.IsNullOrEmpty(Term))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.TryGetTermTranslation(string, string*, bool, int)).MethodHandle;
				}
				return false;
			}
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].TryGetTermTranslation(Term, out Translation))
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
					if (LocalizationManager.IsRight2Left)
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
						if (FixForRTL)
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
							Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL);
						}
					}
					if (LocalizationManager.mGibberishMode)
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
						if (Term.IndexOf("@TEXTURE") == -1)
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
							Translation = Term;
							return true;
						}
					}
					return true;
				}
				i++;
			}
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

		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0);
		}

		public static string ApplyRTLfix(string line, int maxCharacters)
		{
			if (maxCharacters <= 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.ApplyRTLfix(string, int)).MethodHandle;
				}
				return ArabicFixer.Fix(line);
			}
			Regex regex = new Regex(".{0," + maxCharacters + "}(\\s+|$)", RegexOptions.Multiline);
			line = regex.Replace(line, "$0\n");
			if (line.EndsWith("\n\n"))
			{
				line = line.Substring(0, line.Length - 2);
			}
			string[] array = line.Split(new char[]
			{
				'\n'
			});
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i] = ArabicFixer.Fix(array[i]);
				i++;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			line = string.Join("\n", array);
			return line;
		}

		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0)
		{
			if (LocalizationManager.IsRight2Left)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.FixRTL_IfNeeded(string, int)).MethodHandle;
				}
				return LocalizationManager.ApplyRTLfix(text, maxCharacters);
			}
			return text;
		}

		internal static void LocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				Localize localize = array[i];
				localize.OnLocalize(Force);
				i++;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.LocalizeAll(bool)).MethodHandle;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
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
				LocalizationManager.OnLocalizeEvent();
			}
			ResourceManager.pInstance.CleanResourceCache();
		}

		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		private static void UnregisterDeletededSources()
		{
			for (int i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
			{
				if (LocalizationManager.Sources[i] == null)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.UnregisterDeletededSources()).MethodHandle;
					}
					LocalizationManager.RemoveSource(LocalizationManager.Sources[i]);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		private static void RegisterSceneSources()
		{
			LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				if (!LocalizationManager.Sources.Contains(array[i]))
				{
					LocalizationManager.AddSource(array[i]);
				}
				i++;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.RegisterSceneSources()).MethodHandle;
			}
		}

		private static void RegisterSourceInResources()
		{
			foreach (string name in LocalizationManager.GlobalSources)
			{
				GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>(name);
				LanguageSource languageSource;
				if (asset)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.RegisterSourceInResources()).MethodHandle;
					}
					languageSource = asset.GetComponent<LanguageSource>();
				}
				else
				{
					languageSource = null;
				}
				LanguageSource languageSource2 = languageSource;
				if (languageSource2)
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
					if (!LocalizationManager.Sources.Contains(languageSource2))
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
						LocalizationManager.AddSource(languageSource2);
					}
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		internal static void AddSource(LanguageSource Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.AddSource(LanguageSource)).MethodHandle;
				}
				return;
			}
			LocalizationManager.Sources.Add(Source);
			Source.Import_Google(false);
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		internal static void RemoveSource(LanguageSource Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true)
		{
			if (Initialize)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.HasLanguage(string, bool, bool)).MethodHandle;
				}
				LocalizationManager.InitializeIfNeeded();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false) >= 0)
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
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
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
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true) >= 0)
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
						return true;
					}
					j++;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return false;
		}

		public static string GetSupportedLanguage(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, false);
				if (languageIndex >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetSupportedLanguage(string)).MethodHandle;
					}
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Name;
				}
				i++;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			int j = 0;
			int count2 = LocalizationManager.Sources.Count;
			while (j < count2)
			{
				int languageIndex2 = LocalizationManager.Sources[j].GetLanguageIndex(Language, true);
				if (languageIndex2 >= 0)
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
					return LocalizationManager.Sources[j].mLanguages[languageIndex2].Name;
				}
				j++;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return string.Empty;
		}

		public static string GetLanguageCode(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true);
				if (languageIndex >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetLanguageCode(string)).MethodHandle;
					}
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
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
			return string.Empty;
		}

		public static string GetLanguageFromCode(string Code)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code);
				if (languageIndexFromCode >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
				}
				i++;
			}
			return string.Empty;
		}

		public static List<string> GetAllLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources[i].mLanguages.Count;
				while (j < count2)
				{
					if (!list.Contains(LocalizationManager.Sources[i].mLanguages[j].Name))
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetAllLanguages()).MethodHandle;
						}
						list.Add(LocalizationManager.Sources[i].mLanguages[j].Name);
					}
					j++;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				i++;
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
			return list;
		}

		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].GetCategories(false, list);
				i++;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetCategories()).MethodHandle;
			}
			return list;
		}

		public static List<string> GetTermsList()
		{
			if (LocalizationManager.Sources.Count == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetTermsList()).MethodHandle;
				}
				LocalizationManager.UpdateSources();
			}
			if (LocalizationManager.Sources.Count == 1)
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
				return LocalizationManager.Sources[0].GetTermsList();
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList());
				i++;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return new List<string>(hashSet);
		}

		public static TermData GetTermData(string term)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					return termData;
				}
				i++;
			}
			return null;
		}

		public static LanguageSource GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.GetSourceContaining(string, bool)).MethodHandle;
				}
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
					{
						return LocalizationManager.Sources[i];
					}
					i++;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (fallbackToFirst)
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
				if (LocalizationManager.Sources.Count > 0)
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
					return LocalizationManager.Sources[0];
				}
			}
			return null;
		}

		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationManager.FindAsset(string)).MethodHandle;
					}
					return @object;
				}
				i++;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		public delegate void OnLocalizeCallback();
	}
}
