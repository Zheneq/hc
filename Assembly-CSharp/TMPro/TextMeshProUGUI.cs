using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[RequireComponent(typeof(CanvasRenderer))]
	[ExecuteInEditMode]
	[AddComponentMenu("UI/TextMeshPro - Text (UI)", 0xB)]
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

		protected override void Awake()
		{
			this.m_canvas = base.canvas;
			this.m_isOrthographic = true;
			this.m_rectTransform = base.gameObject.GetComponent<RectTransform>();
			if (this.m_rectTransform == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.Awake()).MethodHandle;
				}
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			if (this.m_canvasRenderer == null)
			{
				this.m_canvasRenderer = base.gameObject.AddComponent<CanvasRenderer>();
			}
			if (this.m_mesh == null)
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
				this.m_mesh = new Mesh();
				this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
			}
			base.LoadDefaultSettings();
			this.LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			if (this.m_char_buffer == null)
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
				this.m_char_buffer = new int[this.m_max_characters];
			}
			this.m_cached_TextElement = new TMP_Glyph();
			this.m_isFirstAllocation = true;
			if (this.m_textInfo == null)
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
				this.m_textInfo = new TMP_TextInfo(this);
			}
			if (this.m_fontAsset == null)
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
				Debug.LogWarning("Please assign a Font Asset to this " + base.transform.name + " gameobject.", this);
				return;
			}
			TMP_SubMeshUI[] componentsInChildren = base.GetComponentsInChildren<TMP_SubMeshUI>();
			if (componentsInChildren.Length > 0)
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
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.m_subTextObjects[i + 1] = componentsInChildren[i];
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
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
			this.m_canvas = this.GetCanvas();
			this.SetActiveSubMeshes(true);
			GraphicRegistry.RegisterGraphicForCanvas(this.m_canvas, this);
			this.ComputeMarginSize();
			this.m_verticesAlreadyDirty = false;
			this.m_layoutAlreadyDirty = false;
			this.m_ShouldRecalculateStencil = true;
			this.m_isInputParsingRequired = true;
			this.SetAllDirty();
			this.RecalculateClipping();
		}

		protected override void OnDisable()
		{
			if (this.m_MaskMaterial != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.OnDisable()).MethodHandle;
				}
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_canvasRenderer != null)
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
				this.m_canvasRenderer.Clear();
			}
			this.SetActiveSubMeshes(false);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_rectTransform);
			this.RecalculateClipping();
		}

		protected override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(this.m_canvas, this);
			if (this.m_mesh != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.OnDestroy()).MethodHandle;
				}
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
			if (this.m_MaskMaterial != null)
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
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			this.m_isRegisteredForEvents = false;
		}

		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (this.m_fontAsset == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.LoadFontAsset()).MethodHandle;
				}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					Debug.LogWarning("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to " + base.gameObject.name + ".", this);
					return;
				}
				if (this.m_fontAsset.characterDictionary == null)
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
					Debug.Log("Dictionary is Null!");
				}
				this.m_sharedMaterial = this.m_fontAsset.material;
			}
			else
			{
				if (this.m_fontAsset.characterDictionary == null)
				{
					this.m_fontAsset.ReadFontDefinition();
				}
				if (this.m_sharedMaterial == null && this.m_baseMaterial != null)
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
					this.m_sharedMaterial = this.m_baseMaterial;
					this.m_baseMaterial = null;
				}
				if (!(this.m_sharedMaterial == null))
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
					if (!(this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == null))
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
						if (this.m_fontAsset.atlas.GetInstanceID() == this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							goto IL_228;
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
				if (this.m_fontAsset.material == null)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"The Font Atlas Texture of the Font Asset ",
						this.m_fontAsset.name,
						" assigned to ",
						base.gameObject.name,
						" is missing."
					}), this);
				}
				else
				{
					this.m_sharedMaterial = this.m_fontAsset.material;
				}
			}
			IL_228:
			base.GetSpecialCharacters(this.m_fontAsset);
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}

		private Canvas GetCanvas()
		{
			Canvas result = null;
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			base.gameObject.GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].isActiveAndEnabled)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetCanvas()).MethodHandle;
						}
						result = list[i];
						goto IL_6E;
					}
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
			IL_6E:
			TMP_ListPool<Canvas>.Release(list);
			return result;
		}

		private void UpdateEnvMapMatrix()
		{
			if (!this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) || this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == null)
			{
				return;
			}
			Vector3 euler = this.m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation);
			this.m_EnvMapMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(euler), Vector3.one);
			this.m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, this.m_EnvMapMatrix);
		}

		private void EnableMasking()
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_canvasRenderer.SetMaterial(this.m_fontMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			if (this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				this.m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				this.m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				this.UpdateMask();
			}
			this.m_isMaskingEnabled = true;
		}

		private void DisableMasking()
		{
			if (this.m_fontMaterial != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.DisableMasking()).MethodHandle;
				}
				if (this.m_stencilID > 0)
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
					this.m_sharedMaterial = this.m_MaskMaterial;
				}
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
				UnityEngine.Object.DestroyImmediate(this.m_fontMaterial);
			}
			this.m_isMaskingEnabled = false;
		}

		private void UpdateMask()
		{
			if (this.m_rectTransform != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateMask()).MethodHandle;
				}
				if (!ShaderUtilities.isInitialized)
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
					ShaderUtilities.GetShaderPropertyIDs();
				}
				this.m_isScrollRegionSet = true;
				float num = Mathf.Min(Mathf.Min(this.m_margin.x, this.m_margin.z), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
				float num2 = Mathf.Min(Mathf.Min(this.m_margin.y, this.m_margin.w), this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
				float num3;
				if (num > 0f)
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
					num3 = num;
				}
				else
				{
					num3 = 0f;
				}
				num = num3;
				num2 = ((num2 <= 0f) ? 0f : num2);
				float z = (this.m_rectTransform.rect.width - Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f + num;
				float w = (this.m_rectTransform.rect.height - Mathf.Max(this.m_margin.y, 0f) - Mathf.Max(this.m_margin.w, 0f)) / 2f + num2;
				Vector2 vector = this.m_rectTransform.localPosition + new Vector3((0.5f - this.m_rectTransform.pivot.x) * this.m_rectTransform.rect.width + (Mathf.Max(this.m_margin.x, 0f) - Mathf.Max(this.m_margin.z, 0f)) / 2f, (0.5f - this.m_rectTransform.pivot.y) * this.m_rectTransform.rect.height + (-Mathf.Max(this.m_margin.y, 0f) + Mathf.Max(this.m_margin.w, 0f)) / 2f);
				Vector4 value = new Vector4(vector.x, vector.y, z, w);
				this.m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, value);
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if (!(this.m_fontMaterial == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetMaterial(Material)).MethodHandle;
				}
				if (this.m_fontMaterial.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_54;
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_fontMaterial = this.CreateMaterialInstance(mat);
			IL_54:
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_ShouldRecalculateStencil = true;
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontMaterials == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetMaterials(Material[])).MethodHandle;
				}
				this.m_fontMaterials = new Material[materialCount];
			}
			else if (this.m_fontMaterials.Length != materialCount)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetSharedMaterials()).MethodHandle;
				}
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
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
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
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
					this.m_fontSharedMaterials[i] = this.m_sharedMaterial;
				}
				else
				{
					this.m_fontSharedMaterials[i] = this.m_subTextObjects[i].sharedMaterial;
				}
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
			return this.m_fontSharedMaterials;
		}

		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = this.m_textInfo.materialCount;
			if (this.m_fontSharedMaterials == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetSharedMaterials(Material[])).MethodHandle;
				}
				this.m_fontSharedMaterials = new Material[materialCount];
			}
			else if (this.m_fontSharedMaterials.Length != materialCount)
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
				TMP_TextInfo.Resize<Material>(ref this.m_fontSharedMaterials, materialCount, false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
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
					if (!(materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null))
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
						if (materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
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
						}
						else
						{
							this.m_sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
							this.m_padding = this.GetPaddingForMaterial(this.m_sharedMaterial);
						}
					}
				}
				else if (!(materials[i].GetTexture(ShaderUtilities.ID_MainTex) == null))
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
					if (materials[i].GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() != this.m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
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
					}
					else if (this.m_subTextObjects[i].isDefaultMaterial)
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
						this.m_subTextObjects[i].sharedMaterial = (this.m_fontSharedMaterials[i] = materials[i]);
					}
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			if (this.m_fontMaterial != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetOutlineThickness(float)).MethodHandle;
				}
				if (this.m_sharedMaterial.GetInstanceID() != this.m_fontMaterial.GetInstanceID())
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
					this.m_sharedMaterial = this.m_fontMaterial;
					this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
					goto IL_D3;
				}
			}
			if (this.m_fontMaterial == null)
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
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
				this.m_sharedMaterial = this.m_fontMaterial;
				this.m_canvasRenderer.SetMaterial(this.m_sharedMaterial, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			IL_D3:
			thickness = Mathf.Clamp01(thickness);
			this.m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			this.m_padding = this.GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetFaceColor(Color32)).MethodHandle;
				}
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, color);
		}

		protected override void SetOutlineColor(Color32 color)
		{
			if (this.m_fontMaterial == null)
			{
				this.m_fontMaterial = this.CreateMaterialInstance(this.m_sharedMaterial);
			}
			this.m_sharedMaterial = this.m_fontMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color);
		}

		protected override void SetShaderDepth()
		{
			if (!(this.m_canvas == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetShaderDepth()).MethodHandle;
				}
				if (!(this.m_sharedMaterial == null))
				{
					if (this.m_canvas.renderMode != RenderMode.ScreenSpaceOverlay)
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
						if (!this.m_isOverlay)
						{
							this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
							return;
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
					this.m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
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
		}

		protected override void SetCulling()
		{
			if (this.m_isCullingEnabled)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetCulling()).MethodHandle;
				}
				Material materialForRendering = this.materialForRendering;
				if (materialForRendering != null)
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
					materialForRendering.SetFloat("_CullMode", 2f);
				}
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
							goto IL_A6;
						}
					}
					else
					{
						materialForRendering = this.m_subTextObjects[i].materialForRendering;
						if (materialForRendering != null)
						{
							materialForRendering.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
						}
						i++;
					}
				}
				IL_A6:;
			}
			else
			{
				Material materialForRendering2 = this.materialForRendering;
				if (materialForRendering2 != null)
				{
					materialForRendering2.SetFloat("_CullMode", 0f);
				}
				int j = 1;
				while (j < this.m_subTextObjects.Length)
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
					if (!(this.m_subTextObjects[j] != null))
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							return;
						}
					}
					else
					{
						materialForRendering2 = this.m_subTextObjects[j].materialForRendering;
						if (materialForRendering2 != null)
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
							materialForRendering2.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetPerspectiveCorrection()).MethodHandle;
				}
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
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_isSDFShader = this.m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return this.m_padding;
		}

		private void SetMeshArrays(int size)
		{
			this.m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			this.m_canvasRenderer.SetMesh(this.m_textInfo.meshInfo[0].mesh);
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
			int fontWeightInternal;
			if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetArraySizes(int[])).MethodHandle;
				}
				fontWeightInternal = 0x2BC;
			}
			else
			{
				fontWeightInternal = this.m_fontWeight;
			}
			this.m_fontWeightInternal = fontWeightInternal;
			this.m_fontWeightStack.SetDefault(this.m_fontWeightInternal);
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(this.m_currentMaterialIndex, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			this.m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			if (this.m_textInfo == null)
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
				this.m_textInfo = new TMP_TextInfo();
			}
			this.m_textElementType = TMP_TextElementType.Character;
			if (this.m_linkedTextComponent != null)
			{
				this.m_linkedTextComponent.text = string.Empty;
				this.m_linkedTextComponent.ForceMeshUpdate();
			}
			int num3 = 0;
			while (num3 < chars.Length && chars[num3] != 0)
			{
				if (this.m_textInfo.characterInfo == null)
				{
					goto IL_161;
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
				if (this.m_totalCharacterCount >= this.m_textInfo.characterInfo.Length)
				{
					goto IL_161;
				}
				IL_17A:
				int num4 = chars[num3];
				if (!this.m_isRichText)
				{
					goto IL_305;
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
				if (num4 != 0x3C)
				{
					goto IL_305;
				}
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!base.ValidateHtmlTag(chars, num3 + 1, out num))
				{
					goto IL_305;
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
				num3 = num;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
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
					this.m_isUsingBold = true;
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
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
				IL_B3C:
				num3++;
				continue;
				IL_305:
				bool flag = false;
				bool isUsingAlternateTypeface = false;
				TMP_FontAsset currentFontAsset = this.m_currentFontAsset;
				Material currentMaterial = this.m_currentMaterial;
				int currentMaterialIndex3 = this.m_currentMaterialIndex;
				if (this.m_textElementType == TMP_TextElementType.Character)
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
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
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
						if (char.IsLower((char)num4))
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
							num4 = (int)char.ToUpper((char)num4);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
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
						if (char.IsUpper((char)num4))
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
							num4 = (int)char.ToLower((char)num4);
						}
					}
					else
					{
						if ((this.m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
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
							if ((this.m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
							{
								goto IL_3E8;
							}
						}
						if (char.IsLower((char)num4))
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
							num4 = (int)char.ToUpper((char)num4);
						}
					}
				}
				IL_3E8:
				TMP_FontAsset tmp_FontAsset = base.GetFontAssetForWeight(this.m_fontWeightInternal);
				if (tmp_FontAsset != null)
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
					flag = true;
					isUsingAlternateTypeface = true;
					this.m_currentFontAsset = tmp_FontAsset;
				}
				TMP_Glyph tmp_Glyph;
				tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
				if (tmp_Glyph == null)
				{
					TMP_SpriteAsset tmp_SpriteAsset = base.spriteAsset;
					if (tmp_SpriteAsset != null)
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
						int num5 = -1;
						tmp_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset, num4, out num5);
						if (num5 != -1)
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
							this.m_textElementType = TMP_TextElementType.Sprite;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset.material, tmp_SpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
							MaterialReference[] materialReferences2 = this.m_materialReferences;
							int currentMaterialIndex4 = this.m_currentMaterialIndex;
							materialReferences2[currentMaterialIndex4].referenceCount = materialReferences2[currentMaterialIndex4].referenceCount + 1;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num5;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
							this.m_textElementType = TMP_TextElementType.Character;
							this.m_currentMaterialIndex = currentMaterialIndex3;
							num2++;
							this.m_totalCharacterCount++;
							goto IL_B3C;
						}
					}
				}
				if (tmp_Glyph == null)
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
					if (TMP_Settings.fallbackFontAssets != null)
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
						if (TMP_Settings.fallbackFontAssets.Count > 0)
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
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num4, out tmp_Glyph);
						}
					}
				}
				if (tmp_Glyph == null)
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
					if (TMP_Settings.defaultFontAsset != null)
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
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num4, out tmp_Glyph);
					}
				}
				if (tmp_Glyph == null)
				{
					TMP_SpriteAsset tmp_SpriteAsset2 = TMP_Settings.defaultSpriteAsset;
					if (tmp_SpriteAsset2 != null)
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
						int num6 = -1;
						tmp_SpriteAsset2 = TMP_SpriteAsset.SearchFallbackForSprite(tmp_SpriteAsset2, num4, out num6);
						if (num6 != -1)
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
							this.m_textElementType = TMP_TextElementType.Sprite;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].elementType = this.m_textElementType;
							this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(tmp_SpriteAsset2.material, tmp_SpriteAsset2, this.m_materialReferences, this.m_materialReferenceIndexLookup);
							MaterialReference[] materialReferences3 = this.m_materialReferences;
							int currentMaterialIndex5 = this.m_currentMaterialIndex;
							materialReferences3[currentMaterialIndex5].referenceCount = materialReferences3[currentMaterialIndex5].referenceCount + 1;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteIndex = num6;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].spriteAsset = tmp_SpriteAsset2;
							this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
							this.m_textElementType = TMP_TextElementType.Character;
							this.m_currentMaterialIndex = currentMaterialIndex3;
							num2++;
							this.m_totalCharacterCount++;
							goto IL_B3C;
						}
					}
				}
				if (tmp_Glyph == null)
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
					num4 = (chars[num3] = ((TMP_Settings.missingGlyphCharacter != 0) ? TMP_Settings.missingGlyphCharacter : 0x25A1));
					tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
					if (tmp_Glyph == null && TMP_Settings.fallbackFontAssets != null)
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
						if (TMP_Settings.fallbackFontAssets.Count > 0)
						{
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num4, out tmp_Glyph);
						}
					}
					if (tmp_Glyph == null)
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
						if (TMP_Settings.defaultFontAsset != null)
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
							tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num4, out tmp_Glyph);
						}
					}
					if (tmp_Glyph == null)
					{
						num4 = (chars[num3] = 0x20);
						tmp_FontAsset = TMP_FontUtilities.SearchForGlyph(this.m_currentFontAsset, num4, out tmp_Glyph);
						if (!TMP_Settings.warningsDisabled)
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
							Debug.LogWarning("Character with ASCII value of " + num4 + " was not found in the Font Asset Glyph Table. It was replaced by a space.", this);
						}
					}
				}
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
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].character = (char)num4;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].fontAsset = this.m_currentFontAsset;
				if (flag)
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
					if (TMP_Settings.matchMaterialPreset)
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
						this.m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(this.m_currentMaterial, this.m_currentFontAsset.material);
					}
					else
					{
						this.m_currentMaterial = this.m_currentFontAsset.material;
					}
					this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, this.m_currentFontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
				}
				if (!char.IsWhiteSpace((char)num4))
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
					if (num4 != 0x200B)
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
						if (this.m_materialReferences[this.m_currentMaterialIndex].referenceCount < 0x3FFF)
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
				}
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].material = this.m_currentMaterial;
				this.m_textInfo.characterInfo[this.m_totalCharacterCount].materialReferenceIndex = this.m_currentMaterialIndex;
				this.m_materialReferences[this.m_currentMaterialIndex].isFallbackMaterial = flag;
				if (flag)
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
					this.m_materialReferences[this.m_currentMaterialIndex].fallbackMaterial = currentMaterial;
					this.m_currentFontAsset = currentFontAsset;
					this.m_currentMaterial = currentMaterial;
					this.m_currentMaterialIndex = currentMaterialIndex3;
				}
				this.m_totalCharacterCount++;
				goto IL_B3C;
				IL_161:
				TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, this.m_totalCharacterCount + 1, true);
				goto IL_17A;
			}
			if (this.m_isCalculatingPreferredValues)
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
				this.m_isCalculatingPreferredValues = false;
				this.m_isInputParsingRequired = true;
				return this.m_totalCharacterCount;
			}
			this.m_textInfo.spriteCount = num2;
			int num7 = this.m_textInfo.materialCount = this.m_materialReferenceIndexLookup.Count;
			if (num7 > this.m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize<TMP_MeshInfo>(ref this.m_textInfo.meshInfo, num7, false);
			}
			if (num7 > this.m_subTextObjects.Length)
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
				TMP_TextInfo.Resize<TMP_SubMeshUI>(ref this.m_subTextObjects, Mathf.NextPowerOfTwo(num7 + 1));
			}
			if (this.m_textInfo.characterInfo.Length - this.m_totalCharacterCount > 0x100)
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
				TMP_TextInfo.Resize<TMP_CharacterInfo>(ref this.m_textInfo.characterInfo, Mathf.Max(this.m_totalCharacterCount + 1, 0x100), true);
			}
			int i = 0;
			while (i < num7)
			{
				if (i > 0)
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
					if (this.m_subTextObjects[i] == null)
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
						this.m_subTextObjects[i] = TMP_SubMeshUI.AddSubTextObject(this, this.m_materialReferences[i]);
						this.m_textInfo.meshInfo[i].vertices = null;
					}
					if (this.m_rectTransform.pivot != this.m_subTextObjects[i].rectTransform.pivot)
					{
						this.m_subTextObjects[i].rectTransform.pivot = this.m_rectTransform.pivot;
					}
					if (this.m_subTextObjects[i].sharedMaterial == null)
					{
						goto IL_D58;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_subTextObjects[i].sharedMaterial.GetInstanceID() != this.m_materialReferences[i].material.GetInstanceID())
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_D58;
						}
					}
					IL_E4A:
					if (this.m_materialReferences[i].isFallbackMaterial)
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
						this.m_subTextObjects[i].fallbackMaterial = this.m_materialReferences[i].material;
						this.m_subTextObjects[i].fallbackSourceMaterial = this.m_materialReferences[i].fallbackMaterial;
						goto IL_EA8;
					}
					goto IL_EA8;
					IL_D58:
					bool isDefaultMaterial = this.m_materialReferences[i].isDefaultMaterial;
					this.m_subTextObjects[i].isDefaultMaterial = isDefaultMaterial;
					if (isDefaultMaterial && !(this.m_subTextObjects[i].sharedMaterial == null))
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
						if (this.m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID() == this.m_materialReferences[i].material.GetTexture(ShaderUtilities.ID_MainTex).GetInstanceID())
						{
							goto IL_E4A;
						}
					}
					this.m_subTextObjects[i].sharedMaterial = this.m_materialReferences[i].material;
					this.m_subTextObjects[i].fontAsset = this.m_materialReferences[i].fontAsset;
					this.m_subTextObjects[i].spriteAsset = this.m_materialReferences[i].spriteAsset;
					goto IL_E4A;
				}
				IL_EA8:
				int referenceCount = this.m_materialReferences[i].referenceCount;
				if (this.m_textInfo.meshInfo[i].vertices == null)
				{
					goto IL_F0B;
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
				if (this.m_textInfo.meshInfo[i].vertices.Length < referenceCount * 4)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_F0B;
					}
				}
				else if (this.m_textInfo.meshInfo[i].vertices.Length - referenceCount * 4 > 0x400)
				{
					TMP_MeshInfo[] meshInfo = this.m_textInfo.meshInfo;
					int num8 = i;
					int size;
					if (referenceCount > 0x400)
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
						size = referenceCount + 0x100;
					}
					else
					{
						size = Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 0x100);
					}
					meshInfo[num8].ResizeMeshInfo(size);
				}
				IL_1039:
				i++;
				continue;
				IL_F0B:
				if (this.m_textInfo.meshInfo[i].vertices == null)
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
					if (i == 0)
					{
						this.m_textInfo.meshInfo[i] = new TMP_MeshInfo(this.m_mesh, referenceCount + 1);
					}
					else
					{
						this.m_textInfo.meshInfo[i] = new TMP_MeshInfo(this.m_subTextObjects[i].mesh, referenceCount + 1);
					}
				}
				else
				{
					TMP_MeshInfo[] meshInfo2 = this.m_textInfo.meshInfo;
					int num9 = i;
					int size2;
					if (referenceCount > 0x400)
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
						size2 = referenceCount + 0x100;
					}
					else
					{
						size2 = Mathf.NextPowerOfTwo(referenceCount);
					}
					meshInfo2[num9].ResizeMeshInfo(size2);
				}
				goto IL_1039;
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
			int j = num7;
			while (j < this.m_subTextObjects.Length)
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
				if (!(this.m_subTextObjects[j] != null))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_10C2;
					}
				}
				else
				{
					if (j < this.m_textInfo.meshInfo.Length)
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
						this.m_subTextObjects[j].canvasRenderer.SetMesh(null);
					}
					j++;
				}
			}
			IL_10C2:
			return this.m_totalCharacterCount;
		}

		protected override void ComputeMarginSize()
		{
			if (base.rectTransform != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.ComputeMarginSize()).MethodHandle;
				}
				this.m_marginWidth = this.m_rectTransform.rect.width - this.m_margin.x - this.m_margin.z;
				this.m_marginHeight = this.m_rectTransform.rect.height - this.m_margin.y - this.m_margin.w;
				this.m_RectTransformCorners = this.GetTextContainerLocalCorners();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			this.m_havePropertiesChanged = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			base.OnCanvasHierarchyChanged();
			this.m_canvas = base.canvas;
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_canvas = base.canvas;
			this.ComputeMarginSize();
			this.m_havePropertiesChanged = true;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			this.ComputeMarginSize();
			this.UpdateSubObjectPivot();
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		private void LateUpdate()
		{
			if (this.m_rectTransform.hasChanged)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.LateUpdate()).MethodHandle;
				}
				float y = this.m_rectTransform.lossyScale.y;
				if (!this.m_havePropertiesChanged && y != this.m_previousLossyScaleY)
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
					if (this.m_text != string.Empty)
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
						if (this.m_text != null)
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
							this.UpdateSDFScale(y);
							this.m_previousLossyScaleY = y;
						}
					}
				}
				this.m_rectTransform.hasChanged = false;
			}
			if (this.m_isUsingLegacyAnimationComponent)
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
				this.m_havePropertiesChanged = true;
				this.OnPreRenderCanvas();
			}
		}

		private void OnPreRenderCanvas()
		{
			if (this.m_isAwake)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.OnPreRenderCanvas()).MethodHandle;
				}
				if (this.m_ignoreActiveState || this.IsActive())
				{
					if (this.m_canvas == null)
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
						this.m_canvas = base.canvas;
						if (this.m_canvas == null)
						{
							return;
						}
					}
					this.loopCountA = 0;
					if (!this.m_havePropertiesChanged)
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
						if (!this.m_isLayoutDirty)
						{
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
					if (this.checkPaddingRequired)
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
						this.UpdateMeshPadding();
					}
					if (!this.m_isInputParsingRequired)
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
						if (!this.m_isTextTruncated)
						{
							goto IL_CB;
						}
					}
					base.ParseInputText();
					IL_CB:
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
		}

		protected override void GenerateTextMesh()
		{
			if (!(this.m_fontAsset == null))
			{
				if (this.m_fontAsset.characterDictionary != null)
				{
					if (this.m_textInfo != null)
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
						this.m_textInfo.Clear();
					}
					if (this.m_char_buffer != null)
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
						if (this.m_char_buffer.Length != 0)
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
							if (this.m_char_buffer[0] == 0)
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
								this.m_fontScale = this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
								float num = this.m_fontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
								float num2 = this.m_fontScale;
								this.m_fontScaleMultiplier = 1f;
								this.m_currentFontSize = this.m_fontSize;
								this.m_sizeStack.SetDefault(this.m_currentFontSize);
								this.m_style = this.m_fontStyle;
								int fontWeightInternal;
								if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
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
								float num3 = 0f;
								float num4 = 1f;
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
								float num5 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
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
								int num6 = Mathf.Clamp(this.m_pageToDisplay - 1, 0, this.m_textInfo.pageInfo.Length - 1);
								int num7 = 0;
								int num8 = 0;
								Vector4 margin = this.m_margin;
								float marginWidth = this.m_marginWidth;
								float marginHeight = this.m_marginHeight;
								this.m_marginLeft = 0f;
								this.m_marginRight = 0f;
								this.m_width = -1f;
								float num9 = marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight;
								this.m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
								this.m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
								this.m_textInfo.ClearLineInfo();
								this.m_maxCapHeight = 0f;
								this.m_maxAscender = 0f;
								this.m_maxDescender = 0f;
								float num10 = 0f;
								float num11 = 0f;
								bool flag5 = false;
								this.m_isNewPage = false;
								bool flag6 = true;
								this.m_isNonBreakingSpace = false;
								bool flag7 = false;
								bool flag8 = false;
								int num12 = 0;
								base.SaveWordWrappingState(ref this.m_SavedWordWrapState, -1, -1);
								base.SaveWordWrappingState(ref this.m_SavedLineState, -1, -1);
								this.loopCountA++;
								int num13 = 0;
								int i = 0;
								while (i < this.m_char_buffer.Length)
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
									if (this.m_char_buffer[i] == 0)
									{
										break;
									}
									int num14 = this.m_char_buffer[i];
									this.m_textElementType = this.m_textInfo.characterInfo[this.m_characterCount].elementType;
									this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
									this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
									int currentMaterialIndex = this.m_currentMaterialIndex;
									if (!this.m_isRichText)
									{
										goto IL_60E;
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num14 != 0x3C)
									{
										goto IL_60E;
									}
									this.m_isParsingText = true;
									this.m_textElementType = TMP_TextElementType.Character;
									if (!base.ValidateHtmlTag(this.m_char_buffer, i + 1, out num13))
									{
										goto IL_60E;
									}
									i = num13;
									if (this.m_textElementType != TMP_TextElementType.Character)
									{
										goto IL_60E;
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
									IL_36E5:
									i++;
									continue;
									IL_60E:
									this.m_isParsingText = false;
									bool isUsingAlternateTypeface = this.m_textInfo.characterInfo[this.m_characterCount].isUsingAlternateTypeface;
									if (this.m_characterCount < this.m_firstVisibleCharacter)
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
										this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
										this.m_textInfo.characterInfo[this.m_characterCount].character = '​';
										this.m_characterCount++;
										goto IL_36E5;
									}
									float num15 = 1f;
									if (this.m_textElementType == TMP_TextElementType.Character)
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
										if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
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
											if (char.IsLower((char)num14))
											{
												num14 = (int)char.ToUpper((char)num14);
											}
										}
										else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
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
											if (char.IsUpper((char)num14))
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
												num14 = (int)char.ToLower((char)num14);
											}
										}
										else
										{
											if ((this.m_fontStyle & FontStyles.SmallCaps) != FontStyles.SmallCaps)
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
												if ((this.m_style & FontStyles.SmallCaps) != FontStyles.SmallCaps)
												{
													goto IL_76D;
												}
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
											}
											if (char.IsLower((char)num14))
											{
												num15 = 0.8f;
												num14 = (int)char.ToUpper((char)num14);
											}
										}
									}
									IL_76D:
									if (this.m_textElementType == TMP_TextElementType.Sprite)
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
										this.m_currentSpriteAsset = this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset;
										this.m_spriteIndex = this.m_textInfo.characterInfo[this.m_characterCount].spriteIndex;
										TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
										if (tmp_Sprite == null)
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
											goto IL_36E5;
										}
										if (num14 == 0x3C)
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
											num14 = 0xE000 + this.m_spriteIndex;
										}
										else
										{
											this.m_spriteColor = TMP_Text.s_colorWhite;
										}
										this.m_currentFontAsset = this.m_fontAsset;
										float num16 = this.m_currentFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale;
										num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num16;
										this.m_cached_TextElement = tmp_Sprite;
										this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
										this.m_textInfo.characterInfo[this.m_characterCount].scale = num16;
										this.m_textInfo.characterInfo[this.m_characterCount].spriteAsset = this.m_currentSpriteAsset;
										this.m_textInfo.characterInfo[this.m_characterCount].fontAsset = this.m_currentFontAsset;
										this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex = this.m_currentMaterialIndex;
										this.m_currentMaterialIndex = currentMaterialIndex;
										num3 = 0f;
									}
									else if (this.m_textElementType == TMP_TextElementType.Character)
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
										this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
										if (this.m_cached_TextElement == null)
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
											goto IL_36E5;
										}
										this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
										this.m_currentMaterial = this.m_textInfo.characterInfo[this.m_characterCount].material;
										this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
										this.m_fontScale = this.m_currentFontSize * num15 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale;
										num2 = this.m_fontScale * this.m_fontScaleMultiplier * this.m_cached_TextElement.scale;
										this.m_textInfo.characterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
										this.m_textInfo.characterInfo[this.m_characterCount].scale = num2;
										float padding;
										if (this.m_currentMaterialIndex == 0)
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
											padding = this.m_padding;
										}
										else
										{
											padding = this.m_subTextObjects[this.m_currentMaterialIndex].padding;
										}
										num3 = padding;
									}
									float num17 = num2;
									if (num14 == 0xAD)
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
										num2 = 0f;
									}
									if (this.m_isRightToLeft)
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
										this.m_xAdvance -= ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_wordSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num14))
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
											if (num14 != 0x200B)
											{
												goto IL_B52;
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
										this.m_xAdvance -= this.m_wordSpacing * num2;
									}
									IL_B52:
									this.m_textInfo.characterInfo[this.m_characterCount].character = (char)num14;
									this.m_textInfo.characterInfo[this.m_characterCount].pointSize = this.m_currentFontSize;
									this.m_textInfo.characterInfo[this.m_characterCount].color = this.m_htmlColor;
									this.m_textInfo.characterInfo[this.m_characterCount].underlineColor = this.m_underlineColor;
									this.m_textInfo.characterInfo[this.m_characterCount].strikethroughColor = this.m_strikethroughColor;
									this.m_textInfo.characterInfo[this.m_characterCount].highlightColor = this.m_highlightColor;
									this.m_textInfo.characterInfo[this.m_characterCount].style = this.m_style;
									this.m_textInfo.characterInfo[this.m_characterCount].index = (short)i;
									if (this.m_enableKerning)
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
										if (this.m_characterCount >= 1)
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
											int character = (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].character;
											KerningPairKey kerningPairKey = new KerningPairKey(character, num14);
											KerningPair kerningPair;
											this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
											if (kerningPair != null)
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
												this.m_xAdvance += kerningPair.XadvanceOffset * num2;
											}
										}
									}
									float num18 = 0f;
									if (this.m_monoSpacing != 0f)
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
										num18 = (this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2) * (1f - this.m_charWidthAdjDelta);
										this.m_xAdvance += num18;
									}
									if (this.m_textElementType != TMP_TextElementType.Character)
									{
										goto IL_E33;
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
									if (isUsingAlternateTypeface)
									{
										goto IL_E33;
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
									if ((this.m_style & FontStyles.Bold) != FontStyles.Bold)
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
										if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
										{
											goto IL_E33;
										}
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									float num19;
									if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
									{
										float @float = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
										num19 = this.m_currentFontAsset.boldStyle / 4f * @float * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
										if (num19 + num3 > @float)
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
											num3 = @float - num19;
										}
									}
									else
									{
										num19 = 0f;
									}
									num4 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
									IL_EAC:
									float baseline = this.m_currentFontAsset.fontInfo.Baseline;
									Vector3 vector2;
									vector2.x = this.m_xAdvance + (this.m_cached_TextElement.xOffset - num3 - num19) * num2 * (1f - this.m_charWidthAdjDelta);
									vector2.y = (baseline + this.m_cached_TextElement.yOffset + num3) * num2 - this.m_lineOffset + this.m_baselineOffset;
									vector2.z = 0f;
									Vector3 vector3;
									vector3.x = vector2.x;
									vector3.y = vector2.y - (this.m_cached_TextElement.height + num3 * 2f) * num2;
									vector3.z = 0f;
									Vector3 vector4;
									vector4.x = vector3.x + (this.m_cached_TextElement.width + num3 * 2f + num19 * 2f) * num2 * (1f - this.m_charWidthAdjDelta);
									vector4.y = vector2.y;
									vector4.z = 0f;
									Vector3 vector5;
									vector5.x = vector4.x;
									vector5.y = vector3.y;
									vector5.z = 0f;
									if (this.m_textElementType == TMP_TextElementType.Character)
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
										if (!isUsingAlternateTypeface)
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
											if ((this.m_style & FontStyles.Italic) != FontStyles.Italic)
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
												if ((this.m_fontStyle & FontStyles.Italic) != FontStyles.Italic)
												{
													goto IL_10D3;
												}
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
											}
											float num20 = (float)this.m_currentFontAsset.italicStyle * 0.01f;
											Vector3 b = new Vector3(num20 * ((this.m_cached_TextElement.yOffset + num3 + num19) * num2), 0f, 0f);
											Vector3 b2 = new Vector3(num20 * ((this.m_cached_TextElement.yOffset - this.m_cached_TextElement.height - num3 - num19) * num2), 0f, 0f);
											vector2 += b;
											vector3 += b2;
											vector4 += b;
											vector5 += b2;
										}
									}
									IL_10D3:
									if (this.m_isFXMatrixSet)
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
									float ascender = this.m_currentFontAsset.fontInfo.Ascender;
									float num21;
									if (this.m_textElementType == TMP_TextElementType.Character)
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
										num21 = num2;
									}
									else
									{
										num21 = this.m_textInfo.characterInfo[this.m_characterCount].scale;
									}
									float num22 = ascender * num21 + this.m_baselineOffset;
									this.m_textInfo.characterInfo[this.m_characterCount].ascender = num22 - this.m_lineOffset;
									float maxLineAscender;
									if (num22 > this.m_maxLineAscender)
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
										maxLineAscender = num22;
									}
									else
									{
										maxLineAscender = this.m_maxLineAscender;
									}
									this.m_maxLineAscender = maxLineAscender;
									float descender = this.m_currentFontAsset.fontInfo.Descender;
									float num23;
									if (this.m_textElementType == TMP_TextElementType.Character)
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
										num23 = num2;
									}
									else
									{
										num23 = this.m_textInfo.characterInfo[this.m_characterCount].scale;
									}
									float num24 = descender * num23 + this.m_baselineOffset;
									float num25 = this.m_textInfo.characterInfo[this.m_characterCount].descender = num24 - this.m_lineOffset;
									float maxLineDescender;
									if (num24 < this.m_maxLineDescender)
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
										maxLineDescender = num24;
									}
									else
									{
										maxLineDescender = this.m_maxLineDescender;
									}
									this.m_maxLineDescender = maxLineDescender;
									if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
									{
										goto IL_13E2;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
									{
										goto IL_13E2;
									}
									IL_1474:
									if (this.m_lineNumber == 0)
									{
										goto IL_148E;
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.m_isNewPage)
									{
										goto IL_148E;
									}
									IL_14D5:
									if (this.m_lineOffset == 0f)
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
										num10 = ((num10 <= num22) ? num22 : num10);
									}
									this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
									if (num14 != 9)
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
										if (!char.IsWhiteSpace((char)num14))
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
											if (num14 != 0x200B)
											{
												goto IL_155B;
											}
										}
										if (this.m_textElementType == TMP_TextElementType.Sprite)
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
										}
										else
										{
											if (num14 != 0xA)
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
												if (!char.IsSeparator((char)num14))
												{
													goto IL_226A;
												}
											}
											if (num14 == 0xAD)
											{
												goto IL_226A;
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
											if (num14 == 0x200B)
											{
												goto IL_226A;
											}
											for (;;)
											{
												switch (6)
												{
												case 0:
													continue;
												}
												break;
											}
											if (num14 != 0x2060)
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
												TMP_LineInfo[] lineInfo = this.m_textInfo.lineInfo;
												int lineNumber = this.m_lineNumber;
												lineInfo[lineNumber].spaceCount = lineInfo[lineNumber].spaceCount + 1;
												this.m_textInfo.spaceCount++;
												goto IL_226A;
											}
											goto IL_226A;
										}
									}
									IL_155B:
									this.m_textInfo.characterInfo[this.m_characterCount].isVisible = true;
									num9 = ((this.m_width == -1f) ? (marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(marginWidth + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width));
									this.m_textInfo.lineInfo[this.m_lineNumber].marginLeft = this.m_marginLeft;
									bool flag9;
									if ((this.m_lineJustification & (TextAlignmentOptions)0x10) != (TextAlignmentOptions)0x10)
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
										flag9 = ((this.m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8);
									}
									else
									{
										flag9 = true;
									}
									bool flag10 = flag9;
									float num26 = Mathf.Abs(this.m_xAdvance);
									float num27 = (this.m_isRightToLeft ? 0f : this.m_cached_TextElement.xAdvance) * (1f - this.m_charWidthAdjDelta);
									float num28;
									if (num14 != 0xAD)
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
										num28 = num2;
									}
									else
									{
										num28 = num17;
									}
									if (num26 + num27 * num28 > num9 * ((!flag10) ? 1f : 1.05f))
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
										num8 = this.m_characterCount - 1;
										if (base.enableWordWrapping)
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
											if (this.m_characterCount != this.m_firstCharacterOfLine)
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
												if (num12 == this.m_SavedWordWrapState.previous_WordBreak)
												{
													goto IL_16DB;
												}
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													break;
												}
												if (flag6)
												{
													for (;;)
													{
														switch (2)
														{
														case 0:
															continue;
														}
														goto IL_16DB;
													}
												}
												IL_17F1:
												i = base.RestoreWordWrappingState(ref this.m_SavedWordWrapState);
												num12 = i;
												if (this.m_char_buffer[i] == 0xAD)
												{
													this.m_isTextTruncated = true;
													this.m_char_buffer[i] = 0x2D;
													this.GenerateTextMesh();
													return;
												}
												if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender))
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
													if (this.m_lineHeight == -32767f)
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
														if (!this.m_isNewPage)
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
															float num29 = this.m_maxLineAscender - this.m_startOfLineAscender;
															this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num29);
															this.m_lineOffset += num29;
															this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
															this.m_SavedWordWrapState.previousLineAscender = this.m_maxLineAscender;
														}
													}
												}
												this.m_isNewPage = false;
												float num30 = this.m_maxLineAscender - this.m_lineOffset;
												float num31 = this.m_maxLineDescender - this.m_lineOffset;
												float maxDescender;
												if (this.m_maxDescender < num31)
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
													maxDescender = this.m_maxDescender;
												}
												else
												{
													maxDescender = num31;
												}
												this.m_maxDescender = maxDescender;
												if (!flag5)
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
													num11 = this.m_maxDescender;
												}
												if (this.m_useMaxVisibleDescender)
												{
													if (this.m_characterCount < this.m_maxVisibleCharacters)
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
														if (this.m_lineNumber < this.m_maxVisibleLines)
														{
															goto IL_1967;
														}
													}
													flag5 = true;
												}
												IL_1967:
												this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
												TMP_LineInfo[] lineInfo2 = this.m_textInfo.lineInfo;
												int lineNumber2 = this.m_lineNumber;
												int firstVisibleCharacterOfLine;
												if (this.m_firstCharacterOfLine > this.m_firstVisibleCharacterOfLine)
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
													firstVisibleCharacterOfLine = this.m_firstCharacterOfLine;
												}
												else
												{
													firstVisibleCharacterOfLine = this.m_firstVisibleCharacterOfLine;
												}
												lineInfo2[lineNumber2].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = firstVisibleCharacterOfLine);
												this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = ((this.m_characterCount - 1 <= 0) ? 0 : (this.m_characterCount - 1)));
												this.m_textInfo.lineInfo[this.m_lineNumber].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = ((this.m_lastVisibleCharacterOfLine >= this.m_firstVisibleCharacterOfLine) ? this.m_lastVisibleCharacterOfLine : this.m_firstVisibleCharacterOfLine));
												this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
												this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
												this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num31);
												this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num30);
												this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x;
												this.m_textInfo.lineInfo[this.m_lineNumber].width = num9;
												this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
												this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
												this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num30;
												this.m_textInfo.lineInfo[this.m_lineNumber].descender = num31;
												this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num30 - num31 + num5 * num;
												this.m_firstCharacterOfLine = this.m_characterCount;
												this.m_lineVisibleCharacterCount = 0;
												base.SaveWordWrappingState(ref this.m_SavedLineState, i, this.m_characterCount - 1);
												this.m_lineNumber++;
												flag4 = true;
												if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
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
													base.ResizeLineExtents(this.m_lineNumber);
												}
												if (this.m_lineHeight == -32767f)
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
													float num32 = this.m_textInfo.characterInfo[this.m_characterCount].ascender - this.m_textInfo.characterInfo[this.m_characterCount].baseLine;
													float num33 = 0f - this.m_maxLineDescender + num32 + (num5 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
													this.m_lineOffset += num33;
													this.m_startOfLineAscender = num32;
												}
												else
												{
													this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
												}
												this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
												this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
												this.m_xAdvance = this.tag_Indent;
												goto IL_36E5;
												IL_16DB:
												if (this.m_enableAutoSizing)
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
													if (this.m_fontSize > this.m_fontSizeMin)
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
														if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
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
														this.GenerateTextMesh();
														return;
													}
												}
												if (!this.m_isCharacterWrappingEnabled)
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
													if (!flag7)
													{
														flag7 = true;
													}
													else
													{
														this.m_isCharacterWrappingEnabled = true;
													}
													goto IL_17F1;
												}
												flag8 = true;
												goto IL_17F1;
											}
										}
										if (this.m_enableAutoSizing)
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
											if (this.m_fontSize > this.m_fontSizeMin)
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
												if (this.m_charWidthAdjDelta < this.m_charWidthMaxAdj / 100f)
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
												this.GenerateTextMesh();
												return;
											}
										}
										switch (this.m_overflowMode)
										{
										case TextOverflowModes.Overflow:
											if (this.m_isMaskingEnabled)
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
												this.DisableMasking();
											}
											break;
										case TextOverflowModes.Ellipsis:
											if (this.m_isMaskingEnabled)
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
												this.DisableMasking();
											}
											this.m_isTextTruncated = true;
											if (this.m_characterCount >= 1)
											{
												this.m_char_buffer[i - 1] = 0x2026;
												this.m_char_buffer[i] = 0;
												if (this.m_cached_Ellipsis_GlyphInfo != null)
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
													this.m_textInfo.characterInfo[num8].character = '…';
													this.m_textInfo.characterInfo[num8].textElement = this.m_cached_Ellipsis_GlyphInfo;
													this.m_textInfo.characterInfo[num8].fontAsset = this.m_materialReferences[0].fontAsset;
													this.m_textInfo.characterInfo[num8].material = this.m_materialReferences[0].material;
													this.m_textInfo.characterInfo[num8].materialReferenceIndex = 0;
												}
												else
												{
													Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
												}
												this.m_totalCharacterCount = num8 + 1;
												this.GenerateTextMesh();
												return;
											}
											for (;;)
											{
												switch (6)
												{
												case 0:
													continue;
												}
												break;
											}
											this.m_textInfo.characterInfo[this.m_characterCount].isVisible = false;
											break;
										case TextOverflowModes.Masking:
											if (!this.m_isMaskingEnabled)
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
												this.EnableMasking();
											}
											break;
										case TextOverflowModes.Truncate:
											if (this.m_isMaskingEnabled)
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
									if (num14 != 9)
									{
										Color32 vertexColor;
										if (this.m_overrideHtmlColors)
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
											vertexColor = this.m_fontColor32;
										}
										else
										{
											vertexColor = this.m_htmlColor;
										}
										if (this.m_textElementType == TMP_TextElementType.Character)
										{
											this.SaveGlyphVertexInfo(num3, num19, vertexColor);
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
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (num14 != 0xAD)
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
											if (flag4)
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
												flag4 = false;
												this.m_firstVisibleCharacterOfLine = this.m_characterCount;
											}
											this.m_lineVisibleCharacterCount++;
											this.m_lastVisibleCharacterOfLine = this.m_characterCount;
										}
									}
									IL_226A:
									if (this.m_lineNumber > 0)
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
										if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
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
											if (!this.m_isNewPage)
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
												float num34 = this.m_maxLineAscender - this.m_startOfLineAscender;
												this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num34);
												num25 -= num34;
												this.m_lineOffset += num34;
												this.m_startOfLineAscender += num34;
												this.m_SavedWordWrapState.lineOffset = this.m_lineOffset;
												this.m_SavedWordWrapState.previousLineAscender = this.m_startOfLineAscender;
											}
										}
									}
									this.m_textInfo.characterInfo[this.m_characterCount].lineNumber = (short)this.m_lineNumber;
									this.m_textInfo.characterInfo[this.m_characterCount].pageNumber = (short)this.m_pageNumber;
									if (num14 == 0xA)
									{
										goto IL_23A5;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num14 == 0xD)
									{
										goto IL_23A5;
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
									if (num14 != 0x2026)
									{
										goto IL_23CD;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										goto IL_23A5;
									}
									IL_23EE:
									if (this.m_maxAscender - num25 > marginHeight + 0.0001f)
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
										if (this.m_enableAutoSizing && this.m_lineSpacingDelta > this.m_lineSpacingMax)
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
											for (;;)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												break;
											}
											if (this.m_fontSize > this.m_fontSizeMin)
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
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
											this.m_firstOverflowCharacterIndex = this.m_characterCount;
										}
										switch (this.m_overflowMode)
										{
										case TextOverflowModes.Overflow:
											if (this.m_isMaskingEnabled)
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
												this.DisableMasking();
											}
											break;
										case TextOverflowModes.Ellipsis:
											if (this.m_isMaskingEnabled)
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
												this.DisableMasking();
											}
											if (this.m_lineNumber > 0)
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
												this.m_char_buffer[(int)this.m_textInfo.characterInfo[num8].index] = 0x2026;
												this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num8].index + 1)] = 0;
												if (this.m_cached_Ellipsis_GlyphInfo != null)
												{
													this.m_textInfo.characterInfo[num8].character = '…';
													this.m_textInfo.characterInfo[num8].textElement = this.m_cached_Ellipsis_GlyphInfo;
													this.m_textInfo.characterInfo[num8].fontAsset = this.m_materialReferences[0].fontAsset;
													this.m_textInfo.characterInfo[num8].material = this.m_materialReferences[0].material;
													this.m_textInfo.characterInfo[num8].materialReferenceIndex = 0;
												}
												else
												{
													Debug.LogWarning("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + this.m_fontAsset.name + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file.", this);
												}
												this.m_totalCharacterCount = num8 + 1;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh();
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
												for (;;)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												this.DisableMasking();
											}
											if (this.m_lineNumber > 0)
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
												this.m_char_buffer[(int)(this.m_textInfo.characterInfo[num8].index + 1)] = 0;
												this.m_totalCharacterCount = num8 + 1;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh();
											return;
										case TextOverflowModes.ScrollRect:
											if (!this.m_isMaskingEnabled)
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
												this.EnableMasking();
											}
											break;
										case TextOverflowModes.Page:
											if (this.m_isMaskingEnabled)
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
												this.DisableMasking();
											}
											if (num14 != 0xD)
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
												if (num14 == 0xA)
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
												}
												else
												{
													if (i == 0)
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
														this.ClearMesh();
														return;
													}
													if (num7 == i)
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
														this.m_char_buffer[i] = 0;
														this.m_isTextTruncated = true;
													}
													num7 = i;
													i = base.RestoreWordWrappingState(ref this.m_SavedLineState);
													this.m_isNewPage = true;
													this.m_xAdvance = this.tag_Indent;
													this.m_lineOffset = 0f;
													this.m_maxAscender = 0f;
													num10 = 0f;
													this.m_lineNumber++;
													this.m_pageNumber++;
													goto IL_36E5;
												}
											}
											break;
										case TextOverflowModes.Linked:
											if (this.m_linkedTextComponent != null)
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
												this.m_linkedTextComponent.text = base.text;
												this.m_linkedTextComponent.firstVisibleCharacter = this.m_characterCount;
												this.m_linkedTextComponent.ForceMeshUpdate();
											}
											if (this.m_lineNumber > 0)
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
												this.m_char_buffer[i] = 0;
												this.m_totalCharacterCount = this.m_characterCount;
												this.GenerateTextMesh();
												this.m_isTextTruncated = true;
												return;
											}
											this.ClearMesh();
											return;
										}
									}
									if (num14 == 9)
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
										float num35 = this.m_currentFontAsset.fontInfo.TabWidth * num2;
										float num36 = Mathf.Ceil(this.m_xAdvance / num35) * num35;
										float xAdvance;
										if (num36 > this.m_xAdvance)
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
											xAdvance = num36;
										}
										else
										{
											xAdvance = this.m_xAdvance + num35;
										}
										this.m_xAdvance = xAdvance;
									}
									else if (this.m_monoSpacing != 0f)
									{
										this.m_xAdvance += (this.m_monoSpacing - num18 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (char.IsWhiteSpace((char)num14))
										{
											goto IL_2997;
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
										if (num14 == 0x200B)
										{
											goto IL_2997;
										}
										goto IL_2A3E;
										IL_2997:
										this.m_xAdvance += this.m_wordSpacing * num2;
									}
									else if (!this.m_isRightToLeft)
									{
										this.m_xAdvance += ((this.m_cached_TextElement.xAdvance * num4 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing) * (1f - this.m_charWidthAdjDelta);
										if (!char.IsWhiteSpace((char)num14))
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
											if (num14 != 0x200B)
											{
												goto IL_2A3E;
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
										this.m_xAdvance += this.m_wordSpacing * num2;
									}
									IL_2A3E:
									this.m_textInfo.characterInfo[this.m_characterCount].xAdvance = this.m_xAdvance;
									if (num14 == 0xD)
									{
										this.m_xAdvance = this.tag_Indent;
									}
									if (num14 != 0xA)
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
										if (this.m_characterCount != totalCharacterCount - 1)
										{
											goto IL_30E2;
										}
									}
									if (this.m_lineNumber > 0)
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
										if (!TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == -32767f)
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
											if (!this.m_isNewPage)
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
												float num37 = this.m_maxLineAscender - this.m_startOfLineAscender;
												this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num37);
												num25 -= num37;
												this.m_lineOffset += num37;
											}
										}
									}
									this.m_isNewPage = false;
									float num38 = this.m_maxLineAscender - this.m_lineOffset;
									float num39 = this.m_maxLineDescender - this.m_lineOffset;
									float maxDescender2;
									if (this.m_maxDescender < num39)
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
										maxDescender2 = this.m_maxDescender;
									}
									else
									{
										maxDescender2 = num39;
									}
									this.m_maxDescender = maxDescender2;
									if (!flag5)
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
										num11 = this.m_maxDescender;
									}
									if (this.m_useMaxVisibleDescender)
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
										if (this.m_characterCount >= this.m_maxVisibleCharacters || this.m_lineNumber >= this.m_maxVisibleLines)
										{
											flag5 = true;
										}
									}
									this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex = this.m_firstCharacterOfLine;
									TMP_LineInfo[] lineInfo4 = this.m_textInfo.lineInfo;
									int lineNumber4 = this.m_lineNumber;
									int firstVisibleCharacterOfLine2;
									if (this.m_firstCharacterOfLine > this.m_firstVisibleCharacterOfLine)
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
										firstVisibleCharacterOfLine2 = this.m_firstCharacterOfLine;
									}
									else
									{
										firstVisibleCharacterOfLine2 = this.m_firstVisibleCharacterOfLine;
									}
									lineInfo4[lineNumber4].firstVisibleCharacterIndex = (this.m_firstVisibleCharacterOfLine = firstVisibleCharacterOfLine2);
									this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex = (this.m_lastCharacterOfLine = this.m_characterCount);
									TMP_LineInfo[] lineInfo5 = this.m_textInfo.lineInfo;
									int lineNumber5 = this.m_lineNumber;
									int lastVisibleCharacterOfLine;
									if (this.m_lastVisibleCharacterOfLine < this.m_firstVisibleCharacterOfLine)
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
										lastVisibleCharacterOfLine = this.m_firstVisibleCharacterOfLine;
									}
									else
									{
										lastVisibleCharacterOfLine = this.m_lastVisibleCharacterOfLine;
									}
									lineInfo5[lineNumber5].lastVisibleCharacterIndex = (this.m_lastVisibleCharacterOfLine = lastVisibleCharacterOfLine);
									this.m_textInfo.lineInfo[this.m_lineNumber].characterCount = this.m_textInfo.lineInfo[this.m_lineNumber].lastCharacterIndex - this.m_textInfo.lineInfo[this.m_lineNumber].firstCharacterIndex + 1;
									this.m_textInfo.lineInfo[this.m_lineNumber].visibleCharacterCount = this.m_lineVisibleCharacterCount;
									this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_firstVisibleCharacterOfLine].bottomLeft.x, num39);
									this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].topRight.x, num38);
									this.m_textInfo.lineInfo[this.m_lineNumber].length = this.m_textInfo.lineInfo[this.m_lineNumber].lineExtents.max.x - num3 * num2;
									this.m_textInfo.lineInfo[this.m_lineNumber].width = num9;
									if (this.m_textInfo.lineInfo[this.m_lineNumber].characterCount == 1)
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
										this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
									}
									if (this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].isVisible)
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
										this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastVisibleCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
									}
									else
									{
										this.m_textInfo.lineInfo[this.m_lineNumber].maxAdvance = this.m_textInfo.characterInfo[this.m_lastCharacterOfLine].xAdvance - (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 - this.m_cSpacing;
									}
									this.m_textInfo.lineInfo[this.m_lineNumber].baseline = 0f - this.m_lineOffset;
									this.m_textInfo.lineInfo[this.m_lineNumber].ascender = num38;
									this.m_textInfo.lineInfo[this.m_lineNumber].descender = num39;
									this.m_textInfo.lineInfo[this.m_lineNumber].lineHeight = num38 - num39 + num5 * num;
									this.m_firstCharacterOfLine = this.m_characterCount + 1;
									this.m_lineVisibleCharacterCount = 0;
									if (num14 == 0xA)
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
										base.SaveWordWrappingState(ref this.m_SavedLineState, i, this.m_characterCount);
										base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
										this.m_lineNumber++;
										flag4 = true;
										if (this.m_lineNumber >= this.m_textInfo.lineInfo.Length)
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
											base.ResizeLineExtents(this.m_lineNumber);
										}
										if (this.m_lineHeight == -32767f)
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
											float num33 = 0f - this.m_maxLineDescender + num22 + (num5 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
											this.m_lineOffset += num33;
										}
										else
										{
											this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
										}
										this.m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
										this.m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
										this.m_startOfLineAscender = num22;
										this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
										num8 = this.m_characterCount - 1;
										this.m_characterCount++;
										goto IL_36E5;
									}
									IL_30E2:
									if (this.m_textInfo.characterInfo[this.m_characterCount].isVisible)
									{
										this.m_meshExtents.min.x = Mathf.Min(this.m_meshExtents.min.x, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.x);
										this.m_meshExtents.min.y = Mathf.Min(this.m_meshExtents.min.y, this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft.y);
										this.m_meshExtents.max.x = Mathf.Max(this.m_meshExtents.max.x, this.m_textInfo.characterInfo[this.m_characterCount].topRight.x);
										this.m_meshExtents.max.y = Mathf.Max(this.m_meshExtents.max.y, this.m_textInfo.characterInfo[this.m_characterCount].topRight.y);
									}
									if (this.m_overflowMode == TextOverflowModes.Page)
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
										if (num14 != 0xD)
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
											if (num14 != 0xA)
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
												if (this.m_pageNumber + 1 > this.m_textInfo.pageInfo.Length)
												{
													TMP_TextInfo.Resize<TMP_PageInfo>(ref this.m_textInfo.pageInfo, this.m_pageNumber + 1, true);
												}
												this.m_textInfo.pageInfo[this.m_pageNumber].ascender = num10;
												TMP_PageInfo[] pageInfo = this.m_textInfo.pageInfo;
												int pageNumber = this.m_pageNumber;
												float descender2;
												if (num24 < this.m_textInfo.pageInfo[this.m_pageNumber].descender)
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
													descender2 = num24;
												}
												else
												{
													descender2 = this.m_textInfo.pageInfo[this.m_pageNumber].descender;
												}
												pageInfo[pageNumber].descender = descender2;
												if (this.m_pageNumber == 0 && this.m_characterCount == 0)
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
													this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
												}
												else
												{
													if (this.m_characterCount > 0)
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
														if (this.m_pageNumber != (int)this.m_textInfo.characterInfo[this.m_characterCount - 1].pageNumber)
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
															this.m_textInfo.pageInfo[this.m_pageNumber - 1].lastCharacterIndex = this.m_characterCount - 1;
															this.m_textInfo.pageInfo[this.m_pageNumber].firstCharacterIndex = this.m_characterCount;
															goto IL_340D;
														}
													}
													if (this.m_characterCount == totalCharacterCount - 1)
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
														this.m_textInfo.pageInfo[this.m_pageNumber].lastCharacterIndex = this.m_characterCount;
													}
												}
											}
										}
									}
									IL_340D:
									if (this.m_enableWordWrapping)
									{
										goto IL_343E;
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
									if (this.m_overflowMode == TextOverflowModes.Truncate)
									{
										goto IL_343E;
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
									if (this.m_overflowMode == TextOverflowModes.Ellipsis)
									{
										goto IL_343E;
									}
									IL_36D7:
									this.m_characterCount++;
									goto IL_36E5;
									IL_343E:
									if (char.IsWhiteSpace((char)num14))
									{
										goto IL_348B;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num14 == 0x200B)
									{
										goto IL_348B;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num14 == 0x2D)
									{
										goto IL_348B;
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
									if (num14 == 0xAD)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_348B;
										}
									}
									IL_3506:
									if (num14 <= 0x1100)
									{
										goto IL_352F;
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
									if (num14 < 0x11FF)
									{
										goto IL_35F7;
									}
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										goto IL_352F;
									}
									IL_369F:
									if (!flag6)
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
										if (!this.m_isCharacterWrappingEnabled)
										{
											if (!flag8)
											{
												goto IL_36D7;
											}
											for (;;)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
										}
									}
									base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
									goto IL_36D7;
									IL_35F7:
									if (!this.m_isNonBreakingSpace)
									{
										if (flag6)
										{
											goto IL_367F;
										}
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										if (flag8)
										{
											goto IL_367F;
										}
										if (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num14))
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
											if (this.m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey((int)this.m_textInfo.characterInfo[this.m_characterCount + 1].character))
											{
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													goto IL_367F;
												}
											}
										}
										IL_369D:
										goto IL_36D7;
										IL_367F:
										base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag6 = false;
										goto IL_369D;
									}
									goto IL_369F;
									IL_352F:
									if (num14 > 0x2E80)
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
										if (num14 < 0x9FFF)
										{
											goto IL_35F7;
										}
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (num14 > 0xA960)
									{
										if (num14 < 0xA97F)
										{
											goto IL_35F7;
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
									if (num14 > 0xAC00)
									{
										if (num14 < 0xD7FF)
										{
											goto IL_35F7;
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
									if (num14 > 0xF900)
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
										if (num14 < 0xFAFF)
										{
											goto IL_35F7;
										}
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (num14 > 0xFE30)
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
										if (num14 < 0xFE4F)
										{
											goto IL_35F7;
										}
									}
									if (num14 <= 0xFF00 || num14 >= 0xFFEF)
									{
										goto IL_369F;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										goto IL_35F7;
									}
									IL_348B:
									if (this.m_isNonBreakingSpace)
									{
										if (!flag7)
										{
											goto IL_3506;
										}
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									if (num14 == 0xA0)
									{
										goto IL_3506;
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
									if (num14 == 0x2011)
									{
										goto IL_3506;
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
									if (num14 != 0x202F && num14 != 0x2060)
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
										base.SaveWordWrappingState(ref this.m_SavedWordWrapState, i, this.m_characterCount);
										this.m_isCharacterWrappingEnabled = false;
										flag6 = false;
										goto IL_36D7;
									}
									goto IL_3506;
									IL_23CD:
									this.m_textInfo.lineInfo[this.m_lineNumber].alignment = this.m_lineJustification;
									goto IL_23EE;
									IL_23A5:
									if (this.m_textInfo.lineInfo[this.m_lineNumber].characterCount != 1)
									{
										goto IL_23EE;
									}
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										goto IL_23CD;
									}
									IL_148E:
									float maxAscender;
									if (this.m_maxAscender > num22)
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
										maxAscender = this.m_maxAscender;
									}
									else
									{
										maxAscender = num22;
									}
									this.m_maxAscender = maxAscender;
									this.m_maxCapHeight = Mathf.Max(this.m_maxCapHeight, this.m_currentFontAsset.fontInfo.CapHeight * num2);
									goto IL_14D5;
									IL_13E2:
									float num40 = (num22 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num22 = this.m_maxLineAscender;
									float maxLineAscender2;
									if (num40 > this.m_maxLineAscender)
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
										maxLineAscender2 = num40;
									}
									else
									{
										maxLineAscender2 = this.m_maxLineAscender;
									}
									this.m_maxLineAscender = maxLineAscender2;
									float num41 = (num24 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
									num24 = this.m_maxLineDescender;
									float maxLineDescender2;
									if (num41 < this.m_maxLineDescender)
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
										maxLineDescender2 = num41;
									}
									else
									{
										maxLineDescender2 = this.m_maxLineDescender;
									}
									this.m_maxLineDescender = maxLineDescender2;
									goto IL_1474;
									IL_E33:
									if (this.m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
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
										float float2 = this.m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
										num19 = this.m_currentFontAsset.normalStyle / 4f * float2 * this.m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
										if (num19 + num3 > float2)
										{
											num3 = float2 - num19;
										}
									}
									else
									{
										num19 = 0f;
									}
									num4 = 1f;
									goto IL_EAC;
								}
								float num42 = this.m_maxFontSize - this.m_minFontSize;
								if (!this.m_isCharacterWrappingEnabled)
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
									if (this.m_enableAutoSizing)
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
										if (num42 > 0.051f && this.m_fontSize < this.m_fontSizeMax)
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
											this.m_minFontSize = this.m_fontSize;
											this.m_fontSize += Mathf.Max((this.m_maxFontSize - this.m_fontSize) / 2f, 0.05f);
											this.m_fontSize = (float)((int)(Mathf.Min(this.m_fontSize, this.m_fontSizeMax) * 20f + 0.5f)) / 20f;
											if (this.loopCountA > 0x14)
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
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									this.ClearMesh();
									TMPro_EventManager.ON_TEXT_CHANGED(this);
									return;
								}
								int num43 = this.m_materialReferences[0].referenceCount * 4;
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
															goto IL_3A52;
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
														if (textAlignment == TextAlignmentOptions.TopGeoAligned)
														{
															goto IL_3A52;
														}
														if (textAlignment == TextAlignmentOptions.Flush || textAlignment == TextAlignmentOptions.CenterGeoAligned)
														{
															goto IL_3AF3;
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
														if (textAlignment == TextAlignmentOptions.BottomFlush)
														{
															goto IL_3C0C;
														}
														for (;;)
														{
															switch (4)
															{
															case 0:
																continue;
															}
															break;
														}
														if (textAlignment == TextAlignmentOptions.BottomGeoAligned)
														{
															goto IL_3C0C;
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
														if (textAlignment == TextAlignmentOptions.BaselineFlush)
														{
															goto IL_3CA9;
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
														if (textAlignment == TextAlignmentOptions.BaselineGeoAligned)
														{
															goto IL_3CA9;
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
														if (textAlignment == TextAlignmentOptions.MidlineFlush)
														{
															goto IL_3CF8;
														}
														for (;;)
														{
															switch (5)
															{
															case 0:
																continue;
															}
															break;
														}
														if (textAlignment == TextAlignmentOptions.MidlineGeoAligned)
														{
															goto IL_3CF8;
														}
														for (;;)
														{
															switch (6)
															{
															case 0:
																continue;
															}
															break;
														}
														if (textAlignment != TextAlignmentOptions.CaplineFlush && textAlignment != TextAlignmentOptions.CaplineGeoAligned)
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
															goto IL_3DE5;
														}
														break;
													}
													a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxCapHeight - margin.y - margin.w) / 2f, 0f);
													goto IL_3DE5;
												}
												IL_3CF8:
												a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_meshExtents.max.y + margin.y + this.m_meshExtents.min.y - margin.w) / 2f, 0f);
												goto IL_3DE5;
											}
											IL_3CA9:
											a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f, 0f);
											goto IL_3DE5;
										}
										IL_3C0C:
										if (this.m_overflowMode != TextOverflowModes.Page)
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
											a = rectTransformCorners[0] + new Vector3(margin.x, 0f - num11 + margin.w, 0f);
										}
										else
										{
											a = rectTransformCorners[0] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].descender + margin.w, 0f);
										}
										goto IL_3DE5;
									}
									IL_3AF3:
									if (this.m_overflowMode != TextOverflowModes.Page)
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
										a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_maxAscender + margin.y + num11 - margin.w) / 2f, 0f);
									}
									else
									{
										a = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(margin.x, 0f - (this.m_textInfo.pageInfo[num6].ascender + margin.y + this.m_textInfo.pageInfo[num6].descender - margin.w) / 2f, 0f);
									}
									goto IL_3DE5;
								}
								IL_3A52:
								if (this.m_overflowMode != TextOverflowModes.Page)
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
									a = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_maxAscender - margin.y, 0f);
								}
								else
								{
									a = rectTransformCorners[1] + new Vector3(margin.x, 0f - this.m_textInfo.pageInfo[num6].ascender - margin.y, 0f);
								}
								IL_3DE5:
								Vector3 vector6 = Vector3.zero;
								Vector3 b4 = Vector3.zero;
								int index_X = 0;
								int index_X2 = 0;
								int num44 = 0;
								int num45 = 0;
								int num46 = 0;
								bool flag11 = false;
								bool flag12 = false;
								int num47 = 0;
								bool flag13;
								if (this.m_canvas.worldCamera == null)
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
									flag13 = false;
								}
								else
								{
									flag13 = true;
								}
								bool flag14 = flag13;
								float num48 = this.m_previousLossyScaleY = base.transform.lossyScale.y;
								RenderMode renderMode = this.m_canvas.renderMode;
								float scaleFactor = this.m_canvas.scaleFactor;
								Color32 color = Color.white;
								Color32 underlineColor = Color.white;
								Color32 highlightColor = new Color32(byte.MaxValue, byte.MaxValue, 0, 0x40);
								float num49 = 0f;
								float num50 = 0f;
								float num51 = 0f;
								float num52 = TMP_Text.k_LargePositiveFloat;
								int num53 = 0;
								float num54 = 0f;
								float num55 = 0f;
								float b5 = 0f;
								TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
								int j = 0;
								while (j < this.m_characterCount)
								{
									TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
									char character2 = characterInfo[j].character;
									int lineNumber6 = (int)characterInfo[j].lineNumber;
									TMP_LineInfo tmp_LineInfo = this.m_textInfo.lineInfo[lineNumber6];
									num45 = lineNumber6 + 1;
									TextAlignmentOptions alignment = tmp_LineInfo.alignment;
									switch (alignment)
									{
									case TextAlignmentOptions.TopLeft:
										goto IL_415D;
									case TextAlignmentOptions.Top:
										goto IL_41AC;
									default:
										switch (alignment)
										{
										case TextAlignmentOptions.Left:
											goto IL_415D;
										case TextAlignmentOptions.Center:
											goto IL_41AC;
										default:
											switch (alignment)
											{
											case TextAlignmentOptions.BottomLeft:
												goto IL_415D;
											case TextAlignmentOptions.Bottom:
												goto IL_41AC;
											default:
												switch (alignment)
												{
												case TextAlignmentOptions.BaselineLeft:
													goto IL_415D;
												case TextAlignmentOptions.Baseline:
													goto IL_41AC;
												default:
													switch (alignment)
													{
													case TextAlignmentOptions.MidlineLeft:
														goto IL_415D;
													case TextAlignmentOptions.Midline:
														goto IL_41AC;
													default:
														switch (alignment)
														{
														case TextAlignmentOptions.CaplineLeft:
															goto IL_415D;
														case TextAlignmentOptions.Capline:
															goto IL_41AC;
														default:
															if (alignment == TextAlignmentOptions.TopFlush)
															{
																goto IL_429B;
															}
															for (;;)
															{
																switch (6)
																{
																case 0:
																	continue;
																}
																break;
															}
															if (alignment != TextAlignmentOptions.TopGeoAligned)
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
																if (alignment == TextAlignmentOptions.Flush)
																{
																	goto IL_429B;
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
																if (alignment != TextAlignmentOptions.CenterGeoAligned)
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
																	if (alignment == TextAlignmentOptions.BottomFlush)
																	{
																		goto IL_429B;
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
																	if (alignment != TextAlignmentOptions.BottomGeoAligned)
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
																		if (alignment == TextAlignmentOptions.BaselineFlush)
																		{
																			goto IL_429B;
																		}
																		for (;;)
																		{
																			switch (4)
																			{
																			case 0:
																				continue;
																			}
																			break;
																		}
																		if (alignment != TextAlignmentOptions.BaselineGeoAligned)
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
																			if (alignment == TextAlignmentOptions.MidlineFlush)
																			{
																				goto IL_429B;
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
																			if (alignment != TextAlignmentOptions.MidlineGeoAligned)
																			{
																				if (alignment == TextAlignmentOptions.CaplineFlush)
																				{
																					goto IL_429B;
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
															goto IL_423A;
														case TextAlignmentOptions.CaplineJustified:
															goto IL_429B;
														}
														break;
													case TextAlignmentOptions.MidlineRight:
														goto IL_423A;
													case TextAlignmentOptions.MidlineJustified:
														goto IL_429B;
													}
													break;
												case TextAlignmentOptions.BaselineRight:
													goto IL_423A;
												case TextAlignmentOptions.BaselineJustified:
													goto IL_429B;
												}
												break;
											case TextAlignmentOptions.BottomRight:
												goto IL_423A;
											case TextAlignmentOptions.BottomJustified:
												goto IL_429B;
											}
											break;
										case TextAlignmentOptions.Right:
											goto IL_423A;
										case TextAlignmentOptions.Justified:
											goto IL_429B;
										}
										break;
									case TextAlignmentOptions.TopRight:
										goto IL_423A;
									case TextAlignmentOptions.TopJustified:
										goto IL_429B;
									}
									IL_45AF:
									b4 = a + vector6;
									bool isVisible = characterInfo[j].isVisible;
									if (isVisible)
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
											float num56 = this.m_uvLineOffset * (float)lineNumber6 % 1f;
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
													for (;;)
													{
														switch (7)
														{
														case 0:
															continue;
														}
														break;
													}
													characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
													characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
													characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
													characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num56;
												}
												else
												{
													characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
													characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
													characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
													characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
												}
												break;
											case TextureMappingOptions.Paragraph:
												characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
												characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
												characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
												characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + vector6.x - this.m_meshExtents.min.x) / (this.m_meshExtents.max.x - this.m_meshExtents.min.x) + num56;
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
													characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num56;
													characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num56;
													characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
													characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
													break;
												case TextureMappingOptions.Paragraph:
													characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num56;
													characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - this.m_meshExtents.min.y) / (this.m_meshExtents.max.y - this.m_meshExtents.min.y) + num56;
													characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
													characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
													break;
												case TextureMappingOptions.MatchAspect:
													Debug.Log("ERROR: Cannot Match both Vertical & Horizontal.");
													break;
												}
												float num57 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
												characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num57 + num56;
												characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
												characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num57 + num56;
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
												float num58 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
												characterInfo[j].vertex_BL.uv2.y = num58 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
												characterInfo[j].vertex_TL.uv2.y = num58 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
												characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
												characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
												break;
											}
											}
											num49 = characterInfo[j].scale * (1f - this.m_charWidthAdjDelta);
											if (!characterInfo[j].isUsingAlternateTypeface)
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
												if ((characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
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
													num49 *= -1f;
												}
											}
											if (renderMode != RenderMode.ScreenSpaceOverlay)
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
												if (renderMode != RenderMode.ScreenSpaceCamera)
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
													if (renderMode == RenderMode.WorldSpace)
													{
														num49 *= num48;
													}
												}
												else
												{
													float num59 = num49;
													float num60;
													if (flag14)
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
														num60 = num48;
													}
													else
													{
														num60 = 1f;
													}
													num49 = num59 * num60;
												}
											}
											else
											{
												num49 *= num48 / scaleFactor;
											}
											float num61 = characterInfo[j].vertex_BL.uv2.x;
											float num62 = characterInfo[j].vertex_BL.uv2.y;
											float num63 = characterInfo[j].vertex_TR.uv2.x;
											float num64 = characterInfo[j].vertex_TR.uv2.y;
											float num65 = (float)((int)num61);
											float num66 = (float)((int)num62);
											num61 -= num65;
											num63 -= num65;
											num62 -= num66;
											num64 -= num66;
											characterInfo[j].vertex_BL.uv2.x = base.PackUV(num61, num62);
											characterInfo[j].vertex_BL.uv2.y = num49;
											characterInfo[j].vertex_TL.uv2.x = base.PackUV(num61, num64);
											characterInfo[j].vertex_TL.uv2.y = num49;
											characterInfo[j].vertex_TR.uv2.x = base.PackUV(num63, num64);
											characterInfo[j].vertex_TR.uv2.y = num49;
											characterInfo[j].vertex_BR.uv2.x = base.PackUV(num63, num62);
											characterInfo[j].vertex_BR.uv2.y = num49;
										}
										if (j >= this.m_maxVisibleCharacters)
										{
											goto IL_56F9;
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
										if (num44 >= this.m_maxVisibleWords)
										{
											goto IL_56F9;
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
										if (lineNumber6 >= this.m_maxVisibleLines)
										{
											goto IL_56F9;
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
										if (this.m_overflowMode == TextOverflowModes.Page)
										{
											goto IL_56F9;
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
										TMP_CharacterInfo[] array = characterInfo;
										int num67 = j;
										array[num67].vertex_BL.position = array[num67].vertex_BL.position + b4;
										TMP_CharacterInfo[] array2 = characterInfo;
										int num68 = j;
										array2[num68].vertex_TL.position = array2[num68].vertex_TL.position + b4;
										TMP_CharacterInfo[] array3 = characterInfo;
										int num69 = j;
										array3[num69].vertex_TR.position = array3[num69].vertex_TR.position + b4;
										TMP_CharacterInfo[] array4 = characterInfo;
										int num70 = j;
										array4[num70].vertex_BR.position = array4[num70].vertex_BR.position + b4;
										IL_586C:
										if (elementType == TMP_TextElementType.Character)
										{
											this.FillCharacterVertexBuffers(j, index_X);
											goto IL_5895;
										}
										if (elementType == TMP_TextElementType.Sprite)
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
											this.FillSpriteVertexBuffers(j, index_X2);
											goto IL_5895;
										}
										goto IL_5895;
										IL_56F9:
										if (j < this.m_maxVisibleCharacters)
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
											if (num44 < this.m_maxVisibleWords)
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
												if (lineNumber6 < this.m_maxVisibleLines)
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
													if (this.m_overflowMode == TextOverflowModes.Page)
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
														if ((int)characterInfo[j].pageNumber == num6)
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
															TMP_CharacterInfo[] array5 = characterInfo;
															int num71 = j;
															array5[num71].vertex_BL.position = array5[num71].vertex_BL.position + b4;
															TMP_CharacterInfo[] array6 = characterInfo;
															int num72 = j;
															array6[num72].vertex_TL.position = array6[num72].vertex_TL.position + b4;
															TMP_CharacterInfo[] array7 = characterInfo;
															int num73 = j;
															array7[num73].vertex_TR.position = array7[num73].vertex_TR.position + b4;
															TMP_CharacterInfo[] array8 = characterInfo;
															int num74 = j;
															array8[num74].vertex_BR.position = array8[num74].vertex_BR.position + b4;
															goto IL_586C;
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
										goto IL_586C;
									}
									IL_5895:
									TMP_CharacterInfo[] characterInfo2 = this.m_textInfo.characterInfo;
									int num75 = j;
									characterInfo2[num75].bottomLeft = characterInfo2[num75].bottomLeft + b4;
									TMP_CharacterInfo[] characterInfo3 = this.m_textInfo.characterInfo;
									int num76 = j;
									characterInfo3[num76].topLeft = characterInfo3[num76].topLeft + b4;
									TMP_CharacterInfo[] characterInfo4 = this.m_textInfo.characterInfo;
									int num77 = j;
									characterInfo4[num77].topRight = characterInfo4[num77].topRight + b4;
									TMP_CharacterInfo[] characterInfo5 = this.m_textInfo.characterInfo;
									int num78 = j;
									characterInfo5[num78].bottomRight = characterInfo5[num78].bottomRight + b4;
									TMP_CharacterInfo[] characterInfo6 = this.m_textInfo.characterInfo;
									int num79 = j;
									characterInfo6[num79].origin = characterInfo6[num79].origin + b4.x;
									TMP_CharacterInfo[] characterInfo7 = this.m_textInfo.characterInfo;
									int num80 = j;
									characterInfo7[num80].xAdvance = characterInfo7[num80].xAdvance + b4.x;
									TMP_CharacterInfo[] characterInfo8 = this.m_textInfo.characterInfo;
									int num81 = j;
									characterInfo8[num81].ascender = characterInfo8[num81].ascender + b4.y;
									TMP_CharacterInfo[] characterInfo9 = this.m_textInfo.characterInfo;
									int num82 = j;
									characterInfo9[num82].descender = characterInfo9[num82].descender + b4.y;
									TMP_CharacterInfo[] characterInfo10 = this.m_textInfo.characterInfo;
									int num83 = j;
									characterInfo10[num83].baseLine = characterInfo10[num83].baseLine + b4.y;
									if (isVisible)
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
									}
									if (lineNumber6 != num46)
									{
										goto IL_5A19;
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
									if (j == this.m_characterCount - 1)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											goto IL_5A19;
										}
									}
									IL_5CC7:
									if (char.IsLetterOrDigit(character2))
									{
										goto IL_5D28;
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (character2 == '-')
									{
										goto IL_5D28;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (character2 == '­')
									{
										goto IL_5D28;
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
									if (character2 == '‐')
									{
										goto IL_5D28;
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
									if (character2 != '‑')
									{
										if (!flag12)
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
											if (j != 0)
											{
												goto IL_6077;
											}
											if (char.IsPunctuation(character2))
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
												if (!char.IsWhiteSpace(character2))
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
													if (character2 != '​')
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
														if (j != this.m_characterCount - 1)
														{
															goto IL_6077;
														}
													}
												}
											}
										}
										if (j > 0 && j < characterInfo.Length - 1)
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
											if (j < this.m_characterCount)
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
												if (character2 != '\'')
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
													if (character2 != '’')
													{
														goto IL_5F5E;
													}
													for (;;)
													{
														switch (6)
														{
														case 0:
															continue;
														}
														break;
													}
												}
												if (char.IsLetterOrDigit(characterInfo[j - 1].character))
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
													if (char.IsLetterOrDigit(characterInfo[j + 1].character))
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
														goto IL_6077;
													}
												}
											}
										}
										IL_5F5E:
										if (j != this.m_characterCount - 1)
										{
											goto IL_5F83;
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
										if (!char.IsLetterOrDigit(character2))
										{
											goto IL_5F83;
										}
										int num84 = j;
										IL_5F87:
										int num85 = num84;
										flag12 = false;
										int num86 = this.m_textInfo.wordInfo.Length;
										int wordCount = this.m_textInfo.wordCount;
										if (this.m_textInfo.wordCount + 1 > num86)
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
											TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num86 + 1);
										}
										this.m_textInfo.wordInfo[wordCount].firstCharacterIndex = num47;
										this.m_textInfo.wordInfo[wordCount].lastCharacterIndex = num85;
										this.m_textInfo.wordInfo[wordCount].characterCount = num85 - num47 + 1;
										this.m_textInfo.wordInfo[wordCount].textComponent = this;
										num44++;
										this.m_textInfo.wordCount++;
										TMP_LineInfo[] lineInfo6 = this.m_textInfo.lineInfo;
										int num87 = lineNumber6;
										lineInfo6[num87].wordCount = lineInfo6[num87].wordCount + 1;
										goto IL_6077;
										IL_5F83:
										num84 = j - 1;
										goto IL_5F87;
									}
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										goto IL_5D28;
									}
									IL_6077:
									bool flag15 = (this.m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline;
									if (flag15)
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
										bool flag16 = true;
										int pageNumber2 = (int)this.m_textInfo.characterInfo[j].pageNumber;
										if (j > this.m_maxVisibleCharacters)
										{
											goto IL_6109;
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
										if (lineNumber6 > this.m_maxVisibleLines)
										{
											goto IL_6109;
										}
										for (;;)
										{
											switch (6)
											{
											case 0:
												continue;
											}
											break;
										}
										if (this.m_overflowMode == TextOverflowModes.Page)
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
											if (pageNumber2 + 1 != this.m_pageToDisplay)
											{
												goto IL_6109;
											}
										}
										IL_610C:
										if (!char.IsWhiteSpace(character2))
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
											if (character2 != '​')
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
												num51 = Mathf.Max(num51, this.m_textInfo.characterInfo[j].scale);
												num52 = Mathf.Min((pageNumber2 != num53) ? TMP_Text.k_LargePositiveFloat : num52, this.m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num51);
												num53 = pageNumber2;
											}
										}
										if (!flag)
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
											if (flag16)
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
												if (j <= tmp_LineInfo.lastVisibleCharacterIndex)
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
													if (character2 != '\n' && character2 != '\r')
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
														if (j == tmp_LineInfo.lastVisibleCharacterIndex)
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
															if (char.IsSeparator(character2))
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
																goto IL_6298;
															}
														}
														flag = true;
														num50 = this.m_textInfo.characterInfo[j].scale;
														if (num51 == 0f)
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
															num51 = num50;
														}
														zero = new Vector3(this.m_textInfo.characterInfo[j].bottomLeft.x, num52, 0f);
														color = this.m_textInfo.characterInfo[j].underlineColor;
													}
												}
											}
										}
										IL_6298:
										if (!flag)
										{
											goto IL_6321;
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
										if (this.m_characterCount != 1)
										{
											goto IL_6321;
										}
										flag = false;
										zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num52, 0f);
										float scale = this.m_textInfo.characterInfo[j].scale;
										this.DrawUnderlineMesh(zero, zero2, ref num43, num50, scale, num51, num49, color);
										num51 = 0f;
										num52 = TMP_Text.k_LargePositiveFloat;
										IL_657A:
										goto IL_65F8;
										IL_6321:
										if (flag)
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
											if (j != tmp_LineInfo.lastCharacterIndex)
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
												if (j < tmp_LineInfo.lastVisibleCharacterIndex)
												{
													goto IL_643A;
												}
												for (;;)
												{
													switch (4)
													{
													case 0:
														continue;
													}
													break;
												}
											}
											if (char.IsWhiteSpace(character2))
											{
												goto IL_637D;
											}
											if (character2 == '​')
											{
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													goto IL_637D;
												}
											}
											else
											{
												zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num52, 0f);
												scale = this.m_textInfo.characterInfo[j].scale;
											}
											IL_640E:
											flag = false;
											this.DrawUnderlineMesh(zero, zero2, ref num43, num50, scale, num51, num49, color);
											num51 = 0f;
											num52 = TMP_Text.k_LargePositiveFloat;
											goto IL_657A;
											IL_637D:
											int lastVisibleCharacterIndex = tmp_LineInfo.lastVisibleCharacterIndex;
											zero2 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num52, 0f);
											scale = this.m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
											goto IL_640E;
										}
										IL_643A:
										if (flag)
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
											if (!flag16)
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
												flag = false;
												zero2 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, num52, 0f);
												scale = this.m_textInfo.characterInfo[j - 1].scale;
												this.DrawUnderlineMesh(zero, zero2, ref num43, num50, scale, num51, num49, color);
												num51 = 0f;
												num52 = TMP_Text.k_LargePositiveFloat;
												goto IL_657A;
											}
										}
										if (flag && j < this.m_characterCount - 1 && !color.Compare(this.m_textInfo.characterInfo[j + 1].underlineColor))
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
											flag = false;
											zero2 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, num52, 0f);
											scale = this.m_textInfo.characterInfo[j].scale;
											this.DrawUnderlineMesh(zero, zero2, ref num43, num50, scale, num51, num49, color);
											num51 = 0f;
											num52 = TMP_Text.k_LargePositiveFloat;
											goto IL_657A;
										}
										goto IL_657A;
										IL_6109:
										flag16 = false;
										goto IL_610C;
									}
									if (flag)
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
										flag = false;
										zero2 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, num52, 0f);
										float scale = this.m_textInfo.characterInfo[j - 1].scale;
										this.DrawUnderlineMesh(zero, zero2, ref num43, num50, scale, num51, num49, color);
										num51 = 0f;
										num52 = TMP_Text.k_LargePositiveFloat;
									}
									IL_65F8:
									bool flag17 = (this.m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
									float strikethrough = fontAsset.fontInfo.strikethrough;
									if (flag17)
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
										bool flag18 = true;
										if (j > this.m_maxVisibleCharacters || lineNumber6 > this.m_maxVisibleLines)
										{
											goto IL_6696;
										}
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										if (this.m_overflowMode == TextOverflowModes.Page)
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
											if ((int)(this.m_textInfo.characterInfo[j].pageNumber + 1) != this.m_pageToDisplay)
											{
												for (;;)
												{
													switch (6)
													{
													case 0:
														continue;
													}
													goto IL_6696;
												}
											}
										}
										IL_6699:
										if (!flag2)
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
											if (flag18 && j <= tmp_LineInfo.lastVisibleCharacterIndex && character2 != '\n')
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
												if (character2 != '\r')
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
													if (j != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
													{
														flag2 = true;
														num54 = this.m_textInfo.characterInfo[j].pointSize;
														num55 = this.m_textInfo.characterInfo[j].scale;
														zero3 = new Vector3(this.m_textInfo.characterInfo[j].bottomLeft.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num55, 0f);
														underlineColor = this.m_textInfo.characterInfo[j].strikethroughColor;
														b5 = this.m_textInfo.characterInfo[j].baseLine;
													}
												}
											}
										}
										if (!flag2)
										{
											goto IL_6833;
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
										if (this.m_characterCount != 1)
										{
											goto IL_6833;
										}
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										flag2 = false;
										zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num55, 0f);
										this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
										IL_6B8C:
										goto IL_6BFE;
										IL_6833:
										if (flag2)
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
											if (j == tmp_LineInfo.lastCharacterIndex)
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
												if (char.IsWhiteSpace(character2))
												{
													goto IL_687A;
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
												if (character2 == '​')
												{
													goto IL_687A;
												}
												zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num55, 0f);
												IL_690F:
												flag2 = false;
												this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
												goto IL_6B8C;
												IL_687A:
												int lastVisibleCharacterIndex2 = tmp_LineInfo.lastVisibleCharacterIndex;
												zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num55, 0f);
												goto IL_690F;
											}
										}
										if (flag2)
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
											if (j < this.m_characterCount)
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
												if (this.m_textInfo.characterInfo[j + 1].pointSize != num54 || !TMP_Math.Approximately(this.m_textInfo.characterInfo[j + 1].baseLine + b4.y, b5))
												{
													flag2 = false;
													int lastVisibleCharacterIndex3 = tmp_LineInfo.lastVisibleCharacterIndex;
													if (j > lastVisibleCharacterIndex3)
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
														zero4 = new Vector3(this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, this.m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num55, 0f);
													}
													else
													{
														zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num55, 0f);
													}
													this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
													goto IL_6B8C;
												}
											}
										}
										if (flag2)
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
											if (j < this.m_characterCount)
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
												if (fontAsset.GetInstanceID() != characterInfo[j + 1].fontAsset.GetInstanceID())
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
													flag2 = false;
													zero4 = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].baseLine + strikethrough * num55, 0f);
													this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
													goto IL_6B8C;
												}
											}
										}
										if (flag2 && !flag18)
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
											flag2 = false;
											zero4 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, this.m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num55, 0f);
											this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
											goto IL_6B8C;
										}
										goto IL_6B8C;
										IL_6696:
										flag18 = false;
										goto IL_6699;
									}
									if (flag2)
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
										flag2 = false;
										zero4 = new Vector3(this.m_textInfo.characterInfo[j - 1].topRight.x, this.m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num55, 0f);
										this.DrawUnderlineMesh(zero3, zero4, ref num43, num55, num55, num55, num49, underlineColor);
									}
									IL_6BFE:
									bool flag19 = (this.m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight;
									if (flag19)
									{
										bool flag20 = true;
										int pageNumber3 = (int)this.m_textInfo.characterInfo[j].pageNumber;
										if (j > this.m_maxVisibleCharacters || lineNumber6 > this.m_maxVisibleLines)
										{
											goto IL_6C7A;
										}
										if (this.m_overflowMode == TextOverflowModes.Page && pageNumber3 + 1 != this.m_pageToDisplay)
										{
											for (;;)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												goto IL_6C7A;
											}
										}
										IL_6C7D:
										if (!flag3)
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
											if (flag20)
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
												if (j <= tmp_LineInfo.lastVisibleCharacterIndex)
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
													if (character2 != '\n' && character2 != '\r')
													{
														if (j != tmp_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2))
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
										if (flag3)
										{
											Color32 highlightColor2 = this.m_textInfo.characterInfo[j].highlightColor;
											bool flag21 = false;
											if (!highlightColor.Compare(highlightColor2))
											{
												vector.x = (vector.x + this.m_textInfo.characterInfo[j].bottomLeft.x) / 2f;
												start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[j].descender);
												vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[j].ascender);
												this.DrawTextHighlight(start, vector, ref num43, highlightColor);
												flag3 = true;
												start = vector;
												vector = new Vector3(this.m_textInfo.characterInfo[j].topRight.x, this.m_textInfo.characterInfo[j].descender, 0f);
												highlightColor = this.m_textInfo.characterInfo[j].highlightColor;
												flag21 = true;
											}
											if (!flag21)
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
												start.x = Mathf.Min(start.x, this.m_textInfo.characterInfo[j].bottomLeft.x);
												start.y = Mathf.Min(start.y, this.m_textInfo.characterInfo[j].descender);
												vector.x = Mathf.Max(vector.x, this.m_textInfo.characterInfo[j].topRight.x);
												vector.y = Mathf.Max(vector.y, this.m_textInfo.characterInfo[j].ascender);
											}
										}
										if (!flag3)
										{
											goto IL_6F31;
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
										if (this.m_characterCount != 1)
										{
											goto IL_6F31;
										}
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										flag3 = false;
										this.DrawTextHighlight(start, vector, ref num43, highlightColor);
										IL_6F95:
										goto IL_6FB6;
										IL_6F31:
										if (flag3)
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
											if (j == tmp_LineInfo.lastCharacterIndex || j >= tmp_LineInfo.lastVisibleCharacterIndex)
											{
												flag3 = false;
												this.DrawTextHighlight(start, vector, ref num43, highlightColor);
												goto IL_6F95;
											}
										}
										if (!flag3)
										{
											goto IL_6F95;
										}
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (!flag20)
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
											flag3 = false;
											this.DrawTextHighlight(start, vector, ref num43, highlightColor);
											goto IL_6F95;
										}
										goto IL_6F95;
										IL_6C7A:
										flag20 = false;
										goto IL_6C7D;
									}
									if (flag3)
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
										flag3 = false;
										this.DrawTextHighlight(start, vector, ref num43, highlightColor);
									}
									IL_6FB6:
									num46 = lineNumber6;
									j++;
									continue;
									IL_5D28:
									if (!flag12)
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
										flag12 = true;
										num47 = j;
									}
									if (flag12)
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
										if (j == this.m_characterCount - 1)
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
											int num88 = this.m_textInfo.wordInfo.Length;
											int wordCount2 = this.m_textInfo.wordCount;
											if (this.m_textInfo.wordCount + 1 > num88)
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
												TMP_TextInfo.Resize<TMP_WordInfo>(ref this.m_textInfo.wordInfo, num88 + 1);
											}
											int num85 = j;
											this.m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num47;
											this.m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num85;
											this.m_textInfo.wordInfo[wordCount2].characterCount = num85 - num47 + 1;
											this.m_textInfo.wordInfo[wordCount2].textComponent = this;
											num44++;
											this.m_textInfo.wordCount++;
											TMP_LineInfo[] lineInfo7 = this.m_textInfo.lineInfo;
											int num89 = lineNumber6;
											lineInfo7[num89].wordCount = lineInfo7[num89].wordCount + 1;
										}
									}
									goto IL_6077;
									IL_5A19:
									if (lineNumber6 != num46)
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
										TMP_LineInfo[] lineInfo8 = this.m_textInfo.lineInfo;
										int num90 = num46;
										lineInfo8[num90].baseline = lineInfo8[num90].baseline + b4.y;
										TMP_LineInfo[] lineInfo9 = this.m_textInfo.lineInfo;
										int num91 = num46;
										lineInfo9[num91].ascender = lineInfo9[num91].ascender + b4.y;
										TMP_LineInfo[] lineInfo10 = this.m_textInfo.lineInfo;
										int num92 = num46;
										lineInfo10[num92].descender = lineInfo10[num92].descender + b4.y;
										this.m_textInfo.lineInfo[num46].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num46].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[num46].descender);
										this.m_textInfo.lineInfo[num46].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[num46].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[num46].ascender);
									}
									if (j == this.m_characterCount - 1)
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
										TMP_LineInfo[] lineInfo11 = this.m_textInfo.lineInfo;
										int num93 = lineNumber6;
										lineInfo11[num93].baseline = lineInfo11[num93].baseline + b4.y;
										TMP_LineInfo[] lineInfo12 = this.m_textInfo.lineInfo;
										int num94 = lineNumber6;
										lineInfo12[num94].ascender = lineInfo12[num94].ascender + b4.y;
										TMP_LineInfo[] lineInfo13 = this.m_textInfo.lineInfo;
										int num95 = lineNumber6;
										lineInfo13[num95].descender = lineInfo13[num95].descender + b4.y;
										this.m_textInfo.lineInfo[lineNumber6].lineExtents.min = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber6].firstCharacterIndex].bottomLeft.x, this.m_textInfo.lineInfo[lineNumber6].descender);
										this.m_textInfo.lineInfo[lineNumber6].lineExtents.max = new Vector2(this.m_textInfo.characterInfo[this.m_textInfo.lineInfo[lineNumber6].lastVisibleCharacterIndex].topRight.x, this.m_textInfo.lineInfo[lineNumber6].ascender);
										goto IL_5CC7;
									}
									goto IL_5CC7;
									IL_415D:
									if (!this.m_isRightToLeft)
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
										vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
									}
									else
									{
										vector6 = new Vector3(0f - tmp_LineInfo.maxAdvance, 0f, 0f);
									}
									goto IL_45AF;
									IL_41AC:
									vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width / 2f - tmp_LineInfo.maxAdvance / 2f, 0f, 0f);
									goto IL_45AF;
									IL_423A:
									if (!this.m_isRightToLeft)
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
										vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width - tmp_LineInfo.maxAdvance, 0f, 0f);
									}
									else
									{
										vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
									}
									goto IL_45AF;
									IL_429B:
									if (character2 != '­')
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
										if (character2 != '​')
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
											if (character2 != '⁠')
											{
												char character3 = characterInfo[tmp_LineInfo.lastCharacterIndex].character;
												bool flag22 = (alignment & (TextAlignmentOptions)0x10) == (TextAlignmentOptions)0x10;
												if (char.IsControl(character3))
												{
													goto IL_430C;
												}
												if (lineNumber6 < this.m_lineNumber)
												{
													goto IL_4337;
												}
												for (;;)
												{
													switch (4)
													{
													case 0:
														continue;
													}
													goto IL_430C;
												}
												IL_45AD:
												goto IL_45AF;
												IL_430C:
												if (!flag22)
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
													if (tmp_LineInfo.maxAdvance > tmp_LineInfo.width)
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
													}
													else
													{
														if (!this.m_isRightToLeft)
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
															vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
															goto IL_45AD;
														}
														vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
														goto IL_45AD;
													}
												}
												IL_4337:
												if (lineNumber6 != num46)
												{
													goto IL_436C;
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
												if (j == 0)
												{
													goto IL_436C;
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
												if (j == this.m_firstVisibleCharacter)
												{
													for (;;)
													{
														switch (3)
														{
														case 0:
															continue;
														}
														goto IL_436C;
													}
												}
												else
												{
													float num96;
													if (!this.m_isRightToLeft)
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
														num96 = tmp_LineInfo.width - tmp_LineInfo.maxAdvance;
													}
													else
													{
														num96 = tmp_LineInfo.width + tmp_LineInfo.maxAdvance;
													}
													float num97 = num96;
													int num98 = tmp_LineInfo.visibleCharacterCount - 1;
													int num99;
													if (characterInfo[tmp_LineInfo.lastCharacterIndex].isVisible)
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
														num99 = tmp_LineInfo.spaceCount;
													}
													else
													{
														num99 = tmp_LineInfo.spaceCount - 1;
													}
													int num100 = num99;
													if (flag11)
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
														num100--;
														num98++;
													}
													float num101 = (num100 <= 0) ? 1f : this.m_wordWrappingRatios;
													if (num100 < 1)
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
														num100 = 1;
													}
													if (character2 != '\t')
													{
														if (char.IsSeparator(character2))
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
														}
														else
														{
															if (!this.m_isRightToLeft)
															{
																vector6 += new Vector3(num97 * num101 / (float)num98, 0f, 0f);
																goto IL_455F;
															}
															vector6 -= new Vector3(num97 * num101 / (float)num98, 0f, 0f);
															goto IL_455F;
														}
													}
													if (!this.m_isRightToLeft)
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
														vector6 += new Vector3(num97 * (1f - num101) / (float)num100, 0f, 0f);
													}
													else
													{
														vector6 -= new Vector3(num97 * (1f - num101) / (float)num100, 0f, 0f);
													}
												}
												IL_455F:
												goto IL_45AD;
												IL_436C:
												if (!this.m_isRightToLeft)
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
													vector6 = new Vector3(tmp_LineInfo.marginLeft, 0f, 0f);
												}
												else
												{
													vector6 = new Vector3(tmp_LineInfo.marginLeft + tmp_LineInfo.width, 0f, 0f);
												}
												if (char.IsSeparator(character2))
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
													flag11 = true;
												}
												else
												{
													flag11 = false;
												}
											}
										}
									}
									goto IL_45AF;
								}
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_textInfo.characterCount = (int)((short)this.m_characterCount);
								this.m_textInfo.spriteCount = this.m_spriteCount;
								this.m_textInfo.lineCount = (int)((short)num45);
								TMP_TextInfo textInfo = this.m_textInfo;
								int wordCount3;
								if (num44 != 0 && this.m_characterCount > 0)
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
									wordCount3 = (int)((short)num44);
								}
								else
								{
									wordCount3 = 1;
								}
								textInfo.wordCount = wordCount3;
								this.m_textInfo.pageCount = this.m_pageNumber + 1;
								if (this.m_renderMode == TextRenderFlags.Render)
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
									if (this.m_canvas.additionalShaderChannels != (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent))
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
										this.m_canvas.additionalShaderChannels |= (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent);
									}
									if (this.m_geometrySortingOrder != VertexSortingOrder.Normal)
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
										this.m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
									}
									this.m_mesh.MarkDynamic();
									this.m_mesh.vertices = this.m_textInfo.meshInfo[0].vertices;
									this.m_mesh.uv = this.m_textInfo.meshInfo[0].uvs0;
									this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
									this.m_mesh.colors32 = this.m_textInfo.meshInfo[0].colors32;
									this.m_mesh.RecalculateBounds();
									this.m_canvasRenderer.SetMesh(this.m_mesh);
									Color color2 = this.m_canvasRenderer.GetColor();
									for (int k = 1; k < this.m_textInfo.materialCount; k++)
									{
										this.m_textInfo.meshInfo[k].ClearUnusedVertices();
										if (this.m_subTextObjects[k] == null)
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
											this.m_subTextObjects[k].canvasRenderer.SetMesh(this.m_subTextObjects[k].mesh);
											this.m_subTextObjects[k].canvasRenderer.SetColor(color2);
										}
									}
								}
								TMPro_EventManager.ON_TEXT_CHANGED(this);
								return;
							}
						}
					}
					this.ClearMesh();
					this.m_preferredWidth = 0f;
					this.m_preferredHeight = 0f;
					TMPro_EventManager.ON_TEXT_CHANGED(this);
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GenerateTextMesh()).MethodHandle;
				}
			}
			Debug.LogWarning("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + base.GetInstanceID());
		}

		protected override Vector3[] GetTextContainerLocalCorners()
		{
			if (this.m_rectTransform == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetTextContainerLocalCorners()).MethodHandle;
				}
				this.m_rectTransform = base.rectTransform;
			}
			this.m_rectTransform.GetLocalCorners(this.m_RectTransformCorners);
			return this.m_RectTransformCorners;
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			int num = 1;
			while (num < this.m_subTextObjects.Length && this.m_subTextObjects[num] != null)
			{
				if (this.m_subTextObjects[num].enabled != state)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetActiveSubMeshes(bool)).MethodHandle;
					}
					this.m_subTextObjects[num].enabled = state;
				}
				num++;
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(this.m_subTextObjects[i] != null))
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						goto IL_192;
					}
				}
				else
				{
					Bounds bounds2 = this.m_subTextObjects[i].mesh.bounds;
					float x;
					if (min.x < bounds2.min.x)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetCompoundBounds()).MethodHandle;
						}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
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
			IL_192:
			Vector3 center = (min + max) / 2f;
			Vector2 v = max - min;
			return new Bounds(center, v);
		}

		private void UpdateSDFScale(float lossyScale)
		{
			if (this.m_canvas == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateSDFScale(float)).MethodHandle;
				}
				this.m_canvas = this.GetCanvas();
				if (this.m_canvas == null)
				{
					return;
				}
			}
			lossyScale = ((lossyScale != 0f) ? lossyScale : 1f);
			float scaleFactor = this.m_canvas.scaleFactor;
			float num;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
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
				num = lossyScale / scaleFactor;
			}
			else if (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera)
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
				float num2;
				if (this.m_canvas.worldCamera != null)
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
			for (int i = 0; i < this.m_textInfo.characterCount; i++)
			{
				if (this.m_textInfo.characterInfo[i].isVisible)
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
					if (this.m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
					{
						float num3 = num * this.m_textInfo.characterInfo[i].scale * (1f - this.m_charWidthAdjDelta);
						if (!this.m_textInfo.characterInfo[i].isUsingAlternateTypeface)
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
							if ((this.m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
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
								num3 *= -1f;
							}
						}
						int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
						int vertexIndex = this.m_textInfo.characterInfo[i].vertexIndex;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num3;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num3;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num3;
						this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num3;
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < this.m_textInfo.materialCount; j++)
			{
				if (j == 0)
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
					this.m_mesh.uv2 = this.m_textInfo.meshInfo[0].uvs2;
					this.m_canvasRenderer.SetMesh(this.m_mesh);
				}
				else
				{
					this.m_subTextObjects[j].mesh.uv2 = this.m_textInfo.meshInfo[j].uvs2;
					this.m_subTextObjects[j].canvasRenderer.SetMesh(this.m_subTextObjects[j].mesh);
				}
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

		public override Material materialForRendering
		{
			get
			{
				return TMP_MaterialManager.GetMaterialForRendering(this, this.m_sharedMaterial);
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.set_autoSizeTextContainer(bool)).MethodHandle;
					}
					return;
				}
				this.m_autoSizeTextContainer = value;
				if (this.m_autoSizeTextContainer)
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
					CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
					this.SetLayoutDirty();
				}
			}
		}

		public override Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		public new CanvasRenderer canvasRenderer
		{
			get
			{
				if (this.m_canvasRenderer == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.get_canvasRenderer()).MethodHandle;
					}
					this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
				}
				return this.m_canvasRenderer;
			}
		}

		public InlineGraphicManager inlineGraphicManager
		{
			get
			{
				return this.m_inlineGraphics;
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!base.gameObject.activeInHierarchy)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.CalculateLayoutInputHorizontal()).MethodHandle;
				}
				return;
			}
			if (!this.m_isCalculateSizeRequired)
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
				if (!this.m_rectTransform.hasChanged)
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
			this.m_preferredWidth = base.GetPreferredWidth();
			this.ComputeMarginSize();
			this.m_isLayoutDirty = true;
		}

		public void CalculateLayoutInputVertical()
		{
			if (!base.gameObject.activeInHierarchy)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.CalculateLayoutInputVertical()).MethodHandle;
				}
				return;
			}
			if (!this.m_isCalculateSizeRequired)
			{
				if (!this.m_rectTransform.hasChanged)
				{
					goto IL_5D;
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
			this.m_preferredHeight = base.GetPreferredHeight();
			this.ComputeMarginSize();
			this.m_isLayoutDirty = true;
			IL_5D:
			this.m_isCalculateSizeRequired = false;
		}

		public override void SetVerticesDirty()
		{
			if (!this.m_verticesAlreadyDirty && !(this == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetVerticesDirty()).MethodHandle;
				}
				if (this.IsActive())
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
					if (!CanvasUpdateRegistry.IsRebuildingGraphics())
					{
						this.m_verticesAlreadyDirty = true;
						CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
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
			}
		}

		public override void SetLayoutDirty()
		{
			this.m_isPreferredWidthDirty = true;
			this.m_isPreferredHeightDirty = true;
			if (!this.m_layoutAlreadyDirty)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetLayoutDirty()).MethodHandle;
				}
				if (!(this == null))
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
					if (this.IsActive())
					{
						this.m_layoutAlreadyDirty = true;
						LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
						this.m_isLayoutDirty = true;
						return;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}

		public override void SetMaterialDirty()
		{
			if (!(this == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.SetMaterialDirty()).MethodHandle;
				}
				if (this.IsActive() && !CanvasUpdateRegistry.IsRebuildingGraphics())
				{
					this.m_isMaterialDirty = true;
					CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
					if (this.m_OnDirtyMaterialCallback != null)
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
						this.m_OnDirtyMaterialCallback();
					}
					return;
				}
			}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.Rebuild(CanvasUpdate)).MethodHandle;
				}
				return;
			}
			if (update == CanvasUpdate.Prelayout)
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
				if (this.m_autoSizeTextContainer)
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
					this.m_rectTransform.sizeDelta = base.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
				}
			}
			else if (update == CanvasUpdate.PreRender)
			{
				this.OnPreRenderCanvas();
				this.m_verticesAlreadyDirty = false;
				this.m_layoutAlreadyDirty = false;
				if (!this.m_isMaterialDirty)
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
					return;
				}
				this.UpdateMaterial();
				this.m_isMaterialDirty = false;
			}
		}

		private void UpdateSubObjectPivot()
		{
			if (this.m_textInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateSubObjectPivot()).MethodHandle;
				}
				return;
			}
			int i = 1;
			while (i < this.m_subTextObjects.Length)
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
				if (!(this.m_subTextObjects[i] != null))
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
					this.m_subTextObjects[i].SetPivotDirty();
					i++;
				}
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetModifiedMaterial(Material)).MethodHandle;
				}
				this.m_stencilID = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_stencilID > 0)
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
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_stencilID);
				if (this.m_MaskMaterial != null)
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
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		protected override void UpdateMaterial()
		{
			if (this.m_sharedMaterial == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateMaterial()).MethodHandle;
				}
				return;
			}
			if (this.m_canvasRenderer == null)
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
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, this.m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
		}

		public Vector4 maskOffset
		{
			get
			{
				return this.m_maskOffset;
			}
			set
			{
				this.m_maskOffset = value;
				this.UpdateMask();
				this.m_havePropertiesChanged = true;
			}
		}

		public override void RecalculateClipping()
		{
			base.RecalculateClipping();
		}

		public override void RecalculateMasking()
		{
			this.m_ShouldRecalculateStencil = true;
			this.SetMaterialDirty();
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			if (this.m_ignoreRectMaskCulling)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.Cull(Rect, bool)).MethodHandle;
				}
				return;
			}
			base.Cull(clipRect, validRect);
		}

		public override void UpdateMeshPadding()
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_enableExtraPadding, this.m_isUsingBold);
			this.m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(this.m_sharedMaterial);
			this.m_havePropertiesChanged = true;
			this.checkPaddingRequired = false;
			if (this.m_textInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateMeshPadding()).MethodHandle;
				}
				return;
			}
			for (int i = 1; i < this.m_textInfo.materialCount; i++)
			{
				this.m_subTextObjects[i].UpdateMeshPadding(this.m_enableExtraPadding, this.m_isUsingBold);
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

		protected override void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				this.m_subTextObjects[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		protected override void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				this.m_subTextObjects[i].CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			}
		}

		public override void ForceMeshUpdate()
		{
			this.m_havePropertiesChanged = true;
			this.OnPreRenderCanvas();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			this.m_havePropertiesChanged = true;
			this.m_ignoreActiveState = true;
			this.OnPreRenderCanvas();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			base.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			this.m_renderMode = TextRenderFlags.DontRender;
			this.ComputeMarginSize();
			if (this.m_canvas == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.GetTextInfo(string)).MethodHandle;
				}
				this.m_canvas = base.canvas;
			}
			this.GenerateTextMesh();
			this.m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh()
		{
			this.m_canvasRenderer.SetMesh(null);
			int i = 1;
			while (i < this.m_subTextObjects.Length)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.ClearMesh()).MethodHandle;
				}
				if (!(this.m_subTextObjects[i] != null))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						return;
					}
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(null);
					i++;
				}
			}
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
			if (index == 0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateGeometry(Mesh, int)).MethodHandle;
				}
				this.m_canvasRenderer.SetMesh(mesh);
			}
			else
			{
				this.m_subTextObjects[index].canvasRenderer.SetMesh(mesh);
			}
		}

		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = this.m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh mesh;
				if (i == 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateVertexData(TMP_VertexDataUpdateFlags)).MethodHandle;
					}
					mesh = this.m_mesh;
				}
				else
				{
					mesh = this.m_subTextObjects[i].mesh;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
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
					mesh.vertices = this.m_textInfo.meshInfo[i].vertices;
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
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
				if (i == 0)
				{
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TextMeshProUGUI.UpdateVertexData()).MethodHandle;
					}
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
				if (i == 0)
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
					this.m_canvasRenderer.SetMesh(mesh);
				}
				else
				{
					this.m_subTextObjects[i].canvasRenderer.SetMesh(mesh);
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}

		public void UpdateFontAsset()
		{
			this.LoadFontAsset();
		}
	}
}
