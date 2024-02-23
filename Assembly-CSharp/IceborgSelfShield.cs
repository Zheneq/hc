using System.Collections.Generic;
using System.Text;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgSelfShield : GenericAbility_Container
{
	[Separator("Health to be considered low health if below")]
	public int m_lowHealthThresh;
	[Separator("Shield if all shield depleted on first turn")]
	public int m_shieldOnNextTurnIfDepleted;
	[Separator("Sequences")]
	public GameObject m_shieldRemoveSeqPrefab;

	private AbilityMod_IceborgSelfShield m_abilityMod;
	private Iceborg_SyncComponent m_syncComp;

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(ContextKeys.s_CasterLowHealth.GetName());
		return contextNamesForEditor;
	}

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return new StringBuilder().Append(usageForEditor).Append(ContextVars.GetContextUsageStr(ContextKeys.s_CasterLowHealth.GetName(), "set to 1 if caster is low health, 0 otherwise", false)).ToString();
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
		return m_abilityMod != null
			? m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh)
			: m_lowHealthThresh;
	}

	public int GetShieldOnNextTurnIfDepleted()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldOnNextTurnIfDepletedMod.GetModifiedValue(m_shieldOnNextTurnIfDepleted)
			: m_shieldOnNextTurnIfDepleted;
	}

	public bool IsCasterLowHealth(ActorData caster)
	{
		return GetLowHealthThresh() > 0
		       && caster.HitPoints < GetLowHealthThresh();
	}

	public override List<StatusType> GetStatusToApplyWhenRequested()
	{
		return m_abilityMod != null
		       && m_syncComp != null
		       && m_syncComp.m_selfShieldLowHealthOnTurnStart
			? m_abilityMod.m_lowHealthStatusWhenRequested
			: base.GetStatusToApplyWhenRequested();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgSelfShield;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}
}
