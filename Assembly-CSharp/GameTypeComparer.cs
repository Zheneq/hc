using System;
using System.Collections.Generic;

[Serializable]
public struct GameTypeComparer : IEqualityComparer<GameType>
{
	public bool Equals(GameType x, GameType y)
	{
		return x == y;
	}

	public int GetHashCode(GameType obj)
	{
		return (int)obj;
	}
}
