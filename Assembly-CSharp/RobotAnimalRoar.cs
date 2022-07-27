// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalRoar : Ability
{
	public enum TargetingMode
	{
		Shape,
		Radius
	}

	public TargetingMode m_targetingMode;
	public bool m_penetrateLineOfSight;
	[Header("-- Targeting: Shape")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Seven_x_Seven;
	[Header("-- Inner shape for different damage --")]
	public bool m_useInnerShape;
	public AbilityAreaShape m_innerShape = AbilityAreaShape.Three_x_Three_NoCorners;
	[Header("-- Targeting: Radius")]
	public float m_targetingRadius = 4.49f;
	public float m_innerRadius = -1f;
	public StandardEffectInfo m_allyEffect_includingMe;
	public StandardEffectInfo m_allyEffect_excludingMe;
	public StandardEffectInfo m_enemyEffect;
	public StandardEffectInfo m_selfEffect;
	[Header(" Damage, Inner Shape Damage also used for Radius targeting")]
	public int m_damage;
	public int m_innerShapeDamage = -1;

	private AbilityMod_RobotAnimalRoar m_abilityMod;
#if SERVER
	// added in rogues
	private Passive_RobotAnimal m_passive;
#endif

	private void Start()
	{
#if SERVER
		// added in rogues
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = component.GetPassiveOfType(typeof(Passive_RobotAnimal)) as Passive_RobotAnimal;
		}
#endif
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		bool affectsEnemies = AffectEnemies();
		bool affectsAllies = AffectAllies();
		if (m_targetingMode != TargetingMode.Shape)
		{
			Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetTargetingRadius(), GetPenetrateLos(), affectsEnemies, affectsAllies);
			if (m_selfEffect.m_applyEffect || HasSelfEffectFromBaseMod())
			{
				Targeter.SetAffectedGroups(affectsEnemies, affectsAllies, true);
			}
		}
		else
		{
			AbilityUtil_Targeter.AffectsActor affectsCaster = m_selfEffect.m_applyEffect || HasSelfEffectFromBaseMod()
				? AbilityUtil_Targeter.AffectsActor.Always
				: m_allyEffect_includingMe.m_applyEffect
					? AbilityUtil_Targeter.AffectsActor.Possible
					: AbilityUtil_Targeter.AffectsActor.Never;
			if (UseInnerShape())
			{
				Targeter = new AbilityUtil_Targeter_MultipleShapes(
					this,
					new List<AbilityAreaShape>
					{
						GetInnerShape(),
						GetTargetingShape()
					},
					new List<AbilityTooltipSubject>
					{
						AbilityTooltipSubject.Secondary,
						AbilityTooltipSubject.Primary
					},
					GetPenetrateLos(),
					affectsEnemies,
					affectsAllies,
					m_selfEffect.m_applyEffect);
			}
			else
			{
				AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(
					this,
					GetTargetingShape(),
					GetPenetrateLos(),
					AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
					affectsEnemies,
					affectsAllies,
					affectsCaster);
				targeter.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Ally);
				Targeter = targeter;
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetDamageAmount() != 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		}
		if (UseInnerShape())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetInnerShapeDamage());
		}
		m_enemyEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyEffect_includingMe.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_allyEffect_excludingMe.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		int moddedHealingForAllies = GetModdedHealingForAllies();
		if (moddedHealingForAllies != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, moddedHealingForAllies));
		}
		int moddedTechPointGainForAllies = GetModdedTechPointGainForAllies();
		if (moddedTechPointGainForAllies != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, moddedTechPointGainForAllies));
		}
		m_selfEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (m_targetingMode == TargetingMode.Shape)
		{
			if (UseInnerShape())
			{
				List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					bool isPrimary = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						dictionary[AbilityTooltipSymbol.Damage] = isPrimary ? GetDamageAmount() : GetInnerShapeDamage();
					}
					else
					{
						dictionary[AbilityTooltipSymbol.Damage] = 0;
					}
				}
			}
		}
		else if (GetInnerRadius() > 0f)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes2 = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
			if (ActorData != null
			    && targetActor.GetCurrentBoardSquare() != null
			    && tooltipSubjectTypes2 != null
			    && tooltipSubjectTypes2.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				bool isInner = AreaEffectUtils.IsSquareInConeByActorRadius(
					targetActor.GetCurrentBoardSquare(),
					ActorData.GetFreePos(),
					0f,
					360f,
					GetInnerRadius(),
					0f,
					GetPenetrateLos(),
					ActorData);
				dictionary[AbilityTooltipSymbol.Damage] = isInner ? GetInnerShapeDamage() : GetDamageAmount();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RobotAnimalRoar abilityMod_RobotAnimalRoar = modAsBase as AbilityMod_RobotAnimalRoar;
		AbilityMod.AddToken_EffectInfo(tokens, m_allyEffect_includingMe, "AllyEffect_includingMe", m_allyEffect_includingMe);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyEffect_excludingMe, "AllyEffect_excludingMe", m_allyEffect_excludingMe);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyEffect, "EnemyEffect", m_enemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfEffect, "SelfEffect", m_selfEffect);
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_RobotAnimalRoar != null
			? abilityMod_RobotAnimalRoar.m_damageMod.GetModifiedValue(m_damage)
			: m_damage);
		int innerShapeDamage = m_innerShapeDamage >= 0 ? m_innerShapeDamage : m_damage;
		AddTokenInt(tokens, "InnerShapeDamage", string.Empty, abilityMod_RobotAnimalRoar != null
			? abilityMod_RobotAnimalRoar.m_innerShapeDamageMod.GetModifiedValue(innerShapeDamage)
			: m_innerShapeDamage);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalRoar))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalRoar;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public AbilityAreaShape GetTargetingShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_aoeShape)
			: m_aoeShape;
	}

	public StandardEffectInfo GetEnemyHitEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_enemyHitEffectOverride.m_applyEffect
			? m_abilityMod.m_enemyHitEffectOverride
			: m_enemyEffect;
	}

	public bool GetPenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public int GetTechPointDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointDamageMod.GetModifiedValue(0)
			: 0;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetModdedHealingForAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmountToTargetAllyOnHit
			: 0;
	}

	public int GetModdedTechPointGainForAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainToTargetAllyOnHit
			: 0;
	}

	public bool UseInnerShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useInnerShapeMod.GetModifiedValue(m_useInnerShape)
			: m_useInnerShape;
	}

	public AbilityAreaShape GetInnerShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerShapeMod.GetModifiedValue(m_innerShape)
			: m_innerShape;
	}

	public int GetInnerShapeDamage()
	{
		int damage = m_innerShapeDamage < 0
			? m_damage
			: m_innerShapeDamage;
		return m_abilityMod != null
			? m_abilityMod.m_innerShapeDamageMod.GetModifiedValue(damage)
			: damage;
	}

	public float GetTargetingRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetingRadiusMod.GetModifiedValue(m_targetingRadius)
			: m_targetingRadius;
	}

	public float GetInnerRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_innerRadiusMod.GetModifiedValue(m_innerRadius)
			: m_innerRadius;
	}

	public bool AffectAllies()
	{
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		return m_allyEffect_excludingMe.m_applyEffect
		       || m_allyEffect_includingMe.m_applyEffect
		       || (moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect)
		       || GetModdedHealingForAllies() != 0
		       || GetModdedTechPointGainForAllies() != 0;
	}

	public bool AffectEnemies()
	{
		return GetDamageAmount() > 0
		       || GetTechPointDamage() > 0
		       || GetEnemyHitEffectInfo().m_applyEffect;
	}
	
