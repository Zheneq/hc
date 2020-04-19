using System;
using System.Collections.Generic;

public class SquareInsideChecker_Path : ISquareInsideChecker
{
	private HashSet<BoardSquare> m_squaresInPath;

	public SquareInsideChecker_Path()
	{
		this.m_squaresInPath = new HashSet<BoardSquare>();
	}

	public void UpdateSquaresInPath(BoardSquarePathInfo path)
	{
		this.m_squaresInPath.Clear();
		for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			this.m_squaresInPath.Add(boardSquarePathInfo.square);
		}
	}

	public unsafe bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		bool result = false;
		inLos = false;
		if (this.m_squaresInPath.Contains(square))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SquareInsideChecker_Path.IsSquareInside(BoardSquare, bool*)).MethodHandle;
			}
			inLos = true;
			result = true;
		}
		return result;
	}
}
