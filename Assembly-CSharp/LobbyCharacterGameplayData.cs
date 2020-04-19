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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetDefaultModInfo()).MethodHandle;
				}
				result.SetModForAbility(i, defaultAbilityMod.Index);
			}
			else
			{
				result.SetModForAbility(i, -1);
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return result;
	}

	public LobbyAbilityModGameplayData GetDefaultAbilityMod(int abilityIndex)
	{
		LobbyAbilityModGameplayData result = null;
		LobbyAbilityGameplayData abilityData = this.GetAbilityData(abilityIndex);
		if (abilityData != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetDefaultAbilityMod(int)).MethodHandle;
			}
			using (Dictionary<int, LobbyAbilityModGameplayData>.ValueCollection.Enumerator enumerator = abilityData.AbilityModData.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyAbilityModGameplayData lobbyAbilityModGameplayData = enumerator.Current;
					if (lobbyAbilityModGameplayData.DefaultEquip)
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
						return lobbyAbilityModGameplayData;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetAbilityModGameplayData(int, int)).MethodHandle;
			}
			foreach (LobbyAbilityModGameplayData lobbyAbilityModGameplayData in abilityData.AbilityModData.Values)
			{
				if (lobbyAbilityModGameplayData.Index == abilityModId)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetAbilityData(int)).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetSkinData(int)).MethodHandle;
			}
			return this.SkinData[skinIndex];
		}
		return null;
	}

	public void SetSkinData(int skinIndex, LobbySkinGameplayData skinData)
	{
		if (skinIndex < this.SkinData.Length)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.SetSkinData(int, LobbySkinGameplayData)).MethodHandle;
			}
			this.SkinData[skinIndex] = skinData;
		}
	}

	public int GetColorRequiredLevelForEquip(int skinIndex, int patternIndex, int colorIndex)
	{
		if (skinIndex < this.SkinData.Length)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyCharacterGameplayData.GetColorRequiredLevelForEquip(int, int, int)).MethodHandle;
			}
			if (patternIndex < this.SkinData[skinIndex].PatternData.Length && colorIndex < this.SkinData[skinIndex].PatternData[patternIndex].ColorData.Length)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.SkinData[skinIndex].PatternData[patternIndex].ColorData[colorIndex].RequiredLevelForEquip;
			}
		}
		return 0;
	}
}
