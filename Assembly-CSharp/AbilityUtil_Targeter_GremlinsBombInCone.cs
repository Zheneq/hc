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

	public AbilityUtil_Targeter_GremlinsBombInCone(Ability ability, AbilityAreaShape shape, bool penetrateLoS, DamageOriginType damageOriginType = DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false, AffectsActor affectsCaster = AffectsActor.Possible)
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

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary)
	{
		m_enemyTooltipSubject = enemySubject;
		m_allyTooltipSubject = allySubject;
	}

	protected BoardSquare GetGameplayRefSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		GridPos gridPos;
		if (GetCurrentRangeInSquares() != 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			gridPos = currentTarget.GridPos;
		}
		else
		{
			gridPos = targetingActor.GetGridPosWithIncrementedHeight();
		}
		return Board.Get().GetBoardSquareSafe(gridPos);
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare gameplayRefSquare = GetGameplayRefSquare(currentTarget, targetingActor);
		if (gameplayRefSquare != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
					Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
					centerOfShape.y = travelBoardSquareWorldPosition.y + m_heightOffset;
					return centerOfShape;
				}
				}
			}
		}
		return Vector3.zero;
	}

	private void SetHighlightPos(Vector3 pos)
	{
		using (List<GameObject>.Enumerator enumerator = m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				current.transform.position = pos;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights.Count < 4)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_highlights[2].SetActive(false);
			m_highlights[3].SetActive(false);
			return;
		}
	}

	public override void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 highlightGoalPos = GetHighlightGoalPos(target, targetingActor);
			SetHighlightPos(highlightGoalPos);
			BoardSquare gameplayRefSquare = GetGameplayRefSquare(target, targetingActor);
			UpdateAngleIndicatorHighlights(target, targetingActor, currentTargetIndex, gameplayRefSquare);
			return;
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			int num = 2;
			int num2;
			if (m_showAngleIndicators)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = ((currentTargetIndex == 0) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool flag = (byte)num2 != 0;
			if (flag)
			{
				num = 4;
			}
			if (m_highlights != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_highlights.Count >= num)
				{
					MoveHighlightsTowardPos(highlightGoalPos);
					goto IL_0174;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			if (flag)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_angleIndicatorLength, 0.2f, true, Color.cyan));
				m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_angleIndicatorLength, 0.2f, true, Color.cyan));
			}
			SetHighlightPos(highlightGoalPos);
			goto IL_0174;
			IL_0174:
			GameObject gameObject = m_highlights[0];
			GameObject gameObject2 = m_highlights[1];
			Vector3 damageOrigin;
			if (m_damageOriginType == DamageOriginType.CasterPos)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				damageOrigin = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			}
			else
			{
				damageOrigin = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
			}
			ActorData occupantActor = gameplayRefSquare.OccupantActor;
			if (occupantActor != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (MatchesTeam(occupantActor, targetingActor) && occupantActor.IsVisibleToClient())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, currentTarget.FreePos, gameplayRefSquare, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							HandleAddActorInShape(current, targetingActor, currentTarget, damageOrigin);
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					gameObject.SetActive(true);
					gameObject2.SetActive(false);
					goto IL_02a7;
				}
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			goto IL_02a7;
			IL_02a7:
			if (m_affectsCaster == AffectsActor.Always)
			{
				AddActorInRange(targetingActor, damageOrigin, targetingActor, m_allyTooltipSubject);
			}
			UpdateAngleIndicatorHighlights(currentTarget, targetingActor, currentTargetIndex, gameplayRefSquare);
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	private void UpdateAngleIndicatorHighlights(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, BoardSquare targetSquare)
	{
		int num;
		if (m_showAngleIndicators)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentTargetIndex == 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((m_highlights.Count >= 4) ? 1 : 0);
				goto IL_003e;
			}
		}
		num = 0;
		goto IL_003e;
		IL_003e:
		if (num != 0)
		{
			GameObject gameObject = m_highlights[2];
			GameObject gameObject2 = m_highlights[3];
			Vector3 vec = targetSquare.ToVector3() - targetingActor.GetTravelBoardSquareWorldPosition();
			vec.y = 0f;
			if (vec.magnitude > 0f)
			{
				gameObject.SetActive(true);
				gameObject2.SetActive(true);
				float num2 = VectorUtils.HorizontalAngle_Deg(vec);
				float angle = num2 + m_angleWithCenter;
				float angle2 = num2 - m_angleWithCenter;
				Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
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
		if (!(potentialTarget != targetingActor))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_affectsCaster == AffectsActor.Never)
			{
				return false;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
