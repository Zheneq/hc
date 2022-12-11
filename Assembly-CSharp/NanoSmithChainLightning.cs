using System.Collections.Generic;
using UnityEngine;

public class NanoSmithChainLightning : Ability
{
	[Separator("Laser/Primary Hit")]
	public int m_laserDamage = 20;
	public StandardEffectInfo m_laserEnemyHitEffect;
	public float m_laserRange = 5f;
	public float m_laserWidth = 1f;
	public bool m_penetrateLos;
	public int m_laserMaxHits = 1;
	[Separator("Chain Lightning")]
	public float m_chainRadius = 3f;
	public int m_chainMaxHits = -1;
	public int m_chainDamage = 10;
	public StandardEffectInfo m_chainEnemyHitEffect;
	public int m_energyGainPerChainHit;
	public bool m_chainCanHitInvisibleActors = true;
	[Separator("Extra Absob for Vacuum Bomb cast target")]
	public int m_extraAbsorbPerHitForVacuumBomb;
	public int m_maxExtraAbsorbForVacuumBomb = 10;
	[Header("-- Sequences")]
	public GameObject m_bounceLaserSequencePrefab;
	public GameObject m_selfHitSequencePrefab;

	private AbilityMod_NanoSmithChainLightning m_abilityMod;
	private StandardEffectInfo m_cachedLaserEnemyHitEffect;
	private StandardEffectInfo m_cachedChainEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Chain Lightning";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_ChainLightningLaser(
				this,
				GetLaserWidth(),
				GetLaserRange(),
				m_penetrateLos,
				GetLaserMaxTargets(),
				false,
				GetChainMaxHits(),
				GetChainRadius());
			return;
		}
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			Targeters.Add(new AbilityUtil_Targeter_ChainLightningLaser(
				this,
				GetLaserWidth(),
				GetLaserRange(),
				m_penetrateLos,
				GetLaserMaxTargets(),
				false,
				GetChainMaxHits(),
				GetChainRadius()));
			Targeters[i].SetUseMultiTargetUpdate(true);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_useTargetDataOverrides
		       && m_abilityMod.m_targetDataOverrides.Length > 1 
			? m_abilityMod.m_targetDataOverrides.Length
			: 1;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		m_cachedLaserEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect)
			: m_laserEnemyHitEffect;
		m_cachedChainEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_chainEnemyHitEffectMod.GetModifiedValue(m_chainEnemyHitEffect)
			: m_chainEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return m_cachedLaserEnemyHitEffect ?? m_laserEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxHitsMod.GetModifiedValue(m_laserMaxHits)
			: m_laserMaxHits;
	}

	public float GetChainRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chainRadiusMod.GetModifiedValue(m_chainRadius)
			: m_chainRadius;
	}

	public int GetChainMaxHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chainMaxHitsMod.GetModifiedValue(m_chainMaxHits)
			: m_chainMaxHits;
	}

	public int GetChainDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chainDamageMod.GetModifiedValue(m_chainDamage)
			: m_chainDamage;
	}

	public int GetEnergyGainPerChainHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyPerChainHitMod.GetModifiedValue(m_energyGainPerChainHit)
			: m_energyGainPerChainHit;
	}

	public StandardEffectInfo GetChainEnemyHitEffect()
	{
		return m_cachedChainEnemyHitEffect ?? m_chainEnemyHitEffect;
	}

	public bool ChainCanHitInvisibleActors()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chainCanHitInvisibleActorsMod.GetModifiedValue(m_chainCanHitInvisibleActors)
			: m_chainCanHitInvisibleActors;
	}

	public int GetExtraAbsorbPerHitForVacuumBomb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbPerHitForVacuumBombMod.GetModifiedValue(m_extraAbsorbPerHitForVacuumBomb)
			: m_extraAbsorbPerHitForVacuumBomb;
	}

	public int GetMaxExtraAbsorbForVacuumBomb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraAbsorbForVacuumBombMod.GetModifiedValue(m_maxExtraAbsorbForVacuumBomb)
			: m_maxExtraAbsorbForVacuumBomb;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamage);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_chainDamage);
		m_laserEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_chainEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamage());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetChainDamage());
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyGainPerChainHit() > 0 && Targeters != null && currentTargeterIndex < Targeters.Count)
		{
			List<ActorData> secondaryTargets = Targeters[currentTargeterIndex].GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Secondary);
			return GetEnergyGainPerChainHit() * secondaryTargets.Count;
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithChainLightning abilityMod_NanoSmithChainLightning = modAsBase as AbilityMod_NanoSmithChainLightning;
		AddTokenInt(tokens, "LaserDamage", string.Empty, abilityMod_NanoSmithChainLightning != null
			? abilityMod_NanoSmithChainLightning.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserEnemyHitEffect, "LaserEnemyHitEffect", m_laserEnemyHitEffect);
		AddTokenInt(tokens, "LaserMaxHits", string.Empty, m_laserMaxHits);
		AddTokenInt(tokens, "ChainMaxHits", string.Empty, abilityMod_NanoSmithChainLightning != null
			? abilityMod_NanoSmithChainLightning.m_chainMaxHitsMod.GetModifiedValue(m_chainMaxHits)
			: m_chainMaxHits);
		AddTokenInt(tokens, "ChainDamage", string.Empty, abilityMod_NanoSmithChainLightning != null
			? abilityMod_NanoSmithChainLightning.m_chainDamageMod.GetModifiedValue(m_chainDamage)
			: m_chainDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_chainEnemyHitEffect, "ChainEnemyHitEffect", m_chainEnemyHitEffect);
		AddTokenInt(tokens, "EnergyGainPerChainHit", string.Empty, abilityMod_NanoSmithChainLightning != null
			? abilityMod_NanoSmithChainLightning.m_energyPerChainHitMod.GetModifiedValue(m_energyGainPerChainHit)
			: m_energyGainPerChainHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NanoSmithChainLightning))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_NanoSmithChainLightning;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
