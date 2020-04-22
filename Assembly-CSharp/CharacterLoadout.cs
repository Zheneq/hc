using System;

[Serializable]
public class CharacterLoadout : ICloneable
{
	public const int c_maxLoadoutNameLength = 16;

	public string LoadoutName
	{
		get;
		set;
	}

	public ModStrictness Strictness
	{
		get;
		set;
	}

	public CharacterModInfo ModSet
	{
		get;
		set;
	}

	public CharacterAbilityVfxSwapInfo VFXSet
	{
		get;
		set;
	}

	public CharacterLoadout(CharacterModInfo modInfo, CharacterAbilityVfxSwapInfo vfxInfo, string name = "", ModStrictness strictness = ModStrictness.AllModes)
	{
		LoadoutName = name;
		ModSet = modInfo;
		VFXSet = vfxInfo;
		Strictness = strictness;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
