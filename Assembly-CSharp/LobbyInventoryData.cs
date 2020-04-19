using System;
using System.Collections.Generic;

[Serializable]
public class LobbyInventoryData
{
	public Dictionary<int, LobbyInventoryItemTemplate> ItemTemplates;

	public KarmaTemplate[] KarmaTemplates;

	public LootTable[] LootTables;

	public int DefaultItemValue;

	public int CharacterItemDropBalance;
}
