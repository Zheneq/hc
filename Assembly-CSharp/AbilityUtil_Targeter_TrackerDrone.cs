using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TrackerDrone : AbilityUtil_Targeter
{
	protected TrackerDroneTrackerComponent m_droneTrackerComponent;
	protected float m_radiusAroundStart = 2f;
	protected float m_radiusAroundEnd = 2f;
	protected float m_rangeFromLine = 2f;
	protected bool m_addTargets = true;
	protected bool m_penetrateLoS;
	protected bool m_hitUntracked;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_TrackerDrone(Ability ability, TrackerDroneTrackerComponent droneTracker, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, bool addTargets, bool hitUntrackedTargets)
		: base(ability)
	{
		m_droneTrackerComponent = droneTracker;
		m_radiusAroundStart = radiusAroundStart;
		m_radiusAroundEnd = radiusAroundEnd;
		m_rangeFromLine = rangeFromDir;
		m_addTargets = addTargets;
		m_penetrateLoS = penetrateLoS;
		m_hitUntracked = hitUntrackedTargets;
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
	}

	protected AbilityUtil_Targeter_TrackerDrone(Ability ability)
		: base(ability)
	{
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
	}

	public int GetNumHighlights()
	{
		return 3;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		if (targetSquare == null)
		{
			return;
		}
		Vector3 end = targetSquare.ToVector3();
		Vector3 start = targetingActor.GetFreePos();
		if (m_droneTrackerComponent != null && m_droneTrackerComponent.DroneIsActive())
		{
			BoardSquare droneSquare = Board.Get().GetSquareFromIndex(m_droneTrackerComponent.BoardX(), m_droneTrackerComponent.BoardY());
			start = droneSquare.ToVector3();
		}
		float startRadius = m_droneTrackerComponent != null && m_droneTrackerComponent.DroneIsActive()
			? m_radiusAroundStart
			: 0f;
		if (m_addTargets)
		{
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(start, end, startRadius, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor, null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			foreach (ActorData current in actors)
			{
				bool isPrimaryTarget = m_droneTrackerComponent != null && m_droneTrackerComponent.IsTrackingActor(current.ActorIndex);
				if (current.GetTeam() != targetingActor.GetTeam()
					&& (m_hitUntracked || isPrimaryTarget))
				{
					AddActorInRange(current, start, targetingActor, isPrimaryTarget ? AbilityTooltipSubject.Primary : AbilityTooltipSubject.Secondary);
				}
			}
		}
		float widthInSquares = m_rangeFromLine * 2f;
		if (m_highlights == null || m_highlights.Count < GetNumHighlights())
		{
			m_highlights = new List<GameObject>
			{
				TargeterUtils.CreateLaserBoxHighlight(start, end, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight),
				TargeterUtils.CreateCircleHighlight(start, m_radiusAroundStart, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData),
				TargeterUtils.CreateCircleHighlight(end, m_radiusAroundEnd, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData)
			};
		}
		Vector3 highlightStart = start;
		highlightStart.y = HighlightUtils.GetHighlightHeight();
		Vector3 highlightEnd = end;
		highlightEnd.y = HighlightUtils.GetHighlightHeight();
		if (m_rangeFromLine > 0f && start != end)
		{
			m_highlights[0].SetActive(true);
			TargeterUtils.RefreshLaserBoxHighlight(m_highlights[0], highlightStart, highlightEnd, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			m_highlights[0].SetActive(false);
		}
		if (startRadius > 0f)
		{
			m_highlights[1].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(m_highlights[1], highlightStart, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			m_highlights[1].SetActive(false);
		}
		if (m_radiusAroundEnd > 0f)
		{
			m_highlights[2].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(m_highlights[2], highlightEnd, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			m_highlights[2].SetActive(false);
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(m_indicatorHandler, start, end, startRadius, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor);
			HideUnusedSquareIndicators();
		}
	}
}
