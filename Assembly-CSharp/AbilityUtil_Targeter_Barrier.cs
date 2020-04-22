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

	public AbilityUtil_Targeter_Barrier(Ability ability, float width, bool snapToBorder = false, bool allowAimAtDiagonals = false, bool hideIfMovingFast = true)
		: base(ability)
	{
		m_width = width;
		m_snapToBorder = snapToBorder;
		m_allowAimAtDiagonals = allowAimAtDiagonals;
		m_hideIfMovingFast = hideIfMovingFast;
		m_lastFreePos.x = 10000f;
		m_lastFreePos.z = 10000f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		AbilityTarget abilityTarget = currentTarget;
		if (currentTargetIndex > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			abilityTarget = targets[0];
		}
		m_barrierCenterPos = abilityTarget.FreePos;
		Vector3 vector = m_barrierCenterPos - targetingActor.GetTravelBoardSquareWorldPosition();
		bool flag = false;
		bool active = false;
		Vector3 vector2 = m_barrierCenterPos;
		if (m_snapToBorder)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if ((m_barrierCenterPos - m_lastFreePos).magnitude > 0.2f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = true;
			}
			m_lastFreePos = m_barrierCenterPos;
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
			if (boardSquareSafe != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				active = true;
				vector2 = boardSquareSafe.ToVector3();
				Vector3 freePos = abilityTarget.FreePos;
				if (currentTargetIndex > 0)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					freePos = currentTarget.FreePos;
				}
				vector = VectorUtils.GetDirectionAndOffsetToClosestSide(boardSquareSafe, freePos, m_allowAimAtDiagonals, out Vector3 offset);
				m_barrierCenterPos = vector2 + offset;
			}
		}
		vector.y = 0f;
		vector.Normalize();
		m_barrierDir = vector;
		m_barrierOutwardFacing = -vector;
		float x = m_width * Board.Get().squareSize;
		if (base.Highlight == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			base.Highlight = Object.Instantiate(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine);
			base.Highlight.transform.localScale = new Vector3(x, 1f, 1f);
		}
		float y = 0.1f;
		base.Highlight.transform.position = m_barrierCenterPos + new Vector3(0f, y, 0f);
		base.Highlight.transform.rotation = Quaternion.LookRotation(m_barrierOutwardFacing);
		int num;
		if (m_hideIfMovingFast)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = (flag ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		GameObject highlight = base.Highlight;
		int active2;
		if (m_snapToBorder)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			active2 = ((!flag2) ? 1 : 0);
		}
		else
		{
			active2 = 1;
		}
		highlight.SetActive((byte)active2 != 0);
		if (!m_snapToBorder)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (m_highlights.Count < 2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			m_highlights[1].transform.position = vector2;
			m_highlights[1].SetActive(active);
			return;
		}
	}
}
