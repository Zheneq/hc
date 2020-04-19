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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CountryPrice.StringToCurrencyCode(string)).MethodHandle;
			}
			if (Enum.IsDefined(typeof(CurrencyCode), currencyCodeString))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
