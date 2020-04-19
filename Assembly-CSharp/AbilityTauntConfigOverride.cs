using System;

[Serializable]
public class AbilityTauntConfigOverride
{
	public CharacterType CharacterType;

	public int AbilityIndex;

	public int AbilityTauntIndex;

	public int AbilityTauntID;

	public bool Allowed;

	public AbilityTauntConfigOverride Clone()
	{
		return (AbilityTauntConfigOverride)base.MemberwiseClone();
	}
}
