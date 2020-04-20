using System;

public class OperationOnSquare_TurnOnHiddenSquareIndicator : IOperationOnSquare
{
	private AbilityUtil_Targeter m_targeter;

	public OperationOnSquare_TurnOnHiddenSquareIndicator(AbilityUtil_Targeter targeter)
	{
		this.m_targeter = targeter;
	}

	public void OperateOnSquare(BoardSquare square, ActorData actor, bool squareHasLos)
	{
		if (actor == GameFlowData.Get().activeOwnedActorData)
		{
			if (!squareHasLos)
			{
				this.m_targeter.ShowHiddenSquareIndicatorForSquare(square);
			}
			else if (HighlightUtils.Get().m_cachedShouldShowAffectedSquares)
			{
				this.m_targeter.ShowAffectedSquareIndicatorForSquare(square);
			}
		}
	}

	public bool ShouldEarlyOut()
	{
		return false;
	}
}
