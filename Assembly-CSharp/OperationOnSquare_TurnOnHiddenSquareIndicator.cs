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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OperationOnSquare_TurnOnHiddenSquareIndicator.OperateOnSquare(BoardSquare, ActorData, bool)).MethodHandle;
			}
			if (!squareHasLos)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
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
