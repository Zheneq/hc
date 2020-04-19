using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class AbilityConfigOverride
{
	public CharacterType CharacterType;

	public int AbilityIndex;

	public Dictionary<int, AbilityModConfigOverride> AbilityModConfigs = new Dictionary<int, AbilityModConfigOverride>();

	public Dictionary<int, AbilityTauntConfigOverride> AbilityTauntConfigs = new Dictionary<int, AbilityTauntConfigOverride>();

	public AbilityConfigOverride(CharacterType characterType, int abilityIndex)
	{
		this.CharacterType = characterType;
		this.AbilityIndex = abilityIndex;
	}

	public AbilityModConfigOverride GetAbilityModConfig(int index)
	{
		AbilityModConfigOverride result = null;
		this.AbilityModConfigs.TryGetValue(index, out result);
		return result;
	}

	public AbilityTauntConfigOverride GetAbilityTauntConfig(int tauntId)
	{
		return this.AbilityTauntConfigs.Values.FirstOrDefault((AbilityTauntConfigOverride taunt) => taunt.AbilityTauntID == tauntId);
	}

	public AbilityConfigOverride Clone()
	{
		return (AbilityConfigOverride)base.MemberwiseClone();
	}
}
