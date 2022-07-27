// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalCharge : Ability
{
	public int m_damageAmount = 20;
	public float m_lifeOnFirstHit;
	public float m_lifePerHit;
	public int m_maxTargetsHit = 1;
	public AbilityAreaShape m_targetShape;
	public bool m_targetShapePenetratesLoS;
	public bool m_chaseTarget = true;
	public StandardEffectInfo m_chaserEffect;
	public StandardEffectInfo m_enemyTargetEffect;
	public StandardEffectInfo m_allyTargetEffect;
	public float m_recoveryTime = 1f;
	[Header("-- Targeting: Whether require dashing at target actor")]
	public bool m_requireTargetActor = true;
	public bool m_canIncludeEnemy = true;
	public bool m_canIncludeAlly = true;
	[Header("-- Cooldown reduction on hitting target")]
	public int m_cdrOnHittingAlly;
	public int m_cdrOnHittingEnemy;
	private AbilityMod_RobotAnimalCharge m_abilityMod;

#if SERVER
	// added in rogues
	private Passive_RobotAnimal m_passive;
	// added in rogues
	private AbilityData.ActionType m_actionType = AbilityData.ActionType.INVALID_ACTION;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Death Snuggle";
		}
#if SERVER
		// added in rogues
		PassiveData passiveData = GetComponent<PassiveData>();
		if (passiveData != null)
		{
			m_passive = passiveData.GetPassiveOfType(typeof(Passive_RobotAnimal)) as Passive_RobotAnimal;
		}
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_actionType = abilityData.GetActionTypeOfAbility(this);
		}
#endif
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(
			this,
			m_targetShape,
			m_targetShapePenetratesLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			CanIncludeEnemy(),
			CanIncludeAlly())
		{
			m_forceChase = true
		};
		if (ModdedLifeOnFirstHit() > 0f || ModdedLifePerHit() > 0f)
		{
			// reactor
			abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
			abilityUtil_Targeter_Charge.m_affectCasterDelegate = TargeterIncludeCaster;
			// rogues
			// abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
		}
		Targeter = abilityUtil_Targeter_Charge;
	}

	// removed in rogues
	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		return AbilityUtils.GetEnemyCount(actorsSoFar, caster) > 0;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	public int ModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
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

	public int ModdedHealOnNextTurnStartIfKilledTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnNextTurnStartIfKilledTarget
			: 0;
	}

	public StandardEffectInfo ModdedEffectForSelfPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectToSelfPerAdjacentAlly
			: new StandardEffectInfo();
	}

	public int ModdedTechPointGainPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointsPerAdjacentAlly
			: 0;
	}

	public bool RequireTargetActor()
	{
		return m_abilityMod != null
			? m_abilityMod.m_requireTargetActorMod.GetModifiedValue(m_requireTargetActor)
			: m_requireTargetActor;
	}

	public bool CanIncludeEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canIncludeEnemyMod.GetModifiedValue(m_canIncludeEnemy)
			: m_canIncludeEnemy;
	}

	public bool CanIncludeAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canIncludeAllyMod.GetModifiedValue(m_canIncludeAlly)
			: m_canIncludeAlly;
	}

	public int GetCdrOnHittingAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnHittingAllyMod.GetModifiedValue(m_cdrOnHittingAlly)
			: m_cdrOnHittingAlly;
	}

	public int GetCdrOnHittingEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnHittingEnemyMod.GetModifiedValue(m_cdrOnHittingEnemy)
			: m_cdrOnHittingEnemy;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (!RequireTargetActor())
		{
			return true;
		}
		return HasTargetableActorsInDecision(
			caster,
			CanIncludeEnemy(),
			CanIncludeAlly(),
			false,
			ValidateCheckPath.CanBuildPath,
			true,
			false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!RequireTargetActor())
		{
			return true;
		}
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, CanIncludeAlly(), CanIncludeEnemy());
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_targetShape,
			target,
			m_targetShapePenetratesLoS,
			caster,
			relevantTeams,
			null);
		foreach (ActorData current in actorsInShape)
		{
			if (CanTargetActorInDecision(
				    caster,
				    current,
				    CanIncludeEnemy(),
				    CanIncludeAlly(),
				    false,
				    ValidateCheckPath.CanBuildPath,
				    true,
				    false))
			{
				return true;
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Tertiary, Mathf.RoundToInt(m_lifeOnFirstHit));
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Quaternary, Mathf.RoundToInt(m_lifePerHit));
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, ModdedDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, Mathf.RoundToInt(Mathf.Max(ModdedLifeOnFirstHit(), ModdedLifePerHit())));
		return numbers;
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
			// reactor
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			// rogues
			// List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			
			dictionary[AbilityTooltipSymbol.Healing] = GetLifeGainAmount(visibleActorsInRangeByTooltipSubject.Count);
		}
		// reactor
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		// rogues
		// else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage();
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalCharge abilityMod_RobotAnimalCharge = modAsBase as AbilityMod_RobotAnimalCharge;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_RobotAnimalCharge != null
			? abilityMod_RobotAnimalCharge.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyTargetEffect, "EnemyTargetEffect", m_enemyTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyTargetEffect, "AllyTargetEffect", m_allyTargetEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalCharge))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalCharge;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public int GetLifeGainAmount(int hitCount)
	{
		float num = 0f;
		if (hitCount > 0 && ModdedLifeOnFirstHit() != 0f)
		{
			num += ModdedLifeOnFirstHit();
		}
		if (ModdedLifePerHit() != 0f)
		{
			num += ModdedLifePerHit() * hitCount;
		}
		return Mathf.RoundToInt(num);
	}
	
