// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class GremlinsMultiTargeterBasicAttack : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;
	public bool m_penetrateLos;
	public float m_minDistanceBetweenBombs = 1f;
	public bool m_useShapeForDeadzone;
	public AbilityAreaShape m_deadZoneShape;
	public float m_maxAngleWithFirst = 45f;
	[Header("-- Targeter Angle Indicator Config")]
	public bool m_useAngleIndicators = true;
	public float m_indicatorLineLength = 8f;
	[Header("-- Sequence -------------------------------")]
	public GameObject m_firstBombSequencePrefab;
	public GameObject m_subsequentBombSequencePrefab;

	private GremlinsLandMineInfoComponent m_bombInfoComp;
	private AbilityMod_GremlinsMultiTargeterBasicAttack m_abilityMod;

	public AbilityMod_GremlinsMultiTargeterBasicAttack GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MultiTargeter Basic Attack";
		}
		m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		ResetTooltipAndTargetingNumbers();
		SetupTargeter();
	}

	public AbilityAreaShape GetBombShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bombShapeMod.GetModifiedValue(m_bombShape)
			: m_bombShape;
	}

	public float GetMinDistBetweenBombs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistBetweenBombsMod.GetModifiedValue(m_minDistanceBetweenBombs)
			: m_minDistanceBetweenBombs;
	}

	public bool UseShapeForDeadzone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useShapeForDeadzoneMod.GetModifiedValue(m_useShapeForDeadzone)
			: m_useShapeForDeadzone;
	}

	public AbilityAreaShape GetDeadzoneShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_deadzoneShapeMod.GetModifiedValue(m_deadZoneShape)
			: m_deadZoneShape;
	}

	private int ModdedDirectHitDamagePerShot(int shotIndex)
	{
		return m_abilityMod != null
		       && m_abilityMod.m_directHitDamagePerShot.Count > shotIndex
		       && shotIndex >= 0
			? m_abilityMod.m_directHitDamagePerShot[shotIndex].GetModifiedValue(m_bombInfoComp.m_directHitDamageAmount)
			: m_bombInfoComp.m_directHitDamageAmount;
	}

	private float ModdedMaxAngleWithFirst()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleWithFirst.GetModifiedValue(m_maxAngleWithFirst)
			: m_maxAngleWithFirst;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsMultiTargeterBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_GremlinsMultiTargeterBasicAttack;
			SetupTargeter();
			ResetTargetingNumbersForMines();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
		ResetTargetingNumbersForMines();
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (GetExpectedNumberOfTargeters() > 1)
		{
			ClearTargeters();
			float num = ModdedMaxAngleWithFirst();
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				var targeter = new AbilityUtil_Targeter_GremlinsBombInCone(this, GetBombShape(), m_penetrateLos);
				targeter.SetTooltipSubjectTypes();
				if (num < 180f && m_useAngleIndicators)
				{
					targeter.SetAngleIndicatorConfig(true, num, m_indicatorLineLength);
				}
				targeter.SetUseMultiTargetUpdate(true);
				Targeters.Add(targeter);
			}
		}
		else
		{
			var targeter = new AbilityUtil_Targeter_GremlinsBombInCone(this, GetBombShape(), m_penetrateLos);
			targeter.SetTooltipSubjectTypes();
			Targeter = targeter;
		}
	}

	private void ResetTargetingNumbersForMines()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			GremlinsDropMines gremlinsDropMines = component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines;
			if (gremlinsDropMines != null)
			{
				gremlinsDropMines.ResetNameplateTargetingNumbers();
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_bombInfoComp.m_directHitDamageAmount);
			if (m_bombInfoComp.m_directHitDamageAmount != m_bombInfoComp.m_directHitSubsequentDamageAmount)
			{
				AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_bombInfoComp.m_directHitSubsequentDamageAmount);
			}
		}
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_directHitDamageAmount);
			list.Add(component.m_directHitSubsequentDamageAmount);
			list.Add(component.m_damageAmount);
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare firstSquare = Board.Get().GetSquare(Targeters[0].LastUpdatingGridPos);
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i > 0)
			{
				BoardSquare curSquare = Board.Get().GetSquare(Targeters[i].LastUpdatingGridPos);
				if (curSquare == null || curSquare == firstSquare)
				{
					continue;
				}
			}
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				ModdedDirectHitDamagePerShot(i),
				GetSubsequentHitDamage());
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			AddTokenInt(tokens, "Damage_DirectHit", string.Empty, component.m_directHitDamageAmount);
			AddTokenInt(tokens, "Damage_MoveOverHit", string.Empty, component.m_damageAmount);
			AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration);
		}
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
		if (UseShapeForDeadzone())
		{
			bool isSquareInDeadzone = AreaEffectUtils.IsSquareInShape(
				targetSquare,
				GetDeadzoneShape(),
				caster.GetFreePos(),
				caster.GetCurrentBoardSquare(),
				true,
				caster);
			if (isSquareInDeadzone)
			{
				return false;
			}
		}
		if (targetIndex <= 0)
		{
			return true;
		}
		Vector3 from = Board.Get().GetSquare(currentTargets[0].GridPos).ToVector3() - caster.GetFreePos();
		Vector3 to = targetSquare.ToVector3() - caster.GetFreePos();
		if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)ModdedMaxAngleWithFirst())
		{
			return false;
		}
		float minDist = GetMinDistBetweenBombs() * Board.Get().squareSize;
		for (int i = 0; i < targetIndex; i++)
		{
			BoardSquare curSquare = Board.Get().GetSquare(currentTargets[i].GridPos);
			if (curSquare == targetSquare)
			{
				return false;
			}
			Vector3 vector = targetSquare.ToVector3() - curSquare.ToVector3();
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude < minDist)
			{
				return false;
			}
		}
		return true;
	}

	public int GetSubsequentHitDamage()
	{
		return Mathf.Max(0, m_bombInfoComp.m_directHitSubsequentDamageAmount);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetBombExplosionHitActorsAndDamage(targets, caster, out _, out var bombEndPoints, out var explosionHitActors, null);
		for (int i = 0; i < bombEndPoints.Count; i++)
		{
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				i == 0 ? m_firstBombSequencePrefab : m_subsequentBombSequencePrefab,
				bombEndPoints[i],
				explosionHitActors[i].ToArray(),
				caster,
				additionalData.m_sequenceSource);
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> bombExplosionHitActorsAndDamage = GetBombExplosionHitActorsAndDamage(
			targets,
			caster,
			out var dictionary,
			out var bombEndPoints,
			out var explosionHitActors,
			nonActorTargetInfo);
		bool areAllTargetsPresent = bombExplosionHitActorsAndDamage.Keys.Count == GetExpectedNumberOfTargeters();
		bool isTagAdded = false;
		foreach (ActorData actorData in bombExplosionHitActorsAndDamage.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, dictionary[actorData]));
			actorHitResults.SetBaseDamage(bombExplosionHitActorsAndDamage[actorData]);
			if (areAllTargetsPresent && !isTagAdded)
			{
				actorHitResults.AddHitResultsTag(HitResultsTags.HittingAllTargets);
				isTagAdded = true;
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		for (int i = 0; i < bombEndPoints.Count; i++)
		{
			if (explosionHitActors[i].Count == 0)
			{
				PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(bombEndPoints[i]));
				BoardSquare hitSquare = Board.Get().GetSquareFromVec3(bombEndPoints[i]);
				GremlinsLandMineEffect effect = m_bombInfoComp.CreateLandmineEffect(AsEffectSource(), caster, hitSquare);
				positionHitResults.AddEffect(effect);
				List<Effect> oldEffects = ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GremlinsLandMineEffect));
				foreach (Effect oldEffect in oldEffects)
				{
					if (oldEffect.TargetSquare == hitSquare)
					{
						positionHitResults.AddEffectForRemoval(oldEffect, ServerEffectManager.Get().GetWorldEffects());
					}
				}
				abilityResults.StorePositionHit(positionHitResults);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetBombExplosionHitActorsAndDamage(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, Vector3> damageOrigins,
		out List<Vector3> bombEndPoints,
		out List<List<ActorData>> sequenceExplosionHitActors,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		damageOrigins = new Dictionary<ActorData, Vector3>();
		bombEndPoints = new List<Vector3>();
		sequenceExplosionHitActors = new List<List<ActorData>>();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetBombShape(), targets[i]);
			BoardSquare square = Board.Get().GetSquare(targets[i].GridPos);
			List<ActorData> hitActors = new List<ActorData>();
			List<ActorData> actorsInShape = null;
			if (square.OccupantActor != null
			    && !square.OccupantActor.IgnoreForAbilityHits
			    && square.OccupantActor.GetTeam() != caster.GetTeam())
			{
				actorsInShape = AreaEffectUtils.GetActorsInShape(
					GetBombShape(),
					centerOfShape,
					square,
					false,
					caster,
					caster.GetOtherTeams(),
					nonActorTargetInfo);
			}
			if (actorsInShape != null)
			{
				foreach (ActorData actorData in actorsInShape)
				{
					if (dictionary.ContainsKey(actorData))
					{
						dictionary[actorData] += GetSubsequentHitDamage();
					}
					else
					{
						dictionary[actorData] = ModdedDirectHitDamagePerShot(i);
						damageOrigins[actorData] = centerOfShape;
						hitActors.Add(actorData);
					}
				}
			}
			bombEndPoints.Add(centerOfShape);
			sequenceExplosionHitActors.Add(hitActors);
		}
		return dictionary;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.BaseDamage > 0 && results.IsFromMovement())
		{
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
		else if (results.BaseDamage > 0
		         && !results.IsFromMovement()
		         && results.HasHitResultsTag(HitResultsTags.HittingAllTargets))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.GremlinsStats.TurnsDirectlyHittingTwoEnemiesWithPrimary);
		}
	}
#endif
}
