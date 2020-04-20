using System;
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

	public AbilityUtil_Targeter_RampartDelayedAoe(Ability ability, AbilityAreaShape shapeLowEnergy, AbilityAreaShape shapeFullEnergy, bool penetrateLoS, bool affectsEnemies = true, bool affectsAllies = false, bool affectsCaster = true) : base(ability)
	{
		this.m_shapeLowEnergy = shapeLowEnergy;
		this.m_shapeFullEnergy = shapeFullEnergy;
		this.m_penetrateLoS = penetrateLoS;
		this.m_affectsCaster = affectsCaster;
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_allyTooltipSubject = AbilityTooltipSubject.Primary;
	}

	public GridPos GetCurrentGridPos()
	{
		return this.m_currentGridPos;
	}

	public void SetTooltipSubjectTypes(AbilityTooltipSubject enemySubject = AbilityTooltipSubject.Primary, AbilityTooltipSubject allySubject = AbilityTooltipSubject.Primary)
	{
		this.m_enemyTooltipSubject = enemySubject;
		this.m_allyTooltipSubject = allySubject;
	}

	protected Vector3 GetHighlightGoalPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare targetSquare = this.GetTargetSquare(currentTarget);
		if (targetSquare != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_RampartDelayedAoe.GetHighlightGoalPos(AbilityTarget, ActorData)).MethodHandle;
			}
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shapeFullEnergy, currentTarget.FreePos, targetSquare);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	private BoardSquare GetTargetSquare(AbilityTarget currentTarget)
	{
		return Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.m_currentGridPos = currentTarget.GridPos;
		base.ClearActorsInRange();
		BoardSquare targetSquare = this.GetTargetSquare(currentTarget);
		if (targetSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_RampartDelayedAoe.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
			if (this.m_highlights != null)
			{
				if (this.m_highlights.Count >= 2)
				{
					for (int i = 0; i < this.m_highlights.Count; i++)
					{
						this.m_highlights[i].transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, this.m_highlights[i], ref this.m_curSpeed);
					}
					goto IL_165;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_highlights = new List<GameObject>();
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shapeLowEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData));
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shapeFullEnergy, targetingActor == GameFlowData.Get().activeOwnedActorData));
			for (int j = 0; j < this.m_highlights.Count; j++)
			{
				this.m_highlights[j].transform.position = highlightGoalPos;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			IL_165:
			bool flag = targetingActor.TechPoints >= targetingActor.GetActualMaxTechPoints();
			this.m_highlights[0].SetActive(!flag);
			this.m_highlights[1].SetActive(flag);
			AbilityAreaShape shape = (!flag) ? this.m_shapeLowEnergy : this.m_shapeFullEnergy;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, currentTarget.FreePos, targetSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(shape, currentTarget.FreePos, targetSquare, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			foreach (ActorData actor in actorsInShape)
			{
				base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			if (this.m_affectsCaster)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				base.AddActorInRange(targetingActor, centerOfShape, targetingActor, this.m_allyTooltipSubject, false);
			}
		}
	}
}
