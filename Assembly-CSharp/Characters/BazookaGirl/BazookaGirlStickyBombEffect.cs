// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class BazookaGirlStickyBombEffect : Effect
{
	public ThiefPartingGiftBombInfo m_bombInfo;

	private SpoilsSpawnData m_spoilSpawnOnExplosion;
	private StandardEffectInfo m_onExplosionEffectInfo;
	private List<MiscHitEventData_AddToCasterCooldown> m_onExplosionCooldownEvents = new List<MiscHitEventData_AddToCasterCooldown>();
	private List<ActorData> m_hitActors;

	public BazookaGirlStickyBombEffect(
		EffectSource parent,
		List<ActorData> hitActors,
		ActorData caster,
		ThiefPartingGiftBombInfo bombInfo,
		SpoilsSpawnData spoilSpawnData)
		: base(parent, null, null, caster)
	{
		m_bombInfo = bombInfo;
		m_spoilSpawnOnExplosion = spoilSpawnData;
		m_onExplosionEffectInfo = m_bombInfo.onExplodeEffect;
		m_hitActors = new List<ActorData>(hitActors);
		m_time.duration = Mathf.Max(m_bombInfo.damageDelay + 1, m_time.duration);
		HitPhase = AbilityPriority.Combat_Damage;
		m_effectName = "Zuki - Sticky Bomb";
	}

	// rogues
	// public override bool CanExecuteForTeam_FCFS(Team team)
	// {
	// 	return Caster.GetTeam() != team;
	// }

	public void OverrideOnExplosionEffectInfo(StandardEffectInfo effectInfo)
	{
		m_onExplosionEffectInfo = effectInfo;
	}

	public void AddMiscEventForExplosionHit(MiscHitEventData_AddToCasterCooldown cooldownMiscEvent)
	{
		if (cooldownMiscEvent != null)
		{
			m_onExplosionCooldownEvents.Add(cooldownMiscEvent);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> effectStartSeqDataList = base.GetEffectStartSeqDataList();
		if (m_bombInfo.sequencePrefab != null)
		{
			foreach (ActorData actorData in m_hitActors)
			{
				effectStartSeqDataList.Add(
					new ServerClientUtils.SequenceStartData(
						m_bombInfo.sequencePrefab,
						actorData.GetCurrentBoardSquare(),
						actorData.AsArray(),
						Caster,
						SequenceSource));
			}
		}
		return effectStartSeqDataList;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
		if (m_time.age >= m_bombInfo.damageDelay
		    && m_bombInfo.explosionSequencePrefab != null)
		{
			SequenceSource source = SequenceSource.GetShallowCopy();
			if (AddActorAnimEntryIfHasHits(HitPhase))
			{
				source.SetWaitForClientEnable(true);
			}
			foreach (ActorData actorData in m_effectResults.HitActorsArray())
			{
				effectHitSeqDataList.Add(
					new ServerClientUtils.SequenceStartData(
						m_bombInfo.explosionSequencePrefab,
						actorData.GetCurrentBoardSquare(),
						actorData.AsArray(),
						Caster,
						source));
			}
		}
		return effectHitSeqDataList;
	}

	private List<ActorData> GetTargetsInExplosion()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in m_hitActors)
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && actorData.GetCurrentBoardSquare() != null)
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	public bool IsActorInTargetList(ActorData targetActor)
	{
		return m_hitActors.Contains(targetActor);
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_time.age < m_bombInfo.damageDelay)
		{
			return;
		}
		
		foreach (ActorData actorData in GetTargetsInExplosion())
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetFreePos()));
			actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(actorData, Caster.GetTeam(), m_spoilSpawnOnExplosion));
			actorHitResults.AddBaseDamage(m_bombInfo.damageAmount);
			actorHitResults.AddStandardEffectInfo(m_onExplosionEffectInfo);
			if (m_onExplosionCooldownEvents.Count > 0)
			{
				foreach (MiscHitEventData_AddToCasterCooldown e in m_onExplosionCooldownEvents)
				{
					actorHitResults.AddMiscHitEvent(e);
				}
			}
			effectResults.StoreActorHit(actorHitResults);
		}
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return m_time.age >= m_bombInfo.damageDelay;
	}

	public override ActorData GetActorAnimationActor()
	{
		foreach (ActorData actorData in m_hitActors)
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && actorData.GetCurrentBoardSquare() != null)
			{
				return actorData;
			}
		}
		return Caster;
	}

	// custom
	public override void OnExecutedEffectResults(EffectResults results)
	{
		base.OnExecutedEffectResults(results);
		foreach (KeyValuePair<ActorData,int> resultsDamageResult in results.DamageResults)
		{
			ActorData target = resultsDamageResult.Key;
			int damage = resultsDamageResult.Value;
			if (damage <= 0)
			{
				continue;
			}
			int currentTurn = GameFlowData.Get().CurrentTurn;
			// TODO ZUKI Freelancer stats: Do catalysts count?
			foreach (Ability abilityOfType in results.Caster.GetAbilityData().GetAbilitiesAsList())
			{
				if (abilityOfType != Parent.Ability
				    && abilityOfType.m_actorLastHitTurn != null
				    && abilityOfType.m_actorLastHitTurn.ContainsKey(target)
				    && abilityOfType.m_actorLastHitTurn[target] == currentTurn)
				{
					results.Caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.BazookaGirlStats.StickyBombCombos);
					return;
				}
			}
		}
	}
}
#endif
