using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampDashAndAoe : GenericAbility_Container
{
	[Separator("Shield Cost on Cast", true)]
	public int m_shieldCost = 0x14;

	[Separator("Target Select Component for when shield is down", true)]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;

	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;

	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes", true)]
	public int m_shieldDownCooldown = -1;

	[Header("-- Cdr on suit dash when going into suit form")]
	public int m_cdrOnSuitApply;

	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public int m_shieldDownNoCooldownHealthThresh;

	[Separator("Extra Energy for dashing through or onto orb", true)]
	public int m_extraEnergyForDashOnOrb;

	[Separator("Whether we can move after dashing when out of suit", true)]
	public bool m_shieldDownAllowMoveAfterEvade = true;

	private AbilityMod_ScampDashAndAoe m_abilityMod;

	private Scamp_SyncComponent m_syncComp;

	private OnHitAuthoredData m_cachedShieldDownOnHitData;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + this.m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (this.m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(this.m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.SetupTargetersAndCachedVars();
		if (this.m_abilityMod != null)
		{
			this.m_cachedShieldDownOnHitData = this.m_abilityMod.m_shieldDownOnHitDataMod.symbol_001D(this.m_shieldDownOnHitData);
		}
		else
		{
			this.m_cachedShieldDownOnHitData = this.m_shieldDownOnHitData;
		}
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		base.ClearTargeters();
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
		{
			if (!(this.m_shieldDownTargetSelect == null))
			{
				collection = this.m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_4A;
			}
		}
		collection = this.m_targetSelectComp.CreateTargeters(this);
		IL_4A:
		base.Targeters.AddRange(collection);
	}

	public int GetShieldCost()
	{
		return (!(this.m_abilityMod != null)) ? this.m_shieldCost : this.m_abilityMod.m_shieldCostMod.GetModifiedValue(this.m_shieldCost);
	}

	public int GetShieldDownCooldown()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldDownCooldownMod.GetModifiedValue(this.m_shieldDownCooldown);
		}
		else
		{
			result = this.m_shieldDownCooldown;
		}
		return result;
	}

	public int GetCdrOnSuitApply()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cdrOnSuitApplyMod.GetModifiedValue(this.m_cdrOnSuitApply);
		}
		else
		{
			result = this.m_cdrOnSuitApply;
		}
		return result;
	}

	public int GetShieldDownNoCooldownHealthThresh()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldDownNoCooldownHealthThreshMod.GetModifiedValue(this.m_shieldDownNoCooldownHealthThresh);
		}
		else
		{
			result = this.m_shieldDownNoCooldownHealthThresh;
		}
		return result;
	}

	public int GetExtraEnergyForDashOnOrb()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_extraEnergyForDashOnOrbMod.GetModifiedValue(this.m_extraEnergyForDashOnOrb);
		}
		else
		{
			result = this.m_extraEnergyForDashOnOrb;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_shieldDownOnHitData.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "ShieldCost", string.Empty, this.m_shieldCost, false);
		base.AddTokenInt(tokens, "ShieldDownCooldown", string.Empty, this.m_shieldDownCooldown, false);
		base.AddTokenInt(tokens, "CdrOnSuitApply", string.Empty, this.m_cdrOnSuitApply, false);
		base.AddTokenInt(tokens, "ShidleDownNoCooldownHealthThresh", string.Empty, this.m_shieldDownNoCooldownHealthThresh, false);
		base.AddTokenInt(tokens, "ExtraEnergyForDashOnOrb", string.Empty, this.m_extraEnergyForDashOnOrb, false);
	}

	public bool IsInSuit()
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (this.IsInSuit())
		{
			return base.GetTargetSelectComp();
		}
		return this.m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (this.IsInSuit())
		{
			return base.GetOnHitAuthoredData();
		}
		OnHitAuthoredData result;
		if (this.m_cachedShieldDownOnHitData != null)
		{
			result = this.m_cachedShieldDownOnHitData;
		}
		else
		{
			result = this.m_shieldDownOnHitData;
		}
		return result;
	}

	public override int GetBaseCooldown()
	{
		if (!this.IsInSuit())
		{
			if (this.GetShieldDownCooldown() >= 0)
			{
				return this.GetShieldDownCooldown();
			}
		}
		return base.GetBaseCooldown();
	}

	public int CalcCurrentMaxCooldown(bool inSuit)
	{
		int num;
		if (inSuit)
		{
			num = this.m_cooldown;
		}
		else
		{
			num = this.GetShieldDownCooldown();
		}
		int result = num;
		if (this.GetShieldDownCooldown() < 0)
		{
			result = this.m_cooldown;
		}
		return result;
	}

	public override Ability.MovementAdjustment GetMovementAdjustment()
	{
		if (this.CanOverrideMoveStartSquare())
		{
			return Ability.MovementAdjustment.ReducedMovement;
		}
		return base.GetMovementAdjustment();
	}

	public override bool CanOverrideMoveStartSquare()
	{
		bool result;
		if (!this.IsInSuit())
		{
			result = this.m_shieldDownAllowMoveAfterEvade;
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_ScampDashAndAoe);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (this.m_abilityMod != null)
		{
			this.m_targetSelectComp.SetTargetSelectMod(this.m_abilityMod.m_inSuitTargetSelectMod);
			this.m_shieldDownTargetSelect.SetTargetSelectMod(this.m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			this.m_targetSelectComp.ClearTargetSelectMod();
			this.m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
}
