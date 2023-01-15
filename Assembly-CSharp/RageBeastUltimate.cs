// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RageBeastUltimate : Ability
{
	[Space(10f)]
	public bool m_resetCooldowns = true;
	public AbilityAreaShape m_castExplosionShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public AbilityAreaShape m_walkingSpillShape;
	[Separator("Plasma")]
	public int m_plasmaDamage = 5;
	public int m_plasmaDuration = 2;
	public int m_spillingStateDuration = 2;
	public bool m_penetrateLineOfSight;
	
	// removed in rogues
	[Separator("Direct vs Indirect Damage")]
	public bool m_isDirectDamageOnCast;
	
	[Separator("Self Hit on Cast")]
	public int m_selfHealOnCast;
	public StandardEffectInfo m_extraEffectOnSelf;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_plasmaSequencePrefab;
	public GameObject m_hitByPlasmaSequencePrefab;
	public GameObject m_buffSequencePrefab;

	private AbilityMod_RageBeastUltimate m_abilityMod;
	private StandardEffectInfo m_cachedExtraEffectOnSelf;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_castExplosionShape,
			m_penetrateLineOfSight,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			HasSelfEffectFromBaseMod()
				? AbilityUtil_Targeter.AffectsActor.Always
				: AbilityUtil_Targeter.AffectsActor.Never);
		Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedExtraEffectOnSelf = m_abilityMod != null
			? m_abilityMod.m_extraEffectOnSelfMod.GetModifiedValue(m_extraEffectOnSelf)
			: m_extraEffectOnSelf;
	}

	public int GetSelfHealOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfHealOnCastMod.GetModifiedValue(m_selfHealOnCast)
			: m_selfHealOnCast;
	}

	public StandardEffectInfo GetExtraEffectOnSelf()
	{
		return m_cachedExtraEffectOnSelf ?? m_extraEffectOnSelf;
	}

	private int GetPlasmaDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_plasmaDamageMod.GetModifiedValue(m_plasmaDamage)
			: m_plasmaDamage;
	}

	private int GetPlasmaDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_plasmaDurationMod.GetModifiedValue(m_plasmaDuration)
			: m_plasmaDuration;
	}

	private int GetPlasmaHealing()
	{
		return m_abilityMod != null
			? m_abilityMod.m_plasmaHealingMod.GetModifiedValue(0)
			: 0;
	}

	private int GetPlasmaTechPointGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_plasmaTechPointGainMod.GetModifiedValue(0)
			: 0;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetPlasmaDamage());
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealOnCast());
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastUltimate abilityMod_RageBeastUltimate = modAsBase as AbilityMod_RageBeastUltimate;
		AddTokenInt(tokens, "PlasmaDamage", string.Empty, abilityMod_RageBeastUltimate != null
			? abilityMod_RageBeastUltimate.m_plasmaDamageMod.GetModifiedValue(m_plasmaDamage)
			: m_plasmaDamage);
		AddTokenInt(tokens, "PlasmaDuration", string.Empty, abilityMod_RageBeastUltimate != null
			? abilityMod_RageBeastUltimate.m_plasmaDurationMod.GetModifiedValue(m_plasmaDuration)
			: m_plasmaDuration);
		AddTokenInt(tokens, "SelfHealOnCast", string.Empty, m_selfHealOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_RageBeastUltimate != null
			? abilityMod_RageBeastUltimate.m_extraEffectOnSelfMod.GetModifiedValue(m_extraEffectOnSelf)
			: m_extraEffectOnSelf, "ExtraEffectOnSelf", m_extraEffectOnSelf);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastUltimate))
		{
			m_abilityMod = abilityMod as AbilityMod_RageBeastUltimate;
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
	public override int GetTechPointRegenContribution()
	{
		if (!(m_abilityMod == null) && m_abilityMod.m_passiveTechPointRegen > 0)
		{
			return m_abilityMod.m_passiveTechPointRegen;
		}
		return 0;
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_castExplosionShape,
			caster.GetFreePos(),
			caster.GetCurrentBoardSquare(),
			m_penetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			null);
		actorsInShape.Add(caster);
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare(),
			actorsInShape.ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_castExplosionShape,
			caster.GetFreePos(),
			caster.GetCurrentBoardSquare(),
			m_penetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		foreach (ActorData target in actorsInShape)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(GetPlasmaDamage(), HitActionType.Damage, hitParams);
			// TODO RAGEBEAST use m_isDirectDamageOnCast
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		Effect effect = new RageBeastPlasmaEffect(
			AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			caster,
			GetPlasmaDamage(),
			GetPlasmaDuration(),
			m_spillingStateDuration,
			m_castExplosionShape,
			m_walkingSpillShape,
			m_penetrateLineOfSight,
			m_resetCooldowns,
			actorsInShape,
			m_plasmaSequencePrefab,
			m_hitByPlasmaSequencePrefab,
			m_buffSequencePrefab,
			GetPlasmaHealing(),
			GetPlasmaTechPointGain());
		ActorHitParameters casterHitParams = new ActorHitParameters(caster, caster.GetFreePos());
		ActorHitResults casterHitResults = new ActorHitResults(effect, casterHitParams);
		casterHitResults.SetBaseHealing(GetSelfHealOnCast());
		casterHitResults.AddStandardEffectInfo(GetExtraEffectOnSelf());
		if (m_resetCooldowns)
		{
			casterHitResults.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.ClearCharacterAbilityCooldowns));
		}
		abilityResults.StoreActorHit(casterHitResults);
	}
#endif
}
