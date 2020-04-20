using System;
using UnityEngine;

public class TeleportPathStart : PathStart
{
	public GameObject m_arrowPointTo;

	private void Awake()
	{
		if (this.m_arrowPointTo != null)
		{
			this.m_arrowPointTo.SetActive(false);
		}
	}

	public override void SetColor(Color newColor)
	{
		base.SetColor(newColor);
		if (this.m_arrowPointTo != null)
		{
			MeshRenderer component = this.m_arrowPointTo.GetComponent<MeshRenderer>();
			if (component != null)
			{
				if (component.materials.Length > 0)
				{
					if (component.materials[0] != null)
					{
						component.materials[0].SetColor("_TintColor", newColor);
					}
				}
			}
		}
	}

	public override void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (this.m_arrowPointTo != null)
		{
			this.m_arrowPointTo.SetActive(true);
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(startPosition).GetWorldPosition();
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(endPosition).GetWorldPosition();
			Vector3 forward = worldPosition - worldPosition2;
			this.m_arrowPointTo.transform.forward = forward;
		}
	}
}
