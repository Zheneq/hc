using System;
using UnityEngine;

public class TeleportPathEnd : PathEnd
{
	public GameObject m_arrowPointTo;

	public Animator m_animController;

	public override void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (this.m_arrowPointTo != null)
		{
			this.m_arrowPointTo.SetActive(true);
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(startPosition).GetWorldPosition();
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(endPosition).GetWorldPosition();
			Vector3 forward = worldPosition2 - worldPosition;
			this.m_arrowPointTo.transform.forward = forward;
		}
	}

	public override void Setup()
	{
		this.m_animController.Play("Initial");
	}
}
