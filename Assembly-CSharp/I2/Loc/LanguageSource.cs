using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Source")]
	public class LanguageSource : MonoBehaviour
	{
		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		public LanguageSource.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSource.eGoogleUpdateFrequency.Weekly;

		public static string EmptyCategory = "Default";

		public static char[] CategorySeparators = "/\\".ToCharArray();

		public List<TermData> mTerms = new List<TermData>();

		public List<LanguageData> mLanguages = new List<LanguageData>();

		public bool CaseInsensitiveTerms;

		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		public UnityEngine.Object[] Assets;

		public bool NeverDestroy = true;

		public bool UserAgreesToHaveItOnTheScene;

		public bool UserAgreesToHaveItInsideThePluginsFolder;

		public string Spreadsheet_LocalFileName;

		public string Spreadsheet_LocalCSVSeparator = ",";

		public string Export_CSV(string Category, char Separator = ',')
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc{0}ID", Separator);
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append(Separator);
				LanguageSource.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code), Separator);
			}
			stringBuilder.Append("\n");
			foreach (TermData termData in this.mTerms)
			{
				if (string.IsNullOrEmpty(Category))
				{
					goto IL_F0;
				}
				if (Category == LanguageSource.EmptyCategory)
				{
					if (termData.Term.IndexOfAny(LanguageSource.CategorySeparators) < 0)
					{
						goto IL_F0;
					}
				}
				if (!termData.Term.StartsWith(Category) || !(Category != termData.Term))
				{
					continue;
				}
				string term = termData.Term.Substring(Category.Length + 1);
				IL_137:
				LanguageSource.AppendTerm(stringBuilder, count, term, termData, null, termData.Languages, termData.Languages_Touch, Separator, 1, 2);
				if (termData.HasTouchTranslations())
				{
					LanguageSource.AppendTerm(stringBuilder, count, term, termData, "[touch]", termData.Languages_Touch, null, Separator, 2, 1);
					continue;
				}
				continue;
				IL_F0:
				term = termData.Term;
				goto IL_137;
			}
			return stringBuilder.ToString();
		}

		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string prefix, string[] aLanguages, string[] aSecLanguages, char Separator, byte FlagBitMask, byte SecFlagBitMask)
		{
			LanguageSource.AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(prefix))
			{
				Builder.Append(prefix);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			LanguageSource.AppendString(Builder, termData.Description, Separator);
			Builder.Append(Separator);
			Builder.Append(termData.LocIdStr);
			for (int i = 0; i < Mathf.Min(nLanguages, aLanguages.Length); i++)
			{
				Builder.Append(Separator);
				string text = aLanguages[i];
				bool flag = (termData.Flags[i] & FlagBitMask) > 0;
				if (string.IsNullOrEmpty(text))
				{
					if (aSecLanguages != null)
					{
						text = aSecLanguages[i];
						flag = ((termData.Flags[i] & SecFlagBitMask) > 0);
					}
				}
				text = text.TrimEnd(new char[]
				{
					'\r'
				});
				if (flag)
				{
					Builder.Append("[i2auto]");
				}
				LanguageSource.AppendString(Builder, text, Separator);
			}
			Builder.Append("\n");
		}

		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
			}
			else
			{
				Builder.Append(Text);
			}
		}

		public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string value = this.Export_Google_CreateData();
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("key", this.Google_SpreadsheetKey);
			wwwform.AddField("action", "SetLanguageSource");
			wwwform.AddField("data", value);
			wwwform.AddField("updateMode", UpdateMode.ToString());
			return new WWW(this.Google_WebServiceURL, wwwform);
		}

		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true, null);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			using (List<string>.Enumerator enumerator = categories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append("<I2Loc>");
					}
					string value = this.Export_CSV(text, ',');
					stringBuilder.Append(text);
					stringBuilder.Append("<I2Loc>");
					stringBuilder.Append(value);
				}
			}
			return stringBuilder.ToString();
		}

		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> list = LocalizationReader.ReadCSV(CSVstring, Separator);
			string[] array = list[0];
			int num = 1;
			int num2 = -1;
			int num3 = -1;
			string[] texts = new string[]
			{
				"Key"
			};
			string[] texts2 = new string[]
			{
				"Type"
			};
			string[] texts3 = new string[]
			{
				"Desc",
				"Description"
			};
			string[] texts4 = new string[]
			{
				"ID"
			};
			if (array.Length > 1)
			{
				if (this.ArrayContains(array[0], texts))
				{
					if (UpdateMode == eSpreadsheetUpdateMode.Replace)
					{
						this.ClearAllData();
					}
					if (array.Length > 2)
					{
						if (this.ArrayContains(array[1], texts2))
						{
							num2 = 1;
							num = 2;
						}
						if (this.ArrayContains(array[1], texts3))
						{
							num = 2;
						}
					}
					if (array.Length > 3)
					{
						if (this.ArrayContains(array[2], texts2))
						{
							num2 = 2;
							num = 3;
						}
						if (this.ArrayContains(array[2], texts3))
						{
							num = 3;
						}
					}
					if (array.Length > 4)
					{
						if (this.ArrayContains(array[3], texts4))
						{
							num3 = 3;
							num = 4;
						}
					}
					int num4 = Mathf.Max(array.Length - num, 0);
					int[] array2 = new int[num4];
					for (int i = 0; i < num4; i++)
					{
						string text;
						string text2;
						GoogleLanguages.UnPackCodeFromLanguageName(array[i + num], out text, out text2);
						int num5;
						if (!string.IsNullOrEmpty(text2))
						{
							num5 = this.GetLanguageIndexFromCode(text2);
						}
						else
						{
							num5 = this.GetLanguageIndex(text, true);
						}
						if (num5 < 0)
						{
							LanguageData languageData = new LanguageData();
							languageData.Name = text;
							languageData.Code = text2;
							this.mLanguages.Add(languageData);
							num5 = this.mLanguages.Count - 1;
						}
						array2[i] = num5;
					}
					num4 = this.mLanguages.Count;
					int j = 0;
					int count = this.mTerms.Count;
					while (j < count)
					{
						TermData termData = this.mTerms[j];
						if (termData.Languages.Length < num4)
						{
							Array.Resize<string>(ref termData.Languages, num4);
							Array.Resize<string>(ref termData.Languages_Touch, num4);
							Array.Resize<byte>(ref termData.Flags, num4);
						}
						j++;
					}
					int k = 1;
					int count2 = list.Count;
					while (k < count2)
					{
						array = list[k];
						string text3;
						if (string.IsNullOrEmpty(Category))
						{
							text3 = array[0];
						}
						else
						{
							text3 = Category + "/" + array[0];
						}
						string text4 = text3;
						bool flag = false;
						if (text4.EndsWith("[touch]"))
						{
							text4 = text4.Remove(text4.Length - "[touch]".Length);
							flag = true;
						}
						LanguageSource.ValidateFullTerm(ref text4);
						if (!string.IsNullOrEmpty(text4))
						{
							TermData termData2 = this.GetTermData(text4, false);
							if (termData2 == null)
							{
								if (UpdateMode == eSpreadsheetUpdateMode.Merge)
								{
									goto IL_5EC;
								}
								termData2 = new TermData();
								termData2.Term = text4;
								termData2.Languages = new string[this.mLanguages.Count];
								termData2.Languages_Touch = new string[this.mLanguages.Count];
								termData2.Flags = new byte[this.mLanguages.Count];
								for (int l = 0; l < this.mLanguages.Count; l++)
								{
									termData2.Languages[l] = (termData2.Languages_Touch[l] = string.Empty);
								}
								this.mTerms.Add(termData2);
								this.mDictionary.Add(text4, termData2);
							}
							else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
							{
								goto IL_5EC;
							}
							if (num2 > 0)
							{
								termData2.TermType = LanguageSource.GetTermType(array[num2]);
							}
							if (num3 > 0)
							{
								termData2.LocIdStr = array[num3];
							}
							int num6 = 0;
							if (UpdateMode == eSpreadsheetUpdateMode.Merge)
							{
								num6 = 1;
								if (termData2.TermType == eTermType.Texture)
								{
									goto IL_5EC;
								}
							}
							int m = num6;
							while (m < array2.Length)
							{
								if (m >= array.Length - num)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										goto IL_5EC;
									}
								}
								else
								{
									if (!string.IsNullOrEmpty(array[m + num]))
									{
										string text5 = array[m + num];
										bool flag2 = text5.StartsWith("[i2auto]");
										if (flag2)
										{
											text5 = text5.Substring("[isauto]".Length);
											if (text5.StartsWith("\""))
											{
												if (text5.EndsWith("\""))
												{
													text5 = text5.Substring(1, text5.Length - 2);
												}
											}
										}
										int num7 = array2[m];
										if (flag)
										{
											termData2.Languages_Touch[num7] = text5;
											if (flag2)
											{
												byte[] flags = termData2.Flags;
												int num8 = num7;
												flags[num8] |= 2;
											}
											else
											{
												byte[] flags2 = termData2.Flags;
												int num9 = num7;
												flags2[num9] &= 0xFD;
											}
										}
										else
										{
											termData2.Languages[num7] = text5;
											if (flag2)
											{
												byte[] flags3 = termData2.Flags;
												int num10 = num7;
												flags3[num10] |= 1;
											}
											else
											{
												byte[] flags4 = termData2.Flags;
												int num11 = num7;
												flags4[num11] &= 0xFE;
											}
										}
									}
									m++;
								}
							}
						}
						IL_5EC:
						k++;
					}
					return string.Empty;
				}
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type', 'Desc' and 'ID'";
		}

		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			int num = texts.Length;
			while (i < num)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		public static eTermType GetTermType(string type)
		{
			int i = 0;
			int num = 8;
			while (i <= num)
			{
				eTermType eTermType = (eTermType)i;
				if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
				i++;
			}
			return eTermType.Text;
		}

		private Action<LanguageSource> Event_OnSourceUpdateFromGoogleHolder;
	public event Action<LanguageSource> Event_OnSourceUpdateFromGoogle
		{
			add
			{
				Action<LanguageSource> action = this.Event_OnSourceUpdateFromGoogleHolder;
				Action<LanguageSource> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<LanguageSource>>(ref this.Event_OnSourceUpdateFromGoogleHolder, (Action<LanguageSource>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<LanguageSource> action = this.Event_OnSourceUpdateFromGoogleHolder;
				Action<LanguageSource> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<LanguageSource>>(ref this.Event_OnSourceUpdateFromGoogleHolder, (Action<LanguageSource>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public void Import_Google(bool ForceUpdate = false)
		{
			if (this.GoogleUpdateFrequency == LanguageSource.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!Application.isPlaying)
			{
				return;
			}
			string @string = PlayerPrefs.GetString("I2Source_" + this.Google_SpreadsheetKey, string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				this.Import_Google_Result(@string, eSpreadsheetUpdateMode.Replace);
			}
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			if (!ForceUpdate)
			{
				if (this.GoogleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Always)
				{
					string string2 = PlayerPrefs.GetString("LastGoogleUpdate_" + sourcePlayerPrefName, string.Empty);
					DateTime d;
					if (DateTime.TryParse(string2, out d))
					{
						double totalDays = (DateTime.Now - d).TotalDays;
						LanguageSource.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
						if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Daily)
						{
							if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Weekly)
							{
								if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Monthly)
								{
								}
								else if (totalDays < 31.0)
								{
									return;
								}
							}
							else if (totalDays < 8.0)
							{
								return;
							}
						}
						else if (totalDays < 1.0)
						{
							return;
						}
					}
				}
			}
			PlayerPrefs.SetString("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
			CoroutineManager.pInstance.StartCoroutine(this.Import_Google_Coroutine());
		}

		private string GetSourcePlayerPrefName()
		{
			if (Array.IndexOf<string>(LocalizationManager.GlobalSources, base.name) >= 0)
			{
				return base.name;
			}
			return SceneManager.GetActiveScene().name + "_" + base.name;
		}

		private IEnumerator Import_Google_Coroutine()
		{
			WWW www = this.Import_Google_CreateWWWcall(false);
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				if (www.text != "\"\"")
				{
					PlayerPrefs.SetString("I2Source_" + this.Google_SpreadsheetKey, www.text);
					PlayerPrefs.Save();
					this.Import_Google_Result(www.text, eSpreadsheetUpdateMode.Replace);
					if (this.Event_OnSourceUpdateFromGoogleHolder != null)
					{
						this.Event_OnSourceUpdateFromGoogleHolder(this);
					}
					LocalizationManager.LocalizeAll(false);
					Debug.Log("Done Google Sync '" + www.text + "'");
					goto IL_186;
				}
			}
			Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			IL_186:
			yield break;
		}

		public WWW Import_Google_CreateWWWcall(bool ForceUpdate = false)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			string url = string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", this.Google_WebServiceURL, this.Google_SpreadsheetKey, (!ForceUpdate) ? this.Google_LastUpdatedVersion : "0");
			return new WWW(url);
		}

		public bool HasGoogleSpreadsheet()
		{
			bool result;
			if (!string.IsNullOrEmpty(this.Google_WebServiceURL))
			{
				result = !string.IsNullOrEmpty(this.Google_SpreadsheetKey);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode)
		{
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(JsonString))
			{
				if (!(JsonString == "\"\""))
				{
					if (UpdateMode == eSpreadsheetUpdateMode.Replace)
					{
						this.ClearAllData();
					}
					int num = JsonString.IndexOf("version=");
					int num2 = JsonString.IndexOf("script_version=");
					if (num >= 0)
					{
						if (num2 < 0)
						{
						}
						else
						{
							num += "version=".Length;
							num2 += "script_version=".Length;
							this.Google_LastUpdatedVersion = JsonString.Substring(num, JsonString.IndexOf(",", num) - num);
							int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2) - num2));
							if (num3 < LocalizationManager.GetRequiredWebServiceVersion())
							{
								return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
							}
							int i = JsonString.IndexOf("[i2category]");
							while (i > 0)
							{
								i += "[i2category]".Length;
								int num4 = JsonString.IndexOf("[/i2category]", i);
								string category = JsonString.Substring(i, num4 - i);
								num4 += "[/i2category]".Length;
								int num5 = JsonString.IndexOf("[/i2csv]", num4);
								string csvstring = JsonString.Substring(num4, num5 - num4);
								i = JsonString.IndexOf("[i2category]", num5);
								this.Import_CSV(category, csvstring, UpdateMode, ',');
								if (UpdateMode == eSpreadsheetUpdateMode.Replace)
								{
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
							}
							return empty;
						}
					}
					return "Invalid Response from Google, Most likely the WebService needs to be updated";
				}
			}
			Debug.Log("Language Source was up to date");
			return empty;
		}

		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
			{
				Categories = new List<string>();
			}
			using (List<TermData>.Enumerator enumerator = this.mTerms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TermData termData = enumerator.Current;
					string categoryFromFullTerm = LanguageSource.GetCategoryFromFullTerm(termData.Term, OnlyMainCategory);
					if (!Categories.Contains(categoryFromFullTerm))
					{
						Categories.Add(categoryFromFullTerm);
					}
				}
			}
			Categories.Sort();
			return Categories;
		}

		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			string result;
			if (num < 0)
			{
				result = FullTerm;
			}
			else
			{
				result = FullTerm.Substring(num + 1);
			}
			return result;
		}

		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num;
			if (OnlyMainCategory)
			{
				num = FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			}
			else
			{
				num = FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators);
			}
			int num2 = num;
			string result;
			if (num2 < 0)
			{
				result = LanguageSource.EmptyCategory;
			}
			else
			{
				result = FullTerm.Substring(0, num2);
			}
			return result;
		}

		public unsafe static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			if (num < 0)
			{
				Category = LanguageSource.EmptyCategory;
				Key = FullTerm;
			}
			else
			{
				Category = FullTerm.Substring(0, num);
				Key = FullTerm.Substring(num + 1);
			}
		}

		public static LanguageSource.eInputSpecialization GetCurrentInputType()
		{
			return (Input.GetJoystickNames().Length <= 0) ? LanguageSource.eInputSpecialization.PC : LanguageSource.eInputSpecialization.Controller;
		}

		private void Awake()
		{
			if (this.NeverDestroy)
			{
				if (this.ManagerHasASimilarSource())
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				}
			}
			LocalizationManager.AddSource(this);
			this.UpdateDictionary(false);
		}

		public void UpdateDictionary(bool force = false)
		{
			if (!force)
			{
				if (this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
				{
					return;
				}
			}
			StringComparer stringComparer;
			if (this.CaseInsensitiveTerms)
			{
				stringComparer = StringComparer.OrdinalIgnoreCase;
			}
			else
			{
				stringComparer = StringComparer.Ordinal;
			}
			StringComparer stringComparer2 = stringComparer;
			if (this.mDictionary.Comparer != stringComparer2)
			{
				this.mDictionary = new Dictionary<string, TermData>(stringComparer2);
			}
			else
			{
				this.mDictionary.Clear();
			}
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				LanguageSource.ValidateFullTerm(ref this.mTerms[i].Term);
				if (this.mTerms[i].Languages_Touch == null)
				{
					goto IL_11B;
				}
				if (this.mTerms[i].Languages_Touch.Length != this.mTerms[i].Languages.Length)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_11B;
					}
				}
				IL_146:
				this.mDictionary[this.mTerms[i].Term] = this.mTerms[i];
				this.mTerms[i].Validate();
				i++;
				continue;
				IL_11B:
				this.mTerms[i].Languages_Touch = new string[this.mTerms[i].Languages.Length];
				goto IL_146;
			}
		}

		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while (parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			return text;
		}

		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (string.Compare(this.mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int num = -1;
				int num2 = 0;
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					int commonWordInLanguageNames = LanguageSource.GetCommonWordInLanguageNames(this.mLanguages[j].Name, language);
					if (commonWordInLanguageNames > num2)
					{
						num2 = commonWordInLanguageNames;
						num = j;
					}
					j++;
				}
				if (num >= 0)
				{
					return num;
				}
			}
			return -1;
		}

		public int GetLanguageIndexFromCode(string Code)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (string.Compare(this.mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (!string.IsNullOrEmpty(Language1))
			{
				if (!string.IsNullOrEmpty(Language2))
				{
					string[] array = (from x in Language1.Split("( )-/\\".ToCharArray())
					where !string.IsNullOrEmpty(x)
					select x).ToArray<string>();
					IEnumerable<string> source = Language2.Split("( )-/\\".ToCharArray());
					
					string[] array2 = source.Where(((string x) => !string.IsNullOrEmpty(x))).ToArray<string>();
					int num = 0;
					foreach (string value in array)
					{
						if (array2.Contains(value))
						{
							num++;
						}
					}
					foreach (string value2 in array2)
					{
						if (array.Contains(value2))
						{
							num++;
						}
					}
					return num;
				}
			}
			return 0;
		}

		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSource.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSource.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, false) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			this.mLanguages.Add(languageData);
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				Array.Resize<string>(ref this.mTerms[i].Languages, count);
				Array.Resize<string>(ref this.mTerms[i].Languages_Touch, count);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count);
				i++;
			}
		}

		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, true);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					this.mTerms[i].Languages[j - 1] = this.mTerms[i].Languages[j];
					this.mTerms[i].Languages_Touch[j - 1] = this.mTerms[i].Languages_Touch[j];
					this.mTerms[i].Flags[j - 1] = this.mTerms[i].Flags[j];
				}
				Array.Resize<string>(ref this.mTerms[i].Languages, count - 1);
				Array.Resize<string>(ref this.mTerms[i].Languages_Touch, count - 1);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count - 1);
				i++;
			}
			this.mLanguages.RemoveAt(languageIndex);
		}

		public List<string> GetLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				list.Add(this.mLanguages[i].Name);
				i++;
			}
			return list;
		}

		public string GetTermTranslation(string term)
		{
			int languageIndex = this.GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
			if (languageIndex < 0)
			{
				return string.Empty;
			}
			TermData termData = this.GetTermData(term, false);
			if (termData != null)
			{
				return termData.GetTranslation(languageIndex);
			}
			return string.Empty;
		}

		public unsafe bool TryGetTermTranslation(string term, out string Translation)
		{
			int languageIndex = this.GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
			if (languageIndex >= 0)
			{
				TermData termData = this.GetTermData(term, false);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex);
					if (Translation.IsNullOrEmpty())
					{
						Translation = termData.GetTranslation(0);
					}
					return true;
				}
			}
			Translation = string.Empty;
			return false;
		}

		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text, true);
		}

		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary(false);
			}
			TermData result;
			if (this.mDictionary.TryGetValue(term, out result))
			{
				return result;
			}
			TermData termData = null;
			if (allowCategoryMistmatch)
			{
				string keyFromFullTerm = LanguageSource.GetKeyFromFullTerm(term, false);
				foreach (KeyValuePair<string, TermData> keyValuePair in this.mDictionary)
				{
					if (keyValuePair.Value.IsTerm(keyFromFullTerm, true))
					{
						if (termData != null)
						{
							return null;
						}
						termData = keyValuePair.Value;
					}
				}
				return termData;
			}
			return termData;
		}

		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term, false) != null;
		}

		public List<string> GetTermsList()
		{
			if (this.mDictionary.Count != this.mTerms.Count)
			{
				this.UpdateDictionary(false);
			}
			return new List<string>(this.mDictionary.Keys);
		}

		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			LanguageSource.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (this.mLanguages.Count == 0)
			{
				this.AddLanguage("English", "en");
			}
			TermData termData = this.GetTermData(NewTerm, false);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[this.mLanguages.Count];
				termData.Languages_Touch = new string[this.mLanguages.Count];
				termData.Flags = new byte[this.mLanguages.Count];
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		public void RemoveTerm(string term)
		{
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				if (this.mTerms[i].Term == term)
				{
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
				}
				i++;
			}
		}

		public unsafe static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSource.EmptyCategory) && Term.Length > LanguageSource.EmptyCategory.Length && Term[LanguageSource.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSource.EmptyCategory.Length + 1);
			}
		}

		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true) < 0)
				{
					return false;
				}
				i++;
			}
			if (Source.mTerms.Count != this.mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < this.mTerms.Count; j++)
			{
				if (Source.GetTermData(this.mTerms[j].Term, false) == null)
				{
					return false;
				}
			}
			return true;
		}

		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LanguageSource languageSource = LocalizationManager.Sources[i];
				if (languageSource != null)
				{
					if (languageSource.IsEqualTo(this))
					{
						if (languageSource != this)
						{
							return true;
						}
					}
				}
				i++;
			}
			return false;
		}

		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
		}

		public UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null)
					{
						if (this.Assets[i].name == Name)
						{
							return this.Assets[i];
						}
					}
					i++;
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		public void AddAsset(UnityEngine.Object Obj)
		{
			Array.Resize<UnityEngine.Object>(ref this.Assets, this.Assets.Length + 1);
			this.Assets[this.Assets.Length - 1] = Obj;
		}

		public enum eGoogleUpdateFrequency
		{
			Always,
			Never,
			Daily,
			Weekly,
			Monthly
		}

		public enum eInputSpecialization
		{
			PC,
			Touch,
			Controller
		}
	}
}
