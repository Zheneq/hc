// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarinePrimaryAttack : Ability
{
	[Separator("Targeting")]
	public LaserTargetingInfo m_laserTargetInfo;
	[Header("-- Cone on initial target hit --")]
	public bool m_addConeOnFirstHitTarget = true;
	public ConeTargetingInfo m_coneTargetInfo;
	[Separator("Enemy Hit: Laser")]
	public int m_damageAmount = 18;
	public int m_extraDamageToClosestTarget;
	[Separator("Enemy Hit: Cone")]
	public int m_coneDamageAmount = 18;
	public StandardEffectInfo m_coneEnemyHitEffect;
	[Header("-- Whether length of targeter should ignore world geo")]
	public bool m_laserLengthIgnoreWorldGeo = true;

	private AbilityMod_SpaceMarinePrimaryAttack m_abilityMod;
	private LaserTargetingInfo m_cachedLaserTargetInfo;
	private ConeTargetingInfo m_cachedConeTargetInfo;
	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Piston Punch";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		LaserTargetingInfo laserTargetInfo = GetLaserTargetInfo();
		ConeTargetingInfo coneTargetInfo = GetConeTargetInfo();
		Targeter = new AbilityUtil_Targeter_SpaceMarineBasicAttack(
			this,
			laserTargetInfo.width,
			laserTargetInfo.range,
			laserTargetInfo.maxTargets,
			coneTargetInfo.m_widthAngleDeg,
			coneTargetInfo.m_radiusInSquares,
			laserTargetInfo.penetrateLos)
		{
			AddConeOnFirstLaserHit = AddConeOnFirstHitTarget(),
			LengthIgnoreWorldGeo = m_laserLengthIgnoreWorldGeo
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserTargetInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedLaserTargetInfo = m_abilityMod != null
			? m_abilityMod.m_laserTargetInfoMod.GetModifiedValue(m_laserTargetInfo)
			: m_laserTargetInfo;
		m_cachedConeTargetInfo = m_abilityMod != null
			? m_abilityMod.m_coneTargetInfoMod.GetModifiedValue(m_coneTargetInfo)
			: m_coneTargetInfo;
		m_cachedConeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect)
			: m_coneEnemyHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetInfo()
	{
		return m_cachedLaserTargetInfo ?? m_laserTargetInfo;
	}

	public bool AddConeOnFirstHitTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addConeOnFirstHitTargetMod.GetModifiedValue(m_addConeOnFirstHitTarget)
			: m_addConeOnFirstHitTarget;
	}

	public ConeTargetingInfo GetConeTargetInfo()
	{
		return m_cachedConeTargetInfo ?? m_coneTargetInfo;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetExtraDamageToClosestTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnClosestMod.GetModifiedValue(m_extraDamageToClosestTarget)
			: m_extraDamageToClosestTarget;
	}

	public int GetConeDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageAmountMod.GetModifiedValue(m_coneDamageAmount)
			: m_coneDamageAmount;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return m_cachedConeEnemyHitEffect ?? m_coneEnemyHitEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_coneDamageAmount);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (Targeter is AbilityUtil_Targeter_SpaceMarineBasicAttack)
			{
				int damage = GetLaserDamage();
				AbilityUtil_Targeter_SpaceMarineBasicAttack targeter = Targeter as AbilityUtil_Targeter_SpaceMarineBasicAttack;
				List<ActorData> lastLaserHitActors = targeter.GetLastLaserHitActors();
				if (lastLaserHitActors.Count > 0 && lastLaserHitActors[0] == targetActor)
				{
					damage += GetExtraDamageToClosestTarget();
				}
				results.m_damage = damage;
			}
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
		{
			results.m_damage = GetConeDamageAmount();
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "TotalDamage_Closest", string.Empty, m_damageAmount + m_extraDamageToClosestTarget);
		AddTokenInt(tokens, "ExtraDamageOnClosest", string.Empty, m_extraDamageToClosestTarget);
		AddTokenInt(tokens, "ConeDamageAmount", string.Empty, m_coneDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_coneEnemyHitEffect, "ConeEnemyHitEffect", m_coneEnemyHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarinePrimaryAttack))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SpaceMarinePrimaryAttack;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetLaserHitActors(targets, caster, out Vector3 zoneEndPoint, null);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			zoneEndPoint,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> laserHitActors = GetLaserHitActors(targets, caster, out _, nonActorTargetInfo);
		Vector3 origin = caster.GetCurrentBoardSquare().ToVector3();
		for (int i = 0; i < laserHitActors.Count; i++)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(laserHitActors[i], origin));
			int num = GetLaserDamage();
			if (i == 0)
			{
				num += GetExtraDamageToClosestTarget();
			}
			actorHitResults.SetBaseDamage(num);
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (laserHitActors.Count > 0 && AddConeOnFirstHitTarget())
		{
			Vector3 loSCheckPos = laserHitActors[0].GetLoSCheckPos();
			Vector3 aimDirection = targets[0].AimDirection;
			List<ActorData> coneHitActors = GetConeHitActors(targets, caster, loSCheckPos, aimDirection, nonActorTargetInfo);
			foreach (ActorData actorData in coneHitActors)
			{
				if (!laserHitActors.Contains(actorData))
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
					actorHitResults.SetBaseDamage(GetConeDamageAmount());
					actorHitResults.AddStandardEffectInfo(GetConeEnemyHitEffect());
					abilityResults.StoreActorHit(actorHitResults);
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetLaserHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out Vector3 zoneEndPoint,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		LaserTargetingInfo laserTargetInfo = GetLaserTargetInfo();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			laserTargetInfo.range,
			laserTargetInfo.width,
			caster,
			caster.GetOtherTeams(),
			laserTargetInfo.penetrateLos,
			-1,
			m_laserLengthIgnoreWorldGeo,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		zoneEndPoint = laserCoords.end;
		return actorsInLaser;
	}

	// added in rogues
	private List<ActorData> GetConeHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		Vector3 coneStartPos,
		Vector3 coneDir,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (AddConeOnFirstHitTarget())
		{
			ConeTargetingInfo coneTargetInfo = GetConeTargetInfo();
			return AreaEffectUtils.GetActorsInCone(
				coneStartPos,
				VectorUtils.HorizontalAngle_Deg(coneDir),
				coneTargetInfo.m_widthAngleDeg,
				coneTargetInfo.m_radiusInSquares,
				coneTargetInfo.m_backwardsOffset,
				coneTargetInfo.m_penetrateLos,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
		}
		return null;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SpaceMarineStats.NumSlowsPlusRootsApplied);
		}
	}
#endif
}
