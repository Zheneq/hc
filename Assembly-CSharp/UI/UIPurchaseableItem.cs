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
		m_itemType = PurchaseItemType.None;
		m_gamePack = null;
		m_lootMatrixPack = null;
		m_ggPack = null;
		m_storeID = -1;
		m_charLink = null;
		m_skinIndex = -1;
		m_textureIndex = -1;
		m_tintIndex = -1;
		m_tauntIndex = -1;
		m_inventoryTemplateId = -1;
		m_currencyType = CurrencyType.ISO;
		m_titleID = -1;
		m_bannerID = -1;
		m_emoticonID = -1;
		m_overconID = -1;
		m_purchaseForCash = false;
		m_overlayText = string.Empty;
	}
}
