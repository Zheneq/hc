using System.Collections.Generic;
using UnityEngine;

public class MartyrRedirectDamageFromAlly : MartyrLaserBase
{
	[Header("-- Targeting")]
	public bool m_canTargetEnemies;
	public bool m_canTargetAllies = true;
	public bool m_canTargetSelf = true;
	public bool m_penetratesLoS;
	public float m_enemyHitRadius = 3f;
	public int m_maxEnemyTargets = 4;
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnAlly = 0.5f;
	public float m_damageRedirectToEnemy = 0.5f;
	public int m_techPointGainPerRedirect = 3;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Absorb & Crystal Bonuses")]
	public int m_baseAbsorb;
	public int m_absorbPerCrystalSpent = 5;
	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_allyHitSequence;
	public GameObject m_enemyHitSequence;
	public GameObject m_reactionHitProjectilePrefab;

	private Martyr_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Redirect Damage From Ally";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, GetEnemyHitRadius(), GetPenetratesLoS(), true, false, GetMaxEnemyTargets());
	}

	private void SetCachedFields()
	{
		m_cachedAllyHitEffect = m_allyHitEffect;
		m_cachedEnemyHitEffect = m_enemyHitEffect;
	}

	public float GetDamageReductionOnAlly()
	{
		return m_damageReductionOnAlly;
	}

	public float GetDamageRedirectToEnemy()
	{
		return m_damageRedirectToEnemy;
	}

	public int GetTechPointGainPerRedirect()
	{
		return m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAbsorbAmountPerCrystalSpent()
	{
		return m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbAmount()
	{
		return m_baseAbsorb;
	}

	public bool GetPenetratesLoS()
	{
		return m_penetratesLoS;
	}

	public float GetEnemyHitRadius()
	{
		return m_enemyHitRadius;
	}

	public int GetMaxEnemyTargets()
	{
		return m_maxEnemyTargets;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyEffect", m_enemyHitEffect);
		tokens.Add(new TooltipTokenInt("BaseAbsorb", "Absorb with no crystal bonus", GetBaseAbsorbAmount()));
		tokens.Add(new TooltipTokenInt("AbsorbPerCrystal", "Absorb added per crystal spent", GetAbsorbAmountPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", GetBonusLengthPerCrystalSpent()));
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor.GetTeam() == ActorData.GetTeam())
		{
			int currentAbsorb = GetCurrentAbsorb(ActorData);
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, currentAbsorb, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, m_canTargetEnemies, m_canTargetAllies, m_canTargetSelf, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(caster, target.GetCurrentBestActorTarget(), m_canTargetEnemies, m_canTargetAllies, m_canTargetSelf, ValidateCheckPath.Ignore, !GetPenetratesLoS(), false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrProtectAllyThreshold current in m_thresholdBasedCrystalBonuses)
		{
			list.Add(current);
		}
		return list;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int additionalAbsorb = martyrProtectAllyThreshold != null ? martyrProtectAllyThreshold.m_additionalAbsorb : 0;
		return GetBaseAbsorbAmount()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbAmountPerCrystalSpent()
			+ additionalAbsorb;
	}
}
