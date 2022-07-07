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
		if (targetSquare == null)
		{
			return Vector3.zero;
		}
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shapeFullEnergy, currentTarget.FreePos, targetSquare);
		centerOfShape.y = targetingActor.GetFreePos().y + m_heightOffset;
		return centerOfShape;
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
		if (targetSquare != null)
		{
			Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
			if (m_highlights != null && m_highlights.Count >= 2)
			{
				foreach (GameObject highlight in m_highlights)
				{
					highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, highlight, ref m_curSpeed);
				}
			}
			else
			{
				m_highlights = new List<GameObject>
				{
					HighlightUtils.Get().CreateShapeCursor(m_shapeLowEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData),
					HighlightUtils.Get().CreateShapeCursor(m_shapeFullEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData)
				};
				foreach (GameObject highlight in m_highlights)
				{
					highlight.transform.position = highlightGoalPos;
				}
			}
			bool isFullEnergy = targetingActor.TechPoints >= targetingActor.GetMaxTechPoints();
			m_highlights[0].SetActive(!isFullEnergy);
			m_highlights[1].SetActive(isFullEnergy);
			AbilityAreaShape shape = isFullEnergy ? m_shapeFullEnergy : m_shapeLowEnergy;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(shape, currentTarget.FreePos, targetSquare, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			foreach (ActorData item in actors)
			{
				AddActorInRange(item, centerOfShape, targetingActor);
			}
			if (m_affectsCaster)
			{
				AddActorInRange(targetingActor, centerOfShape, targetingActor, m_allyTooltipSubject);
			}
		}
	}
}
