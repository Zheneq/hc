using System.Collections.Generic;
using UnityEngine;

public class SniperGhillieSuit : Ability
{
	public bool m_isToggleAbility = true;
	public int m_costPerTurn = 1;
	public bool m_proximityBasedInvisibility = true;
	public bool m_unsuppressInvisOnPhaseEnd = true;
	public StandardActorEffectData m_standardActorEffectData;
	[Header("-- Health threshold to trigger cooldown reset, value:(0-1)")]
	public float m_cooldownResetHealthThreshold = -1f;
	[Header("-- Sequences --------------------------------------")]
	public GameObject m_toggleOnSequencePrefab;
	public GameObject m_toggleOffSequencePrefab;

	[TextArea(1, 3)]
	public string m_sequenceNotes;

	private AbilityMod_SniperGhillieSuit m_abilityMod;

	private void Start()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			true,
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	private int GetHealingAmountOnSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingOnSelf
			: 0;
	}

	private StandardActorEffectData GetStealthEffectData()
	{
		return m_abilityMod != null && m_abilityMod.m_useStealthEffectDataOverride
			? m_abilityMod.m_stealthEffectDataOverride
			: m_standardActorEffectData;
	}

	public float GetCooldownResetHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold)
			: m_cooldownResetHealthThreshold;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetHealingAmountOnSelf() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealingAmountOnSelf());
		}
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperGhillieSuit abilityMod_SniperGhillieSuit = modAsBase as AbilityMod_SniperGhillieSuit;
		AddTokenInt(tokens, "CostPerTurn", string.Empty, m_costPerTurn);
		m_standardActorEffectData.AddTooltipTokens(tokens, "StandardActorEffectData");
		AddTokenFloatAsPct(tokens, "CooldownResetHealthThreshold_Pct", string.Empty, abilityMod_SniperGhillieSuit != null
			? abilityMod_SniperGhillieSuit.m_cooldownResetHealthThresholdMod.GetModifiedValue(m_cooldownResetHealthThreshold)
			: m_cooldownResetHealthThreshold);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperGhillieSuit))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SniperGhillieSuit;
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}
}
