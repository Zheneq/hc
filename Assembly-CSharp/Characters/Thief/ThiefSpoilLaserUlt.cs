// ROGUES
// SERVER
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
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetActorToDamage(
			targets,
			caster,
			null,
			out _,
			out List<StatusType> buffsPresent,
			out List<Vector3> sequenceTargetPositions,
			out List<List<ActorData>> sequenceTargets,
			out List<List<PowerUp>> powerupHitList);
		bool isPlayingBuffSequence = m_onBuffCopyAudioSequencePrefab != null && buffsPresent.Count > 0;
		for (int i = 0; i < sequenceTargetPositions.Count; i++)
		{
			bool isPlayingPowerupSequence = powerupHitList[i].Count > 0 && m_powerupReturnPrefab != null;
			if (isPlayingBuffSequence || isPlayingPowerupSequence)
			{
				sequenceTargets[i].Remove(caster);
			}
			else if (additionalData.m_abilityResults.HitActorList().Contains(caster) && !sequenceTargets[i].Contains(caster))
			{
				sequenceTargets[i].Add(caster);
			}

			list.Add(new ServerClientUtils.SequenceStartData(
				m_laserSequencePrefab,
				sequenceTargetPositions[i],
				sequenceTargets[i].ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new SplineProjectileSequence.MultiEventExtraParams
				{
					eventNumberToKeyOffOf = i
				}.ToArray()));
			if (i == 0 && isPlayingBuffSequence)
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					m_onBuffCopyAudioSequencePrefab,
					caster.GetFreePos(),
					caster.AsArray(),
					caster,
					additionalData.m_sequenceSource));
			}
			if (isPlayingPowerupSequence)
			{
				foreach (PowerUp powerUp in powerupHitList[i])
				{
					list.Add(new ServerClientUtils.SequenceStartData(
						m_powerupReturnPrefab,
						caster.GetFreePos(),
						isPlayingBuffSequence
							? new ActorData[0]
							: caster.AsArray(),
						caster,
						additionalData.m_sequenceSource,
						new List<Sequence.IExtraSequenceParams>
						{
							new SplineProjectileSequence.DelayedProjectileExtraParams
							{
								useOverrideStartPos = true,
								overrideStartPos = powerUp.gameObject.transform.position
							},
							new ThiefPowerupReturnProjectileSequence.PowerupTypeExtraParams
							{
								powerupCategory = (int)powerUp.m_chatterCategory
							}
						}.ToArray()));
				}
			}
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> actorToDamage = GetActorToDamage(
			targets,
			caster,
			nonActorTargetInfo,
			out HashSet<PowerUp> powerupsHitSoFar,
			out List<StatusType> buffsPresent,
			out _,
			out _,
			out _);
		foreach (ActorData hitActor in actorToDamage.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(actorToDamage[hitActor]);
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), GetSpoilSpawnData()));
			abilityResults.StoreActorHit(actorHitResults);
		}
		bool hasPowerupHits = HitPowerups() && powerupsHitSoFar.Count > 0;
		if (hasPowerupHits || buffsPresent.Count > 0)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			if (hasPowerupHits)
			{
				foreach (PowerUp powerup in powerupsHitSoFar)
				{
					casterHitResults.AddPowerUpForSteal(powerup);
				}
			}
			StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
			standardActorEffectData.SetValues(
				"Thief Ult Buff Copy",
				Mathf.Max(1, GetCopyBuffDuration()),
				0,
				0,
				0,
				ServerCombatManager.HealingType.Effect,
				0,
				0,
				new AbilityStatMod[0],
				buffsPresent.ToArray(),
				StandardActorEffectData.StatusDelayMode.DefaultBehavior);
			casterHitResults.AddEffect(new StandardActorEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				standardActorEffectData));
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetActorToDamage(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out HashSet<PowerUp> powerupsHitSoFar,
		out List<StatusType> buffsPresent,
		out List<Vector3> sequenceTargetPositions,
		out List<List<ActorData>> sequenceTargets,
		out List<List<PowerUp>> powerupHitList)
	{
		Dictionary<ActorData, int> actorToDamage = new Dictionary<ActorData, int>();
		powerupsHitSoFar = new HashSet<PowerUp>();
		buffsPresent = new List<StatusType>();
		sequenceTargetPositions = new List<Vector3>();
		sequenceTargets = new List<List<ActorData>>();
		powerupHitList = new List<List<PowerUp>>();
		bool isCopyingBuffs = CopyBuffsOnEnemyHit() && m_buffsToCopy != null && m_buffsToCopy.Count > 0;
		foreach (Vector3 direction in GetLaserDirections(targets, caster))
		{
			List<ActorData> hitActorsInDirection = GetHitActorsInDirection(
				direction,
				caster,
				powerupsHitSoFar,
				out VectorUtils.LaserCoords endPoints,
				out List<PowerUp> powerupsHit,
				nonActorTargetInfo);
			foreach (ActorData actorData in hitActorsInDirection)
			{
				if (!actorToDamage.ContainsKey(actorData))
				{
					actorToDamage[actorData] = GetLaserDamageAmount();
				}
				else
				{
					actorToDamage[actorData] += GetLaserSubsequentDamageAmount();
				}
				if (isCopyingBuffs)
				{
					foreach (StatusType buff in m_buffsToCopy)
					{
						if (!buffsPresent.Contains(buff) && actorData.GetActorStatus().HasStatus(buff))
						{
							buffsPresent.Add(buff);
						}
					}
				}
			}
			sequenceTargets.Add(hitActorsInDirection);
			sequenceTargetPositions.Add(endPoints.end);
			powerupHitList.Add(powerupsHit);
		}
		return actorToDamage;
	}

	// added in rogues
	private List<Vector3> GetLaserDirections(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		int laserCount = GetLaserCount();
		if (m_targeterMultiTarget)
		{
			for (int i = 0; i < laserCount && i < targets.Count; i++)
			{
				Vector3 aimDir = targets[i].AimDirection;
				if (i > 0 && GetTargeterMaxAngle() > 0f)
				{
					Vector3 prevAimDir = targets[i - 1].AimDirection;
					float num = Vector3.Angle(aimDir, prevAimDir);
					if (num > GetTargeterMaxAngle())
					{
						aimDir = Vector3.RotateTowards(
							aimDir, 
							prevAimDir,
							0.0174532924f * (num - GetTargeterMaxAngle()), 0f);
					}
				}
				list.Add(aimDir);
			}
		}
		else
		{
			float angleFull = laserCount > 1 ? CalculateFanAngleDegrees(targets[0], caster) : 0f;
			float angleStep = laserCount > 1 ? angleFull / (laserCount - 1) : 0f;
			float angleStart = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection) - 0.5f * angleFull;
			for (int i = 0; i < laserCount; i++)
			{
				Vector3 item = VectorUtils.AngleDegreesToVector(angleStart + i * angleStep);
				list.Add(item);
			}
		}
		return list;
	}

	// added in rogues
	private List<ActorData> GetHitActorsInDirection(
		Vector3 direction,
		ActorData caster,
		HashSet<PowerUp> powerupsHitPreviously,
		out VectorUtils.LaserCoords endPoints,
		out List<PowerUp> powerupsHit,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return ThiefBasicAttack.GetHitActorsInDirectionStatic(
			caster.GetLoSCheckPos(),
			direction,
			caster,
			GetLaserRange(),
			GetLaserWidth(),
			LaserPenetrateLos(),
			GetLaserMaxTargets(),
			false,
			true,
			true,
			GetMaxPowerupsHit(),
			HitPowerups(),
			HitPowerups() && StopOnPowerupHit(),
			IncludeSpoilsPowerups(),
			IgnorePickupTeamRestriction(),
			powerupsHitPreviously,
			out endPoints,
			out powerupsHit,
			nonActorTargetInfo,
			false);
	}

	// added in rogues
	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float distInSquares = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float share = Mathf.Clamp(distInSquares, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return GetTargeterMaxAngle() * (1f - share / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance));
	}

	// added in rogues
	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(
			fanAngleDegrees,
			GetTargeterMaxAngle(),
			m_targeterMinInterpDistance,
			m_targeterMaxInterpDistance);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster == target
		    && results.m_powerUpsToSteal != null
		    && results.m_powerUpsToSteal.Count > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ThiefStats.PowerUpsStolen,
				results.m_powerUpsToSteal.Count);
		}
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ThiefStats.UltDamage,
				results.FinalDamage);
		}
	}
#endif
}
