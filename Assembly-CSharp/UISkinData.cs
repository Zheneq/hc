using System;
using UnityEngine;

public struct UISkinData
{
	public UIPatternData[] m_possiblePatterns;

	public bool m_isAvailable;

	public bool m_isVisible;

	public int m_gameCurrencyCost;

	public float m_progressPct;

	public int m_patternsUnlocked;

	public int m_unlockCharacterLevel;

	public Sprite m_skinImage;

	public int m_defaultPatternIndexForSkin;

	public int m_defaultColorIndexForSkin;

	public GameBalanceVars.UnlockData m_unlockData;

	public int m_skinIndex;

	public CharacterType m_characterType;

	public string m_flavorText;
}
