public class Neko_AdditionalFxDiscPoweredUp : AdditionalVfxContainerBase
{
	private Neko_SyncComponent m_syncComp;
	private BoardSquare m_targetSquare;
	private Sequence m_parentSequence;

	public override void Initialize(Sequence parentSequence)
	{
		base.Initialize(parentSequence);
		if (parentSequence != null && parentSequence.Caster != null)
		{
			m_syncComp = parentSequence.Caster.GetComponent<Neko_SyncComponent>();
			m_targetSquare = Board.Get().GetSquareFromVec3(parentSequence.TargetPos);
			m_parentSequence = parentSequence;
		}
	}

	public override bool CanBeVisible(bool parentSeqVisible)
	{
		return m_parentSequence != null
		       && m_parentSequence.AgeInTurns > 0
		       && parentSeqVisible
		       && m_syncComp != null
		       && m_targetSquare != null
		       && GameFlowData.Get() != null
		       && m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn
		       && m_syncComp.m_clientDiscBuffTargetSquare == m_targetSquare;
	}
}
