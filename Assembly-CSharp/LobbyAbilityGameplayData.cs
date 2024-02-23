using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class LobbyAbilityGameplayData
{
	public string TypeName;

	public string Name;

	public int Index;

	public Dictionary<int, LobbyAbilityModGameplayData> AbilityModData;

	public Dictionary<int, LobbyAbilityTauntData> AbilityTauntData;

	public Dictionary<int, LobbyAbilityVfxSwapData> AbilityVfxSwapData;

	[JsonIgnore]
	public string FullName
	{
		get { return new StringBuilder().Append(Name).Append(" (").Append(TypeName).Append(")").ToString(); }
	}

	public LobbyAbilityGameplayData()
	{
		AbilityModData = new Dictionary<int, LobbyAbilityModGameplayData>();
		AbilityTauntData = new Dictionary<int, LobbyAbilityTauntData>();
		AbilityVfxSwapData = new Dictionary<int, LobbyAbilityVfxSwapData>();
	}

	public LobbyAbilityModGameplayData GetAbilityModData(int index)
	{
		LobbyAbilityModGameplayData value = null;
		AbilityModData.TryGetValue(index, out value);
		return value;
	}

	public LobbyAbilityTauntData GetAbilityTauntData(int index)
	{
		LobbyAbilityTauntData value = null;
		AbilityTauntData.TryGetValue(index, out value);
		return value;
	}

	public LobbyAbilityVfxSwapData GetAbilityVfxSwapData(int index)
	{
		LobbyAbilityVfxSwapData value = null;
		AbilityVfxSwapData.TryGetValue(index, out value);
		return value;
	}
}
