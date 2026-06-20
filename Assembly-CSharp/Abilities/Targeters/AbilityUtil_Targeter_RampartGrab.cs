// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RampartGrab : AbilityUtil_Targeter
{
	public enum DamageOriginType
	{
		CenterOfShape,
		CasterPos
	}

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

	public AbilityUtil_Targeter_RampartGrab(Ability ability, AbilityAreaShape shape, float knockbackDistance, KnockbackType knockbackType, float laserRange, float laserWidth, bool penetrateLos, int maxTargets)
		: base(ability)
	{
		m_shape = shape;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_laserRange = laserRange;
		m_laserWidth = laserWidth;
		m_penetrateLos = penetrateLos;
		m_maxTargets = maxTargets;
		m_enemyTooltipSubject = AbilityTooltipSubject.Primary;
		m_allyTooltipSubject = AbilityTooltipSubject.Primary;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
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
		BoardSquare squareFromAbilityTarget = GetSquareFromAbilityTarget(currentTarget, targetingActor);
		if (squareFromAbilityTarget == null)
		{
			return Vector3.zero;
		}
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
		centerOfShape.y = targetingActor.GetFreePos().y + m_heightOffset;
		return centerOfShape;
	}

	protected BoardSquare GetSquareFromAbilityTarget(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return Board.Get().GetSquare(currentTarget.GridPos);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		m_currentGridPos = currentTarget.GridPos;
		ClearActorsInRange();
		if (currentTargetIndex > 0)
		{
			VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
			laserCoords.start = targetingActor.GetLoSCheckPos();
#if VANILLA
			List<Team> otherTeams = targetingActor.GetEnemyTeamAsList(); // reactor
#else
			List<Team> otherTeams = targetingActor.GetOtherTeams(); // rogues
#endif
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[currentTargetIndex - 1].AimDirection, m_laserRange, m_laserWidth, targetingActor, otherTeams, m_penetrateLos, m_maxTargets, false, false, out laserCoords.end, null);
			int arrowIndex = 0;
			EnableAllMovementArrows();
			BoardSquare squareFromAbilityTarget = GetSquareFromAbilityTarget(currentTarget, targetingActor);
			if (squareFromAbilityTarget != null)
			{
				Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
				if (Highlight == null)
				{
					Highlight = HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData);
					Highlight.transform.position = highlightGoalPos;
				}
				else
				{
					Highlight.transform.position = TargeterUtils.MoveHighlightTowards(highlightGoalPos, Highlight, ref m_curSpeed);
				}
				Highlight.SetActive(true);
				foreach (ActorData actor in actorsInLaser)
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actor, m_knockbackType, currentTarget.AimDirection, squareFromAbilityTarget.ToVector3(), m_knockbackDistance);
					arrowIndex = AddMovementArrowWithPrevious(actor, path, TargeterMovementType.Knockback, arrowIndex);
				}
			}
			SetMovementArrowEnabledFromIndex(arrowIndex, false);
		}
	}
}
