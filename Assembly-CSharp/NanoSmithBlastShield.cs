using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBlastShield : Ability
{
	[Header("-- Shield Effect")]
	public StandardActorEffectData m_shieldEffect;
	public int m_healOnEndIfHasRemainingAbsorb;
	public int m_energyGainOnShieldTarget;
	[Header("-- Extra Effect on Caster if targeting Ally")]
	public StandardEffectInfo m_extraEffectOnCasterIfTargetingAlly;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_shieldSequencePrefab;

	private bool m_allowOnEnemy;
	private AbilityMod_NanoSmithBlastShield m_abilityMod;
	private StandardEffectInfo m_cachedExtraEffectOnCasterIfTargetingAlly;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Blast Shield";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect
			? AbilityUtil_Targeter.AffectsActor.Always
			: AbilityUtil_Targeter.AffectsActor.Possible;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			m_allowOnEnemy,
			true,
			affectsCaster);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedExtraEffectOnCasterIfTargetingAlly = m_abilityMod != null
			? m_abilityMod.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(m_extraEffectOnCasterIfTargetingAlly)
			: m_extraEffectOnCasterIfTargetingAlly;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldEffectOverride.GetModifiedValue(m_shieldEffect)
			: m_shieldEffect;
	}

	public int GetHealOnEndIfHasRemainingAbsorb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(m_healOnEndIfHasRemainingAbsorb)
			: m_healOnEndIfHasRemainingAbsorb;
	}

	public int GetEnergyGainOnShieldTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainOnShieldTargetMod.GetModifiedValue(m_energyGainOnShieldTarget)
			: m_energyGainOnShieldTarget;
	}

	public StandardEffectInfo GetExtraEffectOnCasterIfTargetingAlly()
	{
		return m_cachedExtraEffectOnCasterIfTargetingAlly ?? m_extraEffectOnCasterIfTargetingAlly;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetShieldEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Primary, GetEnergyGainOnShieldTarget());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (!GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (ActorData != null && ActorData == targetActor)
		{
			int allies = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			if (allies > 0)
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				dictionary[AbilityTooltipSymbol.Absorb] = GetExtraEffectOnCasterIfTargetingAlly().m_effectData.m_absorbAmount;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energyGainOnShieldTarget = GetEnergyGainOnShieldTarget();
		if (energyGainOnShieldTarget > 0)
		{
			BoardSquare square = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
			if (square != null && square.OccupantActor == caster)
			{
				return energyGainOnShieldTarget;
			}
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			m_allowOnEnemy,
			true, 
			true,
			ValidateCheckPath.Ignore,
			true,
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBlastShield abilityMod_NanoSmithBlastShield = modAsBase as AbilityMod_NanoSmithBlastShield;
		StandardActorEffectData shieldEffect = abilityMod_NanoSmithBlastShield != null
			? abilityMod_NanoSmithBlastShield.m_shieldEffectOverride.GetModifiedValue(m_shieldEffect)
			: m_shieldEffect;
		shieldEffect.AddTooltipTokens(tokens, "ShieldEffect", abilityMod_NanoSmithBlastShield != null, m_shieldEffect);
		AddTokenInt(tokens, "HealOnEndIfHasRemainingAbsorb", string.Empty, abilityMod_NanoSmithBlastShield != null
			? abilityMod_NanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(m_healOnEndIfHasRemainingAbsorb)
			: m_healOnEndIfHasRemainingAbsorb);
		AddTokenInt(tokens, "EnergyGainOnShieldTarget", string.Empty, abilityMod_NanoSmithBlastShield != null
			? abilityMod_NanoSmithBlastShield.m_energyGainOnShieldTargetMod.GetModifiedValue(m_energyGainOnShieldTarget)
			: m_energyGainOnShieldTarget);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithBlastShield != null
			? abilityMod_NanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(m_extraEffectOnCasterIfTargetingAlly)
			: m_extraEffectOnCasterIfTargetingAlly, "ExtraEffectOnCasterIfTargetingAlly", m_extraEffectOnCasterIfTargetingAlly);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithBlastShield))
		{
			m_abilityMod = abilityMod as AbilityMod_NanoSmithBlastShield;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
