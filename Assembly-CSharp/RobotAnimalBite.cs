// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalBite : Ability
{
	public bool m_penetrateLineOfSight;
	public int m_damageAmount = 20;
	public float m_width = 1f;
	public float m_distance = 2f;
	public int m_maxTargets = 1;
	public float m_lifeOnFirstHit;
	public float m_lifePerHit;

	private AbilityMod_RobotAnimalBite m_abilityMod;
	private RobotAnimal_SyncComponent m_syncComp;

#if SERVER
	// added in rogues
	private Passive_RobotAnimal m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Megabyte";
		}
		m_syncComp = GetComponent<RobotAnimal_SyncComponent>();
		
#if SERVER
		// added in rogues
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = component.GetPassiveOfType(typeof(Passive_RobotAnimal)) as Passive_RobotAnimal;
		}
#endif
		
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(
			this,
			m_width,
			m_distance,
			m_penetrateLineOfSight,
			m_maxTargets,
			false,
			true)
		{
			m_affectCasterDelegate = (ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0
		};
		Targeter = abilityUtil_Targeter_Laser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_distance;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(m_lifeOnFirstHit)),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, Mathf.RoundToInt(m_lifePerHit))
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			int lifeGainAmount = GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
			dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(lifeGainAmount);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			int damageToSelf = 0;
			dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage(false, ref damageToSelf);
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalBite abilityMod_RobotAnimalBite = modAsBase as AbilityMod_RobotAnimalBite;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_RobotAnimalBite != null
			? abilityMod_RobotAnimalBite.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "LifeFirstHit", string.Empty, Mathf.RoundToInt(m_lifeOnFirstHit));
		AddTokenInt(tokens, "LifePerHit", string.Empty, Mathf.RoundToInt(m_lifePerHit));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalBite))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalBite;
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	public float ModdedLifeOnFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lifeOnFirstHitMod.GetModifiedValue(m_lifeOnFirstHit)
			: m_lifeOnFirstHit;
	}

	public float ModdedLifePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lifePerHitMod.GetModifiedValue(m_lifePerHit)
			: m_lifePerHit;
	}

	public int ModdedDamage(bool includeVariance, ref int damageToSelf)
	{
		if (m_abilityMod == null)
		{
			return m_damageAmount;
		}
		int damage = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		if (m_syncComp != null)
		{
			if (m_abilityMod.m_extraDamageOnConsecutiveCast > 0
			    && m_syncComp.m_biteLastCastTurn > 0
			    && GameFlowData.Get().CurrentTurn - m_syncComp.m_biteLastCastTurn == 1)
			{
				damage += m_abilityMod.m_extraDamageOnConsecutiveCast;
			}
			if (m_abilityMod.m_extraDamageOnConsecutiveHit > 0
			    && m_syncComp.m_biteLastHitTurn > 0
			    && GameFlowData.Get().CurrentTurn - m_syncComp.m_biteLastHitTurn == 1)
			{
				damage += m_abilityMod.m_extraDamageOnConsecutiveHit;
			}
		}
		if (includeVariance
		    && m_abilityMod.m_varianceExtraDamageMin >= 0
		    && m_abilityMod.m_varianceExtraDamageMax - m_abilityMod.m_varianceExtraDamageMin > 0)
		{
			int randomDamage = GameplayRandom.Range(m_abilityMod.m_varianceExtraDamageMin, m_abilityMod.m_varianceExtraDamageMax);
			damage += randomDamage;
			damageToSelf = Mathf.RoundToInt(randomDamage * m_abilityMod.m_varianceExtraDamageToSelf);
		}
		return damage;
	}

	public bool HasEffectOnNextTurnStart()
	{
		return m_abilityMod != null && m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect;
	}

	public StandardEffectInfo EffectInfoOnNextTurnStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_perAdjacentEnemyEffectOnSelfNextTurn
			: new StandardEffectInfo();
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float healing = 0f;
		if (hitCount > 0 && ModdedLifeOnFirstHit() != 0f)
		{
			healing += ModdedLifeOnFirstHit();
		}
		if (ModdedLifePerHit() != 0f)
		{
			healing += ModdedLifePerHit() * hitCount;
		}
		return Mathf.RoundToInt(healing);
	}
	
