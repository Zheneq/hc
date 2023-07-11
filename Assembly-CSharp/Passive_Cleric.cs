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

		if (m_syncComp != null
		    && !Owner.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			RemoveAreaBuff();
		}
	}

	// custom
	public override void OnTurnStart()
	{
		base.OnTurnStart();

		if (m_syncComp != null
		    && !Owner.GetAbilityData().ValidateActionIsRequestableDisregardingQueuedActions(m_buffAbilityActionType))
		{
			RemoveAreaBuff();
		}
	}

	// custom
	private void RemoveAreaBuff()
	{
		List<Effect> activeEffects = ServerEffectManager.Get()
			.GetEffectsOnTargetByCaster(Owner, Owner, typeof(MantaRegenerationEffect));
		if (activeEffects.Count > 0)
		{
			ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Owner, Owner.GetFreePos()));
			hitRes.AddEffectForRemoval(activeEffects[0], ServerEffectManager.Get().GetActorEffects(Owner));
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(
				Owner,
				Owner,
				hitRes,
				m_buffAbility,
				true,
				m_buffAbility.m_toggleOffSequencePrefab);
		}
	}
#endif
}
