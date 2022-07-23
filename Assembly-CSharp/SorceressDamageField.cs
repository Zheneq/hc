// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SorceressDamageField : Ability
{
	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;
	public bool m_penetrateLineOfSight;
	public int m_duration;
	public int m_damage;
	public int m_healing;
	public StandardEffectInfo m_effectOnEnemies;
	public StandardEffectInfo m_effectOnAllies;
	[Header("-- Sequences")]
	public GameObject m_hittingEnemyPrefab;
	public GameObject m_hittingAllyPrefab;
	public GameObject m_persistentGroundPrefab;
	public GameObject m_onHitPulsePrefab;
	private AbilityMod_SorceressDamageField m_abilityMod;
	private StandardEffectInfo m_cachedEffectOnEnemies;
	private StandardEffectInfo m_cachedEffectOnAllies;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		bool affectsEnemies = GetDamage() > 0 || GetEnemyHitEffect().m_applyEffect;
		bool affectsAllies = GetHealing() > 0 || GetAllyHitEffect().m_applyEffect;
		AbilityUtil_Targeter.AffectsActor affectsCaster = affectsAllies
			? AbilityUtil_Targeter.AffectsActor.Possible
			: AbilityUtil_Targeter.AffectsActor.Never;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetEffectShape(),
			m_penetrateLineOfSight,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			affectsEnemies,
			affectsAllies,
			affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damage));
		m_effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_healing));
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healing));
		m_effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		m_effectOnEnemies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealing());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealing());
		m_effectOnAllies.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDamageField abilityMod_SorceressDamageField = modAsBase as AbilityMod_SorceressDamageField;
		AddTokenInt(tokens, "Duration", string.Empty, abilityMod_SorceressDamageField != null
			? abilityMod_SorceressDamageField.m_durationMod.GetModifiedValue(m_duration)
			: m_duration);
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_SorceressDamageField != null
			? abilityMod_SorceressDamageField.m_damageMod.GetModifiedValue(m_damage)
			: m_damage);
		AddTokenInt(tokens, "Healing", string.Empty, abilityMod_SorceressDamageField != null
			? abilityMod_SorceressDamageField.m_healingMod.GetModifiedValue(m_healing)
			: m_healing);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDamageField != null
			? abilityMod_SorceressDamageField.m_onEnemyEffectOverride.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDamageField != null
			? abilityMod_SorceressDamageField.m_onAllyEffectOverride.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies, "EffectOnAllies", m_effectOnAllies);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressDamageField))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressDamageField;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape GetEffectShape()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_shapeOverride.GetModifiedValue(m_shape)
			: m_shape;
	}

	private GameObject GetPersistentSequencePrefab()
	{
		return m_abilityMod != null && m_abilityMod.m_persistentSequencePrefabOverride != null
			? m_abilityMod.m_persistentSequencePrefabOverride
			: m_persistentGroundPrefab;
	}

	private int GetDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_durationMod.GetModifiedValue(m_duration)
			: m_duration;
	}

	private int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	private int GetHealing()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingMod.GetModifiedValue(m_healing)
			: m_healing;
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_onEnemyEffectOverride.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies;
		m_cachedEffectOnAllies = m_abilityMod != null
			? m_abilityMod.m_onAllyEffectOverride.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedEffectOnAllies ?? m_effectOnAllies;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetEffectShape(), targets[0]);
		return new ServerClientUtils.SequenceStartData(AsEffectSource().GetSequencePrefab(), centerOfShape, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetEffectShape(), targets[0]);
		List<Team> list = new List<Team>
		{
			Team.TeamA,
			Team.TeamB
		};
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			GetEffectShape(),
			targets[0],
			m_penetrateLineOfSight,
			caster,
			list,
			nonActorTargetInfo);
		foreach (ActorData actorData in actorsInShape)
		{
			if (actorData.GetTeam() != caster.GetTeam())
			{
				if (GetDamage() > 0 || GetEnemyHitEffect().m_applyEffect)
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
					actorHitResults.SetBaseDamage(GetDamage());
					actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
					abilityResults.StoreActorHit(actorHitResults);
				}
			}
			else
			{
				if (GetHealing() > 0 || GetAllyHitEffect().m_applyEffect)
				{
					ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
					actorHitResults2.SetBaseHealing(GetHealing());
					actorHitResults2.AddStandardEffectInfo(GetAllyHitEffect());
					abilityResults.StoreActorHit(actorHitResults2);
				}
			}
		}
		Effect effect = new SorceressDamageFieldEffect(
			AsEffectSource(),
			caster,
			targets[0].GridPos,
			targets[0].FreePos,
			GetDuration(),
			m_penetrateLineOfSight,
			GetEffectShape(),
			GetDamage(),
			GetHealing(),
			GetEnemyHitEffect(),
			GetAllyHitEffect(),
			actorsInShape,
			m_hittingEnemyPrefab,
			m_hittingAllyPrefab,
			GetPersistentSequencePrefab(),
			m_onHitPulsePrefab,
			abilityResults.SequenceSource);
		PositionHitParameters hitParams = new PositionHitParameters(centerOfShape);
		PositionHitResults hitResults = new PositionHitResults(effect, hitParams);
		abilityResults.StorePositionHit(hitResults);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
