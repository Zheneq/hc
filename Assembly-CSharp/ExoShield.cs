using System.Collections.Generic;
using UnityEngine;

public class ExoShield : Ability
{
	[Header("-- Shield/Absorb Effect")]
	public StandardActorEffectData m_absorbEffect;
	[Header("-- Extra shielding when using ult")]
	public int m_extraAbsorbIfSieging;
	[Header("-- Cooldowwn Reduction if no shield used")]
	public int m_cdrIfShieldNotUsed;
	[Header("-- Shielding lost to energy conversion (on effect end")]
	public int m_shieldLostPerEnergyGain;
	public int m_maxShieldLostForEnergyGain;
	[Header("-- Energy to shielding (for old anchored ability, may be outdated")]
	public bool m_enableTechPointToAbsorbConversion;
	public float m_techPointToAbsorbConversionRate = 1f;
	public float m_remainingAbsorbToTechPointConversionRate = 1f;
	public float m_anchoredTechPointToAbsorbConversionRate = 1.5f;
	public float m_anchoredRemainingAbsorbToTechPointConversionRate = 1f;
	[Header("-- (If using energy to shield conversion) Energy to use for conversion, use 0 if there is no max")]
	public int m_maxTechPointsCost;
	public int m_minTechPointsForCast;
	public bool m_freeActionWhileAnchored = true;
	[Header("-- Targeter shape - use for mods to effect nearby actors")]
	public AbilityAreaShape m_targeterShape;
	[Header("-- Animation --")]
	public int m_animIndexWhenAnchored = 7;
	[Header("-- Sequences")]
	public GameObject m_shieldSequencePrefab;

