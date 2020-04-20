using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStimPack : Ability
{
	[Separator("On Hit", true)]
	public int m_selfHealAmount;

	public StandardEffectInfo m_selfHitEffect;

	[Separator("For other abilities when active", true)]
	public bool m_basicAttackIgnoreCover;

	public bool m_basicAttackReduceCoverEffectiveness;

	public float m_grenadeExtraRange;

	public StandardEffectInfo m_dashShootExtraEffect;

	[Separator("CDR - Health threshold to trigger cooldown reset, value:(0-1)", true)]
	public float m_cooldownResetHealthThreshold = -1f;

	[Header("-- CDR - if dash and shoot used on same turn")]
	public int m_cdrIfDashAndShootUsed;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierStimPack m_abilityMod;

	private AbilityData m_abilityData;

	private SoldierGrenade m_grenadeAbility;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedDashShootExtraEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Stim Pack";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_abilityData == null)
		{
			this.m_abilityData = base.GetComponent<AbilityData>();
		}
		if (this.m_abilityData != null && this.m_grenadeAbility == null)
		{
			this.m_grenadeAbility = (this.m_abilityData.GetAbilityOfType(typeof(SoldierGrenade)) as SoldierGrenade);
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		this.m_cachedSelfHitEffect = ((!this.m_abilityMod) ? this.m_selfHitEffect : this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect));
		StandardEffectInfo cachedDashShootExtraEffect;
		if (this.m_abilityMod)
		{
			cachedDashShootExtraEffect = this.m_abilityMod.m_dashShootExtraEffectMod.GetModifiedValue(this.m_dashShootExtraEffect);
		}
		else
		{
			cachedDashShootExtraEffect = this.m_dashShootExtraEffect;
		}
		this.m_cachedDashShootExtraEffect = cachedDashShootExtraEffect;
	}

	public int GetSelfHealAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_selfHealAmountMod.GetModifiedValue(this.m_selfHealAmount);
		}
		else
		{
			result = this.m_selfHealAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfHitEffect != null)
		{
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
		}
		return result;
	}

	public bool BasicAttackIgnoreCover()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_basicAttackIgnoreCoverMod.GetModifiedValue(this.m_basicAttackIgnoreCover);
		}
		else
		{
			result = this.m_basicAttackIgnoreCover;
		}
		return result;
	}

	public bool BasicAttackReduceCoverEffectiveness()
	{
		return (!this.m_abilityMod) ? this.m_basicAttackReduceCoverEffectiveness : this.m_abilityMod.m_basicAttackReduceCoverEffectivenessMod.GetModifiedValue(this.m_basicAttackReduceCoverEffectiveness);
	}

	public float GetGrenadeExtraRange()
	{
		return (!this.m_abilityMod) ? this.m_grenadeExtraRange : this.m_abilityMod.m_grenadeExtraRangeMod.GetModifiedValue(this.m_grenadeExtraRange);
	}

	public StandardEffectInfo GetDashShootExtraEffect()
	{
		return (this.m_cachedDashShootExtraEffect == null) ? this.m_dashShootExtraEffect : this.m_cachedDashShootExtraEffect;
	}

	public float GetCooldownResetHealthThreshold()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(this.m_cooldownResetHealthThreshold);
		}
		else
		{
			result = this.m_cooldownResetHealthThreshold;
		}
		return result;
	}

	public int GetCdrIfDashAndShootUsed()
	{
		return (!this.m_abilityMod) ? this.m_cdrIfDashAndShootUsed : this.m_abilityMod.m_cdrIfDashAndShootUsedMod.GetModifiedValue(this.m_cdrIfDashAndShootUsed);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealAmount());
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierStimPack abilityMod_SoldierStimPack = modAsBase as AbilityMod_SoldierStimPack;
		string name = "SelfHealAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SoldierStimPack)
		{
			val = abilityMod_SoldierStimPack.m_selfHealAmountMod.GetModifiedValue(this.m_selfHealAmount);
		}
		else
		{
			val = this.m_selfHealAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SoldierStimPack)
		{
			effectInfo = abilityMod_SoldierStimPack.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			effectInfo = this.m_selfHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfHitEffect", this.m_selfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_SoldierStimPack) ? this.m_dashShootExtraEffect : abilityMod_SoldierStimPack.m_dashShootExtraEffectMod.GetModifiedValue(this.m_dashShootExtraEffect), "DashShootExtraEffect", this.m_dashShootExtraEffect, true);
		base.AddTokenInt(tokens, "CdrIfDashAndShootUsed", string.Empty, this.m_cdrIfDashAndShootUsed, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierStimPack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SoldierStimPack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