#if SERVER
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
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> innerShapeActors;
		List<ActorData> outerShapeActors;
		if (m_targetingMode == TargetingMode.Shape)
		{
			outerShapeActors = GetHitTargets_Shape(targets, caster, out innerShapeActors, nonActorTargetInfo);
		}
		else
		{
			outerShapeActors = GetHitTargets_Radius(targets, caster, out innerShapeActors, nonActorTargetInfo);
		}
		foreach (ActorData actorData in outerShapeActors)
		{
			bool isSelf = actorData == caster;
			bool isEnemy = actorData.GetTeam() != caster.GetTeam();
			bool isAlly = actorData.GetTeam() == caster.GetTeam() && !isSelf;
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (isSelf)
			{
				actorHitResults.AddStandardEffectInfo(m_selfEffect);
			}
			if (isAlly)
			{
				actorHitResults.AddStandardEffectInfo(m_allyEffect_excludingMe);
				int moddedHealingForAllies = GetModdedHealingForAllies();
				if (moddedHealingForAllies != 0)
				{
					actorHitResults.AddBaseHealing(moddedHealingForAllies);
				}
				int moddedTechPointGainForAllies = GetModdedTechPointGainForAllies();
				if (moddedTechPointGainForAllies != 0)
				{
					actorHitResults.AddTechPointGain(moddedTechPointGainForAllies);
				}
			}
			if (isAlly || isSelf)
			{
				actorHitResults.AddStandardEffectInfo(m_allyEffect_includingMe);
			}
			if (isEnemy)
			{
				actorHitResults.AddStandardEffectInfo(GetEnemyHitEffectInfo());
				actorHitResults.SetTechPointLoss(GetTechPointDamage());
			}
			int num = innerShapeActors.Contains(actorData) ? GetInnerShapeDamage() : GetDamageAmount();
			if (num > 0 && isEnemy)
			{
				actorHitResults.SetBaseDamage(num);
				if (m_passive != null && m_passive.m_shouldApplyAdditionalEffectFromStealth && m_passive.HasEffectOnNextDamageAttack())
				{
					actorHitResults.AddStandardEffectInfo(m_passive.GetEffectOnNextDamageAttack());
				}
				if (m_passive != null && m_passive.ShouldApplyExtraDamageNextAttack())
				{
					actorHitResults.AddBaseDamage(m_passive.GetExtraDamageNextAttack());
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (ServerAbilityUtils.CurrentlyGatheringRealResults() && m_passive != null)
		{
			m_passive.m_shouldApplyAdditionalEffectFromStealth = false;
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitTargets_Shape(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<ActorData> innerShapeActors,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		innerShapeActors = new List<ActorData>();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, AffectAllies(), AffectEnemies());
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			GetTargetingShape(),
			targets[0],
			GetPenetrateLos(),
			caster,
			relevantTeams,
			nonActorTargetInfo);
		if (m_selfEffect.m_applyEffect)
		{
			if (!actorsInShape.Contains(caster))
			{
				actorsInShape.Add(caster);
			}
		}
		else if (actorsInShape.Contains(caster))
		{
			actorsInShape.Remove(caster);
		}
		if (UseInnerShape())
		{
			foreach (ActorData actorData in actorsInShape)
			{
				if (AreaEffectUtils.IsSquareInShape(
					    actorData.GetCurrentBoardSquare(),
					    GetInnerShape(),
					    targets[0].FreePos,
					    square,
					    true,
					    caster))
				{
					innerShapeActors.Add(actorData);
				}
			}
		}
		return actorsInShape;
	}

	// added in rogues
	private List<ActorData> GetHitTargets_Radius(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<ActorData> innerShapeActors,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		innerShapeActors = new List<ActorData>();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, AffectAllies(), AffectEnemies());
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			caster.GetLoSCheckPos(),
			GetTargetingRadius(),
			GetPenetrateLos(),
			caster,
			relevantTeams,
			nonActorTargetInfo);
		if (m_selfEffect.m_applyEffect)
		{
			if (!actorsInRadius.Contains(caster))
			{
				actorsInRadius.Add(caster);
			}
		}
		else if (actorsInRadius.Contains(caster))
		{
			actorsInRadius.Remove(caster);
		}
		if (GetInnerRadius() > 0f)
		{
			foreach (ActorData actorData in actorsInRadius)
			{
				if (AreaEffectUtils.IsSquareInConeByActorRadius(
					    actorData.GetCurrentBoardSquare(),
					    caster.GetLoSCheckPos(),
					    0f,
					    360f,
					    GetInnerRadius(),
					    0f,
					    GetPenetrateLos(),
					    caster))
				{
					innerShapeActors.Add(actorData);
				}
			}
		}
		return actorsInRadius;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RobotAnimalStats.UltHits);
		}
	}
#endif
}
