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

		protected MaterialReference[] m_materialReferences = new MaterialReference[0x20];

		protected Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[0x10]);

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

		protected Color32 m_underlineColor = TMP_Text.s_colorWhite;

		protected Color32 m_strikethroughColor = TMP_Text.s_colorWhite;

		protected Color32 m_highlightColor = TMP_Text.s_colorWhite;

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

		protected TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[0x10]);

		[SerializeField]
		protected int m_fontWeight = 0x190;

		protected int m_fontWeightInternal;

		protected TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[0x10]);

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

		protected TMP_XmlTagStack<TextAlignmentOptions> m_lineJustificationStack = new TMP_XmlTagStack<TextAlignmentOptions>(new TextAlignmentOptions[0x10]);

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

		protected int m_maxVisibleCharacters = 0x1869F;

		protected int m_maxVisibleWords = 0x1869F;

		protected int m_maxVisibleLines = 0x1869F;

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
		protected TMP_Text.TextInputSources m_inputSource;

		protected string old_text;

		protected float m_fontScale;

		protected float m_fontScaleMultiplier;

		protected char[] m_htmlTag = new char[0x80];

		protected XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		protected float[] m_attributeParameterValues = new float[0x10];

		protected float tag_LineIndent;

		protected float tag_Indent;

		protected TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[0x10]);

		protected bool tag_NoParsing;

		protected bool m_isParsingText;

		protected Matrix4x4 m_FXMatrix;

		protected bool m_isFXMatrixSet;

		protected int[] m_char_buffer;

		private TMP_CharacterInfo[] m_internalCharacterInfo;

		protected char[] m_input_CharArray = new char[0x100];

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

		protected TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>(new Color32[0x10]);

		protected TMP_XmlTagStack<Color32> m_underlineColorStack = new TMP_XmlTagStack<Color32>(new Color32[0x10]);

		protected TMP_XmlTagStack<Color32> m_strikethroughColorStack = new TMP_XmlTagStack<Color32>(new Color32[0x10]);

		protected TMP_XmlTagStack<Color32> m_highlightColorStack = new TMP_XmlTagStack<Color32>(new Color32[0x10]);

		protected float m_tabSpacing;

		protected float m_spacing;

		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[0x10]);

		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[0x10]);

		protected float m_padding;

		protected float m_baselineOffset;

		protected TMP_XmlTagStack<float> m_baselineOffsetStack = new TMP_XmlTagStack<float>(new float[0x10]);

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

		private readonly float[] k_Power = new float[]
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

		protected static int k_LargeNegativeInt = -0x7FFFFFFF;

		public string text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				if (this.m_text == value)
				{
					return;
				}
				this.old_text = value;
				this.m_text = value;
				this.m_inputSource = TMP_Text.TextInputSources.String;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isRightToLeftText
		{
			get
			{
				return this.m_isRightToLeft;
			}
			set
			{
				if (this.m_isRightToLeft == value)
				{
					return;
				}
				this.m_isRightToLeft = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public TMP_FontAsset font
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				if (this.m_fontAsset == value)
				{
					return;
				}
				this.m_fontAsset = value;
				this.LoadFontAsset();
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public virtual Material fontSharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				if (this.m_sharedMaterial == value)
				{
					return;
				}
				this.SetSharedMaterial(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return this.GetSharedMaterials();
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public Material fontMaterial
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial != null)
				{
					if (this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
					{
						return;
					}
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public virtual Material[] fontMaterials
		{
			get
			{
				return this.GetMaterials(this.m_fontSharedMaterials);
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public override Color color
		{
			get
			{
				return this.m_fontColor;
			}
			set
			{
				if (this.m_fontColor == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_fontColor = value;
				this.SetVerticesDirty();
			}
		}

		public float alpha
		{
			get
			{
				return this.m_fontColor.a;
			}
			set
			{
				if (this.m_fontColor.a == value)
				{
					return;
				}
				this.m_fontColor.a = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool enableVertexGradient
		{
			get
			{
				return this.m_enableVertexGradient;
			}
			set
			{
				if (this.m_enableVertexGradient == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableVertexGradient = value;
				this.SetVerticesDirty();
			}
		}

		public VertexGradient colorGradient
		{
			get
			{
				return this.m_fontColorGradient;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradient = value;
				this.SetVerticesDirty();
			}
		}

		public TMP_ColorGradient colorGradientPreset
		{
			get
			{
				return this.m_fontColorGradientPreset;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradientPreset = value;
				this.SetVerticesDirty();
			}
		}

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool tintAllSprites
		{
			get
			{
				return this.m_tintAllSprites;
			}
			set
			{
				if (this.m_tintAllSprites == value)
				{
					return;
				}
				this.m_tintAllSprites = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool overrideColorTags
		{
			get
			{
				return this.m_overrideHtmlColors;
			}
			set
			{
				if (this.m_overrideHtmlColors == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_overrideHtmlColors = value;
				this.SetVerticesDirty();
			}
		}

		public Color32 faceColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_faceColor;
				}
				this.m_faceColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
				return this.m_faceColor;
			}
			set
			{
				if (this.m_faceColor.Compare(value))
				{
					return;
				}
				this.SetFaceColor(value);
				this.m_havePropertiesChanged = true;
				this.m_faceColor = value;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public Color32 outlineColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineColor;
				}
				this.m_outlineColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
				return this.m_outlineColor;
			}
			set
			{
				if (this.m_outlineColor.Compare(value))
				{
					return;
				}
				this.SetOutlineColor(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineColor = value;
				this.SetVerticesDirty();
			}
		}

		public float outlineWidth
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineWidth;
				}
				this.m_outlineWidth = this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return this.m_outlineWidth;
			}
			set
			{
				if (this.m_outlineWidth == value)
				{
					return;
				}
				this.SetOutlineThickness(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineWidth = value;
				this.SetVerticesDirty();
			}
		}

		public float fontSize
		{
			get
			{
				return this.m_fontSize;
			}
			set
			{
				if (this.m_fontSize == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_fontSize = value;
				if (!this.m_enableAutoSizing)
				{
					this.m_fontSizeBase = this.m_fontSize;
				}
			}
		}

		public float fontScale
		{
			get
			{
				return this.m_fontScale;
			}
		}

		public int fontWeight
		{
			get
			{
				return this.m_fontWeight;
			}
			set
			{
				if (this.m_fontWeight == value)
				{
					return;
				}
				this.m_fontWeight = value;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!this.font)
				{
					return canvas.scaleFactor;
				}
				if (!(this.m_currentFontAsset == null))
				{
					if (this.m_currentFontAsset.fontInfo.PointSize > 0f)
					{
						if (this.m_fontSize > 0f)
						{
							return this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
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
				return this.m_enableAutoSizing;
			}
			set
			{
				if (this.m_enableAutoSizing == value)
				{
					return;
				}
				this.m_enableAutoSizing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float fontSizeMin
		{
			get
			{
				return this.m_fontSizeMin;
			}
			set
			{
				if (this.m_fontSizeMin == value)
				{
					return;
				}
				this.m_fontSizeMin = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float fontSizeMax
		{
			get
			{
				return this.m_fontSizeMax;
			}
			set
			{
				if (this.m_fontSizeMax == value)
				{
					return;
				}
				this.m_fontSizeMax = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public FontStyles fontStyle
		{
			get
			{
				return this.m_fontStyle;
			}
			set
			{
				if (this.m_fontStyle == value)
				{
					return;
				}
				this.m_fontStyle = value;
				this.m_havePropertiesChanged = true;
				this.checkPaddingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isUsingBold
		{
			get
			{
				return this.m_isUsingBold;
			}
		}

		public TextAlignmentOptions alignment
		{
			get
			{
				return this.m_textAlignment;
			}
			set
			{
				if (this.m_textAlignment == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_textAlignment = value;
				this.SetVerticesDirty();
			}
		}

		public float characterSpacing
		{
			get
			{
				return this.m_characterSpacing;
			}
			set
			{
				if (this.m_characterSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_characterSpacing = value;
			}
		}

		public float wordSpacing
		{
			get
			{
				return this.m_wordSpacing;
			}
			set
			{
				if (this.m_wordSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_wordSpacing = value;
			}
		}

		public float lineSpacing
		{
			get
			{
				return this.m_lineSpacing;
			}
			set
			{
				if (this.m_lineSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacing = value;
			}
		}

		public float lineSpacingAdjustment
		{
			get
			{
				return this.m_lineSpacingMax;
			}
			set
			{
				if (this.m_lineSpacingMax == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacingMax = value;
			}
		}

		public float paragraphSpacing
		{
			get
			{
				return this.m_paragraphSpacing;
			}
			set
			{
				if (this.m_paragraphSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_paragraphSpacing = value;
			}
		}

		public float characterWidthAdjustment
		{
			get
			{
				return this.m_charWidthMaxAdj;
			}
			set
			{
				if (this.m_charWidthMaxAdj == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_charWidthMaxAdj = value;
			}
		}

		public bool enableWordWrapping
		{
			get
			{
				return this.m_enableWordWrapping;
			}
			set
			{
				if (this.m_enableWordWrapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.m_enableWordWrapping = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public float wordWrappingRatios
		{
			get
			{
				return this.m_wordWrappingRatios;
			}
			set
			{
				if (this.m_wordWrappingRatios == value)
				{
					return;
				}
				this.m_wordWrappingRatios = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public TextOverflowModes overflowMode
		{
			get
			{
				return this.m_overflowMode;
			}
			set
			{
				if (this.m_overflowMode == value)
				{
					return;
				}
				this.m_overflowMode = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isTextOverflowing
		{
			get
			{
				if (this.m_firstOverflowCharacterIndex != -1)
				{
					return true;
				}
				return false;
			}
		}

		public int firstOverflowCharacterIndex
		{
			get
			{
				return this.m_firstOverflowCharacterIndex;
			}
		}

		public TMP_Text linkedTextComponent
		{
			get
			{
				return this.m_linkedTextComponent;
			}
			set
			{
				if (this.m_linkedTextComponent != value)
				{
					if (this.m_linkedTextComponent != null)
					{
						this.m_linkedTextComponent.overflowMode = TextOverflowModes.Overflow;
						this.m_linkedTextComponent.linkedTextComponent = null;
						this.m_linkedTextComponent.isLinkedTextComponent = false;
					}
					this.m_linkedTextComponent = value;
					if (this.m_linkedTextComponent != null)
					{
						this.m_linkedTextComponent.isLinkedTextComponent = true;
					}
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isLinkedTextComponent
		{
			get
			{
				return this.m_isLinkedTextComponent;
			}
			set
			{
				this.m_isLinkedTextComponent = value;
				if (!this.m_isLinkedTextComponent)
				{
					this.m_firstVisibleCharacter = 0;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public bool isTextTruncated
		{
			get
			{
				return this.m_isTextTruncated;
			}
		}

		public bool enableKerning
		{
			get
			{
				return this.m_enableKerning;
			}
			set
			{
				if (this.m_enableKerning == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_enableKerning = value;
			}
		}

		public bool extraPadding
		{
			get
			{
				return this.m_enableExtraPadding;
			}
			set
			{
				if (this.m_enableExtraPadding == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableExtraPadding = value;
				this.UpdateMeshPadding();
				this.SetVerticesDirty();
			}
		}

		public bool richText
		{
			get
			{
				return this.m_isRichText;
			}
			set
			{
				if (this.m_isRichText == value)
				{
					return;
				}
				this.m_isRichText = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		public bool parseCtrlCharacters
		{
			get
			{
				return this.m_parseCtrlCharacters;
			}
			set
			{
				if (this.m_parseCtrlCharacters == value)
				{
					return;
				}
				this.m_parseCtrlCharacters = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		public bool isOverlay
		{
			get
			{
				return this.m_isOverlay;
			}
			set
			{
				if (this.m_isOverlay == value)
				{
					return;
				}
				this.m_isOverlay = value;
				this.SetShaderDepth();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public bool isOrthographic
		{
			get
			{
				return this.m_isOrthographic;
			}
			set
			{
				if (this.m_isOrthographic == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isOrthographic = value;
				this.SetVerticesDirty();
			}
		}

		public bool enableCulling
		{
			get
			{
				return this.m_isCullingEnabled;
			}
			set
			{
				if (this.m_isCullingEnabled == value)
				{
					return;
				}
				this.m_isCullingEnabled = value;
				this.SetCulling();
				this.m_havePropertiesChanged = true;
			}
		}

		public bool ignoreRectMaskCulling
		{
			get
			{
				return this.m_ignoreRectMaskCulling;
			}
			set
			{
				if (this.m_ignoreRectMaskCulling == value)
				{
					return;
				}
				this.m_ignoreRectMaskCulling = value;
				this.m_havePropertiesChanged = true;
			}
		}

		public bool ignoreVisibility
		{
			get
			{
				return this.m_ignoreCulling;
			}
			set
			{
				if (this.m_ignoreCulling == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_ignoreCulling = value;
			}
		}

		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return this.m_horizontalMapping;
			}
			set
			{
				if (this.m_horizontalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_horizontalMapping = value;
				this.SetVerticesDirty();
			}
		}

		public TextureMappingOptions verticalMapping
		{
			get
			{
				return this.m_verticalMapping;
			}
			set
			{
				if (this.m_verticalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_verticalMapping = value;
				this.SetVerticesDirty();
			}
		}

		public float mappingUvLineOffset
		{
			get
			{
				return this.m_uvLineOffset;
			}
			set
			{
				if (this.m_uvLineOffset == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_uvLineOffset = value;
				this.SetVerticesDirty();
			}
		}

		public TextRenderFlags renderMode
		{
			get
			{
				return this.m_renderMode;
			}
			set
			{
				if (this.m_renderMode == value)
				{
					return;
				}
				this.m_renderMode = value;
				this.m_havePropertiesChanged = true;
			}
		}

		public VertexSortingOrder geometrySortingOrder
		{
			get
			{
				return this.m_geometrySortingOrder;
			}
			set
			{
				this.m_geometrySortingOrder = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public int firstVisibleCharacter
		{
			get
			{
				return this.m_firstVisibleCharacter;
			}
			set
			{
				if (this.m_firstVisibleCharacter == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_firstVisibleCharacter = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleCharacters
		{
			get
			{
				return this.m_maxVisibleCharacters;
			}
			set
			{
				if (this.m_maxVisibleCharacters == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleCharacters = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleWords
		{
			get
			{
				return this.m_maxVisibleWords;
			}
			set
			{
				if (this.m_maxVisibleWords == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleWords = value;
				this.SetVerticesDirty();
			}
		}

		public int maxVisibleLines
		{
			get
			{
				return this.m_maxVisibleLines;
			}
			set
			{
				if (this.m_maxVisibleLines == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_maxVisibleLines = value;
				this.SetVerticesDirty();
			}
		}

		public bool useMaxVisibleDescender
		{
			get
			{
				return this.m_useMaxVisibleDescender;
			}
			set
			{
				if (this.m_useMaxVisibleDescender == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
			}
		}

		public int pageToDisplay
		{
			get
			{
				return this.m_pageToDisplay;
			}
			set
			{
				if (this.m_pageToDisplay == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_pageToDisplay = value;
				this.SetVerticesDirty();
			}
		}

		public virtual Vector4 margin
		{
			get
			{
				return this.m_margin;
			}
			set
			{
				if (this.m_margin == value)
				{
					return;
				}
				this.m_margin = value;
				this.ComputeMarginSize();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		public TMP_TextInfo textInfo
		{
			get
			{
				return this.m_textInfo;
			}
		}

		public bool havePropertiesChanged
		{
			get
			{
				return this.m_havePropertiesChanged;
			}
			set
			{
				if (this.m_havePropertiesChanged == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.m_isInputParsingRequired = true;
				this.SetAllDirty();
			}
		}

		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return this.m_isUsingLegacyAnimationComponent;
			}
			set
			{
				this.m_isUsingLegacyAnimationComponent = value;
			}
		}

		public new Transform transform
		{
			get
			{
				if (this.m_transform == null)
				{
					this.m_transform = base.GetComponent<Transform>();
				}
				return this.m_transform;
			}
		}

		public new RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		public virtual bool autoSizeTextContainer { get; set; }

		public virtual Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		public bool isVolumetricText
		{
			get
			{
				return this.m_isVolumetricText;
			}
			set
			{
				if (this.m_isVolumetricText == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.m_textInfo.ResetVertexLayout(value);
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		public Bounds bounds
		{
			get
			{
				if (this.m_mesh == null)
				{
					return default(Bounds);
				}
				return this.GetCompoundBounds();
			}
		}

		public Bounds textBounds
		{
			get
			{
				if (this.m_textInfo == null)
				{
					return default(Bounds);
				}
				return this.GetTextBounds();
			}
		}

		protected TMP_SpriteAnimator spriteAnimator
		{
			get
			{
				if (this.m_spriteAnimator == null)
				{
					this.m_spriteAnimator = base.GetComponent<TMP_SpriteAnimator>();
					if (this.m_spriteAnimator == null)
					{
						this.m_spriteAnimator = base.gameObject.AddComponent<TMP_SpriteAnimator>();
					}
				}
				return this.m_spriteAnimator;
			}
		}

		public float flexibleHeight
		{
			get
			{
				return this.m_flexibleHeight;
			}
		}

		public float flexibleWidth
		{
			get
			{
				return this.m_flexibleWidth;
			}
		}

		public float minWidth
		{
			get
			{
				return this.m_minWidth;
			}
		}

		public float minHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		public float maxWidth
		{
			get
			{
				return this.m_maxWidth;
			}
		}

		public float maxHeight
		{
			get
			{
				return this.m_maxHeight;
			}
		}

		protected LayoutElement layoutElement
		{
			get
			{
				if (this.m_LayoutElement == null)
				{
					this.m_LayoutElement = base.GetComponent<LayoutElement>();
				}
				return this.m_LayoutElement;
			}
		}

		public virtual float preferredWidth
		{
			get
			{
				if (!this.m_isPreferredWidthDirty)
				{
					return this.m_preferredWidth;
				}
				this.m_preferredWidth = this.GetPreferredWidth();
				return this.m_preferredWidth;
			}
		}

		public virtual float preferredHeight
		{
			get
			{
				if (!this.m_isPreferredHeightDirty)
				{
					return this.m_preferredHeight;
				}
				this.m_preferredHeight = this.GetPreferredHeight();
				return this.m_preferredHeight;
			}
		}

		public virtual float renderedWidth
		{
			get
			{
				return this.GetRenderedWidth();
			}
		}

		public virtual float renderedHeight
		{
			get
			{
				return this.GetRenderedHeight();
			}
		}

		public int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

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
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		protected void SetVertexColorGradient(TMP_ColorGradient gradient)
		{
			if (gradient == null)
			{
				return;
			}
			this.m_fontColorGradient.bottomLeft = gradient.bottomLeft;
			this.m_fontColorGradient.bottomRight = gradient.bottomRight;
			this.m_fontColorGradient.topLeft = gradient.topLeft;
			this.m_fontColorGradient.topRight = gradient.topRight;
			this.SetVerticesDirty();
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
			this.m_text = text;
			this.m_renderMode = TextRenderFlags.DontRender;
			this.m_isInputParsingRequired = true;
			this.ForceMeshUpdate();
			this.m_renderMode = TextRenderFlags.Render;
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
			this.InternalCrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}

		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			base.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			this.InternalCrossFadeAlpha(alpha, duration, ignoreTimeScale);
		}

		protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
		}

		protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
		}

		protected void ParseInputText()
		{
			this.m_isInputParsingRequired = false;
			switch (this.m_inputSource)
			{
			case TMP_Text.TextInputSources.Text:
			case TMP_Text.TextInputSources.String:
				this.StringToCharArray(this.m_text, ref this.m_char_buffer);
				break;
			case TMP_Text.TextInputSources.SetText:
				this.SetTextArrayToCharArray(this.m_input_CharArray, ref this.m_char_buffer);
				break;
			}
			this.SetArraySizes(this.m_char_buffer);
		}

		public void SetText(string text)
		{
			this.SetText(text, true);
		}

		public void SetText(string text, bool syncTextInputBox)
		{
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetText(string text, float arg0)
		{
			this.SetText(text, arg0, 255f, 255f);
		}

		public void SetText(string text, float arg0, float arg1)
		{
			this.SetText(text, arg0, arg1, 255f);
		}

		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			int precision = 0;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					if (text[i + 2] == ':')
					{
						precision = (int)(text[i + 3] - '0');
					}
					int num2 = (int)(text[i + 1] - '0');
					if (num2 != 0)
					{
						if (num2 != 1)
						{
							if (num2 == 2)
							{
								this.AddFloatToCharArray(arg2, ref num, precision);
							}
						}
						else
						{
							this.AddFloatToCharArray(arg1, ref num, precision);
						}
					}
					else
					{
						this.AddFloatToCharArray(arg0, ref num, precision);
					}
					if (text[i + 2] == ':')
					{
						i += 4;
					}
					else
					{
						i += 2;
					}
				}
				else
				{
					this.m_input_CharArray[num] = c;
					num++;
				}
			}
			this.m_input_CharArray[num] = '\0';
			this.m_charArray_Length = num;
			this.m_inputSource = TMP_Text.TextInputSources.SetText;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetText(StringBuilder text)
		{
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.StringBuilderToIntArray(text, ref this.m_char_buffer);
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		public void SetCharArray(char[] sourceText)
		{
			if (sourceText != null)
			{
				if (sourceText.Length != 0)
				{
					if (this.m_char_buffer == null)
					{
						this.m_char_buffer = new int[8];
					}
					this.m_styleStack.Clear();
					int num = 0;
					int i = 0;
					while (i < sourceText.Length)
					{
						if (sourceText[i] != '\\')
						{
							goto IL_150;
						}
						if (i >= sourceText.Length - 1)
						{
							goto IL_150;
						}
						int num2 = (int)sourceText[i + 1];
						if (num2 != 0x6E)
						{
							if (num2 != 0x72)
							{
								if (num2 != 0x74)
								{
									goto IL_150;
								}
								if (num == this.m_char_buffer.Length)
								{
									this.ResizeInternalArray<int>(ref this.m_char_buffer);
								}
								this.m_char_buffer[num] = 9;
								i++;
								num++;
							}
							else
							{
								if (num == this.m_char_buffer.Length)
								{
									this.ResizeInternalArray<int>(ref this.m_char_buffer);
								}
								this.m_char_buffer[num] = 0xD;
								i++;
								num++;
							}
						}
						else
						{
							if (num == this.m_char_buffer.Length)
							{
								this.ResizeInternalArray<int>(ref this.m_char_buffer);
							}
							this.m_char_buffer[num] = 0xA;
							i++;
							num++;
						}
						IL_24F:
						i++;
						continue;
						IL_150:
						if (sourceText[i] == '<')
						{
							if (this.IsTagName(ref sourceText, "<BR>", i))
							{
								if (num == this.m_char_buffer.Length)
								{
									this.ResizeInternalArray<int>(ref this.m_char_buffer);
								}
								this.m_char_buffer[num] = 0xA;
								num++;
								i += 3;
								goto IL_24F;
							}
							if (this.IsTagName(ref sourceText, "<STYLE=", i))
							{
								int num3 = 0;
								if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref this.m_char_buffer, ref num))
								{
									i = num3;
									goto IL_24F;
								}
							}
							else if (this.IsTagName(ref sourceText, "</STYLE>", i))
							{
								this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
								i += 7;
								goto IL_24F;
							}
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = (int)sourceText[i];
						num++;
						goto IL_24F;
					}
					if (num == this.m_char_buffer.Length)
					{
						this.ResizeInternalArray<int>(ref this.m_char_buffer);
					}
					this.m_char_buffer[num] = 0;
					this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
					this.m_isInputParsingRequired = true;
					this.m_havePropertiesChanged = true;
					this.m_isCalculateSizeRequired = true;
					this.SetVerticesDirty();
					this.SetLayoutDirty();
					return;
				}
			}
		}

		public void SetCharArray(char[] sourceText, int start, int length)
		{
			if (sourceText != null)
			{
				if (sourceText.Length != 0)
				{
					if (length != 0)
					{
						if (this.m_char_buffer == null)
						{
							this.m_char_buffer = new int[8];
						}
						this.m_styleStack.Clear();
						int num = 0;
						int i = start;
						int num2 = start + length;
						while (i < num2)
						{
							if (sourceText[i] != '\\')
							{
								goto IL_148;
							}
							if (i >= length - 1)
							{
								goto IL_148;
							}
							int num3 = (int)sourceText[i + 1];
							if (num3 != 0x6E)
							{
								if (num3 != 0x72)
								{
									if (num3 != 0x74)
									{
										goto IL_148;
									}
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 9;
									i++;
									num++;
								}
								else
								{
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 0xD;
									i++;
									num++;
								}
							}
							else
							{
								if (num == this.m_char_buffer.Length)
								{
									this.ResizeInternalArray<int>(ref this.m_char_buffer);
								}
								this.m_char_buffer[num] = 0xA;
								i++;
								num++;
							}
							IL_25B:
							i++;
							continue;
							IL_148:
							if (sourceText[i] == '<')
							{
								if (this.IsTagName(ref sourceText, "<BR>", i))
								{
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 0xA;
									num++;
									i += 3;
									goto IL_25B;
								}
								if (this.IsTagName(ref sourceText, "<STYLE=", i))
								{
									int num4 = 0;
									if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num))
									{
										i = num4;
										goto IL_25B;
									}
								}
								else if (this.IsTagName(ref sourceText, "</STYLE>", i))
								{
									this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
									i += 7;
									goto IL_25B;
								}
							}
							if (num == this.m_char_buffer.Length)
							{
								this.ResizeInternalArray<int>(ref this.m_char_buffer);
							}
							this.m_char_buffer[num] = (int)sourceText[i];
							num++;
							goto IL_25B;
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 0;
						this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
						this.m_havePropertiesChanged = true;
						this.m_isInputParsingRequired = true;
						this.m_isCalculateSizeRequired = true;
						this.SetVerticesDirty();
						this.SetLayoutDirty();
						return;
					}
				}
			}
		}

		public void SetCharArray(int[] sourceText, int start, int length)
		{
			if (sourceText != null)
			{
				if (sourceText.Length != 0)
				{
					if (length != 0)
					{
						if (this.m_char_buffer == null)
						{
							this.m_char_buffer = new int[8];
						}
						this.m_styleStack.Clear();
						int num = 0;
						int i = start;
						int num2 = start + length;
						while (i < num2)
						{
							if (sourceText[i] != 0x5C)
							{
								goto IL_15F;
							}
							if (i >= length - 1)
							{
								goto IL_15F;
							}
							int num3 = sourceText[i + 1];
							if (num3 != 0x6E)
							{
								if (num3 != 0x72)
								{
									if (num3 != 0x74)
									{
										goto IL_15F;
									}
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 9;
									i++;
									num++;
								}
								else
								{
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 0xD;
									i++;
									num++;
								}
							}
							else
							{
								if (num == this.m_char_buffer.Length)
								{
									this.ResizeInternalArray<int>(ref this.m_char_buffer);
								}
								this.m_char_buffer[num] = 0xA;
								i++;
								num++;
							}
							IL_248:
							i++;
							continue;
							IL_15F:
							if (sourceText[i] == 0x3C)
							{
								if (this.IsTagName(ref sourceText, "<BR>", i))
								{
									if (num == this.m_char_buffer.Length)
									{
										this.ResizeInternalArray<int>(ref this.m_char_buffer);
									}
									this.m_char_buffer[num] = 0xA;
									num++;
									i += 3;
									goto IL_248;
								}
								if (this.IsTagName(ref sourceText, "<STYLE=", i))
								{
									int num4 = 0;
									if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num4, ref this.m_char_buffer, ref num))
									{
										i = num4;
										goto IL_248;
									}
								}
								else if (this.IsTagName(ref sourceText, "</STYLE>", i))
								{
									this.ReplaceClosingStyleTag(ref sourceText, i, ref this.m_char_buffer, ref num);
									i += 7;
									goto IL_248;
								}
							}
							if (num == this.m_char_buffer.Length)
							{
								this.ResizeInternalArray<int>(ref this.m_char_buffer);
							}
							this.m_char_buffer[num] = sourceText[i];
							num++;
							goto IL_248;
						}
						if (num == this.m_char_buffer.Length)
						{
							this.ResizeInternalArray<int>(ref this.m_char_buffer);
						}
						this.m_char_buffer[num] = 0;
						this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
						this.m_havePropertiesChanged = true;
						this.m_isInputParsingRequired = true;
						this.m_isCalculateSizeRequired = true;
						this.SetVerticesDirty();
						this.SetLayoutDirty();
						return;
					}
				}
			}
		}

		protected unsafe void SetTextArrayToCharArray(char[] sourceText, ref int[] charBuffer)
		{
			if (sourceText != null)
			{
				if (this.m_charArray_Length != 0)
				{
					if (charBuffer == null)
					{
						charBuffer = new int[8];
					}
					this.m_styleStack.Clear();
					int num = 0;
					int i = 0;
					while (i < this.m_charArray_Length)
					{
						if (!char.IsHighSurrogate(sourceText[i]))
						{
							goto IL_AF;
						}
						if (!char.IsLowSurrogate(sourceText[i + 1]))
						{
							goto IL_AF;
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
						i++;
						num++;
						IL_19A:
						i++;
						continue;
						IL_AF:
						if (sourceText[i] == '<')
						{
							if (this.IsTagName(ref sourceText, "<BR>", i))
							{
								if (num == charBuffer.Length)
								{
									this.ResizeInternalArray<int>(ref charBuffer);
								}
								charBuffer[num] = 0xA;
								num++;
								i += 3;
								goto IL_19A;
							}
							if (this.IsTagName(ref sourceText, "<STYLE=", i))
							{
								int num2 = 0;
								if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num2, ref charBuffer, ref num))
								{
									i = num2;
									goto IL_19A;
								}
							}
							else if (this.IsTagName(ref sourceText, "</STYLE>", i))
							{
								this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
								i += 7;
								goto IL_19A;
							}
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = (int)sourceText[i];
						num++;
						goto IL_19A;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 0;
					return;
				}
			}
		}

		protected unsafe void StringToCharArray(string sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			this.m_styleStack.SetDefault(0);
			int num = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (this.m_inputSource != TMP_Text.TextInputSources.Text || sourceText[i] != '\\')
				{
					goto IL_27B;
				}
				if (sourceText.Length <= i + 1)
				{
					goto IL_27B;
				}
				int num2 = (int)sourceText[i + 1];
				switch (num2)
				{
				case 0x72:
					if (!this.m_parseCtrlCharacters)
					{
						goto IL_27B;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 0xD;
					i++;
					num++;
					break;
				default:
					if (num2 != 0x55)
					{
						if (num2 != 0x5C)
						{
							if (num2 != 0x6E)
							{
								goto IL_27B;
							}
							if (!this.m_parseCtrlCharacters)
							{
								goto IL_27B;
							}
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 0xA;
							i++;
							num++;
						}
						else
						{
							if (!this.m_parseCtrlCharacters)
							{
								goto IL_27B;
							}
							if (sourceText.Length <= i + 2)
							{
								goto IL_27B;
							}
							if (num + 2 > charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = (int)sourceText[i + 1];
							charBuffer[num + 1] = (int)sourceText[i + 2];
							i += 2;
							num += 2;
						}
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_27B;
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = this.GetUTF32(i + 2);
						i += 9;
						num++;
					}
					break;
				case 0x74:
					if (!this.m_parseCtrlCharacters)
					{
						goto IL_27B;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 9;
					i++;
					num++;
					break;
				case 0x75:
					if (sourceText.Length <= i + 5)
					{
						goto IL_27B;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num++;
					break;
				}
				IL_3EF:
				i++;
				continue;
				IL_27B:
				if (char.IsHighSurrogate(sourceText[i]))
				{
					if (char.IsLowSurrogate(sourceText[i + 1]))
					{
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
						i++;
						num++;
						goto IL_3EF;
					}
				}
				if (sourceText[i] == '<')
				{
					if (this.m_isRichText)
					{
						if (this.IsTagName(ref sourceText, "<BR>", i))
						{
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 0xA;
							num++;
							i += 3;
							goto IL_3EF;
						}
						if (this.IsTagName(ref sourceText, "<STYLE=", i))
						{
							int num3 = 0;
							if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref charBuffer, ref num))
							{
								i = num3;
								goto IL_3EF;
							}
						}
						else if (this.IsTagName(ref sourceText, "</STYLE>", i))
						{
							this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
							i += 7;
							goto IL_3EF;
						}
					}
				}
				if (num == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[num] = (int)sourceText[i];
				num++;
				goto IL_3EF;
			}
			if (num == charBuffer.Length)
			{
				this.ResizeInternalArray<int>(ref charBuffer);
			}
			charBuffer[num] = 0;
		}

		protected unsafe void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
		{
			if (sourceText == null)
			{
				charBuffer[0] = 0;
				return;
			}
			if (charBuffer == null)
			{
				charBuffer = new int[8];
			}
			this.m_styleStack.Clear();
			int num = 0;
			int i = 0;
			while (i < sourceText.Length)
			{
				if (!this.m_parseCtrlCharacters)
				{
					goto IL_229;
				}
				if (sourceText[i] != '\\')
				{
					goto IL_229;
				}
				if (sourceText.Length <= i + 1)
				{
					goto IL_229;
				}
				int num2 = (int)sourceText[i + 1];
				switch (num2)
				{
				case 0x72:
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 0xD;
					i++;
					num++;
					break;
				default:
					if (num2 != 0x55)
					{
						if (num2 != 0x5C)
						{
							if (num2 != 0x6E)
							{
								goto IL_229;
							}
							if (num == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = 0xA;
							i++;
							num++;
						}
						else
						{
							if (sourceText.Length <= i + 2)
							{
								goto IL_229;
							}
							if (num + 2 > charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[num] = (int)sourceText[i + 1];
							charBuffer[num + 1] = (int)sourceText[i + 2];
							i += 2;
							num += 2;
						}
					}
					else
					{
						if (sourceText.Length <= i + 9)
						{
							goto IL_229;
						}
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = this.GetUTF32(i + 2);
						i += 9;
						num++;
					}
					break;
				case 0x74:
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = 9;
					i++;
					num++;
					break;
				case 0x75:
					if (sourceText.Length <= i + 5)
					{
						goto IL_229;
					}
					if (num == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[num] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num++;
					break;
				}
				IL_378:
				i++;
				continue;
				IL_229:
				if (char.IsHighSurrogate(sourceText[i]))
				{
					if (char.IsLowSurrogate(sourceText[i + 1]))
					{
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
						i++;
						num++;
						goto IL_378;
					}
				}
				if (sourceText[i] == '<')
				{
					if (this.IsTagName(ref sourceText, "<BR>", i))
					{
						if (num == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[num] = 0xA;
						num++;
						i += 3;
						goto IL_378;
					}
					if (this.IsTagName(ref sourceText, "<STYLE=", i))
					{
						int num3 = 0;
						if (this.ReplaceOpeningStyleTag(ref sourceText, i, out num3, ref charBuffer, ref num))
						{
							i = num3;
							goto IL_378;
						}
					}
					else if (this.IsTagName(ref sourceText, "</STYLE>", i))
					{
						this.ReplaceClosingStyleTag(ref sourceText, i, ref charBuffer, ref num);
						i += 7;
						goto IL_378;
					}
				}
				if (num == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[num] = (int)sourceText[i];
				num++;
				goto IL_378;
			}
			if (num == charBuffer.Length)
			{
				this.ResizeInternalArray<int>(ref charBuffer);
			}
			charBuffer[num] = 0;
		}

		private unsafe bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					this.m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] styleOpeningTagArray = style.styleOpeningTagArray;
					int i = 0;
					while (i < num)
					{
						int num2 = styleOpeningTagArray[i];
						if (num2 != 0x3C)
						{
							goto IL_13E;
						}
						if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
						{
							if (writeIndex == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[writeIndex] = 0xA;
							writeIndex++;
							i += 3;
						}
						else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
						{
							int num3 = 0;
							if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
							{
								goto IL_13E;
							}
							i = num3;
						}
						else
						{
							if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
							{
								goto IL_13E;
							}
							this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
							i += 7;
						}
						IL_161:
						i++;
						continue;
						IL_13E:
						if (writeIndex == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
						goto IL_161;
					}
					return true;
				}
			}
			return false;
		}

		private unsafe bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					this.m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] styleOpeningTagArray = style.styleOpeningTagArray;
					int i = 0;
					while (i < num)
					{
						int num2 = styleOpeningTagArray[i];
						if (num2 != 0x3C)
						{
							goto IL_132;
						}
						if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
						{
							if (writeIndex == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[writeIndex] = 0xA;
							writeIndex++;
							i += 3;
						}
						else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
						{
							int num3 = 0;
							if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
							{
								goto IL_132;
							}
							i = num3;
						}
						else
						{
							if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
							{
								goto IL_132;
							}
							this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
							i += 7;
						}
						IL_15F:
						i++;
						continue;
						IL_132:
						if (writeIndex == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
						goto IL_15F;
					}
					return true;
				}
			}
			return false;
		}

		private unsafe bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					this.m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] styleOpeningTagArray = style.styleOpeningTagArray;
					int i = 0;
					while (i < num)
					{
						int num2 = styleOpeningTagArray[i];
						if (num2 != 0x3C)
						{
							goto IL_134;
						}
						if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
						{
							if (writeIndex == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[writeIndex] = 0xA;
							writeIndex++;
							i += 3;
						}
						else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
						{
							int num3 = 0;
							if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
							{
								goto IL_134;
							}
							i = num3;
						}
						else
						{
							if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
							{
								goto IL_134;
							}
							this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
							i += 7;
						}
						IL_161:
						i++;
						continue;
						IL_134:
						if (writeIndex == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
						goto IL_161;
					}
					return true;
				}
			}
			return false;
		}

		private unsafe bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
		{
			int tagHashCode = this.GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
			TMP_Style style = TMP_StyleSheet.GetStyle(tagHashCode);
			if (style != null)
			{
				if (srcOffset != 0)
				{
					this.m_styleStack.Add(style.hashCode);
					int num = style.styleOpeningTagArray.Length;
					int[] styleOpeningTagArray = style.styleOpeningTagArray;
					int i = 0;
					while (i < num)
					{
						int num2 = styleOpeningTagArray[i];
						if (num2 != 0x3C)
						{
							goto IL_148;
						}
						if (this.IsTagName(ref styleOpeningTagArray, "<BR>", i))
						{
							if (writeIndex == charBuffer.Length)
							{
								this.ResizeInternalArray<int>(ref charBuffer);
							}
							charBuffer[writeIndex] = 0xA;
							writeIndex++;
							i += 3;
						}
						else if (this.IsTagName(ref styleOpeningTagArray, "<STYLE=", i))
						{
							int num3 = 0;
							if (!this.ReplaceOpeningStyleTag(ref styleOpeningTagArray, i, out num3, ref charBuffer, ref writeIndex))
							{
								goto IL_148;
							}
							i = num3;
						}
						else
						{
							if (!this.IsTagName(ref styleOpeningTagArray, "</STYLE>", i))
							{
								goto IL_148;
							}
							this.ReplaceClosingStyleTag(ref styleOpeningTagArray, i, ref charBuffer, ref writeIndex);
							i += 7;
						}
						IL_175:
						i++;
						continue;
						IL_148:
						if (writeIndex == charBuffer.Length)
						{
							this.ResizeInternalArray<int>(ref charBuffer);
						}
						charBuffer[writeIndex] = num2;
						writeIndex++;
						goto IL_175;
					}
					return true;
				}
			}
			return false;
		}

		private unsafe bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 0x3C)
				{
					goto IL_11A;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 0xA;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_11A;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_11A;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_13A:
				i++;
				continue;
				IL_11A:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_13A;
			}
			return true;
		}

		private unsafe bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 0x3C)
				{
					goto IL_128;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 0xA;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_128;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_128;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_152:
				i++;
				continue;
				IL_128:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_152;
			}
			return true;
		}

		private unsafe bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 0x3C)
				{
					goto IL_11E;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 0xA;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_11E;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_11E;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_148:
				i++;
				continue;
				IL_11E:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_148;
			}
			return true;
		}

		private unsafe bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
		{
			int hashCode = this.m_styleStack.CurrentItem();
			TMP_Style style = TMP_StyleSheet.GetStyle(hashCode);
			this.m_styleStack.Remove();
			if (style == null)
			{
				return false;
			}
			int num = style.styleClosingTagArray.Length;
			int[] styleClosingTagArray = style.styleClosingTagArray;
			int i = 0;
			while (i < num)
			{
				int num2 = styleClosingTagArray[i];
				if (num2 != 0x3C)
				{
					goto IL_110;
				}
				if (this.IsTagName(ref styleClosingTagArray, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						this.ResizeInternalArray<int>(ref charBuffer);
					}
					charBuffer[writeIndex] = 0xA;
					writeIndex++;
					i += 3;
				}
				else if (this.IsTagName(ref styleClosingTagArray, "<STYLE=", i))
				{
					int num3 = 0;
					if (!this.ReplaceOpeningStyleTag(ref styleClosingTagArray, i, out num3, ref charBuffer, ref writeIndex))
					{
						goto IL_110;
					}
					i = num3;
				}
				else
				{
					if (!this.IsTagName(ref styleClosingTagArray, "</STYLE>", i))
					{
						goto IL_110;
					}
					this.ReplaceClosingStyleTag(ref styleClosingTagArray, i, ref charBuffer, ref writeIndex);
					i += 7;
				}
				IL_130:
				i++;
				continue;
				IL_110:
				if (writeIndex == charBuffer.Length)
				{
					this.ResizeInternalArray<int>(ref charBuffer);
				}
				charBuffer[writeIndex] = num2;
				writeIndex++;
				goto IL_130;
			}
			return true;
		}

		private unsafe bool IsTagName(ref string text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private unsafe bool IsTagName(ref char[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private unsafe bool IsTagName(ref int[] text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast((char)text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private unsafe bool IsTagName(ref StringBuilder text, string tag, int index)
		{
			if (text.Length < index + tag.Length)
			{
				return false;
			}
			for (int i = 0; i < tag.Length; i++)
			{
				if (TMP_TextUtilities.ToUpperFast(text[index + i]) != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		private unsafe int GetTagHashCode(ref string text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] != '"')
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						return num;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				return num;
			}
		}

		private unsafe int GetTagHashCode(ref char[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] == '"')
				{
				}
				else
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						break;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			return num;
		}

		private unsafe int GetTagHashCode(ref int[] text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] == 0x22)
				{
				}
				else
				{
					if (text[i] == 0x3E)
					{
						closeIndex = i;
						return num;
					}
					num = ((num << 5) + num ^ text[i]);
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				return num;
			}
		}

		private unsafe int GetTagHashCode(ref StringBuilder text, int index, out int closeIndex)
		{
			int num = 0;
			closeIndex = 0;
			for (int i = index; i < text.Length; i++)
			{
				if (text[i] == '"')
				{
				}
				else
				{
					if (text[i] == '>')
					{
						closeIndex = i;
						return num;
					}
					num = ((num << 5) + num ^ (int)text[i]);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return num;
			}
		}

		private void ResizeInternalArray<T>(ref T[] array)
		{
			int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
			Array.Resize<T>(ref array, newSize);
		}

		protected unsafe void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			number += this.k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			this.AddIntToCharArray(num, ref index, precision);
			if (precision > 0)
			{
				this.m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					this.m_input_CharArray[index++] = (char)(num2 + 0x30);
					number -= (float)num2;
				}
			}
		}

		protected unsafe void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				this.m_input_CharArray[num++] = (char)(number % 0xA + 0x30);
				number /= 0xA;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = this.m_input_CharArray[index];
				this.m_input_CharArray[index] = this.m_input_CharArray[num];
				this.m_input_CharArray[num] = c;
				index++;
			}
			index = num2;
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
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.m_isCalculatingPreferredValues = true;
				this.ParseInputText();
			}
			float preferredWidth = this.GetPreferredWidth();
			float preferredHeight = this.GetPreferredHeight();
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(float width, float height)
		{
			if (!this.m_isInputParsingRequired)
			{
				if (!this.m_isTextTruncated)
				{
					goto IL_3A;
				}
			}
			this.m_isCalculatingPreferredValues = true;
			this.ParseInputText();
			IL_3A:
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text)
		{
			this.m_isCalculatingPreferredValues = true;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = TMP_Text.k_LargePositiveVector2;
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			this.m_isCalculatingPreferredValues = true;
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		protected float GetPreferredWidth()
		{
			float num;
			if (this.m_enableAutoSizing)
			{
				num = this.m_fontSizeMax;
			}
			else
			{
				num = this.m_fontSize;
			}
			float defaultFontSize = num;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			Vector2 marginSize = TMP_Text.k_LargePositiveVector2;
			if (!this.m_isInputParsingRequired)
			{
				if (!this.m_isTextTruncated)
				{
					goto IL_79;
				}
			}
			this.m_isCalculatingPreferredValues = true;
			this.ParseInputText();
			IL_79:
			this.m_recursiveCount = 0;
			float x = this.CalculatePreferredValues(defaultFontSize, marginSize, true).x;
			this.m_isPreferredWidthDirty = false;
			return x;
		}

		protected float GetPreferredWidth(Vector2 margin)
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_recursiveCount = 0;
			return this.CalculatePreferredValues(defaultFontSize, margin, true).x;
		}

		protected float GetPreferredHeight()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			float marginWidth;
			if (this.m_marginWidth != 0f)
			{
				marginWidth = this.m_marginWidth;
			}
			else
			{
				marginWidth = TMP_Text.k_LargePositiveFloat;
			}
			Vector2 marginSize = new Vector2(marginWidth, TMP_Text.k_LargePositiveFloat);
			if (!this.m_isInputParsingRequired)
			{
				if (!this.m_isTextTruncated)
				{
					goto IL_99;
				}
			}
			this.m_isCalculatingPreferredValues = true;
			this.ParseInputText();
			IL_99:
			this.m_recursiveCount = 0;
			float y = this.CalculatePreferredValues(defaultFontSize, marginSize, !this.m_enableAutoSizing).y;
			this.m_isPreferredHeightDirty = false;
			return y;
		}

		protected float GetPreferredHeight(Vector2 margin)
		{
			float num;
			if (this.m_enableAutoSizing)
			{
				num = this.m_fontSizeMax;
			}
			else
			{
				num = this.m_fontSize;
			}
			float defaultFontSize = num;
			this.m_minFontSize = this.m_fontSizeMin;
			this.m_maxFontSize = this.m_fontSizeMax;
			this.m_recursiveCount = 0;
			return this.CalculatePreferredValues(defaultFontSize, margin, true).y;
		}

		public Vector2 GetRenderedValues()
		{
			return this.GetTextBounds().size;
		}

		public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
		{
			return this.GetTextBounds(onlyVisibleCharacters).size;
		}

		protected float GetRenderedWidth()
		{
			return this.GetRenderedValues().x;
		}

		protected float GetRenderedWidth(bool onlyVisibleCharacters)
		{
			return this.GetRenderedValues(onlyVisibleCharacters).x;
		}

		protected float GetRenderedHeight()
		{
			return this.GetRenderedValues().y;
		}

		protected float GetRenderedHeight(bool onlyVisibleCharacters)
		{
			return this.GetRenderedValues(onlyVisibleCharacters).y;
		}

		protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
		{
			if (!(this.m_fontAsset == null))
			{
				if (this.m_fontAsset.characterDictionary != null)
				{
					if (this.m_char_buffer != null)
					{
						if (this.m_char_buffer.Length != 0)
						{
							if (this.m_char_buffer[0] != 0)
							{
								this.m_currentFontAsset = this.m_fontAsset;
								this.m_currentMaterial = this.m_sharedMaterial;
								this.m_currentMaterialIndex = 0;
								this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
								int totalCharacterCount = this.m_totalCharacterCount;
								if (this.m_internalCharacterInfo != null)
								{
									if (totalCharacterCount <= this.m_internalCharacterInfo.Length)
									{
										goto IL_125;
									}
								}
								this.m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount <= 0x400) ? Mathf.NextPowerOfTwo(totalCharacterCount) : (totalCharacterCount + 0x100)];
								IL_125:
								float num = defaultFontSize / this.m_currentFontAsset.fontInfo.PointSize;
								float num2;
								if (this.m_isOrthographic)
								{
									num2 = 1f;
								}
								else
								{
									num2 = 0.1f;
								}
								this.m_fontScale = num * num2;
								this.m_fontScaleMultiplier = 1f;
								float num3 = defaultFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
								float num4;
								if (this.m_isOrthographic)
								{
									num4 = 1f;
								}
								else
								{
									num4 = 0.1f;
								}
								float num5 = num3 * num4;
								float num6 = this.m_fontScale;
								this.m_currentFontSize = defaultFontSize;
								this.m_sizeStack.SetDefault(this.m_currentFontSize);
								this.m_style = this.m_fontStyle;
								this.m_lineJustification = this.m_textAlignment;
								this.m_lineJustificationStack.SetDefault(this.m_lineJustification);
								this.m_baselineOffset = 0f;
								this.m_baselineOffsetStack.Clear();
								this.m_lineOffset = 0f;
								this.m_lineHeight = -32767f;
								float num7 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
								this.m_cSpacing = 0f;
								this.m_monoSpacing = 0f;
								this.m_xAdvance = 0f;
								float a = 0f;
								this.tag_LineIndent = 0f;
								this.tag_Indent = 0f;
								this.m_indentStack.SetDefault(0f);
								this.tag_NoParsing = false;
								this.m_characterCount = 0;
								this.m_firstCharacterOfLine = 0;
								this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
								this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
								this.m_lineNumber = 0;
								float x = marginSize.x;
								this.m_marginLeft = 0f;
								this.m_marginRight = 0f;
								this.m_width = -1f;
								float num8 = 0f;
								float num9 = 0f;
								float num10 = 0f;
								this.m_isCalculatingPreferredValues = true;
								this.m_maxAscender = 0f;
								this.m_maxDescender = 0f;
								bool flag = true;
								bool flag2 = false;
								WordWrapState wordWrapState = default(WordWrapState);
								this.SaveWordWrappingState(ref wordWrapState, 0, 0);
								WordWrapState wordWrapState2 = default(WordWrapState);
								int num11 = 0;
								this.m_recursiveCount++;
								int num12 = 0;
								int num13 = 0;
								while (this.m_char_buffer[num13] != 0)
								{
									int num14 = this.m_char_buffer[num13];
									this.m_textElementType = this.m_textInfo.characterInfo[this.m_characterCount].elementType;
									this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
									this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
									int currentMaterialIndex = this.m_currentMaterialIndex;
									if (!this.m_isRichText || num14 != 0x3C)
									{
										goto IL_44F;
									}
									this.m_isParsingText = true;
									this.m_textElementType = TMP_TextElementType.Character;
									if (!this.ValidateHtmlTag(this.m_char_buffer, num13 + 1, out num12))
									{
										goto IL_44F;
									}
									num13 = num12;
									if (this.m_textElementType != TMP_TextElementType.Character)
									{
										goto IL_44F;
									}
									IL_1681:
									num13++;
									continue;
									IL_44F:
									this.m_isParsingText = false;
									bool isUsingAlternateTypeface = this.m_textInfo.characterInfo[this.m_characterCount].isUsingAlternateTypeface;
									float num15 = 1f;
									if (this.m_textElementType == TMP_TextElementType.Character)
									{
										if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
										{
											if (char.IsLower((char)num14))
											{
												num14 = (int)char.ToUpper((char)num14);
											}
										}
										else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
										{
											if (char.IsUpper((char)num14))
											{
												num14 = (int)char.ToLower((char)num14);
											}
										}
										else
										{
											if ((this.m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
											{
												if ((this.m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
												{
													goto IL_545;
												}
											}
											if (char.IsLower((char)num14))
											{
												num15 = 0.8f;
												num14 = (int)char.ToUpper((char)num14);
											}
										}
									}
									IL_545:
									if (this.m_textElementType == TMP_TextElementType.Sprite)
									{
										this.m_currentSpriteAsset = this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset;
										this.m_spriteIndex = this.m_textInfo.characterInfo[this.m_characterCount].spriteIndex;
										TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
										if (tmp_Sprite == null)
										{
											goto IL_1681;
										}
										if (num14 == 0x3C)
										{
											num14 = 0xE000 + this.m_spriteIndex;
										}
										this.m_currentFontAsset = this.m_fontAsset;
										float num16 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
										float num17;
										if (this.m_isOrthographic)
										{
											num17 = 1f;
										}
										else
										{
											num17 = 0.1f;
										}
										float num18 = num16 * num17;
										num6 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num18;
										this.m_cached_TextElement = tmp_Sprite;
										this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
										this.m_internalCharacterInfo[this.m_characterCount].scale = num18;
										this.m_currentMaterialIndex = currentMaterialIndex;
									}
									else if (this.m_textElementType == TMP_TextElementType.Character)
									{
										this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
										if (this.m_cached_TextElement == null)
										{
											goto IL_1681;
										}
										this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
										float num19 = this.m_currentFontSize * num15 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
										float num20;
										if (this.m_isOrthographic)
										{
											num20 = 1f;
										}
										else
										{
											num20 = 0.1f;
										}
										this.m_fontScale = num19 * num20;
										num6 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
										this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
									}
									float num21 = num6;
									if (num14 == 0xAD)
									{
										num6 = 0f;
									}
									this.m_internalCharacterInfo[this.m_characterCount].character = (char)num14;
									if (this.m_enableKerning)
									{
										if (this.m_characterCount >= 1)
										{
											int character = (int)this.m_internalCharacterInfo[this.m_characterCount - 1].character;
											KerningPairKey kerningPairKey = new KerningPairKey(character, num14);
											KerningPair kerningPair;
											this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
											if (kerningPair != null)
											{
												this.m_xAdvance += kerningPair.XadvanceOffset * num6;
											}
										}
									}
									float num22 = 0f;
									if (this.m_monoSpacing != 0f)
									{
										num22 = this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num6;
										this.m_xAdvance += num22;
									}
									if (this.m_textElementType != TMP_TextElementType.Character)
									{
										goto IL_908;
									}
									if (isUsingAlternateTypeface)
									{
										goto IL_908;
									}
									if ((this.m_style & FontStyles.Bold) != FontStyles.Bold)
									{
										if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
										{
											goto IL_908;
										}
									}
									float num23 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
									IL_90F:
									this.m_internalCharacterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
									float ascender = this.m_currentFontAsset.fontInfo.Ascender;
									float num24;
									if (this.m_textElementType == TMP_TextElementType.Character)
									{
										num24 = num6;
									}
									else
									{
										num24 = this.m_internalCharacterInfo[this.m_characterCount].scale;
									}
									float num25 = ascender * num24 + this.m_baselineOffset;
									this.m_internalCharacterInfo[this.m_characterCount].ascender = num25 - this.m_lineOffset;
									float maxLineAscender;
									if (num25 > this.m_maxLineAscender)
									{
										maxLineAscender = num25;
									}
									else
									{
										maxLineAscender = this.m_maxLineAscender;
									}
									this.m_maxLineAscender = maxLineAscender;
									float descender = this.m_currentFontAsset.fontInfo.Descender;
									float num26;
									if (this.m_textElementType == TMP_TextElementType.Character)
									{
										num26 = num6;
									}
									else
									{
										num26 = this.m_internalCharacterInfo[this.m_characterCount].scale;
									}
									float num27 = descender * num26 + this.m_baselineOffset;
									float num28 = this.m_internalCharacterInfo[this.m_characterCount].descender = num27 - this.m_lineOffset;
									float maxLineDescender;
									if (num27 < this.m_maxLineDescender)
									{
										maxLineDescender = num27;
									}
									else
									{
										maxLineDescender = this.m_maxLineDescender;
									}
									this.m_maxLineDescender = maxLineDescender;
									if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
									{
										goto IL_A86;
									}
									if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
									{
										goto IL_A86;
									}
									IL_B10:
									if (this.m_lineNumber == 0)
									{
										float maxAscender;
										if (this.m_maxAscender > num25)
										{
											maxAscender = this.m_maxAscender;
										}
										else
										{
											maxAscender = num25;
										}
										this.m_maxAscender = maxAscender;
									}
									if (num14 != 9)
									{
										if (!char.IsWhiteSpace((char)num14))
										{
											if (num14 != 0x200B)
											{
												goto IL_B8B;
											}
										}
										if (this.m_textElementType != TMP_TextElementType.Sprite)
										{
											goto IL_FB6;
										}
									}
									IL_B8B:
									float num29;
									if (this.m_width != -1f)
									{
										num29 = Mathf.Min(x + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
									}
									else
									{
										num29 = x + 0.0001f - this.m_marginLeft - this.m_marginRight;
									}
									float num30 = num29;
									bool flag3;
									if ((this.m_lineJustification & (TextAlignmentOptions)0x10) != (TextAlignmentOptions)0x10)
									{
										flag3 = ((this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8);
									}
									else
									{
										flag3 = true;
									}
									bool flag4 = flag3;
									num10 = this.m_xAdvance + this.m_cached_TextElement.xAdvance * ((num14 == 0xAD) ? num21 : num6);
									float num31 = num10;
									float num32 = num30;
									float num33;
									if (flag4)
									{
										num33 = 1.05f;
									}
									else
									{
										num33 = 1f;
									}
									if (num31 > num32 * num33)
									{
										if (this.enableWordWrapping)
										{
											if (this.m_characterCount != this.m_firstCharacterOfLine)
											{
												if (num11 == wordWrapState2.previous_WordBreak)
												{
													goto IL_CA2;
												}
												if (flag)
												{
													goto IL_CA2;
												}
												IL_CC0:
												num13 = this.RestoreWordWrappingState(ref wordWrapState2);
												num11 = num13;
												if (this.m_char_buffer[num13] == 0xAD)
												{
													this.m_isTextTruncated = true;
													this.m_char_buffer[num13] = 0x2D;
													this.CalculatePreferredValues(defaultFontSize, marginSize, true);
													return Vector2.zero;
												}
												if (this.m_lineNumber > 0)
												{
													if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
													{
														if (this.m_lineHeight == -32767f)
														{
															float num34 = this.m_maxLineAscender - this.m_startOfLineAscender;
															this.m_lineOffset += num34;
															wordWrapState2.lineOffset = this.m_lineOffset;
															wordWrapState2.previousLineAscender = this.m_maxLineAscender;
														}
													}
												}
												float num35 = this.m_maxLineAscender - this.m_lineOffset;
												float num36 = this.m_maxLineDescender - this.m_lineOffset;
												float maxDescender;
												if (this.m_maxDescender < num36)
												{
													maxDescender = this.m_maxDescender;
												}
												else
												{
													maxDescender = num36;
												}
												this.m_maxDescender = maxDescender;
												this.m_firstCharacterOfLine = this.m_characterCount;
												num8 += this.m_xAdvance;
												if (this.m_enableWordWrapping)
												{
													num9 = this.m_maxAscender - this.m_maxDescender;
												}
												else
												{
													num9 = Mathf.Max(num9, num35 - num36);
												}
												this.SaveWordWrappingState(ref wordWrapState, num13, this.m_characterCount - 1);
												this.m_lineNumber++;
												if (this.m_lineHeight == -32767f)
												{
													float num37 = this.m_internalCharacterInfo[this.m_characterCount].ascender - this.m_internalCharacterInfo[this.m_characterCount].baseLine;
													float num38 = 0f - this.m_maxLineDescender + num37 + (num7 + this.m_lineSpacing + this.m_lineSpacingDelta) * num5;
													this.m_lineOffset += num38;
													this.m_startOfLineAscender = num37;
												}
												else
												{
													this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num5;
												}
												this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
												this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
												this.m_xAdvance = this.tag_Indent;
												goto IL_1681;
												IL_CA2:
												if (!this.m_isCharacterWrappingEnabled)
												{
													this.m_isCharacterWrappingEnabled = true;
													goto IL_CC0;
												}
												flag2 = true;
												goto IL_CC0;
											}
										}
										if (!ignoreTextAutoSizing)
										{
											if (defaultFontSize > this.m_fontSizeMin)
											{
												if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
												{
												}
												this.m_maxFontSize = defaultFontSize;
												defaultFontSize -= Mathf.Max((defaultFontSize - this.m_minFontSize) / 2f, 0.05f);
												defaultFontSize = (float)((int)(Mathf.Max(defaultFontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
												if (this.m_recursiveCount > 0x14)
												{
													return new Vector2(num8, num9);
												}
												return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
											}
										}
									}
									IL_FB6:
									if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
									{
										if (this.m_lineHeight == -32767f)
										{
											if (!this.m_isNewPage)
											{
												float num39 = this.m_maxLineAscender - this.m_startOfLineAscender;
												num28 -= num39;
												this.m_lineOffset += num39;
												this.m_startOfLineAscender += num39;
												wordWrapState2.lineOffset = this.m_lineOffset;
												wordWrapState2.previousLineAscender = this.m_startOfLineAscender;
											}
										}
									}
									if (num14 == 9)
									{
										float num40 = this.m_currentFontAsset.fontInfo.TabWidth * num6;
										float num41 = Mathf.Ceil(this.m_xAdvance / num40) * num40;
										float xAdvance;
										if (num41 > this.m_xAdvance)
										{
											xAdvance = num41;
										}
										else
										{
											xAdvance = this.m_xAdvance + num40;
										}
										this.m_xAdvance = xAdvance;
									}
									else if (this.m_monoSpacing != 0f)
									{
										this.m_xAdvance += this.m_monoSpacing - num22 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 + this.m_cSpacing;
										if (char.IsWhiteSpace((char)num14))
										{
											goto IL_1120;
										}
										if (num14 == 0x200B)
										{
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												goto IL_1120;
											}
										}
										goto IL_11AC;
										IL_1120:
										this.m_xAdvance += this.m_wordSpacing * num6;
									}
									else
									{
										this.m_xAdvance += (this.m_cached_TextElement.xAdvance * num23 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 + this.m_cSpacing;
										if (!char.IsWhiteSpace((char)num14))
										{
											if (num14 != 0x200B)
											{
												goto IL_11AC;
											}
										}
										this.m_xAdvance += this.m_wordSpacing * num6;
									}
									IL_11AC:
									if (num14 == 0xD)
									{
										a = Mathf.Max(a, num8 + this.m_xAdvance);
										num8 = 0f;
										this.m_xAdvance = this.tag_Indent;
									}
									if (num14 == 0xA)
									{
										goto IL_120B;
									}
									if (this.m_characterCount == totalCharacterCount - 1)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											goto IL_120B;
										}
									}
									IL_13D7:
									if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate)
									{
										goto IL_13FE;
									}
									if (this.m_overflowMode == TextOverflowModes.Ellipsis)
									{
										goto IL_13FE;
									}
									IL_1673:
									this.m_characterCount++;
									goto IL_1681;
									IL_13FE:
									if (char.IsWhiteSpace((char)num14))
									{
										goto IL_1443;
									}
									if (num14 == 0x200B)
									{
										goto IL_1443;
									}
									if (num14 == 0x2D)
									{
										goto IL_1443;
									}
									if (num14 == 0xAD)
									{
										goto IL_1443;
									}
									IL_14B6:
									if (num14 <= 0x1100)
									{
										goto IL_14DF;
									}
									if (num14 < 0x11FF)
									{
										goto IL_1586;
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										goto IL_14DF;
									}
									IL_163F:
									if (!flag && !this.m_isCharacterWrappingEnabled)
									{
										if (!flag2)
										{
											goto IL_1673;
										}
									}
									this.SaveWordWrappingState(ref wordWrapState2, num13, this.m_characterCount);
									goto IL_1673;
									IL_14DF:
									if (num14 > 0x2E80)
									{
										if (num14 < 0x9FFF)
										{
											goto IL_1586;
										}
									}
									if (num14 > 0xA960)
									{
										if (num14 < 0xA97F)
										{
											goto IL_1586;
										}
									}
									if (num14 <= 0xAC00 || num14 >= 0xD7FF)
									{
										if (num14 > 0xF900)
										{
											if (num14 < 0xFAFF)
											{
												goto IL_1586;
											}
										}
										if (num14 <= 0xFE30 || num14 >= 0xFE4F)
										{
											if (num14 <= 0xFF00 || num14 >= 0xFFEF)
											{
												goto IL_163F;
											}
										}
									}
									IL_1586:
									if (!this.m_isNonBreakingSpace)
									{
										if (flag)
										{
											goto IL_1623;
										}
										if (flag2)
										{
											goto IL_1623;
										}
										if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num14))
										{
											if (this.m_characterCount < totalCharacterCount - 1)
											{
												if (!TMP_Settings.linebreakingRules.followingCharacters.ContainsKey((int)this.m_internalCharacterInfo[this.m_characterCount + 1].character))
												{
													for (;;)
													{
														switch (3)
														{
														case 0:
															continue;
														}
														goto IL_1623;
													}
												}
											}
										}
										IL_163D:
										goto IL_1673;
										IL_1623:
										this.SaveWordWrappingState(ref wordWrapState2, num13, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag = false;
										goto IL_163D;
									}
									goto IL_163F;
									IL_1443:
									if (this.m_isNonBreakingSpace)
									{
										goto IL_14B6;
									}
									if (num14 == 0xA0)
									{
										goto IL_14B6;
									}
									if (num14 == 0x2011)
									{
										goto IL_14B6;
									}
									if (num14 == 0x202F)
									{
										goto IL_14B6;
									}
									if (num14 != 0x2060)
									{
										this.SaveWordWrappingState(ref wordWrapState2, num13, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag = false;
										goto IL_1673;
									}
									goto IL_14B6;
									IL_120B:
									if (this.m_lineNumber > 0)
									{
										if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
										{
											if (this.m_lineHeight == -32767f)
											{
												float num42 = this.m_maxLineAscender - this.m_startOfLineAscender;
												num28 -= num42;
												this.m_lineOffset += num42;
											}
										}
									}
									float num43 = this.m_maxLineDescender - this.m_lineOffset;
									this.m_maxDescender = ((this.m_maxDescender >= num43) ? num43 : this.m_maxDescender);
									this.m_firstCharacterOfLine = this.m_characterCount + 1;
									if (num14 == 0xA && this.m_characterCount != totalCharacterCount - 1)
									{
										a = Mathf.Max(a, num8 + num10);
										num8 = 0f;
									}
									else
									{
										num8 = Mathf.Max(a, num8 + num10);
									}
									num9 = this.m_maxAscender - this.m_maxDescender;
									if (num14 == 0xA)
									{
										this.SaveWordWrappingState(ref wordWrapState, num13, this.m_characterCount);
										this.SaveWordWrappingState(ref wordWrapState2, num13, this.m_characterCount);
										this.m_lineNumber++;
										if (this.m_lineHeight == -32767f)
										{
											float num38 = 0f - this.m_maxLineDescender + num25 + (num7 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num5;
											this.m_lineOffset += num38;
										}
										else
										{
											this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num5;
										}
										this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
										this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
										this.m_startOfLineAscender = num25;
										this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
										goto IL_13D7;
									}
									goto IL_13D7;
									IL_A86:
									float num44 = (num25 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num25 = this.m_maxLineAscender;
									float maxLineAscender2;
									if (num44 > this.m_maxLineAscender)
									{
										maxLineAscender2 = num44;
									}
									else
									{
										maxLineAscender2 = this.m_maxLineAscender;
									}
									this.m_maxLineAscender = maxLineAscender2;
									float num45 = (num27 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num27 = this.m_maxLineDescender;
									this.m_maxLineDescender = ((num45 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num45);
									goto IL_B10;
									IL_908:
									num23 = 1f;
									goto IL_90F;
								}
								float num46 = this.m_maxFontSize - this.m_minFontSize;
								if (!this.m_isCharacterWrappingEnabled && !ignoreTextAutoSizing && num46 > 0.051f)
								{
									if (defaultFontSize < this.m_fontSizeMax)
									{
										this.m_minFontSize = defaultFontSize;
										defaultFontSize += Mathf.Max((this.m_maxFontSize - defaultFontSize) / 2f, 0.05f);
										defaultFontSize = (float)((int)(Mathf.Min(defaultFontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
										if (this.m_recursiveCount > 0x14)
										{
											return new Vector2(num8, num9);
										}
										return this.CalculatePreferredValues(defaultFontSize, marginSize, false);
									}
								}
								this.m_isCharacterWrappingEnabled = false;
								this.m_isCalculatingPreferredValues = false;
								float num47 = num8;
								float num48;
								if (this.m_margin.x > 0f)
								{
									num48 = this.m_margin.x;
								}
								else
								{
									num48 = 0f;
								}
								num8 = num47 + num48;
								float num49 = num8;
								float num50;
								if (this.m_margin.z > 0f)
								{
									num50 = this.m_margin.z;
								}
								else
								{
									num50 = 0f;
								}
								num8 = num49 + num50;
								float num51 = num9;
								float num52;
								if (this.m_margin.y > 0f)
								{
									num52 = this.m_margin.y;
								}
								else
								{
									num52 = 0f;
								}
								num9 = num51 + num52;
								float num53 = num9;
								float num54;
								if (this.m_margin.w > 0f)
								{
									num54 = this.m_margin.w;
								}
								else
								{
									num54 = 0f;
								}
								num9 = num53 + num54;
								num8 = (float)((int)(num8 * 100f + 1f)) / 100f;
								num9 = (float)((int)(num9 * 100f + 1f)) / 100f;
								return new Vector2(num8, num9);
							}
						}
					}
					return Vector2.zero;
				}
			}
			Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + base.GetInstanceID());
			return Vector2.zero;
		}

		protected virtual Bounds GetCompoundBounds()
		{
			return default(Bounds);
		}

		protected Bounds GetTextBounds()
		{
			if (this.m_textInfo != null)
			{
				if (this.m_textInfo.characterCount <= this.m_textInfo.characterInfo.Length)
				{
					Extents extents = new Extents(TMP_Text.k_LargePositiveVector2, TMP_Text.k_LargeNegativeVector2);
					int i = 0;
					while (i < this.m_textInfo.characterCount)
					{
						if (i >= this.m_textInfo.characterInfo.Length)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								goto IL_18C;
							}
						}
						else
						{
							if (!this.m_textInfo.characterInfo[i].isVisible)
							{
							}
							else
							{
								extents.min.x = Mathf.Min(extents.min.x, this.m_textInfo.characterInfo[i].bottomLeft.x);
								extents.min.y = Mathf.Min(extents.min.y, this.m_textInfo.characterInfo[i].descender);
								extents.max.x = Mathf.Max(extents.max.x, this.m_textInfo.characterInfo[i].xAdvance);
								extents.max.y = Mathf.Max(extents.max.y, this.m_textInfo.characterInfo[i].ascender);
							}
							i++;
						}
					}
					IL_18C:
					Vector2 v;
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
			if (this.m_textInfo == null)
			{
				return default(Bounds);
			}
			Extents extents = new Extents(TMP_Text.k_LargePositiveVector2, TMP_Text.k_LargeNegativeVector2);
			int i = 0;
			while (i < this.m_textInfo.characterCount)
			{
				if (i > this.maxVisibleCharacters)
				{
					goto IL_7A;
				}
				if ((int)this.m_textInfo.characterInfo[i].lineNumber > this.m_maxVisibleLines)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_7A;
					}
				}
				IL_82:
				if (!onlyVisibleCharacters)
				{
					goto IL_B6;
				}
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					goto IL_B6;
				}
				IL_186:
				i++;
				continue;
				IL_B6:
				extents.min.x = Mathf.Min(extents.min.x, this.m_textInfo.characterInfo[i].origin);
				extents.min.y = Mathf.Min(extents.min.y, this.m_textInfo.characterInfo[i].descender);
				extents.max.x = Mathf.Max(extents.max.x, this.m_textInfo.characterInfo[i].xAdvance);
				extents.max.y = Mathf.Max(extents.max.y, this.m_textInfo.characterInfo[i].ascender);
				goto IL_186;
				IL_7A:
				if (onlyVisibleCharacters)
				{
					IL_1A5:
					Vector2 v;
					v.x = extents.max.x - extents.min.x;
					v.y = extents.max.y - extents.min.y;
					Vector2 v2 = (extents.min + extents.max) / 2f;
					return new Bounds(v2, v);
				}
				goto IL_82;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				goto IL_1A5;
			}
		}

		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		protected void ResizeLineExtents(int size)
		{
			int num;
			if (size > 0x400)
			{
				num = size + 0x100;
			}
			else
			{
				num = Mathf.NextPowerOfTwo(size + 1);
			}
			size = num;
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < this.m_textInfo.lineInfo.Length)
				{
					array[i] = this.m_textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = TMP_Text.k_LargePositiveVector2;
					array[i].lineExtents.max = TMP_Text.k_LargeNegativeVector2;
					array[i].ascender = TMP_Text.k_LargeNegativeFloat;
					array[i].descender = TMP_Text.k_LargePositiveFloat;
				}
			}
			this.m_textInfo.lineInfo = array;
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
			state.currentFontAsset = this.m_currentFontAsset;
			state.currentSpriteAsset = this.m_currentSpriteAsset;
			state.currentMaterial = this.m_currentMaterial;
			state.currentMaterialIndex = this.m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = this.m_lineVisibleCharacterCount;
			state.visible_LinkCount = this.m_textInfo.linkCount;
			state.firstCharacterIndex = this.m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = this.m_lastVisibleCharacterOfLine;
			state.fontStyle = this.m_style;
			state.fontScale = this.m_fontScale;
			state.fontScaleMultiplier = this.m_fontScaleMultiplier;
			state.currentFontSize = this.m_currentFontSize;
			state.xAdvance = this.m_xAdvance;
			state.maxCapHeight = this.m_maxCapHeight;
			state.maxAscender = this.m_maxAscender;
			state.maxDescender = this.m_maxDescender;
			state.maxLineAscender = this.m_maxLineAscender;
			state.maxLineDescender = this.m_maxLineDescender;
			state.previousLineAscender = this.m_startOfLineAscender;
			state.preferredWidth = this.m_preferredWidth;
			state.preferredHeight = this.m_preferredHeight;
			state.meshExtents = this.m_meshExtents;
			state.lineNumber = this.m_lineNumber;
			state.lineOffset = this.m_lineOffset;
			state.baselineOffset = this.m_baselineOffset;
			state.vertexColor = this.m_htmlColor;
			state.underlineColor = this.m_underlineColor;
			state.strikethroughColor = this.m_strikethroughColor;
			state.highlightColor = this.m_highlightColor;
			state.tagNoParsing = this.tag_NoParsing;
			state.basicStyleStack = this.m_fontStyleStack;
			state.colorStack = this.m_colorStack;
			state.underlineColorStack = this.m_underlineColorStack;
			state.strikethroughColorStack = this.m_strikethroughColorStack;
			state.highlightColorStack = this.m_highlightColorStack;
			state.sizeStack = this.m_sizeStack;
			state.indentStack = this.m_indentStack;
			state.fontWeightStack = this.m_fontWeightStack;
			state.styleStack = this.m_styleStack;
			state.baselineStack = this.m_baselineOffsetStack;
			state.actionStack = this.m_actionStack;
			state.materialReferenceStack = this.m_materialReferenceStack;
			state.lineJustificationStack = this.m_lineJustificationStack;
			state.spriteAnimationID = this.m_spriteAnimationID;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				state.lineInfo = this.m_textInfo.lineInfo[this.m_lineNumber];
			}
		}

		protected unsafe int RestoreWordWrappingState(ref WordWrapState state)
		{
			int previous_WordBreak = state.previous_WordBreak;
			this.m_currentFontAsset = state.currentFontAsset;
			this.m_currentSpriteAsset = state.currentSpriteAsset;
			this.m_currentMaterial = state.currentMaterial;
			this.m_currentMaterialIndex = state.currentMaterialIndex;
			this.m_characterCount = state.total_CharacterCount + 1;
			this.m_lineVisibleCharacterCount = state.visible_CharacterCount;
			this.m_textInfo.linkCount = state.visible_LinkCount;
			this.m_firstCharacterOfLine = state.firstCharacterIndex;
			this.m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			this.m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			this.m_style = state.fontStyle;
			this.m_fontScale = state.fontScale;
			this.m_fontScaleMultiplier = state.fontScaleMultiplier;
			this.m_currentFontSize = state.currentFontSize;
			this.m_xAdvance = state.xAdvance;
			this.m_maxCapHeight = state.maxCapHeight;
			this.m_maxAscender = state.maxAscender;
			this.m_maxDescender = state.maxDescender;
			this.m_maxLineAscender = state.maxLineAscender;
			this.m_maxLineDescender = state.maxLineDescender;
			this.m_startOfLineAscender = state.previousLineAscender;
			this.m_preferredWidth = state.preferredWidth;
			this.m_preferredHeight = state.preferredHeight;
			this.m_meshExtents = state.meshExtents;
			this.m_lineNumber = state.lineNumber;
			this.m_lineOffset = state.lineOffset;
			this.m_baselineOffset = state.baselineOffset;
			this.m_htmlColor = state.vertexColor;
			this.m_underlineColor = state.underlineColor;
			this.m_strikethroughColor = state.strikethroughColor;
			this.m_highlightColor = state.highlightColor;
			this.tag_NoParsing = state.tagNoParsing;
			this.m_fontStyleStack = state.basicStyleStack;
			this.m_colorStack = state.colorStack;
			this.m_underlineColorStack = state.underlineColorStack;
			this.m_strikethroughColorStack = state.strikethroughColorStack;
			this.m_highlightColorStack = state.highlightColorStack;
			this.m_sizeStack = state.sizeStack;
			this.m_indentStack = state.indentStack;
			this.m_fontWeightStack = state.fontWeightStack;
			this.m_styleStack = state.styleStack;
			this.m_baselineOffsetStack = state.baselineStack;
			this.m_actionStack = state.actionStack;
			this.m_materialReferenceStack = state.materialReferenceStack;
			this.m_lineJustificationStack = state.lineJustificationStack;
			this.m_spriteAnimationID = state.spriteAnimationID;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				this.m_textInfo.lineInfo[this.m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			byte a;
			if (this.m_fontColor32.a < vertexColor.a)
			{
				a = this.m_fontColor32.a;
			}
			else
			{
				a = vertexColor.a;
			}
			vertexColor.a = a;
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!this.m_overrideHtmlColors && this.m_colorStack.index > 1)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (this.m_fontColorGradientPreset != null)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradientPreset.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradientPreset.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradientPreset.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradientPreset.bottomRight * vertexColor;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradient.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradient.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradient.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradient.bottomRight * vertexColor;
			}
			if (!this.m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = this.m_currentFontAsset.fontInfo;
			Vector2 uv;
			uv.x = (this.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			uv.y = 1f - (this.m_cached_TextElement.y + padding + style_padding + this.m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 uv2;
			uv2.x = uv.x;
			uv2.y = 1f - (this.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 uv3;
			uv3.x = (this.m_cached_TextElement.x + padding + style_padding + this.m_cached_TextElement.width) / fontInfo.AtlasWidth;
			uv3.y = uv2.y;
			Vector2 uv4;
			uv4.x = uv3.x;
			uv4.y = uv.y;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			if (this.m_tintAllSprites)
			{
				this.m_tintSprite = true;
			}
			Color32 color;
			if (this.m_tintSprite)
			{
				color = this.m_spriteColor.Multiply(vertexColor);
			}
			else
			{
				color = this.m_spriteColor;
			}
			Color32 color2 = color;
			byte a;
			if (color2.a < this.m_fontColor32.a)
			{
				a = (color2.a = ((color2.a >= vertexColor.a) ? vertexColor.a : color2.a));
			}
			else
			{
				a = this.m_fontColor32.a;
			}
			color2.a = a;
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color2;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color2;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color2;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color2;
			}
			else
			{
				if (!this.m_overrideHtmlColors)
				{
					if (this.m_colorStack.index > 1)
					{
						this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color2;
						this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color2;
						this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color2;
						this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color2;
						goto IL_55A;
					}
				}
				if (this.m_fontColorGradientPreset != null)
				{
					TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
					int characterCount = this.m_characterCount;
					Color32 color3;
					if (this.m_tintSprite)
					{
						color3 = color2.Multiply(this.m_fontColorGradientPreset.bottomLeft);
					}
					else
					{
						color3 = color2;
					}
					characterInfo[characterCount].vertex_BL.color = color3;
					TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
					int characterCount2 = this.m_characterCount;
					Color32 color4;
					if (this.m_tintSprite)
					{
						color4 = color2.Multiply(this.m_fontColorGradientPreset.topLeft);
					}
					else
					{
						color4 = color2;
					}
					characterInfo2[characterCount2].vertex_TL.color = color4;
					TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
					int characterCount3 = this.m_characterCount;
					Color32 color5;
					if (this.m_tintSprite)
					{
						color5 = color2.Multiply(this.m_fontColorGradientPreset.topRight);
					}
					else
					{
						color5 = color2;
					}
					characterInfo3[characterCount3].vertex_TR.color = color5;
					this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = ((!this.m_tintSprite) ? color2 : color2.Multiply(this.m_fontColorGradientPreset.bottomRight));
				}
				else
				{
					this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = ((!this.m_tintSprite) ? color2 : color2.Multiply(this.m_fontColorGradient.bottomLeft));
					TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
					int characterCount4 = this.m_characterCount;
					Color32 color6;
					if (this.m_tintSprite)
					{
						color6 = color2.Multiply(this.m_fontColorGradient.topLeft);
					}
					else
					{
						color6 = color2;
					}
					characterInfo4[characterCount4].vertex_TL.color = color6;
					TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
					int characterCount5 = this.m_characterCount;
					Color32 color7;
					if (this.m_tintSprite)
					{
						color7 = color2.Multiply(this.m_fontColorGradient.topRight);
					}
					else
					{
						color7 = color2;
					}
					characterInfo5[characterCount5].vertex_TR.color = color7;
					TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
					int characterCount6 = this.m_characterCount;
					Color32 color8;
					if (this.m_tintSprite)
					{
						color8 = color2.Multiply(this.m_fontColorGradient.bottomRight);
					}
					else
					{
						color8 = color2;
					}
					characterInfo6[characterCount6].vertex_BR.color = color8;
				}
			}
			IL_55A:
			Vector2 uv = new Vector2(this.m_cached_TextElement.x / (float)this.m_currentSpriteAsset.spriteSheet.width, this.m_cached_TextElement.y / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv2 = new Vector2(uv.x, (this.m_cached_TextElement.y + this.m_cached_TextElement.height) / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv3 = new Vector2((this.m_cached_TextElement.x + this.m_cached_TextElement.width) / (float)this.m_currentSpriteAsset.spriteSheet.width, uv2.y);
			Vector2 uv4 = new Vector2(uv3.x, uv.y);
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			if (isVolumetric)
			{
				Vector3 b = new Vector3(0f, 0f, this.m_fontSize * this.m_fontScale);
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[4 + index_X4] = characterInfo[i].vertex_BL.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[5 + index_X4] = characterInfo[i].vertex_TL.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[6 + index_X4] = characterInfo[i].vertex_TR.position + b;
				this.m_textInfo.meshInfo[materialReferenceIndex].vertices[7 + index_X4] = characterInfo[i].vertex_BR.position + b;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			if (isVolumetric)
			{
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[4 + index_X4] = characterInfo[i].vertex_BL.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[5 + index_X4] = characterInfo[i].vertex_TL.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[6 + index_X4] = characterInfo[i].vertex_TR.uv;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[7 + index_X4] = characterInfo[i].vertex_BR.uv;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			if (isVolumetric)
			{
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[4 + index_X4] = characterInfo[i].vertex_BL.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[5 + index_X4] = characterInfo[i].vertex_TL.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[6 + index_X4] = characterInfo[i].vertex_TR.uv2;
				this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[7 + index_X4] = characterInfo[i].vertex_BR.uv2;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			if (isVolumetric)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 0x80, byte.MaxValue);
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[4 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[5 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[6 + index_X4] = color;
				this.m_textInfo.meshInfo[materialReferenceIndex].colors32[7 + index_X4] = color;
			}
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + (isVolumetric ? 8 : 4);
		}

		protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		protected unsafe virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
				}
				return;
			}
			int num = index + 0xC;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = this.m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < this.m_cached_Underline_GlyphInfo.width * maxScale)
			{
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = this.m_padding * startScale / maxScale;
			float num4 = this.m_padding * endScale / maxScale;
			float height = this.m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + this.m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(-num2, this.m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(-num2, -(height + this.m_padding) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 0xA] = end + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 0xB] = end + new Vector3(0f, -(height + this.m_padding) * maxScale, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_padding + this.m_cached_Underline_GlyphInfo.height) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector2 = new Vector2(vector.x, 1f - (this.m_cached_Underline_GlyphInfo.y - this.m_padding) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector3 = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector4 = new Vector2(vector3.x, vector.y);
			Vector2 vector5 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector6 = new Vector2(vector5.x, vector.y);
			Vector2 vector7 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
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
			uvs[0xA + index] = vector7;
			uvs[0xB + index] = vector8;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float scale = Mathf.Abs(sdfScale);
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = this.PackUV(0f, 0f, scale);
			uvs2[1 + index] = this.PackUV(0f, 1f, scale);
			uvs2[2 + index] = this.PackUV(x, 1f, scale);
			uvs2[3 + index] = this.PackUV(x, 0f, scale);
			float x2 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = this.PackUV(x2, 0f, scale);
			uvs2[5 + index] = this.PackUV(x2, 1f, scale);
			uvs2[6 + index] = this.PackUV(x, 1f, scale);
			uvs2[7 + index] = this.PackUV(x, 0f, scale);
			x2 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = this.PackUV(x2, 0f, scale);
			uvs2[9 + index] = this.PackUV(x2, 1f, scale);
			uvs2[0xA + index] = this.PackUV(1f, 1f, scale);
			uvs2[0xB + index] = this.PackUV(1f, 0f, scale);
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
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
			colors[0xA + index] = underlineColor;
			colors[0xB + index] = underlineColor;
			index += 0xC;
		}

		protected unsafe virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning("Unable to add underline since the Font Asset doesn't contain the underline character.", this);
				}
				return;
			}
			int num = index + 4;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start;
			vertices[index + 1] = new Vector3(start.x, end.y, 0f);
			vertices[index + 2] = end;
			vertices[index + 3] = new Vector3(end.x, start.y, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((this.m_cached_Underline_GlyphInfo.x + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_cached_Underline_GlyphInfo.height / 2f) / this.m_fontAsset.fontInfo.AtlasHeight);
			uvs[index] = vector;
			uvs[1 + index] = vector;
			uvs[2 + index] = vector;
			uvs[3 + index] = vector;
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			Vector2 vector2 = new Vector2(0f, 1f);
			uvs2[index] = vector2;
			uvs2[1 + index] = vector2;
			uvs2[2 + index] = vector2;
			uvs2[3 + index] = vector2;
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
			byte a;
			if (this.m_htmlColor.a < highlightColor.a)
			{
				a = this.m_htmlColor.a;
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
			if (this.m_text == null)
			{
				if (TMP_Settings.autoSizeTextContainer)
				{
					this.autoSizeTextContainer = true;
				}
				else
				{
					this.m_rectTransform = this.rectTransform;
					if (base.GetType() == typeof(TextMeshPro))
					{
						this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProTextContainerSize;
					}
					else
					{
						this.m_rectTransform.sizeDelta = TMP_Settings.defaultTextMeshProUITextContainerSize;
					}
				}
				this.m_enableWordWrapping = TMP_Settings.enableWordWrapping;
				this.m_enableKerning = TMP_Settings.enableKerning;
				this.m_enableExtraPadding = TMP_Settings.enableExtraPadding;
				this.m_tintAllSprites = TMP_Settings.enableTintAllSprites;
				this.m_parseCtrlCharacters = TMP_Settings.enableParseEscapeCharacters;
				this.m_fontSize = (this.m_fontSizeBase = TMP_Settings.defaultFontSize);
				this.m_fontSizeMin = this.m_fontSize * TMP_Settings.defaultTextAutoSizingMinRatio;
				this.m_fontSizeMax = this.m_fontSize * TMP_Settings.defaultTextAutoSizingMaxRatio;
				this.m_isAlignmentEnumConverted = true;
			}
			else if (!this.m_isAlignmentEnumConverted)
			{
				this.m_isAlignmentEnumConverted = true;
				this.m_textAlignment = TMP_Compatibility.ConvertTextAlignmentEnumValues(this.m_textAlignment);
			}
		}

		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			if (!fontAsset.characterDictionary.TryGetValue(0x5F, out this.m_cached_Underline_GlyphInfo))
			{
			}
			if (!fontAsset.characterDictionary.TryGetValue(0x2026, out this.m_cached_Ellipsis_GlyphInfo))
			{
			}
		}

		protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
		{
			chars[insertionIndex] = (int)c;
			for (int i = insertionIndex + tagLength; i < chars.Length; i++)
			{
				chars[i - 3] = chars[i];
			}
		}

		protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
		{
			bool flag;
			if ((this.m_style & FontStyles.Italic) != FontStyles.Italic)
			{
				flag = ((this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			int num = fontWeight / 0x64;
			TMP_FontAsset result;
			if (flag2)
			{
				result = this.m_currentFontAsset.fontWeights[num].italicTypeface;
			}
			else
			{
				result = this.m_currentFontAsset.fontWeights[num].regularTypeface;
			}
			return result;
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
			if (this.m_textInfo == null)
			{
				return string.Empty;
			}
			int characterCount = this.m_textInfo.characterCount;
			char[] array = new char[characterCount];
			int i = 0;
			while (i < characterCount)
			{
				if (i >= this.m_textInfo.characterInfo.Length)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_7D;
					}
				}
				else
				{
					array[i] = this.m_textInfo.characterInfo[i].character;
					i++;
				}
			}
			IL_7D:
			return new string(array);
		}

		protected Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 result;
			result.x = (float)((int)(x * 511f));
			result.y = (float)((int)(y * 511f));
			result.x = result.x * 4096f + result.y;
			result.y = scale;
			return result;
		}

		protected float PackUV(float x, float y)
		{
			double num = (double)((int)(x * 511f));
			double num2 = (double)((int)(y * 511f));
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
			default:
				switch (hex)
				{
				case 'a':
					return 0xA;
				case 'b':
					return 0xB;
				case 'c':
					return 0xC;
				case 'd':
					return 0xD;
				case 'e':
					return 0xE;
				case 'f':
					return 0xF;
				default:
					return 0xF;
				}
				break;
			case 'A':
				return 0xA;
			case 'B':
				return 0xB;
			case 'C':
				return 0xC;
			case 'D':
				return 0xD;
			case 'E':
				return 0xE;
			case 'F':
				return 0xF;
			}
		}

		protected int GetUTF16(int i)
		{
			int num = this.HexToInt(this.m_text[i]) << 0xC;
			num += this.HexToInt(this.m_text[i + 1]) << 8;
			num += this.HexToInt(this.m_text[i + 2]) << 4;
			return num + this.HexToInt(this.m_text[i + 3]);
		}

		protected int GetUTF32(int i)
		{
			int num = 0;
			num += this.HexToInt(this.m_text[i]) << 0x1E;
			num += this.HexToInt(this.m_text[i + 1]) << 0x18;
			num += this.HexToInt(this.m_text[i + 2]) << 0x14;
			num += this.HexToInt(this.m_text[i + 3]) << 0x10;
			num += this.HexToInt(this.m_text[i + 4]) << 0xC;
			num += this.HexToInt(this.m_text[i + 5]) << 8;
			num += this.HexToInt(this.m_text[i + 6]) << 4;
			return num + this.HexToInt(this.m_text[i + 7]);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			if (tagCount == 4)
			{
				byte r = (byte)(this.HexToInt(hexChars[1]) * 0x10 + this.HexToInt(hexChars[1]));
				byte g = (byte)(this.HexToInt(hexChars[2]) * 0x10 + this.HexToInt(hexChars[2]));
				byte b = (byte)(this.HexToInt(hexChars[3]) * 0x10 + this.HexToInt(hexChars[3]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (tagCount == 5)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[1]) * 0x10 + this.HexToInt(hexChars[1]));
				byte g2 = (byte)(this.HexToInt(hexChars[2]) * 0x10 + this.HexToInt(hexChars[2]));
				byte b2 = (byte)(this.HexToInt(hexChars[3]) * 0x10 + this.HexToInt(hexChars[3]));
				byte a = (byte)(this.HexToInt(hexChars[4]) * 0x10 + this.HexToInt(hexChars[4]));
				return new Color32(r2, g2, b2, a);
			}
			if (tagCount == 7)
			{
				byte r3 = (byte)(this.HexToInt(hexChars[1]) * 0x10 + this.HexToInt(hexChars[2]));
				byte g3 = (byte)(this.HexToInt(hexChars[3]) * 0x10 + this.HexToInt(hexChars[4]));
				byte b3 = (byte)(this.HexToInt(hexChars[5]) * 0x10 + this.HexToInt(hexChars[6]));
				return new Color32(r3, g3, b3, byte.MaxValue);
			}
			if (tagCount == 9)
			{
				byte r4 = (byte)(this.HexToInt(hexChars[1]) * 0x10 + this.HexToInt(hexChars[2]));
				byte g4 = (byte)(this.HexToInt(hexChars[3]) * 0x10 + this.HexToInt(hexChars[4]));
				byte b4 = (byte)(this.HexToInt(hexChars[5]) * 0x10 + this.HexToInt(hexChars[6]));
				byte a2 = (byte)(this.HexToInt(hexChars[7]) * 0x10 + this.HexToInt(hexChars[8]));
				return new Color32(r4, g4, b4, a2);
			}
			if (tagCount == 0xA)
			{
				byte r5 = (byte)(this.HexToInt(hexChars[7]) * 0x10 + this.HexToInt(hexChars[7]));
				byte g5 = (byte)(this.HexToInt(hexChars[8]) * 0x10 + this.HexToInt(hexChars[8]));
				byte b5 = (byte)(this.HexToInt(hexChars[9]) * 0x10 + this.HexToInt(hexChars[9]));
				return new Color32(r5, g5, b5, byte.MaxValue);
			}
			if (tagCount == 0xB)
			{
				byte r6 = (byte)(this.HexToInt(hexChars[7]) * 0x10 + this.HexToInt(hexChars[7]));
				byte g6 = (byte)(this.HexToInt(hexChars[8]) * 0x10 + this.HexToInt(hexChars[8]));
				byte b6 = (byte)(this.HexToInt(hexChars[9]) * 0x10 + this.HexToInt(hexChars[9]));
				byte a3 = (byte)(this.HexToInt(hexChars[0xA]) * 0x10 + this.HexToInt(hexChars[0xA]));
				return new Color32(r6, g6, b6, a3);
			}
			if (tagCount == 0xD)
			{
				byte r7 = (byte)(this.HexToInt(hexChars[7]) * 0x10 + this.HexToInt(hexChars[8]));
				byte g7 = (byte)(this.HexToInt(hexChars[9]) * 0x10 + this.HexToInt(hexChars[0xA]));
				byte b7 = (byte)(this.HexToInt(hexChars[0xB]) * 0x10 + this.HexToInt(hexChars[0xC]));
				return new Color32(r7, g7, b7, byte.MaxValue);
			}
			if (tagCount == 0xF)
			{
				byte r8 = (byte)(this.HexToInt(hexChars[7]) * 0x10 + this.HexToInt(hexChars[8]));
				byte g8 = (byte)(this.HexToInt(hexChars[9]) * 0x10 + this.HexToInt(hexChars[0xA]));
				byte b8 = (byte)(this.HexToInt(hexChars[0xB]) * 0x10 + this.HexToInt(hexChars[0xC]));
				byte a4 = (byte)(this.HexToInt(hexChars[0xD]) * 0x10 + this.HexToInt(hexChars[0xE]));
				return new Color32(r8, g8, b8, a4);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			if (length == 7)
			{
				byte r = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 0x10 + this.HexToInt(hexChars[startIndex + 2]));
				byte g = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 0x10 + this.HexToInt(hexChars[startIndex + 4]));
				byte b = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 0x10 + this.HexToInt(hexChars[startIndex + 6]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (length == 9)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 0x10 + this.HexToInt(hexChars[startIndex + 2]));
				byte g2 = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 0x10 + this.HexToInt(hexChars[startIndex + 4]));
				byte b2 = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 0x10 + this.HexToInt(hexChars[startIndex + 6]));
				byte a = (byte)(this.HexToInt(hexChars[startIndex + 7]) * 0x10 + this.HexToInt(hexChars[startIndex + 8]));
				return new Color32(r2, g2, b2, a);
			}
			return TMP_Text.s_colorWhite;
		}

		private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
		{
			int i = startIndex;
			int num = 0;
			while (i < startIndex + length)
			{
				parameters[num] = this.ConvertToFloat(chars, startIndex, length, out i);
				length -= i - startIndex + 1;
				startIndex = i + 1;
				num++;
			}
			return num;
		}

		protected float ConvertToFloat(char[] chars, int startIndex, int length)
		{
			int num = 0;
			return this.ConvertToFloat(chars, startIndex, length, out num);
		}

		protected unsafe float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
		{
			if (startIndex == 0)
			{
				lastIndex = 0;
				return -9999f;
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
				}
				else if (c == '.')
				{
					num4 = i;
					num3 = -1;
				}
				else if (c == '-')
				{
					num5 = -1;
				}
				else if (c == '+')
				{
					num5 = 1;
				}
				else
				{
					if (c == ',')
					{
						lastIndex = i;
						return num2 * (float)num5;
					}
					if (!char.IsDigit(c))
					{
						lastIndex = i;
						return -9999f;
					}
					switch (num3 + 5)
					{
					case 0:
						num2 += (float)(chars[i] - '0') * 1E-05f;
						break;
					case 1:
						num2 += (float)(chars[i] - '0') * 0.0001f;
						break;
					case 2:
						num2 += (float)(chars[i] - '0') * 0.001f;
						break;
					case 3:
						num2 += (float)(chars[i] - '0') * 0.01f;
						break;
					case 4:
						num2 += (float)(chars[i] - '0') * 0.1f;
						break;
					case 5:
						num2 = (float)(chars[i] - '0');
						break;
					case 6:
					case 7:
					case 8:
					case 9:
					case 0xA:
					case 0xB:
						num2 = num2 * 10f + (float)chars[i] - 48f;
						break;
					}
					if (num4 == 0)
					{
						num3++;
					}
					else
					{
						num3--;
					}
				}
			}
			lastIndex = num;
			return num2 * (float)num5;
		}

		protected unsafe bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			this.m_xmlAttribute[num2].nameHashCode = 0;
			this.m_xmlAttribute[num2].valueType = TagType.None;
			this.m_xmlAttribute[num2].valueHashCode = 0;
			this.m_xmlAttribute[num2].valueStartIndex = 0;
			this.m_xmlAttribute[num2].valueLength = 0;
			this.m_xmlAttribute[1].nameHashCode = 0;
			this.m_xmlAttribute[2].nameHashCode = 0;
			this.m_xmlAttribute[3].nameHashCode = 0;
			this.m_xmlAttribute[4].nameHashCode = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			for (int i = startIndex; i < chars.Length; i++)
			{
				if (chars[i] == 0)
				{
					break;
				}
				if (num >= this.m_htmlTag.Length || chars[i] == 0x3C)
				{
					break;
				}
				if (chars[i] == 0x3E)
				{
					flag2 = true;
					endIndex = i;
					this.m_htmlTag[num] = '\0';
					break;
				}
				this.m_htmlTag[num] = (char)chars[i];
				num++;
				if (b == 1)
				{
					if (tagType == TagType.None)
					{
						if (chars[i] == 0x2B)
						{
							goto IL_161;
						}
						if (chars[i] == 0x2D)
						{
							goto IL_161;
						}
						if (char.IsDigit((char)chars[i]))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								goto IL_161;
							}
						}
						else if (chars[i] == 0x23)
						{
							tagType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute = this.m_xmlAttribute;
							int num3 = num2;
							xmlAttribute[num3].valueLength = xmlAttribute[num3].valueLength + 1;
						}
						else if (chars[i] == 0x22)
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num;
						}
						else
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[i]);
							XML_TagAttribute[] xmlAttribute2 = this.m_xmlAttribute;
							int num4 = num2;
							xmlAttribute2[num4].valueLength = xmlAttribute2[num4].valueLength + 1;
						}
						goto IL_554;
						IL_161:
						tagType = TagType.NumericalValue;
						this.m_xmlAttribute[num2].valueType = TagType.NumericalValue;
						this.m_xmlAttribute[num2].valueStartIndex = num - 1;
						XML_TagAttribute[] xmlAttribute3 = this.m_xmlAttribute;
						int num5 = num2;
						xmlAttribute3[num5].valueLength = xmlAttribute3[num5].valueLength + 1;
					}
					else if (tagType == TagType.NumericalValue)
					{
						if (chars[i] == 0x70)
						{
							goto IL_31D;
						}
						if (chars[i] == 0x65)
						{
							goto IL_31D;
						}
						if (chars[i] == 0x25)
						{
							goto IL_31D;
						}
						if (chars[i] == 0x20)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								goto IL_31D;
							}
						}
						else if (b != 2)
						{
							XML_TagAttribute[] xmlAttribute4 = this.m_xmlAttribute;
							int num6 = num2;
							xmlAttribute4[num6].valueLength = xmlAttribute4[num6].valueLength + 1;
						}
						goto IL_554;
						IL_31D:
						b = 2;
						tagType = TagType.None;
						num2++;
						this.m_xmlAttribute[num2].nameHashCode = 0;
						this.m_xmlAttribute[num2].valueType = TagType.None;
						this.m_xmlAttribute[num2].valueHashCode = 0;
						this.m_xmlAttribute[num2].valueStartIndex = 0;
						this.m_xmlAttribute[num2].valueLength = 0;
						if (chars[i] == 0x65)
						{
							tagUnits = TagUnits.FontUnits;
						}
						else if (chars[i] == 0x25)
						{
							tagUnits = TagUnits.Percentage;
						}
					}
					else if (tagType == TagType.ColorValue)
					{
						if (chars[i] != 0x20)
						{
							XML_TagAttribute[] xmlAttribute5 = this.m_xmlAttribute;
							int num7 = num2;
							xmlAttribute5[num7].valueLength = xmlAttribute5[num7].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
						}
					}
					else if (tagType == TagType.StringValue)
					{
						if (chars[i] != 0x22)
						{
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[i]);
							XML_TagAttribute[] xmlAttribute6 = this.m_xmlAttribute;
							int num8 = num2;
							xmlAttribute6[num8].valueLength = xmlAttribute6[num8].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
						}
					}
				}
				IL_554:
				if (chars[i] == 0x3D)
				{
					b = 1;
				}
				if (b == 0)
				{
					if (chars[i] == 0x20)
					{
						if (flag)
						{
							return false;
						}
						flag = true;
						b = 2;
						tagType = TagType.None;
						num2++;
						this.m_xmlAttribute[num2].nameHashCode = 0;
						this.m_xmlAttribute[num2].valueType = TagType.None;
						this.m_xmlAttribute[num2].valueHashCode = 0;
						this.m_xmlAttribute[num2].valueStartIndex = 0;
						this.m_xmlAttribute[num2].valueLength = 0;
					}
				}
				if (b == 0)
				{
					this.m_xmlAttribute[num2].nameHashCode = (this.m_xmlAttribute[num2].nameHashCode << 3) - this.m_xmlAttribute[num2].nameHashCode + chars[i];
				}
				if (b == 2)
				{
					if (chars[i] == 0x20)
					{
						b = 0;
					}
				}
			}
			if (!flag2)
			{
				return false;
			}
			if (this.tag_NoParsing)
			{
				if (this.m_xmlAttribute[0].nameHashCode != 0x33542D3 && this.m_xmlAttribute[0].nameHashCode != 0x2F23DB3)
				{
					return false;
				}
			}
			if (this.m_xmlAttribute[0].nameHashCode != 0x33542D3)
			{
				if (this.m_xmlAttribute[0].nameHashCode == 0x2F23DB3)
				{
				}
				else
				{
					if (this.m_htmlTag[0] == '#')
					{
						if (num == 4)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
					}
					if (this.m_htmlTag[0] == '#' && num == 5)
					{
						this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
						this.m_colorStack.Add(this.m_htmlColor);
						return true;
					}
					if (this.m_htmlTag[0] == '#')
					{
						if (num == 7)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
					}
					if (this.m_htmlTag[0] == '#')
					{
						if (num == 9)
						{
							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
							this.m_colorStack.Add(this.m_htmlColor);
							return true;
						}
					}
					int nameHashCode = this.m_xmlAttribute[0].nameHashCode;
					switch (nameHashCode)
					{
					case 0x53:
						break;
					default:
						switch (nameHashCode)
						{
						case 0x73:
							break;
						default:
							switch (nameHashCode)
							{
							case 0x19C:
								break;
							default:
								if (nameHashCode != 0x1AA)
								{
									if (nameHashCode != 0x1AB)
									{
										switch (nameHashCode)
										{
										case 0x1BC:
											goto IL_156E;
										default:
										{
											float num12;
											if (nameHashCode != 0xF4AAC9)
											{
												if (nameHashCode != 0x147B2766)
												{
													if (nameHashCode != 0x16504B66)
													{
														if (nameHashCode != 0x1B40B577)
														{
															if (nameHashCode != 0x1EAF47A1)
															{
																if (nameHashCode != 0x20D7F9C8)
																{
																	if (nameHashCode != 0x21C6F46A)
																	{
																		if (nameHashCode == 0x2B8343C1)
																		{
																			goto IL_3A41;
																		}
																		if (nameHashCode == 0x2DABF5E8)
																		{
																			goto IL_3ACA;
																		}
																		if (nameHashCode != 0x2E9AF08A)
																		{
																			if (nameHashCode != 0x419BC966)
																			{
																				if (nameHashCode != 0x421F5578)
																				{
																					if (nameHashCode != 0x421FE49D)
																					{
																						if (nameHashCode != 0x71174431)
																						{
																							if (nameHashCode != 0x7117D356)
																							{
																								if (nameHashCode != 0x77EEF5BE)
																								{
																									if (nameHashCode != -0x70657989)
																									{
																										if (nameHashCode != -0x70449A56)
																										{
																											if (nameHashCode != -0x6E1BE82F)
																											{
																												if (nameHashCode != -0x6D2CED8D)
																												{
																													if (nameHashCode != -0x64BBE163)
																													{
																														if (nameHashCode == -0x63709E36)
																														{
																															goto IL_3A60;
																														}
																														if (nameHashCode == -0x6147EC0F)
																														{
																															goto IL_3AEB;
																														}
																														if (nameHashCode == -0x6058F16D)
																														{
																															goto IL_3AAA;
																														}
																														if (nameHashCode != -0x34BD4043)
																														{
																															if (nameHashCode == -0x32F64D9A)
																															{
																																goto IL_3C5D;
																															}
																															if (nameHashCode == -0x323A7B88)
																															{
																																goto IL_3EAF;
																															}
																															if (nameHashCode == -0x3239EC63)
																															{
																																goto IL_3288;
																															}
																															if (nameHashCode == -0x1A8EEACF)
																															{
																																goto IL_3F81;
																															}
																															if (nameHashCode == -0x1A8E5BAA)
																															{
																																goto IL_337E;
																															}
																															if (nameHashCode != -0x13B73942)
																															{
																																if (nameHashCode != 0x42)
																																{
																																	if (nameHashCode != 0x49)
																																	{
																																		if (nameHashCode == 0x62)
																																		{
																																			goto IL_13F7;
																																		}
																																		if (nameHashCode != 0x69)
																																		{
																																			if (nameHashCode != 0x18B)
																																			{
																																				if (nameHashCode != 0x192)
																																				{
																																					if (nameHashCode != 0x1B2)
																																					{
																																						if (nameHashCode != 0x290)
																																						{
																																							if (nameHashCode != 0x294)
																																							{
																																								if (nameHashCode != 0x29E)
																																								{
																																									if (nameHashCode == 0x390)
																																									{
																																										goto IL_41CC;
																																									}
																																									if (nameHashCode == 0x394)
																																									{
																																										return true;
																																									}
																																									if (nameHashCode != 0x39E)
																																									{
																																										if (nameHashCode != 0xB8F)
																																										{
																																											if (nameHashCode != 0xB93)
																																											{
																																												if (nameHashCode != 0xB9D)
																																												{
																																													if (nameHashCode == 0xC8F)
																																													{
																																														return true;
																																													}
																																													if (nameHashCode == 0xC93)
																																													{
																																														return true;
																																													}
																																													if (nameHashCode != 0xC9D)
																																													{
																																														if (nameHashCode != 0x11CC)
																																														{
																																															if (nameHashCode != 0x1278)
																																															{
																																																if (nameHashCode != 0x1286)
																																																{
																																																	if (nameHashCode == 0x18EC)
																																																	{
																																																		goto IL_1C44;
																																																	}
																																																	if (nameHashCode == 0x1998)
																																																	{
																																																		goto IL_177C;
																																																	}
																																																	if (nameHashCode != 0x19A6)
																																																	{
																																																		if (nameHashCode != 0x50C5)
																																																		{
																																																			if (nameHashCode != 0x5171)
																																																			{
																																																				if (nameHashCode != 0x517F)
																																																				{
																																																					if (nameHashCode == 0x57E5)
																																																					{
																																																						goto IL_1CF4;
																																																					}
																																																					if (nameHashCode == 0x5891)
																																																					{
																																																						goto IL_181C;
																																																					}
																																																					if (nameHashCode != 0x589F)
																																																					{
																																																						int valueHashCode;
																																																						if (nameHashCode != 0x6F5F)
																																																						{
																																																							if (nameHashCode != 0x7625)
																																																							{
																																																								if (nameHashCode != 0x763A)
																																																								{
																																																									if (nameHashCode != 0x79C1)
																																																									{
																																																										if (nameHashCode != 0x79D7)
																																																										{
																																																											if (nameHashCode != 0x7FE9)
																																																											{
																																																												if (nameHashCode == 0xA15F)
																																																												{
																																																													goto IL_2116;
																																																												}
																																																												if (nameHashCode == 0xA825)
																																																												{
																																																													goto IL_16B0;
																																																												}
																																																												if (nameHashCode == 0xA83A)
																																																												{
																																																													goto IL_28A9;
																																																												}
																																																												if (nameHashCode == 0xABC1)
																																																												{
																																																													goto IL_1DE1;
																																																												}
																																																												if (nameHashCode == 0xABD7)
																																																												{
																																																													goto IL_1D99;
																																																												}
																																																												if (nameHashCode != 0xB1E9)
																																																												{
																																																													if (nameHashCode != 0x2282E)
																																																													{
																																																														if (nameHashCode != 0x22EF4)
																																																														{
																																																															if (nameHashCode != 0x22F09)
																																																															{
																																																																if (nameHashCode != 0x23290)
																																																																{
																																																																	if (nameHashCode != 0x238B8)
																																																																	{
																																																																		if (nameHashCode == 0x25A2E)
																																																																		{
																																																																			goto IL_246B;
																																																																		}
																																																																		if (nameHashCode == 0x260F4)
																																																																		{
																																																																			goto IL_171E;
																																																																		}
																																																																		if (nameHashCode == 0x26109)
																																																																		{
																																																																			goto IL_29F7;
																																																																		}
																																																																		if (nameHashCode == 0x26490)
																																																																		{
																																																																			goto IL_1DEA;
																																																																		}
																																																																		if (nameHashCode != 0x26AB8)
																																																																		{
																																																																			if (nameHashCode != 0x2D7AD)
																																																																			{
																																																																				if (nameHashCode != 0x2D8FE)
																																																																				{
																																																																					if (nameHashCode != 0x2EF43)
																																																																					{
																																																																						if (nameHashCode != 0x379E6)
																																																																						{
																																																																							if (nameHashCode != 0x3842E)
																																																																							{
																																																																								if (nameHashCode != 0x3A15E)
																																																																								{
																																																																									if (nameHashCode == 0x435CD)
																																																																									{
																																																																										goto IL_2A77;
																																																																									}
																																																																									if (nameHashCode == 0x4371E)
																																																																									{
																																																																										goto IL_2855;
																																																																									}
																																																																									if (nameHashCode == 0x44760)
																																																																									{
																																																																										return false;
																																																																									}
																																																																									if (nameHashCode == 0x44D63)
																																																																									{
																																																																										goto IL_2C4F;
																																																																									}
																																																																									if (nameHashCode == 0x4D806)
																																																																									{
																																																																										goto IL_40E7;
																																																																									}
																																																																									if (nameHashCode == 0x4E24E)
																																																																									{
																																																																										goto IL_278D;
																																																																									}
																																																																									if (nameHashCode != 0x4FF7E)
																																																																									{
																																																																										if (nameHashCode != 0xEE556)
																																																																										{
																																																																											if (nameHashCode != 0xEFCEC)
																																																																											{
																																																																												if (nameHashCode != 0xF878F)
																																																																												{
																																																																													if (nameHashCode != 0xFAF07)
																																																																													{
																																																																														if (nameHashCode == 0x104376)
																																																																														{
																																																																															goto IL_2B85;
																																																																														}
																																																																														if (nameHashCode == 0x105B0C)
																																																																														{
																																																																															goto IL_3168;
																																																																														}
																																																																														if (nameHashCode == 0x10E5AF)
																																																																														{
																																																																															return true;
																																																																														}
																																																																														if (nameHashCode != 0x110D27)
																																																																														{
																																																																															if (nameHashCode != 0x13A0C6)
																																																																															{
																																																																																if (nameHashCode != 0x14B2E3)
																																																																																{
																																																																																	if (nameHashCode != 0x15FEF4)
																																																																																	{
																																																																																		if (nameHashCode != 0x169E9E)
																																																																																		{
																																																																																			if (nameHashCode != 0x174369)
																																																																																			{
																																																																																				if (nameHashCode != 0x186BFB)
																																																																																				{
																																																																																					if (nameHashCode != 0x18B5DD)
																																																																																					{
																																																																																						if (nameHashCode == 0x1AB5BA)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						if (nameHashCode == 0x1D33C6)
																																																																																						{
																																																																																							goto IL_3F97;
																																																																																						}
																																																																																						if (nameHashCode == 0x1E45E3)
																																																																																						{
																																																																																							goto IL_2F5D;
																																																																																						}
																																																																																						if (nameHashCode == 0x1F91F4)
																																																																																						{
																																																																																							goto IL_317D;
																																																																																						}
																																																																																						if (nameHashCode == 0x20319E)
																																																																																						{
																																																																																							goto IL_3B17;
																																																																																						}
																																																																																						if (nameHashCode == 0x20D669)
																																																																																						{
																																																																																							goto IL_3095;
																																																																																						}
																																																																																						if (nameHashCode == 0x21FEFB)
																																																																																						{
																																																																																							goto IL_4062;
																																																																																						}
																																																																																						if (nameHashCode != 0x2248DD)
																																																																																						{
																																																																																							if (nameHashCode != 0x680065)
																																																																																							{
																																																																																								if (nameHashCode != 0x691282)
																																																																																								{
																																																																																									if (nameHashCode != 0x6A5E93)
																																																																																									{
																																																																																										if (nameHashCode != 0x6AFE3D)
																																																																																										{
																																																																																											if (nameHashCode != 0x6BA308)
																																																																																											{
																																																																																												if (nameHashCode != 0x6CCB9A)
																																																																																												{
																																																																																													if (nameHashCode == 0x719365)
																																																																																													{
																																																																																														goto IL_3FFB;
																																																																																													}
																																																																																													if (nameHashCode == 0x72A582)
																																																																																													{
																																																																																														goto IL_302B;
																																																																																													}
																																																																																													if (nameHashCode == 0x73F193)
																																																																																													{
																																																																																														goto IL_3273;
																																																																																													}
																																																																																													if (nameHashCode == 0x74913D)
																																																																																													{
																																																																																														goto IL_3C45;
																																																																																													}
																																																																																													if (nameHashCode == 0x753608)
																																																																																													{
																																																																																														goto IL_3159;
																																																																																													}
																																																																																													if (nameHashCode != 0x765E9A)
																																																																																													{
																																																																																														if (nameHashCode != 0x8B5EEA)
																																																																																														{
																																																																																															if (nameHashCode != 0xA3A05A)
																																																																																															{
																																																																																																if (nameHashCode == 0xB1A5A9)
																																																																																																{
																																																																																																	goto IL_1CFD;
																																																																																																}
																																																																																																if (nameHashCode == 0xCE640A)
																																																																																																{
																																																																																																	goto IL_3A89;
																																																																																																}
																																																																																																if (nameHashCode != 0xE6A57A)
																																																																																																{
																																																																																																	if (nameHashCode != 0x2D9FC43)
																																																																																																	{
																																																																																																		if (nameHashCode != 0x3004302)
																																																																																																		{
																																																																																																			if (nameHashCode == 0x31D0163)
																																																																																																			{
																																																																																																				goto IL_3AAA;
																																																																																																			}
																																																																																																			if (nameHashCode != 0x3434822)
																																																																																																			{
																																																																																																				if (nameHashCode != 0x454D9F7 && nameHashCode != 0x629FDF7)
																																																																																																				{
																																																																																																					return false;
																																																																																																				}
																																																																																																				valueHashCode = this.m_xmlAttribute[0].valueHashCode;
																																																																																																				if (valueHashCode != 0x2D93756B)
																																																																																																				{
																																																																																																					if (valueHashCode != 0x1F31F54B)
																																																																																																					{
																																																																																																						Material material;
																																																																																																						if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
																																																																																																						{
																																																																																																							if (this.m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																																																							{
																																																																																																								return false;
																																																																																																							}
																																																																																																							this.m_currentMaterial = material;
																																																																																																							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																																																							this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																																																																						}
																																																																																																						else
																																																																																																						{
																																																																																																							material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																																																																																																							if (material == null)
																																																																																																							{
																																																																																																								return false;
																																																																																																							}
																																																																																																							if (this.m_currentFontAsset.atlas.GetInstanceID() != material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																																																							{
																																																																																																								return false;
																																																																																																							}
																																																																																																							MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
																																																																																																							this.m_currentMaterial = material;
																																																																																																							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																																																							this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																																																																						}
																																																																																																						return true;
																																																																																																					}
																																																																																																				}
																																																																																																				if (this.m_currentFontAsset.atlas.GetInstanceID() != this.m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
																																																																																																				{
																																																																																																					return false;
																																																																																																				}
																																																																																																				this.m_currentMaterial = this.m_materialReferences[0].material;
																																																																																																				this.m_currentMaterialIndex = 0;
																																																																																																				this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																																																																																																				return true;
																																																																																																			}
																																																																																																		}
																																																																																																		this.m_baselineOffset = 0f;
																																																																																																		return true;
																																																																																																	}
																																																																																																	goto IL_3AAA;
																																																																																																}
																																																																																															}
																																																																																															this.tag_NoParsing = true;
																																																																																															return true;
																																																																																														}
																																																																																														goto IL_3A89;
																																																																																													}
																																																																																												}
																																																																																												this.m_isFXMatrixSet = false;
																																																																																												return true;
																																																																																											}
																																																																																											IL_3159:
																																																																																											this.m_monoSpacing = 0f;
																																																																																											return true;
																																																																																										}
																																																																																										IL_3C45:
																																																																																										this.m_marginLeft = 0f;
																																																																																										this.m_marginRight = 0f;
																																																																																										return true;
																																																																																									}
																																																																																									IL_3273:
																																																																																									this.tag_Indent = this.m_indentStack.Remove();
																																																																																									return true;
																																																																																								}
																																																																																								IL_302B:
																																																																																								if (!this.m_isParsingText)
																																																																																								{
																																																																																									return true;
																																																																																								}
																																																																																								if (this.m_characterCount > 0)
																																																																																								{
																																																																																									this.m_xAdvance -= this.m_cSpacing;
																																																																																									this.m_textInfo.characterInfo[this.m_characterCount - 1].xAdvance = this.m_xAdvance;
																																																																																								}
																																																																																								this.m_cSpacing = 0f;
																																																																																								return true;
																																																																																							}
																																																																																							IL_3FFB:
																																																																																							if (this.m_isParsingText)
																																																																																							{
																																																																																								Debug.Log(string.Concat(new object[]
																																																																																								{
																																																																																									"Action ID: [",
																																																																																									this.m_actionStack.CurrentItem(),
																																																																																									"] Last character index: ",
																																																																																									this.m_characterCount - 1
																																																																																								}));
																																																																																							}
																																																																																							this.m_actionStack.Remove();
																																																																																							return true;
																																																																																						}
																																																																																					}
																																																																																					int valueHashCode2 = this.m_xmlAttribute[0].valueHashCode;
																																																																																					this.m_spriteIndex = -1;
																																																																																					if (this.m_xmlAttribute[0].valueType != TagType.None)
																																																																																					{
																																																																																						if (this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																																																																																						{
																																																																																						}
																																																																																						else
																																																																																						{
																																																																																							TMP_SpriteAsset tmp_SpriteAsset;
																																																																																							if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode2, out tmp_SpriteAsset))
																																																																																							{
																																																																																								this.m_currentSpriteAsset = tmp_SpriteAsset;
																																																																																								goto IL_353F;
																																																																																							}
																																																																																							if (tmp_SpriteAsset == null)
																																																																																							{
																																																																																								tmp_SpriteAsset = Resources.Load<TMP_SpriteAsset>(TMP_Settings.defaultSpriteAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																																																																																							}
																																																																																							if (tmp_SpriteAsset == null)
																																																																																							{
																																																																																								return false;
																																																																																							}
																																																																																							MaterialReferenceManager.AddSpriteAsset(valueHashCode2, tmp_SpriteAsset);
																																																																																							this.m_currentSpriteAsset = tmp_SpriteAsset;
																																																																																							goto IL_353F;
																																																																																						}
																																																																																					}
																																																																																					if (this.m_spriteAsset != null)
																																																																																					{
																																																																																						this.m_currentSpriteAsset = this.m_spriteAsset;
																																																																																					}
																																																																																					else if (this.m_defaultSpriteAsset != null)
																																																																																					{
																																																																																						this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
																																																																																					}
																																																																																					else if (this.m_defaultSpriteAsset == null)
																																																																																					{
																																																																																						if (TMP_Settings.defaultSpriteAsset != null)
																																																																																						{
																																																																																							this.m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
																																																																																						}
																																																																																						else
																																																																																						{
																																																																																							this.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
																																																																																						}
																																																																																						this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
																																																																																					}
																																																																																					if (this.m_currentSpriteAsset == null)
																																																																																					{
																																																																																						return false;
																																																																																					}
																																																																																					IL_353F:
																																																																																					if (this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																																																																																					{
																																																																																						int num9 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																						if (num9 == -0x270F)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						if (num9 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						this.m_spriteIndex = num9;
																																																																																					}
																																																																																					else if (this.m_xmlAttribute[1].nameHashCode == 0xA953 || this.m_xmlAttribute[1].nameHashCode == 0x7753)
																																																																																					{
																																																																																						int spriteIndexFromHashcode = this.m_currentSpriteAsset.GetSpriteIndexFromHashcode(this.m_xmlAttribute[1].valueHashCode);
																																																																																						if (spriteIndexFromHashcode == -1)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						this.m_spriteIndex = spriteIndexFromHashcode;
																																																																																					}
																																																																																					else
																																																																																					{
																																																																																						if (this.m_xmlAttribute[1].nameHashCode != 0x4828A && this.m_xmlAttribute[1].nameHashCode != 0x3246A)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						int num10 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																																																																																						if (num10 == -0x270F)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						if (num10 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																																																																																						{
																																																																																							return false;
																																																																																						}
																																																																																						this.m_spriteIndex = num10;
																																																																																					}
																																																																																					this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																																					this.m_spriteColor = TMP_Text.s_colorWhite;
																																																																																					this.m_tintSprite = false;
																																																																																					int j = 0;
																																																																																					while (j < this.m_xmlAttribute.Length)
																																																																																					{
																																																																																						if (this.m_xmlAttribute[j].nameHashCode == 0)
																																																																																						{
																																																																																							for (;;)
																																																																																							{
																																																																																								switch (4)
																																																																																								{
																																																																																								case 0:
																																																																																									continue;
																																																																																								}
																																																																																								goto IL_3A03;
																																																																																							}
																																																																																						}
																																																																																						else
																																																																																						{
																																																																																							int nameHashCode2 = this.m_xmlAttribute[j].nameHashCode;
																																																																																							if (nameHashCode2 == 0x6851)
																																																																																							{
																																																																																								goto IL_3907;
																																																																																							}
																																																																																							int num11;
																																																																																							if (nameHashCode2 != 0x7753)
																																																																																							{
																																																																																								if (nameHashCode2 != 0x80FB)
																																																																																								{
																																																																																									if (nameHashCode2 == 0x9A51)
																																																																																									{
																																																																																										goto IL_3907;
																																																																																									}
																																																																																									if (nameHashCode2 == 0xA953)
																																																																																									{
																																																																																										goto IL_37E1;
																																																																																									}
																																																																																									if (nameHashCode2 != 0xB2FB)
																																																																																									{
																																																																																										if (nameHashCode2 != 0x2EF43)
																																																																																										{
																																																																																											if (nameHashCode2 != 0x3246A)
																																																																																											{
																																																																																												if (nameHashCode2 == 0x44D63)
																																																																																												{
																																																																																													goto IL_38CA;
																																																																																												}
																																																																																												if (nameHashCode2 != 0x4828A)
																																																																																												{
																																																																																													if (nameHashCode2 != 0x2248DD)
																																																																																													{
																																																																																														if (nameHashCode2 != 0x18B5DD)
																																																																																														{
																																																																																															return false;
																																																																																														}
																																																																																													}
																																																																																													goto IL_39C6;
																																																																																												}
																																																																																											}
																																																																																											num11 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																																																																																											if (num11 == -0x270F)
																																																																																											{
																																																																																												return false;
																																																																																											}
																																																																																											if (num11 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																																																																																											{
																																																																																												return false;
																																																																																											}
																																																																																											this.m_spriteIndex = num11;
																																																																																											goto IL_39C6;
																																																																																										}
																																																																																										IL_38CA:
																																																																																										this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[j].valueStartIndex, this.m_xmlAttribute[j].valueLength);
																																																																																										goto IL_39C6;
																																																																																									}
																																																																																								}
																																																																																								this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[j].valueStartIndex, this.m_xmlAttribute[j].valueLength) != 0f);
																																																																																								goto IL_39C6;
																																																																																							}
																																																																																							IL_37E1:
																																																																																							num11 = this.m_currentSpriteAsset.GetSpriteIndexFromHashcode(this.m_xmlAttribute[j].valueHashCode);
																																																																																							if (num11 == -1)
																																																																																							{
																																																																																								return false;
																																																																																							}
																																																																																							this.m_spriteIndex = num11;
																																																																																							IL_39C6:
																																																																																							j++;
																																																																																							continue;
																																																																																							IL_3907:
																																																																																							int attributeParameters = this.GetAttributeParameters(this.m_htmlTag, this.m_xmlAttribute[j].valueStartIndex, this.m_xmlAttribute[j].valueLength, ref this.m_attributeParameterValues);
																																																																																							if (attributeParameters != 3)
																																																																																							{
																																																																																								return false;
																																																																																							}
																																																																																							this.m_spriteIndex = (int)this.m_attributeParameterValues[0];
																																																																																							if (this.m_isParsingText)
																																																																																							{
																																																																																								this.spriteAnimator.DoSpriteAnimation(this.m_characterCount, this.m_currentSpriteAsset, this.m_spriteIndex, (int)this.m_attributeParameterValues[1], (int)this.m_attributeParameterValues[2]);
																																																																																							}
																																																																																							goto IL_39C6;
																																																																																						}
																																																																																					}
																																																																																					IL_3A03:
																																																																																					if (this.m_spriteIndex == -1)
																																																																																					{
																																																																																						return false;
																																																																																					}
																																																																																					this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																																																					this.m_textElementType = TMP_TextElementType.Sprite;
																																																																																					return true;
																																																																																				}
																																																																																				IL_4062:
																																																																																				num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																				if (num12 == -9999f)
																																																																																				{
																																																																																					return false;
																																																																																				}
																																																																																				this.m_FXMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num12), Vector3.one);
																																																																																				this.m_isFXMatrixSet = true;
																																																																																				return true;
																																																																																			}
																																																																																			IL_3095:
																																																																																			num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																			if (num12 != -9999f)
																																																																																			{
																																																																																				if (num12 != 0f)
																																																																																				{
																																																																																					if (tagUnits != TagUnits.Pixels)
																																																																																					{
																																																																																						if (tagUnits != TagUnits.FontUnits)
																																																																																						{
																																																																																							if (tagUnits == TagUnits.Percentage)
																																																																																							{
																																																																																								return false;
																																																																																							}
																																																																																						}
																																																																																						else
																																																																																						{
																																																																																							this.m_monoSpacing = num12;
																																																																																							this.m_monoSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																																						}
																																																																																					}
																																																																																					else
																																																																																					{
																																																																																						this.m_monoSpacing = num12;
																																																																																					}
																																																																																					return true;
																																																																																				}
																																																																																			}
																																																																																			return false;
																																																																																		}
																																																																																		IL_3B17:
																																																																																		num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																		if (num12 != -9999f)
																																																																																		{
																																																																																			if (num12 != 0f)
																																																																																			{
																																																																																				this.m_marginLeft = num12;
																																																																																				if (tagUnits != TagUnits.Pixels)
																																																																																				{
																																																																																					if (tagUnits != TagUnits.FontUnits)
																																																																																					{
																																																																																						if (tagUnits == TagUnits.Percentage)
																																																																																						{
																																																																																							float marginWidth = this.m_marginWidth;
																																																																																							float num13;
																																																																																							if (this.m_width != -1f)
																																																																																							{
																																																																																								num13 = this.m_width;
																																																																																							}
																																																																																							else
																																																																																							{
																																																																																								num13 = 0f;
																																																																																							}
																																																																																							this.m_marginLeft = (marginWidth - num13) * this.m_marginLeft / 100f;
																																																																																						}
																																																																																					}
																																																																																					else
																																																																																					{
																																																																																						this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																																					}
																																																																																				}
																																																																																				this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
																																																																																				this.m_marginRight = this.m_marginLeft;
																																																																																				return true;
																																																																																			}
																																																																																		}
																																																																																		return false;
																																																																																	}
																																																																																	IL_317D:
																																																																																	num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																	if (num12 != -9999f)
																																																																																	{
																																																																																		if (num12 != 0f)
																																																																																		{
																																																																																			if (tagUnits != TagUnits.Pixels)
																																																																																			{
																																																																																				if (tagUnits != TagUnits.FontUnits)
																																																																																				{
																																																																																					if (tagUnits != TagUnits.Percentage)
																																																																																					{
																																																																																					}
																																																																																					else
																																																																																					{
																																																																																						this.tag_Indent = this.m_marginWidth * num12 / 100f;
																																																																																					}
																																																																																				}
																																																																																				else
																																																																																				{
																																																																																					this.tag_Indent = num12;
																																																																																					this.tag_Indent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																																				}
																																																																																			}
																																																																																			else
																																																																																			{
																																																																																				this.tag_Indent = num12;
																																																																																			}
																																																																																			this.m_indentStack.Add(this.tag_Indent);
																																																																																			this.m_xAdvance = this.tag_Indent;
																																																																																			return true;
																																																																																		}
																																																																																	}
																																																																																	return false;
																																																																																}
																																																																																IL_2F5D:
																																																																																num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																																if (num12 != -9999f)
																																																																																{
																																																																																	if (num12 != 0f)
																																																																																	{
																																																																																		if (tagUnits != TagUnits.Pixels)
																																																																																		{
																																																																																			if (tagUnits != TagUnits.FontUnits)
																																																																																			{
																																																																																				if (tagUnits == TagUnits.Percentage)
																																																																																				{
																																																																																					return false;
																																																																																				}
																																																																																			}
																																																																																			else
																																																																																			{
																																																																																				this.m_cSpacing = num12;
																																																																																				this.m_cSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																																			}
																																																																																		}
																																																																																		else
																																																																																		{
																																																																																			this.m_cSpacing = num12;
																																																																																		}
																																																																																		return true;
																																																																																	}
																																																																																}
																																																																																return false;
																																																																															}
																																																																															IL_3F97:
																																																																															int valueHashCode3 = this.m_xmlAttribute[0].valueHashCode;
																																																																															if (this.m_isParsingText)
																																																																															{
																																																																																this.m_actionStack.Add(valueHashCode3);
																																																																																Debug.Log(string.Concat(new object[]
																																																																																{
																																																																																	"Action ID: [",
																																																																																	valueHashCode3,
																																																																																	"] First character index: ",
																																																																																	this.m_characterCount
																																																																																}));
																																																																															}
																																																																															return true;
																																																																														}
																																																																													}
																																																																													this.m_width = -1f;
																																																																													return true;
																																																																												}
																																																																												return true;
																																																																											}
																																																																											IL_3168:
																																																																											this.m_htmlColor = this.m_colorStack.Remove();
																																																																											return true;
																																																																										}
																																																																										IL_2B85:
																																																																										this.m_lineJustification = this.m_lineJustificationStack.Remove();
																																																																										return true;
																																																																									}
																																																																								}
																																																																								num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																								if (num12 != -9999f)
																																																																								{
																																																																									if (num12 != 0f)
																																																																									{
																																																																										if (tagUnits != TagUnits.Pixels)
																																																																										{
																																																																											if (tagUnits == TagUnits.FontUnits)
																																																																											{
																																																																												return false;
																																																																											}
																																																																											if (tagUnits != TagUnits.Percentage)
																																																																											{
																																																																											}
																																																																											else
																																																																											{
																																																																												this.m_width = this.m_marginWidth * num12 / 100f;
																																																																											}
																																																																										}
																																																																										else
																																																																										{
																																																																											this.m_width = num12;
																																																																										}
																																																																										return true;
																																																																									}
																																																																								}
																																																																								return false;
																																																																							}
																																																																							IL_278D:
																																																																							num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																																							if (num12 != -9999f)
																																																																							{
																																																																								if (num12 != 0f)
																																																																								{
																																																																									if (tagUnits == TagUnits.Pixels)
																																																																									{
																																																																										this.m_xAdvance += num12;
																																																																										return true;
																																																																									}
																																																																									if (tagUnits == TagUnits.FontUnits)
																																																																									{
																																																																										this.m_xAdvance += num12 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																																																										return true;
																																																																									}
																																																																									if (tagUnits != TagUnits.Percentage)
																																																																									{
																																																																										return false;
																																																																									}
																																																																									return false;
																																																																								}
																																																																							}
																																																																							return false;
																																																																						}
																																																																						IL_40E7:
																																																																						int nameHashCode3 = this.m_xmlAttribute[1].nameHashCode;
																																																																						if (nameHashCode3 != 0x4FF7E)
																																																																						{
																																																																						}
																																																																						else
																																																																						{
																																																																							float num14 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																																																																							if (tagUnits != TagUnits.Pixels)
																																																																							{
																																																																								if (tagUnits != TagUnits.FontUnits)
																																																																								{
																																																																									if (tagUnits != TagUnits.Percentage)
																																																																									{
																																																																									}
																																																																									else
																																																																									{
																																																																										Debug.Log("Table width = " + num14 + "%.");
																																																																									}
																																																																								}
																																																																								else
																																																																								{
																																																																									Debug.Log("Table width = " + num14 + "em.");
																																																																								}
																																																																							}
																																																																							else
																																																																							{
																																																																								Debug.Log("Table width = " + num14 + "px.");
																																																																							}
																																																																						}
																																																																						return true;
																																																																					}
																																																																					IL_2C4F:
																																																																					if (this.m_htmlTag[6] == '#')
																																																																					{
																																																																						if (num == 0xA)
																																																																						{
																																																																							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																																																																							this.m_colorStack.Add(this.m_htmlColor);
																																																																							return true;
																																																																						}
																																																																					}
																																																																					if (this.m_htmlTag[6] == '#')
																																																																					{
																																																																						if (num == 0xB)
																																																																						{
																																																																							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																																																																							this.m_colorStack.Add(this.m_htmlColor);
																																																																							return true;
																																																																						}
																																																																					}
																																																																					if (this.m_htmlTag[6] == '#' && num == 0xD)
																																																																					{
																																																																						this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (this.m_htmlTag[6] == '#')
																																																																					{
																																																																						if (num == 0xF)
																																																																						{
																																																																							this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																																																																							this.m_colorStack.Add(this.m_htmlColor);
																																																																							return true;
																																																																						}
																																																																					}
																																																																					int valueHashCode4 = this.m_xmlAttribute[0].valueHashCode;
																																																																					if (valueHashCode4 == -0x232C3B2)
																																																																					{
																																																																						this.m_htmlColor = new Color32(0xA0, 0x20, 0xF0, byte.MaxValue);
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x1E9D3)
																																																																					{
																																																																						this.m_htmlColor = Color.red;
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x36863E)
																																																																					{
																																																																						this.m_htmlColor = Color.blue;
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x19536F0)
																																																																					{
																																																																						this.m_htmlColor = new Color32(byte.MaxValue, 0x80, 0, byte.MaxValue);
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x7071A47)
																																																																					{
																																																																						this.m_htmlColor = Color.black;
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x73D641B)
																																																																					{
																																																																						this.m_htmlColor = Color.green;
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 == 0x85DAEE7)
																																																																					{
																																																																						this.m_htmlColor = Color.white;
																																																																						this.m_colorStack.Add(this.m_htmlColor);
																																																																						return true;
																																																																					}
																																																																					if (valueHashCode4 != 0x21063284)
																																																																					{
																																																																						return false;
																																																																					}
																																																																					this.m_htmlColor = Color.yellow;
																																																																					this.m_colorStack.Add(this.m_htmlColor);
																																																																					return true;
																																																																				}
																																																																				IL_2855:
																																																																				if (this.m_xmlAttribute[0].valueLength != 3)
																																																																				{
																																																																					return false;
																																																																				}
																																																																				this.m_htmlColor.a = (byte)(this.HexToInt(this.m_htmlTag[7]) * 0x10 + this.HexToInt(this.m_htmlTag[8]));
																																																																				return true;
																																																																			}
																																																																			IL_2A77:
																																																																			int valueHashCode5 = this.m_xmlAttribute[0].valueHashCode;
																																																																			if (valueHashCode5 == -0x1F38AE01)
																																																																			{
																																																																				this.m_lineJustification = TextAlignmentOptions.Justified;
																																																																				this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																																				return true;
																																																																			}
																																																																			if (valueHashCode5 == -0x1B4FBB35)
																																																																			{
																																																																				this.m_lineJustification = TextAlignmentOptions.Center;
																																																																				this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																																				return true;
																																																																			}
																																																																			if (valueHashCode5 == 0x3998DB)
																																																																			{
																																																																				this.m_lineJustification = TextAlignmentOptions.Left;
																																																																				this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																																				return true;
																																																																			}
																																																																			if (valueHashCode5 == 0x74B6C44)
																																																																			{
																																																																				this.m_lineJustification = TextAlignmentOptions.Flush;
																																																																				this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																																				return true;
																																																																			}
																																																																			if (valueHashCode5 != 0x825EC40)
																																																																			{
																																																																				return false;
																																																																			}
																																																																			this.m_lineJustification = TextAlignmentOptions.Right;
																																																																			this.m_lineJustificationStack.Add(this.m_lineJustification);
																																																																			return true;
																																																																		}
																																																																	}
																																																																	this.m_currentFontSize = this.m_sizeStack.Remove();
																																																																	float num15 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																																	float num16;
																																																																	if (this.m_isOrthographic)
																																																																	{
																																																																		num16 = 1f;
																																																																	}
																																																																	else
																																																																	{
																																																																		num16 = 0.1f;
																																																																	}
																																																																	this.m_fontScale = num15 * num16;
																																																																	return true;
																																																																}
																																																																IL_1DEA:
																																																																this.m_isNonBreakingSpace = false;
																																																																return true;
																																																															}
																																																															IL_29F7:
																																																															if (this.m_isParsingText)
																																																															{
																																																																if (!this.m_isCalculatingPreferredValues)
																																																																{
																																																																	this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextLength = this.m_characterCount - this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextfirstCharacterIndex;
																																																																	this.m_textInfo.linkCount++;
																																																																}
																																																															}
																																																															return true;
																																																														}
																																																														IL_171E:
																																																														if ((this.m_fontStyle & FontStyles.Highlight) != FontStyles.Highlight)
																																																														{
																																																															this.m_highlightColor = this.m_highlightColorStack.Remove();
																																																															if (this.m_fontStyleStack.Remove(FontStyles.Highlight) == 0)
																																																															{
																																																																this.m_style &= (FontStyles)(-0x201);
																																																															}
																																																														}
																																																														return true;
																																																													}
																																																													IL_246B:
																																																													MaterialReference materialReference = this.m_materialReferenceStack.Remove();
																																																													this.m_currentFontAsset = materialReference.fontAsset;
																																																													this.m_currentMaterial = materialReference.material;
																																																													this.m_currentMaterialIndex = materialReference.index;
																																																													float num17 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																													float num18;
																																																													if (this.m_isOrthographic)
																																																													{
																																																														num18 = 1f;
																																																													}
																																																													else
																																																													{
																																																														num18 = 0.1f;
																																																													}
																																																													this.m_fontScale = num17 * num18;
																																																													return true;
																																																												}
																																																											}
																																																											num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																											if (num12 == -9999f)
																																																											{
																																																												return false;
																																																											}
																																																											if (tagUnits != TagUnits.Pixels)
																																																											{
																																																												if (tagUnits == TagUnits.FontUnits)
																																																												{
																																																													this.m_currentFontSize = this.m_fontSize * num12;
																																																													this.m_sizeStack.Add(this.m_currentFontSize);
																																																													float num19 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																													float num20;
																																																													if (this.m_isOrthographic)
																																																													{
																																																														num20 = 1f;
																																																													}
																																																													else
																																																													{
																																																														num20 = 0.1f;
																																																													}
																																																													this.m_fontScale = num19 * num20;
																																																													return true;
																																																												}
																																																												if (tagUnits != TagUnits.Percentage)
																																																												{
																																																													return false;
																																																												}
																																																												this.m_currentFontSize = this.m_fontSize * num12 / 100f;
																																																												this.m_sizeStack.Add(this.m_currentFontSize);
																																																												this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																												return true;
																																																											}
																																																											else
																																																											{
																																																												if (this.m_htmlTag[5] == '+')
																																																												{
																																																													this.m_currentFontSize = this.m_fontSize + num12;
																																																													this.m_sizeStack.Add(this.m_currentFontSize);
																																																													float num21 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																													float num22;
																																																													if (this.m_isOrthographic)
																																																													{
																																																														num22 = 1f;
																																																													}
																																																													else
																																																													{
																																																														num22 = 0.1f;
																																																													}
																																																													this.m_fontScale = num21 * num22;
																																																													return true;
																																																												}
																																																												if (this.m_htmlTag[5] == '-')
																																																												{
																																																													this.m_currentFontSize = this.m_fontSize + num12;
																																																													this.m_sizeStack.Add(this.m_currentFontSize);
																																																													float num23 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																													float num24;
																																																													if (this.m_isOrthographic)
																																																													{
																																																														num24 = 1f;
																																																													}
																																																													else
																																																													{
																																																														num24 = 0.1f;
																																																													}
																																																													this.m_fontScale = num23 * num24;
																																																													return true;
																																																												}
																																																												this.m_currentFontSize = num12;
																																																												this.m_sizeStack.Add(this.m_currentFontSize);
																																																												this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																												return true;
																																																											}
																																																										}
																																																										IL_1D99:
																																																										if (this.m_overflowMode == TextOverflowModes.Page)
																																																										{
																																																											this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
																																																											this.m_lineOffset = 0f;
																																																											this.m_pageNumber++;
																																																											this.m_isNewPage = true;
																																																										}
																																																										return true;
																																																									}
																																																									IL_1DE1:
																																																									this.m_isNonBreakingSpace = true;
																																																									return true;
																																																								}
																																																								IL_28A9:
																																																								if (this.m_isParsingText && !this.m_isCalculatingPreferredValues)
																																																								{
																																																									int linkCount = this.m_textInfo.linkCount;
																																																									if (linkCount + 1 > this.m_textInfo.linkInfo.Length)
																																																									{
																																																										TMP_TextInfo.Resize<TMP_LinkInfo>(ref this.m_textInfo.linkInfo, linkCount + 1);
																																																									}
																																																									this.m_textInfo.linkInfo[linkCount].textComponent = this;
																																																									this.m_textInfo.linkInfo[linkCount].hashCode = this.m_xmlAttribute[0].valueHashCode;
																																																									this.m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = this.m_characterCount;
																																																									this.m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + this.m_xmlAttribute[0].valueStartIndex;
																																																									this.m_textInfo.linkInfo[linkCount].linkIdLength = this.m_xmlAttribute[0].valueLength;
																																																									this.m_textInfo.linkInfo[linkCount].SetLinkID(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																								}
																																																								return true;
																																																							}
																																																							IL_16B0:
																																																							this.m_style |= FontStyles.Highlight;
																																																							this.m_fontStyleStack.Add(FontStyles.Highlight);
																																																							this.m_highlightColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																																							this.m_highlightColorStack.Add(this.m_highlightColor);
																																																							return true;
																																																						}
																																																						IL_2116:
																																																						int valueHashCode6 = this.m_xmlAttribute[0].valueHashCode;
																																																						int nameHashCode4 = this.m_xmlAttribute[1].nameHashCode;
																																																						valueHashCode = this.m_xmlAttribute[1].valueHashCode;
																																																						if (valueHashCode6 != 0x2D93756B)
																																																						{
																																																							if (valueHashCode6 != 0x1F31F54B)
																																																							{
																																																								TMP_FontAsset tmp_FontAsset;
																																																								if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode6, out tmp_FontAsset))
																																																								{
																																																									tmp_FontAsset = Resources.Load<TMP_FontAsset>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																																																									if (tmp_FontAsset == null)
																																																									{
																																																										return false;
																																																									}
																																																									MaterialReferenceManager.AddFontAsset(tmp_FontAsset);
																																																								}
																																																								if (nameHashCode4 == 0)
																																																								{
																																																									if (valueHashCode == 0)
																																																									{
																																																										this.m_currentMaterial = tmp_FontAsset.material;
																																																										this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																										this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																										goto IL_2410;
																																																									}
																																																								}
																																																								if (nameHashCode4 != 0x629FDF7)
																																																								{
																																																									if (nameHashCode4 != 0x454D9F7)
																																																									{
																																																										return false;
																																																									}
																																																								}
																																																								Material material;
																																																								if (MaterialReferenceManager.TryGetMaterial(valueHashCode, out material))
																																																								{
																																																									this.m_currentMaterial = material;
																																																									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																									this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																								}
																																																								else
																																																								{
																																																									material = Resources.Load<Material>(TMP_Settings.defaultFontAssetPath + new string(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength));
																																																									if (material == null)
																																																									{
																																																										return false;
																																																									}
																																																									MaterialReferenceManager.AddFontMaterial(valueHashCode, material);
																																																									this.m_currentMaterial = material;
																																																									this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																																																									this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																																																								}
																																																								IL_2410:
																																																								this.m_currentFontAsset = tmp_FontAsset;
																																																								float num25 = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
																																																								float num26;
																																																								if (this.m_isOrthographic)
																																																								{
																																																									num26 = 1f;
																																																								}
																																																								else
																																																								{
																																																									num26 = 0.1f;
																																																								}
																																																								this.m_fontScale = num25 * num26;
																																																								return true;
																																																							}
																																																						}
																																																						this.m_currentFontAsset = this.m_materialReferences[0].fontAsset;
																																																						this.m_currentMaterial = this.m_materialReferences[0].material;
																																																						this.m_currentMaterialIndex = 0;
																																																						this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																																																						this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																																																						return true;
																																																					}
																																																				}
																																																				if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																																																				{
																																																					if (this.m_fontScaleMultiplier < 1f)
																																																					{
																																																						this.m_baselineOffset = this.m_baselineOffsetStack.Pop();
																																																						float fontScaleMultiplier = this.m_fontScaleMultiplier;
																																																						float num27;
																																																						if (this.m_currentFontAsset.fontInfo.SubSize > 0f)
																																																						{
																																																							num27 = this.m_currentFontAsset.fontInfo.SubSize;
																																																						}
																																																						else
																																																						{
																																																							num27 = 1f;
																																																						}
																																																						this.m_fontScaleMultiplier = fontScaleMultiplier / num27;
																																																					}
																																																					if (this.m_fontStyleStack.Remove(FontStyles.Superscript) == 0)
																																																					{
																																																						this.m_style &= (FontStyles)(-0x81);
																																																					}
																																																				}
																																																				return true;
																																																			}
																																																			IL_181C:
																																																			if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																																																			{
																																																				if (this.m_fontScaleMultiplier < 1f)
																																																				{
																																																					this.m_baselineOffset = this.m_baselineOffsetStack.Pop();
																																																					float fontScaleMultiplier2 = this.m_fontScaleMultiplier;
																																																					float num28;
																																																					if (this.m_currentFontAsset.fontInfo.SubSize > 0f)
																																																					{
																																																						num28 = this.m_currentFontAsset.fontInfo.SubSize;
																																																					}
																																																					else
																																																					{
																																																						num28 = 1f;
																																																					}
																																																					this.m_fontScaleMultiplier = fontScaleMultiplier2 / num28;
																																																				}
																																																				if (this.m_fontStyleStack.Remove(FontStyles.Subscript) == 0)
																																																				{
																																																					this.m_style &= (FontStyles)(-0x101);
																																																				}
																																																			}
																																																			return true;
																																																		}
																																																		IL_1CF4:
																																																		this.m_isIgnoringAlignment = false;
																																																		return true;
																																																	}
																																																}
																																																this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																																this.m_baselineOffsetStack.Push(this.m_baselineOffset);
																																																this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																																																this.m_fontStyleStack.Add(FontStyles.Superscript);
																																																this.m_style |= FontStyles.Superscript;
																																																return true;
																																															}
																																															IL_177C:
																																															this.m_fontScaleMultiplier *= ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																																															this.m_baselineOffsetStack.Push(this.m_baselineOffset);
																																															this.m_baselineOffset += this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																																															this.m_fontStyleStack.Add(FontStyles.Subscript);
																																															this.m_style |= FontStyles.Subscript;
																																															return true;
																																														}
																																														IL_1C44:
																																														num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																																														if (num12 == -9999f)
																																														{
																																															return false;
																																														}
																																														if (tagUnits == TagUnits.Pixels)
																																														{
																																															this.m_xAdvance = num12;
																																															return true;
																																														}
																																														if (tagUnits == TagUnits.FontUnits)
																																														{
																																															this.m_xAdvance = num12 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																															return true;
																																														}
																																														if (tagUnits != TagUnits.Percentage)
																																														{
																																															return false;
																																														}
																																														this.m_xAdvance = this.m_marginWidth * num12 / 100f;
																																														return true;
																																													}
																																												}
																																												return true;
																																											}
																																											return true;
																																										}
																																										return true;
																																									}
																																								}
																																								return true;
																																							}
																																							return true;
																																						}
																																						IL_41CC:
																																						int k = 1;
																																						while (k < this.m_xmlAttribute.Length)
																																						{
																																							if (this.m_xmlAttribute[k].nameHashCode == 0)
																																							{
																																								for (;;)
																																								{
																																									switch (2)
																																									{
																																									case 0:
																																										continue;
																																									}
																																									return true;
																																								}
																																							}
																																							else
																																							{
																																								int nameHashCode5 = this.m_xmlAttribute[k].nameHashCode;
																																								if (nameHashCode5 != 0x4FF7E)
																																								{
																																									if (nameHashCode5 != 0x435CD)
																																									{
																																									}
																																									else
																																									{
																																										int valueHashCode7 = this.m_xmlAttribute[k].valueHashCode;
																																										if (valueHashCode7 != -0x1F38AE01)
																																										{
																																											if (valueHashCode7 != -0x1B4FBB35)
																																											{
																																												if (valueHashCode7 != 0x3998DB)
																																												{
																																													if (valueHashCode7 != 0x825EC40)
																																													{
																																													}
																																													else
																																													{
																																														Debug.Log("TD align=\"right\".");
																																													}
																																												}
																																												else
																																												{
																																													Debug.Log("TD align=\"left\".");
																																												}
																																											}
																																											else
																																											{
																																												Debug.Log("TD align=\"center\".");
																																											}
																																										}
																																										else
																																										{
																																											Debug.Log("TD align=\"justified\".");
																																										}
																																									}
																																								}
																																								else
																																								{
																																									float num29 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[k].valueStartIndex, this.m_xmlAttribute[k].valueLength);
																																									if (tagUnits != TagUnits.Pixels)
																																									{
																																										if (tagUnits != TagUnits.FontUnits)
																																										{
																																											if (tagUnits != TagUnits.Percentage)
																																											{
																																											}
																																											else
																																											{
																																												Debug.Log("Table width = " + num29 + "%.");
																																											}
																																										}
																																										else
																																										{
																																											Debug.Log("Table width = " + num29 + "em.");
																																										}
																																									}
																																									else
																																									{
																																										Debug.Log("Table width = " + num29 + "px.");
																																									}
																																								}
																																								k++;
																																							}
																																						}
																																						return true;
																																					}
																																				}
																																				if (this.m_fontStyleStack.Remove(FontStyles.Italic) == 0)
																																				{
																																					this.m_style &= (FontStyles)(-3);
																																				}
																																				return true;
																																			}
																																			break;
																																		}
																																	}
																																	this.m_style |= FontStyles.Italic;
																																	this.m_fontStyleStack.Add(FontStyles.Italic);
																																	return true;
																																}
																																IL_13F7:
																																this.m_style |= FontStyles.Bold;
																																this.m_fontStyleStack.Add(FontStyles.Bold);
																																this.m_fontWeightInternal = 0x2BC;
																																this.m_fontWeightStack.Add(0x2BC);
																																return true;
																															}
																															goto IL_1A44;
																														}
																													}
																													num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																													if (num12 != -9999f)
																													{
																														if (num12 != 0f)
																														{
																															this.m_marginRight = num12;
																															if (tagUnits != TagUnits.Pixels)
																															{
																																if (tagUnits != TagUnits.FontUnits)
																																{
																																	if (tagUnits != TagUnits.Percentage)
																																	{
																																	}
																																	else
																																	{
																																		float marginWidth2 = this.m_marginWidth;
																																		float num30;
																																		if (this.m_width != -1f)
																																		{
																																			num30 = this.m_width;
																																		}
																																		else
																																		{
																																			num30 = 0f;
																																		}
																																		this.m_marginRight = (marginWidth2 - num30) * this.m_marginRight / 100f;
																																	}
																																}
																																else
																																{
																																	this.m_marginRight *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																																}
																															}
																															float marginRight;
																															if (this.m_marginRight >= 0f)
																															{
																																marginRight = this.m_marginRight;
																															}
																															else
																															{
																																marginRight = 0f;
																															}
																															this.m_marginRight = marginRight;
																															return true;
																														}
																													}
																													return false;
																												}
																												IL_3AAA:
																												if (this.m_fontStyleStack.Remove(FontStyles.UpperCase) == 0)
																												{
																													this.m_style &= (FontStyles)(-0x11);
																												}
																												return true;
																											}
																											IL_3AEB:
																											if (this.m_fontStyleStack.Remove(FontStyles.SmallCaps) == 0)
																											{
																												this.m_style &= (FontStyles)(-0x21);
																											}
																											return true;
																										}
																										IL_3A60:
																										if (this.m_fontStyleStack.Remove(FontStyles.LowerCase) == 0)
																										{
																											this.m_style &= (FontStyles)(-9);
																										}
																										return true;
																									}
																									goto IL_1C09;
																								}
																								IL_1A44:
																								num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																								if (num12 != -9999f)
																								{
																									if (num12 == 0f)
																									{
																									}
																									else
																									{
																										if ((this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
																										{
																											return true;
																										}
																										this.m_style &= (FontStyles)(-2);
																										int num31 = (int)num12;
																										if (num31 != 0x64)
																										{
																											if (num31 != 0xC8)
																											{
																												if (num31 != 0x12C)
																												{
																													if (num31 != 0x190)
																													{
																														if (num31 != 0x1F4)
																														{
																															if (num31 != 0x258)
																															{
																																if (num31 != 0x2BC)
																																{
																																	if (num31 != 0x320)
																																	{
																																		if (num31 == 0x384)
																																		{
																																			this.m_fontWeightInternal = 0x384;
																																		}
																																	}
																																	else
																																	{
																																		this.m_fontWeightInternal = 0x320;
																																	}
																																}
																																else
																																{
																																	this.m_fontWeightInternal = 0x2BC;
																																	this.m_style |= FontStyles.Bold;
																																}
																															}
																															else
																															{
																																this.m_fontWeightInternal = 0x258;
																															}
																														}
																														else
																														{
																															this.m_fontWeightInternal = 0x1F4;
																														}
																													}
																													else
																													{
																														this.m_fontWeightInternal = 0x190;
																													}
																												}
																												else
																												{
																													this.m_fontWeightInternal = 0x12C;
																												}
																											}
																											else
																											{
																												this.m_fontWeightInternal = 0xC8;
																											}
																										}
																										else
																										{
																											this.m_fontWeightInternal = 0x64;
																										}
																										this.m_fontWeightStack.Add(this.m_fontWeightInternal);
																										return true;
																									}
																								}
																								return false;
																							}
																							IL_337E:
																							this.tag_LineIndent = 0f;
																							return true;
																						}
																						IL_3F81:
																						this.m_lineHeight = -32767f;
																						return true;
																					}
																					IL_3288:
																					num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																					if (num12 != -9999f)
																					{
																						if (num12 != 0f)
																						{
																							if (tagUnits != TagUnits.Pixels)
																							{
																								if (tagUnits != TagUnits.FontUnits)
																								{
																									if (tagUnits != TagUnits.Percentage)
																									{
																									}
																									else
																									{
																										this.tag_LineIndent = this.m_marginWidth * num12 / 100f;
																									}
																								}
																								else
																								{
																									this.tag_LineIndent = num12;
																									this.tag_LineIndent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																								}
																							}
																							else
																							{
																								this.tag_LineIndent = num12;
																							}
																							this.m_xAdvance += this.tag_LineIndent;
																							return true;
																						}
																					}
																					return false;
																				}
																				IL_3EAF:
																				num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																				if (num12 == -9999f)
																				{
																					return false;
																				}
																				this.m_lineHeight = num12;
																				if (tagUnits != TagUnits.Pixels)
																				{
																					if (tagUnits != TagUnits.FontUnits)
																					{
																						if (tagUnits != TagUnits.Percentage)
																						{
																						}
																						else
																						{
																							this.m_lineHeight = this.m_fontAsset.fontInfo.LineHeight * this.m_lineHeight / 100f * this.m_fontScale;
																						}
																					}
																					else
																					{
																						this.m_lineHeight *= this.m_fontAsset.fontInfo.LineHeight * this.m_fontScale;
																					}
																				}
																				return true;
																			}
																			IL_3C5D:
																			num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																			if (num12 != -9999f)
																			{
																				if (num12 != 0f)
																				{
																					this.m_marginLeft = num12;
																					if (tagUnits != TagUnits.Pixels)
																					{
																						if (tagUnits != TagUnits.FontUnits)
																						{
																							if (tagUnits == TagUnits.Percentage)
																							{
																								this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
																							}
																						}
																						else
																						{
																							this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																						}
																					}
																					float marginLeft;
																					if (this.m_marginLeft >= 0f)
																					{
																						marginLeft = this.m_marginLeft;
																					}
																					else
																					{
																						marginLeft = 0f;
																					}
																					this.m_marginLeft = marginLeft;
																					return true;
																				}
																			}
																			return false;
																		}
																	}
																	IL_3A89:
																	this.m_style |= FontStyles.UpperCase;
																	this.m_fontStyleStack.Add(FontStyles.UpperCase);
																	return true;
																}
																IL_3ACA:
																this.m_style |= FontStyles.SmallCaps;
																this.m_fontStyleStack.Add(FontStyles.SmallCaps);
																return true;
															}
															IL_3A41:
															this.m_style |= FontStyles.LowerCase;
															this.m_fontStyleStack.Add(FontStyles.LowerCase);
															return true;
														}
														IL_1C09:
														this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
														if (this.m_fontWeightInternal == 0x190)
														{
															this.m_style &= (FontStyles)(-2);
														}
														return true;
													}
												}
												if (this.m_currentMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != this.m_materialReferenceStack.PreviousItem().material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
												{
													return false;
												}
												MaterialReference materialReference2 = this.m_materialReferenceStack.Remove();
												this.m_currentMaterial = materialReference2.material;
												this.m_currentMaterialIndex = materialReference2.index;
												return true;
											}
											IL_1CFD:
											num12 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
											if (num12 != -9999f)
											{
												if (num12 != 0f)
												{
													if (tagUnits == TagUnits.Pixels)
													{
														this.m_baselineOffset = num12;
														return true;
													}
													if (tagUnits != TagUnits.FontUnits)
													{
														return tagUnits != TagUnits.Percentage && false;
													}
													this.m_baselineOffset = num12 * this.m_fontScale * this.m_fontAsset.fontInfo.Ascender;
													return true;
												}
											}
											return false;
										}
										case 0x1BE:
											goto IL_165F;
										}
									}
									if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
									{
										this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
										if (this.m_fontStyleStack.Remove(FontStyles.Bold) == 0)
										{
											this.m_style &= (FontStyles)(-2);
										}
									}
									return true;
								}
								return true;
							case 0x19E:
								goto IL_165F;
							}
							IL_156E:
							if ((this.m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough)
							{
								if (this.m_fontStyleStack.Remove(FontStyles.Strikethrough) == 0)
								{
									this.m_style &= (FontStyles)(-0x41);
								}
							}
							return true;
							IL_165F:
							if ((this.m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
							{
								this.m_underlineColor = this.m_underlineColorStack.Remove();
								if (this.m_fontStyleStack.Remove(FontStyles.Underline) == 0)
								{
									this.m_style &= (FontStyles)(-5);
								}
							}
							return true;
						case 0x75:
							goto IL_15B1;
						}
						break;
					case 0x55:
						goto IL_15B1;
					}
					this.m_style |= FontStyles.Strikethrough;
					this.m_fontStyleStack.Add(FontStyles.Strikethrough);
					if (this.m_xmlAttribute[1].nameHashCode != 0x44D63)
					{
						if (this.m_xmlAttribute[1].nameHashCode != 0x2EF43)
						{
							this.m_strikethroughColor = this.m_htmlColor;
							goto IL_155B;
						}
					}
					this.m_strikethroughColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
					IL_155B:
					this.m_strikethroughColorStack.Add(this.m_strikethroughColor);
					return true;
					IL_15B1:
					this.m_style |= FontStyles.Underline;
					this.m_fontStyleStack.Add(FontStyles.Underline);
					if (this.m_xmlAttribute[1].nameHashCode != 0x44D63)
					{
						if (this.m_xmlAttribute[1].nameHashCode != 0x2EF43)
						{
							this.m_underlineColor = this.m_htmlColor;
							goto IL_164C;
						}
					}
					this.m_underlineColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
					IL_164C:
					this.m_underlineColorStack.Add(this.m_underlineColor);
					return true;
				}
			}
			this.tag_NoParsing = false;
			return true;
		}

		protected enum TextInputSources
		{
			Text,
			SetText,
			SetCharArray,
			String
		}
	}
}
