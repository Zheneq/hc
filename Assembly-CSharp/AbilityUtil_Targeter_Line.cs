using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Line : AbilityUtil_Targeter
{
	public float m_lineRange = 10f;

	public bool m_linePenetrateLos;

	public AbilityUtil_Targeter_Line(Ability ability, float range, bool penetrateLos)
		: base(ability)
	{
		m_lineRange = range;
		m_linePenetrateLos = penetrateLos;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		float maxDistanceInWorld = m_lineRange * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(targetingActor.GetTravelBoardSquareWorldPositionForLos(), currentTarget.AimDirection, maxDistanceInWorld, m_linePenetrateLos, targetingActor);
		if (m_highlights != null)
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
			if (m_highlights.Count != 0)
			{
				goto IL_00b9;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
		m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		goto IL_00b9;
		IL_00b9:
		float magnitude = (laserEndPoint - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude;
		Vector3 position = targetingActor.GetTravelBoardSquareWorldPosition() + new Vector3(0f, 0.1f, 0f);
		using (List<GameObject>.Enumerator enumerator = m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				HighlightUtils.Get().ResizeBoundaryLine(magnitude / Board.Get().squareSize, current);
				current.transform.position = position;
				current.transform.rotation = Quaternion.LookRotation(-currentTarget.AimDirection);
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
