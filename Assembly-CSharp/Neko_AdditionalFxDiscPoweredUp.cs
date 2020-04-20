using System;

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
			this.m_syncComp = parentSequence.Caster.GetComponent<Neko_SyncComponent>();
			this.m_targetSquare = Board.Get().GetBoardSquare(parentSequence.TargetPos);
			this.m_parentSequence = parentSequence;
		}
	}

	public override bool CanBeVisible(bool parentSeqVisible)
	{
		if (this.m_parentSequence != null && this.m_parentSequence.AgeInTurns > 0)
		{
			if (parentSeqVisible)
			{
				if (this.m_syncComp != null)
				{
					if (this.m_targetSquare != null)
					{
						if (GameFlowData.Get() != null)
						{
							bool result;
							if (this.m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn)
							{
								result = (this.m_syncComp.m_clientDiscBuffTargetSquare == this.m_targetSquare);
							}
							else
							{
								result = false;
							}
							return result;
						}
					}
				}
			}
		}
		return false;
	}
}
