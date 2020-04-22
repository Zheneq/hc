using Newtonsoft.Json;
using System;

[Serializable]
public class LobbyCardGameplayData
{
	public CardType CardType;

	public string DisplayName;

	public AbilityRunPhase RunPhase;

	public bool IsDefault;

	[JsonIgnore]
	public string FullName => $"{DisplayName} ({CardType.ToString()})";
}
