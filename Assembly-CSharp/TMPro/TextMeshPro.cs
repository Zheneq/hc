using System;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TMPro
{
	[RequireComponent(typeof(MeshFilter))]
	[DisallowMultipleComponent]
	[SelectionBase]
	[RequireComponent(typeof(MeshRenderer))]
	[ExecuteInEditMode]
	[AddComponentMenu("Mesh/TextMeshPro - Text")]
	public class TextMeshPro : TMP_Text, ILayoutElement
	{
		[SerializeField]
		private bool m_hasFontAssetChanged;

		private float m_previousLossyScaleY = -1f;

		[SerializeField]
		private Renderer m_renderer;

		private MeshFilter m_meshFilter;

		private bool m_isFirstAllocation;

		private int m_max_characters = 8;

		private int m_max_numberOfLines = 4;

		private Bounds m_default_bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 0f));

		[SerializeField]
		protected TMP_SubMesh[] m_subTextObjects = new TMP_SubMesh[8];

		private bool m_isMaskingEnabled;

		private bool isMaskUpdateRequired;

		[SerializeField]
		private MaskingTypes m_maskType;

		private Matrix4x4 m_EnvMapMatrix = default(Matrix4x4);

		private Vector3[] m_RectTransformCorners = new Vector3[4];

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private int loopCountA;

		private bool m_currentAutoSizeMode;

		public int sortingLayerID
		{
			get
			{
				return m_renderer.sortingLayerID;
			}
			set
			{
				m_renderer.sortingLayerID = value;
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_renderer.sortingOrder;
			}
			set
			{
				m_renderer.sortingOrder = value;
			}
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
						switch (7)
						{
						case 0:
							break;
						default:
							return;
						}
					}
				}
				m_autoSizeTextContainer = value;
				if (m_autoSizeTextContainer)
				{
					TMP_UpdateManager.RegisterTextElementForLayoutRebuild(this);
					SetLayoutDirty();
				}
			}
		}

		[Obsolete("The TextContainer is now obsolete. Use the RectTransform instead.")]
		public TextContainer textContainer
		{
			get { return null; }
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

		public Renderer renderer
		{
			get
			{
				if (m_renderer == null)
				{
					m_renderer = GetComponent<Renderer>();
				}
				return m_renderer;
			}
		}

		public override Mesh mesh
		{
			get
			{
				if (m_mesh == null)
				{
					m_mesh = new Mesh();
					m_mesh.hideFlags = HideFlags.HideAndDontSave;
					meshFilter.mesh = m_mesh;
				}
				return m_mesh;
			}
		}

		public MeshFilter meshFilter
		{
			get
			{
				if (m_meshFilter == null)
				{
					m_meshFilter = GetComponent<MeshFilter>();
				}
				return m_meshFilter;
			}
		}

		public MaskingTypes maskType
		{
			get
			{
				return m_maskType;
			}
			set
			{
				m_maskType = value;
				SetMask(m_maskType);
			}
		}

		protected override void Awake()
		{
			m_renderer = GetComponent<Renderer>();
			if (m_renderer == null)
			{
				m_renderer = base.gameObject.AddComponent<Renderer>();
			}
			if (base.canvasRenderer != null)
			{
				base.canvasRenderer.hideFlags = HideFlags.HideInInspector;
			}
			else
			{
				CanvasRenderer canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
				canvasRenderer.hideFlags = HideFlags.HideInInspector;
			}
			m_rectTransform = base.rectTransform;
			m_transform = transform;
			m_meshFilter = GetComponent<MeshFilter>();
			if (m_meshFilter == null)
			{
				m_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (m_mesh == null)
			{
				m_mesh = new Mesh();
				m_mesh.hideFlags = HideFlags.HideAndDontSave;
				m_meshFilter.mesh = m_mesh;
			}
			m_meshFilter.hideFlags = HideFlags.HideInInspector;
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
						Debug.LogWarning(new StringBuilder().Append("Please assign a Font Asset to this ").Append(transform.name).Append(" gameobject.").ToString(), this);
						return;
					}
				}
			}
			TMP_SubMesh[] componentsInChildren = GetComponentsInChildren<TMP_SubMesh>();
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
			meshFilter.sharedMesh = mesh;
			SetActiveSubMeshes(true);
			ComputeMarginSize();
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_verticesAlreadyDirty = false;
			SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
			m_meshFilter.sharedMesh = null;
			SetActiveSubMeshes(false);
		}

		protected override void OnDestroy()
		{
			if (m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(m_mesh);
			}
			m_isRegisteredForEvents = false;
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
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
					Debug.LogWarning(new StringBuilder().Append("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to ").Append(base.gameObject.name).Append(".").ToString(), this);
					return;
				}
				if (m_fontAsset.characterDictionary == null)
				{
					Debug.Log("Dictionary is Null!");
				}
				m_renderer.sharedMaterial = m_fontAsset.material;
				m_sharedMaterial = m_fontAsset.material;
				m_sharedMaterial.SetFloat("_CullMode", 0f);
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				m_renderer.receiveShadows = false;
				m_renderer.shadowCastingMode = ShadowCastingMode.Off;
				goto IL_0253;
			}
			if (m_fontAsset.characterDictionary == null)
			{
				m_fontAsset.ReadFontDefinition();
			}
			if (!(m_renderer.sharedMaterial == null) && !(m_renderer.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null))
			{
				if (m_fontAsset.atlas.GetInstanceID() == m_renderer.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
				{
					m_sharedMaterial = m_renderer.sharedMaterial;
					goto IL_020c;
				}
			}
			m_renderer.sharedMaterial = m_fontAsset.material;
			m_sharedMaterial = m_fontAsset.material;
			goto IL_020c;
			IL_0253:
			m_padding = GetPaddingForMaterial();
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			GetSpecialCharacters(m_fontAsset);
			return;
			IL_020c:
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			if (m_sharedMaterial.passCount == 1)
			{
				m_renderer.receiveShadows = false;
				m_renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			goto IL_0253;
		}

		private void UpdateEnvMapMatrix()
		{
			if (!m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap))
			{
				return;
			}
			while (true)
			{
				if (m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null)
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
				Vector3 euler = m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
				m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
				m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, m_EnvMapMatrix);
				return;
			}
		}

		private void SetMask(MaskingTypes maskType)
		{
			if (maskType != 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (maskType != MaskingTypes.MaskSoft)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									if (maskType != MaskingTypes.MaskHard)
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
									m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_HARD);
									m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
									m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
									return;
								}
							}
						}
						m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
						m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
						m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
						return;
					}
				}
			}
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
			m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
		}

		private void SetMaskCoordinates(Vector4 coords)
		{
			m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
		}

		private void SetMaskCoordinates(Vector4 coords, float softX, float softY)
		{
			m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, softX);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, softY);
		}

		private void EnableMasking()
		{
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				m_isMaskingEnabled = true;
				UpdateMask();
			}
		}

		private void DisableMasking()
		{
			if (!m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				return;
			}
			while (true)
			{
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				m_isMaskingEnabled = false;
				UpdateMask();
				return;
			}
		}

		private void UpdateMask()
		{
			if (!m_isMaskingEnabled)
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
			if (!m_isMaskingEnabled)
			{
				return;
			}
			while (true)
			{
				if (m_fontMaterial == null)
				{
					while (true)
					{
						CreateMaterialInstance();
						return;
					}
				}
				return;
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			if (!(m_fontMaterial == null))
			{
				if (m_fontMaterial.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_0053;
				}
			}
			m_fontMaterial = CreateMaterialInstance(mat);
			goto IL_0053;
			IL_0053:
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
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
			while (true)
			{
				m_fontSharedMaterials = m_fontMaterials;
				return m_fontMaterials;
			}
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
			return m_fontSharedMaterials;
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
				Texture texture = materials[i].GetTexture(ShaderUtilities.ID_MainTex);
				if (i == 0)
				{
					if (texture == null)
					{
						continue;
					}
					if (texture.GetInstanceID() != m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
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
					if (texture == null)
					{
						continue;
					}
					if (texture.GetInstanceID() != m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
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
			thickness = Mathf.Clamp01(thickness);
			m_renderer.material.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			if (m_fontMaterial == null)
			{
				m_fontMaterial = m_renderer.material;
			}
			m_fontMaterial = m_renderer.material;
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			m_renderer.material.SetColor(ShaderUtilities.ID_FaceColor, color);
			if (m_fontMaterial == null)
			{
				m_fontMaterial = m_renderer.material;
			}
			m_sharedMaterial = m_fontMaterial;
		}

		protected override void SetOutlineColor(Color32 color)
		{
			m_renderer.material.SetColor(ShaderUtilities.ID_OutlineColor, color);
			if (m_fontMaterial == null)
			{
				m_fontMaterial = m_renderer.material;
			}
			m_sharedMaterial = m_fontMaterial;
		}

		private void CreateMaterialInstance()
		{
			Material material = new Material(m_sharedMaterial);
			material.shaderKeywords = m_sharedMaterial.shaderKeywords;
			material.name += " Instance";
			m_fontMaterial = material;
		}

		protected override void SetShaderDepth()
		{
			if (m_isOverlay)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
						m_renderer.material.renderQueue = 4000;
						m_sharedMaterial = m_renderer.material;
						return;
					}
				}
			}
			m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
			m_renderer.material.renderQueue = -1;
			m_sharedMaterial = m_renderer.material;
		}

		protected override void SetCulling()
		{
			if (m_isCullingEnabled)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						m_renderer.material.SetFloat("_CullMode", 2f);
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
							Renderer renderer = m_subTextObjects[num].renderer;
							if (renderer != null)
							{
								renderer.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
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
			m_renderer.material.SetFloat("_CullMode", 0f);
			int num2 = 1;
			while (num2 < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num2] != null)
					{
						Renderer renderer2 = m_subTextObjects[num2].renderer;
						if (renderer2 != null)
						{
							renderer2.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
						}
						num2++;
						goto IL_00fc;
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
				IL_00fc:;
			}
		}

		private void SetPerspectiveCorrection()
		{
			if (m_isOrthographic)
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
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
			if (m_sharedMaterial == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return 0f;
					}
				}
			}
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
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
			m_fontWeightInternal = (((m_style & FontStyles.Bold) != FontStyles.Bold) ? m_fontWeight : 700);
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
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] != 0)
				{
					if (m_textInfo.characterInfo != null)
					{
						if (m_totalCharacterCount < m_textInfo.characterInfo.Length)
						{
							goto IL_0178;
						}
					}
					TMP_TextInfo.Resize(ref m_textInfo.characterInfo, m_totalCharacterCount + 1, true);
					goto IL_0178;
				}
				break;
				IL_0178:
				int num2 = chars[i];
				if (m_isRichText)
				{
					if (num2 == 60)
					{
						int currentMaterialIndex = m_currentMaterialIndex;
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
								m_currentMaterialIndex = currentMaterialIndex;
								num++;
								m_totalCharacterCount++;
							}
							continue;
						}
					}
				}
				bool flag = false;
				bool isUsingAlternateTypeface = false;
				TMP_FontAsset currentFontAsset = m_currentFontAsset;
				Material currentMaterial = m_currentMaterial;
				int currentMaterialIndex2 = m_currentMaterialIndex;
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
								goto IL_03d7;
							}
						}
						if (char.IsLower((char)num2))
						{
							num2 = char.ToUpper((char)num2);
						}
					}
				}
				goto IL_03d7;
				IL_03d7:
				TMP_FontAsset fontAssetForWeight = GetFontAssetForWeight(m_fontWeightInternal);
				if (fontAssetForWeight != null)
				{
					flag = true;
					isUsingAlternateTypeface = true;
					m_currentFontAsset = fontAssetForWeight;
				}

				TMP_Glyph glyph;
				fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
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
							m_currentMaterialIndex = currentMaterialIndex2;
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
							m_currentMaterialIndex = currentMaterialIndex2;
							num++;
							m_totalCharacterCount++;
							continue;
						}
					}
				}
				if (glyph == null)
				{
					int num3 = i;
					int num4;
					if (TMP_Settings.missingGlyphCharacter == 0)
					{
						num4 = 9633;
					}
					else
					{
						num4 = TMP_Settings.missingGlyphCharacter;
					}
					int num5 = num4;
					chars[num3] = num4;
					num2 = num5;
					fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
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
				if (!char.IsWhiteSpace((char)num2) && num2 != 8203)
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
				m_textInfo.characterInfo[m_totalCharacterCount].material = m_currentMaterial;
				m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
				m_materialReferences[m_currentMaterialIndex].isFallbackMaterial = flag;
				if (flag)
				{
					m_materialReferences[m_currentMaterialIndex].fallbackMaterial = currentMaterial;
					m_currentFontAsset = currentFontAsset;
					m_currentMaterial = currentMaterial;
					m_currentMaterialIndex = currentMaterialIndex2;
				}
				m_totalCharacterCount++;
			}
			if (m_isCalculatingPreferredValues)
			{
				while (true)
				{
					switch (4)
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
			int num6 = m_textInfo.materialCount = m_materialReferenceIndexLookup.Count;
			if (num6 > m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize(ref m_textInfo.meshInfo, num6, false);
			}
			if (num6 > m_subTextObjects.Length)
			{
				TMP_TextInfo.Resize(ref m_subTextObjects, Mathf.NextPowerOfTwo(num6 + 1));
			}
			if (m_textInfo.characterInfo.Length - m_totalCharacterCount > 256)
			{
				TMP_TextInfo.Resize(ref m_textInfo.characterInfo, Mathf.Max(m_totalCharacterCount + 1, 256), true);
			}
			for (int j = 0; j < num6; j++)
			{
				if (j > 0)
				{
					if (m_subTextObjects[j] == null)
					{
						m_subTextObjects[j] = TMP_SubMesh.AddSubTextObject(this, m_materialReferences[j]);
						m_textInfo.meshInfo[j].vertices = null;
					}
					if (!(m_subTextObjects[j].sharedMaterial == null))
					{
						if (m_subTextObjects[j].sharedMaterial.GetInstanceID() == m_materialReferences[j].material.GetInstanceID())
						{
							goto IL_0e13;
						}
					}
					bool isDefaultMaterial = m_materialReferences[j].isDefaultMaterial;
					m_subTextObjects[j].isDefaultMaterial = isDefaultMaterial;
					if (isDefaultMaterial)
					{
						if (!(m_subTextObjects[j].sharedMaterial == null))
						{
							if (m_subTextObjects[j].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == m_materialReferences[j].material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
							{
								goto IL_0e13;
							}
						}
					}
					m_subTextObjects[j].sharedMaterial = m_materialReferences[j].material;
					m_subTextObjects[j].fontAsset = m_materialReferences[j].fontAsset;
					m_subTextObjects[j].spriteAsset = m_materialReferences[j].spriteAsset;
					goto IL_0e13;
				}
				goto IL_0e71;
				IL_0e13:
				if (m_materialReferences[j].isFallbackMaterial)
				{
					m_subTextObjects[j].fallbackMaterial = m_materialReferences[j].material;
					m_subTextObjects[j].fallbackSourceMaterial = m_materialReferences[j].fallbackMaterial;
				}
				goto IL_0e71;
				IL_0e71:
				int referenceCount = m_materialReferences[j].referenceCount;
				if (m_textInfo.meshInfo[j].vertices != null)
				{
					if (m_textInfo.meshInfo[j].vertices.Length >= referenceCount * (m_isVolumetricText ? 8 : 4))
					{
						int num7 = m_textInfo.meshInfo[j].vertices.Length;
						int num8;
						if (!m_isVolumetricText)
						{
							num8 = 4;
						}
						else
						{
							num8 = 8;
						}
						if (num7 - referenceCount * num8 <= 1024)
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
						m_textInfo.meshInfo[j].ResizeMeshInfo(size, m_isVolumetricText);
						continue;
					}
				}
				if (m_textInfo.meshInfo[j].vertices == null)
				{
					if (j == 0)
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_mesh, referenceCount + 1, m_isVolumetricText);
					}
					else
					{
						m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_subTextObjects[j].mesh, referenceCount + 1, m_isVolumetricText);
					}
				}
				else
				{
					m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount <= 1024) ? Mathf.NextPowerOfTwo(referenceCount) : (referenceCount + 256), m_isVolumetricText);
				}
			}
			while (true)
			{
				for (int k = num6; k < m_subTextObjects.Length; k++)
				{
					if (m_subTextObjects[k] != null)
					{
						if (k < m_textInfo.meshInfo.Length)
						{
							m_textInfo.meshInfo[k].ClearUnusedVertices(0, true);
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
			if (base.rectTransform != null)
			{
				m_marginWidth = m_rectTransform.rect.width - m_margin.x - m_margin.z;
				m_marginHeight = m_rectTransform.rect.height - m_margin.y - m_margin.w;
				m_RectTransformCorners = GetTextContainerLocalCorners();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			m_havePropertiesChanged = true;
			isMaskUpdateRequired = true;
			SetVerticesDirty();
		}

		protected override void OnTransformParentChanged()
		{
			SetVerticesDirty();
			SetLayoutDirty();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			ComputeMarginSize();
			SetVerticesDirty();
			SetLayoutDirty();
		}

		private void LateUpdate()
		{
			if (m_rectTransform.hasChanged)
			{
				Vector3 lossyScale = m_rectTransform.lossyScale;
				float y = lossyScale.y;
				if (!m_havePropertiesChanged)
				{
					if (y != m_previousLossyScaleY)
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
				}
			}
			if (!m_isUsingLegacyAnimationComponent)
			{
				return;
			}
			while (true)
			{
				m_havePropertiesChanged = true;
				OnPreRenderObject();
				return;
			}
		}

		private void OnPreRenderObject()
		{
			if (!m_isAwake)
			{
				return;
			}
			while (true)
			{
				if (!m_ignoreActiveState)
				{
					if (!IsActive())
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
				if (isMaskUpdateRequired)
				{
					UpdateMask();
					isMaskUpdateRequired = false;
				}
				if (checkPaddingRequired)
				{
					UpdateMeshPadding();
				}
				if (!m_isInputParsingRequired)
				{
					if (!m_isTextTruncated)
					{
						goto IL_00a9;
					}
				}
				ParseInputText();
				goto IL_00a9;
				IL_00a9:
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
					if (m_char_buffer != null && m_char_buffer.Length != 0)
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
							float num = m_fontSize / m_currentFontAsset.fontInfo.PointSize;
							float num2;
							if (m_isOrthographic)
							{
								num2 = 1f;
							}
							else
							{
								num2 = 0.1f;
							}
							m_fontScale = num * num2;
							float num3 = m_fontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
							float num4;
							if (m_isOrthographic)
							{
								num4 = 1f;
							}
							else
							{
								num4 = 0.1f;
							}
							float num5 = num3 * num4;
							float num6 = m_fontScale;
							m_fontScaleMultiplier = 1f;
							m_currentFontSize = m_fontSize;
							m_sizeStack.SetDefault(m_currentFontSize);
							float num7 = 0f;
							int num8 = 0;
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
							float num9 = 0f;
							float num10 = 0f;
							float num11 = 1f;
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
							float num12 = m_currentFontAsset.fontInfo.LineHeight - (m_currentFontAsset.fontInfo.Ascender - m_currentFontAsset.fontInfo.Descender);
							m_cSpacing = 0f;
							m_monoSpacing = 0f;
							float num13 = 0f;
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
							int num14 = Mathf.Clamp(m_pageToDisplay - 1, 0, m_textInfo.pageInfo.Length - 1);
							int num15 = 0;
							int num16 = 0;
							Vector4 margin = m_margin;
							float marginWidth = m_marginWidth;
							float marginHeight = m_marginHeight;
							m_marginLeft = 0f;
							m_marginRight = 0f;
							m_width = -1f;
							float num17 = marginWidth + 0.0001f - m_marginLeft - m_marginRight;
							m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
							m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
							m_textInfo.ClearLineInfo();
							m_maxCapHeight = 0f;
							m_maxAscender = 0f;
							m_maxDescender = 0f;
							float num18 = 0f;
							float num19 = 0f;
							bool flag5 = false;
							m_isNewPage = false;
							bool flag6 = true;
							m_isNonBreakingSpace = false;
							bool flag7 = false;
							bool flag8 = false;
							int num20 = 0;
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
								int currentMaterialIndex;
								bool isUsingAlternateTypeface;
								float num21;
								if (m_char_buffer[i] != 0)
								{
									num8 = m_char_buffer[i];
									m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
									m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
									m_currentFontAsset = m_materialReferences[m_currentMaterialIndex].fontAsset;
									currentMaterialIndex = m_currentMaterialIndex;
									if (m_isRichText)
									{
										if (num8 == 60)
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
									isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
									if (m_characterCount < m_firstVisibleCharacter)
									{
										m_textInfo.characterInfo[m_characterCount].isVisible = false;
										m_textInfo.characterInfo[m_characterCount].character = '\u200b';
										m_characterCount++;
										continue;
									}
									num21 = 1f;
									if (m_textElementType == TMP_TextElementType.Character)
									{
										if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
										{
											if (char.IsLower((char)num8))
											{
												num8 = char.ToUpper((char)num8);
											}
										}
										else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
										{
											if (char.IsUpper((char)num8))
											{
												num8 = char.ToLower((char)num8);
											}
										}
										else
										{
											if ((m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
											{
												if ((m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
												{
													goto IL_07c4;
												}
											}
											if (char.IsLower((char)num8))
											{
												num21 = 0.8f;
												num8 = char.ToUpper((char)num8);
											}
										}
									}
									goto IL_07c4;
								}
								break;
								IL_2b9b:
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
								float num24;
								m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num24);
								float num25;
								m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num25);
								m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x - num9 * num6;
								m_textInfo.lineInfo[m_lineNumber].width = num17;
								if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
								{
									m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
								}
								if (m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].isVisible)
								{
									m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 - m_cSpacing;
								}
								else
								{
									m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 - m_cSpacing;
								}
								m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
								m_textInfo.lineInfo[m_lineNumber].ascender = num25;
								m_textInfo.lineInfo[m_lineNumber].descender = num24;
								m_textInfo.lineInfo[m_lineNumber].lineHeight = num25 - num24 + num12 * num5;
								m_firstCharacterOfLine = m_characterCount + 1;
								m_lineVisibleCharacterCount = 0;
								float num26;
								if (num8 == 10)
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
										num13 = 0f - m_maxLineDescender + num26 + (num12 + m_lineSpacing + m_paragraphSpacing + m_lineSpacingDelta) * num5;
										m_lineOffset += num13;
									}
									else
									{
										m_lineOffset += m_lineHeight + (m_lineSpacing + m_paragraphSpacing) * num5;
									}
									m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
									m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
									m_startOfLineAscender = num26;
									m_xAdvance = tag_LineIndent + tag_Indent;
									num16 = m_characterCount - 1;
									m_characterCount++;
									continue;
								}
								goto IL_30c7;
								IL_34fe:
								if (num8 > 4352 && num8 < 4607)
								{
									goto IL_35e5;
								}
								if (num8 > 11904)
								{
									if (num8 < 40959)
									{
										goto IL_35e5;
									}
								}
								if (num8 > 43360)
								{
									if (num8 < 43391)
									{
										goto IL_35e5;
									}
								}
								if (num8 > 44032)
								{
									if (num8 < 55295)
									{
										goto IL_35e5;
									}
								}
								if (num8 > 63744)
								{
									if (num8 < 64255)
									{
										goto IL_35e5;
									}
								}
								if (num8 > 65072)
								{
									if (num8 < 65103)
									{
										goto IL_35e5;
									}
								}
								if (num8 > 65280)
								{
									if (num8 < 65519)
									{
										goto IL_35e5;
									}
								}
								goto IL_3697;
								IL_36d9:
								m_characterCount++;
								continue;
								IL_2248:
								float num28;
								if (m_lineNumber > 0)
								{
									if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
									{
										if (m_lineHeight == -32767f)
										{
											if (!m_isNewPage)
											{
												float num27 = m_maxLineAscender - m_startOfLineAscender;
												AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num27);
												num28 -= num27;
												m_lineOffset += num27;
												m_startOfLineAscender += num27;
												m_SavedWordWrapState.lineOffset = m_lineOffset;
												m_SavedWordWrapState.previousLineAscender = m_startOfLineAscender;
											}
										}
									}
								}
								m_textInfo.characterInfo[m_characterCount].lineNumber = (short)m_lineNumber;
								m_textInfo.characterInfo[m_characterCount].pageNumber = (short)m_pageNumber;
								if (num8 != 10)
								{
									if (num8 != 13)
									{
										if (num8 != 8230)
										{
											goto IL_23b5;
										}
									}
								}
								if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
								{
									goto IL_23b5;
								}
								goto IL_23d6;
								IL_14ba:
								if (m_lineOffset == 0f)
								{
									float num29;
									if (num18 > num26)
									{
										num29 = num18;
									}
									else
									{
										num29 = num26;
									}
									num18 = num29;
								}
								m_textInfo.characterInfo[m_characterCount].isVisible = false;
								if (num8 != 9)
								{
									if (!char.IsWhiteSpace((char)num8))
									{
										if (num8 != 8203)
										{
											goto IL_1554;
										}
									}
									if (m_textElementType != TMP_TextElementType.Sprite)
									{
										if (num8 != 10)
										{
											if (!char.IsSeparator((char)num8))
											{
												goto IL_2248;
											}
										}
										if (num8 != 173 && num8 != 8203)
										{
											if (num8 != 8288)
											{
												m_textInfo.lineInfo[m_lineNumber].spaceCount++;
												m_textInfo.spaceCount++;
											}
										}
										goto IL_2248;
									}
								}
								goto IL_1554;
								IL_30c7:
								if (m_textInfo.characterInfo[m_characterCount].isVisible)
								{
									m_meshExtents.min.x = Mathf.Min(m_meshExtents.min.x, m_textInfo.characterInfo[m_characterCount].bottomLeft.x);
									m_meshExtents.min.y = Mathf.Min(m_meshExtents.min.y, m_textInfo.characterInfo[m_characterCount].bottomLeft.y);
									m_meshExtents.max.x = Mathf.Max(m_meshExtents.max.x, m_textInfo.characterInfo[m_characterCount].topRight.x);
									m_meshExtents.max.y = Mathf.Max(m_meshExtents.max.y, m_textInfo.characterInfo[m_characterCount].topRight.y);
								}
								float num30;
								if (m_overflowMode == TextOverflowModes.Page)
								{
									if (num8 != 13)
									{
										if (num8 != 10)
										{
											if (m_pageNumber + 1 > m_textInfo.pageInfo.Length)
											{
												TMP_TextInfo.Resize(ref m_textInfo.pageInfo, m_pageNumber + 1, true);
											}
											m_textInfo.pageInfo[m_pageNumber].ascender = num18;
											float descender;
											if (num30 < m_textInfo.pageInfo[m_pageNumber].descender)
											{
												descender = num30;
											}
											else
											{
												descender = m_textInfo.pageInfo[m_pageNumber].descender;
											}
											m_textInfo.pageInfo[m_pageNumber].descender = descender;
											if (m_pageNumber == 0)
											{
												if (m_characterCount == 0)
												{
													m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
													goto IL_3403;
												}
											}
											if (m_characterCount > 0)
											{
												if (m_pageNumber != m_textInfo.characterInfo[m_characterCount - 1].pageNumber)
												{
													m_textInfo.pageInfo[m_pageNumber - 1].lastCharacterIndex = m_characterCount - 1;
													m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
													goto IL_3403;
												}
											}
											if (m_characterCount == totalCharacterCount - 1)
											{
												m_textInfo.pageInfo[m_pageNumber].lastCharacterIndex = m_characterCount;
											}
										}
									}
								}
								goto IL_3403;
								IL_23d6:
								if (m_maxAscender - num28 > marginHeight + 0.0001f)
								{
									if (m_enableAutoSizing && m_lineSpacingDelta > m_lineSpacingMax)
									{
										if (m_lineNumber > 0)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													loopCountA = 0;
													m_lineSpacingDelta -= 1f;
													GenerateTextMesh();
													return;
												}
											}
										}
									}
									if (m_enableAutoSizing)
									{
										if (m_fontSize > m_fontSizeMin)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
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
												switch (3)
												{
												case 0:
													break;
												default:
													m_char_buffer[m_textInfo.characterInfo[num16].index] = 8230;
													m_char_buffer[m_textInfo.characterInfo[num16].index + 1] = 0;
													if (m_cached_Ellipsis_GlyphInfo != null)
													{
														m_textInfo.characterInfo[num16].character = '…';
														m_textInfo.characterInfo[num16].textElement = m_cached_Ellipsis_GlyphInfo;
														m_textInfo.characterInfo[num16].fontAsset = m_materialReferences[0].fontAsset;
														m_textInfo.characterInfo[num16].material = m_materialReferences[0].material;
														m_textInfo.characterInfo[num16].materialReferenceIndex = 0;
													}
													else
													{
														Debug.LogWarning(new StringBuilder().Append("Unable to use Ellipsis character since it wasn't found in the current Font Asset [").Append(m_fontAsset.name).Append("]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.").ToString(), this);
													}
													m_totalCharacterCount = num16 + 1;
													GenerateTextMesh();
													m_isTextTruncated = true;
													return;
												}
											}
										}
										ClearMesh(false);
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
												switch (5)
												{
												case 0:
													break;
												default:
													m_char_buffer[m_textInfo.characterInfo[num16].index + 1] = 0;
													m_totalCharacterCount = num16 + 1;
													GenerateTextMesh();
													m_isTextTruncated = true;
													return;
												}
											}
										}
										ClearMesh(false);
										return;
									case TextOverflowModes.Page:
										if (m_isMaskingEnabled)
										{
											DisableMasking();
										}
										if (num8 == 13)
										{
											break;
										}
										if (num8 == 10)
										{
											break;
										}
										if (i == 0)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													ClearMesh();
													return;
												}
											}
										}
										if (num15 == i)
										{
											m_char_buffer[i] = 0;
											m_isTextTruncated = true;
										}
										num15 = i;
										i = RestoreWordWrappingState(ref m_SavedLineState);
										m_isNewPage = true;
										m_xAdvance = tag_Indent;
										m_lineOffset = 0f;
										m_maxAscender = 0f;
										num18 = 0f;
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
												switch (7)
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
										ClearMesh(true);
										return;
									}
								}
								float num33;
								if (num8 == 9)
								{
									float num31 = m_currentFontAsset.fontInfo.TabWidth * num6;
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
									m_xAdvance += (m_monoSpacing - num33 + (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 + m_cSpacing) * (1f - m_charWidthAdjDelta);
									if (!char.IsWhiteSpace((char)num8))
									{
										if (num8 != 8203)
										{
											goto IL_2a23;
										}
									}
									m_xAdvance += m_wordSpacing * num6;
								}
								else if (!m_isRightToLeft)
								{
									m_xAdvance += ((m_cached_TextElement.xAdvance * num11 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 + m_cSpacing) * (1f - m_charWidthAdjDelta);
									if (!char.IsWhiteSpace((char)num8))
									{
										if (num8 != 8203)
										{
											goto IL_2a23;
										}
									}
									m_xAdvance += m_wordSpacing * num6;
								}
								goto IL_2a23;
								IL_07c4:
								if (m_textElementType == TMP_TextElementType.Sprite)
								{
									m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
									m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
									TMP_Sprite tMP_Sprite = m_currentSpriteAsset.spriteInfoList[m_spriteIndex];
									if (tMP_Sprite == null)
									{
										continue;
									}
									if (num8 == 60)
									{
										num8 = 57344 + m_spriteIndex;
									}
									else
									{
										m_spriteColor = TMP_Text.s_colorWhite;
									}
									m_currentFontAsset = m_fontAsset;
									float num34 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale * ((!m_isOrthographic) ? 0.1f : 1f);
									num6 = m_fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * num34;
									m_cached_TextElement = tMP_Sprite;
									m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
									m_textInfo.characterInfo[m_characterCount].scale = num34;
									m_textInfo.characterInfo[m_characterCount].spriteAsset = m_currentSpriteAsset;
									m_textInfo.characterInfo[m_characterCount].fontAsset = m_currentFontAsset;
									m_textInfo.characterInfo[m_characterCount].materialReferenceIndex = m_currentMaterialIndex;
									m_currentMaterialIndex = currentMaterialIndex;
									num9 = 0f;
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
									float num35 = m_currentFontSize * num21 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
									float num36;
									if (m_isOrthographic)
									{
										num36 = 1f;
									}
									else
									{
										num36 = 0.1f;
									}
									m_fontScale = num35 * num36;
									num6 = m_fontScale * m_fontScaleMultiplier * m_cached_TextElement.scale;
									m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
									m_textInfo.characterInfo[m_characterCount].scale = num6;
									num9 = ((m_currentMaterialIndex != 0) ? m_subTextObjects[m_currentMaterialIndex].padding : m_padding);
								}
								float num37 = num6;
								if (num8 == 173)
								{
									num6 = 0f;
								}
								if (m_isRightToLeft)
								{
									m_xAdvance -= ((m_cached_TextElement.xAdvance * num11 + m_characterSpacing + m_wordSpacing + m_currentFontAsset.normalSpacingOffset) * num6 + m_cSpacing) * (1f - m_charWidthAdjDelta);
									if (char.IsWhiteSpace((char)num8) || num8 == 8203)
									{
										m_xAdvance -= m_wordSpacing * num6;
									}
								}
								m_textInfo.characterInfo[m_characterCount].character = (char)num8;
								m_textInfo.characterInfo[m_characterCount].pointSize = m_currentFontSize;
								m_textInfo.characterInfo[m_characterCount].color = m_htmlColor;
								m_textInfo.characterInfo[m_characterCount].underlineColor = m_underlineColor;
								m_textInfo.characterInfo[m_characterCount].strikethroughColor = m_strikethroughColor;
								m_textInfo.characterInfo[m_characterCount].highlightColor = m_highlightColor;
								m_textInfo.characterInfo[m_characterCount].style = m_style;
								m_textInfo.characterInfo[m_characterCount].index = (short)i;
								if (m_enableKerning && m_characterCount >= 1)
								{
									int character = m_textInfo.characterInfo[m_characterCount - 1].character;
									KerningPairKey kerningPairKey = new KerningPairKey(character, num8);
									KerningPair value;
									m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out value);
									if (value != null)
									{
										m_xAdvance += value.XadvanceOffset * num6;
									}
								}
								num33 = 0f;
								if (m_monoSpacing != 0f)
								{
									num33 = (m_monoSpacing / 2f - (m_cached_TextElement.width / 2f + m_cached_TextElement.xOffset) * num6) * (1f - m_charWidthAdjDelta);
									m_xAdvance += num33;
								}
								if (m_textElementType != 0 || isUsingAlternateTypeface)
								{
									goto IL_0e4e;
								}
								if ((m_style & FontStyles.Bold) != FontStyles.Bold)
								{
									if ((m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
									{
										goto IL_0e4e;
									}
								}
								if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
								{
									float @float = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
									num10 = m_currentFontAsset.boldStyle / 4f * @float * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
									if (num10 + num9 > @float)
									{
										num9 = @float - num10;
									}
								}
								else
								{
									num10 = 0f;
								}
								num11 = 1f + m_currentFontAsset.boldSpacing * 0.01f;
								goto IL_0ec7;
								IL_10e4:
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
								num26 = m_currentFontAsset.fontInfo.Ascender * ((m_textElementType != 0) ? m_textInfo.characterInfo[m_characterCount].scale : num6) + m_baselineOffset;
								m_textInfo.characterInfo[m_characterCount].ascender = num26 - m_lineOffset;
								m_maxLineAscender = ((!(num26 > m_maxLineAscender)) ? m_maxLineAscender : num26);
								float descender2 = m_currentFontAsset.fontInfo.Descender;
								float num38;
								if (m_textElementType == TMP_TextElementType.Character)
								{
									num38 = num6;
								}
								else
								{
									num38 = m_textInfo.characterInfo[m_characterCount].scale;
								}
								num30 = descender2 * num38 + m_baselineOffset;
								num28 = (m_textInfo.characterInfo[m_characterCount].descender = num30 - m_lineOffset);
								m_maxLineDescender = ((!(num30 < m_maxLineDescender)) ? m_maxLineDescender : num30);
								if ((m_style & FontStyles.Subscript) != FontStyles.Subscript)
								{
									if ((m_style & FontStyles.Superscript) != FontStyles.Superscript)
									{
										goto IL_1461;
									}
								}
								float num39 = (num26 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
								num26 = m_maxLineAscender;
								float maxLineAscender;
								if (num39 > m_maxLineAscender)
								{
									maxLineAscender = num39;
								}
								else
								{
									maxLineAscender = m_maxLineAscender;
								}
								m_maxLineAscender = maxLineAscender;
								float num40 = (num30 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
								num30 = m_maxLineDescender;
								m_maxLineDescender = ((!(num40 < m_maxLineDescender)) ? m_maxLineDescender : num40);
								goto IL_1461;
								IL_23b5:
								m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
								goto IL_23d6;
								IL_17f8:
								i = RestoreWordWrappingState(ref m_SavedWordWrapState);
								num20 = i;
								if (m_char_buffer[i] == 173)
								{
									while (true)
									{
										m_isTextTruncated = true;
										m_char_buffer[i] = 45;
										GenerateTextMesh();
										return;
									}
								}
								if (m_lineNumber > 0)
								{
									if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f)
									{
										if (!m_isNewPage)
										{
											float num41 = m_maxLineAscender - m_startOfLineAscender;
											AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num41);
											m_lineOffset += num41;
											m_SavedWordWrapState.lineOffset = m_lineOffset;
											m_SavedWordWrapState.previousLineAscender = m_maxLineAscender;
										}
									}
								}
								m_isNewPage = false;
								float num42 = m_maxLineAscender - m_lineOffset;
								float num43 = m_maxLineDescender - m_lineOffset;
								float maxDescender;
								if (m_maxDescender < num43)
								{
									maxDescender = m_maxDescender;
								}
								else
								{
									maxDescender = num43;
								}
								m_maxDescender = maxDescender;
								if (!flag5)
								{
									num19 = m_maxDescender;
								}
								if (m_useMaxVisibleDescender)
								{
									if (m_characterCount < m_maxVisibleCharacters)
									{
										if (m_lineNumber < m_maxVisibleLines)
										{
											goto IL_196e;
										}
									}
									flag5 = true;
								}
								goto IL_196e;
								IL_3697:
								if (!flag6)
								{
									if (!m_isCharacterWrappingEnabled)
									{
										if (!flag8)
										{
											goto IL_36d9;
										}
									}
								}
								SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
								goto IL_36d9;
								IL_3403:
								if (!m_enableWordWrapping)
								{
									if (m_overflowMode != TextOverflowModes.Truncate)
									{
										if (m_overflowMode != TextOverflowModes.Ellipsis)
										{
											goto IL_36d9;
										}
									}
								}
								if (!char.IsWhiteSpace((char)num8))
								{
									if (num8 != 8203)
									{
										if (num8 != 45)
										{
											if (num8 != 173)
											{
												goto IL_34fe;
											}
										}
									}
								}
								if (m_isNonBreakingSpace)
								{
									if (!flag7)
									{
										goto IL_34fe;
									}
								}
								if (num8 != 160)
								{
									if (num8 != 8209 && num8 != 8239)
									{
										if (num8 != 8288)
										{
											SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
											m_isCharacterWrappingEnabled = false;
											flag6 = false;
											goto IL_36d9;
										}
									}
								}
								goto IL_34fe;
								IL_35e5:
								if (!m_isNonBreakingSpace)
								{
									if (!flag6)
									{
										if (!flag8)
										{
											if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num8) && m_characterCount < totalCharacterCount - 1)
											{
												if (!TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_textInfo.characterInfo[m_characterCount + 1].character))
												{
													goto IL_3677;
												}
											}
											goto IL_36d9;
										}
									}
									goto IL_3677;
								}
								goto IL_3697;
								IL_0e4e:
								if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
								{
									float float2 = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
									num10 = m_currentFontAsset.normalStyle / 4f * float2 * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
									if (num10 + num9 > float2)
									{
										num9 = float2 - num10;
									}
								}
								else
								{
									num10 = 0f;
								}
								num11 = 1f;
								goto IL_0ec7;
								IL_1554:
								m_textInfo.characterInfo[m_characterCount].isVisible = true;
								float num44;
								if (m_width != -1f)
								{
									num44 = Mathf.Min(marginWidth + 0.0001f - m_marginLeft - m_marginRight, m_width);
								}
								else
								{
									num44 = marginWidth + 0.0001f - m_marginLeft - m_marginRight;
								}
								num17 = num44;
								m_textInfo.lineInfo[m_lineNumber].marginLeft = m_marginLeft;
								int num45;
								if ((m_lineJustification & (TextAlignmentOptions)16) != (TextAlignmentOptions)16)
								{
									num45 = (((m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8) ? 1 : 0);
								}
								else
								{
									num45 = 1;
								}
								bool flag9 = (byte)num45 != 0;
								float num46 = Mathf.Abs(m_xAdvance);
								float num47;
								if (!m_isRightToLeft)
								{
									num47 = m_cached_TextElement.xAdvance;
								}
								else
								{
									num47 = 0f;
								}
								float num48 = num47 * (1f - m_charWidthAdjDelta);
								float num49;
								if (num8 != 173)
								{
									num49 = num6;
								}
								else
								{
									num49 = num37;
								}
								float num50 = num46 + num48 * num49;
								float num51 = num17;
								float num52;
								if (flag9)
								{
									num52 = 1.05f;
								}
								else
								{
									num52 = 1f;
								}
								if (num50 > num51 * num52)
								{
									num16 = m_characterCount - 1;
									if (base.enableWordWrapping)
									{
										if (m_characterCount != m_firstCharacterOfLine)
										{
											if (num20 != m_SavedWordWrapState.previous_WordBreak)
											{
												if (!flag6)
												{
													goto IL_17f8;
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
															if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
															{
																while (true)
																{
																	switch (1)
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
											goto IL_17f8;
										}
									}
									if (m_enableAutoSizing && m_fontSize > m_fontSizeMin)
									{
										while (true)
										{
											switch (7)
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
														switch (2)
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
											m_textInfo.characterInfo[num16].character = '…';
											m_textInfo.characterInfo[num16].textElement = m_cached_Ellipsis_GlyphInfo;
											m_textInfo.characterInfo[num16].fontAsset = m_materialReferences[0].fontAsset;
											m_textInfo.characterInfo[num16].material = m_materialReferences[0].material;
											m_textInfo.characterInfo[num16].materialReferenceIndex = 0;
										}
										else
										{
											Debug.LogWarning(new StringBuilder().Append("Unable to use Ellipsis character since it wasn't found in the current Font Asset [").Append(m_fontAsset.name).Append("]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.").ToString(), this);
										}
										m_totalCharacterCount = num16 + 1;
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
								if (num8 != 9)
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
										SaveGlyphVertexInfo(num9, num10, vertexColor);
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
									if (num8 != 173)
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
								goto IL_2248;
								IL_3677:
								SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
								m_isCharacterWrappingEnabled = false;
								flag6 = false;
								goto IL_36d9;
								IL_196e:
								m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
								m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = (m_firstVisibleCharacterOfLine = ((m_firstCharacterOfLine <= m_firstVisibleCharacterOfLine) ? m_firstVisibleCharacterOfLine : m_firstCharacterOfLine));
								m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = ((m_characterCount - 1 > 0) ? (m_characterCount - 1) : 0));
								int num53;
								if (m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine)
								{
									num53 = m_firstVisibleCharacterOfLine;
								}
								else
								{
									num53 = m_lastVisibleCharacterOfLine;
								}
								firstVisibleCharacterIndex = num53;
								m_lastVisibleCharacterOfLine = num53;
								m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = firstVisibleCharacterIndex;
								m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
								m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
								m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num43);
								m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num42);
								m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x;
								m_textInfo.lineInfo[m_lineNumber].width = num17;
								m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num6 - m_cSpacing;
								m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
								m_textInfo.lineInfo[m_lineNumber].ascender = num42;
								m_textInfo.lineInfo[m_lineNumber].descender = num43;
								m_textInfo.lineInfo[m_lineNumber].lineHeight = num42 - num43 + num12 * num5;
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
									float num54 = m_textInfo.characterInfo[m_characterCount].ascender - m_textInfo.characterInfo[m_characterCount].baseLine;
									num13 = 0f - m_maxLineDescender + num54 + (num12 + m_lineSpacing + m_lineSpacingDelta) * num5;
									m_lineOffset += num13;
									m_startOfLineAscender = num54;
								}
								else
								{
									m_lineOffset += m_lineHeight + m_lineSpacing * num5;
								}
								m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
								m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
								m_xAdvance = tag_Indent;
								continue;
								IL_1461:
								if (m_lineNumber != 0)
								{
									if (!m_isNewPage)
									{
										goto IL_14ba;
									}
								}
								m_maxAscender = ((!(m_maxAscender > num26)) ? num26 : m_maxAscender);
								m_maxCapHeight = Mathf.Max(m_maxCapHeight, m_currentFontAsset.fontInfo.CapHeight * num6);
								goto IL_14ba;
								IL_0ec7:
								float baseline = m_currentFontAsset.fontInfo.Baseline;
								vector4.x = m_xAdvance + (m_cached_TextElement.xOffset - num9 - num10) * num6 * (1f - m_charWidthAdjDelta);
								vector4.y = (baseline + m_cached_TextElement.yOffset + num9) * num6 - m_lineOffset + m_baselineOffset;
								vector4.z = 0f;
								vector3.x = vector4.x;
								vector3.y = vector4.y - (m_cached_TextElement.height + num9 * 2f) * num6;
								vector3.z = 0f;
								vector2.x = vector3.x + (m_cached_TextElement.width + num9 * 2f + num10 * 2f) * num6 * (1f - m_charWidthAdjDelta);
								vector2.y = vector4.y;
								vector2.z = 0f;
								vector5.x = vector2.x;
								vector5.y = vector3.y;
								vector5.z = 0f;
								if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface)
								{
									if ((m_style & FontStyles.Italic) != FontStyles.Italic)
									{
										if ((m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
										{
											goto IL_10e4;
										}
									}
									float num55 = (float)(int)m_currentFontAsset.italicStyle * 0.01f;
									Vector3 vector6 = new Vector3(num55 * ((m_cached_TextElement.yOffset + num9 + num10) * num6), 0f, 0f);
									Vector3 vector7 = new Vector3(num55 * ((m_cached_TextElement.yOffset - m_cached_TextElement.height - num9 - num10) * num6), 0f, 0f);
									vector4 += vector6;
									vector3 += vector7;
									vector2 += vector6;
									vector5 += vector7;
								}
								goto IL_10e4;
								IL_2a23:
								m_textInfo.characterInfo[m_characterCount].xAdvance = m_xAdvance;
								if (num8 == 13)
								{
									m_xAdvance = tag_Indent;
								}
								if (num8 == 10 || m_characterCount == totalCharacterCount - 1)
								{
									if (m_lineNumber > 0)
									{
										if (!TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender))
										{
											if (m_lineHeight == -32767f && !m_isNewPage)
											{
												float num56 = m_maxLineAscender - m_startOfLineAscender;
												AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num56);
												num28 -= num56;
												m_lineOffset += num56;
											}
										}
									}
									m_isNewPage = false;
									num25 = m_maxLineAscender - m_lineOffset;
									num24 = m_maxLineDescender - m_lineOffset;
									float maxDescender2;
									if (m_maxDescender < num24)
									{
										maxDescender2 = m_maxDescender;
									}
									else
									{
										maxDescender2 = num24;
									}
									m_maxDescender = maxDescender2;
									if (!flag5)
									{
										num19 = m_maxDescender;
									}
									if (m_useMaxVisibleDescender)
									{
										if (m_characterCount < m_maxVisibleCharacters)
										{
											if (m_lineNumber < m_maxVisibleLines)
											{
												goto IL_2b9b;
											}
										}
										flag5 = true;
									}
									goto IL_2b9b;
								}
								goto IL_30c7;
							}
							num7 = m_maxFontSize - m_minFontSize;
							if (!m_isCharacterWrappingEnabled)
							{
								if (m_enableAutoSizing && num7 > 0.051f)
								{
									if (m_fontSize < m_fontSizeMax)
									{
										m_minFontSize = m_fontSize;
										m_fontSize += Mathf.Max((m_maxFontSize - m_fontSize) / 2f, 0.05f);
										m_fontSize = (float)(int)(Mathf.Min(m_fontSize, m_fontSizeMax) * 20f + 0.5f) / 20f;
										if (loopCountA > 20)
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
										GenerateTextMesh();
										return;
									}
								}
							}
							m_isCharacterWrappingEnabled = false;
							if (m_characterCount == 0)
							{
								ClearMesh(true);
								TMPro_EventManager.ON_TEXT_CHANGED(this);
								return;
							}
							int index = m_materialReferences[0].referenceCount * (m_isVolumetricText ? 8 : 4);
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
								if (textAlignment != TextAlignmentOptions.Flush)
								{
									if (textAlignment != TextAlignmentOptions.CenterGeoAligned)
									{
										if (textAlignment == TextAlignmentOptions.BottomFlush || textAlignment == TextAlignmentOptions.BottomGeoAligned)
										{
											goto case TextAlignmentOptions.BottomLeft;
										}
										if (textAlignment != TextAlignmentOptions.BaselineFlush)
										{
											if (textAlignment != TextAlignmentOptions.BaselineGeoAligned)
											{
												if (textAlignment != TextAlignmentOptions.MidlineFlush)
												{
													if (textAlignment != TextAlignmentOptions.MidlineGeoAligned)
													{
														if (textAlignment != TextAlignmentOptions.CaplineFlush)
														{
															if (textAlignment != TextAlignmentOptions.CaplineGeoAligned)
															{
																break;
															}
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
								goto case TextAlignmentOptions.Left;
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
									a = rectTransformCorners[1] + new Vector3(margin.x, 0f - m_textInfo.pageInfo[num14].ascender - margin.y, 0f);
								}
								break;
							case TextAlignmentOptions.Left:
							case TextAlignmentOptions.Center:
							case TextAlignmentOptions.Right:
							case TextAlignmentOptions.Justified:
								if (m_overflowMode != TextOverflowModes.Page)
								{
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_maxAscender + margin.y + num19 - margin.w) / 2f, 0f);
								}
								else
								{
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (m_textInfo.pageInfo[num14].ascender + margin.y + m_textInfo.pageInfo[num14].descender - margin.w) / 2f, 0f);
								}
								break;
							case TextAlignmentOptions.BottomLeft:
							case TextAlignmentOptions.Bottom:
							case TextAlignmentOptions.BottomRight:
							case TextAlignmentOptions.BottomJustified:
								if (m_overflowMode != TextOverflowModes.Page)
								{
									a = rectTransformCorners[0] + new Vector3(margin.x, 0f - num19 + margin.w, 0f);
								}
								else
								{
									a = rectTransformCorners[0] + new Vector3(margin.x, 0f - m_textInfo.pageInfo[num14].descender + margin.w, 0f);
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
							int num57 = 0;
							int num58 = 0;
							int num59 = 0;
							bool flag10 = false;
							bool flag11 = false;
							int num60 = 0;
							int num61 = 0;
							Vector3 lossyScale = transform.lossyScale;
							float num62 = m_previousLossyScaleY = lossyScale.y;
							Color32 color = Color.white;
							Color32 underlineColor = Color.white;
							Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, 0, 64);
							float num63 = 0f;
							float num64 = 0f;
							float num65 = 0f;
							float num66 = 0f;
							float num67 = TMP_Text.k_LargePositiveFloat;
							int num68 = 0;
							float num69 = 0f;
							float num70 = 0f;
							float b3 = 0f;
							TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
							int lineNumber;
							for (int j = 0; j < m_characterCount; num59 = lineNumber, j++)
							{
								TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
								char character2 = characterInfo[j].character;
								lineNumber = characterInfo[j].lineNumber;
								TMP_LineInfo tMP_LineInfo = m_textInfo.lineInfo[lineNumber];
								num58 = lineNumber + 1;
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
									b2 = (m_isRightToLeft ? new Vector3(0f - tMP_LineInfo.maxAdvance, 0f, 0f) : new Vector3(tMP_LineInfo.marginLeft, 0f, 0f));
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
									b2 = (m_isRightToLeft ? new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f) : new Vector3(tMP_LineInfo.marginLeft + tMP_LineInfo.width - tMP_LineInfo.maxAdvance, 0f, 0f));
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
										if (character2 == '\u200b' || character2 == '\u2060')
										{
											break;
										}
										char character3 = characterInfo[tMP_LineInfo.lastCharacterIndex].character;
										bool flag12 = (alignment & (TextAlignmentOptions)16) == (TextAlignmentOptions)16;
										if (!char.IsControl(character3))
										{
											if (lineNumber < m_lineNumber)
											{
												goto IL_42aa;
											}
										}
										if (!flag12)
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
										goto IL_42aa;
									}
									IL_42aa:
									if (lineNumber == num59)
									{
										if (j != 0)
										{
											if (j != m_firstVisibleCharacter)
											{
												float num71;
												if (!m_isRightToLeft)
												{
													num71 = tMP_LineInfo.width - tMP_LineInfo.maxAdvance;
												}
												else
												{
													num71 = tMP_LineInfo.width + tMP_LineInfo.maxAdvance;
												}
												float num72 = num71;
												int num73 = tMP_LineInfo.visibleCharacterCount - 1;
												int num74;
												if (characterInfo[tMP_LineInfo.lastCharacterIndex].isVisible)
												{
													num74 = tMP_LineInfo.spaceCount;
												}
												else
												{
													num74 = tMP_LineInfo.spaceCount - 1;
												}
												int num75 = num74;
												if (flag10)
												{
													num75--;
													num73++;
												}
												float num76 = (num75 <= 0) ? 1f : m_wordWrappingRatios;
												if (num75 < 1)
												{
													num75 = 1;
												}
												if (character2 != '\t')
												{
													if (!char.IsSeparator(character2))
													{
														if (!m_isRightToLeft)
														{
															b2 += new Vector3(num72 * num76 / (float)num73, 0f, 0f);
														}
														else
														{
															b2 -= new Vector3(num72 * num76 / (float)num73, 0f, 0f);
														}
														break;
													}
												}
												if (!m_isRightToLeft)
												{
													b2 += new Vector3(num72 * (1f - num76) / (float)num75, 0f, 0f);
												}
												else
												{
													b2 -= new Vector3(num72 * (1f - num76) / (float)num75, 0f, 0f);
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
									if (elementType != 0)
									{
										if (elementType != TMP_TextElementType.Sprite)
										{
										}
									}
									else
									{
										Extents lineExtents = tMP_LineInfo.lineExtents;
										float num77 = m_uvLineOffset * (float)lineNumber % 1f;
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
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num77;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num77;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num77;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num77;
											}
											else
											{
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
											}
											break;
										case TextureMappingOptions.Paragraph:
											characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
											characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
											characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
											characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + b2.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num77;
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
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num77;
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num77;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											case TextureMappingOptions.Paragraph:
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num77;
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num77;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											case TextureMappingOptions.MatchAspect:
												Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
												break;
											}
											float num78 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
											characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num78 + num77;
											characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
											characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num78 + num77;
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
											float num79 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
											characterInfo[j].vertex_BL.uv2.y = num79 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
											characterInfo[j].vertex_TL.uv2.y = num79 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
											characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
											characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
											break;
										}
										}
										num63 = characterInfo[j].scale * num62 * (1f - m_charWidthAdjDelta);
										if (!characterInfo[j].isUsingAlternateTypeface)
										{
											if ((characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
											{
												num63 *= -1f;
											}
										}
										float x = characterInfo[j].vertex_BL.uv2.x;
										float y = characterInfo[j].vertex_BL.uv2.y;
										float x2 = characterInfo[j].vertex_TR.uv2.x;
										float y2 = characterInfo[j].vertex_TR.uv2.y;
										float num80 = (int)x;
										float num81 = (int)y;
										x -= num80;
										x2 -= num80;
										y -= num81;
										y2 -= num81;
										characterInfo[j].vertex_BL.uv2.x = PackUV(x, y);
										characterInfo[j].vertex_BL.uv2.y = num63;
										characterInfo[j].vertex_TL.uv2.x = PackUV(x, y2);
										characterInfo[j].vertex_TL.uv2.y = num63;
										characterInfo[j].vertex_TR.uv2.x = PackUV(x2, y2);
										characterInfo[j].vertex_TR.uv2.y = num63;
										characterInfo[j].vertex_BR.uv2.x = PackUV(x2, y);
										characterInfo[j].vertex_BR.uv2.y = num63;
									}
									if (j < m_maxVisibleCharacters)
									{
										if (num57 < m_maxVisibleWords && lineNumber < m_maxVisibleLines)
										{
											if (m_overflowMode != TextOverflowModes.Page)
											{
												characterInfo[j].vertex_BL.position += zero3;
												characterInfo[j].vertex_TL.position += zero3;
												characterInfo[j].vertex_TR.position += zero3;
												characterInfo[j].vertex_BR.position += zero3;
												goto IL_5781;
											}
										}
									}
									if (j < m_maxVisibleCharacters)
									{
										if (num57 < m_maxVisibleWords)
										{
											if (lineNumber < m_maxVisibleLines)
											{
												if (m_overflowMode == TextOverflowModes.Page)
												{
													if (characterInfo[j].pageNumber == num14)
													{
														characterInfo[j].vertex_BL.position += zero3;
														characterInfo[j].vertex_TL.position += zero3;
														characterInfo[j].vertex_TR.position += zero3;
														characterInfo[j].vertex_BR.position += zero3;
														goto IL_5781;
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
									goto IL_5781;
								}
								goto IL_57b0;
								IL_6015:
								bool flag13 = false;
								goto IL_6018;
								IL_6018:
								int pageNumber;
								if (!char.IsWhiteSpace(character2))
								{
									if (character2 != '\u200b')
									{
										num66 = Mathf.Max(num66, m_textInfo.characterInfo[j].scale);
										num67 = Mathf.Min((pageNumber != num68) ? TMP_Text.k_LargePositiveFloat : num67, m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num66);
										num68 = pageNumber;
									}
								}
								if (!flag && flag13 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
								{
									if (j == tMP_LineInfo.lastVisibleCharacterIndex)
									{
										if (char.IsSeparator(character2))
										{
											goto IL_617c;
										}
									}
									flag = true;
									num64 = m_textInfo.characterInfo[j].scale;
									if (num66 == 0f)
									{
										num66 = num64;
									}
									start = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, num67, 0f);
									color = m_textInfo.characterInfo[j].underlineColor;
								}
								goto IL_617c;
								IL_6b53:
								bool flag14;
								if (!flag3)
								{
									if (flag14)
									{
										if (j <= tMP_LineInfo.lastVisibleCharacterIndex)
										{
											if (character2 != '\n')
											{
												if (character2 != '\r')
												{
													if (j == tMP_LineInfo.lastVisibleCharacterIndex && char.IsSeparator(character2))
													{
													}
													else
													{
														flag3 = true;
														start3 = TMP_Text.k_LargePositiveVector2;
														vector = TMP_Text.k_LargeNegativeVector2;
														color2 = m_textInfo.characterInfo[j].highlightColor;
													}
												}
											}
										}
									}
								}
								if (flag3)
								{
									Color32 highlightColor = m_textInfo.characterInfo[j].highlightColor;
									bool flag15 = false;
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
										flag15 = true;
									}
									if (!flag15)
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
									if (j != tMP_LineInfo.lastCharacterIndex)
									{
										if (j < tMP_LineInfo.lastVisibleCharacterIndex)
										{
											goto IL_6e70;
										}
									}
									flag3 = false;
									DrawTextHighlight(start3, vector, ref index, color2);
									continue;
								}
								goto IL_6e70;
								IL_5e62:
								int num82;
								if (j == m_characterCount - 1)
								{
									if (char.IsLetterOrDigit(character2))
									{
										num82 = j;
										goto IL_5e93;
									}
								}
								num82 = j - 1;
								goto IL_5e93;
								IL_617c:
								if (flag && m_characterCount == 1)
								{
									flag = false;
									zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num67, 0f);
									num65 = m_textInfo.characterInfo[j].scale;
									DrawUnderlineMesh(start, zero, ref index, num64, num65, num66, num63, color);
									num66 = 0f;
									num67 = TMP_Text.k_LargePositiveFloat;
								}
								else
								{
									if (!flag)
									{
										goto IL_62f1;
									}
									if (j != tMP_LineInfo.lastCharacterIndex)
									{
										if (j < tMP_LineInfo.lastVisibleCharacterIndex)
										{
											goto IL_62f1;
										}
									}
									if (char.IsWhiteSpace(character2) || character2 == '\u200b')
									{
										int lastVisibleCharacterIndex = tMP_LineInfo.lastVisibleCharacterIndex;
										zero = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num67, 0f);
										num65 = m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
									}
									else
									{
										zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num67, 0f);
										num65 = m_textInfo.characterInfo[j].scale;
									}
									flag = false;
									DrawUnderlineMesh(start, zero, ref index, num64, num65, num66, num63, color);
									num66 = 0f;
									num67 = TMP_Text.k_LargePositiveFloat;
								}
								goto IL_64af;
								IL_5f83:
								if ((m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
								{
									flag13 = true;
									pageNumber = m_textInfo.characterInfo[j].pageNumber;
									if (j <= m_maxVisibleCharacters)
									{
										if (lineNumber <= m_maxVisibleLines)
										{
											if (m_overflowMode == TextOverflowModes.Page)
											{
												if (pageNumber + 1 != m_pageToDisplay)
												{
													goto IL_6015;
												}
											}
											goto IL_6018;
										}
									}
									goto IL_6015;
								}
								if (flag)
								{
									flag = false;
									zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num67, 0f);
									num65 = m_textInfo.characterInfo[j - 1].scale;
									DrawUnderlineMesh(start, zero, ref index, num64, num65, num66, num63, color);
									num66 = 0f;
									num67 = TMP_Text.k_LargePositiveFloat;
								}
								goto IL_64af;
								IL_6545:
								bool flag16 = false;
								goto IL_6548;
								IL_5bd2:
								if (!char.IsLetterOrDigit(character2))
								{
									if (character2 != '-')
									{
										if (character2 != '­' && character2 != '‐')
										{
											if (character2 != '‑')
											{
												if (!flag11)
												{
													if (j != 0)
													{
														goto IL_5f83;
													}
													if (char.IsPunctuation(character2))
													{
														if (!char.IsWhiteSpace(character2))
														{
															if (character2 != '\u200b')
															{
																if (j != m_characterCount - 1)
																{
																	goto IL_5f83;
																}
															}
														}
													}
												}
												if (j > 0)
												{
													if (j < characterInfo.Length - 1)
													{
														if (j < m_characterCount)
														{
															if (character2 != '\'')
															{
																if (character2 != '’')
																{
																	goto IL_5e62;
																}
															}
															if (char.IsLetterOrDigit(characterInfo[j - 1].character))
															{
																if (char.IsLetterOrDigit(characterInfo[j + 1].character))
																{
																	goto IL_5f83;
																}
															}
														}
													}
												}
												goto IL_5e62;
											}
										}
									}
								}
								if (!flag11)
								{
									flag11 = true;
									num60 = j;
								}
								if (flag11)
								{
									if (j == m_characterCount - 1)
									{
										int num83 = m_textInfo.wordInfo.Length;
										int wordCount = m_textInfo.wordCount;
										if (m_textInfo.wordCount + 1 > num83)
										{
											TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num83 + 1);
										}
										num61 = j;
										m_textInfo.wordInfo[wordCount].firstCharacterIndex = num60;
										m_textInfo.wordInfo[wordCount].lastCharacterIndex = num61;
										m_textInfo.wordInfo[wordCount].characterCount = num61 - num60 + 1;
										m_textInfo.wordInfo[wordCount].textComponent = this;
										num57++;
										m_textInfo.wordCount++;
										m_textInfo.lineInfo[lineNumber].wordCount++;
									}
								}
								goto IL_5f83;
								IL_67c6:
								flag2 = false;
								DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
								goto IL_6ac0;
								IL_5e93:
								num61 = num82;
								flag11 = false;
								int num84 = m_textInfo.wordInfo.Length;
								int wordCount2 = m_textInfo.wordCount;
								if (m_textInfo.wordCount + 1 > num84)
								{
									TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num84 + 1);
								}
								m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num60;
								m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num61;
								m_textInfo.wordInfo[wordCount2].characterCount = num61 - num60 + 1;
								m_textInfo.wordInfo[wordCount2].textComponent = this;
								num57++;
								m_textInfo.wordCount++;
								m_textInfo.lineInfo[lineNumber].wordCount++;
								goto IL_5f83;
								IL_6e70:
								if (!flag3)
								{
									continue;
								}
								if (!flag14)
								{
									flag3 = false;
									DrawTextHighlight(start3, vector, ref index, color2);
								}
								continue;
								IL_6ac0:
								if ((m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
								{
									flag14 = true;
									int pageNumber2 = m_textInfo.characterInfo[j].pageNumber;
									if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines)
									{
										goto IL_6b50;
									}
									if (m_overflowMode == TextOverflowModes.Page)
									{
										if (pageNumber2 + 1 != m_pageToDisplay)
										{
											goto IL_6b50;
										}
									}
									goto IL_6b53;
								}
								if (flag3)
								{
									flag3 = false;
									DrawTextHighlight(start3, vector, ref index, color2);
								}
								continue;
								IL_5781:
								if (elementType == TMP_TextElementType.Character)
								{
									FillCharacterVertexBuffers(j, index_X, m_isVolumetricText);
								}
								else if (elementType == TMP_TextElementType.Sprite)
								{
									FillSpriteVertexBuffers(j, index_X2);
								}
								goto IL_57b0;
								IL_6b50:
								flag14 = false;
								goto IL_6b53;
								IL_6548:
								float strikethrough;
								if (!flag2 && flag16)
								{
									if (j <= tMP_LineInfo.lastVisibleCharacterIndex)
									{
										if (character2 != '\n' && character2 != '\r')
										{
											if (j == tMP_LineInfo.lastVisibleCharacterIndex && char.IsSeparator(character2))
											{
											}
											else
											{
												flag2 = true;
												num69 = m_textInfo.characterInfo[j].pointSize;
												num70 = m_textInfo.characterInfo[j].scale;
												start2 = new Vector3(m_textInfo.characterInfo[j].bottomLeft.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num70, 0f);
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
										zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num70, 0f);
										DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
										goto IL_6ac0;
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
												zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num70, 0f);
												goto IL_67c6;
											}
										}
										int lastVisibleCharacterIndex2 = tMP_LineInfo.lastVisibleCharacterIndex;
										zero2 = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num70, 0f);
										goto IL_67c6;
									}
								}
								if (!flag2 || j >= m_characterCount)
								{
									goto IL_6925;
								}
								if (m_textInfo.characterInfo[j + 1].pointSize == num69)
								{
									if (TMP_Math.Approximately(m_textInfo.characterInfo[j + 1].baseLine + zero3.y, b3))
									{
										goto IL_6925;
									}
								}
								flag2 = false;
								int lastVisibleCharacterIndex3 = tMP_LineInfo.lastVisibleCharacterIndex;
								if (j <= lastVisibleCharacterIndex3)
								{
									zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num70, 0f);
								}
								else
								{
									zero2 = new Vector3(m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num70, 0f);
								}
								DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
								goto IL_6ac0;
								IL_62f1:
								if (flag)
								{
									if (!flag13)
									{
										flag = false;
										zero = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, num67, 0f);
										num65 = m_textInfo.characterInfo[j - 1].scale;
										DrawUnderlineMesh(start, zero, ref index, num64, num65, num66, num63, color);
										num66 = 0f;
										num67 = TMP_Text.k_LargePositiveFloat;
										goto IL_64af;
									}
								}
								if (flag)
								{
									if (j < m_characterCount - 1 && !color.Compare(m_textInfo.characterInfo[j + 1].underlineColor))
									{
										flag = false;
										zero = new Vector3(m_textInfo.characterInfo[j].topRight.x, num67, 0f);
										num65 = m_textInfo.characterInfo[j].scale;
										DrawUnderlineMesh(start, zero, ref index, num64, num65, num66, num63, color);
										num66 = 0f;
										num67 = TMP_Text.k_LargePositiveFloat;
									}
								}
								goto IL_64af;
								IL_6925:
								if (flag2)
								{
									if (j < m_characterCount)
									{
										if (fontAsset.GetInstanceID() != characterInfo[j + 1].fontAsset.GetInstanceID())
										{
											flag2 = false;
											zero2 = new Vector3(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num70, 0f);
											DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
											goto IL_6ac0;
										}
									}
								}
								if (flag2)
								{
									if (!flag16)
									{
										flag2 = false;
										zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num70, 0f);
										DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
									}
								}
								goto IL_6ac0;
								IL_64af:
								bool flag17 = (m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
								strikethrough = fontAsset.fontInfo.strikethrough;
								if (flag17)
								{
									flag16 = true;
									if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines)
									{
										goto IL_6545;
									}
									if (m_overflowMode == TextOverflowModes.Page)
									{
										if (m_textInfo.characterInfo[j].pageNumber + 1 != m_pageToDisplay)
										{
											goto IL_6545;
										}
									}
									goto IL_6548;
								}
								if (flag2)
								{
									flag2 = false;
									zero2 = new Vector3(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num70, 0f);
									DrawUnderlineMesh(start2, zero2, ref index, num70, num70, num70, num63, underlineColor);
								}
								goto IL_6ac0;
								IL_57b0:
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
								if (lineNumber == num59)
								{
									if (j != m_characterCount - 1)
									{
										goto IL_5bd2;
									}
								}
								if (lineNumber != num59)
								{
									m_textInfo.lineInfo[num59].baseline += zero3.y;
									m_textInfo.lineInfo[num59].ascender += zero3.y;
									m_textInfo.lineInfo[num59].descender += zero3.y;
									m_textInfo.lineInfo[num59].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num59].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[num59].descender);
									m_textInfo.lineInfo[num59].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num59].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[num59].ascender);
								}
								if (j == m_characterCount - 1)
								{
									m_textInfo.lineInfo[lineNumber].baseline += zero3.y;
									m_textInfo.lineInfo[lineNumber].ascender += zero3.y;
									m_textInfo.lineInfo[lineNumber].descender += zero3.y;
									m_textInfo.lineInfo[lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[lineNumber].descender);
									m_textInfo.lineInfo[lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[lineNumber].ascender);
								}
								goto IL_5bd2;
							}
							while (true)
							{
								m_textInfo.characterCount = (short)m_characterCount;
								m_textInfo.spriteCount = m_spriteCount;
								m_textInfo.lineCount = (short)num58;
								TMP_TextInfo textInfo = m_textInfo;
								int wordCount3;
								if (num57 != 0)
								{
									if (m_characterCount > 0)
									{
										wordCount3 = (short)num57;
										goto IL_6f3d;
									}
								}
								wordCount3 = 1;
								goto IL_6f3d;
								IL_6f3d:
								textInfo.wordCount = wordCount3;
								m_textInfo.pageCount = m_pageNumber + 1;
								if (m_renderMode == TextRenderFlags.Render)
								{
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
									}
								}
								TMPro_EventManager.ON_TEXT_CHANGED(this);
								return;
							}
						}
					}
					ClearMesh(true);
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

		private void SetMeshFilters(bool state)
		{
			if (m_meshFilter != null)
			{
				if (state)
				{
					m_meshFilter.sharedMesh = m_mesh;
				}
				else
				{
					m_meshFilter.sharedMesh = null;
				}
			}
			int num = 1;
			while (num < m_subTextObjects.Length)
			{
				while (true)
				{
					if (!(m_subTextObjects[num] != null))
					{
						return;
					}
					if (m_subTextObjects[num].meshFilter != null)
					{
						if (state)
						{
							m_subTextObjects[num].meshFilter.sharedMesh = m_subTextObjects[num].mesh;
						}
						else
						{
							m_subTextObjects[num].meshFilter.sharedMesh = null;
						}
					}
					num++;
					goto IL_00b5;
				}
				IL_00b5:;
			}
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			int num = 1;
			while (num < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num] != null)
					{
						if (m_subTextObjects[num].enabled != state)
						{
							m_subTextObjects[num].enabled = state;
						}
						num++;
						goto IL_0039;
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
				IL_0039:;
			}
		}

		protected override void ClearSubMeshObjects()
		{
			int num = 1;
			while (num < m_subTextObjects.Length)
			{
				while (true)
				{
					if (m_subTextObjects[num] != null)
					{
						Debug.Log(new StringBuilder().Append("Destroying Sub Text object[").Append(num).Append("].").ToString());
						UnityEngine.Object.DestroyImmediate(m_subTextObjects[num]);
						num++;
						goto IL_0031;
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
				IL_0031:;
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
				float num = lossyScale * m_textInfo.characterInfo[i].scale * (1f - m_charWidthAdjDelta);
				if (!m_textInfo.characterInfo[i].isUsingAlternateTypeface)
				{
					if ((m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num *= -1f;
					}
				}
				int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
				int vertexIndex = m_textInfo.characterInfo[i].vertexIndex;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num;
				m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num;
			}
			while (true)
			{
				for (int j = 0; j < m_textInfo.meshInfo.Length; j++)
				{
					if (j == 0)
					{
						m_mesh.uv2 = m_textInfo.meshInfo[0].uvs2;
					}
					else
					{
						m_subTextObjects[j].mesh.uv2 = m_textInfo.meshInfo[j].uvs2;
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

		public void SetMask(MaskingTypes type, Vector4 maskCoords)
		{
			SetMask(type);
			SetMaskCoordinates(maskCoords);
		}

		public void SetMask(MaskingTypes type, Vector4 maskCoords, float softnessX, float softnessY)
		{
			SetMask(type);
			SetMaskCoordinates(maskCoords, softnessX, softnessY);
		}

		public override void SetVerticesDirty()
		{
			if (m_verticesAlreadyDirty)
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
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					TMP_UpdateManager.RegisterTextElementForGraphicRebuild(this);
					m_verticesAlreadyDirty = true;
					return;
				}
			}
		}

		public override void SetLayoutDirty()
		{
			m_isPreferredWidthDirty = true;
			m_isPreferredHeightDirty = true;
			if (m_layoutAlreadyDirty || this == null)
			{
				return;
			}
			if (!IsActive())
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
			m_layoutAlreadyDirty = true;
			m_isLayoutDirty = true;
		}

		public override void SetMaterialDirty()
		{
			UpdateMaterial();
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
			if (update == CanvasUpdate.Prelayout)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (m_autoSizeTextContainer)
						{
							m_rectTransform.sizeDelta = GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
						}
						return;
					}
				}
			}
			if (update != CanvasUpdate.PreRender)
			{
				return;
			}
			while (true)
			{
				OnPreRenderObject();
				m_verticesAlreadyDirty = false;
				m_layoutAlreadyDirty = false;
				if (!m_isMaterialDirty)
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
				UpdateMaterial();
				m_isMaterialDirty = false;
				return;
			}
		}

		protected override void UpdateMaterial()
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
						return;
					}
				}
			}
			if (m_renderer == null)
			{
				m_renderer = renderer;
			}
			if (m_renderer.sharedMaterial.GetInstanceID() == m_sharedMaterial.GetInstanceID())
			{
				return;
			}
			while (true)
			{
				m_renderer.sharedMaterial = m_sharedMaterial;
				return;
			}
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
					switch (6)
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
		}

		public override void ForceMeshUpdate()
		{
			m_havePropertiesChanged = true;
			OnPreRenderObject();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			m_havePropertiesChanged = true;
			m_ignoreActiveState = true;
			OnPreRenderObject();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			m_renderMode = TextRenderFlags.DontRender;
			ComputeMarginSize();
			GenerateTextMesh();
			m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh(bool updateMesh)
		{
			if (m_textInfo.meshInfo[0].mesh == null)
			{
				m_textInfo.meshInfo[0].mesh = m_mesh;
			}
			m_textInfo.ClearMeshInfo(updateMesh);
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
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
			}
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
			}
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

		public void UpdateFontAsset()
		{
			LoadFontAsset();
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				while (true)
				{
					return;
				}
			}
			m_currentAutoSizeMode = m_enableAutoSizing;
			if (!m_isCalculateSizeRequired)
			{
				if (!m_rectTransform.hasChanged)
				{
					return;
				}
			}
			m_minWidth = 0f;
			m_flexibleWidth = 0f;
			if (m_enableAutoSizing)
			{
				m_fontSize = m_fontSizeMax;
			}
			m_marginWidth = TMP_Text.k_LargePositiveFloat;
			m_marginHeight = TMP_Text.k_LargePositiveFloat;
			if (!m_isInputParsingRequired)
			{
				if (!m_isTextTruncated)
				{
					goto IL_00c7;
				}
			}
			ParseInputText();
			goto IL_00c7;
			IL_00c7:
			GenerateTextMesh();
			m_renderMode = TextRenderFlags.Render;
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
					goto IL_00b9;
				}
			}
			m_minHeight = 0f;
			m_flexibleHeight = 0f;
			if (m_enableAutoSizing)
			{
				m_currentAutoSizeMode = true;
				m_enableAutoSizing = false;
			}
			m_marginHeight = TMP_Text.k_LargePositiveFloat;
			GenerateTextMesh();
			m_enableAutoSizing = m_currentAutoSizeMode;
			m_renderMode = TextRenderFlags.Render;
			ComputeMarginSize();
			m_isLayoutDirty = true;
			goto IL_00b9;
			IL_00b9:
			m_isCalculateSizeRequired = false;
		}
	}
}
