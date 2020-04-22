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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, GetTargeterShape(), false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, true, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return base.CanTriggerAnimAtIndexForTaunt(animIndex) || animIndex == m_animIndexWhenAnchored;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (m_syncComponent != null)
		{
			if (m_syncComponent.m_anchored)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return (ActorModelData.ActionAnimationType)m_animIndexWhenAnchored;
					}
				}
			}
		}
		return base.GetActionAnimType();
	}

	private void SetCachedFields()
	{
		m_cachedAbsorbEffect = ((!m_abilityMod) ? m_absorbEffect : m_abilityMod.m_absorbEffectMod.GetModifiedValue(m_absorbEffect));
	}

	public StandardActorEffectData GetAbsorbEffect()
	{
		StandardActorEffectData result;
		if (m_cachedAbsorbEffect != null)
		{
			result = m_cachedAbsorbEffect;
		}
		else
		{
			result = m_absorbEffect;
		}
		return result;
	}

	public int GetExtraAbsorbIfSieging()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraAbsorbIfSiegingMod.GetModifiedValue(m_extraAbsorbIfSieging);
		}
		else
		{
			result = m_extraAbsorbIfSieging;
		}
		return result;
	}

	public int GetCdrIfShieldNotUsed()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrIfShieldNotUsedMod.GetModifiedValue(m_cdrIfShieldNotUsed);
		}
		else
		{
			result = m_cdrIfShieldNotUsed;
		}
		return result;
	}

	public int GetShieldLostPerEnergyGain()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_shieldLostPerEnergyGainMod.GetModifiedValue(m_shieldLostPerEnergyGain);
		}
		else
		{
			result = m_shieldLostPerEnergyGain;
		}
		return result;
	}

	public int GetMaxShieldLostForEnergyGain()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxShieldLostForEnergyGainMod.GetModifiedValue(m_maxShieldLostForEnergyGain);
		}
		else
		{
			result = m_maxShieldLostForEnergyGain;
		}
		return result;
	}

	public int GetMaxTechPointsCost()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTechPointsCostMod.GetModifiedValue(m_maxTechPointsCost);
		}
		else
		{
			result = m_maxTechPointsCost;
		}
		return result;
	}

	public int GetMinTechPointsForCast()
	{
		return (!m_abilityMod) ? m_minTechPointsForCast : m_abilityMod.m_minTechPointsForCastMod.GetModifiedValue(m_minTechPointsForCast);
	}

	public bool FreeActionWhileAnchored()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_freeActionWhileAnchoredMod.GetModifiedValue(m_freeActionWhileAnchored);
		}
		else
		{
			result = m_freeActionWhileAnchored;
		}
		return result;
	}

	public AbilityAreaShape GetTargeterShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_targeterShapeMod.GetModifiedValue(m_targeterShape);
		}
		else
		{
			result = m_targeterShape;
		}
		return result;
	}

	private bool WillBeAnchoredDuringCombat()
	{
		int num;
		if (m_syncComponent != null)
		{
			num = (m_syncComponent.m_anchored ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool flag2 = base.ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ExoAnchorLaser));
		return flag || flag2;
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_anchoredTechPointToAbsorbConversionRate;
				}
			}
		}
		return m_techPointToAbsorbConversionRate;
	}

	public float GetAbsorbToTechPointConversionRate()
	{
		if (m_syncComponent != null && m_syncComponent.m_anchored)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_anchoredRemainingAbsorbToTechPointConversionRate;
				}
			}
		}
		return m_remainingAbsorbToTechPointConversionRate;
	}

	private int GetTechPointForShieldConversion(ActorData caster)
	{
		if (GetMaxTechPointsCost() > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return Mathf.Min(GetMaxTechPointsCost(), caster.TechPoints);
				}
			}
		}
		return caster.TechPoints;
	}

	private int GetAbsorbForEnergyToAbsorbConversion(ActorData caster, bool anchoredAmount)
	{
		return Mathf.RoundToInt((float)GetTechPointForShieldConversion(caster) * GetTechPointToAbsorbConversionRate(anchoredAmount));
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
		if (targetActor == base.ActorData)
		{
			int num = GetAbsorbEffect().m_absorbAmount;
			if (m_enableTechPointToAbsorbConversion)
			{
				Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, -GetTechPointForShieldConversion(targetActor), AbilityTooltipSymbol.Energy);
				num = GetAbsorbForEnergyToAbsorbConversion(targetActor, WillBeAnchoredDuringCombat());
			}
			if (GetExtraAbsorbIfSieging() > 0)
			{
				if (IsSiegingThisTurn(base.ActorData))
				{
					num += GetExtraAbsorbIfSieging();
				}
			}
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, num, AbilityTooltipSymbol.Absorb);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoShield abilityMod_ExoShield = modAsBase as AbilityMod_ExoShield;
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_ExoShield)
		{
			standardActorEffectData = abilityMod_ExoShield.m_absorbEffectMod.GetModifiedValue(m_absorbEffect);
		}
		else
		{
			standardActorEffectData = m_absorbEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "AbsorbEffect", abilityMod_ExoShield != null, m_absorbEffect);
		AddTokenInt(tokens, "ExtraAbsorbIfSieging", string.Empty, (!abilityMod_ExoShield) ? m_extraAbsorbIfSieging : abilityMod_ExoShield.m_extraAbsorbIfSiegingMod.GetModifiedValue(m_extraAbsorbIfSieging));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ExoShield)
		{
			val = abilityMod_ExoShield.m_cdrIfShieldNotUsedMod.GetModifiedValue(m_cdrIfShieldNotUsed);
		}
		else
		{
			val = m_cdrIfShieldNotUsed;
		}
		AddTokenInt(tokens, "CdrIfShieldNotUsed", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ExoShield)
		{
			val2 = abilityMod_ExoShield.m_shieldLostPerEnergyGainMod.GetModifiedValue(m_shieldLostPerEnergyGain);
		}
		else
		{
			val2 = m_shieldLostPerEnergyGain;
		}
		AddTokenInt(tokens, "ShieldLostPerEnergyGain", empty2, val2);
		AddTokenInt(tokens, "MaxShieldLostForEnergyGain", string.Empty, (!abilityMod_ExoShield) ? m_maxShieldLostForEnergyGain : abilityMod_ExoShield.m_maxShieldLostForEnergyGainMod.GetModifiedValue(m_maxShieldLostForEnergyGain));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ExoShield)
		{
			val3 = abilityMod_ExoShield.m_maxTechPointsCostMod.GetModifiedValue(m_maxTechPointsCost);
		}
		else
		{
			val3 = m_maxTechPointsCost;
		}
		AddTokenInt(tokens, "MaxEnergyForShieldConversion", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ExoShield)
		{
			val4 = abilityMod_ExoShield.m_minTechPointsForCastMod.GetModifiedValue(m_minTechPointsForCast);
		}
		else
		{
			val4 = m_minTechPointsForCast;
		}
		AddTokenInt(tokens, "MinEnergyForShieldConversion", empty4, val4);
		AddTokenInt(tokens, "Max_TP_Cost", "up to this much energy will be converted to absorb", GetMaxTechPointsCost());
		AddTokenInt(tokens, "Min_TP", "the ability can only be cast with at least this much energy", GetMinTechPointsForCast());
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate", "the amount of absorb gained per energy", GetTechPointToAbsorbConversionRate(false)));
		tokens.Add(new TooltipTokenFloat("TP_Absorb_Rate_Anchored", "the amount of absorb gained per energy while anchored", GetTechPointToAbsorbConversionRate(true)));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate", "the amount of energy re-gained per remaining absorb", m_remainingAbsorbToTechPointConversionRate));
		tokens.Add(new TooltipTokenFloat("Absorb_TP_Rate_Anchored", "the amount of energy re-gained per remaining absorb while anchored", m_anchoredRemainingAbsorbToTechPointConversionRate));
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_enableTechPointToAbsorbConversion)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return caster.TechPoints > GetMinTechPointsForCast();
				}
			}
		}
		return true;
	}

	public override bool IsFreeAction()
	{
		if (base.IsFreeAction())
		{
			return true;
		}
		int result;
		if (m_freeActionWhileAnchored)
		{
			if (m_syncComponent != null)
			{
				result = (m_syncComponent.m_anchored ? 1 : 0);
				goto IL_004d;
			}
		}
		result = 0;
		goto IL_004d;
		IL_004d:
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ExoShield))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ExoShield);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
