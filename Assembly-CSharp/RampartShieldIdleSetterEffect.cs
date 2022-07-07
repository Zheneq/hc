// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
// added in rogues
public class RampartShieldIdleSetterEffect : Effect
{
	private Passive_Rampart m_passive;

	public RampartShieldIdleSetterEffect(EffectSource parent, ActorData caster, Passive_Rampart passive) : base(parent, null, caster, caster)
	{
		m_effectName = "Rampart Shield IdleType Setter";
		m_time.duration = 0;
		HitPhase = AbilityPriority.Combat_Final;
		m_passive = passive;
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == HitPhase && m_effectResults.HitActorsArray().Length != 0)
		{
			m_passive.SetIdleTypeForShield(false);
		}
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		if (array.Length != 0)
		{
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			shallowCopy.RemoveAtEndOfTurn = true;
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_passive.m_removeShieldWaitSequencePrefab, Target.GetCurrentBoardSquare(), array, Caster, shallowCopy);
			list.Add(item);
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_passive.GetCurrentShieldBarrier() != null)
		{
			ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			effectResults.StoreActorHit(hitResults);
		}
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	public override bool IgnoreCameraFraming()
	{
		return true;
	}
}
#endif
