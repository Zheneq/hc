using System;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PlayerModData
{
	public int AbilityId { get; set; }

	public int AbilityModID { get; set; }

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Ability[",
			this.AbilityId,
			"]->Mod[",
			this.AbilityModID,
			"]"
		});
	}
}
