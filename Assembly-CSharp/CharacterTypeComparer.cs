using System;
using System.Collections.Generic;

public struct CharacterTypeComparer : IEqualityComparer<CharacterType>
{
	public bool Equals(CharacterType x, CharacterType y)
	{
		return x == y;
	}

	public int GetHashCode(CharacterType obj)
	{
		return (int)obj;
	}
}
