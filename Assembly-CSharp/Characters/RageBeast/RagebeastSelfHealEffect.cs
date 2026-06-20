// ROGUES
// SERVER

#if SERVER
public class RagebeastSelfHealEffect : StandardActorEffect
{
	private bool m_shouldSkipGatherResultsAndHits;

	public RagebeastSelfHealEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		AbilityPriority hitPhase)
		: base(parent, targetSquare, target, caster, data)
	{
		HitPhase = hitPhase;
	}

	public void SetSkipGatherResultsAndHits(bool skip)
	{
		m_shouldSkipGatherResultsAndHits = skip;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!m_shouldSkipGatherResultsAndHits)
		{
			base.GatherEffectResults(ref effectResults, isReal);
		}
	}
}
#endif
