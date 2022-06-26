using System.Collections.Generic;
using UnityEngine;

public class MartyrSpendCrystals : Ability
{
	public enum TargetingMode
	{
		OnSelf,
		Aoe
	}

	public TargetingMode m_targetingMode;
	[Header("-- Self Healing & Absorb")]
	public StandardEffectInfo m_spentCrystalsEffect;
	[Header("-- Self Healing")]
	public int m_selfHealBase;
	public int m_selfHealPerCrystalSpent;
	public int m_selfHealPerEnemyHit;
	public bool m_selfHealIsOverTime = true;
	[Space(10f)]
	public int m_extraSelfHealPerTurnAtMaxEnergy;
	public int m_maxExtraSelfHealForMaxEnergy;
	[Header("-- Self Absorb")]
	public int m_selfAbsorbBase;
	public int m_selfAbsorbPerCrystalSpent;
	[Header("-- Enemy Hit (if using AoE targeting)")]
	public float m_aoeRadiusBase = 2f;
	public float m_aoeRadiuePerCrystal = 0.5f;
	public bool m_penetrateLos;
	public int m_damageBase;
	public int m_damagePerCrystal;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Ally Hit (if using AoE targeting")]
	public int m_allyHealBase;
	public int m_allyHealPerCrystal;
	public int m_allyHealPerEnemyHit;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Energy Use --")]
	public bool m_clearEnergyOnCast = true;
	public int m_selfEnergyGainOnCast;
	[Header("-- Cooldown Reduction on other abilities")]
	public AbilityData.ActionType m_protectAllyActionType = AbilityData.ActionType.INVALID_ACTION;
	public int m_cdrOnProtectAllyAbility;
	[Header("-- Sequences")]
	public GameObject m_castSequence;

	private Martyr_SyncComponent m_syncComponent;
	private AbilityMod_MartyrSpendCrystals m_abilityMod;
	private StandardEffectInfo m_cachedSpentCrystalsEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Spend Crystals";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		if (m_targetingMode == TargetingMode.OnSelf)
		{
			Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always)
			{
				ShowArcToShape = false
			};
		}
		else
		{
			AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, 1f, PenetrateLos(), IncludeEnemies(), IncludeAllies());
			abilityUtil_Targeter_AoE_Smooth.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), true);
			abilityUtil_Targeter_AoE_Smooth.m_customRadiusDelegate = GetTargeterRadius;
			Targeter = abilityUtil_Targeter_AoE_Smooth;
		}
	}

	private float GetTargeterRadius(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return GetCurrentAoeRadius(targetingActor);
	}

	private void SetCachedFields()
	{
		m_cachedSpentCrystalsEffect = m_abilityMod
			? m_abilityMod.m_spentCrystalsEffectMod.GetModifiedValue(m_spentCrystalsEffect)
			: m_spentCrystalsEffect;
		m_cachedEnemyHitEffect = m_abilityMod
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedAllyHitEffect = m_abilityMod
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public StandardEffectInfo GetSpentCrystalsEffect()
	{
		return m_cachedSpentCrystalsEffect ?? m_spentCrystalsEffect;
	}

	public int GetSelfHealBase()
	{
		return m_abilityMod
			? m_abilityMod.m_selfHealBaseMod.GetModifiedValue(m_selfHealBase)
			: m_selfHealBase;
	}

	public int GetSelfHealPerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_selfHealPerCrystalSpentMod.GetModifiedValue(m_selfHealPerCrystalSpent)
			: m_selfHealPerCrystalSpent;
	}

	public int GetSelfHealPerEnemyHit()
	{
		return m_abilityMod
			? m_abilityMod.m_selfHealPerEnemyHitMod.GetModifiedValue(m_selfHealPerEnemyHit)
			: m_selfHealPerEnemyHit;
	}

	public bool SelfHealIsOverTime()
	{
		return m_abilityMod
			? m_abilityMod.m_selfHealIsOverTimeMod.GetModifiedValue(m_selfHealIsOverTime)
			: m_selfHealIsOverTime;
	}

	public int GetExtraSelfHealPerTurnAtMaxEnergy()
	{
		return m_abilityMod
			? m_abilityMod.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(m_extraSelfHealPerTurnAtMaxEnergy)
			: m_extraSelfHealPerTurnAtMaxEnergy;
	}

	public int GetMaxExtraSelfHealForMaxEnergy()
	{
		return m_abilityMod
			? m_abilityMod.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(m_maxExtraSelfHealForMaxEnergy)
			: m_maxExtraSelfHealForMaxEnergy;
	}

	public int GetSelfAbsorbBase()
	{
		return m_abilityMod
			? m_abilityMod.m_selfAbsorbBaseMod.GetModifiedValue(m_selfAbsorbBase)
			: m_selfAbsorbBase;
	}

	public int GetSelfAbsorbPerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(m_selfAbsorbPerCrystalSpent)
			: m_selfAbsorbPerCrystalSpent;
	}

	public float GetAoeRadiusBase()
	{
		return m_abilityMod
			? m_abilityMod.m_aoeRadiusBaseMod.GetModifiedValue(m_aoeRadiusBase)
			: m_aoeRadiusBase;
	}

	public float GetAoeRadiuePerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_aoeRadiuePerCrystalMod.GetModifiedValue(m_aoeRadiuePerCrystal)
			: m_aoeRadiuePerCrystal;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetDamageBase()
	{
		return m_abilityMod
			? m_abilityMod.m_damageBaseMod.GetModifiedValue(m_damageBase)
			: m_damageBase;
	}

	public int GetDamagePerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_damagePerCrystalMod.GetModifiedValue(m_damagePerCrystal)
			: m_damagePerCrystal;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAllyHealBase()
	{
		return m_abilityMod
			? m_abilityMod.m_allyHealBaseMod.GetModifiedValue(m_allyHealBase)
			: m_allyHealBase;
	}

	public int GetAllyHealPerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_allyHealPerCrystalMod.GetModifiedValue(m_allyHealPerCrystal)
			: m_allyHealPerCrystal;
	}

	public int GetAllyHealPerEnemyHit()
	{
		return m_abilityMod
			? m_abilityMod.m_allyHealPerEnemyHitMod.GetModifiedValue(m_allyHealPerEnemyHit)
			: m_allyHealPerEnemyHit;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool ClearEnergyOnCast()
	{
		return m_abilityMod
			? m_abilityMod.m_clearEnergyOnCastMod.GetModifiedValue(m_clearEnergyOnCast)
			: m_clearEnergyOnCast;
	}

	public int GetSelfEnergyGainOnCast()
	{
		return m_abilityMod
			? m_abilityMod.m_selfEnergyGainOnCastMod.GetModifiedValue(m_selfEnergyGainOnCast)
			: m_selfEnergyGainOnCast;
	}

	public int GetCdrOnProtectAllyAbility()
	{
		return m_abilityMod
			? m_abilityMod.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(m_cdrOnProtectAllyAbility)
			: m_cdrOnProtectAllyAbility;
	}

	public float GetCurrentAoeRadius(ActorData caster)
	{
		float extraRaduis = 0f;
		if (m_syncComponent != null)
		{
			extraRaduis = GetAoeRadiuePerCrystal() * m_syncComponent.SpentDamageCrystals(caster);
		}
		return GetAoeRadiusBase() + extraRaduis;
	}

	public bool IncludeEnemies()
	{
		return GetDamageBase() > 0 || GetDamagePerCrystal() > 0 || GetEnemyHitEffect().m_applyEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHealBase() > 0 || GetAllyHealPerCrystal() > 0 || GetAllyHitEffect().m_applyEffect;
	}

	private int GetCurrentAbsorbOnSelf(ActorData caster)
	{
		return GetSelfAbsorbBase() + m_syncComponent.SpentDamageCrystals(caster) * GetSelfAbsorbPerCrystalSpent();
	}

	private int GetCurrentHealingOnSelf(ActorData caster, int numEnemiesHit)
	{
		int extraSelfHeal = 0;
		if (GetSelfHealPerEnemyHit() > 0)
		{
			extraSelfHeal += GetSelfHealPerEnemyHit() * numEnemiesHit;
		}
		if (GetExtraSelfHealPerTurnAtMaxEnergy() > 0 && m_syncComponent != null && m_syncComponent.m_syncNumTurnsAtFullEnergy > 1)
		{
			int extraSelfHealForMaxEnergy = GetExtraSelfHealPerTurnAtMaxEnergy() * (m_syncComponent.m_syncNumTurnsAtFullEnergy - 1);
			if (GetMaxExtraSelfHealForMaxEnergy() > 0)
			{
				extraSelfHealForMaxEnergy = Mathf.Min(GetMaxExtraSelfHealForMaxEnergy(), extraSelfHealForMaxEnergy);
			}
			extraSelfHeal += extraSelfHealForMaxEnergy;
		}
		return GetSelfHealBase()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetSelfHealPerCrystalSpent()
			+ extraSelfHeal;
	}

	private int GetCurrentHealingOnAlly(ActorData caster, int numEnemiesHit)
	{
		int extraAllyHeal = 0;
		if (GetSelfHealPerEnemyHit() > 0)
		{
			extraAllyHeal = GetAllyHealPerEnemyHit() * numEnemiesHit;
		}
		return GetAllyHealBase()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAllyHealPerCrystal()
			+ extraAllyHeal;
	}

	private int GetCurrentDamage(ActorData caster)
	{
		return GetDamageBase() + m_syncComponent.SpentDamageCrystals(caster) * GetDamagePerCrystal();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrSpendCrystals abilityMod_MartyrSpendCrystals = modAsBase as AbilityMod_MartyrSpendCrystals;
		StandardEffectInfo spentCrystalsEffect = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_spentCrystalsEffectMod.GetModifiedValue(m_spentCrystalsEffect)
			: m_spentCrystalsEffect;
		AbilityMod.AddToken_EffectInfo(tokens, spentCrystalsEffect, "SpentCrystalsEffect", m_spentCrystalsEffect);
		int selfHealBase = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfHealBaseMod.GetModifiedValue(m_selfHealBase)
			: m_selfHealBase;
		AddTokenInt(tokens, "SelfHealBase", "", selfHealBase);
		int selfHealPerCrystalSpent = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfHealPerCrystalSpentMod.GetModifiedValue(m_selfHealPerCrystalSpent)
			: m_selfHealPerCrystalSpent;
		AddTokenInt(tokens, "SelfHealPerCrystalSpent", "", selfHealPerCrystalSpent);
		int selfHealPerEnemyHit = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfHealPerEnemyHitMod.GetModifiedValue(m_selfHealPerEnemyHit) 
			: m_selfHealPerEnemyHit;
		AddTokenInt(tokens, "SelfHealPerEnemyHit", "", selfHealPerEnemyHit);
		int extraSelfHealPerTurnAtMaxEnergy = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergyMod.GetModifiedValue(m_extraSelfHealPerTurnAtMaxEnergy)
			: m_extraSelfHealPerTurnAtMaxEnergy;
		AddTokenInt(tokens, "ExtraSelfHealPerTurnAtMaxEnergy", "", extraSelfHealPerTurnAtMaxEnergy);
		int maxExtraSelfHealForMaxEnergy = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_maxExtraSelfHealForMaxEnergyMod.GetModifiedValue(m_maxExtraSelfHealForMaxEnergy)
			: m_maxExtraSelfHealForMaxEnergy;
		AddTokenInt(tokens, "MaxExtraSelfHealForMaxEnergy", "", maxExtraSelfHealForMaxEnergy);
		int selfAbsorbBase = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfAbsorbBaseMod.GetModifiedValue(m_selfAbsorbBase)
			: m_selfAbsorbBase;
		AddTokenInt(tokens, "SelfAbsorbBase", "", selfAbsorbBase);
		int selfAbsorbPerCrystalSpent = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfAbsorbPerCrystalSpentMod.GetModifiedValue(m_selfAbsorbPerCrystalSpent)
			: m_selfAbsorbPerCrystalSpent;
		AddTokenInt(tokens, "SelfAbsorbPerCrystalSpent", "", selfAbsorbPerCrystalSpent);
		float aoeRadiusBase = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_aoeRadiusBaseMod.GetModifiedValue(m_aoeRadiusBase)
			: m_aoeRadiusBase;
		AddTokenFloat(tokens, "AoeRadiusBase", "", aoeRadiusBase);
		float aoeRadiuePerCrystal = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_aoeRadiuePerCrystalMod.GetModifiedValue(m_aoeRadiuePerCrystal)
			: m_aoeRadiuePerCrystal;
		AddTokenFloat(tokens, "AoeRadiuePerCrystal", "", aoeRadiuePerCrystal);
		int damageBase = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_damageBaseMod.GetModifiedValue(m_damageBase)
			: m_damageBase;
		AddTokenInt(tokens, "DamageBase", "", damageBase);
		int damagePerCrystal = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_damagePerCrystalMod.GetModifiedValue(m_damagePerCrystal)
			: m_damagePerCrystal;
		AddTokenInt(tokens, "DamagePerCrystal", "", damagePerCrystal);
		StandardEffectInfo enemyHitEffect = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		int allyHealBase = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_allyHealBaseMod.GetModifiedValue(m_allyHealBase)
			: m_allyHealBase;
		AddTokenInt(tokens, "AllyHealBase", "", allyHealBase);
		int allyHealPerCrystalMod = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_allyHealPerCrystalMod.GetModifiedValue(m_allyHealPerCrystal)
			: m_allyHealPerCrystal;
		AddTokenInt(tokens, "AllyHealPerCrystal", "", allyHealPerCrystalMod);
		int allyHealPerEnemyHit = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_allyHealPerEnemyHitMod.GetModifiedValue(m_allyHealPerEnemyHit)
			: m_allyHealPerEnemyHit;
		AddTokenInt(tokens, "AllyHealPerEnemyHit", "", allyHealPerEnemyHit);
		StandardEffectInfo allyHitEffect = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		int selfEnergyGainOnCast = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_selfEnergyGainOnCastMod.GetModifiedValue(m_selfEnergyGainOnCast)
			: m_selfEnergyGainOnCast;
		AddTokenInt(tokens, "SelfEnergyGainOnCast", "", selfEnergyGainOnCast);
		int cdrOnProtectAllyAbility = abilityMod_MartyrSpendCrystals
			? abilityMod_MartyrSpendCrystals.m_cdrOnProtectAllyAbilityMod.GetModifiedValue(m_cdrOnProtectAllyAbility)
			: m_cdrOnProtectAllyAbility;
		AddTokenInt(tokens, "CdrOnProtectAllyAbility", "", cdrOnProtectAllyAbility);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetSpentCrystalsEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, 1);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		ActorData actorData = ActorData;
		int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		if (targetActor == actorData)
		{
			int currentAbsorbOnSelf = GetCurrentAbsorbOnSelf(actorData);
			int currentHealingOnSelf = GetCurrentHealingOnSelf(actorData, visibleActorsCountByTooltipSubject);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, actorData, currentAbsorbOnSelf, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, actorData, currentHealingOnSelf, AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self);
		}
		else
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
				{
					symbolToValue[AbilityTooltipSymbol.Damage] = GetCurrentDamage(actorData);
				}
				else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
				{
					symbolToValue[AbilityTooltipSymbol.Healing] = GetCurrentHealingOnAlly(actorData, visibleActorsCountByTooltipSubject);
				}
			}
		}
		return symbolToValue;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetSelfEnergyGainOnCast() > 0)
		{
			return GetSelfEnergyGainOnCast();
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComponent != null && m_syncComponent.DamageCrystals > 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrSpendCrystals))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrSpendCrystals);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
