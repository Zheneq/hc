using System;
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

		protected override void Awake()
		{
			this.m_renderer = base.GetComponent<Renderer>();
			if (this.m_renderer == null)
			{
				this.m_renderer = base.gameObject.AddComponent<Renderer>();
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
			this.m_rectTransform = base.rectTransform;
			this.m_transform = this.transform;
			this.m_meshFilter = base.GetComponent<MeshFilter>();
			if (this.m_meshFilter == null)
			{
				this.m_meshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this.m_mesh == null)
			{
				this.m_mesh = new Mesh();
				this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
				this.m_meshFilter.mesh = this.m_mesh;
			}
			this.m_meshFilter.hideFlags = HideFlags.HideInInspector;
			base.LoadDefaultSettings();
			this.LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			if (this.m_char_buffer == null)
			{
				this.m_char_buffer = new int[this.m_max_characters];
			}
			this.m_cached_TextElement = new TMP_Glyph();
			this.m_isFirstAllocation = true;
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo(this);
			}
			if (this.m_fontAsset == null)
			{
				Debug.LogWarning("Please assign a Font Asset to this " + this.transform.name + " gameobject.", this);
				return;
			}
			TMP_SubMesh[] componentsInChildren = base.GetComponentsInChildren<TMP_SubMesh>();
			if (componentsInChildren.Length > 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.m_subTextObjects[i + 1] = componentsInChildren[i];
				}
			}
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.m_isAwake = true;
		}

		protected override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
			{
				this.m_isRegisteredForEvents = true;
			}
			this.meshFilter.sharedMesh = this.mesh;
			this.SetActiveSubMeshes(true);
			this.ComputeMarginSize();
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_verticesAlreadyDirty = false;
			this.SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
			this.m_meshFilter.sharedMesh = null;
			this.SetActiveSubMeshes(false);
		}

		protected override void OnDestroy()
		{
			if (this.m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
			this.m_isRegisteredForEvents = false;
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
		}

		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontAsset == null)
			{
				if (TMP_Settings.defaultFontAsset != null)
				{
					this.m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					this.m_fontAsset = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				}
				if (this.m_fontAsset == null)
				{
					Debug.LogWarning("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to " + base.gameObject.name + ".", this);
					return;
				}
				if (this.m_fontAsset.characterDictionary == null)
				{
					Debug.Log("Dictionary is Null!");
				}
				this.m_renderer.sharedMaterial = this.m_fontAsset.material;
				this.m_sharedMaterial = this.m_fontAsset.material;
				this.m_sharedMaterial.SetFloat("_CullMode", 0f);
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				this.m_renderer.receiveShadows = false;
				this.m_renderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			else
			{
				if (this.m_fontAsset.characterDictionary == null)
				{
					this.m_fontAsset.ReadFontDefinition();
				}
				if (!(this.m_renderer.sharedMaterial == null) && !(this.m_renderer.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null))
				{
					if (this.m_fontAsset.atlas.GetInstanceID() == this.m_renderer.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
					{
						this.m_sharedMaterial = this.m_renderer.sharedMaterial;
						goto IL_20C;
					}
				}
				this.m_renderer.sharedMaterial = this.m_fontAsset.material;
				this.m_sharedMaterial = this.m_fontAsset.material;
				IL_20C:
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				if (this.m_sharedMaterial.passCount == 1)
				{
					this.m_renderer.receiveShadows = false;
					this.m_renderer.shadowCastingMode = ShadowCastingMode.Off;
				}
			}
			this.m_padding = this.GetPaddingForMaterial();
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			base.GetSpecialCharacters(this.m_fontAsset);
		}

		private void UpdateEnvMapMatrix()
		{
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap))
			{
				if (!(this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null))
				{
					Vector3 euler = this.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
					this.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
					this.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, this.m_EnvMapMatrix);
					return;
				}
			}
		}

		private void SetMask(MaskingTypes maskType)
		{
			if (maskType != MaskingTypes.MaskOff)
			{
				if (maskType != MaskingTypes.MaskSoft)
				{
					if (maskType != MaskingTypes.MaskHard)
					{
					}
					else
					{
						this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_HARD);
						this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
						this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
					}
				}
				else
				{
					this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
					this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
					this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				}
			}
			else
			{
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
			}
		}

		private void SetMaskCoordinates(Vector4 coords)
		{
			this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
		}

		private void SetMaskCoordinates(Vector4 coords, float softX, float softY)
		{
			this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, softX);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, softY);
		}

		private void EnableMasking()
		{
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.m_isMaskingEnabled = true;
				this.UpdateMask();
			}
		}

		private void DisableMasking()
		{
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.m_isMaskingEnabled = false;
				this.UpdateMask();
			}
		}

		private void UpdateMask()
		{
			if (!this.m_isMaskingEnabled)
			{
				return;
			}
			if (this.m_isMaskingEnabled)
			{
				if (this.m_fontMaterial == null)
				{
					this.CreateMaterialInstance();
				}
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			if (!(this.m_fontMaterial == null))
			{
				if (this.m_fontMaterial.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_53;
				}
			}
			this.m_fontMaterial = this.CreateMaterialInstance(mat);
			IL_53:
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontMaterials == null)
			{
				this.m_fontMaterials = new Material[materialCount];
			}
			else if (this.m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					this.m_fontMaterials[i] = this.m_subTextObjects[i].material;
				}
			}
			this.m_fontSharedMaterials = this.m_fontMaterials;
			return this.m_fontMaterials;
		}

		protected override void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		protected override Material[] GetSharedMaterials()
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					this.m_fontSharedMaterials[i] = this.m_sharedMaterial;
				}
				else
				{
					this.m_fontSharedMaterials[i] = this.m_subTextObjects[i].sharedMaterial;
				}
			}
			return this.m_fontSharedMaterials;
		}

		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
			{
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				Texture texture = materials[i].GetTexture(ShaderUtilities.ID_MainTex);
				if (i == 0)
				{
					if (!(texture == null))
					{
						if (texture.GetInstanceID() != this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
						}
						else
						{
							this.m_sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
							this.m_padding = this.GetPaddingForMaterial(this.m_sharedMaterial);
						}
					}
				}
				else if (!(texture == null))
				{
					if (texture.GetInstanceID() != this.m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
					{
					}
					else if (this.m_subTextObjects[i].isDefaultMaterial)
					{
						this.m_subTextObjects[i].sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			thickness = Mathf.Clamp01(thickness);
			this.m_renderer.material.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_fontMaterial = this.m_renderer.material;
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			this.m_renderer.material.SetColor(ShaderUtilities.ID_FaceColor, color);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_sharedMaterial = this.m_fontMaterial;
		}

		protected override void SetOutlineColor(Color32 color)
		{
			this.m_renderer.material.SetColor(ShaderUtilities.ID_OutlineColor, color);
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.m_renderer.material;
			}
			this.m_sharedMaterial = this.m_fontMaterial;
		}

		private void CreateMaterialInstance()
		{
			Material material = new Material(this.m_sharedMaterial);
			material.shaderKeywords = this.m_sharedMaterial.shaderKeywords;
			Material material2 = material;
			material2.name += " Instance";
			this.m_fontMaterial = material;
		}

		protected override void SetShaderDepth()
		{
			if (this.m_isOverlay)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
				this.m_renderer.material.renderQueue = 0xFA0;
				this.m_sharedMaterial = this.m_renderer.material;
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				this.m_renderer.material.renderQueue = -1;
				this.m_sharedMaterial = this.m_renderer.material;
			}
		}

		protected override void SetCulling()
		{
			if (this.m_isCullingEnabled)
			{
				this.m_renderer.material.SetFloat("_CullMode", 2f);
				int i = 1;
				while (i < this.m_subTextObjects.Length)
				{
					if (!(this.m_subTextObjects[i] != null))
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_99;
						}
					}
					else
					{
						Renderer renderer = this.m_subTextObjects[i].renderer;
						if (renderer != null)
						{
							renderer.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
						}
						i++;
					}
				}
				IL_99:;
			}
			else
			{
				this.m_renderer.material.SetFloat("_CullMode", 0f);
				int j = 1;
				while (j < this.m_subTextObjects.Length)
				{
					if (!(this.m_subTextObjects[j] != null))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							return;
						}
					}
					else
					{
						Renderer renderer2 = this.m_subTextObjects[j].renderer;
						if (renderer2 != null)
						{
							renderer2.material.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
						}
						j++;
					}
				}
			}
		}

		private void SetPerspectiveCorrection()
		{
			if (this.m_isOrthographic)
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
		}

		protected override float GetPaddingForMaterial(Material mat)
		{
			this.m_padding = ShaderUtilities.GetPadding(mat, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		protected override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_sharedMaterial == null)
			{
				return 0f;
			}
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		protected override int SetArraySizes(int[] chars)
		{
			int num = 0;
			int num2 = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			this.tag_NoParsing = false;
			this.m_style = this.m_fontStyle;
			this.m_fontWeightInternal = (((this.m_style & FontStyles.Bold) != FontStyles.Bold) ? this.m_fontWeight : 0x2BC);
			this.m_fontWeightStack.SetDefault(this.m_fontWeightInternal);
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(this.m_currentMaterialIndex, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			if (this.m_textInfo == null)
			{
				this.m_textInfo = new TMP_TextInfo();
			}
			this.m_textElementType = TMP_TextElementType.Character;
			if (this.m_linkedTextComponent != null)
			{
				this.m_linkedTextComponent.text = string.Empty;
				this.m_linkedTextComponent.ForceMeshUpdate();
			}
			int i = 0;
			while (i < chars.Length)
			{
				if (chars[i] != 0)
				{
					if (this.m_textInfo.characterInfo == null)
					{
						goto IL_15F;
					}
					if (this.m_totalCharacterCount >= this.m_textInfo.characterInfo.Length)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_15F;
						}
					}
					IL_178:
					int num3 = chars[i];
					if (!this.m_isRichText)
					{
						goto IL_301;
					}
					if (num3 != 0x3C)
					{
						goto IL_301;
					}
					int currentMaterialIndex = this.m_currentMaterialIndex;
					if (!base.ValidateHtmlTag(chars, i + 1, out num))
					{
						goto IL_301;
					}
					i = num;
					if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
					{
						this.m_isUsingBold = true;
					}
					if (this.m_textElementType == TMP_TextElementType.Sprite)
					{
						MaterialReference[] materialReferences = this.m_materialReferences;
						int currentMaterialIndex2 = this.m_currentMaterialIndex;
						materialReferences[currentMaterialIndex2].referenceCount = materialReferences[currentMaterialIndex2].referenceCount + 1;
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)(0xE000 + this.m_spriteIndex);
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = this.m_spriteIndex;
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = this.m_currentSpriteAsset;
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
						this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
						this.m_textElementType = TMP_TextElementType.Character;
						this.m_currentMaterialIndex = currentMaterialIndex;
						num2++;
						this.m_totalCharacterCount++;
					}
					IL_B47:
					i++;
					continue;
					IL_301:
					bool flag = false;
					bool isUsingAlternateTypeface = false;
					TMP_FontAsset currentFontAsset = this.m_currentFontAsset;
					Material currentMaterial = this.m_currentMaterial;
					int currentMaterialIndex3 = this.m_currentMaterialIndex;
					if (this.m_textElementType == TMP_TextElementType.Character)
					{
						if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
						{
							if (char.IsLower((char)num3))
							{
								num3 = (int)char.ToUpper((char)num3);
							}
						}
						else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
						{
							if (char.IsUpper((char)num3))
							{
								num3 = (int)char.ToLower((char)num3);
							}
						}
						else
						{
							if ((this.m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
							{
								if ((this.m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
								{
									goto IL_3D7;
								}
							}
							if (char.IsLower((char)num3))
							{
								num3 = (int)char.ToUpper((char)num3);
							}
						}
					}
					IL_3D7:
					TMP_FontAsset tmp_FontAsset = base.GetFontAssetForWeight(this.m_fontWeightInternal);
					if (tmp_FontAsset != null)
					{
						flag = true;
						isUsingAlternateTypeface = true;
						this.m_currentFontAsset = tmp_FontAsset;
					}
					TMP_Glyph tmp_Glyph;
					tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num3, out tmp_Glyph);
					if (tmp_Glyph == null)
					{
						TMP_SpriteAsset tmp_SpriteAsset = base.spriteAsset;
						if (tmp_SpriteAsset != null)
						{
							int num4 = -1;
							tmp_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset, num3, out num4);
							if (num4 != -1)
							{
								this.m_textElementType = TMP_TextElementType.Sprite;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
								this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset.material, tmp_SpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
								MaterialReference[] materialReferences2 = this.m_materialReferences;
								int currentMaterialIndex4 = this.m_currentMaterialIndex;
								materialReferences2[currentMaterialIndex4].referenceCount = materialReferences2[currentMaterialIndex4].referenceCount + 1;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num3;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num4;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
								this.m_textElementType = TMP_TextElementType.Character;
								this.m_currentMaterialIndex = currentMaterialIndex3;
								num2++;
								this.m_totalCharacterCount++;
								goto IL_B47;
							}
						}
					}
					if (tmp_Glyph == null)
					{
						if (TMP_Settings.fallbackFontAssets != null)
						{
							if (TMP_Settings.fallbackFontAssets.Count > 0)
							{
								tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num3, out tmp_Glyph);
							}
						}
					}
					if (tmp_Glyph == null)
					{
						if (TMP_Settings.defaultFontAsset != null)
						{
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num3, out tmp_Glyph);
						}
					}
					if (tmp_Glyph == null)
					{
						TMP_SpriteAsset tmp_SpriteAsset2 = TMP_Settings.defaultSpriteAsset;
						if (tmp_SpriteAsset2 != null)
						{
							int num5 = -1;
							tmp_SpriteAsset2 = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset2, num3, out num5);
							if (num5 != -1)
							{
								this.m_textElementType = TMP_TextElementType.Sprite;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
								this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset2.material, tmp_SpriteAsset2, this.m_materialReferences, this.m_materialReferenceIndexLookup);
								MaterialReference[] materialReferences3 = this.m_materialReferences;
								int currentMaterialIndex5 = this.m_currentMaterialIndex;
								materialReferences3[currentMaterialIndex5].referenceCount = materialReferences3[currentMaterialIndex5].referenceCount + 1;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num3;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num5;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset2;
								this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
								this.m_textElementType = TMP_TextElementType.Character;
								this.m_currentMaterialIndex = currentMaterialIndex3;
								num2++;
								this.m_totalCharacterCount++;
								goto IL_B47;
							}
						}
					}
					if (tmp_Glyph == null)
					{
						int num6 = i;
						int num7;
						if (TMP_Settings.missingGlyphCharacter == 0)
						{
							num7 = 0x25A1;
						}
						else
						{
							num7 = TMP_Settings.missingGlyphCharacter;
						}
						num3 = (chars[num6] = num7);
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num3, out tmp_Glyph);
						if (tmp_Glyph == null)
						{
							if (TMP_Settings.fallbackFontAssets != null)
							{
								if (TMP_Settings.fallbackFontAssets.Count > 0)
								{
									tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num3, out tmp_Glyph);
								}
							}
						}
						if (tmp_Glyph == null)
						{
							if (TMP_Settings.defaultFontAsset != null)
							{
								tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num3, out tmp_Glyph);
							}
						}
						if (tmp_Glyph == null)
						{
							num3 = (chars[i] = 0x20);
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num3, out tmp_Glyph);
							if (!TMP_Settings.warningsDisabled)
							{
								Debug.LogWarning("Character with ASCII value of " + num3 + " was not found in the Font Asset Glyph Table. It was replaced by a space.", this);
							}
						}
					}
					if (tmp_FontAsset != null)
					{
						if (tmp_FontAsset.GetInstanceID() != this.m_currentFontAsset.GetInstanceID())
						{
							flag = true;
							isUsingAlternateTypeface = false;
							this.m_currentFontAsset = tmp_FontAsset;
						}
					}
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = TMP_TextElementType.Character;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].textElement = tmp_Glyph;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].isUsingAlternateTypeface = isUsingAlternateTypeface;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num3;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
					if (flag)
					{
						if (TMP_Settings.matchMaterialPreset)
						{
							this.m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(this.m_currentMaterial, this.m_currentFontAsset.material);
						}
						else
						{
							this.m_currentMaterial = this.m_currentFontAsset.material;
						}
						this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
					}
					if (!char.IsWhiteSpace((char)num3) && num3 != 0x200B)
					{
						if (this.m_materialReferences[this.m_currentMaterialIndex].referenceCount < 0x3FFF)
						{
							MaterialReference[] materialReferences4 = this.m_materialReferences;
							int currentMaterialIndex6 = this.m_currentMaterialIndex;
							materialReferences4[currentMaterialIndex6].referenceCount = materialReferences4[currentMaterialIndex6].referenceCount + 1;
						}
						else
						{
							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(new Material(this.m_currentMaterial), this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
							MaterialReference[] materialReferences5 = this.m_materialReferences;
							int currentMaterialIndex7 = this.m_currentMaterialIndex;
							materialReferences5[currentMaterialIndex7].referenceCount = materialReferences5[currentMaterialIndex7].referenceCount + 1;
						}
					}
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].material = this.m_currentMaterial;
					this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
					this.m_materialReferences[this.m_currentMaterialIndex].isFallbackMaterial = flag;
					if (flag)
					{
						this.m_materialReferences[this.m_currentMaterialIndex].fallbackMaterial = currentMaterial;
						this.m_currentFontAsset = currentFontAsset;
						this.m_currentMaterial = currentMaterial;
						this.m_currentMaterialIndex = currentMaterialIndex3;
					}
					this.m_totalCharacterCount++;
					goto IL_B47;
					IL_15F:
					TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, this.m_totalCharacterCount + 1, true);
					goto IL_178;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					goto IL_B63;
				}
			}
			IL_B63:
			if (this.m_isCalculatingPreferredValues)
			{
				this.m_isCalculatingPreferredValues = false;
				this.m_isInputParsingRequired = true;
				return this.m_totalCharacterCount;
			}
			this.m_textInfo.spriteCount = num2;
			int num8 = this.m_textInfo.materialCount = this.m_materialReferenceIndexLookup.Count;
			if (num8 > this.m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize<TMP_MeshInfo>(ref this.m_textInfo.meshInfo, num8, false);
			}
			if (num8 > this.m_subTextObjects.Length)
			{
				TMP_TextInfo.Resize<TMP_SubMesh>(ref this.m_subTextObjects, Mathf.NextPowerOfTwo(num8 + 1));
			}
			if (this.m_textInfo.characterInfo.Length - this.m_totalCharacterCount > 0x100)
			{
				TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, Mathf.Max(this.m_totalCharacterCount + 1, 0x100), true);
			}
			int j = 0;
			while (j < num8)
			{
				if (j > 0)
				{
					if (this.m_subTextObjects[j] == null)
					{
						this.m_subTextObjects[j] = TMP_SubMesh.AddSubTextObject(this, this.m_materialReferences[j]);
						this.m_textInfo.meshInfo[j].vertices = null;
					}
					if (this.m_subTextObjects[j].sharedMaterial == null)
					{
						goto IL_D0C;
					}
					if (this.m_subTextObjects[j].sharedMaterial.GetInstanceID() != this.m_materialReferences[j].material.GetInstanceID())
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_D0C;
						}
					}
					IL_E13:
					if (this.m_materialReferences[j].isFallbackMaterial)
					{
						this.m_subTextObjects[j].fallbackMaterial = this.m_materialReferences[j].material;
						this.m_subTextObjects[j].fallbackSourceMaterial = this.m_materialReferences[j].fallbackMaterial;
						goto IL_E71;
					}
					goto IL_E71;
					IL_D0C:
					bool isDefaultMaterial = this.m_materialReferences[j].isDefaultMaterial;
					this.m_subTextObjects[j].isDefaultMaterial = isDefaultMaterial;
					if (isDefaultMaterial)
					{
						if (!(this.m_subTextObjects[j].sharedMaterial == null))
						{
							if (this.m_subTextObjects[j].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == this.m_materialReferences[j].material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
							{
								goto IL_E13;
							}
						}
					}
					this.m_subTextObjects[j].sharedMaterial = this.m_materialReferences[j].material;
					this.m_subTextObjects[j].fontAsset = this.m_materialReferences[j].fontAsset;
					this.m_subTextObjects[j].spriteAsset = this.m_materialReferences[j].spriteAsset;
					goto IL_E13;
				}
				IL_E71:
				int referenceCount = this.m_materialReferences[j].referenceCount;
				if (this.m_textInfo.meshInfo[j].vertices == null)
				{
					goto IL_ED5;
				}
				if (this.m_textInfo.meshInfo[j].vertices.Length < referenceCount * (this.m_isVolumetricText ? 8 : 4))
				{
					goto IL_ED5;
				}
				int num9 = this.m_textInfo.meshInfo[j].vertices.Length;
				int num10 = referenceCount;
				int num11;
				if (!this.m_isVolumetricText)
				{
					num11 = 4;
				}
				else
				{
					num11 = 8;
				}
				if (num9 - num10 * num11 > 0x400)
				{
					TMP_MeshInfo[] meshInfo = this.m_textInfo.meshInfo;
					int num12 = j;
					int size;
					if (referenceCount > 0x400)
					{
						size = referenceCount + 0x100;
					}
					else
					{
						size = Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 0x100);
					}
					meshInfo[num12].ResizeMeshInfo(size, this.m_isVolumetricText);
				}
				IL_1034:
				j++;
				continue;
				IL_ED5:
				if (this.m_textInfo.meshInfo[j].vertices == null)
				{
					if (j == 0)
					{
						this.m_textInfo.meshInfo[j] = new TMP_MeshInfo(this.m_mesh, referenceCount + 1, this.m_isVolumetricText);
					}
					else
					{
						this.m_textInfo.meshInfo[j] = new TMP_MeshInfo(this.m_subTextObjects[j].mesh, referenceCount + 1, this.m_isVolumetricText);
					}
				}
				else
				{
					this.m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount <= 0x400) ? Mathf.NextPowerOfTwo(referenceCount) : (referenceCount + 0x100), this.m_isVolumetricText);
				}
				goto IL_1034;
			}
			int k = num8;
			while (k < this.m_subTextObjects.Length)
			{
				if (!(this.m_subTextObjects[k] != null))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_10AC;
					}
				}
				else
				{
					if (k < this.m_textInfo.meshInfo.Length)
					{
						this.m_textInfo.meshInfo[k].ClearUnusedVertices(0, true);
					}
					k++;
				}
			}
			IL_10AC:
			return this.m_totalCharacterCount;
		}

		protected override void ComputeMarginSize()
		{
			if (base.rectTransform != null)
			{
				this.m_marginWidth = this.m_rectTransform.rect.width - this.m_margin.x - this.m_margin.z;
				this.m_marginHeight = this.m_rectTransform.rect.height - this.m_margin.y - this.m_margin.w;
				this.m_RectTransformCorners = this.GetTextContainerLocalCorners();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.m_havePropertiesChanged = true;
			this.isMaskUpdateRequired = true;
			this.SetVerticesDirty();
		}

		protected override void OnTransformParentChanged()
		{
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			this.ComputeMarginSize();
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		private void LateUpdate()
		{
			if (this.m_rectTransform.hasChanged)
			{
				float y = this.m_rectTransform.lossyScale.y;
				if (!this.m_havePropertiesChanged)
				{
					if (y != this.m_previousLossyScaleY)
					{
						if (this.m_text != string.Empty)
						{
							if (this.m_text != null)
							{
								this.UpdateSDFScale(y);
								this.m_previousLossyScaleY = y;
							}
						}
					}
				}
			}
			if (this.m_isUsingLegacyAnimationComponent)
			{
				this.m_havePropertiesChanged = true;
				this.OnPreRenderObject();
			}
		}

		private void OnPreRenderObject()
		{
			if (this.m_isAwake)
			{
				if (!this.m_ignoreActiveState)
				{
					if (!this.IsActive())
					{
						return;
					}
				}
				this.loopCountA = 0;
				if (!this.m_havePropertiesChanged)
				{
					if (!this.m_isLayoutDirty)
					{
						return;
					}
				}
				if (this.isMaskUpdateRequired)
				{
					this.UpdateMask();
					this.isMaskUpdateRequired = false;
				}
				if (this.checkPaddingRequired)
				{
					this.UpdateMeshPadding();
				}
				if (!this.m_isInputParsingRequired)
				{
					if (!this.m_isTextTruncated)
					{
						goto IL_A9;
					}
				}
				base.ParseInputText();
				IL_A9:
				if (this.m_enableAutoSizing)
				{
					this.m_fontSize = Mathf.Clamp(this.m_fontSize, this.m_fontSizeMin, this.m_fontSizeMax);
				}
				this.m_maxFontSize = this.m_fontSizeMax;
				this.m_minFontSize = this.m_fontSizeMin;
				this.m_lineSpacingDelta = 0f;
				this.m_charWidthAdjDelta = 0f;
				this.m_isCharacterWrappingEnabled = false;
				this.m_isTextTruncated = false;
				this.m_havePropertiesChanged = false;
				this.m_isLayoutDirty = false;
				this.m_ignoreActiveState = false;
				this.GenerateTextMesh();
				return;
			}
		}

		protected override void GenerateTextMesh()
		{
			if (!(this.m_fontAsset == null))
			{
				if (this.m_fontAsset.characterDictionary != null)
				{
					if (this.m_textInfo != null)
					{
						this.m_textInfo.Clear();
					}
					if (this.m_char_buffer != null && this.m_char_buffer.Length != 0)
					{
						if (this.m_char_buffer[0] == 0)
						{
						}
						else
						{
							this.m_currentFontAsset = this.m_fontAsset;
							this.m_currentMaterial = this.m_sharedMaterial;
							this.m_currentMaterialIndex = 0;
							this.m_materialReferenceStack.SetDefault(new MaterialReference(this.m_currentMaterialIndex, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
							this.m_currentSpriteAsset = this.m_spriteAsset;
							if (this.m_spriteAnimator != null)
							{
								this.m_spriteAnimator.StopAllAnimations();
							}
							int totalCharacterCount = this.m_totalCharacterCount;
							float num = this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
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
							float num3 = this.m_fontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
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
							this.m_fontScaleMultiplier = 1f;
							this.m_currentFontSize = this.m_fontSize;
							this.m_sizeStack.SetDefault(this.m_currentFontSize);
							this.m_style = this.m_fontStyle;
							int fontWeightInternal;
							if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
							{
								fontWeightInternal = 0x2BC;
							}
							else
							{
								fontWeightInternal = this.m_fontWeight;
							}
							this.m_fontWeightInternal = fontWeightInternal;
							this.m_fontWeightStack.SetDefault(this.m_fontWeightInternal);
							this.m_fontStyleStack.Clear();
							this.m_lineJustification = this.m_textAlignment;
							this.m_lineJustificationStack.SetDefault(this.m_lineJustification);
							float num7 = 0f;
							float num8 = 1f;
							this.m_baselineOffset = 0f;
							this.m_baselineOffsetStack.Clear();
							bool flag = false;
							Vector3 zero = Vector3.zero;
							Vector3 zero2 = Vector3.zero;
							bool flag2 = false;
							Vector3 zero3 = Vector3.zero;
							Vector3 zero4 = Vector3.zero;
							bool flag3 = false;
							Vector3 start = Vector3.zero;
							Vector3 vector = Vector3.zero;
							this.m_fontColor32 = this.m_fontColor;
							this.m_htmlColor = this.m_fontColor32;
							this.m_underlineColor = this.m_htmlColor;
							this.m_strikethroughColor = this.m_htmlColor;
							this.m_colorStack.SetDefault(this.m_htmlColor);
							this.m_underlineColorStack.SetDefault(this.m_htmlColor);
							this.m_strikethroughColorStack.SetDefault(this.m_htmlColor);
							this.m_highlightColorStack.SetDefault(this.m_htmlColor);
							this.m_actionStack.Clear();
							this.m_isFXMatrixSet = false;
							this.m_lineOffset = 0f;
							this.m_lineHeight = -32767f;
							float num9 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
							this.m_cSpacing = 0f;
							this.m_monoSpacing = 0f;
							this.m_xAdvance = 0f;
							this.tag_LineIndent = 0f;
							this.tag_Indent = 0f;
							this.m_indentStack.SetDefault(0f);
							this.tag_NoParsing = false;
							this.m_characterCount = 0;
							this.m_firstCharacterOfLine = 0;
							this.m_lastCharacterOfLine = 0;
							this.m_firstVisibleCharacterOfLine = 0;
							this.m_lastVisibleCharacterOfLine = 0;
							this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
							this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
							this.m_lineNumber = 0;
							this.m_lineVisibleCharacterCount = 0;
							bool flag4 = true;
							this.m_firstOverflowCharacterIndex = -1;
							this.m_pageNumber = 0;
							int num10 = Mathf.Clamp(this.m_pageToDisplay - 1, 0, this.m_textInfo.pageInfo.Length - 1);
							int num11 = 0;
							int num12 = 0;
							Vector4 margin = this.m_margin;
							float marginWidth = this.m_marginWidth;
							float marginHeight = this.m_marginHeight;
							this.m_marginLeft = 0f;
							this.m_marginRight = 0f;
							this.m_width = -1f;
							float num13 = marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight;
							this.m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
							this.m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
							this.m_textInfo.ClearLineInfo();
							this.m_maxCapHeight = 0f;
							this.m_maxAscender = 0f;
							this.m_maxDescender = 0f;
							float num14 = 0f;
							float num15 = 0f;
							bool flag5 = false;
							this.m_isNewPage = false;
							bool flag6 = true;
							this.m_isNonBreakingSpace = false;
							bool flag7 = false;
							bool flag8 = false;
							int num16 = 0;
							base.SaveWordWrappingState(ref this.m_SavedWordWrapState, -1, -1);
							base.SaveWordWrappingState(ref this.m_SavedLineState, -1, -1);
							this.loopCountA++;
							int num17 = 0;
							int i = 0;
							while (i < this.m_char_buffer.Length)
							{
								if (this.m_char_buffer[i] == 0)
								{
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										goto IL_371B;
									}
								}
								else
								{
									int num18 = this.m_char_buffer[i];
									this.m_textElementType = this.m_textInfo.characterInfo[this.m_characterCount].elementType;
									this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
									this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
									int currentMaterialIndex = this.m_currentMaterialIndex;
									if (!this.m_isRichText)
									{
										goto IL_665;
									}
									if (num18 != 0x3C)
									{
										goto IL_665;
									}
									this.m_isParsingText = true;
									this.m_textElementType = TMP_TextElementType.Character;
									if (!base.ValidateHtmlTag(this.m_char_buffer, i + 1, out num17))
									{
										goto IL_665;
									}
									i = num17;
									if (this.m_textElementType != TMP_TextElementType.Character)
									{
										goto IL_665;
									}
									IL_36E7:
									i++;
									continue;
									IL_665:
									this.m_isParsingText = false;
									bool isUsingAlternateTypeface = this.m_textInfo.characterInfo[this.m_characterCount].isUsingAlternateTypeface;
									if (this.m_characterCount < this.m_firstVisibleCharacter)
									{
										this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
										this.m_textInfo.characterInfo[this.m_characterCount].character = '​';
										this.m_characterCount++;
										goto IL_36E7;
									}
									float num19 = 1f;
									if (this.m_textElementType == TMP_TextElementType.Character)
									{
										if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
										{
											if (char.IsLower((char)num18))
											{
												num18 = (int)char.ToUpper((char)num18);
											}
										}
										else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
										{
											if (char.IsUpper((char)num18))
											{
												num18 = (int)char.ToLower((char)num18);
											}
										}
										else
										{
											if ((this.m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
											{
												if ((this.m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
												{
													goto IL_7C4;
												}
											}
											if (char.IsLower((char)num18))
											{
												num19 = 0.8f;
												num18 = (int)char.ToUpper((char)num18);
											}
										}
									}
									IL_7C4:
									if (this.m_textElementType == TMP_TextElementType.Sprite)
									{
										this.m_currentSpriteAsset = this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset;
										this.m_spriteIndex = this.m_textInfo.characterInfo[this.m_characterCount].spriteIndex;
										TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
										if (tmp_Sprite == null)
										{
											goto IL_36E7;
										}
										if (num18 == 0x3C)
										{
											num18 = 0xE000 + this.m_spriteIndex;
										}
										else
										{
											this.m_spriteColor = TMP_Text.s_colorWhite;
										}
										this.m_currentFontAsset = this.m_fontAsset;
										float num20 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
										num6 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num20;
										this.m_cached_TextElement = tmp_Sprite;
										this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
										this.m_textInfo.characterInfo[this.m_characterCount].scale = num20;
										this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset = this.m_currentSpriteAsset;
										this.m_textInfo.characterInfo[this.m_characterCount].fontAsset = this.m_currentFontAsset;
										this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex = this.m_currentMaterialIndex;
										this.m_currentMaterialIndex = currentMaterialIndex;
										num7 = 0f;
									}
									else if (this.m_textElementType == TMP_TextElementType.Character)
									{
										this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
										if (this.m_cached_TextElement == null)
										{
											goto IL_36E7;
										}
										this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
										this.m_currentMaterial = this.m_textInfo.characterInfo[this.m_characterCount].material;
										this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
										float num21 = this.m_currentFontSize * num19 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
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
										num6 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
										this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
										this.m_textInfo.characterInfo[this.m_characterCount].scale = num6;
										num7 = ((this.m_currentMaterialIndex != 0) ? this.m_subTextObjects[this.m_currentMaterialIndex].padding : this.m_padding);
									}
									float num23 = num6;
									if (num18 == 0xAD)
									{
										num6 = 0f;
									}
									if (this.m_isRightToLeft)
									{
										this.m_xAdvance -= ((this.m_cached_TextElement.xAdvance * num8 + this.m_characterSpacing + this.m_wordSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (char.IsWhiteSpace((char)num18) || num18 == 0x200B)
										{
											this.m_xAdvance -= this.m_wordSpacing * num6;
										}
									}
									this.m_textInfo.characterInfo[this.m_characterCount].character = (char)num18;
									this.m_textInfo.characterInfo[this.m_characterCount].pointSize = this.m_currentFontSize;
									this.m_textInfo.characterInfo[this.m_characterCount].color = this.m_htmlColor;
									this.m_textInfo.characterInfo[this.m_characterCount].underlineColor = this.m_underlineColor;
									this.m_textInfo.characterInfo[this.m_characterCount].strikethroughColor = this.m_strikethroughColor;
									this.m_textInfo.characterInfo[this.m_characterCount].highlightColor = this.m_highlightColor;
									this.m_textInfo.characterInfo[this.m_characterCount].style = this.m_style;
									this.m_textInfo.characterInfo[this.m_characterCount].index = (short)i;
									if (this.m_enableKerning && this.m_characterCount >= 1)
									{
										int character = (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
										KerningPairKey kerningPairKey = new KerningPairKey(character, num18);
										KerningPair kerningPair;
										this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
										if (kerningPair != null)
										{
											this.m_xAdvance += kerningPair.XadvanceOffset * num6;
										}
									}
									float num24 = 0f;
									if (this.m_monoSpacing != 0f)
									{
										num24 = (this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num6) * (1f - this.m_charWidthAdjDelta);
										this.m_xAdvance += num24;
									}
									if (this.m_textElementType != TMP_TextElementType.Character || isUsingAlternateTypeface)
									{
										goto IL_E4E;
									}
									if ((this.m_style & FontStyles.Bold) != FontStyles.Bold)
									{
										if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
										{
											goto IL_E4E;
										}
									}
									float num25;
									if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
									{
										float @float = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
										num25 = this.m_currentFontAsset.boldStyle / 4f * @float * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
										if (num25 + num7 > @float)
										{
											num7 = @float - num25;
										}
									}
									else
									{
										num25 = 0f;
									}
									num8 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
									IL_EC7:
									float baseline = this.m_currentFontAsset.fontInfo.Baseline;
									Vector3 vector2;
									vector2.x = this.m_xAdvance + (this.m_cached_TextElement.xOffset - num7 - num25) * num6 * (1f - this.m_charWidthAdjDelta);
									vector2.y = (baseline + this.m_cached_TextElement.yOffset + num7) * num6 - this.m_lineOffset + this.m_baselineOffset;
									vector2.z = 0f;
									Vector3 vector3;
									vector3.x = vector2.x;
									vector3.y = vector2.y - (this.m_cached_TextElement.height + num7 * 2f) * num6;
									vector3.z = 0f;
									Vector3 vector4;
									vector4.x = vector3.x + (this.m_cached_TextElement.width + num7 * 2f + num25 * 2f) * num6 * (1f - this.m_charWidthAdjDelta);
									vector4.y = vector2.y;
									vector4.z = 0f;
									Vector3 vector5;
									vector5.x = vector4.x;
									vector5.y = vector3.y;
									vector5.z = 0f;
									if (this.m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface)
									{
										if ((this.m_style & FontStyles.Italic) != FontStyles.Italic)
										{
											if ((this.m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
											{
												goto IL_10E4;
											}
										}
										float num26 = (float)this.m_currentFontAsset.italicStyle * 0.01f;
										Vector3 b = new Vector3(num26 * ((this.m_cached_TextElement.yOffset + num7 + num25) * num6), 0f, 0f);
										Vector3 b2 = new Vector3(num26 * ((this.m_cached_TextElement.yOffset - this.m_cached_TextElement.height - num7 - num25) * num6), 0f, 0f);
										vector2 += b;
										vector3 += b2;
										vector4 += b;
										vector5 += b2;
									}
									IL_10E4:
									if (this.m_isFXMatrixSet)
									{
										Vector3 b3 = (vector4 + vector3) / 2f;
										vector2 = this.m_FXMatrix.MultiplyPoint3x4(vector2 - b3) + b3;
										vector3 = this.m_FXMatrix.MultiplyPoint3x4(vector3 - b3) + b3;
										vector4 = this.m_FXMatrix.MultiplyPoint3x4(vector4 - b3) + b3;
										vector5 = this.m_FXMatrix.MultiplyPoint3x4(vector5 - b3) + b3;
									}
									this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft = vector3;
									this.m_textInfo.characterInfo[this.m_characterCount].topLeft = vector2;
									this.m_textInfo.characterInfo[this.m_characterCount].topRight = vector4;
									this.m_textInfo.characterInfo[this.m_characterCount].bottomRight = vector5;
									this.m_textInfo.characterInfo[this.m_characterCount].origin = this.m_xAdvance;
									this.m_textInfo.characterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
									this.m_textInfo.characterInfo[this.m_characterCount].aspectRatio = (vector4.x - vector3.x) / (vector2.y - vector3.y);
									float num27 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? this.m_textInfo.characterInfo[this.m_characterCount].scale : num6) + this.m_baselineOffset;
									this.m_textInfo.characterInfo[this.m_characterCount].ascender = num27 - this.m_lineOffset;
									this.m_maxLineAscender = ((num27 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num27);
									float descender = this.m_currentFontAsset.fontInfo.Descender;
									float num28;
									if (this.m_textElementType == TMP_TextElementType.Character)
									{
										num28 = num6;
									}
									else
									{
										num28 = this.m_textInfo.characterInfo[this.m_characterCount].scale;
									}
									float num29 = descender * num28 + this.m_baselineOffset;
									float num30 = this.m_textInfo.characterInfo[this.m_characterCount].descender = num29 - this.m_lineOffset;
									this.m_maxLineDescender = ((num29 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num29);
									if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
									{
										goto IL_13D9;
									}
									if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_13D9;
										}
									}
									IL_1461:
									if (this.m_lineNumber == 0)
									{
										goto IL_147B;
									}
									if (this.m_isNewPage)
									{
										for (;;)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											goto IL_147B;
										}
									}
									IL_14BA:
									if (this.m_lineOffset == 0f)
									{
										float num31;
										if (num14 > num27)
										{
											num31 = num14;
										}
										else
										{
											num31 = num27;
										}
										num14 = num31;
									}
									this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
									if (num18 != 9)
									{
										if (!char.IsWhiteSpace((char)num18))
										{
											if (num18 != 0x200B)
											{
												goto IL_1554;
											}
										}
										if (this.m_textElementType == TMP_TextElementType.Sprite)
										{
										}
										else
										{
											if (num18 != 0xA)
											{
												if (!char.IsSeparator((char)num18))
												{
													goto IL_2248;
												}
											}
											if (num18 == 0xAD || num18 == 0x200B)
											{
												goto IL_2248;
											}
											if (num18 != 0x2060)
											{
												TMP_LineInfo[] lineInfo = this.m_textInfo.lineInfo;
												int lineNumber = this.m_lineNumber;
												lineInfo[lineNumber].spaceCount = lineInfo[lineNumber].spaceCount + 1;
												this.m_textInfo.spaceCount++;
												goto IL_2248;
											}
											goto IL_2248;
										}
									}
									IL_1554:
									this.m_textInfo.characterInfo[this.m_characterCount].isVisible = true;
									float num32;
									if (this.m_width != -1f)
									{
										num32 = Mathf.Min(marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
									}
									else
									{
										num32 = marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight;
									}
									num13 = num32;
									this.m_textInfo.lineInfo[this.m_lineNumber].marginLeft = this.m_marginLeft;
									bool flag9;
									if ((this.m_lineJustification & (TextAlignmentOptions)0x10) != (TextAlignmentOptions)0x10)
									{
										flag9 = ((this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8);
									}
									else
									{
										flag9 = true;
									}
									bool flag10 = flag9;
									float num33 = Mathf.Abs(this.m_xAdvance);
									float num34;
									if (!this.m_isRightToLeft)
									{
										num34 = this.m_cached_TextElement.xAdvance;
									}
									else
									{
										num34 = 0f;
									}
									float num35 = num34 * (1f - this.m_charWidthAdjDelta);
									float num36;
									if (num18 != 0xAD)
									{
										num36 = num6;
									}
									else
									{
										num36 = num23;
									}
									float obj = num33 + num35 * num36;
									float num37 = num13;
									float num38;
									if (flag10)
									{
										num38 = 1.05f;
									}
									else
									{
										num38 = 1f;
									}
									if (obj > num37 * num38)
									{
										num12 = this.m_characterCount - 1;
										if (base.enableWordWrapping)
										{
											if (this.m_characterCount != this.m_firstCharacterOfLine)
											{
												if (num16 == this.m_SavedWordWrapState.previous_WordBreak)
												{
													goto IL_16D8;
												}
												if (flag6)
												{
													for (;;)
													{
														switch (1)
														{
														case 0:
															continue;
														}
														goto IL_16D8;
													}
												}
												IL_17F8:
												i = base.RestoreWordWrappingState(ref this.m_SavedWordWrapState);
												num16 = i;
												if (this.m_char_buffer[i] == 0xAD)
												{
													this.m_isTextTruncated = true;
													this.m_char_buffer[i] = 0x2D;
													this.GenerateTextMesh();
													return;
												}
												if (this.m_lineNumber > 0)
												{
													if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
													{
														if (!this.m_isNewPage)
														{
															float num39 = this.m_maxLineAscender - this.m_startOfLineAscender;
															this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num39);
															this.m_lineOffset += num39;
															this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
															this.m_SavedWordWrapState.previousLineAscender = this.m_maxLineAscender;
														}
													}
												}
												this.m_isNewPage = false;
												float num40 = this.m_maxLineAscender - this.m_lineOffset;
												float num41 = this.m_maxLineDescender - this.m_lineOffset;
												float maxDescender;
												if (this.m_maxDescender < num41)
												{
													maxDescender = this.m_maxDescender;
												}
												else
												{
													maxDescender = num41;
												}
												this.m_maxDescender = maxDescender;
												if (!flag5)
												{
													num15 = this.m_maxDescender;
												}
												if (this.m_useMaxVisibleDescender)
												{
													if (this.m_characterCount < this.m_maxVisibleCharacters)
													{
														if (this.m_lineNumber < this.m_maxVisibleLines)
														{
															goto IL_196E;
														}
													}
													flag5 = true;
												}
												IL_196E:
												this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
												this.m_textInfo.lineInfo[this.m_lineNumber].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = ((this.m_firstCharacterOfLine <= this.m_firstVisibleCharacterOfLine) ? this.m_firstVisibleCharacterOfLine : this.m_firstCharacterOfLine));
												this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = ((this.m_characterCount - 1 <= 0) ? 0 : (this.m_characterCount - 1)));
												TMP_LineInfo[] lineInfo2 = this.m_textInfo.lineInfo;
												int lineNumber2 = this.m_lineNumber;
												int lastVisibleCharacterOfLine;
												if (this.m_lastVisibleCharacterOfLine < this.m_firstVisibleCharacterOfLine)
												{
													lastVisibleCharacterOfLine = this.m_firstVisibleCharacterOfLine;
												}
												else
												{
													lastVisibleCharacterOfLine = this.m_lastVisibleCharacterOfLine;
												}
												lineInfo2[lineNumber2].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = lastVisibleCharacterOfLine);
												this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
												this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
												this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num41);
												this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num40);
												this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x;
												this.m_textInfo.lineInfo[this.m_lineNumber].width = num13;
												this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 - this.m_cSpacing;
												this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
												this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num40;
												this.m_textInfo.lineInfo[this.m_lineNumber].descender = num41;
												this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num40 - num41 + num9 * num5;
												this.m_firstCharacterOfLine = this.m_characterCount;
												this.m_lineVisibleCharacterCount = 0;
												base.SaveWordWrappingState(ref this.m_SavedLineState, i, this.m_characterCount - 1);
												this.m_lineNumber++;
												flag4 = true;
												if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
												{
													base.ResizeLineExtents(this.m_lineNumber);
												}
												if (this.m_lineHeight == -32767f)
												{
													float num42 = this.m_textInfo.characterInfo[this.m_characterCount].ascender - this.m_textInfo.characterInfo[this.m_characterCount].baseLine;
													float num43 = 0f - this.m_maxLineDescender + num42 + (num9 + this.m_lineSpacing + this.m_lineSpacingDelta) * num5;
													this.m_lineOffset += num43;
													this.m_startOfLineAscender = num42;
												}
												else
												{
													this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num5;
												}
												this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
												this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
												this.m_xAdvance = this.tag_Indent;
												goto IL_36E7;
												IL_16D8:
												if (this.m_enableAutoSizing)
												{
													if (this.m_fontSize > this.m_fontSizeMin)
													{
														if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
														{
															this.loopCountA = 0;
															this.m_charWidthAdjDelta += 0.01f;
															this.GenerateTextMesh();
															return;
														}
														this.m_maxFontSize = this.m_fontSize;
														this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
														this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
														if (this.loopCountA > 0x14)
														{
															return;
														}
														this.GenerateTextMesh();
														return;
													}
												}
												if (!this.m_isCharacterWrappingEnabled)
												{
													if (!flag7)
													{
														flag7 = true;
													}
													else
													{
														this.m_isCharacterWrappingEnabled = true;
													}
													goto IL_17F8;
												}
												flag8 = true;
												goto IL_17F8;
											}
										}
										if (this.m_enableAutoSizing && this.m_fontSize > this.m_fontSizeMin)
										{
											if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
											{
												this.loopCountA = 0;
												this.m_charWidthAdjDelta += 0.01f;
												this.GenerateTextMesh();
												return;
											}
											this.m_maxFontSize = this.m_fontSize;
											this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
											this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
											if (this.loopCountA > 0x14)
											{
												return;
											}
											this.GenerateTextMesh();
											return;
										}
										else
										{
											switch (this.m_overflowMode)
											{
											case TextOverflowModes.Overflow:
												if (this.m_isMaskingEnabled)
												{
													this.DisableMasking();
												}
												break;
											case TextOverflowModes.Ellipsis:
												if (this.m_isMaskingEnabled)
												{
													this.DisableMasking();
												}
												this.m_isTextTruncated = true;
												if (this.m_characterCount >= 1)
												{
													this.m_char_buffer[i - 1] = 0x2026;
													this.m_char_buffer[i] = 0;
													if (this.m_cached_Ellipsis_GlyphInfo != null)
													{
														this.m_textInfo.characterInfo[num12].character = '…';
														this.m_textInfo.characterInfo[num12].textElement = this.m_cached_Ellipsis_GlyphInfo;
														this.m_textInfo.characterInfo[num12].fontAsset = this.m_materialReferences[0].fontAsset;
														this.m_textInfo.characterInfo[num12].material = this.m_materialReferences[0].material;
														this.m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
													}
													else
													{
														Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
													}
													this.m_totalCharacterCount = num12 + 1;
													this.GenerateTextMesh();
													return;
												}
												this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
												break;
											case TextOverflowModes.Masking:
												if (!this.m_isMaskingEnabled)
												{
													this.EnableMasking();
												}
												break;
											case TextOverflowModes.Truncate:
												if (this.m_isMaskingEnabled)
												{
													this.DisableMasking();
												}
												this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
												break;
											case TextOverflowModes.ScrollRect:
												if (!this.m_isMaskingEnabled)
												{
													this.EnableMasking();
												}
												break;
											}
										}
									}
									if (num18 != 9)
									{
										Color32 vertexColor;
										if (this.m_overrideHtmlColors)
										{
											vertexColor = this.m_fontColor32;
										}
										else
										{
											vertexColor = this.m_htmlColor;
										}
										if (this.m_textElementType == TMP_TextElementType.Character)
										{
											this.SaveGlyphVertexInfo(num7, num25, vertexColor);
										}
										else if (this.m_textElementType == TMP_TextElementType.Sprite)
										{
											this.SaveSpriteVertexInfo(vertexColor);
										}
									}
									else
									{
										this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
										this.m_lastVisibleCharacterOfLine = this.m_characterCount;
										TMP_LineInfo[] lineInfo3 = this.m_textInfo.lineInfo;
										int lineNumber3 = this.m_lineNumber;
										lineInfo3[lineNumber3].spaceCount = lineInfo3[lineNumber3].spaceCount + 1;
										this.m_textInfo.spaceCount++;
									}
									if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
									{
										if (num18 != 0xAD)
										{
											if (flag4)
											{
												flag4 = false;
												this.m_firstVisibleCharacterOfLine = this.m_characterCount;
											}
											this.m_lineVisibleCharacterCount++;
											this.m_lastVisibleCharacterOfLine = this.m_characterCount;
										}
									}
									IL_2248:
									if (this.m_lineNumber > 0)
									{
										if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
										{
											if (this.m_lineHeight == -32767f)
											{
												if (!this.m_isNewPage)
												{
													float num44 = this.m_maxLineAscender - this.m_startOfLineAscender;
													this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num44);
													num30 -= num44;
													this.m_lineOffset += num44;
													this.m_startOfLineAscender += num44;
													this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
													this.m_SavedWordWrapState.previousLineAscender = this.m_startOfLineAscender;
												}
											}
										}
									}
									this.m_textInfo.characterInfo[this.m_characterCount].lineNumber = (short)this.m_lineNumber;
									this.m_textInfo.characterInfo[this.m_characterCount].pageNumber = (short)this.m_pageNumber;
									if (num18 == 0xA)
									{
										goto IL_238D;
									}
									if (num18 == 0xD)
									{
										goto IL_238D;
									}
									if (num18 != 0x2026)
									{
										goto IL_23B5;
									}
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										goto IL_238D;
									}
									IL_23D6:
									if (this.m_maxAscender - num30 > marginHeight + 0.0001f)
									{
										if (this.m_enableAutoSizing && this.m_lineSpacingDelta > this.m_lineSpacingMax)
										{
											if (this.m_lineNumber > 0)
											{
												this.loopCountA = 0;
												this.m_lineSpacingDelta -= 1f;
												this.GenerateTextMesh();
												return;
											}
										}
										if (this.m_enableAutoSizing)
										{
											if (this.m_fontSize > this.m_fontSizeMin)
											{
												this.m_maxFontSize = this.m_fontSize;
												this.m_fontSize -= Mathf.Max((this.m_fontSize - this.m_minFontSize) / 2f, 0.05f);
												this.m_fontSize = (float)((int)(Mathf.Max(this.m_fontSize, this.m_fontSizeMin) * 20f + 0.5f)) / 20f;
												if (this.loopCountA > 0x14)
												{
													return;
												}
												this.GenerateTextMesh();
												return;
											}
										}
										if (this.m_firstOverflowCharacterIndex == -1)
										{
											this.m_firstOverflowCharacterIndex = this.m_characterCount;
										}
										switch (this.m_overflowMode)
										{
										case TextOverflowModes.Overflow:
											if (this.m_isMaskingEnabled)
											{
												this.DisableMasking();
											}
											break;
										case TextOverflowModes.Ellipsis:
											if (this.m_isMaskingEnabled)
											{
												this.DisableMasking();
											}
											if (this.m_lineNumber > 0)
											{
												this.m_char_buffer[(int)this.m_textInfo.characterInfo[num12].index] = 0x2026;
												this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num12].index + 1)] = 0;
												if (this.m_cached_Ellipsis_GlyphInfo != null)
												{
													this.m_textInfo.characterInfo[num12].character = '…';
													this.m_textInfo.characterInfo[num12].textElement = this.m_cached_Ellipsis_GlyphInfo;
													this.m_textInfo.characterInfo[num12].fontAsset = this.m_materialReferences[0].fontAsset;
													this.m_textInfo.characterInfo[num12].material = this.m_materialReferences[0].material;
													this.m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
												}
												else
												{
													Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
												}
												this.m_totalCharacterCount = num12 + 1;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh(false);
											return;
										case TextOverflowModes.Masking:
											if (!this.m_isMaskingEnabled)
											{
												this.EnableMasking();
											}
											break;
										case TextOverflowModes.Truncate:
											if (this.m_isMaskingEnabled)
											{
												this.DisableMasking();
											}
											if (this.m_lineNumber > 0)
											{
												this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num12].index + 1)] = 0;
												this.m_totalCharacterCount = num12 + 1;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh(false);
											return;
										case TextOverflowModes.ScrollRect:
											if (!this.m_isMaskingEnabled)
											{
												this.EnableMasking();
											}
											break;
										case TextOverflowModes.Page:
											if (this.m_isMaskingEnabled)
											{
												this.DisableMasking();
											}
											if (num18 != 0xD)
											{
												if (num18 == 0xA)
												{
												}
												else
												{
													if (i == 0)
													{
														this.ClearMesh();
														return;
													}
													if (num11 == i)
													{
														this.m_char_buffer[i] = 0;
														this.m_isTextTruncated = true;
													}
													num11 = i;
													i = base.RestoreWordWrappingState(ref this.m_SavedLineState);
													this.m_isNewPage = true;
													this.m_xAdvance = this.tag_Indent;
													this.m_lineOffset = 0f;
													this.m_maxAscender = 0f;
													num14 = 0f;
													this.m_lineNumber++;
													this.m_pageNumber++;
													goto IL_36E7;
												}
											}
											break;
										case TextOverflowModes.Linked:
											if (this.m_linkedTextComponent != null)
											{
												this.m_linkedTextComponent.text = base.text;
												this.m_linkedTextComponent.firstVisibleCharacter = this.m_characterCount;
												this.m_linkedTextComponent.ForceMeshUpdate();
											}
											if (this.m_lineNumber > 0)
											{
												this.m_char_buffer[i] = 0;
												this.m_totalCharacterCount = this.m_characterCount;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh(true);
											return;
										}
									}
									if (num18 == 9)
									{
										float num45 = this.m_currentFontAsset.fontInfo.TabWidth * num6;
										float num46 = Mathf.Ceil(this.m_xAdvance / num45) * num45;
										float xAdvance;
										if (num46 > this.m_xAdvance)
										{
											xAdvance = num46;
										}
										else
										{
											xAdvance = this.m_xAdvance + num45;
										}
										this.m_xAdvance = xAdvance;
									}
									else if (this.m_monoSpacing != 0f)
									{
										this.m_xAdvance += (this.m_monoSpacing - num24 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (char.IsWhiteSpace((char)num18))
										{
											goto IL_2989;
										}
										if (num18 == 0x200B)
										{
											for (;;)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												goto IL_2989;
											}
										}
										goto IL_2A23;
										IL_2989:
										this.m_xAdvance += this.m_wordSpacing * num6;
									}
									else if (!this.m_isRightToLeft)
									{
										this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num8 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num18))
										{
											if (num18 != 0x200B)
											{
												goto IL_2A23;
											}
										}
										this.m_xAdvance += this.m_wordSpacing * num6;
									}
									IL_2A23:
									this.m_textInfo.characterInfo[this.m_characterCount].xAdvance = this.m_xAdvance;
									if (num18 == 0xD)
									{
										this.m_xAdvance = this.tag_Indent;
									}
									if (num18 == 0xA || this.m_characterCount == totalCharacterCount - 1)
									{
										if (this.m_lineNumber > 0)
										{
											if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
											{
												if (this.m_lineHeight == -32767f && !this.m_isNewPage)
												{
													float num47 = this.m_maxLineAscender - this.m_startOfLineAscender;
													this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num47);
													num30 -= num47;
													this.m_lineOffset += num47;
												}
											}
										}
										this.m_isNewPage = false;
										float num48 = this.m_maxLineAscender - this.m_lineOffset;
										float num49 = this.m_maxLineDescender - this.m_lineOffset;
										float maxDescender2;
										if (this.m_maxDescender < num49)
										{
											maxDescender2 = this.m_maxDescender;
										}
										else
										{
											maxDescender2 = num49;
										}
										this.m_maxDescender = maxDescender2;
										if (!flag5)
										{
											num15 = this.m_maxDescender;
										}
										if (this.m_useMaxVisibleDescender)
										{
											if (this.m_characterCount < this.m_maxVisibleCharacters)
											{
												if (this.m_lineNumber < this.m_maxVisibleLines)
												{
													goto IL_2B9B;
												}
											}
											flag5 = true;
										}
										IL_2B9B:
										this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
										TMP_LineInfo[] lineInfo4 = this.m_textInfo.lineInfo;
										int lineNumber4 = this.m_lineNumber;
										int firstVisibleCharacterOfLine;
										if (this.m_firstCharacterOfLine > this.m_firstVisibleCharacterOfLine)
										{
											firstVisibleCharacterOfLine = this.m_firstCharacterOfLine;
										}
										else
										{
											firstVisibleCharacterOfLine = this.m_firstVisibleCharacterOfLine;
										}
										lineInfo4[lineNumber4].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = firstVisibleCharacterOfLine);
										this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = this.m_characterCount);
										TMP_LineInfo[] lineInfo5 = this.m_textInfo.lineInfo;
										int lineNumber5 = this.m_lineNumber;
										int lastVisibleCharacterOfLine2;
										if (this.m_lastVisibleCharacterOfLine < this.m_firstVisibleCharacterOfLine)
										{
											lastVisibleCharacterOfLine2 = this.m_firstVisibleCharacterOfLine;
										}
										else
										{
											lastVisibleCharacterOfLine2 = this.m_lastVisibleCharacterOfLine;
										}
										lineInfo5[lineNumber5].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = lastVisibleCharacterOfLine2);
										this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
										this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
										this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num49);
										this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num48);
										this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x - num7 * num6;
										this.m_textInfo.lineInfo[this.m_lineNumber].width = num13;
										if (this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
										{
											this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
										}
										if (this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].isVisible)
										{
											this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 - this.m_cSpacing;
										}
										else
										{
											this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num6 - this.m_cSpacing;
										}
										this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
										this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num48;
										this.m_textInfo.lineInfo[this.m_lineNumber].descender = num49;
										this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num48 - num49 + num9 * num5;
										this.m_firstCharacterOfLine = this.m_characterCount + 1;
										this.m_lineVisibleCharacterCount = 0;
										if (num18 == 0xA)
										{
											base.SaveWordWrappingState(ref this.m_SavedLineState, i, this.m_characterCount);
											base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
											this.m_lineNumber++;
											flag4 = true;
											if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
											{
												base.ResizeLineExtents(this.m_lineNumber);
											}
											if (this.m_lineHeight == -32767f)
											{
												float num43 = 0f - this.m_maxLineDescender + num27 + (num9 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num5;
												this.m_lineOffset += num43;
											}
											else
											{
												this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num5;
											}
											this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
											this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
											this.m_startOfLineAscender = num27;
											this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
											num12 = this.m_characterCount - 1;
											this.m_characterCount++;
											goto IL_36E7;
										}
									}
									if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
									{
										this.m_meshExtents.min.x = Mathf.Min(this.m_meshExtents.min.x, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.x);
										this.m_meshExtents.min.y = Mathf.Min(this.m_meshExtents.min.y, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.y);
										this.m_meshExtents.max.x = Mathf.Max(this.m_meshExtents.max.x, this.m_textInfo.characterInfo[this.m_characterCount].topRight.x);
										this.m_meshExtents.max.y = Mathf.Max(this.m_meshExtents.max.y, this.m_textInfo.characterInfo[this.m_characterCount].topRight.y);
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
									{
										if (num18 != 0xD)
										{
											if (num18 != 0xA)
											{
												if (this.m_pageNumber + 1 > this.m_textInfo.pageInfo.Length)
												{
													TMP_TextInfo.Resize<TMP_PageInfo>(ref this.m_textInfo.pageInfo, this.m_pageNumber + 1, true);
												}
												this.m_textInfo.pageInfo[this.m_pageNumber].ascender = num14;
												TMP_PageInfo[] pageInfo = this.m_textInfo.pageInfo;
												int pageNumber = this.m_pageNumber;
												float descender2;
												if (num29 < this.m_textInfo.pageInfo[this.m_pageNumber].descender)
												{
													descender2 = num29;
												}
												else
												{
													descender2 = this.m_textInfo.pageInfo[this.m_pageNumber].descender;
												}
												pageInfo[pageNumber].descender = descender2;
												if (this.m_pageNumber == 0)
												{
													if (this.m_characterCount == 0)
													{
														this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
														goto IL_3403;
													}
												}
												if (this.m_characterCount > 0)
												{
													if (this.m_pageNumber != (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].pageNumber)
													{
														this.m_textInfo.pageInfo[this.m_pageNumber - 1].lastCharacterIndex = this.m_characterCount - 1;
														this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
														goto IL_3403;
													}
												}
												if (this.m_characterCount == totalCharacterCount - 1)
												{
													this.m_textInfo.pageInfo[this.m_pageNumber].lastCharacterIndex = this.m_characterCount;
												}
											}
										}
									}
									IL_3403:
									if (this.m_enableWordWrapping)
									{
										goto IL_3434;
									}
									if (this.m_overflowMode == TextOverflowModes.Truncate)
									{
										goto IL_3434;
									}
									if (this.m_overflowMode == TextOverflowModes.Ellipsis)
									{
										goto IL_3434;
									}
									IL_36D9:
									this.m_characterCount++;
									goto IL_36E7;
									IL_3434:
									if (char.IsWhiteSpace((char)num18))
									{
										goto IL_3479;
									}
									if (num18 == 0x200B)
									{
										goto IL_3479;
									}
									if (num18 == 0x2D)
									{
										goto IL_3479;
									}
									if (num18 == 0xAD)
									{
										goto IL_3479;
									}
									IL_34FE:
									if (num18 > 0x1100 && num18 < 0x11FF)
									{
										goto IL_35E5;
									}
									if (num18 > 0x2E80)
									{
										if (num18 < 0x9FFF)
										{
											goto IL_35E5;
										}
									}
									if (num18 > 0xA960)
									{
										if (num18 < 0xA97F)
										{
											goto IL_35E5;
										}
									}
									if (num18 > 0xAC00)
									{
										if (num18 < 0xD7FF)
										{
											goto IL_35E5;
										}
									}
									if (num18 > 0xF900)
									{
										if (num18 < 0xFAFF)
										{
											goto IL_35E5;
										}
									}
									if (num18 > 0xFE30)
									{
										if (num18 < 0xFE4F)
										{
											goto IL_35E5;
										}
									}
									if (num18 > 0xFF00)
									{
										if (num18 < 0xFFEF)
										{
											goto IL_35E5;
										}
									}
									IL_3697:
									if (!flag6)
									{
										if (!this.m_isCharacterWrappingEnabled)
										{
											if (!flag8)
											{
												goto IL_36D9;
											}
										}
									}
									base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
									goto IL_36D9;
									IL_35E5:
									if (!this.m_isNonBreakingSpace)
									{
										if (flag6)
										{
											goto IL_3677;
										}
										if (flag8)
										{
											goto IL_3677;
										}
										if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num18) && this.m_characterCount < totalCharacterCount - 1)
										{
											if (!TMP_Settings.linebreakingRules.followingCharacters.ContainsKey((int)this.m_textInfo.characterInfo[this.m_characterCount + 1].character))
											{
												goto IL_3677;
											}
										}
										IL_3695:
										goto IL_36D9;
										IL_3677:
										base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag6 = false;
										goto IL_3695;
									}
									goto IL_3697;
									IL_3479:
									if (this.m_isNonBreakingSpace)
									{
										if (!flag7)
										{
											goto IL_34FE;
										}
									}
									if (num18 == 0xA0)
									{
										goto IL_34FE;
									}
									if (num18 == 0x2011 || num18 == 0x202F)
									{
										goto IL_34FE;
									}
									if (num18 != 0x2060)
									{
										base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag6 = false;
										goto IL_36D9;
									}
									goto IL_34FE;
									IL_23B5:
									this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
									goto IL_23D6;
									IL_238D:
									if (this.m_textInfo.lineInfo[this.m_lineNumber].characterCount != 1)
									{
										goto IL_23D6;
									}
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										goto IL_23B5;
									}
									IL_147B:
									this.m_maxAscender = ((this.m_maxAscender <= num27) ? num27 : this.m_maxAscender);
									this.m_maxCapHeight = Mathf.Max(this.m_maxCapHeight, this.m_currentFontAsset.fontInfo.CapHeight * num6);
									goto IL_14BA;
									IL_13D9:
									float num50 = (num27 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num27 = this.m_maxLineAscender;
									float maxLineAscender;
									if (num50 > this.m_maxLineAscender)
									{
										maxLineAscender = num50;
									}
									else
									{
										maxLineAscender = this.m_maxLineAscender;
									}
									this.m_maxLineAscender = maxLineAscender;
									float num51 = (num29 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num29 = this.m_maxLineDescender;
									this.m_maxLineDescender = ((num51 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num51);
									goto IL_1461;
									IL_E4E:
									if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
									{
										float float2 = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
										num25 = this.m_currentFontAsset.normalStyle / 4f * float2 * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
										if (num25 + num7 > float2)
										{
											num7 = float2 - num25;
										}
									}
									else
									{
										num25 = 0f;
									}
									num8 = 1f;
									goto IL_EC7;
								}
							}
							IL_371B:
							float num52 = this.m_maxFontSize - this.m_minFontSize;
							if (!this.m_isCharacterWrappingEnabled)
							{
								if (this.m_enableAutoSizing && num52 > 0.051f)
								{
									if (this.m_fontSize < this.m_fontSizeMax)
									{
										this.m_minFontSize = this.m_fontSize;
										this.m_fontSize += Mathf.Max((this.m_maxFontSize - this.m_fontSize) / 2f, 0.05f);
										this.m_fontSize = (float)((int)(Mathf.Min(this.m_fontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
										if (this.loopCountA > 0x14)
										{
											return;
										}
										this.GenerateTextMesh();
										return;
									}
								}
							}
							this.m_isCharacterWrappingEnabled = false;
							if (this.m_characterCount == 0)
							{
								this.ClearMesh(true);
								TMPro_EventManager.ON_TEXT_CHANGED(this);
								return;
							}
							int num53 = this.m_materialReferences[0].referenceCount * (this.m_isVolumetricText ? 8 : 4);
							this.m_textInfo.meshInfo[0].Clear(false);
							Vector3 a = Vector3.zero;
							Vector3[] rectTransformCorners = this.m_RectTransformCorners;
							TextAlignmentOptions textAlignment = this.m_textAlignment;
							switch (textAlignment)
							{
							case TextAlignmentOptions.TopLeft:
							case TextAlignmentOptions.Top:
							case TextAlignmentOptions.TopRight:
							case TextAlignmentOptions.TopJustified:
								break;
							default:
								switch (textAlignment)
								{
								case TextAlignmentOptions.Left:
								case TextAlignmentOptions.Center:
								case TextAlignmentOptions.Right:
								case TextAlignmentOptions.Justified:
									break;
								default:
									switch (textAlignment)
									{
									case TextAlignmentOptions.BottomLeft:
									case TextAlignmentOptions.Bottom:
									case TextAlignmentOptions.BottomRight:
									case TextAlignmentOptions.BottomJustified:
										break;
									default:
										switch (textAlignment)
										{
										case TextAlignmentOptions.BaselineLeft:
										case TextAlignmentOptions.Baseline:
										case TextAlignmentOptions.BaselineRight:
										case TextAlignmentOptions.BaselineJustified:
											break;
										default:
											switch (textAlignment)
											{
											case TextAlignmentOptions.MidlineLeft:
											case TextAlignmentOptions.Midline:
											case TextAlignmentOptions.MidlineRight:
											case TextAlignmentOptions.MidlineJustified:
												break;
											default:
												switch (textAlignment)
												{
												case TextAlignmentOptions.CaplineLeft:
												case TextAlignmentOptions.Capline:
												case TextAlignmentOptions.CaplineRight:
												case TextAlignmentOptions.CaplineJustified:
													break;
												default:
													if (textAlignment == TextAlignmentOptions.TopFlush)
													{
														goto IL_3A58;
													}
													if (textAlignment == TextAlignmentOptions.TopGeoAligned)
													{
														goto IL_3A58;
													}
													if (textAlignment == TextAlignmentOptions.Flush)
													{
														goto IL_3AF7;
													}
													if (textAlignment == TextAlignmentOptions.CenterGeoAligned)
													{
														goto IL_3AF7;
													}
													if (textAlignment == TextAlignmentOptions.BottomFlush || textAlignment == TextAlignmentOptions.BottomGeoAligned)
													{
														goto IL_3C08;
													}
													if (textAlignment == TextAlignmentOptions.BaselineFlush)
													{
														goto IL_3CA3;
													}
													if (textAlignment == TextAlignmentOptions.BaselineGeoAligned)
													{
														goto IL_3CA3;
													}
													if (textAlignment == TextAlignmentOptions.MidlineFlush)
													{
														goto IL_3CEE;
													}
													if (textAlignment == TextAlignmentOptions.MidlineGeoAligned)
													{
														goto IL_3CEE;
													}
													if (textAlignment != TextAlignmentOptions.CaplineFlush)
													{
														if (textAlignment != TextAlignmentOptions.CaplineGeoAligned)
														{
															goto IL_3DDB;
														}
													}
													break;
												}
												a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxCapHeight - margin.y - margin.w) / 2f, 0f);
												goto IL_3DDB;
											}
											IL_3CEE:
											a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_meshExtents.max.y + margin.y + this.m_meshExtents.min.y - margin.w) / 2f, 0f);
											goto IL_3DDB;
										}
										IL_3CA3:
										a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
										goto IL_3DDB;
									}
									IL_3C08:
									if (this.m_overflowMode != TextOverflowModes.Page)
									{
										a = rectTransformCorners[0] + new Vector3(margin.x, 0f - num15 + margin.w, 0f);
									}
									else
									{
										a = rectTransformCorners[0] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num10].descender + margin.w, 0f);
									}
									goto IL_3DDB;
								}
								IL_3AF7:
								if (this.m_overflowMode != TextOverflowModes.Page)
								{
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxAscender + margin.y + num15 - margin.w) / 2f, 0f);
								}
								else
								{
									a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_textInfo.pageInfo[num10].ascender + margin.y + this.m_textInfo.pageInfo[num10].descender - margin.w) / 2f, 0f);
								}
								goto IL_3DDB;
							}
							IL_3A58:
							if (this.m_overflowMode != TextOverflowModes.Page)
							{
								a = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_maxAscender - margin.y, 0f);
							}
							else
							{
								a = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num10].ascender - margin.y, 0f);
							}
							IL_3DDB:
							Vector3 vector6 = Vector3.zero;
							Vector3 b4 = Vector3.zero;
							int index_X = 0;
							int index_X2 = 0;
							int num54 = 0;
							int num55 = 0;
							int num56 = 0;
							bool flag11 = false;
							bool flag12 = false;
							int num57 = 0;
							float num58 = this.m_previousLossyScaleY = this.transform.lossyScale.y;
							Color32 color = Color.white;
							Color32 underlineColor = Color.white;
							Color32 highlightColor = new Color32(byte.MaxValue, byte.MaxValue, 0, 0x40);
							float num59 = 0f;
							float num60 = 0f;
							float num61 = 0f;
							float num62 = TMP_Text.k_LargePositiveFloat;
							int num63 = 0;
							float num64 = 0f;
							float num65 = 0f;
							float b5 = 0f;
							TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
							int j = 0;
							while (j < this.m_characterCount)
							{
								TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
								char character2 = characterInfo[j].character;
								int lineNumber6 = (int)characterInfo[j].lineNumber;
								TMP_LineInfo tmp_LineInfo = this.m_textInfo.lineInfo[lineNumber6];
								num55 = lineNumber6 + 1;
								TextAlignmentOptions alignment = tmp_LineInfo.alignment;
								switch (alignment)
								{
								case TextAlignmentOptions.TopLeft:
									goto IL_40EE;
								case TextAlignmentOptions.Top:
									goto IL_4133;
								default:
									switch (alignment)
									{
									case TextAlignmentOptions.Left:
										goto IL_40EE;
									case TextAlignmentOptions.Center:
										goto IL_4133;
									default:
										switch (alignment)
										{
										case TextAlignmentOptions.BottomLeft:
											goto IL_40EE;
										case TextAlignmentOptions.Bottom:
											goto IL_4133;
										default:
											switch (alignment)
											{
											case TextAlignmentOptions.BaselineLeft:
												goto IL_40EE;
											case TextAlignmentOptions.Baseline:
												goto IL_4133;
											default:
												switch (alignment)
												{
												case TextAlignmentOptions.MidlineLeft:
													goto IL_40EE;
												case TextAlignmentOptions.Midline:
													goto IL_4133;
												default:
													switch (alignment)
													{
													case TextAlignmentOptions.CaplineLeft:
														goto IL_40EE;
													case TextAlignmentOptions.Capline:
														goto IL_4133;
													default:
														if (alignment == TextAlignmentOptions.TopFlush)
														{
															goto IL_4218;
														}
														if (alignment != TextAlignmentOptions.TopGeoAligned)
														{
															if (alignment == TextAlignmentOptions.Flush)
															{
																goto IL_4218;
															}
															if (alignment != TextAlignmentOptions.CenterGeoAligned)
															{
																if (alignment == TextAlignmentOptions.BottomFlush)
																{
																	goto IL_4218;
																}
																if (alignment != TextAlignmentOptions.BottomGeoAligned)
																{
																	if (alignment == TextAlignmentOptions.BaselineFlush)
																	{
																		goto IL_4218;
																	}
																	if (alignment != TextAlignmentOptions.BaselineGeoAligned)
																	{
																		if (alignment == TextAlignmentOptions.MidlineFlush)
																		{
																			goto IL_4218;
																		}
																		if (alignment != TextAlignmentOptions.MidlineGeoAligned)
																		{
																			if (alignment == TextAlignmentOptions.CaplineFlush)
																			{
																				goto IL_4218;
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
														vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - (tmp_LineInfo.lineExtents.min.x + tmp_LineInfo.lineExtents.max.x) / 2f, 0f, 0f);
														break;
													case TextAlignmentOptions.CaplineRight:
														goto IL_41C1;
													case TextAlignmentOptions.CaplineJustified:
														goto IL_4218;
													}
													break;
												case TextAlignmentOptions.MidlineRight:
													goto IL_41C1;
												case TextAlignmentOptions.MidlineJustified:
													goto IL_4218;
												}
												break;
											case TextAlignmentOptions.BaselineRight:
												goto IL_41C1;
											case TextAlignmentOptions.BaselineJustified:
												goto IL_4218;
											}
											break;
										case TextAlignmentOptions.BottomRight:
											goto IL_41C1;
										case TextAlignmentOptions.BottomJustified:
											goto IL_4218;
										}
										break;
									case TextAlignmentOptions.Right:
										goto IL_41C1;
									case TextAlignmentOptions.Justified:
										goto IL_4218;
									}
									break;
								case TextAlignmentOptions.TopRight:
									goto IL_41C1;
								case TextAlignmentOptions.TopJustified:
									goto IL_4218;
								}
								IL_4528:
								b4 = a + vector6;
								bool isVisible = characterInfo[j].isVisible;
								if (isVisible)
								{
									TMP_TextElementType elementType = characterInfo[j].elementType;
									if (elementType != TMP_TextElementType.Character)
									{
										if (elementType != TMP_TextElementType.Sprite)
										{
										}
									}
									else
									{
										Extents lineExtents = tmp_LineInfo.lineExtents;
										float num66 = this.m_uvLineOffset * (float)lineNumber6 % 1f;
										switch (this.m_horizontalMapping)
										{
										case TextureMappingOptions.Character:
											characterInfo[j].vertex_BL.uv2.x = 0f;
											characterInfo[j].vertex_TL.uv2.x = 0f;
											characterInfo[j].vertex_TR.uv2.x = 1f;
											characterInfo[j].vertex_BR.uv2.x = 1f;
											break;
										case TextureMappingOptions.Line:
											if (this.m_textAlignment != TextAlignmentOptions.Justified)
											{
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num66;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num66;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num66;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num66;
											}
											else
											{
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
											}
											break;
										case TextureMappingOptions.Paragraph:
											characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
											characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
											characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
											characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num66;
											break;
										case TextureMappingOptions.MatchAspect:
										{
											switch (this.m_verticalMapping)
											{
											case TextureMappingOptions.Character:
												characterInfo[j].vertex_BL.uv2.y = 0f;
												characterInfo[j].vertex_TL.uv2.y = 1f;
												characterInfo[j].vertex_TR.uv2.y = 0f;
												characterInfo[j].vertex_BR.uv2.y = 1f;
												break;
											case TextureMappingOptions.Line:
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num66;
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num66;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											case TextureMappingOptions.Paragraph:
												characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num66;
												characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num66;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											case TextureMappingOptions.MatchAspect:
												Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
												break;
											}
											float num67 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
											characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num67 + num66;
											characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
											characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num67 + num66;
											characterInfo[j].vertex_BR.uv2.x = characterInfo[j].vertex_TR.uv2.x;
											break;
										}
										}
										switch (this.m_verticalMapping)
										{
										case TextureMappingOptions.Character:
											characterInfo[j].vertex_BL.uv2.y = 0f;
											characterInfo[j].vertex_TL.uv2.y = 1f;
											characterInfo[j].vertex_TR.uv2.y = 1f;
											characterInfo[j].vertex_BR.uv2.y = 0f;
											break;
										case TextureMappingOptions.Line:
											characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender);
											characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - tmp_LineInfo.descender) / (tmp_LineInfo.ascender - tmp_LineInfo.descender);
											characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
											characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
											break;
										case TextureMappingOptions.Paragraph:
											characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y);
											characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y);
											characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
											characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
											break;
										case TextureMappingOptions.MatchAspect:
										{
											float num68 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
											characterInfo[j].vertex_BL.uv2.y = num68 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
											characterInfo[j].vertex_TL.uv2.y = num68 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
											characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
											characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
											break;
										}
										}
										num59 = characterInfo[j].scale * num58 * (1f - this.m_charWidthAdjDelta);
										if (!characterInfo[j].isUsingAlternateTypeface)
										{
											if ((characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
											{
												num59 *= -1f;
											}
										}
										float num69 = characterInfo[j].vertex_BL.uv2.x;
										float num70 = characterInfo[j].vertex_BL.uv2.y;
										float num71 = characterInfo[j].vertex_TR.uv2.x;
										float num72 = characterInfo[j].vertex_TR.uv2.y;
										float num73 = (float)((int)num69);
										float num74 = (float)((int)num70);
										num69 -= num73;
										num71 -= num73;
										num70 -= num74;
										num72 -= num74;
										characterInfo[j].vertex_BL.uv2.x = base.PackUV(num69, num70);
										characterInfo[j].vertex_BL.uv2.y = num59;
										characterInfo[j].vertex_TL.uv2.x = base.PackUV(num69, num72);
										characterInfo[j].vertex_TL.uv2.y = num59;
										characterInfo[j].vertex_TR.uv2.x = base.PackUV(num71, num72);
										characterInfo[j].vertex_TR.uv2.y = num59;
										characterInfo[j].vertex_BR.uv2.x = base.PackUV(num71, num70);
										characterInfo[j].vertex_BR.uv2.y = num59;
									}
									if (j >= this.m_maxVisibleCharacters)
									{
										goto IL_5610;
									}
									if (num54 >= this.m_maxVisibleWords || lineNumber6 >= this.m_maxVisibleLines)
									{
										goto IL_5610;
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
									{
										goto IL_5610;
									}
									TMP_CharacterInfo[] array = characterInfo;
									int num75 = j;
									array[num75].vertex_BL.position = array[num75].vertex_BL.position + b4;
									TMP_CharacterInfo[] array2 = characterInfo;
									int num76 = j;
									array2[num76].vertex_TL.position = array2[num76].vertex_TL.position + b4;
									TMP_CharacterInfo[] array3 = characterInfo;
									int num77 = j;
									array3[num77].vertex_TR.position = array3[num77].vertex_TR.position + b4;
									TMP_CharacterInfo[] array4 = characterInfo;
									int num78 = j;
									array4[num78].vertex_BR.position = array4[num78].vertex_BR.position + b4;
									IL_5781:
									if (elementType == TMP_TextElementType.Character)
									{
										this.FillCharacterVertexBuffers(j, index_X, this.m_isVolumetricText);
										goto IL_57B0;
									}
									if (elementType == TMP_TextElementType.Sprite)
									{
										this.FillSpriteVertexBuffers(j, index_X2);
										goto IL_57B0;
									}
									goto IL_57B0;
									IL_5610:
									if (j < this.m_maxVisibleCharacters)
									{
										if (num54 < this.m_maxVisibleWords)
										{
											if (lineNumber6 < this.m_maxVisibleLines)
											{
												if (this.m_overflowMode == TextOverflowModes.Page)
												{
													if ((int)characterInfo[j].pageNumber == num10)
													{
														TMP_CharacterInfo[] array5 = characterInfo;
														int num79 = j;
														array5[num79].vertex_BL.position = array5[num79].vertex_BL.position + b4;
														TMP_CharacterInfo[] array6 = characterInfo;
														int num80 = j;
														array6[num80].vertex_TL.position = array6[num80].vertex_TL.position + b4;
														TMP_CharacterInfo[] array7 = characterInfo;
														int num81 = j;
														array7[num81].vertex_TR.position = array7[num81].vertex_TR.position + b4;
														TMP_CharacterInfo[] array8 = characterInfo;
														int num82 = j;
														array8[num82].vertex_BR.position = array8[num82].vertex_BR.position + b4;
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
								IL_57B0:
								TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
								int num83 = j;
								characterInfo2[num83].bottomLeft = characterInfo2[num83].bottomLeft + b4;
								TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
								int num84 = j;
								characterInfo3[num84].topLeft = characterInfo3[num84].topLeft + b4;
								TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
								int num85 = j;
								characterInfo4[num85].topRight = characterInfo4[num85].topRight + b4;
								TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
								int num86 = j;
								characterInfo5[num86].bottomRight = characterInfo5[num86].bottomRight + b4;
								TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
								int num87 = j;
								characterInfo6[num87].origin = characterInfo6[num87].origin + b4.x;
								TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
								int num88 = j;
								characterInfo7[num88].xAdvance = characterInfo7[num88].xAdvance + b4.x;
								TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
								int num89 = j;
								characterInfo8[num89].ascender = characterInfo8[num89].ascender + b4.y;
								TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
								int num90 = j;
								characterInfo9[num90].descender = characterInfo9[num90].descender + b4.y;
								TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
								int num91 = j;
								characterInfo10[num91].baseLine = characterInfo10[num91].baseLine + b4.y;
								if (isVisible)
								{
								}
								if (lineNumber6 != num56)
								{
									goto IL_5924;
								}
								if (j == this.m_characterCount - 1)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										goto IL_5924;
									}
								}
								IL_5BD2:
								if (char.IsLetterOrDigit(character2))
								{
									goto IL_5C1D;
								}
								if (character2 == '-')
								{
									goto IL_5C1D;
								}
								if (character2 == '­' || character2 == '‐')
								{
									goto IL_5C1D;
								}
								if (character2 != '‑')
								{
									if (!flag12)
									{
										if (j != 0)
										{
											goto IL_5F83;
										}
										if (char.IsPunctuation(character2))
										{
											if (!char.IsWhiteSpace(character2))
											{
												if (character2 != '​')
												{
													if (j != this.m_characterCount - 1)
													{
														goto IL_5F83;
													}
												}
											}
										}
									}
									if (j > 0)
									{
										if (j < characterInfo.Length - 1)
										{
											if (j < this.m_characterCount)
											{
												if (character2 != '\'')
												{
													if (character2 != '’')
													{
														goto IL_5E62;
													}
												}
												if (char.IsLetterOrDigit(characterInfo[j - 1].character))
												{
													if (char.IsLetterOrDigit(characterInfo[j + 1].character))
													{
														goto IL_5F83;
													}
												}
											}
										}
									}
									IL_5E62:
									if (j != this.m_characterCount - 1)
									{
										goto IL_5E8F;
									}
									if (!char.IsLetterOrDigit(character2))
									{
										goto IL_5E8F;
									}
									int num92 = j;
									IL_5E93:
									int num93 = num92;
									flag12 = false;
									int num94 = this.m_textInfo.wordInfo.Length;
									int wordCount = this.m_textInfo.wordCount;
									if (this.m_textInfo.wordCount + 1 > num94)
									{
										TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num94 + 1);
									}
									this.m_textInfo.wordInfo[wordCount].firstCharacterIndex = num57;
									this.m_textInfo.wordInfo[wordCount].lastCharacterIndex = num93;
									this.m_textInfo.wordInfo[wordCount].characterCount = num93 - num57 + 1;
									this.m_textInfo.wordInfo[wordCount].textComponent = this;
									num54++;
									this.m_textInfo.wordCount++;
									TMP_LineInfo[] lineInfo6 = this.m_textInfo.lineInfo;
									int num95 = lineNumber6;
									lineInfo6[num95].wordCount = lineInfo6[num95].wordCount + 1;
									goto IL_5F83;
									IL_5E8F:
									num92 = j - 1;
									goto IL_5E93;
								}
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									goto IL_5C1D;
								}
								IL_5F83:
								bool flag13 = (this.m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline;
								if (flag13)
								{
									bool flag14 = true;
									int pageNumber2 = (int)this.m_textInfo.characterInfo[j].pageNumber;
									if (j > this.m_maxVisibleCharacters)
									{
										goto IL_6015;
									}
									if (lineNumber6 > this.m_maxVisibleLines)
									{
										goto IL_6015;
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
									{
										if (pageNumber2 + 1 != this.m_pageToDisplay)
										{
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												goto IL_6015;
											}
										}
									}
									IL_6018:
									if (!char.IsWhiteSpace(character2))
									{
										if (character2 != '​')
										{
											num61 = Mathf.Max(num61, this.m_textInfo.characterInfo[j].scale);
											num62 = Mathf.Min((pageNumber2 != num63) ? TMP_Text.k_LargePositiveFloat : num62, this.m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num61);
											num63 = pageNumber2;
										}
									}
									if (!flag && flag14 && j <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r')
									{
										if (j == tmp_LineInfo.lastVisibleCharacterIndex)
										{
											if (char.IsSeparator(character2))
											{
												goto IL_617C;
											}
										}
										flag = true;
										num60 = this.m_textInfo.characterInfo[j].scale;
										if (num61 == 0f)
										{
											num61 = num60;
										}
										zero = new Vector3(this.m_textInfo.characterInfo[j].bottomLeft.x, num62, 0f);
										color = this.m_textInfo.characterInfo[j].underlineColor;
									}
									IL_617C:
									if (flag && this.m_characterCount == 1)
									{
										flag = false;
										zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num62, 0f);
										float scale = this.m_textInfo.characterInfo[j].scale;
										this.DrawUnderlineMesh(zero, zero2, ref num53, num60, scale, num61, num59, color);
										num61 = 0f;
										num62 = TMP_Text.k_LargePositiveFloat;
									}
									else
									{
										if (flag)
										{
											if (j != tmp_LineInfo.lastCharacterIndex)
											{
												if (j < tmp_LineInfo.lastVisibleCharacterIndex)
												{
													goto IL_62F1;
												}
											}
											float scale;
											if (char.IsWhiteSpace(character2) || character2 == '​')
											{
												int lastVisibleCharacterIndex = tmp_LineInfo.lastVisibleCharacterIndex;
												zero2 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num62, 0f);
												scale = this.m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
											}
											else
											{
												zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num62, 0f);
												scale = this.m_textInfo.characterInfo[j].scale;
											}
											flag = false;
											this.DrawUnderlineMesh(zero, zero2, ref num53, num60, scale, num61, num59, color);
											num61 = 0f;
											num62 = TMP_Text.k_LargePositiveFloat;
											goto IL_6431;
										}
										IL_62F1:
										if (flag)
										{
											if (!flag14)
											{
												flag = false;
												zero2 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, num62, 0f);
												float scale = this.m_textInfo.characterInfo[j - 1].scale;
												this.DrawUnderlineMesh(zero, zero2, ref num53, num60, scale, num61, num59, color);
												num61 = 0f;
												num62 = TMP_Text.k_LargePositiveFloat;
												goto IL_6431;
											}
										}
										if (flag)
										{
											if (j < this.m_characterCount - 1 && !color.Compare(this.m_textInfo.characterInfo[j + 1].underlineColor))
											{
												flag = false;
												zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num62, 0f);
												float scale = this.m_textInfo.characterInfo[j].scale;
												this.DrawUnderlineMesh(zero, zero2, ref num53, num60, scale, num61, num59, color);
												num61 = 0f;
												num62 = TMP_Text.k_LargePositiveFloat;
											}
										}
									}
									IL_6431:
									goto IL_64AF;
									IL_6015:
									flag14 = false;
									goto IL_6018;
								}
								if (flag)
								{
									flag = false;
									zero2 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, num62, 0f);
									float scale = this.m_textInfo.characterInfo[j - 1].scale;
									this.DrawUnderlineMesh(zero, zero2, ref num53, num60, scale, num61, num59, color);
									num61 = 0f;
									num62 = TMP_Text.k_LargePositiveFloat;
								}
								IL_64AF:
								bool flag15 = (this.m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
								float strikethrough = fontAsset.fontInfo.strikethrough;
								if (flag15)
								{
									bool flag16 = true;
									if (j > this.m_maxVisibleCharacters || lineNumber6 > this.m_maxVisibleLines)
									{
										goto IL_6545;
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
									{
										if ((int)(this.m_textInfo.characterInfo[j].pageNumber + 1) != this.m_pageToDisplay)
										{
											for (;;)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												goto IL_6545;
											}
										}
									}
									IL_6548:
									if (!flag2 && flag16)
									{
										if (j <= tmp_LineInfo.lastVisibleCharacterIndex)
										{
											if (character2 != '\n' && character2 != '\r')
											{
												if (j == tmp_LineInfo.lastVisibleCharacterIndex && char.IsSeparator(character2))
												{
												}
												else
												{
													flag2 = true;
													num64 = this.m_textInfo.characterInfo[j].pointSize;
													num65 = this.m_textInfo.characterInfo[j].scale;
													zero3 = new Vector3(this.m_textInfo.characterInfo[j].bottomLeft.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num65, 0f);
													underlineColor = this.m_textInfo.characterInfo[j].strikethroughColor;
													b5 = this.m_textInfo.characterInfo[j].baseLine;
												}
											}
										}
									}
									if (!flag2)
									{
										goto IL_66EC;
									}
									if (this.m_characterCount != 1)
									{
										goto IL_66EC;
									}
									flag2 = false;
									zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num65, 0f);
									this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
									IL_6A58:
									goto IL_6AC0;
									IL_66EC:
									if (flag2)
									{
										if (j == tmp_LineInfo.lastCharacterIndex)
										{
											if (char.IsWhiteSpace(character2))
											{
												goto IL_6731;
											}
											if (character2 == '​')
											{
												goto IL_6731;
											}
											zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num65, 0f);
											IL_67C6:
											flag2 = false;
											this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
											goto IL_6A58;
											IL_6731:
											int lastVisibleCharacterIndex2 = tmp_LineInfo.lastVisibleCharacterIndex;
											zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num65, 0f);
											goto IL_67C6;
										}
									}
									if (flag2 && j < this.m_characterCount)
									{
										if (this.m_textInfo.characterInfo[j + 1].pointSize == num64)
										{
											if (TMP_Math.Approximately(this.m_textInfo.characterInfo[j + 1].baseLine + b4.y, b5))
											{
												goto IL_6925;
											}
										}
										flag2 = false;
										int lastVisibleCharacterIndex3 = tmp_LineInfo.lastVisibleCharacterIndex;
										if (j > lastVisibleCharacterIndex3)
										{
											zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num65, 0f);
										}
										else
										{
											zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num65, 0f);
										}
										this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
										goto IL_6A58;
									}
									IL_6925:
									if (flag2)
									{
										if (j < this.m_characterCount)
										{
											if (fontAsset.GetInstanceID() != characterInfo[j + 1].fontAsset.GetInstanceID())
											{
												flag2 = false;
												zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num65, 0f);
												this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
												goto IL_6A58;
											}
										}
									}
									if (!flag2)
									{
										goto IL_6A58;
									}
									if (!flag16)
									{
										flag2 = false;
										zero4 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, this.m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num65, 0f);
										this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
										goto IL_6A58;
									}
									goto IL_6A58;
									IL_6545:
									flag16 = false;
									goto IL_6548;
								}
								if (flag2)
								{
									flag2 = false;
									zero4 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, this.m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num65, 0f);
									this.DrawUnderlineMesh(zero3, zero4, ref num53, num65, num65, num65, num59, underlineColor);
								}
								IL_6AC0:
								bool flag17 = (this.m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight;
								if (flag17)
								{
									bool flag18 = true;
									int pageNumber3 = (int)this.m_textInfo.characterInfo[j].pageNumber;
									if (j > this.m_maxVisibleCharacters || lineNumber6 > this.m_maxVisibleLines)
									{
										goto IL_6B50;
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
									{
										if (pageNumber3 + 1 != this.m_pageToDisplay)
										{
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												goto IL_6B50;
											}
										}
									}
									IL_6B53:
									if (!flag3)
									{
										if (flag18)
										{
											if (j <= tmp_LineInfo.lastVisibleCharacterIndex)
											{
												if (character2 != '\n')
												{
													if (character2 != '\r')
													{
														if (j == tmp_LineInfo.lastVisibleCharacterIndex && char.IsSeparator(character2))
														{
														}
														else
														{
															flag3 = true;
															start = TMP_Text.k_LargePositiveVector2;
															vector = TMP_Text.k_LargeNegativeVector2;
															highlightColor = this.m_textInfo.characterInfo[j].highlightColor;
														}
													}
												}
											}
										}
									}
									if (flag3)
									{
										Color32 highlightColor2 = this.m_textInfo.characterInfo[j].highlightColor;
										bool flag19 = false;
										if (!highlightColor.Compare(highlightColor2))
										{
											vector.x = (vector.x + this.m_textInfo.characterInfo[j].bottomLeft.x) / 2f;
											start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[j].descender);
											vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[j].ascender);
											this.DrawTextHighlight(start, vector, ref num53, highlightColor);
											flag3 = true;
											start = vector;
											vector = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].descender, 0f);
											highlightColor = this.m_textInfo.characterInfo[j].highlightColor;
											flag19 = true;
										}
										if (!flag19)
										{
											start.x = Mathf.Min(start.x, this.m_textInfo.characterInfo[j].bottomLeft.x);
											start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[j].descender);
											vector.x = Mathf.Max(vector.x, this.m_textInfo.characterInfo[j].topRight.x);
											vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[j].ascender);
										}
									}
									if (!flag3)
									{
										goto IL_6E25;
									}
									if (this.m_characterCount != 1)
									{
										goto IL_6E25;
									}
									flag3 = false;
									this.DrawTextHighlight(start, vector, ref num53, highlightColor);
									IL_6E9D:
									goto IL_6EBE;
									IL_6E25:
									if (flag3)
									{
										if (j != tmp_LineInfo.lastCharacterIndex)
										{
											if (j < tmp_LineInfo.lastVisibleCharacterIndex)
											{
												goto IL_6E70;
											}
										}
										flag3 = false;
										this.DrawTextHighlight(start, vector, ref num53, highlightColor);
										goto IL_6E9D;
									}
									IL_6E70:
									if (!flag3)
									{
										goto IL_6E9D;
									}
									if (!flag18)
									{
										flag3 = false;
										this.DrawTextHighlight(start, vector, ref num53, highlightColor);
										goto IL_6E9D;
									}
									goto IL_6E9D;
									IL_6B50:
									flag18 = false;
									goto IL_6B53;
								}
								if (flag3)
								{
									flag3 = false;
									this.DrawTextHighlight(start, vector, ref num53, highlightColor);
								}
								IL_6EBE:
								num56 = lineNumber6;
								j++;
								continue;
								IL_5C1D:
								if (!flag12)
								{
									flag12 = true;
									num57 = j;
								}
								if (flag12)
								{
									if (j == this.m_characterCount - 1)
									{
										int num96 = this.m_textInfo.wordInfo.Length;
										int wordCount2 = this.m_textInfo.wordCount;
										if (this.m_textInfo.wordCount + 1 > num96)
										{
											TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num96 + 1);
										}
										int num93 = j;
										this.m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num57;
										this.m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num93;
										this.m_textInfo.wordInfo[wordCount2].characterCount = num93 - num57 + 1;
										this.m_textInfo.wordInfo[wordCount2].textComponent = this;
										num54++;
										this.m_textInfo.wordCount++;
										TMP_LineInfo[] lineInfo7 = this.m_textInfo.lineInfo;
										int num97 = lineNumber6;
										lineInfo7[num97].wordCount = lineInfo7[num97].wordCount + 1;
									}
								}
								goto IL_5F83;
								IL_5924:
								if (lineNumber6 != num56)
								{
									TMP_LineInfo[] lineInfo8 = this.m_textInfo.lineInfo;
									int num98 = num56;
									lineInfo8[num98].baseline = lineInfo8[num98].baseline + b4.y;
									TMP_LineInfo[] lineInfo9 = this.m_textInfo.lineInfo;
									int num99 = num56;
									lineInfo9[num99].ascender = lineInfo9[num99].ascender + b4.y;
									TMP_LineInfo[] lineInfo10 = this.m_textInfo.lineInfo;
									int num100 = num56;
									lineInfo10[num100].descender = lineInfo10[num100].descender + b4.y;
									this.m_textInfo.lineInfo[num56].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num56].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[num56].descender);
									this.m_textInfo.lineInfo[num56].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num56].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[num56].ascender);
								}
								if (j == this.m_characterCount - 1)
								{
									TMP_LineInfo[] lineInfo11 = this.m_textInfo.lineInfo;
									int num101 = lineNumber6;
									lineInfo11[num101].baseline = lineInfo11[num101].baseline + b4.y;
									TMP_LineInfo[] lineInfo12 = this.m_textInfo.lineInfo;
									int num102 = lineNumber6;
									lineInfo12[num102].ascender = lineInfo12[num102].ascender + b4.y;
									TMP_LineInfo[] lineInfo13 = this.m_textInfo.lineInfo;
									int num103 = lineNumber6;
									lineInfo13[num103].descender = lineInfo13[num103].descender + b4.y;
									this.m_textInfo.lineInfo[lineNumber6].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber6].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[lineNumber6].descender);
									this.m_textInfo.lineInfo[lineNumber6].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber6].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[lineNumber6].ascender);
									goto IL_5BD2;
								}
								goto IL_5BD2;
								IL_40EE:
								if (!this.m_isRightToLeft)
								{
									vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
								}
								else
								{
									vector6 = new Vector3(0f - tmp_LineInfo.maxAdvance, 0f, 0f);
								}
								goto IL_4528;
								IL_4133:
								vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - tmp_LineInfo.maxAdvance / 2f, 0f, 0f);
								goto IL_4528;
								IL_41C1:
								if (!this.m_isRightToLeft)
								{
									vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0f, 0f);
								}
								else
								{
									vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
								}
								goto IL_4528;
								IL_4218:
								if (character2 != '­')
								{
									if (character2 != '​' && character2 != '⁠')
									{
										char character3 = characterInfo[tmp_LineInfo.lastCharacterIndex].character;
										bool flag20 = (alignment & (TextAlignmentOptions)0x10) == (TextAlignmentOptions)0x10;
										if (char.IsControl(character3))
										{
											goto IL_427F;
										}
										if (lineNumber6 >= this.m_lineNumber)
										{
											goto IL_427F;
										}
										goto IL_42AA;
										IL_4526:
										goto IL_4528;
										IL_427F:
										if (!flag20)
										{
											if (tmp_LineInfo.maxAdvance > tmp_LineInfo.width)
											{
											}
											else
											{
												if (!this.m_isRightToLeft)
												{
													vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
													goto IL_4526;
												}
												vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
												goto IL_4526;
											}
										}
										IL_42AA:
										if (lineNumber6 != num56)
										{
											goto IL_42DF;
										}
										if (j == 0)
										{
											goto IL_42DF;
										}
										if (j == this.m_firstVisibleCharacter)
										{
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												goto IL_42DF;
											}
										}
										else
										{
											float num104;
											if (!this.m_isRightToLeft)
											{
												num104 = tmp_LineInfo.width - tmp_LineInfo.maxAdvance;
											}
											else
											{
												num104 = tmp_LineInfo.width + tmp_LineInfo.maxAdvance;
											}
											float num105 = num104;
											int num106 = tmp_LineInfo.visibleCharacterCount - 1;
											int num107;
											if (characterInfo[tmp_LineInfo.lastCharacterIndex].isVisible)
											{
												num107 = tmp_LineInfo.spaceCount;
											}
											else
											{
												num107 = tmp_LineInfo.spaceCount - 1;
											}
											int num108 = num107;
											if (flag11)
											{
												num108--;
												num106++;
											}
											float num109 = (num108 <= 0) ? 1f : this.m_wordWrappingRatios;
											if (num108 < 1)
											{
												num108 = 1;
											}
											if (character2 != '\t')
											{
												if (char.IsSeparator(character2))
												{
												}
												else
												{
													if (!this.m_isRightToLeft)
													{
														vector6 += new Vector3(num105 * num109 / (float)num106, 0f, 0f);
														goto IL_44D8;
													}
													vector6 -= new Vector3(num105 * num109 / (float)num106, 0f, 0f);
													goto IL_44D8;
												}
											}
											if (!this.m_isRightToLeft)
											{
												vector6 += new Vector3(num105 * (1f - num109) / (float)num108, 0f, 0f);
											}
											else
											{
												vector6 -= new Vector3(num105 * (1f - num109) / (float)num108, 0f, 0f);
											}
										}
										IL_44D8:
										goto IL_4526;
										IL_42DF:
										if (!this.m_isRightToLeft)
										{
											vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
										}
										else
										{
											vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
										}
										if (char.IsSeparator(character2))
										{
											flag11 = true;
										}
										else
										{
											flag11 = false;
										}
									}
								}
								goto IL_4528;
							}
							this.m_textInfo.characterCount = (int)((short)this.m_characterCount);
							this.m_textInfo.spriteCount = this.m_spriteCount;
							this.m_textInfo.lineCount = (int)((short)num55);
							TMP_TextInfo textInfo = this.m_textInfo;
							int wordCount3;
							if (num54 != 0)
							{
								if (this.m_characterCount > 0)
								{
									wordCount3 = (int)((short)num54);
									goto IL_6F3D;
								}
							}
							wordCount3 = 1;
							IL_6F3D:
							textInfo.wordCount = wordCount3;
							this.m_textInfo.pageCount = this.m_pageNumber + 1;
							if (this.m_renderMode == TextRenderFlags.Render)
							{
								if (this.m_geometrySortingOrder != VertexSortingOrder.Normal)
								{
									this.m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
								}
								this.m_mesh.MarkDynamic();
								this.m_mesh.vertices = this.m_textInfo.meshInfo[0].vertices;
								this.m_mesh.uv = this.m_textInfo.meshInfo[0].uvs0;
								this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
								this.m_mesh.colors32 = this.m_textInfo.meshInfo[0].colors32;
								this.m_mesh.RecalculateBounds();
								for (int k = 1; k < this.m_textInfo.materialCount; k++)
								{
									this.m_textInfo.meshInfo[k].ClearUnusedVertices();
									if (this.m_subTextObjects[k] == null)
									{
									}
									else
									{
										if (this.m_geometrySortingOrder != VertexSortingOrder.Normal)
										{
											this.m_textInfo.meshInfo[k].SortGeometry(VertexSortingOrder.Reverse);
										}
										this.m_subTextObjects[k].mesh.vertices = this.m_textInfo.meshInfo[k].vertices;
										this.m_subTextObjects[k].mesh.uv = this.m_textInfo.meshInfo[k].uvs0;
										this.m_subTextObjects[k].mesh.uv2 = this.m_textInfo.meshInfo[k].uvs2;
										this.m_subTextObjects[k].mesh.colors32 = this.m_textInfo.meshInfo[k].colors32;
										this.m_subTextObjects[k].mesh.RecalculateBounds();
									}
								}
							}
							TMPro_EventManager.ON_TEXT_CHANGED(this);
							return;
						}
					}
					this.ClearMesh(true);
					this.m_preferredWidth = 0f;
					this.m_preferredHeight = 0f;
					TMPro_EventManager.ON_TEXT_CHANGED(this);
					return;
				}
			}
			Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + base.GetInstanceID());
		}

		protected override Vector3[] GetTextContainerLocalCorners()
		{
			if (this.m_rectTransform == null)
			{
				this.m_rectTransform = base.rectTransform;
			}
			this.m_rectTransform.GetLocalCorners(this.m_RectTransformCorners);
			return this.m_RectTransformCorners;
		}

		private void SetMeshFilters(bool state)
		{
			if (this.m_meshFilter != null)
			{
				if (state)
				{
					this.m_meshFilter.sharedMesh = this.m_mesh;
				}
				else
				{
					this.m_meshFilter.sharedMesh = null;
				}
			}
			for (int i = 1; i < this.m_subTextObjects.Length; i++)
			{
				if (!(this.m_subTextObjects[i] != null))
				{
					break;
				}
				if (this.m_subTextObjects[i].meshFilter != null)
				{
					if (state)
					{
						this.m_subTextObjects[i].meshFilter.sharedMesh = this.m_subTextObjects[i].mesh;
					}
					else
					{
						this.m_subTextObjects[i].meshFilter.sharedMesh = null;
					}
				}
			}
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			int i = 1;
			while (i < this.m_subTextObjects.Length)
			{
				if (!(this.m_subTextObjects[i] != null))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						return;
					}
				}
				else
				{
					if (this.m_subTextObjects[i].enabled != state)
					{
						this.m_subTextObjects[i].enabled = state;
					}
					i++;
				}
			}
		}

		protected override void ClearSubMeshObjects()
		{
			int i = 1;
			while (i < this.m_subTextObjects.Length)
			{
				if (!(this.m_subTextObjects[i] != null))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						return;
					}
				}
				else
				{
					Debug.Log("Destroying Sub Text object[" + i + "].");
					UnityEngine.Object.DestroyImmediate(this.m_subTextObjects[i]);
					i++;
				}
			}
		}

		protected override Bounds GetCompoundBounds()
		{
			Bounds bounds = this.m_mesh.bounds;
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			int i = 1;
			while (i < this.m_subTextObjects.Length)
			{
				if (!(this.m_subTextObjects[i] != null))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_196;
					}
				}
				else
				{
					Bounds bounds2 = this.m_subTextObjects[i].mesh.bounds;
					float x;
					if (min.x < bounds2.min.x)
					{
						x = min.x;
					}
					else
					{
						x = bounds2.min.x;
					}
					min.x = x;
					float y;
					if (min.y < bounds2.min.y)
					{
						y = min.y;
					}
					else
					{
						y = bounds2.min.y;
					}
					min.y = y;
					float x2;
					if (max.x > bounds2.max.x)
					{
						x2 = max.x;
					}
					else
					{
						x2 = bounds2.max.x;
					}
					max.x = x2;
					float y2;
					if (max.y > bounds2.max.y)
					{
						y2 = max.y;
					}
					else
					{
						y2 = bounds2.max.y;
					}
					max.y = y2;
					i++;
				}
			}
			IL_196:
			Vector3 center = (min + max) / 2f;
			Vector2 v = max - min;
			return new Bounds(center, v);
		}

		private void UpdateSDFScale(float lossyScale)
		{
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					if (this.m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
					{
						float num = lossyScale * this.m_textInfo.characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
						if (!this.m_textInfo.characterInfo[i].isUsingAlternateTypeface)
						{
							if ((this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
							{
								num *= -1f;
							}
						}
						int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
						int vertexIndex = this.m_textInfo.characterInfo[i].vertexIndex;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num;
					}
				}
			}
			for (int j = 0; j < this.m_textInfo.meshInfo.Length; j++)
			{
				if (j == 0)
				{
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
				}
				else
				{
					this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
				}
			}
		}

		protected override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			Vector3 b = new Vector3(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
				int num = i;
				characterInfo[num].bottomLeft = characterInfo[num].bottomLeft - b;
				TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
				int num2 = i;
				characterInfo2[num2].topLeft = characterInfo2[num2].topLeft - b;
				TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
				int num3 = i;
				characterInfo3[num3].topRight = characterInfo3[num3].topRight - b;
				TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
				int num4 = i;
				characterInfo4[num4].bottomRight = characterInfo4[num4].bottomRight - b;
				TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
				int num5 = i;
				characterInfo5[num5].ascender = characterInfo5[num5].ascender - b.y;
				TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
				int num6 = i;
				characterInfo6[num6].baseLine = characterInfo6[num6].baseLine - b.y;
				TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
				int num7 = i;
				characterInfo7[num7].descender = characterInfo7[num7].descender - b.y;
				if (this.m_textInfo.characterInfo[i].isVisible)
				{
					TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
					int num8 = i;
					characterInfo8[num8].vertex_BL.position = characterInfo8[num8].vertex_BL.position - b;
					TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
					int num9 = i;
					characterInfo9[num9].vertex_TL.position = characterInfo9[num9].vertex_TL.position - b;
					TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
					int num10 = i;
					characterInfo10[num10].vertex_TR.position = characterInfo10[num10].vertex_TR.position - b;
					TMP_CharacterInfo[] characterInfo11 = this.m_textInfo.characterInfo;
					int num11 = i;
					characterInfo11[num11].vertex_BR.position = characterInfo11[num11].vertex_BR.position - b;
				}
			}
		}

		public int sortingLayerID
		{
			get
			{
				return this.m_renderer.sortingLayerID;
			}
			set
			{
				this.m_renderer.sortingLayerID = value;
			}
		}

		public int sortingOrder
		{
			get
			{
				return this.m_renderer.sortingOrder;
			}
			set
			{
				this.m_renderer.sortingOrder = value;
			}
		}

		public override bool autoSizeTextContainer
		{
			get
			{
				return this.m_autoSizeTextContainer;
			}
			set
			{
				if (this.m_autoSizeTextContainer == value)
				{
					return;
				}
				this.m_autoSizeTextContainer = value;
				if (this.m_autoSizeTextContainer)
				{
					TMP_UpdateManager.RegisterTextElementForLayoutRebuild(this);
					this.SetLayoutDirty();
				}
			}
		}

		[Obsolete("The TextContainer is now obsolete. Use the RectTransform instead.")]
		public TextContainer textContainer
		{
			get
			{
				return null;
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

		public Renderer renderer
		{
			get
			{
				if (this.m_renderer == null)
				{
					this.m_renderer = base.GetComponent<Renderer>();
				}
				return this.m_renderer;
			}
		}

		public override Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
					this.meshFilter.mesh = this.m_mesh;
				}
				return this.m_mesh;
			}
		}

		public MeshFilter meshFilter
		{
			get
			{
				if (this.m_meshFilter == null)
				{
					this.m_meshFilter = base.GetComponent<MeshFilter>();
				}
				return this.m_meshFilter;
			}
		}

		public MaskingTypes maskType
		{
			get
			{
				return this.m_maskType;
			}
			set
			{
				this.m_maskType = value;
				this.SetMask(this.m_maskType);
			}
		}

		public void SetMask(MaskingTypes type, Vector4 maskCoords)
		{
			this.SetMask(type);
			this.SetMaskCoordinates(maskCoords);
		}

		public void SetMask(MaskingTypes type, Vector4 maskCoords, float softnessX, float softnessY)
		{
			this.SetMask(type);
			this.SetMaskCoordinates(maskCoords, softnessX, softnessY);
		}

		public override void SetVerticesDirty()
		{
			if (!this.m_verticesAlreadyDirty)
			{
				if (!(this == null))
				{
					if (this.IsActive())
					{
						TMP_UpdateManager.RegisterTextElementForGraphicRebuild(this);
						this.m_verticesAlreadyDirty = true;
						return;
					}
				}
			}
		}

		public override void SetLayoutDirty()
		{
			this.m_isPreferredWidthDirty = true;
			this.m_isPreferredHeightDirty = true;
			if (!this.m_layoutAlreadyDirty && !(this == null))
			{
				if (this.IsActive())
				{
					this.m_layoutAlreadyDirty = true;
					this.m_isLayoutDirty = true;
					return;
				}
			}
		}

		public override void SetMaterialDirty()
		{
			this.UpdateMaterial();
		}

		public override void SetAllDirty()
		{
			this.SetLayoutDirty();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
		}

		public override void Rebuild(CanvasUpdate update)
		{
			if (this == null)
			{
				return;
			}
			if (update == CanvasUpdate.Prelayout)
			{
				if (this.m_autoSizeTextContainer)
				{
					this.m_rectTransform.sizeDelta = base.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
				}
			}
			else if (update == CanvasUpdate.PreRender)
			{
				this.OnPreRenderObject();
				this.m_verticesAlreadyDirty = false;
				this.m_layoutAlreadyDirty = false;
				if (!this.m_isMaterialDirty)
				{
					return;
				}
				this.UpdateMaterial();
				this.m_isMaterialDirty = false;
			}
		}

		protected override void UpdateMaterial()
		{
			if (this.m_sharedMaterial == null)
			{
				return;
			}
			if (this.m_renderer == null)
			{
				this.m_renderer = this.renderer;
			}
			if (this.m_renderer.sharedMaterial.GetInstanceID() != this.m_sharedMaterial.GetInstanceID())
			{
				this.m_renderer.sharedMaterial = this.m_sharedMaterial;
			}
		}

		public override void UpdateMeshPadding()
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_havePropertiesChanged = true;
			this.checkPaddingRequired = false;
			if (this.m_textInfo == null)
			{
				return;
			}
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].UpdateMeshPadding(this.m_enableExtraPadding, this.m_isUsingBold);
			}
		}

		public override void ForceMeshUpdate()
		{
			this.m_havePropertiesChanged = true;
			this.OnPreRenderObject();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			this.m_havePropertiesChanged = true;
			this.m_ignoreActiveState = true;
			this.OnPreRenderObject();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			base.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			this.m_renderMode = TextRenderFlags.DontRender;
			this.ComputeMarginSize();
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh(bool updateMesh)
		{
			if (this.m_textInfo.meshInfo[0].mesh == null)
			{
				this.m_textInfo.meshInfo[0].mesh = this.m_mesh;
			}
			this.m_textInfo.ClearMeshInfo(updateMesh);
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
		}

		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				}
				mesh.RecalculateBounds();
			}
		}

		public override void UpdateVertexData()
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
				{
					mesh = this.m_mesh;
				}
				else
				{
					this.m_textInfo.meshInfo[i].ClearUnusedVertices();
					mesh = this.m_subTextObjects[i].mesh;
				}
				mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				mesh.uv = this.m_textInfo.meshInfo[i].uvs0;
				mesh.uv2 = this.m_textInfo.meshInfo[i].uvs2;
				mesh.colors32 = this.m_textInfo.meshInfo[i].colors32;
				mesh.RecalculateBounds();
			}
		}

		public void UpdateFontAsset()
		{
			this.LoadFontAsset();
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.m_currentAutoSizeMode = this.m_enableAutoSizing;
			if (!this.m_isCalculateSizeRequired)
			{
				if (!this.m_rectTransform.hasChanged)
				{
					return;
				}
			}
			this.m_minWidth = 0f;
			this.m_flexibleWidth = 0f;
			if (this.m_enableAutoSizing)
			{
				this.m_fontSize = this.m_fontSizeMax;
			}
			this.m_marginWidth = TMP_Text.k_LargePositiveFloat;
			this.m_marginHeight = TMP_Text.k_LargePositiveFloat;
			if (!this.m_isInputParsingRequired)
			{
				if (!this.m_isTextTruncated)
				{
					goto IL_C7;
				}
			}
			base.ParseInputText();
			IL_C7:
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			this.ComputeMarginSize();
			this.m_isLayoutDirty = true;
		}

		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.m_isCalculateSizeRequired)
			{
				if (!this.m_rectTransform.hasChanged)
				{
					goto IL_B9;
				}
			}
			this.m_minHeight = 0f;
			this.m_flexibleHeight = 0f;
			if (this.m_enableAutoSizing)
			{
				this.m_currentAutoSizeMode = true;
				this.m_enableAutoSizing = false;
			}
			this.m_marginHeight = TMP_Text.k_LargePositiveFloat;
			this.GenerateTextMesh();
			this.m_enableAutoSizing = this.m_currentAutoSizeMode;
			this.m_renderMode = TextRenderFlags.Render;
			this.ComputeMarginSize();
			this.m_isLayoutDirty = true;
			IL_B9:
			this.m_isCalculateSizeRequired = false;
		}
	}
}
