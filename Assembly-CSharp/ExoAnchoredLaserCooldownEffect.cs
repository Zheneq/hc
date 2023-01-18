// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class ExoAnchoredLaserCooldownEffect : StandardActorEffect
{
	private bool m_readyToEnd;
	private StandardEffectInfo m_effectOnAnchorEnd;
	private GameObject m_unanchorSequencePrefab;
	private Passive_Exo m_passive;
	private AbilityData.ActionType m_anchorLaserActionType = AbilityData.ActionType.INVALID_ACTION;

	public ExoAnchoredLaserCooldownEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		int cooldownToAdd,
		StandardEffectInfo effectOnAnchorEnd,
		GameObject unanchorAnimSequence)
		: base(parent, targetSquare, target, caster, data)
	{
		m_effectOnAnchorEnd = effectOnAnchorEnd;
		HitPhase = AbilityPriority.Prep_Defense;
		m_unanchorSequencePrefab = unanchorAnimSequence;
		PassiveData component = caster.GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = component.GetPassiveOfType(typeof(Passive_Exo)) as Passive_Exo;
		}
		ExoAnchorLaser abilityOfType = caster.GetAbilityData().GetAbilityOfType<ExoAnchorLaser>();
		if (abilityOfType != null)
		{
			m_anchorLaserActionType = caster.GetAbilityData().GetActionTypeOfAbility(abilityOfType);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (Target.GetAbilityData() != null
		    && m_time.age == 1
		    && m_effectResults.HitActorsArray().Length != 0)
		{
			list.Add(
				new ServerClientUtils.SequenceStartData(
					m_unanchorSequencePrefab,
					Caster.GetCurrentBoardSquare(),
					new[] { Caster },
					Caster,
					SequenceSource));
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		AbilityData abilityData = Target.GetAbilityData();
		if (m_time.age == 1
		    && abilityData != null
		    && m_passive != null
		    && m_passive.m_persistingBarrierInstance != null
		    // TODO EXO check this
		    // reactor
		    && !ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(ExoAnchorLaser))
			// && Caster.TeamSensitiveData_authority.HasQueuedAction(m_anchorLaserActionType)
		    // rogues
		    // && !Caster.GetActorTurnSM().PveIsAbilityAtIndexUsed((int)m_anchorLaserActionType)
		    )
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			actorHitResults.AddBarrierForRemoval(m_passive.m_persistingBarrierInstance, true);
			actorHitResults.AddStandardEffectInfo(m_effectOnAnchorEnd);
			effectResults.StoreActorHit(actorHitResults);
			if (isReal)
			{
				m_readyToEnd = true;
			}
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_readyToEnd || !m_passive.IsAnchored();
	}
}
#endif
