using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Exo Shield";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_syncComponent = base.GetComponent<Exo_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetTargeterShape(), false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == this.m_animIndexWhenAnchored;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (this.m_syncComponent != null)
		{
			if (this.m_syncComponent.m_anchored)
			{
				return (ActorModelData.ActionAnimationType)this.m_animIndexWhenAnchored;
			}
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		this.m_cachedAbsorbEffect = ((!this.m_abilityMod) ? this.m_absorbEffect : this.m_abilityMod.m_absorbEffectMod.GetModifiedValue(this.m_absorbEffect));
	}

	public StandardActorEffectData GetAbsorbEffect()
	{
		StandardActorEffectData result;
		if (this.m_cachedAbsorbEffect != null)
		{
			result = this.m_cachedAbsorbEffect;
		}
		else
		{
			result = this.m_absorbEffect;
		}
		return result;
	}

	public int GetExtraAbsorbIfSieging()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraAbsorbIfSiegingMod.GetModifiedValue(this.m_extraAbsorbIfSieging);
		}
		else
		{
			result = this.m_extraAbsorbIfSieging;
		}
		return result;
	}

	public int GetCdrIfShieldNotUsed()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cdrIfShieldNotUsedMod.GetModifiedValue(this.m_cdrIfShieldNotUsed);
		}
		else
		{
			result = this.m_cdrIfShieldNotUsed;
		}
		return result;
	}

	public int GetShieldLostPerEnergyGain()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_shieldLostPerEnergyGainMod.GetModifiedValue(this.m_shieldLostPerEnergyGain);
		}
		else
		{
			result = this.m_shieldLostPerEnergyGain;
		}
		return result;
	}

	public int GetMaxShieldLostForEnergyGain()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxShieldLostForEnergyGainMod.GetModifiedValue(this.m_maxShieldLostForEnergyGain);
		}
		else
		{
			result = this.m_maxShieldLostForEnergyGain;
		}
		return result;
	}

	public int GetMaxTechPointsCost()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTechPointsCostMod.GetModifiedValue(this.m_maxTechPointsCost);
		}
		else
		{
			result = this.m_maxTechPointsCost;
		}
		return result;
	}

	public int GetMinTechPointsForCast()
	{
		return (!this.m_abilityMod) ? this.m_minTechPointsForCast : this.m_abilityMod.m_minTechPointsForCastMod.GetModifiedValue(this.m_minTechPointsForCast);
	}

	public bool FreeActionWhileAnchored()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_freeActionWhileAnchoredMod.GetModifiedValue(this.m_freeActionWhileAnchored);
		}
		else
		{
			result = this.m_freeActionWhileAnchored;
		}
		return result;
	}

	public AbilityAreaShape GetTargeterShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_targeterShapeMod.GetModifiedValue(this.m_targeterShape);
		}
		else
		{
			result = this.m_targeterShape;
		}
		return result;
	}

	private bool WillBeAnchoredDuringCombat()
	{
		bool flag;
		if (this.m_syncComponent != null)
		{
			flag = this.m_syncComponent.m_anchored;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3 = base.ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ExoAnchorLaser));
		return flag2 || flag3;
	}

	private bool IsSiegingThisTurn(ActorData caster)
	{
		if (caster != null)
		{
			if (caster.GetAbilityData() != null)
			{
				return caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ExoAnchorLaser));
			}
		}
		return false;
	}

	private float GetTechPointToAbsorbConversionRate(bool anchoredAmount)
	{
		if (anchoredAmount)
		{
			return this.m_anchoredTechPointToAbsorbConversionRate;
		}
		return this.m_techPointToAbsorbConversionRate;
	}

	public float GetAbsorbToTechPointConversionRate()
	{
		if (this.m_syncComponent != null && this.m_syncComponent.m_anchored)
		{
			return this.m_anchoredRemainingAbsorbToTechPointConversionRate;
		}
		return this.m_remainingAbsorbToTechPointConversionRate;
	}

	private int GetTechPointForShieldConversion(ActorData caster)
	{
		if (this.GetMaxTechPointsCost() > 0)
		{
			return Mathf.Min(this.GetMaxTechPointsCost(), caster.TechPoints);
		}
		return caster.TechPoints;
	}

	private int GetAbsorbForEnergyToAbsorbConversion(ActorData caster, bool anchoredAmount)
	{
		return Mathf.RoundToInt((float)this.GetTechPointForShieldConversion(caster) * this.GetTechPointToAbsorbConversionRate(anchoredAmount));
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetAbsorbEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		if (this.m_enableTechPointToAbsorbConversion)
		{
			AbilityTooltipHelper.ReportAbsorb(ref result, AbilityTooltipSubject.Self, 0x64);
			AbilityTooltipHelper.ReportEnergy(ref result, AbilityTooltipSubject.Self, -0x64);
		}
		StandardEffectInfo moddedEffectForAllies = base.GetModdedEffectForAllies();
		if (moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect)
		{
			moddedEffectForAllies.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == base.ActorData)
		{
			int num = this.GetAbsorbEffect().m_absorbAmount;
			if (this.m_enableTechPointToAbsorbConversion)
			{
				Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, -this.GetTechPointForShieldConversion(targetActor), AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Primary);
				num = this.GetAbsorbForEnergyToAbsorbConversion(targetActor, this.WillBeAnchoredDuringCombat());
			}
			if (this.GetExtraAbsorbIfSieging() > 0)
			{
				if (this.IsSiegingThisTurn(base.ActorData))
				{
					num += this.GetExtraAbsorbIfSieging();
				}
			}
			Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, num, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Primary);
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoShield abilityMod_ExoShield = modAsBase as AbilityMod_ExoShield;
		StandardActorEffectData standardActorEffectData;
		if (abilityMod_ExoShield)
		{
			standardActorEffectData = abilityMod_ExoShield.m_absorbEffectMod.GetModifiedValue(this.m_absorbEffect);
		}
		else
		{
			standardActorEffectData = this.m_absorbEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "AbsorbEffect", abilityMod_ExoShield != null, this.m_absorbEffect);
		base.AddTokenInt(tokens, "ExtraAbsorbIfSieging", string.Empty, (!abilityMod_ExoShield) ? this.m_extraAbsorbIfSieging : abilityMod_ExoShield.m_extraAbsorbIfSiegingMod.GetModifiedValue(this.m_extraAbsorbIfSieging), false);
		string name = "CdrIfShieldNotUsed";
		string empty = string.Empty;
		int val;
		if (abilityMod_ExoShield)
		{
			val = abilityMod_ExoShield.m_cdrIfShieldNotUsedMod.GetModifiedValue(this.m_cdrIfShieldNotUsed);
		}
		else
		{
			val = this.m_cdrIfShieldNotUsed;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ShieldLostPerEnergyGain";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ExoShield)
		{
			val2 = abilityMod_ExoShield.m_shieldLostPerEnergyGainMod.GetModifiedValue(this.m_shieldLostPerEnergyGain);
		}
		else
		{
			val2 = this.m_shieldLostPerEnergyGain;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "MaxShieldLostForEnergyGain", string.Empty, (!abilityMod_ExoShield) ? this.m_maxShieldLostForEnergyGain : abilityMod_ExoShield.m_maxShieldLostForEnergyGainMod.GetModifiedValue(this.m_maxShieldLostForEnergyGain), false);
		string name3 = "MaxEnergyForShieldConversion";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ExoShield)
		{
			val3 = abilityMod_ExoShield.m_maxTechPointsCostMod.GetModifiedValue(this.m_maxTechPointsCost);
		}
		else
		{
			val3 = this.m_maxTechPointsCost;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "MinEnergyForShieldConversion";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ExoShield)
		{
			val4 = abilityMod_ExoShield.m_minTechPointsForCastMod.GetModifiedValue(this.m_minTechPointsForCast);
		}
		else
		{
			val4 = this.m_minTechPointsForCast;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "Max_TP_Cost", "up to this much energy will be converted to absorb", this.GetMaxTechPointsCost(), false);
		base.AddTokenInt(tokens, "Min_TP", "the ability can only be cast with at least this much energy", this.GetMinTechPointsForCast(), false);
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate", "the amount of absorb gained per energy", this.GetTechPointToAbsorbConversionRate(false)));
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate_Anchored", "the amount of absorb gained per energy while anchored", this.GetTechPointToAbsorbConversionRate(true)));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate", "the amount of energy re-gained per remaining absorb", this.m_remainingAbsorbToTechPointConversionRate));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate_Anchored", "the amount of energy re-gained per remaining absorb while anchored", this.m_anchoredRemainingAbsorbToTechPointConversionRate));
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_enableTechPointToAbsorbConversion)
		{
			return caster.TechPoints > this.GetMinTechPointsForCast();
		}
		return true;
	}

	public override bool IsFreeAction()
	{
		if (base.IsFreeAction())
		{
			return true;
		}
		if (this.m_freeActionWhileAnchored)
		{
			if (this.m_syncComponent != null)
			{
				return this.m_syncComponent.m_anchored;
			}
		}
		return false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoShield))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ExoShield);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
