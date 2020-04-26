using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class BankComponent
{
	public const int MAX_SAVED_TRANSACTIONS = 20;

	public CurrencyWallet CurrentAmounts
	{
		get;
		set;
	}

	public Queue<CurrencyTransaction> Transactions
	{
		get;
		set;
	}

	public BankComponent()
	{
		CurrentAmounts = new CurrencyWallet();
		Transactions = new Queue<CurrencyTransaction>();
	}

	public BankComponent(List<CurrencyData> currentBalances)
	{
		CurrentAmounts = new CurrencyWallet(currentBalances);
		Transactions = new Queue<CurrencyTransaction>();
	}

	public int GetCurrentAmount(CurrencyType currencyType)
	{
		return CurrentAmounts.GetCurrentAmount(currencyType);
	}

	public bool CanAfford(CurrencyData cost)
	{
		return CurrentAmounts.CanAfford(cost);
	}

	public virtual void SetValue(CurrencyData newBalance)
	{
		CurrentAmounts.SetValue(newBalance);
	}

	public CurrencyData ChangeValue(CurrencyType currencyType, int amount, string source)
	{
		CurrencyData currencyData = CurrentAmounts.ChangeValue(currencyType, amount);
		if (currencyData == null)
		{
			return null;
		}
		Transactions.Enqueue(new CurrencyTransaction
		{
			Type = currencyType,
			Amount = amount,
			Source = source,
			Time = DateTime.UtcNow
		});
		while (Transactions.Count > 20)
		{
			Transactions.Dequeue();
		}
		while (true)
		{
			return currencyData;
		}
	}

	public BankComponent Clone()
	{
		string value = JsonConvert.SerializeObject(this);
		return JsonConvert.DeserializeObject<BankComponent>(value);
	}
}
