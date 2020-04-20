using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgSelfShield : GenericAbility_Container
{
	[Separator("Health to be considered low health if below", true)]
	public int m_lowHealthThresh;

	[Separator("Shield if all shield depleted on first turn", true)]
	public int m_shieldOnNextTurnIfDepleted;

	[Separator("Sequences", true)]
	public GameObject m_shieldRemoveSeqPrefab;

	private AbilityMod_IceborgSelfShield m_abilityMod;

	private Iceborg_SyncComponent m_syncComp;

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(ContextKeys.symbol_0012.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return usageForEditor + ContextVars.GetDebugString(ContextKeys.symbol_0012.GetName(), "set to 1 if caster is low health, 0 otherwise", false);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "LowHealthThresh", string.Empty, this.m_lowHealthThresh, false);
		base.AddTokenInt(tokens, "ShieldOnNextTurnIfDepleted", string.Empty, this.m_shieldOnNextTurnIfDepleted, false);
	}

	public int GetLowHealthThresh()
	{
		return (!(this.m_abilityMod != null)) ? this.m_lowHealthThresh : this.m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(this.m_lowHealthThresh);
	}

	public int GetShieldOnNextTurnIfDepleted()
	{
		return (!(this.m_abilityMod != null)) ? this.m_shieldOnNextTurnIfDepleted : this.m_abilityMod.m_shieldOnNextTurnIfDepletedMod.GetModifiedValue(this.m_shieldOnNextTurnIfDepleted);
	}

	public bool IsCasterLowHealth(ActorData caster)
	{
		bool result = false;
		if (this.GetLowHealthThresh() > 0)
		{
			if (caster.HitPoints < this.GetLowHealthThresh())
			{
				result = true;
			}
		}
		return result;
	}

	public override List<StatusType> GetStatusToApplyWhenRequested()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_syncComp != null)
			{
				if (this.m_syncComp.m_selfShieldLowHealthOnTurnStart)
				{
					return this.m_abilityMod.m_lowHealthStatusWhenRequested;
				}
			}
		}
		return base.GetStatusToApplyWhenRequested();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_IceborgSelfShield);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}
}
