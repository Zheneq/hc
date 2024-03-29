﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class FishManSplittingLaser : Ability
{
	[Header("-- Primary Laser")]
	public bool m_primaryLaserCanHitEnemies = true;
	public bool m_primaryLaserCanHitAllies;
	public int m_primaryTargetDamageAmount = 5;
	public int m_primaryTargetHealingAmount;
	public StandardEffectInfo m_primaryTargetEnemyHitEffect;
	public StandardEffectInfo m_primaryTargetAllyHitEffect;
	public LaserTargetingInfo m_primaryTargetingInfo;
	[Header("-- Secondary Lasers")]
	public bool m_secondaryLasersCanHitEnemies = true;
	public bool m_secondaryLasersCanHitAllies;
	public int m_secondaryTargetDamageAmount = 5;
	public int m_secondaryTargetHealingAmount;
	public StandardEffectInfo m_secondaryTargetEnemyHitEffect;
	public StandardEffectInfo m_secondaryTargetAllyHitEffect;
	public LaserTargetingInfo m_secondaryTargetingInfo;
	[Header("-- Split Data")]
	public bool m_alwaysSplit;
	public float m_minSplitAngle = 60f;
	public float m_maxSplitAngle = 120f;
	public float m_lengthForMinAngle = 3f;
	public float m_lengthForMaxAngle = 9f;
	public int m_numSplitBeamPairs = 1;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_splitProjectileSequence;

	private StandardEffectInfo m_cachedPrimaryTargetEnemyHitEffect;
	private StandardEffectInfo m_cachedPrimaryTargetAllyHitEffect;
	private LaserTargetingInfo m_cachedPrimaryTargetingInfo;
	private StandardEffectInfo m_cachedSecondaryTargetEnemyHitEffect;
	private StandardEffectInfo m_cachedSecondaryTargetAllyHitEffect;
	private LaserTargetingInfo m_cachedSecondaryTargetingInfo;
	private AbilityMod_FishManSplittingLaser m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_SplittingLaser(
			this,
			GetMinSplitAngle(),
			GetMaxSplitAngle(),
			GetLengthForMinAngle(),
			GetLengthForMaxAngle(),
			GetNumSplitBeamPairs(),
			AlwaysSplit(),
			GetPrimaryTargetingInfo().range,
			GetPrimaryTargetingInfo().width,
			GetPrimaryTargetingInfo().penetrateLos,
			GetPrimaryTargetingInfo().maxTargets,
			m_primaryLaserCanHitEnemies,
			m_primaryLaserCanHitAllies,
			GetSecondaryTargetingInfo().range,
			GetSecondaryTargetingInfo().width,
			GetSecondaryTargetingInfo().penetrateLos,
			GetSecondaryTargetingInfo().maxTargets,
			m_secondaryLasersCanHitEnemies,
			m_secondaryLasersCanHitAllies);
	}

	private void SetCachedFields()
	{
		m_cachedPrimaryTargetEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(m_primaryTargetEnemyHitEffect)
			: m_primaryTargetEnemyHitEffect;
		m_cachedPrimaryTargetAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_primaryTargetAllyHitEffectMod.GetModifiedValue(m_primaryTargetAllyHitEffect)
			: m_primaryTargetAllyHitEffect;
		m_cachedPrimaryTargetingInfo = m_abilityMod != null
			? m_abilityMod.m_primaryTargetingInfoMod.GetModifiedValue(m_primaryTargetingInfo)
			: m_primaryTargetingInfo;
		m_cachedSecondaryTargetEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(m_secondaryTargetEnemyHitEffect)
			: m_secondaryTargetEnemyHitEffect;
		m_cachedSecondaryTargetAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(m_secondaryTargetAllyHitEffect)
			: m_secondaryTargetAllyHitEffect;
		m_cachedSecondaryTargetingInfo = m_abilityMod != null
			? m_abilityMod.m_secondaryTargetingInfoMod.GetModifiedValue(m_secondaryTargetingInfo)
			: m_secondaryTargetingInfo;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManSplittingLaser))
		{
			m_abilityMod = abilityMod as AbilityMod_FishManSplittingLaser;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public bool PrimaryLaserCanHitEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_primaryLaserCanHitEnemiesMod.GetModifiedValue(m_primaryLaserCanHitEnemies)
			: m_primaryLaserCanHitEnemies;
	}

	public bool PrimaryLaserCanHitAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_primaryLaserCanHitAlliesMod.GetModifiedValue(m_primaryLaserCanHitAllies)
			: m_primaryLaserCanHitAllies;
	}

	public int GetPrimaryTargetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_primaryTargetDamageAmountMod.GetModifiedValue(m_primaryTargetDamageAmount)
			: m_primaryTargetDamageAmount;
	}

	public int GetPrimaryTargetHealingAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_primaryTargetHealingAmountMod.GetModifiedValue(m_primaryTargetHealingAmount)
			: m_primaryTargetHealingAmount;
	}

	public StandardEffectInfo GetPrimaryTargetEnemyHitEffect()
	{
		return m_cachedPrimaryTargetEnemyHitEffect ?? m_primaryTargetEnemyHitEffect;
	}

	public StandardEffectInfo GetPrimaryTargetAllyHitEffect()
	{
		return m_cachedPrimaryTargetAllyHitEffect ?? m_primaryTargetAllyHitEffect;
	}

	public LaserTargetingInfo GetPrimaryTargetingInfo()
	{
		return m_cachedPrimaryTargetingInfo ?? m_primaryTargetingInfo;
	}

	public bool SecondaryLasersCanHitEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondaryLasersCanHitEnemiesMod.GetModifiedValue(m_secondaryLasersCanHitEnemies)
			: m_secondaryLasersCanHitEnemies;
	}

	public bool SecondaryLasersCanHitAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondaryLasersCanHitAlliesMod.GetModifiedValue(m_secondaryLasersCanHitAllies)
			: m_secondaryLasersCanHitAllies;
	}

	public int GetSecondaryTargetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondaryTargetDamageAmountMod.GetModifiedValue(m_secondaryTargetDamageAmount)
			: m_secondaryTargetDamageAmount;
	}

	public int GetSecondaryTargetHealingAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondaryTargetHealingAmountMod.GetModifiedValue(m_secondaryTargetHealingAmount)
			: m_secondaryTargetHealingAmount;
	}

	public StandardEffectInfo GetSecondaryTargetEnemyHitEffect()
	{
		return m_cachedSecondaryTargetEnemyHitEffect ?? m_secondaryTargetEnemyHitEffect;
	}

	public StandardEffectInfo GetSecondaryTargetAllyHitEffect()
	{
		return m_cachedSecondaryTargetAllyHitEffect ?? m_secondaryTargetAllyHitEffect;
	}

	public LaserTargetingInfo GetSecondaryTargetingInfo()
	{
		return m_cachedSecondaryTargetingInfo ?? m_secondaryTargetingInfo;
	}

	public bool AlwaysSplit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_alwaysSplitMod.GetModifiedValue(m_alwaysSplit)
			: m_alwaysSplit;
	}

	public float GetMinSplitAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minSplitAngleMod.GetModifiedValue(m_minSplitAngle)
			: m_minSplitAngle;
	}

	public float GetMaxSplitAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxSplitAngleMod.GetModifiedValue(m_maxSplitAngle)
			: m_maxSplitAngle;
	}

	public float GetLengthForMinAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lengthForMinAngleMod.GetModifiedValue(m_lengthForMinAngle)
			: m_lengthForMinAngle;
	}

	public float GetLengthForMaxAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lengthForMaxAngleMod.GetModifiedValue(m_lengthForMaxAngle)
			: m_lengthForMaxAngle;
	}

	public int GetNumSplitBeamPairs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numSplitBeamPairsMod.GetModifiedValue(m_numSplitBeamPairs)
			: m_numSplitBeamPairs;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManSplittingLaser abilityMod_FishManSplittingLaser = modAsBase as AbilityMod_FishManSplittingLaser;
		AddTokenInt(tokens, "PrimaryTargetDamageAmount", string.Empty, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_primaryTargetDamageAmountMod.GetModifiedValue(m_primaryTargetDamageAmount)
			: m_primaryTargetDamageAmount);
		AddTokenInt(tokens, "PrimaryTargetHealingAmount", string.Empty, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_primaryTargetHealingAmountMod.GetModifiedValue(m_primaryTargetHealingAmount)
			: m_primaryTargetHealingAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(m_primaryTargetEnemyHitEffect)
			: m_primaryTargetEnemyHitEffect, "PrimaryTargetEnemyHitEffect", m_primaryTargetEnemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_primaryTargetAllyHitEffectMod.GetModifiedValue(m_primaryTargetAllyHitEffect)
			: m_primaryTargetAllyHitEffect, "PrimaryTargetAllyHitEffect", m_primaryTargetAllyHitEffect);
		AddTokenInt(tokens, "SecondaryTargetDamageAmount", string.Empty, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_secondaryTargetDamageAmountMod.GetModifiedValue(m_secondaryTargetDamageAmount)
			: m_secondaryTargetDamageAmount);
		AddTokenInt(tokens, "SecondaryTargetHealingAmount", string.Empty, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_secondaryTargetHealingAmountMod.GetModifiedValue(m_secondaryTargetHealingAmount)
			: m_secondaryTargetHealingAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(m_secondaryTargetEnemyHitEffect)
			: m_secondaryTargetEnemyHitEffect, "SecondaryTargetEnemyHitEffect", m_secondaryTargetEnemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(m_secondaryTargetAllyHitEffect)
			: m_secondaryTargetAllyHitEffect, "SecondaryTargetAllyHitEffect", m_secondaryTargetAllyHitEffect);
		AddTokenInt(tokens, "NumSplitBeamPairs", string.Empty, abilityMod_FishManSplittingLaser != null
			? abilityMod_FishManSplittingLaser.m_numSplitBeamPairsMod.GetModifiedValue(m_numSplitBeamPairs)
			: m_numSplitBeamPairs);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetPrimaryTargetDamageAmount()));
		GetPrimaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, GetSecondaryTargetDamageAmount()));
		GetSecondaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Tertiary, GetPrimaryTargetHealingAmount()));
		GetPrimaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Quaternary, GetSecondaryTargetHealingAmount()));
		GetSecondaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Quaternary);
		return numbers;
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		Vector3 aimDirection = targets[0].AimDirection;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			aimDirection,
			GetPrimaryTargetingInfo().range,
			GetPrimaryTargetingInfo().width,
			caster,
			GetPrimaryLaserAffectedTeams(caster),
			GetPrimaryTargetingInfo().penetrateLos,
			GetPrimaryTargetingInfo().maxTargets,
			false,
			true,
			out laserCoords.end,
			null);
		list.Add(new ServerClientUtils.SequenceStartData(
			m_castSequence,
			laserCoords.end,
			actorsInLaser.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new HealLaserSequence.ExtraParams
			{
				endPos = laserCoords.end
			}.ToArray()));
		if (AlwaysSplit() || actorsInLaser.Count > 0)
		{
			float num = CalculateSplitAngleDegrees(targets[0], caster);
			float num2 = VectorUtils.HorizontalAngle_Deg(aimDirection);
			int numSplitBeamPairs = GetNumSplitBeamPairs();
			for (int i = 0; i < numSplitBeamPairs; i++)
			{
				float angle = num2 + num * (i + 1);
				float angle2 = num2 - num * (i + 1);
				Vector3 dir = VectorUtils.AngleDegreesToVector(angle);
				Vector3 dir2 = VectorUtils.AngleDegreesToVector(angle2);
				VectorUtils.LaserCoords laserCoords2;
				laserCoords2.start = laserCoords.end;
				VectorUtils.LaserCoords laserCoords3;
				laserCoords3.start = laserCoords.end;
				List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(
					laserCoords2.start,
					dir,
					GetSecondaryTargetingInfo().range,
					GetSecondaryTargetingInfo().width,
					caster,
					GetSplitLaserAffectedTeams(caster),
					GetSecondaryTargetingInfo().penetrateLos,
					GetSecondaryTargetingInfo().maxTargets,
					false,
					false,
					out laserCoords2.end,
					null,
					actorsInLaser);
				List<ActorData> actorsInLaser3 = AreaEffectUtils.GetActorsInLaser(
					laserCoords3.start,
					dir2,
					GetSecondaryTargetingInfo().range,
					GetSecondaryTargetingInfo().width,
					caster,
					GetSplitLaserAffectedTeams(caster),
					GetSecondaryTargetingInfo().penetrateLos,
					GetSecondaryTargetingInfo().maxTargets,
					false,
					false,
					out laserCoords3.end,
					null,
					actorsInLaser);
				HealLaserSequence.ExtraParams extraParams2 = new HealLaserSequence.ExtraParams();
				HealLaserSequence.ExtraParams extraParams3 = new HealLaserSequence.ExtraParams();
				extraParams2.endPos = laserCoords2.end;
				extraParams3.endPos = laserCoords3.end;
				list.Add(new ServerClientUtils.SequenceStartData(
					m_splitProjectileSequence,
					laserCoords2.start,
					actorsInLaser2.ToArray(),
					caster,
					additionalData.m_sequenceSource,
					extraParams2.ToArray()));
				list.Add(new ServerClientUtils.SequenceStartData(
					m_splitProjectileSequence,
					laserCoords3.start,
					actorsInLaser3.ToArray(),
					caster,
					additionalData.m_sequenceSource,
					extraParams3.ToArray()));
			}
		}
		return list;
	}

	// added in rogues
	private List<Team> GetPrimaryLaserAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			if (PrimaryLaserCanHitEnemies())
			{
				list.AddRange(caster.GetOtherTeams());
			}
			if (PrimaryLaserCanHitAllies())
			{
				list.Add(caster.GetTeam());
			}
		}
		return list;
	}

	// added in rogues
	private List<Team> GetSplitLaserAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			if (SecondaryLasersCanHitEnemies())
			{
				list.AddRange(caster.GetOtherTeams());
			}
			if (SecondaryLasersCanHitAllies())
			{
				list.Add(caster.GetTeam());
			}
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		Vector3 aimDirection = targets[0].AimDirection;
		List<NonActorTargetInfo> list = new List<NonActorTargetInfo>();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			aimDirection,
			GetPrimaryTargetingInfo().range,
			GetPrimaryTargetingInfo().width,
			caster,
			GetPrimaryLaserAffectedTeams(caster),
			GetPrimaryTargetingInfo().penetrateLos,
			GetPrimaryTargetingInfo().maxTargets,
			false,
			true,
			out laserCoords.end,
			list);
		foreach (ActorData actor in actorsInLaser)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actor, laserCoords.start);
			if (actor.GetTeam() == caster.GetTeam())
			{
				abilityResults.StoreActorHit(new ActorHitResults(
					GetPrimaryTargetHealingAmount(), HitActionType.Healing, GetPrimaryTargetAllyHitEffect(), hitParams));
			}
			else
			{
				abilityResults.StoreActorHit(new ActorHitResults(
					GetPrimaryTargetDamageAmount(), HitActionType.Damage, GetPrimaryTargetEnemyHitEffect(), hitParams));
			}
		}
		if (AlwaysSplit() || actorsInLaser.Count > 0)
		{
			float num = CalculateSplitAngleDegrees(targets[0], caster);
			float num2 = VectorUtils.HorizontalAngle_Deg(aimDirection);
			int numSplitBeamPairs = GetNumSplitBeamPairs();
			for (int j = 0; j < numSplitBeamPairs; j++)
			{
				float angle = num2 + num * (j + 1);
				float angle2 = num2 - num * (j + 1);
				Vector3 dir = VectorUtils.AngleDegreesToVector(angle);
				Vector3 dir2 = VectorUtils.AngleDegreesToVector(angle2);
				VectorUtils.LaserCoords laserCoords2;
				laserCoords2.start = laserCoords.end;
				VectorUtils.LaserCoords laserCoords3;
				laserCoords3.start = laserCoords.end;
				List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(
					laserCoords2.start,
					dir,
					GetSecondaryTargetingInfo().range,
					GetSecondaryTargetingInfo().width,
					caster,
					GetSplitLaserAffectedTeams(caster),
					GetSecondaryTargetingInfo().penetrateLos,
					GetSecondaryTargetingInfo().maxTargets,
					false,
					false,
					out laserCoords2.end,
					list,
					actorsInLaser);
				List<ActorData> actorsInLaser3 = AreaEffectUtils.GetActorsInLaser(
					laserCoords3.start,
					dir2,
					GetSecondaryTargetingInfo().range,
					GetSecondaryTargetingInfo().width,
					caster,
					GetSplitLaserAffectedTeams(caster),
					GetSecondaryTargetingInfo().penetrateLos,
					GetSecondaryTargetingInfo().maxTargets,
					false,
					false,
					out laserCoords3.end,
					list,
					actorsInLaser);
				foreach (ActorData actor in actorsInLaser2)
				{
					ActorHitParameters hitParams2 = new ActorHitParameters(actor, laserCoords2.start);
					if (actor.GetTeam() == caster.GetTeam())
					{
						abilityResults.StoreActorHit(new ActorHitResults(
							GetSecondaryTargetHealingAmount(), HitActionType.Healing, GetSecondaryTargetAllyHitEffect(), hitParams2));
					}
					else
					{
						abilityResults.StoreActorHit(new ActorHitResults(
							GetSecondaryTargetDamageAmount(), HitActionType.Damage, GetSecondaryTargetEnemyHitEffect(), hitParams2));
					}
				}
				foreach (ActorData actor in actorsInLaser3)
				{
					ActorHitParameters hitParams3 = new ActorHitParameters(actor, laserCoords3.start);
					if (actor.GetTeam() == caster.GetTeam())
					{
						abilityResults.StoreActorHit(new ActorHitResults(
							GetSecondaryTargetHealingAmount(), HitActionType.Healing, GetSecondaryTargetAllyHitEffect(), hitParams3));
					}
					else
					{
						abilityResults.StoreActorHit(new ActorHitResults(
							GetSecondaryTargetDamageAmount(), HitActionType.Damage, GetSecondaryTargetEnemyHitEffect(), hitParams3));
					}
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(list);
	}

	// added in rogues
	private float CalculateSplitAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float lengthRange = GetLengthForMaxAngle() - GetLengthForMinAngle();
		float angleRange = GetMaxSplitAngle() - GetMinSplitAngle();
		if (lengthRange <= 0f)
		{
			return GetMinSplitAngle();
		}
		float length = Mathf.Clamp(
			(currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize,
			GetLengthForMinAngle(),
			GetLengthForMaxAngle());
		float share = 1f - (length - GetLengthForMinAngle()) / lengthRange;
		return GetMinSplitAngle() + angleRange * share;
	}
#endif
}
