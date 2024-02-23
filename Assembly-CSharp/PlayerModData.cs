using System;
using System.Runtime.InteropServices;
using System.Text;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PlayerModData
{
	public int AbilityId
	{
		get;
		set;
	}

	public int AbilityModID
	{
		get;
		set;
	}

	public override string ToString()
	{
		return new StringBuilder().Append("Ability[").Append(AbilityId).Append("]->Mod[").Append(AbilityModID).Append("]").ToString();
	}
}
