// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SenseiAppendStatusEffect : StandardActorEffect
{
	private Passive_Sensei m_passive;
	private StandardEffectInfo m_effectAddedForAllyAttack;
	private StandardEffectInfo m_effectAddedForEnemyAttack;
	private int m_energyGainOnAllyHit;
	private bool m_endEffectIfAppendedStatus;
	private AbilityPriority m_earliestPriorityToConsider;
	private bool m_delayEffectApply;
	private bool m_requireDamageToTransfer;
	private bool m_appendedThisTurn;
	private bool m_shouldEndOnTurnEnd;
	private GameObject m_hitOnAllySequencePrefab;
	private GameObject m_hitOnEnemySequencePrefab;

	public SenseiAppendStatusEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		Passive_Sensei passive,
		StandardEffectInfo effectToAddForAllyAttack,
		StandardEffectInfo effectToAddForEnemyAttack,
		int energyGainOnAllyHit,
		bool endEffectIfAppendedStatus,
		AbilityPriority earliestPriorityToConsider,
		bool delayEffectApply,
		bool requireDamageToTransfer,
		GameObject hitOnAllySequencePrefab,
		GameObject hitOnEnemySequencePrefab)
		: base(parent, targetSquare, target, caster, data)
	{
		m_passive = passive;
		m_effectAddedForAllyAttack = effectToAddForAllyAttack;
		m_effectAddedForEnemyAttack = effectToAddForEnemyAttack;
		m_energyGainOnAllyHit = energyGainOnAllyHit;
		m_endEffectIfAppendedStatus = endEffectIfAppendedStatus;
		m_earliestPriorityToConsider = earliestPriorityToConsider;
		m_delayEffectApply = delayEffectApply;
		m_requireDamageToTransfer = requireDamageToTransfer;
		m_hitOnAllySequencePrefab = hitOnAllySequencePrefab;
		m_hitOnEnemySequencePrefab = hitOnEnemySequencePrefab;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		foreach (GameObject gameObject in m_data.m_sequencePrefabs)
		{
			if (gameObject != null)
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					gameObject, TargetSquare, Target.AsArray(), Target, SequenceSource));
			}
		}
		return list;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_appendedThisTurn = false;
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_appendedThisTurn)
		{
			m_shouldEndOnTurnEnd = true;
		}
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_appendedThisTurn = false;
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || (m_endEffectIfAppendedStatus && m_shouldEndOnTurnEnd);
	}

	public override bool ShouldForceReactToHit(ActorHitResults incomingHit)
	{
		return true;
	}

	public override bool CanReactToNormalMovementHit(ActorHitResults hit, bool isIncoming)
	{
		return !isIncoming;
	}

	public override void GatherResultsInResponseToOutgoingActorHit(ActorHitResults outgoingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (Target == outgoingHit.m_hitParameters.Caster
		    && (outgoingHit.m_hitParameters.Effect == null || outgoingHit.m_hitParameters.Effect != this)
		    && AreaEffectUtils.IsActorTargetable(outgoingHit.m_hitParameters.Target)
		    && (ServerActionBuffer.Get() == null || ServerActionBuffer.Get().AbilityPhase >= m_earliestPriorityToConsider)
		    && outgoingHit.m_hitParameters.Target.GetTeam() != Target.GetTeam()
		    && (!m_requireDamageToTransfer || outgoingHit.FinalDamage > 0))
		{
			AbilityResults_Reaction abilityResults = new AbilityResults_Reaction();
			ActorData target = outgoingHit.m_hitParameters.Target;
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, target.GetFreePos()));
			StandardEffectInfo standardEffectInfo;
			GameObject sequencePrefab;
			if (Target.GetTeam() == Caster.GetTeam())
			{
				standardEffectInfo = m_effectAddedForAllyAttack;
				sequencePrefab = m_hitOnAllySequencePrefab;
			}
			else
			{
				standardEffectInfo = m_effectAddedForEnemyAttack;
				sequencePrefab = m_hitOnEnemySequencePrefab;
				actorHitResults.SetTechPointGain(m_energyGainOnAllyHit);
			}
			if (m_delayEffectApply)
			{
				actorHitResults.AddMiscHitEvent(
					new MiscHitEventData_UpdatePassive(
						m_passive, 
						new List<MiscHitEventPassiveUpdateParams> 
						{
							new Passive_Sensei.AppendEffectParam(target, standardEffectInfo)
						}));
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(standardEffectInfo);
			}
			abilityResults.SetupGameplayData(this, actorHitResults, outgoingHit.m_reactionDepth, null, isReal, outgoingHit);
			abilityResults.SetupSequenceData(sequencePrefab, target.GetCurrentBoardSquare(), SequenceSource);
			reactions.Add(abilityResults);
			if (isReal)
			{
				m_appendedThisTurn = true;
			}
		}
	}
}
#endif
