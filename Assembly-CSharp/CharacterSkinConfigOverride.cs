using System;
using System.Collections.Generic;

[Serializable]
public class CharacterSkinConfigOverride
{
	public CharacterType CharacterType;

	public List<SkinConfigOverride> SkinConfigs;

	public CharacterSkinConfigOverride(CharacterType characterType)
	{
		this.CharacterType = characterType;
		this.SkinConfigs = new List<SkinConfigOverride>();
	}

	public CharacterSkinConfigOverride Clone()
	{
		return (CharacterSkinConfigOverride)base.MemberwiseClone();
	}
}
