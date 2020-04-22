using System;

[Serializable]
public class CurrencyTransaction : CurrencyData
{
	public string Source
	{
		get;
		set;
	}

	public DateTime Time
	{
		get;
		set;
	}
}
