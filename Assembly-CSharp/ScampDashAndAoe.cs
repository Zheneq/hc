using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScampDashAndAoe : GenericAbility_Container
{
	[Separator("Shield Cost on Cast")]
	public int m_shieldCost = 20;
	[Separator("Target Select Component for when shield is down")]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;
	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;
	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes")]
	public int m_shieldDownCooldown = -1;
	[Header("-- Cdr on suit dash when going into suit form")]
	public int m_cdrOnSuitApply;
	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public int m_shieldDownNoCooldownHealthThresh;
	[Separator("Extra Energy for dashing through or onto orb")]
	public int m_extraEnergyForDashOnOrb;
	[Separator("Whether we can move after dashing when out of suit")]
	public bool m_shieldDownAllowMoveAfterEvade = true;

	private AbilityMod_ScampDashAndAoe m_abilityMod;
	private Scamp_SyncComponent m_syncComp;
	private OnHitAuthoredData m_cachedShieldDownOnHitData;

	public override string GetOnHitDataDesc()
	{
		return new StringBuilder().Append(base.GetOnHitDataDesc()).Append("\n-- On Hit Data when shields are down --\n").Append(m_shieldDownOnHitData.GetInEditorDesc()).ToString();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		m_cachedShieldDownOnHitData = m_abilityMod != null
			? m_abilityMod.m_shieldDownOnHitDataMod.GetModdedOnHitData(m_shieldDownOnHitData)
			: m_shieldDownOnHitData;
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		Targeters.AddRange(!hasShield && m_shieldDownTargetSelect != null
			? m_shieldDownTargetSelect.CreateTargeters(this)
			: m_targetSelectComp.CreateTargeters(this));
	}

	public int GetShieldCost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldCostMod.GetModifiedValue(m_shieldCost)
			: m_shieldCost;
	}

	public int GetShieldDownCooldown()
	{
		int result;
		result = m_abilityMod != null ? m_abilityMod.m_shieldDownCooldownMod.GetModifiedValue(m_shieldDownCooldown) : m_shieldDownCooldown;
		return result;
	}

	public int GetCdrOnSuitApply()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnSuitApplyMod.GetModifiedValue(m_cdrOnSuitApply)
			: m_cdrOnSuitApply;
	}

	public int GetShieldDownNoCooldownHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDownNoCooldownHealthThreshMod.GetModifiedValue(m_shieldDownNoCooldownHealthThresh)
			: m_shieldDownNoCooldownHealthThresh;
	}

	public int GetExtraEnergyForDashOnOrb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForDashOnOrbMod.GetModifiedValue(m_extraEnergyForDashOnOrb)
			: m_extraEnergyForDashOnOrb;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_shieldDownOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ShieldCost", string.Empty, m_shieldCost);
		AddTokenInt(tokens, "ShieldDownCooldown", string.Empty, m_shieldDownCooldown);
		AddTokenInt(tokens, "CdrOnSuitApply", string.Empty, m_cdrOnSuitApply);
		AddTokenInt(tokens, "ShidleDownNoCooldownHealthThresh", string.Empty, m_shieldDownNoCooldownHealthThresh);
		AddTokenInt(tokens, "ExtraEnergyForDashOnOrb", string.Empty, m_extraEnergyForDashOnOrb);
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return IsInSuit()
			? base.GetTargetSelectComp()
			: m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		return IsInSuit()
			? base.GetOnHitAuthoredData()
			: m_cachedShieldDownOnHitData ?? m_shieldDownOnHitData;
	}

	public override int GetBaseCooldown()
	{
		return !IsInSuit() && GetShieldDownCooldown() >= 0
			? GetShieldDownCooldown()
			: base.GetBaseCooldown();
	}

	public int CalcCurrentMaxCooldown(bool inSuit)
	{
		return GetShieldDownCooldown() < 0 || inSuit
			? m_cooldown
			: GetShieldDownCooldown();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		return CanOverrideMoveStartSquare()
			? MovementAdjustment.ReducedMovement
			: base.GetMovementAdjustment();
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return !IsInSuit() && m_shieldDownAllowMoveAfterEvade;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_ScampDashAndAoe;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_inSuitTargetSelectMod);
			m_shieldDownTargetSelect.SetTargetSelectMod(m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
			m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
}
