// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class GremlinsBombingRun : Ability
{
	[Header("-- Targeter ------------------------------------")]
	public bool m_targeterMultiStep = true;
	public int m_squaresPerExplosion = 3;
	[Header("-- Multi-Step Targeter Only --")]
	public int m_maxSquaresPerStep = 6;
	public float m_maxAngleWithFirstStep = 45f;
	[Header("-- Explosion ------------------------------------")]
	public int m_explosionDamageAmount = 5;
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- Sequences ------------------------------------")]
	public GameObject m_castSequencePrefab;
	public GameObject m_animListenerSequencePrefab;

	private AbilityMod_GremlinsBombingRun m_abilityMod;
	private int m_numSteps = 1;
	private GremlinsLandMineInfoComponent m_bombInfoComp;

	public AbilityMod_GremlinsBombingRun GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bombing Run";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (GetMaxSquaresPerJump() - 1) * m_numSteps;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	public int GetMinSquaresPerJump()
	{
		int squaresPerExplosion = m_abilityMod != null
			? m_abilityMod.m_minSquaresPerExplosionMod.GetModifiedValue(m_squaresPerExplosion)
			: m_squaresPerExplosion;
		return Mathf.Max(1, squaresPerExplosion);
	}

	public int GetMaxSquaresPerJump()
	{
		int maxSquaresPerStep = m_abilityMod != null
			? m_abilityMod.m_maxSquaresPerExplosionMod.GetModifiedValue(m_maxSquaresPerStep)
			: m_maxSquaresPerStep;
		if (maxSquaresPerStep > 0 && maxSquaresPerStep < GetMinSquaresPerJump())
		{
			maxSquaresPerStep = GetMinSquaresPerJump() + 1;
		}
		return maxSquaresPerStep;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		return m_abilityMod != null
			? m_abilityMod.m_angleWithFirstStepMod.GetModifiedValue(m_maxAngleWithFirstStep)
			: m_maxAngleWithFirstStep;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape)
			: m_explosionShape;
	}

	public bool ShouldLeaveMinesAtTouchedSquares()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_shouldLeaveMinesAtTouchedSquares.GetModifiedValue(false);
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		m_numSteps = m_targeterMultiStep
			? Mathf.Max(GetNumTargets(), 1)
			: 1;
		if (m_numSteps < 2)
		{
			Targeter = new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump());
		}
		else
		{
			ClearTargeters();
			for (int i = 0; i < m_numSteps; i++)
			{
				Targeters.Add(new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump()));
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_numSteps;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isValid = true;
		BoardSquare square = Board.Get().GetSquare(target.GridPos);
		if (square == null || !square.IsValidForGameplay())
		{
			return false;
		}
		if (m_numSteps < 2)
		{
			isValid = KnockbackUtils.CanBuildStraightLineChargePath(
				caster,
				square,
				caster.GetCurrentBoardSquare(),
				false,
				out _);
		}
		else
		{
			if (targetIndex > 0)
			{
				BoardSquare square2 = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
				if (square == square2)
				{
					isValid = false;
				}
				else if (GetMaxAngleWithFirstSegment() > 0f)
				{
					Vector3 b = targetIndex > 1
						? Board.Get().GetSquare(currentTargets[targetIndex - 2].GridPos).ToVector3()
						: caster.GetFreePos();
					Vector3 a = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos).ToVector3();
					Vector3 from = a - b;
					from.y = 0f;
					Vector3 to = square.ToVector3() - square2.ToVector3();
					if (Mathf.RoundToInt(Vector3.Angle(from, to)) > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
					{
						isValid = false;
					}
				}
			}
			if (isValid)
			{
				bool canJump;
				int numSquaresInPath;
				if (targetIndex == 0)
				{
					canJump = KnockbackUtils.CanBuildStraightLineChargePath(
						caster,
						square,
						caster.GetCurrentBoardSquare(),
						false,
						out numSquaresInPath);
				}
				else
				{
					canJump = KnockbackUtils.CanBuildStraightLineChargePath(
						caster,
						square,
						Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos),
						false,
						out numSquaresInPath);
				}

				isValid = canJump
				       && isValid
				       && numSquaresInPath > GetMinSquaresPerJump()
				       && (GetMaxSquaresPerJump() <= 0 || numSquaresInPath <= GetMaxSquaresPerJump());
				if (isValid && targetIndex < m_numSteps - 1)
				{
					List<BoardSquare> list = new List<BoardSquare>();
					list.Add(caster.GetCurrentBoardSquare());
					for (int i = 0; i < targetIndex; i++)
					{
						if (i >= currentTargets.Count)
						{
							break;
						}
						list.Add(Board.Get().GetSquare(currentTargets[i].GridPos));
					}
					isValid = CanTargetForNextClick(square, targetIndex + 1, list, caster);
				}
			}
		}
		return isValid;
	}

	private bool CanTargetForNextClick(
		BoardSquare fromSquare,
		int nextTargetIndex,
		List<BoardSquare> squaresAddedSoFar,
		ActorData caster)
	{
		if (nextTargetIndex >= m_numSteps)
		{
			return true;
		}
		if (nextTargetIndex <= 0 || squaresAddedSoFar.Count <= 0 || fromSquare == null)
		{
			return false;
		}
		bool isValid = false;
		Vector3 prevPos = squaresAddedSoFar[squaresAddedSoFar.Count - 1].ToVector3();
		Vector3 curPos = fromSquare.ToVector3();
		Vector3 prevJump = curPos - prevPos;
		prevJump.y = 0f;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(prevJump);
		float coneWidthDegrees = Mathf.Clamp(2f * GetMaxAngleWithFirstSegment() + 10f, 0f, 360f);
		if (GetMaxAngleWithFirstSegment() <= 0f)
		{
			coneWidthDegrees = 360f;
		}
		float coneLengthRadiusInSquares = GetMaxSquaresPerJump() * 1.42f;
		List<BoardSquare> squaresInCone = AreaEffectUtils.GetSquaresInCone(
			fromSquare.ToVector3(),
			coneCenterAngleDegrees,
			coneWidthDegrees,
			coneLengthRadiusInSquares,
			0f,
			false,
			caster);
		float minDistance = GetMinSquaresPerJump() * Board.Get().squareSize;
		foreach (BoardSquare squareInRange in squaresInCone)
		{
			if (isValid)
			{
				break;
			}
			if (!squareInRange.IsValidForGameplay() || squareInRange == fromSquare)
			{
				continue;
			}
			Vector3 nextJump = squareInRange.ToVector3() - fromSquare.ToVector3();
			nextJump.y = 0f;
			if (nextJump.magnitude < minDistance
			    || Mathf.RoundToInt(Vector3.Angle(prevJump, nextJump)) > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
			{
				continue;
			}
			bool canJump = KnockbackUtils.CanBuildStraightLineChargePath(
				caster,
				squareInRange,
				fromSquare,
				false,
				out var numSquaresInPath);

			if (canJump
			    && numSquaresInPath > GetMinSquaresPerJump()
			    && (GetMaxSquaresPerJump() <= 0 || numSquaresInPath <= GetMaxSquaresPerJump()))
			{
				isValid = true;
			}
		}
		return isValid;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsBombingRun abilityMod_GremlinsBombingRun = modAsBase as AbilityMod_GremlinsBombingRun;
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_GremlinsBombingRun != null
			? abilityMod_GremlinsBombingRun.m_damageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsBombingRun))
		{
			m_abilityMod = abilityMod as AbilityMod_GremlinsBombingRun;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
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
		int numSteps = m_numSteps;
		if (numSteps < 2)
		{
			return base.GetChargePath(targets, caster, additionalData);
		}
		ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[numSteps + 1];
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.Pivot
		};
		for (int i = 0; i < numSteps; i++)
		{
			int num = i + 1;
			array[num] = new ServerEvadeUtils.ChargeSegment
			{
				m_pos = Board.Get().GetSquare(targets[i].GridPos),
				m_end = BoardSquarePathInfo.ChargeEndType.Pivot
			};
		}
		array[array.Length - 1].m_end = BoardSquarePathInfo.ChargeEndType.Impact;
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
	public override BoardSquare GetIdealDestination(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		int numSteps = m_numSteps;
		if (numSteps < 2)
		{
			return base.GetIdealDestination(targets, caster, additionalData);
		}
		return Board.Get().GetSquare(targets[numSteps - 1].GridPos);
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetHitActors(targets, caster, out var hitTargetsList, out var explosionCenterSquares, null);
		List<Vector3> list3 = new List<Vector3>();
		foreach (BoardSquare square in explosionCenterSquares)
		{
			list3.Add(square.ToVector3());
		}
		HitOnAnimationEventSequence.ExtraParams extraParams = new HitOnAnimationEventSequence.ExtraParams
		{
			hitPositionsList = list3,
			hitTargetsList = hitTargetsList
		};
		list.Add(new ServerClientUtils.SequenceStartData(
			m_animListenerSequencePrefab,
			Vector3.zero,
			new ActorData[0],
			caster,
			additionalData.m_sequenceSource,
			extraParams.ToArray()));
		list.Add(new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			Vector3.zero,
			new ActorData[0],
			caster,
			additionalData.m_sequenceSource));
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<List<NonActorTargetInfo>> nonActorTargetInfoInExplosions = new List<List<NonActorTargetInfo>>();
		List<Barrier> processedBarriers = new List<Barrier>();
		GetHitActors(
			targets,
			caster,
			out var sequenceHitActors,
			out var explosionCenterSquares,
			nonActorTargetInfoInExplosions);
		List<ActorData> processedHits = new List<ActorData>();
		for (int i = 0; i < explosionCenterSquares.Count; i++)
		{
			foreach (ActorData actorData in sequenceHitActors[i])
			{
				if (!processedHits.Contains(actorData))
				{
					ActorHitParameters actorHitParameters = new ActorHitParameters(actorData, explosionCenterSquares[i].ToVector3());
					ActorHitResults actorHitResults = new ActorHitResults(actorHitParameters);
					actorHitResults.SetBaseDamage(GetDamageAmount());
					abilityResults.StoreActorHit(actorHitResults);
					processedHits.Add(actorData);
				}
			}
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(explosionCenterSquares[i].ToVector3()));
			if (ShouldLeaveMinesAtTouchedSquares())
			{
				GremlinsLandMineEffect effect = m_bombInfoComp.CreateLandmineEffect(AsEffectSource(), caster, explosionCenterSquares[i]);
				positionHitResults.AddEffect(effect);
				foreach (Effect effect2 in ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GremlinsLandMineEffect)))
				{
					if (effect2.TargetSquare == explosionCenterSquares[i])
					{
						positionHitResults.AddEffectForRemoval(effect2, ServerEffectManager.Get().GetWorldEffects());
					}
				}
			}
			List<NonActorTargetInfo> explosionTargetInfo = nonActorTargetInfoInExplosions[i];
			for (int j = explosionTargetInfo.Count - 1; j >= 0; j--)
			{
				NonActorTargetInfo nonActorTargetInfo = explosionTargetInfo[j];
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock)
				{
					NonActorTargetInfo_BarrierBlock nonActorTargetInfo_BarrierBlock = nonActorTargetInfo as NonActorTargetInfo_BarrierBlock;
					if (nonActorTargetInfo_BarrierBlock.m_barrier != null
					    && !processedBarriers.Contains(nonActorTargetInfo_BarrierBlock.m_barrier))
					{
						nonActorTargetInfo_BarrierBlock.AddPositionReactionHitToAbilityResults(caster, positionHitResults, abilityResults, false);
						processedBarriers.Add(nonActorTargetInfo_BarrierBlock.m_barrier);
					}
					explosionTargetInfo.RemoveAt(j);
				}
			}
			abilityResults.StorePositionHit(positionHitResults);
		}
		foreach (List<NonActorTargetInfo> nonActorTargetInfo2 in nonActorTargetInfoInExplosions)
		{
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo2);
		}
	}

	// added in rogues
	private List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> sequenceHitActors,
		out List<BoardSquare> explosionCenterSquares,
		List<List<NonActorTargetInfo>> nonActorTargetInfoInExplosions)
	{
		explosionCenterSquares = new List<BoardSquare>();
		if (m_numSteps < 2)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(caster, square);
			if (path != null)
			{
				BoardSquarePathInfo step = path;
				int num = 0;
				while (step != null)
				{
					if (num % GetMinSquaresPerJump() == 0)
					{
						explosionCenterSquares.Add(step.square);
					}
					step = step.next;
					num++;
				}
			}
			else
			{
				explosionCenterSquares.Add(caster.GetSquareAtPhaseStart());
			}
		}
		else
		{
			explosionCenterSquares.Add(caster.GetSquareAtPhaseStart());
			for (int i = 0; i < m_numSteps; i++)
			{
				explosionCenterSquares.Add(Board.Get().GetSquare(targets[i].GridPos));
			}
		}
		return GetHitActorsFromSquares(explosionCenterSquares, caster, out sequenceHitActors, nonActorTargetInfoInExplosions);
	}

	// added in rogues
	private List<ActorData> GetHitActorsFromSquares(
		List<BoardSquare> explosionSquares,
		ActorData caster,
		out List<List<ActorData>> sequenceHitActors,
		List<List<NonActorTargetInfo>> nonActorTargetInfoForExplosions)
	{
		sequenceHitActors = new List<List<ActorData>>();
		nonActorTargetInfoForExplosions?.Clear();
		List<ActorData> list = new List<ActorData>();
		foreach (BoardSquare square in explosionSquares)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = nonActorTargetInfoForExplosions != null
				? new List<NonActorTargetInfo>()
				: null;
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetExplosionShape(), square.ToVector3(), square);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				GetExplosionShape(),
				centerOfShape,
				square,
				false,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			nonActorTargetInfoForExplosions?.Add(nonActorTargetInfo);
			ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInShape);
			sequenceHitActors.Add(actorsInShape);
			foreach (ActorData item in actorsInShape)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.BaseDamage <= 0 || !results.IsFromMovement())
		{
			return;
		}
		if (results.ForMovementStage == MovementStage.Knockback)
		{
			Ability abilityOfType = caster.GetAbilityData().GetAbilityOfType(typeof(GremlinsBigBang));
			int currentTurn = GameFlowData.Get().CurrentTurn;
			if (abilityOfType.m_actorLastHitTurn != null
			    && abilityOfType.m_actorLastHitTurn.ContainsKey(target)
			    && abilityOfType.m_actorLastHitTurn[target] == currentTurn)
			{
				caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.GremlinsStats.MinesTriggeredByKnockbacksFromMe);
			}
		}
		else if (results.ForMovementStage == MovementStage.Normal
		         || results.ForMovementStage == MovementStage.Evasion)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.GremlinsStats.MinesTriggeredByMovers);
		}
	}
#endif
}
