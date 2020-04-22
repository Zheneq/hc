using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserMultiple : AbilityUtil_Targeter
{
	protected int m_laserCount;

	protected float m_angleInBetween;

	protected LaserTargetingInfo m_laserInfo;

	public AbilityUtil_Targeter_LaserMultiple(Ability ability, LaserTargetingInfo laserTargetingInfo, int laserCount, float angleInBetween)
		: base(ability)
	{
		m_laserCount = laserCount;
		m_angleInBetween = angleInBetween;
		m_laserInfo = laserTargetingInfo;
		SetAffectedGroups(m_laserInfo.affectsEnemies, m_laserInfo.affectsAllies, m_laserInfo.affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_laserCount < 0)
		{
			return;
		}
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_laserCount)
			{
				goto IL_0084;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_laserCount; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		}
		goto IL_0084;
		IL_0084:
		ClearActorsInRange();
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		float widthInWorld = m_laserInfo.width * Board.Get().squareSize;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		float num2 = num - 0.5f * (float)(m_laserCount - 1) * m_angleInBetween;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		for (int j = 0; j < m_laserCount; j++)
		{
			Vector3 vector = VectorUtils.AngleDegreesToVector(num2 + (float)j * m_angleInBetween);
			laserCoords.start = travelBoardSquareWorldPositionForLos;
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_laserInfo.affectsAllies, m_laserInfo.affectsEnemies);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector, m_laserInfo.range, m_laserInfo.width, targetingActor, relevantTeams, m_laserInfo.penetrateLos, m_laserInfo.maxTargets, false, false, out laserCoords.end, null);
			VectorUtils.LaserCoords laserCoords2 = laserCoords;
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, laserCoords2.start, targetingActor, AbilityTooltipSubject.Primary, true);
				}
			}
			if (m_affectsTargetingActor)
			{
				AddActorInRange(targetingActor, laserCoords2.start, targetingActor);
			}
			float magnitude = (laserCoords2.end - laserCoords2.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, m_highlights[j]);
			m_highlights[j].transform.position = laserCoords2.start + new Vector3(0f, y, 0f);
			m_highlights[j].transform.rotation = Quaternion.LookRotation(vector);
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
