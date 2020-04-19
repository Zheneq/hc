using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class LobbyAbilityGameplayData
{
	public string TypeName;

	public string Name;

	public int Index;

	public Dictionary<int, LobbyAbilityModGameplayData> AbilityModData;

	public Dictionary<int, LobbyAbilityTauntData> AbilityTauntData;

	public Dictionary<int, LobbyAbilityVfxSwapData> AbilityVfxSwapData;

	public LobbyAbilityGameplayData()
	{
		this.AbilityModData = new Dictionary<int, LobbyAbilityModGameplayData>();
		this.AbilityTauntData = new Dictionary<int, LobbyAbilityTauntData>();
		this.AbilityVfxSwapData = new Dictionary<int, LobbyAbilityVfxSwapData>();
	}

	[JsonIgnore]
	public string FullName
	{
		get
		{
			return string.Format("{0} ({1})", this.Name, this.TypeName);
		}
	}

	public LobbyAbilityModGameplayData GetAbilityModData(int index)
	{
		LobbyAbilityModGameplayData result = null;
		this.AbilityModData.TryGetValue(index, out result);
		return result;
	}

	public LobbyAbilityTauntData GetAbilityTauntData(int index)
	{
		LobbyAbilityTauntData result = null;
		this.AbilityTauntData.TryGetValue(index, out result);
		return result;
	}

	public LobbyAbilityVfxSwapData GetAbilityVfxSwapData(int index)
	{
		LobbyAbilityVfxSwapData result = null;
		this.AbilityVfxSwapData.TryGetValue(index, out result);
		return result;
	}
}
