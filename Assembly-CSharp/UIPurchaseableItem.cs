using System;

public class UIPurchaseableItem
{
	public PurchaseItemType m_itemType;

	public GamePack m_gamePack;

	public CharacterResourceLink m_charLink;

	public LootMatrixPack m_lootMatrixPack;

	public GGPack m_ggPack;

	public int m_skinIndex;

	public int m_textureIndex;

	public int m_tintIndex;

	public int m_tauntIndex;

	public int m_storeID;

	public int m_inventoryTemplateId;

	public int m_titleID;

	public int m_bannerID;

	public int m_emoticonID;

	public int m_overconID;

	public int m_abilityID;

	public int m_abilityVfxID;

	public int m_loadingScreenBackgroundId;

	public CurrencyType m_currencyType;

	public bool m_purchaseForCash;

	public string m_overlayText;

	public int m_sortOrder;

	public UIPurchaseableItem()
	{
		this.m_itemType = PurchaseItemType.None;
		this.m_gamePack = null;
		this.m_lootMatrixPack = null;
		this.m_ggPack = null;
		this.m_storeID = -1;
		this.m_charLink = null;
		this.m_skinIndex = -1;
		this.m_textureIndex = -1;
		this.m_tintIndex = -1;
		this.m_tauntIndex = -1;
		this.m_inventoryTemplateId = -1;
		this.m_currencyType = CurrencyType.ISO;
		this.m_titleID = -1;
		this.m_bannerID = -1;
		this.m_emoticonID = -1;
		this.m_overconID = -1;
		this.m_purchaseForCash = false;
		this.m_overlayText = string.Empty;
	}
}
