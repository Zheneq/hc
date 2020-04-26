using UnityEngine;

public class TeleportPathStart : PathStart
{
	public GameObject m_arrowPointTo;

	private void Awake()
	{
		if (!(m_arrowPointTo != null))
		{
			return;
		}
		while (true)
		{
			m_arrowPointTo.SetActive(false);
			return;
		}
	}

	public override void SetColor(Color newColor)
	{
		base.SetColor(newColor);
		if (!(m_arrowPointTo != null))
		{
			return;
		}
		while (true)
		{
			MeshRenderer component = m_arrowPointTo.GetComponent<MeshRenderer>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				if (component.materials.Length <= 0)
				{
					return;
				}
				while (true)
				{
					if (component.materials[0] != null)
					{
						while (true)
						{
							component.materials[0].SetColor("_TintColor", newColor);
							return;
						}
					}
					return;
				}
			}
		}
	}

	public override void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (!(m_arrowPointTo != null))
		{
			return;
		}
		while (true)
		{
			m_arrowPointTo.SetActive(true);
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(startPosition).GetWorldPosition();
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(endPosition).GetWorldPosition();
			Vector3 forward = worldPosition - worldPosition2;
			m_arrowPointTo.transform.forward = forward;
			return;
		}
	}
}
