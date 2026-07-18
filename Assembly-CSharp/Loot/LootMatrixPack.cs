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
		if (EventEndPacific.IsNullOrEmpty() || EventStartPacific.IsNullOrEmpty())
		{
			return false;
		}
		
		DateTime offset = CommerceClient.Get().LastPacificTimePriceRequestWithServerTimeOffset;
		return offset >= Convert.ToDateTime(EventStartPacific)
		       && offset < Convert.ToDateTime(EventEndPacific);
	}
}