	private Exo_SyncComponent m_syncComponent;
	private AbilityMod_ExoShield m_abilityMod;
	private StandardActorEffectData m_cachedAbsorbEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exo Shield";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComponent = GetComponent<Exo_SyncComponent>();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetTargeterShape(),
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			true,
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == m_animIndexWhenAnchored;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		return m_syncComponent != null && m_syncComponent.m_anchored
			? (ActorModelData.ActionAnimationType)m_animIndexWhenAnchored
			: base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		m_cachedAbsorbEffect = m_abilityMod != null
			? m_abilityMod.m_absorbEffectMod.GetModifiedValue(m_absorbEffect)
			: m_absorbEffect;
	}

	public StandardActorEffectData GetAbsorbEffect()
	{
		return m_cachedAbsorbEffect ?? m_absorbEffect;
	}

	public int GetExtraAbsorbIfSieging()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbIfSiegingMod.GetModifiedValue(m_extraAbsorbIfSieging)
			: m_extraAbsorbIfSieging;
	}

	public int GetCdrIfShieldNotUsed()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfShieldNotUsedMod.GetModifiedValue(m_cdrIfShieldNotUsed)
			: m_cdrIfShieldNotUsed;
	}

	public int GetShieldLostPerEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldLostPerEnergyGainMod.GetModifiedValue(m_shieldLostPerEnergyGain)
			: m_shieldLostPerEnergyGain;
	}

	public int GetMaxShieldLostForEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxShieldLostForEnergyGainMod.GetModifiedValue(m_maxShieldLostForEnergyGain)
			: m_maxShieldLostForEnergyGain;
	}

	public int GetMaxTechPointsCost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTechPointsCostMod.GetModifiedValue(m_maxTechPointsCost)
			: m_maxTechPointsCost;
	}

	public int GetMinTechPointsForCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minTechPointsForCastMod.GetModifiedValue(m_minTechPointsForCast)
			: m_minTechPointsForCast;
	}

	public bool FreeActionWhileAnchored()
	{
		return m_abilityMod != null
			? m_abilityMod.m_freeActionWhileAnchoredMod.GetModifiedValue(m_freeActionWhileAnchored)
			: m_freeActionWhileAnchored;
	}

	public AbilityAreaShape GetTargeterShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterShapeMod.GetModifiedValue(m_targeterShape)
			: m_targeterShape;
	}

	private bool WillBeAnchoredDuringCombat()
	{
		return m_syncComponent != null && m_syncComponent.m_anchored
		       || ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ExoAnchorLaser));
	}

	private bool IsSiegingThisTurn(ActorData caster)
	{
		return caster != null
		       && caster.GetAbilityData() != null
		       && caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ExoAnchorLaser));
	}

	private float GetTechPointToAbsorbConversionRate(bool anchoredAmount)
	{
		return anchoredAmount
			? m_anchoredTechPointToAbsorbConversionRate
			: m_techPointToAbsorbConversionRate;
	}

	public float GetAbsorbToTechPointConversionRate()
	{
		return m_syncComponent != null && m_syncComponent.m_anchored
			? m_anchoredRemainingAbsorbToTechPointConversionRate
			: m_remainingAbsorbToTechPointConversionRate;
	}

	private int GetTechPointForShieldConversion(ActorData caster)
	{
		return GetMaxTechPointsCost() > 0
			? Mathf.Min(GetMaxTechPointsCost(), caster.TechPoints)
			: caster.TechPoints;
	}

	private int GetAbsorbForEnergyToAbsorbConversion(ActorData caster, bool anchoredAmount)
	{
		return Mathf.RoundToInt(GetTechPointForShieldConversion(caster) * GetTechPointToAbsorbConversionRate(anchoredAmount));
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAbsorbEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		if (m_enableTechPointToAbsorbConversion)
		{
			AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 100);
			AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Self, -100);
		}
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		if (moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect)
		{
			moddedEffectForAllies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor != ActorData)
		{
			return symbolToValue;
		}
		int num = GetAbsorbEffect().m_absorbAmount;
		if (m_enableTechPointToAbsorbConversion)
		{
			AddNameplateValueForSingleHit(
				ref symbolToValue,
				Targeter,
				targetActor,
				-GetTechPointForShieldConversion(targetActor),
				AbilityTooltipSymbol.Energy);
			num = GetAbsorbForEnergyToAbsorbConversion(targetActor, WillBeAnchoredDuringCombat());
		}
		if (GetExtraAbsorbIfSieging() > 0 && IsSiegingThisTurn(ActorData))
		{
			num += GetExtraAbsorbIfSieging();
		}
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, num, AbilityTooltipSymbol.Absorb);
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoShield abilityMod_ExoShield = modAsBase as AbilityMod_ExoShield;
		StandardActorEffectData absorbEffect = abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_absorbEffectMod.GetModifiedValue(m_absorbEffect)
			: m_absorbEffect;
		absorbEffect.AddTooltipTokens(tokens, "AbsorbEffect", abilityMod_ExoShield != null, m_absorbEffect);
		AddTokenInt(tokens, "ExtraAbsorbIfSieging", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_extraAbsorbIfSiegingMod.GetModifiedValue(m_extraAbsorbIfSieging)
			: m_extraAbsorbIfSieging);
		AddTokenInt(tokens, "CdrIfShieldNotUsed", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_cdrIfShieldNotUsedMod.GetModifiedValue(m_cdrIfShieldNotUsed)
			: m_cdrIfShieldNotUsed);
		AddTokenInt(tokens, "ShieldLostPerEnergyGain", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_shieldLostPerEnergyGainMod.GetModifiedValue(m_shieldLostPerEnergyGain)
			: m_shieldLostPerEnergyGain);
		AddTokenInt(tokens, "MaxShieldLostForEnergyGain", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_maxShieldLostForEnergyGainMod.GetModifiedValue(m_maxShieldLostForEnergyGain)
			: m_maxShieldLostForEnergyGain);
		AddTokenInt(tokens, "MaxEnergyForShieldConversion", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_maxTechPointsCostMod.GetModifiedValue(m_maxTechPointsCost)
			: m_maxTechPointsCost);
		AddTokenInt(tokens, "MinEnergyForShieldConversion", string.Empty, abilityMod_ExoShield != null
			? abilityMod_ExoShield.m_minTechPointsForCastMod.GetModifiedValue(m_minTechPointsForCast)
			: m_minTechPointsForCast);
		AddTokenInt(tokens, "Max_TP_Cost", "up to this much energy will be converted to absorb", GetMaxTechPointsCost());
		AddTokenInt(tokens, "Min_TP", "the ability can only be cast with at least this much energy", GetMinTechPointsForCast());
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate", "the amount of absorb gained per energy", GetTechPointToAbsorbConversionRate(false)));
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate_Anchored", "the amount of absorb gained per energy while anchored", GetTechPointToAbsorbConversionRate(true)));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate", "the amount of energy re-gained per remaining absorb", m_remainingAbsorbToTechPointConversionRate));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate_Anchored", "the amount of energy re-gained per remaining absorb while anchored", m_anchoredRemainingAbsorbToTechPointConversionRate));
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !m_enableTechPointToAbsorbConversion || caster.TechPoints > GetMinTechPointsForCast();
	}

	public override bool IsFreeAction()
	{
		if (base.IsFreeAction())
		{
			return true;
		}
		return m_freeActionWhileAnchored
		       && m_syncComponent != null
		       && m_syncComponent.m_anchored;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoShield))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoShield;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
