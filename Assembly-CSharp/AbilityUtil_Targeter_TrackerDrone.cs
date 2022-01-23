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
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
	}

	protected AbilityUtil_Targeter_TrackerDrone(Ability ability)
		: base(ability)
	{
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
	}

	public int GetNumHighlights()
	{
		return 3;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (boardSquareSafe == null)
		{
			while (true)
			{
				return;
			}
		}
		Vector3 vector = boardSquareSafe.ToVector3();
		Vector3 vector2 = targetingActor.GetFreePos();
		if (m_droneTrackerComponent != null && m_droneTrackerComponent.DroneIsActive())
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(m_droneTrackerComponent.BoardX(), m_droneTrackerComponent.BoardY());
			vector2 = boardSquare.ToVector3();
		}
		float num;
		if (m_droneTrackerComponent != null && m_droneTrackerComponent.DroneIsActive())
		{
			num = m_radiusAroundStart;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		if (m_addTargets)
		{
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(vector2, vector, num2, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor, null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					int num3;
					if (m_droneTrackerComponent != null)
					{
						num3 = (m_droneTrackerComponent.IsTrackingActor(current.ActorIndex) ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					bool flag = (byte)num3 != 0;
					if (current.GetTeam() != targetingActor.GetTeam())
					{
						if (!m_hitUntracked)
						{
							if (!flag)
							{
								continue;
							}
						}
						Vector3 damageOrigin = vector2;
						int subjectType;
						if (flag)
						{
							subjectType = 1;
						}
						else
						{
							subjectType = 2;
						}
						AddActorInRange(current, damageOrigin, targetingActor, (AbilityTooltipSubject)subjectType);
					}
				}
			}
		}
		bool flag2 = m_rangeFromLine > 0f;
		bool flag3 = num2 > 0f;
		bool flag4 = m_radiusAroundEnd > 0f;
		int index = 0;
		int index2 = 1;
		int index3 = 2;
		float widthInSquares = m_rangeFromLine * 2f;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= GetNumHighlights())
			{
				goto IL_02cc;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(TargeterUtils.CreateLaserBoxHighlight(vector2, vector, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight));
		m_highlights.Add(TargeterUtils.CreateCircleHighlight(vector2, m_radiusAroundStart, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
		m_highlights.Add(TargeterUtils.CreateCircleHighlight(vector, m_radiusAroundEnd, TargeterUtils.HeightAdjustType.DontAdjustHeight, targetingActor == GameFlowData.Get().activeOwnedActorData));
		goto IL_02cc;
		IL_0354:
		Vector3 vector3;
		if (flag3)
		{
			m_highlights[index2].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(m_highlights[index2], vector3, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			m_highlights[index2].SetActive(false);
		}
		Vector3 vector4;
		if (flag4)
		{
			m_highlights[index3].SetActive(true);
			TargeterUtils.RefreshCircleHighlight(m_highlights[index3], vector4, TargeterUtils.HeightAdjustType.DontAdjustHeight);
		}
		else
		{
			m_highlights[index3].SetActive(false);
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInRadiusOfLine(m_indicatorHandler, vector2, vector, num2, m_radiusAroundEnd, m_rangeFromLine, m_penetrateLoS, targetingActor);
			HideUnusedSquareIndicators();
		}
		return;
		IL_02cc:
		vector3 = vector2;
		vector3.y = HighlightUtils.GetHighlightHeight();
		vector4 = vector;
		vector4.y = HighlightUtils.GetHighlightHeight();
		if (flag2)
		{
			if (vector2 != vector)
			{
				m_highlights[index].SetActive(true);
				TargeterUtils.RefreshLaserBoxHighlight(m_highlights[index], vector3, vector4, widthInSquares, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				goto IL_0354;
			}
		}
		m_highlights[index].SetActive(false);
		goto IL_0354;
	}
}
