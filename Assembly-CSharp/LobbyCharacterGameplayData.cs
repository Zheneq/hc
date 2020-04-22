using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LobbyCharacterGameplayData
{
	public CharacterType CharacterType;

	public string DisplayName;

	public CharacterRole CharacterRole;

	public CharacterConfig CharacterConfig;

	public LobbyAbilityGameplayData[] AbilityData;

	public LobbySkinGameplayData[] SkinData;

	public CountryPrices Prices;

	[JsonIgnore]
	public string FullName => $"{DisplayName} ({CharacterType.ToString()})";

	public LobbyCharacterGameplayData()
	{
		Array.Resize(ref AbilityData, 5);
	}

	public CharacterModInfo GetDefaultModInfo()
	{
		CharacterModInfo result = default(CharacterModInfo);
		for (int i = 0; i < AbilityData.Length; i++)
		{
			LobbyAbilityModGameplayData defaultAbilityMod = GetDefaultAbilityMod(i);
			if (defaultAbilityMod != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result.SetModForAbility(i, defaultAbilityMod.Index);
			}
			else
			{
				result.SetModForAbility(i, -1);
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	public LobbyAbilityModGameplayData GetDefaultAbilityMod(int abilityIndex)
	{
		LobbyAbilityModGameplayData result = null;
		LobbyAbilityGameplayData abilityData = GetAbilityData(abilityIndex);
		if (abilityData != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					using (Dictionary<int, LobbyAbilityModGameplayData>.ValueCollection.Enumerator enumerator = abilityData.AbilityModData.Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							LobbyAbilityModGameplayData current = enumerator.Current;
							if (current.DefaultEquip)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										return current;
									}
								}
							}
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	public LobbyAbilityModGameplayData GetAbilityModGameplayData(int abilityIndex, int abilityModId)
	{
		LobbyAbilityModGameplayData result = null;
		LobbyAbilityGameplayData abilityData = GetAbilityData(abilityIndex);
		if (abilityData != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					{
						foreach (LobbyAbilityModGameplayData value in abilityData.AbilityModData.Values)
						{
							if (value.Index == abilityModId)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										return value;
									}
								}
							}
						}
						return result;
					}
				}
			}
		}
		return result;
	}

	public LobbyAbilityGameplayData GetAbilityData(int abilityIndex)
	{
		if (abilityIndex < AbilityData.Length)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return AbilityData[abilityIndex];
				}
			}
		}
		return null;
	}

	public void SetAbilityData(int abilityIndex, LobbyAbilityGameplayData abilityData)
	{
		if (abilityIndex < AbilityData.Length)
		{
			AbilityData[abilityIndex] = abilityData;
		}
	}

	public LobbySkinGameplayData GetSkinData(int skinIndex)
	{
		if (skinIndex < SkinData.Length)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return SkinData[skinIndex];
				}
			}
		}
		return null;
	}

	public void SetSkinData(int skinIndex, LobbySkinGameplayData skinData)
	{
		if (skinIndex >= SkinData.Length)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SkinData[skinIndex] = skinData;
			return;
		}
	}

	public int GetColorRequiredLevelForEquip(int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex < SkinData.Length)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (patternIndex < SkinData[skinIndex].PatternData.Length && colorIndex < SkinData[skinIndex].PatternData[patternIndex].ColorData.Length)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return SkinData[skinIndex].PatternData[patternIndex].ColorData[colorIndex].RequiredLevelForEquip;
					}
				}
			}
		}
		return 0;
	}
}
