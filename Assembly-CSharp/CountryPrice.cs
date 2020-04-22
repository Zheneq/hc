using Newtonsoft.Json;
using System;

[Serializable]
public class CountryPrice
{
	public CurrencyCode Currency;

	public float Price;

	[JsonConstructor]
	public CountryPrice()
	{
		Currency = CurrencyCode.USD;
		Price = 0f;
	}

	public CountryPrice(CurrencyCode currency, float price)
	{
		Currency = currency;
		Price = price;
	}

	public CountryPrice(string currencyString, float price)
		: this(StringToCurrencyCode(currencyString), price)
	{
	}

	public static CurrencyCode StringToCurrencyCode(string currencyCodeString)
	{
		CurrencyCode result = CurrencyCode.USD;
		if (!currencyCodeString.IsNullOrEmpty())
		{
			if (Enum.IsDefined(typeof(CurrencyCode), currencyCodeString))
			{
				result = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currencyCodeString);
			}
		}
		return result;
	}

	public override bool Equals(object obj)
	{
		CountryPrice countryPrice = obj as CountryPrice;
		if (countryPrice == null)
		{
			return false;
		}
		return Currency == countryPrice.Currency && Price == countryPrice.Price;
	}

	public override int GetHashCode()
	{
		return string.Concat(Currency, "|", Price).GetHashCode();
	}
}
