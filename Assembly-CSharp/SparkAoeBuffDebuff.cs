// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SparkAoeBuffDebuff : Ability
{
	public enum TargetingType
	{
		UseShape,
		UseRadius
	}

	[Header("-- Targeting")]
	public TargetingType m_TargetingType;
	public bool m_penetrateLos;
	[Header("-- Shape")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;
	[Header("-- Radius")]
	public float m_radius = 6f;
	[Header("-- Damage and Healing")]
	public int m_damageAmount;
	public int m_allyHealAmount = 10;
	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;
	public int m_selfHealAmountPerHit;
	public bool m_selfHealCountEnemyHit = true;
	public bool m_selfHealCountAllyHit = true;
	[Header("-- Normal Hit Effects")]
	public StandardEffectInfo m_selfHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_sequenceOnEnemies;
	public GameObject m_sequenceOnAllies;

	private AbilityMod_SparkAoeBuffDebuff m_abilityMod;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spark Aoe Buff Debuff";
		}
		SetupTargeter();
	}

	public float GetTargetingRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_radiusMod.GetModifiedValue(m_radius)
			: m_radius;
	}

	public AbilityAreaShape GetHitShape()
	{
		return m_shape;
	}

	public bool ShouldIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetAllyHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return mod != null
			? mod.m_allyHealMod.GetModifiedValue(m_allyHealAmount)
			: m_allyHealAmount;
	}

	public int GetBaseSelfHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return mod != null
			? mod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal)
			: m_baseSelfHeal;
	}

	public int GetSelfHealPerHit(AbilityMod_SparkAoeBuffDebuff mod)
	{
		return mod != null
			? mod.m_selfHealPerHitMod.GetModifiedValue(m_selfHealAmountPerHit)
			: m_selfHealAmountPerHit;
	}

	public bool SelfHealCountAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealHitCountAlly.GetModifiedValue(m_selfHealCountAllyHit)
			: m_selfHealCountAllyHit;
	}

	public bool SelfHealCountEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealHitCountEnemy.GetModifiedValue(m_selfHealCountEnemyHit)
			: m_selfHealCountEnemyHit;
	}

	public int GetShieldOnSelfPerAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0)
			: 0;
	}

	public int GetShieldOnSelfDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldOnSelfDuration
			: 1;
	}

	public bool IncludeCaster()
	{
		return GetSelfHitEffect().m_applyEffect
			|| GetSelfHealPerHit(m_abilityMod) > 0
			|| GetBaseSelfHeal(m_abilityMod) > 0
			|| GetShieldOnSelfPerAllyHit() > 0;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect
			|| GetAllyHeal(m_abilityMod) > 0;
	}

	public bool IncludeEnemies()
	{
		return GetEnemyHitEffect().m_applyEffect
			|| m_damageAmount > 0;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		// rogues
		//return m_cachedSelfHitEffect;
		// reactor
		return m_cachedSelfHitEffect ?? m_selfHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		// rogues
		//return m_cachedAllyHitEffect;
		// reactor
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		// rogues
		//return m_cachedEnemyHitEffect;
		// reactor
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
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

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_TargetingType == TargetingType.UseShape)
		{
			AbilityUtil_Targeter.AffectsActor affectsCaster = IncludeCaster()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never;
			Targeter = new AbilityUtil_Targeter_Shape(this, GetHitShape(), ShouldIgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetTargetingRadius(), ShouldIgnoreLos(), IncludeEnemies(), IncludeAllies());
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkAoeBuffDebuff mod = modAsBase as AbilityMod_SparkAoeBuffDebuff;
		AddTokenInt(tokens, "Heal_OnAlly", "heal on ally", GetAllyHeal(mod));
		AddTokenInt(tokens, "Heal_OnSelfBase", "heal on self, base amount", GetBaseSelfHeal(mod));
		AddTokenInt(tokens, "Heal_OnSelfPerHit", "heal on self, per hit", GetSelfHealPerHit(mod));
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect) : m_allyHitEffect, "EffectOnAlly", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect) : m_selfHitEffect, "EffectOnSelf", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect) : m_enemyHitEffect, "EffectOnEnemy", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		// reactor
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		// rogues
		//m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		//m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);

		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal(m_abilityMod) + GetSelfHealPerHit(m_abilityMod));
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, GetShieldOnSelfPerAllyHit());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHeal(m_abilityMod));
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealPerHit(m_abilityMod) <= 0 && GetBaseSelfHeal(m_abilityMod) <= 0 && GetShieldOnSelfPerAllyHit() <= 0)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> primaryTargets = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			int enemyHits = 0;
			int allyHits = 0;
			for (int i = 0; i < primaryTargets.Count; i++)
			{
				if (primaryTargets[i].GetTeam() != targetActor.GetTeam())
				{
					enemyHits++;
				}
				else if (primaryTargets[i] != targetActor)
				{
					allyHits++;
				}
			}
			result[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(allyHits, enemyHits);
			if (GetShieldOnSelfPerAllyHit() > 0)
			{
				// reactor
				StandardEffectInfo selfHitEffect = GetSelfHitEffect();
				int absorb = selfHitEffect.m_applyEffect && selfHitEffect.m_effectData.m_absorbAmount > 0
					? selfHitEffect.m_effectData.m_absorbAmount
					: 0;
				// rogues
				//int absorb = 0;
				result[AbilityTooltipSymbol.Absorb] = absorb + allyHits * GetShieldOnSelfPerAllyHit();
			}
		}
		return result;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		if (GetSelfHealPerHit(m_abilityMod) > 0
			|| GetBaseSelfHeal(m_abilityMod) > 0)
		{
			int hits = 0;
			if (SelfHealCountAllyHit())
			{
				hits += allyHits;
			}
			if (SelfHealCountEnemyHit())
			{
				hits += enemyHits;
			}
			return GetBaseSelfHeal(m_abilityMod) + hits * GetSelfHealPerHit(m_abilityMod);
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkAoeBuffDebuff))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkAoeBuffDebuff);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	// server-only
#if SERVER
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		List<ActorData> list3 = new List<ActorData>();
		foreach (ActorData actorData in additionalData.m_abilityResults.HitActorList())
		{
			if (m_sequenceOnAllies != null && actorData.GetTeam() == caster.GetTeam())
			{
				list2.Add(actorData);
			}
			else if (m_sequenceOnEnemies != null && actorData.GetTeam() != caster.GetTeam())
			{
				list3.Add(actorData);
			}
			else
			{
				list.Add(actorData);
			}
		}
		List<ServerClientUtils.SequenceStartData> list4 = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_castSequencePrefab, targets[0].FreePos, list.ToArray(), caster, additionalData.m_sequenceSource, null);
		list4.Add(item);
		if (list2.Count > 0)
		{
			ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(m_sequenceOnAllies, caster.GetFreePos(), list2.ToArray(), caster, additionalData.m_sequenceSource, null);
			list4.Add(item2);
		}
		if (list3.Count > 0)
		{
			ServerClientUtils.SequenceStartData item3 = new ServerClientUtils.SequenceStartData(m_sequenceOnEnemies, caster.GetFreePos(), list3.ToArray(), caster, additionalData.m_sequenceSource, null);
			list4.Add(item3);
		}
		return list4;
	}
