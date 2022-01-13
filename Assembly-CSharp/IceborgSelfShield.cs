using AbilityContextNamespace;
using System.Collections.Generic;
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
		contextNamesForEditor.Add(ContextKeys._0012.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return usageForEditor + ContextVars.GetContextUsageStr(ContextKeys._0012.GetName(), "set to 1 if caster is low health, 0 otherwise", false);
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
		AddTokenInt(tokens, "ShieldOnNextTurnIfDepleted", string.Empty, m_shieldOnNextTurnIfDepleted);
	}

	public int GetLowHealthThresh()
	{
		return (!(m_abilityMod != null)) ? m_lowHealthThresh : m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh);
	}

	public int GetShieldOnNextTurnIfDepleted()
	{
		return (!(m_abilityMod != null)) ? m_shieldOnNextTurnIfDepleted : m_abilityMod.m_shieldOnNextTurnIfDepletedMod.GetModifiedValue(m_shieldOnNextTurnIfDepleted);
	}

	public bool IsCasterLowHealth(ActorData caster)
	{
		bool result = false;
		if (GetLowHealthThresh() > 0)
		{
			if (caster.HitPoints < GetLowHealthThresh())
			{
				result = true;
			}
		}
		return result;
	}

	public override List<StatusType> GetStatusToApplyWhenRequested()
	{
		if (m_abilityMod != null)
		{
			if (m_syncComp != null)
			{
				if (m_syncComp.m_selfShieldLowHealthOnTurnStart)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return m_abilityMod.m_lowHealthStatusWhenRequested;
						}
					}
				}
			}
		}
		return base.GetStatusToApplyWhenRequested();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_IceborgSelfShield);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
