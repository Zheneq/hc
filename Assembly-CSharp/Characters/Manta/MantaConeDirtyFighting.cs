// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaConeDirtyFighting : Ability
{
	[Header("-- Targeting")]
	public float m_coneRange = 4f;
	public float m_coneWidth = 60f;
	public bool m_penetrateLoS;
	public int m_maxTargets = 5;
	public float m_coneBackwardOffset;
	[Header("-- Hit Damage/Effects")]
	public int m_onCastDamageAmount;
	public StandardActorEffectData m_dirtyFightingEffectData;
	public StandardEffectInfo m_enemyHitEffectData;
	public StandardEffectInfo m_effectOnTargetFromExplosion;
	[Header("-- On Reaction Hit/Explosion Triggered")]
	public int m_effectExplosionDamage = 30;
	[Tooltip("whether allies other than yourself should be able to trigger the explosion")]
	public bool m_explodeOnlyFromSelfDamage;
	public int m_techPointGainPerExplosion = 5;
	public int m_healAmountPerExplosion;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_effectOnExplosionSequencePrefab;

	private AbilityMod_MantaConeDirtyFighting m_abilityMod;
	private StandardActorEffectData m_cachedDirtyFightingEffectData;
	private StandardEffectInfo m_cachedEnemyHitEffectData;
	private StandardEffectInfo m_cachedEffectOnTargetFromExplosion;
	private StandardEffectInfo m_cachedEffectOnTargetWhenExpiresWithoutExplosion;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dirty Fighting Cone";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirtyFightingEffectData = m_abilityMod != null
			? m_abilityMod.m_dirtyFightingEffectDataMod.GetModifiedValue(m_dirtyFightingEffectData)
			: m_dirtyFightingEffectData;
		m_cachedEnemyHitEffectData = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData)
			: m_enemyHitEffectData;
		m_cachedEffectOnTargetFromExplosion = m_abilityMod != null
			? m_abilityMod.m_effectOnTargetFromExplosionMod.GetModifiedValue(m_effectOnTargetFromExplosion)
			: m_effectOnTargetFromExplosion;
		m_cachedEffectOnTargetWhenExpiresWithoutExplosion = m_abilityMod != null
			? m_abilityMod.m_effectOnTargetWhenExpiresWithoutExplosionMod.GetModifiedValue(null)
			: null;
	}

	public float GetConeRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneRangeMod.GetModifiedValue(m_coneRange)
			: m_coneRange;
	}

	public float GetConeWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthMod.GetModifiedValue(m_coneWidth)
			: m_coneWidth;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public int GetOnCastDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount)
			: m_onCastDamageAmount;
	}

	public StandardActorEffectData GetDirtyFightingEffectData()
	{
		return m_cachedDirtyFightingEffectData ?? m_dirtyFightingEffectData;
	}

	public StandardEffectInfo GetEnemyHitEffectData()
	{
		return m_cachedEnemyHitEffectData ?? m_enemyHitEffectData;
	}

	public StandardEffectInfo GetEffectOnTargetFromExplosion()
	{
		return m_cachedEffectOnTargetFromExplosion ?? m_effectOnTargetFromExplosion;
	}

	public StandardActorEffectData GetEffectOnTargetWhenExpiresWithoutExplosion()
	{
		return m_cachedEffectOnTargetWhenExpiresWithoutExplosion != null
		       && m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_applyEffect
			? m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_effectData
			: null;
	}

	public int GetEffectExplosionDamage()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage) 
			: m_effectExplosionDamage;
	}

	public bool ExplodeOnlyFromSelfDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodeOnlyFromSelfDamageMod.GetModifiedValue(m_explodeOnlyFromSelfDamage)
			: m_explodeOnlyFromSelfDamage;
	}

	public int GetTechPointGainPerExplosion()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerExplosionMod.GetModifiedValue(m_techPointGainPerExplosion)
			: m_techPointGainPerExplosion;
	}

	public int GetHealAmountPerExplosion()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healPerExplosionMod.GetModifiedValue(m_healAmountPerExplosion)
			: m_healAmountPerExplosion;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaConeDirtyFighting))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaConeDirtyFighting;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_DirectionCone(
			this,
			GetConeWidth(),
			GetConeRange(),
			m_coneBackwardOffset,
			PenetrateLoS(),
			true,
			true,
			false,
			false,
			GetMaxTargets());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "OnCastDamageAmount", string.Empty, m_onCastDamageAmount);
		m_dirtyFightingEffectData.AddTooltipTokens(tokens, "DirtyFightingEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectData, "EnemyHitEffectData", m_enemyHitEffectData);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetFromExplosion, "EffectOnTargetFromExplosion", m_effectOnTargetFromExplosion);
		AddTokenInt(tokens, "EffectExplosionDamage", string.Empty, m_effectExplosionDamage);
		AddTokenInt(tokens, "TechPointGainPerExplosion", string.Empty, m_techPointGainPerExplosion);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetOnCastDamageAmount());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, GetEffectExplosionDamage());
		GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetHitActors(targets, caster, null);
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(GetOnCastDamageAmount());
			MantaDirtyFightingEffect effect = new MantaDirtyFightingEffect(
				AsEffectSource(),
				actorData.GetCurrentBoardSquare(),
				actorData,
				caster,
				GetDirtyFightingEffectData(),
				GetEffectExplosionDamage(),
				GetEffectOnTargetFromExplosion().m_applyEffect
					? GetEffectOnTargetFromExplosion().m_effectData
					: null,
				m_effectOnExplosionSequencePrefab,
				ExplodeOnlyFromSelfDamage(),
				GetTechPointGainPerExplosion(),
				GetHealAmountPerExplosion(),
				GetEffectOnTargetWhenExpiresWithoutExplosion());
			actorHitResults.AddEffect(effect);
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffectData());
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		return AreaEffectUtils.GetActorsInCone(
			laserCoords.start,
			VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
			GetConeWidth(),
			GetConeRange(),
			m_coneBackwardOffset,
			PenetrateLoS(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MantaStats.NumDamagingPutridSprayHits);
		}
	}
#endif
}
