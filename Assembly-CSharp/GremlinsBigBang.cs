// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class GremlinsBigBang : Ability
{
	public int m_bombDamageAmount = 5;
	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;
	public bool m_bombPenetrateLineOfSight;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	public float m_knockbackDistance = 2f;
	public AbilityAreaShape m_knockbackShape = AbilityAreaShape.Five_x_Five;
	[Header("-- Sequences -----------------------------------")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_GremlinsBigBang m_abilityMod;

	public AbilityMod_GremlinsBigBang GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Big Bang";
		}
		SetupTargeter();
	}

	protected void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost(
			this,
			ModdedBombShape(),
			m_bombPenetrateLineOfSight,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Possible,
			ModdedKnockbackShape(),
			GetKnockbackDistance(),
			m_knockbackType,
			false,
			true,
			ModdedKnockbackDistanceForSingleTarget());
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsBigBang))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_GremlinsBigBang;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private AbilityAreaShape ModdedBombShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bombShape.GetModifiedValue(m_bombShape)
			: m_bombShape;
	}

	private AbilityAreaShape ModdedKnockbackShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackShape.GetModifiedValue(m_knockbackShape)
			: m_knockbackShape;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	private int ModdedExtraDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerTarget.GetModifiedValue(0)
			: 0;
	}

	private int ModdedDamageForSingleTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIfSingleTarget.GetModifiedValue(m_bombDamageAmount)
			: m_bombDamageAmount;
	}

	private float ModdedKnockbackDistanceForSingleTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceIfSingleTarget.GetModifiedValue(GetKnockbackDistance())
			: GetKnockbackDistance();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_bombDamageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeter.GetActorsInRange();
		int numVisibleTargets = 0;
		foreach (AbilityUtil_Targeter.ActorTarget target in actorsInRange)
		{
			if (target.m_actor.IsActorVisibleToClient())
			{
				numVisibleTargets++;
			}
		}
		int extraDamage = (numVisibleTargets - 1) * ModdedExtraDamagePerTarget();
		int baseDamage = m_bombDamageAmount;
		if (numVisibleTargets == 1)
		{
			baseDamage = ModdedDamageForSingleTarget();
		}
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, baseDamage + extraDamage);
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", string.Empty, m_bombDamageAmount);
	}
	
#if SERVER
	// added in rogues
	private List<ActorData> GetKnockbackHitActors(List<AbilityTarget> targets, ActorData caster)
	{
		return AreaEffectUtils.GetActorsInShape(
			ModdedKnockbackShape(),
			targets[0],
			m_bombPenetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			null);
	}

	// added in rogues
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

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> bombHitActors = GetBombHitActors(targets, caster, nonActorTargetInfo);
		List<ActorData> knockbackHitActors = GetKnockbackHitActors(targets, caster);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(ModdedKnockbackShape(), targets[0]);
		int extraDamage = ModdedExtraDamagePerTarget() * (bombHitActors.Count - 1);
		int baseDamage = m_bombDamageAmount;
		float distance = GetKnockbackDistance();
		if (bombHitActors.Count == 1)
		{
			baseDamage = ModdedDamageForSingleTarget();
			distance = ModdedKnockbackDistanceForSingleTarget();
		}
		foreach (ActorData actorData in bombHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, targets[0].FreePos));
			actorHitResults.SetBaseDamage(baseDamage + extraDamage);
			if (knockbackHitActors.Contains(actorData))
			{
				KnockbackHitData knockbackData = new KnockbackHitData(
					actorData,
					caster,
					m_knockbackType,
					targets[0].AimDirection,
					centerOfShape,
					distance);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		foreach (ActorData actorData in knockbackHitActors)
		{
			if (!bombHitActors.Contains(actorData))
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, targets[0].FreePos));
				KnockbackHitData knockbackData = new KnockbackHitData(
					actorData,
					caster,
					m_knockbackType,
					targets[0].AimDirection,
					centerOfShape,
					GetKnockbackDistance());
				actorHitResults.AddKnockbackData(knockbackData);
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetBombHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInShape(
			ModdedBombShape(),
			targets[0],
			m_bombPenetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
	}
#endif
}
