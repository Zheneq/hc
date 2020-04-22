using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
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