#if SERVER
	// added in rogues
	private List<ActorData> GetHitTargets(
		List<AbilityTarget> targets,
		ActorData caster,
		out Vector3 zoneEndPoint,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			m_distance,
			m_width,
			caster,
			caster.GetOtherTeams(),
			m_penetrateLineOfSight,
			m_maxTargets,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		zoneEndPoint = laserCoords.end;
		return actorsInLaser;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetHitTargets(targets, caster, out Vector3 targetPos, null);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targetPos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[0]);
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitTargets = GetHitTargets(targets, caster, out Vector3 _, null);
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_biteLastCastTurn = GameFlowData.Get().CurrentTurn;
			if (hitTargets.Count > 0)
			{
				m_syncComp.Networkm_biteLastHitTurn = GameFlowData.Get().CurrentTurn;
			}
		}
		if (m_passive != null)
		{
			m_passive.m_biteLastCastTurn = GameFlowData.Get().CurrentTurn;
			m_passive.m_biteAdjacentEnemies.Clear();
			List<BoardSquare> squares = new List<BoardSquare>();
			Board.Get().GetAllAdjacentSquares(caster.GetCurrentBoardSquare().x, caster.GetCurrentBoardSquare().y, ref squares);
			foreach (BoardSquare square in squares)
			{
				if (square.OccupantActor != null
				    && square.OccupantActor.GetTeam() != caster.GetTeam()
				    && !square.OccupantActor.IgnoreForAbilityHits)
				{
					m_passive.m_biteAdjacentEnemies.Add(square.OccupantActor);
				}
			}
		}
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(targets, caster, out Vector3 _, nonActorTargetInfo);
		Vector3 origin = caster.GetCurrentBoardSquare().ToVector3();
		int damageToSelf = 0;
		int baseDamage = ModdedDamage(true, ref damageToSelf);
		foreach (ActorData target in hitTargets)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, origin));
			actorHitResults.SetBaseDamage(baseDamage);
			if (m_abilityMod != null)
			{
				actorHitResults.AddStandardEffectInfo(m_abilityMod.m_effectOnEnemyOverride);
			}
			if (m_passive != null && m_passive.m_shouldApplyAdditionalEffectFromStealth && m_passive.HasEffectOnNextDamageAttack())
			{
				actorHitResults.AddStandardEffectInfo(m_passive.GetEffectOnNextDamageAttack());
			}
			if (m_passive != null && m_passive.ShouldApplyExtraDamageNextAttack())
			{
				actorHitResults.AddBaseDamage(m_passive.GetExtraDamageNextAttack());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		
		// TODO PUP suspicious broken code
		ActorHitParameters hitParams = null;
		new ActorHitParameters(caster, origin);
		ActorHitResults actorHitResults2 = null;
		new ActorHitResults(hitParams);
		
		if (damageToSelf > 0)
		{
			actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, origin));
			actorHitResults2.SetBaseDamage(damageToSelf);
		}
		if (ServerAbilityUtils.CurrentlyGatheringRealResults() && m_passive != null)
		{
			m_passive.m_shouldApplyAdditionalEffectFromStealth = false;
		}
		int lifeGainAmount = GetLifeGainAmount(hitTargets.Count);
		if (lifeGainAmount > 0f)
		{
			if (actorHitResults2 == null)
			{
				actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, origin));
			}
			actorHitResults2.SetBaseHealing(lifeGainAmount);
		}
		if (actorHitResults2 != null)
		{
			abilityResults.StoreActorHit(actorHitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster == target && results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RobotAnimalStats.HealingFromPrimary, results.FinalHealing);
		}
	}
#endif
}
