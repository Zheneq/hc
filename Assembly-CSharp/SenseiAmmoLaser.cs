// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SenseiAmmoLaser : Ability
{
	[Header("-- Targeting --")]
	public LaserTargetingInfo m_laserTargetingInfo;
	public int m_maxOrbsPerCast = 3;
	[Header("-- On Hit --")]
	public int m_damage = 10;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_healOnAlly = 10;
	public StandardEffectInfo m_allyHitEffect;
	public int m_healOnSelfPerHit = 5;
	[Header("-- Sequences: assuming projectile sequence, one per hit target/ammo")]
	public GameObject m_castSequencePrefab;
	public float m_delayBetweenOrbSequence = 0.25f;

	private Sensei_SyncComponent m_syncComp;

#if SERVER
	// added in rogues
	private Passive_Sensei m_passive;
	private int m_numOrbsUsedInLastGatherResults;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiAmmoLaser";
		}
		Setup();
	}

	private void Setup()
	{
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType(typeof(Passive_Sensei)) as Passive_Sensei;
#endif
		m_syncComp = GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, m_laserTargetingInfo);
		abilityUtil_Targeter_Laser.SetAffectedGroups(m_laserTargetingInfo.affectsEnemies, m_laserTargetingInfo.affectsAllies, m_healOnSelfPerHit > 0);
		abilityUtil_Targeter_Laser.m_customMaxTargetsDelegate = GetMaxTargetsForTargeter;
		abilityUtil_Targeter_Laser.m_affectCasterDelegate = TargeterIncludeCaster;
		Targeter = abilityUtil_Targeter_Laser;
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		return m_healOnSelfPerHit > 0 && actorsSoFar.Count > 0;
	}

	private int GetMaxTargetsForTargeter(ActorData caster)
	{
		return GetCurrentMaxTargets();
	}

	public int GetCurrentMaxTargets()
	{
		int targets = m_laserTargetingInfo.maxTargets;
		if (m_syncComp != null)
		{
			targets = m_syncComp.m_syncCurrentNumOrbs;
			if (m_maxOrbsPerCast > 0 && targets > m_maxOrbsPerCast)
			{
				targets = m_maxOrbsPerCast;
			}
		}
		return targets;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return GetCurrentMaxTargets() > 0;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_healOnAlly);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, m_healOnSelfPerHit);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (m_healOnSelfPerHit > 0 && Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			results.m_healing = m_healOnSelfPerHit * (allyNum + enemyNum);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.m_numOrbsUsedByAbility += m_numOrbsUsedInLastGatherResults;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<ActorData> hitActors = GetHitActors(targets, caster, null, out Vector3 endPos);
		int targetNum = Mathf.Max(GetCurrentMaxTargets(), hitActors.Count);
		for (int i = 0; i < targetNum; i++)
		{
			bool lastTarget = i == targetNum - 1;
			bool hasTarget = i < hitActors.Count;
			Vector3 targetPos = hasTarget ? hitActors[i].GetFreePos() : endPos;
			List<ActorData> targetActors = new List<ActorData>();
			if (hasTarget)
			{
				targetActors.Add(hitActors[i]);
			}
			if (lastTarget && additionalData.m_abilityResults.HitActorList().Contains(caster))
			{
				targetActors.Add(caster);
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targetPos,
				targetActors.ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new SplineProjectileSequence.DelayedProjectileExtraParams
				{
					startDelay = m_delayBetweenOrbSequence * i
				}.ToArray()));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_numOrbsUsedInLastGatherResults = GetCurrentMaxTargets();
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, nonActorTargetInfo, out _);
		Vector3 freePos = caster.GetFreePos();
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, freePos));
			if (actorData.GetTeam() == caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(m_healOnAlly);
				actorHitResults.AddStandardEffectInfo(m_allyHitEffect);
			}
			else
			{
				actorHitResults.SetBaseDamage(m_damage);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (m_healOnSelfPerHit > 0 && hitActors.Count > 0)
		{
			int baseHealing = m_healOnSelfPerHit * hitActors.Count;
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.SetBaseHealing(baseHealing);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo, out Vector3 endPos)
	{
		LaserTargetingInfo laserTargetingInfo = m_laserTargetingInfo;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			targets[0].AimDirection,
			laserTargetingInfo.range,
			laserTargetingInfo.width,
			caster,
			laserTargetingInfo.GetAffectedTeams(caster),
			laserTargetingInfo.penetrateLos,
			0,
			false,
			true,
			out endPos,
			nonActorTargetInfo);
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInLaser, GetCurrentMaxTargets());
		return actorsInLaser;
	}
#endif
}
