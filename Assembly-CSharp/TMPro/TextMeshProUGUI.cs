using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[RequireComponent(typeof(CanvasRenderer))]
	[ExecuteInEditMode]
	[AddComponentMenu("UI/TextMeshPro - Text (UI)", 11)]
	[RequireComponent(typeof(RectTransform))]
	[SelectionBase]
	[DisallowMultipleComponent]
	public class TextMeshProUGUI : TMP_Text, ILayoutElement
	{
		[SerializeField]
		private bool m_hasFontAssetChanged;

		[SerializeField]
		protected TMP_SubMeshUI[] m_subTextObjects = new TMP_SubMeshUI[8];

		private float m_previousLossyScaleY = -1f;

		private Vector3[] m_RectTransformCorners = new Vector3[4];

		private CanvasRenderer m_canvasRenderer;

		private Canvas m_canvas;

		private bool m_isFirstAllocation;

		private int m_max_characters = 8;

		private bool m_isMaskingEnabled;

		[SerializeField]
		private Material m_baseMaterial;

		private bool m_isScrollRegionSet;

		private int m_stencilID;

		[SerializeField]
		private Vector4 m_maskOffset;

		private Matrix4x4 m_EnvMapMatrix = default(Matrix4x4);

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private int m_recursiveCountA;

		private int loopCountA;

		private bool m_isRebuildingLayout;

		public override Material materialForRendering
		{
			get { return TMP_MaterialManager.GetMaterialForRendering(this, m_sharedMaterial); }
		}

		public override bool autoSizeTextContainer
		{
			get
			{
				return m_autoSizeTextContainer;
			}
			set
			{
				if (m_autoSizeTextContainer == value)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return;
						}
					}
				}
				m_autoSizeTextContainer = value;
				if (!m_autoSizeTextContainer)
				{
					return;
				}
				while (true)
				{
					CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
					SetLayoutDirty();
					return;
				}
			}
		}

		public override Mesh mesh
		{
			get { return m_mesh; }
		}

		public new CanvasRenderer canvasRenderer
		{
			get
			{
				if (m_canvasRenderer == null)
				{
					m_canvasRenderer = GetComponent<CanvasRenderer>();
				}
				return m_canvasRenderer;
			}
		}

		public InlineGraphicManager inlineGraphicManager
		{
			get { return m_inlineGraphics; }
		}

		public Vector4 maskOffset
		{
			get
			{
				return m_maskOffset;
			}
			set
			{
				m_maskOffset = value;
				UpdateMask();
				m_havePropertiesChanged = true;
			}
		}

		protected override void Awake()
		{
			m_canvas = base.canvas;
			m_isOrthographic = true;
			m_rectTransform = base.gameObject.GetComponent<RectTransform>();
			if (m_rectTransform == null)
			{
				m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			m_canvasRenderer = GetComponent<CanvasRenderer>();
			if (m_canvasRenderer == null)
			{
				m_canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
			}
			if (m_mesh == null)
			{
				m_mesh = new Mesh();
				m_mesh.hideFlags = HideFlags.HideAndDontSave;
			}
			LoadDefaultSettings();
			LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			if (m_char_buffer == null)
			{
				m_char_buffer = new int[m_max_characters];
			}
			m_cached_TextElement = new TMP_Glyph();
			m_isFirstAllocation = true;
			if (m_textInfo == null)
			{
				m_textInfo = new TMP_TextInfo(this);
			}
			if (m_fontAsset == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Debug.LogWarning(new StringBuilder().Append("Please assign a Font Asset to this ").Append(base.transform.name).Append(" gameobject.").ToString(), this);
						return;
					}
				}
			}
			TMP_SubMeshUI[] componentsInChildren = GetComponentsInChildren<TMP_SubMeshUI>();
			if (componentsInChildren.Length > 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					m_subTextObjects[i + 1] = componentsInChildren[i];
				}
			}
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			m_isAwake = true;
		}

		protected override void OnEnable()
		{
			if (!m_isRegisteredForEvents)
			{
				m_isRegisteredForEvents = true;
			}
			m_canvas = GetCanvas();
			SetActiveSubMeshes(true);
			GraphicRegistry.RegisterGraphicForCanvas(m_canvas, this);
			ComputeMarginSize();
			m_verticesAlreadyDirty = false;
			m_layoutAlreadyDirty = false;
			m_ShouldRecalculateStencil = true;
			m_isInputParsingRequired = true;
			SetAllDirty();
			RecalculateClipping();
		}

		protected override void OnDisable()
		{
			if (m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
				m_MaskMaterial = null;
			}
			GraphicRegistry.UnregisterGraphicForCanvas(m_canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (m_canvasRenderer != null)
			{
				m_canvasRenderer.Clear();
			}
			SetActiveSubMeshes(false);
			LayoutRebuilder.MarkLayoutForRebuild(m_rectTransform);
			RecalculateClipping();
		}

		protected override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(m_canvas, this);
			if (m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(m_mesh);
			}
			if (m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
				m_MaskMaterial = null;
			}
			m_isRegisteredForEvents = false;
		}

		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (m_fontAsset == null)
			{
				if (TMP_Settings.defaultFontAsset != null)
				{
					m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					m_fontAsset = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				}
				if (m_fontAsset == null)
				{
					while (true)
					{
						Debug.LogWarning(new StringBuilder().Append("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to ").Append(base.gameObject.name).Append(".").ToString(), this);
						return;
					}
				}
				if (m_fontAsset.characterDictionary == null)
				{
					Debug.Log("Dictionary is Null!");
				}
				m_sharedMaterial = m_fontAsset.material;
			}
			else
			{
				if (m_fontAsset.characterDictionary == null)
				{
					m_fontAsset.ReadFontDefinition();
				}
				if (m_sharedMaterial == null && m_baseMaterial != null)
				{
					m_sharedMaterial = m_baseMaterial;
					m_baseMaterial = null;
				}
				if (!(m_sharedMaterial == null))
				{
					if (!(m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null))
					{
						if (m_fontAsset.atlas.GetInstanceID() == m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							goto IL_0228;
						}
					}
				}
				if (m_fontAsset.material == null)
				{
					Debug.LogWarning(new StringBuilder().Append("The Font Atlas Texture of the Font Asset ").Append(m_fontAsset.name).Append(" assigned to ").Append(base.gameObject.name).Append(" is missing.").ToString(), this);
				}
				else
				{
					m_sharedMaterial = m_fontAsset.material;
				}
			}
			goto IL_0228;
			IL_0228:
			GetSpecialCharacters(m_fontAsset);
			m_padding = GetPaddingForMaterial();
			SetMaterialDirty();
		}

		private Canvas GetCanvas()
		{
			Canvas result = null;
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent(false, list);
			if (list.Count > 0)
			{
				int num = 0;
				while (true)
				{
					if (num < list.Count)
					{
						if (list[num].isActiveAndEnabled)
						{
							result = list[num];
							break;
						}
						num++;
						continue;
					}
					break;
				}
			}
			TMP_ListPool<Canvas>.Release(list);
			return result;
		}

		private void UpdateEnvMapMatrix()
		{
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) && !(m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null))
			{
				Vector3 euler = m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
				m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
				m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, m_EnvMapMatrix);
			}
		}

		private void EnableMasking()
		{
			if (m_fontMaterial == null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
				m_canvasRenderer.SetMaterial(m_fontMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			m_sharedMaterial = m_fontMaterial;
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				UpdateMask();
			}
			m_isMaskingEnabled = true;
		}

		private void DisableMasking()
		{
			if (m_fontMaterial != null)
			{
				if (m_stencilID > 0)
				{
					m_sharedMaterial = m_MaskMaterial;
				}
				m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
				UnityEngine.Object.DestroyImmediate(m_fontMaterial);
			}
			m_isMaskingEnabled = false;
		}

		private void UpdateMask()
		{
			if (!(m_rectTransform != null))
			{
				return;
			}
			while (true)
			{
				if (!ShaderUtilities.isInitialized)
				{
					ShaderUtilities.GetShaderPropertyIDs();
				}
				m_isScrollRegionSet = true;
				float num = Mathf.Min(Mathf.Min(m_margin.x, m_margin.z), m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
				float num2 = Mathf.Min(Mathf.Min(m_margin.y, m_margin.w), m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
				float num3;
				if (num > 0f)
				{
					num3 = num;
				}
				else
				{
					num3 = 0f;
				}
				num = num3;
				num2 = ((!(num2 > 0f)) ? 0f : num2);
				float z = (m_rectTransform.rect.width - Mathf.Max(m_margin.x, 0f) - Mathf.Max(m_margin.z, 0f)) / 2f + num;
				float w = (m_rectTransform.rect.height - Mathf.Max(m_margin.y, 0f) - Mathf.Max(m_margin.w, 0f)) / 2f + num2;
				Vector3 localPosition = m_rectTransform.localPosition;
				Vector2 pivot = m_rectTransform.pivot;
				float x = (0.5f - pivot.x) * m_rectTransform.rect.width + (Mathf.Max(m_margin.x, 0f) - Mathf.Max(m_margin.z, 0f)) / 2f;
				Vector2 pivot2 = m_rectTransform.pivot;
				Vector2 vector = localPosition + new Vector3(x, (0.5f - pivot2.y) * m_rectTransform.rect.height + (0f - Mathf.Max(m_margin.y, 0f) + Mathf.Max(m_margin.w, 0f)) / 2f);
				Vector4 value = new Vector4(vector.x, vector.y, z, w);
				m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, value);
				return;
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (!(m_fontMaterial == null))
			{
				if (m_fontMaterial.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_0054;
				}
			}
			m_fontMaterial = CreateMaterialInstance(mat);
			goto IL_0054;
			IL_0054:
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			m_ShouldRecalculateStencil = true;
			SetVerticesDirty();
			SetMaterialDirty();
			return m_sharedMaterial;
		}

		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontMaterials == null)
			{
				m_fontMaterials = new Material[materialCount];
			}
			else if (m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					m_fontMaterials[i] = m_subTextObjects[i].material;
				}
			}
			m_fontSharedMaterials = m_fontMaterials;
			return m_fontMaterials;
		}

		protected override void SetSharedMaterial(Material mat)
		{
			m_sharedMaterial = mat;
			m_padding = GetPaddingForMaterial();
			SetMaterialDirty();
		}

		protected override Material[] GetSharedMaterials()
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontSharedMaterials == null)
			{
				m_fontSharedMaterials = new Material[materialCount];
			}
			else if (m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					m_fontSharedMaterials[i] = m_sharedMaterial;
				}
				else
				{
					m_fontSharedMaterials[i] = m_subTextObjects[i].sharedMaterial;
				}
			}
			while (true)
			{
				return m_fontSharedMaterials;
			}
		}

		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontSharedMaterials == null)
			{
				m_fontSharedMaterials = new Material[materialCount];
			}
			else if (m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					if (materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null)
					{
						continue;
					}
					if (materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
					{
					}
					else
					{
						m_sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
						m_padding = GetPaddingForMaterial(m_sharedMaterial);
					}
				}
				else
				{
					if (materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null)
					{
						continue;
					}
					if (materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
					{
					}
					else if (m_subTextObjects[i].isDefaultMaterial)
					{
						m_subTextObjects[i].sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			if (m_fontMaterial != null)
			{
				if (m_sharedMaterial.GetInstanceID() != m_fontMaterial.GetInstanceID())
				{
					m_sharedMaterial = m_fontMaterial;
					m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
					goto IL_00d3;
				}
			}
			if (m_fontMaterial == null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
				m_sharedMaterial = m_fontMaterial;
				m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			goto IL_00d3;
			IL_00d3:
			thickness = Mathf.Clamp01(thickness);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			m_padding = GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			if (m_fontMaterial == null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, color);
		}

		protected override void SetOutlineColor(Color32 color)
		{
			if (m_fontMaterial == null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color);
		}

		protected override void SetShaderDepth()
		{
			if (m_canvas == null)
			{
				return;
			}
			while (true)
			{
				if (m_sharedMaterial == null)
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
				if (m_canvas.renderMode != 0)
				{
					if (!m_isOverlay)
					{
						m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
						return;
					}
				}
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
				return;
			}
		}

		protected override void SetCulling()
		{
			if (m_isCullingEnabled)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						Material materialForRendering = this.materialForRendering;
						if (materialForRendering != null)
						{
							materialForRendering.SetFloat("_CullMode", 2f);
						}
						int num = 1;
						while (true)
						{
							if (num >= m_subTextObjects.Length)
							{
								return;
							}
							if (!(m_subTextObjects[num] != null))
							{
								break;
							}
							materialForRendering = m_subTextObjects[num].materialForRendering;
							if (materialForRendering != null)
							{
								materialForRendering.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
							}
							num++;
						}
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
				}
			}
			Material materialForRendering2 = this.materialForRendering;
			if (materialForRendering2 != null)
			{
				materialForRendering2.SetFloat("_CullMode", 0f);
			}
			int num2 = 1;
			while (num2 < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num2] != null)
					{
						materialForRendering2 = m_subTextObjects[num2].materialForRendering;
						if (materialForRendering2 != null)
						{
							materialForRendering2.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
						}
						num2++;
						goto IL_010a;
					}
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
				IL_010a:;
			}
		}

		private void SetPerspectiveCorrection()
		{
			if (m_isOrthographic)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
						return;
					}
				}
			}
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
		}

		protected override float GetPaddingForMaterial(Material mat)
		{
			m_padding = ShaderUtilities.GetPadding(mat, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
		}

		protected override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
		}

		private void SetMeshArrays(int size)
		{
			m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			m_canvasRenderer.SetMesh(m_textInfo.meshInfo[0].mesh);
		}

		protected override int SetArraySizes(int[] chars)
		{
			int endIndex = 0;
			int num = 0;
			m_totalCharacterCount = 0;
			m_isUsingBold = false;
			m_isParsingText = false;
			tag_NoParsing = false;
			m_style = m_fontStyle;
			int fontWeightInternal;
			if ((m_style & FontStyles.Bold) == FontStyles.Bold)
			{
				fontWeightInternal = 700;
			}
			else
			{
				fontWeightInternal = m_fontWeight;
			}
			m_fontWeightInternal = fontWeightInternal;
			m_fontWeightStack.SetDefault(m_fontWeightInternal);
			m_currentFontAsset = m_fontAsset;
			m_currentMaterial = m_sharedMaterial;
			m_currentMaterialIndex = 0;
			m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
			m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
			if (m_textInfo == null)
			{
				m_textInfo = new TMP_TextInfo();
			}
			m_textElementType = TMP_TextElementType.Character;
			if (m_linkedTextComponent != null)
			{
				m_linkedTextComponent.text = string.Empty;
				m_linkedTextComponent.ForceMeshUpdate();
			}
			for (int i = 0; i < chars.Length && chars[i] != 0; i++)
			{
				if (m_textInfo.characterInfo != null)
				{
					if (m_totalCharacterCount < m_textInfo.characterInfo.Length)
					{
						goto IL_017a;
					}
				}
				TMP_TextInfo.Resize(ref m_textInfo.characterInfo, m_totalCharacterCount + 1, true);
				goto IL_017a;
				IL_03e8:
				TMP_FontAsset fontAssetForWeight = GetFontAssetForWeight(m_fontWeightInternal);
				bool flag;
				bool isUsingAlternateTypeface;
				if (fontAssetForWeight != null)
				{
					flag = true;
					isUsingAlternateTypeface = true;
					m_currentFontAsset = fontAssetForWeight;
				}
				int num2;
				TMP_Glyph glyph;
				fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
				int currentMaterialIndex;
				if (glyph == null)
				{
					TMP_SpriteAsset spriteAsset = base.spriteAsset;
					if (spriteAsset != null)
					{
						int spriteIndex = -1;
						spriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(spriteAsset, num2, out spriteIndex);
						if (spriteIndex != -1)
						{
							m_textElementType = TMP_TextElementType.Sprite;
							m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
							m_currentMaterialIndex = MaterialReference.AddMaterialReference(spriteAsset.material, spriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
							m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = spriteIndex;
							m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = spriteAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
							m_textElementType = TMP_TextElementType.Character;
							m_currentMaterialIndex = currentMaterialIndex;
							num++;
							m_totalCharacterCount++;
							continue;
						}
					}
				}
				if (glyph == null)
				{
					if (TMP_Settings.fallbackFontAssets != null)
					{
						if (TMP_Settings.fallbackFontAssets.Count > 0)
						{
							fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num2, out glyph);
						}
					}
				}
				if (glyph == null)
				{
					if (TMP_Settings.defaultFontAsset != null)
					{
						fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num2, out glyph);
					}
				}
				if (glyph == null)
				{
					TMP_SpriteAsset defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
					if (defaultSpriteAsset != null)
					{
						int spriteIndex2 = -1;
						defaultSpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(defaultSpriteAsset, num2, out spriteIndex2);
						if (spriteIndex2 != -1)
						{
							m_textElementType = TMP_TextElementType.Sprite;
							m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
							m_currentMaterialIndex = MaterialReference.AddMaterialReference(defaultSpriteAsset.material, defaultSpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
							m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = spriteIndex2;
							m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = defaultSpriteAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
							m_textElementType = TMP_TextElementType.Character;
							m_currentMaterialIndex = currentMaterialIndex;
							num++;
							m_totalCharacterCount++;
							continue;
						}
					}
				}
				if (glyph == null)
				{
					num2 = (chars[i] = ((TMP_Settings.missingGlyphCharacter != 0) ? TMP_Settings.missingGlyphCharacter : 9633));
					fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
					if (glyph == null && TMP_Settings.fallbackFontAssets != null)
					{
						if (TMP_Settings.fallbackFontAssets.Count > 0)
						{
							fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num2, out glyph);
						}
					}
					if (glyph == null)
					{
						if (TMP_Settings.defaultFontAsset != null)
						{
							fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num2, out glyph);
						}
					}
					if (glyph == null)
					{
						num2 = (chars[i] = 32);
						fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
						if (!TMP_Settings.warningsDisabled)
						{
							Debug.LogWarning(new StringBuilder().Append("Character with ASCII value of ").Append(num2).Append(" was not found in the Font Asset Glyph Table. It was replaced by a space.").ToString(), this);
						}
					}
				}
				if (fontAssetForWeight != null)
				{
					if (fontAssetForWeight.GetInstanceID() != m_currentFontAsset.GetInstanceID())
					{
						flag = true;
						isUsingAlternateTypeface = false;
						m_currentFontAsset = fontAssetForWeight;
					}
				}
				m_textInfo.characterInfo[m_totalCharacterCount].elementType = TMP_TextElementType.Character;
				m_textInfo.characterInfo[m_totalCharacterCount].textElement = glyph;
				m_textInfo.characterInfo[m_totalCharacterCount].isUsingAlternateTypeface = isUsingAlternateTypeface;
				m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
				m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
				if (flag)
				{
					if (TMP_Settings.matchMaterialPreset)
					{
						m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(m_currentMaterial, m_currentFontAsset.material);
					}
					else
					{
						m_currentMaterial = m_currentFontAsset.material;
					}
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
				}
				if (!char.IsWhiteSpace((char)num2))
				{
					if (num2 != 8203)
					{
						if (m_materialReferences[m_currentMaterialIndex].referenceCount < 16383)
						{
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
						}
						else
						{
							m_currentMaterialIndex = MaterialReference.AddMaterialReference(new Material(m_currentMaterial), m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
						}
					}
				}
				m_textInfo.characterInfo[m_totalCharacterCount].material = m_currentMaterial;
				m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
				m_materialReferences[m_currentMaterialIndex].isFallbackMaterial = flag;
				Material currentMaterial;
				TMP_FontAsset currentFontAsset;
				if (flag)
				{
					m_materialReferences[m_currentMaterialIndex].fallbackMaterial = currentMaterial;
					m_currentFontAsset = currentFontAsset;
					m_currentMaterial = currentMaterial;
					m_currentMaterialIndex = currentMaterialIndex;
				}
				m_totalCharacterCount++;
				continue;
				IL_017a:
				num2 = chars[i];
				if (m_isRichText)
				{
					if (num2 == 60)
					{
						int currentMaterialIndex2 = m_currentMaterialIndex;
						if (ValidateHtmlTag(chars, i + 1, out endIndex))
						{
							i = endIndex;
							if ((m_style & FontStyles.Bold) == FontStyles.Bold)
							{
								m_isUsingBold = true;
							}
							if (m_textElementType == TMP_TextElementType.Sprite)
							{
								m_materialReferences[m_currentMaterialIndex].referenceCount++;
								m_textInfo.characterInfo[m_totalCharacterCount].character = (char)(57344 + m_spriteIndex);
								m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = m_spriteIndex;
								m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
								m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = m_currentSpriteAsset;
								m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
								m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
								m_textElementType = TMP_TextElementType.Character;
								m_currentMaterialIndex = currentMaterialIndex2;
								num++;
								m_totalCharacterCount++;
							}
							continue;
						}
					}
				}
				flag = false;
				isUsingAlternateTypeface = false;
				currentFontAsset = m_currentFontAsset;
				currentMaterial = m_currentMaterial;
				currentMaterialIndex = m_currentMaterialIndex;
				if (m_textElementType == TMP_TextElementType.Character)
				{
					if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num2))
						{
							num2 = char.ToUpper((char)num2);
						}
					}
					else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num2))
						{
							num2 = char.ToLower((char)num2);
						}
					}
					else
					{
						if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
						{
							if ((m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
							{
								goto IL_03e8;
							}
						}
						if (char.IsLower((char)num2))
						{
							num2 = char.ToUpper((char)num2);
						}
					}
				}
				goto IL_03e8;
			}
			if (m_isCalculatingPreferredValues)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_isCalculatingPreferredValues = false;
						m_isInputParsingRequired = true;
						return m_totalCharacterCount;
					}
				}
			}
			m_textInfo.spriteCount = num;
			int num3 = m_textInfo.materialCount = m_materialReferenceIndexLookup.Count;
			if (num3 > m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize(ref m_textInfo.meshInfo, num3, false);
			}
			if (num3 > m_subTextObjects.Length)
			{
				TMP_TextInfo.Resize(ref m_subTextObjects, Mathf.NextPowerOfTwo(num3 + 1));
			}
			if (m_textInfo.characterInfo.Length - m_totalCharacterCount > 256)
			{
				TMP_TextInfo.Resize(ref m_textInfo.characterInfo, Mathf.Max(m_totalCharacterCount + 1, 256), true);
			}
			for (int j = 0; j < num3; j++)
			{
				if (j > 0)
				{
					if (m_subTextObjects[j] == null)
					{
						m_subTextObjects[j] = TMP_SubMeshUI.AddSubTextObject(this, m_materialReferences[j]);
						m_textInfo.meshInfo[j].vertices = null;
					}
					if (m_rectTransform.pivot != m_subTextObjects[j].rectTransform.pivot)
					{
						m_subTextObjects[j].rectTransform.pivot = m_rectTransform.pivot;
					}
					if (!(m_subTextObjects[j].sharedMaterial == null))
					{
						if (m_subTextObjects[j].sharedMaterial.GetInstanceID() == m_materialReferences[j].material.GetInstanceID())
						{
							goto IL_0e4a;
						}
					}
					bool isDefaultMaterial = m_materialReferences[j].isDefaultMaterial;
					m_subTextObjects[j].isDefaultMaterial = isDefaultMaterial;
					if (isDefaultMaterial && !(m_subTextObjects[j].sharedMaterial == null))
					{
						if (m_subTextObjects[j].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == m_materialReferences[j].material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							goto IL_0e4a;
						}
					}
					m_subTextObjects[j].sharedMaterial = m_materialReferences[j].material;
					m_subTextObjects[j].fontAsset = m_materialReferences[j].fontAsset;
					m_subTextObjects[j].spriteAsset = m_materialReferences[j].spriteAsset;
					goto IL_0e4a;
				}
				goto IL_0ea8;
				IL_0ea8:
				int referenceCount = m_materialReferences[j].referenceCount;
				if (m_textInfo.meshInfo[j].vertices != null)
				{
					if (m_textInfo.meshInfo[j].vertices.Length >= referenceCount * 4)
					{
						if (m_textInfo.meshInfo[j].vertices.Length - referenceCount * 4 <= 1024)
						{
							continue;
						}
						int size;
						if (referenceCount > 1024)
						{
							size = referenceCount + 256;
						}
						else
						{
							size = Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 256);
						}
						m_textInfo.meshInfo[j].ResizeMeshInfo(size);
						continue;
					}
				}
				if (m_textInfo.meshInfo[j].vertices == null)
				{
					if (j == 0)
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_mesh, referenceCount + 1);
					}
					else
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_subTextObjects[j].mesh, referenceCount + 1);
					}
					continue;
				}
				int size2;
				if (referenceCount > 1024)
				{
					size2 = referenceCount + 256;
				}
				else
				{
					size2 = Mathf.NextPowerOfTwo(referenceCount);
				}
				m_textInfo.meshInfo[j].ResizeMeshInfo(size2);
				continue;
				IL_0e4a:
				if (m_materialReferences[j].isFallbackMaterial)
				{
					m_subTextObjects[j].fallbackMaterial = m_materialReferences[j].material;
					m_subTextObjects[j].fallbackSourceMaterial = m_materialReferences[j].fallbackMaterial;
				}
				goto IL_0ea8;
			}
			while (true)
			{
				for (int k = num3; k < m_subTextObjects.Length; k++)
				{
					if (m_subTextObjects[k] != null)
					{
						if (k < m_textInfo.meshInfo.Length)
						{
							m_subTextObjects[k].canvasRenderer.SetMesh(null);
						}
						continue;
					}
					break;
				}
				return m_totalCharacterCount;
			}
		}

		protected override void ComputeMarginSize()
		{
			if (!(base.rectTransform != null))
			{
				return;
			}
			while (true)
			{
				m_marginWidth = m_rectTransform.rect.width - m_margin.x - m_margin.z;
				m_marginHeight = m_rectTransform.rect.height - m_margin.y - m_margin.w;
				m_RectTransformCorners = GetTextContainerLocalCorners();
				return;
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			m_havePropertiesChanged = true;
			SetVerticesDirty();
			SetLayoutDirty();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			m_canvas = base.canvas;
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			m_canvas = base.canvas;
			ComputeMarginSize();
			m_havePropertiesChanged = true;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (base.gameObject.activeInHierarchy)
			{
				ComputeMarginSize();
				UpdateSubObjectPivot();
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}

		private void LateUpdate()
		{
			if (m_rectTransform.hasChanged)
			{
				Vector3 lossyScale = m_rectTransform.lossyScale;
				float y = lossyScale.y;
				if (!m_havePropertiesChanged && y != m_previousLossyScaleY)
				{
					if (m_text != string.Empty)
					{
						if (m_text != null)
						{
							UpdateSDFScale(y);
							m_previousLossyScaleY = y;
						}
					}
				}
				m_rectTransform.hasChanged = false;
			}
			if (!m_isUsingLegacyAnimationComponent)
			{
				return;
			}
			while (true)
			{
				m_havePropertiesChanged = true;
				OnPreRenderCanvas();
				return;
			}
		}

		private void OnPreRenderCanvas()
		{
			if (!m_isAwake)
			{
				return;
			}
			while (true)
			{
				if (!m_ignoreActiveState && !IsActive())
				{
					return;
				}
				if (m_canvas == null)
				{
					m_canvas = base.canvas;
					if (m_canvas == null)
					{
						return;
					}
				}
				loopCountA = 0;
				if (!m_havePropertiesChanged)
				{
					if (!m_isLayoutDirty)
					{
						return;
					}
				}
				if (checkPaddingRequired)
				{
					UpdateMeshPadding();
				}
				if (!m_isInputParsingRequired)
				{
					if (!m_isTextTruncated)
					{
						goto IL_00cb;
					}
				}
				ParseInputText();
				goto IL_00cb;
				IL_00cb:
				if (m_enableAutoSizing)
				{
					m_fontSize = Mathf.Clamp(m_fontSize, m_fontSizeMin, m_fontSizeMax);
				}
				m_maxFontSize = m_fontSizeMax;
				m_minFontSize = m_fontSizeMin;
				m_lineSpacingDelta = 0f;
				m_charWidthAdjDelta = 0f;
				m_isCharacterWrappingEnabled = false;
				m_isTextTruncated = false;
				m_havePropertiesChanged = false;
				m_isLayoutDirty = false;
				m_ignoreActiveState = false;
				GenerateTextMesh();
				return;
			}
		}

		protected override void GenerateTextMesh()
		{
			if (!(m_fontAsset == null))
			{
				if (m_fontAsset.characterDictionary != null)
				{
					if (m_textInfo != null)
					{
						m_textInfo.Clear();
					}
					if (m_char_buffer != null)
					{
						if (m_char_buffer.Length != 0)
						{
							if (m_char_buffer[0] != 0)
							{
								m_currentFontAsset = m_fontAsset;
								m_currentMaterial = m_sharedMaterial;
								m_currentMaterialIndex = 0;
								m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
								m_currentSpriteAsset = m_spriteAsset;
								if (m_spriteAnimator != null)
								{
									m_spriteAnimator.StopAllAnimations();
								}
								int totalCharacterCount = m_totalCharacterCount;
								m_fontScale = m_fontSize / m_currentFontAsset.fontInfo.PointSize;
								float num = m_fontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
								float num2 = m_fontScale;
								m_fontScaleMultiplier = 1f;
								m_currentFontSize = m_fontSize;
								m_sizeStack.SetDefault(m_currentFontSize);
								float num3 = 0f;
								int num4 = 0;
								m_style = m_fontStyle;
								int fontWeightInternal;
								if ((m_style & FontStyles.Bold) == FontStyles.Bold)
								{
									fontWeightInternal = 700;
								}
								else
								{
									fontWeightInternal = m_fontWeight;
								}
								m_fontWeightInternal = fontWeightInternal;
								m_fontWeightStack.SetDefault(m_fontWeightInternal);
								m_fontStyleStack.Clear();
								m_lineJustification = m_textAlignment;
								m_lineJustificationStack.SetDefault(m_lineJustification);
								float num5 = 0f;
								float num6 = 0f;
								float num7 = 1f;
								m_baselineOffset = 0f;
								m_baselineOffsetStack.Clear();
								bool flag = false;
								Vector3 start = Vector3.zero;
								Vector3 zero = Vector3.zero;
								bool flag2 = false;
								Vector3 start2 = Vector3.zero;
								Vector3 zero2 = Vector3.zero;
								bool flag3 = false;
								Vector3 start3 = Vector3.zero;
								Vector3 vector = Vector3.zero;
								m_fontColor32 = m_fontColor;
								m_htmlColor = m_fontColor32;
								m_underlineColor = m_htmlColor;
								m_strikethroughColor = m_htmlColor;
								m_colorStack.SetDefault(m_htmlColor);
								m_underlineColorStack.SetDefault(m_htmlColor);
								m_strikethroughColorStack.SetDefault(m_htmlColor);
								m_highlightColorStack.SetDefault(m_htmlColor);
								m_actionStack.Clear();
								m_isFXMatrixSet = false;
								m_lineOffset = 0f;
								m_lineHeight = -32767f;
								float num8 = m_currentFontAsset.fontInfo.LineHeight - (m_currentFontAsset.fontInfo.Ascender - m_currentFontAsset.fontInfo.Descender);
								m_cSpacing = 0f;
								m_monoSpacing = 0f;
								float num9 = 0f;
								m_xAdvance = 0f;
								tag_LineIndent = 0f;
								tag_Indent = 0f;
								m_indentStack.SetDefault(0f);
								tag_NoParsing = false;
								m_characterCount = 0;
								m_firstCharacterOfLine = 0;
								m_lastCharacterOfLine = 0;
								m_firstVisibleCharacterOfLine = 0;
								m_lastVisibleCharacterOfLine = 0;
								m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
								m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
								m_lineNumber = 0;
								m_lineVisibleCharacterCount = 0;
								bool flag4 = true;
								m_firstOverflowCharacterIndex = -1;
								m_pageNumber = 0;
								int num10 = Mathf.Clamp(m_pageToDisplay - 1, 0, m_textInfo.pageInfo.Length - 1);
								int num11 = 0;
								int num12 = 0;
								Vector4 margin = m_margin;
								float marginWidth = m_marginWidth;
								float marginHeight = m_marginHeight;
								m_marginLeft = 0f;
								m_marginRight = 0f;
								m_width = -1f;
								float num13 = marginWidth + 0.0001f - m_marginLeft - m_marginRight;
								m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
								m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
								m_textInfo.ClearLineInfo();
								m_maxCapHeight = 0f;
								m_maxAscender = 0f;
								m_maxDescender = 0f;
								float num14 = 0f;
								float num15 = 0f;
								bool flag5 = false;
								m_isNewPage = false;
								bool flag6 = true;
								m_isNonBreakingSpace = false;
								bool flag7 = false;
								bool flag8 = false;
								int num16 = 0;
								SaveWordWrappingState(ref m_SavedWordWrapState, -1, -1);
								SaveWordWrappingState(ref m_SavedLineState, -1, -1);
								loopCountA++;
								int endIndex = 0;
								Vector3 vector2 = default(Vector3);
								Vector3 vector3 = default(Vector3);
								Vector3 vector4 = default(Vector3);
								Vector3 vector5 = default(Vector3);
								for (int i = 0; i < m_char_buffer.Length; i++)
								{
									if (m_char_buffer[i] == 0)
									{
										break;
									}
									num4 = m_char_buffer[i];
									m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
									m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
									m_currentFontAsset = m_materialReferences[m_currentMaterialIndex].fontAsset;
									int currentMaterialIndex = m_currentMaterialIndex;
									if (m_isRichText)
									{
										if (num4 == 60)
										{
											m_isParsingText = true;
											m_textElementType = TMP_TextElementType.Character;
											if (ValidateHtmlTag(m_char_buffer, i + 1, out endIndex))
											{
												i = endIndex;
												if (m_textElementType == TMP_TextElementType.Character)
												{
													continue;
												}
											}
										}
									}
									m_isParsingText = false;
									bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
									if (m_characterCount < m_firstVisibleCharacter)
									{
										m_textInfo.characterInfo[m_characterCount].isVisible = false;
										m_textInfo.characterInfo[m_characterCount].character = '\u200b';
										m_characterCount++;
										continue;
									}
									float num17 = 1f;
									if (m_textElementType == TMP_TextElementType.Character)
									{
										if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
										{
											if (char.IsLower((char)num4))
											{
												num4 = char.ToUpper((char)num4);
											}
										}
										else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
										{
											if (char.IsUpper((char)num4))
											{
												num4 = char.ToLower((char)num4);
											}
										}
										else
										{
											if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
											{
												if ((m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
												{
													goto IL_076d;
												}
											}
											if (char.IsLower((char)num4))
											{
												num17 = 0.8f;
												num4 = char.ToUpper((char)num4);
											}
										}
									}
									goto IL_076d;
									IL_340d:
									if (!m_enableWordWrapping)
									{
										if (m_overflowMode != TextOverflowModes.Truncate)
										{
											if (m_overflowMode != TextOverflowModes.Ellipsis)
											{
												goto IL_36d7;
											}
										}
									}
									if (!char.IsWhiteSpace((char)num4))
									{
										if (num4 != 8203)
										{
											if (num4 != 45)
											{
												if (num4 != 173)
												{
													goto IL_3506;
												}
											}
										}
									}
									if (m_isNonBreakingSpace)
									{
										if (!flag7)
										{
											goto IL_3506;
										}
									}
									if (num4 != 160)
									{
										if (num4 != 8209)
										{
											if (num4 != 8239 && num4 != 8288)
											{
												SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
												m_isCharacterWrappingEnabled = false;
												flag6 = false;
												goto IL_36d7;
											}
										}
									}
									goto IL_3506;
									IL_2a3e:
									m_textInfo.characterInfo[m_characterCount].xAdvance = m_xAdvance;
									if (num4 == 13)
									{
										m_xAdvance = tag_Indent;
									}
									if (num4 != 10)
									{
										if (m_characterCount != totalCharacterCount - 1)
										{
											goto IL_30e2;
										}
									}
									float num19;
									if (m_lineNumber > 0)
									{
										if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f)
										{
											if (!m_isNewPage)
											{
												float num18 = m_maxLineAscender - m_startOfLineAscender;
												AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num18);
												num19 -= num18;
												m_lineOffset += num18;
											}
										}
									}
									m_isNewPage = false;
									float num20 = m_maxLineAscender - m_lineOffset;
									float num21 = m_maxLineDescender - m_lineOffset;
									float maxDescender;
									if (m_maxDescender < num21)
									{
										maxDescender = m_maxDescender;
									}
									else
									{
										maxDescender = num21;
									}
									m_maxDescender = maxDescender;
									if (!flag5)
									{
										num15 = m_maxDescender;
									}
									if (m_useMaxVisibleDescender)
									{
										if (m_characterCount >= m_maxVisibleCharacters || m_lineNumber >= m_maxVisibleLines)
										{
											flag5 = true;
										}
									}
									m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
									int num22;
									if (m_firstCharacterOfLine > m_firstVisibleCharacterOfLine)
									{
										num22 = m_firstCharacterOfLine;
									}
									else
									{
										num22 = m_firstVisibleCharacterOfLine;
									}
									int firstVisibleCharacterIndex = num22;
									m_firstVisibleCharacterOfLine = num22;
									m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = firstVisibleCharacterIndex;
									m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = m_characterCount);
									int num23;
									if (m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine)
									{
										num23 = m_firstVisibleCharacterOfLine;
									}
									else
									{
										num23 = m_lastVisibleCharacterOfLine;
									}
									firstVisibleCharacterIndex = num23;
									m_lastVisibleCharacterOfLine = num23;
									m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = firstVisibleCharacterIndex;
									m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
									m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
									m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num21);
									m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num20);
									m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x - num5 * num2;
									m_textInfo.lineInfo[m_lineNumber].width = num13;
									if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
									{
										m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
									}
									if (m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].isVisible)
									{
										m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
									}
									else
									{
										m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
									}
									m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
									m_textInfo.lineInfo[m_lineNumber].ascender = num20;
									m_textInfo.lineInfo[m_lineNumber].descender = num21;
									m_textInfo.lineInfo[m_lineNumber].lineHeight = num20 - num21 + num8 * num;
									m_firstCharacterOfLine = m_characterCount + 1;
									m_lineVisibleCharacterCount = 0;
									float num24;
									if (num4 == 10)
									{
										SaveWordWrappingState(ref m_SavedLineState, i, m_characterCount);
										SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
										m_lineNumber++;
										flag4 = true;
										if (m_lineNumber >= m_textInfo.lineInfo.Length)
										{
											ResizeLineExtents(m_lineNumber);
										}
										if (m_lineHeight == -32767f)
										{
											num9 = 0f - m_maxLineDescender + num24 + (num8 + m_lineSpacing + m_paragraphSpacing + m_lineSpacingDelta) * num;
											m_lineOffset += num9;
										}
										else
										{
											m_lineOffset += m_lineHeight + (m_lineSpacing + m_paragraphSpacing) * num;
										}
										m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
										m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
										m_startOfLineAscender = num24;
										m_xAdvance = tag_LineIndent + tag_Indent;
										num12 = m_characterCount - 1;
										m_characterCount++;
										continue;
									}
									goto IL_30e2;
									IL_1967:
									m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
									int num25;
									if (m_firstCharacterOfLine > m_firstVisibleCharacterOfLine)
									{
										num25 = m_firstCharacterOfLine;
									}
									else
									{
										num25 = m_firstVisibleCharacterOfLine;
									}
									firstVisibleCharacterIndex = num25;
									m_firstVisibleCharacterOfLine = num25;
									m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = firstVisibleCharacterIndex;
									m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = ((m_characterCount - 1 > 0) ? (m_characterCount - 1) : 0));
									m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = (m_lastVisibleCharacterOfLine = ((m_lastVisibleCharacterOfLine >= m_firstVisibleCharacterOfLine) ? m_lastVisibleCharacterOfLine : m_firstVisibleCharacterOfLine));
									m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
									m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
									float num26;
									m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num26);
									float num27;
									m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num27);
									m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x;
									m_textInfo.lineInfo[m_lineNumber].width = num13;
									m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
									m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
									m_textInfo.lineInfo[m_lineNumber].ascender = num27;
									m_textInfo.lineInfo[m_lineNumber].descender = num26;
									m_textInfo.lineInfo[m_lineNumber].lineHeight = num27 - num26 + num8 * num;
									m_firstCharacterOfLine = m_characterCount;
									m_lineVisibleCharacterCount = 0;
									SaveWordWrappingState(ref m_SavedLineState, i, m_characterCount - 1);
									m_lineNumber++;
									flag4 = true;
									if (m_lineNumber >= m_textInfo.lineInfo.Length)
									{
										ResizeLineExtents(m_lineNumber);
									}
									if (m_lineHeight == -32767f)
									{
										float num28 = m_textInfo.characterInfo[m_characterCount].ascender - m_textInfo.characterInfo[m_characterCount].baseLine;
										num9 = 0f - m_maxLineDescender + num28 + (num8 + m_lineSpacing + m_lineSpacingDelta) * num;
										m_lineOffset += num9;
										m_startOfLineAscender = num28;
									}
									else
									{
										m_lineOffset += m_lineHeight + m_lineSpacing * num;
									}
									m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
									m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
									m_xAdvance = tag_Indent;
									continue;
									IL_36d7:
									m_characterCount++;
									continue;
									IL_076d:
									if (m_textElementType == TMP_TextElementType.Sprite)
									{
										m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
										m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
										TMP_Sprite tMP_Sprite = m_currentSpriteAsset.spriteInfoList[m_spriteIndex];
										if (tMP_Sprite == null)
										{
											continue;
										}
										if (num4 == 60)
										{
											num4 = 57344 + m_spriteIndex;
										}
										else
										{
											m_spriteColor = TMP_Text.s_colorWhite;
										}
										m_currentFontAsset = m_fontAsset;
										float num29 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
										num2 = m_fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * num29;
										m_cached_TextElement = tMP_Sprite;
										m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
										m_textInfo.characterInfo[m_characterCount].scale = num29;
										m_textInfo.characterInfo[m_characterCount].spriteAsset = m_currentSpriteAsset;
										m_textInfo.characterInfo[m_characterCount].fontAsset = m_currentFontAsset;
										m_textInfo.characterInfo[m_characterCount].materialReferenceIndex = m_currentMaterialIndex;
										m_currentMaterialIndex = currentMaterialIndex;
										num5 = 0f;
									}
									else if (m_textElementType == TMP_TextElementType.Character)
									{
										m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
										if (m_cached_TextElement == null)
										{
											continue;
										}
										m_currentFontAsset = m_textInfo.characterInfo[m_characterCount].fontAsset;
										m_currentMaterial = m_textInfo.characterInfo[m_characterCount].material;
										m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
										m_fontScale = m_currentFontSize * num17 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
										num2 = m_fontScale * m_fontScaleMultiplier * m_cached_TextElement.scale;
										m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
										m_textInfo.characterInfo[m_characterCount].scale = num2;
										float padding;
										if (m_currentMaterialIndex == 0)
										{
											padding = m_padding;
										}
										else
										{
											padding = m_subTextObjects[m_currentMaterialIndex].padding;
										}
										num5 = padding;
									}
									float num30 = num2;
									if (num4 == 173)
									{
										num2 = 0f;
									}
									if (m_isRightToLeft)
									{
										m_xAdvance -= ((m_cached_TextElement.xAdvance * num7 + m_characterSpacing + m_wordSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num4))
										{
											if (num4 != 8203)
											{
												goto IL_0b52;
											}
										}
										m_xAdvance -= m_wordSpacing * num2;
									}
									goto IL_0b52;
									IL_23ee:
									if (m_maxAscender - num19 > marginHeight + 0.0001f)
									{
										if (m_enableAutoSizing && m_lineSpacingDelta > m_lineSpacingMax)
										{
											if (m_lineNumber > 0)
											{
												loopCountA = 0;
												m_lineSpacingDelta -= 1f;
												GenerateTextMesh();
												return;
											}
										}
										if (m_enableAutoSizing)
										{
											if (m_fontSize > m_fontSizeMin)
											{
												while (true)
												{
													switch (3)
													{
													case 0:
														break;
													default:
														m_maxFontSize = m_fontSize;
														m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
														m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
														if (loopCountA <= 20)
														{
															GenerateTextMesh();
														}
														return;
													}
												}
											}
										}
										if (m_firstOverflowCharacterIndex == -1)
										{
											m_firstOverflowCharacterIndex = m_characterCount;
										}
										switch (m_overflowMode)
										{
										case TextOverflowModes.Overflow:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											break;
										case TextOverflowModes.Ellipsis:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											if (m_lineNumber > 0)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														m_char_buffer[m_textInfo.characterInfo[num12].index] = 8230;
														m_char_buffer[m_textInfo.characterInfo[num12].index + 1] = 0;
														if (m_cached_Ellipsis_GlyphInfo != null)
														{
															m_textInfo.characterInfo[num12].character = '…';
															m_textInfo.characterInfo[num12].textElement = m_cached_Ellipsis_GlyphInfo;
															m_textInfo.characterInfo[num12].fontAsset = m_materialReferences[0].fontAsset;
															m_textInfo.characterInfo[num12].material = m_materialReferences[0].material;
															m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
														}
														else
														{
															Debug.LogWarning(new StringBuilder().Append("Unable to use Ellipsis character since it wasn't found in the current Font Asset [").Append(m_fontAsset.name).Append("]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.").ToString(), this);
														}
														m_totalCharacterCount = num12 + 1;
														GenerateTextMesh();
														m_isTextTruncated = true;
														return;
													}
												}
											}
											ClearMesh();
											return;
										case TextOverflowModes.Masking:
											if (!m_isMaskingEnabled)
											{
												EnableMasking();
											}
											break;
										case TextOverflowModes.ScrollRect:
											if (!m_isMaskingEnabled)
											{
												EnableMasking();
											}
											break;
										case TextOverflowModes.Truncate:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											if (m_lineNumber > 0)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														break;
													default:
														m_char_buffer[m_textInfo.characterInfo[num12].index + 1] = 0;
														m_totalCharacterCount = num12 + 1;
														GenerateTextMesh();
														m_isTextTruncated = true;
														return;
													}
												}
											}
											ClearMesh();
											return;
										case TextOverflowModes.Page:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											if (num4 == 13)
											{
												break;
											}
											if (num4 == 10)
											{
												break;
											}
											if (i == 0)
											{
												while (true)
												{
													switch (6)
													{
													case 0:
														break;
													default:
														ClearMesh();
														return;
													}
												}
											}
											if (num11 == i)
											{
												m_char_buffer[i] = 0;
												m_isTextTruncated = true;
											}
											num11 = i;
											i = RestoreWordWrappingState(ref m_SavedLineState);
											m_isNewPage = true;
											m_xAdvance = tag_Indent;
											m_lineOffset = 0f;
											m_maxAscender = 0f;
											num14 = 0f;
											m_lineNumber++;
											m_pageNumber++;
											continue;
										case TextOverflowModes.Linked:
											if (m_linkedTextComponent != null)
											{
												m_linkedTextComponent.text = base.text;
												m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
												m_linkedTextComponent.ForceMeshUpdate();
											}
											if (m_lineNumber > 0)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														m_char_buffer[i] = 0;
														m_totalCharacterCount = m_characterCount;
														GenerateTextMesh();
														m_isTextTruncated = true;
														return;
													}
												}
											}
											ClearMesh();
											return;
										}
									}
									float num33;
									if (num4 == 9)
									{
										float num31 = m_currentFontAsset.fontInfo.TabWidth * num2;
										float num32 = Mathf.Ceil(m_xAdvance / num31) * num31;
										float xAdvance;
										if (num32 > m_xAdvance)
										{
											xAdvance = num32;
										}
										else
										{
											xAdvance = m_xAdvance + num31;
										}
										m_xAdvance = xAdvance;
									}
									else if (m_monoSpacing != 0f)
									{
										m_xAdvance += (m_monoSpacing - num33 + (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num4))
										{
											if (num4 != 8203)
											{
												goto IL_2a3e;
											}
										}
										m_xAdvance += m_wordSpacing * num2;
									}
									else if (!m_isRightToLeft)
									{
										m_xAdvance += ((m_cached_TextElement.xAdvance * num7 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num4))
										{
											if (num4 != 8203)
											{
												goto IL_2a3e;
											}
										}
										m_xAdvance += m_wordSpacing * num2;
									}
									goto IL_2a3e;
									IL_367f:
									SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
									m_isCharacterWrappingEnabled = false;
									flag6 = false;
									goto IL_36d7;
									IL_369f:
									if (!flag6)
									{
										if (!m_isCharacterWrappingEnabled)
										{
											if (!flag8)
											{
												goto IL_36d7;
											}
										}
									}
									SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
									goto IL_36d7;
									IL_17f1:
									i = RestoreWordWrappingState(ref m_SavedWordWrapState);
									num16 = i;
									if (m_char_buffer[i] == 173)
									{
										m_isTextTruncated = true;
										m_char_buffer[i] = 45;
										GenerateTextMesh();
										return;
									}
									if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
									{
										if (m_lineHeight == -32767f)
										{
											if (!m_isNewPage)
											{
												float num34 = m_maxLineAscender - m_startOfLineAscender;
												AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num34);
												m_lineOffset += num34;
												m_SavedWordWrapState.lineOffset = m_lineOffset;
												m_SavedWordWrapState.previousLineAscender = m_maxLineAscender;
											}
										}
									}
									m_isNewPage = false;
									num27 = m_maxLineAscender - m_lineOffset;
									num26 = m_maxLineDescender - m_lineOffset;
									float maxDescender2;
									if (m_maxDescender < num26)
									{
										maxDescender2 = m_maxDescender;
									}
									else
									{
										maxDescender2 = num26;
									}
									m_maxDescender = maxDescender2;
									if (!flag5)
									{
										num15 = m_maxDescender;
									}
									if (m_useMaxVisibleDescender)
									{
										if (m_characterCount < m_maxVisibleCharacters)
										{
											if (m_lineNumber < m_maxVisibleLines)
											{
												goto IL_1967;
											}
										}
										flag5 = true;
									}
									goto IL_1967;
									IL_23cd:
									m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
									goto IL_23ee;
									IL_1474:
									if (m_lineNumber != 0)
									{
										if (!m_isNewPage)
										{
											goto IL_14d5;
										}
									}
									float maxAscender;
									if (m_maxAscender > num24)
									{
										maxAscender = m_maxAscender;
									}
									else
									{
										maxAscender = num24;
									}
									m_maxAscender = maxAscender;
									m_maxCapHeight = Mathf.Max(m_maxCapHeight, m_currentFontAsset.fontInfo.CapHeight * num2);
									goto IL_14d5;
									IL_0e33:
									if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
									{
										float @float = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
										num6 = m_currentFontAsset.normalStyle / 4f * @float * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
										if (num6 + num5 > @float)
										{
											num5 = @float - num6;
										}
									}
									else
									{
										num6 = 0f;
									}
									num7 = 1f;
									goto IL_0eac;
									IL_10d3:
									if (m_isFXMatrixSet)
									{
										Vector3 b = (vector2 + vector3) / 2f;
										vector4 = m_FXMatrix.MultiplyPoint3x4(vector4 - b) + b;
										vector3 = m_FXMatrix.MultiplyPoint3x4(vector3 - b) + b;
										vector2 = m_FXMatrix.MultiplyPoint3x4(vector2 - b) + b;
										vector5 = m_FXMatrix.MultiplyPoint3x4(vector5 - b) + b;
									}
									m_textInfo.characterInfo[m_characterCount].bottomLeft = vector3;
									m_textInfo.characterInfo[m_characterCount].topLeft = vector4;
									m_textInfo.characterInfo[m_characterCount].topRight = vector2;
									m_textInfo.characterInfo[m_characterCount].bottomRight = vector5;
									m_textInfo.characterInfo[m_characterCount].origin = m_xAdvance;
									m_textInfo.characterInfo[m_characterCount].baseLine = 0f - m_lineOffset + m_baselineOffset;
									m_textInfo.characterInfo[m_characterCount].aspectRatio = (vector2.x - vector3.x) / (vector4.y - vector3.y);
									float ascender = m_currentFontAsset.fontInfo.Ascender;
									float num35;
									if (m_textElementType == TMP_TextElementType.Character)
									{
										num35 = num2;
									}
									else
									{
										num35 = m_textInfo.characterInfo[m_characterCount].scale;
									}
									num24 = ascender * num35 + m_baselineOffset;
									m_textInfo.characterInfo[m_characterCount].ascender = num24 - m_lineOffset;
									float maxLineAscender;
									if (num24 > m_maxLineAscender)
									{
										maxLineAscender = num24;
									}
									else
									{
										maxLineAscender = m_maxLineAscender;
									}
									m_maxLineAscender = maxLineAscender;
									float descender = m_currentFontAsset.fontInfo.Descender;
									float num36;
									if (m_textElementType == TMP_TextElementType.Character)
									{
										num36 = num2;
									}
									else
									{
										num36 = m_textInfo.characterInfo[m_characterCount].scale;
									}
									float num37 = descender * num36 + m_baselineOffset;
									num19 = (m_textInfo.characterInfo[m_characterCount].descender = num37 - m_lineOffset);
									float maxLineDescender;
									if (num37 < m_maxLineDescender)
									{
										maxLineDescender = num37;
									}
									else
									{
										maxLineDescender = m_maxLineDescender;
									}
									m_maxLineDescender = maxLineDescender;
									if ((m_style & FontStyles.Subscript) != FontStyles.Subscript)
									{
										if ((m_style & FontStyles.Superscript) != FontStyles.Superscript)
										{
											goto IL_1474;
										}
									}
									float num38 = (num24 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
									num24 = m_maxLineAscender;
									float maxLineAscender2;
									if (num38 > m_maxLineAscender)
									{
										maxLineAscender2 = num38;
									}
									else
									{
										maxLineAscender2 = m_maxLineAscender;
									}
									m_maxLineAscender = maxLineAscender2;
									float num39 = (num37 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
									num37 = m_maxLineDescender;
									float maxLineDescender2;
									if (num39 < m_maxLineDescender)
									{
										maxLineDescender2 = num39;
									}
									else
									{
										maxLineDescender2 = m_maxLineDescender;
									}
									m_maxLineDescender = maxLineDescender2;
									goto IL_1474;
									IL_0b52:
									m_textInfo.characterInfo[m_characterCount].character = (char)num4;
									m_textInfo.characterInfo[m_characterCount].pointSize = m_currentFontSize;
									m_textInfo.characterInfo[m_characterCount].color = m_htmlColor;
									m_textInfo.characterInfo[m_characterCount].underlineColor = m_underlineColor;
									m_textInfo.characterInfo[m_characterCount].strikethroughColor = m_strikethroughColor;
									m_textInfo.characterInfo[m_characterCount].highlightColor = m_highlightColor;
									m_textInfo.characterInfo[m_characterCount].style = m_style;
									m_textInfo.characterInfo[m_characterCount].index = (short)i;
									if (m_enableKerning)
									{
										if (m_characterCount >= 1)
										{
											int character = m_textInfo.characterInfo[m_characterCount - 1].character;
											KerningPairKey kerningPairKey = new KerningPairKey(character, num4);
											KerningPair value;
											m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out value);
											if (value != null)
											{
												m_xAdvance += value.XadvanceOffset * num2;
											}
										}
									}
									num33 = 0f;
									if (m_monoSpacing != 0f)
									{
										num33 = (m_monoSpacing / 2f - (m_cached_TextElement.width / 2f + m_cached_TextElement.xOffset) * num2) * (1f - m_charWidthAdjDelta);
										m_xAdvance += num33;
									}
									if (m_textElementType == TMP_TextElementType.Character)
									{
										if (!isUsingAlternateTypeface)
										{
											if ((m_style & FontStyles.Bold) != FontStyles.Bold)
											{
												if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
												{
													goto IL_0e33;
												}
											}
											if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
											{
												float float2 = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
												num6 = m_currentFontAsset.boldStyle / 4f * float2 * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
												if (num6 + num5 > float2)
												{
													num5 = float2 - num6;
												}
											}
											else
											{
												num6 = 0f;
											}
											num7 = 1f + m_currentFontAsset.boldSpacing * 0.01f;
											goto IL_0eac;
										}
									}
									goto IL_0e33;
									IL_3506:
									if (num4 > 4352)
									{
										if (num4 < 4607)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 11904)
									{
										if (num4 < 40959)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 43360)
									{
										if (num4 < 43391)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 44032)
									{
										if (num4 < 55295)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 63744)
									{
										if (num4 < 64255)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 65072)
									{
										if (num4 < 65103)
										{
											goto IL_35f7;
										}
									}
									if (num4 > 65280 && num4 < 65519)
									{
										goto IL_35f7;
									}
									goto IL_369f;
									IL_30e2:
									if (m_textInfo.characterInfo[m_characterCount].isVisible)
									{
										m_meshExtents.min.x = Mathf.Min(m_meshExtents.min.x, m_textInfo.characterInfo[m_characterCount].bottomLeft.x);
										m_meshExtents.min.y = Mathf.Min(m_meshExtents.min.y, m_textInfo.characterInfo[m_characterCount].bottomLeft.y);
										m_meshExtents.max.x = Mathf.Max(m_meshExtents.max.x, m_textInfo.characterInfo[m_characterCount].topRight.x);
										m_meshExtents.max.y = Mathf.Max(m_meshExtents.max.y, m_textInfo.characterInfo[m_characterCount].topRight.y);
									}
									if (m_overflowMode == TextOverflowModes.Page)
									{
										if (num4 != 13)
										{
											if (num4 != 10)
											{
												if (m_pageNumber + 1 > m_textInfo.pageInfo.Length)
												{
													TMP_TextInfo.Resize(ref m_textInfo.pageInfo, m_pageNumber + 1, true);
												}
												m_textInfo.pageInfo[m_pageNumber].ascender = num14;
												float descender2;
												if (num37 < m_textInfo.pageInfo[m_pageNumber].descender)
												{
													descender2 = num37;
												}
												else
												{
													descender2 = m_textInfo.pageInfo[m_pageNumber].descender;
												}
												m_textInfo.pageInfo[m_pageNumber].descender = descender2;
												if (m_pageNumber == 0 && m_characterCount == 0)
												{
													m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
												}
												else
												{
													if (m_characterCount > 0)
													{
														if (m_pageNumber != m_textInfo.characterInfo[m_characterCount - 1].pageNumber)
														{
															m_textInfo.pageInfo[m_pageNumber - 1].lastCharacterIndex = m_characterCount - 1;
															m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
															goto IL_340d;
														}
													}
													if (m_characterCount == totalCharacterCount - 1)
													{
														m_textInfo.pageInfo[m_pageNumber].lastCharacterIndex = m_characterCount;
													}
												}
											}
										}
									}
									goto IL_340d;
									IL_226a:
									if (m_lineNumber > 0)
									{
										if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f)
										{
											if (!m_isNewPage)
											{
												float num40 = m_maxLineAscender - m_startOfLineAscender;
												AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num40);
												num19 -= num40;
												m_lineOffset += num40;
												m_startOfLineAscender += num40;
												m_SavedWordWrapState.lineOffset = m_lineOffset;
												m_SavedWordWrapState.previousLineAscender = m_startOfLineAscender;
											}
										}
									}
									m_textInfo.characterInfo[m_characterCount].lineNumber = (short)m_lineNumber;
									m_textInfo.characterInfo[m_characterCount].pageNumber = (short)m_pageNumber;
									if (num4 != 10)
									{
										if (num4 != 13)
										{
											if (num4 != 8230)
											{
												goto IL_23cd;
											}
										}
									}
									if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
									{
										goto IL_23cd;
									}
									goto IL_23ee;
									IL_155b:
									m_textInfo.characterInfo[m_characterCount].isVisible = true;
									num13 = ((m_width == -1f) ? (marginWidth + 0.0001f - m_marginLeft - m_marginRight) : Mathf.Min(marginWidth + 0.0001f - m_marginLeft - m_marginRight, m_width));
									m_textInfo.lineInfo[m_lineNumber].marginLeft = m_marginLeft;
									int num41;
									if ((m_lineJustification & (TextAlignmentOptions)16) != (TextAlignmentOptions)16)
									{
										num41 = (((m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8) ? 1 : 0);
									}
									else
									{
										num41 = 1;
									}
									bool flag9 = (byte)num41 != 0;
									float num42 = Mathf.Abs(m_xAdvance);
									float num43 = (m_isRightToLeft ? 0f : m_cached_TextElement.xAdvance) * (1f - m_charWidthAdjDelta);
									float num44;
									if (num4 != 173)
									{
										num44 = num2;
									}
									else
									{
										num44 = num30;
									}
									if (num42 + num43 * num44 > num13 * ((!flag9) ? 1f : 1.05f))
									{
										num12 = m_characterCount - 1;
										if (base.enableWordWrapping)
										{
											if (m_characterCount != m_firstCharacterOfLine)
											{
												if (num16 != m_SavedWordWrapState.previous_WordBreak)
												{
													if (!flag6)
													{
														goto IL_17f1;
													}
												}
												if (m_enableAutoSizing)
												{
													if (m_fontSize > m_fontSizeMin)
													{
														while (true)
														{
															switch (1)
															{
															case 0:
																break;
															default:
																if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
																{
																	while (true)
																	{
																		switch (3)
																		{
																		case 0:
																			break;
																		default:
																			loopCountA = 0;
																			m_charWidthAdjDelta += 0.01f;
																			GenerateTextMesh();
																			return;
																		}
																	}
																}
																m_maxFontSize = m_fontSize;
																m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
																m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
																if (loopCountA > 20)
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
																GenerateTextMesh();
																return;
															}
														}
													}
												}
												if (!m_isCharacterWrappingEnabled)
												{
													if (!flag7)
													{
														flag7 = true;
													}
													else
													{
														m_isCharacterWrappingEnabled = true;
													}
												}
												else
												{
													flag8 = true;
												}
												goto IL_17f1;
											}
										}
										if (m_enableAutoSizing)
										{
											if (m_fontSize > m_fontSizeMin)
											{
												while (true)
												{
													switch (1)
													{
													case 0:
														break;
													default:
														if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
														{
															while (true)
															{
																switch (7)
																{
																case 0:
																	break;
																default:
																	loopCountA = 0;
																	m_charWidthAdjDelta += 0.01f;
																	GenerateTextMesh();
																	return;
																}
															}
														}
														m_maxFontSize = m_fontSize;
														m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
														m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
														if (loopCountA > 20)
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
														GenerateTextMesh();
														return;
													}
												}
											}
										}
										switch (m_overflowMode)
										{
										case TextOverflowModes.Overflow:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											break;
										case TextOverflowModes.Ellipsis:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											m_isTextTruncated = true;
											if (m_characterCount < 1)
											{
												m_textInfo.characterInfo[m_characterCount].isVisible = false;
												break;
											}
											m_char_buffer[i - 1] = 8230;
											m_char_buffer[i] = 0;
											if (m_cached_Ellipsis_GlyphInfo != null)
											{
												m_textInfo.characterInfo[num12].character = '…';
												m_textInfo.characterInfo[num12].textElement = m_cached_Ellipsis_GlyphInfo;
												m_textInfo.characterInfo[num12].fontAsset = m_materialReferences[0].fontAsset;
												m_textInfo.characterInfo[num12].material = m_materialReferences[0].material;
												m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
											}
											else
											{
												Debug.LogWarning(new StringBuilder().Append("Unable to use Ellipsis character since it wasn't found in the current Font Asset [").Append(m_fontAsset.name).Append("]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.").ToString(), this);
											}
											m_totalCharacterCount = num12 + 1;
											GenerateTextMesh();
											return;
										case TextOverflowModes.Masking:
											if (!m_isMaskingEnabled)
											{
												EnableMasking();
											}
											break;
										case TextOverflowModes.ScrollRect:
											if (!m_isMaskingEnabled)
											{
												EnableMasking();
											}
											break;
										case TextOverflowModes.Truncate:
											if (m_isMaskingEnabled)
											{
												DisableMasking();
											}
											m_textInfo.characterInfo[m_characterCount].isVisible = false;
											break;
										}
									}
									if (num4 != 9)
									{
										Color32 vertexColor;
										if (m_overrideHtmlColors)
										{
											vertexColor = m_fontColor32;
										}
										else
										{
											vertexColor = m_htmlColor;
										}
										if (m_textElementType == TMP_TextElementType.Character)
										{
											SaveGlyphVertexInfo(num5, num6, vertexColor);
										}
										else if (m_textElementType == TMP_TextElementType.Sprite)
										{
											SaveSpriteVertexInfo(vertexColor);
										}
									}
									else
									{
										m_textInfo.characterInfo[m_characterCount].isVisible = false;
										m_lastVisibleCharacterOfLine = m_characterCount;
										m_textInfo.lineInfo[m_lineNumber].spaceCount++;
										m_textInfo.spaceCount++;
									}
									if (m_textInfo.characterInfo[m_characterCount].isVisible)
									{
										if (num4 != 173)
										{
											if (flag4)
											{
												flag4 = false;
												m_firstVisibleCharacterOfLine = m_characterCount;
											}
											m_lineVisibleCharacterCount++;
											m_lastVisibleCharacterOfLine = m_characterCount;
										}
									}
									goto IL_226a;
									IL_0eac:
									float baseline = m_currentFontAsset.fontInfo.Baseline;
									vector4.x = m_xAdvance + (m_cached_TextElement.xOffset - num5 - num6) * num2 * (1f - m_charWidthAdjDelta);
									vector4.y = (baseline + m_cached_TextElement.yOffset + num5) * num2 - m_lineOffset + m_baselineOffset;
									vector4.z = 0f;
									vector3.x = vector4.x;
									vector3.y = vector4.y - (m_cached_TextElement.height + num5 * 2f) * num2;
									vector3.z = 0f;
									vector2.x = vector3.x + (m_cached_TextElement.width + num5 * 2f + num6 * 2f) * num2 * (1f - m_charWidthAdjDelta);
									vector2.y = vector4.y;
									vector2.z = 0f;
									vector5.x = vector2.x;
									vector5.y = vector3.y;
									vector5.z = 0f;
									if (m_textElementType == TMP_TextElementType.Character)
									{
										if (!isUsingAlternateTypeface)
										{
											if ((m_style & FontStyles.Italic) != FontStyles.Italic)
											{
												if ((m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
												{
													goto IL_10d3;
												}
											}
											float num45 = (float)(int)m_currentFontAsset.italicStyle * 0.01f;
											Vector3 vector6 = new Vector3(num45 * ((m_cached_TextElement.yOffset + num5 + num6) * num2), 0f, 0f);
											Vector3 vector7 = new Vector3(num45 * ((m_cached_TextElement.yOffset - m_cached_TextElement.height - num5 - num6) * num2), 0f, 0f);
											vector4 += vector6;
											vector3 += vector7;
											vector2 += vector6;
											vector5 += vector7;
										}
									}
									goto IL_10d3;
									IL_35f7:
									if (!m_isNonBreakingSpace)
									{
										if (!flag6)
										{
											if (!flag8)
											{
												if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num4))
												{
													if (m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_textInfo.characterInfo[m_characterCount + 1].character))
													{
														goto IL_367f;
													}
												}
												goto IL_36d7;
											}
										}
										goto IL_367f;
									}
									goto IL_369f;
									IL_14d5:
									if (m_lineOffset == 0f)
									{
										num14 = ((!(num14 > num24)) ? num24 : num14);
									}
									m_textInfo.characterInfo[m_characterCount].isVisible = false;
									if (num4 != 9)
									{
										if (!char.IsWhiteSpace((char)num4))
										{
											if (num4 != 8203)
											{
												goto IL_155b;
											}
										}
										if (m_textElementType != TMP_TextElementType.Sprite)
										{
											if (num4 != 10)
											{
												if (!char.IsSeparator((char)num4))
												{
													goto IL_226a;
												}
											}
											if (num4 != 173)
											{
												if (num4 != 8203)
												{
													if (num4 != 8288)
													{
														m_textInfo.lineInfo[m_lineNumber].spaceCount++;
														m_textInfo.spaceCount++;
													}
												}
											}
											goto IL_226a;
										}
									}
									goto IL_155b;
								}
								num3 = m_maxFontSize - m_minFontSize;
								if (!m_isCharacterWrappingEnabled)
								{
									if (m_enableAutoSizing)
									{
										if (num3 > 0.051f && m_fontSize < m_fontSizeMax)
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													break;
												default:
													m_minFontSize = m_fontSize;
													m_fontSize += Mathf.Max((m_maxFontSize - m_fontSize) / 2f, 0.05f);
													m_fontSize = (float)(int)(Mathf.Min(m_fontSize, m_fontSizeMax) * 20f + 0.5f) / 20f;
													if (loopCountA > 20)
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
													GenerateTextMesh();
													return;
												}
											}
										}
									}
								}
								m_isCharacterWrappingEnabled = false;
								if (m_characterCount == 0)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											ClearMesh();
											TMPro_EventManager.ON_TEXT_CHANGED(this);
											return;
										}
									}
								}
								int index = m_materialReferences[0].referenceCount * 4;
								m_textInfo.meshInfo[0].Clear(false);
								Vector3 a = Vector3.zero;
								Vector3[] rectTransformCorners = m_RectTransformCorners;
								TextAlignmentOptions textAlignment = m_textAlignment;
								switch (textAlignment)
								{
								default:
									if (textAlignment == TextAlignmentOptions.TopGeoAligned)
									{
										goto case TextAlignmentOptions.TopLeft;
									}
									if (textAlignment == TextAlignmentOptions.Flush || textAlignment == TextAlignmentOptions.CenterGeoAligned)
									{
										goto case TextAlignmentOptions.Left;
									}
									if (textAlignment != TextAlignmentOptions.BottomFlush)
									{
										if (textAlignment != TextAlignmentOptions.BottomGeoAligned)
										{
											if (textAlignment != TextAlignmentOptions.BaselineFlush)
											{
												if (textAlignment != TextAlignmentOptions.BaselineGeoAligned)
												{
													if (textAlignment != TextAlignmentOptions.MidlineFlush)
													{
														if (textAlignment != TextAlignmentOptions.MidlineGeoAligned)
														{
															if (textAlignment != TextAlignmentOptions.CaplineFlush && textAlignment != TextAlignmentOptions.CaplineGeoAligned)
															{
																break;
															}
															goto case TextAlignmentOptions.CaplineLeft;
														}
													}
													goto case TextAlignmentOptions.MidlineLeft;
												}
											}
											goto case TextAlignmentOptions.BaselineLeft;
										}
									}
									goto case TextAlignmentOptions.BottomLeft;
								case TextAlignmentOptions.TopLeft:
								case TextAlignmentOptions.Top:
								case TextAlignmentOptions.TopRight:
								case TextAlignmentOptions.TopJustified:
								case TextAlignmentOptions.TopFlush:
									if (m_overflowMode != TextOverflowModes.Page)
									{
										a = rectTransformCorners[1] + new Vector3(margin.x, 0f - m_maxAscender - margin.y, 0f);
									}
									else
									{
										a = rectTransformCorners[1] + new Vector3(margin.x, 0f - m_textInfo.pageInfo[num10].ascender - margin.y, 0f);
									}
									break;
								case TextAlignmentOptions.Left:
								case TextAlignmentOptions.Center:
								case TextAlignmentOptions.Right:
								case TextAlignmentOptions.Justified:
									if (m_overflowMode != TextOverflowModes.Page)
									{
										a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_maxAscender + margin.y + num15 - margin.w) / 2f, 0f);
									}
									else
									{
										a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_textInfo.pageInfo[num10].ascender + margin.y + m_textInfo.pageInfo[num10].descender - margin.w) / 2f, 0f);
									}
									break;
								case TextAlignmentOptions.BottomLeft:
								case TextAlignmentOptions.Bottom:
								case TextAlignmentOptions.BottomRight:
								case TextAlignmentOptions.BottomJustified:
									if (m_overflowMode != TextOverflowModes.Page)
									{
										a = rectTransformCorners[0] + new Vector3(margin.x, 0f - num15 + margin.w, 0f);
									}
									else
									{
										a = rectTransformCorners[0] + new Vector3(margin.x, 0f - m_textInfo.pageInfo[num10].descender + margin.w, 0f);
									}
									break;
								case TextAlignmentOptions.BaselineLeft:
								case TextAlignmentOptions.Baseline:
								case TextAlignmentOptions.BaselineRight:
								case TextAlignmentOptions.BaselineJustified:
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
									break;
								case TextAlignmentOptions.MidlineLeft:
								case TextAlignmentOptions.Midline:
								case TextAlignmentOptions.MidlineRight:
								case TextAlignmentOptions.MidlineJustified:
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_meshExtents.max.y + margin.y + m_meshExtents.min.y - margin.w) / 2f, 0f);
									break;
								case TextAlignmentOptions.CaplineLeft:
								case TextAlignmentOptions.Capline:
								case TextAlignmentOptions.CaplineRight:
								case TextAlignmentOptions.CaplineJustified:
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_maxCapHeight - margin.y - margin.w) / 2f, 0f);
									break;
								}
								Vector3 b2 = Vector3.zero;
								Vector3 zero3 = Vector3.zero;
								int index_X = 0;
								int index_X2 = 0;
								int num46 = 0;
								int num47 = 0;
								int num48 = 0;
								bool flag10 = false;
								bool flag11 = false;
								int num49 = 0;
								int num50 = 0;
								int num51;
								if (m_canvas.worldCamera == null)
								{
									num51 = 0;
								}
								else
								{
									num51 = 1;
								}
								bool flag12 = (byte)num51 != 0;
								Vector3 lossyScale = base.transform.lossyScale;
								float num52 = m_previousLossyScaleY = lossyScale.y;
								RenderMode renderMode = m_canvas.renderMode;
								float scaleFactor = m_canvas.scaleFactor;
								Color32 color = Color.white;
								Color32 underlineColor = Color.white;
								Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
								float num53 = 0f;
								float num54 = 0f;
								float num55 = 0f;
								float num56 = 0f;
								float num57 = TMP_Text.k_LargePositiveFloat;
								int num58 = 0;
								float num59 = 0f;
								float num60 = 0f;
								float b3 = 0f;
								TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
								int lineNumber;
								for (int j = 0; j < m_characterCount; num48 = lineNumber, j++)
								{
									TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
									char character2 = characterInfo[j].character;
									lineNumber = characterInfo[j].lineNumber;
									TMP_LineInfo tMP_LineInfo = m_textInfo.lineInfo[lineNumber];
									num47 = lineNumber + 1;
									TextAlignmentOptions alignment = tMP_LineInfo.alignment;
									switch (alignment)
									{
									default:
										if (alignment != TextAlignmentOptions.TopGeoAligned)
										{
											if (alignment == TextAlignmentOptions.Flush)
											{
												goto case TextAlignmentOptions.TopJustified;
											}
											if (alignment != TextAlignmentOptions.CenterGeoAligned)
											{
												if (alignment == TextAlignmentOptions.BottomFlush)
												{
													goto case TextAlignmentOptions.TopJustified;
												}
												if (alignment != TextAlignmentOptions.BottomGeoAligned)
												{
													if (alignment == TextAlignmentOptions.BaselineFlush)
													{
														goto case TextAlignmentOptions.TopJustified;
													}
													if (alignment != TextAlignmentOptions.BaselineGeoAligned)
													{
														if (alignment == TextAlignmentOptions.MidlineFlush)
														{
															goto case TextAlignmentOptions.TopJustified;
														}
														if (alignment != TextAlignmentOptions.MidlineGeoAligned)
														{
															if (alignment == TextAlignmentOptions.CaplineFlush)
															{
																goto case TextAlignmentOptions.TopJustified;
															}
															if (alignment != TextAlignmentOptions.CaplineGeoAligned)
															{
																break;
															}
														}
													}
												}
											}
										}
										b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - (tMP_LineInfo.lineExtents.min.x + tMP_LineInfo.lineExtents.max.x) / 2f, 0f, 0f);
										break;
									case TextAlignmentOptions.TopLeft:
									case TextAlignmentOptions.Left:
									case TextAlignmentOptions.BottomLeft:
									case TextAlignmentOptions.BaselineLeft:
									case TextAlignmentOptions.MidlineLeft:
									case TextAlignmentOptions.CaplineLeft:
										if (!m_isRightToLeft)
										{
											b2 = new Vector3(tMP_LineInfo.marginLeft, 0f, 0f);
										}
										else
										{
											b2 = new Vector3(0f - tMP_LineInfo.maxAdvance, 0f, 0f);
										}
										break;
									case TextAlignmentOptions.Top:
									case TextAlignmentOptions.Center:
									case TextAlignmentOptions.Bottom:
									case TextAlignmentOptions.Baseline:
									case TextAlignmentOptions.Midline:
									case TextAlignmentOptions.Capline:
										b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - tMP_LineInfo.maxAdvance / 2f, 0f, 0f);
										break;
									case TextAlignmentOptions.TopRight:
									case TextAlignmentOptions.Right:
									case TextAlignmentOptions.BottomRight:
									case TextAlignmentOptions.BaselineRight:
									case TextAlignmentOptions.MidlineRight:
									case TextAlignmentOptions.CaplineRight:
										if (!m_isRightToLeft)
										{
											b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width - tMP_LineInfo.maxAdvance, 0f, 0f);
										}
										else
										{
											b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
										}
										break;
									case TextAlignmentOptions.TopJustified:
									case TextAlignmentOptions.TopFlush:
									case TextAlignmentOptions.Justified:
									case TextAlignmentOptions.BottomJustified:
									case TextAlignmentOptions.BaselineJustified:
									case TextAlignmentOptions.MidlineJustified:
									case TextAlignmentOptions.CaplineJustified:
										{
											if (character2 == '­')
											{
												break;
											}
											if (character2 == '\u200b')
											{
												break;
											}
											if (character2 == '\u2060')
											{
												break;
											}
											char character3 = characterInfo[tMP_LineInfo.lastCharacterIndex].character;
											bool flag13 = (alignment & (TextAlignmentOptions)16) == (TextAlignmentOptions)16;
											if (!char.IsControl(character3))
											{
												if (lineNumber < m_lineNumber)
												{
													goto IL_4337;
												}
											}
											if (!flag13)
											{
												if (!(tMP_LineInfo.maxAdvance > tMP_LineInfo.width))
												{
													if (!m_isRightToLeft)
													{
														b2 = new Vector3(tMP_LineInfo.marginLeft, 0f, 0f);
													}
													else
													{
														b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
													}
													break;
												}
											}
											goto IL_4337;
										}
										IL_4337:
										if (lineNumber == num48)
										{
											if (j != 0)
											{
												if (j != m_firstVisibleCharacter)
												{
													float num61;
													if (!m_isRightToLeft)
													{
														num61 = tMP_LineInfo.width - tMP_LineInfo.maxAdvance;
													}
													else
													{
														num61 = tMP_LineInfo.width + tMP_LineInfo.maxAdvance;
													}
													float num62 = num61;
													int num63 = tMP_LineInfo.visibleCharacterCount - 1;
													int num64;
													if (characterInfo[tMP_LineInfo.lastCharacterIndex].isVisible)
													{
														num64 = tMP_LineInfo.spaceCount;
													}
													else
													{
														num64 = tMP_LineInfo.spaceCount - 1;
													}
													int num65 = num64;
													if (flag10)
													{
														num65--;
														num63++;
													}
													float num66 = (num65 <= 0) ? 1f : m_wordWrappingRatios;
													if (num65 < 1)
													{
														num65 = 1;
													}
													if (character2 != '\t')
													{
														if (!char.IsSeparator(character2))
														{
															if (!m_isRightToLeft)
															{
																b2 += new Vector3(num62 * num66 / (float)num63, 0f, 0f);
															}
															else
															{
																b2 -= new Vector3(num62 * num66 / (float)num63, 0f, 0f);
															}
															break;
														}
													}
													if (!m_isRightToLeft)
													{
														b2 += new Vector3(num62 * (1f - num66) / (float)num65, 0f, 0f);
													}
													else
													{
														b2 -= new Vector3(num62 * (1f - num66) / (float)num65, 0f, 0f);
													}
													break;
												}
											}
										}
										if (m_isRightToLeft)
										{
											b2 = new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
										}
										else
										{
											b2 = new Vector3(tMP_LineInfo.marginLeft, 0f, 0f);
										}
										if (char.IsSeparator(character2))
										{
											flag10 = true;
										}
										else
										{
											flag10 = false;
										}
										break;
									}
									zero3 = a + b2;
									bool isVisible = characterInfo[j].isVisible;
									TMP_TextElementType elementType;
									if (isVisible)
									{
										elementType = characterInfo[j].elementType;
										switch (elementType)
										{
										case TMP_TextElementType.Character:
										{
											Extents lineExtents = tMP_LineInfo.lineExtents;
											float num67 = m_uvLineOffset * (float)lineNumber % 1f;
											switch (m_horizontalMapping)
											{
											case TextureMappingOptions.Character:
												characterInfo[j].vertex_BL.uv2.x = 0f;
												characterInfo[j].vertex_TL.uv2.x = 0f;
												characterInfo[j].vertex_TR.uv2.x = 1f;
												characterInfo[j].vertex_BR.uv2.x = 1f;
												break;
											case TextureMappingOptions.Line:
												if (m_textAlignment != TextAlignmentOptions.Justified)
												{
													characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num67;
													characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num67;
													characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num67;
													characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num67;
												}
												else
												{
													characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
													characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
													characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
													characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
												}
												break;
											case TextureMappingOptions.Paragraph:
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num67;
												break;
											case TextureMappingOptions.MatchAspect:
											{
												switch (m_verticalMapping)
												{
												case TextureMappingOptions.Character:
													characterInfo[j].vertex_BL.uv2.y = 0f;
													characterInfo[j].vertex_TL.uv2.y = 1f;
													characterInfo[j].vertex_TR.uv2.y = 0f;
													characterInfo[j].vertex_BR.uv2.y = 1f;
													break;
												case TextureMappingOptions.Line:
													characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num67;
													characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num67;
													characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
													characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
													break;
												case TextureMappingOptions.Paragraph:
													characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num67;
													characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num67;
													characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
													characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
													break;
												case TextureMappingOptions.MatchAspect:
													Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
													break;
												}
												float num68 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
												characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num68 + num67;
												characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
												characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num68 + num67;
												characterInfo[j].vertex_BR.uv2.x = characterInfo[j].vertex_TR.uv2.x;
												break;
											}
											}
											switch (m_verticalMapping)
											{
											case TextureMappingOptions.Character:
												characterInfo[j].vertex_BL.uv2.y = 0f;
												characterInfo[j].vertex_TL.uv2.y = 1f;
												characterInfo[j].vertex_TR.uv2.y = 1f;
												characterInfo[j].vertex_BR.uv2.y = 0f;
												break;
											case TextureMappingOptions.Line:
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												break;
											case TextureMappingOptions.Paragraph:
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												break;
											case TextureMappingOptions.MatchAspect:
											{
												float num69 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
												characterInfo[j].vertex_BL.uv2.y = num69 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
												characterInfo[j].vertex_TL.uv2.y = num69 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											}
											}
											num53 = characterInfo[j].scale * (1f - m_charWidthAdjDelta);
											if (!characterInfo[j].isUsingAlternateTypeface)
											{
												if ((characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
												{
													num53 *= -1f;
												}
											}
											if (renderMode != 0)
											{
												if (renderMode != RenderMode.ScreenSpaceCamera)
												{
													if (renderMode == RenderMode.WorldSpace)
													{
														num53 *= num52;
													}
												}
												else
												{
													float num70 = num53;
													float num71;
													if (flag12)
													{
														num71 = num52;
													}
													else
													{
														num71 = 1f;
													}
													num53 = num70 * num71;
												}
											}
											else
											{
												num53 *= num52 / scaleFactor;
											}
											float x = characterInfo[j].vertex_BL.uv2.x;
											float y = characterInfo[j].vertex_BL.uv2.y;
											float x2 = characterInfo[j].vertex_TR.uv2.x;
											float y2 = characterInfo[j].vertex_TR.uv2.y;
											float num72 = (int)x;
											float num73 = (int)y;
											x -= num72;
											x2 -= num72;
											y -= num73;
											y2 -= num73;
											characterInfo[j].vertex_BL.uv2.x = PackUV(x, y);
											characterInfo[j].vertex_BL.uv2.y = num53;
											characterInfo[j].vertex_TL.uv2.x = PackUV(x, y2);
											characterInfo[j].vertex_TL.uv2.y = num53;
											characterInfo[j].vertex_TR.uv2.x = PackUV(x2, y2);
											characterInfo[j].vertex_TR.uv2.y = num53;
											characterInfo[j].vertex_BR.uv2.x = PackUV(x2, y);
											characterInfo[j].vertex_BR.uv2.y = num53;
											break;
										}
										}
										if (j < m_maxVisibleCharacters)
										{
											if (num46 < m_maxVisibleWords)
											{
												if (lineNumber < m_maxVisibleLines)
												{
													if (m_overflowMode != TextOverflowModes.Page)
													{
														characterInfo[j].vertex_BL.position += zero3;
														characterInfo[j].vertex_TL.position += zero3;
														characterInfo[j].vertex_TR.position += zero3;
														characterInfo[j].vertex_BR.position += zero3;
														goto IL_586c;
													}
												}
											}
										}
										if (j < m_maxVisibleCharacters)
										{
											if (num46 < m_maxVisibleWords)
											{
												if (lineNumber < m_maxVisibleLines)
												{
													if (m_overflowMode == TextOverflowModes.Page)
													{
														if (characterInfo[j].pageNumber == num10)
														{
															characterInfo[j].vertex_BL.position += zero3;
															characterInfo[j].vertex_TL.position += zero3;
															characterInfo[j].vertex_TR.position += zero3;
															characterInfo[j].vertex_BR.position += zero3;
															goto IL_586c;
														}
													}
												}
											}
										}
										characterInfo[j].vertex_BL.position = Vector3.zero;
										characterInfo[j].vertex_TL.position = Vector3.zero;
										characterInfo[j].vertex_TR.position = Vector3.zero;
										characterInfo[j].vertex_BR.position = Vector3.zero;
										characterInfo[j].isVisible = false;
										goto IL_586c;
									}
									goto IL_5895;
									IL_5f5e:
									int num74;
									if (j == m_characterCount - 1)
									{
										if (char.IsLetterOrDigit(character2))
										{
											num74 = j;
											goto IL_5f87;
										}
									}
									num74 = j - 1;
									goto IL_5f87;
									IL_643a:
									bool flag14;
									if (flag)
									{
										if (!flag14)
										{
											flag = false;
											zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num57, 0f);
											num55 = m_textInfo.characterInfo[j - 1].scale;
											DrawUnderlineMesh(start, zero, ref index, num54, num55, num56, num53, color);
											num56 = 0f;
											num57 = TMP_Text.k_LargePositiveFloat;
											goto IL_65f8;
										}
									}
									if (flag && j < m_characterCount - 1 && !color.Compare(m_textInfo.characterInfo[j + 1].underlineColor))
									{
										flag = false;
										zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num57, 0f);
										num55 = m_textInfo.characterInfo[j].scale;
										DrawUnderlineMesh(start, zero, ref index, num54, num55, num56, num53, color);
										num56 = 0f;
										num57 = TMP_Text.k_LargePositiveFloat;
									}
									goto IL_65f8;
									IL_6bfe:
									bool flag15;
									if ((m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
									{
										flag15 = true;
										int pageNumber = m_textInfo.characterInfo[j].pageNumber;
										if (j <= m_maxVisibleCharacters && lineNumber <= m_maxVisibleLines)
										{
											if (m_overflowMode != TextOverflowModes.Page || pageNumber + 1 == m_pageToDisplay)
											{
												goto IL_6c7d;
											}
										}
										flag15 = false;
										goto IL_6c7d;
									}
									if (flag3)
									{
										flag3 = false;
										DrawTextHighlight(start3, vector, ref index, color2);
									}
									continue;
									IL_65f8:
									bool flag16 = (m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
									float strikethrough = fontAsset.fontInfo.strikethrough;
									bool flag17;
									if (flag16)
									{
										flag17 = true;
										if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines)
										{
											goto IL_6696;
										}
										if (m_overflowMode == TextOverflowModes.Page)
										{
											if (m_textInfo.characterInfo[j].pageNumber + 1 != m_pageToDisplay)
											{
												goto IL_6696;
											}
										}
										goto IL_6699;
									}
									if (flag2)
									{
										flag2 = false;
										zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num60, 0f);
										DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
									}
									goto IL_6bfe;
									IL_6109:
									flag14 = false;
									goto IL_610c;
									IL_586c:
									if (elementType == TMP_TextElementType.Character)
									{
										FillCharacterVertexBuffers(j, index_X);
									}
									else if (elementType == TMP_TextElementType.Sprite)
									{
										FillSpriteVertexBuffers(j, index_X2);
									}
									goto IL_5895;
									IL_6c7d:
									if (!flag3)
									{
										if (flag15)
										{
											if (j <= tMP_LineInfo.lastVisibleCharacterIndex)
											{
												if (character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
												{
													flag3 = true;
													start3 = TMP_Text.k_LargePositiveVector2;
													vector = TMP_Text.k_LargeNegativeVector2;
													color2 = m_textInfo.characterInfo[j].highlightColor;
												}
											}
										}
									}
									if (flag3)
									{
										Color32 highlightColor = m_textInfo.characterInfo[j].highlightColor;
										bool flag18 = false;
										if (!color2.Compare(highlightColor))
										{
											vector.x = (vector.x + m_textInfo.characterInfo[j].bottomLeft.x) / 2f;
											start3.y = Mathf.Min(start3.y, m_textInfo.characterInfo[j].descender);
											vector.y = Mathf.Max(vector.y, m_textInfo.characterInfo[j].ascender);
											DrawTextHighlight(start3, vector, ref index, color2);
											flag3 = true;
											start3 = vector;
											vector = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].descender, 0f);
											color2 = m_textInfo.characterInfo[j].highlightColor;
											flag18 = true;
										}
										if (!flag18)
										{
											start3.x = Mathf.Min(start3.x, m_textInfo.characterInfo[j].bottomLeft.x);
											start3.y = Mathf.Min(start3.y, m_textInfo.characterInfo[j].descender);
											vector.x = Mathf.Max(vector.x, m_textInfo.characterInfo[j].topRight.x);
											vector.y = Mathf.Max(vector.y, m_textInfo.characterInfo[j].ascender);
										}
									}
									if (flag3)
									{
										if (m_characterCount == 1)
										{
											flag3 = false;
											DrawTextHighlight(start3, vector, ref index, color2);
											continue;
										}
									}
									if (flag3)
									{
										if (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex)
										{
											flag3 = false;
											DrawTextHighlight(start3, vector, ref index, color2);
											continue;
										}
									}
									if (!flag3)
									{
										continue;
									}
									if (!flag15)
									{
										flag3 = false;
										DrawTextHighlight(start3, vector, ref index, color2);
									}
									continue;
									IL_6298:
									if (flag)
									{
										if (m_characterCount == 1)
										{
											flag = false;
											zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num57, 0f);
											num55 = m_textInfo.characterInfo[j].scale;
											DrawUnderlineMesh(start, zero, ref index, num54, num55, num56, num53, color);
											num56 = 0f;
											num57 = TMP_Text.k_LargePositiveFloat;
											goto IL_65f8;
										}
									}
									if (flag)
									{
										if (j != tMP_LineInfo.lastCharacterIndex)
										{
											if (j < tMP_LineInfo.lastVisibleCharacterIndex)
											{
												goto IL_643a;
											}
										}
										if (!char.IsWhiteSpace(character2))
										{
											if (character2 != '\u200b')
											{
												zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num57, 0f);
												num55 = m_textInfo.characterInfo[j].scale;
												goto IL_640e;
											}
										}
										int lastVisibleCharacterIndex = tMP_LineInfo.lastVisibleCharacterIndex;
										zero = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num57, 0f);
										num55 = m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
										goto IL_640e;
									}
									goto IL_643a;
									IL_610c:
									int pageNumber2;
									if (!char.IsWhiteSpace(character2))
									{
										if (character2 != '\u200b')
										{
											num56 = Mathf.Max(num56, m_textInfo.characterInfo[j].scale);
											num57 = Mathf.Min((pageNumber2 != num58) ? TMP_Text.k_LargePositiveFloat : num57, m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num56);
											num58 = pageNumber2;
										}
									}
									if (!flag)
									{
										if (flag14)
										{
											if (j <= tMP_LineInfo.lastVisibleCharacterIndex)
											{
												if (character2 != '\n' && character2 != '\r')
												{
													if (j == tMP_LineInfo.lastVisibleCharacterIndex)
													{
														if (char.IsSeparator(character2))
														{
															goto IL_6298;
														}
													}
													flag = true;
													num54 = m_textInfo.characterInfo[j].scale;
													if (num56 == 0f)
													{
														num56 = num54;
													}
													start = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, num57, 0f);
													color = m_textInfo.characterInfo[j].underlineColor;
												}
											}
										}
									}
									goto IL_6298;
									IL_6699:
									if (!flag2)
									{
										if (flag17 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n')
										{
											if (character2 != '\r')
											{
												if (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
												{
													flag2 = true;
													num59 = m_textInfo.characterInfo[j].pointSize;
													num60 = m_textInfo.characterInfo[j].scale;
													start2 = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num60, 0f);
													underlineColor = m_textInfo.characterInfo[j].strikethroughColor;
													b3 = m_textInfo.characterInfo[j].baseLine;
												}
											}
										}
									}
									if (flag2)
									{
										if (m_characterCount == 1)
										{
											flag2 = false;
											zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num60, 0f);
											DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
											goto IL_6bfe;
										}
									}
									if (flag2)
									{
										if (j == tMP_LineInfo.lastCharacterIndex)
										{
											if (!char.IsWhiteSpace(character2))
											{
												if (character2 != '\u200b')
												{
													zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num60, 0f);
													goto IL_690f;
												}
											}
											int lastVisibleCharacterIndex2 = tMP_LineInfo.lastVisibleCharacterIndex;
											zero2 = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num60, 0f);
											goto IL_690f;
										}
									}
									if (flag2)
									{
										if (j < m_characterCount)
										{
											if (m_textInfo.characterInfo[j + 1].pointSize != num59 || !TMP_Math.Approximately(m_textInfo.characterInfo[j + 1].baseLine + zero3.y, b3))
											{
												flag2 = false;
												int lastVisibleCharacterIndex3 = tMP_LineInfo.lastVisibleCharacterIndex;
												if (j <= lastVisibleCharacterIndex3)
												{
													zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num60, 0f);
												}
												else
												{
													zero2 = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num60, 0f);
												}
												DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
												goto IL_6bfe;
											}
										}
									}
									if (flag2)
									{
										if (j < m_characterCount)
										{
											if (fontAsset.GetInstanceID() != characterInfo[j + 1].fontAsset.GetInstanceID())
											{
												flag2 = false;
												zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num60, 0f);
												DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
												goto IL_6bfe;
											}
										}
									}
									if (flag2 && !flag17)
									{
										flag2 = false;
										zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num60, 0f);
										DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
									}
									goto IL_6bfe;
									IL_5895:
									m_textInfo.characterInfo[j].bottomLeft += zero3;
									m_textInfo.characterInfo[j].topLeft += zero3;
									m_textInfo.characterInfo[j].topRight += zero3;
									m_textInfo.characterInfo[j].bottomRight += zero3;
									m_textInfo.characterInfo[j].origin += zero3.x;
									m_textInfo.characterInfo[j].xAdvance += zero3.x;
									m_textInfo.characterInfo[j].ascender += zero3.y;
									m_textInfo.characterInfo[j].descender += zero3.y;
									m_textInfo.characterInfo[j].baseLine += zero3.y;
									if (isVisible)
									{
									}
									if (lineNumber == num48)
									{
										if (j != m_characterCount - 1)
										{
											goto IL_5cc7;
										}
									}
									if (lineNumber != num48)
									{
										m_textInfo.lineInfo[num48].baseline += zero3.y;
										m_textInfo.lineInfo[num48].ascender += zero3.y;
										m_textInfo.lineInfo[num48].descender += zero3.y;
										m_textInfo.lineInfo[num48].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num48].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[num48].descender);
										m_textInfo.lineInfo[num48].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num48].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[num48].ascender);
									}
									if (j == m_characterCount - 1)
									{
										m_textInfo.lineInfo[lineNumber].baseline += zero3.y;
										m_textInfo.lineInfo[lineNumber].ascender += zero3.y;
										m_textInfo.lineInfo[lineNumber].descender += zero3.y;
										m_textInfo.lineInfo[lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[lineNumber].descender);
										m_textInfo.lineInfo[lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[lineNumber].ascender);
									}
									goto IL_5cc7;
									IL_5f87:
									num50 = num74;
									flag11 = false;
									int num75 = m_textInfo.wordInfo.Length;
									int wordCount = m_textInfo.wordCount;
									if (m_textInfo.wordCount + 1 > num75)
									{
										TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num75 + 1);
									}
									m_textInfo.wordInfo[wordCount].firstCharacterIndex = num49;
									m_textInfo.wordInfo[wordCount].lastCharacterIndex = num50;
									m_textInfo.wordInfo[wordCount].characterCount = num50 - num49 + 1;
									m_textInfo.wordInfo[wordCount].textComponent = this;
									num46++;
									m_textInfo.wordCount++;
									m_textInfo.lineInfo[lineNumber].wordCount++;
									goto IL_6077;
									IL_5cc7:
									if (!char.IsLetterOrDigit(character2))
									{
										if (character2 != '-')
										{
											if (character2 != '­')
											{
												if (character2 != '‐')
												{
													if (character2 != '‑')
													{
														if (!flag11)
														{
															if (j != 0)
															{
																goto IL_6077;
															}
															if (char.IsPunctuation(character2))
															{
																if (!char.IsWhiteSpace(character2))
																{
																	if (character2 != '\u200b')
																	{
																		if (j != m_characterCount - 1)
																		{
																			goto IL_6077;
																		}
																	}
																}
															}
														}
														if (j > 0 && j < characterInfo.Length - 1)
														{
															if (j < m_characterCount)
															{
																if (character2 != '\'')
																{
																	if (character2 != '’')
																	{
																		goto IL_5f5e;
																	}
																}
																if (char.IsLetterOrDigit(characterInfo[j - 1].character))
																{
																	if (char.IsLetterOrDigit(characterInfo[j + 1].character))
																	{
																		goto IL_6077;
																	}
																}
															}
														}
														goto IL_5f5e;
													}
												}
											}
										}
									}
									if (!flag11)
									{
										flag11 = true;
										num49 = j;
									}
									if (flag11)
									{
										if (j == m_characterCount - 1)
										{
											int num76 = m_textInfo.wordInfo.Length;
											int wordCount2 = m_textInfo.wordCount;
											if (m_textInfo.wordCount + 1 > num76)
											{
												TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num76 + 1);
											}
											num50 = j;
											m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num49;
											m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num50;
											m_textInfo.wordInfo[wordCount2].characterCount = num50 - num49 + 1;
											m_textInfo.wordInfo[wordCount2].textComponent = this;
											num46++;
											m_textInfo.wordCount++;
											m_textInfo.lineInfo[lineNumber].wordCount++;
										}
									}
									goto IL_6077;
									IL_640e:
									flag = false;
									DrawUnderlineMesh(start, zero, ref index, num54, num55, num56, num53, color);
									num56 = 0f;
									num57 = TMP_Text.k_LargePositiveFloat;
									goto IL_65f8;
									IL_6077:
									if ((m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
									{
										flag14 = true;
										pageNumber2 = m_textInfo.characterInfo[j].pageNumber;
										if (j <= m_maxVisibleCharacters)
										{
											if (lineNumber <= m_maxVisibleLines)
											{
												if (m_overflowMode == TextOverflowModes.Page)
												{
													if (pageNumber2 + 1 != m_pageToDisplay)
													{
														goto IL_6109;
													}
												}
												goto IL_610c;
											}
										}
										goto IL_6109;
									}
									if (flag)
									{
										flag = false;
										zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num57, 0f);
										num55 = m_textInfo.characterInfo[j - 1].scale;
										DrawUnderlineMesh(start, zero, ref index, num54, num55, num56, num53, color);
										num56 = 0f;
										num57 = TMP_Text.k_LargePositiveFloat;
									}
									goto IL_65f8;
									IL_690f:
									flag2 = false;
									DrawUnderlineMesh(start2, zero2, ref index, num60, num60, num60, num53, underlineColor);
									goto IL_6bfe;
									IL_6696:
									flag17 = false;
									goto IL_6699;
								}
								while (true)
								{
									m_textInfo.characterCount = (short)m_characterCount;
									m_textInfo.spriteCount = m_spriteCount;
									m_textInfo.lineCount = (short)num47;
									TMP_TextInfo textInfo = m_textInfo;
									int wordCount3;
									if (num46 != 0 && m_characterCount > 0)
									{
										wordCount3 = (short)num46;
									}
									else
									{
										wordCount3 = 1;
									}
									textInfo.wordCount = wordCount3;
									m_textInfo.pageCount = m_pageNumber + 1;
									if (m_renderMode == TextRenderFlags.Render)
									{
										if (m_canvas.additionalShaderChannels != (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent))
										{
											m_canvas.additionalShaderChannels |= (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
										}
										if (m_geometrySortingOrder != 0)
										{
											m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
										}
										m_mesh.MarkDynamic();
										m_mesh.vertices = m_textInfo.meshInfo[0].vertices;
										m_mesh.uv = m_textInfo.meshInfo[0].uvs0;
										m_mesh.uv2 = m_textInfo.meshInfo[0].uvs2;
										m_mesh.colors32 = m_textInfo.meshInfo[0].colors32;
										m_mesh.RecalculateBounds();
										m_canvasRenderer.SetMesh(m_mesh);
										Color color3 = m_canvasRenderer.GetColor();
										for (int k = 1; k < m_textInfo.materialCount; k++)
										{
											m_textInfo.meshInfo[k].ClearUnusedVertices();
											if (m_subTextObjects[k] == null)
											{
												continue;
											}
											if (m_geometrySortingOrder != 0)
											{
												m_textInfo.meshInfo[k].SortGeometry(VertexSortingOrder.Reverse);
											}
											m_subTextObjects[k].mesh.vertices = m_textInfo.meshInfo[k].vertices;
											m_subTextObjects[k].mesh.uv = m_textInfo.meshInfo[k].uvs0;
											m_subTextObjects[k].mesh.uv2 = m_textInfo.meshInfo[k].uvs2;
											m_subTextObjects[k].mesh.colors32 = m_textInfo.meshInfo[k].colors32;
											m_subTextObjects[k].mesh.RecalculateBounds();
											m_subTextObjects[k].canvasRenderer.SetMesh(m_subTextObjects[k].mesh);
											m_subTextObjects[k].canvasRenderer.SetColor(color3);
										}
									}
									TMPro_EventManager.ON_TEXT_CHANGED(this);
									return;
								}
							}
						}
					}
					ClearMesh();
					m_preferredWidth = 0f;
					m_preferredHeight = 0f;
					TMPro_EventManager.ON_TEXT_CHANGED(this);
					return;
				}
			}
			Debug.LogWarning(new StringBuilder().Append("Can't Generate Mesh! No Font Asset has been assigned to Object ID: ").Append(GetInstanceID()).ToString());
		}

		protected override Vector3[] GetTextContainerLocalCorners()
		{
			if (m_rectTransform == null)
			{
				m_rectTransform = base.rectTransform;
			}
			m_rectTransform.GetLocalCorners(m_RectTransformCorners);
			return m_RectTransformCorners;
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			for (int i = 1; i < m_subTextObjects.Length && m_subTextObjects[i] != null; i++)
			{
				if (m_subTextObjects[i].enabled != state)
				{
					m_subTextObjects[i].enabled = state;
				}
			}
		}

		protected override Bounds GetCompoundBounds()
		{
			Bounds bounds = m_mesh.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			for (int i = 1; i < m_subTextObjects.Length; i++)
			{
				if (m_subTextObjects[i] != null)
				{
					Bounds bounds2 = m_subTextObjects[i].mesh.bounds;
					float x = min.x;
					Vector3 min2 = bounds2.min;
					float x2;
					if (x < min2.x)
					{
						x2 = min.x;
					}
					else
					{
						Vector3 min3 = bounds2.min;
						x2 = min3.x;
					}
					min.x = x2;
					float y = min.y;
					Vector3 min4 = bounds2.min;
					float y2;
					if (y < min4.y)
					{
						y2 = min.y;
					}
					else
					{
						Vector3 min5 = bounds2.min;
						y2 = min5.y;
					}
					min.y = y2;
					float x3 = max.x;
					Vector3 max2 = bounds2.max;
					float x4;
					if (x3 > max2.x)
					{
						x4 = max.x;
					}
					else
					{
						Vector3 max3 = bounds2.max;
						x4 = max3.x;
					}
					max.x = x4;
					float y3 = max.y;
					Vector3 max4 = bounds2.max;
					float y4;
					if (y3 > max4.y)
					{
						y4 = max.y;
					}
					else
					{
						Vector3 max5 = bounds2.max;
						y4 = max5.y;
					}
					max.y = y4;
					continue;
				}
				break;
			}
			Vector3 center = (min + max) / 2f;
			Vector2 v = max - min;
			return new Bounds(center, v);
		}

		private void UpdateSDFScale(float lossyScale)
		{
			if (m_canvas == null)
			{
				m_canvas = GetCanvas();
				if (m_canvas == null)
				{
					return;
				}
			}
			lossyScale = ((lossyScale != 0f) ? lossyScale : 1f);
			float num = 0f;
			float scaleFactor = m_canvas.scaleFactor;
			if (m_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				num = lossyScale / scaleFactor;
			}
			else if (m_canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				float num2;
				if (m_canvas.worldCamera != null)
				{
					num2 = lossyScale;
				}
				else
				{
					num2 = 1f;
				}
				num = num2;
			}
			else
			{
				num = lossyScale;
			}
			for (int i = 0; i < m_textInfo.characterCount; i++)
			{
				if (!m_textInfo.characterInfo[i].isVisible)
				{
					continue;
				}
				if (m_textInfo.characterInfo[i].elementType != 0)
				{
					continue;
				}
				float num3 = num * m_textInfo.characterInfo[i].scale * (1f - m_charWidthAdjDelta);
				if (!m_textInfo.characterInfo[i].isUsingAlternateTypeface)
				{
					if ((m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num3 *= -1f;
					}
				}
				int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
				int vertexIndex = m_textInfo.characterInfo[i].vertexIndex;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num3;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num3;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num3;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num3;
			}
			while (true)
			{
				for (int j = 0; j < m_textInfo.materialCount; j++)
				{
					if (j == 0)
					{
						m_mesh.uv2 = m_textInfo.meshInfo[0].uvs2;
						m_canvasRenderer.SetMesh(m_mesh);
					}
					else
					{
						m_subTextObjects[j].mesh.uv2 = m_textInfo.meshInfo[j].uvs2;
						m_subTextObjects[j].canvasRenderer.SetMesh(m_subTextObjects[j].mesh);
					}
				}
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

		protected override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			Vector3 vector = new Vector3(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				m_textInfo.characterInfo[i].bottomLeft -= vector;
				m_textInfo.characterInfo[i].topLeft -= vector;
				m_textInfo.characterInfo[i].topRight -= vector;
				m_textInfo.characterInfo[i].bottomRight -= vector;
				m_textInfo.characterInfo[i].ascender -= vector.y;
				m_textInfo.characterInfo[i].baseLine -= vector.y;
				m_textInfo.characterInfo[i].descender -= vector.y;
				if (m_textInfo.characterInfo[i].isVisible)
				{
					m_textInfo.characterInfo[i].vertex_BL.position -= vector;
					m_textInfo.characterInfo[i].vertex_TL.position -= vector;
					m_textInfo.characterInfo[i].vertex_TR.position -= vector;
					m_textInfo.characterInfo[i].vertex_BR.position -= vector;
				}
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
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
			if (!m_isCalculateSizeRequired)
			{
				if (!m_rectTransform.hasChanged)
				{
					return;
				}
			}
			m_preferredWidth = GetPreferredWidth();
			ComputeMarginSize();
			m_isLayoutDirty = true;
		}

		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				while (true)
				{
					return;
				}
			}
			if (!m_isCalculateSizeRequired)
			{
				if (!m_rectTransform.hasChanged)
				{
					goto IL_005d;
				}
			}
			m_preferredHeight = GetPreferredHeight();
			ComputeMarginSize();
			m_isLayoutDirty = true;
			goto IL_005d;
			IL_005d:
			m_isCalculateSizeRequired = false;
		}

		public override void SetVerticesDirty()
		{
			if (m_verticesAlreadyDirty || this == null)
			{
				return;
			}
			while (true)
			{
				if (!IsActive())
				{
					return;
				}
				while (true)
				{
					if (CanvasUpdateRegistry.IsRebuildingGraphics())
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
					m_verticesAlreadyDirty = true;
					CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
					return;
				}
			}
		}

		public override void SetLayoutDirty()
		{
			m_isPreferredWidthDirty = true;
			m_isPreferredHeightDirty = true;
			if (m_layoutAlreadyDirty)
			{
				return;
			}
			while (true)
			{
				if (this == null)
				{
					return;
				}
				while (true)
				{
					if (!IsActive())
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
					m_layoutAlreadyDirty = true;
					LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
					m_isLayoutDirty = true;
					return;
				}
			}
		}

		public override void SetMaterialDirty()
		{
			if (this == null)
			{
				return;
			}
			while (true)
			{
				if (!IsActive() || CanvasUpdateRegistry.IsRebuildingGraphics())
				{
					return;
				}
				m_isMaterialDirty = true;
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
				if (m_OnDirtyMaterialCallback == null)
				{
					return;
				}
				while (true)
				{
					m_OnDirtyMaterialCallback();
					return;
				}
			}
		}

		public override void SetAllDirty()
		{
			SetLayoutDirty();
			SetVerticesDirty();
			SetMaterialDirty();
		}

		public override void Rebuild(CanvasUpdate update)
		{
			if (this == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			switch (update)
			{
			case CanvasUpdate.Prelayout:
				while (true)
				{
					if (m_autoSizeTextContainer)
					{
						while (true)
						{
							m_rectTransform.sizeDelta = GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
							return;
						}
					}
					return;
				}
			case CanvasUpdate.PreRender:
				OnPreRenderCanvas();
				m_verticesAlreadyDirty = false;
				m_layoutAlreadyDirty = false;
				if (!m_isMaterialDirty)
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
				UpdateMaterial();
				m_isMaterialDirty = false;
				break;
			}
		}

		private void UpdateSubObjectPivot()
		{
			if (m_textInfo == null)
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
			int num = 1;
			while (num < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num] != null)
					{
						m_subTextObjects[num].SetPivotDirty();
						num++;
						goto IL_0031;
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
				IL_0031:;
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (m_ShouldRecalculateStencil)
			{
				m_stencilID = TMP_MaterialManager.GetStencilID(base.gameObject);
				m_ShouldRecalculateStencil = false;
			}
			if (m_stencilID > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, m_stencilID);
				if (m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
				}
				m_MaskMaterial = material;
			}
			return material;
		}

		protected override void UpdateMaterial()
		{
			if (m_sharedMaterial == null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			if (m_canvasRenderer == null)
			{
				m_canvasRenderer = canvasRenderer;
			}
			m_canvasRenderer.materialCount = 1;
			m_canvasRenderer.SetMaterial(materialForRendering, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
		}

		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		public override void RecalculateMasking()
		{
			m_ShouldRecalculateStencil = true;
			SetMaterialDirty();
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			if (m_ignoreRectMaskCulling)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			base.Cull(clipRect, validRect);
		}

		public override void UpdateMeshPadding()
		{
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_havePropertiesChanged = true;
			checkPaddingRequired = false;
			if (m_textInfo == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			for (int i = 1; i < m_textInfo.materialCount; i++)
			{
				m_subTextObjects[i].UpdateMeshPadding(m_enableExtraPadding, m_isUsingBold);
			}
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

		protected override void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				m_subTextObjects[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		protected override void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				m_subTextObjects[i].CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			}
		}

		public override void ForceMeshUpdate()
		{
			m_havePropertiesChanged = true;
			OnPreRenderCanvas();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			m_havePropertiesChanged = true;
			m_ignoreActiveState = true;
			OnPreRenderCanvas();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			m_renderMode = TextRenderFlags.DontRender;
			ComputeMarginSize();
			if (m_canvas == null)
			{
				m_canvas = base.canvas;
			}
			GenerateTextMesh();
			m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh()
		{
			m_canvasRenderer.SetMesh(null);
			int num = 1;
			while (num < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num] != null)
					{
						m_subTextObjects[num].canvasRenderer.SetMesh(null);
						num++;
						goto IL_0027;
					}
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
				IL_0027:;
			}
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
			if (index == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_canvasRenderer.SetMesh(mesh);
						return;
					}
				}
			}
			m_subTextObjects[index].canvasRenderer.SetMesh(mesh);
		}

		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = m_mesh;
				}
				else
				{
					mesh = m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					mesh.vertices = m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					mesh.uv = m_textInfo.meshInfo[i].uvs0;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					mesh.uv2 = m_textInfo.meshInfo[i].uvs2;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					mesh.colors32 = m_textInfo.meshInfo[i].colors32;
				}
				mesh.RecalculateBounds();
				if (i == 0)
				{
					m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
		}

		public override void UpdateVertexData()
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = m_mesh;
				}
				else
				{
					m_textInfo.meshInfo[i].ClearUnusedVertices();
					mesh = m_subTextObjects[i].mesh;
				}
				mesh.vertices = m_textInfo.meshInfo[i].vertices;
				mesh.uv = m_textInfo.meshInfo[i].uvs0;
				mesh.uv2 = m_textInfo.meshInfo[i].uvs2;
				mesh.colors32 = m_textInfo.meshInfo[i].colors32;
				mesh.RecalculateBounds();
				if (i == 0)
				{
					m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
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

		public void UpdateFontAsset()
		{
			LoadFontAsset();
		}
	}
}
