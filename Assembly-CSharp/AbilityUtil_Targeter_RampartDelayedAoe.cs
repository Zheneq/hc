using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RampartDelayedAoe : AbilityUtil_Targeter
{
	public AbilityAreaShape m_shapeLowEnergy;

	public AbilityAreaShape m_shapeFullEnergy;

	public bool m_penetrateLoS;

	public bool m_affectsCaster;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_RampartDelayedAoe(Ability ability, AbilityAreaShape shapeLowEnergy, AbilityAreaShape shapeFullEnergy, bool penetrateLoS, bool affectsEnemies = true, bool affectsAllies = false, bool affectsCaster = true)
		: base(ability)
	{
		m_shapeLowEnergy = shapeLowEnergy;
		m_shapeFullEnergy = shapeFullEnergy;
		m_penetrateLoS = penetrateLoS;
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

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare targetSquare = GetTargetSquare(currentTarget);
		if (targetSquare != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shapeFullEnergy, currentTarget.FreePos, targetSquare);
					Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
					centerOfShape.y = travelBoardSquareWorldPosition.y + m_heightOffset;
					return centerOfShape;
				}
				}
			}
		}
		return Vector3.zero;
	}

	private BoardSquare GetTargetSquare(AbilityTarget currentTarget)
	{
		return Board.Get().GetSquare(currentTarget.GridPos);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		BoardSquare targetSquare = GetTargetSquare(currentTarget);
		if (!(targetSquare != null))
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
					for (int i = 0; i < m_highlights.Count; i++)
					{
						m_highlights[i].transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, m_highlights[i], ref m_curSpeed);
					}
					goto IL_0165;
				}
			}
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shapeLowEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shapeFullEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData));
			for (int j = 0; j < m_highlights.Count; j++)
			{
				m_highlights[j].transform.position = highlightGoalPos;
			}
			goto IL_0165;
			IL_0165:
			bool flag = targetingActor.TechPoints >= targetingActor.GetActualMaxTechPoints();
			m_highlights[0].SetActive(!flag);
			m_highlights[1].SetActive(flag);
			AbilityAreaShape shape = (!flag) ? m_shapeLowEnergy : m_shapeFullEnergy;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(shape, currentTarget.FreePos, targetSquare, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			foreach (ActorData item in actors)
			{
				AddActorInRange(item, centerOfShape, targetingActor);
			}
			if (m_affectsCaster)
			{
				while (true)
				{
					AddActorInRange(targetingActor, centerOfShape, targetingActor, m_allyTooltipSubject);
					return;
				}
			}
			return;
		}
	}
}
