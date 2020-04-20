using System;
using System.Collections.Generic;

[Serializable]
public class CountryPrices
{
	public CountryPrice[] Prices;

	public float GetPrice(string currencyCodeString)
	{
		return this.GetPrice(CountryPrice.StringToCurrencyCode(currencyCodeString));
	}

	public float GetPrice(CurrencyCode currencyCode)
	{
		int num;
		if (this.Prices != null)
		{
			num = this.Prices.Length;
		}
		else
		{
			num = 0;
		}
		for (int i = 0; i < num; i++)
		{
			if (this.Prices[i].Currency == currencyCode)
			{
				return this.Prices[i].Price;
			}
		}
		return 0f;
	}

	public override bool Equals(object obj)
	{
		CountryPrices countryPrices = obj as CountryPrices;
		if (countryPrices == null)
		{
			return false;
		}
		List<CountryPrice> list = new List<CountryPrice>();
		List<CountryPrice> list2 = new List<CountryPrice>();
		list.AddRange(this.Prices);
		list2.AddRange(countryPrices.Prices);
		int i = list.Count - 1;
		IL_D8:
		while (i >= 0)
		{
			bool flag = false;
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				if (list[i].Equals(list2[j]))
				{
					list.RemoveAt(i);
					list2.RemoveAt(j);
					flag = true;
					IL_A5:
					if (!flag)
					{
						if (list[i].Price > 0f)
						{
							return false;
						}
					}
					i--;
					goto IL_D8;
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				goto IL_A5;
			}
		}
		bool flag2 = false;
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].Price > 0f)
			{
				flag2 = true;
				IL_128:
				for (int l = 0; l < list2.Count; l++)
				{
					if (list2[l].Price > 0f)
					{
						flag2 = true;
						break;
					}
				}
				return !flag2;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_128;
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
