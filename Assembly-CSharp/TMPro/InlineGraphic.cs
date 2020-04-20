using System;
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
				if (this.texture == null)
				{
					return Graphic.s_WhiteTexture;
				}
				return this.texture;
			}
		}

		protected override void Awake()
		{
			this.m_manager = base.GetComponentInParent<InlineGraphicManager>();
		}

		protected override void OnEnable()
		{
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_manager != null)
			{
				if (this.m_manager.spriteAsset != null)
				{
					this.texture = this.m_manager.spriteAsset.spriteSheet;
				}
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
			if (this.m_RectTransform == null)
			{
				this.m_RectTransform = base.gameObject.GetComponent<RectTransform>();
			}
			if (this.m_ParentRectTransform == null)
			{
				this.m_ParentRectTransform = this.m_RectTransform.parent.GetComponent<RectTransform>();
			}
			if (this.m_RectTransform.pivot != this.m_ParentRectTransform.pivot)
			{
				this.m_RectTransform.pivot = this.m_ParentRectTransform.pivot;
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
