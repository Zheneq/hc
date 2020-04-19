using System;

[Serializable]
public class CashShopFeaturedItem
{
	public QuestPrerequisites Prerequisites;

	public PurchaseItemType ItemType;

	public int TypeSpecificData;

	public CharacterType CharacterType;

	public int SkinIndex;

	public int PatternIndex;

	public int SortOrder;

	public string TextOverlay;

	public bool IsCash;
}
