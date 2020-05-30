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
			m_targetSquare = Board.Get().GetSquare(parentSequence.TargetPos);
			m_parentSequence = parentSequence;
		}
	}

	public override bool CanBeVisible(bool parentSeqVisible)
	{
		if (m_parentSequence != null && m_parentSequence.AgeInTurns > 0)
		{
			if (parentSeqVisible)
			{
				if (m_syncComp != null)
				{
					if (m_targetSquare != null)
					{
						if (GameFlowData.Get() != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
								{
									int result;
									if (m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn)
									{
										result = ((m_syncComp.m_clientDiscBuffTargetSquare == m_targetSquare) ? 1 : 0);
									}
									else
									{
										result = 0;
									}
									return (byte)result != 0;
								}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}
}
