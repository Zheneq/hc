using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonConverter(typeof(CurrencyWallet.JsonConverter))]
[Serializable]
public class CurrencyWallet : IEnumerable<CurrencyData>, IEnumerable
{
	public List<CurrencyData> Data;

	public CurrencyWallet()
	{
		this.Data = new List<CurrencyData>();
	}

	public CurrencyWallet(List<CurrencyData> data)
	{
		this.Data = data;
	}

	public CurrencyData this[int i]
	{
		get
		{
			return this.Data[i];
		}
	}

	public int Count
	{
		get
		{
			return this.Data.Count;
		}
	}

	public IEnumerator<CurrencyData> GetEnumerator()
	{
		return this.Data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	public int GetCurrentAmount(CurrencyType currencyType)
	{
		return this.GetValue(currencyType).Amount;
	}

	public bool CanAfford(CurrencyData cost)
	{
		return this.GetCurrentAmount(cost.Type) >= cost.Amount;
	}

	public CurrencyData GetValue(CurrencyType currencyType)
	{
		for (int i = 0; i < this.Data.Count; i++)
		{
			if (this.Data[i].Type == currencyType)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CurrencyWallet.GetValue(CurrencyType)).MethodHandle;
				}
				return this.Data[i];
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		return new CurrencyData
		{
			Type = currencyType,
			Amount = 0
		};
	}

	public void SetValue(CurrencyData newBalance)
	{
		for (int i = 0; i < this.Data.Count; i++)
		{
			if (this.Data[i].Type == newBalance.Type)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CurrencyWallet.SetValue(CurrencyData)).MethodHandle;
				}
				this.Data[i] = newBalance;
				return;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		this.Data.Add(newBalance);
	}

	public CurrencyData ChangeValue(CurrencyType currencyType, int amount)
	{
		CurrencyData currencyData = null;
		for (int i = 0; i < this.Data.Count; i++)
		{
			if (this.Data[i].Type == currencyType)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CurrencyWallet.ChangeValue(CurrencyType, int)).MethodHandle;
				}
				currencyData = this.Data[i];
				IL_5A:
				if (currencyData == null)
				{
					currencyData = new CurrencyData
					{
						Type = currencyType,
						Amount = amount
					};
					this.Data.Add(currencyData);
				}
				else
				{
					int num = currencyData.Amount + amount;
					if (num < 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						Log.Error(string.Format("Cannot withdraw {0} amount {1}, insufficient amount available.", currencyType, amount), new object[0]);
						return null;
					}
					currencyData.Amount = num;
					if (amount < 0)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						currencyData.m_TotalSpent -= amount;
					}
				}
				return currencyData;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_5A;
		}
	}

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
			CurrencyWallet currencyWallet;
			if (existingValue != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(CurrencyWallet.JsonConverter.ReadJson(JsonReader, Type, object, JsonSerializer)).MethodHandle;
				}
				currencyWallet = (CurrencyWallet)existingValue;
			}
			else
			{
				currencyWallet = new CurrencyWallet();
			}
			CurrencyWallet currencyWallet2 = currencyWallet;
			serializer.Populate(reader, currencyWallet2.Data);
			return currencyWallet2;
		}
	}
}
