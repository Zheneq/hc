// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

// empty in rogues
public class ClaymoreAoeBuffDebuff : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;
	public bool m_penetrateLos;
	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;
	public int m_selfHealAmountPerHit;
	public bool m_selfHealCountEnemyHit = true;
	public bool m_selfHealCountAllyHit = true;
	[Header("-- Normal Hit Effects")]
	public StandardEffectInfo m_selfHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Energy Gain/Loss for hit actors")]
	public int m_allyEnergyGain;
	public int m_enemyEnergyLoss;
	public bool m_energyChangeOnlyIfHasAdjacent;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_ClaymoreAoeBuffDebuff m_abilityMod;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Thundering Roar";
		}
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool GetPenetrateLos()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos) 
			: m_penetrateLos;
	}

	public int GetBaseSelfHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal)
			: m_baseSelfHeal;
	}

	public int GetSelfHealAmountPerHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit) 
			: m_selfHealAmountPerHit;
	}

	public bool GetSelfHealCountEnemyHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_selfHealCountEnemyHitMod.GetModifiedValue(m_selfHealCountEnemyHit) 
			: m_selfHealCountEnemyHit;
	}

	public bool GetSelfHealCountAllyHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_selfHealCountAllyHitMod.GetModifiedValue(m_selfHealCountAllyHit) 
			: m_selfHealCountAllyHit;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetAllyEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain)
			: m_allyEnergyGain;
	}

	public int GetEnemyEnergyLoss()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_enemyEnergyLossMod.GetModifiedValue(m_enemyEnergyLoss) 
			: m_enemyEnergyLoss;
	}

	public bool GetEnergyChangeOnlyIfHasAdjacent()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_energyChangeOnlyIfHasAdjacentMod.GetModifiedValue(m_energyChangeOnlyIfHasAdjacent) 
			: m_energyChangeOnlyIfHasAdjacent;
	}

	public bool IncludeCaster()
	{
		return GetSelfHitEffect().m_applyEffect
		       || GetBaseSelfHeal() > 0
		       || GetSelfHealAmountPerHit() > 0;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect
		       || GetAllyEnergyGain() > 0;
	}

	public bool IncludeEnemies()
	{
		return GetEnemyHitEffect().m_applyEffect
		       || GetEnemyEnergyLoss() > 0;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = IncludeCaster()
			? AbilityUtil_Targeter.AffectsActor.Possible
			: AbilityUtil_Targeter.AffectsActor.Never;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetShape(),
			GetPenetrateLos(),
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			IncludeEnemies(),
			IncludeAllies(),
			affectsCaster);
		Targeter.ShowArcToShape = false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreAoeBuffDebuff abilityMod_ClaymoreAoeBuffDebuff = modAsBase as AbilityMod_ClaymoreAoeBuffDebuff;
		string empty = string.Empty;
		AddTokenInt(tokens, "BaseSelfHeal", empty, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal)
			: m_baseSelfHeal);
		AddTokenInt(tokens, "SelfHealAmountPerHit", string.Empty, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_selfHealAmountPerHitMod.GetModifiedValue(m_selfHealAmountPerHit)
			: m_selfHealAmountPerHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AllyEnergyGain", string.Empty, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_allyEnergyGainMod.GetModifiedValue(m_allyEnergyGain)
			: m_allyEnergyGain);
		AddTokenInt(tokens, "EnemyEnergyLoss", string.Empty, abilityMod_ClaymoreAoeBuffDebuff != null
			? abilityMod_ClaymoreAoeBuffDebuff.m_enemyEnergyLossMod.GetModifiedValue(m_enemyEnergyLoss)
			: m_enemyEnergyLoss);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal() + GetSelfHealAmountPerHit());
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Ally, GetAllyEnergyGain());
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Enemy, -1 * GetEnemyEnergyLoss());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealAmountPerHit() <= 0 && GetBaseSelfHeal() <= 0)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return dictionary;
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			int enemyHits = 0;
			int allyHits = 0;
			foreach (ActorData visibleActor in visibleActorsInRangeByTooltipSubject)
			{
				if (visibleActor.GetTeam() != targetActor.GetTeam())
				{
					enemyHits++;
				}
				else if (visibleActor != targetActor)
				{
					allyHits++;
				}
			}
			dictionary[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(allyHits, enemyHits);
		}
		else if (GetEnergyChangeOnlyIfHasAdjacent())
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy)
			    && GetEnemyEnergyLoss() > 0)
			{
				int enemyEnergyLoss;
				if (AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.GetTeamAsList()))
				{
					enemyEnergyLoss = -1 * GetEnemyEnergyLoss();
				}
				else
				{
					enemyEnergyLoss = 0;
				}
				dictionary[AbilityTooltipSymbol.Energy] = enemyEnergyLoss;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally)
			         && GetAllyEnergyGain() > 0)
			{
				bool hasAdjacentAlly = AreaEffectUtils.HasAdjacentActorOfTeam(targetActor, targetActor.GetTeamAsList());
				dictionary[AbilityTooltipSymbol.Energy] = hasAdjacentAlly ? GetAllyEnergyGain() : 0;
			}
		}
		return dictionary;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		if (GetSelfHealAmountPerHit() <= 0 && GetBaseSelfHeal() <= 0)
		{
			return 0;
		}
		int numHits = 0;
		if (GetSelfHealCountAllyHit())
		{
			numHits += allyHits;
		}
		if (GetSelfHealCountEnemyHit())
		{
			numHits += enemyHits;
		}
		return GetBaseSelfHeal() + numHits * GetSelfHealAmountPerHit();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreAoeBuffDebuff))
		{
			m_abilityMod = abilityMod as AbilityMod_ClaymoreAoeBuffDebuff;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);

		if (targetSquare == null)
		{
			return;
		}
		Vector3 freePos = currentTarget.FreePos;
		Vector3 damageOrigin = AreaEffectUtils.GetCenterOfShape(m_shape, freePos, targetSquare);
		List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
			GetShape(),
			freePos,
			targetSquare,
			GetPenetrateLos(),
			caster,
			GetAffectedTeams(caster),
			nonActorTargetInfo);
		actors.Remove(caster);
		bool isCasterInShape = AreaEffectUtils.IsSquareInShape(
			caster.GetCurrentBoardSquare(),
			GetShape(),
			freePos, 
			targetSquare,
			GetPenetrateLos(),
			caster);
		if (IncludeCaster() && isCasterInShape)
		{
			actors.Add(caster);
		}

		int enemyHits = 0;
		int allyHits = 0;
		foreach (ActorData target in actors)
		{
			if (target.GetTeam() != caster.GetTeam())
			{
				enemyHits++;
			}
			else if (target != caster)
			{
				allyHits++;
			}
		}
		foreach (ActorData target in actors)
		{
			if (target == caster)
			{
				int healing = CalcSelfHealAmountFromHits(allyHits, enemyHits);
				if (healing > 0 || GetSelfHitEffect() != null)
				{
					ActorHitParameters hitParams = new ActorHitParameters(target, damageOrigin);
					ActorHitResults hitResults = new ActorHitResults(healing, HitActionType.Healing, GetSelfHitEffect(), hitParams);
					abilityResults.StoreActorHit(hitResults);
				}
			}
			else if (target.GetTeam() != caster.GetTeam())
			{
				int enemyEnergyLoss = !GetEnergyChangeOnlyIfHasAdjacent()
				                      || AreaEffectUtils.HasAdjacentActorOfTeam(target, target.GetTeamAsList())
					? GetEnemyEnergyLoss()
					: 0;
				if (GetEnemyHitEffect() != null || enemyEnergyLoss > 0)
				{
					ActorHitParameters hitParams = new ActorHitParameters(target, damageOrigin);
					ActorHitResults hitResults = new ActorHitResults(enemyEnergyLoss, HitActionType.TechPointsLoss, GetEnemyHitEffect(), hitParams);
					abilityResults.StoreActorHit(hitResults);
				}
			}
			else
			{
				int allyEnergyGain = !GetEnergyChangeOnlyIfHasAdjacent()
				                     || AreaEffectUtils.HasAdjacentActorOfTeam(target, target.GetTeamAsList())
					? GetAllyEnergyGain()
					: 0;
				if (GetAllyHitEffect() != null || allyEnergyGain > 0)
				{
					ActorHitParameters hitParams = new ActorHitParameters(target, damageOrigin);
					ActorHitResults hitResults = new ActorHitResults(allyEnergyGain, HitActionType.TechPointsGain, GetAllyHitEffect(), hitParams);
					abilityResults.StoreActorHit(hitResults);
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// custom
	public List<Team> GetAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (caster != null)
		{
			if (IncludeAllies())
			{
				list.Add(caster.GetTeam());
			}
			if (IncludeEnemies())
			{
				list.AddRange(caster.GetOtherTeams());
			}
		}
		return list;
	}
	
	// custom
	public override void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(
		ActorData effectCaster,
		ActorData weakenedActor,
		int damageReduced)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ClaymoreStats.DamageMitigatedByShout, damageReduced);
	}
#endif
}
