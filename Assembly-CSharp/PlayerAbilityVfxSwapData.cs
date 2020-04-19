using System;

[Serializable]
public struct PlayerAbilityVfxSwapData
{
	public int AbilityId;

	public int AbilityVfxSwapID;

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Ability[",
			this.AbilityId,
			"]->VfxSwap[",
			this.AbilityVfxSwapID,
			"]"
		});
	}
}
