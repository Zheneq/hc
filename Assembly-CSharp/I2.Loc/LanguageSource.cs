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

		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		public eGoogleUpdateFrequency GoogleUpdateFrequency = eGoogleUpdateFrequency.Weekly;

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
					action = Interlocked.CompareExchange(ref this.Event_OnSourceUpdateFromGoogleHolder, (Action<LanguageSource>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
				while (true)
				{
					return;
				}
			}
			remove
			{
				Action<LanguageSource> action = this.Event_OnSourceUpdateFromGoogleHolder;
				Action<LanguageSource> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref this.Event_OnSourceUpdateFromGoogleHolder, (Action<LanguageSource>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
				while (true)
				{
					return;
				}
			}
		}

		public string Export_CSV(string Category, char Separator = ',')
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc{0}ID", Separator);
			foreach (LanguageData mLanguage in mLanguages)
			{
				stringBuilder.Append(Separator);
				AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code), Separator);
			}
			stringBuilder.Append("\n");
			foreach (TermData mTerm in mTerms)
			{
				if (string.IsNullOrEmpty(Category))
				{
					goto IL_00f0;
				}
				if (Category == EmptyCategory)
				{
					if (mTerm.Term.IndexOfAny(CategorySeparators) < 0)
					{
						goto IL_00f0;
					}
				}
				if (!mTerm.Term.StartsWith(Category) || !(Category != mTerm.Term))
				{
					continue;
				}
				string term = mTerm.Term.Substring(Category.Length + 1);
				goto IL_0137;
				IL_00f0:
				term = mTerm.Term;
				goto IL_0137;
				IL_0137:
				AppendTerm(stringBuilder, count, term, mTerm, null, mTerm.Languages, mTerm.Languages_Touch, Separator, 1, 2);
				if (mTerm.HasTouchTranslations())
				{
					AppendTerm(stringBuilder, count, term, mTerm, "[touch]", mTerm.Languages_Touch, null, Separator, 2, 1);
				}
			}
			return stringBuilder.ToString();
		}

		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string prefix, string[] aLanguages, string[] aSecLanguages, char Separator, byte FlagBitMask, byte SecFlagBitMask)
		{
			AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(prefix))
			{
				Builder.Append(prefix);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			AppendString(Builder, termData.Description, Separator);
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
				text = text.TrimEnd('\r');
				if (flag)
				{
					Builder.Append("[i2auto]");
				}
				AppendString(Builder, text, Separator);
			}
			Builder.Append("\n");
		}

		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (string.IsNullOrEmpty(Text))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						Text = Text.Replace("\"", "\"\"");
						Builder.AppendFormat("\"{0}\"", Text);
						return;
					}
				}
			}
			Builder.Append(Text);
		}

		public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string value = Export_Google_CreateData();
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("key", Google_SpreadsheetKey);
			wWWForm.AddField("action", "SetLanguageSource");
			wWWForm.AddField("data", value);
			wWWForm.AddField("updateMode", UpdateMode.ToString());
			return new WWW(Google_WebServiceURL, wWWForm);
		}

		private string Export_Google_CreateData()
		{
			List<string> categories = GetCategories(true);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			using (List<string>.Enumerator enumerator = categories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append("<I2Loc>");
					}
					string value = Export_CSV(current);
					stringBuilder.Append(current);
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
			string[] texts = new string[1]
			{
				"Key"
			};
			string[] texts2 = new string[1]
			{
				"Type"
			};
			string[] texts3 = new string[2]
			{
				"Desc",
				"Description"
			};
			string[] texts4 = new string[1]
			{
				"ID"
			};
			if (array.Length > 1)
			{
				if (ArrayContains(array[0], texts))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
						{
							if (UpdateMode == eSpreadsheetUpdateMode.Replace)
							{
								ClearAllData();
							}
							if (array.Length > 2)
							{
								if (ArrayContains(array[1], texts2))
								{
									num2 = 1;
									num = 2;
								}
								if (ArrayContains(array[1], texts3))
								{
									num = 2;
								}
							}
							if (array.Length > 3)
							{
								if (ArrayContains(array[2], texts2))
								{
									num2 = 2;
									num = 3;
								}
								if (ArrayContains(array[2], texts3))
								{
									num = 3;
								}
							}
							if (array.Length > 4)
							{
								if (ArrayContains(array[3], texts4))
								{
									num3 = 3;
									num = 4;
								}
							}
							int num4 = Mathf.Max(array.Length - num, 0);
							int[] array2 = new int[num4];
							for (int i = 0; i < num4; i++)
							{
								GoogleLanguages.UnPackCodeFromLanguageName(array[i + num], out string Language, out string code);
								int num5 = -1;
								if (!string.IsNullOrEmpty(code))
								{
									num5 = GetLanguageIndexFromCode(code);
								}
								else
								{
									num5 = GetLanguageIndex(Language);
								}
								if (num5 < 0)
								{
									LanguageData languageData = new LanguageData();
									languageData.Name = Language;
									languageData.Code = code;
									mLanguages.Add(languageData);
									num5 = mLanguages.Count - 1;
								}
								array2[i] = num5;
							}
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
								{
									num4 = mLanguages.Count;
									int j = 0;
									for (int count = mTerms.Count; j < count; j++)
									{
										TermData termData = mTerms[j];
										if (termData.Languages.Length < num4)
										{
											Array.Resize(ref termData.Languages, num4);
											Array.Resize(ref termData.Languages_Touch, num4);
											Array.Resize(ref termData.Flags, num4);
										}
									}
									int k = 1;
									for (int count2 = list.Count; k < count2; k++)
									{
										array = list[k];
										object obj;
										if (string.IsNullOrEmpty(Category))
										{
											obj = array[0];
										}
										else
										{
											obj = Category + "/" + array[0];
										}
										string Term = (string)obj;
										bool flag = false;
										if (Term.EndsWith("[touch]"))
										{
											Term = Term.Remove(Term.Length - "[touch]".Length);
											flag = true;
										}
										ValidateFullTerm(ref Term);
										if (!string.IsNullOrEmpty(Term))
										{
											TermData termData2 = GetTermData(Term);
											if (termData2 == null)
											{
												if (UpdateMode == eSpreadsheetUpdateMode.Merge)
												{
													continue;
												}
												termData2 = new TermData();
												termData2.Term = Term;
												termData2.Languages = new string[mLanguages.Count];
												termData2.Languages_Touch = new string[mLanguages.Count];
												termData2.Flags = new byte[mLanguages.Count];
												for (int l = 0; l < mLanguages.Count; l++)
												{
													termData2.Languages[l] = (termData2.Languages_Touch[l] = string.Empty);
												}
												mTerms.Add(termData2);
												mDictionary.Add(Term, termData2);
											}
											else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
											{
												continue;
											}
											if (num2 > 0)
											{
												termData2.TermType = GetTermType(array[num2]);
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
													continue;
												}
											}
											for (int m = num6; m < array2.Length; m++)
											{
												if (m >= array.Length - num)
												{
													break;
												}
												if (!string.IsNullOrEmpty(array[m + num]))
												{
													string text = array[m + num];
													bool flag2 = text.StartsWith("[i2auto]");
													if (flag2)
													{
														text = text.Substring("[isauto]".Length);
														if (text.StartsWith("\""))
														{
															if (text.EndsWith("\""))
															{
																text = text.Substring(1, text.Length - 2);
															}
														}
													}
													int num7 = array2[m];
													if (flag)
													{
														termData2.Languages_Touch[num7] = text;
														if (flag2)
														{
															termData2.Flags[num7] |= 2;
														}
														else
														{
															termData2.Flags[num7] &= 253;
														}
													}
													else
													{
														termData2.Languages[num7] = text;
														if (flag2)
														{
															termData2.Flags[num7] |= 1;
														}
														else
														{
															termData2.Flags[num7] &= 254;
														}
													}
												}
											}
										}
									}
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											return string.Empty;
										}
									}
								}
								}
							}
						}
						}
					}
				}
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type', 'Desc' and 'ID'";
		}

		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			for (int num = texts.Length; i < num; i++)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) < 0)
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
			while (true)
			{
				return false;
			}
		}

		public static eTermType GetTermType(string type)
		{
			int i = 0;
			for (int num = 8; i <= num; i++)
			{
				eTermType eTermType = (eTermType)i;
				if (!string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				while (true)
				{
					return (eTermType)i;
				}
			}
			while (true)
			{
				return eTermType.Text;
			}
		}

		public void Import_Google(bool ForceUpdate = false)
		{
			if (GoogleUpdateFrequency == eGoogleUpdateFrequency.Never)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			if (!Application.isPlaying)
			{
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
			string @string = PlayerPrefs.GetString("I2Source_" + Google_SpreadsheetKey, string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				Import_Google_Result(@string, eSpreadsheetUpdateMode.Replace);
			}
			string sourcePlayerPrefName = GetSourcePlayerPrefName();
			if (!ForceUpdate)
			{
				if (GoogleUpdateFrequency != 0)
				{
					string string2 = PlayerPrefs.GetString("LastGoogleUpdate_" + sourcePlayerPrefName, string.Empty);
					if (DateTime.TryParse(string2, out DateTime result))
					{
						double totalDays = (DateTime.Now - result).TotalDays;
						eGoogleUpdateFrequency googleUpdateFrequency = GoogleUpdateFrequency;
						if (googleUpdateFrequency != eGoogleUpdateFrequency.Daily)
						{
							if (googleUpdateFrequency != eGoogleUpdateFrequency.Weekly)
							{
								if (googleUpdateFrequency != eGoogleUpdateFrequency.Monthly)
								{
								}
								else if (totalDays < 31.0)
								{
									while (true)
									{
										switch (3)
										{
										default:
											return;
										case 0:
											break;
										}
									}
								}
							}
							else if (totalDays < 8.0)
							{
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
						}
						else if (totalDays < 1.0)
						{
							return;
						}
					}
				}
			}
			PlayerPrefs.SetString("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
			CoroutineManager.pInstance.StartCoroutine(Import_Google_Coroutine());
		}

		private string GetSourcePlayerPrefName()
		{
			if (Array.IndexOf(LocalizationManager.GlobalSources, base.name) >= 0)
			{
				return base.name;
			}
			return SceneManager.GetActiveScene().name + "_" + base.name;
		}

		private IEnumerator Import_Google_Coroutine()
		{
			WWW www = Import_Google_CreateWWWcall();
			if (www == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						yield break;
					}
				}
			}
			if (!www.isDone)
			{
				yield return null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				if (string.IsNullOrEmpty(www.error))
				{
					if (www.text != "\"\"")
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								PlayerPrefs.SetString("I2Source_" + Google_SpreadsheetKey, www.text);
								PlayerPrefs.Save();
								Import_Google_Result(www.text, eSpreadsheetUpdateMode.Replace);
								if (this.Event_OnSourceUpdateFromGoogleHolder != null)
								{
									this.Event_OnSourceUpdateFromGoogleHolder(this);
								}
								LocalizationManager.LocalizeAll();
								Debug.Log("Done Google Sync '" + www.text + "'");
								yield break;
							}
						}
					}
				}
				Debug.Log("Language Source was up-to-date with Google Spreadsheet");
				yield break;
			}
		}

		public WWW Import_Google_CreateWWWcall(bool ForceUpdate = false)
		{
			if (!HasGoogleSpreadsheet())
			{
				return null;
			}
			string url = string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", Google_WebServiceURL, Google_SpreadsheetKey, (!ForceUpdate) ? Google_LastUpdatedVersion : "0");
			return new WWW(url);
		}

		public bool HasGoogleSpreadsheet()
		{
			int result;
			if (!string.IsNullOrEmpty(Google_WebServiceURL))
			{
				result = ((!string.IsNullOrEmpty(Google_SpreadsheetKey)) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
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
						ClearAllData();
					}
					int num = JsonString.IndexOf("version=");
					int num2 = JsonString.IndexOf("script_version=");
					if (num >= 0)
					{
						if (num2 >= 0)
						{
							num += "version=".Length;
							num2 += "script_version=".Length;
							Google_LastUpdatedVersion = JsonString.Substring(num, JsonString.IndexOf(",", num) - num);
							int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2) - num2));
							if (num3 < LocalizationManager.GetRequiredWebServiceVersion())
							{
								return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
							}
							int num4 = JsonString.IndexOf("[i2category]");
							while (num4 > 0)
							{
								num4 += "[i2category]".Length;
								int num5 = JsonString.IndexOf("[/i2category]", num4);
								string category = JsonString.Substring(num4, num5 - num4);
								num5 += "[/i2category]".Length;
								int num6 = JsonString.IndexOf("[/i2csv]", num5);
								string cSVstring = JsonString.Substring(num5, num6 - num5);
								num4 = JsonString.IndexOf("[i2category]", num6);
								Import_CSV(category, cSVstring, UpdateMode);
								if (UpdateMode == eSpreadsheetUpdateMode.Replace)
								{
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
							}
							while (true)
							{
								return empty;
							}
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
			using (List<TermData>.Enumerator enumerator = mTerms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TermData current = enumerator.Current;
					string categoryFromFullTerm = GetCategoryFromFullTerm(current.Term, OnlyMainCategory);
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
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
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
				num = FullTerm.IndexOfAny(CategorySeparators);
			}
			else
			{
				num = FullTerm.LastIndexOfAny(CategorySeparators);
			}
			int num2 = num;
			string result;
			if (num2 < 0)
			{
				result = EmptyCategory;
			}
			else
			{
				result = FullTerm.Substring(0, num2);
			}
			return result;
		}

		public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(CategorySeparators) : FullTerm.IndexOfAny(CategorySeparators);
			if (num < 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						Category = EmptyCategory;
						Key = FullTerm;
						return;
					}
				}
			}
			Category = FullTerm.Substring(0, num);
			Key = FullTerm.Substring(num + 1);
		}

		public static eInputSpecialization GetCurrentInputType()
		{
			return (Input.GetJoystickNames().Length > 0) ? eInputSpecialization.Controller : eInputSpecialization.PC;
		}

		private void Awake()
		{
			if (NeverDestroy)
			{
				if (ManagerHasASimilarSource())
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
			UpdateDictionary();
		}

		public void UpdateDictionary(bool force = false)
		{
			if (!force)
			{
				if (mDictionary != null && mDictionary.Count == mTerms.Count)
				{
					while (true)
					{
						switch (1)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			StringComparer stringComparer;
			if (CaseInsensitiveTerms)
			{
				stringComparer = StringComparer.OrdinalIgnoreCase;
			}
			else
			{
				stringComparer = StringComparer.Ordinal;
			}
			StringComparer stringComparer2 = stringComparer;
			if (mDictionary.Comparer != stringComparer2)
			{
				mDictionary = new Dictionary<string, TermData>(stringComparer2);
			}
			else
			{
				mDictionary.Clear();
			}
			int i = 0;
			for (int count = mTerms.Count; i < count; mDictionary[mTerms[i].Term] = mTerms[i], mTerms[i].Validate(), i++)
			{
				ValidateFullTerm(ref mTerms[i].Term);
				if (mTerms[i].Languages_Touch != null)
				{
					if (mTerms[i].Languages_Touch.Length == mTerms[i].Languages.Length)
					{
						continue;
					}
				}
				mTerms[i].Languages_Touch = new string[mTerms[i].Languages.Length];
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

		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while ((bool)parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			while (true)
			{
				return text;
			}
		}

		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true)
		{
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (string.Compare(mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) != 0)
				{
					continue;
				}
				while (true)
				{
					return i;
				}
			}
			while (true)
			{
				if (AllowDiscartingRegion)
				{
					int num = -1;
					int num2 = 0;
					int j = 0;
					for (int count2 = mLanguages.Count; j < count2; j++)
					{
						int commonWordInLanguageNames = GetCommonWordInLanguageNames(mLanguages[j].Name, language);
						if (commonWordInLanguageNames > num2)
						{
							num2 = commonWordInLanguageNames;
							num = j;
						}
					}
					if (num >= 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return num;
							}
						}
					}
				}
				return -1;
			}
		}

		public int GetLanguageIndexFromCode(string Code)
		{
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (string.Compare(mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) != 0)
				{
					continue;
				}
				while (true)
				{
					return i;
				}
			}
			while (true)
			{
				return -1;
			}
		}

		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (!string.IsNullOrEmpty(Language1))
			{
				if (!string.IsNullOrEmpty(Language2))
				{
					string[] array = (from x in Language1.Split("( )-/\\".ToCharArray())
						where !string.IsNullOrEmpty(x)
						select x).ToArray();
					string[] source = Language2.Split("( )-/\\".ToCharArray());
					
					string[] array2 = source.Where(((string x) => !string.IsNullOrEmpty(x))).ToArray();
					int num = 0;
					string[] array3 = array;
					foreach (string value in array3)
					{
						if (array2.Contains(value))
						{
							num++;
						}
					}
					while (true)
					{
						string[] array4 = array2;
						foreach (string value2 in array4)
						{
							if (array.Contains(value2))
							{
								num++;
							}
						}
						while (true)
						{
							return num;
						}
					}
				}
			}
			return 0;
		}

		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = GetLanguageWithoutRegion(Language1);
			Language2 = GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return Language;
					}
				}
			}
			return Language.Substring(0, num).Trim();
		}

		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (GetLanguageIndex(LanguageName, false) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			mLanguages.Add(languageData);
			int count = mLanguages.Count;
			int i = 0;
			for (int count2 = mTerms.Count; i < count2; i++)
			{
				Array.Resize(ref mTerms[i].Languages, count);
				Array.Resize(ref mTerms[i].Languages_Touch, count);
				Array.Resize(ref mTerms[i].Flags, count);
			}
			while (true)
			{
				return;
			}
		}

		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = GetLanguageIndex(LanguageName);
			if (languageIndex < 0)
			{
				return;
			}
			int count = mLanguages.Count;
			int num = 0;
			int count2 = mTerms.Count;
			while (num < count2)
			{
				for (int i = languageIndex + 1; i < count; i++)
				{
					mTerms[num].Languages[i - 1] = mTerms[num].Languages[i];
					mTerms[num].Languages_Touch[i - 1] = mTerms[num].Languages_Touch[i];
					mTerms[num].Flags[i - 1] = mTerms[num].Flags[i];
				}
				while (true)
				{
					Array.Resize(ref mTerms[num].Languages, count - 1);
					Array.Resize(ref mTerms[num].Languages_Touch, count - 1);
					Array.Resize(ref mTerms[num].Flags, count - 1);
					num++;
					goto IL_0133;
				}
				IL_0133:;
			}
			mLanguages.RemoveAt(languageIndex);
		}

		public List<string> GetLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				list.Add(mLanguages[i].Name);
			}
			while (true)
			{
				return list;
			}
		}

		public string GetTermTranslation(string term)
		{
			int languageIndex = GetLanguageIndex(LocalizationManager.CurrentLanguage);
			if (languageIndex < 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return string.Empty;
					}
				}
			}
			TermData termData = GetTermData(term);
			if (termData != null)
			{
				return termData.GetTranslation(languageIndex);
			}
			return string.Empty;
		}

		public bool TryGetTermTranslation(string term, out string Translation)
		{
			int languageIndex = GetLanguageIndex(LocalizationManager.CurrentLanguage);
			if (languageIndex >= 0)
			{
				TermData termData = GetTermData(term);
				if (termData != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							Translation = termData.GetTranslation(languageIndex);
							if (Translation.IsNullOrEmpty())
							{
								Translation = termData.GetTranslation(0);
							}
							return true;
						}
					}
				}
			}
			Translation = string.Empty;
			return false;
		}

		public TermData AddTerm(string term)
		{
			return AddTerm(term, eTermType.Text);
		}

		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			if (mDictionary.Count == 0)
			{
				UpdateDictionary();
			}
			if (mDictionary.TryGetValue(term, out TermData value))
			{
				return value;
			}
			TermData termData = null;
			if (allowCategoryMistmatch)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						string keyFromFullTerm = GetKeyFromFullTerm(term);
						{
							foreach (KeyValuePair<string, TermData> item in mDictionary)
							{
								if (item.Value.IsTerm(keyFromFullTerm, true))
								{
									if (termData != null)
									{
										return null;
									}
									termData = item.Value;
								}
							}
							return termData;
						}
					}
					}
				}
			}
			return termData;
		}

		public bool ContainsTerm(string term)
		{
			return GetTermData(term) != null;
		}

		public List<string> GetTermsList()
		{
			if (mDictionary.Count != mTerms.Count)
			{
				UpdateDictionary();
			}
			return new List<string>(mDictionary.Keys);
		}

		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (mLanguages.Count == 0)
			{
				AddLanguage("English", "en");
			}
			TermData termData = GetTermData(NewTerm);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[mLanguages.Count];
				termData.Languages_Touch = new string[mLanguages.Count];
				termData.Flags = new byte[mLanguages.Count];
				mTerms.Add(termData);
				mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		public void RemoveTerm(string term)
		{
			int i = 0;
			for (int count = mTerms.Count; i < count; i++)
			{
				if (!(mTerms[i].Term == term))
				{
					continue;
				}
				while (true)
				{
					mTerms.RemoveAt(i);
					mDictionary.Remove(term);
					return;
				}
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

		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (!Term.StartsWith(EmptyCategory) || Term.Length <= EmptyCategory.Length || Term[EmptyCategory.Length] != '/')
			{
				return;
			}
			while (true)
			{
				Term = Term.Substring(EmptyCategory.Length + 1);
				return;
			}
		}

		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != mLanguages.Count)
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
			int i = 0;
			for (int count = mLanguages.Count; i < count; i++)
			{
				if (Source.GetLanguageIndex(mLanguages[i].Name) >= 0)
				{
					continue;
				}
				while (true)
				{
					return false;
				}
			}
			while (true)
			{
				if (Source.mTerms.Count != mTerms.Count)
				{
					return false;
				}
				for (int j = 0; j < mTerms.Count; j++)
				{
					if (Source.GetTermData(mTerms[j].Term) != null)
					{
						continue;
					}
					while (true)
					{
						return false;
					}
				}
				while (true)
				{
					return true;
				}
			}
		}

		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			for (int count = LocalizationManager.Sources.Count; i < count; i++)
			{
				LanguageSource languageSource = LocalizationManager.Sources[i];
				if (!(languageSource != null))
				{
					continue;
				}
				if (!languageSource.IsEqualTo(this))
				{
					continue;
				}
				if (!(languageSource != this))
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
			while (true)
			{
				return false;
			}
		}

		public void ClearAllData()
		{
			mTerms.Clear();
			mLanguages.Clear();
			mDictionary.Clear();
		}

		public UnityEngine.Object FindAsset(string Name)
		{
			if (Assets != null)
			{
				int i = 0;
				for (int num = Assets.Length; i < num; i++)
				{
					if (Assets[i] != null)
					{
						if (Assets[i].name == Name)
						{
							return Assets[i];
						}
					}
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			return Array.IndexOf(Assets, Obj) >= 0;
		}

		public void AddAsset(UnityEngine.Object Obj)
		{
			Array.Resize(ref Assets, Assets.Length + 1);
			Assets[Assets.Length - 1] = Obj;
		}
	}
}
