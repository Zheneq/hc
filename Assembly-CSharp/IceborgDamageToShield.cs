using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgDamageToShield : GenericAbility_Container
{
	[Separator("Persistent Aoe Duration", true)]
	public int m_duration = 1;

	[Separator("On Hit Data for Persistent Aoe", "yellow")]
	public OnHitAuthoredData m_persistentAoeOnHitData;

	[Separator("Damage to shield multiplier", true)]
	public float m_damageToShieldMult = 0.5f;

	[Separator("Apply Nova effect?", true)]
	public bool m_applyDelayedAoeEffect = true;

	[Separator("Sequences for Persistent Aoe Effect", true)]
	public GameObject m_persistentSeqPrefab;

	public GameObject m_onAoeTriggerSequence;

	[Header("-- for shield effect (applied on start of turns)")]
	public GameObject m_shieldPersistentSeqPrefab;

	private Iceborg_SyncComponent m_syncComp;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Persistent Aoe Hits --\n" + this.m_persistentAoeOnHitData.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		if (this.m_syncComp == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageToShield.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
		{
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (actorHitContext.ContainsKey(targetActor))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgDamageToShield.PostProcessTargetingNumbers(ActorData, int, Dictionary<ActorData, ActorHitContext>, ContextVars, ActorData, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (targetActor.GetTeam() != caster.GetTeam())
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
				ActorHitContext actorContext = actorHitContext[targetActor];
				GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, this.m_persistentAoeOnHitData.m_enemyHitIntFields, this.m_calculatedValuesForTargeter);
				results.m_damage = this.m_calculatedValuesForTargeter.m_damage;
			}
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
