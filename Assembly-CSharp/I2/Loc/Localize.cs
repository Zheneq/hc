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

		private Action EventFindTargetHolder;
	public event Action EventFindTarget
		{
			add
			{
				Action action = this.EventFindTargetHolder;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.EventFindTargetHolder, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.EventFindTargetHolder;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.EventFindTargetHolder, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private void Awake()
		{
			this.RegisterTargets();
			this.EventFindTargetHolder();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		private void RegisterTargets()
		{
			if (this.EventFindTargetHolder != null)
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
				if (base.enabled)
				{
					if (!(base.gameObject == null))
					{
						if (base.gameObject.activeInHierarchy)
						{
							goto IL_60;
						}
					}
				}
				return;
			}
			IL_60:
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!Force)
			{
				if (this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
				{
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
				if (!this.IgnoreRTL)
				{
					if (this.AllowMainTermToBeRTL)
					{
						if (!string.IsNullOrEmpty(Localize.MainTranslation))
						{
							Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL);
						}
					}
					if (this.AllowSecondTermToBeRTL)
					{
						if (!string.IsNullOrEmpty(Localize.SecondaryTranslation))
						{
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
			if (this.EventFindTargetHolder == null)
			{
				this.RegisterTargets();
			}
			this.EventFindTargetHolder();
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
				UnityEngine.Object.Destroy(targetCache);
			}
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as GameObject);
			}
			else
			{
				Transform transform = base.transform;
				GameObject gameObject;
				if (transform.childCount < 1)
				{
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
				if (this.mTarget || this.HasTargetCache())
				{
					goto IL_41;
				}
			}
			this.FindTarget();
			IL_41:
			PrimaryTerm = string.Empty;
			SecondaryTerm = string.Empty;
			if (this.mTarget != null)
			{
				if (!string.IsNullOrEmpty(this.mTerm))
				{
					if (!string.IsNullOrEmpty(this.mTermSecondary))
					{
						goto IL_BA;
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
				if (!string.IsNullOrEmpty(Main))
				{
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
				int num = Translation.LastIndexOfAny("/\\".ToCharArray());
				if (num >= 0)
				{
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
				if (translation.Length > 1)
				{
					if (translation[0] == '[')
					{
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
				int i = 0;
				int num = this.TranslatedObjects.Length;
				while (i < num)
				{
					if (this.TranslatedObjects[i] as T != null)
					{
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
			this.EventFindTargetHolder += this.FindTarget_TMPLabel;
			this.EventFindTargetHolder += this.FindTarget_TMPUGUILabel;
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
			}
			TMP_FontAsset tmp_FontAsset = this.GetSecondaryTranslatedObj<TMP_FontAsset>(ref MainTranslation, ref SecondaryTranslation);
			if (tmp_FontAsset != null)
			{
				if (this.mTarget_TMPLabel.font != tmp_FontAsset)
				{
					this.mTarget_TMPLabel.font = tmp_FontAsset;
				}
			}
			else
			{
				Material secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Material>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget_TMPLabel.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget_TMPLabel.font.name))
					{
						tmp_FontAsset = this.GetTMPFontFromMaterial(secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							this.mTarget_TMPLabel.font = tmp_FontAsset;
						}
					}
					this.mTarget_TMPLabel.fontSharedMaterial = secondaryTranslatedObj;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentTMPro = this.mTarget_TMPLabel.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
			{
				if (this.mTarget_TMPLabel.text != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						int alignment = (int)this.mTarget_TMPLabel.alignment;
						if (alignment % 4 == 0)
						{
							this.mTarget_TMPLabel.alignment = ((!LocalizationManager.IsRight2Left) ? this.mOriginalAlignmentTMPro : (this.mTarget_TMPLabel.alignment + 2));
						}
						else if (alignment % 4 == 2)
						{
							TMP_Text tmp_Text = this.mTarget_TMPLabel;
							TextAlignmentOptions alignment2;
							if (LocalizationManager.IsRight2Left)
							{
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
					if (this.mTarget_TMPUGUILabel.fontMaterial != secondaryTranslatedObj)
					{
						if (!secondaryTranslatedObj.name.StartsWith(this.mTarget_TMPUGUILabel.font.name))
						{
							tmp_FontAsset = this.GetTMPFontFromMaterial(secondaryTranslatedObj.name);
							if (tmp_FontAsset != null)
							{
								this.mTarget_TMPUGUILabel.font = tmp_FontAsset;
							}
						}
						this.mTarget_TMPUGUILabel.fontSharedMaterial = secondaryTranslatedObj;
					}
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentTMPro = this.mTarget_TMPUGUILabel.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
			{
				if (this.mTarget_TMPUGUILabel.text != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						int alignment = (int)this.mTarget_TMPUGUILabel.alignment;
						if (alignment % 4 == 0)
						{
							TMP_Text tmp_Text = this.mTarget_TMPUGUILabel;
							TextAlignmentOptions alignment2;
							if (LocalizationManager.IsRight2Left)
							{
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
				string translation = matName.Substring(0, num + " SDF".Length);
				return this.GetObject<TMP_FontAsset>(translation);
			}
			return null;
		}

		public void RegisterEvents_UGUI()
		{
			this.EventFindTargetHolder += this.FindTarget_uGUI_Text;
			this.EventFindTargetHolder += this.FindTarget_uGUI_Image;
			this.EventFindTargetHolder += this.FindTarget_uGUI_RawImage;
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
				if (secondaryTranslatedObj != this.mTarget_uGUI_Text.font)
				{
					this.mTarget_uGUI_Text.font = secondaryTranslatedObj;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentUGUI = this.mTarget_uGUI_Text.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_uGUI_Text.text != MainTranslation)
			{
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
				{
					if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperLeft)
					{
						if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperCenter)
						{
							if (this.mTarget_uGUI_Text.alignment != TextAnchor.UpperRight)
							{
								if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleLeft)
								{
									if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleCenter)
									{
										if (this.mTarget_uGUI_Text.alignment != TextAnchor.MiddleRight)
										{
											if (this.mTarget_uGUI_Text.alignment != TextAnchor.LowerLeft)
											{
												if (this.mTarget_uGUI_Text.alignment != TextAnchor.LowerCenter)
												{
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
												alignment = TextAnchor.LowerRight;
											}
											else
											{
												alignment = this.mOriginalAlignmentUGUI;
											}
											text.alignment = alignment;
											goto IL_219;
										}
									}
								}
								Text text2 = this.mTarget_uGUI_Text;
								TextAnchor alignment2;
								if (LocalizationManager.IsRight2Left)
								{
									alignment2 = TextAnchor.MiddleRight;
								}
								else
								{
									alignment2 = this.mOriginalAlignmentUGUI;
								}
								text2.alignment = alignment2;
								goto IL_219;
							}
						}
					}
					Text text3 = this.mTarget_uGUI_Text;
					TextAnchor alignment3;
					if (LocalizationManager.IsRight2Left)
					{
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
				if (!(sprite.name != MainTranslation))
				{
					return;
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
			}
			this.mTarget_uGUI_RawImage.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		public void RegisterEvents_UnityStandard()
		{
			this.EventFindTargetHolder += this.FindTarget_GUIText;
			this.EventFindTargetHolder += this.FindTarget_TextMesh;
			this.EventFindTargetHolder += this.FindTarget_AudioSource;
			this.EventFindTargetHolder += this.FindTarget_GUITexture;
			this.EventFindTargetHolder += this.FindTarget_Child;
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
				if (this.mTarget_GUIText.font != null)
				{
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
				if (this.mTarget_GUITexture.texture)
				{
					this.SetFinalTerms(this.mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
					return;
				}
			}
			this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public unsafe void SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (this.mTarget_AudioSource)
			{
				if (this.mTarget_AudioSource.clip)
				{
					this.SetFinalTerms(this.mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
					return;
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
				if (this.mTarget_GUIText.font != secondaryTranslatedObj)
				{
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
				if (this.mTarget_GUIText.text != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						GUIText guitext = this.mTarget_GUIText;
						TextAlignment alignment;
						if (LocalizationManager.IsRight2Left)
						{
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
				if (this.mTarget_TextMesh.font != secondaryTranslatedObj)
				{
					this.mTarget_TextMesh.font = secondaryTranslatedObj;
					base.GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mOriginalAlignmentStd = this.mTarget_TextMesh.alignment;
			}
			if (!string.IsNullOrEmpty(MainTranslation))
			{
				if (this.mTarget_TextMesh.text != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
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
				this.mTarget_AudioSource.clip = audioClip;
			}
			if (isPlaying)
			{
				if (this.mTarget_AudioSource.clip)
				{
					this.mTarget_AudioSource.Play();
				}
			}
		}

		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_GUITexture.texture;
			if (texture != null)
			{
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
				if (this.mTarget_Child.name == MainTranslation)
				{
					return;
				}
			}
			GameObject gameObject = this.mTarget_Child;
			GameObject gameObject2 = this.FindTranslatedObject<GameObject>(MainTranslation);
			if (gameObject2)
			{
				this.mTarget_Child = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
				Transform transform = this.mTarget_Child.transform;
				Transform transform2;
				if (gameObject)
				{
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
