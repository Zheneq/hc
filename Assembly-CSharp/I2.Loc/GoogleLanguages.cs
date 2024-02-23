using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	public static class GoogleLanguages
	{
		public struct LanguageCodeDef
		{
			public string Code;

			public string GoogleCode;
		}

		public static Dictionary<string, LanguageCodeDef> mLanguageDef = new Dictionary<string, LanguageCodeDef>
		{
			{
				"Afrikaans",
				new LanguageCodeDef
				{
					Code = "af"
				}
			},
			{
				"Albanian",
				new LanguageCodeDef
				{
					Code = "sq"
				}
			},
			{
				"Arabic",
				new LanguageCodeDef
				{
					Code = "ar"
				}
			},
			{
				"Arabic/Algeria",
				new LanguageCodeDef
				{
					Code = "ar-DZ",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Bahrain",
				new LanguageCodeDef
				{
					Code = "ar-BH",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Egypt",
				new LanguageCodeDef
				{
					Code = "ar-EG",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Iraq",
				new LanguageCodeDef
				{
					Code = "ar-IQ",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Jordan",
				new LanguageCodeDef
				{
					Code = "ar-JO",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Kuwait",
				new LanguageCodeDef
				{
					Code = "ar-KW",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Lebanon",
				new LanguageCodeDef
				{
					Code = "ar-LB",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Libya",
				new LanguageCodeDef
				{
					Code = "ar-LY",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Morocco",
				new LanguageCodeDef
				{
					Code = "ar-MA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Oman",
				new LanguageCodeDef
				{
					Code = "ar-OM",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Qatar",
				new LanguageCodeDef
				{
					Code = "ar-QA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Saudi Arabia",
				new LanguageCodeDef
				{
					Code = "ar-SA",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Syria",
				new LanguageCodeDef
				{
					Code = "ar-SY",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Tunisia",
				new LanguageCodeDef
				{
					Code = "ar-TN",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/U.A.E.",
				new LanguageCodeDef
				{
					Code = "ar-AE",
					GoogleCode = "ar"
				}
			},
			{
				"Arabic/Yemen",
				new LanguageCodeDef
				{
					Code = "ar-YE",
					GoogleCode = "ar"
				}
			},
			{
				"Armenian",
				new LanguageCodeDef
				{
					Code = "hy"
				}
			},
			{
				"Azerbaijani",
				new LanguageCodeDef
				{
					Code = "az"
				}
			},
			{
				"Basque",
				new LanguageCodeDef
				{
					Code = "eu"
				}
			},
			{
				"Basque/Spain",
				new LanguageCodeDef
				{
					Code = "eu-ES",
					GoogleCode = "eu"
				}
			},
			{
				"Belarusian",
				new LanguageCodeDef
				{
					Code = "be"
				}
			},
			{
				"Bosnian",
				new LanguageCodeDef
				{
					Code = "bs"
				}
			},
			{
				"Bulgariaa",
				new LanguageCodeDef
				{
					Code = "bg"
				}
			},
			{
				"Catalan",
				new LanguageCodeDef
				{
					Code = "ca"
				}
			},
			{
				"Chinese",
				new LanguageCodeDef
				{
					Code = "zh",
					GoogleCode = "zh-CN"
				}
			},
			{
				"Chinese/Hong Kong",
				new LanguageCodeDef
				{
					Code = "zh-HK",
					GoogleCode = "zh-TW"
				}
			},
			{
				"Chinese/Macau",
				new LanguageCodeDef
				{
					Code = "zh-MO",
					GoogleCode = "zh-CN"
				}
			},
			{
				"Chinese/PRC",
				new LanguageCodeDef
				{
					Code = "zh-CN",
					GoogleCode = "zh-CN"
				}
			},
			{
				"Chinese/Simplified",
				new LanguageCodeDef
				{
					Code = "zh-CN",
					GoogleCode = "zh-CN"
				}
			},
			{
				"Chinese/Singapore",
				new LanguageCodeDef
				{
					Code = "zh-SG",
					GoogleCode = "zh-CN"
				}
			},
			{
				"Chinese/Taiwan",
				new LanguageCodeDef
				{
					Code = "zh-TW",
					GoogleCode = "zh-TW"
				}
			},
			{
				"Chinese/Traditional",
				new LanguageCodeDef
				{
					Code = "zh-TW",
					GoogleCode = "zh-TW"
				}
			},
			{
				"Croatian",
				new LanguageCodeDef
				{
					Code = "hr"
				}
			},
			{
				"Croatian/Bosnia and Herzegovina",
				new LanguageCodeDef
				{
					Code = "hr-BA",
					GoogleCode = "hr"
				}
			},
			{
				"Czech",
				new LanguageCodeDef
				{
					Code = "cs"
				}
			},
			{
				"Danish",
				new LanguageCodeDef
				{
					Code = "da"
				}
			},
			{
				"Dutch",
				new LanguageCodeDef
				{
					Code = "nl"
				}
			},
			{
				"Dutch/Belgium",
				new LanguageCodeDef
				{
					Code = "nl-BE",
					GoogleCode = "nl"
				}
			},
			{
				"Dutch/Netherlands",
				new LanguageCodeDef
				{
					Code = "nl-NL",
					GoogleCode = "nl"
				}
			},
			{
				"English",
				new LanguageCodeDef
				{
					Code = "en"
				}
			},
			{
				"English/Australia",
				new LanguageCodeDef
				{
					Code = "en-AU",
					GoogleCode = "en"
				}
			},
			{
				"English/Belize",
				new LanguageCodeDef
				{
					Code = "en-BZ",
					GoogleCode = "en"
				}
			},
			{
				"English/Canada",
				new LanguageCodeDef
				{
					Code = "en-CA",
					GoogleCode = "en"
				}
			},
			{
				"English/Caribbean",
				new LanguageCodeDef
				{
					Code = "en-CB",
					GoogleCode = "en"
				}
			},
			{
				"English/Ireland",
				new LanguageCodeDef
				{
					Code = "en-IE",
					GoogleCode = "en"
				}
			},
			{
				"English/Jamaica",
				new LanguageCodeDef
				{
					Code = "en-JM",
					GoogleCode = "en"
				}
			},
			{
				"English/New Zealand",
				new LanguageCodeDef
				{
					Code = "en-NZ",
					GoogleCode = "en"
				}
			},
			{
				"English/Republic of the Philippines",
				new LanguageCodeDef
				{
					Code = "en-PH",
					GoogleCode = "en"
				}
			},
			{
				"English/South Africa",
				new LanguageCodeDef
				{
					Code = "en-ZA",
					GoogleCode = "en"
				}
			},
			{
				"English/Trinidad",
				new LanguageCodeDef
				{
					Code = "en-TT",
					GoogleCode = "en"
				}
			},
			{
				"English/United Kingdom",
				new LanguageCodeDef
				{
					Code = "en-GB",
					GoogleCode = "en"
				}
			},
			{
				"English/United States",
				new LanguageCodeDef
				{
					Code = "en-US",
					GoogleCode = "en"
				}
			},
			{
				"English/Zimbabwe",
				new LanguageCodeDef
				{
					Code = "en-ZW",
					GoogleCode = "en"
				}
			},
			{
				"Esperanto",
				new LanguageCodeDef
				{
					Code = "eo"
				}
			},
			{
				"Estonian",
				new LanguageCodeDef
				{
					Code = "et"
				}
			},
			{
				"Faeroese",
				new LanguageCodeDef
				{
					Code = "fo"
				}
			},
			{
				"Filipino",
				new LanguageCodeDef
				{
					Code = "tl"
				}
			},
			{
				"Finnish",
				new LanguageCodeDef
				{
					Code = "fi"
				}
			},
			{
				"French",
				new LanguageCodeDef
				{
					Code = "fr"
				}
			},
			{
				"French/Belgium",
				new LanguageCodeDef
				{
					Code = "fr-BE",
					GoogleCode = "fr"
				}
			},
			{
				"French/Canada",
				new LanguageCodeDef
				{
					Code = "fr-CA",
					GoogleCode = "fr"
				}
			},
			{
				"French/France",
				new LanguageCodeDef
				{
					Code = "fr-FR",
					GoogleCode = "fr"
				}
			},
			{
				"French/Luxembourg",
				new LanguageCodeDef
				{
					Code = "fr-LU",
					GoogleCode = "fr"
				}
			},
			{
				"French/Principality of Monaco",
				new LanguageCodeDef
				{
					Code = "fr-MC",
					GoogleCode = "fr"
				}
			},
			{
				"French/Switzerland",
				new LanguageCodeDef
				{
					Code = "fr-CH",
					GoogleCode = "fr"
				}
			},
			{
				"Galician",
				new LanguageCodeDef
				{
					Code = "gl"
				}
			},
			{
				"Galician/Spain",
				new LanguageCodeDef
				{
					Code = "gl-ES",
					GoogleCode = "gl"
				}
			},
			{
				"Georgian",
				new LanguageCodeDef
				{
					Code = "ka"
				}
			},
			{
				"German",
				new LanguageCodeDef
				{
					Code = "de"
				}
			},
			{
				"German/Austria",
				new LanguageCodeDef
				{
					Code = "de-AT",
					GoogleCode = "de"
				}
			},
			{
				"German/Germany",
				new LanguageCodeDef
				{
					Code = "de-DE",
					GoogleCode = "de"
				}
			},
			{
				"German/Liechtenstein",
				new LanguageCodeDef
				{
					Code = "de-LI",
					GoogleCode = "de"
				}
			},
			{
				"German/Luxembourg",
				new LanguageCodeDef
				{
					Code = "de-LU",
					GoogleCode = "de"
				}
			},
			{
				"German/Switzerland",
				new LanguageCodeDef
				{
					Code = "de-CH",
					GoogleCode = "de"
				}
			},
			{
				"Greek",
				new LanguageCodeDef
				{
					Code = "el"
				}
			},
			{
				"Gujarati",
				new LanguageCodeDef
				{
					Code = "gu"
				}
			},
			{
				"Hebrew",
				new LanguageCodeDef
				{
					Code = "he",
					GoogleCode = "iw"
				}
			},
			{
				"Hindi",
				new LanguageCodeDef
				{
					Code = "hi"
				}
			},
			{
				"Hungarian",
				new LanguageCodeDef
				{
					Code = "hu"
				}
			},
			{
				"Icelandic",
				new LanguageCodeDef
				{
					Code = "is"
				}
			},
			{
				"Indonesian",
				new LanguageCodeDef
				{
					Code = "id"
				}
			},
			{
				"Irish",
				new LanguageCodeDef
				{
					Code = "ga"
				}
			},
			{
				"Italian",
				new LanguageCodeDef
				{
					Code = "it"
				}
			},
			{
				"Italian/Italy",
				new LanguageCodeDef
				{
					Code = "it-IT",
					GoogleCode = "it"
				}
			},
			{
				"Italian/Switzerland",
				new LanguageCodeDef
				{
					Code = "it-CH",
					GoogleCode = "it"
				}
			},
			{
				"Japanese",
				new LanguageCodeDef
				{
					Code = "ja"
				}
			},
			{
				"Kannada",
				new LanguageCodeDef
				{
					Code = "kn"
				}
			},
			{
				"Kazakh",
				new LanguageCodeDef
				{
					Code = "kk"
				}
			},
			{
				"Korean",
				new LanguageCodeDef
				{
					Code = "ko"
				}
			},
			{
				"Kurdish",
				new LanguageCodeDef
				{
					Code = "ku"
				}
			},
			{
				"Kyrgyz",
				new LanguageCodeDef
				{
					Code = "ky"
				}
			},
			{
				"Latin",
				new LanguageCodeDef
				{
					Code = "la"
				}
			},
			{
				"Latvian",
				new LanguageCodeDef
				{
					Code = "lv"
				}
			},
			{
				"Lithuanian",
				new LanguageCodeDef
				{
					Code = "lt"
				}
			},
			{
				"Macedonian",
				new LanguageCodeDef
				{
					Code = "mk"
				}
			},
			{
				"Malay",
				new LanguageCodeDef
				{
					Code = "ms"
				}
			},
			{
				"Malay/Brunei Darussalam",
				new LanguageCodeDef
				{
					Code = "ms-BN",
					GoogleCode = "ms"
				}
			},
			{
				"Malay/Malaysia",
				new LanguageCodeDef
				{
					Code = "ms-MY",
					GoogleCode = "ms"
				}
			},
			{
				"Malayalam",
				new LanguageCodeDef
				{
					Code = "ml"
				}
			},
			{
				"Maltese",
				new LanguageCodeDef
				{
					Code = "mt"
				}
			},
			{
				"Maori",
				new LanguageCodeDef
				{
					Code = "mi"
				}
			},
			{
				"Marathi",
				new LanguageCodeDef
				{
					Code = "mr"
				}
			},
			{
				"Mongolian",
				new LanguageCodeDef
				{
					Code = "mn"
				}
			},
			{
				"Northern Sotho",
				new LanguageCodeDef
				{
					Code = "ns",
					GoogleCode = "nso"
				}
			},
			{
				"Norwegian",
				new LanguageCodeDef
				{
					Code = "nb",
					GoogleCode = "no"
				}
			},
			{
				"Norwegian/Nynorsk",
				new LanguageCodeDef
				{
					Code = "nn",
					GoogleCode = "no"
				}
			},
			{
				"Pashto",
				new LanguageCodeDef
				{
					Code = "ps"
				}
			},
			{
				"Persian",
				new LanguageCodeDef
				{
					Code = "fa"
				}
			},
			{
				"Polish",
				new LanguageCodeDef
				{
					Code = "pl"
				}
			},
			{
				"Portuguese",
				new LanguageCodeDef
				{
					Code = "pt"
				}
			},
			{
				"Portuguese/Brazil",
				new LanguageCodeDef
				{
					Code = "pt-BR",
					GoogleCode = "pt"
				}
			},
			{
				"Portuguese/Portugal",
				new LanguageCodeDef
				{
					Code = "pt-PT",
					GoogleCode = "pt"
				}
			},
			{
				"Punjabi",
				new LanguageCodeDef
				{
					Code = "pa"
				}
			},
			{
				"Quechua",
				new LanguageCodeDef
				{
					Code = "qu"
				}
			},
			{
				"Quechua/Bolivia",
				new LanguageCodeDef
				{
					Code = "qu-BO",
					GoogleCode = "qu"
				}
			},
			{
				"Quechua/Ecuador",
				new LanguageCodeDef
				{
					Code = "qu-EC",
					GoogleCode = "qu"
				}
			},
			{
				"Quechua/Peru",
				new LanguageCodeDef
				{
					Code = "qu-PE",
					GoogleCode = "qu"
				}
			},
			{
				"Rhaeto-Romanic",
				new LanguageCodeDef
				{
					Code = "rm",
					GoogleCode = "ro"
				}
			},
			{
				"Romanian",
				new LanguageCodeDef
				{
					Code = "ro"
				}
			},
			{
				"Russian",
				new LanguageCodeDef
				{
					Code = "ru"
				}
			},
			{
				"Russian/Republic of Moldova",
				new LanguageCodeDef
				{
					Code = "ru-MO",
					GoogleCode = "ru"
				}
			},
			{
				"Serbian",
				new LanguageCodeDef
				{
					Code = "sr"
				}
			},
			{
				"Serbian/Bosnia and Herzegovina",
				new LanguageCodeDef
				{
					Code = "sr-BA",
					GoogleCode = "sr"
				}
			},
			{
				"Serbian/Serbia and Montenegro",
				new LanguageCodeDef
				{
					Code = "sr-SP",
					GoogleCode = "sr"
				}
			},
			{
				"Slovak",
				new LanguageCodeDef
				{
					Code = "sk"
				}
			},
			{
				"Slovenian",
				new LanguageCodeDef
				{
					Code = "sl"
				}
			},
			{
				"Spanish",
				new LanguageCodeDef
				{
					Code = "es"
				}
			},
			{
				"Spanish/Argentina",
				new LanguageCodeDef
				{
					Code = "es-AR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Bolivia",
				new LanguageCodeDef
				{
					Code = "es-BO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Castilian",
				new LanguageCodeDef
				{
					Code = "es-ES",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Chile",
				new LanguageCodeDef
				{
					Code = "es-CL",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Colombia",
				new LanguageCodeDef
				{
					Code = "es-CO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Costa Rica",
				new LanguageCodeDef
				{
					Code = "es-CR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Dominican Republic",
				new LanguageCodeDef
				{
					Code = "es-DO",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Ecuador",
				new LanguageCodeDef
				{
					Code = "es-EC",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/El Salvador",
				new LanguageCodeDef
				{
					Code = "es-SV",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Guatemala",
				new LanguageCodeDef
				{
					Code = "es-GT",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Honduras",
				new LanguageCodeDef
				{
					Code = "es-HN",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Mexico",
				new LanguageCodeDef
				{
					Code = "es-MX",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Nicaragua",
				new LanguageCodeDef
				{
					Code = "es-NI",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Panama",
				new LanguageCodeDef
				{
					Code = "es-PA",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Paraguay",
				new LanguageCodeDef
				{
					Code = "es-PY",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Peru",
				new LanguageCodeDef
				{
					Code = "es-PE",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Puerto Rico",
				new LanguageCodeDef
				{
					Code = "es-PR",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Spain",
				new LanguageCodeDef
				{
					Code = "es"
				}
			},
			{
				"Spanish/Uruguay",
				new LanguageCodeDef
				{
					Code = "es-UY",
					GoogleCode = "es"
				}
			},
			{
				"Spanish/Venezuela",
				new LanguageCodeDef
				{
					Code = "es-VE",
					GoogleCode = "es"
				}
			},
			{
				"Swahili",
				new LanguageCodeDef
				{
					Code = "sw"
				}
			},
			{
				"Swedish",
				new LanguageCodeDef
				{
					Code = "sv"
				}
			},
			{
				"Swedish/Finland",
				new LanguageCodeDef
				{
					Code = "sv-FI",
					GoogleCode = "sv"
				}
			},
			{
				"Swedish/Sweden",
				new LanguageCodeDef
				{
					Code = "sv-SE",
					GoogleCode = "sv"
				}
			},
			{
				"Tamil",
				new LanguageCodeDef
				{
					Code = "ta"
				}
			},
			{
				"Tatar",
				new LanguageCodeDef
				{
					Code = "tt"
				}
			},
			{
				"Telugu",
				new LanguageCodeDef
				{
					Code = "te"
				}
			},
			{
				"Thai",
				new LanguageCodeDef
				{
					Code = "th"
				}
			},
			{
				"Turkish",
				new LanguageCodeDef
				{
					Code = "tr"
				}
			},
			{
				"Ukrainian",
				new LanguageCodeDef
				{
					Code = "uk"
				}
			},
			{
				"Urdu",
				new LanguageCodeDef
				{
					Code = "ur"
				}
			},
			{
				"Uzbek",
				new LanguageCodeDef
				{
					Code = "uz"
				}
			},
			{
				"Vietnamese",
				new LanguageCodeDef
				{
					Code = "vi"
				}
			},
			{
				"Welsh",
				new LanguageCodeDef
				{
					Code = "cy"
				}
			},
			{
				"Xhosa",
				new LanguageCodeDef
				{
					Code = "xh"
				}
			},
			{
				"Yiddish",
				new LanguageCodeDef
				{
					Code = "yi"
				}
			},
			{
				"Zulu",
				new LanguageCodeDef
				{
					Code = "zu"
				}
			}
		};

		public static string GetLanguageCode(string Filter, bool ShowWarnings = false)
		{
			if (string.IsNullOrEmpty(Filter))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return string.Empty;
					}
				}
			}
			string[] filters = Filter.ToLowerInvariant().Split(" /(),".ToCharArray());
			using (Dictionary<string, LanguageCodeDef>.Enumerator enumerator = mLanguageDef.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					System.Collections.Generic.KeyValuePair<string, LanguageCodeDef> current = enumerator.Current;
					if (LanguageMatchesFilter(current.Key, filters))
					{
						LanguageCodeDef value = current.Value;
						return value.Code;
					}
				}
			}
			if (ShowWarnings)
			{
				Debug.Log(string.Format("Language '{0}' not recognized. Please, add the language code to GoogleTranslation.cs", Filter));
			}
			return string.Empty;
		}

		public static List<string> GetLanguagesForDropdown(string Filter, string CodesToExclude)
		{
			string[] filters = Filter.ToLowerInvariant().Split(" /(),".ToCharArray());
			List<string> list = new List<string>();
			using (Dictionary<string, LanguageCodeDef>.Enumerator enumerator = mLanguageDef.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					System.Collections.Generic.KeyValuePair<string, LanguageCodeDef> current = enumerator.Current;
					if (!string.IsNullOrEmpty(Filter))
					{
						if (!LanguageMatchesFilter(current.Key, filters))
						{
							continue;
						}
					}
					string[] array = new string[1];
					LanguageCodeDef value = current.Value;
					array[0] = new StringBuilder().Append("[").Append(value.Code).Append("]").ToString();
					string text = string.Concat(array);
					if (!CodesToExclude.Contains(text))
					{
						list.Add(new StringBuilder().Append(current.Key).Append(" ").Append(text).ToString());
					}
				}
			}
			for (int num = list.Count - 2; num >= 0; num--)
			{
				string text2 = list[num].Substring(0, list[num].IndexOf(" ["));
				if (list[num + 1].StartsWith(text2))
				{
					list[num] = new StringBuilder().Append(text2).Append("/").Append((object)list[num]).ToString();
					list.Insert(num + 1, new StringBuilder().Append(text2).Append("/").ToString());
				}
			}
			while (true)
			{
				return list;
			}
		}

		public static string GetClosestLanguage(string Filter)
		{
			if (string.IsNullOrEmpty(Filter))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return string.Empty;
					}
				}
			}
			string[] filters = Filter.ToLowerInvariant().Split(" /(),".ToCharArray());
			foreach (System.Collections.Generic.KeyValuePair<string, LanguageCodeDef> item in mLanguageDef)
			{
				if (LanguageMatchesFilter(item.Key, filters))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return item.Key;
						}
					}
				}
			}
			return string.Empty;
		}

		private static bool LanguageMatchesFilter(string Language, string[] Filters)
		{
			Language = Language.ToLowerInvariant();
			int i = 0;
			for (int num = Filters.Length; i < num; i++)
			{
				if (Filters[i] != string.Empty)
				{
					if (!Language.Contains(Filters[i].ToLower()))
					{
						return false;
					}
					Language = Language.Remove(Language.IndexOf(Filters[i]), Filters[i].Length);
				}
			}
			while (true)
			{
				return true;
			}
		}

		public static string GetFormatedLanguageName(string Language)
		{
			string empty = string.Empty;
			int num = Language.IndexOf(" [");
			if (num > 0)
			{
				Language = Language.Substring(0, num);
			}
			num = Language.IndexOf('/');
			if (num > 0)
			{
				empty = Language.Substring(0, num);
				if (Language == new StringBuilder().Append(empty).Append("/").Append(empty).ToString())
				{
					return empty;
				}

				Language = new StringBuilder().Append(Language.Replace("/", " (")).Append(")").ToString();
			}
			return Language;
		}

		public static string GetCodedLanguage(string Language, string code)
		{
			string languageCode = GetLanguageCode(Language);
			if (string.Compare(code, languageCode, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Language;
			}
			return new StringBuilder().Append(Language).Append(" [").Append(code).Append("]").ToString();
		}

		public static void UnPackCodeFromLanguageName(string CodedLanguage, out string Language, out string code)
		{
			if (string.IsNullOrEmpty(CodedLanguage))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						Language = string.Empty;
						code = string.Empty;
						return;
					}
				}
			}
			int num = CodedLanguage.IndexOf("[");
			if (num < 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						Language = CodedLanguage;
						code = GetLanguageCode(Language);
						return;
					}
				}
			}
			Language = CodedLanguage.Substring(0, num).Trim();
			code = CodedLanguage.Substring(num + 1, CodedLanguage.IndexOf("]", num) - num - 1);
		}

		public static string GetGoogleLanguageCode(string InternationalCode)
		{
			using (Dictionary<string, LanguageCodeDef>.Enumerator enumerator = mLanguageDef.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					System.Collections.Generic.KeyValuePair<string, LanguageCodeDef> current = enumerator.Current;
					LanguageCodeDef value = current.Value;
					if (InternationalCode == value.Code)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
							{
								LanguageCodeDef value2 = current.Value;
								string result;
								if (!string.IsNullOrEmpty(value2.GoogleCode))
								{
									LanguageCodeDef value3 = current.Value;
									result = value3.GoogleCode;
								}
								else
								{
									result = InternationalCode;
								}
								return result;
							}
							}
						}
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return InternationalCode;
					}
				}
			}
		}

		public static List<string> GetAllInternationalCodes()
		{
			HashSet<string> hashSet = new HashSet<string>();
			using (Dictionary<string, LanguageCodeDef>.Enumerator enumerator = mLanguageDef.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LanguageCodeDef value = enumerator.Current.Value;
					hashSet.Add(value.Code);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_0011;
					}
				}
				end_IL_0011:;
			}
			return new List<string>(hashSet);
		}
	}
}
