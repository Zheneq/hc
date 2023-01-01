// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherShieldGeneratorArrow : Ability
{
	[Header("-- Ground effect")]
	public bool m_penetrateLoS;
	public bool m_affectsEnemies;
	public bool m_affectsAllies;
	public bool m_affectsCaster;
	public int m_lessAbsorbPerTurn = 5;
	public StandardGroundEffectInfo m_groundEffectInfo;
	public StandardEffectInfo m_directHitEnemyEffect;
	public StandardEffectInfo m_directHitAllyEffect;
	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public StandardEffectInfo m_extraAllyHitEffect;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_ArcherShieldGeneratorArrow m_abilityMod;
	private Archer_SyncComponent m_syncComp;
	private StandardGroundEffectInfo m_cachedGroundEffect;
	private StandardEffectInfo m_cachedDirectHitEnemyEffect;
	private StandardEffectInfo m_cachedDirectHitAllyEffect;
	private StandardEffectInfo m_cachedExtraAllyHitEffect;
	
#if SERVER
	// added in rogues
	private Passive_Archer m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Shield Generator Arrow";
		}
		m_syncComp = GetComponent<Archer_SyncComponent>();
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType(typeof(Passive_Archer)) as Passive_Archer;
#endif
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetGroundEffectInfo().m_groundEffectData.shape,
			PenetrateLoS(),
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			AffectsEnemies(),
			AffectsAllies(),
			AffectsCaster()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherShieldGeneratorArrow))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherShieldGeneratorArrow;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedGroundEffect = m_groundEffectInfo;
		m_cachedDirectHitEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_directHitEnemyEffectMod.GetModifiedValue(m_directHitEnemyEffect)
			: m_directHitEnemyEffect;
		m_cachedDirectHitAllyEffect = m_abilityMod != null
			? m_abilityMod.m_directHitAllyEffectMod.GetModifiedValue(m_directHitAllyEffect)
			: m_directHitAllyEffect;
		m_cachedExtraAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_extraAllyHitEffectMod.GetModifiedValue(m_extraAllyHitEffect)
			: m_extraAllyHitEffect;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public bool AffectsEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(m_affectsEnemies)
			: m_affectsEnemies;
	}

	public bool AffectsAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsAlliesMod.GetModifiedValue(m_affectsAllies)
			: m_affectsAllies;
	}

	public bool AffectsCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsCasterMod.GetModifiedValue(m_affectsCaster)
			: m_affectsCaster;
	}

	private StandardGroundEffectInfo GetGroundEffectInfo()
	{
		return m_cachedGroundEffect ?? m_groundEffectInfo;
	}

	public int GetLessAbsorbPerTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessAbsorbPerTurnMod.GetModifiedValue(m_lessAbsorbPerTurn)
			: m_lessAbsorbPerTurn;
	}

	public StandardEffectInfo GetDirectHitEnemyEffect()
	{
		return m_cachedDirectHitEnemyEffect ?? m_directHitEnemyEffect;
	}

	public StandardEffectInfo GetDirectHitAllyEffect()
	{
		return m_cachedDirectHitAllyEffect ?? m_directHitAllyEffect;
	}

	public StandardEffectInfo GetExtraAllyHitEffect()
	{
		return m_cachedExtraAllyHitEffect ?? m_extraAllyHitEffect;
	}

	public int GetCooldownReductionOnDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionOnDash.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbPerEnemyHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbIfEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbIfEnemyHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbIfOnlyOneAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbIfOnlyOneAllyHit.GetModifiedValue(0)
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LessAbsorbPerTurn", string.Empty, m_lessAbsorbPerTurn);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitEnemyEffect, "DirectHitEnemyEffect", m_directHitEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitAllyEffect, "DirectHitAllyEffect", m_directHitAllyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraAllyHitEffect, "ExtraAllyHitEffect", m_extraAllyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_groundEffectInfo.m_applyGroundEffect)
		{
			m_groundEffectInfo.m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		}
		if (AffectsAllies())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		if (AffectsCaster())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		if (AffectsEnemies())
		{
			GetDirectHitEnemyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_syncComp != null && targetActor.GetTeam() == ActorData.GetTeam())
		{
			int extraAbsorb = m_syncComp.m_extraAbsorbForShieldGenerator;
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeters[currentTargeterIndex].GetActorsInRange();
			if (!actorsInRange.IsNullOrEmpty())
			{
				int enemiesInRange = actorsInRange.Count(t => t.m_actor.GetTeam() != ActorData.GetTeam());
				if (actorsInRange.Count - enemiesInRange == 1)
				{
					extraAbsorb += GetExtraAbsorbIfOnlyOneAllyHit();
				}
				extraAbsorb += GetExtraAbsorbPerEnemyHit() * enemiesInRange;
				if (enemiesInRange > 0)
				{
					extraAbsorb += GetExtraAbsorbIfEnemyHit();
				}
			}
			int absorb = GetDirectHitAllyEffect().m_effectData.m_absorbAmount + extraAbsorb;
			StandardEffectInfo extraAllyHitEffect = GetExtraAllyHitEffect();
			if (extraAllyHitEffect.m_applyEffect && extraAllyHitEffect.m_effectData.m_absorbAmount > 0)
			{
				absorb += extraAllyHitEffect.m_effectData.m_absorbAmount;
			}
			dictionary[AbilityTooltipSymbol.Absorb] = absorb;
		}
		return dictionary;
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_extraAbsorbForShieldGenerator = 0;
			foreach (ActorData actor in additionalData.m_abilityResults.HitActorList())
			{
				m_syncComp.AddShieldGeneratorTarget(actor);
			}
		}
		if (m_passive != null)
		{
			m_passive.m_turnShieldGenEffectExpires = GameFlowData.Get().CurrentTurn + GetDirectHitAllyEffect().m_effectData.m_duration;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetGroundEffectInfo().m_groundEffectData.shape, targets[0]);
		centerOfShape.y = Board.Get().BaselineHeight;
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				centerOfShape,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> list = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, list);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetGroundEffectInfo().m_groundEffectData.shape, targets[0]);
		if (GetGroundEffectInfo().m_applyGroundEffect)
		{
			PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(centerOfShape));
			ArcherAbsorbGroundEffect archerAbsorbGroundEffect = new ArcherAbsorbGroundEffect(
				AsEffectSource(),
				square,
				centerOfShape,
				null,
				caster,
				GetGroundEffectInfo().m_groundEffectData,
				m_lessAbsorbPerTurn);
			archerAbsorbGroundEffect.AddToActorsHitThisTurn(hitActors);
			archerAbsorbGroundEffect.OverrideHitPhaseBeforeStart(m_runPriority);
			positionHitResults.AddEffect(archerAbsorbGroundEffect);
			abilityResults.StorePositionHit(positionHitResults);
		}
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			if (GetGroundEffectInfo().m_applyGroundEffect)
			{
				GetGroundEffectInfo().SetupActorHitResult(ref actorHitResults, caster, square);
			}
			if (actorData.GetTeam() == caster.GetTeam())
			{
				if (((actorData == caster && AffectsCaster()) || (actorData != caster && AffectsAllies()))
				    && GetDirectHitAllyEffect().m_applyEffect)
				{
					StandardEffectInfo hitAllyEffect = GetDirectHitAllyEffect().GetShallowCopy();
					StandardActorEffectData hitAllyEffectData = hitAllyEffect.m_effectData.GetShallowCopy();
					hitAllyEffect.m_effectData = hitAllyEffectData;
					if (m_syncComp != null)
					{
						hitAllyEffectData.m_absorbAmount += m_syncComp.m_extraAbsorbForShieldGenerator;
					}
					IEnumerable<ActorData> source = hitActors;
					int enemiesHit = source.Count(a => a.GetTeam() != caster.GetTeam());
					if (hitActors.Count - enemiesHit == 1)
					{
						hitAllyEffectData.m_absorbAmount += GetExtraAbsorbIfOnlyOneAllyHit();
					}
					hitAllyEffectData.m_absorbAmount += enemiesHit * GetExtraAbsorbPerEnemyHit();
					if (enemiesHit > 0)
					{
						hitAllyEffectData.m_absorbAmount += GetExtraAbsorbIfEnemyHit();
					}
					actorHitResults.AddStandardEffectInfo(hitAllyEffect);
					actorHitResults.AddStandardEffectInfo(GetExtraAllyHitEffect());
				}
			}
			else if (AffectsEnemies())
			{
				actorHitResults.AddStandardEffectInfo(GetDirectHitEnemyEffect());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(list);
	}

	// added in rogues
	private new List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargets)
	{
		return GetGroundEffectInfo().m_groundEffectData.GetAffectableActorsInField(
			Board.Get().GetSquare(targets[0].GridPos),
			targets[0].FreePos,
			caster,
			nonActorTargets);
	}

	// added in rogues
	public override void OnEffectAbsorbedDamage(ActorData effectCaster, int damageAbsorbed)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ArcherStats.ShieldArrowEffectiveShieldAndWeakenedMitigation, damageAbsorbed);
	}

	// added in rogues
	public override void OnCalculatedDamageReducedFromWeakenedGrantedByMyEffect(ActorData effectCaster, ActorData weakenedActor, int damageReduced)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ArcherStats.ShieldArrowEffectiveShieldAndWeakenedMitigation, damageReduced);
	}
#endif
}
