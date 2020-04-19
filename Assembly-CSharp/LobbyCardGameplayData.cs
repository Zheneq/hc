using System;
using Newtonsoft.Json;

[Serializable]
public class LobbyCardGameplayData
{
	public CardType CardType;

	public string DisplayName;

	public AbilityRunPhase RunPhase;

	public bool IsDefault;

	[JsonIgnore]
	public string FullName
	{
		get
		{
			return string.Format("{0} ({1})", this.DisplayName, this.CardType.ToString());
		}
	}
}
