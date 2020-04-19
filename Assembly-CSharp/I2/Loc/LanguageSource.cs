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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Export_CSV(string, char)).MethodHandle;
				}
				if (Category == LanguageSource.EmptyCategory)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.AppendTerm(StringBuilder, int, string, TermData, string, string[], string[], char, byte, byte)).MethodHandle;
				}
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (aSecLanguages != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.AppendString(StringBuilder, string, char)).MethodHandle;
				}
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator + "\n\"").ToCharArray()) >= 0)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Export_Google_CreateData()).MethodHandle;
						}
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Import_CSV(string, string, eSpreadsheetUpdateMode, char)).MethodHandle;
				}
				if (this.ArrayContains(array[0], texts))
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
					if (UpdateMode == eSpreadsheetUpdateMode.Replace)
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
						this.ClearAllData();
					}
					if (array.Length > 2)
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
						if (this.ArrayContains(array[1], texts2))
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.ArrayContains(array[2], texts2))
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.ArrayContains(array[3], texts4))
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
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							num5 = this.GetLanguageIndexFromCode(text2);
						}
						else
						{
							num5 = this.GetLanguageIndex(text, true);
						}
						if (num5 < 0)
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
							LanguageData languageData = new LanguageData();
							languageData.Name = text;
							languageData.Code = text2;
							this.mLanguages.Add(languageData);
							num5 = this.mLanguages.Count - 1;
						}
						array2[i] = num5;
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
					num4 = this.mLanguages.Count;
					int j = 0;
					int count = this.mTerms.Count;
					while (j < count)
					{
						TermData termData = this.mTerms[j];
						if (termData.Languages.Length < num4)
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
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
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
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (UpdateMode == eSpreadsheetUpdateMode.Merge)
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
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								this.mTerms.Add(termData2);
								this.mDictionary.Add(text4, termData2);
							}
							else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
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
								goto IL_5EC;
							}
							if (num2 > 0)
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
								termData2.TermType = LanguageSource.GetTermType(array[num2]);
							}
							if (num3 > 0)
							{
								termData2.LocIdStr = array[num3];
							}
							int num6 = 0;
							if (UpdateMode == eSpreadsheetUpdateMode.Merge)
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
										for (;;)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
										string text5 = array[m + num];
										bool flag2 = text5.StartsWith("[i2auto]");
										if (flag2)
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
											text5 = text5.Substring("[isauto]".Length);
											if (text5.StartsWith("\""))
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
												if (text5.EndsWith("\""))
												{
													text5 = text5.Substring(1, text5.Length - 2);
												}
											}
										}
										int num7 = array2[m];
										if (flag)
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
											termData2.Languages_Touch[num7] = text5;
											if (flag2)
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
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													break;
												}
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.ArrayContains(string, string[])).MethodHandle;
					}
					return true;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetTermType(string)).MethodHandle;
					}
					return (eTermType)i;
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
			return eTermType.Text;
		}

		public event Action<LanguageSource> Event_OnSourceUpdateFromGoogle
		{
			add
			{
				Action<LanguageSource> action = this.Event_OnSourceUpdateFromGoogle;
				Action<LanguageSource> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<LanguageSource>>(ref this.Event_OnSourceUpdateFromGoogle, (Action<LanguageSource>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.add_Event_OnSourceUpdateFromGoogle(Action<LanguageSource>)).MethodHandle;
				}
			}
			remove
			{
				Action<LanguageSource> action = this.Event_OnSourceUpdateFromGoogle;
				Action<LanguageSource> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<LanguageSource>>(ref this.Event_OnSourceUpdateFromGoogle, (Action<LanguageSource>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.remove_Event_OnSourceUpdateFromGoogle(Action<LanguageSource>)).MethodHandle;
				}
			}
		}

		public void Import_Google(bool ForceUpdate = false)
		{
			if (this.GoogleUpdateFrequency == LanguageSource.eGoogleUpdateFrequency.Never)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Import_Google(bool)).MethodHandle;
				}
				return;
			}
			if (!Application.isPlaying)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.GoogleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Always)
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
					string string2 = PlayerPrefs.GetString("LastGoogleUpdate_" + sourcePlayerPrefName, string.Empty);
					DateTime d;
					if (DateTime.TryParse(string2, out d))
					{
						double totalDays = (DateTime.Now - d).TotalDays;
						LanguageSource.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
						if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Daily)
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
							if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Weekly)
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
								if (googleUpdateFrequency != LanguageSource.eGoogleUpdateFrequency.Monthly)
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
								}
								else if (totalDays < 31.0)
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
									return;
								}
							}
							else if (totalDays < 8.0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.<Import_Google_Coroutine>c__Iterator0.MoveNext()).MethodHandle;
				}
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (string.IsNullOrEmpty(www.error))
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
				if (www.text != "\"\"")
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
					PlayerPrefs.SetString("I2Source_" + this.Google_SpreadsheetKey, www.text);
					PlayerPrefs.Save();
					this.Import_Google_Result(www.text, eSpreadsheetUpdateMode.Replace);
					if (this.Event_OnSourceUpdateFromGoogle != null)
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
						this.Event_OnSourceUpdateFromGoogle(this);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.HasGoogleSpreadsheet()).MethodHandle;
				}
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.ClearAllData();
					}
					int num = JsonString.IndexOf("version=");
					int num2 = JsonString.IndexOf("script_version=");
					if (num >= 0)
					{
						if (num2 < 0)
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
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
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
							return empty;
						}
					}
					return "Invalid Response from Google, Most likely the WebService needs to be updated";
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Import_Google_Result(string, eSpreadsheetUpdateMode)).MethodHandle;
				}
			}
			Debug.Log("Language Source was up to date");
			return empty;
		}

		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetCategories(bool, List<string>)).MethodHandle;
				}
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						Categories.Add(categoryFromFullTerm);
					}
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
			Categories.Sort();
			return Categories;
		}

		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			string result;
			if (num < 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetKeyFromFullTerm(string, bool)).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetCategoryFromFullTerm(string, bool)).MethodHandle;
				}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.DeserializeFullTerm(string, string*, string*, bool)).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.Awake()).MethodHandle;
				}
				if (this.ManagerHasASimilarSource())
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				if (Application.isPlaying)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.UpdateDictionary(bool)).MethodHandle;
				}
				if (this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
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
					return;
				}
			}
			StringComparer stringComparer;
			if (this.CaseInsensitiveTerms)
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
				stringComparer = StringComparer.OrdinalIgnoreCase;
			}
			else
			{
				stringComparer = StringComparer.Ordinal;
			}
			StringComparer stringComparer2 = stringComparer;
			if (this.mDictionary.Comparer != stringComparer2)
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
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

		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while (parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetSourceName()).MethodHandle;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetLanguageIndex(string, bool)).MethodHandle;
					}
					return i;
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
			if (AllowDiscartingRegion)
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
				int num = -1;
				int num2 = 0;
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					int commonWordInLanguageNames = LanguageSource.GetCommonWordInLanguageNames(this.mLanguages[j].Name, language);
					if (commonWordInLanguageNames > num2)
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
						num2 = commonWordInLanguageNames;
						num = j;
					}
					j++;
				}
				if (num >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetLanguageIndexFromCode(string)).MethodHandle;
					}
					return i;
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
			return -1;
		}

		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (!string.IsNullOrEmpty(Language1))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetCommonWordInLanguageNames(string, string)).MethodHandle;
				}
				if (!string.IsNullOrEmpty(Language2))
				{
					string[] array = (from x in Language1.Split("( )-/\\".ToCharArray())
					where !string.IsNullOrEmpty(x)
					select x).ToArray<string>();
					IEnumerable<string> source = Language2.Split("( )-/\\".ToCharArray());
					if (LanguageSource.<>f__am$cache1 == null)
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
						LanguageSource.<>f__am$cache1 = ((string x) => !string.IsNullOrEmpty(x));
					}
					string[] array2 = source.Where(LanguageSource.<>f__am$cache1).ToArray<string>();
					int num = 0;
					foreach (string value in array)
					{
						if (array2.Contains(value))
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
							num++;
						}
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
					foreach (string value2 in array2)
					{
						if (array.Contains(value2))
						{
							num++;
						}
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
					return num;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetLanguageWithoutRegion(string)).MethodHandle;
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.AddLanguage(string, string)).MethodHandle;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.RemoveLanguage(string)).MethodHandle;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetLanguages()).MethodHandle;
			}
			return list;
		}

		public string GetTermTranslation(string term)
		{
			int languageIndex = this.GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
			if (languageIndex < 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetTermTranslation(string)).MethodHandle;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.TryGetTermTranslation(string, string*)).MethodHandle;
				}
				TermData termData = this.GetTermData(term, false);
				if (termData != null)
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
					Translation = termData.GetTranslation(languageIndex);
					if (Translation.IsNullOrEmpty())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetTermData(string, bool)).MethodHandle;
				}
				return null;
			}
			if (this.mDictionary.Count == 0)
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				string keyFromFullTerm = LanguageSource.GetKeyFromFullTerm(term, false);
				foreach (KeyValuePair<string, TermData> keyValuePair in this.mDictionary)
				{
					if (keyValuePair.Value.IsTerm(keyFromFullTerm, true))
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
						if (termData != null)
						{
							return null;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.GetTermsList()).MethodHandle;
				}
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.RemoveTerm(string)).MethodHandle;
					}
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
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
		}

		public unsafe static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSource.EmptyCategory) && Term.Length > LanguageSource.EmptyCategory.Length && Term[LanguageSource.EmptyCategory.Length] == '/')
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.ValidateFullTerm(string*)).MethodHandle;
				}
				Term = Term.Substring(LanguageSource.EmptyCategory.Length + 1);
			}
		}

		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.IsEqualTo(LanguageSource)).MethodHandle;
				}
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true) < 0)
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
					return false;
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
			if (Source.mTerms.Count != this.mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < this.mTerms.Count; j++)
			{
				if (Source.GetTermData(this.mTerms[j].Term, false) == null)
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
					return false;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.ManagerHasASimilarSource()).MethodHandle;
					}
					if (languageSource.IsEqualTo(this))
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
						if (languageSource != this)
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
							return true;
						}
					}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LanguageSource.FindAsset(string)).MethodHandle;
				}
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null)
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
