using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GremlinsBombInCone : AbilityUtil_Targeter
{
	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}

	public AbilityAreaShape m_shape;
	public bool m_penetrateLoS;
	public AffectsActor m_affectsCaster;

	private float m_heightOffset = 0.1f;
	private float m_curSpeed;
	private bool m_showAngleIndicators;
	private float m_angleWithCenter = 15f;
	private float m_angleIndicatorLength = 15f;

	protected AbilityTooltipSubject m_enemyTooltipSubject;
	protected AbilityTooltipSubject m_allyTooltipSubject;

	public DamageOriginType m_damageOriginType;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_GremlinsBombInCone(
		Ability ability,
		AbilityAreaShape shape,
		bool penetrateLoS,
		DamageOriginType damageOriginType = DamageOriginType.CenterOfShape,
		bool affectsEnemies = true,
		bool affectsAllies = false,
		AffectsActor affectsCaster = AffectsActor.Possible)
		: base(ability)
	{
		m_shape = shape;
		m_penetrateLoS = penetrateLoS;
		m_damageOriginType = damageOriginType;
		m_affectsCaster = affectsCaster;
		m_affectsEnemies = affectsEnemies;
		m_affectsAllies = affectsAllies;
		m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		m_showArcToShape = HighlightUtils.Get().m_showTargetingArcsForShapes;
	}

	public GridPos GetCurrentGridPos()
	{
		return m_currentGridPos;
	}

	public void SetAngleIndicatorConfig(bool showAngleIndicator, float angleWithCenter, float indicatorLength = 15f)
	{
		m_showAngleIndicators = showAngleIndicator;
		m_angleWithCenter = angleWithCenter;
		m_angleIndicatorLength = indicatorLength;
	}

	public void SetTooltipSubjectTypes(
		AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary,
		AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary)
	{
		m_enemyTooltipSubject = enemySubject;
		m_allyTooltipSubject = allySubject;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		GridPos gridPos = GetCurrentRangeInSquares() != 0f
			? currentTarget.GridPos
			: targetingActor.GetGridPos();
		return Board.Get().GetSquare(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare == null)
		{
			return Vector3.zero;
		}
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
		centerOfShape.y = travelBoardSquareWorldPosition.y + m_heightOffset;
		return centerOfShape;
	}

	private void SetHighlightPos(Vector3 pos)
	{
		foreach (GameObject highlight in m_highlights)
		{
			highlight.transform.position = pos;
		}
	}

	private void MoveHighlightsTowardPos(Vector3 pos)
	{
		m_highlights[0].transform.position = TargeterUtils.MoveHighlightTowards(pos, m_highlights[0], ref m_curSpeed);
		m_highlights[1].transform.position = m_highlights[0].transform.position;
	}

	private bool MatchesTeam(ActorData targetActor, ActorData caster)
	{
		if (targetActor == caster && m_affectsCaster == AffectsActor.Never)
		{
			return false;
		}
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			return m_affectsAllies;
		}
		return m_affectsEnemies;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights.Count >= 4)
		{
			m_highlights[2].SetActive(false);
			m_highlights[3].SetActive(false);
		}
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (m_highlights != null)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(target, targetingActor);
			SetHighlightPos(highlightGoalPos);
			BoardSquare gameplayRefSquare = GetGameplayRefSquare(target, targetingActor);
			UpdateAngleIndicatorHighlights(target, targetingActor, currentTargetIndex, gameplayRefSquare);
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare == null)
		{
			return;
		}
		Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
		int num = 2;
		bool flag = m_showAngleIndicators && currentTargetIndex == 0;
		if (flag)
		{
			num = 4;
		}
		if (m_highlights != null && m_highlights.Count >= num)
		{
			MoveHighlightsTowardPos(highlightGoalPos);
		}
		else
		{
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			if (flag)
			{
				m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_angleIndicatorLength, 0.2f, true, Color.cyan));
				m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_angleIndicatorLength, 0.2f, true, Color.cyan));
			}
			SetHighlightPos(highlightGoalPos);
		}
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		Vector3 damageOrigin = m_damageOriginType == DamageOriginType.CasterPos
			? targetingActor.GetLoSCheckPos()
			: AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
		ActorData occupantActor = gameplayRefSquare.OccupantActor;
		if (occupantActor != null
		    && MatchesTeam(occupantActor, targetingActor)
		    && occupantActor.IsActorVisibleToClient())
		{
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
				m_shape,
				currentTarget.FreePos,
				gameplayRefSquare,
				m_penetrateLoS,
				targetingActor,
				GetAffectedTeams(),
				null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			foreach (ActorData potentialTarget in actors)
			{
				HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin);
			}
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
		}
		else
		{
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
		}
		if (m_affectsCaster == AffectsActor.Always)
		{
			AddActorInRange(targetingActor, damageOrigin, targetingActor, m_allyTooltipSubject);
		}
		UpdateAngleIndicatorHighlights(currentTarget, targetingActor, currentTargetIndex, gameplayRefSquare);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	private void UpdateAngleIndicatorHighlights(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, BoardSquare targetSquare)
	{
		if (m_showAngleIndicators
		    && currentTargetIndex == 0
		    && m_highlights.Count >= 4)
		{
			GameObject gameObject = m_highlights[2];
			GameObject gameObject2 = m_highlights[3];
			Vector3 vec = targetSquare.ToVector3() - targetingActor.GetFreePos();
			vec.y = 0f;
			if (vec.magnitude > 0f)
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
				float baseAngle = VectorUtils.HorizontalAngle_Deg(vec);
				float angle = baseAngle + m_angleWithCenter;
				float angle2 = baseAngle - m_angleWithCenter;
				Vector3 travelBoardSquareWorldPosition = targetingActor.GetFreePos();
				travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
				gameObject.transform.position = travelBoardSquareWorldPosition;
				gameObject2.transform.position = travelBoardSquareWorldPosition;
				gameObject.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
				gameObject2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
			}
			else
			{
				gameObject.SetActive(false);
				gameObject2.SetActive(false);
			}
		}
	}

	protected virtual bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin)
	{
		if (potentialTarget == targetingActor && m_affectsCaster == AffectsActor.Never)
		{
			return false;
		}
		if (potentialTarget.GetTeam() == targetingActor.GetTeam())
		{
			AddActorInRange(potentialTarget, damageOrigin, targetingActor, m_allyTooltipSubject);
		}
		else
		{
			AddActorInRange(potentialTarget, damageOrigin, targetingActor, m_enemyTooltipSubject);
		}
		return true;
	}
}
