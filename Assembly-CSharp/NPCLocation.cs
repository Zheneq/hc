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
				this.m_boardSquare = Board.Get().GetBoardSquareSafe(base.transform.position.x, base.transform.position.z);
			}
			return this.m_boardSquare;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_NPC.png");
	}
}
