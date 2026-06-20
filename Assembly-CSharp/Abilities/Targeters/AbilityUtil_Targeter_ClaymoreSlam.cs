// ROGUES
// SERVER
using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreSlam : AbilityUtil_Targeter
{
	public float m_laserRange;
	
	private float m_laserWidth;
	private int m_laserMaxTargets;
	private float m_coneAngleDegrees;
	private float m_coneLengthRadius;
	private bool m_penetrateLos;
	private float m_coneBackwardOffsetInSquares;
	private bool m_appendTooltipForDuplicates;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_ClaymoreSlam(
		Ability ability,
		float laserRange,
		float laserWidth,
		int laserMaxTargets,
		float coneAngleDegrees,
		float coneLengthRadius,
		float coneBackwardOffsetInSquares,
		bool penetrateLos,
		bool affectEnemies = true,
		bool affectAllies = false,
		bool affectCaster = false,
		bool appendTooltipForDuplicates = false)
		: base(ability)
	{
		m_laserRange = laserRange;
		m_laserWidth = laserWidth;
		m_laserMaxTargets = laserMaxTargets;
		m_coneAngleDegrees = coneAngleDegrees;
		m_coneLengthRadius = coneLengthRadius;
		m_penetrateLos = penetrateLos;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_affectsEnemies = affectEnemies;
		m_affectsAllies = affectAllies;
		m_affectsTargetingActor = affectCaster;
		m_appendTooltipForDuplicates = appendTooltipForDuplicates;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_laserWidth));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		AllocateHighlights();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		Vector3 aimDirection = currentTarget.AimDirection;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			aimDirection,
			m_laserRange,
			m_laserWidth,
			targetingActor,
			relevantTeams,
			m_penetrateLos,
			m_laserMaxTargets,
			false,
			false,
			out laserCoords.end,
			null);
		VectorUtils.LaserCoords laserCoords2 = laserCoords;
		if (m_affectsTargetingActor && !actorsInLaser.Contains(targetingActor))
		{
			AddActorInRange(targetingActor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Self);
		}
		
		// removed in rogues
		int num = 0;
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
		float squareSizeStatic = Board.SquareSizeStatic;
		// end removed in rogues
		
		foreach (ActorData current in actorsInLaser)
		{
			if (ShouldAddActor(current, targetingActor))
			{
				// removed in rogues
				float value = (current.GetFreePos() - travelBoardSquareWorldPosition).magnitude / squareSizeStatic;
				// end removed in rogues
				
				AddActorInRange(current, travelBoardSquareWorldPositionForLos, targetingActor);
				
				// removed in rogues
				ActorHitContext actorHitContext = m_actorContextVars[current];
				actorHitContext.m_hitOrigin = laserCoords2.start;
				actorHitContext.m_contextVars.SetValue(ContextKeys.s_HitOrder.GetKey(), num);
				actorHitContext.m_contextVars.SetValue(ContextKeys.s_DistFromStart.GetKey(), value);
				num++;
				// end removed in rogues
			}
		}
		float aimAngle = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(
			travelBoardSquareWorldPositionForLos,
			aimAngle,
			m_coneAngleDegrees,
			m_coneLengthRadius,
			m_coneBackwardOffsetInSquares,
			m_penetrateLos,
			targetingActor,
#if VANILLA
			targetingActor.GetEnemyTeam(), // reactor
#else
			targetingActor.GetOtherTeams(), // rogues
#endif
			null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		foreach (ActorData actor in actors)
		{
			if (ShouldAddActor(actor, targetingActor))
			{
				AddActorInRange(
					actor,
					travelBoardSquareWorldPositionForLos,
					targetingActor,
					AbilityTooltipSubject.Secondary,
					m_appendTooltipForDuplicates);
				
				// removed in rogues
				if (!actorsInLaser.Contains(actor) && actor != targetingActor)
				{
					float value2 = (actor.GetFreePos() - travelBoardSquareWorldPosition).magnitude / squareSizeStatic;
					ActorHitContext actorHitContext2 = m_actorContextVars[actor];
					actorHitContext2.m_hitOrigin = laserCoords2.start;
					actorHitContext2.m_contextVars.SetValue(ContextKeys.s_DistFromStart.GetKey(), value2);
				}
				// end removed in rogues
			}
		}
		GameObject gameObject = m_highlights[0];
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = travelBoardSquareWorldPositionForLos + new Vector3(0f, y, 0f) - aimDirection * d;
		gameObject.transform.position = position;
		gameObject.transform.rotation = Quaternion.LookRotation(aimDirection);
		HighlightUtils.Get().RotateAndResizeRectangularCursor(m_highlights[1], travelBoardSquareWorldPositionForLos, laserCoords2.end, m_laserWidth);
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[0] as SquareInsideChecker_Box;
			SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[1] as SquareInsideChecker_Cone;
			squareInsideChecker_Box.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, laserCoords2.end, targetingActor);
			squareInsideChecker_Cone.UpdateConeProperties(
				travelBoardSquareWorldPositionForLos,
				m_coneAngleDegrees,
				m_coneLengthRadius,
				m_coneBackwardOffsetInSquares,
				aimAngle,
				targetingActor
				// added in rogues
				// , m_penetrateLos
				);
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				m_indicatorHandler,
				travelBoardSquareWorldPositionForLos,
				laserCoords2.end,
				m_laserWidth,
				targetingActor,
				m_penetrateLos,
				null,
				m_squarePosCheckerList);
			AreaEffectUtils.OperateOnSquaresInCone(
				m_indicatorHandler,
				travelBoardSquareWorldPositionForLos,
				aimAngle,
				m_coneAngleDegrees,
				m_coneLengthRadius,
				m_coneBackwardOffsetInSquares,
				targetingActor,
				m_penetrateLos,
				m_squarePosCheckerList);
			HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		if (actor == caster)
		{
			return m_affectsTargetingActor;
		}
		if (actor.GetTeam() == caster.GetTeam() && m_affectsAllies)
		{
			return true;
		}
		if (actor.GetTeam() != caster.GetTeam() && m_affectsEnemies)
		{
			return true;
		}
		return false;
	}

	private void AllocateHighlights()
	{
		if (m_highlights != null && m_highlights.Count >= 2)
		{
			return;
		}
		m_highlights = new List<GameObject>();
		float radiusInWorld = (m_coneLengthRadius + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
		m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneAngleDegrees));
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(m_laserWidth, 1f));
	}
}
