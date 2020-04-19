using System;
using UnityEngine;

public struct UIPatternData
{
	public UIColorData[] m_possibleColors;

	public Color m_buttonColor;

	public int m_textureIndex;

	public bool m_isAvailable;

	public bool m_isVisible;

	public int m_gameCurrencyCost;

	public int m_colorsUnlocked;

	public int m_unlockCharacterLevel;

	public GameBalanceVars.UnlockData m_unlockData;
}
