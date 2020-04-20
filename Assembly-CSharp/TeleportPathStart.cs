using System;
using UnityEngine;

public class TeleportPathStart : PathStart
{
	public GameObject m_arrowPointTo;

	private void Awake()
	{
		if (this.m_arrowPointTo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeleportPathStart.Awake()).MethodHandle;
			}
			this.m_arrowPointTo.SetActive(false);
		}
	}

	public override void SetColor(Color newColor)
	{
		base.SetColor(newColor);
		if (this.m_arrowPointTo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeleportPathStart.SetColor(Color)).MethodHandle;
			}
			MeshRenderer component = this.m_arrowPointTo.GetComponent<MeshRenderer>();
			if (component != null)
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
				if (component.materials.Length > 0)
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
					if (component.materials[0] != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeleportPathStart.AbilityCasted(GridPos, GridPos)).MethodHandle;
			}
			this.m_arrowPointTo.SetActive(true);
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(startPosition).GetWorldPosition();
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(endPosition).GetWorldPosition();
			Vector3 forward = worldPosition - worldPosition2;
			this.m_arrowPointTo.transform.forward = forward;
		}
	}
}
