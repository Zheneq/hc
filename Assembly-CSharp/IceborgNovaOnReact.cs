using System.Collections.Generic;
using UnityEngine;

public class IceborgNovaOnReact : GenericAbility_Container
{
	[Separator("On Hit Data for React Hits", "yellow")]
	public OnHitAuthoredData m_reactOnHitData;

	[Space(10f)]
	public int m_reactDuration = 1;

	public bool m_reactRequireDamage = true;

	public bool m_reactEffectEndEarlyIfTriggered;

	[Separator("Energy on bearer/caster per reaction", true)]
	public int m_energyOnTargetPerReaction;

	public int m_energyOnCasterPerReaction;

	[Separator("Passive Bonus Energy Gain for Nova Core Triggering", true)]
	public int m_extraEnergyPerNovaCoreTrigger;

	[Separator("Damage Threshold to apply instance to self on turn start. Ignored if <= 0", true)]
	public int m_damageThreshForInstanceOnSelf;

	[Separator("Sequences", true)]
	public GameObject m_reactPersistentSeqPrefab;

	public GameObject m_reactOnTriggerSeqPrefab;

	private AbilityMod_IceborgNovaOnReact m_abilityMod;

	private Iceborg_SyncComponent m_syncComp;

	private OnHitAuthoredData m_cachedReactOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for Reaction --\n" + m_reactOnHitData.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		SetCachedFields();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_reactOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ReactDuration", string.Empty, m_reactDuration);
		AddTokenInt(tokens, "EnergyOnTargetPerReaction", string.Empty, m_energyOnTargetPerReaction);
		AddTokenInt(tokens, "EnergyOnCasterPerReaction", string.Empty, m_energyOnCasterPerReaction);
		AddTokenInt(tokens, "ExtraEnergyPerNovaCoreTrigger", string.Empty, m_extraEnergyPerNovaCoreTrigger);
		AddTokenInt(tokens, "DamageThreshForInstanceOnSelf", string.Empty, m_damageThreshForInstanceOnSelf);
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
		if (!(m_syncComp != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_syncComp.AddTooltipTokens(tokens);
			return;
		}
	}

	private void SetCachedFields()
	{
		m_cachedReactOnHitData = ((!(m_abilityMod != null)) ? m_reactOnHitData : m_abilityMod.m_reactOnHitDataMod._001D(m_reactOnHitData));
	}

	public OnHitAuthoredData GetReactOnHitData()
	{
		return (m_cachedReactOnHitData == null) ? m_reactOnHitData : m_cachedReactOnHitData;
	}

	public int GetReactDuration()
	{
		return (!(m_abilityMod != null)) ? m_reactDuration : m_abilityMod.m_reactDurationMod.GetModifiedValue(m_reactDuration);
	}

	public bool ReactRequireDamage()
	{
		bool result;
		if (m_abilityMod != null)
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
			result = m_abilityMod.m_reactRequireDamageMod.GetModifiedValue(m_reactRequireDamage);
		}
		else
		{
			result = m_reactRequireDamage;
		}
		return result;
	}

	public bool ReactEffectEndEarlyIfTriggered()
	{
		bool result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_reactEffectEndEarlyIfTriggeredMod.GetModifiedValue(m_reactEffectEndEarlyIfTriggered);
		}
		else
		{
			result = m_reactEffectEndEarlyIfTriggered;
		}
		return result;
	}

	public int GetEnergyOnTargetPerReaction()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (6)
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
			result = m_abilityMod.m_energyOnTargetPerReactionMod.GetModifiedValue(m_energyOnTargetPerReaction);
		}
		else
		{
			result = m_energyOnTargetPerReaction;
		}
		return result;
	}

	public int GetEnergyOnCasterPerReaction()
	{
		return (!(m_abilityMod != null)) ? m_energyOnCasterPerReaction : m_abilityMod.m_energyOnCasterPerReactionMod.GetModifiedValue(m_energyOnCasterPerReaction);
	}

	public int GetExtraEnergyPerNovaCoreTrigger()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (1)
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
			result = m_abilityMod.m_extraEnergyPerNovaCoreTriggerMod.GetModifiedValue(m_extraEnergyPerNovaCoreTrigger);
		}
		else
		{
			result = m_extraEnergyPerNovaCoreTrigger;
		}
		return result;
	}

	public int GetDamageThreshForInstanceOnSelf()
	{
		int result;
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (7)
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
			result = m_abilityMod.m_damageThreshForInstanceOnSelfMod.GetModifiedValue(m_damageThreshForInstanceOnSelf);
		}
		else
		{
			result = m_damageThreshForInstanceOnSelf;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgNovaOnReact);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
