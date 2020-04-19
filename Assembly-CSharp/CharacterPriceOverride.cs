using System;

[Serializable]
public class CharacterPriceOverride
{
	public CharacterType characterType;

	public string productCode;

	public CountryPrices prices;
}
