using Newtonsoft.Json;
using System;
using System.Text;

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
		get { return new StringBuilder().Append(DisplayName).Append(" (").Append(CardType.ToString()).Append(")").ToString(); }
	}
}
