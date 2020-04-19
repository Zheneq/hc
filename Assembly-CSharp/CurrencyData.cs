using System;
using Newtonsoft.Json;
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

	public CurrencyData()
	{
		this.m_Type = CurrencyType.ISO;
		this.m_Amount = 0;
		this.m_TotalSpent = 0;
	}

	public CurrencyType Type
	{
		get
		{
			return this.m_Type;
		}
		set
		{
			this.m_Type = value;
		}
	}

	public int Amount
	{
		get
		{
			return this.m_Amount;
		}
		set
		{
			this.m_Amount = value;
		}
	}

	public int TotalSpent
	{
		get
		{
			return this.m_TotalSpent;
		}
		set
		{
			this.m_TotalSpent = value;
		}
	}

	public override string ToString()
	{
		return string.Format("{0}: {1:N0}", this.m_Type.ToString(), this.m_Amount);
	}
}
