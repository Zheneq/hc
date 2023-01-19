// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SenseiStatusOrbs : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetAlly = true;
	public bool m_canTargetEnemy = true;
	public bool m_canTagetSelf;
	public bool m_targetingIgnoreLos;
	[Header("-- Effect data for on cast hit targets --")]
	public StandardActorEffectData m_enemyCastHitEffectData;
	public StandardActorEffectData m_allyCastHitEffectData;
	[Header("-- Combat phase hit params --")]
	public int m_numOrbs = 3;
	public float m_orbHitRadius = 3.5f;
	public bool m_orbHitIgnoreLos;
	[Header("    - if targeted Ally on cast -")]
	public int m_fromAllyDamageOnHit = 10;
	public int m_fromAllySelfHealPerHit = 2;
	public StandardEffectInfo m_fromAllyEnemyHitEffect;
	[Header("    - if targeted Enemy on cast -")]
	public int m_fromEnemyHealOnHit = 5;
	public int m_fromEnemySelfDamagePerHit = 1;
	public StandardEffectInfo m_fromEnemyAllyHitEffect;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	[Header("    - for orb originating from an Ally towards Enemy")]
	public GameObject m_orbToAllySequencePrefab;
	[Header("    - for orb originating from an Enemy towards Ally")]
	public GameObject m_orbToEnemySequencePrefab;
	[Header("    - for hit on effect's target when orbs launch")]
	public GameObject m_hitOnEffectTargetSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiStatusOrbs";
		}
		Setup();
	}

	private void Setup()
	{
		Targeter = new AbilityUtil_Targeter_AoE_AroundActor(
			this,
			m_orbHitRadius,
			m_orbHitIgnoreLos,
			true,
			true,
			m_numOrbs,
			m_canTargetEnemy,
			m_canTargetAlly,
			m_canTagetSelf)
		{
			m_customShouldIncludeActorDelegate = ShouldAddActorForTargeter,
			m_allyOccupantSubject = AbilityTooltipSubject.Tertiary,
			m_enemyOccupantSubject = AbilityTooltipSubject.Tertiary
		};
	}

	private bool ShouldAddActorForTargeter(ActorData potentialActor, Vector3 centerPos, ActorData targetingActor)
	{
		if (potentialActor != null)
		{
			BoardSquare centerSquare = Board.Get().GetSquareFromVec3(centerPos);
			ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(centerSquare, true, true, targetingActor);
			if (targetableActorOnSquare != null
			    && targetableActorOnSquare.IsActorVisibleToClient()
			    && potentialActor != targetableActorOnSquare)
			{
				return potentialActor.GetTeam() != targetableActorOnSquare.GetTeam();
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_healing = 0;
		results.m_damage = 0;
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Tertiary) > 0)
		{
			if (ActorData.GetTeam() == targetActor.GetTeam())
			{
				int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				results.m_healing = Mathf.Max(0, m_fromAllySelfHealPerHit * visibleActorsCountByTooltipSubject);
			}
			else
			{
				int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
				allyNum += Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Self);
				results.m_damage = Mathf.Max(0, m_fromEnemySelfDamagePerHit * allyNum);
			}
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = m_fromAllyDamageOnHit;
		}
		else
		{
			results.m_healing = m_fromEnemyHealOnHit;
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			m_canTargetEnemy,
			m_canTargetAlly,
			m_canTagetSelf,
			ValidateCheckPath.Ignore,
			!m_targetingIgnoreLos,
			true);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(
			caster,
			m_canTargetEnemy,
			m_canTargetAlly,
			m_canTagetSelf,
			ValidateCheckPath.Ignore,
			!m_targetingIgnoreLos,
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				Board.Get().GetSquare(targets[0].GridPos),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorData hitActor = GetHitActor(targets, caster);
		if (hitActor != null)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
			StandardActorEffectData data = hitActor.GetTeam() == caster.GetTeam() ? m_allyCastHitEffectData : m_enemyCastHitEffectData;
			actorHitResults.AddEffect(new SenseiStatusOrbEffect(
				AsEffectSource(),
				hitActor.GetCurrentBoardSquare(),
				hitActor,
				caster,
				data,
				m_numOrbs,
				m_orbHitRadius,
				m_orbHitIgnoreLos,
				m_fromAllyDamageOnHit,
				m_fromAllySelfHealPerHit,
				m_fromAllyEnemyHitEffect,
				m_fromEnemyHealOnHit,
				m_fromEnemySelfDamagePerHit,
				m_fromEnemyAllyHitEffect,
				m_orbToAllySequencePrefab,
				m_orbToEnemySequencePrefab,
				m_hitOnEffectTargetSequencePrefab));
			abilityResults.StoreActorHit(actorHitResults);
		}
	}

	// added in rogues
	private ActorData GetHitActor(List<AbilityTarget> targets, ActorData caster)
	{
		return AreaEffectUtils.GetTargetableActorOnSquare(Board.Get().GetSquare(targets[0].GridPos), m_canTargetEnemy, m_canTargetAlly, caster);
	}
#endif
}
