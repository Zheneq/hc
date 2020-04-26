using System;

[Serializable]
public class StoreItemPriceOverride
{
	public string productCode;

	public CountryPrices prices;

	public int inventoryTemplateId;
}
