using System.Collections.Generic;

public class SquareInsideChecker_Path : ISquareInsideChecker
{
	private HashSet<BoardSquare> m_squaresInPath;

	public SquareInsideChecker_Path()
	{
		m_squaresInPath = new HashSet<BoardSquare>();
	}

	public void UpdateSquaresInPath(BoardSquarePathInfo path)
	{
		m_squaresInPath.Clear();
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			m_squaresInPath.Add(boardSquarePathInfo.square);
		}
	}

	public bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		bool result = false;
		inLos = false;
		if (m_squaresInPath.Contains(square))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			inLos = true;
			result = true;
		}
		return result;
	}
}
