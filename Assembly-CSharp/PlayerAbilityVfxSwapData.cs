using System;
using System.Text;

[Serializable]
public struct PlayerAbilityVfxSwapData
{
	public int AbilityId;

	public int AbilityVfxSwapID;

	public override string ToString()
	{
		return new StringBuilder().Append("Ability[").Append(AbilityId).Append("]->VfxSwap[").Append(AbilityVfxSwapID).Append("]").ToString();
	}
}
