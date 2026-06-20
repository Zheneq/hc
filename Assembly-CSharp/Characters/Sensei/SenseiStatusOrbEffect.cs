// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SenseiStatusOrbEffect : StandardActorEffect
{
	private int m_numOrbs;
	private float m_orbHitRadius;
	private bool m_orbHitIgnoreLos;
	private int m_fromAllyDamageOnHit;
	private int m_fromAllySelfHealPerHit;
	private StandardEffectInfo m_fromAllyEnemyHitEffect;
	private int m_fromEnemyHealOnHit;
	private int m_fromEnemySelfDamagePerHit;
	private StandardEffectInfo m_fromEnemyAllyHitEffect;
	private GameObject m_orbToAllySequencePrefab;
	private GameObject m_orbToEnemySequencePrefab;
	private GameObject m_hitOnEffectTargetSequencePrefab;

	public SenseiStatusOrbEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		int numOrbs,
		float orbHitRadius,
		bool orbHitIgnoreLos,
		int fromAllyDamageOnHit,
		int fromAllySelfHealPerHit,
		StandardEffectInfo fromAllyEnemyHitEffect,
		int fromEnemyHealOnHit,
		int fromEnemySelfDamagePerHit,
		StandardEffectInfo fromEnemyAllyHitEffect,
		GameObject orbToAllySequencePrefab,
		GameObject orbToEnemySequencePrefab,
		GameObject hitOnEffectTargetSequencePrefab)
		: base(parent, targetSquare, target, caster, data)
	{
		m_numOrbs = numOrbs;
		m_orbHitRadius = orbHitRadius;
		m_orbHitIgnoreLos = orbHitIgnoreLos;
		m_fromAllyDamageOnHit = fromAllyDamageOnHit;
		m_fromAllySelfHealPerHit = fromAllySelfHealPerHit;
		m_fromAllyEnemyHitEffect = fromAllyEnemyHitEffect;
		m_fromEnemyHealOnHit = fromEnemyHealOnHit;
		m_fromEnemySelfDamagePerHit = fromEnemySelfDamagePerHit;
		m_fromEnemyAllyHitEffect = fromEnemyAllyHitEffect;
		m_orbToAllySequencePrefab = orbToAllySequencePrefab;
		m_orbToEnemySequencePrefab = orbToEnemySequencePrefab;
		m_hitOnEffectTargetSequencePrefab = hitOnEffectTargetSequencePrefab;
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		bool flag = false;
		foreach (ActorData actorData in array)
		{
			if (actorData != Target)
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					actorData.GetTeam() == Caster.GetTeam()
						? m_orbToAllySequencePrefab
						: m_orbToEnemySequencePrefab,
					actorData.GetFreePos(),
					actorData.AsArray(),
					Target,
					SequenceSource));
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_hitOnEffectTargetSequencePrefab,
				Target.GetFreePos(),
				Target.AsArray(),
				Target,
				SequenceSource));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		Vector3 freePos = Target.GetFreePos();
		List<ActorData> orbHitActors = GetOrbHitActors();
		foreach (ActorData actorData in orbHitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, freePos));
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				actorHitResults.SetBaseHealing(m_fromEnemyHealOnHit);
				actorHitResults.AddStandardEffectInfo(m_fromEnemyAllyHitEffect);
			}
			else
			{
				actorHitResults.SetBaseDamage(m_fromAllyDamageOnHit);
				actorHitResults.AddStandardEffectInfo(m_fromAllyEnemyHitEffect);
			}
			effectResults.StoreActorHit(actorHitResults);
		}
		if (orbHitActors.Count > 0)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			if (Target.GetTeam() == Caster.GetTeam())
			{
				int baseHealing = m_fromAllySelfHealPerHit * orbHitActors.Count;
				actorHitResults2.SetBaseHealing(baseHealing);
			}
			else
			{
				int baseDamage = m_fromEnemySelfDamagePerHit * orbHitActors.Count;
				actorHitResults2.SetBaseDamage(baseDamage);
			}
			effectResults.StoreActorHit(actorHitResults2);
		}
	}

	private List<ActorData> GetOrbHitActors()
	{
		bool flag = Target.GetTeam() == Caster.GetTeam();
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(Caster, !flag, flag);
		Vector3 freePos = Target.GetFreePos();
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(freePos, m_orbHitRadius, m_orbHitIgnoreLos, Caster, relevantTeams, null);
		actorsInRadius.Remove(Target);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadius, freePos);
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadius, m_numOrbs);
		return actorsInRadius;
	}
}
#endif
