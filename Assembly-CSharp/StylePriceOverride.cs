using System;

[Serializable]
public class StylePriceOverride
{
	public string productCode;

	public CharacterType characterType;

	public int skinIndex;

	public int textureIndex;

	public int colorIndex;

	public CountryPrices prices;
}
