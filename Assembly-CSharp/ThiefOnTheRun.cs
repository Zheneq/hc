// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ThiefOnTheRun : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiStep = true;
	public float m_minDistanceBetweenSteps = 2f;
	public float m_minDistanceBetweenAnySteps = -1f;
	public float m_maxDistanceBetweenSteps = 10f;
	[Header("-- Dash Hit Size")]
	public float m_dashRadius = 1f;
	public bool m_dashPenetrateLineOfSight;
	[Header("-- Hit Damage and Effect")]
	public int m_damageAmount;
	public int m_subsequentDamage;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Hid On Self")]
	public StandardEffectInfo m_effectOnSelfThroughSmokeField;
	public int m_cooldownReductionIfNoEnemy;
	public AbilityData.ActionType m_cooldownReductionOnAbility = AbilityData.ActionType.ABILITY_3;
	[Header("-- Spoil Powerup Spawn")]
	public SpoilsSpawnData m_spoilSpawnInfo;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private int m_numChargePiviots = 1;
	private AbilityMod_ThiefOnTheRun m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEffectOnSelfThroughSmokeField;
	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "On the Run";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_numChargePiviots = m_targeterMultiStep
			? Mathf.Max(GetNumTargets(), 1)
			: 1;
		float dashRadius = GetDashRadius();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_ChargeAoE(
				this,
				dashRadius,
				dashRadius,
				dashRadius,
				-1,
				false,
				DashPenetrateLineOfSight());
		}
		else
		{
			ClearTargeters();
			int expectedNumberOfTargeters = GetExpectedNumberOfTargeters();
			for (int i = 0; i < expectedNumberOfTargeters; i++)
			{
				AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
					this,
					dashRadius,
					dashRadius,
					dashRadius,
					-1,
					false,
					DashPenetrateLineOfSight());
				if (i < expectedNumberOfTargeters - 1)
				{
					targeter.UseEndPosAsDamageOriginIfOverlap = true;
				}
				Targeters.Add(targeter);
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_numChargePiviots;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (GetMaxDistanceBetweenSteps() - 0.5f) * m_numChargePiviots;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEffectOnSelfThroughSmokeField = m_abilityMod != null
			? m_abilityMod.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField)
			: m_effectOnSelfThroughSmokeField;
		m_cachedSpoilSpawnInfo = m_abilityMod != null
			? m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo)
			: m_spoilSpawnInfo;
	}

	public float GetMinDistanceBetweenSteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenStepsMod.GetModifiedValue(m_minDistanceBetweenSteps)
			: m_minDistanceBetweenSteps;
	}

	public float GetMinDistanceBetweenAnySteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceBetweenAnyStepsMod.GetModifiedValue(m_minDistanceBetweenAnySteps)
			: m_minDistanceBetweenAnySteps;
	}

	public float GetMaxDistanceBetweenSteps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistanceBetweenStepsMod.GetModifiedValue(m_maxDistanceBetweenSteps)
			: m_maxDistanceBetweenSteps;
	}

	public float GetDashRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashRadiusMod.GetModifiedValue(m_dashRadius)
			: m_dashRadius;
	}

	public bool DashPenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashPenetrateLineOfSightMod.GetModifiedValue(m_dashPenetrateLineOfSight)
			: m_dashPenetrateLineOfSight;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetSubsequentDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage)
			: m_subsequentDamage;
	}

	// TODO THIEF unused
	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public StandardEffectInfo GetEffectOnSelfThroughSmokeField()
	{
		return m_cachedEffectOnSelfThroughSmokeField ?? m_effectOnSelfThroughSmokeField;
	}

	public int GetCooldownReductionIfNoEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy)
			: m_cooldownReductionIfNoEnemy;
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		return m_cachedSpoilSpawnInfo ?? m_spoilSpawnInfo;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (ActorData != null && ActorData.GetCurrentBoardSquare() != null)
		{
			for (int i = 0; i <= currentTargeterIndex && i < Targeters.Count; i++)
			{
				BoardSquare startSquare = i > 0
					? Board.Get().GetSquare(Targeters[i - 1].LastUpdatingGridPos)
					: ActorData.GetCurrentBoardSquare();
				int subsequentAmount = GetSubsequentDamage();
				if (targetActor.GetCurrentBoardSquare() == startSquare)
				{
					subsequentAmount = 0;
				}
				AddNameplateValueForOverlap(
					ref symbolToValue,
					Targeters[i],
					targetActor,
					currentTargeterIndex,
					GetDamageAmount(),
					subsequentAmount);
			}
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefOnTheRun abilityMod_ThiefOnTheRun = modAsBase as AbilityMod_ThiefOnTheRun;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "SubsequentDamage", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_subsequentDamageMod.GetModifiedValue(m_subsequentDamage)
			: m_subsequentDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_effectOnSelfThroughSmokeFieldMod.GetModifiedValue(m_effectOnSelfThroughSmokeField)
			: m_effectOnSelfThroughSmokeField, "EffectOnSelfThroughSmokeField", m_effectOnSelfThroughSmokeField);
		AddTokenInt(tokens, "CooldownReductionIfNoEnemy", string.Empty, abilityMod_ThiefOnTheRun != null
			? abilityMod_ThiefOnTheRun.m_cooldownReductionIfNoEnemyMod.GetModifiedValue(m_cooldownReductionIfNoEnemy)
			: m_cooldownReductionIfNoEnemy);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay())
		{
			return false;
		}
		if (GetExpectedNumberOfTargeters() < 2)
		{
			return KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
		}
		BoardSquare startSquare = targetIndex == 0
			? caster.GetCurrentBoardSquare()
			: Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
		bool canCharge = KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, startSquare, false) != null;
		float stepDist = Vector3.Distance(startSquare.ToVector3(), targetSquare.ToVector3());
		bool isValidStepDist =
			stepDist >= GetMinDistanceBetweenSteps() * Board.Get().squareSize
			&& stepDist <= GetMaxDistanceBetweenSteps() * Board.Get().squareSize;
		if (canCharge
		    && isValidStepDist
		    && GetMinDistanceBetweenAnySteps() > 0f)
		{
			for (int i = 0; i < targetIndex; i++)
			{
				BoardSquare visitedSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
				float distToVisitedSquare = Vector3.Distance(visitedSquare.ToVector3(), targetSquare.ToVector3());
				isValidStepDist &= distToVisitedSquare >= GetMinDistanceBetweenAnySteps() * Board.Get().squareSize;
			}
		}

		return canCharge && isValidStepDist;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefOnTheRun))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefOnTheRun;
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
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		int numChargePivots = m_numChargePiviots;
		if (numChargePivots < 2)
		{
			return base.GetChargePath(targets, caster, additionalData);
		}
		
		ServerEvadeUtils.ChargeSegment[] path = new ServerEvadeUtils.ChargeSegment[numChargePivots + 1];
		path[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.Pivot
		};
		for (int i = 0; i < numChargePivots; i++)
		{
			path[i + 1] = new ServerEvadeUtils.ChargeSegment
			{
				m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
				m_pos = Board.Get().GetSquare(targets[i].GridPos)
			};
		}
		path[path.Length - 1].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(path));
		foreach (ServerEvadeUtils.ChargeSegment segment in path)
		{
			if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
			{
				segment.m_segmentMovementSpeed = segmentMovementSpeed;
			}
		}
		return path;
	}

	// added in rogues
	public override BoardSquare GetIdealDestination(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		int numChargePivots = m_numChargePiviots;
		if (numChargePivots < 2)
		{
			return base.GetIdealDestination(targets, caster, additionalData);
		}
		return Board.Get().GetSquare(targets[numChargePivots - 1].GridPos);
	}

	// added in rogues
	internal override bool IsStealthEvade()
	{
		if (ActorData == null || !GetEffectOnSelfThroughSmokeField().m_applyEffect)
		{
			return false;
		}
		StatusType[] statusChanges = GetEffectOnSelfThroughSmokeField().m_effectData.m_statusChanges;
		bool isInvisible = false;
		foreach (StatusType statusType in statusChanges)
		{
			if (statusType == StatusType.InvisibleToEnemies)
			{
				isInvisible = true;
				break;
			}
		}
		return isInvisible && IsDashingOverSmokeBomb(ActorData);
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 targetPos = caster.GetLoSCheckPos(Board.Get().GetSquare(targets[0].GridPos));
		FindHitActors(targets, caster, out _, out List<List<ActorData>> actorsInSegments, out _, null);
		for (int i = 0; i < actorsInSegments.Count; i++)
		{
			if (i == 0
			    && additionalData.m_abilityResults.HitActorList().Contains(caster)
			    && !actorsInSegments[i].Contains(caster))
			{
				actorsInSegments[i].Add(caster);
			}
			list.Add(new ServerClientUtils.SequenceStartData(m_castSequencePrefab,
				targetPos,
				actorsInSegments[i].ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new SimpleAttachedVFXSequence.MultiEventExtraParams
				{
					eventNumberToKeyOffOf = i
				}.ToArray()));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(
			targets,
			caster,
			out Dictionary<ActorData, int> actorToHitCount,
			out _,
			out Dictionary<ActorData, Vector3> actorToDamageOrigins,
			nonActorTargetInfo);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, actorToDamageOrigins[hitActor]));
			int damage = GetDamageAmount() + (actorToHitCount[hitActor] - 1) * GetSubsequentDamage();
			actorHitResults.SetBaseDamage(damage);
			actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), GetSpoilSpawnInfo()));
			abilityResults.StoreActorHit(actorHitResults);
		}
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		bool affectsCaster = false;
		if (GetCooldownReductionIfNoEnemy() > 0 && hitActors.Count == 0)
		{
			casterHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
				m_cooldownReductionOnAbility, -1 * GetCooldownReductionIfNoEnemy()));
			affectsCaster = true;
		}
		if (GetEffectOnSelfThroughSmokeField().m_applyEffect && IsDashingOverSmokeBomb(caster))
		{
			casterHitResults.AddStandardEffectInfo(GetEffectOnSelfThroughSmokeField());
			affectsCaster = true;
		}
		if (affectsCaster)
		{
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private bool IsDashingOverSmokeBomb(ActorData caster)
	{
		List<Effect> thiefEffects = ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(ThiefSmokeBombEffect));
		List<ThiefSmokeBombEffect> bombEffects = new List<ThiefSmokeBombEffect>();
		foreach (Effect effect in thiefEffects)
		{
			if (effect is ThiefSmokeBombEffect thiefSmokeBombEffect)
			{
				bombEffects.Add(thiefSmokeBombEffect);
			}
		}

		foreach (BoardSquare square in ServerActionBuffer.Get().GetSquaresInProcessedEvade(caster))
		{
			foreach (ThiefSmokeBombEffect bombEffect in bombEffects)
			{
				if (bombEffect.IsSquareInAnyShape(square))
				{
					return true;
				}
			}
		}
		return false;
	}

	// added in rogues
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, int> actorToHitCount,
		out List<List<ActorData>> actorsInSegments,
		out Dictionary<ActorData, Vector3> actorToDamageOrigins,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		List<ActorData> hitActors = new List<ActorData>();
		actorToHitCount = new Dictionary<ActorData, int>();
		actorsInSegments = new List<List<ActorData>>();
		actorToDamageOrigins = new Dictionary<ActorData, Vector3>();
		float dashRadius = GetDashRadius();
		for (int i = 0; i < m_numChargePiviots; i++)
		{
			List<ActorData> hitActorsInSegment = new List<ActorData>();
			BoardSquare segmentStartSquare = i > 0
				? Board.Get().GetSquare(targets[i - 1].GridPos)
				: squareAtPhaseStart;
			BoardSquare segmentEndSquare = Board.Get().GetSquare(targets[i].GridPos);
			Vector3 segmentStartPos = caster.GetLoSCheckPos(segmentStartSquare);
			Vector3 segmentEndPos = caster.GetLoSCheckPos(segmentEndSquare);
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(
				segmentStartPos,
				segmentEndPos,
				dashRadius,
				dashRadius,
				dashRadius,
				DashPenetrateLineOfSight(),
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRadiusOfLine);
			foreach (ActorData hitActor in actorsInRadiusOfLine)
			{
				if (!hitActors.Contains(hitActor))
				{
					hitActors.Add(hitActor);
				}
				if (actorToHitCount.ContainsKey(hitActor))
				{
					if (hitActor.GetCurrentBoardSquare() != segmentStartSquare)
					{
						actorToHitCount[hitActor]++;
					}
				}
				else
				{
					actorToHitCount[hitActor] = 1;
					hitActorsInSegment.Add(hitActor);
				}
				if (actorToDamageOrigins.ContainsKey(hitActor))
				{
					ActorCover actorCover = hitActor.GetActorCover();
					if (!actorCover.IsInCoverWrt(segmentStartPos) // , out _ in rogues
					    && actorCover.IsInCoverWrt(actorToDamageOrigins[hitActor])) // , out _ in rogues
					{
						actorToDamageOrigins[hitActor] = segmentStartPos;
					}
				}
				else
				{
					actorToDamageOrigins[hitActor] = segmentStartPos;
				}
			}
			actorsInSegments.Add(hitActorsInSegment);
		}
		return hitActors;
	}
#endif
}
