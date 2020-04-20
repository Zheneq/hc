using System;
using UnityEngine;

public class NPCLocation : MonoBehaviour
{
	private BoardSquare m_boardSquare;

	public BoardSquare boardSquare
	{
		get
		{
			if (this.m_boardSquare == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NPCLocation.get_boardSquare()).MethodHandle;
				}
				this.m_boardSquare = Board.Get().GetBoardSquareSafe(base.transform.position.x, base.transform.position.z);
			}
			return this.m_boardSquare;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCLocation.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_NPC.png");
	}
}