#endif

	// server-only
#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, nonActorTargetInfo);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < hitActors.Count; i++)
		{
			if (hitActors[i].GetTeam() != caster.GetTeam())
			{
				num++;
			}
			else if (hitActors[i] != caster)
			{
				num2++;
			}
		}
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			if (actorData == caster)
			{
				actorHitResults.AddStandardEffectInfo(GetSelfHitEffect());
				if (GetSelfHealPerHit(m_abilityMod) > 0 || GetBaseSelfHeal(m_abilityMod) > 0)
				{
					int baseHealing = CalcSelfHealAmountFromHits(num2, num);
					actorHitResults.SetBaseHealing(baseHealing);
				}
				if (GetShieldOnSelfPerAllyHit() > 0 && num2 > 0)
				{
					int absorbAmount = GetShieldOnSelfPerAllyHit() * num2;
					StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
					standardActorEffectData.InitWithDefaultValues();
					standardActorEffectData.m_absorbAmount = absorbAmount;
					standardActorEffectData.m_duration = GetShieldOnSelfDuration();
					StandardActorEffect effect = new StandardActorEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), caster, caster, standardActorEffectData);
					actorHitResults.AddEffect(effect);
				}
			}
			else if (actorData.GetTeam() == caster.GetTeam())
			{
				if (GetAllyHeal(m_abilityMod) > 0)
				{
					actorHitResults.AddBaseHealing(GetAllyHeal(m_abilityMod));
				}
				actorHitResults.AddStandardEffectInfo(GetAllyHitEffect());
			}
			else
			{
				if (m_damageAmount > 0)
				{
					actorHitResults.AddBaseDamage(m_damageAmount);
				}
				actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif

	// server-only
#if SERVER
	public new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (m_TargetingType == TargetingType.UseShape)
		{
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(GetHitShape(), targets[0], ShouldIgnoreLos(), caster, relevantTeams, nonActorTargetInfo);
			if (!IncludeCaster())
			{
				actorsInShape.Remove(caster);
			}
			return actorsInShape;
		}
		List<Team> relevantTeams2 = TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies());
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.GetLoSCheckPos(), GetTargetingRadius(), ShouldIgnoreLos(), caster, relevantTeams2, nonActorTargetInfo);
		if (!IncludeCaster())
		{
			actorsInRadius.Remove(caster);
		}
		return actorsInRadius;
	}
#endif

	// server-only
#if SERVER
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SparkStats.HealingFromUlt, results.FinalHealing);
		}
	}
#endif
}
