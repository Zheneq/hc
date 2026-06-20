// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
// added in rogues
public class ExoPreventRagdollEffect : Effect
{
	public ExoPreventRagdollEffect(EffectSource parent, ActorData caster)
		: base(parent, null, caster, caster)
	{
		m_effectName = "Exo Ragdoll Prevent Effect";
		m_time.duration = 0;
		HitPhase = AbilityPriority.Combat_Final;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		if (array.Length != 0)
		{
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			shallowCopy.RemoveAtEndOfTurn = true;
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				Target.GetCurrentBoardSquare(),
				array,
				Caster,
				shallowCopy);
			list.Add(item);
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(ExoAnchorLaser)))
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
