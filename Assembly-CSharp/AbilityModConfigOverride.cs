using System;

[Serializable]
public class AbilityModConfigOverride
{
	public CharacterType CharacterType;

	public int AbilityIndex;

	public int AbilityModIndex;

	public bool Allowed;

	public AbilityModConfigOverride Clone()
	{
		return (AbilityModConfigOverride)base.MemberwiseClone();
	}
}
