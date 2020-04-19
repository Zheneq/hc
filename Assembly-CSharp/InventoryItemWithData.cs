using System;

[Serializable]
public class InventoryItemWithData
{
	public InventoryItem Item;

	public int IsoGained;

	public InventoryItemWithData(InventoryItem item, int isoGained)
	{
		this.Item = item;
		this.IsoGained = isoGained;
	}
}
