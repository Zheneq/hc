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
			Vector3 b = Board.\u000E().\u000E(startPosition).\u001D();
			Vector3 a = Board.\u000E().\u000E(endPosition).\u001D();
			Vector3 forward = a - b;
			this.m_arrowPointTo.transform.forward = forward;
		}
	}

	public override void Setup()
	{
		this.m_animController.Play("Initial");
	}
}
