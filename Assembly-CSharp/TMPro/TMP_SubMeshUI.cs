using System;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	public class TMP_SubMeshUI : MaskableGraphic, IClippable, IMaskable, IMaterialModifier
	{
		[SerializeField]
		private TMP_FontAsset m_fontAsset;

		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		private Material m_material;

		[SerializeField]
		private Material m_sharedMaterial;

		private Material m_fallbackMaterial;

		private Material m_fallbackSourceMaterial;

		[SerializeField]
		private bool m_isDefaultMaterial;

		[SerializeField]
		private float m_padding;

		[SerializeField]
		private CanvasRenderer m_canvasRenderer;

		private Mesh m_mesh;

		[SerializeField]
		private TextMeshProUGUI m_TextComponent;

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private bool m_materialDirty;

		[SerializeField]
		private int m_materialReferenceIndex;

		public TMP_FontAsset fontAsset
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				this.m_fontAsset = value;
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
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (this.sharedMaterial != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.get_mainTexture()).MethodHandle;
					}
					return this.sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex);
				}
				return null;
			}
		}

		public override Material material
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.set_material(Material)).MethodHandle;
					}
					if (this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
					{
						return;
					}
				}
				this.m_material = value;
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		public Material sharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				this.SetSharedMaterial(value);
			}
		}

		public Material fallbackMaterial
		{
			get
			{
				return this.m_fallbackMaterial;
			}
			set
			{
				if (this.m_fallbackMaterial == value)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.set_fallbackMaterial(Material)).MethodHandle;
					}
					return;
				}
				if (this.m_fallbackMaterial != null)
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
					if (this.m_fallbackMaterial != value)
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
						TMP_MaterialManager.ReleaseFallbackMaterial(this.m_fallbackMaterial);
					}
				}
				this.m_fallbackMaterial = value;
				TMP_MaterialManager.AddFallbackMaterialReference(this.m_fallbackMaterial);
				this.SetSharedMaterial(this.m_fallbackMaterial);
			}
		}

		public Material fallbackSourceMaterial
		{
			get
			{
				return this.m_fallbackSourceMaterial;
			}
			set
			{
				this.m_fallbackSourceMaterial = value;
			}
		}

		public override Material materialForRendering
		{
			get
			{
				return TMP_MaterialManager.GetMaterialForRendering(this, this.m_sharedMaterial);
			}
		}

		public bool isDefaultMaterial
		{
			get
			{
				return this.m_isDefaultMaterial;
			}
			set
			{
				this.m_isDefaultMaterial = value;
			}
		}

		public float padding
		{
			get
			{
				return this.m_padding;
			}
			set
			{
				this.m_padding = value;
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
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.get_canvasRenderer()).MethodHandle;
					}
					this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
				}
				return this.m_canvasRenderer;
			}
		}

		public Mesh mesh
		{
			get
			{
				if (this.m_mesh == null)
				{
					this.m_mesh = new Mesh();
					this.m_mesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_mesh;
			}
			set
			{
				this.m_mesh = value;
			}
		}

		public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject("TMP UI SubObject [" + materialReference.material.name + "]", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(textComponent.transform, false);
			gameObject.layer = textComponent.gameObject.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.sizeDelta = Vector2.zero;
			component.pivot = textComponent.rectTransform.pivot;
			TMP_SubMeshUI tmp_SubMeshUI = gameObject.AddComponent<TMP_SubMeshUI>();
			tmp_SubMeshUI.m_canvasRenderer = tmp_SubMeshUI.canvasRenderer;
			tmp_SubMeshUI.m_TextComponent = textComponent;
			tmp_SubMeshUI.m_materialReferenceIndex = materialReference.index;
			tmp_SubMeshUI.m_fontAsset = materialReference.fontAsset;
			tmp_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
			tmp_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tmp_SubMeshUI.SetSharedMaterial(materialReference.material);
			return tmp_SubMeshUI;
		}

		protected override void OnEnable()
		{
			if (!this.m_isRegisteredForEvents)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.OnEnable()).MethodHandle;
				}
				this.m_isRegisteredForEvents = true;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		protected override void OnDisable()
		{
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_MaskMaterial != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.OnDisable()).MethodHandle;
				}
				TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				this.m_MaskMaterial = null;
			}
			if (this.m_fallbackMaterial != null)
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
				TMP_MaterialManager.ReleaseFallbackMaterial(this.m_fallbackMaterial);
				this.m_fallbackMaterial = null;
			}
			base.OnDisable();
		}

		protected override void OnDestroy()
		{
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.OnDestroy()).MethodHandle;
				}
				UnityEngine.Object.DestroyImmediate(this.m_mesh);
			}
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
			if (this.m_fallbackMaterial != null)
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
				TMP_MaterialManager.ReleaseFallbackMaterial(this.m_fallbackMaterial);
				this.m_fallbackMaterial = null;
			}
			this.m_isRegisteredForEvents = false;
			this.RecalculateClipping();
		}

		protected override void OnTransformParentChanged()
		{
			if (!this.IsActive())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.OnTransformParentChanged()).MethodHandle;
				}
				return;
			}
			this.m_ShouldRecalculateStencil = true;
			this.RecalculateClipping();
			this.RecalculateMasking();
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (this.m_ShouldRecalculateStencil)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.GetModifiedMaterial(Material)).MethodHandle;
				}
				this.m_StencilValue = TMP_MaterialManager.GetStencilID(base.gameObject);
				this.m_ShouldRecalculateStencil = false;
			}
			if (this.m_StencilValue > 0)
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
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, this.m_StencilValue);
				if (this.m_MaskMaterial != null)
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
					TMP_MaterialManager.ReleaseStencilMaterial(this.m_MaskMaterial);
				}
				this.m_MaskMaterial = material;
			}
			return material;
		}

		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(this.m_sharedMaterial, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		public float GetPaddingForMaterial(Material mat)
		{
			return ShaderUtilities.GetPadding(mat, this.m_TextComponent.extraPadding, this.m_TextComponent.isUsingBold);
		}

		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			this.m_padding = ShaderUtilities.GetPadding(this.m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		public override void SetAllDirty()
		{
		}

		public override void SetVerticesDirty()
		{
			if (!this.IsActive())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.SetVerticesDirty()).MethodHandle;
				}
				return;
			}
			if (this.m_TextComponent != null)
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
				this.m_TextComponent.havePropertiesChanged = true;
				this.m_TextComponent.SetVerticesDirty();
			}
		}

		public override void SetLayoutDirty()
		{
		}

		public override void SetMaterialDirty()
		{
			this.m_materialDirty = true;
			this.UpdateMaterial();
			if (this.m_OnDirtyMaterialCallback != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.SetMaterialDirty()).MethodHandle;
				}
				this.m_OnDirtyMaterialCallback();
			}
		}

		public void SetPivotDirty()
		{
			if (!this.IsActive())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.SetPivotDirty()).MethodHandle;
				}
				return;
			}
			base.rectTransform.pivot = this.m_TextComponent.rectTransform.pivot;
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			if (this.m_TextComponent.ignoreRectMaskCulling)
			{
				return;
			}
			base.Cull(clipRect, validRect);
		}

		protected override void UpdateGeometry()
		{
			Debug.Log("UpdateGeometry()");
		}

		public override void Rebuild(CanvasUpdate update)
		{
			if (update == CanvasUpdate.PreRender)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.Rebuild(CanvasUpdate)).MethodHandle;
				}
				if (!this.m_materialDirty)
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
					return;
				}
				this.UpdateMaterial();
				this.m_materialDirty = false;
			}
		}

		public void RefreshMaterial()
		{
			this.UpdateMaterial();
		}

		protected override void UpdateMaterial()
		{
			if (this.m_canvasRenderer == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.UpdateMaterial()).MethodHandle;
				}
				this.m_canvasRenderer = this.canvasRenderer;
			}
			this.m_canvasRenderer.materialCount = 1;
			this.m_canvasRenderer.SetMaterial(this.materialForRendering, 0);
			this.m_canvasRenderer.SetTexture(this.mainTexture);
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

		private Material GetMaterial()
		{
			return this.m_sharedMaterial;
		}

		private Material GetMaterial(Material mat)
		{
			if (!(this.m_material == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.GetMaterial(Material)).MethodHandle;
				}
				if (this.m_material.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_53;
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
			this.m_material = this.CreateMaterialInstance(mat);
			IL_53:
			this.m_sharedMaterial = this.m_material;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetVerticesDirty();
			this.SetMaterialDirty();
			return this.m_sharedMaterial;
		}

		private Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		private Material GetSharedMaterial()
		{
			if (this.m_canvasRenderer == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TMP_SubMeshUI.GetSharedMaterial()).MethodHandle;
				}
				this.m_canvasRenderer = base.GetComponent<CanvasRenderer>();
			}
			return this.m_canvasRenderer.GetMaterial();
		}

		private void SetSharedMaterial(Material mat)
		{
			this.m_sharedMaterial = mat;
			this.m_Material = this.m_sharedMaterial;
			this.m_padding = this.GetPaddingForMaterial();
			this.SetMaterialDirty();
		}
	}
}
