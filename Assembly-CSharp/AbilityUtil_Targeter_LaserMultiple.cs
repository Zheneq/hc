using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserMultiple : AbilityUtil_Targeter
{
	protected int m_laserCount;

	protected float m_angleInBetween;

	protected LaserTargetingInfo m_laserInfo;

	public AbilityUtil_Targeter_LaserMultiple(Ability ability, LaserTargetingInfo laserTargetingInfo, int laserCount, float angleInBetween) : base(ability)
	{
		this.m_laserCount = laserCount;
		this.m_angleInBetween = angleInBetween;
		this.m_laserInfo = laserTargetingInfo;
		base.SetAffectedGroups(this.m_laserInfo.affectsEnemies, this.m_laserInfo.affectsAllies, this.m_laserInfo.affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_laserCount < 0)
		{
			return;
		}
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_laserCount)
			{
				goto IL_84;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_laserCount; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		}
		IL_84:
		base.ClearActorsInRange();
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		float widthInWorld = this.m_laserInfo.width * Board.Get().squareSize;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float num = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
		float num2 = num - 0.5f * (float)(this.m_laserCount - 1) * this.m_angleInBetween;
		for (int j = 0; j < this.m_laserCount; j++)
		{
			Vector3 vector = VectorUtils.AngleDegreesToVector(num2 + (float)j * this.m_angleInBetween);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = travelBoardSquareWorldPositionForLos;
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_laserInfo.affectsAllies, this.m_laserInfo.affectsEnemies);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector, this.m_laserInfo.range, this.m_laserInfo.width, targetingActor, relevantTeams, this.m_laserInfo.penetrateLos, this.m_laserInfo.maxTargets, false, false, out laserCoords.end, null, null, false, true);
			VectorUtils.LaserCoords laserCoords2 = laserCoords;
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, laserCoords2.start, targetingActor, AbilityTooltipSubject.Primary, true);
				}
			}
			if (this.m_affectsTargetingActor)
			{
				base.AddActorInRange(targetingActor, laserCoords2.start, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			float magnitude = (laserCoords2.end - laserCoords2.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, this.m_highlights[j]);
			this.m_highlights[j].transform.position = laserCoords2.start + new Vector3(0f, y, 0f);
			this.m_highlights[j].transform.rotation = Quaternion.LookRotation(vector);
		}
	}
}
