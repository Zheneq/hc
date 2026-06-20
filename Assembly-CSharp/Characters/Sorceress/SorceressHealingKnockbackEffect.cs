// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// missing in reactor
public class SorceressHealingKnockbackEffect : Effect
{
	public int m_damageOnExplodeAmount;
	public StandardEffectInfo m_enemyHitEffect;
	public float m_knockbackDistance;
	public bool m_aoePenetratesLos;
	public AbilityAreaShape m_aoeShape;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	private GameObject m_effectSequencePrefab;
	private GameObject m_detonateSequencePrefab;
	private GameObject m_gameplayHitSequencePrefab;

	public SorceressHealingKnockbackEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		SequenceSource parentSequenceSource,
		int damageOnExplodeAmount,
		StandardEffectInfo enemyHitEffect,
		float knockbackDistance,
		KnockbackType knockbackType,
		AbilityAreaShape aoeShape,
		bool aoePenetratesLos,
		GameObject effectSequence,
		GameObject detonateSequence,
		GameObject gameplayHitSequence)
		: base(parent, targetSquare, target, caster)
	{
		m_damageOnExplodeAmount = damageOnExplodeAmount;
		m_enemyHitEffect = enemyHitEffect;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_aoeShape = aoeShape;
		m_aoePenetratesLos = aoePenetratesLos;
		m_effectSequencePrefab = effectSequence;
		m_detonateSequencePrefab = detonateSequence;
		m_gameplayHitSequencePrefab = gameplayHitSequence;
		HitPhase = AbilityPriority.Combat_Knockback;
		m_time.duration = 1;
		m_effectName = parent.Ability.m_abilityName;
		SequenceSource = new SequenceSource(OnHit_Base, OnHit_Base, false, parentSequenceSource);
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_effectSequencePrefab, TargetSquare, Target.AsArray(), Caster, SequenceSource);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
		if (GetCasterAnimationIndex(HitPhase) > 0 || AddActorAnimEntryIfHasHits(HitPhase))
		{
			shallowCopy.SetWaitForClientEnable(true);
		}
		if (m_gameplayHitSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_gameplayHitSequencePrefab, TargetSquare, m_effectResults.HitActorsArray(), Caster, shallowCopy));
		}
		if (m_detonateSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_detonateSequencePrefab, TargetSquare, Target.AsArray(), Caster, shallowCopy));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_aoeShape, Target.GetFreePos(), Target.GetCurrentBoardSquare());
		foreach (ActorData target in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, centerOfShape);
			ActorHitResults actorHitResults = new ActorHitResults(m_damageOnExplodeAmount, HitActionType.Damage, hitParams);
			actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
			KnockbackHitData knockbackData = new KnockbackHitData(
				target,
				Target,
				m_knockbackType,
				Vector3.zero,
				centerOfShape,
				m_knockbackDistance);
			actorHitResults.AddKnockbackData(knockbackData);
			effectResults.StoreActorHit(actorHitResults);
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInShape(
			m_aoeShape,
			Target.GetFreePos(),
			Target.GetCurrentBoardSquare(),
			m_aoePenetratesLos,
			Caster,
			Caster.GetOtherTeams(),
			nonActorTargetInfo);
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return HitPhase == phaseIndex;
	}
}
#endif
