using System.Collections.Generic;
using UnityEngine;

public class ThiefSpoilLaserUlt : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget;
	public float m_targeterMaxAngle = 120f;
	public float m_targeterMinInterpDistance = 1.5f;
	public float m_targeterMaxInterpDistance = 6f;
	[Header("-- Damage")]
	public int m_laserDamageAmount = 3;
	public int m_laserSubsequentDamageAmount = 3;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;
	public float m_laserWidth = 0.5f;
	public int m_laserMaxTargets = 1;
	public int m_laserCount = 2;
	public bool m_laserPenetrateLos;
	[Header("-- Spoil Spawn Data On Enemy Hit")]
	public SpoilsSpawnData m_spoilSpawnData;
	[Header("-- PowerUp/Spoils Interaction")]
	public bool m_hitPowerups;
	public bool m_stopOnPowerupHit = true;
	public bool m_includeSpoilsPowerups = true;
	public bool m_ignorePickupTeamRestriction;
	public int m_maxPowerupsHit;
	[Header("-- Buffs Copy --")]
	public bool m_copyBuffsOnEnemyHit;
	public int m_copyBuffDuration = 2;
	public List<StatusType> m_buffsToCopy;
	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;
	public GameObject m_powerupReturnPrefab;
	public GameObject m_onBuffCopyAudioSequencePrefab;

	private AbilityMod_ThiefSpoilLaserUlt m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private SpoilsSpawnData m_cachedSpoilSpawnData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ult 2";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_targeterMultiTarget)
		{
			ClearTargeters();
			for (int i = 0; i < GetLaserCount(); i++)
			{
				Targeters.Add(new AbilityUtil_Targeter_ThiefFanLaser(
					this,
					0f,
					GetTargeterMaxAngle(),
					m_targeterMinInterpDistance,
					m_targeterMaxInterpDistance,
					GetLaserRange(),
					GetLaserWidth(),
					GetLaserMaxTargets(),
					GetLaserCount(),
					LaserPenetrateLos(),
					HitPowerups(),
					StopOnPowerupHit(),
					IncludeSpoilsPowerups(),
					IgnorePickupTeamRestriction(),
					GetMaxPowerupsHit()));
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_ThiefFanLaser(
				this,
				0f,
				GetTargeterMaxAngle(),
				m_targeterMinInterpDistance,
				m_targeterMaxInterpDistance,
				GetLaserRange(),
				GetLaserWidth(),
				GetLaserMaxTargets(),
				GetLaserCount(),
				LaserPenetrateLos(),
				HitPowerups(),
				StopOnPowerupHit(),
				IncludeSpoilsPowerups(),
				IgnorePickupTeamRestriction(),
				GetMaxPowerupsHit());
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_targeterMultiTarget
			? GetLaserCount()
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
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedSpoilSpawnData = m_abilityMod != null
			? m_abilityMod.m_spoilSpawnDataMod.GetModifiedValue(m_spoilSpawnData)
			: m_spoilSpawnData;
	}

	public float GetTargeterMaxAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle)
			: m_targeterMaxAngle;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetLaserSubsequentDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
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

	public int GetLaserMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets;
	}

	public int GetLaserCount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount;
	}

	public bool LaserPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(m_laserPenetrateLos)
			: m_laserPenetrateLos;
	}

	public SpoilsSpawnData GetSpoilSpawnData()
	{
		return m_cachedSpoilSpawnData ?? m_spoilSpawnData;
	}

	public bool HitPowerups()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitPowerupsMod.GetModifiedValue(m_hitPowerups)
			: m_hitPowerups;
	}

	public bool StopOnPowerupHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(m_stopOnPowerupHit)
			: m_stopOnPowerupHit;
	}

	public bool IncludeSpoilsPowerups()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(m_includeSpoilsPowerups)
			: m_includeSpoilsPowerups;
	}

	public bool IgnorePickupTeamRestriction()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(m_ignorePickupTeamRestriction)
			: m_ignorePickupTeamRestriction;
	}

	public int GetMaxPowerupsHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxPowerupsHitMod.GetModifiedValue(m_maxPowerupsHit)
			: m_maxPowerupsHit;
	}

	public bool CopyBuffsOnEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_copyBuffsOnEnemyHitMod.GetModifiedValue(m_copyBuffsOnEnemyHit)
			: m_copyBuffsOnEnemyHit;
	}

	public int GetCopyBuffDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_copyBuffDurationMod.GetModifiedValue(m_copyBuffDuration)
			: m_copyBuffDuration;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			AccumulateDamageFromTargeter(targetActor, Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				AccumulateDamageFromTargeter(targetActor, Targeters[i], dictionary);
			}
		}
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(
		ActorData targetActor,
		AbilityUtil_Targeter targeter,
		Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		foreach (AbilityTooltipSubject item in tooltipSubjectTypes)
		{
			if (item != AbilityTooltipSubject.Primary)
			{
				continue;
			}
			if (!symbolToDamage.ContainsKey(AbilityTooltipSymbol.Damage))
			{
				symbolToDamage[AbilityTooltipSymbol.Damage] = GetLaserDamageAmount();
			}
			else
			{
				symbolToDamage[AbilityTooltipSymbol.Damage] += GetLaserSubsequentDamageAmount();
			}
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefSpoilLaserUlt abilityMod_ThiefSpoilLaserUlt = modAsBase as AbilityMod_ThiefSpoilLaserUlt;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount);
		AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, m_laserDamageAmount + m_laserSubsequentDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "LaserMaxTargets", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_laserMaxTargetsMod.GetModifiedValue(m_laserMaxTargets)
			: m_laserMaxTargets);
		AddTokenInt(tokens, "LaserCount", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount);
		AddTokenInt(tokens, "CopyBuffDuration", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_copyBuffDurationMod.GetModifiedValue(m_copyBuffDuration)
			: m_copyBuffDuration);
		AddTokenInt(tokens, "MaxPowerupsHit", string.Empty, abilityMod_ThiefSpoilLaserUlt != null
			? abilityMod_ThiefSpoilLaserUlt.m_maxPowerupsHitMod.GetModifiedValue(m_maxPowerupsHit)
			: m_maxPowerupsHit);
	}

	public override bool HasRestrictedFreePosDistance(
		ActorData aimingActor,
		int targetIndex,
		List<AbilityTarget> targetsSoFar,
		out float min,
		out float max)
	{
		min = m_targeterMinInterpDistance * Board.Get().squareSize;
		max = m_targeterMaxInterpDistance * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefSpoilLaserUlt))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefSpoilLaserUlt;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
