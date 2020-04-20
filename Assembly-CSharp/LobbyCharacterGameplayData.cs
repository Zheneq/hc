using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

	public LobbyCharacterGameplayData()
	{
		Array.Resize<LobbyAbilityGameplayData>(ref this.AbilityData, 5);
	}

	[JsonIgnore]
	public string FullName
	{
		get
		{
			return string.Format("{0} ({1})", this.DisplayName, this.CharacterType.ToString());
		}
	}

	public CharacterModInfo GetDefaultModInfo()
	{
		CharacterModInfo result = default(CharacterModInfo);
		for (int i = 0; i < this.AbilityData.Length; i++)
		{
			LobbyAbilityModGameplayData defaultAbilityMod = this.GetDefaultAbilityMod(i);
			if (defaultAbilityMod != null)
			{
				result.SetModForAbility(i, defaultAbilityMod.Index);
			}
			else
			{
				result.SetModForAbility(i, -1);
			}
		}
		return result;
	}

	public LobbyAbilityModGameplayData GetDefaultAbilityMod(int abilityIndex)
	{
		LobbyAbilityModGameplayData result = null;
		LobbyAbilityGameplayData abilityData = this.GetAbilityData(abilityIndex);
		if (abilityData != null)
		{
			using (Dictionary<int, LobbyAbilityModGameplayData>.ValueCollection.Enumerator enumerator = abilityData.AbilityModData.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyAbilityModGameplayData lobbyAbilityModGameplayData = enumerator.Current;
					if (lobbyAbilityModGameplayData.DefaultEquip)
					{
						return lobbyAbilityModGameplayData;
					}
				}
			}
		}
		return result;
	}

	public LobbyAbilityModGameplayData GetAbilityModGameplayData(int abilityIndex, int abilityModId)
	{
		LobbyAbilityModGameplayData result = null;
		LobbyAbilityGameplayData abilityData = this.GetAbilityData(abilityIndex);
		if (abilityData != null)
		{
			foreach (LobbyAbilityModGameplayData lobbyAbilityModGameplayData in abilityData.AbilityModData.Values)
			{
				if (lobbyAbilityModGameplayData.Index == abilityModId)
				{
					result = lobbyAbilityModGameplayData;
					break;
				}
			}
		}
		return result;
	}

	public LobbyAbilityGameplayData GetAbilityData(int abilityIndex)
	{
		if (abilityIndex < this.AbilityData.Length)
		{
			return this.AbilityData[abilityIndex];
		}
		return null;
	}

	public void SetAbilityData(int abilityIndex, LobbyAbilityGameplayData abilityData)
	{
		if (abilityIndex < this.AbilityData.Length)
		{
			this.AbilityData[abilityIndex] = abilityData;
		}
	}

	public LobbySkinGameplayData GetSkinData(int skinIndex)
	{
		if (skinIndex < this.SkinData.Length)
		{
			return this.SkinData[skinIndex];
		}
		return null;
	}

	public void SetSkinData(int skinIndex, LobbySkinGameplayData skinData)
	{
		if (skinIndex < this.SkinData.Length)
		{
			this.SkinData[skinIndex] = skinData;
		}
	}

	public int GetColorRequiredLevelForEquip(int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex < this.SkinData.Length)
		{
			if (patternIndex < this.SkinData[skinIndex].PatternData.Length && colorIndex < this.SkinData[skinIndex].PatternData[patternIndex].ColorData.Length)
			{
				return this.SkinData[skinIndex].PatternData[patternIndex].ColorData[colorIndex].RequiredLevelForEquip;
			}
		}
		return 0;
	}
}
