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

		public string mTerm = string.Empty;

		public string mTermSecondary = string.Empty;

		[NonSerialized]
		public string FinalTerm;

		[NonSerialized]
		public string FinalSecondaryTerm;

		public TermModification PrimaryTermModifier;

		public TermModification SecondaryTermModifier;

		public bool LocalizeOnAwake = true;

		private string LastLocalizedLanguage;

		public LanguageSource Source;

		[NonSerialized]
		public UnityEngine.Object mTarget;

		public DelegateSetFinalTerms EventSetFinalTerms;

		public DelegateDoLocalize EventDoLocalize;

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
				return mTerm;
			}
			set
			{
				mTerm = value;
			}
		}

		public string SecondaryTerm
		{
			get
			{
				return mTermSecondary;
			}
			set
			{
				mTermSecondary = value;
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
					action = Interlocked.CompareExchange(ref this.EventFindTargetHolder, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
				while (true)
				{
					return;
				}
			}
			remove
			{
				Action action = this.EventFindTargetHolder;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref this.EventFindTargetHolder, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
				while (true)
				{
					return;
				}
			}
		}

		private void Awake()
		{
			RegisterTargets();
			this.EventFindTargetHolder();
			if (LocalizeOnAwake)
			{
				OnLocalize();
			}
		}

		private void RegisterTargets()
		{
			if (this.EventFindTargetHolder == null)
			{
				RegisterEvents_NGUI();
				RegisterEvents_DFGUI();
				RegisterEvents_UGUI();
				RegisterEvents_2DToolKit();
				RegisterEvents_TextMeshPro();
				RegisterEvents_UnityStandard();
				RegisterEvents_SVG();
			}
		}

		private void OnEnable()
		{
			OnLocalize();
		}

		public void OnLocalize(bool Force = false)
		{
			if (!Force)
			{
				if (!base.enabled)
				{
					return;
				}
				if (base.gameObject == null)
				{
					return;
				}
				if (!base.gameObject.activeInHierarchy)
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
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
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
			if (!Force)
			{
				if (LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
				{
					while (true)
					{
						switch (7)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (!string.IsNullOrEmpty(FinalTerm))
			{
				if (!string.IsNullOrEmpty(FinalSecondaryTerm))
				{
					goto IL_00ec;
				}
			}
			GetFinalTerms(out FinalTerm, out FinalSecondaryTerm);
			goto IL_00ec;
			IL_00ec:
			if (string.IsNullOrEmpty(FinalTerm) && string.IsNullOrEmpty(FinalSecondaryTerm))
			{
				return;
			}
			CallBackTerm = FinalTerm;
			CallBackSecondaryTerm = FinalSecondaryTerm;
			MainTranslation = LocalizationManager.GetTermTranslation(FinalTerm);
			SecondaryTranslation = LocalizationManager.GetTermTranslation(FinalSecondaryTerm);
			if (string.IsNullOrEmpty(MainTranslation) && string.IsNullOrEmpty(SecondaryTranslation))
			{
				return;
			}
			CurrentLocalizeComponent = this;
			LocalizeCallBack.Execute(this);
			if (!HasTargetCache())
			{
				FindTarget();
			}
			if (!HasTargetCache())
			{
				return;
			}
			if (LocalizationManager.IsRight2Left)
			{
				if (!IgnoreRTL)
				{
					if (AllowMainTermToBeRTL)
					{
						if (!string.IsNullOrEmpty(MainTranslation))
						{
							MainTranslation = LocalizationManager.ApplyRTLfix(MainTranslation, MaxCharactersInRTL);
						}
					}
					if (AllowSecondTermToBeRTL)
					{
						if (!string.IsNullOrEmpty(SecondaryTranslation))
						{
							SecondaryTranslation = LocalizationManager.ApplyRTLfix(SecondaryTranslation);
						}
					}
				}
			}
			switch (PrimaryTermModifier)
			{
			case TermModification.ToUpper:
				MainTranslation = MainTranslation.ToUpper();
				break;
			case TermModification.ToLower:
				MainTranslation = MainTranslation.ToLower();
				break;
			case TermModification.ToUpperFirst:
				MainTranslation = GoogleTranslation.UppercaseFirst(MainTranslation);
				break;
			case TermModification.ToTitle:
				MainTranslation = GoogleTranslation.TitleCase(MainTranslation);
				break;
			}
			switch (SecondaryTermModifier)
			{
			case TermModification.ToUpper:
				SecondaryTranslation = SecondaryTranslation.ToUpper();
				break;
			case TermModification.ToLower:
				SecondaryTranslation = SecondaryTranslation.ToLower();
				break;
			case TermModification.ToUpperFirst:
				SecondaryTranslation = GoogleTranslation.UppercaseFirst(SecondaryTranslation);
				break;
			case TermModification.ToTitle:
				SecondaryTranslation = GoogleTranslation.TitleCase(SecondaryTranslation);
				break;
			}
			EventDoLocalize(MainTranslation, SecondaryTranslation);
			CurrentLocalizeComponent = null;
		}

		public bool FindTarget()
		{
			if (this.EventFindTargetHolder == null)
			{
				RegisterTargets();
			}
			this.EventFindTargetHolder();
			return HasTargetCache();
		}

		public void FindAndCacheTarget<T>(ref T targetCache, DelegateSetFinalTerms setFinalTerms, DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL) where T : Component
		{
			if (mTarget != null)
			{
				targetCache = (mTarget as T);
			}
			else
			{
				mTarget = (targetCache = GetComponent<T>());
			}
			if (!((UnityEngine.Object)targetCache != (UnityEngine.Object)null))
			{
				return;
			}
			while (true)
			{
				EventSetFinalTerms = setFinalTerms;
				EventDoLocalize = doLocalize;
				CanUseSecondaryTerm = UseSecondaryTerm;
				AllowMainTermToBeRTL = MainRTL;
				AllowSecondTermToBeRTL = SecondRTL;
				return;
			}
		}

		private void FindAndCacheTarget(ref GameObject targetCache, DelegateSetFinalTerms setFinalTerms, DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		{
			if (mTarget != targetCache && (bool)targetCache)
			{
				UnityEngine.Object.Destroy(targetCache);
			}
			if (mTarget != null)
			{
				targetCache = (mTarget as GameObject);
			}
			else
			{
				Transform transform = base.transform;
				object obj;
				if (transform.childCount < 1)
				{
					obj = null;
				}
				else
				{
					obj = transform.GetChild(0).gameObject;
				}
				GameObject gameObject = (GameObject)obj;
				targetCache = (GameObject)obj;
				mTarget = gameObject;
			}
			if (targetCache != null)
			{
				EventSetFinalTerms = setFinalTerms;
				EventDoLocalize = doLocalize;
				CanUseSecondaryTerm = UseSecondaryTerm;
				AllowMainTermToBeRTL = MainRTL;
				AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private bool HasTargetCache()
		{
			return EventDoLocalize != null;
		}

		public void GetFinalTerms(out string PrimaryTerm, out string SecondaryTerm)
		{
			if (EventSetFinalTerms != null)
			{
				if ((bool)mTarget || HasTargetCache())
				{
					goto IL_0041;
				}
			}
			FindTarget();
			goto IL_0041;
			IL_00ba:
			if (!string.IsNullOrEmpty(mTerm))
			{
				PrimaryTerm = mTerm;
			}
			if (!string.IsNullOrEmpty(mTermSecondary))
			{
				SecondaryTerm = mTermSecondary;
			}
			return;
			IL_0041:
			PrimaryTerm = string.Empty;
			SecondaryTerm = string.Empty;
			if (mTarget != null)
			{
				if (!string.IsNullOrEmpty(mTerm))
				{
					if (!string.IsNullOrEmpty(mTermSecondary))
					{
						goto IL_00ba;
					}
				}
				if (EventSetFinalTerms != null)
				{
					EventSetFinalTerms(mTerm, mTermSecondary, out PrimaryTerm, out SecondaryTerm);
				}
			}
			goto IL_00ba;
		}

		public string GetMainTargetsText()
		{
			string primaryTerm = null;
			string secondaryTerm = null;
			if (EventSetFinalTerms != null)
			{
				EventSetFinalTerms(null, null, out primaryTerm, out secondaryTerm);
			}
			string result;
			if (string.IsNullOrEmpty(primaryTerm))
			{
				result = mTerm;
			}
			else
			{
				result = primaryTerm;
			}
			return result;
		}

		private void SetFinalTerms(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm, bool RemoveNonASCII)
		{
			string text;
			if (RemoveNonASCII)
			{
				if (!string.IsNullOrEmpty(Main))
				{
					text = Regex.Replace(Main, "[^a-zA-Z0-9_ ]+", " ");
					goto IL_003f;
				}
			}
			text = Main;
			goto IL_003f;
			IL_003f:
			PrimaryTerm = text;
			SecondaryTerm = Secondary;
		}

		public void SetTerm(string primary, string secondary = null)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				string text2 = FinalTerm = (Term = primary);
			}
			if (!string.IsNullOrEmpty(secondary))
			{
				string text2 = FinalSecondaryTerm = (SecondaryTerm = secondary);
			}
			OnLocalize(true);
		}

		private T GetSecondaryTranslatedObj<T>(ref string MainTranslation, ref string SecondaryTranslation) where T : UnityEngine.Object
		{
			string secondary = null;
			DeserializeTranslation(MainTranslation, out MainTranslation, out secondary);
			T @object = GetObject<T>(secondary);
			if ((UnityEngine.Object)@object == (UnityEngine.Object)null)
			{
				@object = GetObject<T>(SecondaryTranslation);
			}
			return @object;
		}

		private T GetObject<T>(string Translation) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return (T)null;
			}
			T translatedObject = GetTranslatedObject<T>(Translation);
			if ((UnityEngine.Object)translatedObject == (UnityEngine.Object)null)
			{
				int num = Translation.LastIndexOfAny("/\\".ToCharArray());
				if (num >= 0)
				{
					Translation = Translation.Substring(num + 1);
					translatedObject = GetTranslatedObject<T>(Translation);
				}
			}
			return translatedObject;
		}

		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return FindTranslatedObject<T>(Translation);
		}

		private void DeserializeTranslation(string translation, out string value, out string secondary)
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
				return (T)null;
			}
			if (TranslatedObjects != null)
			{
				int i = 0;
				for (int num = TranslatedObjects.Length; i < num; i++)
				{
					if ((UnityEngine.Object)(TranslatedObjects[i] as T) != (UnityEngine.Object)null)
					{
						if (value == TranslatedObjects[i].name)
						{
							return TranslatedObjects[i] as T;
						}
					}
				}
			}
			T val = LocalizationManager.FindAsset(value) as T;
			if ((bool)(UnityEngine.Object)val)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return val;
					}
				}
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		public bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			if (Array.IndexOf(TranslatedObjects, Obj) >= 0)
			{
				return true;
			}
			return ResourceManager.pInstance.HasAsset(Obj);
		}

		public void AddTranslatedObject(UnityEngine.Object Obj)
		{
			Array.Resize(ref TranslatedObjects, TranslatedObjects.Length + 1);
			TranslatedObjects[TranslatedObjects.Length - 1] = Obj;
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
			EventFindTarget += FindTarget_TMPLabel;
			EventFindTarget += FindTarget_TMPUGUILabel;
		}

		private void FindTarget_TMPLabel()
		{
			FindAndCacheTarget(ref mTarget_TMPLabel, SetFinalTerms_TMPLabel, DoLocalize_TMPLabel, true, true, false);
		}

		private void FindTarget_TMPUGUILabel()
		{
			FindAndCacheTarget(ref mTarget_TMPUGUILabel, SetFinalTerms_TMPUGUILabel, DoLocalize_TMPUGUILabel, true, true, false);
		}

		private void SetFinalTerms_TMPLabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(mTarget_TMPLabel.font != null)) ? string.Empty : mTarget_TMPLabel.font.name;
			SetFinalTerms(mTarget_TMPLabel.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		private void SetFinalTerms_TMPUGUILabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string text;
			if (mTarget_TMPUGUILabel.font != null)
			{
				text = mTarget_TMPUGUILabel.font.name;
			}
			else
			{
				text = string.Empty;
			}
			string secondary = text;
			SetFinalTerms(mTarget_TMPUGUILabel.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void DoLocalize_TMPLabel(string MainTranslation, string SecondaryTranslation)
		{
			if (!Application.isPlaying)
			{
			}
			TMP_FontAsset secondaryTranslatedObj = GetSecondaryTranslatedObj<TMP_FontAsset>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (mTarget_TMPLabel.font != secondaryTranslatedObj)
				{
					mTarget_TMPLabel.font = secondaryTranslatedObj;
				}
			}
			else
			{
				Material secondaryTranslatedObj2 = GetSecondaryTranslatedObj<Material>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj2 != null && mTarget_TMPLabel.fontMaterial != secondaryTranslatedObj2)
				{
					if (!secondaryTranslatedObj2.name.StartsWith(mTarget_TMPLabel.font.name))
					{
						secondaryTranslatedObj = GetTMPFontFromMaterial(secondaryTranslatedObj2.name);
						if (secondaryTranslatedObj != null)
						{
							mTarget_TMPLabel.font = secondaryTranslatedObj;
						}
					}
					mTarget_TMPLabel.fontSharedMaterial = secondaryTranslatedObj2;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mOriginalAlignmentTMPro = mTarget_TMPLabel.alignment;
			}
			if (string.IsNullOrEmpty(MainTranslation))
			{
				return;
			}
			while (true)
			{
				if (!(mTarget_TMPLabel.text != MainTranslation))
				{
					return;
				}
				while (true)
				{
					if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						int alignment = (int)mTarget_TMPLabel.alignment;
						if (alignment % 4 == 0)
						{
							mTarget_TMPLabel.alignment = ((!LocalizationManager.IsRight2Left) ? mOriginalAlignmentTMPro : (mTarget_TMPLabel.alignment + 2));
						}
						else if (alignment % 4 == 2)
						{
							TextMeshPro textMeshPro = mTarget_TMPLabel;
							int alignment2;
							if (LocalizationManager.IsRight2Left)
							{
								alignment2 = (int)(mTarget_TMPLabel.alignment - 2);
							}
							else
							{
								alignment2 = (int)mOriginalAlignmentTMPro;
							}
							textMeshPro.alignment = (TextAlignmentOptions)alignment2;
						}
					}
					mTarget_TMPLabel.text = MainTranslation;
					return;
				}
			}
		}

		public void DoLocalize_TMPUGUILabel(string MainTranslation, string SecondaryTranslation)
		{
			TMP_FontAsset secondaryTranslatedObj = GetSecondaryTranslatedObj<TMP_FontAsset>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (mTarget_TMPUGUILabel.font != secondaryTranslatedObj)
				{
					mTarget_TMPUGUILabel.font = secondaryTranslatedObj;
				}
			}
			else
			{
				Material secondaryTranslatedObj2 = GetSecondaryTranslatedObj<Material>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj2 != null)
				{
					if (mTarget_TMPUGUILabel.fontMaterial != secondaryTranslatedObj2)
					{
						if (!secondaryTranslatedObj2.name.StartsWith(mTarget_TMPUGUILabel.font.name))
						{
							secondaryTranslatedObj = GetTMPFontFromMaterial(secondaryTranslatedObj2.name);
							if (secondaryTranslatedObj != null)
							{
								mTarget_TMPUGUILabel.font = secondaryTranslatedObj;
							}
						}
						mTarget_TMPUGUILabel.fontSharedMaterial = secondaryTranslatedObj2;
					}
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mOriginalAlignmentTMPro = mTarget_TMPUGUILabel.alignment;
			}
			if (string.IsNullOrEmpty(MainTranslation))
			{
				return;
			}
			while (true)
			{
				if (!(mTarget_TMPUGUILabel.text != MainTranslation))
				{
					return;
				}
				while (true)
				{
					if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						int alignment = (int)mTarget_TMPUGUILabel.alignment;
						if (alignment % 4 == 0)
						{
							TextMeshProUGUI textMeshProUGUI = mTarget_TMPUGUILabel;
							int alignment2;
							if (LocalizationManager.IsRight2Left)
							{
								alignment2 = (int)(mTarget_TMPUGUILabel.alignment + 2);
							}
							else
							{
								alignment2 = (int)mOriginalAlignmentTMPro;
							}
							textMeshProUGUI.alignment = (TextAlignmentOptions)alignment2;
						}
						else if (alignment % 4 == 2)
						{
							mTarget_TMPUGUILabel.alignment = ((!LocalizationManager.IsRight2Left) ? mOriginalAlignmentTMPro : (mTarget_TMPUGUILabel.alignment - 2));
						}
					}
					mTarget_TMPUGUILabel.text = MainTranslation;
					return;
				}
			}
		}

		private TMP_FontAsset GetTMPFontFromMaterial(string matName)
		{
			int num = matName.IndexOf(" SDF");
			if (num > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						string translation = matName.Substring(0, num + " SDF".Length);
						return GetObject<TMP_FontAsset>(translation);
					}
					}
				}
			}
			return null;
		}

		public void RegisterEvents_UGUI()
		{
			EventFindTarget += FindTarget_uGUI_Text;
			EventFindTarget += FindTarget_uGUI_Image;
			EventFindTarget += FindTarget_uGUI_RawImage;
		}

		private void FindTarget_uGUI_Text()
		{
			FindAndCacheTarget(ref mTarget_uGUI_Text, SetFinalTerms_uGUI_Text, DoLocalize_uGUI_Text, true, true, false);
		}

		private void FindTarget_uGUI_Image()
		{
			FindAndCacheTarget(ref mTarget_uGUI_Image, SetFinalTerms_uGUI_Image, DoLocalize_uGUI_Image, false, false, false);
		}

		private void FindTarget_uGUI_RawImage()
		{
			FindAndCacheTarget(ref mTarget_uGUI_RawImage, SetFinalTerms_uGUI_RawImage, DoLocalize_uGUI_RawImage, false, false, false);
		}

		private void SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string text;
			if (mTarget_uGUI_Text.font != null)
			{
				text = mTarget_uGUI_Text.font.name;
			}
			else
			{
				text = string.Empty;
			}
			string secondary = text;
			SetFinalTerms(mTarget_uGUI_Text.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			SetFinalTerms(mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public void SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			SetFinalTerms(mTarget_uGUI_RawImage.texture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public static T FindInParents<T>(Transform tr) where T : Component
		{
			if (!tr)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return (T)null;
					}
				}
			}
			T component = tr.GetComponent<T>();
			while (!(UnityEngine.Object)component)
			{
				if ((bool)tr)
				{
					component = tr.GetComponent<T>();
					tr = tr.parent;
					continue;
				}
				break;
			}
			return component;
		}

		public void DoLocalize_uGUI_Text(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (secondaryTranslatedObj != mTarget_uGUI_Text.font)
				{
					mTarget_uGUI_Text.font = secondaryTranslatedObj;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mOriginalAlignmentUGUI = mTarget_uGUI_Text.alignment;
			}
			if (string.IsNullOrEmpty(MainTranslation) || !(mTarget_uGUI_Text.text != MainTranslation))
			{
				return;
			}
			while (true)
			{
				if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
				{
					if (mTarget_uGUI_Text.alignment != 0)
					{
						if (mTarget_uGUI_Text.alignment != TextAnchor.UpperCenter)
						{
							if (mTarget_uGUI_Text.alignment != TextAnchor.UpperRight)
							{
								if (mTarget_uGUI_Text.alignment != TextAnchor.MiddleLeft)
								{
									if (mTarget_uGUI_Text.alignment != TextAnchor.MiddleCenter)
									{
										if (mTarget_uGUI_Text.alignment != TextAnchor.MiddleRight)
										{
											if (mTarget_uGUI_Text.alignment != TextAnchor.LowerLeft)
											{
												if (mTarget_uGUI_Text.alignment != TextAnchor.LowerCenter)
												{
													if (mTarget_uGUI_Text.alignment != TextAnchor.LowerRight)
													{
														goto IL_0219;
													}
												}
											}
											Text text = mTarget_uGUI_Text;
											int alignment;
											if (LocalizationManager.IsRight2Left)
											{
												alignment = 8;
											}
											else
											{
												alignment = (int)mOriginalAlignmentUGUI;
											}
											text.alignment = (TextAnchor)alignment;
											goto IL_0219;
										}
									}
								}
								Text text2 = mTarget_uGUI_Text;
								int alignment2;
								if (LocalizationManager.IsRight2Left)
								{
									alignment2 = 5;
								}
								else
								{
									alignment2 = (int)mOriginalAlignmentUGUI;
								}
								text2.alignment = (TextAnchor)alignment2;
								goto IL_0219;
							}
						}
					}
					Text text3 = mTarget_uGUI_Text;
					int alignment3;
					if (LocalizationManager.IsRight2Left)
					{
						alignment3 = 2;
					}
					else
					{
						alignment3 = (int)mOriginalAlignmentUGUI;
					}
					text3.alignment = (TextAnchor)alignment3;
				}
				goto IL_0219;
				IL_0219:
				mTarget_uGUI_Text.text = MainTranslation;
				mTarget_uGUI_Text.SetVerticesDirty();
				return;
			}
		}

		public void DoLocalize_uGUI_Image(string MainTranslation, string SecondaryTranslation)
		{
			Sprite sprite = mTarget_uGUI_Image.sprite;
			if (!(sprite == null))
			{
				if (!(sprite.name != MainTranslation))
				{
					return;
				}
			}
			mTarget_uGUI_Image.sprite = FindTranslatedObject<Sprite>(MainTranslation);
		}

		public void DoLocalize_uGUI_RawImage(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = mTarget_uGUI_RawImage.texture;
			if (!(texture == null))
			{
				if (!(texture.name != MainTranslation))
				{
					return;
				}
			}
			mTarget_uGUI_RawImage.texture = FindTranslatedObject<Texture>(MainTranslation);
		}

		public void RegisterEvents_UnityStandard()
		{
			EventFindTarget += FindTarget_GUIText;
			EventFindTarget += FindTarget_TextMesh;
			EventFindTarget += FindTarget_AudioSource;
			EventFindTarget += FindTarget_GUITexture;
			EventFindTarget += FindTarget_Child;
		}

		private void FindTarget_GUIText()
		{
			FindAndCacheTarget(ref mTarget_GUIText, SetFinalTerms_GUIText, DoLocalize_GUIText, true, true, false);
		}

		private void FindTarget_TextMesh()
		{
			FindAndCacheTarget(ref mTarget_TextMesh, SetFinalTerms_TextMesh, DoLocalize_TextMesh, true, true, false);
		}

		private void FindTarget_AudioSource()
		{
			FindAndCacheTarget(ref mTarget_AudioSource, SetFinalTerms_AudioSource, DoLocalize_AudioSource, false, false, false);
		}

		private void FindTarget_GUITexture()
		{
			FindAndCacheTarget(ref mTarget_GUITexture, SetFinalTerms_GUITexture, DoLocalize_GUITexture, false, false, false);
		}

		private void FindTarget_Child()
		{
			FindAndCacheTarget(ref mTarget_Child, SetFinalTerms_Child, DoLocalize_Child, false, false, false);
		}

		public void SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (string.IsNullOrEmpty(Secondary))
			{
				if (mTarget_GUIText.font != null)
				{
					Secondary = mTarget_GUIText.font.name;
				}
			}
			SetFinalTerms(mTarget_GUIText.text, Secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public void SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string secondary = (!(mTarget_TextMesh.font != null)) ? string.Empty : mTarget_TextMesh.font.name;
			SetFinalTerms(mTarget_TextMesh.text, secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public void SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if ((bool)mTarget_GUITexture)
			{
				if ((bool)mTarget_GUITexture.texture)
				{
					SetFinalTerms(mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
					return;
				}
			}
			SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public void SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if ((bool)mTarget_AudioSource)
			{
				if ((bool)mTarget_AudioSource.clip)
				{
					SetFinalTerms(mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
					return;
				}
			}
			SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public void SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			SetFinalTerms(mTarget_Child.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		private void DoLocalize_GUIText(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (mTarget_GUIText.font != secondaryTranslatedObj)
				{
					mTarget_GUIText.font = secondaryTranslatedObj;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mOriginalAlignmentStd = mTarget_GUIText.alignment;
			}
			if (string.IsNullOrEmpty(MainTranslation))
			{
				return;
			}
			while (true)
			{
				if (!(mTarget_GUIText.text != MainTranslation))
				{
					return;
				}
				while (true)
				{
					if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
					{
						GUIText gUIText = mTarget_GUIText;
						int alignment;
						if (LocalizationManager.IsRight2Left)
						{
							alignment = 2;
						}
						else
						{
							alignment = (int)mOriginalAlignmentStd;
						}
						gUIText.alignment = (TextAlignment)alignment;
					}
					mTarget_GUIText.text = MainTranslation;
					return;
				}
			}
		}

		private void DoLocalize_TextMesh(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (mTarget_TextMesh.font != secondaryTranslatedObj)
				{
					mTarget_TextMesh.font = secondaryTranslatedObj;
					GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
				}
			}
			if (mInitializeAlignment)
			{
				mInitializeAlignment = false;
				mOriginalAlignmentStd = mTarget_TextMesh.alignment;
			}
			if (string.IsNullOrEmpty(MainTranslation))
			{
				return;
			}
			while (true)
			{
				if (!(mTarget_TextMesh.text != MainTranslation))
				{
					return;
				}
				if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
				{
					mTarget_TextMesh.alignment = ((!LocalizationManager.IsRight2Left) ? mOriginalAlignmentStd : TextAlignment.Right);
				}
				mTarget_TextMesh.text = MainTranslation;
				return;
			}
		}

		private void DoLocalize_AudioSource(string MainTranslation, string SecondaryTranslation)
		{
			bool isPlaying = mTarget_AudioSource.isPlaying;
			AudioClip clip = mTarget_AudioSource.clip;
			AudioClip audioClip = FindTranslatedObject<AudioClip>(MainTranslation);
			if (clip != audioClip)
			{
				mTarget_AudioSource.clip = audioClip;
			}
			if (!isPlaying)
			{
				return;
			}
			while (true)
			{
				if ((bool)mTarget_AudioSource.clip)
				{
					while (true)
					{
						mTarget_AudioSource.Play();
						return;
					}
				}
				return;
			}
		}

		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = mTarget_GUITexture.texture;
			if (!(texture != null))
			{
				return;
			}
			while (true)
			{
				if (texture.name != MainTranslation)
				{
					mTarget_GUITexture.texture = FindTranslatedObject<Texture>(MainTranslation);
				}
				return;
			}
		}

		private void DoLocalize_Child(string MainTranslation, string SecondaryTranslation)
		{
			if ((bool)mTarget_Child)
			{
				if (mTarget_Child.name == MainTranslation)
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
			GameObject gameObject = mTarget_Child;
			GameObject gameObject2 = FindTranslatedObject<GameObject>(MainTranslation);
			if ((bool)gameObject2)
			{
				mTarget_Child = UnityEngine.Object.Instantiate(gameObject2);
				Transform transform = mTarget_Child.transform;
				Transform transform2;
				if ((bool)gameObject)
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
			if ((bool)gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}
}
