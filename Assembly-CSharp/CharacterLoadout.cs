using System;

[Serializable]
public class CharacterLoadout : ICloneable
{
	public const int c_maxLoadoutNameLength = 0x10;

	public CharacterLoadout(CharacterModInfo modInfo, CharacterAbilityVfxSwapInfo vfxInfo, string name = "", ModStrictness strictness = ModStrictness.AllModes)
	{
		this.LoadoutName = name;
		this.ModSet = modInfo;
		this.VFXSet = vfxInfo;
		this.Strictness = strictness;
	}

	public string LoadoutName { get; set; }

	public ModStrictness Strictness { get; set; }

	public CharacterModInfo ModSet { get; set; }

	public CharacterAbilityVfxSwapInfo VFXSet { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
