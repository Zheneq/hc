using System;

[Serializable]
public class LootMatrixPackData
{
	public int LootMatrixInventoryIndexID;
	public LootMatrixPack[] m_lootMatrixPacks;

	public static LootMatrixPackData Get()
	{
		return GameWideData.Get().m_lootMatrixPackData;
	}
}
