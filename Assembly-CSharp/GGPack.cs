using System;
using UnityEngine;

[Serializable]
public class GGPack
{
	[HideInInspector]
	public int Index;

	public int NumberOfBoosts;

	public CountryPrices Prices;

	public string ProductCode;

	public int SortOrder;

	public Sprite GGPackSprite;
}
