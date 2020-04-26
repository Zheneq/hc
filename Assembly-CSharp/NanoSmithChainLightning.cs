using System.Collections.Generic;
using UnityEngine;

public class NanoSmithChainLightning : Ability
{
	[Separator("Laser/Primary Hit", true)]
	public int m_laserDamage = 20;

	public StandardEffectInfo m_laserEnemyHitEffect;

	public float m_laserRange = 5f;

	public float m_laserWidth = 1f;

	public bool m_penetrateLos;

	public int m_laserMaxHits = 1;

	[Separator("Chain Lightning", true)]
	public float m_chainRadius = 3f;

	public int m_chainMaxHits = -1;

	public int m_chainDamage = 10;

	public StandardEffectInfo m_chainEnemyHitEffect;

	public int m_energyGainPerChainHit;

	public bool m_chainCanHitInvisibleActors = true;

	[Separator("Extra Absob for Vacuum Bomb cast target", true)]
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_ChainLightningLaser(this, GetLaserWidth(), GetLaserRange(), m_penetrateLos, GetLaserMaxTargets(), false, GetChainMaxHits(), GetChainRadius());
					return;
				}
			}
		}
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			base.Targeters.Add(new AbilityUtil_Targeter_ChainLightningLaser(this, GetLaserWidth(), GetLaserRange(), m_penetrateLos, GetLaserMaxTargets(), false, GetChainMaxHits(), GetChainRadius()));
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useTargetDataOverrides)
			{
				if (m_abilityMod.m_targetDataOverrides.Length > 1)
				{
					result = m_abilityMod.m_targetDataOverrides.Length;
				}
			}
		}
		return result;
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
		StandardEffectInfo cachedLaserEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedLaserEnemyHitEffect = m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = m_laserEnemyHitEffect;
		}
		m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
		m_cachedChainEnemyHitEffect = ((!m_abilityMod) ? m_chainEnemyHitEffect : m_abilityMod.m_chainEnemyHitEffectMod.GetModifiedValue(m_chainEnemyHitEffect));
	}

	public int GetLaserDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_laserDamage;
		}
		else
		{
			result = m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserEnemyHitEffect != null)
		{
			result = m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = m_laserEnemyHitEffect;
		}
		return result;
	}

	public float GetLaserRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!m_abilityMod) ? m_laserWidth : m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public bool PenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetLaserMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserMaxHitsMod.GetModifiedValue(m_laserMaxHits);
		}
		else
		{
			result = m_laserMaxHits;
		}
		return result;
	}

	public float GetChainRadius()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_chainRadius;
		}
		else
		{
			result = m_abilityMod.m_chainRadiusMod.GetModifiedValue(m_chainRadius);
		}
		return result;
	}

	public int GetChainMaxHits()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_chainMaxHits;
		}
		else
		{
			result = m_abilityMod.m_chainMaxHitsMod.GetModifiedValue(m_chainMaxHits);
		}
		return result;
	}

	public int GetChainDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_chainDamageMod.GetModifiedValue(m_chainDamage) : m_chainDamage;
	}

	public int GetEnergyGainPerChainHit()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_energyGainPerChainHit;
		}
		else
		{
			result = m_abilityMod.m_energyPerChainHitMod.GetModifiedValue(m_energyGainPerChainHit);
		}
		return result;
	}

	public StandardEffectInfo GetChainEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedChainEnemyHitEffect != null)
		{
			result = m_cachedChainEnemyHitEffect;
		}
		else
		{
			result = m_chainEnemyHitEffect;
		}
		return result;
	}

	public bool ChainCanHitInvisibleActors()
	{
		return (!m_abilityMod) ? m_chainCanHitInvisibleActors : m_abilityMod.m_chainCanHitInvisibleActorsMod.GetModifiedValue(m_chainCanHitInvisibleActors);
	}

	public int GetExtraAbsorbPerHitForVacuumBomb()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraAbsorbPerHitForVacuumBombMod.GetModifiedValue(m_extraAbsorbPerHitForVacuumBomb);
		}
		else
		{
			result = m_extraAbsorbPerHitForVacuumBomb;
		}
		return result;
	}

	public int GetMaxExtraAbsorbForVacuumBomb()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxExtraAbsorbForVacuumBombMod.GetModifiedValue(m_maxExtraAbsorbForVacuumBomb);
		}
		else
		{
			result = m_maxExtraAbsorbForVacuumBomb;
		}
		return result;
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
		if (GetEnergyGainPerChainHit() > 0 && base.Targeters != null && currentTargeterIndex < base.Targeters.Count)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeters[currentTargeterIndex].GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Secondary);
			return GetEnergyGainPerChainHit() * visibleActorsInRangeByTooltipSubject.Count;
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithChainLightning abilityMod_NanoSmithChainLightning = modAsBase as AbilityMod_NanoSmithChainLightning;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_NanoSmithChainLightning)
		{
			val = abilityMod_NanoSmithChainLightning.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		else
		{
			val = m_laserDamage;
		}
		AddTokenInt(tokens, "LaserDamage", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserEnemyHitEffect, "LaserEnemyHitEffect", m_laserEnemyHitEffect);
		AddTokenInt(tokens, "LaserMaxHits", string.Empty, m_laserMaxHits);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_NanoSmithChainLightning)
		{
			val2 = abilityMod_NanoSmithChainLightning.m_chainMaxHitsMod.GetModifiedValue(m_chainMaxHits);
		}
		else
		{
			val2 = m_chainMaxHits;
		}
		AddTokenInt(tokens, "ChainMaxHits", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_NanoSmithChainLightning)
		{
			val3 = abilityMod_NanoSmithChainLightning.m_chainDamageMod.GetModifiedValue(m_chainDamage);
		}
		else
		{
			val3 = m_chainDamage;
		}
		AddTokenInt(tokens, "ChainDamage", empty3, val3);
		AbilityMod.AddToken_EffectInfo(tokens, m_chainEnemyHitEffect, "ChainEnemyHitEffect", m_chainEnemyHitEffect);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_NanoSmithChainLightning)
		{
			val4 = abilityMod_NanoSmithChainLightning.m_energyPerChainHitMod.GetModifiedValue(m_energyGainPerChainHit);
		}
		else
		{
			val4 = m_energyGainPerChainHit;
		}
		AddTokenInt(tokens, "EnergyGainPerChainHit", empty4, val4);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithChainLightning))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_NanoSmithChainLightning);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
