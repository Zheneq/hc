using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TextMeshPro - Input Field", 0xB)]
	public class TMP_InputField : Selectable, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, IScrollHandler, IEventSystemHandler
	{
		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[]
		{
			' ',
			'.',
			',',
			'\t',
			'\r',
			'\n'
		};

		[SerializeField]
		protected RectTransform m_TextViewport;

		[SerializeField]
		protected TMP_Text m_TextComponent;

		protected RectTransform m_TextComponentRectTransform;

		[SerializeField]
		protected Graphic m_Placeholder;

		[SerializeField]
		protected Scrollbar m_VerticalScrollbar;

		[SerializeField]
		protected TMP_ScrollbarEventHandler m_VerticalScrollbarEventHandler;

		private float m_ScrollPosition;

		[SerializeField]
		protected float m_ScrollSensitivity = 1f;

		[SerializeField]
		private TMP_InputField.ContentType m_ContentType;

		[SerializeField]
		private TMP_InputField.InputType m_InputType;

		[SerializeField]
		private char m_AsteriskChar = '*';

		[SerializeField]
		private TouchScreenKeyboardType m_KeyboardType;

		[SerializeField]
		private TMP_InputField.LineType m_LineType;

		[SerializeField]
		private bool m_HideMobileInput;

		[SerializeField]
		private TMP_InputField.CharacterValidation m_CharacterValidation;

		[SerializeField]
		private string m_RegexValue = string.Empty;

		[SerializeField]
		private float m_GlobalPointSize = 14f;

		[SerializeField]
		private int m_CharacterLimit;

		[SerializeField]
		private TMP_InputField.SubmitEvent m_OnEndEdit = new TMP_InputField.SubmitEvent();

		[SerializeField]
		private TMP_InputField.SubmitEvent m_OnSubmit = new TMP_InputField.SubmitEvent();

		[SerializeField]
		private TMP_InputField.SelectionEvent m_OnSelect = new TMP_InputField.SelectionEvent();

		[SerializeField]
		private TMP_InputField.SelectionEvent m_OnDeselect = new TMP_InputField.SelectionEvent();

		[SerializeField]
		private TMP_InputField.TextSelectionEvent m_OnTextSelection = new TMP_InputField.TextSelectionEvent();

		[SerializeField]
		private TMP_InputField.TextSelectionEvent m_OnEndTextSelection = new TMP_InputField.TextSelectionEvent();

		[SerializeField]
		private TMP_InputField.OnChangeEvent m_OnValueChanged = new TMP_InputField.OnChangeEvent();

		[SerializeField]
		private TMP_InputField.OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(0.196078435f, 0.196078435f, 0.196078435f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor;

		[SerializeField]
		private Color m_SelectionColor = new Color(0.65882355f, 0.807843149f, 1f, 0.7529412f);

		[SerializeField]
		protected string m_Text = string.Empty;

		[Range(0f, 4f)]
		[SerializeField]
		private float m_CaretBlinkRate = 0.85f;

		[SerializeField]
		[Range(1f, 5f)]
		private int m_CaretWidth = 1;

		[SerializeField]
		private bool m_ReadOnly;

		[SerializeField]
		private bool m_RichText = true;

		protected int m_StringPosition;

		protected int m_StringSelectPosition;

		protected int m_CaretPosition;

		protected int m_CaretSelectPosition;

		private RectTransform caretRectTrans;

		protected UIVertex[] m_CursorVerts;

		private CanvasRenderer m_CachedInputRenderer;

		private Vector2 m_DefaultTransformPosition;

		private Vector2 m_LastPosition;

		[NonSerialized]
		protected Mesh m_Mesh;

		private bool m_AllowInput;

		private bool m_ShouldActivateNextUpdate;

		private bool m_UpdateDrag;

		private bool m_DragPositionOutOfBounds;

		private const float kHScrollSpeed = 0.05f;

		private const float kVScrollSpeed = 0.1f;

		protected bool m_CaretVisible;

		private Coroutine m_BlinkCoroutine;

		private float m_BlinkStartTime;

		private Coroutine m_DragCoroutine;

		private string m_OriginalText = string.Empty;

		private bool m_WasCanceled;

		private bool m_HasDoneFocusTransition;

		private bool m_IsScrollbarUpdateRequired;

		private bool m_IsUpdatingScrollbarValues;

		private bool m_isLastKeyBackspace;

		private float m_ClickStartTime;

		private float m_DoubleClickDelay = 0.5f;

		private const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

		[SerializeField]
		protected TMP_FontAsset m_GlobalFontAsset;

		[SerializeField]
		protected bool m_OnFocusSelectAll = true;

		protected bool m_isSelectAll;

		[SerializeField]
		protected bool m_ResetOnDeActivation = true;

		[SerializeField]
		private bool m_RestoreOriginalTextOnEscape = true;

		[SerializeField]
		protected bool m_isRichTextEditingAllowed = true;

		[SerializeField]
		protected TMP_InputValidator m_InputValidator;

		private bool m_isSelected;

		private bool isStringPositionDirty;

		private bool m_forceRectTransformAdjustment;

		private Event m_ProcessingEvent = new Event();

		protected TMP_InputField()
		{
		}

		protected Mesh mesh
		{
			get
			{
				if (this.m_Mesh == null)
				{
					this.m_Mesh = new Mesh();
				}
				return this.m_Mesh;
			}
		}

		public bool shouldHideMobileInput
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				switch (platform)
				{
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
					break;
				default:
					if (platform != RuntimePlatform.TizenPlayer)
					{
						if (platform != RuntimePlatform.tvOS)
						{
							return true;
						}
					}
					break;
				}
				return this.m_HideMobileInput;
			}
			set
			{
				SetPropertyUtility.SetStruct<bool>(ref this.m_HideMobileInput, value);
			}
		}

		public string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				if (this.text == value)
				{
					return;
				}
				this.m_Text = value;
				if (this.m_Keyboard != null)
				{
					this.m_Keyboard.text = this.m_Text;
				}
				if (this.m_StringPosition > this.m_Text.Length)
				{
					this.m_StringPosition = (this.m_StringSelectPosition = this.m_Text.Length);
				}
				this.AdjustTextPositionRelativeToViewport(0f);
				this.m_forceRectTransformAdjustment = true;
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		public bool isFocused
		{
			get
			{
				return this.m_AllowInput;
			}
		}

		public float caretBlinkRate
		{
			get
			{
				return this.m_CaretBlinkRate;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_CaretBlinkRate, value) && this.m_AllowInput)
				{
					this.SetCaretActive();
				}
			}
		}

		public int caretWidth
		{
			get
			{
				return this.m_CaretWidth;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_CaretWidth, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		public RectTransform textViewport
		{
			get
			{
				return this.m_TextViewport;
			}
			set
			{
				SetPropertyUtility.SetClass<RectTransform>(ref this.m_TextViewport, value);
			}
		}

		public TMP_Text textComponent
		{
			get
			{
				return this.m_TextComponent;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_Text>(ref this.m_TextComponent, value);
			}
		}

		public Graphic placeholder
		{
			get
			{
				return this.m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass<Graphic>(ref this.m_Placeholder, value);
			}
		}

		public Scrollbar verticalScrollbar
		{
			get
			{
				return this.m_VerticalScrollbar;
			}
			set
			{
				if (this.m_VerticalScrollbar != null)
				{
					this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.OnScrollbarValueChange));
				}
				SetPropertyUtility.SetClass<Scrollbar>(ref this.m_VerticalScrollbar, value);
				if (this.m_VerticalScrollbar)
				{
					this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnScrollbarValueChange));
				}
			}
		}

		public float scrollSensitivity
		{
			get
			{
				return this.m_ScrollSensitivity;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_ScrollSensitivity, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		public Color caretColor
		{
			get
			{
				Color result;
				if (this.customCaretColor)
				{
					result = this.m_CaretColor;
				}
				else
				{
					result = this.textComponent.color;
				}
				return result;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_CaretColor, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		public bool customCaretColor
		{
			get
			{
				return this.m_CustomCaretColor;
			}
			set
			{
				if (this.m_CustomCaretColor != value)
				{
					this.m_CustomCaretColor = value;
					this.MarkGeometryAsDirty();
				}
			}
		}

		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor;
			}
			set
			{
				if (SetPropertyUtility.SetColor(ref this.m_SelectionColor, value))
				{
					this.MarkGeometryAsDirty();
				}
			}
		}

		public TMP_InputField.SubmitEvent onEndEdit
		{
			get
			{
				return this.m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.SubmitEvent>(ref this.m_OnEndEdit, value);
			}
		}

		public TMP_InputField.SubmitEvent onSubmit
		{
			get
			{
				return this.m_OnSubmit;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.SubmitEvent>(ref this.m_OnSubmit, value);
			}
		}

		public TMP_InputField.SelectionEvent onSelect
		{
			get
			{
				return this.m_OnSelect;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.SelectionEvent>(ref this.m_OnSelect, value);
			}
		}

		public TMP_InputField.SelectionEvent onDeselect
		{
			get
			{
				return this.m_OnDeselect;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.SelectionEvent>(ref this.m_OnDeselect, value);
			}
		}

		public TMP_InputField.TextSelectionEvent onTextSelection
		{
			get
			{
				return this.m_OnTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.TextSelectionEvent>(ref this.m_OnTextSelection, value);
			}
		}

		public TMP_InputField.TextSelectionEvent onEndTextSelection
		{
			get
			{
				return this.m_OnEndTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.TextSelectionEvent>(ref this.m_OnEndTextSelection, value);
			}
		}

		public TMP_InputField.OnChangeEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.OnChangeEvent>(ref this.m_OnValueChanged, value);
			}
		}

		public TMP_InputField.OnValidateInput onValidateInput
		{
			get
			{
				return this.m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass<TMP_InputField.OnValidateInput>(ref this.m_OnValidateInput, value);
			}
		}

		public int characterLimit
		{
			get
			{
				return this.m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<int>(ref this.m_CharacterLimit, Math.Max(0, value)))
				{
					this.UpdateLabel();
				}
			}
		}

		public float pointSize
		{
			get
			{
				return this.m_GlobalPointSize;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<float>(ref this.m_GlobalPointSize, Math.Max(0f, value)))
				{
					this.SetGlobalPointSize(this.m_GlobalPointSize);
					this.UpdateLabel();
				}
			}
		}

		public TMP_FontAsset fontAsset
		{
			get
			{
				return this.m_GlobalFontAsset;
			}
			set
			{
				if (SetPropertyUtility.SetClass<TMP_FontAsset>(ref this.m_GlobalFontAsset, value))
				{
					this.SetGlobalFontAsset(this.m_GlobalFontAsset);
					this.UpdateLabel();
				}
			}
		}

		public bool onFocusSelectAll
		{
			get
			{
				return this.m_OnFocusSelectAll;
			}
			set
			{
				this.m_OnFocusSelectAll = value;
			}
		}

		public bool resetOnDeActivation
		{
			get
			{
				return this.m_ResetOnDeActivation;
			}
			set
			{
				this.m_ResetOnDeActivation = value;
			}
		}

		public bool restoreOriginalTextOnEscape
		{
			get
			{
				return this.m_RestoreOriginalTextOnEscape;
			}
			set
			{
				this.m_RestoreOriginalTextOnEscape = value;
			}
		}

		public bool isRichTextEditingAllowed
		{
			get
			{
				return this.m_isRichTextEditingAllowed;
			}
			set
			{
				this.m_isRichTextEditingAllowed = value;
			}
		}

		public TMP_InputField.ContentType contentType
		{
			get
			{
				return this.m_ContentType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TMP_InputField.ContentType>(ref this.m_ContentType, value))
				{
					this.EnforceContentType();
				}
			}
		}

		public TMP_InputField.LineType lineType
		{
			get
			{
				return this.m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TMP_InputField.LineType>(ref this.m_LineType, value))
				{
					this.SetTextComponentWrapMode();
				}
				this.SetToCustomIfContentTypeIsNot(new TMP_InputField.ContentType[]
				{
					TMP_InputField.ContentType.Standard,
					TMP_InputField.ContentType.Autocorrected
				});
			}
		}

		public TMP_InputField.InputType inputType
		{
			get
			{
				return this.m_InputType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TMP_InputField.InputType>(ref this.m_InputType, value))
				{
					this.SetToCustom();
				}
			}
		}

		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return this.m_KeyboardType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TouchScreenKeyboardType>(ref this.m_KeyboardType, value))
				{
					this.SetToCustom();
				}
			}
		}

		public TMP_InputField.CharacterValidation characterValidation
		{
			get
			{
				return this.m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<TMP_InputField.CharacterValidation>(ref this.m_CharacterValidation, value))
				{
					this.SetToCustom();
				}
			}
		}

		public TMP_InputValidator inputValidator
		{
			get
			{
				return this.m_InputValidator;
			}
			set
			{
				if (SetPropertyUtility.SetClass<TMP_InputValidator>(ref this.m_InputValidator, value))
				{
					this.SetToCustom(TMP_InputField.CharacterValidation.CustomValidator);
				}
			}
		}

		public bool readOnly
		{
			get
			{
				return this.m_ReadOnly;
			}
			set
			{
				this.m_ReadOnly = value;
			}
		}

		public bool richText
		{
			get
			{
				return this.m_RichText;
			}
			set
			{
				this.m_RichText = value;
				this.SetTextComponentRichTextMode();
			}
		}

		public bool multiLine
		{
			get
			{
				bool result;
				if (this.m_LineType != TMP_InputField.LineType.MultiLineNewline)
				{
					result = (this.lineType == TMP_InputField.LineType.MultiLineSubmit);
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		public char asteriskChar
		{
			get
			{
				return this.m_AsteriskChar;
			}
			set
			{
				if (SetPropertyUtility.SetStruct<char>(ref this.m_AsteriskChar, value))
				{
					this.UpdateLabel();
				}
			}
		}

		public bool wasCanceled
		{
			get
			{
				return this.m_WasCanceled;
			}
		}

		protected unsafe void ClampStringPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > this.text.Length)
			{
				pos = this.text.Length;
			}
		}

		protected unsafe void ClampCaretPos(ref int pos)
		{
			if (pos < 0)
			{
				pos = 0;
			}
			else if (pos > this.m_TextComponent.textInfo.characterCount - 1)
			{
				pos = this.m_TextComponent.textInfo.characterCount - 1;
			}
		}

		protected int caretPositionInternal
		{
			get
			{
				return this.m_CaretPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_CaretPosition = value;
				this.ClampCaretPos(ref this.m_CaretPosition);
			}
		}

		protected int stringPositionInternal
		{
			get
			{
				return this.m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_StringPosition = value;
				this.ClampStringPos(ref this.m_StringPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return this.m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_CaretSelectPosition = value;
				this.ClampCaretPos(ref this.m_CaretSelectPosition);
			}
		}

		protected int stringSelectPositionInternal
		{
			get
			{
				return this.m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				this.m_StringSelectPosition = value;
				this.ClampStringPos(ref this.m_StringSelectPosition);
			}
		}

		private bool hasSelection
		{
			get
			{
				return this.stringPositionInternal != this.stringSelectPositionInternal;
			}
		}

		public int caretPosition
		{
			get
			{
				return this.m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				this.selectionAnchorPosition = value;
				this.selectionFocusPosition = value;
				this.isStringPositionDirty = true;
			}
		}

		public int selectionAnchorPosition
		{
			get
			{
				return this.m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.caretPositionInternal = value;
				this.stringPositionInternal = value;
				this.isStringPositionDirty = true;
			}
		}

		public int selectionFocusPosition
		{
			get
			{
				return this.m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.caretSelectPositionInternal = value;
				this.stringSelectPositionInternal = value;
				this.isStringPositionDirty = true;
			}
		}

		public int stringPosition
		{
			get
			{
				return this.stringSelectPositionInternal;
			}
			set
			{
				this.selectionStringAnchorPosition = value;
				this.selectionStringFocusPosition = value;
			}
		}

		public int selectionStringAnchorPosition
		{
			get
			{
				return this.stringPositionInternal;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.stringPositionInternal = value;
			}
		}

		public int selectionStringFocusPosition
		{
			get
			{
				return this.stringSelectPositionInternal;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					return;
				}
				this.stringSelectPositionInternal = value;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.m_Text == null)
			{
				this.m_Text = string.Empty;
			}
			if (Application.isPlaying)
			{
				if (this.m_CachedInputRenderer == null)
				{
					if (this.m_TextComponent != null)
					{
						GameObject gameObject = new GameObject(base.transform.name + " Input Caret", new Type[]
						{
							typeof(RectTransform)
						});
						TMP_SelectionCaret tmp_SelectionCaret = gameObject.AddComponent<TMP_SelectionCaret>();
						tmp_SelectionCaret.color = Color.clear;
						gameObject.hideFlags = HideFlags.DontSave;
						gameObject.transform.SetParent(this.m_TextComponent.transform.parent);
						gameObject.transform.SetAsFirstSibling();
						gameObject.layer = base.gameObject.layer;
						this.caretRectTrans = gameObject.GetComponent<RectTransform>();
						this.m_CachedInputRenderer = gameObject.GetComponent<CanvasRenderer>();
						this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
						gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
						this.AssignPositioningIfNeeded();
					}
				}
			}
			if (this.m_CachedInputRenderer != null)
			{
				this.m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
			}
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
				this.m_TextComponent.RegisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
				this.m_TextComponent.ignoreRectMaskCulling = true;
				this.m_DefaultTransformPosition = this.m_TextComponent.rectTransform.localPosition;
				if (this.m_VerticalScrollbar != null)
				{
					this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.OnScrollbarValueChange));
				}
				this.UpdateLabel();
			}
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		protected override void OnDisable()
		{
			this.m_BlinkCoroutine = null;
			this.DeactivateInputField();
			if (this.m_TextComponent != null)
			{
				this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.MarkGeometryAsDirty));
				this.m_TextComponent.UnregisterDirtyVerticesCallback(new UnityAction(this.UpdateLabel));
				if (this.m_VerticalScrollbar != null)
				{
					this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.OnScrollbarValueChange));
				}
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_CachedInputRenderer != null)
			{
				this.m_CachedInputRenderer.Clear();
			}
			if (this.m_Mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Mesh);
			}
			this.m_Mesh = null;
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
			base.OnDisable();
		}

		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == this.m_TextComponent)
			{
				if (Application.isPlaying)
				{
					this.caretPositionInternal = this.GetCaretPositionFromStringIndex(this.stringPositionInternal);
					this.caretSelectPositionInternal = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
				}
			}
		}

		private IEnumerator CaretBlink()
		{
			this.m_CaretVisible = true;
			yield return null;
			while (this.m_CaretBlinkRate > 0f)
			{
				float blinkPeriod = 1f / this.m_CaretBlinkRate;
				bool blinkState = (Time.unscaledTime - this.m_BlinkStartTime) % blinkPeriod < blinkPeriod / 2f;
				if (this.m_CaretVisible != blinkState)
				{
					this.m_CaretVisible = blinkState;
					if (!this.hasSelection)
					{
						this.MarkGeometryAsDirty();
					}
				}
				yield return null;
			}
			this.m_BlinkCoroutine = null;
			yield break;
		}

		private void SetCaretVisible()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			this.m_CaretVisible = true;
			this.m_BlinkStartTime = Time.unscaledTime;
			this.SetCaretActive();
		}

		private void SetCaretActive()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			if (this.m_CaretBlinkRate > 0f)
			{
				if (this.m_BlinkCoroutine == null)
				{
					this.m_BlinkCoroutine = base.StartCoroutine(this.CaretBlink());
				}
			}
			else
			{
				this.m_CaretVisible = true;
			}
		}

		protected void OnFocus()
		{
			if (this.m_OnFocusSelectAll)
			{
				this.SelectAll();
			}
		}

		protected void SelectAll()
		{
			this.m_isSelectAll = true;
			this.stringPositionInternal = this.text.Length;
			this.stringSelectPositionInternal = 0;
		}

		public void MoveTextEnd(bool shift)
		{
			if (this.m_isRichTextEditingAllowed)
			{
				int length = this.text.Length;
				if (shift)
				{
					this.stringSelectPositionInternal = length;
				}
				else
				{
					this.stringPositionInternal = length;
					this.stringSelectPositionInternal = this.stringPositionInternal;
				}
			}
			else
			{
				int num = this.m_TextComponent.textInfo.characterCount - 1;
				if (shift)
				{
					this.caretSelectPositionInternal = num;
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(num);
				}
				else
				{
					int num2 = num;
					this.caretSelectPositionInternal = num2;
					this.caretPositionInternal = num2;
					num2 = this.GetStringIndexFromCaretPosition(num);
					this.stringPositionInternal = num2;
					this.stringSelectPositionInternal = num2;
				}
			}
			this.UpdateLabel();
		}

		public void MoveTextStart(bool shift)
		{
			if (this.m_isRichTextEditingAllowed)
			{
				int num = 0;
				if (shift)
				{
					this.stringSelectPositionInternal = num;
				}
				else
				{
					this.stringPositionInternal = num;
					this.stringSelectPositionInternal = this.stringPositionInternal;
				}
			}
			else
			{
				int num2 = 0;
				if (shift)
				{
					this.caretSelectPositionInternal = num2;
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(num2);
				}
				else
				{
					int num3 = num2;
					this.caretSelectPositionInternal = num3;
					this.caretPositionInternal = num3;
					num3 = this.GetStringIndexFromCaretPosition(num2);
					this.stringPositionInternal = num3;
					this.stringSelectPositionInternal = num3;
				}
			}
			this.UpdateLabel();
		}

		public void MoveToEndOfLine(bool shift, bool ctrl)
		{
			int lineNumber = (int)this.m_TextComponent.textInfo.characterInfo[this.caretPositionInternal].lineNumber;
			int num;
			if (ctrl)
			{
				num = this.m_TextComponent.textInfo.characterCount - 1;
			}
			else
			{
				num = this.m_TextComponent.textInfo.lineInfo[lineNumber].lastCharacterIndex;
			}
			int num2 = num;
			num2 = this.GetStringIndexFromCaretPosition(num2);
			if (shift)
			{
				this.stringSelectPositionInternal = num2;
			}
			else
			{
				this.stringPositionInternal = num2;
				this.stringSelectPositionInternal = this.stringPositionInternal;
			}
			this.UpdateLabel();
		}

		public void MoveToStartOfLine(bool shift, bool ctrl)
		{
			int lineNumber = (int)this.m_TextComponent.textInfo.characterInfo[this.caretPositionInternal].lineNumber;
			int num;
			if (ctrl)
			{
				num = 0;
			}
			else
			{
				num = this.m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex;
			}
			int num2 = num;
			num2 = this.GetStringIndexFromCaretPosition(num2);
			if (shift)
			{
				this.stringSelectPositionInternal = num2;
			}
			else
			{
				this.stringPositionInternal = num2;
				this.stringSelectPositionInternal = this.stringPositionInternal;
			}
			this.UpdateLabel();
		}

		private static string clipboard
		{
			get
			{
				return GUIUtility.systemCopyBuffer;
			}
			set
			{
				GUIUtility.systemCopyBuffer = value;
			}
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.isSupported;
		}

		protected virtual void LateUpdate()
		{
			if (this.m_ShouldActivateNextUpdate)
			{
				if (!this.isFocused)
				{
					this.ActivateInputFieldInternal();
					this.m_ShouldActivateNextUpdate = false;
					return;
				}
				this.m_ShouldActivateNextUpdate = false;
			}
			if (this.m_IsScrollbarUpdateRequired)
			{
				this.UpdateScrollbar();
				this.m_IsScrollbarUpdateRequired = false;
			}
			if (!this.InPlaceEditing())
			{
				if (this.isFocused)
				{
					this.AssignPositioningIfNeeded();
					if (this.m_Keyboard != null)
					{
						if (this.m_Keyboard.active)
						{
							string text = this.m_Keyboard.text;
							if (this.m_Text != text)
							{
								if (this.m_ReadOnly)
								{
									this.m_Keyboard.text = this.m_Text;
								}
								else
								{
									this.m_Text = string.Empty;
									foreach (char c in text)
									{
										if (c == '\r' || c == '\u0003')
										{
											c = '\n';
										}
										if (this.onValidateInput != null)
										{
											c = this.onValidateInput(this.m_Text, this.m_Text.Length, c);
										}
										else if (this.characterValidation != TMP_InputField.CharacterValidation.None)
										{
											c = this.Validate(this.m_Text, this.m_Text.Length, c);
										}
										if (this.lineType == TMP_InputField.LineType.MultiLineSubmit)
										{
											if (c == '\n')
											{
												this.m_Keyboard.text = this.m_Text;
												this.OnSubmit(null);
												this.OnDeselect(null);
												return;
											}
										}
										if (c != '\0')
										{
											this.m_Text += c;
										}
									}
									if (this.characterLimit > 0)
									{
										if (this.m_Text.Length > this.characterLimit)
										{
											this.m_Text = this.m_Text.Substring(0, this.characterLimit);
										}
									}
									int length = this.m_Text.Length;
									this.stringSelectPositionInternal = length;
									this.stringPositionInternal = length;
									if (this.m_Text != text)
									{
										this.m_Keyboard.text = this.m_Text;
									}
									this.SendOnValueChangedAndUpdateLabel();
								}
							}
							if (this.m_Keyboard.done)
							{
								if (this.m_Keyboard.wasCanceled)
								{
									this.m_WasCanceled = true;
								}
								this.OnDeselect(null);
							}
							return;
						}
					}
					if (this.m_Keyboard != null)
					{
						if (!this.m_ReadOnly)
						{
							this.text = this.m_Keyboard.text;
						}
						if (this.m_Keyboard.wasCanceled)
						{
							this.m_WasCanceled = true;
						}
						if (this.m_Keyboard.done)
						{
							this.OnSubmit(null);
						}
					}
					this.OnDeselect(null);
					return;
				}
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			if (this.IsActive())
			{
				if (this.IsInteractable())
				{
					if (eventData.button == PointerEventData.InputButton.Left)
					{
						if (this.m_TextComponent != null)
						{
							return this.m_Keyboard == null;
						}
					}
				}
			}
			return false;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.m_UpdateDrag = true;
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			CaretPosition caretPosition;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(this.m_TextComponent, eventData.position, eventData.pressEventCamera, out caretPosition);
			if (caretPosition == CaretPosition.Left)
			{
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition);
			}
			else if (caretPosition == CaretPosition.Right)
			{
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
			}
			this.caretSelectPositionInternal = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
			this.MarkGeometryAsDirty();
			this.m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(this.textViewport, eventData.position, eventData.pressEventCamera);
			if (this.m_DragPositionOutOfBounds && this.m_DragCoroutine == null)
			{
				this.m_DragCoroutine = base.StartCoroutine(this.MouseDragOutsideRect(eventData));
			}
			eventData.Use();
		}

		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			while (this.m_UpdateDrag)
			{
				if (!this.m_DragPositionOutOfBounds)
				{
					break;
				}
				Vector2 localMousePos;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(this.textViewport, eventData.position, eventData.pressEventCamera, out localMousePos);
				Rect rect = this.textViewport.rect;
				if (this.multiLine)
				{
					if (localMousePos.y > rect.yMax)
					{
						this.MoveUp(true, true);
					}
					else if (localMousePos.y < rect.yMin)
					{
						this.MoveDown(true, true);
					}
				}
				else if (localMousePos.x < rect.xMin)
				{
					this.MoveLeft(true, false);
				}
				else if (localMousePos.x > rect.xMax)
				{
					this.MoveRight(true, false);
				}
				this.UpdateLabel();
				float num;
				if (this.multiLine)
				{
					num = 0.1f;
				}
				else
				{
					num = 0.05f;
				}
				float delay = num;
				yield return new WaitForSeconds(delay);
			}
			this.m_DragCoroutine = null;
			yield break;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.m_UpdateDrag = false;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			bool allowInput = this.m_AllowInput;
			base.OnPointerDown(eventData);
			if (!this.InPlaceEditing() && (this.m_Keyboard == null || !this.m_Keyboard.active))
			{
				this.OnSelect(eventData);
				return;
			}
			bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			bool flag2 = false;
			float unscaledTime = Time.unscaledTime;
			if (this.m_ClickStartTime + this.m_DoubleClickDelay > unscaledTime)
			{
				flag2 = true;
			}
			this.m_ClickStartTime = unscaledTime;
			if (!allowInput)
			{
				if (this.m_OnFocusSelectAll)
				{
					goto IL_2C3;
				}
			}
			CaretPosition caretPosition;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(this.m_TextComponent, eventData.position, eventData.pressEventCamera, out caretPosition);
			if (flag)
			{
				if (caretPosition == CaretPosition.Left)
				{
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition);
				}
				else if (caretPosition == CaretPosition.Right)
				{
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
				}
			}
			else if (caretPosition == CaretPosition.Left)
			{
				int num = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition);
				this.stringSelectPositionInternal = num;
				this.stringPositionInternal = num;
			}
			else if (caretPosition == CaretPosition.Right)
			{
				int num = this.GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
				this.stringSelectPositionInternal = num;
				this.stringPositionInternal = num;
			}
			if (flag2)
			{
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, eventData.position, eventData.pressEventCamera);
				if (num2 != -1)
				{
					this.caretPositionInternal = this.m_TextComponent.textInfo.wordInfo[num2].firstCharacterIndex;
					this.caretSelectPositionInternal = this.m_TextComponent.textInfo.wordInfo[num2].lastCharacterIndex + 1;
					this.stringPositionInternal = this.GetStringIndexFromCaretPosition(this.caretPositionInternal);
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
				}
				else
				{
					this.caretPositionInternal = this.GetCaretPositionFromStringIndex(this.stringPositionInternal);
					this.stringSelectPositionInternal++;
					this.caretSelectPositionInternal = this.caretPositionInternal + 1;
					this.caretSelectPositionInternal = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
				}
			}
			else
			{
				int num = this.GetCaretPositionFromStringIndex(this.stringPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
			IL_2C3:
			this.UpdateLabel();
			eventData.Use();
		}

		protected TMP_InputField.EditState KeyPressed(Event evt)
		{
			EventModifiers modifiers = evt.modifiers;
			RuntimePlatform platform = Application.platform;
			bool flag;
			if (platform != RuntimePlatform.OSXEditor)
			{
				flag = (platform == RuntimePlatform.OSXPlayer);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			bool flag3 = (!flag2) ? ((modifiers & EventModifiers.Control) != EventModifiers.None) : ((modifiers & EventModifiers.Command) != EventModifiers.None);
			bool flag4 = (modifiers & EventModifiers.Shift) != EventModifiers.None;
			bool flag5 = (modifiers & EventModifiers.Alt) != EventModifiers.None;
			bool flag6;
			if (flag3)
			{
				if (!flag5)
				{
					flag6 = !flag4;
					goto IL_80;
				}
			}
			flag6 = false;
			IL_80:
			bool flag7 = flag6;
			KeyCode keyCode = evt.keyCode;
			switch (keyCode)
			{
			case KeyCode.KeypadEnter:
				break;
			default:
				switch (keyCode)
				{
				case KeyCode.A:
					if (flag7)
					{
						this.SelectAll();
						return TMP_InputField.EditState.Continue;
					}
					goto IL_26D;
				default:
					switch (keyCode)
					{
					case KeyCode.V:
						if (flag7)
						{
							this.Append(TMP_InputField.clipboard);
							return TMP_InputField.EditState.Continue;
						}
						goto IL_26D;
					default:
						if (keyCode == KeyCode.Backspace)
						{
							this.Backspace();
							return TMP_InputField.EditState.Continue;
						}
						if (keyCode != KeyCode.Return)
						{
							if (keyCode == KeyCode.Escape)
							{
								this.m_WasCanceled = true;
								return TMP_InputField.EditState.Finish;
							}
							if (keyCode != KeyCode.Delete)
							{
								goto IL_26D;
							}
							this.ForwardSpace();
							return TMP_InputField.EditState.Continue;
						}
						break;
					case KeyCode.X:
						if (flag7)
						{
							if (this.inputType != TMP_InputField.InputType.Password)
							{
								TMP_InputField.clipboard = this.GetSelectedString();
							}
							else
							{
								TMP_InputField.clipboard = string.Empty;
							}
							this.Delete();
							this.SendOnValueChangedAndUpdateLabel();
							return TMP_InputField.EditState.Continue;
						}
						goto IL_26D;
					}
					break;
				case KeyCode.C:
					if (flag7)
					{
						if (this.inputType != TMP_InputField.InputType.Password)
						{
							TMP_InputField.clipboard = this.GetSelectedString();
						}
						else
						{
							TMP_InputField.clipboard = string.Empty;
						}
						return TMP_InputField.EditState.Continue;
					}
					goto IL_26D;
				}
				break;
			case KeyCode.UpArrow:
				this.MoveUp(flag4);
				return TMP_InputField.EditState.Continue;
			case KeyCode.DownArrow:
				this.MoveDown(flag4);
				return TMP_InputField.EditState.Continue;
			case KeyCode.RightArrow:
				this.MoveRight(flag4, flag3);
				return TMP_InputField.EditState.Continue;
			case KeyCode.LeftArrow:
				this.MoveLeft(flag4, flag3);
				return TMP_InputField.EditState.Continue;
			case KeyCode.Home:
				this.MoveToStartOfLine(flag4, flag3);
				return TMP_InputField.EditState.Continue;
			case KeyCode.End:
				this.MoveToEndOfLine(flag4, flag3);
				return TMP_InputField.EditState.Continue;
			case KeyCode.PageUp:
				this.MovePageUp(flag4);
				return TMP_InputField.EditState.Continue;
			case KeyCode.PageDown:
				this.MovePageDown(flag4);
				return TMP_InputField.EditState.Continue;
			}
			if (this.lineType != TMP_InputField.LineType.MultiLineNewline)
			{
				return TMP_InputField.EditState.Finish;
			}
			IL_26D:
			char c = evt.character;
			if (!this.multiLine)
			{
				if (c != '\t')
				{
					if (c != '\r')
					{
						if (c != '\n')
						{
							goto IL_2BB;
						}
					}
				}
				return TMP_InputField.EditState.Continue;
			}
			IL_2BB:
			if (c != '\r')
			{
				if (c != '\u0003')
				{
					goto IL_2DE;
				}
			}
			c = '\n';
			IL_2DE:
			if (this.IsValidChar(c))
			{
				this.Append(c);
			}
			if (c == '\0')
			{
				if (Input.compositionString.Length > 0)
				{
					this.UpdateLabel();
				}
			}
			return TMP_InputField.EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			if (c == '\u007f')
			{
				return false;
			}
			if (c != '\t')
			{
				if (c != '\n')
				{
					return this.m_TextComponent.font.HasCharacter(c, true);
				}
			}
			return true;
		}

		public void ProcessEvent(Event e)
		{
			this.KeyPressed(e);
		}

		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (!this.isFocused)
			{
				return;
			}
			bool flag = false;
			while (Event.PopEvent(this.m_ProcessingEvent))
			{
				if (this.m_ProcessingEvent.rawType == EventType.KeyDown)
				{
					flag = true;
					TMP_InputField.EditState editState = this.KeyPressed(this.m_ProcessingEvent);
					if (editState == TMP_InputField.EditState.Finish)
					{
						this.SendOnSubmit();
						this.DeactivateInputField();
						break;
					}
				}
				EventType type = this.m_ProcessingEvent.type;
				if (type != EventType.ValidateCommand)
				{
					if (type != EventType.ExecuteCommand)
					{
						continue;
					}
				}
				string commandName = this.m_ProcessingEvent.commandName;
				if (commandName != null)
				{
					if (!(commandName == "SelectAll"))
					{
					}
					else
					{
						this.SelectAll();
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.UpdateLabel();
			}
			eventData.Use();
		}

		public virtual void OnScroll(PointerEventData eventData)
		{
			if (this.m_TextComponent.preferredHeight < this.m_TextViewport.rect.height)
			{
				return;
			}
			float num = -eventData.scrollDelta.y;
			this.m_ScrollPosition += 1f / (float)this.m_TextComponent.textInfo.lineCount * num * this.m_ScrollSensitivity;
			this.m_ScrollPosition = Mathf.Clamp01(this.m_ScrollPosition);
			this.AdjustTextPositionRelativeToViewport(this.m_ScrollPosition);
			this.m_AllowInput = false;
			if (this.m_VerticalScrollbar)
			{
				this.m_IsUpdatingScrollbarValues = true;
				this.m_VerticalScrollbar.value = this.m_ScrollPosition;
			}
		}

		private string GetSelectedString()
		{
			if (!this.hasSelection)
			{
				return string.Empty;
			}
			int num = this.stringPositionInternal;
			int num2 = this.stringSelectPositionInternal;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return this.text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (this.stringSelectPositionInternal + 1 >= this.text.Length)
			{
				return this.text.Length;
			}
			int num = this.text.IndexOfAny(TMP_InputField.kSeparators, this.stringSelectPositionInternal + 1);
			if (num == -1)
			{
				num = this.text.Length;
			}
			else
			{
				num++;
			}
			return num;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Max(this.stringPositionInternal, this.stringSelectPositionInternal);
				this.stringSelectPositionInternal = num;
				this.stringPositionInternal = num;
				num = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
				return;
			}
			int num2;
			if (ctrl)
			{
				num2 = this.FindtNextWordBegin();
			}
			else if (this.m_isRichTextEditingAllowed)
			{
				num2 = this.stringSelectPositionInternal + 1;
			}
			else
			{
				num2 = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal + 1);
			}
			if (shift)
			{
				this.stringSelectPositionInternal = num2;
				this.caretSelectPositionInternal = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
			}
			else
			{
				int num = num2;
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
				num = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
		}

		private int FindtPrevWordBegin()
		{
			if (this.stringSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = this.text.LastIndexOfAny(TMP_InputField.kSeparators, this.stringSelectPositionInternal - 2);
			if (num == -1)
			{
				num = 0;
			}
			else
			{
				num++;
			}
			return num;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			if (this.hasSelection)
			{
				if (!shift)
				{
					int num = Mathf.Min(this.stringPositionInternal, this.stringSelectPositionInternal);
					this.stringSelectPositionInternal = num;
					this.stringPositionInternal = num;
					num = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
					this.caretSelectPositionInternal = num;
					this.caretPositionInternal = num;
					return;
				}
			}
			int num2;
			if (ctrl)
			{
				num2 = this.FindtPrevWordBegin();
			}
			else if (this.m_isRichTextEditingAllowed)
			{
				num2 = this.stringSelectPositionInternal - 1;
			}
			else
			{
				num2 = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal - 1);
			}
			if (shift)
			{
				this.stringSelectPositionInternal = num2;
				this.caretSelectPositionInternal = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
			}
			else
			{
				int num = num2;
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
				num = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal);
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
			}
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= this.m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = (int)tmp_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				return (!goToFirstChar) ? originalPos : 0;
			}
			int num = this.m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = this.m_TextComponent.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex; i < num; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo2 = this.m_TextComponent.textInfo.characterInfo[i];
				float num5 = tmp_CharacterInfo.origin - tmp_CharacterInfo2.origin;
				float num6 = num5 / (tmp_CharacterInfo2.xAdvance - tmp_CharacterInfo2.origin);
				if (num6 >= 0f)
				{
					if (num6 <= 1f)
					{
						if (num6 < 0.5f)
						{
							return i;
						}
						return i + 1;
					}
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num3)
				{
					num2 = i;
					num3 = num5;
					num4 = num6;
				}
			}
			if (num2 == -1)
			{
				return num;
			}
			if (num4 < 0.5f)
			{
				return num2;
			}
			return num2 + 1;
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= this.m_TextComponent.textInfo.characterCount)
			{
				return this.m_TextComponent.textInfo.characterCount - 1;
			}
			TMP_CharacterInfo tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = (int)tmp_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= this.m_TextComponent.textInfo.lineCount)
			{
				int result;
				if (goToLastChar)
				{
					result = this.m_TextComponent.textInfo.characterCount - 1;
				}
				else
				{
					result = originalPos;
				}
				return result;
			}
			int lastCharacterIndex = this.m_TextComponent.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
			int num = -1;
			float num2 = 32767f;
			float num3 = 0f;
			int i = this.m_TextComponent.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex;
			while (i < lastCharacterIndex)
			{
				TMP_CharacterInfo tmp_CharacterInfo2 = this.m_TextComponent.textInfo.characterInfo[i];
				float num4 = tmp_CharacterInfo.origin - tmp_CharacterInfo2.origin;
				float num5 = num4 / (tmp_CharacterInfo2.xAdvance - tmp_CharacterInfo2.origin);
				if (num5 >= 0f && num5 <= 1f)
				{
					if (num5 < 0.5f)
					{
						return i;
					}
					return i + 1;
				}
				else
				{
					num4 = Mathf.Abs(num4);
					if (num4 < num2)
					{
						num = i;
						num2 = num4;
						num3 = num5;
					}
					i++;
				}
			}
			if (num == -1)
			{
				return lastCharacterIndex;
			}
			if (num3 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		private int PageUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= this.m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = (int)tmp_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				return (!goToFirstChar) ? originalPos : 0;
			}
			float height = this.m_TextViewport.rect.height;
			int i = lineNumber - 1;
			while (i > 0)
			{
				if (this.m_TextComponent.textInfo.lineInfo[i].baseline > this.m_TextComponent.textInfo.lineInfo[lineNumber].baseline + height)
				{
					IL_DC:
					int lastCharacterIndex = this.m_TextComponent.textInfo.lineInfo[i].lastCharacterIndex;
					int num = -1;
					float num2 = 32767f;
					float num3 = 0f;
					for (int j = this.m_TextComponent.textInfo.lineInfo[i].firstCharacterIndex; j < lastCharacterIndex; j++)
					{
						TMP_CharacterInfo tmp_CharacterInfo2 = this.m_TextComponent.textInfo.characterInfo[j];
						float num4 = tmp_CharacterInfo.origin - tmp_CharacterInfo2.origin;
						float num5 = num4 / (tmp_CharacterInfo2.xAdvance - tmp_CharacterInfo2.origin);
						if (num5 >= 0f)
						{
							if (num5 <= 1f)
							{
								if (num5 < 0.5f)
								{
									return j;
								}
								return j + 1;
							}
						}
						num4 = Mathf.Abs(num4);
						if (num4 < num2)
						{
							num = j;
							num2 = num4;
							num3 = num5;
						}
					}
					if (num == -1)
					{
						return lastCharacterIndex;
					}
					if (num3 < 0.5f)
					{
						return num;
					}
					return num + 1;
				}
				else
				{
					i--;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				goto IL_DC;
			}
		}

		private int PageDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= this.m_TextComponent.textInfo.characterCount)
			{
				return this.m_TextComponent.textInfo.characterCount - 1;
			}
			TMP_CharacterInfo tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = (int)tmp_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= this.m_TextComponent.textInfo.lineCount)
			{
				return (!goToLastChar) ? originalPos : (this.m_TextComponent.textInfo.characterCount - 1);
			}
			float height = this.m_TextViewport.rect.height;
			int i = lineNumber + 1;
			while (i < this.m_TextComponent.textInfo.lineCount - 1)
			{
				if (this.m_TextComponent.textInfo.lineInfo[i].baseline < this.m_TextComponent.textInfo.lineInfo[lineNumber].baseline - height)
				{
					IL_121:
					int lastCharacterIndex = this.m_TextComponent.textInfo.lineInfo[i].lastCharacterIndex;
					int num = -1;
					float num2 = 32767f;
					float num3 = 0f;
					for (int j = this.m_TextComponent.textInfo.lineInfo[i].firstCharacterIndex; j < lastCharacterIndex; j++)
					{
						TMP_CharacterInfo tmp_CharacterInfo2 = this.m_TextComponent.textInfo.characterInfo[j];
						float num4 = tmp_CharacterInfo.origin - tmp_CharacterInfo2.origin;
						float num5 = num4 / (tmp_CharacterInfo2.xAdvance - tmp_CharacterInfo2.origin);
						if (num5 >= 0f)
						{
							if (num5 <= 1f)
							{
								if (num5 < 0.5f)
								{
									return j;
								}
								return j + 1;
							}
						}
						num4 = Mathf.Abs(num4);
						if (num4 < num2)
						{
							num = j;
							num2 = num4;
							num3 = num5;
						}
					}
					if (num == -1)
					{
						return lastCharacterIndex;
					}
					if (num3 < 0.5f)
					{
						return num;
					}
					return num + 1;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				goto IL_121;
			}
		}

		private void MoveDown(bool shift)
		{
			this.MoveDown(shift, true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (this.hasSelection)
			{
				if (!shift)
				{
					int num = Mathf.Max(this.caretPositionInternal, this.caretSelectPositionInternal);
					this.caretSelectPositionInternal = num;
					this.caretPositionInternal = num;
				}
			}
			int num2;
			if (this.multiLine)
			{
				num2 = this.LineDownCharacterPosition(this.caretSelectPositionInternal, goToLastChar);
			}
			else
			{
				num2 = this.m_TextComponent.textInfo.characterCount - 1;
			}
			int num3 = num2;
			if (shift)
			{
				this.caretSelectPositionInternal = num3;
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
			}
			else
			{
				int num = num3;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
				num = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
			}
		}

		private void MoveUp(bool shift)
		{
			this.MoveUp(shift, true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Min(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
			int num2 = (!this.multiLine) ? 0 : this.LineUpCharacterPosition(this.caretSelectPositionInternal, goToFirstChar);
			if (shift)
			{
				this.caretSelectPositionInternal = num2;
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
			}
			else
			{
				int num = num2;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
				num = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
			}
		}

		private void MovePageUp(bool shift)
		{
			this.MovePageUp(shift, true);
		}

		private void MovePageUp(bool shift, bool goToFirstChar)
		{
			if (this.hasSelection && !shift)
			{
				int num = Mathf.Min(this.caretPositionInternal, this.caretSelectPositionInternal);
				this.caretSelectPositionInternal = num;
				this.caretPositionInternal = num;
			}
			int num2;
			if (this.multiLine)
			{
				num2 = this.PageUpCharacterPosition(this.caretSelectPositionInternal, goToFirstChar);
			}
			else
			{
				num2 = 0;
			}
			int num3 = num2;
			if (shift)
			{
				this.caretSelectPositionInternal = num3;
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
			}
			else
			{
				int num = num3;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
				num = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
			}
			if (this.m_LineType != TMP_InputField.LineType.SingleLine)
			{
				float num4 = this.m_TextViewport.rect.height;
				float num5 = this.m_TextComponent.rectTransform.position.y + this.m_TextComponent.textBounds.max.y;
				float num6 = this.m_TextViewport.position.y + this.m_TextViewport.rect.yMax;
				num4 = ((num6 <= num5 + num4) ? (num6 - num5) : num4);
				this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num4);
				this.AssignPositioningIfNeeded();
				this.m_IsScrollbarUpdateRequired = true;
			}
		}

		private void MovePageDown(bool shift)
		{
			this.MovePageDown(shift, true);
		}

		private void MovePageDown(bool shift, bool goToLastChar)
		{
			if (this.hasSelection)
			{
				if (!shift)
				{
					int num = Mathf.Max(this.caretPositionInternal, this.caretSelectPositionInternal);
					this.caretSelectPositionInternal = num;
					this.caretPositionInternal = num;
				}
			}
			int num2;
			if (this.multiLine)
			{
				num2 = this.PageDownCharacterPosition(this.caretSelectPositionInternal, goToLastChar);
			}
			else
			{
				num2 = this.m_TextComponent.textInfo.characterCount - 1;
			}
			int num3 = num2;
			if (shift)
			{
				this.caretSelectPositionInternal = num3;
				this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
			}
			else
			{
				int num = num3;
				this.caretPositionInternal = num;
				this.caretSelectPositionInternal = num;
				num = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
				this.stringPositionInternal = num;
				this.stringSelectPositionInternal = num;
			}
			if (this.m_LineType != TMP_InputField.LineType.SingleLine)
			{
				float num4 = this.m_TextViewport.rect.height;
				float num5 = this.m_TextComponent.rectTransform.position.y + this.m_TextComponent.textBounds.min.y;
				float num6 = this.m_TextViewport.position.y + this.m_TextViewport.rect.yMin;
				float num7;
				if (num6 > num5 + num4)
				{
					num7 = num4;
				}
				else
				{
					num7 = num6 - num5;
				}
				num4 = num7;
				this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num4);
				this.AssignPositioningIfNeeded();
				this.m_IsScrollbarUpdateRequired = true;
			}
		}

		private void Delete()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.stringPositionInternal == this.stringSelectPositionInternal)
			{
				return;
			}
			if (!this.m_isRichTextEditingAllowed)
			{
				if (this.m_isSelectAll)
				{
				}
				else
				{
					this.stringPositionInternal = this.GetStringIndexFromCaretPosition(this.caretPositionInternal);
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.caretSelectPositionInternal);
					if (this.caretPositionInternal < this.caretSelectPositionInternal)
					{
						this.m_Text = this.text.Substring(0, this.stringPositionInternal) + this.text.Substring(this.stringSelectPositionInternal, this.text.Length - this.stringSelectPositionInternal);
						this.stringSelectPositionInternal = this.stringPositionInternal;
						this.caretSelectPositionInternal = this.caretPositionInternal;
						return;
					}
					this.m_Text = this.text.Substring(0, this.stringSelectPositionInternal) + this.text.Substring(this.stringPositionInternal, this.text.Length - this.stringPositionInternal);
					this.stringPositionInternal = this.stringSelectPositionInternal;
					this.stringPositionInternal = this.stringSelectPositionInternal;
					this.caretPositionInternal = this.caretSelectPositionInternal;
					return;
				}
			}
			if (this.stringPositionInternal < this.stringSelectPositionInternal)
			{
				this.m_Text = this.text.Substring(0, this.stringPositionInternal) + this.text.Substring(this.stringSelectPositionInternal, this.text.Length - this.stringSelectPositionInternal);
				this.stringSelectPositionInternal = this.stringPositionInternal;
			}
			else
			{
				this.m_Text = this.text.Substring(0, this.stringSelectPositionInternal) + this.text.Substring(this.stringPositionInternal, this.text.Length - this.stringPositionInternal);
				this.stringPositionInternal = this.stringSelectPositionInternal;
			}
			this.m_isSelectAll = false;
		}

		private void ForwardSpace()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.hasSelection)
			{
				this.Delete();
				this.SendOnValueChangedAndUpdateLabel();
			}
			else if (this.m_isRichTextEditingAllowed)
			{
				if (this.stringPositionInternal < this.text.Length)
				{
					this.m_Text = this.text.Remove(this.stringPositionInternal, 1);
					this.SendOnValueChangedAndUpdateLabel();
				}
			}
			else if (this.caretPositionInternal < this.m_TextComponent.textInfo.characterCount - 1)
			{
				int stringIndexFromCaretPosition = this.GetStringIndexFromCaretPosition(this.caretPositionInternal);
				this.stringPositionInternal = stringIndexFromCaretPosition;
				this.stringSelectPositionInternal = stringIndexFromCaretPosition;
				this.m_Text = this.text.Remove(this.stringPositionInternal, 1);
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		private void Backspace()
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (this.hasSelection)
			{
				this.Delete();
				this.SendOnValueChangedAndUpdateLabel();
			}
			else if (this.m_isRichTextEditingAllowed)
			{
				if (this.stringPositionInternal > 0)
				{
					this.m_Text = this.text.Remove(this.stringPositionInternal - 1, 1);
					int num = this.stringPositionInternal - 1;
					this.stringPositionInternal = num;
					this.stringSelectPositionInternal = num;
					this.m_isLastKeyBackspace = true;
					this.SendOnValueChangedAndUpdateLabel();
				}
			}
			else
			{
				if (this.caretPositionInternal > 0)
				{
					this.m_Text = this.text.Remove(this.GetStringIndexFromCaretPosition(this.caretPositionInternal - 1), 1);
					int num = this.caretPositionInternal - 1;
					this.caretPositionInternal = num;
					this.caretSelectPositionInternal = num;
					num = this.GetStringIndexFromCaretPosition(this.caretPositionInternal);
					this.stringPositionInternal = num;
					this.stringSelectPositionInternal = num;
				}
				this.m_isLastKeyBackspace = true;
				this.SendOnValueChangedAndUpdateLabel();
			}
		}

		protected virtual void Append(string input)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (!this.InPlaceEditing())
			{
				return;
			}
			int i = 0;
			int length = input.Length;
			while (i < length)
			{
				char c = input[i];
				if (c >= ' ')
				{
					goto IL_89;
				}
				if (c == '\t')
				{
					goto IL_89;
				}
				if (c == '\r' || c == '\n')
				{
					goto IL_89;
				}
				if (c == '\n')
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_89;
					}
				}
				IL_90:
				i++;
				continue;
				IL_89:
				this.Append(c);
				goto IL_90;
			}
		}

		protected virtual void Append(char input)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			if (!this.InPlaceEditing())
			{
				return;
			}
			if (this.onValidateInput != null)
			{
				input = this.onValidateInput(this.text, this.stringPositionInternal, input);
			}
			else if (this.characterValidation == TMP_InputField.CharacterValidation.CustomValidator)
			{
				input = this.Validate(this.text, this.stringPositionInternal, input);
				if (input == '\0')
				{
					return;
				}
				this.SendOnValueChanged();
				this.UpdateLabel();
				return;
			}
			else if (this.characterValidation != TMP_InputField.CharacterValidation.None)
			{
				input = this.Validate(this.text, this.stringPositionInternal, input);
			}
			if (input == '\0')
			{
				return;
			}
			this.Insert(input);
		}

		private void Insert(char c)
		{
			if (this.m_ReadOnly)
			{
				return;
			}
			string text = c.ToString();
			this.Delete();
			if (this.characterLimit > 0 && this.text.Length >= this.characterLimit)
			{
				return;
			}
			this.m_Text = this.text.Insert(this.m_StringPosition, text);
			this.stringSelectPositionInternal = (this.stringPositionInternal += text.Length);
			this.SendOnValueChanged();
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			this.SendOnValueChanged();
			this.UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (this.onValueChanged != null)
			{
				this.onValueChanged.Invoke(this.text);
			}
		}

		protected void SendOnEndEdit()
		{
			if (this.onEndEdit != null)
			{
				this.onEndEdit.Invoke(this.m_Text);
			}
		}

		protected void SendOnSubmit()
		{
			if (this.onSubmit != null)
			{
				this.onSubmit.Invoke(this.m_Text);
			}
		}

		protected void SendOnFocus()
		{
			if (this.onSelect != null)
			{
				this.onSelect.Invoke(this.m_Text);
			}
		}

		protected void SendOnFocusLost()
		{
			if (this.onDeselect != null)
			{
				this.onDeselect.Invoke(this.m_Text);
			}
		}

		protected void SendOnTextSelection()
		{
			this.m_isSelected = true;
			if (this.onTextSelection != null)
			{
				this.onTextSelection.Invoke(this.m_Text, this.stringPositionInternal, this.stringSelectPositionInternal);
			}
		}

		protected void SendOnEndTextSelection()
		{
			if (!this.m_isSelected)
			{
				return;
			}
			if (this.onEndTextSelection != null)
			{
				this.onEndTextSelection.Invoke(this.m_Text, this.stringPositionInternal, this.stringSelectPositionInternal);
			}
			this.m_isSelected = false;
		}

		protected void UpdateLabel()
		{
			if (this.m_TextComponent != null && this.m_TextComponent.font != null)
			{
				string text;
				if (Input.compositionString.Length > 0)
				{
					text = this.text.Substring(0, this.m_StringPosition) + Input.compositionString + this.text.Substring(this.m_StringPosition);
				}
				else
				{
					text = this.text;
				}
				string str;
				if (this.inputType == TMP_InputField.InputType.Password)
				{
					str = new string(this.asteriskChar, text.Length);
				}
				else
				{
					str = text;
				}
				bool flag = string.IsNullOrEmpty(text);
				if (this.m_Placeholder != null)
				{
					this.m_Placeholder.enabled = flag;
				}
				if (!flag)
				{
					this.SetCaretVisible();
				}
				this.m_TextComponent.text = str + "​";
				this.MarkGeometryAsDirty();
				this.m_IsScrollbarUpdateRequired = true;
			}
		}

		private void UpdateScrollbar()
		{
			if (this.m_VerticalScrollbar)
			{
				float size = this.m_TextViewport.rect.height / this.m_TextComponent.preferredHeight;
				this.m_IsUpdatingScrollbarValues = true;
				this.m_VerticalScrollbar.size = size;
				this.m_VerticalScrollbar.value = this.m_TextComponent.rectTransform.anchoredPosition.y / (this.m_TextComponent.preferredHeight - this.m_TextViewport.rect.height);
			}
		}

		private void OnScrollbarValueChange(float value)
		{
			if (this.m_IsUpdatingScrollbarValues)
			{
				this.m_IsUpdatingScrollbarValues = false;
				return;
			}
			if (value >= 0f)
			{
				if (value <= 1f)
				{
					this.AdjustTextPositionRelativeToViewport(value);
					this.m_ScrollPosition = value;
					return;
				}
			}
		}

		private void AdjustTextPositionRelativeToViewport(float relativePosition)
		{
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			if (textInfo != null)
			{
				if (textInfo.lineInfo != null)
				{
					if (textInfo.lineCount != 0)
					{
						if (textInfo.lineCount <= textInfo.lineInfo.Length)
						{
							this.m_TextComponent.rectTransform.anchoredPosition = new Vector2(this.m_TextComponent.rectTransform.anchoredPosition.x, (this.m_TextComponent.preferredHeight - this.m_TextViewport.rect.height) * relativePosition);
							this.AssignPositioningIfNeeded();
							return;
						}
					}
				}
			}
		}

		private int GetCaretPositionFromStringIndex(int stringIndex)
		{
			int characterCount = this.m_TextComponent.textInfo.characterCount;
			for (int i = 0; i < characterCount; i++)
			{
				if ((int)this.m_TextComponent.textInfo.characterInfo[i].index >= stringIndex)
				{
					return i;
				}
			}
			return characterCount;
		}

		private int GetStringIndexFromCaretPosition(int caretPosition)
		{
			this.ClampCaretPos(ref caretPosition);
			return (int)this.m_TextComponent.textInfo.characterInfo[caretPosition].index;
		}

		public void ForceLabelUpdate()
		{
			this.UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (update != CanvasUpdate.LatePreRender)
			{
			}
			else
			{
				this.UpdateGeometry();
			}
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (!this.shouldHideMobileInput)
			{
				return;
			}
			if (this.m_CachedInputRenderer == null)
			{
				return;
			}
			this.OnFillVBO(this.mesh);
			this.m_CachedInputRenderer.SetMesh(this.mesh);
		}

		private void AssignPositioningIfNeeded()
		{
			if (this.m_TextComponent != null)
			{
				if (this.caretRectTrans != null)
				{
					if (!(this.caretRectTrans.localPosition != this.m_TextComponent.rectTransform.localPosition))
					{
						if (!(this.caretRectTrans.localRotation != this.m_TextComponent.rectTransform.localRotation))
						{
							if (!(this.caretRectTrans.localScale != this.m_TextComponent.rectTransform.localScale))
							{
								if (!(this.caretRectTrans.anchorMin != this.m_TextComponent.rectTransform.anchorMin))
								{
									if (!(this.caretRectTrans.anchorMax != this.m_TextComponent.rectTransform.anchorMax) && !(this.caretRectTrans.anchoredPosition != this.m_TextComponent.rectTransform.anchoredPosition))
									{
										if (!(this.caretRectTrans.sizeDelta != this.m_TextComponent.rectTransform.sizeDelta))
										{
											if (!(this.caretRectTrans.pivot != this.m_TextComponent.rectTransform.pivot))
											{
												return;
											}
										}
									}
								}
							}
						}
					}
					this.caretRectTrans.localPosition = this.m_TextComponent.rectTransform.localPosition;
					this.caretRectTrans.localRotation = this.m_TextComponent.rectTransform.localRotation;
					this.caretRectTrans.localScale = this.m_TextComponent.rectTransform.localScale;
					this.caretRectTrans.anchorMin = this.m_TextComponent.rectTransform.anchorMin;
					this.caretRectTrans.anchorMax = this.m_TextComponent.rectTransform.anchorMax;
					this.caretRectTrans.anchoredPosition = this.m_TextComponent.rectTransform.anchoredPosition;
					this.caretRectTrans.sizeDelta = this.m_TextComponent.rectTransform.sizeDelta;
					this.caretRectTrans.pivot = this.m_TextComponent.rectTransform.pivot;
				}
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			VertexHelper vertexHelper = new VertexHelper();
			try
			{
				if (!this.isFocused)
				{
					if (this.m_ResetOnDeActivation)
					{
						vertexHelper.FillMesh(vbo);
						return;
					}
				}
				if (this.isStringPositionDirty)
				{
					this.stringPositionInternal = this.GetStringIndexFromCaretPosition(this.m_CaretPosition);
					this.stringSelectPositionInternal = this.GetStringIndexFromCaretPosition(this.m_CaretSelectPosition);
					this.isStringPositionDirty = false;
				}
				if (!this.hasSelection)
				{
					this.GenerateCaret(vertexHelper, Vector2.zero);
					this.SendOnEndTextSelection();
				}
				else
				{
					this.GenerateHightlight(vertexHelper, Vector2.zero);
					this.SendOnTextSelection();
				}
				vertexHelper.FillMesh(vbo);
			}
			finally
			{
				if (vertexHelper != null)
				{
					((IDisposable)vertexHelper).Dispose();
				}
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			if (!this.m_CaretVisible)
			{
				return;
			}
			if (this.m_CursorVerts == null)
			{
				this.CreateCursorVerts();
			}
			float num = (float)this.m_CaretWidth;
			int characterCount = this.m_TextComponent.textInfo.characterCount;
			Vector2 zero = Vector2.zero;
			this.caretPositionInternal = this.GetCaretPositionFromStringIndex(this.stringPositionInternal);
			TMP_CharacterInfo tmp_CharacterInfo;
			float num2;
			if (this.caretPositionInternal == 0)
			{
				tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[0];
				zero = new Vector2(tmp_CharacterInfo.origin, tmp_CharacterInfo.descender);
				num2 = tmp_CharacterInfo.ascender - tmp_CharacterInfo.descender;
			}
			else if (this.caretPositionInternal < characterCount)
			{
				tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[this.caretPositionInternal];
				zero = new Vector2(tmp_CharacterInfo.origin, tmp_CharacterInfo.descender);
				num2 = tmp_CharacterInfo.ascender - tmp_CharacterInfo.descender;
			}
			else
			{
				tmp_CharacterInfo = this.m_TextComponent.textInfo.characterInfo[characterCount - 1];
				zero = new Vector2(tmp_CharacterInfo.xAdvance, tmp_CharacterInfo.descender);
				num2 = tmp_CharacterInfo.ascender - tmp_CharacterInfo.descender;
			}
			if (this.isFocused)
			{
				if (zero != this.m_LastPosition)
				{
					goto IL_1A4;
				}
			}
			if (!this.m_forceRectTransformAdjustment)
			{
				goto IL_1B3;
			}
			IL_1A4:
			this.AdjustRectTransformRelativeToViewport(zero, num2, tmp_CharacterInfo.isVisible);
			IL_1B3:
			this.m_LastPosition = zero;
			float num3 = zero.y + num2;
			float y = num3 - num2;
			this.m_CursorVerts[0].position = new Vector3(zero.x, y, 0f);
			this.m_CursorVerts[1].position = new Vector3(zero.x, num3, 0f);
			this.m_CursorVerts[2].position = new Vector3(zero.x + num, num3, 0f);
			this.m_CursorVerts[3].position = new Vector3(zero.x + num, y, 0f);
			this.m_CursorVerts[0].color = this.caretColor;
			this.m_CursorVerts[1].color = this.caretColor;
			this.m_CursorVerts[2].color = this.caretColor;
			this.m_CursorVerts[3].color = this.caretColor;
			vbo.AddUIVertexQuad(this.m_CursorVerts);
			int height = Screen.height;
			zero.y = (float)height - zero.y;
			Input.compositionCursorPos = zero;
		}

		private void CreateCursorVerts()
		{
			this.m_CursorVerts = new UIVertex[4];
			for (int i = 0; i < this.m_CursorVerts.Length; i++)
			{
				this.m_CursorVerts[i] = UIVertex.simpleVert;
				this.m_CursorVerts[i].uv0 = Vector2.zero;
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
			this.caretPositionInternal = (this.m_CaretPosition = this.GetCaretPositionFromStringIndex(this.stringPositionInternal));
			this.caretSelectPositionInternal = (this.m_CaretSelectPosition = this.GetCaretPositionFromStringIndex(this.stringSelectPositionInternal));
			Vector2 startPosition;
			float height;
			if (this.caretSelectPositionInternal < textInfo.characterCount)
			{
				startPosition = new Vector2(textInfo.characterInfo[this.caretSelectPositionInternal].origin, textInfo.characterInfo[this.caretSelectPositionInternal].descender);
				height = textInfo.characterInfo[this.caretSelectPositionInternal].ascender - textInfo.characterInfo[this.caretSelectPositionInternal].descender;
			}
			else
			{
				startPosition = new Vector2(textInfo.characterInfo[this.caretSelectPositionInternal - 1].xAdvance, textInfo.characterInfo[this.caretSelectPositionInternal - 1].descender);
				height = textInfo.characterInfo[this.caretSelectPositionInternal - 1].ascender - textInfo.characterInfo[this.caretSelectPositionInternal - 1].descender;
			}
			this.AdjustRectTransformRelativeToViewport(startPosition, height, true);
			int num = Mathf.Max(0, this.caretPositionInternal);
			int num2 = Mathf.Max(0, this.caretSelectPositionInternal);
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			num2--;
			int num4 = (int)textInfo.characterInfo[num].lineNumber;
			int lastCharacterIndex = textInfo.lineInfo[num4].lastCharacterIndex;
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.zero;
			simpleVert.color = this.selectionColor;
			int i = num;
			while (i <= num2)
			{
				if (i >= textInfo.characterCount)
				{
					break;
				}
				if (i == lastCharacterIndex)
				{
					goto IL_20C;
				}
				if (i == num2)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_20C;
					}
				}
				IL_3B4:
				i++;
				continue;
				IL_20C:
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[num];
				TMP_CharacterInfo tmp_CharacterInfo2 = textInfo.characterInfo[i];
				if (i > 0)
				{
					if (tmp_CharacterInfo2.character == '\n' && textInfo.characterInfo[i - 1].character == '\r')
					{
						tmp_CharacterInfo2 = textInfo.characterInfo[i - 1];
					}
				}
				Vector2 vector = new Vector2(tmp_CharacterInfo.origin, textInfo.lineInfo[num4].ascender);
				Vector2 vector2 = new Vector2(tmp_CharacterInfo2.xAdvance, textInfo.lineInfo[num4].descender);
				int currentVertCount = vbo.currentVertCount;
				simpleVert.position = new Vector3(vector.x, vector2.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector2.x, vector2.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector2.x, vector.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector.x, vector.y, 0f);
				vbo.AddVert(simpleVert);
				vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
				vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
				num = i + 1;
				num4++;
				if (num4 < textInfo.lineCount)
				{
					lastCharacterIndex = textInfo.lineInfo[num4].lastCharacterIndex;
					goto IL_3B4;
				}
				goto IL_3B4;
			}
			this.m_IsScrollbarUpdateRequired = true;
		}

		private void AdjustRectTransformRelativeToViewport(Vector2 startPosition, float height, bool isCharVisible)
		{
			float xMin = this.m_TextViewport.rect.xMin;
			float xMax = this.m_TextViewport.rect.xMax;
			float num = xMax - (this.m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x + this.m_TextComponent.margin.z + (float)this.m_CaretWidth);
			if (num < 0f)
			{
				if (this.multiLine)
				{
					if (!this.multiLine)
					{
						goto IL_F7;
					}
					if (!isCharVisible)
					{
						goto IL_F7;
					}
				}
				this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(num, 0f);
				this.AssignPositioningIfNeeded();
			}
			IL_F7:
			float num2 = this.m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x - this.m_TextComponent.margin.x - xMin;
			if (num2 < 0f)
			{
				this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(-num2, 0f);
				this.AssignPositioningIfNeeded();
			}
			if (this.m_LineType != TMP_InputField.LineType.SingleLine)
			{
				float num3 = this.m_TextViewport.rect.yMax - (this.m_TextComponent.rectTransform.anchoredPosition.y + startPosition.y + height);
				if (num3 < -0.0001f)
				{
					this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num3);
					this.AssignPositioningIfNeeded();
					this.m_IsScrollbarUpdateRequired = true;
				}
				float num4 = this.m_TextComponent.rectTransform.anchoredPosition.y + startPosition.y - this.m_TextViewport.rect.yMin;
				if (num4 < 0f)
				{
					this.m_TextComponent.rectTransform.anchoredPosition -= new Vector2(0f, num4);
					this.AssignPositioningIfNeeded();
					this.m_IsScrollbarUpdateRequired = true;
				}
			}
			if (this.m_isLastKeyBackspace)
			{
				float num5 = this.m_TextComponent.rectTransform.anchoredPosition.x + this.m_TextComponent.textInfo.characterInfo[0].origin - this.m_TextComponent.margin.x;
				float num6 = this.m_TextComponent.rectTransform.anchoredPosition.x + this.m_TextComponent.textInfo.characterInfo[this.m_TextComponent.textInfo.characterCount - 1].origin + this.m_TextComponent.margin.z;
				if (this.m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x <= xMin + 0.0001f)
				{
					if (num5 < xMin)
					{
						float x = Mathf.Min((xMax - xMin) / 2f, xMin - num5);
						this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(x, 0f);
						this.AssignPositioningIfNeeded();
					}
				}
				else if (num6 < xMax)
				{
					if (num5 < xMin)
					{
						float x2 = Mathf.Min(xMax - num6, xMin - num5);
						this.m_TextComponent.rectTransform.anchoredPosition += new Vector2(x2, 0f);
						this.AssignPositioningIfNeeded();
					}
				}
				this.m_isLastKeyBackspace = false;
			}
			this.m_forceRectTransformAdjustment = false;
		}

		protected char Validate(string text, int pos, char ch)
		{
			if (this.characterValidation != TMP_InputField.CharacterValidation.None)
			{
				if (base.enabled)
				{
					if (this.characterValidation != TMP_InputField.CharacterValidation.Integer)
					{
						if (this.characterValidation == TMP_InputField.CharacterValidation.Decimal)
						{
						}
						else
						{
							if (this.characterValidation == TMP_InputField.CharacterValidation.Digit)
							{
								if (ch >= '0')
								{
									if (ch <= '9')
									{
										return ch;
									}
								}
								return '\0';
							}
							if (this.characterValidation == TMP_InputField.CharacterValidation.Alphanumeric)
							{
								if (ch >= 'A' && ch <= 'Z')
								{
									return ch;
								}
								if (ch >= 'a')
								{
									if (ch <= 'z')
									{
										return ch;
									}
								}
								if (ch >= '0')
								{
									if (ch <= '9')
									{
										return ch;
									}
								}
								return '\0';
							}
							else if (this.characterValidation == TMP_InputField.CharacterValidation.Name)
							{
								char c;
								if (text.Length > 0)
								{
									c = text[Mathf.Clamp(pos, 0, text.Length - 1)];
								}
								else
								{
									c = ' ';
								}
								char c2 = c;
								char c3;
								if (text.Length > 0)
								{
									c3 = text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
								}
								else
								{
									c3 = '\n';
								}
								char c4 = c3;
								if (char.IsLetter(ch))
								{
									if (char.IsLower(ch))
									{
										if (c2 == ' ')
										{
											return char.ToUpper(ch);
										}
									}
									if (char.IsUpper(ch))
									{
										if (c2 != ' ' && c2 != '\'')
										{
											return char.ToLower(ch);
										}
									}
									return ch;
								}
								if (ch == '\'')
								{
									if (c2 != ' ')
									{
										if (c2 != '\'')
										{
											if (c4 != '\'')
											{
												if (!text.Contains("'"))
												{
													return ch;
												}
											}
										}
									}
								}
								else if (ch == ' ' && c2 != ' ')
								{
									if (c2 != '\'')
									{
										if (c4 != ' ')
										{
											if (c4 != '\'')
											{
												return ch;
											}
										}
									}
								}
								return '\0';
							}
							else if (this.characterValidation == TMP_InputField.CharacterValidation.EmailAddress)
							{
								if (ch >= 'A')
								{
									if (ch <= 'Z')
									{
										return ch;
									}
								}
								if (ch >= 'a')
								{
									if (ch <= 'z')
									{
										return ch;
									}
								}
								if (ch >= '0')
								{
									if (ch <= '9')
									{
										return ch;
									}
								}
								if (ch == '@')
								{
									if (text.IndexOf('@') == -1)
									{
										return ch;
									}
								}
								if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
								{
									return ch;
								}
								if (ch == '.')
								{
									char c5 = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
									char c6 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
									if (c5 != '.')
									{
										if (c6 != '.')
										{
											return ch;
										}
									}
								}
								return '\0';
							}
							else if (this.characterValidation == TMP_InputField.CharacterValidation.Regex)
							{
								if (Regex.IsMatch(ch.ToString(), this.m_RegexValue))
								{
									return ch;
								}
								return '\0';
							}
							else
							{
								if (this.characterValidation != TMP_InputField.CharacterValidation.CustomValidator)
								{
									return '\0';
								}
								if (this.m_InputValidator != null)
								{
									char result = this.m_InputValidator.Validate(ref text, ref pos, ch);
									this.m_Text = text;
									int num = pos;
									this.stringPositionInternal = num;
									this.stringSelectPositionInternal = num;
									return result;
								}
								return '\0';
							}
						}
					}
					bool flag;
					if (pos == 0)
					{
						if (text.Length > 0)
						{
							flag = (text[0] == '-');
							goto IL_90;
						}
					}
					flag = false;
					IL_90:
					bool flag2 = flag;
					bool flag3 = this.stringPositionInternal == 0 || this.stringSelectPositionInternal == 0;
					if (!flag2)
					{
						if (ch >= '0')
						{
							if (ch <= '9')
							{
								return ch;
							}
						}
						if (ch == '-')
						{
							if (pos != 0)
							{
								if (!flag3)
								{
									goto IL_F7;
								}
							}
							return ch;
						}
						IL_F7:
						if (ch == '.')
						{
							if (this.characterValidation == TMP_InputField.CharacterValidation.Decimal)
							{
								if (!text.Contains("."))
								{
									return ch;
								}
							}
						}
					}
					return '\0';
				}
			}
			return ch;
		}

		public void ActivateInputField()
		{
			if (!(this.m_TextComponent == null))
			{
				if (!(this.m_TextComponent.font == null))
				{
					if (this.IsActive())
					{
						if (this.IsInteractable())
						{
							if (this.isFocused)
							{
								if (this.m_Keyboard != null)
								{
									if (!this.m_Keyboard.active)
									{
										this.m_Keyboard.active = true;
										this.m_Keyboard.text = this.m_Text;
									}
								}
							}
							this.m_ShouldActivateNextUpdate = true;
							return;
						}
					}
				}
			}
		}

		private void ActivateInputFieldInternal()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			if (TouchScreenKeyboard.isSupported)
			{
				if (Input.touchSupported)
				{
					TouchScreenKeyboard.hideInput = this.shouldHideMobileInput;
				}
				TouchScreenKeyboard keyboard;
				if (this.inputType == TMP_InputField.InputType.Password)
				{
					keyboard = TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, false, this.multiLine, true);
				}
				else
				{
					keyboard = TouchScreenKeyboard.Open(this.m_Text, this.keyboardType, this.inputType == TMP_InputField.InputType.AutoCorrect, this.multiLine);
				}
				this.m_Keyboard = keyboard;
				this.MoveTextEnd(false);
			}
			else
			{
				Input.imeCompositionMode = IMECompositionMode.On;
				this.OnFocus();
			}
			this.m_AllowInput = true;
			this.m_OriginalText = this.text;
			this.m_WasCanceled = false;
			this.SetCaretVisible();
			this.UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.SendOnFocus();
			this.ActivateInputField();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.ActivateInputField();
		}

		public void OnControlClick()
		{
		}

		public void DeactivateInputField()
		{
			if (!this.m_AllowInput)
			{
				return;
			}
			this.m_HasDoneFocusTransition = false;
			this.m_AllowInput = false;
			if (this.m_Placeholder != null)
			{
				this.m_Placeholder.enabled = string.IsNullOrEmpty(this.m_Text);
			}
			if (this.m_TextComponent != null)
			{
				if (this.IsInteractable())
				{
					if (this.m_WasCanceled)
					{
						if (this.m_RestoreOriginalTextOnEscape)
						{
							this.text = this.m_OriginalText;
						}
					}
					if (this.m_Keyboard != null)
					{
						this.m_Keyboard.active = false;
						this.m_Keyboard = null;
					}
					if (this.m_ResetOnDeActivation)
					{
						this.m_StringPosition = (this.m_StringSelectPosition = 0);
						this.m_CaretPosition = (this.m_CaretSelectPosition = 0);
						this.m_TextComponent.rectTransform.localPosition = this.m_DefaultTransformPosition;
						if (this.caretRectTrans != null)
						{
							this.caretRectTrans.localPosition = Vector3.zero;
						}
					}
					this.SendOnEndEdit();
					this.SendOnEndTextSelection();
					Input.imeCompositionMode = IMECompositionMode.Auto;
				}
			}
			this.MarkGeometryAsDirty();
			this.m_IsScrollbarUpdateRequired = true;
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			this.DeactivateInputField();
			base.OnDeselect(eventData);
			this.SendOnFocusLost();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (this.IsActive())
			{
				if (this.IsInteractable())
				{
					if (!this.isFocused)
					{
						this.m_ShouldActivateNextUpdate = true;
					}
					this.SendOnSubmit();
					return;
				}
			}
		}

		private void EnforceContentType()
		{
			switch (this.contentType)
			{
			case TMP_InputField.ContentType.Standard:
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.None;
				return;
			case TMP_InputField.ContentType.Autocorrected:
				this.m_InputType = TMP_InputField.InputType.AutoCorrect;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.None;
				return;
			case TMP_InputField.ContentType.IntegerNumber:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.Integer;
				return;
			case TMP_InputField.ContentType.DecimalNumber:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.Decimal;
				return;
			case TMP_InputField.ContentType.Alphanumeric:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
				return;
			case TMP_InputField.ContentType.Name:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.Name;
				return;
			case TMP_InputField.ContentType.EmailAddress:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Standard;
				this.m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.EmailAddress;
				return;
			case TMP_InputField.ContentType.Password:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Password;
				this.m_KeyboardType = TouchScreenKeyboardType.Default;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.None;
				return;
			case TMP_InputField.ContentType.Pin:
				this.m_LineType = TMP_InputField.LineType.SingleLine;
				this.m_TextComponent.enableWordWrapping = false;
				this.m_InputType = TMP_InputField.InputType.Password;
				this.m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				this.m_CharacterValidation = TMP_InputField.CharacterValidation.Digit;
				return;
			default:
				return;
			}
		}

		private void SetTextComponentWrapMode()
		{
			if (this.m_TextComponent == null)
			{
				return;
			}
			if (this.m_LineType == TMP_InputField.LineType.SingleLine)
			{
				this.m_TextComponent.enableWordWrapping = false;
			}
			else
			{
				this.m_TextComponent.enableWordWrapping = true;
			}
		}

		private void SetTextComponentRichTextMode()
		{
			if (this.m_TextComponent == null)
			{
				return;
			}
			this.m_TextComponent.richText = this.m_RichText;
		}

		private void SetToCustomIfContentTypeIsNot(params TMP_InputField.ContentType[] allowedContentTypes)
		{
			if (this.contentType == TMP_InputField.ContentType.Custom)
			{
				return;
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (this.contentType == allowedContentTypes[i])
				{
					return;
				}
			}
			this.contentType = TMP_InputField.ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (this.contentType == TMP_InputField.ContentType.Custom)
			{
				return;
			}
			this.contentType = TMP_InputField.ContentType.Custom;
		}

		private void SetToCustom(TMP_InputField.CharacterValidation characterValidation)
		{
			if (this.contentType == TMP_InputField.ContentType.Custom)
			{
				return;
			}
			this.contentType = TMP_InputField.ContentType.Custom;
		}

		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.m_HasDoneFocusTransition)
			{
				state = Selectable.SelectionState.Highlighted;
			}
			else if (state == Selectable.SelectionState.Pressed)
			{
				this.m_HasDoneFocusTransition = true;
			}
			base.DoStateTransition(state, instant);
		}

		public void SetGlobalPointSize(float pointSize)
		{
			TMP_Text tmp_Text = this.m_Placeholder as TMP_Text;
			if (tmp_Text != null)
			{
				tmp_Text.fontSize = pointSize;
			}
			this.textComponent.fontSize = pointSize;
		}

		public void SetGlobalFontAsset(TMP_FontAsset fontAsset)
		{
			TMP_Text tmp_Text = this.m_Placeholder as TMP_Text;
			if (tmp_Text != null)
			{
				tmp_Text.font = fontAsset;
			}
			this.textComponent.font = fontAsset;
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		bool ICanvasElement.IsDestroyed()
		{
			return base.IsDestroyed();
		}

		public enum ContentType
		{
			Standard,
			Autocorrected,
			IntegerNumber,
			DecimalNumber,
			Alphanumeric,
			Name,
			EmailAddress,
			Password,
			Pin,
			Custom
		}

		public enum InputType
		{
			Standard,
			AutoCorrect,
			Password
		}

		public enum CharacterValidation
		{
			None,
			Digit,
			Integer,
			Decimal,
			Alphanumeric,
			Name,
			Regex,
			EmailAddress,
			CustomValidator
		}

		public enum LineType
		{
			SingleLine,
			MultiLineSubmit,
			MultiLineNewline
		}

		public delegate char OnValidateInput(string text, int charIndex, char addedChar);

		[Serializable]
		public class SubmitEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class OnChangeEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class SelectionEvent : UnityEvent<string>
		{
		}

		[Serializable]
		public class TextSelectionEvent : UnityEvent<string, int, int>
		{
		}

		protected enum EditState
		{
			Continue,
			Finish
		}
	}
}
