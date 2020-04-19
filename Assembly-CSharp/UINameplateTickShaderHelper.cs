using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameplateTickShaderHelper : BaseMeshEffect
{
	private float m_tickDivisions;

	private Vector4 m_textureRect;

	protected override void Start()
	{
		base.Start();
		Image component = base.GetComponent<Image>();
		this.m_textureRect.x = component.sprite.textureRect.x;
		this.m_textureRect.y = component.sprite.textureRect.y;
		this.m_textureRect.z = component.sprite.textureRect.width;
		this.m_textureRect.w = component.sprite.textureRect.height;
	}

	internal void SetTickDivisions(float divisions)
	{
		this.m_tickDivisions = Mathf.Min(30f, divisions);
	}

	public override void ModifyMesh(VertexHelper vh)
	{
		if (!this.IsActive())
		{
			return;
		}
		List<UIVertex> stream = new List<UIVertex>(vh.currentVertCount);
		vh.GetUIVertexStream(stream);
		UIVertex vertex = default(UIVertex);
		for (int i = 0; i < vh.currentVertCount; i++)
		{
			vh.PopulateUIVertex(ref vertex, i);
			vertex.uv1.x = this.m_tickDivisions;
			vertex.tangent = this.m_textureRect;
			vh.SetUIVertex(vertex, i);
		}
	}
}
