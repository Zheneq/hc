using System.Collections.Generic;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Size = 1)]
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
