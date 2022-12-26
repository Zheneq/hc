// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class MantaRegenerationEffect : StandardActorEffect
{
	public int m_maxRegeneration;
	public int m_turnsOfRegeneration;
	public float m_damageToHealRatio;
	public int m_techPointGainPerIncomingHit;

	private GameObject m_incomingHitSequencePrefab;
	private Passive_Manta m_passive;

	public MantaRegenerationEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		int maxRegen,
		int turnsOfRegen,
		float damageToHealRatio,
		int techPointsPerHit,
		GameObject incomingHitSequencePrefab)
		: base(parent, targetSquare, target, caster, data)
	{
		m_maxRegeneration = maxRegen;
		m_turnsOfRegeneration = turnsOfRegen;
		SetDurationBeforeStart(m_turnsOfRegeneration + 1);
		m_damageToHealRatio = damageToHealRatio;
		m_techPointGainPerIncomingHit = techPointsPerHit;
		m_incomingHitSequencePrefab = incomingHitSequencePrefab;
		m_perTurnHitDelay = 1;
		PassiveData passiveData = caster.GetPassiveData();
		if (passiveData != null)
		{
			m_passive = passiveData.GetPassiveOfType(typeof(Passive_Manta)) as Passive_Manta;
		}
	}

	public override int GetHealingPerTurn()
	{
		if (m_passive == null)
		{
			return 0;
		}
		float num = m_passive.DamageReceivedForRegeneration;
		num *= m_damageToHealRatio;
		num = Mathf.Min(m_maxRegeneration, num);
		return Mathf.RoundToInt(num / m_turnsOfRegeneration);
	}

	public override int GetExpectedHealOverTimeTotal()
	{
		if (m_time.age <= 0 || m_time.duration <= 0)
		{
			return 0;
		}
		int healingPerTurn = GetHealingPerTurn();
		int num = Mathf.Max(m_time.age, m_perTurnHitDelay);
		num = Mathf.Max(1, num);
		int num2 = m_time.duration - num;
		return Mathf.Max(0, healingPerTurn * num2);
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>();
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (m_data.m_sequencePrefabs.Length != 0)
		{
			GameObject[] sequencePrefabs = m_data.m_sequencePrefabs;
			foreach (GameObject prefab in sequencePrefabs)
			{
				ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
					prefab, TargetSquare, Target.AsArray(), Caster, SequenceSource);
				list.Add(item);
			}
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		ActorHitResults actorHitResults = BuildMainTargetHitResults();
		if (actorHitResults != null)
		{
			effectResults.StoreActorHit(actorHitResults);
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (m_time.age == 0 && incomingHit.HasDamage)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			actorHitResults.AddTechPointGain(m_techPointGainPerIncomingHit);
			AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
			abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
			abilityResults_Reaction.SetupSequenceData(m_incomingHitSequencePrefab, Caster.GetCurrentBoardSquare(), SequenceSource);
			reactions.Add(abilityResults_Reaction);
		}
	}
}
#endif
