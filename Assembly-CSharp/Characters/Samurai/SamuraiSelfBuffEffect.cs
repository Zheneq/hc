// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SamuraiSelfBuffEffect : StandardActorEffect
{
	private AbilityStatMod m_buffFromFirstIncomingHit;
	private AbilityStatMod m_buffPerIncomingHit;
	private int m_techPointGainPerIncomingHit;
	private bool m_buffFromIndirectDamage;
	private int m_cdrIfNotHit;
	private AbilityData.ActionType m_cdrActionType;
	private GameObject m_reactionSequence;
	private GameObject m_buffSequence;
	private bool m_buffsApplied;
	private bool m_readyToEnd;
	private Samurai_SyncComponent m_syncComp;

	public SamuraiSelfBuffEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		AbilityStatMod buffFromFirstIncomingHit,
		AbilityStatMod buffPerIncomingHit,
		int tpGainPerIncomingHit,
		bool buffFromIndirectDamage,
		int cdrIfNotHit,
		AbilityData.ActionType cdrActionType,
		GameObject reactionSequence,
		GameObject buffSequence)
		: base(parent, targetSquare, target, caster, data)
	{
		m_buffFromFirstIncomingHit = buffFromFirstIncomingHit;
		m_buffPerIncomingHit = buffPerIncomingHit;
		m_techPointGainPerIncomingHit = tpGainPerIncomingHit;
		m_buffFromIndirectDamage = buffFromIndirectDamage;
		m_cdrIfNotHit = cdrIfNotHit;
		m_cdrActionType = cdrActionType;
		m_reactionSequence = reactionSequence;
		m_buffSequence = buffSequence;
		HitPhase = AbilityPriority.Prep_Defense;
		m_syncComp = caster.GetComponent<Samurai_SyncComponent>();
		m_syncComp.m_selfBuffIncomingHitsThisTurn = 0;
	}

	public SamuraiSelfBuffEffect(ActorHitParameters hitParams, StandardActorEffectData data) : base(hitParams, data)
	{
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp.m_selfBuffIncomingHitsThisTurn > 0)
		{
			Caster.GetActorStats().RemoveStatMod(m_buffFromFirstIncomingHit);
		}
		if (Caster != null)
		{
			for (int i = 1; i < m_syncComp.m_selfBuffIncomingHitsThisTurn; i++)
			{
				Caster.GetActorStats().RemoveStatMod(m_buffPerIncomingHit);
			}
		}
		m_syncComp.Networkm_selfBuffIncomingHitsThisTurn = 0;
		m_syncComp.Networkm_lastSelfBuffTurn = -1;
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_time.age <= m_perTurnHitDelay && m_syncComp.m_selfBuffIncomingHitsThisTurn <= 0)
		{
			m_readyToEnd = true;
			if (m_cdrIfNotHit > 0)
			{
				Caster.GetAbilityData().ApplyCooldownReduction(m_cdrActionType, m_cdrIfNotHit);
			}
		}
	}

	public void SetReadyToEnd()
	{
		m_readyToEnd = m_time.age >= m_perTurnHitDelay;
	}

	public override bool ShouldEndEarly()
	{
		return m_readyToEnd || base.ShouldEndEarly();
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_buffSequence,
				Caster.GetCurrentBoardSquare(),
				new[] { Caster },
				Caster,
				SequenceSource)
		};
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		base.GatherEffectResults(ref effectResults, isReal);
		if (m_syncComp.m_selfBuffIncomingHitsThisTurn > 0 && !m_buffsApplied)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			AbilityStatMod[] array = new AbilityStatMod[m_syncComp.m_selfBuffIncomingHitsThisTurn];
			array[0] = m_buffFromFirstIncomingHit;
			for (int i = 1; i < m_syncComp.m_selfBuffIncomingHitsThisTurn; i++)
			{
				array[i] = m_buffPerIncomingHit;
			}
			actorHitResults.AddPermanentStatMods(array);
			effectResults.StoreActorHit(actorHitResults);
			if (isReal)
			{
				m_buffsApplied = true;
			}
		}
	}

	public override bool ShouldForceReactToHit(ActorHitResults incomingHit)
	{
		return m_buffFromIndirectDamage;
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (m_time.age < m_perTurnHitDelay && incomingHit.HasDamage)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			actorHitResults.AddTechPointGain(m_techPointGainPerIncomingHit);
			reactions.Add(new AbilityResults_Reaction(
				this,
				actorHitResults,
				m_reactionSequence,
				Caster.GetCurrentBoardSquare(),
				SequenceSource,
				incomingHit.m_reactionDepth,
				isReal,
				incomingHit));
			if (isReal && GameFlowData.Get().IsInResolveState())
			{
				Samurai_SyncComponent syncComp = m_syncComp;
				syncComp.Networkm_selfBuffIncomingHitsThisTurn = syncComp.m_selfBuffIncomingHitsThisTurn + 1;
			}
		}
	}
}
#endif
