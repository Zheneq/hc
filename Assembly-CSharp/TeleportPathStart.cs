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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			MeshRenderer component = m_arrowPointTo.GetComponent<MeshRenderer>();
			if (!(component != null))
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (component.materials.Length <= 0)
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
					if (component.materials[0] != null)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_arrowPointTo.SetActive(true);
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(startPosition).GetWorldPosition();
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(endPosition).GetWorldPosition();
			Vector3 forward = worldPosition - worldPosition2;
			m_arrowPointTo.transform.forward = forward;
			return;
		}
	}
}
