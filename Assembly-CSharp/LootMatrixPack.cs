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
		return StringUtil.TR_GetMatrixPackEventText(Index);
	}

	public string GetDescription()
	{
		return StringUtil.TR_GetMatrixPackDescription(Index);
	}

	public bool IsInEvent()
	{
		bool result = false;
		if (!EventEndPacific.IsNullOrEmpty())
		{
			if (!EventStartPacific.IsNullOrEmpty())
			{
				DateTime lastPacificTimePriceRequestWithServerTimeOffset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
				DateTime t = Convert.ToDateTime(EventStartPacific);
				DateTime t2 = Convert.ToDateTime(EventEndPacific);
				int num;
				if (lastPacificTimePriceRequestWithServerTimeOffset >= t)
				{
					num = ((lastPacificTimePriceRequestWithServerTimeOffset < t2) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				result = ((byte)num != 0);
			}
		}
		return result;
	}
}
