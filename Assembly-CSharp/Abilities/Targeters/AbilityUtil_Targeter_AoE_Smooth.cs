using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_AoE_Smooth : AbilityUtil_Targeter
{
	public delegate float GetRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor);
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
	public delegate bool ShouldIncludeActorDelegate(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor);
	public delegate Vector3 CustomCenterPosDelegate(ActorData caster, AbilityTarget currentTarget);

	public float m_radius;
	public bool m_penetrateLoS;
	public bool m_penetrateEnemyBarriers;
	public float m_heightOffset = 0.1f;
	public int m_maxTargets = -1;
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;
	public bool m_adjustPosInConfirmedTargeting;
	public Vector3 m_lastUpdatedCenterPos = Vector3.zero;
	public GetRadiusDelegate m_customRadiusDelegate;
	public IsAffectingCasterDelegate m_affectCasterDelegate;
	public ShouldIncludeActorDelegate m_customShouldIncludeActorDelegate;
	public CustomCenterPosDelegate m_customCenterPosDelegate;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_AoE_Smooth(
		Ability ability,
		float radius,
		bool penetrateLoS,
		bool affectsEnemies = true,
		bool affectsAllies = false,
		int maxTargets = -1)
		: base(ability)
	{
		m_radius = radius;
		m_penetrateLoS = penetrateLoS;
		m_affectsEnemies = affectsEnemies;
		m_affectsAllies = affectsAllies;
		m_maxTargets = maxTargets;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public void SetupKnockbackData(float knockbackDistance, KnockbackType knockbackType)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
	}

	protected virtual float GetRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return m_customRadiusDelegate?.Invoke(currentTarget, targetingActor) ?? m_radius;
	}

	protected virtual bool GetPenetrateLoS()
	{
		return m_penetrateLoS;
	}

	protected virtual Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		if (m_customCenterPosDelegate != null)
		{
			return m_customCenterPosDelegate(targetingActor, currentTarget);
		}
		return range == 0f
			? targetingActor.GetFreePos()
			: currentTarget.FreePos;
	}

	protected virtual Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return GetRefPos(currentTarget, targetingActor, range);
	}

	public virtual void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		if (Highlight != null)
		{
			return;
		}
		Highlight = HighlightUtils.Get().CreateAoECursor(
			radiusInSquares * Board.Get().squareSize,
			targetingActor == GameFlowData.Get().activeOwnedActorData);
		Highlight.SetActive(true);
	}

	public override void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateConfirmedTargeting(currentTarget, targetingActor);
		if (m_adjustPosInConfirmedTargeting && m_customCenterPosDelegate != null && Highlight != null)
		{
			Vector3 vector = m_customCenterPosDelegate(targetingActor, currentTarget);
			vector.y = HighlightUtils.GetHighlightHeight();
			Highlight.transform.position = vector;
			m_lastUpdatedCenterPos = vector;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float radius = GetRadius(currentTarget, targetingActor);
		CreateHighlightObjectsIfNeeded(radius, targetingActor);
		ClearActorsInRange();
		float currentRangeInSquares = GetCurrentRangeInSquares();
		Vector3 refPos = GetRefPos(currentTarget, targetingActor, currentRangeInSquares);
		Highlight.SetActive(true);
		Vector3 casterPos = targetingActor.GetFreePos();
		refPos.y = casterPos.y + m_heightOffset;
		Highlight.transform.position = refPos;
		m_lastUpdatedCenterPos = refPos;
		if (m_penetrateEnemyBarriers)
		{
			BarrierManager.Get().SuppressAbilityBlocks_Start();
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(
			refPos,
			radius,
			GetPenetrateLoS(),
			targetingActor,
			GetAffectedTeams(),
			null);
		if (m_penetrateEnemyBarriers)
		{
			BarrierManager.Get().SuppressAbilityBlocks_End();
		}
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		if (m_customShouldIncludeActorDelegate != null)
		{
			for (int num = actors.Count - 1; num >= 0; num--)
			{
				if (!m_customShouldIncludeActorDelegate(actors[num], refPos, targetingActor))
				{
					actors.RemoveAt(num);
				}
			}
		}
		if (m_maxTargets > 0 && actors.Count > m_maxTargets)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref actors, refPos);
			TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
		}
		bool selfHit = false;
		if (actors.Contains(targetingActor))
		{
			selfHit = true;
			actors.Remove(targetingActor);
		}
		foreach (ActorData item in actors)
		{
			AddActorInRange(item, GetDamageOrigin(currentTarget, targetingActor, currentRangeInSquares), targetingActor);
			ActorHitContext actorHitContext = m_actorContextVars[item];
			float value = VectorUtils.HorizontalPlaneDistInSquares(refPos, item.GetFreePos());
			actorHitContext.m_contextVars.SetValue(ContextKeys.s_DistFromStart.GetKey(), value);
		}
		if ((m_affectsTargetingActor || selfHit)
		    && (m_affectCasterDelegate == null || m_affectCasterDelegate(targetingActor, actors)))
		{
			AddActorInRange(
				targetingActor,
				targetingActor.GetFreePos(),
				targetingActor,
				selfHit
					? AbilityTooltipSubject.Primary
					: AbilityTooltipSubject.Self);
			m_actorContextVars[targetingActor].m_contextVars.SetValue(
				ContextKeys.s_DistFromStart.GetKey(),
				VectorUtils.HorizontalPlaneDistInSquares(refPos, targetingActor.GetFreePos()));
		}

		int arrowIndex = 0;
		if (m_knockbackDistance > 0f)
		{
			EnableAllMovementArrows();
			foreach (ActorData targetActor in actors)
			{
				if (targetActor.GetTeam() != targetingActor.GetTeam())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
						targetActor,
						m_knockbackType,
						currentTarget.AimDirection,
						refPos,
						m_knockbackDistance);
					arrowIndex = AddMovementArrowWithPrevious(targetActor, path, TargeterMovementType.Knockback, arrowIndex);
				}
			}
		}
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		HandleHiddenSquareIndicators(targetingActor, refPos);
	}

	protected virtual void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
		if (targetingActor != GameFlowData.Get().activeOwnedActorData)
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		AreaEffectUtils.OperateOnSquaresInCone(
			m_indicatorHandler,
			centerPos,
			0f,
			360f,
			m_radius,
			0f,
			targetingActor,
			GetPenetrateLoS());
		HideUnusedSquareIndicators();
	}
}
