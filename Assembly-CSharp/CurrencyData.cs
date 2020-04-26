using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class CurrencyData
{
	[JsonIgnore]
	public CurrencyType m_Type;

	[JsonIgnore]
	public int m_Amount;

	[HideInInspector]
	public int m_TotalSpent;

	public CurrencyType Type
	{
		get
		{
			return m_Type;
		}
		set
		{
			m_Type = value;
		}
	}

	public int Amount
	{
		get
		{
			return m_Amount;
		}
		set
		{
			m_Amount = value;
		}
	}

	public int TotalSpent
	{
		get
		{
			return m_TotalSpent;
		}
		set
		{
			m_TotalSpent = value;
		}
	}

	public CurrencyData()
	{
		m_Type = CurrencyType.ISO;
		m_Amount = 0;
		m_TotalSpent = 0;
	}

	public override string ToString()
	{
		return $"{m_Type.ToString()}: {m_Amount:N0}";
	}
}
