using System.Collections.Generic;
using UnityEngine;

public class MartyrHealOverTime : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetAlly = true;
	public bool m_targetingPenetrateLos;
	public int m_healBase = 5;
	public int m_healPerCrystal = 3;
	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public StandardActorEffectData m_healEffectData;
	[Header("-- Extra healing if has Aoe on React effect")]
	public int m_extraHealingIfHasAoeOnReact;
	[Header("-- Extra Effect for low health --")]
	public bool m_onlyAddExtraEffecForFirstTurn;
	public float m_lowHealthThreshold;
	public StandardEffectInfo m_extraEffectForLowHealth;
	[Header("-- Heal/Effect on Caster if targeting Ally")]
	public int m_baseSelfHealIfTargetAlly;
	public int m_selfHealPerCrystalIfTargetAlly;
	public bool m_addHealEffectOnSelfIfTargetAlly;
	public StandardActorEffectData m_healEffectOnSelfIfTargetAlly;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private Martyr_SyncComponent m_syncComponent;
	private AbilityMod_MartyrHealOverTime m_abilityMod;
	private StandardActorEffectData m_cachedHealEffectData;
	private StandardEffectInfo m_cachedExtraEffectForLowHealth;
	private StandardActorEffectData m_cachedHealEffectOnSelfIfTargetAlly;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MartyrHealOverTime";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComponent == null)
		{
			m_syncComponent = GetComponent<Martyr_SyncComponent>();
		}
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = HasSelfHitIfTargetingAlly() ? AbilityUtil_Targeter.AffectsActor.Always : AbilityUtil_Targeter.AffectsActor.Possible;
		Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, CanTargetAlly(), affectsCaster);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedHealEffectData = m_abilityMod
			? m_abilityMod.m_healEffectDataMod.GetModifiedValue(m_healEffectData)
			: m_healEffectData;
		m_cachedExtraEffectForLowHealth = m_abilityMod
			? m_abilityMod.m_extraEffectForLowHealthMod.GetModifiedValue(m_extraEffectForLowHealth)
			: m_extraEffectForLowHealth;
		m_cachedHealEffectOnSelfIfTargetAlly = m_abilityMod
			? m_abilityMod.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_healEffectOnSelfIfTargetAlly)
			: m_healEffectOnSelfIfTargetAlly;
	}

	public bool CanTargetAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly)
			: m_canTargetAlly;
	}

	public bool TargetingPenetrateLos()
	{
		return m_abilityMod
			? m_abilityMod.m_targetingPenetrateLosMod.GetModifiedValue(m_targetingPenetrateLos)
			: m_targetingPenetrateLos;
	}

	public StandardActorEffectData GetHealEffectData()
	{
		return m_cachedHealEffectData ?? m_healEffectData;
	}

	public int GetHealBase()
	{
		return m_abilityMod
			? m_abilityMod.m_healBaseMod.GetModifiedValue(m_healBase)
			: m_healBase;
	}

	public int GetHealPerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_healPerCrystalMod.GetModifiedValue(m_healPerCrystal)
			: m_healPerCrystal;
	}

	public int GetExtraHealingIfHasAoeOnReact()
	{
		return m_abilityMod
			? m_abilityMod.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(m_extraHealingIfHasAoeOnReact)
			: m_extraHealingIfHasAoeOnReact;
	}

	public bool OnlyAddExtraEffecForFirstTurn()
	{
		return m_abilityMod
			? m_abilityMod.m_onlyAddExtraEffecForFirstTurnMod.GetModifiedValue(m_onlyAddExtraEffecForFirstTurn)
			: m_onlyAddExtraEffecForFirstTurn;
	}

	public float GetLowHealthThreshold()
	{
		return m_abilityMod
			? m_abilityMod.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold)
			: m_lowHealthThreshold;
	}

	public StandardEffectInfo GetExtraEffectForLowHealth()
	{
		return m_cachedExtraEffectForLowHealth ?? m_extraEffectForLowHealth;
	}

	public int GetBaseSelfHealIfTargetAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(m_baseSelfHealIfTargetAlly)
			: m_baseSelfHealIfTargetAlly;
	}

	public int GetSelfHealPerCrystalIfTargetAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(m_selfHealPerCrystalIfTargetAlly)
			: m_selfHealPerCrystalIfTargetAlly;
	}

	public bool AddHealEffectOnSelfIfTargetAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_addHealEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_addHealEffectOnSelfIfTargetAlly)
			: m_addHealEffectOnSelfIfTargetAlly;
	}

	public StandardActorEffectData GetHealEffectOnSelfIfTargetAlly()
	{
		return m_cachedHealEffectOnSelfIfTargetAlly ?? m_healEffectOnSelfIfTargetAlly;
	}

	public int GetCurrentHealing(ActorData caster)
	{
		return GetHealBase() + GetHealPerCrystal() * m_syncComponent.SpentDamageCrystals(caster);
	}

	public int GetSelfHealingIfTargetingAlly(ActorData caster)
	{
		int num = GetBaseSelfHealIfTargetAlly();
		if (GetSelfHealPerCrystalIfTargetAlly() > 0)
		{
			num += GetSelfHealPerCrystalIfTargetAlly() * m_syncComponent.SpentDamageCrystals(caster);
		}
		return num;
	}

	public bool HasSelfHitIfTargetingAlly()
	{
		return GetBaseSelfHealIfTargetAlly() > 0 || GetSelfHealPerCrystalIfTargetAlly() > 0 || AddHealEffectOnSelfIfTargetAlly();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, false, CanTargetAlly(), true, ValidateCheckPath.Ignore, TargetingPenetrateLos(), true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Primary, 1);
		AbilityTooltipHelper.ReportAbsorb(ref number, AbilityTooltipSubject.Primary, 1);
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}

		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
		if (ActorData != null
			&& ActorData == targetActor
			&& visibleActorsCountByTooltipSubject > 0)
		{
			int healing = GetSelfHealingIfTargetingAlly(ActorData);
			if (m_syncComponent != null
				&& m_syncComponent.ActorHasAoeOnReactEffect(targetActor)
				&& GetExtraHealingIfHasAoeOnReact() > 0)
			{
				healing += GetExtraHealingIfHasAoeOnReact();
			}
			dictionary[AbilityTooltipSymbol.Healing] = healing;
			dictionary[AbilityTooltipSymbol.Absorb] = 0;
		}
		else
		{
			int healing = GetCurrentHealing(ActorData);
			if (m_syncComponent != null
				&& m_syncComponent.ActorHasAoeOnReactEffect(targetActor)
				&& GetExtraHealingIfHasAoeOnReact() > 0)
			{
				healing += GetExtraHealingIfHasAoeOnReact();
			}
			dictionary[AbilityTooltipSymbol.Healing] = healing;
			dictionary[AbilityTooltipSymbol.Absorb] = 0;
			if (GetLowHealthThreshold() > 0f
				&& targetActor.GetHitPointPercent() <= GetLowHealthThreshold())
			{
				StandardEffectInfo extraEffectForLowHealth = GetExtraEffectForLowHealth();
				if (extraEffectForLowHealth.m_applyEffect)
				{
					dictionary[AbilityTooltipSymbol.Absorb] = extraEffectForLowHealth.m_effectData.m_absorbAmount;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrHealOverTime abilityMod_MartyrHealOverTime = modAsBase as AbilityMod_MartyrHealOverTime;
		int healBase = abilityMod_MartyrHealOverTime ? abilityMod_MartyrHealOverTime.m_healBaseMod.GetModifiedValue(m_healBase) : m_healBase;
		AddTokenInt(tokens, "HealBase", "", healBase);
		int healPerCrystal = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_healPerCrystalMod.GetModifiedValue(m_healPerCrystal)
			: m_healPerCrystal;
		AddTokenInt(tokens, "HealPerCrystal", "", healPerCrystal);
		StandardActorEffectData healEffectData = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_healEffectDataMod.GetModifiedValue(m_healEffectData)
			: m_healEffectData;
		healEffectData.AddTooltipTokens(tokens, "HealEffectData", abilityMod_MartyrHealOverTime != null, m_healEffectData);
		int extraHealingIfHasAoeOnReact = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_extraHealingIfHasAoeOnReactMod.GetModifiedValue(m_extraHealingIfHasAoeOnReact)
			: m_extraHealingIfHasAoeOnReact;
		AddTokenInt(tokens, "ExtraHealingIfHasAoeOnReact", "", extraHealingIfHasAoeOnReact);
		float lowHealthThreshold_Pct = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_lowHealthThresholdMod.GetModifiedValue(m_lowHealthThreshold)
			: m_lowHealthThreshold;
		AddTokenFloatAsPct(tokens, "LowHealthThreshold_Pct", "", lowHealthThreshold_Pct);
		StandardEffectInfo extraEffectForLowHealth = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_extraEffectForLowHealthMod.GetModifiedValue(m_extraEffectForLowHealth)
			: m_extraEffectForLowHealth;
		AbilityMod.AddToken_EffectInfo(tokens, extraEffectForLowHealth, "ExtraEffectForLowHealth", m_extraEffectForLowHealth);
		int baseSelfHealIfTargetAlly = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_baseSelfHealIfTargetAllyMod.GetModifiedValue(m_baseSelfHealIfTargetAlly)
			: m_baseSelfHealIfTargetAlly;
		AddTokenInt(tokens, "BaseSelfHealIfTargetAlly", "", baseSelfHealIfTargetAlly);
		int selfHealPerCrystalIfTargetAlly = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_selfHealPerCrystalIfTargetAllyMod.GetModifiedValue(m_selfHealPerCrystalIfTargetAlly)
			: m_selfHealPerCrystalIfTargetAlly;
		AddTokenInt(tokens, "SelfHealPerCrystalIfTargetAlly", "", selfHealPerCrystalIfTargetAlly);
		StandardActorEffectData healEffectOnSelfIfTargetAlly = abilityMod_MartyrHealOverTime
			? abilityMod_MartyrHealOverTime.m_healEffectOnSelfIfTargetAllyMod.GetModifiedValue(m_healEffectOnSelfIfTargetAlly)
			: m_healEffectOnSelfIfTargetAlly;
		healEffectOnSelfIfTargetAlly.AddTooltipTokens(tokens, "HealEffectOnSelfIfTargetAlly", abilityMod_MartyrHealOverTime != null, m_healEffectOnSelfIfTargetAlly);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrHealOverTime))
		{
			m_abilityMod = abilityMod as AbilityMod_MartyrHealOverTime;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
