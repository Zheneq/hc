using System;
using UnityEngine;

[Serializable]
public class LootMatrixPack
{
	[HideInInspector]
	public int Index;

	[Multiline]
	public string Description;

	public int NumberOfMatrixes;

	public BonusLootMatrixes[] BonusMatrixes;

	public CountryPrices Prices;

	public string ProductCode;

	public bool IsBundle;

	public bool NonEventHidden;

	public bool EventHidden;

	public string EventStartPacific;

	public string EventEndPacific;

	public int SortOrder;

	public Sprite LootMatrixPackSprite;

	public Sprite EventPackSprite;

	public string EventText;

	public string GetEventText()
	{
		return StringUtil.TR_GetMatrixPackEventText(this.Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_GetMatrixPackDescription(this.Index);
	}

	public bool IsInEvent()
	{
		bool result = false;
		if (!this.EventEndPacific.IsNullOrEmpty())
		{
			if (!this.EventStartPacific.IsNullOrEmpty())
			{
				DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
				DateTime t = Convert.ToDateTime(this.EventStartPacific);
				DateTime t2 = Convert.ToDateTime(this.EventEndPacific);
				bool flag;
				if (lastPacificTimePriceRequestWithServerTimeOffset >= t)
				{
					flag = (lastPacificTimePriceRequestWithServerTimeOffset < t2);
				}
				else
				{
					flag = false;
				}
				result = flag;
			}
		}
		return result;
	}
}
