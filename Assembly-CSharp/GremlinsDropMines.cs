// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class GremlinsDropMines : Ability
{
	[Header("-- Mine Placement")]
	public AbilityAreaShape m_minePlaceShape = AbilityAreaShape.Three_x_Three;
	public bool m_placeBombOnCasterSquare;
	public bool m_ignoreLos;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefabNorth;
	public GameObject m_castSequencePrefabSouth;
	public GameObject m_castSequencePrefabEast;
	public GameObject m_castSequencePrefabWest;
	public GameObject m_castSequencePrefabDiag;

	private GremlinsLandMineInfoComponent m_bombInfoComp;
	private AbilityMod_GremlinsDropMines m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffectInfo;

	public AbilityMod_GremlinsDropMines GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Drop Mines";
		}
		m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_minePlaceShape,
			m_ignoreLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never);
		Targeter.ShowArcToShape = false;
		ResetTooltipAndTargetingNumbers();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombInfoComp.m_damageAmount);
			m_bombInfoComp.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombInfoComp.GetDamageOnMovedOver());
			GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int result = 0;
		if (m_bombInfoComp != null && m_bombInfoComp.GetEnergyOnExplosion() > 0)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			result = m_bombInfoComp.GetEnergyOnExplosion() * visibleActorsInRangeByTooltipSubject.Count;
		}
		return result;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_damageAmount);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			AddTokenInt(tokens, "Damage", string.Empty, component.m_damageAmount);
			AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration);
			AddTokenInt(tokens, "EnergyGainOnMineHit", "energy gain on mine explosion", component.m_energyGainOnExplosion);
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsDropMines))
		{
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_GremlinsDropMines;
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (m_bombInfoComp != null)
		{
			m_cachedEnemyHitEffectInfo = m_bombInfoComp.GetEnemyHitEffectOnMovedOver();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_cachedEnemyHitEffectInfo
			: m_bombInfoComp.m_enemyHitEffect;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		caster.TurnToDirection(new Vector3(0f, 0f, 1f));
	}

#if SERVER
	// added in rogues
	public GameObject GetCastSequenceForSquare(ActorData caster, BoardSquare targetSquare)
	{
		Vector3 vector = caster.GetCurrentBoardSquare().ToVector3() - targetSquare.ToVector3();
		vector.y = 0f;
		vector.Normalize();
		if (Vector3.Dot(vector, new Vector3(0f, 0f, -1f)) > 0.9f)
		{
			return m_castSequencePrefabNorth;
		}
		if (Vector3.Dot(vector, new Vector3(0f, 0f, 1f)) > 0.9f)
		{
			return m_castSequencePrefabSouth;
		}
		if (Vector3.Dot(vector, new Vector3(-1f, 0f, 0f)) > 0.9f)
		{
			return m_castSequencePrefabEast;
		}
		if (Vector3.Dot(vector, new Vector3(1f, 0f, 0f)) > 0.9f)
		{
			return m_castSequencePrefabWest;
		}
		return m_castSequencePrefabDiag;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetHitActorsAndBombEffects(targets, caster, out var hitActors, out var mineSquares);
		foreach (ActorData actor in hitActors)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				GetCastSequenceForSquare(caster, actor.GetCurrentBoardSquare()),
				actor.GetCurrentBoardSquare(),
				actor.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		foreach (BoardSquare square in mineSquares)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				GetCastSequenceForSquare(caster, square),
				square,
				new ActorData[0],
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		GetHitActorsAndBombEffects(targets, caster, out var hitActors, out var mineSquares);
		Vector3 freePos = caster.GetFreePos();
		foreach (ActorData target in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, freePos));
			actorHitResults.SetBaseDamage(m_bombInfoComp.GetDamageOnMovedOver());
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			actorHitResults.SetTechPointGainOnCaster(m_bombInfoComp.GetEnergyOnExplosion());
			abilityResults.StoreActorHit(actorHitResults);
		}
		List<Effect> worldEffectsByCaster = ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GremlinsLandMineEffect));
		foreach (BoardSquare boardSquare in mineSquares)
		{
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(boardSquare.ToVector3()));
			GremlinsLandMineEffect effect = m_bombInfoComp.CreateLandmineEffect(AsEffectSource(), caster, boardSquare);
			positionHitResults.AddEffect(effect);
			foreach (Effect oldEffect in worldEffectsByCaster)
			{
				if (oldEffect.TargetSquare == boardSquare)
				{
					positionHitResults.AddEffectForRemoval(oldEffect, ServerEffectManager.Get().GetWorldEffects());
				}
			}
			abilityResults.StorePositionHit(positionHitResults);
		}
	}

	// added in rogues
	private void GetHitActorsAndBombEffects(List<AbilityTarget> targets, ActorData caster, out List<ActorData> hitActors, out List<BoardSquare> mineSquares)
	{
		hitActors = new List<ActorData>();
		mineSquares = new List<BoardSquare>();
		List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
			m_minePlaceShape,
			caster.GetFreePos(),
			caster.GetCurrentBoardSquare(),
			m_ignoreLos,
			caster);
		foreach (BoardSquare boardSquare in squaresInShape)
		{
			if (boardSquare.OccupantActor != null
			    && !boardSquare.OccupantActor.IgnoreForAbilityHits
			    && boardSquare.OccupantActor.GetTeam() != caster.GetTeam())
			{
				hitActors.Add(boardSquare.OccupantActor);
			}
			else if (m_placeBombOnCasterSquare
			         || boardSquare.OccupantActor == null
			         || boardSquare.OccupantActor != caster)
			{
				mineSquares.Add(boardSquare);
			}
		}
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
			else if (results.ForMovementStage == MovementStage.Normal || results.ForMovementStage == MovementStage.Evasion)
			{
				caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.GremlinsStats.MinesTriggeredByMovers);
			}
		}
	}
#endif
}
