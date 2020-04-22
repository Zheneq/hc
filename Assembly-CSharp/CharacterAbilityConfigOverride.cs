using System;

[Serializable]
public class CharacterAbilityConfigOverride
{
	public CharacterType CharacterType;

	public AbilityConfigOverride[] AbilityConfigs;

	public CharacterAbilityConfigOverride(CharacterType characterType)
	{
		CharacterType = characterType;
		Array.Resize(ref AbilityConfigs, 5);
	}

	public AbilityConfigOverride GetAbilityConfig(int abilityIndex)
	{
		if (abilityIndex < AbilityConfigs.Length)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return AbilityConfigs[abilityIndex];
				}
			}
		}
		return null;
	}

	public void SetAbilityConfig(int abilityIndex, AbilityConfigOverride abilityConfig)
	{
		if (abilityIndex < AbilityConfigs.Length)
		{
			AbilityConfigs[abilityIndex] = abilityConfig;
		}
	}

	public CharacterAbilityConfigOverride Clone()
	{
		return (CharacterAbilityConfigOverride)MemberwiseClone();
	}
}
