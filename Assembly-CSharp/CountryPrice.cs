using System;
using Newtonsoft.Json;

[Serializable]
public class CountryPrice
{
	public CurrencyCode Currency;

	public float Price;

	[JsonConstructor]
	public CountryPrice()
	{
		this.Currency = CurrencyCode.USD;
		this.Price = 0f;
	}

	public CountryPrice(CurrencyCode currency, float price)
	{
		this.Currency = currency;
		this.Price = price;
	}

	public CountryPrice(string currencyString, float price) : this(CountryPrice.StringToCurrencyCode(currencyString), price)
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
		return countryPrice != null && this.Currency == countryPrice.Currency && this.Price == countryPrice.Price;
	}

	public override int GetHashCode()
	{
		return (this.Currency + "|" + this.Price).GetHashCode();
	}
}
