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
			if (!squareHasLos)
			{
				return;
			}
		}
		this.m_squares.Add(square);
	}

	public bool ShouldEarlyOut()
	{
		return false;
	}
}
