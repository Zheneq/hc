// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlRocketJump : Ability
{
	public int m_damageAmount = 20;
	// added in rogues
	// public float m_knockbackDistance;
	// added in rogues
	// public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	public bool m_penetrateLineOfSight;
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;
	// added in rogues
	// public GameObject m_knockbackGameplayHitSequence;

	private AbilityMod_BazookaGirlRocketJump m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		bool affectsAllies = moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect;
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		bool affectsCaster = moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect;
		// reactor
		Targeter = new AbilityUtil_Targeter_RocketJump(this, m_shape, m_penetrateLineOfSight, 0f, affectsAllies);
		// rogues
		// Targeter = new AbilityUtil_Targeter_RocketJump(this, m_shape, m_penetrateLineOfSight, m_knockbackDistance, affectsAllies);
		Targeter.SetAffectedGroups(true, affectsAllies, affectsCaster);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null
		    || !tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			return null;
		}
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			[AbilityTooltipSymbol.Damage] = GetDamageAmount()
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlRocketJump mod = modAsBase as AbilityMod_BazookaGirlRocketJump;
		int damage = mod != null
			? mod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
		AddTokenInt(tokens, "DamageAmount", string.Empty, damage);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlRocketJump))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BazookaGirlRocketJump;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public bool ResetCooldownOnKill()
	{
		return m_abilityMod != null && m_abilityMod.m_resetCooldownOnKill;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 freePos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
		ExplosionSequence.ExtraParams extraParams = new ExplosionSequence.ExtraParams
		{
			radius = 3f,
			team = caster.GetTeam()
		};
		return new ServerClientUtils.SequenceStartData(m_sequencePrefab, freePos, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, extraParams.ToArray());
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in FindHitActors(caster, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetSquareAtPhaseStart().ToVector3()));
			if (actorData.GetTeam() != caster.GetTeam())
			{
				actorHitResults.SetBaseDamage(GetDamageAmount());
				// rogues
				// if (m_knockbackDistance > 0f)
				// {
				// 	DelayedAoeKnockbackEffect effect = new DelayedAoeKnockbackEffect(AsEffectSource(), caster.GetSquareAtPhaseStart(), actorData, caster, DelayedAoeKnockbackEffect.KnockbackCenterType.FromTargetSquare, 0, 0, null, m_shape, m_penetrateLineOfSight, m_knockbackType, m_knockbackDistance / Board.SquareSizeStatic, false, null, m_knockbackGameplayHitSequence);
				// 	actorHitResults.AddEffect(effect);
				// }
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		StandardEffectInfo moddedEffectForAllies = GetModdedEffectForAllies();
		bool includeAllies = moddedEffectForAllies != null && moddedEffectForAllies.m_applyEffect;
		Vector3 freePos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, true);
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_shape, freePos, caster.GetSquareAtPhaseStart(), m_penetrateLineOfSight, caster, relevantTeams, nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInShape);
		return actorsInShape;
	}
#endif
}
