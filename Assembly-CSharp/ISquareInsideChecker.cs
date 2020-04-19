using System;

public interface ISquareInsideChecker
{
	bool IsSquareInside(BoardSquare square, out bool inLos);
}
