using System;

[Serializable]
public class LobbyGamePack
{
	public int Index;

	public CountryPrices Prices;

	public string ProductCode;

	public string Entitlement;

	public GamePackUpgrade[] Upgrades;
}
