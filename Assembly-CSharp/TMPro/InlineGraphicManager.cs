using System;
using UnityEngine;

namespace TMPro
{
	[ExecuteInEditMode]
	public class InlineGraphicManager : MonoBehaviour
	{
		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		[HideInInspector]
		private InlineGraphic m_inlineGraphic;

		[SerializeField]
		[HideInInspector]
		private CanvasRenderer m_inlineGraphicCanvasRenderer;

		private UIVertex[] m_uiVertex;

		private RectTransform m_inlineGraphicRectTransform;

		private TMP_Text m_textComponent;

		private bool m_isInitialized;

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.LoadSpriteAsset(value);
			}
		}

		public InlineGraphic inlineGraphic
		{
			get
			{
				return this.m_inlineGraphic;
			}
			set
			{
				if (this.m_inlineGraphic != value)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.set_inlineGraphic(InlineGraphic)).MethodHandle;
					}
					this.m_inlineGraphic = value;
				}
			}
		}

		public CanvasRenderer canvasRenderer
		{
			get
			{
				return this.m_inlineGraphicCanvasRenderer;
			}
		}

		public UIVertex[] uiVertex
		{
			get
			{
				return this.m_uiVertex;
			}
		}

		private void Awake()
		{
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.Awake()).MethodHandle;
				}
				Debug.LogWarning("InlineGraphicManager component is now Obsolete and has been removed from [" + base.gameObject.name + "] along with its InlineGraphic child.", this);
			}
			if (this.inlineGraphic.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(this.inlineGraphic.gameObject);
				this.inlineGraphic = null;
			}
			UnityEngine.Object.DestroyImmediate(this);
		}

		private void OnEnable()
		{
			base.enabled = false;
		}

		private void OnDisable()
		{
		}

		private void OnDestroy()
		{
		}

		private void LoadSpriteAsset(TMP_SpriteAsset spriteAsset)
		{
			if (spriteAsset == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.LoadSpriteAsset(TMP_SpriteAsset)).MethodHandle;
				}
				if (TMP_Settings.defaultSpriteAsset != null)
				{
					spriteAsset = TMP_Settings.defaultSpriteAsset;
				}
				else
				{
					spriteAsset = (Resources.Load("Sprite Assets/Default Sprite Asset") as TMP_SpriteAsset);
				}
			}
			this.m_spriteAsset = spriteAsset;
			this.m_inlineGraphic.texture = this.m_spriteAsset.spriteSheet;
			if (this.m_textComponent != null && this.m_isInitialized)
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
				this.m_textComponent.havePropertiesChanged = true;
				this.m_textComponent.SetVerticesDirty();
			}
		}

		public void AddInlineGraphicsChild()
		{
			if (this.m_inlineGraphic != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.AddInlineGraphicsChild()).MethodHandle;
				}
				return;
			}
			GameObject gameObject = new GameObject("Inline Graphic");
			this.m_inlineGraphic = gameObject.AddComponent<InlineGraphic>();
			this.m_inlineGraphicRectTransform = gameObject.GetComponent<RectTransform>();
			this.m_inlineGraphicCanvasRenderer = gameObject.GetComponent<CanvasRenderer>();
			this.m_inlineGraphicRectTransform.SetParent(base.transform, false);
			this.m_inlineGraphicRectTransform.localPosition = Vector3.zero;
			this.m_inlineGraphicRectTransform.anchoredPosition3D = Vector3.zero;
			this.m_inlineGraphicRectTransform.sizeDelta = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMin = Vector2.zero;
			this.m_inlineGraphicRectTransform.anchorMax = Vector2.one;
			this.m_textComponent = base.GetComponent<TMP_Text>();
		}

		public void AllocatedVertexBuffers(int size)
		{
			if (this.m_inlineGraphic == null)
			{
				this.AddInlineGraphicsChild();
				this.LoadSpriteAsset(this.m_spriteAsset);
			}
			if (this.m_uiVertex == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.AllocatedVertexBuffers(int)).MethodHandle;
				}
				this.m_uiVertex = new UIVertex[4];
			}
			int num = size * 4;
			if (num > this.m_uiVertex.Length)
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
				this.m_uiVertex = new UIVertex[Mathf.NextPowerOfTwo(num)];
			}
		}

		public void UpdatePivot(Vector2 pivot)
		{
			if (this.m_inlineGraphicRectTransform == null)
			{
				this.m_inlineGraphicRectTransform = this.m_inlineGraphic.GetComponent<RectTransform>();
			}
			this.m_inlineGraphicRectTransform.pivot = pivot;
		}

		public void ClearUIVertex()
		{
			if (this.uiVertex != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.ClearUIVertex()).MethodHandle;
				}
				if (this.uiVertex.Length > 0)
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
					Array.Clear(this.uiVertex, 0, this.uiVertex.Length);
					this.m_inlineGraphicCanvasRenderer.Clear();
				}
			}
		}

		public void DrawSprite(UIVertex[] uiVertices, int spriteCount)
		{
			if (this.m_inlineGraphicCanvasRenderer == null)
			{
				this.m_inlineGraphicCanvasRenderer = this.m_inlineGraphic.GetComponent<CanvasRenderer>();
			}
			this.m_inlineGraphicCanvasRenderer.SetVertices(uiVertices, spriteCount * 4);
			this.m_inlineGraphic.UpdateMaterial();
		}

		public TMP_Sprite GetSprite(int index)
		{
			if (this.m_spriteAsset == null)
			{
				Debug.LogWarning("No Sprite Asset is assigned.", this);
				return null;
			}
			if (this.m_spriteAsset.spriteInfoList != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.GetSprite(int)).MethodHandle;
				}
				if (index <= this.m_spriteAsset.spriteInfoList.Count - 1)
				{
					return this.m_spriteAsset.spriteInfoList[index];
				}
			}
			Debug.LogWarning("Sprite index exceeds the number of sprites in this Sprite Asset.", this);
			return null;
		}

		public int GetSpriteIndexByHashCode(int hashCode)
		{
			if (this.m_spriteAsset == null || this.m_spriteAsset.spriteInfoList == null)
			{
				Debug.LogWarning("No Sprite Asset is assigned.", this);
				return -1;
			}
			return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.hashCode == hashCode);
		}

		public int GetSpriteIndexByIndex(int index)
		{
			if (!(this.m_spriteAsset == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(InlineGraphicManager.GetSpriteIndexByIndex(int)).MethodHandle;
				}
				if (this.m_spriteAsset.spriteInfoList != null)
				{
					return this.m_spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => item.id == index);
				}
			}
			Debug.LogWarning("No Sprite Asset is assigned.", this);
			return -1;
		}

		public void SetUIVertex(UIVertex[] uiVertex)
		{
			this.m_uiVertex = uiVertex;
		}
	}
}
