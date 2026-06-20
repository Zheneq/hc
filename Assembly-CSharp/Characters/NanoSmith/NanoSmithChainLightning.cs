// ROGUES
// SERVER
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

	// added in rogues
#if SERVER
	private NanoSmith_SyncComponent m_syncComp;
#endif

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
		// added in rogues
#if SERVER
		m_syncComp = GetComponent<NanoSmith_SyncComponent>();
#endif
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

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp == null || GetExtraAbsorbPerHitForVacuumBomb() <= 0)
		{
			return;
		}
		List<ActorData> htiActors = additionalData.m_abilityResults.HitActorList();
		int hitEnemyNum = 0;
		foreach (var hitActor in htiActors)
		{
			if (hitActor.GetTeam() != caster.GetTeam())
			{
				hitEnemyNum++;
			}
		}
		if (hitEnemyNum > 0)
		{
			int maxExtraAbsorb = GetMaxExtraAbsorbForVacuumBomb();
			int extraAbsorb = m_syncComp.m_extraAbsorbOnVacuumBomb;
			extraAbsorb += hitEnemyNum * GetExtraAbsorbPerHitForVacuumBomb();
			if (maxExtraAbsorb > 0 && extraAbsorb > maxExtraAbsorb)
			{
				extraAbsorb = maxExtraAbsorb;
			}
			m_syncComp.Networkm_extraAbsorbOnVacuumBomb = extraAbsorb;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (AbilityTarget targeter in targets)
		{
			GetSequenceSetupInfo(targeter, caster, out _, out var extraParams);
			List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
			if (hitActors.Contains(caster))
			{
				hitActors.Remove(caster);
				list.Add(new ServerClientUtils.SequenceStartData(
					m_selfHitSequencePrefab,
					caster.GetFreePos(),
					new[] { caster },
					caster,
					additionalData.m_sequenceSource));
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_bounceLaserSequencePrefab,
				caster.GetCurrentBoardSquare(),
				hitActors.ToArray(),
				caster,
				additionalData.m_sequenceSource,
				extraParams.ToArray()));
		}
		return list;
	}

	// added in rogues
	private void GetSequenceSetupInfo(
		AbilityTarget targeter,
		ActorData caster,
		out Vector3 targetPos,
		out BouncingShotSequence.ExtraParams extraParams)
	{
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> actorToSegmentInfo =
			GetActorToSegmentInfo(targeter, caster, out List<Vector3> endPoints, null);
		BouncingShotSequence.ExtraParams extraParams2 = new BouncingShotSequence.ExtraParams
		{
			laserTargets = actorToSegmentInfo,
			segmentPts = endPoints
		};
		targetPos = endPoints[0];
		extraParams = extraParams2;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = new Dictionary<ActorData, ActorHitResults>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (AbilityTarget targeter in targets)
		{
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> actorToSegmentInfo =
				GetActorToSegmentInfo(targeter, caster, out _, nonActorTargetInfo);
			foreach (ActorData actorData in actorToSegmentInfo.Keys)
			{
				if (dictionary.ContainsKey(actorData))
				{
					if (actorToSegmentInfo[actorData].m_endpointIndex == 0)
					{
						dictionary[actorData].AddBaseDamage(GetLaserDamage());
						dictionary[actorData].AddStandardEffectInfo(GetLaserEnemyHitEffect());
					}
					else
					{
						dictionary[actorData].AddBaseDamage(GetChainDamage());
						dictionary[actorData].AddTechPointGainOnCaster(GetEnergyGainPerChainHit());
						dictionary[actorData].AddStandardEffectInfo(GetChainEnemyHitEffect());
					}
				}
				else
				{
					ActorHitResults actorHitResults = new ActorHitResults(
						new ActorHitParameters(actorData, actorToSegmentInfo[actorData].m_segmentOrigin));
					if (actorToSegmentInfo[actorData].m_endpointIndex == 0)
					{
						actorHitResults.SetBaseDamage(GetLaserDamage());
						actorHitResults.AddStandardEffectInfo(GetLaserEnemyHitEffect());
					}
					else
					{
						actorHitResults.SetBaseDamage(GetChainDamage());
						actorHitResults.SetTechPointGainOnCaster(GetEnergyGainPerChainHit());
						actorHitResults.AddStandardEffectInfo(GetChainEnemyHitEffect());
					}
					dictionary[actorData] = actorHitResults;
				}
			}
		}
		foreach (ActorHitResults hitResults in dictionary.Values)
		{
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> GetActorToSegmentInfo(
		AbilityTarget targeter,
		ActorData caster,
		out List<Vector3> endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary
			= new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		endPoints = new List<Vector3>();
		List<ActorData> list = new List<ActorData>();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targeter.AimDirection,
			GetLaserRange(),
			GetLaserWidth(),
			caster,
			caster.GetOtherTeams(),
			PenetrateLos(),
			GetLaserMaxTargets(),
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		foreach (ActorData actorData in actorsInLaser)
		{
			AreaEffectUtils.BouncingLaserInfo value;
			value.m_segmentOrigin = laserCoords.start;
			value.m_endpointIndex = 0;
			dictionary.Add(actorData, value);
			list.Add(actorData);
		}
		endPoints.Add(laserCoords.end);
		int num = 0;
		if (actorsInLaser.Count > 0)
		{
			ActorData actorData = actorsInLaser[actorsInLaser.Count - 1];
			int chainMaxHits = GetChainMaxHits();
			while (actorData != null && (chainMaxHits <= 0 || num < chainMaxHits))
			{
				ActorData actorData3 = FindChainHitActor(actorData, caster, list);
				if (actorData3 != null)
				{
					AreaEffectUtils.BouncingLaserInfo value2;
					value2.m_segmentOrigin = actorData.GetLoSCheckPos();
					value2.m_endpointIndex = 1 + num;
					dictionary.Add(actorData3, value2);
					list.Add(actorData3);
					endPoints.Add(actorData3.GetLoSCheckPos());
					num++;
				}
				actorData = actorData3;
			}
		}
		return dictionary;
	}

	// added in rogues
	private ActorData FindChainHitActor(ActorData fromActor, ActorData caster, List<ActorData> actorsAddedSoFar)
	{
		ActorData result = null;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, true);
		Vector3 loSCheckPos = fromActor.GetLoSCheckPos();
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			loSCheckPos, GetChainRadius(), PenetrateLos(), caster, relevantTeams, null);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadius, loSCheckPos);
		foreach (ActorData actorData in actorsInRadius)
		{
			if (!actorsAddedSoFar.Contains(actorData)
			    && (ChainCanHitInvisibleActors() || actorData.IsActorVisibleIgnoringFogOfWar(caster)))
			{
				result = actorData;
				break;
			}
		}
		return result;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalTechPointsCasterGain > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.NanoSmithStats.EnergyFromChainLightning, results.FinalTechPointsCasterGain);
		}
	}
#endif
}
