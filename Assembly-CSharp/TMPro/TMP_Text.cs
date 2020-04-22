using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro
{
	public class TMP_Text : MaskableGraphic
	{
		protected enum TextInputSources
		{
			Text,
			SetText,
			SetCharArray,
			String
		}

		[SerializeField]
		protected string m_text;

		[SerializeField]
		protected bool m_isRightToLeft;

		[SerializeField]
		protected TMP_FontAsset m_fontAsset;

		protected TMP_FontAsset m_currentFontAsset;

		protected bool m_isSDFShader;

		[SerializeField]
		protected Material m_sharedMaterial;

		protected Material m_currentMaterial;

		protected MaterialReference[] m_materialReferences = new MaterialReference[32];

		protected Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[16]);

		protected int m_currentMaterialIndex;

		[SerializeField]
		protected Material[] m_fontSharedMaterials;

		[SerializeField]
		protected Material m_fontMaterial;

		[SerializeField]
		protected Material[] m_fontMaterials;

		protected bool m_isMaterialDirty;

		[SerializeField]
		protected Color32 m_fontColor32 = Color.white;

		[SerializeField]
		protected Color m_fontColor = Color.white;

		protected static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		protected Color32 m_underlineColor = s_colorWhite;

		protected Color32 m_strikethroughColor = s_colorWhite;

		protected Color32 m_highlightColor = s_colorWhite;

		[SerializeField]
		protected bool m_enableVertexGradient;

		[SerializeField]
		protected VertexGradient m_fontColorGradient = new VertexGradient(Color.white);

		[SerializeField]
		protected TMP_ColorGradient m_fontColorGradientPreset;

		[SerializeField]
		protected TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		protected bool m_tintAllSprites;

		protected bool m_tintSprite;

		protected Color32 m_spriteColor;

		[SerializeField]
		protected bool m_overrideHtmlColors;

		[SerializeField]
		protected Color32 m_faceColor = Color.white;

		[SerializeField]
		protected Color32 m_outlineColor = Color.black;

		protected float m_outlineWidth;

		[SerializeField]
		protected float m_fontSize = 36f;

		protected float m_currentFontSize;

		[SerializeField]
		protected float m_fontSizeBase = 36f;

		protected TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[16]);

		[SerializeField]
		protected int m_fontWeight = 400;

		protected int m_fontWeightInternal;

		protected TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[16]);

		[SerializeField]
		protected bool m_enableAutoSizing;

		protected float m_maxFontSize;

		protected float m_minFontSize;

		[SerializeField]
		protected float m_fontSizeMin;

		[SerializeField]
		protected float m_fontSizeMax;

		[SerializeField]
		protected FontStyles m_fontStyle;

		protected FontStyles m_style;

		protected TMP_BasicXmlTagStack m_fontStyleStack;

		protected bool m_isUsingBold;

		[FormerlySerializedAs("m_lineJustification")]
		[SerializeField]
		protected TextAlignmentOptions m_textAlignment = TextAlignmentOptions.TopLeft;

		protected TextAlignmentOptions m_lineJustification;

		protected TMP_XmlTagStack<TextAlignmentOptions> m_lineJustificationStack = new TMP_XmlTagStack<TextAlignmentOptions>(new TextAlignmentOptions[16]);

		protected Vector3[] m_textContainerLocalCorners = new Vector3[4];

		[SerializeField]
		protected bool m_isAlignmentEnumConverted;

		[SerializeField]
		protected float m_characterSpacing;

		protected float m_cSpacing;

		protected float m_monoSpacing;

		[SerializeField]
		protected float m_wordSpacing;

		[SerializeField]
		protected float m_lineSpacing;

		protected float m_lineSpacingDelta;

		protected float m_lineHeight = -32767f;

		[SerializeField]
		protected float m_lineSpacingMax;

		[SerializeField]
		protected float m_paragraphSpacing;

		[SerializeField]
		protected float m_charWidthMaxAdj;

		protected float m_charWidthAdjDelta;

		[SerializeField]
		protected bool m_enableWordWrapping;

		protected bool m_isCharacterWrappingEnabled;

		protected bool m_isNonBreakingSpace;

		protected bool m_isIgnoringAlignment;

		[SerializeField]
		protected float m_wordWrappingRatios = 0.4f;

		[SerializeField]
		protected TextOverflowModes m_overflowMode;

		[SerializeField]
		protected int m_firstOverflowCharacterIndex = -1;

		[SerializeField]
		protected TMP_Text m_linkedTextComponent;

		[SerializeField]
		protected bool m_isLinkedTextComponent;

		protected bool m_isTextTruncated;

		[SerializeField]
		protected bool m_enableKerning;

		[SerializeField]
		protected bool m_enableExtraPadding;

		[SerializeField]
		protected bool checkPaddingRequired;

		[SerializeField]
		protected bool m_isRichText = true;

		[SerializeField]
		protected bool m_parseCtrlCharacters = true;

		protected bool m_isOverlay;

		[SerializeField]
		protected bool m_isOrthographic;

		[SerializeField]
		protected bool m_isCullingEnabled;

		[SerializeField]
		protected bool m_ignoreRectMaskCulling;

		[SerializeField]
		protected bool m_ignoreCulling = true;

		[SerializeField]
		protected TextureMappingOptions m_horizontalMapping;

		[SerializeField]
		protected TextureMappingOptions m_verticalMapping;

		[SerializeField]
		protected float m_uvLineOffset;

		protected TextRenderFlags m_renderMode = TextRenderFlags.Render;

		[SerializeField]
		protected VertexSortingOrder m_geometrySortingOrder;

		[SerializeField]
		protected int m_firstVisibleCharacter;

		protected int m_maxVisibleCharacters = 99999;

		protected int m_maxVisibleWords = 99999;

		protected int m_maxVisibleLines = 99999;

		[SerializeField]
		protected bool m_useMaxVisibleDescender = true;

		[SerializeField]
		protected int m_pageToDisplay = 1;

		protected bool m_isNewPage;

		[SerializeField]
		protected Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

		protected float m_marginLeft;

		protected float m_marginRight;

		protected float m_marginWidth;

		protected float m_marginHeight;

		protected float m_width = -1f;

		[SerializeField]
		protected TMP_TextInfo m_textInfo;

		[SerializeField]
		protected bool m_havePropertiesChanged;

		[SerializeField]
		protected bool m_isUsingLegacyAnimationComponent;

		protected Transform m_transform;

		protected RectTransform m_rectTransform;

		protected bool m_autoSizeTextContainer;

		protected Mesh m_mesh;

		[SerializeField]
		protected bool m_isVolumetricText;

		[SerializeField]
		protected TMP_SpriteAnimator m_spriteAnimator;

		protected float m_flexibleHeight = -1f;

		protected float m_flexibleWidth = -1f;

		protected float m_minWidth;

		protected float m_minHeight;

		protected float m_maxWidth;

		protected float m_maxHeight;

		protected LayoutElement m_LayoutElement;

		protected float m_preferredWidth;

		protected float m_renderedWidth;

		protected bool m_isPreferredWidthDirty;

		protected float m_preferredHeight;

		protected float m_renderedHeight;

		protected bool m_isPreferredHeightDirty;

		protected bool m_isCalculatingPreferredValues;

		private int m_recursiveCount;

		protected int m_layoutPriority;

		protected bool m_isCalculateSizeRequired;

		protected bool m_isLayoutDirty;

		protected bool m_verticesAlreadyDirty;

		protected bool m_layoutAlreadyDirty;

		protected bool m_isAwake;

		[SerializeField]
		protected bool m_isInputParsingRequired;

		[SerializeField]
		protected TextInputSources m_inputSource;

		protected string old_text;

		protected float m_fontScale;

		protected float m_fontScaleMultiplier;

		protected char[] m_htmlTag = new char[128];

		protected XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		protected float[] m_attributeParameterValues = new float[16];

		protected float tag_LineIndent;

		protected float tag_Indent;

		protected TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[16]);

		protected bool tag_NoParsing;

		protected bool m_isParsingText;

		protected Matrix4x4 m_FXMatrix;

		protected bool m_isFXMatrixSet;

		protected int[] m_char_buffer;

		private TMP_CharacterInfo[] m_internalCharacterInfo;

		protected char[] m_input_CharArray = new char[256];

		private int m_charArray_Length;

		protected int m_totalCharacterCount;

		protected WordWrapState m_SavedWordWrapState = default(WordWrapState);

		protected WordWrapState m_SavedLineState = default(WordWrapState);

		protected int m_characterCount;

		protected int m_firstCharacterOfLine;

		protected int m_firstVisibleCharacterOfLine;

		protected int m_lastCharacterOfLine;

		protected int m_lastVisibleCharacterOfLine;

		protected int m_lineNumber;

		protected int m_lineVisibleCharacterCount;

		protected int m_pageNumber;

		protected float m_maxAscender;

		protected float m_maxCapHeight;

		protected float m_maxDescender;

		protected float m_maxLineAscender;

		protected float m_maxLineDescender;

		protected float m_startOfLineAscender;

		protected float m_lineOffset;

		protected Extents m_meshExtents;

		protected Color32 m_htmlColor = new Color(255f, 255f, 255f, 128f);

		protected TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_underlineColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_strikethroughColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected TMP_XmlTagStack<Color32> m_highlightColorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		protected float m_tabSpacing;

		protected float m_spacing;

		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		protected float m_padding;

		protected float m_baselineOffset;

		protected TMP_XmlTagStack<float> m_baselineOffsetStack = new TMP_XmlTagStack<float>(new float[16]);

		protected float m_xAdvance;

		protected TMP_TextElementType m_textElementType;

		protected TMP_TextElement m_cached_TextElement;

		protected TMP_Glyph m_cached_Underline_GlyphInfo;

		protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		protected TMP_SpriteAsset m_defaultSpriteAsset;

		protected TMP_SpriteAsset m_currentSpriteAsset;

		protected int m_spriteCount;

		protected int m_spriteIndex;

		protected InlineGraphicManager m_inlineGraphics;

		protected int m_spriteAnimationID;

		protected bool m_ignoreActiveState;

		private readonly float[] k_Power = new float[10]
		{
			0.5f,
			0.05f,
			0.005f,
			0.0005f,
			5E-05f,
			5E-06f,
			5E-07f,
			5E-08f,
			5E-09f,
			5E-10f
		};

		protected static Vector2 k_LargePositiveVector2 = new Vector2(2.14748365E+09f, 2.14748365E+09f);

		protected static Vector2 k_LargeNegativeVector2 = new Vector2(-2.14748365E+09f, -2.14748365E+09f);

		protected static float k_LargePositiveFloat = 32767f;

		protected static float k_LargeNegativeFloat = -32767f;

		protected static int k_LargePositiveInt = int.MaxValue;

		protected static int k_LargeNegativeInt = -2147483647;

		public string text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (m_text == value)
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
				m_text = (old_text = value);
				m_inputSource = TextInputSources.String;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool isRightToLeftText
		{
			get
			{
				return m_isRightToLeft;
			}
			set
			{
				if (m_isRightToLeft == value)
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
				m_isRightToLeft = value;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public TMP_FontAsset font
		{
			get
			{
				return m_fontAsset;
			}
			set
			{
				if (!(m_fontAsset == value))
				{
					m_fontAsset = value;
					LoadFontAsset();
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					m_isInputParsingRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public virtual Material fontSharedMaterial
		{
			get
			{
				return m_sharedMaterial;
			}
			set
			{
				if (m_sharedMaterial == value)
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
				SetSharedMaterial(value);
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return GetSharedMaterials();
			}
			set
			{
				SetSharedMaterials(value);
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public Material fontMaterial
		{
			get
			{
				return GetMaterial(m_sharedMaterial);
			}
			set
			{
				if (m_sharedMaterial != null)
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
					if (m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
					{
						while (true)
						{
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
				m_sharedMaterial = value;
				m_padding = GetPaddingForMaterial();
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public virtual Material[] fontMaterials
		{
			get
			{
				return GetMaterials(m_fontSharedMaterials);
			}
			set
			{
				SetSharedMaterials(value);
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public override Color color
		{
			get
			{
				return m_fontColor;
			}
			set
			{
				if (m_fontColor == value)
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
				m_havePropertiesChanged = true;
				m_fontColor = value;
				SetVerticesDirty();
			}
		}

		public float alpha
		{
			get
			{
				return m_fontColor.a;
			}
			set
			{
				if (m_fontColor.a != value)
				{
					m_fontColor.a = value;
					m_havePropertiesChanged = true;
					SetVerticesDirty();
				}
			}
		}

		public bool enableVertexGradient
		{
			get
			{
				return m_enableVertexGradient;
			}
			set
			{
				if (m_enableVertexGradient == value)
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
				m_havePropertiesChanged = true;
				m_enableVertexGradient = value;
				SetVerticesDirty();
			}
		}

		public VertexGradient colorGradient
		{
			get
			{
				return m_fontColorGradient;
			}
			set
			{
				m_havePropertiesChanged = true;
				m_fontColorGradient = value;
				SetVerticesDirty();
			}
		}

		public TMP_ColorGradient colorGradientPreset
		{
			get
			{
				return m_fontColorGradientPreset;
			}
			set
			{
				m_havePropertiesChanged = true;
				m_fontColorGradientPreset = value;
				SetVerticesDirty();
			}
		}

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return m_spriteAsset;
			}
			set
			{
				m_spriteAsset = value;
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool tintAllSprites
		{
			get
			{
				return m_tintAllSprites;
			}
			set
			{
				if (m_tintAllSprites == value)
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
				m_tintAllSprites = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}

		public bool overrideColorTags
		{
			get
			{
				return m_overrideHtmlColors;
			}
			set
			{
				if (m_overrideHtmlColors == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_overrideHtmlColors = value;
				SetVerticesDirty();
			}
		}

		public Color32 faceColor
		{
			get
			{
				if (m_sharedMaterial == null)
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
							return m_faceColor;
						}
					}
				}
				m_faceColor = m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
				return m_faceColor;
			}
			set
			{
				if (m_faceColor.Compare(value))
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
							return;
						}
					}
				}
				SetFaceColor(value);
				m_havePropertiesChanged = true;
				m_faceColor = value;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public Color32 outlineColor
		{
			get
			{
				if (m_sharedMaterial == null)
				{
					return m_outlineColor;
				}
				m_outlineColor = m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
				return m_outlineColor;
			}
			set
			{
				if (m_outlineColor.Compare(value))
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
				SetOutlineColor(value);
				m_havePropertiesChanged = true;
				m_outlineColor = value;
				SetVerticesDirty();
			}
		}

		public float outlineWidth
		{
			get
			{
				if (m_sharedMaterial == null)
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
							return m_outlineWidth;
						}
					}
				}
				m_outlineWidth = m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return m_outlineWidth;
			}
			set
			{
				if (m_outlineWidth != value)
				{
					SetOutlineThickness(value);
					m_havePropertiesChanged = true;
					m_outlineWidth = value;
					SetVerticesDirty();
				}
			}
		}

		public float fontSize
		{
			get
			{
				return m_fontSize;
			}
			set
			{
				if (m_fontSize == value)
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
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_fontSize = value;
				if (!m_enableAutoSizing)
				{
					m_fontSizeBase = m_fontSize;
				}
			}
		}

		public float fontScale => m_fontScale;

		public int fontWeight
		{
			get
			{
				return m_fontWeight;
			}
			set
			{
				if (m_fontWeight == value)
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
				m_fontWeight = value;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
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
							return 1f;
						}
					}
				}
				if (!font)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return canvas.scaleFactor;
						}
					}
				}
				if (!(m_currentFontAsset == null))
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
					if (!(m_currentFontAsset.fontInfo.PointSize <= 0f))
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
						if (!(m_fontSize <= 0f))
						{
							return m_fontSize / m_currentFontAsset.fontInfo.PointSize;
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
				}
				return 1f;
			}
		}

		public bool enableAutoSizing
		{
			get
			{
				return m_enableAutoSizing;
			}
			set
			{
				if (m_enableAutoSizing == value)
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
				m_enableAutoSizing = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public float fontSizeMin
		{
			get
			{
				return m_fontSizeMin;
			}
			set
			{
				if (m_fontSizeMin != value)
				{
					m_fontSizeMin = value;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public float fontSizeMax
		{
			get
			{
				return m_fontSizeMax;
			}
			set
			{
				if (m_fontSizeMax == value)
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
				m_fontSizeMax = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public FontStyles fontStyle
		{
			get
			{
				return m_fontStyle;
			}
			set
			{
				if (m_fontStyle == value)
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
				m_fontStyle = value;
				m_havePropertiesChanged = true;
				checkPaddingRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool isUsingBold => m_isUsingBold;

		public TextAlignmentOptions alignment
		{
			get
			{
				return m_textAlignment;
			}
			set
			{
				if (m_textAlignment == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_textAlignment = value;
				SetVerticesDirty();
			}
		}

		public float characterSpacing
		{
			get
			{
				return m_characterSpacing;
			}
			set
			{
				if (m_characterSpacing == value)
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
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_characterSpacing = value;
			}
		}

		public float wordSpacing
		{
			get
			{
				return m_wordSpacing;
			}
			set
			{
				if (m_wordSpacing == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_wordSpacing = value;
			}
		}

		public float lineSpacing
		{
			get
			{
				return m_lineSpacing;
			}
			set
			{
				if (m_lineSpacing != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
					m_lineSpacing = value;
				}
			}
		}

		public float lineSpacingAdjustment
		{
			get
			{
				return m_lineSpacingMax;
			}
			set
			{
				if (m_lineSpacingMax == value)
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
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_lineSpacingMax = value;
			}
		}

		public float paragraphSpacing
		{
			get
			{
				return m_paragraphSpacing;
			}
			set
			{
				if (m_paragraphSpacing == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_paragraphSpacing = value;
			}
		}

		public float characterWidthAdjustment
		{
			get
			{
				return m_charWidthMaxAdj;
			}
			set
			{
				if (m_charWidthMaxAdj != value)
				{
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
					m_charWidthMaxAdj = value;
				}
			}
		}

		public bool enableWordWrapping
		{
			get
			{
				return m_enableWordWrapping;
			}
			set
			{
				if (m_enableWordWrapping == value)
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
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				m_isCalculateSizeRequired = true;
				m_enableWordWrapping = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public float wordWrappingRatios
		{
			get
			{
				return m_wordWrappingRatios;
			}
			set
			{
				if (m_wordWrappingRatios == value)
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
				m_wordWrappingRatios = value;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public TextOverflowModes overflowMode
		{
			get
			{
				return m_overflowMode;
			}
			set
			{
				if (m_overflowMode == value)
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
				m_overflowMode = value;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool isTextOverflowing
		{
			get
			{
				if (m_firstOverflowCharacterIndex != -1)
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
							return true;
						}
					}
				}
				return false;
			}
		}

		public int firstOverflowCharacterIndex => m_firstOverflowCharacterIndex;

		public TMP_Text linkedTextComponent
		{
			get
			{
				return m_linkedTextComponent;
			}
			set
			{
				if (m_linkedTextComponent != value)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (m_linkedTextComponent != null)
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
						m_linkedTextComponent.overflowMode = TextOverflowModes.Overflow;
						m_linkedTextComponent.linkedTextComponent = null;
						m_linkedTextComponent.isLinkedTextComponent = false;
					}
					m_linkedTextComponent = value;
					if (m_linkedTextComponent != null)
					{
						m_linkedTextComponent.isLinkedTextComponent = true;
					}
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool isLinkedTextComponent
		{
			get
			{
				return m_isLinkedTextComponent;
			}
			set
			{
				m_isLinkedTextComponent = value;
				if (!m_isLinkedTextComponent)
				{
					m_firstVisibleCharacter = 0;
				}
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		public bool isTextTruncated => m_isTextTruncated;

		public bool enableKerning
		{
			get
			{
				return m_enableKerning;
			}
			set
			{
				if (m_enableKerning == value)
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
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_enableKerning = value;
			}
		}

		public bool extraPadding
		{
			get
			{
				return m_enableExtraPadding;
			}
			set
			{
				if (m_enableExtraPadding == value)
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
				m_havePropertiesChanged = true;
				m_enableExtraPadding = value;
				UpdateMeshPadding();
				SetVerticesDirty();
			}
		}

		public bool richText
		{
			get
			{
				return m_isRichText;
			}
			set
			{
				if (m_isRichText != value)
				{
					m_isRichText = value;
					m_havePropertiesChanged = true;
					m_isCalculateSizeRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
					m_isInputParsingRequired = true;
				}
			}
		}

		public bool parseCtrlCharacters
		{
			get
			{
				return m_parseCtrlCharacters;
			}
			set
			{
				if (m_parseCtrlCharacters == value)
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
							return;
						}
					}
				}
				m_parseCtrlCharacters = value;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				m_isInputParsingRequired = true;
			}
		}

		public bool isOverlay
		{
			get
			{
				return m_isOverlay;
			}
			set
			{
				if (m_isOverlay != value)
				{
					m_isOverlay = value;
					SetShaderDepth();
					m_havePropertiesChanged = true;
					SetVerticesDirty();
				}
			}
		}

		public bool isOrthographic
		{
			get
			{
				return m_isOrthographic;
			}
			set
			{
				if (m_isOrthographic == value)
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
				m_havePropertiesChanged = true;
				m_isOrthographic = value;
				SetVerticesDirty();
			}
		}

		public bool enableCulling
		{
			get
			{
				return m_isCullingEnabled;
			}
			set
			{
				if (m_isCullingEnabled == value)
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
				m_isCullingEnabled = value;
				SetCulling();
				m_havePropertiesChanged = true;
			}
		}

		public bool ignoreRectMaskCulling
		{
			get
			{
				return m_ignoreRectMaskCulling;
			}
			set
			{
				if (m_ignoreRectMaskCulling == value)
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
				m_ignoreRectMaskCulling = value;
				m_havePropertiesChanged = true;
			}
		}

		public bool ignoreVisibility
		{
			get
			{
				return m_ignoreCulling;
			}
			set
			{
				if (m_ignoreCulling != value)
				{
					m_havePropertiesChanged = true;
					m_ignoreCulling = value;
				}
			}
		}

		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return m_horizontalMapping;
			}
			set
			{
				if (m_horizontalMapping == value)
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
				m_havePropertiesChanged = true;
				m_horizontalMapping = value;
				SetVerticesDirty();
			}
		}

		public TextureMappingOptions verticalMapping
		{
			get
			{
				return m_verticalMapping;
			}
			set
			{
				if (m_verticalMapping == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_verticalMapping = value;
				SetVerticesDirty();
			}
		}

		public float mappingUvLineOffset
		{
			get
			{
				return m_uvLineOffset;
			}
			set
			{
				if (m_uvLineOffset != value)
				{
					m_havePropertiesChanged = true;
					m_uvLineOffset = value;
					SetVerticesDirty();
				}
			}
		}

		public TextRenderFlags renderMode
		{
			get
			{
				return m_renderMode;
			}
			set
			{
				if (m_renderMode == value)
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
				m_renderMode = value;
				m_havePropertiesChanged = true;
			}
		}

		public VertexSortingOrder geometrySortingOrder
		{
			get
			{
				return m_geometrySortingOrder;
			}
			set
			{
				m_geometrySortingOrder = value;
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}

		public int firstVisibleCharacter
		{
			get
			{
				return m_firstVisibleCharacter;
			}
			set
			{
				if (m_firstVisibleCharacter == value)
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
				m_havePropertiesChanged = true;
				m_firstVisibleCharacter = value;
				SetVerticesDirty();
			}
		}

		public int maxVisibleCharacters
		{
			get
			{
				return m_maxVisibleCharacters;
			}
			set
			{
				if (m_maxVisibleCharacters == value)
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
				m_havePropertiesChanged = true;
				m_maxVisibleCharacters = value;
				SetVerticesDirty();
			}
		}

		public int maxVisibleWords
		{
			get
			{
				return m_maxVisibleWords;
			}
			set
			{
				if (m_maxVisibleWords == value)
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
				m_havePropertiesChanged = true;
				m_maxVisibleWords = value;
				SetVerticesDirty();
			}
		}

		public int maxVisibleLines
		{
			get
			{
				return m_maxVisibleLines;
			}
			set
			{
				if (m_maxVisibleLines != value)
				{
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					m_maxVisibleLines = value;
					SetVerticesDirty();
				}
			}
		}

		public bool useMaxVisibleDescender
		{
			get
			{
				return m_useMaxVisibleDescender;
			}
			set
			{
				if (m_useMaxVisibleDescender == value)
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
							return;
						}
					}
				}
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				SetVerticesDirty();
			}
		}

		public int pageToDisplay
		{
			get
			{
				return m_pageToDisplay;
			}
			set
			{
				if (m_pageToDisplay == value)
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
				m_havePropertiesChanged = true;
				m_pageToDisplay = value;
				SetVerticesDirty();
			}
		}

		public virtual Vector4 margin
		{
			get
			{
				return m_margin;
			}
			set
			{
				if (m_margin == value)
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
				m_margin = value;
				ComputeMarginSize();
				m_havePropertiesChanged = true;
				SetVerticesDirty();
			}
		}

		public TMP_TextInfo textInfo => m_textInfo;

		public bool havePropertiesChanged
		{
			get
			{
				return m_havePropertiesChanged;
			}
			set
			{
				if (m_havePropertiesChanged != value)
				{
					m_havePropertiesChanged = value;
					m_isInputParsingRequired = true;
					SetAllDirty();
				}
			}
		}

		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return m_isUsingLegacyAnimationComponent;
			}
			set
			{
				m_isUsingLegacyAnimationComponent = value;
			}
		}

		public new Transform transform
		{
			get
			{
				if (m_transform == null)
				{
					m_transform = GetComponent<Transform>();
				}
				return m_transform;
			}
		}

		public new RectTransform rectTransform
		{
			get
			{
				if (m_rectTransform == null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_rectTransform = GetComponent<RectTransform>();
				}
				return m_rectTransform;
			}
		}

		public virtual bool autoSizeTextContainer
		{
			get;
			set;
		}

		public virtual Mesh mesh => m_mesh;

		public bool isVolumetricText
		{
			get
			{
				return m_isVolumetricText;
			}
			set
			{
				if (m_isVolumetricText != value)
				{
					m_havePropertiesChanged = value;
					m_textInfo.ResetVertexLayout(value);
					m_isInputParsingRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
				}
			}
		}

		public Bounds bounds
		{
			get
			{
				if (m_mesh == null)
				{
					return default(Bounds);
				}
				return GetCompoundBounds();
			}
		}

		public Bounds textBounds
		{
			get
			{
				if (m_textInfo == null)
				{
					return default(Bounds);
				}
				return GetTextBounds();
			}
		}

		protected TMP_SpriteAnimator spriteAnimator
		{
			get
			{
				if (m_spriteAnimator == null)
				{
					m_spriteAnimator = GetComponent<TMP_SpriteAnimator>();
					if (m_spriteAnimator == null)
					{
						m_spriteAnimator = base.gameObject.AddComponent<TMP_SpriteAnimator>();
					}
				}
				return m_spriteAnimator;
			}
		}

		public float flexibleHeight => m_flexibleHeight;

		public float flexibleWidth => m_flexibleWidth;

		public float minWidth => m_minWidth;

		public float minHeight => m_minHeight;

		public float maxWidth => m_maxWidth;

		public float maxHeight => m_maxHeight;

		protected LayoutElement layoutElement
		{
			get
			{
				if (m_LayoutElement == null)
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
					m_LayoutElement = GetComponent<LayoutElement>();
				}
				return m_LayoutElement;
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				if (!m_isPreferredWidthDirty)
				{
					return m_preferredWidth;
				}
				m_preferredWidth = GetPreferredWidth();
				return m_preferredWidth;
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				if (!m_isPreferredHeightDirty)
				{
					return m_preferredHeight;
				}
				m_preferredHeight = GetPreferredHeight();
				return m_preferredHeight;
			}
		}

		public virtual float renderedWidth => GetRenderedWidth();

		public virtual float renderedHeight => GetRenderedHeight();

		public int layoutPriority => m_layoutPriority;

		protected virtual void LoadFontAsset()
		{
		}

		protected virtual void SetSharedMaterial(Material mat)
		{
		}

		protected virtual Material GetMaterial(Material mat)
		{
			return null;
		}

		protected virtual void SetFontBaseMaterial(Material mat)
		{
		}

		protected virtual Material[] GetSharedMaterials()
		{
			return null;
		}

		protected virtual void SetSharedMaterials(Material[] materials)
		{
		}

		protected virtual Material[] GetMaterials(Material[] mats)
		{
			return null;
		}

		protected virtual Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			material.name += " (Instance)";
			return material;
		}

		protected void SetVertexColorGradient(TMP_ColorGradient gradient)
		{
			if (gradient == null)
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
			m_fontColorGradient.bottomLeft = gradient.bottomLeft;
			m_fontColorGradient.bottomRight = gradient.bottomRight;
			m_fontColorGradient.topLeft = gradient.topLeft;
			m_fontColorGradient.topRight = gradient.topRight;
			SetVerticesDirty();
		}

		protected void SetTextSortingOrder(VertexSortingOrder order)
		{
		}

		protected void SetTextSortingOrder(int[] order)
		{
		}

		protected virtual void SetFaceColor(Color32 color)
		{
		}

		protected virtual void SetOutlineColor(Color32 color)
		{
		}

		protected virtual void SetOutlineThickness(float thickness)
		{
		}

		protected virtual void SetShaderDepth()
		{
		}

		protected virtual void SetCulling()
		{
		}

		protected virtual float GetPaddingForMaterial()
		{
			return 0f;
		}

		protected virtual float GetPaddingForMaterial(Material mat)
		{
			return 0f;
		}

		protected virtual Vector3[] GetTextContainerLocalCorners()
		{
			return null;
		}

		public virtual void ForceMeshUpdate()
		{
		}

		public virtual void ForceMeshUpdate(bool ignoreActiveState)
		{
		}

		internal void SetTextInternal(string text)
		{
			m_text = text;
			m_renderMode = TextRenderFlags.DontRender;
			m_isInputParsingRequired = true;
			ForceMeshUpdate();
			m_renderMode = TextRenderFlags.Render;
		}

		public virtual void UpdateGeometry(Mesh mesh, int index)
		{
		}

		public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
		}

		public virtual void UpdateVertexData()
		{
		}

		public virtual void SetVertices(Vector3[] vertices)
		{
		}

		public virtual void UpdateMeshPadding()
		{
		}

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			base.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}

		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			base.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			InternalCrossFadeAlpha(alpha, duration, ignoreTimeScale);
		}

		protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
		}

		protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
		}

		protected void ParseInputText()
		{
			m_isInputParsingRequired = false;
			switch (m_inputSource)
			{
			case TextInputSources.Text:
			case TextInputSources.String:
				StringToCharArray(m_text, ref m_char_buffer);
				break;
			case TextInputSources.SetText:
				SetTextArrayToCharArray(m_input_CharArray, ref m_char_buffer);
				break;
			}
			SetArraySizes(m_char_buffer);
		}

		public void SetText(string text)
		{
			SetText(text, true);
		}

		public void SetText(string text, bool syncTextInputBox)
		{
			m_inputSource = TextInputSources.SetCharArray;
			StringToCharArray(text, ref m_char_buffer);
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}

		public void SetText(string text, float arg0)
		{
			SetText(text, arg0, 255f, 255f);
		}

		public void SetText(string text, float arg0, float arg1)
		{
			SetText(text, arg0, arg1, 255f);
		}

		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			int precision = 0;
			int index = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
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
					if (text[i + 2] == ':')
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
						precision = text[i + 3] - 48;
					}
					int num = text[i + 1] - 48;
					if (num != 0)
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
						switch (num)
						{
						case 1:
							AddFloatToCharArray(arg1, ref index, precision);
							break;
						case 2:
							AddFloatToCharArray(arg2, ref index, precision);
							break;
						}
					}
					else
					{
						AddFloatToCharArray(arg0, ref index, precision);
					}
					if (text[i + 2] == ':')
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
						i += 4;
					}
					else
					{
						i += 2;
					}
				}
				else
				{
					m_input_CharArray[index] = c;
					index++;
				}
			}
			m_input_CharArray[index] = '\0';
			m_charArray_Length = index;
			m_inputSource = TextInputSources.SetText;
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}

		public void SetText(StringBuilder text)
		{
			m_inputSource = TextInputSources.SetCharArray;
			StringBuilderToIntArray(text, ref m_char_buffer);
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText)
		{
			if (sourceText == null)
			{
				return;
			}
			if (sourceText.Length == 0)
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
			if (m_char_buffer == null)
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
				m_char_buffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (sourceText[i] == '\\')
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
					if (i < sourceText.Length - 1)
					{
						int num = sourceText[i + 1];
						if (num == 110)
						{
							if (writeIndex == m_char_buffer.Length)
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
								ResizeInternalArray(ref m_char_buffer);
							}
							m_char_buffer[writeIndex] = 10;
							i++;
							writeIndex++;
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
						if (num == 114)
						{
							if (writeIndex == m_char_buffer.Length)
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
								ResizeInternalArray(ref m_char_buffer);
							}
							m_char_buffer[writeIndex] = 13;
							i++;
							writeIndex++;
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
						if (num == 116)
						{
							if (writeIndex == m_char_buffer.Length)
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
								ResizeInternalArray(ref m_char_buffer);
							}
							m_char_buffer[writeIndex] = 9;
							i++;
							writeIndex++;
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
				}
				if (sourceText[i] == '<')
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
					if (IsTagName(ref sourceText, "<BR>", i))
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
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
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
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
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
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == m_char_buffer.Length)
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
					ResizeInternalArray(ref m_char_buffer);
				}
				m_char_buffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (writeIndex == m_char_buffer.Length)
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
					ResizeInternalArray(ref m_char_buffer);
				}
				m_char_buffer[writeIndex] = 0;
				m_inputSource = TextInputSources.SetCharArray;
				m_isInputParsingRequired = true;
				m_havePropertiesChanged = true;
				m_isCalculateSizeRequired = true;
				SetVerticesDirty();
				SetLayoutDirty();
				return;
			}
		}

		public void SetCharArray(char[] sourceText, int start, int length)
		{
			if (sourceText == null)
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
				if (sourceText.Length == 0)
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
					if (length == 0)
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
					if (m_char_buffer == null)
					{
						m_char_buffer = new int[8];
					}
					m_styleStack.Clear();
					int writeIndex = 0;
					int i = start;
					for (int num = start + length; i < num; i++)
					{
						if (sourceText[i] == '\\')
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
							if (i < length - 1)
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
								int num2 = sourceText[i + 1];
								switch (num2)
								{
								case 110:
									if (writeIndex == m_char_buffer.Length)
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
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 10;
									i++;
									writeIndex++;
									continue;
								case 114:
									if (writeIndex == m_char_buffer.Length)
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
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 13;
									i++;
									writeIndex++;
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
								if (num2 == 116)
								{
									if (writeIndex == m_char_buffer.Length)
									{
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 9;
									i++;
									writeIndex++;
									continue;
								}
							}
						}
						if (sourceText[i] == '<')
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
							if (IsTagName(ref sourceText, "<BR>", i))
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
								if (writeIndex == m_char_buffer.Length)
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
									ResizeInternalArray(ref m_char_buffer);
								}
								m_char_buffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref sourceText, "<STYLE=", i))
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
								int srcOffset = 0;
								if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
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
									i = srcOffset;
									continue;
								}
							}
							else if (IsTagName(ref sourceText, "</STYLE>", i))
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
								ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == m_char_buffer.Length)
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
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = sourceText[i];
						writeIndex++;
					}
					if (writeIndex == m_char_buffer.Length)
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
						ResizeInternalArray(ref m_char_buffer);
					}
					m_char_buffer[writeIndex] = 0;
					m_inputSource = TextInputSources.SetCharArray;
					m_havePropertiesChanged = true;
					m_isInputParsingRequired = true;
					m_isCalculateSizeRequired = true;
					SetVerticesDirty();
					SetLayoutDirty();
					return;
				}
			}
		}

		public void SetCharArray(int[] sourceText, int start, int length)
		{
			if (sourceText == null)
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
				if (sourceText.Length == 0)
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
					if (length == 0)
					{
						return;
					}
					if (m_char_buffer == null)
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
						m_char_buffer = new int[8];
					}
					m_styleStack.Clear();
					int writeIndex = 0;
					int i = start;
					for (int num = start + length; i < num; i++)
					{
						if (sourceText[i] == 92)
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
							if (i < length - 1)
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
								int num2 = sourceText[i + 1];
								if (num2 == 110)
								{
									if (writeIndex == m_char_buffer.Length)
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
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 10;
									i++;
									writeIndex++;
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
								if (num2 == 114)
								{
									if (writeIndex == m_char_buffer.Length)
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
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 13;
									i++;
									writeIndex++;
									continue;
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
								if (num2 == 116)
								{
									if (writeIndex == m_char_buffer.Length)
									{
										ResizeInternalArray(ref m_char_buffer);
									}
									m_char_buffer[writeIndex] = 9;
									i++;
									writeIndex++;
									continue;
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
						}
						if (sourceText[i] == 60)
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
							if (IsTagName(ref sourceText, "<BR>", i))
							{
								if (writeIndex == m_char_buffer.Length)
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
									ResizeInternalArray(ref m_char_buffer);
								}
								m_char_buffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref sourceText, "<STYLE=", i))
							{
								int srcOffset = 0;
								if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref m_char_buffer, ref writeIndex))
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
									i = srcOffset;
									continue;
								}
							}
							else if (IsTagName(ref sourceText, "</STYLE>", i))
							{
								ReplaceClosingStyleTag(ref sourceText, i, ref m_char_buffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == m_char_buffer.Length)
						{
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = sourceText[i];
						writeIndex++;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						if (writeIndex == m_char_buffer.Length)
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
							ResizeInternalArray(ref m_char_buffer);
						}
						m_char_buffer[writeIndex] = 0;
						m_inputSource = TextInputSources.SetCharArray;
						m_havePropertiesChanged = true;
						m_isInputParsingRequired = true;
						m_isCalculateSizeRequired = true;
						SetVerticesDirty();
						SetLayoutDirty();
						return;
					}
				}
			}
		}

		protected void SetTextArrayToCharArray(char[] sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
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
				if (m_charArray_Length == 0)
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
				if (charBuffer == null)
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
					charBuffer = new int[8];
				}
				m_styleStack.Clear();
				int writeIndex = 0;
				for (int i = 0; i < m_charArray_Length; i++)
				{
					if (char.IsHighSurrogate(sourceText[i]))
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
						if (char.IsLowSurrogate(sourceText[i + 1]))
						{
							if (writeIndex == charBuffer.Length)
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
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
							i++;
							writeIndex++;
							continue;
						}
					}
					if (sourceText[i] == '<')
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
						if (IsTagName(ref sourceText, "<BR>", i))
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
							if (writeIndex == charBuffer.Length)
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
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 10;
							writeIndex++;
							i += 3;
							continue;
						}
						if (IsTagName(ref sourceText, "<STYLE=", i))
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
							int srcOffset = 0;
							if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
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
								i = srcOffset;
								continue;
							}
						}
						else if (IsTagName(ref sourceText, "</STYLE>", i))
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
							ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
							i += 7;
							continue;
						}
					}
					if (writeIndex == charBuffer.Length)
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
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = sourceText[i];
					writeIndex++;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					if (writeIndex == charBuffer.Length)
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
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 0;
					return;
				}
			}
		}

		protected void StringToCharArray(string sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
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
						charBuffer[0] = 0;
						return;
					}
				}
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			m_styleStack.SetDefault(0);
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (m_inputSource == TextInputSources.Text && sourceText[i] == '\\')
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
					if (sourceText.Length > i + 1)
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
						int num = sourceText[i + 1];
						switch (num)
						{
						default:
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (num != 110)
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
								break;
							}
							if (!m_parseCtrlCharacters)
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
								break;
							}
							if (writeIndex == charBuffer.Length)
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
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 10;
							i++;
							writeIndex++;
							continue;
						case 85:
							if (sourceText.Length <= i + 9)
							{
								break;
							}
							if (writeIndex == charBuffer.Length)
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
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = GetUTF32(i + 2);
							i += 9;
							writeIndex++;
							continue;
						case 92:
							if (!m_parseCtrlCharacters)
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
							if (sourceText.Length <= i + 2)
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
								break;
							}
							if (writeIndex + 2 > charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = sourceText[i + 1];
							charBuffer[writeIndex + 1] = sourceText[i + 2];
							i += 2;
							writeIndex += 2;
							continue;
						case 114:
							if (!m_parseCtrlCharacters)
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
								break;
							}
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 13;
							i++;
							writeIndex++;
							continue;
						case 116:
							if (!m_parseCtrlCharacters)
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
								break;
							}
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 9;
							i++;
							writeIndex++;
							continue;
						case 117:
							if (sourceText.Length <= i + 5)
							{
								break;
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
							if (writeIndex == charBuffer.Length)
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
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = (ushort)GetUTF16(i + 2);
							i += 5;
							writeIndex++;
							continue;
						}
					}
				}
				if (char.IsHighSurrogate(sourceText[i]))
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
					if (char.IsLowSurrogate(sourceText[i + 1]))
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
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
						i++;
						writeIndex++;
						continue;
					}
				}
				if (sourceText[i] == '<')
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
					if (m_isRichText)
					{
						if (IsTagName(ref sourceText, "<BR>", i))
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
							if (writeIndex == charBuffer.Length)
							{
								ResizeInternalArray(ref charBuffer);
							}
							charBuffer[writeIndex] = 10;
							writeIndex++;
							i += 3;
							continue;
						}
						if (IsTagName(ref sourceText, "<STYLE=", i))
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
							int srcOffset = 0;
							if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
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
								i = srcOffset;
								continue;
							}
						}
						else if (IsTagName(ref sourceText, "</STYLE>", i))
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
							ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
							i += 7;
							continue;
						}
					}
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = 0;
				return;
			}
		}

		protected void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
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
						charBuffer[0] = 0;
						return;
					}
				}
			}
			if (charBuffer == null)
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
				charBuffer = new int[8];
			}
			m_styleStack.Clear();
			int writeIndex = 0;
			for (int i = 0; i < sourceText.Length; i++)
			{
				if (m_parseCtrlCharacters)
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
					if (sourceText[i] == '\\')
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
						if (sourceText.Length > i + 1)
						{
							int num = sourceText[i + 1];
							switch (num)
							{
							default:
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								switch (num)
								{
								case 92:
									if (sourceText.Length <= i + 2)
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
										break;
									}
									if (writeIndex + 2 > charBuffer.Length)
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
										ResizeInternalArray(ref charBuffer);
									}
									charBuffer[writeIndex] = sourceText[i + 1];
									charBuffer[writeIndex + 1] = sourceText[i + 2];
									i += 2;
									writeIndex += 2;
									continue;
								case 110:
									if (writeIndex == charBuffer.Length)
									{
										ResizeInternalArray(ref charBuffer);
									}
									charBuffer[writeIndex] = 10;
									i++;
									writeIndex++;
									continue;
								}
								break;
							case 85:
								if (sourceText.Length <= i + 9)
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
									break;
								}
								if (writeIndex == charBuffer.Length)
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
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = GetUTF32(i + 2);
								i += 9;
								writeIndex++;
								continue;
							case 114:
								if (writeIndex == charBuffer.Length)
								{
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 13;
								i++;
								writeIndex++;
								continue;
							case 116:
								if (writeIndex == charBuffer.Length)
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
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 9;
								i++;
								writeIndex++;
								continue;
							case 117:
								if (sourceText.Length <= i + 5)
								{
									break;
								}
								if (writeIndex == charBuffer.Length)
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
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = (ushort)GetUTF16(i + 2);
								i += 5;
								writeIndex++;
								continue;
							}
						}
					}
				}
				if (char.IsHighSurrogate(sourceText[i]))
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
					if (char.IsLowSurrogate(sourceText[i + 1]))
					{
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
						i++;
						writeIndex++;
						continue;
					}
				}
				if (sourceText[i] == '<')
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
					if (IsTagName(ref sourceText, "<BR>", i))
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
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref sourceText, "<STYLE=", i))
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
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref sourceText, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref sourceText, "</STYLE>", i))
					{
						ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = sourceText[i];
				writeIndex++;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = 0;
				return;
			}
		}

		private bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] text = style.styleOpeningTagArray;
					for (int i = 0; i < num; i++)
					{
						int num2 = text[i];
						if (num2 == 60)
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
							if (IsTagName(ref text, "<BR>", i))
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
								if (writeIndex == charBuffer.Length)
								{
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref text, "<STYLE=", i))
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
								int srcOffset2 = 0;
								if (ReplaceOpeningStyleTag(ref text, i, out srcOffset2, ref charBuffer, ref writeIndex))
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
									i = srcOffset2;
									continue;
								}
							}
							else if (IsTagName(ref text, "</STYLE>", i))
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
								ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
					}
					return true;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			return false;
		}

		private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] text = style.styleOpeningTagArray;
					for (int i = 0; i < num; i++)
					{
						int num2 = text[i];
						if (num2 == 60)
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
							if (IsTagName(ref text, "<BR>", i))
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
								if (writeIndex == charBuffer.Length)
								{
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref text, "<STYLE=", i))
							{
								int srcOffset2 = 0;
								if (ReplaceOpeningStyleTag(ref text, i, out srcOffset2, ref charBuffer, ref writeIndex))
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
									i = srcOffset2;
									continue;
								}
							}
							else if (IsTagName(ref text, "</STYLE>", i))
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
								ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						return true;
					}
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			return false;
		}

		private bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] text = style.styleOpeningTagArray;
					for (int i = 0; i < num; i++)
					{
						int num2 = text[i];
						if (num2 == 60)
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
							if (IsTagName(ref text, "<BR>", i))
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
								if (writeIndex == charBuffer.Length)
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
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref text, "<STYLE=", i))
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
								int srcOffset2 = 0;
								if (ReplaceOpeningStyleTag(ref text, i, out srcOffset2, ref charBuffer, ref writeIndex))
								{
									i = srcOffset2;
									continue;
								}
							}
							else if (IsTagName(ref text, "</STYLE>", i))
							{
								ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						return true;
					}
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			return false;
		}

		private bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
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
				if (srcOffset != 0)
				{
					m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] text = style.styleOpeningTagArray;
					for (int i = 0; i < num; i++)
					{
						int num2 = text[i];
						if (num2 == 60)
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
							if (IsTagName(ref text, "<BR>", i))
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
								if (writeIndex == charBuffer.Length)
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
									ResizeInternalArray(ref charBuffer);
								}
								charBuffer[writeIndex] = 10;
								writeIndex++;
								i += 3;
								continue;
							}
							if (IsTagName(ref text, "<STYLE=", i))
							{
								int srcOffset2 = 0;
								if (ReplaceOpeningStyleTag(ref text, i, out srcOffset2, ref charBuffer, ref writeIndex))
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
									i = srcOffset2;
									continue;
								}
							}
							else if (IsTagName(ref text, "</STYLE>", i))
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
								ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
								i += 7;
								continue;
							}
						}
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
					}
					return true;
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
			return false;
		}

		private bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			m_styleStack.Remove();
			if (style == null)
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
						return false;
					}
				}
			}
			int num = style.styleClosingTagArray.Length;
			int[] text = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = text[i];
				if (num2 == 60)
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
					if (IsTagName(ref text, "<BR>", i))
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
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref text, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref text, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref text, "</STYLE>", i))
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
						ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			m_styleStack.Remove();
			if (style == null)
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
						return false;
					}
				}
			}
			int num = style.styleClosingTagArray.Length;
			int[] text = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = text[i];
				if (num2 == 60)
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
					if (IsTagName(ref text, "<BR>", i))
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
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref text, "<STYLE=", i))
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
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref text, i, out srcOffset, ref charBuffer, ref writeIndex))
						{
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref text, "</STYLE>", i))
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
						ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			return true;
		}

		private bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] text = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = text[i];
				if (num2 == 60)
				{
					if (IsTagName(ref text, "<BR>", i))
					{
						if (writeIndex == charBuffer.Length)
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
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref text, "<STYLE=", i))
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
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref text, i, out srcOffset, ref charBuffer, ref writeIndex))
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
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref text, "</STYLE>", i))
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
						ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
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
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				return true;
			}
		}

		private bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] text = style.styleClosingTagArray;
			for (int i = 0; i < num; i++)
			{
				int num2 = text[i];
				if (num2 == 60)
				{
					if (IsTagName(ref text, "<BR>", i))
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
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = 10;
						writeIndex++;
						i += 3;
						continue;
					}
					if (IsTagName(ref text, "<STYLE=", i))
					{
						int srcOffset = 0;
						if (ReplaceOpeningStyleTag(ref text, i, out srcOffset, ref charBuffer, ref writeIndex))
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
							i = srcOffset;
							continue;
						}
					}
					else if (IsTagName(ref text, "</STYLE>", i))
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
						ReplaceClosingStyleTag(ref text, i, ref charBuffer, ref writeIndex);
						i += 7;
						continue;
					}
				}
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return true;
			}
		}

		private bool IsTagName(ref string text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
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
						return false;
					}
				}
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
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
		}

		private bool IsTagName(ref char[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
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
						return false;
					}
				}
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				return true;
			}
		}

		private bool IsTagName(ref int[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
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
						return false;
					}
				}
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast((char)text[index + i]) == tag[i])
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
					return false;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return true;
			}
		}

		private bool IsTagName(ref StringBuilder text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) == tag[i])
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
			return true;
		}

		private int GetTagHashCode(ref string text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			int num2 = index;
			while (true)
			{
				if (num2 < text.Length)
				{
					if (text[num2] != '"')
					{
						if (text[num2] == '>')
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
							closeIndex = num2;
							break;
						}
						num = (((num << 5) + num) ^ text[num2]);
					}
					num2++;
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
				break;
			}
			return num;
		}

		private int GetTagHashCode(ref char[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] == '"')
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
					continue;
				}
				if (text[i] == '>')
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
					closeIndex = i;
					break;
				}
				num = (((num << 5) + num) ^ text[i]);
			}
			return num;
		}

		private int GetTagHashCode(ref int[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			int num2 = index;
			while (true)
			{
				if (num2 < text.Length)
				{
					if (text[num2] == 34)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
					}
					else
					{
						if (text[num2] == 62)
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
							closeIndex = num2;
							break;
						}
						num = (((num << 5) + num) ^ text[num2]);
					}
					num2++;
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
				break;
			}
			return num;
		}

		private int GetTagHashCode(ref StringBuilder text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			int num2 = index;
			while (true)
			{
				if (num2 < text.Length)
				{
					if (text[num2] == '"')
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
					}
					else
					{
						if (text[num2] == '>')
						{
							closeIndex = num2;
							break;
						}
						num = (((num << 5) + num) ^ text[num2]);
					}
					num2++;
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
			return num;
		}

		private void ResizeInternalArray<T>(ref T[] array)
		{
			int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
			Array.Resize(ref array, newSize);
		}

		protected void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
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
				m_input_CharArray[index++] = '-';
				number = 0f - number;
			}
			number += k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			AddIntToCharArray(num, ref index, precision);
			if (precision <= 0)
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
				m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					m_input_CharArray[index++] = (char)(num2 + 48);
					number -= (float)num2;
				}
				return;
			}
		}

		protected void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				m_input_CharArray[num++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = m_input_CharArray[index];
				m_input_CharArray[index] = m_input_CharArray[num];
				m_input_CharArray[num] = c;
				index++;
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
				index = num2;
				return;
			}
		}

		protected virtual int SetArraySizes(int[] chars)
		{
			return 0;
		}

		protected virtual void GenerateTextMesh()
		{
		}

		public Vector2 GetPreferredValues()
		{
			if (m_isInputParsingRequired || m_isTextTruncated)
			{
				m_isCalculatingPreferredValues = true;
				ParseInputText();
			}
			float preferredWidth = GetPreferredWidth();
			float preferredHeight = GetPreferredHeight();
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(float width, float height)
		{
			if (!m_isInputParsingRequired)
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
				if (!m_isTextTruncated)
				{
					goto IL_003a;
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
			m_isCalculatingPreferredValues = true;
			ParseInputText();
			goto IL_003a;
			IL_003a:
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = GetPreferredWidth(margin);
			float preferredHeight = GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text)
		{
			m_isCalculatingPreferredValues = true;
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			Vector2 margin = k_LargePositiveVector2;
			float preferredWidth = GetPreferredWidth(margin);
			float preferredHeight = GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			m_isCalculatingPreferredValues = true;
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = GetPreferredWidth(margin);
			float preferredHeight = GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		protected float GetPreferredWidth()
		{
			float num;
			if (m_enableAutoSizing)
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
				num = m_fontSizeMax;
			}
			else
			{
				num = m_fontSize;
			}
			float defaultFontSize = num;
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			Vector2 marginSize = k_LargePositiveVector2;
			if (!m_isInputParsingRequired)
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
				if (!m_isTextTruncated)
				{
					goto IL_0079;
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
			m_isCalculatingPreferredValues = true;
			ParseInputText();
			goto IL_0079;
			IL_0079:
			m_recursiveCount = 0;
			Vector2 vector = CalculatePreferredValues(defaultFontSize, marginSize, true);
			float x = vector.x;
			m_isPreferredWidthDirty = false;
			return x;
		}

		protected float GetPreferredWidth(Vector2 margin)
		{
			float defaultFontSize = (!m_enableAutoSizing) ? m_fontSize : m_fontSizeMax;
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			m_recursiveCount = 0;
			Vector2 vector = CalculatePreferredValues(defaultFontSize, margin, true);
			return vector.x;
		}

		protected float GetPreferredHeight()
		{
			float defaultFontSize = (!m_enableAutoSizing) ? m_fontSize : m_fontSizeMax;
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			float marginWidth;
			if (m_marginWidth != 0f)
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
				marginWidth = m_marginWidth;
			}
			else
			{
				marginWidth = k_LargePositiveFloat;
			}
			Vector2 marginSize = new Vector2(marginWidth, k_LargePositiveFloat);
			if (!m_isInputParsingRequired)
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
				if (!m_isTextTruncated)
				{
					goto IL_0099;
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
			m_isCalculatingPreferredValues = true;
			ParseInputText();
			goto IL_0099;
			IL_0099:
			m_recursiveCount = 0;
			Vector2 vector = CalculatePreferredValues(defaultFontSize, marginSize, !m_enableAutoSizing);
			float y = vector.y;
			m_isPreferredHeightDirty = false;
			return y;
		}

		protected float GetPreferredHeight(Vector2 margin)
		{
			float num;
			if (m_enableAutoSizing)
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
				num = m_fontSizeMax;
			}
			else
			{
				num = m_fontSize;
			}
			float defaultFontSize = num;
			m_minFontSize = m_fontSizeMin;
			m_maxFontSize = m_fontSizeMax;
			m_recursiveCount = 0;
			Vector2 vector = CalculatePreferredValues(defaultFontSize, margin, true);
			return vector.y;
		}

		public Vector2 GetRenderedValues()
		{
			return GetTextBounds().size;
		}

		public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
		{
			return GetTextBounds(onlyVisibleCharacters).size;
		}

		protected float GetRenderedWidth()
		{
			Vector2 renderedValues = GetRenderedValues();
			return renderedValues.x;
		}

		protected float GetRenderedWidth(bool onlyVisibleCharacters)
		{
			Vector2 renderedValues = GetRenderedValues(onlyVisibleCharacters);
			return renderedValues.x;
		}

		protected float GetRenderedHeight()
		{
			Vector2 renderedValues = GetRenderedValues();
			return renderedValues.y;
		}

		protected float GetRenderedHeight(bool onlyVisibleCharacters)
		{
			Vector2 renderedValues = GetRenderedValues(onlyVisibleCharacters);
			return renderedValues.y;
		}

		protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
		{
			int totalCharacterCount;
			if (!(m_fontAsset == null))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_fontAsset.characterDictionary != null)
				{
					if (m_char_buffer != null)
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
						if (m_char_buffer.Length != 0)
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
							if (m_char_buffer[0] != 0)
							{
								m_currentFontAsset = m_fontAsset;
								m_currentMaterial = m_sharedMaterial;
								m_currentMaterialIndex = 0;
								m_materialReferenceStack.SetDefault(new MaterialReference(0, m_currentFontAsset, null, m_currentMaterial, m_padding));
								totalCharacterCount = m_totalCharacterCount;
								if (m_internalCharacterInfo != null)
								{
									if (totalCharacterCount <= m_internalCharacterInfo.Length)
									{
										goto IL_0125;
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
								m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount <= 1024) ? Mathf.NextPowerOfTwo(totalCharacterCount) : (totalCharacterCount + 256)];
								goto IL_0125;
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
					return Vector2.zero;
				}
			}
			Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + GetInstanceID());
			return Vector2.zero;
			IL_0125:
			float num = defaultFontSize / m_currentFontAsset.fontInfo.PointSize;
			float num2;
			if (m_isOrthographic)
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
				num2 = 1f;
			}
			else
			{
				num2 = 0.1f;
			}
			m_fontScale = num * num2;
			m_fontScaleMultiplier = 1f;
			float num3 = defaultFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
			float num4;
			if (m_isOrthographic)
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
				num4 = 1f;
			}
			else
			{
				num4 = 0.1f;
			}
			float num5 = num3 * num4;
			float num6 = m_fontScale;
			m_currentFontSize = defaultFontSize;
			m_sizeStack.SetDefault(m_currentFontSize);
			float num7 = 0f;
			int num8 = 0;
			m_style = m_fontStyle;
			m_lineJustification = m_textAlignment;
			m_lineJustificationStack.SetDefault(m_lineJustification);
			float num9 = 1f;
			m_baselineOffset = 0f;
			m_baselineOffsetStack.Clear();
			m_lineOffset = 0f;
			m_lineHeight = -32767f;
			float num10 = m_currentFontAsset.fontInfo.LineHeight - (m_currentFontAsset.fontInfo.Ascender - m_currentFontAsset.fontInfo.Descender);
			m_cSpacing = 0f;
			m_monoSpacing = 0f;
			float num11 = 0f;
			m_xAdvance = 0f;
			float a = 0f;
			tag_LineIndent = 0f;
			tag_Indent = 0f;
			m_indentStack.SetDefault(0f);
			tag_NoParsing = false;
			m_characterCount = 0;
			m_firstCharacterOfLine = 0;
			m_maxLineAscender = k_LargeNegativeFloat;
			m_maxLineDescender = k_LargePositiveFloat;
			m_lineNumber = 0;
			float x = marginSize.x;
			m_marginLeft = 0f;
			m_marginRight = 0f;
			m_width = -1f;
			float num12 = 0f;
			float num13 = 0f;
			float num14 = 0f;
			m_isCalculatingPreferredValues = true;
			m_maxAscender = 0f;
			m_maxDescender = 0f;
			bool flag = true;
			bool flag2 = false;
			WordWrapState state = default(WordWrapState);
			SaveWordWrappingState(ref state, 0, 0);
			WordWrapState state2 = default(WordWrapState);
			int num15 = 0;
			m_recursiveCount++;
			int endIndex = 0;
			for (int i = 0; m_char_buffer[i] != 0; i++)
			{
				num8 = m_char_buffer[i];
				m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				m_currentFontAsset = m_materialReferences[m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = m_currentMaterialIndex;
				if (m_isRichText && num8 == 60)
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
					m_isParsingText = true;
					m_textElementType = TMP_TextElementType.Character;
					if (ValidateHtmlTag(m_char_buffer, i + 1, out endIndex))
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
						i = endIndex;
						if (m_textElementType == TMP_TextElementType.Character)
						{
							continue;
						}
					}
				}
				m_isParsingText = false;
				bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
				float num16 = 1f;
				if (m_textElementType == TMP_TextElementType.Character)
				{
					if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
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
						if (char.IsLower((char)num8))
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
							num8 = char.ToUpper((char)num8);
						}
					}
					else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
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
						if (char.IsUpper((char)num8))
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
							num8 = char.ToLower((char)num8);
						}
					}
					else
					{
						if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
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
							if ((m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
							{
								goto IL_0545;
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
						if (char.IsLower((char)num8))
						{
							num16 = 0.8f;
							num8 = char.ToUpper((char)num8);
						}
					}
				}
				goto IL_0545;
				IL_14b6:
				if (num8 > 4352)
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
					if (num8 < 4607)
					{
						goto IL_1586;
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
				if (num8 > 11904)
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
					if (num8 < 40959)
					{
						goto IL_1586;
					}
				}
				if (num8 > 43360)
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
					if (num8 < 43391)
					{
						goto IL_1586;
					}
				}
				if (num8 <= 44032 || num8 >= 55295)
				{
					if (num8 > 63744)
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
						if (num8 < 64255)
						{
							goto IL_1586;
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
					if (num8 <= 65072 || num8 >= 65103)
					{
						if (num8 <= 65280 || num8 >= 65519)
						{
							goto IL_163f;
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
				goto IL_1586;
				IL_0b8b:
				float num17;
				if (m_width != -1f)
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
					num17 = Mathf.Min(x + 0.0001f - m_marginLeft - m_marginRight, m_width);
				}
				else
				{
					num17 = x + 0.0001f - m_marginLeft - m_marginRight;
				}
				float num18 = num17;
				int num19;
				if ((m_lineJustification & (TextAlignmentOptions)16) != (TextAlignmentOptions)16)
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
					num19 = (((m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8) ? 1 : 0);
				}
				else
				{
					num19 = 1;
				}
				bool flag3 = (byte)num19 != 0;
				float num20;
				num14 = m_xAdvance + m_cached_TextElement.xAdvance * ((num8 == 173) ? num20 : num6);
				float num21 = num14;
				float num22;
				if (flag3)
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
					num22 = 1.05f;
				}
				else
				{
					num22 = 1f;
				}
				if (num21 > num18 * num22)
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
					if (enableWordWrapping)
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
						if (m_characterCount != m_firstCharacterOfLine)
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
							if (num15 != state2.previous_WordBreak)
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
								if (!flag)
								{
									goto IL_0cc0;
								}
							}
							if (!m_isCharacterWrappingEnabled)
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
								m_isCharacterWrappingEnabled = true;
							}
							else
							{
								flag2 = true;
							}
							goto IL_0cc0;
						}
					}
					if (!ignoreTextAutoSizing)
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
						if (defaultFontSize > m_fontSizeMin)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
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
									}
									m_maxFontSize = defaultFontSize;
									defaultFontSize -= Mathf.Max((defaultFontSize - m_minFontSize) / 2f, 0.05f);
									defaultFontSize = (float)(int)(Mathf.Max(defaultFontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
									if (m_recursiveCount > 20)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												return new Vector2(num12, num13);
											}
										}
									}
									return CalculatePreferredValues(defaultFontSize, marginSize, false);
								}
							}
						}
					}
				}
				goto IL_0fb6;
				IL_1586:
				if (!m_isNonBreakingSpace)
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
					if (!flag)
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
						if (!flag2)
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
							if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num8))
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
								if (m_characterCount < totalCharacterCount - 1)
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
									if (!TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_internalCharacterInfo[m_characterCount + 1].character))
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
										goto IL_1623;
									}
								}
							}
							goto IL_1673;
						}
					}
					goto IL_1623;
				}
				goto IL_163f;
				IL_1623:
				SaveWordWrappingState(ref state2, i, m_characterCount);
				m_isCharacterWrappingEnabled = false;
				flag = false;
				goto IL_1673;
				IL_0908:
				num9 = 1f;
				goto IL_090f;
				IL_090f:
				m_internalCharacterInfo[m_characterCount].baseLine = 0f - m_lineOffset + m_baselineOffset;
				float ascender = m_currentFontAsset.fontInfo.Ascender;
				float num23;
				if (m_textElementType == TMP_TextElementType.Character)
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
					num23 = num6;
				}
				else
				{
					num23 = m_internalCharacterInfo[m_characterCount].scale;
				}
				float num24 = ascender * num23 + m_baselineOffset;
				m_internalCharacterInfo[m_characterCount].ascender = num24 - m_lineOffset;
				float maxLineAscender;
				if (num24 > m_maxLineAscender)
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
					maxLineAscender = num24;
				}
				else
				{
					maxLineAscender = m_maxLineAscender;
				}
				m_maxLineAscender = maxLineAscender;
				float descender = m_currentFontAsset.fontInfo.Descender;
				float num25;
				if (m_textElementType == TMP_TextElementType.Character)
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
					num25 = num6;
				}
				else
				{
					num25 = m_internalCharacterInfo[m_characterCount].scale;
				}
				float num26 = descender * num25 + m_baselineOffset;
				float num27 = m_internalCharacterInfo[m_characterCount].descender = num26 - m_lineOffset;
				float maxLineDescender;
				if (num26 < m_maxLineDescender)
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
					maxLineDescender = num26;
				}
				else
				{
					maxLineDescender = m_maxLineDescender;
				}
				m_maxLineDescender = maxLineDescender;
				if ((m_style & FontStyles.Subscript) != FontStyles.Subscript)
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
					if ((m_style & FontStyles.Superscript) != FontStyles.Superscript)
					{
						goto IL_0b10;
					}
				}
				float num28 = (num24 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
				num24 = m_maxLineAscender;
				float maxLineAscender2;
				if (num28 > m_maxLineAscender)
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
					maxLineAscender2 = num28;
				}
				else
				{
					maxLineAscender2 = m_maxLineAscender;
				}
				m_maxLineAscender = maxLineAscender2;
				float num29 = (num26 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
				num26 = m_maxLineDescender;
				m_maxLineDescender = ((!(num29 < m_maxLineDescender)) ? m_maxLineDescender : num29);
				goto IL_0b10;
				IL_13d7:
				if (!m_enableWordWrapping && m_overflowMode != TextOverflowModes.Truncate)
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
					if (m_overflowMode != TextOverflowModes.Ellipsis)
					{
						goto IL_1673;
					}
				}
				if (!char.IsWhiteSpace((char)num8))
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
					if (num8 != 8203)
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
						if (num8 != 45)
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
							if (num8 != 173)
							{
								goto IL_14b6;
							}
						}
					}
				}
				if (!m_isNonBreakingSpace)
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
					if (num8 != 160)
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
						if (num8 != 8209)
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
							if (num8 != 8239)
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
								if (num8 != 8288)
								{
									SaveWordWrappingState(ref state2, i, m_characterCount);
									m_isCharacterWrappingEnabled = false;
									flag = false;
									goto IL_1673;
								}
							}
						}
					}
				}
				goto IL_14b6;
				IL_0545:
				if (m_textElementType == TMP_TextElementType.Sprite)
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
					m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
					m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
					TMP_Sprite tMP_Sprite = m_currentSpriteAsset.spriteInfoList[m_spriteIndex];
					if (tMP_Sprite == null)
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
						continue;
					}
					if (num8 == 60)
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
						num8 = 57344 + m_spriteIndex;
					}
					m_currentFontAsset = m_fontAsset;
					float num30 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
					float num31;
					if (m_isOrthographic)
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
						num31 = 1f;
					}
					else
					{
						num31 = 0.1f;
					}
					float num32 = num30 * num31;
					num6 = m_fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * num32;
					m_cached_TextElement = tMP_Sprite;
					m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
					m_internalCharacterInfo[m_characterCount].scale = num32;
					m_currentMaterialIndex = currentMaterialIndex;
				}
				else if (m_textElementType == TMP_TextElementType.Character)
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
					m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
					if (m_cached_TextElement == null)
					{
						continue;
					}
					m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
					float num33 = m_currentFontSize * num16 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
					float num34;
					if (m_isOrthographic)
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
						num34 = 1f;
					}
					else
					{
						num34 = 0.1f;
					}
					m_fontScale = num33 * num34;
					num6 = m_fontScale * m_fontScaleMultiplier * m_cached_TextElement.scale;
					m_internalCharacterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
				}
				num20 = num6;
				if (num8 == 173)
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
					num6 = 0f;
				}
				m_internalCharacterInfo[m_characterCount].character = (char)num8;
				if (m_enableKerning)
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
					if (m_characterCount >= 1)
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
						int character = m_internalCharacterInfo[m_characterCount - 1].character;
						KerningPairKey kerningPairKey = new KerningPairKey(character, num8);
						m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out KerningPair value);
						if (value != null)
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
							m_xAdvance += value.XadvanceOffset * num6;
						}
					}
				}
				float num35 = 0f;
				if (m_monoSpacing != 0f)
				{
					num35 = m_monoSpacing / 2f - (m_cached_TextElement.width / 2f + m_cached_TextElement.xOffset) * num6;
					m_xAdvance += num35;
				}
				if (m_textElementType == TMP_TextElementType.Character)
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
					if (!isUsingAlternateTypeface)
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
						if ((m_style & FontStyles.Bold) != FontStyles.Bold)
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
							if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
							{
								goto IL_0908;
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
						num9 = 1f + m_currentFontAsset.boldSpacing * 0.01f;
						goto IL_090f;
					}
				}
				goto IL_0908;
				IL_0cc0:
				i = RestoreWordWrappingState(ref state2);
				num15 = i;
				if (m_char_buffer[i] == 173)
				{
					m_isTextTruncated = true;
					m_char_buffer[i] = 45;
					CalculatePreferredValues(defaultFontSize, marginSize, true);
					return Vector2.zero;
				}
				if (m_lineNumber > 0)
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
					if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
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
						if (m_lineHeight == -32767f)
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
							float num36 = m_maxLineAscender - m_startOfLineAscender;
							m_lineOffset += num36;
							state2.lineOffset = m_lineOffset;
							state2.previousLineAscender = m_maxLineAscender;
						}
					}
				}
				float num37 = m_maxLineAscender - m_lineOffset;
				float num38 = m_maxLineDescender - m_lineOffset;
				float maxDescender;
				if (m_maxDescender < num38)
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
					maxDescender = m_maxDescender;
				}
				else
				{
					maxDescender = num38;
				}
				m_maxDescender = maxDescender;
				m_firstCharacterOfLine = m_characterCount;
				num12 += m_xAdvance;
				if (m_enableWordWrapping)
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
					num13 = m_maxAscender - m_maxDescender;
				}
				else
				{
					num13 = Mathf.Max(num13, num37 - num38);
				}
				SaveWordWrappingState(ref state, i, m_characterCount - 1);
				m_lineNumber++;
				if (m_lineHeight == -32767f)
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
					float num39 = m_internalCharacterInfo[m_characterCount].ascender - m_internalCharacterInfo[m_characterCount].baseLine;
					num11 = 0f - m_maxLineDescender + num39 + (num10 + m_lineSpacing + m_lineSpacingDelta) * num5;
					m_lineOffset += num11;
					m_startOfLineAscender = num39;
				}
				else
				{
					m_lineOffset += m_lineHeight + m_lineSpacing * num5;
				}
				m_maxLineAscender = k_LargeNegativeFloat;
				m_maxLineDescender = k_LargePositiveFloat;
				m_xAdvance = tag_Indent;
				continue;
				IL_11ac:
				if (num8 == 13)
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
					a = Mathf.Max(a, num12 + m_xAdvance);
					num12 = 0f;
					m_xAdvance = tag_Indent;
				}
				if (num8 != 10)
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
					if (m_characterCount != totalCharacterCount - 1)
					{
						goto IL_13d7;
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
				if (m_lineNumber > 0)
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
					if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
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
						if (m_lineHeight == -32767f)
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
							float num40 = m_maxLineAscender - m_startOfLineAscender;
							num27 -= num40;
							m_lineOffset += num40;
						}
					}
				}
				float num41 = m_maxLineDescender - m_lineOffset;
				m_maxDescender = ((!(m_maxDescender < num41)) ? num41 : m_maxDescender);
				m_firstCharacterOfLine = m_characterCount + 1;
				if (num8 == 10 && m_characterCount != totalCharacterCount - 1)
				{
					a = Mathf.Max(a, num12 + num14);
					num12 = 0f;
				}
				else
				{
					num12 = Mathf.Max(a, num12 + num14);
				}
				num13 = m_maxAscender - m_maxDescender;
				if (num8 == 10)
				{
					SaveWordWrappingState(ref state, i, m_characterCount);
					SaveWordWrappingState(ref state2, i, m_characterCount);
					m_lineNumber++;
					if (m_lineHeight == -32767f)
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
						num11 = 0f - m_maxLineDescender + num24 + (num10 + m_lineSpacing + m_paragraphSpacing + m_lineSpacingDelta) * num5;
						m_lineOffset += num11;
					}
					else
					{
						m_lineOffset += m_lineHeight + (m_lineSpacing + m_paragraphSpacing) * num5;
					}
					m_maxLineAscender = k_LargeNegativeFloat;
					m_maxLineDescender = k_LargePositiveFloat;
					m_startOfLineAscender = num24;
					m_xAdvance = tag_LineIndent + tag_Indent;
				}
				goto IL_13d7;
				IL_163f:
				if (!flag && !m_isCharacterWrappingEnabled)
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
						goto IL_1673;
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
				SaveWordWrappingState(ref state2, i, m_characterCount);
				goto IL_1673;
				IL_0b10:
				if (m_lineNumber == 0)
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
					float maxAscender;
					if (m_maxAscender > num24)
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
						maxAscender = m_maxAscender;
					}
					else
					{
						maxAscender = num24;
					}
					m_maxAscender = maxAscender;
				}
				if (num8 != 9)
				{
					if (!char.IsWhiteSpace((char)num8))
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
						if (num8 != 8203)
						{
							goto IL_0b8b;
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
					if (m_textElementType != TMP_TextElementType.Sprite)
					{
						goto IL_0fb6;
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
				goto IL_0b8b;
				IL_1673:
				m_characterCount++;
				continue;
				IL_0fb6:
				if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
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
					if (m_lineHeight == -32767f)
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
						if (!m_isNewPage)
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
							float num42 = m_maxLineAscender - m_startOfLineAscender;
							num27 -= num42;
							m_lineOffset += num42;
							m_startOfLineAscender += num42;
							state2.lineOffset = m_lineOffset;
							state2.previousLineAscender = m_startOfLineAscender;
						}
					}
				}
				if (num8 == 9)
				{
					float num43 = m_currentFontAsset.fontInfo.TabWidth * num6;
					float num44 = Mathf.Ceil(m_xAdvance / num43) * num43;
					float xAdvance;
					if (num44 > m_xAdvance)
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
						xAdvance = num44;
					}
					else
					{
						xAdvance = m_xAdvance + num43;
					}
					m_xAdvance = xAdvance;
				}
				else if (m_monoSpacing != 0f)
				{
					m_xAdvance += m_monoSpacing - num35 + (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 + m_cSpacing;
					if (!char.IsWhiteSpace((char)num8))
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
						if (num8 != 8203)
						{
							goto IL_11ac;
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
					m_xAdvance += m_wordSpacing * num6;
				}
				else
				{
					m_xAdvance += (m_cached_TextElement.xAdvance * num9 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 + m_cSpacing;
					if (!char.IsWhiteSpace((char)num8))
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
						if (num8 != 8203)
						{
							goto IL_11ac;
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
					m_xAdvance += m_wordSpacing * num6;
				}
				goto IL_11ac;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				num7 = m_maxFontSize - m_minFontSize;
				if (!m_isCharacterWrappingEnabled && !ignoreTextAutoSizing && num7 > 0.051f)
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
					if (defaultFontSize < m_fontSizeMax)
					{
						m_minFontSize = defaultFontSize;
						defaultFontSize += Mathf.Max((m_maxFontSize - defaultFontSize) / 2f, 0.05f);
						defaultFontSize = (float)(int)(Mathf.Min(defaultFontSize, m_fontSizeMax) * 20f + 0.5f) / 20f;
						if (m_recursiveCount > 20)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									return new Vector2(num12, num13);
								}
							}
						}
						return CalculatePreferredValues(defaultFontSize, marginSize, false);
					}
				}
				m_isCharacterWrappingEnabled = false;
				m_isCalculatingPreferredValues = false;
				float num45 = num12;
				float num46;
				if (m_margin.x > 0f)
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
					num46 = m_margin.x;
				}
				else
				{
					num46 = 0f;
				}
				num12 = num45 + num46;
				float num47 = num12;
				float num48;
				if (m_margin.z > 0f)
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
					num48 = m_margin.z;
				}
				else
				{
					num48 = 0f;
				}
				num12 = num47 + num48;
				float num49 = num13;
				float num50;
				if (m_margin.y > 0f)
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
					num50 = m_margin.y;
				}
				else
				{
					num50 = 0f;
				}
				num13 = num49 + num50;
				float num51 = num13;
				float num52;
				if (m_margin.w > 0f)
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
					num52 = m_margin.w;
				}
				else
				{
					num52 = 0f;
				}
				num13 = num51 + num52;
				num12 = (float)(int)(num12 * 100f + 1f) / 100f;
				num13 = (float)(int)(num13 * 100f + 1f) / 100f;
				return new Vector2(num12, num13);
			}
		}

		protected virtual Bounds GetCompoundBounds()
		{
			return default(Bounds);
		}

		protected Bounds GetTextBounds()
		{
			if (m_textInfo != null)
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
				if (m_textInfo.characterCount <= m_textInfo.characterInfo.Length)
				{
					Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
					for (int i = 0; i < m_textInfo.characterCount; i++)
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
						if (i < m_textInfo.characterInfo.Length)
						{
							if (!m_textInfo.characterInfo[i].isVisible)
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
							}
							else
							{
								extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[i].bottomLeft.x);
								extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[i].descender);
								extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[i].xAdvance);
								extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[i].ascender);
							}
							continue;
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
						break;
					}
					Vector2 v = default(Vector2);
					v.x = extents.max.x - extents.min.x;
					v.y = extents.max.y - extents.min.y;
					Vector3 center = (extents.min + extents.max) / 2f;
					return new Bounds(center, v);
				}
			}
			return default(Bounds);
		}

		protected Bounds GetTextBounds(bool onlyVisibleCharacters)
		{
			if (m_textInfo == null)
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
						return default(Bounds);
					}
				}
			}
			Extents extents = new Extents(k_LargePositiveVector2, k_LargeNegativeVector2);
			int num = 0;
			while (true)
			{
				if (num < m_textInfo.characterCount)
				{
					if (num <= maxVisibleCharacters)
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
						if (m_textInfo.characterInfo[num].lineNumber <= m_maxVisibleLines)
						{
							goto IL_0082;
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
					if (onlyVisibleCharacters)
					{
						break;
					}
					goto IL_0082;
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
				break;
				IL_0186:
				num++;
				continue;
				IL_0082:
				if (onlyVisibleCharacters)
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
					if (!m_textInfo.characterInfo[num].isVisible)
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
						goto IL_0186;
					}
				}
				extents.min.x = Mathf.Min(extents.min.x, m_textInfo.characterInfo[num].origin);
				extents.min.y = Mathf.Min(extents.min.y, m_textInfo.characterInfo[num].descender);
				extents.max.x = Mathf.Max(extents.max.x, m_textInfo.characterInfo[num].xAdvance);
				extents.max.y = Mathf.Max(extents.max.y, m_textInfo.characterInfo[num].ascender);
				goto IL_0186;
			}
			Vector2 v = default(Vector2);
			v.x = extents.max.x - extents.min.x;
			v.y = extents.max.y - extents.min.y;
			Vector2 v2 = (extents.min + extents.max) / 2f;
			return new Bounds(v2, v);
		}

		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		protected void ResizeLineExtents(int size)
		{
			int num;
			if (size > 1024)
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
				num = size + 256;
			}
			else
			{
				num = Mathf.NextPowerOfTwo(size + 1);
			}
			size = num;
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < m_textInfo.lineInfo.Length)
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
					array[i] = m_textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = k_LargePositiveVector2;
					array[i].lineExtents.max = k_LargeNegativeVector2;
					array[i].ascender = k_LargeNegativeFloat;
					array[i].descender = k_LargePositiveFloat;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				m_textInfo.lineInfo = array;
				return;
			}
		}

		public virtual TMP_TextInfo GetTextInfo(string text)
		{
			return null;
		}

		protected virtual void ComputeMarginSize()
		{
		}

		protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
		{
			state.currentFontAsset = m_currentFontAsset;
			state.currentSpriteAsset = m_currentSpriteAsset;
			state.currentMaterial = m_currentMaterial;
			state.currentMaterialIndex = m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = m_lineVisibleCharacterCount;
			state.visible_LinkCount = m_textInfo.linkCount;
			state.firstCharacterIndex = m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = m_lastVisibleCharacterOfLine;
			state.fontStyle = m_style;
			state.fontScale = m_fontScale;
			state.fontScaleMultiplier = m_fontScaleMultiplier;
			state.currentFontSize = m_currentFontSize;
			state.xAdvance = m_xAdvance;
			state.maxCapHeight = m_maxCapHeight;
			state.maxAscender = m_maxAscender;
			state.maxDescender = m_maxDescender;
			state.maxLineAscender = m_maxLineAscender;
			state.maxLineDescender = m_maxLineDescender;
			state.previousLineAscender = m_startOfLineAscender;
			state.preferredWidth = m_preferredWidth;
			state.preferredHeight = m_preferredHeight;
			state.meshExtents = m_meshExtents;
			state.lineNumber = m_lineNumber;
			state.lineOffset = m_lineOffset;
			state.baselineOffset = m_baselineOffset;
			state.vertexColor = m_htmlColor;
			state.underlineColor = m_underlineColor;
			state.strikethroughColor = m_strikethroughColor;
			state.highlightColor = m_highlightColor;
			state.tagNoParsing = tag_NoParsing;
			state.basicStyleStack = m_fontStyleStack;
			state.colorStack = m_colorStack;
			state.underlineColorStack = m_underlineColorStack;
			state.strikethroughColorStack = m_strikethroughColorStack;
			state.highlightColorStack = m_highlightColorStack;
			state.sizeStack = m_sizeStack;
			state.indentStack = m_indentStack;
			state.fontWeightStack = m_fontWeightStack;
			state.styleStack = m_styleStack;
			state.baselineStack = m_baselineOffsetStack;
			state.actionStack = m_actionStack;
			state.materialReferenceStack = m_materialReferenceStack;
			state.lineJustificationStack = m_lineJustificationStack;
			state.spriteAnimationID = m_spriteAnimationID;
			if (m_lineNumber < m_textInfo.lineInfo.Length)
			{
				state.lineInfo = m_textInfo.lineInfo[m_lineNumber];
			}
		}

		protected int RestoreWordWrappingState(ref WordWrapState state)
		{
			int previous_WordBreak = state.previous_WordBreak;
			m_currentFontAsset = state.currentFontAsset;
			m_currentSpriteAsset = state.currentSpriteAsset;
			m_currentMaterial = state.currentMaterial;
			m_currentMaterialIndex = state.currentMaterialIndex;
			m_characterCount = state.total_CharacterCount + 1;
			m_lineVisibleCharacterCount = state.visible_CharacterCount;
			m_textInfo.linkCount = state.visible_LinkCount;
			m_firstCharacterOfLine = state.firstCharacterIndex;
			m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			m_style = state.fontStyle;
			m_fontScale = state.fontScale;
			m_fontScaleMultiplier = state.fontScaleMultiplier;
			m_currentFontSize = state.currentFontSize;
			m_xAdvance = state.xAdvance;
			m_maxCapHeight = state.maxCapHeight;
			m_maxAscender = state.maxAscender;
			m_maxDescender = state.maxDescender;
			m_maxLineAscender = state.maxLineAscender;
			m_maxLineDescender = state.maxLineDescender;
			m_startOfLineAscender = state.previousLineAscender;
			m_preferredWidth = state.preferredWidth;
			m_preferredHeight = state.preferredHeight;
			m_meshExtents = state.meshExtents;
			m_lineNumber = state.lineNumber;
			m_lineOffset = state.lineOffset;
			m_baselineOffset = state.baselineOffset;
			m_htmlColor = state.vertexColor;
			m_underlineColor = state.underlineColor;
			m_strikethroughColor = state.strikethroughColor;
			m_highlightColor = state.highlightColor;
			tag_NoParsing = state.tagNoParsing;
			m_fontStyleStack = state.basicStyleStack;
			m_colorStack = state.colorStack;
			m_underlineColorStack = state.underlineColorStack;
			m_strikethroughColorStack = state.strikethroughColorStack;
			m_highlightColorStack = state.highlightColorStack;
			m_sizeStack = state.sizeStack;
			m_indentStack = state.indentStack;
			m_fontWeightStack = state.fontWeightStack;
			m_styleStack = state.styleStack;
			m_baselineOffsetStack = state.baselineStack;
			m_actionStack = state.actionStack;
			m_materialReferenceStack = state.materialReferenceStack;
			m_lineJustificationStack = state.lineJustificationStack;
			m_spriteAnimationID = state.spriteAnimationID;
			if (m_lineNumber < m_textInfo.lineInfo.Length)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_textInfo.lineInfo[m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
			byte a;
			if (m_fontColor32.a < vertexColor.a)
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
				a = m_fontColor32.a;
			}
			else
			{
				a = vertexColor.a;
			}
			vertexColor.a = a;
			if (!m_enableVertexGradient)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!m_overrideHtmlColors && m_colorStack.index > 1)
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
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (m_fontColorGradientPreset != null)
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = m_fontColorGradientPreset.bottomLeft * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = m_fontColorGradientPreset.topLeft * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = m_fontColorGradientPreset.topRight * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = m_fontColorGradientPreset.bottomRight * vertexColor;
			}
			else
			{
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = m_fontColorGradient.bottomLeft * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = m_fontColorGradient.topLeft * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = m_fontColorGradient.topRight * vertexColor;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = m_fontColorGradient.bottomRight * vertexColor;
			}
			if (!m_isSDFShader)
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
				style_padding = 0f;
			}
			FaceInfo fontInfo = m_currentFontAsset.fontInfo;
			Vector2 uv = default(Vector2);
			uv.x = (m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			uv.y = 1f - (m_cached_TextElement.y + padding + style_padding + m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 uv2 = default(Vector2);
			uv2.x = uv.x;
			uv2.y = 1f - (m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 uv3 = default(Vector2);
			uv3.x = (m_cached_TextElement.x + padding + style_padding + m_cached_TextElement.width) / fontInfo.AtlasWidth;
			uv3.y = uv2.y;
			Vector2 uv4 = default(Vector2);
			uv4.x = uv3.x;
			uv4.y = uv.y;
			m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = uv;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = uv2;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = uv3;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv4;
		}

		protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			m_textInfo.characterInfo[m_characterCount].vertex_BL.position = m_textInfo.characterInfo[m_characterCount].bottomLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.position = m_textInfo.characterInfo[m_characterCount].topLeft;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.position = m_textInfo.characterInfo[m_characterCount].topRight;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.position = m_textInfo.characterInfo[m_characterCount].bottomRight;
			if (m_tintAllSprites)
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
				m_tintSprite = true;
			}
			Color32 color;
			if (m_tintSprite)
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
				color = m_spriteColor.Multiply(vertexColor);
			}
			else
			{
				color = m_spriteColor;
			}
			Color32 color2 = color;
			byte a;
			if (color2.a < m_fontColor32.a)
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
				a = (color2.a = ((color2.a >= vertexColor.a) ? vertexColor.a : color2.a));
			}
			else
			{
				a = m_fontColor32.a;
			}
			color2.a = a;
			if (!m_enableVertexGradient)
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
				m_textInfo.characterInfo[m_characterCount].vertex_BL.color = color2;
				m_textInfo.characterInfo[m_characterCount].vertex_TL.color = color2;
				m_textInfo.characterInfo[m_characterCount].vertex_TR.color = color2;
				m_textInfo.characterInfo[m_characterCount].vertex_BR.color = color2;
			}
			else
			{
				if (!m_overrideHtmlColors)
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
					if (m_colorStack.index > 1)
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
						m_textInfo.characterInfo[m_characterCount].vertex_BL.color = color2;
						m_textInfo.characterInfo[m_characterCount].vertex_TL.color = color2;
						m_textInfo.characterInfo[m_characterCount].vertex_TR.color = color2;
						m_textInfo.characterInfo[m_characterCount].vertex_BR.color = color2;
						goto IL_055a;
					}
				}
				if (m_fontColorGradientPreset != null)
				{
					ref TMP_Vertex vertex_BL = ref m_textInfo.characterInfo[m_characterCount].vertex_BL;
					Color32 color3;
					if (m_tintSprite)
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
						color3 = color2.Multiply(m_fontColorGradientPreset.bottomLeft);
					}
					else
					{
						color3 = color2;
					}
					vertex_BL.color = color3;
					ref TMP_Vertex vertex_TL = ref m_textInfo.characterInfo[m_characterCount].vertex_TL;
					Color32 color4;
					if (m_tintSprite)
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
						color4 = color2.Multiply(m_fontColorGradientPreset.topLeft);
					}
					else
					{
						color4 = color2;
					}
					vertex_TL.color = color4;
					ref TMP_Vertex vertex_TR = ref m_textInfo.characterInfo[m_characterCount].vertex_TR;
					Color32 color5;
					if (m_tintSprite)
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
						color5 = color2.Multiply(m_fontColorGradientPreset.topRight);
					}
					else
					{
						color5 = color2;
					}
					vertex_TR.color = color5;
					m_textInfo.characterInfo[m_characterCount].vertex_BR.color = ((!m_tintSprite) ? color2 : color2.Multiply(m_fontColorGradientPreset.bottomRight));
				}
				else
				{
					m_textInfo.characterInfo[m_characterCount].vertex_BL.color = ((!m_tintSprite) ? color2 : color2.Multiply(m_fontColorGradient.bottomLeft));
					ref TMP_Vertex vertex_TL2 = ref m_textInfo.characterInfo[m_characterCount].vertex_TL;
					Color32 color6;
					if (m_tintSprite)
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
						color6 = color2.Multiply(m_fontColorGradient.topLeft);
					}
					else
					{
						color6 = color2;
					}
					vertex_TL2.color = color6;
					ref TMP_Vertex vertex_TR2 = ref m_textInfo.characterInfo[m_characterCount].vertex_TR;
					Color32 color7;
					if (m_tintSprite)
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
						color7 = color2.Multiply(m_fontColorGradient.topRight);
					}
					else
					{
						color7 = color2;
					}
					vertex_TR2.color = color7;
					ref TMP_Vertex vertex_BR = ref m_textInfo.characterInfo[m_characterCount].vertex_BR;
					Color32 color8;
					if (m_tintSprite)
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
						color8 = color2.Multiply(m_fontColorGradient.bottomRight);
					}
					else
					{
						color8 = color2;
					}
					vertex_BR.color = color8;
				}
			}
			goto IL_055a;
			IL_055a:
			Vector2 uv = new Vector2(m_cached_TextElement.x / (float)m_currentSpriteAsset.spriteSheet.width, m_cached_TextElement.y / (float)m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv2 = new Vector2(uv.x, (m_cached_TextElement.y + m_cached_TextElement.height) / (float)m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv3 = new Vector2((m_cached_TextElement.x + m_cached_TextElement.width) / (float)m_currentSpriteAsset.spriteSheet.width, uv2.y);
			Vector2 uv4 = new Vector2(uv3.x, uv.y);
			m_textInfo.characterInfo[m_characterCount].vertex_BL.uv = uv;
			m_textInfo.characterInfo[m_characterCount].vertex_TL.uv = uv2;
			m_textInfo.characterInfo[m_characterCount].vertex_TR.uv = uv3;
			m_textInfo.characterInfo[m_characterCount].vertex_BR.uv = uv4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
		{
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			if (isVolumetric)
			{
				Vector3 b = new Vector3(0f, 0f, m_fontSize * m_fontScale);
				m_textInfo.meshInfo[materialReferenceIndex].vertices[4 + index_X4] = characterInfo[i].vertex_BL.position + b;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[5 + index_X4] = characterInfo[i].vertex_TL.position + b;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[6 + index_X4] = characterInfo[i].vertex_TR.position + b;
				m_textInfo.meshInfo[materialReferenceIndex].vertices[7 + index_X4] = characterInfo[i].vertex_BR.position + b;
			}
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			if (isVolumetric)
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
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[4 + index_X4] = characterInfo[i].vertex_BL.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[5 + index_X4] = characterInfo[i].vertex_TL.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[6 + index_X4] = characterInfo[i].vertex_TR.uv;
				m_textInfo.meshInfo[materialReferenceIndex].uvs0[7 + index_X4] = characterInfo[i].vertex_BR.uv;
			}
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			if (isVolumetric)
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
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[4 + index_X4] = characterInfo[i].vertex_BL.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[5 + index_X4] = characterInfo[i].vertex_TL.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[6 + index_X4] = characterInfo[i].vertex_TR.uv2;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[7 + index_X4] = characterInfo[i].vertex_BR.uv2;
			}
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			if (isVolumetric)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 128, byte.MaxValue);
				m_textInfo.meshInfo[materialReferenceIndex].colors32[4 + index_X4] = color;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[5 + index_X4] = color;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[6 + index_X4] = color;
				m_textInfo.meshInfo[materialReferenceIndex].colors32[7 + index_X4] = color;
			}
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + (isVolumetric ? 8 : 4);
		}

		protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			m_textInfo.characterInfo[i].vertexIndex = index_X4;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
		{
			if (m_cached_Underline_GlyphInfo == null)
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
						if (!TMP_Settings.warningsDisabled)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
									return;
								}
							}
						}
						return;
					}
				}
			}
			int num = index + 12;
			if (num > m_textInfo.meshInfo[0].vertices.Length)
			{
				m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < m_cached_Underline_GlyphInfo.width * maxScale)
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
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = m_padding * startScale / maxScale;
			float num4 = m_padding * endScale / maxScale;
			float height = m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(0f - num2, m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(0f - num2, (0f - (height + m_padding)) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 10] = end + new Vector3(0f, m_padding * maxScale, 0f);
			vertices[index + 11] = end + new Vector3(0f, (0f - (height + m_padding)) * maxScale, 0f);
			Vector2[] uvs = m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((m_cached_Underline_GlyphInfo.x - num3) / m_fontAsset.fontInfo.AtlasWidth, 1f - (m_cached_Underline_GlyphInfo.y + m_padding + m_cached_Underline_GlyphInfo.height) / m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector2 = new Vector2(vector.x, 1f - (m_cached_Underline_GlyphInfo.y - m_padding) / m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector3 = new Vector2((m_cached_Underline_GlyphInfo.x - num3 + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector4 = new Vector2(vector3.x, vector.y);
			Vector2 vector5 = new Vector2((m_cached_Underline_GlyphInfo.x + num4 + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector6 = new Vector2(vector5.x, vector.y);
			Vector2 vector7 = new Vector2((m_cached_Underline_GlyphInfo.x + num4 + m_cached_Underline_GlyphInfo.width) / m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector8 = new Vector2(vector7.x, vector.y);
			uvs[index] = vector;
			uvs[1 + index] = vector2;
			uvs[2 + index] = vector3;
			uvs[3 + index] = vector4;
			uvs[4 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector.y);
			uvs[5 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector2.y);
			uvs[6 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector2.y);
			uvs[7 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector.y);
			uvs[8 + index] = vector6;
			uvs[9 + index] = vector5;
			uvs[10 + index] = vector7;
			uvs[11 + index] = vector8;
			float num5 = 0f;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float scale = Mathf.Abs(sdfScale);
			Vector2[] uvs2 = m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = PackUV(0f, 0f, scale);
			uvs2[1 + index] = PackUV(0f, 1f, scale);
			uvs2[2 + index] = PackUV(x, 1f, scale);
			uvs2[3 + index] = PackUV(x, 0f, scale);
			num5 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = PackUV(num5, 0f, scale);
			uvs2[5 + index] = PackUV(num5, 1f, scale);
			uvs2[6 + index] = PackUV(x, 1f, scale);
			uvs2[7 + index] = PackUV(x, 0f, scale);
			num5 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = PackUV(num5, 0f, scale);
			uvs2[9 + index] = PackUV(num5, 1f, scale);
			uvs2[10 + index] = PackUV(1f, 1f, scale);
			uvs2[11 + index] = PackUV(1f, 0f, scale);
			Color32[] colors = m_textInfo.meshInfo[0].colors32;
			colors[index] = underlineColor;
			colors[1 + index] = underlineColor;
			colors[2 + index] = underlineColor;
			colors[3 + index] = underlineColor;
			colors[4 + index] = underlineColor;
			colors[5 + index] = underlineColor;
			colors[6 + index] = underlineColor;
			colors[7 + index] = underlineColor;
			colors[8 + index] = underlineColor;
			colors[9 + index] = underlineColor;
			colors[10 + index] = underlineColor;
			colors[11 + index] = underlineColor;
			index += 12;
		}

		protected virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
		{
			if (m_cached_Underline_GlyphInfo == null)
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
						if (!TMP_Settings.warningsDisabled)
						{
							Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
						}
						return;
					}
				}
			}
			int num = index + 4;
			if (num > m_textInfo.meshInfo[0].vertices.Length)
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
				m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			Vector3[] vertices = m_textInfo.meshInfo[0].vertices;
			vertices[index] = start;
			vertices[index + 1] = new Vector3(start.x, end.y, 0f);
			vertices[index + 2] = end;
			vertices[index + 3] = new Vector3(end.x, start.y, 0f);
			Vector2[] uvs = m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((m_cached_Underline_GlyphInfo.x + m_cached_Underline_GlyphInfo.width / 2f) / m_fontAsset.fontInfo.AtlasWidth, 1f - (m_cached_Underline_GlyphInfo.y + m_cached_Underline_GlyphInfo.height / 2f) / m_fontAsset.fontInfo.AtlasHeight);
			uvs[index] = vector;
			uvs[1 + index] = vector;
			uvs[2 + index] = vector;
			uvs[3 + index] = vector;
			Vector2[] uvs2 = m_textInfo.meshInfo[0].uvs2;
			Vector2 vector2 = new Vector2(0f, 1f);
			uvs2[index] = vector2;
			uvs2[1 + index] = vector2;
			uvs2[2 + index] = vector2;
			uvs2[3 + index] = vector2;
			Color32[] colors = m_textInfo.meshInfo[0].colors32;
			byte a;
			if (m_htmlColor.a < highlightColor.a)
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
				a = m_htmlColor.a;
			}
			else
			{
				a = highlightColor.a;
			}
			highlightColor.a = a;
			colors[index] = highlightColor;
			colors[1 + index] = highlightColor;
			colors[2 + index] = highlightColor;
			colors[3 + index] = highlightColor;
			index += 4;
		}

		protected void LoadDefaultSettings()
		{
			if (m_text == null)
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
						if (TMP_Settings.autoSizeTextContainer)
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
							autoSizeTextContainer = true;
						}
						else
						{
							m_rectTransform = rectTransform;
							if (GetType() == typeof(TextMeshPro))
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
								m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProTextContainerSize;
							}
							else
							{
								m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProUITextContainerSize;
							}
						}
						m_enableWordWrapping = TMP_Settings.enableWordWrapping;
						m_enableKerning = TMP_Settings.enableKerning;
						m_enableExtraPadding = TMP_Settings.enableExtraPadding;
						m_tintAllSprites = TMP_Settings.enableTintAllSprites;
						m_parseCtrlCharacters = TMP_Settings.enableParseEscapeCharacters;
						m_fontSize = (m_fontSizeBase = TMP_Settings.defaultFontSize);
						m_fontSizeMin = m_fontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
						m_fontSizeMax = m_fontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
						m_isAlignmentEnumConverted = true;
						return;
					}
				}
			}
			if (m_isAlignmentEnumConverted)
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
				m_isAlignmentEnumConverted = true;
				m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(m_textAlignment);
				return;
			}
		}

		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			if (!fontAsset.characterDictionary.TryGetValue(95, out m_cached_Underline_GlyphInfo))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			if (!fontAsset.characterDictionary.TryGetValue(8230, out m_cached_Ellipsis_GlyphInfo))
			{
				while (true)
				{
					switch (4)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}

		protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
		{
			chars[insertionIndex] = c;
			for (int i = insertionIndex + tagLength; i < chars.Length; i++)
			{
				chars[i - 3] = chars[i];
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

		protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
		{
			int num;
			if ((m_style & FontStyles.Italic) != FontStyles.Italic)
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
				num = (((m_fontStyle & FontStyles.Italic) == FontStyles.Italic) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			TMP_FontAsset tMP_FontAsset = null;
			int num2 = fontWeight / 100;
			if (flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_currentFontAsset.fontWeights[num2].italicTypeface;
					}
				}
			}
			return m_currentFontAsset.fontWeights[num2].regularTypeface;
		}

		protected virtual void SetActiveSubMeshes(bool state)
		{
		}

		protected virtual void ClearSubMeshObjects()
		{
		}

		public virtual void ClearMesh()
		{
		}

		public virtual void ClearMesh(bool uploadGeometry)
		{
		}

		public virtual string GetParsedText()
		{
			if (m_textInfo == null)
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
						return string.Empty;
					}
				}
			}
			int characterCount = m_textInfo.characterCount;
			char[] array = new char[characterCount];
			for (int i = 0; i < characterCount; i++)
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
				if (i < m_textInfo.characterInfo.Length)
				{
					array[i] = m_textInfo.characterInfo[i].character;
					continue;
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
				break;
			}
			return new string(array);
		}

		protected Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 result = default(Vector2);
			result.x = (int)(x * 511f);
			result.y = (int)(y * 511f);
			result.x = result.x * 4096f + result.y;
			result.y = scale;
			return result;
		}

		protected float PackUV(float x, float y)
		{
			double num = (int)(x * 511f);
			double num2 = (int)(y * 511f);
			return (float)(num * 4096.0 + num2);
		}

		protected int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			case 'a':
				return 10;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			default:
				return 15;
			}
		}

		protected int GetUTF16(int i)
		{
			int num = HexToInt(m_text[i]) << 12;
			num += HexToInt(m_text[i + 1]) << 8;
			num += HexToInt(m_text[i + 2]) << 4;
			return num + HexToInt(m_text[i + 3]);
		}

		protected int GetUTF32(int i)
		{
			int num = 0;
			num += HexToInt(m_text[i]) << 30;
			num += HexToInt(m_text[i + 1]) << 24;
			num += HexToInt(m_text[i + 2]) << 20;
			num += HexToInt(m_text[i + 3]) << 16;
			num += HexToInt(m_text[i + 4]) << 12;
			num += HexToInt(m_text[i + 5]) << 8;
			num += HexToInt(m_text[i + 6]) << 4;
			return num + HexToInt(m_text[i + 7]);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			if (tagCount == 4)
			{
				byte r = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
				byte g = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
				byte b = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (tagCount == 5)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						byte r2 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
						byte g2 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
						byte b2 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
						byte a = (byte)(HexToInt(hexChars[4]) * 16 + HexToInt(hexChars[4]));
						return new Color32(r2, g2, b2, a);
					}
					}
				}
			}
			if (tagCount == 7)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						byte r3 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
						byte g3 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
						byte b3 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
						return new Color32(r3, g3, b3, byte.MaxValue);
					}
					}
				}
			}
			if (tagCount == 9)
			{
				byte r4 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
				byte g4 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
				byte b4 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
				byte a2 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
				return new Color32(r4, g4, b4, a2);
			}
			if (tagCount == 10)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						byte r5 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
						byte g5 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
						byte b5 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
						return new Color32(r5, g5, b5, byte.MaxValue);
					}
					}
				}
			}
			if (tagCount == 11)
			{
				byte r6 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
				byte g6 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
				byte b6 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
				byte a3 = (byte)(HexToInt(hexChars[10]) * 16 + HexToInt(hexChars[10]));
				return new Color32(r6, g6, b6, a3);
			}
			if (tagCount == 13)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						byte r7 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
						byte g7 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
						byte b7 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
						return new Color32(r7, g7, b7, byte.MaxValue);
					}
					}
				}
			}
			if (tagCount == 15)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						byte r8 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
						byte g8 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
						byte b8 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
						byte a4 = (byte)(HexToInt(hexChars[13]) * 16 + HexToInt(hexChars[14]));
						return new Color32(r8, g8, b8, a4);
					}
					}
				}
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			switch (length)
			{
			case 7:
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
					byte r2 = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
					byte g2 = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
					byte b2 = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
					return new Color32(r2, g2, b2, byte.MaxValue);
				}
			case 9:
			{
				byte r = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
				byte g = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
				byte b = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
				byte a = (byte)(HexToInt(hexChars[startIndex + 7]) * 16 + HexToInt(hexChars[startIndex + 8]));
				return new Color32(r, g, b, a);
			}
			default:
				return s_colorWhite;
			}
		}

		private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
		{
			int lastIndex = startIndex;
			int num = 0;
			while (lastIndex < startIndex + length)
			{
				parameters[num] = ConvertToFloat(chars, startIndex, length, out lastIndex);
				length -= lastIndex - startIndex + 1;
				startIndex = lastIndex + 1;
				num++;
			}
			return num;
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length)
		{
			int lastIndex = 0;
			return ConvertToFloat(chars, startIndex, length, out lastIndex);
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
		{
			if (startIndex == 0)
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
						lastIndex = 0;
						return -9999f;
					}
				}
			}
			int num = startIndex + length;
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			int num5 = 1;
			for (int i = startIndex; i < num; i++)
			{
				char c = chars[i];
				if (c == ' ')
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
					continue;
				}
				if (c == '.')
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
					num4 = i;
					num3 = -1;
					continue;
				}
				switch (c)
				{
				case '-':
					num5 = -1;
					continue;
				case '+':
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num5 = 1;
					continue;
				case ',':
					lastIndex = i;
					return num2 * (float)num5;
				}
				if (!char.IsDigit(c))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							lastIndex = i;
							return -9999f;
						}
					}
				}
				switch (num3)
				{
				case 0:
					num2 = chars[i] - 48;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
					num2 = num2 * 10f + (float)(int)chars[i] - 48f;
					break;
				case -1:
					num2 += (float)(chars[i] - 48) * 0.1f;
					break;
				case -2:
					num2 += (float)(chars[i] - 48) * 0.01f;
					break;
				case -3:
					num2 += (float)(chars[i] - 48) * 0.001f;
					break;
				case -4:
					num2 += (float)(chars[i] - 48) * 0.0001f;
					break;
				case -5:
					num2 += (float)(chars[i] - 48) * 1E-05f;
					break;
				}
				if (num4 == 0)
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
					num3++;
				}
				else
				{
					num3--;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				lastIndex = num;
				return num2 *= (float)num5;
			}
		}

		protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			m_xmlAttribute[num2].nameHashCode = 0;
			m_xmlAttribute[num2].valueType = TagType.None;
			m_xmlAttribute[num2].valueHashCode = 0;
			m_xmlAttribute[num2].valueStartIndex = 0;
			m_xmlAttribute[num2].valueLength = 0;
			m_xmlAttribute[1].nameHashCode = 0;
			m_xmlAttribute[2].nameHashCode = 0;
			m_xmlAttribute[3].nameHashCode = 0;
			m_xmlAttribute[4].nameHashCode = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			for (int num3 = startIndex; num3 < chars.Length; num3++)
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
				if (chars[num3] == 0)
				{
					break;
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
				if (num >= m_htmlTag.Length || chars[num3] == 60)
				{
					break;
				}
				if (chars[num3] == 62)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					flag2 = true;
					endIndex = num3;
					m_htmlTag[num] = '\0';
					break;
				}
				m_htmlTag[num] = (char)chars[num3];
				num++;
				if (b == 1)
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
					if (tagType == TagType.None)
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
						if (chars[num3] != 43)
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
							if (chars[num3] != 45)
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
								if (!char.IsDigit((char)chars[num3]))
								{
									if (chars[num3] == 35)
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
										tagType = TagType.ColorValue;
										m_xmlAttribute[num2].valueType = TagType.ColorValue;
										m_xmlAttribute[num2].valueStartIndex = num - 1;
										m_xmlAttribute[num2].valueLength++;
									}
									else if (chars[num3] == 34)
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
										tagType = TagType.StringValue;
										m_xmlAttribute[num2].valueType = TagType.StringValue;
										m_xmlAttribute[num2].valueStartIndex = num;
									}
									else
									{
										tagType = TagType.StringValue;
										m_xmlAttribute[num2].valueType = TagType.StringValue;
										m_xmlAttribute[num2].valueStartIndex = num - 1;
										m_xmlAttribute[num2].valueHashCode = (((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ chars[num3]);
										m_xmlAttribute[num2].valueLength++;
									}
									goto IL_0554;
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
						tagType = TagType.NumericalValue;
						m_xmlAttribute[num2].valueType = TagType.NumericalValue;
						m_xmlAttribute[num2].valueStartIndex = num - 1;
						m_xmlAttribute[num2].valueLength++;
					}
					else if (tagType == TagType.NumericalValue)
					{
						if (chars[num3] != 112)
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
							if (chars[num3] != 101)
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
								if (chars[num3] != 37)
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
									if (chars[num3] != 32)
									{
										if (b != 2)
										{
											m_xmlAttribute[num2].valueLength++;
										}
										goto IL_0554;
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
						b = 2;
						tagType = TagType.None;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagType.None;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
						if (chars[num3] == 101)
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
							tagUnits = TagUnits.FontUnits;
						}
						else if (chars[num3] == 37)
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
							tagUnits = TagUnits.Percentage;
						}
					}
					else if (tagType == TagType.ColorValue)
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
						if (chars[num3] != 32)
						{
							m_xmlAttribute[num2].valueLength++;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							m_xmlAttribute[num2].nameHashCode = 0;
							m_xmlAttribute[num2].valueType = TagType.None;
							m_xmlAttribute[num2].valueHashCode = 0;
							m_xmlAttribute[num2].valueStartIndex = 0;
							m_xmlAttribute[num2].valueLength = 0;
						}
					}
					else if (tagType == TagType.StringValue)
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
						if (chars[num3] != 34)
						{
							m_xmlAttribute[num2].valueHashCode = (((m_xmlAttribute[num2].valueHashCode << 5) + m_xmlAttribute[num2].valueHashCode) ^ chars[num3]);
							m_xmlAttribute[num2].valueLength++;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							m_xmlAttribute[num2].nameHashCode = 0;
							m_xmlAttribute[num2].valueType = TagType.None;
							m_xmlAttribute[num2].valueHashCode = 0;
							m_xmlAttribute[num2].valueStartIndex = 0;
							m_xmlAttribute[num2].valueLength = 0;
						}
					}
				}
				goto IL_0554;
				IL_0554:
				if (chars[num3] == 61)
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
					b = 1;
				}
				if (b == 0)
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
					if (chars[num3] == 32)
					{
						if (flag)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								return false;
							}
						}
						flag = true;
						b = 2;
						tagType = TagType.None;
						num2++;
						m_xmlAttribute[num2].nameHashCode = 0;
						m_xmlAttribute[num2].valueType = TagType.None;
						m_xmlAttribute[num2].valueHashCode = 0;
						m_xmlAttribute[num2].valueStartIndex = 0;
						m_xmlAttribute[num2].valueLength = 0;
					}
				}
				if (b == 0)
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
					m_xmlAttribute[num2].nameHashCode = (m_xmlAttribute[num2].nameHashCode << 3) - m_xmlAttribute[num2].nameHashCode + chars[num3];
				}
				if (b == 2)
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
					if (chars[num3] == 32)
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
						b = 0;
					}
				}
			}
			if (!flag2)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			if (tag_NoParsing)
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
				if (m_xmlAttribute[0].nameHashCode != 53822163 && m_xmlAttribute[0].nameHashCode != 49429939)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
			if (m_xmlAttribute[0].nameHashCode != 53822163)
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
				if (m_xmlAttribute[0].nameHashCode != 49429939)
				{
					if (m_htmlTag[0] == '#')
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
						if (num == 4)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									m_htmlColor = HexCharsToColor(m_htmlTag, num);
									m_colorStack.Add(m_htmlColor);
									return true;
								}
							}
						}
					}
					if (m_htmlTag[0] == '#' && num == 5)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_htmlColor = HexCharsToColor(m_htmlTag, num);
								m_colorStack.Add(m_htmlColor);
								return true;
							}
						}
					}
					if (m_htmlTag[0] == '#')
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
						if (num == 7)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_htmlColor = HexCharsToColor(m_htmlTag, num);
									m_colorStack.Add(m_htmlColor);
									return true;
								}
							}
						}
					}
					if (m_htmlTag[0] == '#')
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
						if (num == 9)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									m_htmlColor = HexCharsToColor(m_htmlTag, num);
									m_colorStack.Add(m_htmlColor);
									return true;
								}
							}
						}
					}
					float num4 = 0f;
					int nameHashCode = m_xmlAttribute[0].nameHashCode;
					int valueHashCode;
					int valueHashCode2;
					int valueHashCode3;
					int valueHashCode7;
					int nameHashCode4;
					int valueHashCode4;
					TMP_FontAsset fontAsset;
					Material material;
					float num21;
					float num22;
					int nameHashCode5;
					MaterialReference materialReference3;
					float num24;
					float num25;
					switch (nameHashCode)
					{
					default:
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
						switch (nameHashCode)
						{
						case 427:
							goto IL_1431;
						case 444:
							goto IL_156e;
						case 446:
							goto IL_165f;
						case 16034505:
							goto IL_1cfd;
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
						if (nameHashCode != 343615334)
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
							if (nameHashCode != 374360934)
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
								if (nameHashCode == 457225591)
								{
									goto IL_1c09;
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
								if (nameHashCode == 514803617)
								{
									goto IL_3a41;
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
								if (nameHashCode != 551025096)
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
									if (nameHashCode != 566686826)
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
										if (nameHashCode == 730022849)
										{
											goto IL_3a41;
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
										if (nameHashCode == 766244328)
										{
											goto IL_3aca;
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
										if (nameHashCode != 781906058)
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
											if (nameHashCode == 1100728678)
											{
												goto IL_3c5d;
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
											if (nameHashCode == 1109349752)
											{
												goto IL_3eaf;
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
											if (nameHashCode == 1109386397)
											{
												goto IL_3288;
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
											if (nameHashCode != 1897350193)
											{
												if (nameHashCode == 1897386838)
												{
													goto IL_337e;
												}
												if (nameHashCode == 2012149182)
												{
													goto IL_1a44;
												}
												if (nameHashCode == -1885698441)
												{
													goto IL_1c09;
												}
												if (nameHashCode == -1883544150)
												{
													goto IL_3a60;
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
												if (nameHashCode != -1847322671)
												{
													if (nameHashCode != -1831660941)
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
														if (nameHashCode != -1690034531)
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
															if (nameHashCode == -1668324918)
															{
																goto IL_3a60;
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
															if (nameHashCode == -1632103439)
															{
																goto IL_3aeb;
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
															if (nameHashCode == -1616441709)
															{
																goto IL_3aaa;
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
															if (nameHashCode != -884817987)
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
																if (nameHashCode == -855002522)
																{
																	goto IL_3c5d;
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
																if (nameHashCode == -842693512)
																{
																	goto IL_3eaf;
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
																if (nameHashCode == -842656867)
																{
																	goto IL_3288;
																}
																if (nameHashCode != -445573839)
																{
																	if (nameHashCode != -445537194)
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
																		if (nameHashCode != -330774850)
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
																			if (nameHashCode != 66)
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
																				if (nameHashCode != 73)
																				{
																					if (nameHashCode == 98)
																					{
																						goto IL_13f7;
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
																					if (nameHashCode != 105)
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
																						if (nameHashCode != 395)
																						{
																							if (nameHashCode != 402)
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
																								if (nameHashCode != 434)
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
																									if (nameHashCode != 656)
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
																										if (nameHashCode != 660)
																										{
																											if (nameHashCode != 670)
																											{
																												if (nameHashCode == 912)
																												{
																													goto IL_41cc;
																												}
																												if (nameHashCode == 916)
																												{
																													goto IL_41c8;
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
																												if (nameHashCode != 926)
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
																													if (nameHashCode == 2959)
																													{
																														break;
																													}
																													if (nameHashCode != 2963)
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
																														if (nameHashCode != 2973)
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
																															if (nameHashCode == 3215)
																															{
																																break;
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
																															if (nameHashCode == 3219)
																															{
																																goto IL_41ca;
																															}
																															if (nameHashCode != 3229)
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
																																if (nameHashCode != 4556)
																																{
																																	if (nameHashCode != 4728)
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
																																		if (nameHashCode != 4742)
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
																																			if (nameHashCode == 6380)
																																			{
																																				goto IL_1c44;
																																			}
																																			if (nameHashCode == 6552)
																																			{
																																				goto IL_177c;
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
																																			if (nameHashCode != 6566)
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
																																				if (nameHashCode != 20677)
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
																																					if (nameHashCode != 20849)
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
																																						if (nameHashCode != 20863)
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
																																							if (nameHashCode == 22501)
																																							{
																																								goto IL_1cf4;
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
																																							if (nameHashCode == 22673)
																																							{
																																								goto IL_181c;
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
																																							if (nameHashCode != 22687)
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
																																								if (nameHashCode == 28511)
																																								{
																																									goto IL_2116;
																																								}
																																								if (nameHashCode == 30245)
																																								{
																																									goto IL_16b0;
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
																																								if (nameHashCode != 30266)
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
																																									if (nameHashCode != 31169)
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
																																										if (nameHashCode != 31191)
																																										{
																																											if (nameHashCode != 32745)
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
																																												if (nameHashCode == 41311)
																																												{
																																													goto IL_2116;
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
																																												if (nameHashCode == 43045)
																																												{
																																													goto IL_16b0;
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
																																												if (nameHashCode == 43066)
																																												{
																																													goto IL_28a9;
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
																																												if (nameHashCode == 43969)
																																												{
																																													goto IL_1de1;
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
																																												if (nameHashCode == 43991)
																																												{
																																													goto IL_1d99;
																																												}
																																												if (nameHashCode != 45545)
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
																																													if (nameHashCode == 141358)
																																													{
																																														goto IL_246b;
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
																																													if (nameHashCode == 143092)
																																													{
																																														goto IL_171e;
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
																																													if (nameHashCode != 143113)
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
																																														if (nameHashCode != 144016)
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
																																															if (nameHashCode != 145592)
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
																																																if (nameHashCode == 154158)
																																																{
																																																	goto IL_246b;
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
																																																if (nameHashCode == 155892)
																																																{
																																																	goto IL_171e;
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
																																																if (nameHashCode == 155913)
																																																{
																																																	goto IL_29f7;
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
																																																if (nameHashCode == 156816)
																																																{
																																																	goto IL_1dea;
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
																																																if (nameHashCode != 158392)
																																																{
																																																	if (nameHashCode == 186285)
																																																	{
																																																		goto IL_2a77;
																																																	}
																																																	if (nameHashCode == 186622)
																																																	{
																																																		goto IL_2855;
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
																																																	if (nameHashCode == 192323)
																																																	{
																																																		goto IL_2c4f;
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
																																																	if (nameHashCode != 227814)
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
																																																		if (nameHashCode != 230446)
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
																																																			if (nameHashCode != 237918)
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
																																																				if (nameHashCode == 275917)
																																																				{
																																																					goto IL_2a77;
																																																				}
																																																				if (nameHashCode == 276254)
																																																				{
																																																					goto IL_2855;
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
																																																				if (nameHashCode == 280416)
																																																				{
																																																					return false;
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
																																																				if (nameHashCode == 281955)
																																																				{
																																																					goto IL_2c4f;
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
																																																				if (nameHashCode == 317446)
																																																				{
																																																					goto IL_40e7;
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
																																																				if (nameHashCode == 320078)
																																																				{
																																																					goto IL_278d;
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
																																																				if (nameHashCode != 327550)
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
																																																					if (nameHashCode == 976214)
																																																					{
																																																						goto IL_2b85;
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
																																																					if (nameHashCode == 982252)
																																																					{
																																																						goto IL_3168;
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
																																																					if (nameHashCode != 1017743)
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
																																																						if (nameHashCode != 1027847)
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
																																																							if (nameHashCode == 1065846)
																																																							{
																																																								goto IL_2b85;
																																																							}
																																																							if (nameHashCode == 1071884)
																																																							{
																																																								goto IL_3168;
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
																																																							if (nameHashCode == 1107375)
																																																							{
																																																								goto IL_41c2;
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
																																																							if (nameHashCode != 1117479)
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
																																																								if (nameHashCode == 1286342)
																																																								{
																																																									goto IL_3f97;
																																																								}
																																																								if (nameHashCode == 1356515)
																																																								{
																																																									goto IL_2f5d;
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
																																																								if (nameHashCode == 1441524)
																																																								{
																																																									goto IL_317d;
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
																																																								if (nameHashCode == 1482398)
																																																								{
																																																									goto IL_3b17;
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
																																																								if (nameHashCode == 1524585)
																																																								{
																																																									goto IL_3095;
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
																																																								if (nameHashCode != 1600507)
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
																																																									if (nameHashCode != 1619421)
																																																									{
																																																										if (nameHashCode == 1750458)
																																																										{
																																																											return false;
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
																																																										if (nameHashCode == 1913798)
																																																										{
																																																											goto IL_3f97;
																																																										}
																																																										if (nameHashCode == 1983971)
																																																										{
																																																											goto IL_2f5d;
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
																																																										if (nameHashCode == 2068980)
																																																										{
																																																											goto IL_317d;
																																																										}
																																																										if (nameHashCode == 2109854)
																																																										{
																																																											goto IL_3b17;
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
																																																										if (nameHashCode == 2152041)
																																																										{
																																																											goto IL_3095;
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
																																																										if (nameHashCode == 2227963)
																																																										{
																																																											goto IL_4062;
																																																										}
																																																										if (nameHashCode != 2246877)
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
																																																											if (nameHashCode != 6815845)
																																																											{
																																																												if (nameHashCode == 6886018)
																																																												{
																																																													goto IL_302b;
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
																																																												if (nameHashCode == 6971027)
																																																												{
																																																													goto IL_3273;
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
																																																												if (nameHashCode != 7011901)
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
																																																													if (nameHashCode != 7054088)
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
																																																														if (nameHashCode != 7130010)
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
																																																															if (nameHashCode == 7443301)
																																																															{
																																																																goto IL_3ffb;
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
																																																															if (nameHashCode == 7513474)
																																																															{
																																																																goto IL_302b;
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
																																																															if (nameHashCode == 7598483)
																																																															{
																																																																goto IL_3273;
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
																																																															if (nameHashCode == 7639357)
																																																															{
																																																																goto IL_3c45;
																																																															}
																																																															if (nameHashCode == 7681544)
																																																															{
																																																																goto IL_3159;
																																																															}
																																																															if (nameHashCode != 7757466)
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
																																																																if (nameHashCode != 9133802)
																																																																{
																																																																	if (nameHashCode != 10723418)
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
																																																																		if (nameHashCode == 11642281)
																																																																		{
																																																																			goto IL_1cfd;
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
																																																																		if (nameHashCode == 13526026)
																																																																		{
																																																																			goto IL_3a89;
																																																																		}
																																																																		if (nameHashCode != 15115642)
																																																																		{
																																																																			if (nameHashCode != 47840323)
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
																																																																				if (nameHashCode != 50348802)
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
																																																																					if (nameHashCode == 52232547)
																																																																					{
																																																																						goto IL_3aaa;
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
																																																																					if (nameHashCode != 54741026)
																																																																					{
																																																																						while (true)
																																																																						{
																																																																							switch (7)
																																																																							{
																																																																							case 0:
																																																																								break;
																																																																							default:
																																																																								if (nameHashCode != 72669687 && nameHashCode != 103415287)
																																																																								{
																																																																									while (true)
																																																																									{
																																																																										switch (1)
																																																																										{
																																																																										case 0:
																																																																											break;
																																																																										default:
																																																																											return false;
																																																																										}
																																																																									}
																																																																								}
																																																																								valueHashCode4 = m_xmlAttribute[0].valueHashCode;
																																																																								if (valueHashCode4 != 764638571)
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
																																																																									if (valueHashCode4 != 523367755)
																																																																									{
																																																																										if (MaterialReferenceManager.TryGetMaterial(valueHashCode4, out material))
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
																																																																											if (m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																											{
																																																																												return false;
																																																																											}
																																																																											m_currentMaterial = material;
																																																																											m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
																																																																											m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
																																																																										}
																																																																										else
																																																																										{
																																																																											material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
																																																																											if (material == null)
																																																																											{
																																																																												while (true)
																																																																												{
																																																																													switch (2)
																																																																													{
																																																																													case 0:
																																																																														break;
																																																																													default:
																																																																														return false;
																																																																													}
																																																																												}
																																																																											}
																																																																											if (m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																											{
																																																																												while (true)
																																																																												{
																																																																													switch (5)
																																																																													{
																																																																													case 0:
																																																																														break;
																																																																													default:
																																																																														return false;
																																																																													}
																																																																												}
																																																																											}
																																																																											MaterialReferenceManager.AddFontMaterial(valueHashCode4, material);
																																																																											m_currentMaterial = material;
																																																																											m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
																																																																											m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
																																																																										}
																																																																										return true;
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
																																																																								if (m_currentFontAsset.atlas.GetInstanceID() != m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																								{
																																																																									while (true)
																																																																									{
																																																																										switch (4)
																																																																										{
																																																																										case 0:
																																																																											break;
																																																																										default:
																																																																											return false;
																																																																										}
																																																																									}
																																																																								}
																																																																								m_currentMaterial = m_materialReferences[0].material;
																																																																								m_currentMaterialIndex = 0;
																																																																								m_materialReferenceStack.Add(m_materialReferences[0]);
																																																																								return true;
																																																																							}
																																																																						}
																																																																					}
																																																																				}
																																																																				m_baselineOffset = 0f;
																																																																				return true;
																																																																			}
																																																																			goto IL_3aaa;
																																																																		}
																																																																	}
																																																																	tag_NoParsing = true;
																																																																	return true;
																																																																}
																																																																goto IL_3a89;
																																																															}
																																																														}
																																																														m_isFXMatrixSet = false;
																																																														return true;
																																																													}
																																																													goto IL_3159;
																																																												}
																																																												goto IL_3c45;
																																																											}
																																																											goto IL_3ffb;
																																																										}
																																																									}
																																																									int valueHashCode5 = m_xmlAttribute[0].valueHashCode;
																																																									m_spriteIndex = -1;
																																																									if (m_xmlAttribute[0].valueType != 0)
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
																																																										if (m_xmlAttribute[0].valueType != TagType.NumericalValue)
																																																										{
																																																											if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode5, out TMP_SpriteAsset spriteAsset))
																																																											{
																																																												m_currentSpriteAsset = spriteAsset;
																																																											}
																																																											else
																																																											{
																																																												if (spriteAsset == null)
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
																																																													spriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
																																																												}
																																																												if (spriteAsset == null)
																																																												{
																																																													return false;
																																																												}
																																																												MaterialReferenceManager.AddSpriteAsset(valueHashCode5, spriteAsset);
																																																												m_currentSpriteAsset = spriteAsset;
																																																											}
																																																											goto IL_353f;
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
																																																									if (m_spriteAsset != null)
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
																																																										m_currentSpriteAsset = m_spriteAsset;
																																																									}
																																																									else if (m_defaultSpriteAsset != null)
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
																																																										m_currentSpriteAsset = m_defaultSpriteAsset;
																																																									}
																																																									else if (m_defaultSpriteAsset == null)
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
																																																										if (TMP_Settings.defaultSpriteAsset != null)
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
																																																											m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
																																																										}
																																																										else
																																																										{
																																																											m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
																																																										}
																																																										m_currentSpriteAsset = m_defaultSpriteAsset;
																																																									}
																																																									if (m_currentSpriteAsset == null)
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
																																																									goto IL_353f;
																																																								}
																																																								goto IL_4062;
																																																							}
																																																						}
																																																						m_width = -1f;
																																																						return true;
																																																					}
																																																					goto IL_41c2;
																																																				}
																																																			}
																																																			num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
																																																			if (num4 != -9999f)
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
																																																				if (num4 != 0f)
																																																				{
																																																					if (tagUnits != 0)
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
																																																						if (tagUnits == TagUnits.FontUnits)
																																																						{
																																																							return false;
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
																																																						if (tagUnits != TagUnits.Percentage)
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
																																																						}
																																																						else
																																																						{
																																																							m_width = m_marginWidth * num4 / 100f;
																																																						}
																																																					}
																																																					else
																																																					{
																																																						m_width = num4;
																																																					}
																																																					return true;
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
																																																			return false;
																																																		}
																																																		goto IL_278d;
																																																	}
																																																	goto IL_40e7;
																																																}
																																															}
																																															m_currentFontSize = m_sizeStack.Remove();
																																															float num5 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
																																															float num6;
																																															if (m_isOrthographic)
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
																																																num6 = 1f;
																																															}
																																															else
																																															{
																																																num6 = 0.1f;
																																															}
																																															m_fontScale = num5 * num6;
																																															return true;
																																														}
																																														goto IL_1dea;
																																													}
																																													goto IL_29f7;
																																												}
																																											}
																																											num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
																																											if (num4 == -9999f)
																																											{
																																												while (true)
																																												{
																																													switch (4)
																																													{
																																													case 0:
																																														break;
																																													default:
																																														return false;
																																													}
																																												}
																																											}
																																											switch (tagUnits)
																																											{
																																											default:
																																												while (true)
																																												{
																																													switch (1)
																																													{
																																													case 0:
																																														continue;
																																													}
																																													return false;
																																												}
																																											case TagUnits.Pixels:
																																												if (m_htmlTag[5] == '+')
																																												{
																																													while (true)
																																													{
																																														switch (4)
																																														{
																																														case 0:
																																															break;
																																														default:
																																														{
																																															m_currentFontSize = m_fontSize + num4;
																																															m_sizeStack.Add(m_currentFontSize);
																																															float num7 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
																																															float num8;
																																															if (m_isOrthographic)
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
																																																num8 = 1f;
																																															}
																																															else
																																															{
																																																num8 = 0.1f;
																																															}
																																															m_fontScale = num7 * num8;
																																															return true;
																																														}
																																														}
																																													}
																																												}
																																												if (m_htmlTag[5] == '-')
																																												{
																																													while (true)
																																													{
																																														switch (5)
																																														{
																																														case 0:
																																															break;
																																														default:
																																														{
																																															m_currentFontSize = m_fontSize + num4;
																																															m_sizeStack.Add(m_currentFontSize);
																																															float num9 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
																																															float num10;
																																															if (m_isOrthographic)
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
																																																num10 = 1f;
																																															}
																																															else
																																															{
																																																num10 = 0.1f;
																																															}
																																															m_fontScale = num9 * num10;
																																															return true;
																																														}
																																														}
																																													}
																																												}
																																												m_currentFontSize = num4;
																																												m_sizeStack.Add(m_currentFontSize);
																																												m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * ((!m_isOrthographic) ? 0.1f : 1f);
																																												return true;
																																											case TagUnits.FontUnits:
																																											{
																																												m_currentFontSize = m_fontSize * num4;
																																												m_sizeStack.Add(m_currentFontSize);
																																												float num11 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
																																												float num12;
																																												if (m_isOrthographic)
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
																																													num12 = 1f;
																																												}
																																												else
																																												{
																																													num12 = 0.1f;
																																												}
																																												m_fontScale = num11 * num12;
																																												return true;
																																											}
																																											case TagUnits.Percentage:
																																												m_currentFontSize = m_fontSize * num4 / 100f;
																																												m_sizeStack.Add(m_currentFontSize);
																																												m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * ((!m_isOrthographic) ? 0.1f : 1f);
																																												return true;
																																											}
																																										}
																																										goto IL_1d99;
																																									}
																																									goto IL_1de1;
																																								}
																																								goto IL_28a9;
																																							}
																																						}
																																						if ((m_style & FontStyles.Superscript) == FontStyles.Superscript)
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
																																							if (m_fontScaleMultiplier < 1f)
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
																																								m_baselineOffset = m_baselineOffsetStack.Pop();
																																								float fontScaleMultiplier = m_fontScaleMultiplier;
																																								float num13;
																																								if (m_currentFontAsset.fontInfo.SubSize > 0f)
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
																																									num13 = m_currentFontAsset.fontInfo.SubSize;
																																								}
																																								else
																																								{
																																									num13 = 1f;
																																								}
																																								m_fontScaleMultiplier = fontScaleMultiplier / num13;
																																							}
																																							if (m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
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
																																								m_style &= (FontStyles)(-129);
																																							}
																																						}
																																						return true;
																																					}
																																					goto IL_181c;
																																				}
																																				goto IL_1cf4;
																																			}
																																		}
																																		m_fontScaleMultiplier *= ((!(m_currentFontAsset.fontInfo.SubSize > 0f)) ? 1f : m_currentFontAsset.fontInfo.SubSize);
																																		m_baselineOffsetStack.Push(m_baselineOffset);
																																		m_baselineOffset += m_currentFontAsset.fontInfo.SuperscriptOffset * m_fontScale * m_fontScaleMultiplier;
																																		m_fontStyleStack.Add(FontStyles.Superscript);
																																		m_style |= FontStyles.Superscript;
																																		return true;
																																	}
																																	goto IL_177c;
																																}
																																goto IL_1c44;
																															}
																														}
																														return true;
																													}
																													goto IL_41ca;
																												}
																											}
																											return true;
																										}
																										goto IL_41c8;
																									}
																									goto IL_41cc;
																								}
																							}
																							if (m_fontStyleStack.Remove(FontStyles.Italic) == 0)
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
																								m_style &= (FontStyles)(-3);
																							}
																							return true;
																						}
																						goto IL_1431;
																					}
																				}
																				m_style |= FontStyles.Italic;
																				m_fontStyleStack.Add(FontStyles.Italic);
																				return true;
																			}
																			goto IL_13f7;
																		}
																		goto IL_1a44;
																	}
																	goto IL_337e;
																}
																goto IL_3f81;
															}
														}
														num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
														if (num4 != -9999f)
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
															if (num4 != 0f)
															{
																m_marginRight = num4;
																if (tagUnits != 0)
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
																	if (tagUnits != TagUnits.FontUnits)
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
																		if (tagUnits != TagUnits.Percentage)
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
																		}
																		else
																		{
																			float marginWidth = m_marginWidth;
																			float num14;
																			if (m_width != -1f)
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
																				num14 = m_width;
																			}
																			else
																			{
																				num14 = 0f;
																			}
																			m_marginRight = (marginWidth - num14) * m_marginRight / 100f;
																		}
																	}
																	else
																	{
																		m_marginRight *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
																	}
																}
																float marginRight;
																if (m_marginRight >= 0f)
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
																	marginRight = m_marginRight;
																}
																else
																{
																	marginRight = 0f;
																}
																m_marginRight = marginRight;
																return true;
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
														return false;
													}
													goto IL_3aaa;
												}
												goto IL_3aeb;
											}
											goto IL_3f81;
										}
									}
									goto IL_3a89;
								}
								goto IL_3aca;
							}
						}
						int instanceID = m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID();
						MaterialReference materialReference = m_materialReferenceStack.PreviousItem();
						if (instanceID != materialReference.material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						MaterialReference materialReference2 = m_materialReferenceStack.Remove();
						m_currentMaterial = materialReference2.material;
						m_currentMaterialIndex = materialReference2.index;
						return true;
					}
					case 83:
					case 115:
						m_style |= FontStyles.Strikethrough;
						m_fontStyleStack.Add(FontStyles.Strikethrough);
						if (m_xmlAttribute[1].nameHashCode != 281955)
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
							if (m_xmlAttribute[1].nameHashCode != 192323)
							{
								m_strikethroughColor = m_htmlColor;
								goto IL_155b;
							}
						}
						m_strikethroughColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
						goto IL_155b;
					case 412:
						goto IL_156e;
					case 85:
					case 117:
						m_style |= FontStyles.Underline;
						m_fontStyleStack.Add(FontStyles.Underline);
						if (m_xmlAttribute[1].nameHashCode != 281955)
						{
							if (m_xmlAttribute[1].nameHashCode != 192323)
							{
								m_underlineColor = m_htmlColor;
								goto IL_164c;
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
						m_underlineColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
						goto IL_164c;
					case 414:
						goto IL_165f;
					case 426:
						{
							return true;
						}
						IL_3f97:
						valueHashCode = m_xmlAttribute[0].valueHashCode;
						if (m_isParsingText)
						{
							m_actionStack.Add(valueHashCode);
							Debug.Log("Action ID: [" + valueHashCode + "] First character index: " + m_characterCount);
						}
						return true;
						IL_2a77:
						valueHashCode2 = m_xmlAttribute[0].valueHashCode;
						if (valueHashCode2 != -523808257)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									if (valueHashCode2 != -458210101)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												break;
											default:
												switch (valueHashCode2)
												{
												default:
													while (true)
													{
														switch (3)
														{
														case 0:
															break;
														default:
															if (valueHashCode2 != 136703040)
															{
																while (true)
																{
																	switch (5)
																	{
																	case 0:
																		break;
																	default:
																		return false;
																	}
																}
															}
															m_lineJustification = TextAlignmentOptions.Right;
															m_lineJustificationStack.Add(m_lineJustification);
															return true;
														}
													}
												case 3774683:
													m_lineJustification = TextAlignmentOptions.Left;
													m_lineJustificationStack.Add(m_lineJustification);
													return true;
												case 122383428:
													m_lineJustification = TextAlignmentOptions.Flush;
													m_lineJustificationStack.Add(m_lineJustification);
													return true;
												}
											}
										}
									}
									m_lineJustification = TextAlignmentOptions.Center;
									m_lineJustificationStack.Add(m_lineJustification);
									return true;
								}
							}
						}
						m_lineJustification = TextAlignmentOptions.Justified;
						m_lineJustificationStack.Add(m_lineJustification);
						return true;
						IL_41c2:
						return true;
						IL_2f5d:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits == TagUnits.Percentage)
										{
											return false;
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
									else
									{
										m_cSpacing = num4;
										m_cSpacing *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								else
								{
									m_cSpacing = num4;
								}
								return true;
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
						return false;
						IL_164c:
						m_underlineColorStack.Add(m_underlineColor);
						return true;
						IL_41ca:
						return true;
						IL_2c4f:
						if (m_htmlTag[6] == '#')
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
							if (num == 10)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										m_htmlColor = HexCharsToColor(m_htmlTag, num);
										m_colorStack.Add(m_htmlColor);
										return true;
									}
								}
							}
						}
						if (m_htmlTag[6] == '#')
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
							if (num == 11)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										m_htmlColor = HexCharsToColor(m_htmlTag, num);
										m_colorStack.Add(m_htmlColor);
										return true;
									}
								}
							}
						}
						if (m_htmlTag[6] == '#' && num == 13)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									m_htmlColor = HexCharsToColor(m_htmlTag, num);
									m_colorStack.Add(m_htmlColor);
									return true;
								}
							}
						}
						if (m_htmlTag[6] == '#')
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
							if (num == 15)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										m_htmlColor = HexCharsToColor(m_htmlTag, num);
										m_colorStack.Add(m_htmlColor);
										return true;
									}
								}
							}
						}
						valueHashCode3 = m_xmlAttribute[0].valueHashCode;
						if (valueHashCode3 != -36881330)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									if (valueHashCode3 != 125395)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												switch (valueHashCode3)
												{
												default:
													while (true)
													{
														switch (5)
														{
														case 0:
															break;
														default:
															switch (valueHashCode3)
															{
															default:
																while (true)
																{
																	switch (6)
																	{
																	case 0:
																		break;
																	default:
																		if (valueHashCode3 != 554054276)
																		{
																			while (true)
																			{
																				switch (7)
																				{
																				case 0:
																					break;
																				default:
																					return false;
																				}
																			}
																		}
																		m_htmlColor = Color.yellow;
																		m_colorStack.Add(m_htmlColor);
																		return true;
																	}
																}
															case 121463835:
																m_htmlColor = Color.green;
																m_colorStack.Add(m_htmlColor);
																return true;
															case 140357351:
																m_htmlColor = Color.white;
																m_colorStack.Add(m_htmlColor);
																return true;
															}
														}
													}
												case 3573310:
													m_htmlColor = Color.blue;
													m_colorStack.Add(m_htmlColor);
													return true;
												case 117905991:
													m_htmlColor = Color.black;
													m_colorStack.Add(m_htmlColor);
													return true;
												case 26556144:
													m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
													m_colorStack.Add(m_htmlColor);
													return true;
												}
											}
										}
									}
									m_htmlColor = Color.red;
									m_colorStack.Add(m_htmlColor);
									return true;
								}
							}
						}
						m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
						m_colorStack.Add(m_htmlColor);
						return true;
						IL_2b85:
						m_lineJustification = m_lineJustificationStack.Remove();
						return true;
						IL_3168:
						m_htmlColor = m_colorStack.Remove();
						return true;
						IL_155b:
						m_strikethroughColorStack.Add(m_strikethroughColor);
						return true;
						IL_41c8:
						return true;
						IL_165f:
						if ((m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
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
							m_underlineColor = m_underlineColorStack.Remove();
							if (m_fontStyleStack.Remove(FontStyles.Underline) == 0)
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
								m_style &= (FontStyles)(-5);
							}
						}
						return true;
						IL_3aca:
						m_style |= FontStyles.SmallCaps;
						m_fontStyleStack.Add(FontStyles.SmallCaps);
						return true;
						IL_3a41:
						m_style |= FontStyles.LowerCase;
						m_fontStyleStack.Add(FontStyles.LowerCase);
						return true;
						IL_1c09:
						m_fontWeightInternal = m_fontWeightStack.Remove();
						if (m_fontWeightInternal == 400)
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
							m_style &= (FontStyles)(-2);
						}
						return true;
						IL_3aaa:
						if (m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
						{
							m_style &= (FontStyles)(-17);
						}
						return true;
						IL_3a89:
						m_style |= FontStyles.UpperCase;
						m_fontStyleStack.Add(FontStyles.UpperCase);
						return true;
						IL_156e:
						if ((m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough)
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
							if (m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
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
								m_style &= (FontStyles)(-65);
							}
						}
						return true;
						IL_3288:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits != TagUnits.Percentage)
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
										}
										else
										{
											tag_LineIndent = m_marginWidth * num4 / 100f;
										}
									}
									else
									{
										tag_LineIndent = num4;
										tag_LineIndent *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								else
								{
									tag_LineIndent = num4;
								}
								m_xAdvance += tag_LineIndent;
								return true;
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
						return false;
						IL_353f:
						if (m_xmlAttribute[0].valueType == TagType.NumericalValue)
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
							int num15 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
							if (num15 == -9999)
							{
								return false;
							}
							if (num15 > m_currentSpriteAsset.spriteInfoList.Count - 1)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									return false;
								}
							}
							m_spriteIndex = num15;
						}
						else if (m_xmlAttribute[1].nameHashCode == 43347 || m_xmlAttribute[1].nameHashCode == 30547)
						{
							int spriteIndexFromHashcode = m_currentSpriteAsset.GetSpriteIndexFromHashcode(m_xmlAttribute[1].valueHashCode);
							if (spriteIndexFromHashcode == -1)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									return false;
								}
							}
							m_spriteIndex = spriteIndexFromHashcode;
						}
						else
						{
							if (m_xmlAttribute[1].nameHashCode != 295562 && m_xmlAttribute[1].nameHashCode != 205930)
							{
								return false;
							}
							int num16 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
							if (num16 == -9999)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									return false;
								}
							}
							if (num16 > m_currentSpriteAsset.spriteInfoList.Count - 1)
							{
								return false;
							}
							m_spriteIndex = num16;
						}
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
						m_spriteColor = s_colorWhite;
						m_tintSprite = false;
						for (int i = 0; i < m_xmlAttribute.Length; i++)
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
							int num17;
							if (m_xmlAttribute[i].nameHashCode != 0)
							{
								int nameHashCode2 = m_xmlAttribute[i].nameHashCode;
								num17 = 0;
								if (nameHashCode2 != 26705)
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
									if (nameHashCode2 != 30547)
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
										if (nameHashCode2 != 33019)
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
											if (nameHashCode2 == 39505)
											{
												goto IL_3907;
											}
											if (nameHashCode2 == 43347)
											{
												goto IL_37e1;
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
											if (nameHashCode2 != 45819)
											{
												if (nameHashCode2 != 192323)
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
													if (nameHashCode2 != 205930)
													{
														if (nameHashCode2 == 281955)
														{
															goto IL_38ca;
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
														if (nameHashCode2 != 295562)
														{
															if (nameHashCode2 == 2246877)
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
															if (nameHashCode2 == 1619421)
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
																return false;
															}
														}
													}
													num17 = (int)ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
													if (num17 == -9999)
													{
														while (true)
														{
															switch (7)
															{
															case 0:
																break;
															default:
																return false;
															}
														}
													}
													if (num17 > m_currentSpriteAsset.spriteInfoList.Count - 1)
													{
														return false;
													}
													m_spriteIndex = num17;
													continue;
												}
												goto IL_38ca;
											}
										}
										m_tintSprite = (ConvertToFloat(m_htmlTag, m_xmlAttribute[i].valueStartIndex, m_xmlAttribute[i].valueLength) != 0f);
										continue;
									}
									goto IL_37e1;
								}
								goto IL_3907;
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
							IL_37e1:
							num17 = m_currentSpriteAsset.GetSpriteIndexFromHashcode(m_xmlAttribute[i].valueHashCode);
							if (num17 == -1)
							{
								return false;
							}
							m_spriteIndex = num17;
							continue;
							IL_3907:
							int attributeParameters = GetAttributeParameters(m_htmlTag, m_xmlAttribute[i].valueStartIndex, m_xmlAttribute[i].valueLength, ref m_attributeParameterValues);
							if (attributeParameters != 3)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										return false;
									}
								}
							}
							m_spriteIndex = (int)m_attributeParameterValues[0];
							if (m_isParsingText)
							{
								spriteAnimator.DoSpriteAnimation(m_characterCount, m_currentSpriteAsset, m_spriteIndex, (int)m_attributeParameterValues[1], (int)m_attributeParameterValues[2]);
							}
							continue;
							IL_38ca:
							m_spriteColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[i].valueStartIndex, m_xmlAttribute[i].valueLength);
						}
						if (m_spriteIndex == -1)
						{
							return false;
						}
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentSpriteAsset.material, m_currentSpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
						m_textElementType = TMP_TextElementType.Sprite;
						return true;
						IL_3eaf:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 == -9999f)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						m_lineHeight = num4;
						if (tagUnits != 0)
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
							if (tagUnits != TagUnits.FontUnits)
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
								if (tagUnits != TagUnits.Percentage)
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
								}
								else
								{
									m_lineHeight = m_fontAsset.fontInfo.LineHeight * m_lineHeight / 100f * m_fontScale;
								}
							}
							else
							{
								m_lineHeight *= m_fontAsset.fontInfo.LineHeight * m_fontScale;
							}
						}
						return true;
						IL_3ffb:
						if (m_isParsingText)
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
							Debug.Log("Action ID: [" + m_actionStack.CurrentItem() + "] Last character index: " + (m_characterCount - 1));
						}
						m_actionStack.Remove();
						return true;
						IL_337e:
						tag_LineIndent = 0f;
						return true;
						IL_171e:
						if ((m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
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
							m_highlightColor = m_highlightColorStack.Remove();
							if (m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
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
								m_style &= (FontStyles)(-513);
							}
						}
						return true;
						IL_1cfd:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								switch (tagUnits)
								{
								case TagUnits.Pixels:
									m_baselineOffset = num4;
									return true;
								case TagUnits.FontUnits:
									m_baselineOffset = num4 * m_fontScale * m_fontAsset.fontInfo.Ascender;
									return true;
								case TagUnits.Percentage:
									return false;
								default:
									return false;
								}
							}
						}
						return false;
						IL_3a60:
						if (m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
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
							m_style &= (FontStyles)(-9);
						}
						return true;
						IL_3f81:
						m_lineHeight = -32767f;
						return true;
						IL_3c5d:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								m_marginLeft = num4;
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits == TagUnits.Percentage)
										{
											m_marginLeft = (m_marginWidth - ((m_width == -1f) ? 0f : m_width)) * m_marginLeft / 100f;
										}
									}
									else
									{
										m_marginLeft *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								float marginLeft;
								if (m_marginLeft >= 0f)
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
									marginLeft = m_marginLeft;
								}
								else
								{
									marginLeft = 0f;
								}
								m_marginLeft = marginLeft;
								return true;
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
						return false;
						IL_1c44:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 == -9999f)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						switch (tagUnits)
						{
						default:
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								return false;
							}
						case TagUnits.Pixels:
							m_xAdvance = num4;
							return true;
						case TagUnits.FontUnits:
							m_xAdvance = num4 * m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
							return true;
						case TagUnits.Percentage:
							m_xAdvance = m_marginWidth * num4 / 100f;
							return true;
						}
						IL_3159:
						m_monoSpacing = 0f;
						return true;
						IL_3c45:
						m_marginLeft = 0f;
						m_marginRight = 0f;
						return true;
						IL_302b:
						if (!m_isParsingText)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
						if (m_characterCount > 0)
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
							m_xAdvance -= m_cSpacing;
							m_textInfo.characterInfo[m_characterCount - 1].xAdvance = m_xAdvance;
						}
						m_cSpacing = 0f;
						return true;
						IL_2855:
						if (m_xmlAttribute[0].valueLength != 3)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						m_htmlColor.a = (byte)(HexToInt(m_htmlTag[7]) * 16 + HexToInt(m_htmlTag[8]));
						return true;
						IL_13f7:
						m_style |= FontStyles.Bold;
						m_fontStyleStack.Add(FontStyles.Bold);
						m_fontWeightInternal = 700;
						m_fontWeightStack.Add(700);
						return true;
						IL_181c:
						if ((m_style & FontStyles.Subscript) == FontStyles.Subscript)
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
							if (m_fontScaleMultiplier < 1f)
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
								m_baselineOffset = m_baselineOffsetStack.Pop();
								float fontScaleMultiplier2 = m_fontScaleMultiplier;
								float num18;
								if (m_currentFontAsset.fontInfo.SubSize > 0f)
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
									num18 = m_currentFontAsset.fontInfo.SubSize;
								}
								else
								{
									num18 = 1f;
								}
								m_fontScaleMultiplier = fontScaleMultiplier2 / num18;
							}
							if (m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
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
								m_style &= (FontStyles)(-257);
							}
						}
						return true;
						IL_41cc:
						for (int j = 1; j < m_xmlAttribute.Length; j++)
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
							if (m_xmlAttribute[j].nameHashCode != 0)
							{
								int nameHashCode3 = m_xmlAttribute[j].nameHashCode;
								if (nameHashCode3 != 327550)
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
									if (nameHashCode3 != 275917)
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
										continue;
									}
									int valueHashCode6 = m_xmlAttribute[j].valueHashCode;
									if (valueHashCode6 != -523808257)
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
										switch (valueHashCode6)
										{
										case 3774683:
											Debug.Log("TD align=\"left\".");
											continue;
										case -458210101:
											Debug.Log("TD align=\"center\".");
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
										if (valueHashCode6 != 136703040)
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
										}
										else
										{
											Debug.Log("TD align=\"right\".");
										}
									}
									else
									{
										Debug.Log("TD align=\"justified\".");
									}
									continue;
								}
								float num19 = ConvertToFloat(m_htmlTag, m_xmlAttribute[j].valueStartIndex, m_xmlAttribute[j].valueLength);
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits != TagUnits.Percentage)
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
											Debug.Log("Table width = " + num19 + "%.");
										}
									}
									else
									{
										Debug.Log("Table width = " + num19 + "em.");
									}
								}
								else
								{
									Debug.Log("Table width = " + num19 + "px.");
								}
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
						return true;
						IL_1a44:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if ((m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
								{
									return true;
								}
								m_style &= (FontStyles)(-2);
								int num20 = (int)num4;
								if (num20 != 100)
								{
									if (num20 != 200)
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
										if (num20 != 300)
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
											if (num20 != 400)
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
												if (num20 != 500)
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
													if (num20 != 600)
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
														if (num20 != 700)
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
															if (num20 != 800)
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
																if (num20 == 900)
																{
																	m_fontWeightInternal = 900;
																}
															}
															else
															{
																m_fontWeightInternal = 800;
															}
														}
														else
														{
															m_fontWeightInternal = 700;
															m_style |= FontStyles.Bold;
														}
													}
													else
													{
														m_fontWeightInternal = 600;
													}
												}
												else
												{
													m_fontWeightInternal = 500;
												}
											}
											else
											{
												m_fontWeightInternal = 400;
											}
										}
										else
										{
											m_fontWeightInternal = 300;
										}
									}
									else
									{
										m_fontWeightInternal = 200;
									}
								}
								else
								{
									m_fontWeightInternal = 100;
								}
								m_fontWeightStack.Add(m_fontWeightInternal);
								return true;
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
						return false;
						IL_177c:
						m_fontScaleMultiplier *= ((!(m_currentFontAsset.fontInfo.SubSize > 0f)) ? 1f : m_currentFontAsset.fontInfo.SubSize);
						m_baselineOffsetStack.Push(m_baselineOffset);
						m_baselineOffset += m_currentFontAsset.fontInfo.SubscriptOffset * m_fontScale * m_fontScaleMultiplier;
						m_fontStyleStack.Add(FontStyles.Subscript);
						m_style |= FontStyles.Subscript;
						return true;
						IL_3273:
						tag_Indent = m_indentStack.Remove();
						return true;
						IL_1431:
						if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
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
							m_fontWeightInternal = m_fontWeightStack.Remove();
							if (m_fontStyleStack.Remove(FontStyles.Bold) == 0)
							{
								m_style &= (FontStyles)(-2);
							}
						}
						return true;
						IL_1cf4:
						m_isIgnoringAlignment = false;
						return true;
						IL_3aeb:
						if (m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
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
							m_style &= (FontStyles)(-33);
						}
						return true;
						IL_278d:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if (tagUnits != 0)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											if (tagUnits != TagUnits.FontUnits)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														if (tagUnits != TagUnits.Percentage)
														{
															while (true)
															{
																switch (1)
																{
																case 0:
																	break;
																default:
																	return false;
																}
															}
														}
														return false;
													}
												}
											}
											m_xAdvance += num4 * m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
											return true;
										}
									}
								}
								m_xAdvance += num4;
								return true;
							}
						}
						return false;
						IL_4062:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 == -9999f)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return false;
								}
							}
						}
						m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num4), Vector3.one);
						m_isFXMatrixSet = true;
						return true;
						IL_3095:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits == TagUnits.Percentage)
										{
											return false;
										}
									}
									else
									{
										m_monoSpacing = num4;
										m_monoSpacing *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								else
								{
									m_monoSpacing = num4;
								}
								return true;
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
						return false;
						IL_2116:
						valueHashCode7 = m_xmlAttribute[0].valueHashCode;
						nameHashCode4 = m_xmlAttribute[1].nameHashCode;
						valueHashCode4 = m_xmlAttribute[1].valueHashCode;
						if (valueHashCode7 != 764638571)
						{
							if (valueHashCode7 != 523367755)
							{
								if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode7, out fontAsset))
								{
									fontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength));
									if (fontAsset == null)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											return false;
										}
									}
									MaterialReferenceManager.AddFontAsset(fontAsset);
								}
								if (nameHashCode4 == 0)
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
									if (valueHashCode4 == 0)
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
										m_currentMaterial = fontAsset.material;
										m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
										m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
										goto IL_2410;
									}
								}
								if (nameHashCode4 != 103415287)
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
									if (nameHashCode4 != 72669687)
									{
										return false;
									}
								}
								if (MaterialReferenceManager.TryGetMaterial(valueHashCode4, out material))
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
									m_currentMaterial = material;
									m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
									m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
								}
								else
								{
									material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength));
									if (material == null)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											return false;
										}
									}
									MaterialReferenceManager.AddFontMaterial(valueHashCode4, material);
									m_currentMaterial = material;
									m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, fontAsset, m_materialReferences, m_materialReferenceIndexLookup);
									m_materialReferenceStack.Add(m_materialReferences[m_currentMaterialIndex]);
								}
								goto IL_2410;
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
						m_currentFontAsset = m_materialReferences[0].fontAsset;
						m_currentMaterial = m_materialReferences[0].material;
						m_currentMaterialIndex = 0;
						m_fontScale = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * ((!m_isOrthographic) ? 0.1f : 1f);
						m_materialReferenceStack.Add(m_materialReferences[0]);
						return true;
						IL_1d99:
						if (m_overflowMode == TextOverflowModes.Page)
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
							m_xAdvance = tag_LineIndent + tag_Indent;
							m_lineOffset = 0f;
							m_pageNumber++;
							m_isNewPage = true;
						}
						return true;
						IL_16b0:
						m_style |= FontStyles.Highlight;
						m_fontStyleStack.Add(FontStyles.Highlight);
						m_highlightColor = HexCharsToColor(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						m_highlightColorStack.Add(m_highlightColor);
						return true;
						IL_2410:
						m_currentFontAsset = fontAsset;
						num21 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
						if (m_isOrthographic)
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
							num22 = 1f;
						}
						else
						{
							num22 = 0.1f;
						}
						m_fontScale = num21 * num22;
						return true;
						IL_40e7:
						nameHashCode5 = m_xmlAttribute[1].nameHashCode;
						if (nameHashCode5 != 327550)
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
						}
						else
						{
							float num23 = ConvertToFloat(m_htmlTag, m_xmlAttribute[1].valueStartIndex, m_xmlAttribute[1].valueLength);
							if (tagUnits != 0)
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
								if (tagUnits != TagUnits.FontUnits)
								{
									if (tagUnits != TagUnits.Percentage)
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
									}
									else
									{
										Debug.Log("Table width = " + num23 + "%.");
									}
								}
								else
								{
									Debug.Log("Table width = " + num23 + "em.");
								}
							}
							else
							{
								Debug.Log("Table width = " + num23 + "px.");
							}
						}
						return true;
						IL_1de1:
						m_isNonBreakingSpace = true;
						return true;
						IL_28a9:
						if (m_isParsingText && !m_isCalculatingPreferredValues)
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
							int linkCount = m_textInfo.linkCount;
							if (linkCount + 1 > m_textInfo.linkInfo.Length)
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
								TMP_TextInfo.Resize(ref m_textInfo.linkInfo, linkCount + 1);
							}
							m_textInfo.linkInfo[linkCount].textComponent = this;
							m_textInfo.linkInfo[linkCount].hashCode = m_xmlAttribute[0].valueHashCode;
							m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = m_characterCount;
							m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + m_xmlAttribute[0].valueStartIndex;
							m_textInfo.linkInfo[linkCount].linkIdLength = m_xmlAttribute[0].valueLength;
							m_textInfo.linkInfo[linkCount].SetLinkID(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						}
						return true;
						IL_29f7:
						if (m_isParsingText)
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
							if (!m_isCalculatingPreferredValues)
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
								m_textInfo.linkInfo[m_textInfo.linkCount].linkTextLength = m_characterCount - m_textInfo.linkInfo[m_textInfo.linkCount].linkTextfirstCharacterIndex;
								m_textInfo.linkCount++;
							}
						}
						return true;
						IL_246b:
						materialReference3 = m_materialReferenceStack.Remove();
						m_currentFontAsset = materialReference3.fontAsset;
						m_currentMaterial = materialReference3.material;
						m_currentMaterialIndex = materialReference3.index;
						num24 = m_currentFontSize / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
						if (m_isOrthographic)
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
							num25 = 1f;
						}
						else
						{
							num25 = 0.1f;
						}
						m_fontScale = num24 * num25;
						return true;
						IL_317d:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits != TagUnits.Percentage)
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
										}
										else
										{
											tag_Indent = m_marginWidth * num4 / 100f;
										}
									}
									else
									{
										tag_Indent = num4;
										tag_Indent *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								else
								{
									tag_Indent = num4;
								}
								m_indentStack.Add(tag_Indent);
								m_xAdvance = tag_Indent;
								return true;
							}
						}
						return false;
						IL_3b17:
						num4 = ConvertToFloat(m_htmlTag, m_xmlAttribute[0].valueStartIndex, m_xmlAttribute[0].valueLength);
						if (num4 != -9999f)
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
							if (num4 != 0f)
							{
								m_marginLeft = num4;
								if (tagUnits != 0)
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
									if (tagUnits != TagUnits.FontUnits)
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
										if (tagUnits == TagUnits.Percentage)
										{
											float marginWidth2 = m_marginWidth;
											float num26;
											if (m_width != -1f)
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
												num26 = m_width;
											}
											else
											{
												num26 = 0f;
											}
											m_marginLeft = (marginWidth2 - num26) * m_marginLeft / 100f;
										}
									}
									else
									{
										m_marginLeft *= m_fontScale * m_fontAsset.fontInfo.TabWidth / (float)(int)m_fontAsset.tabSize;
									}
								}
								m_marginLeft = ((!(m_marginLeft >= 0f)) ? 0f : m_marginLeft);
								m_marginRight = m_marginLeft;
								return true;
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
						return false;
						IL_1dea:
						m_isNonBreakingSpace = false;
						return true;
					}
					return true;
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
			tag_NoParsing = false;
			return true;
		}
	}
}
