using System;

[Serializable]
public struct PlayerAbilityVfxSwapData
{
	public int AbilityId;

	public int AbilityVfxSwapID;

	public override string ToString()
	{
		return "Ability[" + AbilityId + "]->VfxSwap[" + AbilityVfxSwapID + "]";
	}
}
