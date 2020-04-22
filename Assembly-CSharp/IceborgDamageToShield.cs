using AbilityContextNamespace;
using System.Collections.Generic;
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!actorHitContext.ContainsKey(targetActor))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (targetActor.GetTeam() != caster.GetTeam())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					ActorHitContext actorContext = actorHitContext[targetActor];
					GenericAbility_Container.CalcIntFieldValues(targetActor, caster, actorContext, abilityContext, m_persistentAoeOnHitData.m_enemyHitIntFields, m_calculatedValuesForTargeter);
					results.m_damage = m_calculatedValuesForTargeter.m_damage;
					return;
				}
			}
			return;
		}
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
	}

	protected override void GenModImpl_ClearModRef()
	{
	}
}
