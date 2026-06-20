// ROGUES
// SERVER
using System.Collections.Generic;

#if SERVER
// custom
public class ClericRangedHealEffect : Effect
{
	public int m_techPointGainPerIncomingHit;
	public StandardEffectInfo m_reactionEffect; // TODO CLERIC (not used in the ability or any of the mods)

	public ClericRangedHealEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		int techPointGainPerIncomingHit,
		StandardEffectInfo reactionEffect)
		: base(parent, targetSquare, target, caster)
	{
		m_techPointGainPerIncomingHit = techPointGainPerIncomingHit;
		m_reactionEffect = reactionEffect;
		m_time.duration = 1;
	}
	
	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>();
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>();
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (!incomingHit.HasDamage)
		{
			return;
		}
		// TODO CLERIC check this mod in old replays (as if anybody used it)
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Caster.GetFreePos()));
		actorHitResults.AddTechPointGain(m_techPointGainPerIncomingHit);
		AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
		abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
		abilityResults_Reaction.SetupSequenceData(null, Target.GetCurrentBoardSquare(), SequenceSource);
		reactions.Add(abilityResults_Reaction);
	}
}
#endif
