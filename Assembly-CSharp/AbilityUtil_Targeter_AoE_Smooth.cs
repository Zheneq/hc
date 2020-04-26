using AbilityContextNamespace;
using System.Collections.Generic;
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

	public AbilityUtil_Targeter_AoE_Smooth(Ability ability, float radius, bool penetrateLoS, bool affectsEnemies = true, bool affectsAllies = false, int maxTargets = -1)
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
		if (m_customRadiusDelegate != null)
		{
			return m_customRadiusDelegate(currentTarget, targetingActor);
		}
		return m_radius;
	}

	protected virtual bool GetPenetrateLoS()
	{
		return m_penetrateLoS;
	}

	protected virtual Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		Vector3 zero = Vector3.zero;
		if (m_customCenterPosDelegate != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_customCenterPosDelegate(targetingActor, currentTarget);
				}
			}
		}
		if (range != 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return currentTarget.FreePos;
				}
			}
		}
		return targetingActor.GetTravelBoardSquareWorldPosition();
	}

	protected virtual Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return GetRefPos(currentTarget, targetingActor, range);
	}

	public virtual void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		if (!(base.Highlight == null))
		{
			return;
		}
		while (true)
		{
			base.Highlight = HighlightUtils.Get().CreateAoECursor(radiusInSquares * Board.Get().squareSize, targetingActor == GameFlowData.Get().activeOwnedActorData);
			base.Highlight.SetActive(true);
			return;
		}
	}

	public override void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateConfirmedTargeting(currentTarget, targetingActor);
		if (!m_adjustPosInConfirmedTargeting)
		{
			return;
		}
		while (true)
		{
			if (m_customCenterPosDelegate == null)
			{
				return;
			}
			while (true)
			{
				if (base.Highlight != null)
				{
					Vector3 vector = m_customCenterPosDelegate(targetingActor, currentTarget);
					vector.y = HighlightUtils.GetHighlightHeight();
					base.Highlight.transform.position = vector;
					m_lastUpdatedCenterPos = vector;
				}
				return;
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float radius = GetRadius(currentTarget, targetingActor);
		CreateHighlightObjectsIfNeeded(radius, targetingActor);
		ClearActorsInRange();
		float currentRangeInSquares = GetCurrentRangeInSquares();
		Vector3 refPos = GetRefPos(currentTarget, targetingActor, currentRangeInSquares);
		base.Highlight.SetActive(true);
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		refPos.y = travelBoardSquareWorldPosition.y + m_heightOffset;
		base.Highlight.transform.position = refPos;
		m_lastUpdatedCenterPos = refPos;
		if (m_penetrateEnemyBarriers)
		{
			BarrierManager.Get().SuppressAbilityBlocks_Start();
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(refPos, radius, GetPenetrateLoS(), targetingActor, GetAffectedTeams(), null);
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
		bool flag = false;
		if (actors.Contains(targetingActor))
		{
			flag = true;
			actors.Remove(targetingActor);
		}
		foreach (ActorData item in actors)
		{
			AddActorInRange(item, GetDamageOrigin(currentTarget, targetingActor, currentRangeInSquares), targetingActor);
			ActorHitContext actorHitContext = m_actorContextVars[item];
			float value = VectorUtils.HorizontalPlaneDistInSquares(refPos, item.GetTravelBoardSquareWorldPosition());
			actorHitContext._0015.SetFloat(ContextKeys._0018.GetHash(), value);
		}
		if (!m_affectsTargetingActor)
		{
			if (!flag)
			{
				goto IL_02a8;
			}
		}
		if (m_affectCasterDelegate != null)
		{
			if (!m_affectCasterDelegate(targetingActor, actors))
			{
				goto IL_02a8;
			}
		}
		AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, flag ? AbilityTooltipSubject.Primary : AbilityTooltipSubject.Self);
		ActorHitContext actorHitContext2 = m_actorContextVars[targetingActor];
		float value2 = VectorUtils.HorizontalPlaneDistInSquares(refPos, targetingActor.GetTravelBoardSquareWorldPosition());
		actorHitContext2._0015.SetFloat(ContextKeys._0018.GetHash(), value2);
		goto IL_02a8;
		IL_02a8:
		int num2 = 0;
		if (m_knockbackDistance > 0f)
		{
			EnableAllMovementArrows();
			using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (current2.GetTeam() != targetingActor.GetTeam())
					{
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current2, m_knockbackType, currentTarget.AimDirection, refPos, m_knockbackDistance);
						num2 = AddMovementArrowWithPrevious(current2, path, TargeterMovementType.Knockback, num2);
					}
				}
			}
		}
		SetMovementArrowEnabledFromIndex(num2, false);
		HandleHiddenSquareIndicators(targetingActor, refPos);
	}

	protected virtual void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, centerPos, 0f, 360f, m_radius, 0f, targetingActor, GetPenetrateLoS());
			HideUnusedSquareIndicators();
		}
	}
}
