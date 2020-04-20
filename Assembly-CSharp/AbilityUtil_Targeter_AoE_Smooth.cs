using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_AoE_Smooth : AbilityUtil_Targeter
{
	public float m_radius;

	public bool m_penetrateLoS;

	public bool m_penetrateEnemyBarriers;

	public float m_heightOffset = 0.1f;

	public int m_maxTargets = -1;

	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public bool m_adjustPosInConfirmedTargeting;

	public Vector3 m_lastUpdatedCenterPos = Vector3.zero;

	public AbilityUtil_Targeter_AoE_Smooth.GetRadiusDelegate m_customRadiusDelegate;

	public AbilityUtil_Targeter_AoE_Smooth.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_AoE_Smooth.ShouldIncludeActorDelegate m_customShouldIncludeActorDelegate;

	public AbilityUtil_Targeter_AoE_Smooth.CustomCenterPosDelegate m_customCenterPosDelegate;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_AoE_Smooth(Ability ability, float radius, bool penetrateLoS, bool affectsEnemies = true, bool affectsAllies = false, int maxTargets = -1) : base(ability)
	{
		this.m_radius = radius;
		this.m_penetrateLoS = penetrateLoS;
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_maxTargets = maxTargets;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public void SetupKnockbackData(float knockbackDistance, KnockbackType knockbackType)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
	}

	protected virtual float GetRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_customRadiusDelegate != null)
		{
			return this.m_customRadiusDelegate(currentTarget, targetingActor);
		}
		return this.m_radius;
	}

	protected virtual bool GetPenetrateLoS()
	{
		return this.m_penetrateLoS;
	}

	protected virtual Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		Vector3 result = Vector3.zero;
		if (this.m_customCenterPosDelegate != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth.GetRefPos(AbilityTarget, ActorData, float)).MethodHandle;
			}
			result = this.m_customCenterPosDelegate(targetingActor, currentTarget);
		}
		else if (range != 0f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = currentTarget.FreePos;
		}
		else
		{
			result = targetingActor.GetTravelBoardSquareWorldPosition();
		}
		return result;
	}

	protected virtual Vector3 GetDamageOrigin(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		return this.GetRefPos(currentTarget, targetingActor, range);
	}

	public virtual void CreateHighlightObjectsIfNeeded(float radiusInSquares, ActorData targetingActor)
	{
		if (base.Highlight == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth.CreateHighlightObjectsIfNeeded(float, ActorData)).MethodHandle;
			}
			base.Highlight = HighlightUtils.Get().CreateAoECursor(radiusInSquares * Board.Get().squareSize, targetingActor == GameFlowData.Get().activeOwnedActorData);
			base.Highlight.SetActive(true);
		}
	}

	public override void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_adjustPosInConfirmedTargeting)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth.UpdateConfirmedTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_customCenterPosDelegate != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (base.Highlight != null)
				{
					Vector3 vector = this.m_customCenterPosDelegate(targetingActor, currentTarget);
					vector.y = HighlightUtils.GetHighlightHeight();
					base.Highlight.transform.position = vector;
					this.m_lastUpdatedCenterPos = vector;
				}
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float radius = this.GetRadius(currentTarget, targetingActor);
		this.CreateHighlightObjectsIfNeeded(radius, targetingActor);
		base.ClearActorsInRange();
		float currentRangeInSquares = this.GetCurrentRangeInSquares();
		Vector3 refPos = this.GetRefPos(currentTarget, targetingActor, currentRangeInSquares);
		base.Highlight.SetActive(true);
		refPos.y = targetingActor.GetTravelBoardSquareWorldPosition().y + this.m_heightOffset;
		base.Highlight.transform.position = refPos;
		this.m_lastUpdatedCenterPos = refPos;
		if (this.m_penetrateEnemyBarriers)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_Smooth.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			BarrierManager.Get().SuppressAbilityBlocks_Start();
		}
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(refPos, radius, this.GetPenetrateLoS(), targetingActor, base.GetAffectedTeams(), null, false, default(Vector3));
		if (this.m_penetrateEnemyBarriers)
		{
			BarrierManager.Get().SuppressAbilityBlocks_End();
		}
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
		if (this.m_customShouldIncludeActorDelegate != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int i = actorsInRadius.Count - 1; i >= 0; i--)
			{
				if (!this.m_customShouldIncludeActorDelegate(actorsInRadius[i], refPos, targetingActor))
				{
					actorsInRadius.RemoveAt(i);
				}
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
		if (this.m_maxTargets > 0 && actorsInRadius.Count > this.m_maxTargets)
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
			TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadius, refPos);
			TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadius, this.m_maxTargets);
		}
		bool flag = false;
		if (actorsInRadius.Contains(targetingActor))
		{
			flag = true;
			actorsInRadius.Remove(targetingActor);
		}
		foreach (ActorData actorData in actorsInRadius)
		{
			base.AddActorInRange(actorData, this.GetDamageOrigin(currentTarget, targetingActor, currentRangeInSquares), targetingActor, AbilityTooltipSubject.Primary, false);
			ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
			float value = VectorUtils.HorizontalPlaneDistInSquares(refPos, actorData.GetTravelBoardSquareWorldPosition());
			actorHitContext.\u0015.SetFloat(ContextKeys.\u0018.GetHash(), value);
		}
		if (!this.m_affectsTargetingActor)
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
			if (!flag)
			{
				goto IL_2A8;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (this.m_affectCasterDelegate != null)
		{
			if (!this.m_affectCasterDelegate(targetingActor, actorsInRadius))
			{
				goto IL_2A8;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, (!flag) ? AbilityTooltipSubject.Self : AbilityTooltipSubject.Primary, false);
		ActorHitContext actorHitContext2 = this.m_actorContextVars[targetingActor];
		float value2 = VectorUtils.HorizontalPlaneDistInSquares(refPos, targetingActor.GetTravelBoardSquareWorldPosition());
		actorHitContext2.\u0015.SetFloat(ContextKeys.\u0018.GetHash(), value2);
		IL_2A8:
		int num = 0;
		if (this.m_knockbackDistance > 0f)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			base.EnableAllMovementArrows();
			using (List<ActorData>.Enumerator enumerator2 = actorsInRadius.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					if (actorData2.GetTeam() != targetingActor.GetTeam())
					{
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData2, this.m_knockbackType, currentTarget.AimDirection, refPos, this.m_knockbackDistance);
						num = base.AddMovementArrowWithPrevious(actorData2, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		base.SetMovementArrowEnabledFromIndex(num, false);
		this.HandleHiddenSquareIndicators(targetingActor, refPos);
	}

	protected virtual void HandleHiddenSquareIndicators(ActorData targetingActor, Vector3 centerPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, centerPos, 0f, 360f, this.m_radius, 0f, targetingActor, this.GetPenetrateLoS(), null);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate float GetRadiusDelegate(AbilityTarget currentTarget, ActorData targetingActor);

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate bool ShouldIncludeActorDelegate(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor);

	public delegate Vector3 CustomCenterPosDelegate(ActorData caster, AbilityTarget currentTarget);
}
