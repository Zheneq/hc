using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[AddComponentMenu("UI/TextMeshPro - Input Field", 11)]
	public class TMP_InputField : Selectable, IUpdateSelectedHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, IScrollHandler, IEventSystemHandler
	{
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

		protected TouchScreenKeyboard m_Keyboard;

		private static readonly char[] kSeparators = new char[6]
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
		private ContentType m_ContentType;

		[SerializeField]
		private InputType m_InputType;

		[SerializeField]
		private char m_AsteriskChar = '*';

		[SerializeField]
		private TouchScreenKeyboardType m_KeyboardType;

		[SerializeField]
		private LineType m_LineType;

		[SerializeField]
		private bool m_HideMobileInput;

		[SerializeField]
		private CharacterValidation m_CharacterValidation;

		[SerializeField]
		private string m_RegexValue = string.Empty;

		[SerializeField]
		private float m_GlobalPointSize = 14f;

		[SerializeField]
		private int m_CharacterLimit;

		[SerializeField]
		private SubmitEvent m_OnEndEdit = new SubmitEvent();

		[SerializeField]
		private SubmitEvent m_OnSubmit = new SubmitEvent();

		[SerializeField]
		private SelectionEvent m_OnSelect = new SelectionEvent();

		[SerializeField]
		private SelectionEvent m_OnDeselect = new SelectionEvent();

		[SerializeField]
		private TextSelectionEvent m_OnTextSelection = new TextSelectionEvent();

		[SerializeField]
		private TextSelectionEvent m_OnEndTextSelection = new TextSelectionEvent();

		[SerializeField]
		private OnChangeEvent m_OnValueChanged = new OnChangeEvent();

		[SerializeField]
		private OnValidateInput m_OnValidateInput;

		[SerializeField]
		private Color m_CaretColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

		[SerializeField]
		private bool m_CustomCaretColor;

		[SerializeField]
		private Color m_SelectionColor = new Color(56f / 85f, 206f / 255f, 1f, 64f / 85f);

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

		protected Mesh mesh
		{
			get
			{
				if (m_Mesh == null)
				{
					m_Mesh = new Mesh();
				}
				return m_Mesh;
			}
		}

		public bool shouldHideMobileInput
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				switch (platform)
				{
				default:
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (platform == RuntimePlatform.tvOS)
					{
						break;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						return true;
					}
				case RuntimePlatform.IPhonePlayer:
				case RuntimePlatform.Android:
				case RuntimePlatform.TizenPlayer:
					break;
				}
				return m_HideMobileInput;
			}
			set
			{
				SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
			}
		}

		public string text
		{
			get
			{
				return m_Text;
			}
			set
			{
				if (text == value)
				{
					return;
				}
				m_Text = value;
				if (m_Keyboard != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_Keyboard.text = m_Text;
				}
				if (m_StringPosition > m_Text.Length)
				{
					m_StringPosition = (m_StringSelectPosition = m_Text.Length);
				}
				AdjustTextPositionRelativeToViewport(0f);
				m_forceRectTransformAdjustment = true;
				SendOnValueChangedAndUpdateLabel();
			}
		}

		public bool isFocused => m_AllowInput;

		public float caretBlinkRate
		{
			get
			{
				return m_CaretBlinkRate;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value) || !m_AllowInput)
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetCaretActive();
					return;
				}
			}
		}

		public int caretWidth
		{
			get
			{
				return m_CaretWidth;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_CaretWidth, value))
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					MarkGeometryAsDirty();
					return;
				}
			}
		}

		public RectTransform textViewport
		{
			get
			{
				return m_TextViewport;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextViewport, value);
			}
		}

		public TMP_Text textComponent
		{
			get
			{
				return m_TextComponent;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_TextComponent, value);
			}
		}

		public Graphic placeholder
		{
			get
			{
				return m_Placeholder;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_Placeholder, value);
			}
		}

		public Scrollbar verticalScrollbar
		{
			get
			{
				return m_VerticalScrollbar;
			}
			set
			{
				if (m_VerticalScrollbar != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_VerticalScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChange);
				}
				SetPropertyUtility.SetClass(ref m_VerticalScrollbar, value);
				if (!m_VerticalScrollbar)
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_VerticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
					return;
				}
			}
		}

		public float scrollSensitivity
		{
			get
			{
				return m_ScrollSensitivity;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_ScrollSensitivity, value))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					MarkGeometryAsDirty();
					return;
				}
			}
		}

		public Color caretColor
		{
			get
			{
				Color result;
				if (customCaretColor)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = m_CaretColor;
				}
				else
				{
					result = textComponent.color;
				}
				return result;
			}
			set
			{
				if (!SetPropertyUtility.SetColor(ref m_CaretColor, value))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					MarkGeometryAsDirty();
					return;
				}
			}
		}

		public bool customCaretColor
		{
			get
			{
				return m_CustomCaretColor;
			}
			set
			{
				if (m_CustomCaretColor != value)
				{
					m_CustomCaretColor = value;
					MarkGeometryAsDirty();
				}
			}
		}

		public Color selectionColor
		{
			get
			{
				return m_SelectionColor;
			}
			set
			{
				if (!SetPropertyUtility.SetColor(ref m_SelectionColor, value))
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					MarkGeometryAsDirty();
					return;
				}
			}
		}

		public SubmitEvent onEndEdit
		{
			get
			{
				return m_OnEndEdit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndEdit, value);
			}
		}

		public SubmitEvent onSubmit
		{
			get
			{
				return m_OnSubmit;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnSubmit, value);
			}
		}

		public SelectionEvent onSelect
		{
			get
			{
				return m_OnSelect;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnSelect, value);
			}
		}

		public SelectionEvent onDeselect
		{
			get
			{
				return m_OnDeselect;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnDeselect, value);
			}
		}

		public TextSelectionEvent onTextSelection
		{
			get
			{
				return m_OnTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnTextSelection, value);
			}
		}

		public TextSelectionEvent onEndTextSelection
		{
			get
			{
				return m_OnEndTextSelection;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnEndTextSelection, value);
			}
		}

		public OnChangeEvent onValueChanged
		{
			get
			{
				return m_OnValueChanged;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValueChanged, value);
			}
		}

		public OnValidateInput onValidateInput
		{
			get
			{
				return m_OnValidateInput;
			}
			set
			{
				SetPropertyUtility.SetClass(ref m_OnValidateInput, value);
			}
		}

		public int characterLimit
		{
			get
			{
				return m_CharacterLimit;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
				{
					UpdateLabel();
				}
			}
		}

		public float pointSize
		{
			get
			{
				return m_GlobalPointSize;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_GlobalPointSize, Math.Max(0f, value)))
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetGlobalPointSize(m_GlobalPointSize);
					UpdateLabel();
					return;
				}
			}
		}

		public TMP_FontAsset fontAsset
		{
			get
			{
				return m_GlobalFontAsset;
			}
			set
			{
				if (!SetPropertyUtility.SetClass(ref m_GlobalFontAsset, value))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetGlobalFontAsset(m_GlobalFontAsset);
					UpdateLabel();
					return;
				}
			}
		}

		public bool onFocusSelectAll
		{
			get
			{
				return m_OnFocusSelectAll;
			}
			set
			{
				m_OnFocusSelectAll = value;
			}
		}

		public bool resetOnDeActivation
		{
			get
			{
				return m_ResetOnDeActivation;
			}
			set
			{
				m_ResetOnDeActivation = value;
			}
		}

		public bool restoreOriginalTextOnEscape
		{
			get
			{
				return m_RestoreOriginalTextOnEscape;
			}
			set
			{
				m_RestoreOriginalTextOnEscape = value;
			}
		}

		public bool isRichTextEditingAllowed
		{
			get
			{
				return m_isRichTextEditingAllowed;
			}
			set
			{
				m_isRichTextEditingAllowed = value;
			}
		}

		public ContentType contentType
		{
			get
			{
				return m_ContentType;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_ContentType, value))
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					EnforceContentType();
					return;
				}
			}
		}

		public LineType lineType
		{
			get
			{
				return m_LineType;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_LineType, value))
				{
					SetTextComponentWrapMode();
				}
				SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
			}
		}

		public InputType inputType
		{
			get
			{
				return m_InputType;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_InputType, value))
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetToCustom();
					return;
				}
			}
		}

		public TouchScreenKeyboardType keyboardType
		{
			get
			{
				return m_KeyboardType;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_KeyboardType, value))
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetToCustom();
					return;
				}
			}
		}

		public CharacterValidation characterValidation
		{
			get
			{
				return m_CharacterValidation;
			}
			set
			{
				if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value))
				{
					SetToCustom();
				}
			}
		}

		public TMP_InputValidator inputValidator
		{
			get
			{
				return m_InputValidator;
			}
			set
			{
				if (!SetPropertyUtility.SetClass(ref m_InputValidator, value))
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetToCustom(CharacterValidation.CustomValidator);
					return;
				}
			}
		}

		public bool readOnly
		{
			get
			{
				return m_ReadOnly;
			}
			set
			{
				m_ReadOnly = value;
			}
		}

		public bool richText
		{
			get
			{
				return m_RichText;
			}
			set
			{
				m_RichText = value;
				SetTextComponentRichTextMode();
			}
		}

		public bool multiLine
		{
			get
			{
				int result;
				if (m_LineType != LineType.MultiLineNewline)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = ((lineType == LineType.MultiLineSubmit) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}
		}

		public char asteriskChar
		{
			get
			{
				return m_AsteriskChar;
			}
			set
			{
				if (!SetPropertyUtility.SetStruct(ref m_AsteriskChar, value))
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UpdateLabel();
					return;
				}
			}
		}

		public bool wasCanceled => m_WasCanceled;

		protected int caretPositionInternal
		{
			get
			{
				return m_CaretPosition + Input.compositionString.Length;
			}
			set
			{
				m_CaretPosition = value;
				ClampCaretPos(ref m_CaretPosition);
			}
		}

		protected int stringPositionInternal
		{
			get
			{
				return m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				m_StringPosition = value;
				ClampStringPos(ref m_StringPosition);
			}
		}

		protected int caretSelectPositionInternal
		{
			get
			{
				return m_CaretSelectPosition + Input.compositionString.Length;
			}
			set
			{
				m_CaretSelectPosition = value;
				ClampCaretPos(ref m_CaretSelectPosition);
			}
		}

		protected int stringSelectPositionInternal
		{
			get
			{
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				m_StringSelectPosition = value;
				ClampStringPos(ref m_StringSelectPosition);
			}
		}

		private bool hasSelection => stringPositionInternal != stringSelectPositionInternal;

		public int caretPosition
		{
			get
			{
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				selectionAnchorPosition = value;
				selectionFocusPosition = value;
				isStringPositionDirty = true;
			}
		}

		public int selectionAnchorPosition
		{
			get
			{
				return m_StringPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return;
						}
					}
				}
				caretPositionInternal = value;
				stringPositionInternal = value;
				isStringPositionDirty = true;
			}
		}

		public int selectionFocusPosition
		{
			get
			{
				return m_StringSelectPosition + Input.compositionString.Length;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return;
						}
					}
				}
				caretSelectPositionInternal = value;
				stringSelectPositionInternal = value;
				isStringPositionDirty = true;
			}
		}

		public int stringPosition
		{
			get
			{
				return stringSelectPositionInternal;
			}
			set
			{
				selectionStringAnchorPosition = value;
				selectionStringFocusPosition = value;
			}
		}

		public int selectionStringAnchorPosition
		{
			get
			{
				return stringPositionInternal;
			}
			set
			{
				if (Input.compositionString.Length != 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return;
						}
					}
				}
				stringPositionInternal = value;
			}
		}

		public int selectionStringFocusPosition
		{
			get
			{
				return stringSelectPositionInternal;
			}
			set
			{
				if (Input.compositionString.Length == 0)
				{
					stringSelectPositionInternal = value;
				}
			}
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

		protected TMP_InputField()
		{
		}

		protected void ClampStringPos(ref int pos)
		{
			if (pos < 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						pos = 0;
						return;
					}
				}
			}
			if (pos <= text.Length)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				pos = text.Length;
				return;
			}
		}

		protected void ClampCaretPos(ref int pos)
		{
			if (pos < 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						pos = 0;
						return;
					}
				}
			}
			if (pos <= m_TextComponent.textInfo.characterCount - 1)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				pos = m_TextComponent.textInfo.characterCount - 1;
				return;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (m_Text == null)
			{
				m_Text = string.Empty;
			}
			if (Application.isPlaying)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_CachedInputRenderer == null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_TextComponent != null)
					{
						GameObject gameObject = new GameObject(base.transform.name + " Input Caret", typeof(RectTransform));
						TMP_SelectionCaret tMP_SelectionCaret = gameObject.AddComponent<TMP_SelectionCaret>();
						tMP_SelectionCaret.color = Color.clear;
						gameObject.hideFlags = HideFlags.DontSave;
						gameObject.transform.SetParent(m_TextComponent.transform.parent);
						gameObject.transform.SetAsFirstSibling();
						gameObject.layer = base.gameObject.layer;
						caretRectTrans = gameObject.GetComponent<RectTransform>();
						m_CachedInputRenderer = gameObject.GetComponent<CanvasRenderer>();
						m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
						gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
						AssignPositioningIfNeeded();
					}
				}
			}
			if (m_CachedInputRenderer != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
			}
			if (m_TextComponent != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
				m_TextComponent.ignoreRectMaskCulling = true;
				m_DefaultTransformPosition = m_TextComponent.rectTransform.localPosition;
				if (m_VerticalScrollbar != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_VerticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
				}
				UpdateLabel();
			}
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
		}

		protected override void OnDisable()
		{
			m_BlinkCoroutine = null;
			DeactivateInputField();
			if (m_TextComponent != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
				m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
				if (m_VerticalScrollbar != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					m_VerticalScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChange);
				}
			}
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (m_CachedInputRenderer != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_CachedInputRenderer.Clear();
			}
			if (m_Mesh != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UnityEngine.Object.DestroyImmediate(m_Mesh);
			}
			m_Mesh = null;
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
			base.OnDisable();
		}

		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (!(obj == m_TextComponent))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (Application.isPlaying)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
						caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
						return;
					}
				}
				return;
			}
		}

		private IEnumerator CaretBlink()
		{
			m_CaretVisible = true;
			yield return null;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private void SetCaretVisible()
		{
			if (!m_AllowInput)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_CaretVisible = true;
			m_BlinkStartTime = Time.unscaledTime;
			SetCaretActive();
		}

		private void SetCaretActive()
		{
			if (!m_AllowInput)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (m_CaretBlinkRate > 0f)
			{
				if (m_BlinkCoroutine == null)
				{
					m_BlinkCoroutine = StartCoroutine(CaretBlink());
				}
			}
			else
			{
				m_CaretVisible = true;
			}
		}

		protected void OnFocus()
		{
			if (!m_OnFocusSelectAll)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				SelectAll();
				return;
			}
		}

		protected void SelectAll()
		{
			m_isSelectAll = true;
			stringPositionInternal = text.Length;
			stringSelectPositionInternal = 0;
		}

		public void MoveTextEnd(bool shift)
		{
			if (m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				int length = text.Length;
				if (shift)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					stringSelectPositionInternal = length;
				}
				else
				{
					stringPositionInternal = length;
					stringSelectPositionInternal = stringPositionInternal;
				}
			}
			else
			{
				int num = m_TextComponent.textInfo.characterCount - 1;
				if (!shift)
				{
					int num4 = caretPositionInternal = (caretSelectPositionInternal = num);
					num4 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(num)));
				}
				else
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					caretSelectPositionInternal = num;
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(num);
				}
			}
			UpdateLabel();
		}

		public void MoveTextStart(bool shift)
		{
			if (m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				int num = 0;
				if (shift)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					stringSelectPositionInternal = num;
				}
				else
				{
					stringPositionInternal = num;
					stringSelectPositionInternal = stringPositionInternal;
				}
			}
			else
			{
				int num2 = 0;
				if (!shift)
				{
					int num5 = caretPositionInternal = (caretSelectPositionInternal = num2);
					num5 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(num2)));
				}
				else
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					caretSelectPositionInternal = num2;
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(num2);
				}
			}
			UpdateLabel();
		}

		public void MoveToEndOfLine(bool shift, bool ctrl)
		{
			int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
			int num;
			if (ctrl)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = m_TextComponent.textInfo.characterCount - 1;
			}
			else
			{
				num = m_TextComponent.textInfo.lineInfo[lineNumber].lastCharacterIndex;
			}
			int caretPosition = num;
			caretPosition = GetStringIndexFromCaretPosition(caretPosition);
			if (shift)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				stringSelectPositionInternal = caretPosition;
			}
			else
			{
				stringPositionInternal = caretPosition;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		public void MoveToStartOfLine(bool shift, bool ctrl)
		{
			int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
			int num;
			if (ctrl)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = 0;
			}
			else
			{
				num = m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex;
			}
			int caretPosition = num;
			caretPosition = GetStringIndexFromCaretPosition(caretPosition);
			if (shift)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				stringSelectPositionInternal = caretPosition;
			}
			else
			{
				stringPositionInternal = caretPosition;
				stringSelectPositionInternal = stringPositionInternal;
			}
			UpdateLabel();
		}

		private bool InPlaceEditing()
		{
			return !TouchScreenKeyboard.isSupported;
		}

		protected virtual void LateUpdate()
		{
			if (m_ShouldActivateNextUpdate)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!isFocused)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						ActivateInputFieldInternal();
						m_ShouldActivateNextUpdate = false;
						return;
					}
				}
				m_ShouldActivateNextUpdate = false;
			}
			if (m_IsScrollbarUpdateRequired)
			{
				UpdateScrollbar();
				m_IsScrollbarUpdateRequired = false;
			}
			if (InPlaceEditing())
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (!isFocused)
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
				AssignPositioningIfNeeded();
				if (m_Keyboard != null)
				{
					if (m_Keyboard.active)
					{
						string text = m_Keyboard.text;
						if (m_Text != text)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_ReadOnly)
							{
								m_Keyboard.text = m_Text;
							}
							else
							{
								m_Text = string.Empty;
								for (int i = 0; i < text.Length; i++)
								{
									char c = text[i];
									if (c == '\r' || c == '\u0003')
									{
										c = '\n';
									}
									if (onValidateInput != null)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
										c = onValidateInput(m_Text, m_Text.Length, c);
									}
									else if (characterValidation != 0)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										c = Validate(m_Text, m_Text.Length, c);
									}
									if (lineType == LineType.MultiLineSubmit)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c == '\n')
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													break;
												default:
													m_Keyboard.text = m_Text;
													OnSubmit(null);
													OnDeselect(null);
													return;
												}
											}
										}
									}
									if (c != 0)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										m_Text += c;
									}
								}
								if (characterLimit > 0)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (m_Text.Length > characterLimit)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										m_Text = m_Text.Substring(0, characterLimit);
									}
								}
								int num2 = stringPositionInternal = (stringSelectPositionInternal = m_Text.Length);
								if (m_Text != text)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									m_Keyboard.text = m_Text;
								}
								SendOnValueChangedAndUpdateLabel();
							}
						}
						if (!m_Keyboard.done)
						{
							return;
						}
						if (m_Keyboard.wasCanceled)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							m_WasCanceled = true;
						}
						OnDeselect(null);
						return;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (m_Keyboard != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_ReadOnly)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						this.text = m_Keyboard.text;
					}
					if (m_Keyboard.wasCanceled)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						m_WasCanceled = true;
					}
					if (m_Keyboard.done)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						OnSubmit(null);
					}
				}
				OnDeselect(null);
				return;
			}
		}

		private bool MayDrag(PointerEventData eventData)
		{
			int result;
			if (IsActive())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (IsInteractable())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (eventData.button == PointerEventData.InputButton.Left)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_TextComponent != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							result = ((m_Keyboard == null) ? 1 : 0);
							goto IL_006b;
						}
					}
				}
			}
			result = 0;
			goto IL_006b;
			IL_006b:
			return (byte)result != 0;
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_UpdateDrag = true;
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			CaretPosition cursor;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
			if (cursor == CaretPosition.Left)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
			}
			else if (cursor == CaretPosition.Right)
			{
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
			}
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			MarkGeometryAsDirty();
			m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textViewport, eventData.position, eventData.pressEventCamera);
			if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_DragCoroutine = StartCoroutine(MouseDragOutsideRect(eventData));
			}
			eventData.Use();
		}

		private IEnumerator MouseDragOutsideRect(PointerEventData eventData)
		{
			if (m_UpdateDrag)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_DragPositionOutOfBounds)
				{
					RectTransformUtility.ScreenPointToLocalPointInRectangle(textViewport, eventData.position, eventData.pressEventCamera, out Vector2 localMousePos);
					Rect rect = textViewport.rect;
					if (multiLine)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (localMousePos.y > rect.yMax)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							MoveUp(true, true);
						}
						else if (localMousePos.y < rect.yMin)
						{
							MoveDown(true, true);
						}
					}
					else if (localMousePos.x < rect.xMin)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						MoveLeft(true, false);
					}
					else if (localMousePos.x > rect.xMax)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						MoveRight(true, false);
					}
					UpdateLabel();
					float num;
					if (multiLine)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num = 0.1f;
					}
					else
					{
						num = 0.05f;
					}
					float delay = num;
					yield return new WaitForSeconds(delay);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			m_DragCoroutine = null;
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_UpdateDrag = false;
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			if (!MayDrag(eventData))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			bool allowInput = m_AllowInput;
			base.OnPointerDown(eventData);
			if (!InPlaceEditing() && (m_Keyboard == null || !m_Keyboard.active))
			{
				OnSelect(eventData);
				return;
			}
			bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			bool flag2 = false;
			float unscaledTime = Time.unscaledTime;
			if (m_ClickStartTime + m_DoubleClickDelay > unscaledTime)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag2 = true;
			}
			m_ClickStartTime = unscaledTime;
			if (!allowInput)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_OnFocusSelectAll)
				{
					goto IL_02c3;
				}
			}
			CaretPosition cursor;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
			if (flag)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (cursor == CaretPosition.Left)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
				}
				else if (cursor == CaretPosition.Right)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
				}
			}
			else if (cursor == CaretPosition.Left)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int num2 = stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition));
			}
			else if (cursor == CaretPosition.Right)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				int num2 = stringPositionInternal = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1);
			}
			if (!flag2)
			{
				int num2 = caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal));
			}
			else
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int num6 = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, eventData.position, eventData.pressEventCamera);
				if (num6 != -1)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					caretPositionInternal = m_TextComponent.textInfo.wordInfo[num6].firstCharacterIndex;
					caretSelectPositionInternal = m_TextComponent.textInfo.wordInfo[num6].lastCharacterIndex + 1;
					stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
				}
				else
				{
					caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
					stringSelectPositionInternal++;
					caretSelectPositionInternal = caretPositionInternal + 1;
					caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
				}
			}
			goto IL_02c3;
			IL_02c3:
			UpdateLabel();
			eventData.Use();
		}

		protected EditState KeyPressed(Event evt)
		{
			EventModifiers modifiers = evt.modifiers;
			RuntimePlatform platform = Application.platform;
			int num;
			if (platform != 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = ((platform == RuntimePlatform.OSXPlayer) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (num == 0) ? ((modifiers & EventModifiers.Control) != 0) : ((modifiers & EventModifiers.Command) != 0);
			bool flag2 = (modifiers & EventModifiers.Shift) != 0;
			bool flag3 = (modifiers & EventModifiers.Alt) != 0;
			int num2;
			if (flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag3)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = ((!flag2) ? 1 : 0);
					goto IL_0080;
				}
			}
			num2 = 0;
			goto IL_0080;
			IL_02bb:
			char c;
			if (c != '\r')
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (c != '\u0003')
				{
					goto IL_02de;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			c = '\n';
			goto IL_02de;
			IL_0080:
			bool flag4 = (byte)num2 != 0;
			KeyCode keyCode = evt.keyCode;
			switch (keyCode)
			{
			default:
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (keyCode != KeyCode.Escape)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (keyCode != KeyCode.Delete)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
					ForwardSpace();
					return EditState.Continue;
				}
				m_WasCanceled = true;
				return EditState.Finish;
			case KeyCode.Backspace:
				Backspace();
				return EditState.Continue;
			case KeyCode.Home:
				MoveToStartOfLine(flag2, flag);
				return EditState.Continue;
			case KeyCode.End:
				MoveToEndOfLine(flag2, flag);
				return EditState.Continue;
			case KeyCode.A:
				if (!flag4)
				{
					break;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					SelectAll();
					return EditState.Continue;
				}
			case KeyCode.C:
				if (!flag4)
				{
					break;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (inputType != InputType.Password)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = string.Empty;
					}
					return EditState.Continue;
				}
			case KeyCode.V:
				if (!flag4)
				{
					break;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					Append(clipboard);
					return EditState.Continue;
				}
			case KeyCode.X:
				if (flag4)
				{
					if (inputType != InputType.Password)
					{
						clipboard = GetSelectedString();
					}
					else
					{
						clipboard = string.Empty;
					}
					Delete();
					SendOnValueChangedAndUpdateLabel();
					return EditState.Continue;
				}
				break;
			case KeyCode.LeftArrow:
				MoveLeft(flag2, flag);
				return EditState.Continue;
			case KeyCode.RightArrow:
				MoveRight(flag2, flag);
				return EditState.Continue;
			case KeyCode.UpArrow:
				MoveUp(flag2);
				return EditState.Continue;
			case KeyCode.DownArrow:
				MoveDown(flag2);
				return EditState.Continue;
			case KeyCode.PageUp:
				MovePageUp(flag2);
				return EditState.Continue;
			case KeyCode.PageDown:
				MovePageDown(flag2);
				return EditState.Continue;
			case KeyCode.Return:
			case KeyCode.KeypadEnter:
				if (lineType == LineType.MultiLineNewline)
				{
					break;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					return EditState.Finish;
				}
			}
			c = evt.character;
			if (!multiLine)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (c != '\t')
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (c != '\r')
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (c != '\n')
						{
							goto IL_02bb;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				return EditState.Continue;
			}
			goto IL_02bb;
			IL_02de:
			if (IsValidChar(c))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Append(c);
			}
			if (c == '\0')
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Input.compositionString.Length > 0)
				{
					UpdateLabel();
				}
			}
			return EditState.Continue;
		}

		private bool IsValidChar(char c)
		{
			if (c == '\u007f')
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return false;
					}
				}
			}
			if (c != '\t')
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (c != '\n')
				{
					return m_TextComponent.font.HasCharacter(c, true);
				}
			}
			return true;
		}

		public void ProcessEvent(Event e)
		{
			KeyPressed(e);
		}

		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			if (!isFocused)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			bool flag = false;
			while (Event.PopEvent(m_ProcessingEvent))
			{
				if (m_ProcessingEvent.rawType == EventType.KeyDown)
				{
					flag = true;
					EditState editState = KeyPressed(m_ProcessingEvent);
					if (editState == EditState.Finish)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						SendOnSubmit();
						DeactivateInputField();
						break;
					}
				}
				EventType type = m_ProcessingEvent.type;
				if (type != EventType.ValidateCommand)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (type != EventType.ExecuteCommand)
					{
						continue;
					}
				}
				string commandName = m_ProcessingEvent.commandName;
				if (commandName == null)
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(commandName == "SelectAll"))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					SelectAll();
					flag = true;
				}
			}
			if (flag)
			{
				UpdateLabel();
			}
			eventData.Use();
		}

		public virtual void OnScroll(PointerEventData eventData)
		{
			if (m_TextComponent.preferredHeight < m_TextViewport.rect.height)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			Vector2 scrollDelta = eventData.scrollDelta;
			float num = 0f - scrollDelta.y;
			m_ScrollPosition += 1f / (float)m_TextComponent.textInfo.lineCount * num * m_ScrollSensitivity;
			m_ScrollPosition = Mathf.Clamp01(m_ScrollPosition);
			AdjustTextPositionRelativeToViewport(m_ScrollPosition);
			m_AllowInput = false;
			if (!m_VerticalScrollbar)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				m_IsUpdatingScrollbarValues = true;
				m_VerticalScrollbar.value = m_ScrollPosition;
				return;
			}
		}

		private string GetSelectedString()
		{
			if (!hasSelection)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return string.Empty;
					}
				}
			}
			int num = stringPositionInternal;
			int num2 = stringSelectPositionInternal;
			if (num > num2)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			return text.Substring(num, num2 - num);
		}

		private int FindtNextWordBegin()
		{
			if (stringSelectPositionInternal + 1 >= text.Length)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return text.Length;
					}
				}
			}
			int num = text.IndexOfAny(kSeparators, stringSelectPositionInternal + 1);
			if (num == -1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return text.Length;
					}
				}
			}
			return num + 1;
		}

		private void MoveRight(bool shift, bool ctrl)
		{
			int num3;
			if (hasSelection && !shift)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						num3 = (stringPositionInternal = (stringSelectPositionInternal = Mathf.Max(stringPositionInternal, stringSelectPositionInternal)));
						num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
						return;
					}
				}
			}
			int num5;
			if (!ctrl)
			{
				num5 = ((!m_isRichTextEditingAllowed) ? GetStringIndexFromCaretPosition(caretSelectPositionInternal + 1) : (stringSelectPositionInternal + 1));
			}
			else
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num5 = FindtNextWordBegin();
			}
			if (shift)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						stringSelectPositionInternal = num5;
						caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
						return;
					}
				}
			}
			num3 = (stringSelectPositionInternal = (stringPositionInternal = num5));
			num3 = (caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
		}

		private int FindtPrevWordBegin()
		{
			if (stringSelectPositionInternal - 2 < 0)
			{
				return 0;
			}
			int num = text.LastIndexOfAny(kSeparators, stringSelectPositionInternal - 2);
			if (num == -1)
			{
				return 0;
			}
			return num + 1;
		}

		private void MoveLeft(bool shift, bool ctrl)
		{
			int num3;
			if (hasSelection)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!shift)
				{
					num3 = (stringPositionInternal = (stringSelectPositionInternal = Mathf.Min(stringPositionInternal, stringSelectPositionInternal)));
					num3 = (caretPositionInternal = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
					return;
				}
			}
			int num5;
			if (ctrl)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num5 = FindtPrevWordBegin();
			}
			else if (m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num5 = stringSelectPositionInternal - 1;
			}
			else
			{
				num5 = GetStringIndexFromCaretPosition(caretSelectPositionInternal - 1);
			}
			if (shift)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						stringSelectPositionInternal = num5;
						caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
						return;
					}
				}
			}
			num3 = (stringSelectPositionInternal = (stringPositionInternal = num5));
			num3 = (caretSelectPositionInternal = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal)));
		}

		private int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				originalPos--;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return (!goToFirstChar) ? originalPos : 0;
					}
				}
			}
			int num = m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex; i < num; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num6 <= 1f)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (num6 < 0.5f)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											return i;
										}
									}
								}
								return i + 1;
							}
						}
					}
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num3)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = i;
					num3 = num5;
					num4 = num6;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (num2 == -1)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return num;
						}
					}
				}
				if (num4 < 0.5f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return num2;
						}
					}
				}
				return num2 + 1;
			}
		}

		private int LineDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return m_TextComponent.textInfo.characterCount - 1;
					}
				}
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
			{
				int result;
				if (goToLastChar)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					result = m_TextComponent.textInfo.characterCount - 1;
				}
				else
				{
					result = originalPos;
				}
				return result;
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
			int num = -1;
			float num2 = 32767f;
			float num3 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num4 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num5 = num4 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num5 >= 0f && num5 <= 1f)
				{
					if (num5 < 0.5f)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return i;
							}
						}
					}
					return i + 1;
				}
				num4 = Mathf.Abs(num4);
				if (num4 < num2)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num = i;
					num2 = num4;
					num3 = num5;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
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
		}

		private int PageUpCharacterPosition(int originalPos, bool goToFirstChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				originalPos--;
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber - 1 < 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return (!goToFirstChar) ? originalPos : 0;
					}
				}
			}
			float height = m_TextViewport.rect.height;
			int num = lineNumber - 1;
			while (true)
			{
				if (num > 0)
				{
					if (m_TextComponent.textInfo.lineInfo[num].baseline > m_TextComponent.textInfo.lineInfo[lineNumber].baseline + height)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
					num--;
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[num].lastCharacterIndex;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[num].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num6 <= 1f)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								if (num6 < 0.5f)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											return i;
										}
									}
								}
								return i + 1;
							}
						}
					}
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num3)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = i;
					num3 = num5;
					num4 = num6;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (num2 == -1)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return lastCharacterIndex;
						}
					}
				}
				if (num4 < 0.5f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return num2;
						}
					}
				}
				return num2 + 1;
			}
		}

		private int PageDownCharacterPosition(int originalPos, bool goToLastChar)
		{
			if (originalPos >= m_TextComponent.textInfo.characterCount)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return m_TextComponent.textInfo.characterCount - 1;
					}
				}
			}
			TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
			int lineNumber = tMP_CharacterInfo.lineNumber;
			if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return (!goToLastChar) ? originalPos : (m_TextComponent.textInfo.characterCount - 1);
					}
				}
			}
			float height = m_TextViewport.rect.height;
			int num = lineNumber + 1;
			while (true)
			{
				if (num < m_TextComponent.textInfo.lineCount - 1)
				{
					if (m_TextComponent.textInfo.lineInfo[num].baseline < m_TextComponent.textInfo.lineInfo[lineNumber].baseline - height)
					{
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[num].lastCharacterIndex;
			int num2 = -1;
			float num3 = 32767f;
			float num4 = 0f;
			for (int i = m_TextComponent.textInfo.lineInfo[num].firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[i];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				float num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num6 <= 1f)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (num6 < 0.5f)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											return i;
										}
									}
								}
								return i + 1;
							}
						}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (num2 == -1)
				{
					return lastCharacterIndex;
				}
				if (num4 < 0.5f)
				{
					return num2;
				}
				return num2 + 1;
			}
		}

		private void MoveDown(bool shift)
		{
			MoveDown(shift, true);
		}

		private void MoveDown(bool shift, bool goToLastChar)
		{
			if (hasSelection)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!shift)
				{
					int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
				}
			}
			int num4;
			if (multiLine)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num4 = LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar);
			}
			else
			{
				num4 = m_TextComponent.textInfo.characterCount - 1;
			}
			int num5 = num4;
			if (shift)
			{
				caretSelectPositionInternal = num5;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			else
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num5);
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
		}

		private void MoveUp(bool shift)
		{
			MoveUp(shift, true);
		}

		private void MoveUp(bool shift, bool goToFirstChar)
		{
			int num3;
			if (hasSelection && !shift)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num3 = (caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal)));
			}
			int num4 = multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0;
			if (shift)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						caretSelectPositionInternal = num4;
						stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
						return;
					}
				}
			}
			num3 = (caretSelectPositionInternal = (caretPositionInternal = num4));
			num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
		}

		private void MovePageUp(bool shift)
		{
			MovePageUp(shift, true);
		}

		private void MovePageUp(bool shift, bool goToFirstChar)
		{
			if (hasSelection && !shift)
			{
				int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
			}
			int num4;
			if (multiLine)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num4 = PageUpCharacterPosition(caretSelectPositionInternal, goToFirstChar);
			}
			else
			{
				num4 = 0;
			}
			int num5 = num4;
			if (!shift)
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num5);
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
			else
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				caretSelectPositionInternal = num5;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			if (m_LineType == LineType.SingleLine)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				float height = m_TextViewport.rect.height;
				Vector3 position = m_TextComponent.rectTransform.position;
				float y = position.y;
				Vector3 max = m_TextComponent.textBounds.max;
				float num9 = y + max.y;
				Vector3 position2 = m_TextViewport.position;
				float num10 = position2.y + m_TextViewport.rect.yMax;
				height = ((!(num10 > num9 + height)) ? (num10 - num9) : height);
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, height);
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
				return;
			}
		}

		private void MovePageDown(bool shift)
		{
			MovePageDown(shift, true);
		}

		private void MovePageDown(bool shift, bool goToLastChar)
		{
			if (hasSelection)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!shift)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					int num3 = caretPositionInternal = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
				}
			}
			int num4;
			if (multiLine)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num4 = PageDownCharacterPosition(caretSelectPositionInternal, goToLastChar);
			}
			else
			{
				num4 = m_TextComponent.textInfo.characterCount - 1;
			}
			int num5 = num4;
			if (!shift)
			{
				int num3 = caretSelectPositionInternal = (caretPositionInternal = num5);
				num3 = (stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal)));
			}
			else
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				caretSelectPositionInternal = num5;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			}
			if (m_LineType == LineType.SingleLine)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				float height = m_TextViewport.rect.height;
				Vector3 position = m_TextComponent.rectTransform.position;
				float y = position.y;
				Vector3 min = m_TextComponent.textBounds.min;
				float num9 = y + min.y;
				Vector3 position2 = m_TextViewport.position;
				float num10 = position2.y + m_TextViewport.rect.yMin;
				float num11;
				if (num10 > num9 + height)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num11 = height;
				}
				else
				{
					num11 = num10 - num9;
				}
				height = num11;
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, height);
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
				return;
			}
		}

		private void Delete()
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (stringPositionInternal == stringSelectPositionInternal)
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
			if (!m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_isSelectAll)
				{
					stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
					if (caretPositionInternal < caretSelectPositionInternal)
					{
						m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
						stringSelectPositionInternal = stringPositionInternal;
						caretSelectPositionInternal = caretPositionInternal;
					}
					else
					{
						m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
						stringPositionInternal = stringSelectPositionInternal;
						stringPositionInternal = stringSelectPositionInternal;
						caretPositionInternal = caretSelectPositionInternal;
					}
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (stringPositionInternal < stringSelectPositionInternal)
			{
				m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
				stringSelectPositionInternal = stringPositionInternal;
			}
			else
			{
				m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
				stringPositionInternal = stringSelectPositionInternal;
			}
			m_isSelectAll = false;
		}

		private void ForwardSpace()
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (hasSelection)
			{
				Delete();
				SendOnValueChangedAndUpdateLabel();
				return;
			}
			if (m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (stringPositionInternal < text.Length)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									m_Text = text.Remove(stringPositionInternal, 1);
									SendOnValueChangedAndUpdateLabel();
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (caretPositionInternal >= m_TextComponent.textInfo.characterCount - 1)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				int num2 = stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal));
				m_Text = text.Remove(stringPositionInternal, 1);
				SendOnValueChangedAndUpdateLabel();
				return;
			}
		}

		private void Backspace()
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (hasSelection)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Delete();
						SendOnValueChangedAndUpdateLabel();
						return;
					}
				}
			}
			if (m_isRichTextEditingAllowed)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (stringPositionInternal > 0)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_Text = text.Remove(stringPositionInternal - 1, 1);
									stringSelectPositionInternal = --stringPositionInternal;
									m_isLastKeyBackspace = true;
									SendOnValueChangedAndUpdateLabel();
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (caretPositionInternal > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_Text = text.Remove(GetStringIndexFromCaretPosition(caretPositionInternal - 1), 1);
				caretSelectPositionInternal = --caretPositionInternal;
				int num2 = stringSelectPositionInternal = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal));
			}
			m_isLastKeyBackspace = true;
			SendOnValueChangedAndUpdateLabel();
		}

		protected virtual void Append(string input)
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (!InPlaceEditing())
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
			int i = 0;
			for (int length = input.Length; i < length; i++)
			{
				char c = input[i];
				if (c < ' ')
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (c != '\t')
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (c != '\r' && c != '\n')
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (c != '\n')
							{
								continue;
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
				Append(c);
			}
		}

		protected virtual void Append(char input)
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (!InPlaceEditing())
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
			if (onValidateInput != null)
			{
				input = onValidateInput(text, stringPositionInternal, input);
			}
			else
			{
				if (characterValidation == CharacterValidation.CustomValidator)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						input = Validate(text, stringPositionInternal, input);
						if (input == '\0')
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
						SendOnValueChanged();
						UpdateLabel();
						return;
					}
				}
				if (characterValidation != 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					input = Validate(text, stringPositionInternal, input);
				}
			}
			if (input == '\0')
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
			Insert(input);
		}

		private void Insert(char c)
		{
			if (m_ReadOnly)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			string text = c.ToString();
			Delete();
			if (characterLimit > 0 && this.text.Length >= characterLimit)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			m_Text = this.text.Insert(m_StringPosition, text);
			stringSelectPositionInternal = (stringPositionInternal += text.Length);
			SendOnValueChanged();
		}

		private void SendOnValueChangedAndUpdateLabel()
		{
			SendOnValueChanged();
			UpdateLabel();
		}

		private void SendOnValueChanged()
		{
			if (onValueChanged == null)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				onValueChanged.Invoke(text);
				return;
			}
		}

		protected void SendOnEndEdit()
		{
			if (onEndEdit == null)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				onEndEdit.Invoke(m_Text);
				return;
			}
		}

		protected void SendOnSubmit()
		{
			if (onSubmit == null)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				onSubmit.Invoke(m_Text);
				return;
			}
		}

		protected void SendOnFocus()
		{
			if (onSelect != null)
			{
				onSelect.Invoke(m_Text);
			}
		}

		protected void SendOnFocusLost()
		{
			if (onDeselect != null)
			{
				onDeselect.Invoke(m_Text);
			}
		}

		protected void SendOnTextSelection()
		{
			m_isSelected = true;
			if (onTextSelection != null)
			{
				onTextSelection.Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
			}
		}

		protected void SendOnEndTextSelection()
		{
			if (!m_isSelected)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (onEndTextSelection != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				onEndTextSelection.Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
			}
			m_isSelected = false;
		}

		protected void UpdateLabel()
		{
			if (!(m_TextComponent != null) || !(m_TextComponent.font != null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				string text;
				if (Input.compositionString.Length > 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					text = this.text.Substring(0, m_StringPosition) + Input.compositionString + this.text.Substring(m_StringPosition);
				}
				else
				{
					text = this.text;
				}
				string str = (inputType != InputType.Password) ? text : new string(asteriskChar, text.Length);
				bool flag = string.IsNullOrEmpty(text);
				if (m_Placeholder != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_Placeholder.enabled = flag;
				}
				if (!flag)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					SetCaretVisible();
				}
				m_TextComponent.text = str + "\u200b";
				MarkGeometryAsDirty();
				m_IsScrollbarUpdateRequired = true;
				return;
			}
		}

		private void UpdateScrollbar()
		{
			if (!m_VerticalScrollbar)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				float size = m_TextViewport.rect.height / m_TextComponent.preferredHeight;
				m_IsUpdatingScrollbarValues = true;
				m_VerticalScrollbar.size = size;
				Scrollbar verticalScrollbar = m_VerticalScrollbar;
				Vector2 anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
				verticalScrollbar.value = anchoredPosition.y / (m_TextComponent.preferredHeight - m_TextViewport.rect.height);
				return;
			}
		}

		private void OnScrollbarValueChange(float value)
		{
			if (m_IsUpdatingScrollbarValues)
			{
				m_IsUpdatingScrollbarValues = false;
			}
			else
			{
				if (value < 0f)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (value > 1f)
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
					AdjustTextPositionRelativeToViewport(value);
					m_ScrollPosition = value;
					return;
				}
			}
		}

		private void AdjustTextPositionRelativeToViewport(float relativePosition)
		{
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			if (textInfo == null)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (textInfo.lineInfo == null)
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (textInfo.lineCount == 0)
					{
						return;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						if (textInfo.lineCount > textInfo.lineInfo.Length)
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
						RectTransform rectTransform = m_TextComponent.rectTransform;
						Vector2 anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
						rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, (m_TextComponent.preferredHeight - m_TextViewport.rect.height) * relativePosition);
						AssignPositioningIfNeeded();
						return;
					}
				}
			}
		}

		private int GetCaretPositionFromStringIndex(int stringIndex)
		{
			int characterCount = m_TextComponent.textInfo.characterCount;
			for (int i = 0; i < characterCount; i++)
			{
				if (m_TextComponent.textInfo.characterInfo[i].index < stringIndex)
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return i;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				return characterCount;
			}
		}

		private int GetStringIndexFromCaretPosition(int caretPosition)
		{
			ClampCaretPos(ref caretPosition);
			return m_TextComponent.textInfo.characterInfo[caretPosition].index;
		}

		public void ForceLabelUpdate()
		{
			UpdateLabel();
		}

		private void MarkGeometryAsDirty()
		{
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}

		public virtual void Rebuild(CanvasUpdate update)
		{
			if (update != CanvasUpdate.LatePreRender)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			UpdateGeometry();
		}

		public virtual void LayoutComplete()
		{
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		private void UpdateGeometry()
		{
			if (!shouldHideMobileInput)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (m_CachedInputRenderer == null)
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
			OnFillVBO(mesh);
			m_CachedInputRenderer.SetMesh(mesh);
		}

		private void AssignPositioningIfNeeded()
		{
			if (!(m_TextComponent != null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!(caretRectTrans != null))
				{
					return;
				}
				if (!(caretRectTrans.localPosition != m_TextComponent.rectTransform.localPosition))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(caretRectTrans.localRotation != m_TextComponent.rectTransform.localRotation))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(caretRectTrans.localScale != m_TextComponent.rectTransform.localScale))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!(caretRectTrans.anchorMin != m_TextComponent.rectTransform.anchorMin))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!(caretRectTrans.anchorMax != m_TextComponent.rectTransform.anchorMax) && !(caretRectTrans.anchoredPosition != m_TextComponent.rectTransform.anchoredPosition))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!(caretRectTrans.sizeDelta != m_TextComponent.rectTransform.sizeDelta))
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										if (!(caretRectTrans.pivot != m_TextComponent.rectTransform.pivot))
										{
											return;
										}
										while (true)
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
							}
						}
					}
				}
				caretRectTrans.localPosition = m_TextComponent.rectTransform.localPosition;
				caretRectTrans.localRotation = m_TextComponent.rectTransform.localRotation;
				caretRectTrans.localScale = m_TextComponent.rectTransform.localScale;
				caretRectTrans.anchorMin = m_TextComponent.rectTransform.anchorMin;
				caretRectTrans.anchorMax = m_TextComponent.rectTransform.anchorMax;
				caretRectTrans.anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
				caretRectTrans.sizeDelta = m_TextComponent.rectTransform.sizeDelta;
				caretRectTrans.pivot = m_TextComponent.rectTransform.pivot;
				return;
			}
		}

		private void OnFillVBO(Mesh vbo)
		{
			VertexHelper vertexHelper = new VertexHelper();
			try
			{
				if (!isFocused)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_ResetOnDeActivation)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								vertexHelper.FillMesh(vbo);
								return;
							}
						}
					}
				}
				if (isStringPositionDirty)
				{
					stringPositionInternal = GetStringIndexFromCaretPosition(m_CaretPosition);
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(m_CaretSelectPosition);
					isStringPositionDirty = false;
				}
				if (!hasSelection)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					GenerateCaret(vertexHelper, Vector2.zero);
					SendOnEndTextSelection();
				}
				else
				{
					GenerateHightlight(vertexHelper, Vector2.zero);
					SendOnTextSelection();
				}
				vertexHelper.FillMesh(vbo);
			}
			finally
			{
				if (vertexHelper != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							((IDisposable)vertexHelper).Dispose();
							goto end_IL_00bd;
						}
					}
				}
				end_IL_00bd:;
			}
		}

		private void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
		{
			if (!m_CaretVisible)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
			if (m_CursorVerts == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				CreateCursorVerts();
			}
			float num = m_CaretWidth;
			int characterCount = m_TextComponent.textInfo.characterCount;
			Vector2 vector = Vector2.zero;
			float num2 = 0f;
			caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
			TMP_CharacterInfo tMP_CharacterInfo;
			if (caretPositionInternal == 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[0];
				vector = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			else if (caretPositionInternal < characterCount)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[caretPositionInternal];
				vector = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			else
			{
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[characterCount - 1];
				vector = new Vector2(tMP_CharacterInfo.xAdvance, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			if (isFocused)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (vector != m_LastPosition)
				{
					goto IL_01a4;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_forceRectTransformAdjustment)
			{
				goto IL_01a4;
			}
			goto IL_01b3;
			IL_01a4:
			AdjustRectTransformRelativeToViewport(vector, num2, tMP_CharacterInfo.isVisible);
			goto IL_01b3;
			IL_01b3:
			m_LastPosition = vector;
			float num3 = vector.y + num2;
			float y = num3 - num2;
			m_CursorVerts[0].position = new Vector3(vector.x, y, 0f);
			m_CursorVerts[1].position = new Vector3(vector.x, num3, 0f);
			m_CursorVerts[2].position = new Vector3(vector.x + num, num3, 0f);
			m_CursorVerts[3].position = new Vector3(vector.x + num, y, 0f);
			m_CursorVerts[0].color = caretColor;
			m_CursorVerts[1].color = caretColor;
			m_CursorVerts[2].color = caretColor;
			m_CursorVerts[3].color = caretColor;
			vbo.AddUIVertexQuad(m_CursorVerts);
			int height = Screen.height;
			vector.y = (float)height - vector.y;
			Input.compositionCursorPos = vector;
		}

		private void CreateCursorVerts()
		{
			m_CursorVerts = new UIVertex[4];
			for (int i = 0; i < m_CursorVerts.Length; i++)
			{
				m_CursorVerts[i] = UIVertex.simpleVert;
				m_CursorVerts[i].uv0 = Vector2.zero;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}

		private void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
		{
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			caretPositionInternal = (m_CaretPosition = GetCaretPositionFromStringIndex(stringPositionInternal));
			caretSelectPositionInternal = (m_CaretSelectPosition = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
			float num = 0f;
			Vector2 startPosition;
			if (caretSelectPositionInternal < textInfo.characterCount)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal].origin, textInfo.characterInfo[caretSelectPositionInternal].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal].ascender - textInfo.characterInfo[caretSelectPositionInternal].descender;
			}
			else
			{
				startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal - 1].xAdvance, textInfo.characterInfo[caretSelectPositionInternal - 1].descender);
				num = textInfo.characterInfo[caretSelectPositionInternal - 1].ascender - textInfo.characterInfo[caretSelectPositionInternal - 1].descender;
			}
			AdjustRectTransformRelativeToViewport(startPosition, num, true);
			int num2 = Mathf.Max(0, caretPositionInternal);
			int num3 = Mathf.Max(0, caretSelectPositionInternal);
			if (num2 > num3)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				int num4 = num2;
				num2 = num3;
				num3 = num4;
			}
			num3--;
			int num5 = textInfo.characterInfo[num2].lineNumber;
			int lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.uv0 = Vector2.zero;
			simpleVert.color = selectionColor;
			for (int i = num2; i <= num3; i++)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (i >= textInfo.characterCount)
				{
					break;
				}
				if (i != lastCharacterIndex)
				{
					if (i != num3)
					{
						continue;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[num2];
				TMP_CharacterInfo tMP_CharacterInfo2 = textInfo.characterInfo[i];
				if (i > 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (tMP_CharacterInfo2.character == '\n' && textInfo.characterInfo[i - 1].character == '\r')
					{
						tMP_CharacterInfo2 = textInfo.characterInfo[i - 1];
					}
				}
				Vector2 vector = new Vector2(tMP_CharacterInfo.origin, textInfo.lineInfo[num5].ascender);
				Vector2 vector2 = new Vector2(tMP_CharacterInfo2.xAdvance, textInfo.lineInfo[num5].descender);
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
				num2 = i + 1;
				num5++;
				if (num5 < textInfo.lineCount)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
				}
			}
			m_IsScrollbarUpdateRequired = true;
		}

		private void AdjustRectTransformRelativeToViewport(Vector2 startPosition, float height, bool isCharVisible)
		{
			float xMin = m_TextViewport.rect.xMin;
			float xMax = m_TextViewport.rect.xMax;
			Vector2 anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
			float num = anchoredPosition.x + startPosition.x;
			Vector4 margin = m_TextComponent.margin;
			float num2 = xMax - (num + margin.z + (float)m_CaretWidth);
			if (num2 < 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!multiLine)
				{
					goto IL_00c6;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (multiLine)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (isCharVisible)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						goto IL_00c6;
					}
				}
			}
			goto IL_00f7;
			IL_00c6:
			m_TextComponent.rectTransform.anchoredPosition += new Vector2(num2, 0f);
			AssignPositioningIfNeeded();
			goto IL_00f7;
			IL_00f7:
			Vector2 anchoredPosition2 = m_TextComponent.rectTransform.anchoredPosition;
			float num3 = anchoredPosition2.x + startPosition.x;
			Vector4 margin2 = m_TextComponent.margin;
			float num4 = num3 - margin2.x - xMin;
			if (num4 < 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f - num4, 0f);
				AssignPositioningIfNeeded();
			}
			if (m_LineType != 0)
			{
				float yMax = m_TextViewport.rect.yMax;
				Vector2 anchoredPosition3 = m_TextComponent.rectTransform.anchoredPosition;
				float num5 = yMax - (anchoredPosition3.y + startPosition.y + height);
				if (num5 < -0.0001f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num5);
					AssignPositioningIfNeeded();
					m_IsScrollbarUpdateRequired = true;
				}
				Vector2 anchoredPosition4 = m_TextComponent.rectTransform.anchoredPosition;
				float num6 = anchoredPosition4.y + startPosition.y - m_TextViewport.rect.yMin;
				if (num6 < 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_TextComponent.rectTransform.anchoredPosition -= new Vector2(0f, num6);
					AssignPositioningIfNeeded();
					m_IsScrollbarUpdateRequired = true;
				}
			}
			if (m_isLastKeyBackspace)
			{
				Vector2 anchoredPosition5 = m_TextComponent.rectTransform.anchoredPosition;
				float num7 = anchoredPosition5.x + m_TextComponent.textInfo.characterInfo[0].origin;
				Vector4 margin3 = m_TextComponent.margin;
				float num8 = num7 - margin3.x;
				Vector2 anchoredPosition6 = m_TextComponent.rectTransform.anchoredPosition;
				float num9 = anchoredPosition6.x + m_TextComponent.textInfo.characterInfo[m_TextComponent.textInfo.characterCount - 1].origin;
				Vector4 margin4 = m_TextComponent.margin;
				float num10 = num9 + margin4.z;
				Vector2 anchoredPosition7 = m_TextComponent.rectTransform.anchoredPosition;
				if (anchoredPosition7.x + startPosition.x <= xMin + 0.0001f)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num8 < xMin)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						float x = Mathf.Min((xMax - xMin) / 2f, xMin - num8);
						m_TextComponent.rectTransform.anchoredPosition += new Vector2(x, 0f);
						AssignPositioningIfNeeded();
					}
				}
				else if (num10 < xMax)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num8 < xMin)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						float x2 = Mathf.Min(xMax - num10, xMin - num8);
						m_TextComponent.rectTransform.anchoredPosition += new Vector2(x2, 0f);
						AssignPositioningIfNeeded();
					}
				}
				m_isLastKeyBackspace = false;
			}
			m_forceRectTransformAdjustment = false;
		}

		protected char Validate(string text, int pos, char ch)
		{
			int num6;
			if (characterValidation != 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (base.enabled)
				{
					if (characterValidation != CharacterValidation.Integer)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (characterValidation != CharacterValidation.Decimal)
						{
							if (characterValidation == CharacterValidation.Digit)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (ch >= '0')
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= '9')
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
							}
							else if (characterValidation == CharacterValidation.Alphanumeric)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (ch >= 'A' && ch <= 'Z')
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											return ch;
										}
									}
								}
								if (ch >= 'a')
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= 'z')
									{
										return ch;
									}
								}
								if (ch >= '0')
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= '9')
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
							}
							else if (characterValidation == CharacterValidation.Name)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								int num;
								if (text.Length > 0)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									num = text[Mathf.Clamp(pos, 0, text.Length - 1)];
								}
								else
								{
									num = 32;
								}
								char c = (char)num;
								int num2;
								if (text.Length > 0)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									num2 = text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
								}
								else
								{
									num2 = 10;
								}
								char c2 = (char)num2;
								if (char.IsLetter(ch))
								{
									if (char.IsLower(ch))
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c == ' ')
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													return char.ToUpper(ch);
												}
											}
										}
									}
									if (char.IsUpper(ch))
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c != ' ' && c != '\'')
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													return char.ToLower(ch);
												}
											}
										}
									}
									return ch;
								}
								if (ch == '\'')
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (c != ' ')
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c != '\'')
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											if (c2 != '\'')
											{
												while (true)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
												if (!text.Contains("'"))
												{
													return ch;
												}
											}
										}
									}
								}
								else if (ch == ' ' && c != ' ')
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (c != '\'')
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c2 != ' ')
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												break;
											}
											if (c2 != '\'')
											{
												while (true)
												{
													switch (5)
													{
													case 0:
														break;
													default:
														return ch;
													}
												}
											}
										}
									}
								}
							}
							else if (characterValidation == CharacterValidation.EmailAddress)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (ch >= 'A')
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= 'Z')
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
								if (ch >= 'a')
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= 'z')
									{
										while (true)
										{
											switch (2)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
								if (ch >= '0')
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (ch <= '9')
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
								if (ch == '@')
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									if (text.IndexOf('@') == -1)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												return ch;
											}
										}
									}
								}
								if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										return ch;
									}
								}
								if (ch == '.')
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									char c3 = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
									char c4 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
									if (c3 != '.')
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										if (c4 != '.')
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													return ch;
												}
											}
										}
									}
								}
							}
							else if (characterValidation == CharacterValidation.Regex)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (Regex.IsMatch(ch.ToString(), m_RegexValue))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
											return ch;
										}
									}
								}
							}
							else if (characterValidation == CharacterValidation.CustomValidator)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (m_InputValidator != null)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
										{
											char result = m_InputValidator.Validate(ref text, ref pos, ch);
											m_Text = text;
											int num5 = stringSelectPositionInternal = (stringPositionInternal = pos);
											return result;
										}
										}
									}
								}
							}
							goto IL_053f;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (pos == 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (text.Length > 0)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							num6 = ((text[0] == '-') ? 1 : 0);
							goto IL_0090;
						}
					}
					num6 = 0;
					goto IL_0090;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return ch;
			IL_00f7:
			if (ch == '.')
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (characterValidation == CharacterValidation.Decimal)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!text.Contains("."))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return ch;
							}
						}
					}
				}
			}
			goto IL_053f;
			IL_0090:
			bool flag = (byte)num6 != 0;
			bool flag2 = stringPositionInternal == 0 || stringSelectPositionInternal == 0;
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ch >= '0')
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ch <= '9')
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return ch;
							}
						}
					}
				}
				if (ch == '-')
				{
					if (pos != 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!flag2)
						{
							goto IL_00f7;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					return ch;
				}
				goto IL_00f7;
			}
			goto IL_053f;
			IL_053f:
			return '\0';
		}

		public void ActivateInputField()
		{
			if (m_TextComponent == null)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_TextComponent.font == null)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (!IsActive())
					{
						return;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						if (!IsInteractable())
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
						if (isFocused)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_Keyboard != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!m_Keyboard.active)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									m_Keyboard.active = true;
									m_Keyboard.text = m_Text;
								}
							}
						}
						m_ShouldActivateNextUpdate = true;
						return;
					}
				}
			}
		}

		private void ActivateInputFieldInternal()
		{
			if (EventSystem.current == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			if (TouchScreenKeyboard.isSupported)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Input.touchSupported)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					TouchScreenKeyboard.hideInput = shouldHideMobileInput;
				}
				TouchScreenKeyboard keyboard;
				if (inputType == InputType.Password)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					keyboard = TouchScreenKeyboard.Open(m_Text, keyboardType, false, multiLine, true);
				}
				else
				{
					keyboard = TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine);
				}
				m_Keyboard = keyboard;
				MoveTextEnd(false);
			}
			else
			{
				Input.imeCompositionMode = IMECompositionMode.On;
				OnFocus();
			}
			m_AllowInput = true;
			m_OriginalText = text;
			m_WasCanceled = false;
			SetCaretVisible();
			UpdateLabel();
		}

		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			SendOnFocus();
			ActivateInputField();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				ActivateInputField();
			}
		}

		public void OnControlClick()
		{
		}

		public void DeactivateInputField()
		{
			if (!m_AllowInput)
			{
				return;
			}
			m_HasDoneFocusTransition = false;
			m_AllowInput = false;
			if (m_Placeholder != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_Placeholder.enabled = string.IsNullOrEmpty(m_Text);
			}
			if (m_TextComponent != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (IsInteractable())
				{
					if (m_WasCanceled)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_RestoreOriginalTextOnEscape)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							text = m_OriginalText;
						}
					}
					if (m_Keyboard != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						m_Keyboard.active = false;
						m_Keyboard = null;
					}
					if (m_ResetOnDeActivation)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						m_StringPosition = (m_StringSelectPosition = 0);
						m_CaretPosition = (m_CaretSelectPosition = 0);
						m_TextComponent.rectTransform.localPosition = m_DefaultTransformPosition;
						if (caretRectTrans != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							caretRectTrans.localPosition = Vector3.zero;
						}
					}
					SendOnEndEdit();
					SendOnEndTextSelection();
					Input.imeCompositionMode = IMECompositionMode.Auto;
				}
			}
			MarkGeometryAsDirty();
			m_IsScrollbarUpdateRequired = true;
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			DeactivateInputField();
			base.OnDeselect(eventData);
			SendOnFocusLost();
		}

		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (!IsActive())
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!IsInteractable())
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
				if (!isFocused)
				{
					m_ShouldActivateNextUpdate = true;
				}
				SendOnSubmit();
				return;
			}
		}

		private void EnforceContentType()
		{
			switch (contentType)
			{
			case ContentType.Standard:
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Autocorrected:
				m_InputType = InputType.AutoCorrect;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.IntegerNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Integer;
				break;
			case ContentType.DecimalNumber:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
				m_CharacterValidation = CharacterValidation.Decimal;
				break;
			case ContentType.Alphanumeric:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
				m_CharacterValidation = CharacterValidation.Alphanumeric;
				break;
			case ContentType.Name:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.Name;
				break;
			case ContentType.EmailAddress:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Standard;
				m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
				m_CharacterValidation = CharacterValidation.EmailAddress;
				break;
			case ContentType.Password:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.Default;
				m_CharacterValidation = CharacterValidation.None;
				break;
			case ContentType.Pin:
				m_LineType = LineType.SingleLine;
				m_TextComponent.enableWordWrapping = false;
				m_InputType = InputType.Password;
				m_KeyboardType = TouchScreenKeyboardType.NumberPad;
				m_CharacterValidation = CharacterValidation.Digit;
				break;
			}
		}

		private void SetTextComponentWrapMode()
		{
			if (m_TextComponent == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			if (m_LineType == LineType.SingleLine)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_TextComponent.enableWordWrapping = false;
						return;
					}
				}
			}
			m_TextComponent.enableWordWrapping = true;
		}

		private void SetTextComponentRichTextMode()
		{
			if (m_TextComponent == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			m_TextComponent.richText = m_RichText;
		}

		private void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
		{
			if (contentType == ContentType.Custom)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			for (int i = 0; i < allowedContentTypes.Length; i++)
			{
				if (contentType == allowedContentTypes[i])
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
			contentType = ContentType.Custom;
		}

		private void SetToCustom()
		{
			if (contentType == ContentType.Custom)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return;
					}
				}
			}
			contentType = ContentType.Custom;
		}

		private void SetToCustom(CharacterValidation characterValidation)
		{
			if (contentType == ContentType.Custom)
			{
				characterValidation = CharacterValidation.CustomValidator;
				return;
			}
			contentType = ContentType.Custom;
			characterValidation = CharacterValidation.CustomValidator;
		}

		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			if (m_HasDoneFocusTransition)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				state = SelectionState.Highlighted;
			}
			else if (state == SelectionState.Pressed)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_HasDoneFocusTransition = true;
			}
			base.DoStateTransition(state, instant);
		}

		public void SetGlobalPointSize(float pointSize)
		{
			TMP_Text tMP_Text = m_Placeholder as TMP_Text;
			if (tMP_Text != null)
			{
				tMP_Text.fontSize = pointSize;
			}
			textComponent.fontSize = pointSize;
		}

		public void SetGlobalFontAsset(TMP_FontAsset fontAsset)
		{
			TMP_Text tMP_Text = m_Placeholder as TMP_Text;
			if (tMP_Text != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				tMP_Text.font = fontAsset;
			}
			textComponent.font = fontAsset;
		}

		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		bool ICanvasElement.IsDestroyed()
		{
			return IsDestroyed();
		}
	}
}
