using System;
using System.Collections.Generic;

[Serializable]
public class CountryPrices
{
	public CountryPrice[] Prices;

	public float GetPrice(string currencyCodeString)
	{
		return GetPrice(CountryPrice.StringToCurrencyCode(currencyCodeString));
	}

	public float GetPrice(CurrencyCode currencyCode)
	{
		int num;
		if (Prices != null)
		{
			num = Prices.Length;
		}
		else
		{
			num = 0;
		}
		for (int i = 0; i < num; i++)
		{
			if (Prices[i].Currency != currencyCode)
			{
				continue;
			}
			while (true)
			{
				return Prices[i].Price;
			}
		}
		return 0f;
	}

	public override bool Equals(object obj)
	{
		CountryPrices countryPrices = obj as CountryPrices;
		if (countryPrices == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		List<CountryPrice> list = new List<CountryPrice>();
		List<CountryPrice> list2 = new List<CountryPrice>();
		list.AddRange(Prices);
		list2.AddRange(countryPrices.Prices);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			bool flag = false;
			int num2 = list2.Count - 1;
			while (true)
			{
				if (num2 >= 0)
				{
					if (list[num].Equals(list2[num2]))
					{
						list.RemoveAt(num);
						list2.RemoveAt(num2);
						flag = true;
						break;
					}
					num2--;
					continue;
				}
				break;
			}
			if (!flag)
			{
				if (list[num].Price > 0f)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
		}
		while (true)
		{
			bool flag2 = false;
			int num3 = 0;
			while (true)
			{
				if (num3 < list.Count)
				{
					if (list[num3].Price > 0f)
					{
						flag2 = true;
						break;
					}
					num3++;
					continue;
				}
				break;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				if (list2[i].Price > 0f)
				{
					flag2 = true;
					break;
				}
			}
			return !flag2;
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
