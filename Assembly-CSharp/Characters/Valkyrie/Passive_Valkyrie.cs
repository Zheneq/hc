// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class Passive_Valkyrie : Passive
{
	private Valkyrie_SyncComponent m_syncComp;
	private ValkyrieGuard m_guardAbility;
	private ValkyrieDashAoE m_dashAbility;
	private ValkyriePullToLaserCenter m_ultAbility;
	private bool m_tookDamageThisTurn;
	private bool m_guardIsUp;
	private int m_lastUltCastTurn = -1;
	private int m_lastGuardCastTurn = -1;

	public int DamageThroughGuardCoverThisTurn { get; private set; }

#if SERVER
	//Added in rouges
	protected override void OnStartup()
	{
		base.OnStartup();
		m_syncComp = Owner.GetComponent<Valkyrie_SyncComponent>();
		AbilityData component = Owner.GetComponent<AbilityData>();
		if (component != null)
		{
			m_guardAbility = component.GetAbilityOfType(typeof(ValkyrieGuard)) as ValkyrieGuard;
			m_dashAbility = component.GetAbilityOfType(typeof(ValkyrieDashAoE)) as ValkyrieDashAoE;
			m_ultAbility = component.GetAbilityOfType(typeof(ValkyriePullToLaserCenter)) as ValkyriePullToLaserCenter;
		}
		Owner.OnKnockbackHitExecutedDelegate += OnKnockbackMovementHitExecuted;
	}

	//Added in rouges
	private void OnDestroy()
	{
		Owner.OnKnockbackHitExecutedDelegate -= OnKnockbackMovementHitExecuted;
	}

	//Added in rouges
	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		AbilityData abilityData = Owner.GetAbilityData();
		if (abilityData == null || damageAmount <= 0)
		{
			return;
		}
		if (IsCoverGuardActive(abilityData))
		{
			bool tooNearForCover = false;
			if (IsDamageCoveredByGuard(damageSource, ref tooNearForCover))
			{
				DamageThroughGuardCoverThisTurn += damageAmount;
				if (m_syncComp != null && m_guardAbility != null)
				{
					m_syncComp.Networkm_extraDamageNextShieldThrow = Mathf.Min(
						m_syncComp.Networkm_extraDamageNextShieldThrow + m_guardAbility.GetExtraDamageNextShieldThrowPerCoveredHit(),
						m_guardAbility.GetMaxExtraDamageNextShieldThrow());
				}
			}
		}
		if (abilityData.HasQueuedAbilityOfType(typeof(ValkyrieDashAoE)) // , true in rogues
		    && !m_tookDamageThisTurn
		    && m_dashAbility != null
		    && m_dashAbility.GetCooldownReductionOnHitAmount() != 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			actorHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
				m_dashAbility.m_cooldownReductionIfDamagedThisTurn.abilitySlot,
				m_dashAbility.GetCooldownReductionOnHitAmount()));
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, actorHitResults, m_dashAbility);
		}
		m_tookDamageThisTurn = true;
	}

	//Added in rouges
	public bool IsCoverGuardActive(AbilityData abilityData)
	{
		return abilityData.HasQueuedAbilityOfType(typeof(ValkyrieGuard)) // , true in rogues
		       || (m_guardAbility != null && m_guardAbility.CoverLastsForever() && m_guardIsUp);
	}

	//Added in rouges
	public bool IsDamageCoveredByGuard(DamageSource damageSource, ref bool tooNearForCover)
	{
		ActorCover.CoverDirections coverDirection = m_syncComp.m_coverDirection;
		float dist = (damageSource.DamageSourceLocation - Owner.GetFreePos()).magnitude;
		tooNearForCover = GameplayData.Get().m_coverMinDistance * Board.Get().squareSize > dist;
		
		// custom
		if (m_guardAbility.CoverIgnoreMinDist())
		{
			tooNearForCover = false;
		}
		if (damageSource.IgnoresCover || dist <= 0.5f * Board.Get().squareSize)
		{
			return false;
		}
		// end custom
		
		float angleToDamage = VectorUtils.HorizontalAngle_Deg(damageSource.DamageSourceLocation - Owner.GetFreePos());
		float shieldAngle = VectorUtils.HorizontalAngle_Deg(Owner.GetActorCover().GetCoverOffset(coverDirection));
		
		// custom
		return AreaEffectUtils.IsAngleWithinCone(angleToDamage, shieldAngle, GameplayData.Get().m_coverProtectionAngle * 0.5f);
		// rogues
		// return Mathf.Abs(angleToDamage - shieldAngle) <= GameplayData.Get().m_coverProtectionAngle * 0.5f;
	}

	//Added in rouges
	public override void OnDied(List<UnresolvedHealthChange> killers)
	{
		base.OnDied(killers);
		m_guardIsUp = false;
	}

	//Added in rouges
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_tookDamageThisTurn = false;
	}

	//Added in rouges
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (DamageThroughGuardCoverThisTurn == 0
		    && m_lastGuardCastTurn == GameFlowData.Get().CurrentTurn
		    && m_guardAbility != null)
		{
			AbilityModCooldownReduction cooldownReductionOnNoBlock = m_guardAbility.GetCooldownReductionOnNoBlock();
			if (cooldownReductionOnNoBlock != null && cooldownReductionOnNoBlock.HasCooldownReduction())
			{
				ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				cooldownReductionOnNoBlock.AppendCooldownMiscEvents(hitRes, true, 0, 0);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, hitRes, m_guardAbility);
			}
		}
		DamageThroughGuardCoverThisTurn = 0;
		if (m_lastUltCastTurn != GameFlowData.Get().CurrentTurn)
		{
			m_syncComp.Networkm_skipDamageReductionForNextStab = false;
		}
	}

	//Added in rouges
	public override void OnAbilityCastResolved(Ability ability)
	{
		base.OnAbilityCastResolved(ability);
		if (ability is ValkyrieGuard || ability is ValkyrieDashAoE)
		{
			m_guardIsUp = true;
			if (ability is ValkyrieGuard)
			{
				m_lastGuardCastTurn = GameFlowData.Get().CurrentTurn;
			}
		}
		else if (ability is ValkyriePullToLaserCenter && m_ultAbility != null)
		{
			m_lastUltCastTurn = GameFlowData.Get().CurrentTurn;
			m_syncComp.Networkm_skipDamageReductionForNextStab = m_ultAbility.ShouldSkipDamageReductionOnNextTurnStab();
		}
	}

	//Added in rouges
	public override void OnMovementResultsGathered(MovementCollection stabilizedMovements)
	{
		if (ServerEffectManager.Get().HasEffectByCaster(Owner, Owner, typeof(ValkyrieGuardEndingEffect)))
		{
			Owner.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ValkyrieStats.DamageMitigatedByCoverOnTurnsWithGuard,
				Owner.GetActorBehavior().serverIncomingDamageReducedByCoverThisTurn);
		}
	}

	//Added in rouges
	private void OnKnockbackMovementHitExecuted(ActorData target, ActorHitResults hitRes)
	{
		if (hitRes.HasDamage && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(ValkyrieThrowShield)))
		{
			Owner.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ValkyrieStats.DamageDoneByThrowShieldAndKnockback,
				hitRes.FinalDamage);
		}
	}
#endif
}
