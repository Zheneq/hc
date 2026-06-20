// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreKnockbackLaser : AbilityUtil_Targeter
{
	private float m_laserWidth = 1f;
	private float m_laserRange = 15f;
	private bool m_penetrateLos;
	private bool m_lengthIgnoreGeo;
	private int m_maxTargets = -1;

	private float m_laserMiddleWidth;
	private float m_knockbackDistance;
	private KnockbackType m_knockbackType;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ClaymoreKnockbackLaser(
		Ability ability,
		float laserWidth,
		float distance,
		bool penetrateLos,
		bool lengthLgnoreGeo,
		int maxTargets,
		float laserMiddleWidth,
		float knockbackDistance,
		KnockbackType knockbackType)
		: base(ability)
	{
		m_laserWidth = laserWidth;
		m_laserRange = distance;
		m_penetrateLos = penetrateLos;
		m_lengthIgnoreGeo = lengthLgnoreGeo;
		m_maxTargets = maxTargets;
		m_laserMiddleWidth = laserMiddleWidth;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public float GetLaserRange()
	{
		return m_laserRange;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float laserWidthInWorld = m_laserWidth * Board.Get().squareSize;
		float laserMiddleWidthInWorld = m_laserMiddleWidth * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		ClearActorsInRange();
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			currentTarget.AimDirection,
			m_laserRange,
			m_laserWidth,
			targetingActor,
#if VANILLA
			targetingActor.GetEnemyTeamAsList(), // reactor
#else
			targetingActor.GetOtherTeams(), // rogues
#endif
			m_penetrateLos,
			m_maxTargets,
			m_lengthIgnoreGeo,
			false,
			out laserCoords.end,
			null);
		List<ActorData> actorsInMiddleLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			currentTarget.AimDirection,
			m_laserRange,
			m_laserMiddleWidth,
			targetingActor,
#if VANILLA
			targetingActor.GetEnemyTeamAsList(), // reactor
#else
			targetingActor.GetOtherTeams(), // rogues
#endif
			m_penetrateLos,
			m_maxTargets,
			m_lengthIgnoreGeo,
			false,
			out laserCoords.end,
			null);
		foreach (ActorData actor in actorsInMiddleLaser)
		{
			AddActorInRange(actor, laserCoords.start, targetingActor);
			// rogues
			// m_actorContextVars[actor].m_contextVars.SetValue(TargetSelect_LaserNested.s_InPrimaryRadius.GetKey(), 1);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		Vector3 casterPos = targetingActor.GetFreePos();
		foreach (ActorData item in actorsInLaser)
		{
			if (!actorsInMiddleLaser.Contains(item))
			{
				AddActorInRange(item, laserCoords.start, targetingActor, AbilityTooltipSubject.Secondary);
				// rogues
				// m_actorContextVars[item].m_contextVars.SetValue(TargetSelect_LaserNested.s_InPrimaryRadius.GetKey(), 0);
				if (targetingActor.TechPoints + targetingActor.ReservedTechPoints >= targetingActor.GetMaxTechPoints())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
						item,
						m_knockbackType,
						currentTarget.AimDirection,
						casterPos,
						m_knockbackDistance);
					arrowIndex = AddMovementArrowWithPrevious(item, path, TargeterMovementType.Knockback, arrowIndex);
				}
			}
		}
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
		}
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		if (m_highlights == null || m_highlights.Count < 2)
		{
			m_highlights = new List<GameObject>
			{
				HighlightUtils.Get().CreateRectangularCursor(laserWidthInWorld, 1f),
				HighlightUtils.Get().CreateRectangularCursor(laserMiddleWidthInWorld, 1f)
			};
		}
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		for (int i = 0; i < m_highlights.Count; i++)
		{
			float widthInWorld = i == 0 ? laserWidthInWorld : laserMiddleWidthInWorld;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, m_highlights[i]);
			m_highlights[i].transform.position = laserCoords.start + new Vector3(0f, y, 0f);
			m_highlights[i].transform.rotation = Quaternion.LookRotation(normalized);
		}
		DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			// reactor
			float widthInSquares = Mathf.Max(m_laserWidth, m_laserMiddleWidth);
			// rogues
			// float widthInSquares = m_laserWidth;

			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, widthInSquares, targetingActor, m_penetrateLos);
			HideUnusedSquareIndicators();
		}
	}
}
