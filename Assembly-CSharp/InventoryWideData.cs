using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWideData : MonoBehaviour
{
	[Serializable]
	public class LockboxModel
	{
		public int TemplateId;

		public GameObject ModelPrefab;
	}

	private static InventoryWideData s_instance;

	public List<InventoryItemTemplate> m_inventoryItemTemplates;

	public List<LootTable> m_lootTables;

	public List<KarmaTemplate> m_karmaTemplates;

	public LockboxModel[] m_lockboxModels;

	public int m_defaultItemValue;

	public int m_characterItemDropBalance;

	private const string kFallbackSprite = "QuestRewards/general";

	private const string kTauntIconPath = "QuestRewards/taunt";

	private const string kTitleIconPath = "QuestRewards/titleIcon";

	private const string kVfxIconPath = "QuestRewards/vfxicon";

	private const string kModIconPath = "QuestRewards/modicon";

	private const string kSystemIconSprite = "QuestRewards/contract";

	private void Awake()
	{
		s_instance = this;
	}

	public static InventoryWideData Get()
	{
		return s_instance;
	}

	private List<int> GetAllItemTemplateIDsFromLootTable(int lootTableID, List<int> tablesChecked)
	{
		if (tablesChecked.Contains(lootTableID))
		{
			return new List<int>();
		}
		List<int> list = new List<int>();
		foreach (LootTable lootTable in m_lootTables)
		{
			if (lootTableID == lootTable.Index)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				tablesChecked.Add(lootTableID);
				using (List<LootTableEntry>.Enumerator enumerator2 = lootTable.Entries.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LootTableEntry current2 = enumerator2.Current;
						if (current2.Type == LootTableEntryType.InventoryItemTemplate)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(current2.Index);
						}
						else if (current2.Type == LootTableEntryType.LootTable)
						{
							list.AddRange(GetAllItemTemplateIDsFromLootTable(current2.Index, tablesChecked));
						}
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (lootTable.FallbackEntry.Type == LootTableEntryType.LootTable)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					list.AddRange(GetAllItemTemplateIDsFromLootTable(lootTable.FallbackEntry.Index, tablesChecked));
				}
				else if (lootTable.FallbackEntry.Type == LootTableEntryType.InventoryItemTemplate)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(lootTable.FallbackEntry.Index);
				}
				using (List<CheckKarma>.Enumerator enumerator3 = lootTable.CheckKarmas.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						CheckKarma current3 = enumerator3.Current;
						if (current3.KarmaRewardEntry.Type == LootTableEntryType.InventoryItemTemplate)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(current3.KarmaRewardEntry.Index);
						}
						else if (current3.KarmaRewardEntry.Type == LootTableEntryType.LootTable)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							list.AddRange(GetAllItemTemplateIDsFromLootTable(current3.KarmaRewardEntry.Index, tablesChecked));
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		return list;
	}

	public List<int> GetAllItemTemplateIDsFromLootTable(int lootTableID)
	{
		return GetAllItemTemplateIDsFromLootTable(lootTableID, new List<int>()).Distinct().ToList();
	}

	public int GetNextIDForInventoryItem()
	{
		int num = 1;
		using (List<InventoryItemTemplate>.Enumerator enumerator = m_inventoryItemTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItemTemplate current = enumerator.Current;
				if (num <= current.Index)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num = current.Index + 1;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
	}

	public int GetNextIDForLootTable()
	{
		int num = 1;
		using (List<LootTable>.Enumerator enumerator = m_lootTables.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LootTable current = enumerator.Current;
				if (num <= current.Index)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num = current.Index + 1;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
	}

	public int GetNextIDForKarmaTemplate()
	{
		int num = 1;
		using (List<KarmaTemplate>.Enumerator enumerator = m_karmaTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KarmaTemplate current = enumerator.Current;
				if (num <= current.Index)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num = current.Index + 1;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return num;
				}
			}
		}
	}

	private void CheckInventoryItemTemplateIndexes(bool logError)
	{
		int num = GetNextIDForInventoryItem();
		bool flag = false;
		foreach (InventoryItemTemplate inventoryItemTemplate in m_inventoryItemTemplates)
		{
			bool flag2 = false;
			if (inventoryItemTemplate.Index == 0)
			{
				flag2 = true;
			}
			else
			{
				int num2 = 0;
				using (List<InventoryItemTemplate>.Enumerator enumerator2 = m_inventoryItemTemplates.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						InventoryItemTemplate current2 = enumerator2.Current;
						if (current2.Index == inventoryItemTemplate.Index)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							num2++;
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (num2 > 1)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = true;
				}
			}
			if (flag2)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				inventoryItemTemplate.Index = num;
				num++;
			}
			flag = (flag || flag2);
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (logError)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					Debug.LogError("Has bad inventory item ID");
					return;
				}
			}
			return;
		}
	}

	private void CheckLootTableIndexes(bool logError)
	{
		int num = GetNextIDForLootTable();
		bool flag = false;
		using (List<LootTable>.Enumerator enumerator = m_lootTables.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LootTable current = enumerator.Current;
				bool flag2 = false;
				if (current.Index == 0)
				{
					flag2 = true;
				}
				else
				{
					int num2 = 0;
					using (List<LootTable>.Enumerator enumerator2 = m_lootTables.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							LootTable current2 = enumerator2.Current;
							if (current2.Index == current.Index)
							{
								num2++;
							}
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								goto end_IL_004d;
							}
						}
						end_IL_004d:;
					}
					if (num2 > 1)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = true;
					}
				}
				if (flag2)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					current.Index = num;
					num++;
				}
				flag = (flag || flag2);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (flag && logError)
		{
			Debug.LogError("Has bad Loot Table ID");
		}
	}

	private void CheckKarmaTemplateIndexes(bool logError)
	{
		int num = GetNextIDForKarmaTemplate();
		bool flag = false;
		using (List<KarmaTemplate>.Enumerator enumerator = m_karmaTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KarmaTemplate current = enumerator.Current;
				bool flag2 = false;
				if (current.Index == 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					flag2 = true;
				}
				else
				{
					int num2 = 0;
					foreach (KarmaTemplate karmaTemplate in m_karmaTemplates)
					{
						if (karmaTemplate.Index == current.Index)
						{
							num2++;
						}
					}
					if (num2 > 1)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					current.Index = num;
					num++;
				}
				flag = (flag || flag2);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (logError)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					Debug.LogError("Has bad Karma ID");
					return;
				}
			}
			return;
		}
	}

	public void CheckAllIndices(bool logErrorOnBadIndex)
	{
		if (!Application.isEditor)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CheckInventoryItemTemplateIndexes(logErrorOnBadIndex);
			CheckLootTableIndexes(logErrorOnBadIndex);
			CheckKarmaTemplateIndexes(logErrorOnBadIndex);
			return;
		}
	}

	public InventoryItemTemplate GetItemTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		InventoryItemTemplate inventoryItemTemplate = m_inventoryItemTemplates.Find((InventoryItemTemplate i) => i.Index == templateId);
		if (inventoryItemTemplate == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					throw new Exception($"Inventory template item list is malformed: template {templateId} not found.");
				}
			}
		}
		return inventoryItemTemplate;
	}

	public KarmaTemplate GetKarmaTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		KarmaTemplate karmaTemplate = m_karmaTemplates.Find((KarmaTemplate i) => i.Index == templateId);
		if (karmaTemplate == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new Exception($"Karma template list is malformed: template {templateId} not found.");
				}
			}
		}
		return karmaTemplate;
	}

	public LootTable GetLootTable(int tableId)
	{
		if (tableId <= 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		LootTable lootTable = m_lootTables.Find((LootTable l) => l.Index == tableId);
		if (lootTable == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					throw new Exception($"Loot table list is malformed: table {tableId} not found.");
				}
			}
		}
		return lootTable;
	}

	public GameObject GetLockboxPrefab(int templateId)
	{
		for (int i = 0; i < m_lockboxModels.Length; i++)
		{
			if (m_lockboxModels[i].TemplateId != templateId)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return m_lockboxModels[i].ModelPrefab;
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return null;
		}
	}

	public static string TypeDisplayString(InventoryItemTemplate item)
	{
		if (item == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return string.Empty;
				}
			}
		}
		if (item.Type == InventoryItemType.BannerID)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.TypeSpecificData[0]);
					if (banner == null)
					{
						return item.Type.DisplayString();
					}
					if (banner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return StringUtil.TR("Banner", "Rewards");
							}
						}
					}
					return StringUtil.TR("Emblem", "Rewards");
				}
				}
			}
		}
		return item.Type.DisplayString();
	}

	public static bool IsOwned(int itemTemplateId)
	{
		InventoryItemTemplate itemTemplate = Get().GetItemTemplate(itemTemplateId);
		return IsOwned(itemTemplate);
	}

	public static bool IsOwned(InventoryItemTemplate itemTemplate)
	{
		switch (itemTemplate.Type)
		{
		case InventoryItemType.TitleID:
		{
			int num = itemTemplate.TypeSpecificData[0];
			for (int i = 0; i < GameBalanceVars.Get().PlayerTitles.Length; i++)
			{
				if (GameBalanceVars.Get().PlayerTitles[i].ID != num)
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return ClientGameManager.Get().IsTitleUnlocked(GameBalanceVars.Get().PlayerTitles[i]);
				}
			}
			return false;
		}
		case InventoryItemType.BannerID:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(itemTemplate.TypeSpecificData[0]);
			List<GameBalanceVars.UnlockConditionValue> unlockConditionValues;
			return ClientGameManager.Get().IsBannerUnlocked(banner, out unlockConditionValues);
		}
		case InventoryItemType.Skin:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.GetSkin(itemTemplate.TypeSpecificData[1]).Unlocked;
		}
		case InventoryItemType.Texture:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.GetSkin(itemTemplate.TypeSpecificData[1]).GetPattern(itemTemplate.TypeSpecificData[2]).Unlocked;
		}
		case InventoryItemType.Style:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.GetSkin(itemTemplate.TypeSpecificData[1]).GetPattern(itemTemplate.TypeSpecificData[2]).GetColor(itemTemplate.TypeSpecificData[3])
				.Unlocked;
		}
		case InventoryItemType.Taunt:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.GetTaunt(itemTemplate.TypeSpecificData[1]).Unlocked;
		}
		case InventoryItemType.Mod:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.IsModUnlocked(itemTemplate.TypeSpecificData[1], itemTemplate.TypeSpecificData[2]);
		}
		case InventoryItemType.ChatEmoji:
			return ClientGameManager.Get().IsEmojiUnlocked(GameBalanceVars.Get().ChatEmojis[itemTemplate.TypeSpecificData[0] - 1]);
		case InventoryItemType.Overcon:
			return ClientGameManager.Get().IsOverconUnlocked(itemTemplate.TypeSpecificData[0]);
		case InventoryItemType.Lockbox:
		case InventoryItemType.Currency:
		case InventoryItemType.Material:
		case InventoryItemType.Experience:
		case InventoryItemType.Faction:
		case InventoryItemType.Conveyance:
		case InventoryItemType.FreelancerExpBonus:
			return false;
		case InventoryItemType.Unlock:
			if (itemTemplate.TypeSpecificData[0] == 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.DailyQuestsAvailable;
					}
				}
			}
			return false;
		case InventoryItemType.AbilityVfxSwap:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(itemTemplate.TypeSpecificData[1], itemTemplate.TypeSpecificData[2]);
		}
		case InventoryItemType.LoadingScreenBackground:
			return ClientGameManager.Get().IsLoadingScreenBackgroundUnlocked(itemTemplate.TypeSpecificData[0]);
		default:
			throw new Exception("Not implemented, waiting on Sam");
		}
	}

	public static string GetSpritePath(InventoryItemTemplate itemTemplate)
	{
		if (itemTemplate == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return string.Empty;
				}
			}
		}
		if (!itemTemplate.IconPath.IsNullOrEmpty())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!itemTemplate.IconPath.Trim().IsNullOrEmpty())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return itemTemplate.IconPath;
					}
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.BannerID)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(itemTemplate.TypeSpecificData[0]);
			if (banner != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!banner.m_iconResourceString.IsNullOrEmpty())
				{
					return banner.m_iconResourceString;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return "Banners/Background/02_blue";
		}
		if (itemTemplate.Type == InventoryItemType.ChatEmoji)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Overcon)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Faction)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Mod)
		{
			return "QuestRewards/modicon";
		}
		if (itemTemplate.Type == InventoryItemType.Skin)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GameWideData.Get().GetCharacterResourceLink((CharacterType)itemTemplate.TypeSpecificData[0]).m_skins[itemTemplate.TypeSpecificData[1]].m_skinSelectionIconPath;
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Style)
		{
			try
			{
				return GameWideData.Get().GetCharacterResourceLink((CharacterType)itemTemplate.TypeSpecificData[0]).m_skins[itemTemplate.TypeSpecificData[1]].m_patterns[itemTemplate.TypeSpecificData[2]].m_colors[itemTemplate.TypeSpecificData[3]].m_iconResourceString;
			}
			catch (ArgumentOutOfRangeException)
			{
				Log.Error($"The style not found: char {(CharacterType)itemTemplate.TypeSpecificData[0]}, skin {itemTemplate.TypeSpecificData[1]}, pat {itemTemplate.TypeSpecificData[2]}, style {itemTemplate.TypeSpecificData[3]}");
				return "QuestRewards/general";
			}
		}
		if (itemTemplate.Type == InventoryItemType.Taunt)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return "QuestRewards/taunt";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.TitleID)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return "QuestRewards/titleIcon";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Material)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Lockbox)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Currency)
		{
			switch (itemTemplate.TypeSpecificData[0])
			{
			case 3:
				return "QuestRewards/ggPack_01";
			case 0:
				return "QuestRewards/iso_01";
			case 1:
				return "QuestRewards/modToken";
			case 8:
				return "QuestRewards/freelancerCurrency_01";
			default:
				return "QuestRewards/general";
			}
		}
		if (itemTemplate.Type == InventoryItemType.AbilityVfxSwap)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return "QuestRewards/vfxicon";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Experience)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Unlock)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return "QuestRewards/contract";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.Conveyance)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return "QuestRewards/general";
				}
			}
		}
		if (itemTemplate.Type == InventoryItemType.FreelancerExpBonus)
		{
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.LoadingScreenBackground)
		{
			GameBalanceVars.LoadingScreenBackground loadingScreenBackground = GameBalanceVars.Get().GetLoadingScreenBackground(itemTemplate.TypeSpecificData[0]);
			if (loadingScreenBackground != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!loadingScreenBackground.m_iconPath.IsNullOrEmpty())
				{
					return loadingScreenBackground.m_iconPath;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return "QuestRewards/general";
		}
		throw new Exception("Sprite Not Implemented for " + itemTemplate.Type);
	}

	public static string GetTypeString(InventoryItemTemplate template, int count)
	{
		return RewardUtils.GetTypeDisplayString(RewardUtils.GetRewardTypeFromInventoryItem(template), count > 1);
	}

	public static Sprite GetItemFg(InventoryItemTemplate template)
	{
		if (template == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return null;
				}
			}
		}
		AbilityData component;
		AbilityData.ActionType actionType;
		if (template.Type == InventoryItemType.Taunt)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			int index = template.TypeSpecificData[1];
			actionType = characterResourceLink.m_taunts[index].m_actionForTaunt;
		}
		else
		{
			CharacterResourceLink characterResourceLink;
			if (template.Type != InventoryItemType.AbilityVfxSwap)
			{
				if (template.Type == InventoryItemType.Mod)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
							component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
							actionType = (AbilityData.ActionType)template.TypeSpecificData[1];
							Ability ability = null;
							if (actionType == AbilityData.ActionType.ABILITY_0)
							{
								ability = component.m_ability0;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_1)
							{
								ability = component.m_ability1;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_2)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								ability = component.m_ability2;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_3)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								ability = component.m_ability3;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_4)
							{
								ability = component.m_ability4;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_5)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								ability = component.m_ability5;
							}
							else if (actionType == AbilityData.ActionType.ABILITY_6)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								ability = component.m_ability6;
							}
							AbilityMod abilityMod = AbilityModHelper.GetAvailableModsForAbility(ability)[template.TypeSpecificData[2]];
							return abilityMod.m_iconSprite;
						}
						}
					}
				}
				return null;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = (AbilityData.ActionType)template.TypeSpecificData[1];
		}
		if (actionType == AbilityData.ActionType.ABILITY_0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return component.m_sprite0;
				}
			}
		}
		if (actionType == AbilityData.ActionType.ABILITY_1)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return component.m_sprite1;
				}
			}
		}
		if (actionType == AbilityData.ActionType.ABILITY_2)
		{
			return component.m_sprite2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_3)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return component.m_sprite3;
				}
			}
		}
		switch (actionType)
		{
		case AbilityData.ActionType.ABILITY_4:
			return component.m_sprite4;
		case AbilityData.ActionType.ABILITY_5:
			return component.m_sprite5;
		case AbilityData.ActionType.ABILITY_6:
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return component.m_sprite6;
			}
		default:
			return null;
		}
	}

	public static List<InventoryItemTemplate> GetTemplatesFromLootMatrixPack(LootMatrixPack pack)
	{
		List<InventoryItemTemplate> list = new List<InventoryItemTemplate>();
		for (int i = 0; i < pack.BonusMatrixes.Length; i++)
		{
			list.Add(Get().GetItemTemplate(pack.BonusMatrixes[i].LootMatrixId));
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}
}
