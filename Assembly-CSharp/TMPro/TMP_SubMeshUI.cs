using System;
using System.Text;
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
				return m_fontAsset;
			}
			set
			{
				m_fontAsset = value;
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
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (sharedMaterial != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex);
						}
					}
				}
				return null;
			}
		}

		public override Material material
		{
			get
			{
				return GetMaterial(m_sharedMaterial);
			}
			set
			{
				if (m_sharedMaterial != null)
				{
					if (m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
					{
						return;
					}
				}
				m_sharedMaterial = (m_material = value);
				m_padding = GetPaddingForMaterial();
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}

		public Material sharedMaterial
		{
			get
			{
				return m_sharedMaterial;
			}
			set
			{
				SetSharedMaterial(value);
			}
		}

		public Material fallbackMaterial
		{
			get
			{
				return m_fallbackMaterial;
			}
			set
			{
				if (m_fallbackMaterial == value)
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
				if (m_fallbackMaterial != null)
				{
					if (m_fallbackMaterial != value)
					{
						TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
					}
				}
				m_fallbackMaterial = value;
				TMP_MaterialManager.AddFallbackMaterialReference(m_fallbackMaterial);
				SetSharedMaterial(m_fallbackMaterial);
			}
		}

		public Material fallbackSourceMaterial
		{
			get
			{
				return m_fallbackSourceMaterial;
			}
			set
			{
				m_fallbackSourceMaterial = value;
			}
		}

		public override Material materialForRendering
		{
			get { return TMP_MaterialManager.GetMaterialForRendering(this, m_sharedMaterial); }
		}

		public bool isDefaultMaterial
		{
			get
			{
				return m_isDefaultMaterial;
			}
			set
			{
				m_isDefaultMaterial = value;
			}
		}

		public float padding
		{
			get
			{
				return m_padding;
			}
			set
			{
				m_padding = value;
			}
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

		public Mesh mesh
		{
			get
			{
				if (m_mesh == null)
				{
					m_mesh = new Mesh();
					m_mesh.hideFlags = HideFlags.HideAndDontSave;
				}
				return m_mesh;
			}
			set
			{
				m_mesh = value;
			}
		}

		public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
		{
			GameObject gameObject = new GameObject(new StringBuilder().Append("TMP UI SubObject [").Append(materialReference.material.name).Append("]").ToString(), typeof(RectTransform));
			gameObject.transform.SetParent(textComponent.transform, false);
			gameObject.layer = textComponent.gameObject.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.sizeDelta = Vector2.zero;
			component.pivot = textComponent.rectTransform.pivot;
			TMP_SubMeshUI tMP_SubMeshUI = gameObject.AddComponent<TMP_SubMeshUI>();
			tMP_SubMeshUI.m_canvasRenderer = tMP_SubMeshUI.canvasRenderer;
			tMP_SubMeshUI.m_TextComponent = textComponent;
			tMP_SubMeshUI.m_materialReferenceIndex = materialReference.index;
			tMP_SubMeshUI.m_fontAsset = materialReference.fontAsset;
			tMP_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
			tMP_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tMP_SubMeshUI.SetSharedMaterial(materialReference.material);
			return tMP_SubMeshUI;
		}

		protected override void OnEnable()
		{
			if (!m_isRegisteredForEvents)
			{
				m_isRegisteredForEvents = true;
			}
			m_ShouldRecalculateStencil = true;
			RecalculateClipping();
			RecalculateMasking();
		}

		protected override void OnDisable()
		{
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
				m_MaskMaterial = null;
			}
			if (m_fallbackMaterial != null)
			{
				TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				m_fallbackMaterial = null;
			}
			base.OnDisable();
		}

		protected override void OnDestroy()
		{
			if (m_mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(m_mesh);
			}
			if (m_MaskMaterial != null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
			}
			if (m_fallbackMaterial != null)
			{
				TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				m_fallbackMaterial = null;
			}
			m_isRegisteredForEvents = false;
			RecalculateClipping();
		}

		protected override void OnTransformParentChanged()
		{
			if (!IsActive())
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
			m_ShouldRecalculateStencil = true;
			RecalculateClipping();
			RecalculateMasking();
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material material = baseMaterial;
			if (m_ShouldRecalculateStencil)
			{
				m_StencilValue = TMP_MaterialManager.GetStencilID(base.gameObject);
				m_ShouldRecalculateStencil = false;
			}
			if (m_StencilValue > 0)
			{
				material = TMP_MaterialManager.GetStencilMaterial(baseMaterial, m_StencilValue);
				if (m_MaskMaterial != null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
				}
				m_MaskMaterial = material;
			}
			return material;
		}

		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(m_sharedMaterial, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
		}

		public float GetPaddingForMaterial(Material mat)
		{
			return ShaderUtilities.GetPadding(mat, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
		}

		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		public override void SetAllDirty()
		{
		}

		public override void SetVerticesDirty()
		{
			if (!IsActive())
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
			if (!(m_TextComponent != null))
			{
				return;
			}
			while (true)
			{
				m_TextComponent.havePropertiesChanged = true;
				m_TextComponent.SetVerticesDirty();
				return;
			}
		}

		public override void SetLayoutDirty()
		{
		}

		public override void SetMaterialDirty()
		{
			m_materialDirty = true;
			UpdateMaterial();
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

		public void SetPivotDirty()
		{
			if (!IsActive())
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
			base.rectTransform.pivot = m_TextComponent.rectTransform.pivot;
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			if (!m_TextComponent.ignoreRectMaskCulling)
			{
				base.Cull(clipRect, validRect);
			}
		}

		protected override void UpdateGeometry()
		{
			Debug.Log("UpdateGeometry()");
		}

		public override void Rebuild(CanvasUpdate update)
		{
			if (update != CanvasUpdate.PreRender)
			{
				return;
			}
			while (true)
			{
				if (!m_materialDirty)
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
				UpdateMaterial();
				m_materialDirty = false;
				return;
			}
		}

		public void RefreshMaterial()
		{
			UpdateMaterial();
		}

		protected override void UpdateMaterial()
		{
			if (m_canvasRenderer == null)
			{
				m_canvasRenderer = canvasRenderer;
			}
			m_canvasRenderer.materialCount = 1;
			m_canvasRenderer.SetMaterial(materialForRendering, 0);
			m_canvasRenderer.SetTexture(mainTexture);
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

		private Material GetMaterial()
		{
			return m_sharedMaterial;
		}

		private Material GetMaterial(Material mat)
		{
			if (!(m_material == null))
			{
				if (m_material.GetInstanceID() == mat.GetInstanceID())
				{
					goto IL_0053;
				}
			}
			m_material = CreateMaterialInstance(mat);
			goto IL_0053;
			IL_0053:
			m_sharedMaterial = m_material;
			m_padding = GetPaddingForMaterial();
			SetVerticesDirty();
			SetMaterialDirty();
			return m_sharedMaterial;
		}

		private Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			material.name += " (Instance)";
			return material;
		}

		private Material GetSharedMaterial()
		{
			if (m_canvasRenderer == null)
			{
				m_canvasRenderer = GetComponent<CanvasRenderer>();
			}
			return m_canvasRenderer.GetMaterial();
		}

		private void SetSharedMaterial(Material mat)
		{
			m_sharedMaterial = mat;
			m_Material = m_sharedMaterial;
			m_padding = GetPaddingForMaterial();
			SetMaterialDirty();
		}
	}
}
