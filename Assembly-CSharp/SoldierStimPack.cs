using System.Collections.Generic;
using UnityEngine;

public class SoldierStimPack : Ability
{
	[Separator("On Hit")]
	public int m_selfHealAmount;
	public StandardEffectInfo m_selfHitEffect;
	[Separator("For other abilities when active")]
	public bool m_basicAttackIgnoreCover;
	public bool m_basicAttackReduceCoverEffectiveness;
	public float m_grenadeExtraRange;
	public StandardEffectInfo m_dashShootExtraEffect;
	[Separator("CDR - Health threshold to trigger cooldown reset, value:(0-1)")]
	public float m_cooldownResetHealthThreshold = -1f;
	[Header("-- CDR - if dash and shoot used on same turn")]
	public int m_cdrIfDashAndShootUsed;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SoldierStimPack m_abilityMod;
	private AbilityData m_abilityData;
	private SoldierGrenade m_grenadeAbility;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedDashShootExtraEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stim Pack";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null && m_grenadeAbility == null)
		{
			m_grenadeAbility = m_abilityData.GetAbilityOfType(typeof(SoldierGrenade)) as SoldierGrenade;
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedDashShootExtraEffect = m_abilityMod != null
			? m_abilityMod.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect)
			: m_dashShootExtraEffect;
	}

	public int GetSelfHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public bool BasicAttackIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_basicAttackIgnoreCoverMod.GetModifiedValue(m_basicAttackIgnoreCover)
			: m_basicAttackIgnoreCover;
	}

	public bool BasicAttackReduceCoverEffectiveness()
	{
		return m_abilityMod != null
			? m_abilityMod.m_basicAttackReduceCoverEffectivenessMod.GetModifiedValue(m_basicAttackReduceCoverEffectiveness)
			: m_basicAttackReduceCoverEffectiveness;
	}

	public float GetGrenadeExtraRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_grenadeExtraRangeMod.GetModifiedValue(m_grenadeExtraRange)
			: m_grenadeExtraRange;
	}

	public StandardEffectInfo GetDashShootExtraEffect()
	{
		return m_cachedDashShootExtraEffect ?? m_dashShootExtraEffect;
	}

	public float GetCooldownResetHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold)
			: m_cooldownResetHealthThreshold;
	}

	public int GetCdrIfDashAndShootUsed()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfDashAndShootUsedMod.GetModifiedValue(m_cdrIfDashAndShootUsed)
			: m_cdrIfDashAndShootUsed;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetSelfHealAmount());
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		return number;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierStimPack abilityMod_SoldierStimPack = modAsBase as AbilityMod_SoldierStimPack;
		AddTokenInt(tokens, "SelfHealAmount", string.Empty, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_selfHealAmountMod.GetModifiedValue(m_selfHealAmount)
			: m_selfHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierStimPack != null
			? abilityMod_SoldierStimPack.m_dashShootExtraEffectMod.GetModifiedValue(m_dashShootExtraEffect)
			: m_dashShootExtraEffect, "DashShootExtraEffect", m_dashShootExtraEffect);
		AddTokenInt(tokens, "CdrIfDashAndShootUsed", string.Empty, m_cdrIfDashAndShootUsed);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierStimPack))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierStimPack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
