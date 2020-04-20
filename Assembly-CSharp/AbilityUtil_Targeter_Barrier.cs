﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Barrier : AbilityUtil_Targeter
{
	public float m_width;

	public bool m_snapToBorder;

	private bool m_allowAimAtDiagonals;

	private Vector3 m_lastFreePos = Vector3.zero;

	private bool m_hideIfMovingFast = true;

	protected Vector3 m_barrierOutwardFacing;

	protected Vector3 m_barrierCenterPos;

	protected Vector3 m_barrierDir;

	public AbilityUtil_Targeter_Barrier(Ability ability, float width, bool snapToBorder = false, bool allowAimAtDiagonals = false, bool hideIfMovingFast = true) : base(ability)
	{
		this.m_width = width;
		this.m_snapToBorder = snapToBorder;
		this.m_allowAimAtDiagonals = allowAimAtDiagonals;
		this.m_hideIfMovingFast = hideIfMovingFast;
		this.m_lastFreePos.x = 10000f;
		this.m_lastFreePos.z = 10000f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		AbilityTarget abilityTarget = currentTarget;
		if (currentTargetIndex > 0)
		{
			abilityTarget = targets[0];
		}
		this.m_barrierCenterPos = abilityTarget.FreePos;
		Vector3 vector = this.m_barrierCenterPos - targetingActor.GetTravelBoardSquareWorldPosition();
		bool flag = false;
		bool active = false;
		Vector3 vector2 = this.m_barrierCenterPos;
		if (this.m_snapToBorder)
		{
			if ((this.m_barrierCenterPos - this.m_lastFreePos).magnitude > 0.2f)
			{
				flag = true;
			}
			this.m_lastFreePos = this.m_barrierCenterPos;
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
			if (boardSquareSafe != null)
			{
				active = true;
				vector2 = boardSquareSafe.ToVector3();
				Vector3 freePos = abilityTarget.FreePos;
				if (currentTargetIndex > 0)
				{
					freePos = currentTarget.FreePos;
				}
				Vector3 b;
				vector = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquareSafe, freePos, this.m_allowAimAtDiagonals, out b);
				this.m_barrierCenterPos = vector2 + b;
			}
		}
		vector.y = 0f;
		vector.Normalize();
		this.m_barrierDir = vector;
		this.m_barrierOutwardFacing = -vector;
		float x = this.m_width * Board.Get().squareSize;
		if (base.Highlight == null)
		{
			base.Highlight = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine);
			base.Highlight.transform.localScale = new Vector3(x, 1f, 1f);
		}
		float y = 0.1f;
		base.Highlight.transform.position = this.m_barrierCenterPos + new Vector3(0f, y, 0f);
		base.Highlight.transform.rotation = Quaternion.LookRotation(this.m_barrierOutwardFacing);
		bool flag2;
		if (this.m_hideIfMovingFast)
		{
			flag2 = flag;
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		GameObject highlight = base.Highlight;
		bool active2;
		if (this.m_snapToBorder)
		{
			active2 = !flag3;
		}
		else
		{
			active2 = true;
		}
		highlight.SetActive(active2);
		if (this.m_snapToBorder)
		{
			if (this.m_highlights.Count < 2)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			this.m_highlights[1].transform.position = vector2;
			this.m_highlights[1].SetActive(active);
		}
	}
}
