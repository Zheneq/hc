using System;
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
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for Reaction --\n" + this.m_reactOnHitData.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		this.SetCachedFields();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_reactOnHitData.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "ReactDuration", string.Empty, this.m_reactDuration, false);
		base.AddTokenInt(tokens, "EnergyOnTargetPerReaction", string.Empty, this.m_energyOnTargetPerReaction, false);
		base.AddTokenInt(tokens, "EnergyOnCasterPerReaction", string.Empty, this.m_energyOnCasterPerReaction, false);
		base.AddTokenInt(tokens, "ExtraEnergyPerNovaCoreTrigger", string.Empty, this.m_extraEnergyPerNovaCoreTrigger, false);
		base.AddTokenInt(tokens, "DamageThreshForInstanceOnSelf", string.Empty, this.m_damageThreshForInstanceOnSelf, false);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		}
		if (this.m_syncComp != null)
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
			this.m_syncComp.AddTooltipTokens(tokens);
		}
	}

	private void SetCachedFields()
	{
		this.m_cachedReactOnHitData = ((!(this.m_abilityMod != null)) ? this.m_reactOnHitData : this.m_abilityMod.m_reactOnHitDataMod.\u001D(this.m_reactOnHitData));
	}

	public OnHitAuthoredData GetReactOnHitData()
	{
		return (this.m_cachedReactOnHitData == null) ? this.m_reactOnHitData : this.m_cachedReactOnHitData;
	}

	public int GetReactDuration()
	{
		return (!(this.m_abilityMod != null)) ? this.m_reactDuration : this.m_abilityMod.m_reactDurationMod.GetModifiedValue(this.m_reactDuration);
	}

	public bool ReactRequireDamage()
	{
		bool result;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.ReactRequireDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactRequireDamageMod.GetModifiedValue(this.m_reactRequireDamage);
		}
		else
		{
			result = this.m_reactRequireDamage;
		}
		return result;
	}

	public bool ReactEffectEndEarlyIfTriggered()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.ReactEffectEndEarlyIfTriggered()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactEffectEndEarlyIfTriggeredMod.GetModifiedValue(this.m_reactEffectEndEarlyIfTriggered);
		}
		else
		{
			result = this.m_reactEffectEndEarlyIfTriggered;
		}
		return result;
	}

	public int GetEnergyOnTargetPerReaction()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.GetEnergyOnTargetPerReaction()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyOnTargetPerReactionMod.GetModifiedValue(this.m_energyOnTargetPerReaction);
		}
		else
		{
			result = this.m_energyOnTargetPerReaction;
		}
		return result;
	}

	public int GetEnergyOnCasterPerReaction()
	{
		return (!(this.m_abilityMod != null)) ? this.m_energyOnCasterPerReaction : this.m_abilityMod.m_energyOnCasterPerReactionMod.GetModifiedValue(this.m_energyOnCasterPerReaction);
	}

	public int GetExtraEnergyPerNovaCoreTrigger()
	{
		int result;
		if (this.m_abilityMod != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.GetExtraEnergyPerNovaCoreTrigger()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraEnergyPerNovaCoreTriggerMod.GetModifiedValue(this.m_extraEnergyPerNovaCoreTrigger);
		}
		else
		{
			result = this.m_extraEnergyPerNovaCoreTrigger;
		}
		return result;
	}

	public int GetDamageThreshForInstanceOnSelf()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(IceborgNovaOnReact.GetDamageThreshForInstanceOnSelf()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageThreshForInstanceOnSelfMod.GetModifiedValue(this.m_damageThreshForInstanceOnSelf);
		}
		else
		{
			result = this.m_damageThreshForInstanceOnSelf;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgNovaOnReact);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
