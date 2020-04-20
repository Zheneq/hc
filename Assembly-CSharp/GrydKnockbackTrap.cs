using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydKnockbackTrap : Ability
{
	[Header("-- Trap Ground Field")]
	public GroundEffectField m_trapFieldInfo;

	[Header("-- Extra Damage")]
	public int m_extraDamagePerTurn;

	public int m_maxExtraDamage;

	public int m_knockbackAmount = 2;

	public bool m_lockToCardinalDirs = true;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private GroundEffectField m_cachedTrapFieldInfo;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Knockback Trap";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		GroundEffectField trapFieldInfo = this.GetTrapFieldInfo();
		AbilityUtil_Targeter.AffectsActor affectsActor;
		if (trapFieldInfo.IncludeAllies())
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Possible;
		}
		else
		{
			affectsActor = AbilityUtil_Targeter.AffectsActor.Never;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsActor;
		base.Targeters.Clear();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_KnockbackAoE abilityUtil_Targeter_KnockbackAoE = new AbilityUtil_Targeter_KnockbackAoE(this, trapFieldInfo.shape, trapFieldInfo.penetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, trapFieldInfo.IncludeEnemies(), trapFieldInfo.IncludeAllies(), affectsCaster, AbilityUtil_Targeter.AffectsActor.Never, (float)this.m_knockbackAmount, KnockbackType.ForwardAlongAimDir);
			abilityUtil_Targeter_KnockbackAoE.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_KnockbackAoE.m_lockToCardinalDirs = this.m_lockToCardinalDirs;
			abilityUtil_Targeter_KnockbackAoE.m_showArrowHighlight = true;
			base.Targeters.Add(abilityUtil_Targeter_KnockbackAoE);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		this.m_cachedTrapFieldInfo = this.m_trapFieldInfo;
	}

	public GroundEffectField GetTrapFieldInfo()
	{
		GroundEffectField result;
		if (this.m_cachedTrapFieldInfo != null)
		{
			result = this.m_cachedTrapFieldInfo;
		}
		else
		{
			result = this.m_trapFieldInfo;
		}
		return result;
	}

	public int GetExtraDamagePerTurn()
	{
		return this.m_extraDamagePerTurn;
	}

	public int GetMaxExtraDamage()
	{
		return this.m_maxExtraDamage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		int damageAmount = this.GetTrapFieldInfo().damageAmount;
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, damageAmount);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefHiddenTrap abilityMod_ThiefHiddenTrap = modAsBase as AbilityMod_ThiefHiddenTrap;
		this.m_trapFieldInfo.AddTooltipTokens(tokens, "GroundEffect", false, null);
		base.AddTokenInt(tokens, "ExtraDamagePerTurn", string.Empty, (!abilityMod_ThiefHiddenTrap) ? this.m_extraDamagePerTurn : abilityMod_ThiefHiddenTrap.m_extraDamagePerTurnMod.GetModifiedValue(this.m_extraDamagePerTurn), false);
		string name = "MaxExtraDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefHiddenTrap)
		{
			val = abilityMod_ThiefHiddenTrap.m_maxExtraDamageMod.GetModifiedValue(this.m_maxExtraDamage);
		}
		else
		{
			val = this.m_maxExtraDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
	}
}
