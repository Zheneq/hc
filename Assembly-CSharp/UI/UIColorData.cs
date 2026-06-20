using UnityEngine;

public struct UIColorData
{
	public Color m_buttonColor;

	public bool m_isAvailable;

	public bool m_isOwned;

	public bool m_isSkinAvailable;

	public bool m_isVisible;

	public int m_isoCurrencyCost;

	public int m_freelancerCurrencyCost;

	public float m_realCurrencyCost;

	public int m_unlockCharacterLevel;

	public string m_name;

	public string m_description;

	public int m_sortOrder;

	public GameBalanceVars.UnlockData m_unlockData;

	public GameBalanceVars.ColorUnlockData m_colorUnlockable;

	public int m_skinIndex;

	public int m_patternIndex;

	public int m_colorIndex;

	public CharacterType m_characterType;

	public string m_flavorText;

	public int m_requiredLevelForEquip;

	public StyleLevelType m_styleLevelType;

	public InventoryItemRarity m_rarity;
}
