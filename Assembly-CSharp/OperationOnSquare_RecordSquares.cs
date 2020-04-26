using System.Collections.Generic;

public class OperationOnSquare_RecordSquares : IOperationOnSquare
{
	public bool IgnoreLos;

	private HashSet<BoardSquare> m_squares = new HashSet<BoardSquare>();

	public void ClearRecordedSquares()
	{
		m_squares.Clear();
	}

	public List<BoardSquare> GetSquaresList()
	{
		return new List<BoardSquare>(m_squares);
	}

	public void OperateOnSquare(BoardSquare square, ActorData actor, bool squareHasLos)
	{
		if (!IgnoreLos)
		{
			if (!squareHasLos)
			{
				return;
			}
		}
		m_squares.Add(square);
	}

	public bool ShouldEarlyOut()
	{
		return false;
	}
}
