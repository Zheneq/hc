// ROGUES
// SERVER
using System.Collections.Generic;

public class Passive_Manta : Passive
{
	private MantaCreateBarriers m_createBarriersAbility;
	private MantaRegeneration m_regenAbility;
	private StandardBarrierData m_ultBarrierInfo;
	private List<BarrierPoseInfo> m_ultBarrierLocations;

	public int DamageReceivedForRegeneration { get; private set; }
	
#if SERVER
	// added in rogues
	protected override void OnStartup()
	{
		base.OnStartup();
		m_createBarriersAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(MantaCreateBarriers)) as MantaCreateBarriers;
		m_regenAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(MantaRegeneration)) as MantaRegeneration;
	}

	// added in rogues
	public override void OnDamaged(ActorData damageCaster, DamageSource damageSource, int damageAmount)
	{
		base.OnDamaged(damageCaster, damageSource, damageAmount);
		List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(Owner, Owner, typeof(MantaRegenerationEffect));
		if (!effectsOnTargetByCaster.IsNullOrEmpty()
		    && effectsOnTargetByCaster[0] is MantaRegenerationEffect mantaRegenerationEffect
		    && mantaRegenerationEffect.m_time.age == 0)
		{
			DamageReceivedForRegeneration += damageAmount;
		}
	}

	// added in rogues
	public override void OnAbilityCastResolved(Ability ability)
	{
		base.OnAbilityCastResolved(ability);
		if (ability is MantaRegeneration)
		{
			DamageReceivedForRegeneration = 0;
		}
	}

	// added in rogues
	public void SetDelayedBarrierInfo(StandardBarrierData barrierData, List<BarrierPoseInfo> poses)
	{
		m_ultBarrierInfo = barrierData;
		m_ultBarrierLocations = poses;
	}

	// added in rogues
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		AbilityData abilityData = Owner.GetAbilityData();
		if (abilityData != null
		    && abilityData.HasQueuedAbilityOfType(typeof(MantaRegeneration)) // , true in rogues
		    && DamageReceivedForRegeneration == 0)
		{
			AbilityModCooldownReduction cooldownReductionOnNoDamage = m_regenAbility.GetCooldownReductionOnNoDamage();
			if (cooldownReductionOnNoDamage != null && cooldownReductionOnNoDamage.HasCooldownReduction())
			{
				ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
				cooldownReductionOnNoDamage.AppendCooldownMiscEvents(hitRes, true, 0, 0);
				MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(Owner, Owner, hitRes, m_regenAbility);
			}
		}
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_ultBarrierInfo != null && m_ultBarrierLocations != null)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Owner, m_ultBarrierLocations[0].midpoint));
			float squareSize = Board.Get().squareSize;
			m_ultBarrierInfo.m_width = m_ultBarrierLocations[0].widthInWorld / squareSize;
			foreach (BarrierPoseInfo barrierPose in m_ultBarrierLocations)
			{
				Barrier barrier = new Barrier(
					m_createBarriersAbility.m_abilityName,
					barrierPose.midpoint,
					barrierPose.facingDirection,
					Owner,
					m_ultBarrierInfo);
				barrier.SetSourceAbility(m_createBarriersAbility);
				actorHitResults.AddBarrier(barrier);
			}
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
				Owner, Owner, actorHitResults, m_createBarriersAbility, false);
		}
		m_ultBarrierInfo = null;
		m_ultBarrierLocations = null;
	}
#endif
}
