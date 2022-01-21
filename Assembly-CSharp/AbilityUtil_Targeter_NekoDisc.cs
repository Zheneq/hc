using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NekoDisc : AbilityUtil_Targeter_Laser
{
	private float m_aoeRadiusAtEnd;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private SquareInsideChecker_Box m_laserChecker;

	private SquareInsideChecker_Cone m_coneChecker;

	public bool CanEndOnHalfHeightWalls
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_NekoDisc(Ability ability, float width, float distance, float aoeRadiusAtEnd, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false)
		: base(ability, width, distance, penetrateLoS, maxTargets, affectsAllies, affectsCaster)
	{
		m_aoeRadiusAtEnd = aoeRadiusAtEnd;
		CanEndOnHalfHeightWalls = true;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_laserChecker = new SquareInsideChecker_Box(m_width);
		m_coneChecker = new SquareInsideChecker_Cone();
		m_squarePosCheckerList.Add(m_laserChecker);
		m_squarePosCheckerList.Add(m_coneChecker);
	}

	public AbilityUtil_Targeter_NekoDisc(Ability ability, LaserTargetingInfo laserTargetingInfo)
		: base(ability, laserTargetingInfo)
	{
		CanEndOnHalfHeightWalls = true;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_laserChecker = new SquareInsideChecker_Box(m_width);
		m_coneChecker = new SquareInsideChecker_Cone();
		m_squarePosCheckerList.Add(m_laserChecker);
		m_squarePosCheckerList.Add(m_coneChecker);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (m_highlights.Count < 2)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(HighlightUtils.Get().CreateAoECursor(m_aoeRadiusAtEnd * Board.SquareSizeStatic, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, GetDistance(), GetWidth(), targetingActor, targetingActor.GetEnemyTeams(), false, GetMaxTargets(), false, false, out Vector3 laserEndPos, null);
		Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, laserEndPos);
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(laserEndPos, m_aoeRadiusAtEnd, false, targetingActor, targetingActor.GetEnemyTeam(), null, true, coneLosCheckPos);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		AddActorsInRange(actors, travelBoardSquareWorldPositionForLos, targetingActor);
		BoardSquare discEndSquare = NekoBoomerangDisc.GetDiscEndSquare(travelBoardSquareWorldPositionForLos, laserEndPos);
		Vector3 baselineHeight = discEndSquare.GetPosAtBaselineHeight();
		baselineHeight.y = HighlightUtils.GetHighlightHeight();
		m_highlights[1].transform.position = baselineHeight;
		laserEndPos.y = HighlightUtils.GetHighlightHeight();
		m_highlights[2].transform.position = laserEndPos;
	}

	protected override void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			m_laserChecker.UpdateBoxProperties(startPos, endPos, targetingActor);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
			m_coneChecker.UpdateConeProperties(endPos, 360f, m_aoeRadiusAtEnd, 0f, 0f, targetingActor);
			m_coneChecker.SetLosPosOverride(true, coneLosCheckPos, true);
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, GetWidth(), targetingActor, GetPenetrateLoS(), null, m_squarePosCheckerList);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, endPos, 0f, 360f, m_aoeRadiusAtEnd, 0f, targetingActor, GetPenetrateLoS(), m_squarePosCheckerList);
			HideUnusedSquareIndicators();
		}
	}
}
