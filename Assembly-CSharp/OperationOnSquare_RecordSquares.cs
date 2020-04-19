using System;
using System.Collections.Generic;

public class OperationOnSquare_RecordSquares : IOperationOnSquare
{
	public bool IgnoreLos;

	private HashSet<BoardSquare> m_squares = new HashSet<BoardSquare>();

	public void ClearRecordedSquares()
	{
		this.m_squares.Clear();
	}

	public List<BoardSquare> GetSquaresList()
	{
		return new List<BoardSquare>(this.m_squares);
	}

	public void OperateOnSquare(BoardSquare square, ActorData actor, bool squareHasLos)
	{
		if (!this.IgnoreLos)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OperationOnSquare_RecordSquares.OperateOnSquare(BoardSquare, ActorData, bool)).MethodHandle;
			}
			if (!squareHasLos)
			{
				return;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_squares.Add(square);
	}

	public bool ShouldEarlyOut()
	{
		return false;
	}
}
