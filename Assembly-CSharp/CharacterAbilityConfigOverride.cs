using System;

[Serializable]
public class CharacterAbilityConfigOverride
{
	public CharacterType CharacterType;

	public AbilityConfigOverride[] AbilityConfigs;

	public CharacterAbilityConfigOverride(CharacterType characterType)
	{
		this.CharacterType = characterType;
		Array.Resize<AbilityConfigOverride>(ref this.AbilityConfigs, 5);
	}

	public AbilityConfigOverride GetAbilityConfig(int abilityIndex)
	{
		if (abilityIndex < this.AbilityConfigs.Length)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CharacterAbilityConfigOverride.GetAbilityConfig(int)).MethodHandle;
			}
			return this.AbilityConfigs[abilityIndex];
		}
		return null;
	}

	public void SetAbilityConfig(int abilityIndex, AbilityConfigOverride abilityConfig)
	{
		if (abilityIndex < this.AbilityConfigs.Length)
		{
			this.AbilityConfigs[abilityIndex] = abilityConfig;
		}
	}

	public CharacterAbilityConfigOverride Clone()
	{
		return (CharacterAbilityConfigOverride)base.MemberwiseClone();
	}
}
