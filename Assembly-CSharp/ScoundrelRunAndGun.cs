using System.Collections.Generic;
using UnityEngine;

// TODO LOCKWOOD server
// reworked in rogues
public class ScoundrelRunAndGun : Ability  // GenericAbility_Container in rogues
{
	public int m_damageAmount = 20;
	public float m_damageRadius = 5f;
	public bool m_penetrateLineOfSight;
	[Header("-- For energy refund from mod --")]
	public bool m_energyRefundAffectedByBuff;

	private AbilityMod_ScoundrelRunAndGun m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Run and Gun";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				m_damageRadius,
				m_damageRadius,
				m_damageRadius,
				-1,
				false,
				m_penetrateLineOfSight);
		}
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(
				this,
				m_damageRadius,
				m_damageRadius,
				m_damageRadius,
				-1,
				false,
				m_penetrateLineOfSight));
			Targeters[i].SetUseMultiTargetUpdate(true);
		}
	}

	private int ModdedDamageAmount()
	{
		int damage = m_damageAmount;
		if (m_abilityMod != null)
		{
			damage = Mathf.Max(0, m_abilityMod.m_damageMod.GetModifiedValue(damage));
		}
		return damage;
	}

	private int GetTechPointGainWithNoHits()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_techPointGainWithNoHits.GetModifiedValue(0);
		}
		return 0;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetTechPointGainWithNoHits() > 0
			&& Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
		{
			return GetTechPointGainWithNoHits();
		}
		return 0;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return m_energyRefundAffectedByBuff;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScoundrelRunAndGun))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_ScoundrelRunAndGun;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, ModdedDamageAmount());
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (m_abilityMod != null && m_abilityMod.m_numTargeters > 1)
		{
			return m_abilityMod.m_numTargeters;
		}
		return 1;
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
		if (GetExpectedNumberOfTargeters() < 2)
		{
			return KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false) != null;
		}

		BoardSquare startSquare;
		if (targetIndex == 0)
		{
			startSquare = caster.GetCurrentBoardSquare();
		}
		else
		{
			startSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
		}
		bool isValidPath = KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, startSquare, false) != null;
		float dist = Vector3.Distance(startSquare.ToVector3(), targetSquare.ToVector3());
		bool isValidDist = dist >= m_abilityMod.m_minDistanceBetweenSteps * Board.Get().squareSize
			&& dist <= m_abilityMod.m_maxDistanceBetweenSteps * Board.Get().squareSize;
		if (isValidPath && isValidDist && m_abilityMod.m_minDistanceBetweenAnySteps > 0f)
		{
			for (int i = 0; i < targetIndex; i++)
			{
				BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
				float distFromPrev = Vector3.Distance(prevTargetSquare.ToVector3(), targetSquare.ToVector3());
				isValidDist &= distFromPrev >= m_abilityMod.m_minDistanceBetweenAnySteps * Board.Get().squareSize;
			}
		}
		return isValidPath && isValidDist;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
	
#if SERVER
	// added in rogues
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		if (GetExpectedNumberOfTargeters() < 2)
		{
			return base.GetValidChargeTestSourceSquare(chargeSegments);
		}
		return chargeSegments[chargeSegments.Length - 1].m_pos;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		if (GetExpectedNumberOfTargeters() < 2)
		{
			return base.GetChargeBestSquareTestVector(chargeSegments);
		}
		return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return GetExpectedNumberOfTargeters() > 1;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
		ServerEvadeUtils.ChargeSegment[] segments = new ServerEvadeUtils.ChargeSegment[expectedNumberOfTargeters + 1];
		segments[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.Pivot
		};
		for (int i = 0; i < expectedNumberOfTargeters; i++)
		{
			int num = i + 1;
			segments[num] = new ServerEvadeUtils.ChargeSegment
			{
				m_pos = Board.Get().GetSquare(targets[i].GridPos),
				m_end = BoardSquarePathInfo.ChargeEndType.Pivot
			};
		}
		segments[segments.Length - 1].m_end = BoardSquarePathInfo.ChargeEndType.Impact;
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(segments));
		foreach (ServerEvadeUtils.ChargeSegment segment in segments)
		{
			if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
			{
				segment.m_segmentMovementSpeed = segmentMovementSpeed;
			}
		}
		return segments;
	}

	// added in rogues
	public override BoardSquare GetIdealDestination(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
		if (expectedNumberOfTargeters < 2)
		{
			return base.GetIdealDestination(targets, caster, additionalData);
		}
		return Board.Get().GetSquare(targets[expectedNumberOfTargeters - 1].GridPos);
	}
	
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_sequencePrefab,
			Board.Get().GetSquareFromVec3(caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart())),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (GetExpectedNumberOfTargeters() > 1)
		{
			Log.Error("Multiple targeters not supported!");
		}
		
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		BoardSquare startSquare = caster.GetSquareAtPhaseStart();
		Vector3 startPos = startSquare.ToVector3();
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(
			startPos,
			targetSquare.ToVector3(),
			m_damageRadius,
			m_damageRadius,
			m_damageRadius,
			m_penetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		int damage = ModdedDamageAmount();
		foreach (ActorData hitActor in actors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, startPos);
			ActorHitResults hitResults = new ActorHitResults(damage, HitActionType.Damage, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// custom
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ScoundrelStats.DamageDodgedPlusDealtByUlt, damageDodged);
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ScoundrelStats.DamageDodgedPlusDealtByUlt, results.FinalDamage);
		}
	}
#endif
}
