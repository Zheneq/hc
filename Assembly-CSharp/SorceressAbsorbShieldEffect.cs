// ROGUES
// SERVER
#if SERVER
// missing in reactor
public class SorceressAbsorbShieldEffect : Effect
{
	public SorceressAbsorbShieldEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		int duration,
		int absorbAmount,
		SequenceSource parentSequenceSource)
		: base(parent, targetSquare, target, caster)
	{
		m_time.duration = duration;
		InitAbsorbtion(absorbAmount);
		m_effectName = parent.Ability.m_abilityName;
		SequenceSource = new SequenceSource(OnHit_Base, OnHit_Base, false, parentSequenceSource);
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			SequenceLookup.Get().GetSimpleHitSequencePrefab(),
			TargetSquare,
			Target.AsArray(),
			Caster,
			SequenceSource);
	}
}
#endif
