using System;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Localize")]
	public class Localize : MonoBehaviour
	{
		public string mTerm = string.Empty;

		public string mTermSecondary = string.Empty;

		[NonSerialized]
		public string FinalTerm;

		[NonSerialized]
		public string FinalSecondaryTerm;

		public Localize.TermModification PrimaryTermModifier;

		public Localize.TermModification SecondaryTermModifier;

		public bool LocalizeOnAwake = true;

		private string LastLocalizedLanguage;

		public LanguageSource Source;

		[NonSerialized]
		public UnityEngine.Object mTarget;

		public Localize.DelegateSetFinalTerms EventSetFinalTerms;

		public Localize.DelegateDoLocalize EventDoLocalize;

		public bool CanUseSecondaryTerm;

		public bool AllowMainTermToBeRTL;

		public bool AllowSecondTermToBeRTL;

		public bool IgnoreRTL;

		public int MaxCharactersInRTL;

		public bool CorrectAlignmentForRTL = true;

		public UnityEngine.Object[] TranslatedObjects;

		public EventCallback LocalizeCallBack = new EventCallback();

		public static string MainTranslation;

		public static string SecondaryTranslation;

		public static string CallBackTerm;

		public static string CallBackSecondaryTerm;

		public static Localize CurrentLocalizeComponent;

		private TextMeshPro mTarget_TMPLabel;

		private TextMeshProUGUI mTarget_TMPUGUILabel;

		private TextAlignmentOptions mOriginalAlignmentTMPro = TextAlignmentOptions.TopLeft;

		[NonSerialized]
		public string TMP_previewLanguage;

		private Text mTarget_uGUI_Text;

		private Image mTarget_uGUI_Image;

		private RawImage mTarget_uGUI_RawImage;

		private TextAnchor mOriginalAlignmentUGUI;

		private GUIText mTarget_GUIText;

		private TextMesh mTarget_TextMesh;

		private AudioSource mTarget_AudioSource;

		private GUITexture mTarget_GUITexture;

		private GameObject mTarget_Child;

		private bool mInitializeAlignment = true;

		private TextAlignment mOriginalAlignmentStd;

		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.mTerm = value;
			}
		}

		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.mTermSecondary = value;
			}
		}

		public event Action EventFindTarget
		{
			add
			{
				Action action = this.EventFindTarget;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.EventFindTarget, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.add_EventFindTarget(Action)).MethodHandle;
				}
			}
			remove
			{
				Action action = this.EventFindTarget;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.EventFindTarget, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.remove_EventFindTarget(Action)).MethodHandle;
				}
			}
		}

		private void Awake()
		{
			this.RegisterTargets();
			this.EventFindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		private void RegisterTargets()
		{
			if (this.EventFindTarget != null)
			{
				return;
			}
			Localize.RegisterEvents_NGUI();
			Localize.RegisterEvents_DFGUI();
			this.RegisterEvents_UGUI();
			Localize.RegisterEvents_2DToolKit();
			this.RegisterEvents_TextMeshPro();
			this.RegisterEvents_UnityStandard();
			Localize.RegisterEvents_SVG();
		}

		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		public void OnLocalize(bool Force = false)
		{
			if (!Force)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.OnLocalize(bool)).MethodHandle;
				}
				if (base.enabled)
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
					if (!(base.gameObject == null))
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
						if (base.gameObject.activeInHierarchy)
						{
							goto IL_60;
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
					}
				}
				return;
			}
			IL_60:
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
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
			if (!Force)
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
				if (this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
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
					return;
				}
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (!string.IsNullOrEmpty(this.FinalTerm))
			{
				if (!string.IsNullOrEmpty(this.FinalSecondaryTerm))
				{
					goto IL_EC;
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
			this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			IL_EC:
			if (string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = LocalizationManager.GetTermTranslation(this.FinalTerm);
			Localize.SecondaryTranslation = LocalizationManager.GetTermTranslation(this.FinalSecondaryTerm);
			if (string.IsNullOrEmpty(Localize.MainTranslation) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			this.LocalizeCallBack.Execute(this);
			if (!this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!this.HasTargetCache())
			{
				return;
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
				if (!this.IgnoreRTL)
				{
					if (this.AllowMainTermToBeRTL)
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
						if (!string.IsNullOrEmpty(Localize.MainTranslation))
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
							Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL);
						}
					}
					if (this.AllowSecondTermToBeRTL)
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
						if (!string.IsNullOrEmpty(Localize.SecondaryTranslation))
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
							Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
						}
					}
				}
			}
			switch (this.PrimaryTermModifier)
			{
			case Localize.TermModification.ToUpper:
				Localize.MainTranslation = Localize.MainTranslation.ToUpper();
				break;
			case Localize.TermModification.ToLower:
				Localize.MainTranslation = Localize.MainTranslation.ToLower();
				break;
			case Localize.TermModification.ToUpperFirst:
				Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
				break;
			case Localize.TermModification.ToTitle:
				Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
				break;
			}
			switch (this.SecondaryTermModifier)
			{
			case Localize.TermModification.ToUpper:
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
				break;
			case Localize.TermModification.ToLower:
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
				break;
			case Localize.TermModification.ToUpperFirst:
				Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
				break;
			case Localize.TermModification.ToTitle:
				Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
				break;
			}
			this.EventDoLocalize(Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		public bool FindTarget()
		{
			if (this.EventFindTarget == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.FindTarget()).MethodHandle;
				}
				this.RegisterTargets();
			}
			this.EventFindTarget();
			return this.HasTargetCache();
		}

		public unsafe void FindAndCacheTarget<T>(ref T targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL) where T : Component
		{
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as T);
			}
			else
			{
				this.mTarget = (targetCache = base.GetComponent<T>());
			}
			if (targetCache != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.FindAndCacheTarget(T*, Localize.DelegateSetFinalTerms, Localize.DelegateDoLocalize, bool, bool, bool)).MethodHandle;
				}
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private unsafe void FindAndCacheTarget(ref GameObject targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		{
			if (this.mTarget != targetCache && targetCache)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.FindAndCacheTarget(GameObject*, Localize.DelegateSetFinalTerms, Localize.DelegateDoLocalize, bool, bool, bool)).MethodHandle;
				}
				UnityEngine.Object.Destroy(targetCache);
			}
			if (this.mTarget != null)
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
				targetCache = (this.mTarget as GameObject);
			}
			else
			{
				Transform transform = base.transform;
				GameObject gameObject;
				if (transform.childCount < 1)
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
					gameObject = null;
				}
				else
				{
					gameObject = transform.GetChild(0).gameObject;
				}
				GameObject gameObject2;
				targetCache = (gameObject2 = gameObject);
				this.mTarget = gameObject2;
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private bool HasTargetCache()
		{
			return this.EventDoLocalize != null;
		}

		public unsafe void GetFinalTerms(out string PrimaryTerm, out string SecondaryTerm)
		{
			if (this.EventSetFinalTerms != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.GetFinalTerms(string*, string*)).MethodHandle;
				}
				if (this.mTarget || this.HasTargetCache())
				{
					goto IL_41;
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
			this.FindTarget();
			IL_41:
			PrimaryTerm = string.Empty;
			SecondaryTerm = string.Empty;
			if (this.mTarget != null)
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
				if (!string.IsNullOrEmpty(this.mTerm))
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
					if (!string.IsNullOrEmpty(this.mTermSecondary))
					{
						goto IL_BA;
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
				if (this.EventSetFinalTerms != null)
				{
					this.EventSetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
				}
			}
			IL_BA:
			if (!string.IsNullOrEmpty(this.mTerm))
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
				PrimaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				SecondaryTerm = this.mTermSecondary;
			}
		}

		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.EventSetFinalTerms != null)
			{
				this.EventSetFinalTerms(null, null, out text, out text2);
			}
			string result;
			if (string.IsNullOrEmpty(text))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.GetMainTargetsText()).MethodHandle;
				}
				result = this.mTerm;
			}
			else
			{
				result = text;
			}
			return result;
		}

		private unsafe void SetFinalTerms(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm, bool RemoveNonASCII)
		{
			string text;
			if (RemoveNonASCII)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms(string, string, string*, string*, bool)).MethodHandle;
				}
				if (!string.IsNullOrEmpty(Main))
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
					text = Regex.Replace(Main, "[^a-zA-Z0-9_ ]+", " ");
					goto IL_3F;
				}
			}
			text = Main;
			IL_3F:
			PrimaryTerm = text;
			SecondaryTerm = Secondary;
		}

		public void SetTerm(string primary, string secondary = null)
		{
			if (!string.IsNullOrEmpty(primary))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetTerm(string, string)).MethodHandle;
				}
				this.Term = primary;
				this.FinalTerm = primary;
			}
			if (!string.IsNullOrEmpty(secondary))
			{
				this.SecondaryTerm = secondary;
				this.FinalSecondaryTerm = secondary;
			}
			this.OnLocalize(true);
		}

		private T GetSecondaryTranslatedObj<T>(ref string MainTranslation, ref string SecondaryTranslation) where T : UnityEngine.Object
		{
			string translation = null;
			this.DeserializeTranslation(MainTranslation, out MainTranslation, out translation);
			T @object = this.GetObject<T>(translation);
			if (@object == null)
			{
				@object = this.GetObject<T>(SecondaryTranslation);
			}
			return @object;
		}

		private T GetObject<T>(string Translation) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return (T)((object)null);
			}
			T translatedObject = this.GetTranslatedObject<T>(Translation);
			if (translatedObject == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.GetObject(string)).MethodHandle;
				}
				int num = Translation.LastIndexOfAny("/\\".ToCharArray());
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
					Translation = Translation.Substring(num + 1);
					translatedObject = this.GetTranslatedObject<T>(Translation);
				}
			}
			return translatedObject;
		}

		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		private unsafe void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DeserializeTranslation(string, string*, string*)).MethodHandle;
				}
				if (translation.Length > 1)
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
					if (translation[0] == '[')
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
						int num = translation.IndexOf(']');
						if (num > 0)
						{
							secondary = translation.Substring(1, num - 1);
							value = translation.Substring(num + 1);
							return;
						}
					}
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				return (T)((object)null);
			}
			if (this.TranslatedObjects != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.FindTranslatedObject(string)).MethodHandle;
				}
				int i = 0;
				int num = this.TranslatedObjects.Length;
				while (i < num)
				{
					if (this.TranslatedObjects[i] as T != null)
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
						if (value == this.TranslatedObjects[i].name)
						{
							return this.TranslatedObjects[i] as T;
						}
					}
					i++;
				}
			}
			T t = LocalizationManager.FindAsset(value) as T;
			if (t)
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
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		public bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.TranslatedObjects, Obj) >= 0 || ResourceManager.pInstance.HasAsset(Obj);
		}

		public void AddTranslatedObject(UnityEngine.Object Obj)
		{
			Array.Resize<UnityEngine.Object>(ref this.TranslatedObjects, this.TranslatedObjects.Length + 1);
			this.TranslatedObjects[this.TranslatedObjects.Length - 1] = Obj;
		}

		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		public static void RegisterEvents_2DToolKit()
		{
		}

		public static void RegisterEvents_DFGUI()
		{
		}

		public static void RegisterEvents_NGUI()
		{
		}

		public static void RegisterEvents_SVG()
		{
		}

		public void RegisterEvents_TextMeshPro()
		{
			this.EventFindTarget += this.FindTarget_TMPLabel;
			this.EventFindTarget += this.FindTarget_TMPUGUILabel;
		}

		private void FindTarget_TMPLabel()
		{
			this.FindAndCacheTarget<TextMeshPro>(ref this.mTarget_TMPLabel, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TMPLabel), new Localize.DelegateDoLocalize(this.DoLocalize_TMPLabel), true, true, false);
		}

		private void FindTarget_TMPUGUILabel()
		{
			this.FindAndCacheTarget<TextMeshProUGUI>(ref this.mTarget_TMPUGUILabel, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TMPUGUILabel), new Localize.DelegateDoLocalize(this.DoLocalize_TMPUGUILabel), true, true, false);
		}

		private void SetFinalTerms_TMPLabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_TMPLabel.font != null)) ? string.Empty : this.mTarget_TMPLabel.font.name;
			this.SetFinalTerms(this.mTarget_TMPLabel.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		private unsafe void SetFinalTerms_TMPUGUILabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string text;
			if (this.mTarget_TMPUGUILabel.font != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms_TMPUGUILabel(string, string, string*, string*)).MethodHandle;
				}
				text = this.mTarget_TMPUGUILabel.font.name;
			}
			else
			{
				text = string.Empty;
			}
			string secondary = text;
			this.SetFinalTerms(this.mTarget_TMPUGUILabel.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void DoLocalize_TMPLabel(string MainTranslation, string SecondaryTranslation)
		{
			if (!Application.isPlaying)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_TMPLabel(string, string)).MethodHandle;
				}
			}
			TMP_FontAsset tmp_FontAsset = this.GetSecondaryTranslatedObj<TMP_FontAsset>(ref MainTranslation, ref SecondaryTranslation);
			if (tmp_FontAsset != null)
			{
				if (this.mTarget_TMPLabel.font != tmp_FontAsset)
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
					this.mTarget_TMPLabel.font = tmp_FontAsset;
				}
			}
			else
			{
				Material secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Material>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget_TMPLabel.fontMaterial != secondaryTranslatedObj)
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
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget_TMPLabel.font.name))
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
						tmp_FontAsset = this.GetTMPFontFromMaterial(secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
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
							this.mTarget_TMPLabel.font = tmp_FontAsset;
						}
					}
					this.mTarget_TMPLabel.fontSharedMaterial = secondaryTranslatedObj;
				}
			}
			if (this.mInitializeAlignment)
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
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentTMPro = this.mTarget_TMPLabel.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
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
				if (this.mTarget_TMPLabel.text != MainTranslation)
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
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						int alignment = (int)this.mTarget_TMPLabel.alignment;
						if (alignment % 4 == 0)
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
							this.mTarget_TMPLabel.alignment = ((!LocalizationManager.IsRight2Left) ? this.mOriginalAlignmentTMPro : (this.mTarget_TMPLabel.alignment + 2));
						}
						else if (alignment % 4 == 2)
						{
							TMP_Text tmp_Text = this.mTarget_TMPLabel;
							TextAlignmentOptions alignment2;
							if (LocalizationManager.IsRight2Left)
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
								alignment2 = this.mTarget_TMPLabel.alignment - 2;
							}
							else
							{
								alignment2 = this.mOriginalAlignmentTMPro;
							}
							tmp_Text.alignment = alignment2;
						}
					}
					this.mTarget_TMPLabel.text = MainTranslation;
				}
			}
		}

		public void DoLocalize_TMPUGUILabel(string MainTranslation, string SecondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = this.GetSecondaryTranslatedObj<TMP_FontAsset>(ref MainTranslation, ref SecondaryTranslation);
			if (tmp_FontAsset != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_TMPUGUILabel(string, string)).MethodHandle;
				}
				if (this.mTarget_TMPUGUILabel.font != tmp_FontAsset)
				{
					this.mTarget_TMPUGUILabel.font = tmp_FontAsset;
				}
			}
			else
			{
				Material secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Material>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj != null)
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
					if (this.mTarget_TMPUGUILabel.fontMaterial != secondaryTranslatedObj)
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
						if (!secondaryTranslatedObj.name.StartsWith(this.mTarget_TMPUGUILabel.font.name))
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
							tmp_FontAsset = this.GetTMPFontFromMaterial(secondaryTranslatedObj.name);
							if (tmp_FontAsset != null)
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
								this.mTarget_TMPUGUILabel.font = tmp_FontAsset;
							}
						}
						this.mTarget_TMPUGUILabel.fontSharedMaterial = secondaryTranslatedObj;
					}
				}
			}
			if (this.mInitializeAlignment)
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
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentTMPro = this.mTarget_TMPUGUILabel.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
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
				if (this.mTarget_TMPUGUILabel.text != MainTranslation)
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
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
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
						int alignment = (int)this.mTarget_TMPUGUILabel.alignment;
						if (alignment % 4 == 0)
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
							TMP_Text tmp_Text = this.mTarget_TMPUGUILabel;
							TextAlignmentOptions alignment2;
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
								alignment2 = this.mTarget_TMPUGUILabel.alignment + 2;
							}
							else
							{
								alignment2 = this.mOriginalAlignmentTMPro;
							}
							tmp_Text.alignment = alignment2;
						}
						else if (alignment % 4 == 2)
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
							this.mTarget_TMPUGUILabel.alignment = ((!LocalizationManager.IsRight2Left) ? this.mOriginalAlignmentTMPro : (this.mTarget_TMPUGUILabel.alignment - 2));
						}
					}
					this.mTarget_TMPUGUILabel.text = MainTranslation;
				}
			}
		}

		private TMP_FontAsset GetTMPFontFromMaterial(string matName)
		{
			int num = matName.IndexOf(" SDF");
			if (num > 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.GetTMPFontFromMaterial(string)).MethodHandle;
				}
				string translation = matName.Substring(0, num + " SDF".Length);
				return this.GetObject<TMP_FontAsset>(translation);
			}
			return null;
		}

		public void RegisterEvents_UGUI()
		{
			this.EventFindTarget += this.FindTarget_uGUI_Text;
			this.EventFindTarget += this.FindTarget_uGUI_Image;
			this.EventFindTarget += this.FindTarget_uGUI_RawImage;
		}

		private void FindTarget_uGUI_Text()
		{
			this.FindAndCacheTarget<Text>(ref this.mTarget_uGUI_Text, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Text), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Text), true, true, false);
		}

		private void FindTarget_uGUI_Image()
		{
			this.FindAndCacheTarget<Image>(ref this.mTarget_uGUI_Image, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Image), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Image), false, false, false);
		}

		private void FindTarget_uGUI_RawImage()
		{
			this.FindAndCacheTarget<RawImage>(ref this.mTarget_uGUI_RawImage, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_RawImage), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_RawImage), false, false, false);
		}

		private unsafe void SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string text;
			if (this.mTarget_uGUI_Text.font != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms_uGUI_Text(string, string, string*, string*)).MethodHandle;
				}
				text = this.mTarget_uGUI_Text.font.name;
			}
			else
			{
				text = string.Empty;
			}
			string secondary = text;
			this.SetFinalTerms(this.mTarget_uGUI_Text.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public void SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_uGUI_RawImage.texture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public static T FindInParents<T>(Transform tr) where T : Component
		{
			if (!tr)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.FindInParents(Transform)).MethodHandle;
				}
				return (T)((object)null);
			}
			T component = tr.GetComponent<T>();
			while (!component)
			{
				if (!tr)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						return component;
					}
				}
				else
				{
					component = tr.GetComponent<T>();
					tr = tr.parent;
				}
			}
			return component;
		}

		public void DoLocalize_uGUI_Text(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_uGUI_Text(string, string)).MethodHandle;
				}
				if (secondaryTranslatedObj != this.mTarget_uGUI_Text.font)
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
					this.mTarget_uGUI_Text.font = secondaryTranslatedObj;
				}
			}
			if (this.mInitializeAlignment)
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
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentUGUI = this.mTarget_uGUI_Text.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_uGUI_Text.text != MainTranslation)
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
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
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
					if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperLeft)
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
						if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperCenter)
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
							if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperRight)
							{
								if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleLeft)
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
									if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleCenter)
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
										if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleRight)
										{
											if (this.mTarget_uGUI_Text.alignment != TextAnchor.LowerLeft)
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
												if (this.mTarget_uGUI_Text.alignment != TextAnchor.LowerCenter)
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
													if (this.mTarget_uGUI_Text.alignment != TextAnchor.LowerRight)
													{
														goto IL_219;
													}
												}
											}
											Text text = this.mTarget_uGUI_Text;
											TextAnchor alignment;
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
												alignment = TextAnchor.LowerRight;
											}
											else
											{
												alignment = this.mOriginalAlignmentUGUI;
											}
											text.alignment = alignment;
											goto IL_219;
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
								}
								Text text2 = this.mTarget_uGUI_Text;
								TextAnchor alignment2;
								if (LocalizationManager.IsRight2Left)
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
									alignment2 = TextAnchor.MiddleRight;
								}
								else
								{
									alignment2 = this.mOriginalAlignmentUGUI;
								}
								text2.alignment = alignment2;
								goto IL_219;
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
					Text text3 = this.mTarget_uGUI_Text;
					TextAnchor alignment3;
					if (LocalizationManager.IsRight2Left)
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
						alignment3 = TextAnchor.UpperRight;
					}
					else
					{
						alignment3 = this.mOriginalAlignmentUGUI;
					}
					text3.alignment = alignment3;
				}
				IL_219:
				this.mTarget_uGUI_Text.text = MainTranslation;
				this.mTarget_uGUI_Text.SetVerticesDirty();
			}
		}

		public void DoLocalize_uGUI_Image(string MainTranslation, string SecondaryTranslation)
		{
			Sprite sprite = this.mTarget_uGUI_Image.sprite;
			if (!(sprite == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_uGUI_Image(string, string)).MethodHandle;
				}
				if (!(sprite.name != MainTranslation))
				{
					return;
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
			this.mTarget_uGUI_Image.sprite = this.FindTranslatedObject<Sprite>(MainTranslation);
		}

		public void DoLocalize_uGUI_RawImage(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_uGUI_RawImage.texture;
			if (!(texture == null))
			{
				if (!(texture.name != MainTranslation))
				{
					return;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_uGUI_RawImage(string, string)).MethodHandle;
				}
			}
			this.mTarget_uGUI_RawImage.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		public void RegisterEvents_UnityStandard()
		{
			this.EventFindTarget += this.FindTarget_GUIText;
			this.EventFindTarget += this.FindTarget_TextMesh;
			this.EventFindTarget += this.FindTarget_AudioSource;
			this.EventFindTarget += this.FindTarget_GUITexture;
			this.EventFindTarget += this.FindTarget_Child;
		}

		private void FindTarget_GUIText()
		{
			this.FindAndCacheTarget<GUIText>(ref this.mTarget_GUIText, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUIText), new Localize.DelegateDoLocalize(this.DoLocalize_GUIText), true, true, false);
		}

		private void FindTarget_TextMesh()
		{
			this.FindAndCacheTarget<TextMesh>(ref this.mTarget_TextMesh, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TextMesh), new Localize.DelegateDoLocalize(this.DoLocalize_TextMesh), true, true, false);
		}

		private void FindTarget_AudioSource()
		{
			this.FindAndCacheTarget<AudioSource>(ref this.mTarget_AudioSource, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_AudioSource), new Localize.DelegateDoLocalize(this.DoLocalize_AudioSource), false, false, false);
		}

		private void FindTarget_GUITexture()
		{
			this.FindAndCacheTarget<GUITexture>(ref this.mTarget_GUITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUITexture), new Localize.DelegateDoLocalize(this.DoLocalize_GUITexture), false, false, false);
		}

		private void FindTarget_Child()
		{
			this.FindAndCacheTarget(ref this.mTarget_Child, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_Child), new Localize.DelegateDoLocalize(this.DoLocalize_Child), false, false, false);
		}

		public unsafe void SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (string.IsNullOrEmpty(Secondary))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms_GUIText(string, string, string*, string*)).MethodHandle;
				}
				if (this.mTarget_GUIText.font != null)
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
					Secondary = this.mTarget_GUIText.font.name;
				}
			}
			this.SetFinalTerms(this.mTarget_GUIText.text, Secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public void SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string secondary = (!(this.mTarget_TextMesh.font != null)) ? string.Empty : this.mTarget_TextMesh.font.name;
			this.SetFinalTerms(this.mTarget_TextMesh.text, secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public unsafe void SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (this.mTarget_GUITexture)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms_GUITexture(string, string, string*, string*)).MethodHandle;
				}
				if (this.mTarget_GUITexture.texture)
				{
					this.SetFinalTerms(this.mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
					return;
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
			this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public unsafe void SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (this.mTarget_AudioSource)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.SetFinalTerms_AudioSource(string, string, string*, string*)).MethodHandle;
				}
				if (this.mTarget_AudioSource.clip)
				{
					this.SetFinalTerms(this.mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
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
			this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public void SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_Child.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		private void DoLocalize_GUIText(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_GUIText(string, string)).MethodHandle;
				}
				if (this.mTarget_GUIText.font != secondaryTranslatedObj)
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
					this.mTarget_GUIText.font = secondaryTranslatedObj;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentStd = this.mTarget_GUIText.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
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
				if (this.mTarget_GUIText.text != MainTranslation)
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
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
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
						GUIText guitext = this.mTarget_GUIText;
						TextAlignment alignment;
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
							alignment = TextAlignment.Right;
						}
						else
						{
							alignment = this.mOriginalAlignmentStd;
						}
						guitext.alignment = alignment;
					}
					this.mTarget_GUIText.text = MainTranslation;
				}
			}
		}

		private void DoLocalize_TextMesh(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_TextMesh(string, string)).MethodHandle;
				}
				if (this.mTarget_TextMesh.font != secondaryTranslatedObj)
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
					this.mTarget_TextMesh.font = secondaryTranslatedObj;
					base.GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
				}
			}
			if (this.mInitializeAlignment)
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
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentStd = this.mTarget_TextMesh.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
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
				if (this.mTarget_TextMesh.text != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
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
						this.mTarget_TextMesh.alignment = ((!LocalizationManager.IsRight2Left) ? this.mOriginalAlignmentStd : TextAlignment.Right);
					}
					this.mTarget_TextMesh.text = MainTranslation;
				}
			}
		}

		private void DoLocalize_AudioSource(string MainTranslation, string SecondaryTranslation)
		{
			bool isPlaying = this.mTarget_AudioSource.isPlaying;
			AudioClip clip = this.mTarget_AudioSource.clip;
			AudioClip audioClip = this.FindTranslatedObject<AudioClip>(MainTranslation);
			if (clip != audioClip)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_AudioSource(string, string)).MethodHandle;
				}
				this.mTarget_AudioSource.clip = audioClip;
			}
			if (isPlaying)
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
				if (this.mTarget_AudioSource.clip)
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
					this.mTarget_AudioSource.Play();
				}
			}
		}

		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_GUITexture.texture;
			if (texture != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_GUITexture(string, string)).MethodHandle;
				}
				if (texture.name != MainTranslation)
				{
					this.mTarget_GUITexture.texture = this.FindTranslatedObject<Texture>(MainTranslation);
				}
			}
		}

		private void DoLocalize_Child(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_Child)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Localize.DoLocalize_Child(string, string)).MethodHandle;
				}
				if (this.mTarget_Child.name == MainTranslation)
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
			GameObject gameObject = this.mTarget_Child;
			GameObject gameObject2 = this.FindTranslatedObject<GameObject>(MainTranslation);
			if (gameObject2)
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
				this.mTarget_Child = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
				Transform transform = this.mTarget_Child.transform;
				Transform transform2;
				if (gameObject)
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
					transform2 = gameObject.transform;
				}
				else
				{
					transform2 = gameObject2.transform;
				}
				Transform transform3 = transform2;
				transform.parent = base.transform;
				transform.localScale = transform3.localScale;
				transform.localRotation = transform3.localRotation;
				transform.localPosition = transform3.localPosition;
			}
			if (gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		public enum TermModification
		{
			DontModify,
			ToUpper,
			ToLower,
			ToUpperFirst,
			ToTitle
		}

		public delegate void DelegateSetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		public delegate void DelegateDoLocalize(string primaryTerm, string secondaryTerm);
	}
}
