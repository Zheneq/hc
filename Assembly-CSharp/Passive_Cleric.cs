using System.Collections.Generic;
using UnityEngine;

public class Passive_Cleric : Passive
{
	[Header("Cleric cooldown reduction when taking damage")]
	public AbilityModCooldownReduction m_cooldownReductionOnDamageTaken;
	
#if SERVER
	// custom
    private Cleric_SyncComponent m_syncComp;
    private ClericAreaBuff m_buffAbility;
    private AbilityData.ActionType m_buffAbilityActionType;
    
    // custom
    protected override void OnStartup()
    {
	    base.OnStartup();
	    
	    m_syncComp = Owner.GetComponent<Cleric_SyncComponent>();
	    m_buffAbility = Owner.GetAbilityData().GetAbilityOfType(typeof(ClericAreaBuff)) as ClericAreaBuff;
	    m_buffAbilityActionType = Owner.GetAbilityData().GetActionTypeOfAbility(m_buffAbility);
    }
	
    // custom
	public override void OnResolveStart(bool hasAbilities, bool hasMovement)
	{
		base.OnResolveStart(hasAbilities, hasMovement);

		if (!ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(ClericAreaBuff)))
		{
			Log.Info($"ClericAreaBuff cancelled, removing");
			RemoveAreaBuff(m_buffAbility.GetCooldownWhenBuffLapses());
		}
	}

	// custom
	public override void OnTurnStart()
	{
		base.OnTurnStart();

		if (!Owner.GetAbilityData().ValidateActionIsRequestableDisregardingQueuedActions(m_buffAbilityActionType))
		{
			Log.Info($"ClericAreaBuff depleted, removing");
			// We have do deduct 1 from the cooldown since it's already next turn
			// It doesn't makes sense to check requestability before Cleric got his per-turn tech point regen
			RemoveAreaBuff(m_buffAbility.GetCooldownWhenBuffLapses() - 1);
		}
	}

	// custom
	private void RemoveAreaBuff(int cooldown)
	{
		List<Effect> activeEffects = ServerEffectManager.Get()
			.GetEffectsOnTargetByCaster(Owner, Owner, typeof(ClericAreaBuffEffect));
		if (activeEffects.Count > 0)
		{
			ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			hitRes.AddEffectForRemoval(activeEffects[0], ServerEffectManager.Get().GetActorEffects(Owner));
			hitRes.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_buffAbilityActionType, cooldown)
			{
				m_ignoreCooldownMax = true
			});
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
				Owner,
				Owner,
				hitRes,
				m_buffAbility,
				true,
				m_buffAbility.m_toggleOffSequencePrefab);
		}
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_turnsAreaBuffActive = 0;
			Log.Info($"ClericAreaBuff ended, reset to {m_syncComp.Networkm_turnsAreaBuffActive}");
		}
	}
#endif
}
