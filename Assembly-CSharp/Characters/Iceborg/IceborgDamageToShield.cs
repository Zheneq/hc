using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgDamageToShield : GenericAbility_Container
{
	[Separator("Persistent Aoe Duration")]
	public int m_duration = 1;
	[Separator("On Hit Data for Persistent Aoe", "yellow")]
	public OnHitAuthoredData m_persistentAoeOnHitData;
	[Separator("Damage to shield multiplier")]
	public float m_damageToShieldMult = 0.5f;
	[Separator("Apply Nova effect?")]
	public bool m_applyDelayedAoeEffect = true;
	[Separator("Sequences for Persistent Aoe Effect")]
	public GameObject m_persistentSeqPrefab;
	public GameObject m_onAoeTriggerSequence;
	[Header("-- for shield effect (applied on start of turns)")]
	public GameObject m_shieldPersistentSeqPrefab;

	private Iceborg_SyncComponent m_syncComp;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Persistent Aoe Hits --\n" + m_persistentAoeOnHitData.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (!actorHitContext.ContainsKey(targetActor)
		    || targetActor.GetTeam() == caster.GetTeam())
		{
			return;
		}
		ActorHitContext actorContext = actorHitContext[targetActor];
		CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, m_persistentAoeOnHitData.m_enemyHitIntFields, m_calculatedValuesForTargeter);
		results.m_damage = m_calculatedValuesForTargeter.m_damage;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
