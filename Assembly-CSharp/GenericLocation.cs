using System;
using UnityEngine;

public class GenericLocation : MonoBehaviour
{
	private BoardSquare m_boardSquare;

	public BoardSquare boardSquare
	{
		get
		{
			return this.m_boardSquare;
		}
	}

	public void Initialize()
	{
		this.m_boardSquare = Board.Get().GetBoardSquareSafe(base.transform.position.x, base.transform.position.z);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "locationIcon.png");
	}
}
