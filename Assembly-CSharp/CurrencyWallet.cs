using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
[JsonConverter(typeof(JsonConverter))]
public class CurrencyWallet : IEnumerable<CurrencyData>, IEnumerable
{
	private class JsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(CurrencyWallet);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			CurrencyWallet currencyWallet = (CurrencyWallet)value;
			serializer.Serialize(writer, currencyWallet.Data);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object obj;
			if (existingValue != null)
			{
				obj = (CurrencyWallet)existingValue;
			}
			else
			{
				obj = new CurrencyWallet();
			}
			CurrencyWallet currencyWallet = (CurrencyWallet)obj;
			serializer.Populate(reader, currencyWallet.Data);
			return currencyWallet;
		}
	}

	public List<CurrencyData> Data;

	public CurrencyData this[int i] => Data[i];

	public int Count => Data.Count;

	public CurrencyWallet()
	{
		Data = new List<CurrencyData>();
	}

	public CurrencyWallet(List<CurrencyData> data)
	{
		Data = data;
	}

	public IEnumerator<CurrencyData> GetEnumerator()
	{
		return Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public int GetCurrentAmount(CurrencyType currencyType)
	{
		return GetValue(currencyType).Amount;
	}

	public bool CanAfford(CurrencyData cost)
	{
		return GetCurrentAmount(cost.Type) >= cost.Amount;
	}

	public CurrencyData GetValue(CurrencyType currencyType)
	{
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data[i].Type != currencyType)
			{
				continue;
			}
			while (true)
			{
				return Data[i];
			}
		}
		while (true)
		{
			CurrencyData currencyData = new CurrencyData();
			currencyData.Type = currencyType;
			currencyData.Amount = 0;
			return currencyData;
		}
	}

	public void SetValue(CurrencyData newBalance)
	{
		for (int i = 0; i < Data.Count; i++)
		{
			if (Data[i].Type != newBalance.Type)
			{
				continue;
			}
			while (true)
			{
				Data[i] = newBalance;
				return;
			}
		}
		while (true)
		{
			Data.Add(newBalance);
			return;
		}
	}

	public CurrencyData ChangeValue(CurrencyType currencyType, int amount)
	{
		CurrencyData currencyData = null;
		int num = 0;
		while (true)
		{
			if (num < Data.Count)
			{
				if (Data[num].Type == currencyType)
				{
					currencyData = Data[num];
					break;
				}
				num++;
				continue;
			}
			break;
		}
		int num2 = 0;
		if (currencyData == null)
		{
			CurrencyData currencyData2 = new CurrencyData();
			currencyData2.Type = currencyType;
			currencyData2.Amount = amount;
			currencyData = currencyData2;
			num2 = amount;
			Data.Add(currencyData);
		}
		else
		{
			num2 = currencyData.Amount + amount;
			if (num2 < 0)
			{
				while (true)
				{
					Log.Error($"Cannot withdraw {currencyType} amount {amount}, insufficient amount available.");
					return null;
				}
			}
			currencyData.Amount = num2;
			if (amount < 0)
			{
				currencyData.m_TotalSpent -= amount;
			}
		}
		return currencyData;
	}
}
