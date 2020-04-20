using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NekoDisc : AbilityUtil_Targeter_Laser
{
	private float m_aoeRadiusAtEnd;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private SquareInsideChecker_Box m_laserChecker;

	private SquareInsideChecker_Cone m_coneChecker;

	public AbilityUtil_Targeter_NekoDisc(Ability ability, float width, float distance, float aoeRadiusAtEnd, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false) : base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies, affectsCaster)
	{
		this.m_aoeRadiusAtEnd = aoeRadiusAtEnd;
		this.CanEndOnHalfHeightWalls = true;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_laserChecker = new SquareInsideChecker_Box(this.m_width);
		this.m_coneChecker = new SquareInsideChecker_Cone();
		this.m_squarePosCheckerList.Add(this.m_laserChecker);
		this.m_squarePosCheckerList.Add(this.m_coneChecker);
	}

	public AbilityUtil_Targeter_NekoDisc(Ability ability, LaserTargetingInfo laserTargetingInfo) : base(ability, laserTargetingInfo)
	{
		this.CanEndOnHalfHeightWalls = true;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_laserChecker = new SquareInsideChecker_Box(this.m_width);
		this.m_coneChecker = new SquareInsideChecker_Cone();
		this.m_squarePosCheckerList.Add(this.m_laserChecker);
		this.m_squarePosCheckerList.Add(this.m_coneChecker);
	}

	public bool CanEndOnHalfHeightWalls { get; set; }

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (this.m_highlights.Count < 2)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_NekoDisc.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			this.m_highlights.Add(HighlightUtils.Get().CreateAoECursor(this.m_aoeRadiusAtEnd * Board.SquareSizeStatic, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, this.GetDistance(), this.GetWidth(), targetingActor, targetingActor.GetOpposingTeams(), false, this.GetMaxTargets(), false, false, out vector, null, null, false, true);
		Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, vector);
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(vector, this.m_aoeRadiusAtEnd, false, targetingActor, targetingActor.GetOpposingTeam(), null, true, coneLosCheckPos);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
		base.AddActorsInRange(actorsInRadius, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
		BoardSquare discEndSquare = NekoBoomerangDisc.GetDiscEndSquare(travelBoardSquareWorldPositionForLos, vector);
		Vector3 baselineHeight = discEndSquare.GetBaselineHeight();
		baselineHeight.y = HighlightUtils.GetHighlightHeight();
		this.m_highlights[1].transform.position = baselineHeight;
		vector.y = HighlightUtils.GetHighlightHeight();
		this.m_highlights[2].transform.position = vector;
	}

	protected override void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			this.m_laserChecker.UpdateBoxProperties(startPos, endPos, targetingActor);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
			this.m_coneChecker.UpdateConeProperties(endPos, 360f, this.m_aoeRadiusAtEnd, 0f, 0f, targetingActor);
			this.m_coneChecker.SetLosPosOverride(true, coneLosCheckPos, true);
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, this.GetWidth(), targetingActor, this.GetPenetrateLoS(), null, this.m_squarePosCheckerList, true);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, endPos, 0f, 360f, this.m_aoeRadiusAtEnd, 0f, targetingActor, this.GetPenetrateLoS(), this.m_squarePosCheckerList);
			base.HideUnusedSquareIndicators();
		}
	}
}
