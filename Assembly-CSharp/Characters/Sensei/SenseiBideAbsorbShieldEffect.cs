// ROGUES
// SERVER

#if SERVER
// added in rogues
public class SenseiBideAbsorbShieldEffect : StandardActorEffect
{
	private Passive_Sensei m_passive;

	public SenseiBideAbsorbShieldEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData effectData,
		Passive_Sensei passive)
		: base(parent, targetSquare, target, caster, effectData)
	{
		m_passive = passive;
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_passive != null && GameFlowData.Get() != null)
		{
			m_passive.m_bideAbsorbRemainingOnEnd = Absorbtion.m_absorbRemaining;
			m_passive.m_bideAbsorbEndTurn = GameFlowData.Get().CurrentTurn;
		}
	}
}
#endif
