public class OperationOnSquare_TurnOnHiddenSquareIndicator : IOperationOnSquare
{
	private AbilityUtil_Targeter m_targeter;

	public OperationOnSquare_TurnOnHiddenSquareIndicator(AbilityUtil_Targeter targeter)
	{
		m_targeter = targeter;
	}

	public void OperateOnSquare(BoardSquare square, ActorData actor, bool squareHasLos)
	{
		if (!(actor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			if (!squareHasLos)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_targeter.ShowHiddenSquareIndicatorForSquare(square);
						return;
					}
				}
			}
			if (HighlightUtils.Get().m_cachedShouldShowAffectedSquares)
			{
				m_targeter.ShowAffectedSquareIndicatorForSquare(square);
			}
			return;
		}
	}

	public bool ShouldEarlyOut()
	{
		return false;
	}
}
