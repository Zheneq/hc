using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RampartGrab : AbilityUtil_Targeter
{
	private AbilityAreaShape m_shape;

	private float m_knockbackDistance;

	private KnockbackType m_knockbackType;

	public float m_laserRange;

	private float m_laserWidth;

	private bool m_penetrateLos;

	private int m_maxTargets;

	private float m_heightOffset = 0.1f;

	private float m_curSpeed;

	protected AbilityTooltipSubject m_enemyTooltipSubject;

	protected AbilityTooltipSubject m_allyTooltipSubject;

	private GridPos m_currentGridPos = GridPos.s_invalid;

	public AbilityUtil_Targeter_RampartGrab(Ability ability, AbilityAreaShape shape, float knockbackDistance, KnockbackType knockbackType, float laserRange, float laserWidth, bool penetrateLos, int maxTargets) : base(ability)
	{
		this.m_shape = shape;
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_laserRange = laserRange;
		this.m_laserWidth = laserWidth;
		this.m_penetrateLos = penetrateLos;
		this.m_maxTargets = maxTargets;
		this.m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
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
		BoardSquare squareFromAbilityTarget = this.GetSquareFromAbilityTarget(currentTarget, targetingActor);
		if (squareFromAbilityTarget != null)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, currentTarget);
			centerOfShape.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
			return centerOfShape;
		}
		return Vector3.zero;
	}

	protected BoardSquare GetSquareFromAbilityTarget(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		this.m_currentGridPos = currentTarget.GridPos;
		base.ClearActorsInRange();
		if (currentTargetIndex > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_RampartGrab.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[currentTargetIndex - 1].AimDirection, this.m_laserRange, this.m_laserWidth, targetingActor, targetingActor.GetOpposingTeams(), this.m_penetrateLos, this.m_maxTargets, false, false, out laserCoords.end, null, null, false, true);
			int num = 0;
			base.EnableAllMovementArrows();
			BoardSquare squareFromAbilityTarget = this.GetSquareFromAbilityTarget(currentTarget, targetingActor);
			if (squareFromAbilityTarget != null)
			{
				Vector3 highlightGoalPos = this.GetHighlightGoalPos(currentTarget, targetingActor);
				if (base.Highlight == null)
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
					base.Highlight = HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData);
					base.Highlight.transform.position = highlightGoalPos;
				}
				else
				{
					base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, base.Highlight, ref this.m_curSpeed);
				}
				base.Highlight.SetActive(true);
				using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, squareFromAbilityTarget.ToVector3(), this.m_knockbackDistance);
						num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			base.SetMovementArrowEnabledFromIndex(num, false);
		}
	}

	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}
}