#if SERVER
	// added in rogues
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[chargeSegments.Length - 1].m_pos;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] ProcessChargeDodge(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerEvadeUtils.ChargeInfo charge,
		List<ServerEvadeUtils.EvadeInfo> evades)
	{
		return ServerEvadeUtils.ProcessChargeDodgeForStopOnTargetHit(
			Board.Get().GetSquare(targets[0].GridPos),
			targets,
			caster,
			charge,
			evades);
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		ServerEvadeUtils.ChargeSegment[] array;
		if (targets[0].GetCurrentBestActorTarget() != null)
		{
			array = new ServerEvadeUtils.ChargeSegment[3];
		}
		else
		{
			array = new ServerEvadeUtils.ChargeSegment[2];
		}
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare()
		};
		array[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = Board.Get().GetSquare(targets[0].GridPos),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement
		};
		if (targets[0].GetCurrentBestActorTarget() == null)
		{
			array[1].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
		}
		else
		{
			array[1].m_end = BoardSquarePathInfo.ChargeEndType.Impact;
			array[2] = new ServerEvadeUtils.ChargeSegment
			{
				m_reverseFacing = true,
				m_segmentMovementDuration = m_recoveryTime,
				m_cycle = BoardSquarePathInfo.ChargeCycleType.Recovery,
				m_end = BoardSquarePathInfo.ChargeEndType.Recovery,
				m_pos = Board.Get().GetSquare(targets[0].GridPos)
			};
		}
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
		foreach (ServerEvadeUtils.ChargeSegment segment in array)
		{
			if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
			{
				segment.m_segmentMovementSpeed = segmentMovementSpeed;
			}
		}
		return array;
	}

	// added in rogues
	private List<ActorData> GetHitTargets(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, CanIncludeAlly(), CanIncludeEnemy());
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_targetShape,
			targets[0],
			m_targetShapePenetratesLoS,
			caster,
			relevantTeams,
			null);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInShape);
		actorsInShape.Remove(caster);
		if (m_maxTargetsHit > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref actorsInShape, targets[0].FreePos);
			int num = Mathf.Min(m_maxTargetsHit, actorsInShape.Count);
			int num2 = actorsInShape.Count - num;
			if (num2 > 0)
			{
				int index = actorsInShape.Count - num2;
				actorsInShape.RemoveRange(index, num2);
			}
		}
		return actorsInShape;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetFreePos(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitTargets = GetHitTargets(targets, caster, null);
		if (m_passive != null)
		{
			if (hitTargets.Count > 0)
			{
				m_passive.m_chargeLastHitTurn = GameFlowData.Get().CurrentTurn;
			}
			m_passive.m_chargeHitActors.Clear();
			foreach (ActorData actorData in hitTargets)
			{
				if (actorData.GetTeam() != caster.GetTeam())
				{
					m_passive.m_chargeHitActors.Add(actorData);
				}
			}
		}
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(targets, caster, nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_targetShape, targets[0]);
		ActorHitResults actorHitResults = null;
		int num = 0;
		for (int i = 0; i < hitTargets.Count; i++)
		{
			ActorData actorData = hitTargets[i];
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			if (actorData.GetTeam() != caster.GetTeam())
			{
				num++;
				actorHitResults2.SetBaseDamage(ModdedDamage());
				actorHitResults2.AddStandardEffectInfo(m_enemyTargetEffect);
				if (m_passive != null && m_passive.m_shouldApplyAdditionalEffectFromStealth && m_passive.HasEffectOnNextDamageAttack())
				{
					actorHitResults2.AddStandardEffectInfo(m_passive.GetEffectOnNextDamageAttack());
				}
				if (m_passive != null && m_passive.ShouldApplyExtraDamageNextAttack())
				{
					actorHitResults2.AddBaseDamage(m_passive.GetExtraDamageNextAttack());
				}
				if (GetCdrOnHittingEnemy() > 0)
				{
					actorHitResults2.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_actionType, -1 * GetCdrOnHittingEnemy()));
				}
			}
			else
			{
				actorHitResults2.AddStandardEffectInfo(m_allyTargetEffect);
				if (GetCdrOnHittingAlly() > 0)
				{
					actorHitResults2.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(m_actionType, -1 * GetCdrOnHittingAlly()));
				}
			}
			if (m_chaseTarget && i == 0)
			{
				actorHitResults = new ActorHitResults(new ActorHitParameters(caster, centerOfShape));
				actorHitResults.AddStandardEffectInfo(m_chaserEffect);
				actorHitResults2.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.CasterForceChaseTarget));
			}
			abilityResults.StoreActorHit(actorHitResults2);
		}
		int num2 = ModdedTechPointGainPerAdjacentAlly();
		StandardEffectInfo standardEffectInfo = ModdedEffectForSelfPerAdjacentAlly();
		if (num2 > 0 || (standardEffectInfo != null && standardEffectInfo.m_applyEffect))
		{
			List<BoardSquare> list = new List<BoardSquare>();
			Board.Get().GetAllAdjacentSquares(caster.GetCurrentBoardSquare().x, caster.GetCurrentBoardSquare().y, ref list);
			foreach (BoardSquare boardSquare in list)
			{
				if (boardSquare.OccupantActor != null && boardSquare.OccupantActor.GetTeam() == caster.GetTeam() && boardSquare.OccupantActor != caster && !boardSquare.OccupantActor.IgnoreForAbilityHits)
				{
					if (actorHitResults == null)
					{
						actorHitResults = new ActorHitResults(new ActorHitParameters(caster, centerOfShape));
					}
					if (standardEffectInfo != null && standardEffectInfo.m_applyEffect)
					{
						actorHitResults.AddStandardEffectInfo(standardEffectInfo);
					}
					if (num2 > 0)
					{
						actorHitResults.AddTechPointGainOnCaster(num2);
					}
				}
			}
		}
		if (ServerAbilityUtils.CurrentlyGatheringRealResults() && m_passive != null)
		{
			m_passive.m_shouldApplyAdditionalEffectFromStealth = false;
		}
		if (num > 0)
		{
			int lifeGainAmount = GetLifeGainAmount(num);
			if (actorHitResults == null)
			{
				actorHitResults = new ActorHitResults(new ActorHitParameters(caster, centerOfShape));
			}
			actorHitResults.SetBaseHealing(lifeGainAmount);
			if (m_abilityMod != null && m_abilityMod.m_effectOnSelf.m_applyEffect)
			{
				actorHitResults.AddStandardEffectInfo(m_abilityMod.m_effectOnSelf);
			}
		}
		if (actorHitResults != null)
		{
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RobotAnimalStats.DamageDonePlusDodgedByPounce, damageDodged);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RobotAnimalStats.DamageDonePlusDodgedByPounce, results.FinalDamage);
		}
	}
#endif
}
