using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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

	public AbilityUtil_Targeter_ClaymoreSlam(Ability ability, float laserRange, float laserWidth, int laserMaxTargets, float coneAngleDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool penetrateLos, bool affectEnemies = true, bool affectAllies = false, bool affectCaster = false, bool appendTooltipForDuplicates = false) : base(ability)
	{
		this.m_laserRange = laserRange;
		this.m_laserWidth = laserWidth;
		this.m_laserMaxTargets = laserMaxTargets;
		this.m_coneAngleDegrees = coneAngleDegrees;
		this.m_coneLengthRadius = coneLengthRadius;
		this.m_penetrateLos = penetrateLos;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_affectsEnemies = affectEnemies;
		this.m_affectsAllies = affectAllies;
		this.m_affectsTargetingActor = affectCaster;
		this.m_appendTooltipForDuplicates = appendTooltipForDuplicates;
		bool shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		}
		else
		{
			shouldShowActorRadius = true;
		}
		this.m_shouldShowActorRadius = shouldShowActorRadius;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_laserWidth));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.AllocateHighlights();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget.AimDirection;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, aimDirection, this.m_laserRange, this.m_laserWidth, targetingActor, relevantTeams, this.m_penetrateLos, this.m_laserMaxTargets, false, false, out laserCoords.end, null, null, false, true);
		VectorUtils.LaserCoords laserCoords2 = laserCoords;
		if (this.m_affectsTargetingActor)
		{
			if (!actorsInLaser.Contains(targetingActor))
			{
				base.AddActorInRange(targetingActor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Self, false);
			}
		}
		int num = 0;
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		float squareSizeStatic = Board.SquareSizeStatic;
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (this.ShouldAddActor(actorData, targetingActor))
				{
					float value = (actorData.GetTravelBoardSquareWorldPosition() - travelBoardSquareWorldPosition).magnitude / squareSizeStatic;
					base.AddActorInRange(actorData, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
					ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
					actorHitContext.symbol_001D = laserCoords2.start;
					actorHitContext.symbol_0015.SetInt(ContextKeys.symbol_0011.GetHash(), num);
					actorHitContext.symbol_0015.SetFloat(ContextKeys.symbol_0018.GetHash(), value);
					num++;
				}
			}
		}
		float num2 = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, num2, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, this.m_penetrateLos, targetingActor, targetingActor.GetOpposingTeam(), null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		using (List<ActorData>.Enumerator enumerator2 = actorsInCone.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ActorData actorData2 = enumerator2.Current;
				if (this.ShouldAddActor(actorData2, targetingActor))
				{
					base.AddActorInRange(actorData2, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Secondary, this.m_appendTooltipForDuplicates);
					if (!actorsInLaser.Contains(actorData2))
					{
						if (actorData2 != targetingActor)
						{
							float value2 = (actorData2.GetTravelBoardSquareWorldPosition() - travelBoardSquareWorldPosition).magnitude / squareSizeStatic;
							ActorHitContext actorHitContext2 = this.m_actorContextVars[actorData2];
							actorHitContext2.symbol_001D = laserCoords2.start;
							actorHitContext2.symbol_0015.SetFloat(ContextKeys.symbol_0018.GetHash(), value2);
						}
					}
				}
			}
		}
		GameObject gameObject = this.m_highlights[0];
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = travelBoardSquareWorldPositionForLos + new Vector3(0f, y, 0f) - aimDirection * d;
		gameObject.transform.position = position;
		gameObject.transform.rotation = Quaternion.LookRotation(aimDirection);
		HighlightUtils.Get().RotateAndResizeRectangularCursor(this.m_highlights[1], travelBoardSquareWorldPositionForLos, laserCoords2.end, this.m_laserWidth);
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[0] as SquareInsideChecker_Box;
			SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[1] as SquareInsideChecker_Cone;
			squareInsideChecker_Box.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, laserCoords2.end, targetingActor);
			squareInsideChecker_Cone.UpdateConeProperties(travelBoardSquareWorldPositionForLos, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, num2, targetingActor);
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, laserCoords2.end, this.m_laserWidth, targetingActor, this.m_penetrateLos, null, this.m_squarePosCheckerList, true);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, num2, this.m_coneAngleDegrees, this.m_coneLengthRadius, this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLos, this.m_squarePosCheckerList);
			base.HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = this.m_affectsTargetingActor;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (this.m_affectsAllies)
				{
					return true;
				}
			}
			if (actor.GetTeam() != caster.GetTeam() && this.m_affectsEnemies)
			{
				result = true;
			}
		}
		return result;
	}

	private void AllocateHighlights()
	{
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				return;
			}
		}
		this.m_highlights = new List<GameObject>();
		float radiusInWorld = (this.m_coneLengthRadius + this.m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
		this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneAngleDegrees));
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(this.m_laserWidth, 1f, null));
	}
}
