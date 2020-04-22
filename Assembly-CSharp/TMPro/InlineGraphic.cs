using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	public class InlineGraphic : MaskableGraphic
	{
		public Texture texture;

		private InlineGraphicManager m_manager;

		private RectTransform m_RectTransform;

		private RectTransform m_ParentRectTransform;

		public override Texture mainTexture
		{
			get
			{
				if (texture == null)
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
							return Graphic.s_WhiteTexture;
						}
					}
				}
				return texture;
			}
		}

		protected override void Awake()
		{
			m_manager = GetComponentInParent<InlineGraphicManager>();
		}

		protected override void OnEnable()
		{
			if (m_RectTransform == null)
			{
				m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (!(m_manager != null))
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
				if (m_manager.spriteAsset != null)
				{
					texture = m_manager.spriteAsset.spriteSheet;
				}
				return;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
		}

		protected override void OnTransformParentChanged()
		{
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (m_RectTransform == null)
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
				m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (m_ParentRectTransform == null)
			{
				m_ParentRectTransform = m_RectTransform.parent.GetComponent<RectTransform>();
			}
			if (!(m_RectTransform.pivot != m_ParentRectTransform.pivot))
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
				m_RectTransform.pivot = m_ParentRectTransform.pivot;
				return;
			}
		}

		public new void UpdateMaterial()
		{
			base.UpdateMaterial();
		}

		protected override void UpdateGeometry()
		{
		}
	}
}
