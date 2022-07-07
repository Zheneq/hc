// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartDashAndAimShield : Ability
{
	[Header("-- Charge Size")]
	public float m_chargeRadius = 2f;
	public float m_radiusAroundStart;
	public float m_radiusAroundEnd;
	public bool m_chargePenetrateLos;
	[Header("-- Hit Damage and Effect (in Charge)")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_allyHealAmount;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Shield Barrier (Barrier Data specified on Passive)")]
	public bool m_allowAimAtDiagonals;
	[Header("-- Cooldown by distance, [ Cooldown = Max(minCooldown, distance+cooldownModifierAdd) ], add modifier can be negative")]
	public bool m_setCooldownByDistance = true;
	public int m_minCooldown = 1;
	public int m_cooldownModifierAdd;
	[Header("-- Distance by Energy")]
	public bool m_useEnergyForMoveDistance;
	public int m_minEnergyToCast = 30;
	public int m_energyPerMove = 15;
	public bool m_useAllEnergyIfUsedForDistance = true;
	[Header("-- For Hitting In Front of Shield (damage is added to base damage)")]
	public bool m_hitInFrontOfShield;
	public float m_shieldFrontHitLength = 1.5f;
	public int m_damageForShieldFront;
	public StandardEffectInfo m_shieldFrontEnemyEffect;
	public bool m_shieldFrontLangthIgnoreLos;
	[Header("-- Sequences")]
	public GameObject m_applyShieldSequencePrefab;

	private bool m_snapToGrid = true;
	private Passive_Rampart m_passive;
	private AbilityMod_RampartDashAndAimShield m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedShieldFrontEnemyEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

#if SERVER
	private Barrier m_lastGatheredBarrier;
	private Vector3 m_lastGatheredBarrierFacing = Vector3.forward;
#endif
	
	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Intercept";
		}
		if (GetNumTargets() != 2)
		{
			Debug.LogError("RampartDashAndAimShield: Expected 2 entries in Target Data");
		}
		SetupTargeter();
		ResetTooltipAndTargetingNumbers();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_passive == null)
		{
			m_passive = GetComponent<PassiveData>().GetPassiveOfType(typeof(Passive_Rampart)) as Passive_Rampart;
		}
		float width = m_passive != null ? m_passive.GetShieldBarrierData().m_width : 3f;
		ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE targeter1 = new AbilityUtil_Targeter_ChargeAoE(this, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetChargeRadius(), 0, false, ChargePenetrateLos());
		targeter1.SetAffectedGroups(true, IncludeAllies(), false);
		Targeters.Add(targeter1);
		if (HitInFrontOfShield())
		{
			AbilityUtil_Targeter_RampartKnockbackBarrier targeter2 = new AbilityUtil_Targeter_RampartKnockbackBarrier(this, width, GetShieldFrontHitLength(), m_shieldFrontLangthIgnoreLos, 0f, KnockbackType.AwayFromSource, false, true, false);
			targeter2.SetUseMultiTargetUpdate(true);
			targeter2.SetTooltipSubjectType(AbilityTooltipSubject.Primary);
			Targeters.Add(targeter2);
		}
		else
		{
			AbilityUtil_Targeter_Barrier targeter2 = new AbilityUtil_Targeter_Barrier(this, width, m_snapToGrid, AllowAimAtDiagonals(), false);
			targeter2.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter2);
		}
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedShieldFrontEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect)
			: m_shieldFrontEnemyEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect || GetAllyHealAmount() > 0;
	}

	public float GetChargeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeRadiusMod.GetModifiedValue(m_chargeRadius)
			: m_chargeRadius;
	}

	public float GetRadiusAroundStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusAroundStartMod.GetModifiedValue(m_radiusAroundStart)
			: m_radiusAroundStart;
	}

	public float GetRadiusAroundEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusAroundEndMod.GetModifiedValue(m_radiusAroundEnd)
			: m_radiusAroundEnd;
	}

	public bool ChargePenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargePenetrateLosMod.GetModifiedValue(m_chargePenetrateLos)
			: m_chargePenetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAllyHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public bool AllowAimAtDiagonals()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allowAimAtDiagonalsMod.GetModifiedValue(m_allowAimAtDiagonals)
			: m_allowAimAtDiagonals;
	}

	public bool SetCooldownByDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_setCooldownByDistanceMod.GetModifiedValue(m_setCooldownByDistance)
			: m_setCooldownByDistance;
	}

	public int GetMinCooldown()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minCooldownMod.GetModifiedValue(m_minCooldown)
			: m_minCooldown;
	}

	public int GetCooldownModifierAdd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd)
			: m_cooldownModifierAdd;
	}

	public bool UseEnergyForMoveDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useEnergyForMoveDistanceMod.GetModifiedValue(m_useEnergyForMoveDistance)
			: m_useEnergyForMoveDistance;
	}

	public int GetMinEnergyToCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast)
			: m_minEnergyToCast;
	}

	public int GetEnergyPerMove()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove)
			: m_energyPerMove;
	}

	public bool UseAllEnergyIfUsedForDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useAllEnergyIfUsedForDistanceMod.GetModifiedValue(m_useAllEnergyIfUsedForDistance)
			: m_useAllEnergyIfUsedForDistance;
	}

	public bool HitInFrontOfShield()
	{
		return m_abilityMod != null
			? m_abilityMod.m_hitInFrontOfShieldMod.GetModifiedValue(m_hitInFrontOfShield)
			: m_hitInFrontOfShield;
	}

	public float GetShieldFrontHitLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldFrontHitLengthMod.GetModifiedValue(m_shieldFrontHitLength)
			: m_shieldFrontHitLength;
	}

	public int GetDamageForShieldFront()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront)
			: m_damageForShieldFront;
	}

	public StandardEffectInfo GetShieldFrontEnemyEffect()
	{
		return m_cachedShieldFrontEnemyEffect ?? m_shieldFrontEnemyEffect;
	}

	public float GetShieldFrontLaserWidth()
	{
		return m_passive.GetShieldBarrierData().m_width;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartDashAndAimShield abilityMod_RampartDashAndAimShield = modAsBase as AbilityMod_RampartDashAndAimShield;
		AddTokenInt(tokens, "DamageAmount", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AllyHealAmount", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_allyHealAmountMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "MinCooldown", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_minCooldownMod.GetModifiedValue(m_minCooldown)
			: m_minCooldown);
		AddTokenInt(tokens, "CooldownModifierAdd", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_cooldownModifierAddMod.GetModifiedValue(m_cooldownModifierAdd)
			: m_cooldownModifierAdd);
		AddTokenInt(tokens, "MinEnergyToCast", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_minEnergyToCastMod.GetModifiedValue(m_minEnergyToCast)
			: m_minEnergyToCast);
		AddTokenInt(tokens, "EnergyPerMove", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_energyPerMoveMod.GetModifiedValue(m_energyPerMove)
			: m_energyPerMove);
		AddTokenInt(tokens, "DamageForShieldFront", "", abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_damageForShieldFrontMod.GetModifiedValue(m_damageForShieldFront)
			: m_damageForShieldFront);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RampartDashAndAimShield != null
			? abilityMod_RampartDashAndAimShield.m_shieldFrontEnemyEffectMod.GetModifiedValue(m_shieldFrontEnemyEffect)
			: m_shieldFrontEnemyEffect, "ShieldFrontEnemyEffect", m_shieldFrontEnemyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealAmount());
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		if (m_passive != null)
		{
			m_passive.GetShieldBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes1 = Targeter.GetTooltipSubjectTypes(targetActor);
		List<AbilityTooltipSubject> tooltipSubjectTypes2 = Targeters[1].GetTooltipSubjectTypes(targetActor);
		dictionary[AbilityTooltipSymbol.Damage] = 0;
		if (tooltipSubjectTypes1 != null
			&& tooltipSubjectTypes1.Contains(AbilityTooltipSubject.Enemy)
			&& tooltipSubjectTypes1.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] += GetDamageAmount();
		}
		if (tooltipSubjectTypes2 != null
			&& tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy)
			&& tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] += GetDamageForShieldFront();
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result = true;
		if (UseEnergyForMoveDistance())
		{
			result = caster.TechPoints >= GetMinEnergyToCast();
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}
		if (targetIndex == 0)
		{
			BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare);
			if (path == null)
			{
				return false;
			}
			if (!UseEnergyForMoveDistance() || GetEnergyPerMove() <= 0)
			{
				return true;
			}
			int maxLen = caster.TechPoints / GetEnergyPerMove();
			int len = 0;
			BoardSquarePathInfo it = path;
			while (it.next != null)
			{
				it = it.next;
				len++;
			}
			return len <= maxLen;
		}
		else
		{
			return Board.Get().GetSquare(currentTargets[0].GridPos) == targetSquare;
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartDashAndAimShield))
		{
			m_abilityMod = abilityMod as AbilityMod_RampartDashAndAimShield;
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

#if SERVER
	// added in rogues
	internal override Vector3 GetFacingDirAfterMovement(ServerEvadeUtils.EvadeInfo evade)
	{
		GetBarrierPositionAndFacing(evade.m_request.m_targets, evade.GetMover(), out Vector3 _, out Vector3 result);
		return result;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.SetShieldBarrier(m_lastGatheredBarrier, m_lastGatheredBarrierFacing);
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetBarrierPositionAndFacing(targets, caster, out Vector3 _, out Vector3 vector2);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		return new ServerClientUtils.SequenceStartData(m_applyShieldSequencePrefab, square.ToVector3(), Quaternion.LookRotation(vector2), additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		GetBarrierPositionAndFacing(targets, caster, out Vector3 center, out Vector3 vector);
		Barrier barrier = new Barrier(m_abilityName, center, vector, caster, m_passive.GetShieldBarrierData(), true, abilityResults.SequenceSource);
		barrier.SetSourceAbility(this);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(Board.Get().GetSquare(targets[0].GridPos).ToVector3()));
		positionHitResults.AddBarrier(barrier);
		if (ServerAbilityUtils.CurrentlyGatheringRealResults())
		{
			m_lastGatheredBarrier = barrier;
			m_lastGatheredBarrierFacing = vector;
		}
		abilityResults.StorePositionHit(positionHitResults);
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (SetCooldownByDistance() || UseEnergyForMoveDistance())
		{
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, Board.Get().GetSquare(targets[0].GridPos), squareAtPhaseStart, false);
			int num = -1;
			while (boardSquarePathInfo != null)
			{
				num++;
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			if (SetCooldownByDistance())
			{
				int num2 = Mathf.Max(m_minCooldown, num + GetCooldownModifierAdd());
				num2 = Mathf.Max(0, num2);
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(this);
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(actionTypeOfAbility, num2));
			}
			if (UseEnergyForMoveDistance())
			{
				int techPointLoss = UseAllEnergyIfUsedForDistance() ? caster.TechPoints : Mathf.Max(0, num * GetEnergyPerMove());
				actorHitResults.SetTechPointLoss(techPointLoss);
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetChargeHitActors(targets, caster, out var list, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData, squareAtPhaseStart.ToVector3()));
			if (actorData.GetTeam() != caster.GetTeam())
			{
				int num3 = GetDamageAmount();
				if (list.Contains(actorData))
				{
					num3 += GetDamageForShieldFront();
					actorHitResults2.AddStandardEffectInfo(GetShieldFrontEnemyEffect());
				}
				actorHitResults2.SetBaseDamage(num3);
				actorHitResults2.AddStandardEffectInfo(GetEnemyHitEffect());
			}
			else
			{
				actorHitResults2.SetBaseHealing(GetAllyHealAmount());
				actorHitResults2.AddStandardEffectInfo(GetAllyHitEffect());
			}
			abilityResults.StoreActorHit(actorHitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private void GetBarrierPositionAndFacing(List<AbilityTarget> targets, ActorData caster, out Vector3 position, out Vector3 facing)
	{
		facing = targets[0].AimDirection;
		position = caster.GetFreePos();
		if (targets.Count > 1 && m_snapToGrid)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				facing = VectorUtils.GetDirectionAndOffsetToClosestSide(square, targets[1].FreePos, AllowAimAtDiagonals(), out Vector3 vector);
				position = caster.GetFreePos() + vector;
			}
		}
	}

	// added in rogues
	private List<ActorData> GetChargeHitActors(List<AbilityTarget> targets, ActorData caster, out List<ActorData> shieldFrontHitActors, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		shieldFrontHitActors = new List<ActorData>();
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(squareAtPhaseStart);
		Vector3 loSCheckPos2 = caster.GetLoSCheckPos(Board.Get().GetSquare(targets[0].GridPos));
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), true);
		List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(loSCheckPos, loSCheckPos2, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetChargeRadius(), ChargePenetrateLos(), caster, relevantTeams, nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRadiusOfLine);
		if (HitInFrontOfShield())
		{
			GetBarrierPositionAndFacing(targets, caster, out Vector3 _, out Vector3 vector2);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = loSCheckPos2;
			shieldFrontHitActors = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, GetShieldFrontHitLength() + 0.5f, GetShieldFrontLaserWidth(), caster, caster.GetOtherTeams(), false, -1, m_shieldFrontLangthIgnoreLos, true, out laserCoords.end, nonActorTargetInfo, null, true);
			ServerAbilityUtils.RemoveEvadersFromHitTargets(ref shieldFrontHitActors);
			if (shieldFrontHitActors.Count > 0)
			{
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, -1f * vector2, 2f, GetShieldFrontLaserWidth(), caster, caster.GetOtherTeams(), true, -1, true, true, out Vector3 _, null, null, true);
				for (int i = shieldFrontHitActors.Count - 1; i >= 0; i--)
				{
					ActorData item = shieldFrontHitActors[i];
					if (actorsInLaser.Contains(item))
					{
						shieldFrontHitActors.RemoveAt(i);
					}
				}
			}
		}
		foreach (ActorData hitActor in shieldFrontHitActors)
		{
			if (!actorsInRadiusOfLine.Contains(hitActor))
			{
				actorsInRadiusOfLine.Add(hitActor);
			}
		}
		actorsInRadiusOfLine.Remove(caster);
		return actorsInRadiusOfLine;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return new List<Vector3>
		{
			targets[0].FreePos
		};
	}

	// added in rogues
	public override bool UseTargeterGridPosForCameraBounds()
	{
		return false;
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RampartStats.UltDamageDealtPlusDodged, damageDodged);
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.RampartStats.UltDamageDealtPlusDodged, results.FinalDamage);
		}
	}
#endif
}
