using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ShapeOnHit : AbilityUtil_Targeter
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

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	public DamageOriginType m_damageOriginType;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_ShapeOnHit(Ability ability, AbilityAreaShape shape, bool penetrateLoS, DamageOriginType damageOriginType = DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false, AffectsActor affectsCaster = AffectsActor.Possible)
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
	}

	public GridPos GetCurrentGridPos()
	{
		return m_currentGridPos;
	}

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary)
	{
		m_enemyTooltipSubject = enemySubject;
		m_allyTooltipSubject = allySubject;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		GridPos gridPos = (GetCurrentRangeInSquares() == 0f) ? targetingActor.GetGridPosWithIncrementedHeight() : currentTarget.GridPos;
		return Board.Get().GetSquare(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			centerOfShape.y = travelBoardSquareWorldPosition.y + m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
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
		if (!(targetActor != caster))
		{
			if (m_affectsCaster == AffectsActor.Never)
			{
				return false;
			}
		}
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			return m_affectsAllies;
		}
		return m_affectsEnemies;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (!(gameplayRefSquare != null))
		{
			return;
		}
		while (true)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			if (m_highlights != null)
			{
				if (m_highlights.Count >= 2)
				{
					MoveHighlightsTowardPos(highlightGoalPos);
					goto IL_00eb;
				}
			}
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			SetHighlightPos(highlightGoalPos);
			goto IL_00eb;
			IL_00eb:
			GameObject gameObject = m_highlights[0];
			GameObject gameObject2 = m_highlights[1];
			Vector3 damageOrigin;
			if (m_damageOriginType == DamageOriginType.CasterPos)
			{
				damageOrigin = targetingActor.GetLoSCheckPos();
			}
			else
			{
				damageOrigin = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
			}
			ActorData occupantActor = gameplayRefSquare.OccupantActor;
			if (occupantActor != null)
			{
				if (MatchesTeam(occupantActor, targetingActor) && occupantActor.IsVisibleToClient())
				{
					List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, currentTarget.FreePos, gameplayRefSquare, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							HandleAddActorInShape(current, targetingActor, currentTarget, damageOrigin);
						}
					}
					gameObject.SetActive(true);
					gameObject2.SetActive(false);
					goto IL_020e;
				}
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			goto IL_020e;
			IL_020e:
			if (m_affectsCaster == AffectsActor.Always)
			{
				AddActorInRange(targetingActor, damageOrigin, targetingActor, m_allyTooltipSubject);
			}
			return;
		}
	}

	protected virtual bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin)
	{
		if (!(potentialTarget != targetingActor))
		{
			if (m_affectsCaster == AffectsActor.Never)
			{
				return false;
			}
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
