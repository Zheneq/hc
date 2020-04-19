using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWideData : MonoBehaviour
{
	private static InventoryWideData s_instance;

	public List<InventoryItemTemplate> m_inventoryItemTemplates;

	public List<LootTable> m_lootTables;

	public List<KarmaTemplate> m_karmaTemplates;

	public InventoryWideData.LockboxModel[] m_lockboxModels;

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
		InventoryWideData.s_instance = this;
	}

	public static InventoryWideData Get()
	{
		return InventoryWideData.s_instance;
	}

	private List<int> GetAllItemTemplateIDsFromLootTable(int lootTableID, List<int> tablesChecked)
	{
		if (tablesChecked.Contains(lootTableID))
		{
			return new List<int>();
		}
		List<int> list = new List<int>();
		foreach (LootTable lootTable in this.m_lootTables)
		{
			if (lootTableID == lootTable.Index)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetAllItemTemplateIDsFromLootTable(int, List<int>)).MethodHandle;
				}
				tablesChecked.Add(lootTableID);
				using (List<LootTableEntry>.Enumerator enumerator2 = lootTable.Entries.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LootTableEntry lootTableEntry = enumerator2.Current;
						if (lootTableEntry.Type == LootTableEntryType.InventoryItemTemplate)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(lootTableEntry.Index);
						}
						else if (lootTableEntry.Type == LootTableEntryType.LootTable)
						{
							list.AddRange(this.GetAllItemTemplateIDsFromLootTable(lootTableEntry.Index, tablesChecked));
						}
					}
					for (;;)
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					list.AddRange(this.GetAllItemTemplateIDsFromLootTable(lootTable.FallbackEntry.Index, tablesChecked));
				}
				else if (lootTable.FallbackEntry.Type == LootTableEntryType.InventoryItemTemplate)
				{
					for (;;)
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
						CheckKarma checkKarma = enumerator3.Current;
						if (checkKarma.KarmaRewardEntry.Type == LootTableEntryType.InventoryItemTemplate)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(checkKarma.KarmaRewardEntry.Index);
						}
						else if (checkKarma.KarmaRewardEntry.Type == LootTableEntryType.LootTable)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							list.AddRange(this.GetAllItemTemplateIDsFromLootTable(checkKarma.KarmaRewardEntry.Index, tablesChecked));
						}
					}
					for (;;)
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
		return this.GetAllItemTemplateIDsFromLootTable(lootTableID, new List<int>()).Distinct<int>().ToList<int>();
	}

	public int GetNextIDForInventoryItem()
	{
		int num = 1;
		using (List<InventoryItemTemplate>.Enumerator enumerator = this.m_inventoryItemTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItemTemplate inventoryItemTemplate = enumerator.Current;
				if (num <= inventoryItemTemplate.Index)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetNextIDForInventoryItem()).MethodHandle;
					}
					num = inventoryItemTemplate.Index + 1;
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	public int GetNextIDForLootTable()
	{
		int num = 1;
		using (List<LootTable>.Enumerator enumerator = this.m_lootTables.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LootTable lootTable = enumerator.Current;
				if (num <= lootTable.Index)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetNextIDForLootTable()).MethodHandle;
					}
					num = lootTable.Index + 1;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	public int GetNextIDForKarmaTemplate()
	{
		int num = 1;
		using (List<KarmaTemplate>.Enumerator enumerator = this.m_karmaTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KarmaTemplate karmaTemplate = enumerator.Current;
				if (num <= karmaTemplate.Index)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetNextIDForKarmaTemplate()).MethodHandle;
					}
					num = karmaTemplate.Index + 1;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}

	private void CheckInventoryItemTemplateIndexes(bool logError)
	{
		int num = this.GetNextIDForInventoryItem();
		bool flag = false;
		foreach (InventoryItemTemplate inventoryItemTemplate in this.m_inventoryItemTemplates)
		{
			bool flag2 = false;
			if (inventoryItemTemplate.Index == 0)
			{
				flag2 = true;
			}
			else
			{
				int num2 = 0;
				using (List<InventoryItemTemplate>.Enumerator enumerator2 = this.m_inventoryItemTemplates.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						InventoryItemTemplate inventoryItemTemplate2 = enumerator2.Current;
						if (inventoryItemTemplate2.Index == inventoryItemTemplate.Index)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.CheckInventoryItemTemplateIndexes(bool)).MethodHandle;
							}
							num2++;
						}
					}
					for (;;)
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
					for (;;)
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
				for (;;)
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
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (logError)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogError("Has bad inventory item ID");
			}
		}
	}

	private void CheckLootTableIndexes(bool logError)
	{
		int num = this.GetNextIDForLootTable();
		bool flag = false;
		using (List<LootTable>.Enumerator enumerator = this.m_lootTables.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				LootTable lootTable = enumerator.Current;
				bool flag2 = false;
				if (lootTable.Index == 0)
				{
					flag2 = true;
				}
				else
				{
					int num2 = 0;
					using (List<LootTable>.Enumerator enumerator2 = this.m_lootTables.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							LootTable lootTable2 = enumerator2.Current;
							if (lootTable2.Index == lootTable.Index)
							{
								num2++;
							}
						}
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.CheckLootTableIndexes(bool)).MethodHandle;
						}
					}
					if (num2 > 1)
					{
						for (;;)
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					lootTable.Index = num;
					num++;
				}
				flag = (flag || flag2);
			}
			for (;;)
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
		int num = this.GetNextIDForKarmaTemplate();
		bool flag = false;
		using (List<KarmaTemplate>.Enumerator enumerator = this.m_karmaTemplates.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KarmaTemplate karmaTemplate = enumerator.Current;
				bool flag2 = false;
				if (karmaTemplate.Index == 0)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.CheckKarmaTemplateIndexes(bool)).MethodHandle;
					}
					flag2 = true;
				}
				else
				{
					int num2 = 0;
					foreach (KarmaTemplate karmaTemplate2 in this.m_karmaTemplates)
					{
						if (karmaTemplate2.Index == karmaTemplate.Index)
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
					karmaTemplate.Index = num;
					num++;
				}
				flag = (flag || flag2);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (logError)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogError("Has bad Karma ID");
			}
		}
	}

	public void CheckAllIndices(bool logErrorOnBadIndex)
	{
		if (Application.isEditor)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.CheckAllIndices(bool)).MethodHandle;
			}
			this.CheckInventoryItemTemplateIndexes(logErrorOnBadIndex);
			this.CheckLootTableIndexes(logErrorOnBadIndex);
			this.CheckKarmaTemplateIndexes(logErrorOnBadIndex);
		}
	}

	public InventoryItemTemplate GetItemTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetItemTemplate(int)).MethodHandle;
			}
			return null;
		}
		InventoryItemTemplate inventoryItemTemplate = this.m_inventoryItemTemplates.Find((InventoryItemTemplate i) => i.Index == templateId);
		if (inventoryItemTemplate == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			throw new Exception(string.Format("Inventory template item list is malformed: template {0} not found.", templateId));
		}
		return inventoryItemTemplate;
	}

	public KarmaTemplate GetKarmaTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetKarmaTemplate(int)).MethodHandle;
			}
			return null;
		}
		KarmaTemplate karmaTemplate = this.m_karmaTemplates.Find((KarmaTemplate i) => i.Index == templateId);
		if (karmaTemplate == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			throw new Exception(string.Format("Karma template list is malformed: template {0} not found.", templateId));
		}
		return karmaTemplate;
	}

	public LootTable GetLootTable(int tableId)
	{
		if (tableId <= 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetLootTable(int)).MethodHandle;
			}
			return null;
		}
		LootTable lootTable = this.m_lootTables.Find((LootTable l) => l.Index == tableId);
		if (lootTable == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			throw new Exception(string.Format("Loot table list is malformed: table {0} not found.", tableId));
		}
		return lootTable;
	}

	public GameObject GetLockboxPrefab(int templateId)
	{
		for (int i = 0; i < this.m_lockboxModels.Length; i++)
		{
			if (this.m_lockboxModels[i].TemplateId == templateId)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetLockboxPrefab(int)).MethodHandle;
				}
				return this.m_lockboxModels[i].ModelPrefab;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	public static string TypeDisplayString(InventoryItemTemplate item)
	{
		if (item == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.TypeDisplayString(InventoryItemTemplate)).MethodHandle;
			}
			return string.Empty;
		}
		if (item.Type != InventoryItemType.BannerID)
		{
			return item.Type.DisplayString();
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.TypeSpecificData[0]);
		if (banner == null)
		{
			return item.Type.DisplayString();
		}
		if (banner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return StringUtil.TR("Banner", "Rewards");
		}
		return StringUtil.TR("Emblem", "Rewards");
	}

	public static bool IsOwned(int itemTemplateId)
	{
		InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemTemplateId);
		return InventoryWideData.IsOwned(itemTemplate);
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
				if (GameBalanceVars.Get().PlayerTitles[i].ID == num)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.IsOwned(InventoryItemTemplate)).MethodHandle;
					}
					return ClientGameManager.Get().IsTitleUnlocked(GameBalanceVars.Get().PlayerTitles[i]);
				}
			}
			return false;
		}
		case InventoryItemType.BannerID:
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(itemTemplate.TypeSpecificData[0]);
			List<GameBalanceVars.UnlockConditionValue> list;
			return ClientGameManager.Get().IsBannerUnlocked(banner, out list);
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
			return playerCharacterData.CharacterComponent.GetSkin(itemTemplate.TypeSpecificData[1]).GetPattern(itemTemplate.TypeSpecificData[2]).GetColor(itemTemplate.TypeSpecificData[3]).Unlocked;
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
		case InventoryItemType.Lockbox:
		case InventoryItemType.Currency:
		case InventoryItemType.Material:
		case InventoryItemType.Experience:
		case InventoryItemType.Faction:
		case InventoryItemType.Conveyance:
		case InventoryItemType.FreelancerExpBonus:
			return false;
		case InventoryItemType.ChatEmoji:
			return ClientGameManager.Get().IsEmojiUnlocked(GameBalanceVars.Get().ChatEmojis[itemTemplate.TypeSpecificData[0] - 1]);
		case InventoryItemType.AbilityVfxSwap:
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData((CharacterType)itemTemplate.TypeSpecificData[0]);
			return playerCharacterData.CharacterComponent.IsAbilityVfxSwapUnlocked(itemTemplate.TypeSpecificData[1], itemTemplate.TypeSpecificData[2]);
		}
		case InventoryItemType.Overcon:
			return ClientGameManager.Get().IsOverconUnlocked(itemTemplate.TypeSpecificData[0]);
		case InventoryItemType.Unlock:
			if (itemTemplate.TypeSpecificData[0] == 0)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return ClientGameManager.Get().GetPlayerAccountData().AccountComponent.DailyQuestsAvailable;
			}
			return false;
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetSpritePath(InventoryItemTemplate)).MethodHandle;
			}
			return string.Empty;
		}
		if (!itemTemplate.IconPath.IsNullOrEmpty())
		{
			for (;;)
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return itemTemplate.IconPath;
			}
		}
		if (itemTemplate.Type == InventoryItemType.BannerID)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(itemTemplate.TypeSpecificData[0]);
			if (banner != null)
			{
				for (;;)
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
				for (;;)
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Overcon)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Faction)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Mod)
		{
			return "QuestRewards/modicon";
		}
		if (itemTemplate.Type == InventoryItemType.Skin)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return GameWideData.Get().GetCharacterResourceLink((CharacterType)itemTemplate.TypeSpecificData[0]).m_skins[itemTemplate.TypeSpecificData[1]].m_skinSelectionIconPath;
		}
		if (itemTemplate.Type == InventoryItemType.Style)
		{
			try
			{
				return GameWideData.Get().GetCharacterResourceLink((CharacterType)itemTemplate.TypeSpecificData[0]).m_skins[itemTemplate.TypeSpecificData[1]].m_patterns[itemTemplate.TypeSpecificData[2]].m_colors[itemTemplate.TypeSpecificData[3]].m_iconResourceString;
			}
			catch (ArgumentOutOfRangeException)
			{
				Log.Error(string.Format("The style not found: char {0}, skin {1}, pat {2}, style {3}", new object[]
				{
					(CharacterType)itemTemplate.TypeSpecificData[0],
					itemTemplate.TypeSpecificData[1],
					itemTemplate.TypeSpecificData[2],
					itemTemplate.TypeSpecificData[3]
				}), new object[0]);
				return "QuestRewards/general";
			}
		}
		if (itemTemplate.Type == InventoryItemType.Taunt)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/taunt";
		}
		if (itemTemplate.Type == InventoryItemType.TitleID)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/titleIcon";
		}
		if (itemTemplate.Type == InventoryItemType.Material)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Lockbox)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Currency)
		{
			CurrencyType currencyType = (CurrencyType)itemTemplate.TypeSpecificData[0];
			switch (currencyType)
			{
			case CurrencyType.ISO:
				return "QuestRewards/iso_01";
			case CurrencyType.ModToken:
				return "QuestRewards/modToken";
			default:
				if (currencyType != CurrencyType.FreelancerCurrency)
				{
					return "QuestRewards/general";
				}
				return "QuestRewards/freelancerCurrency_01";
			case CurrencyType.GGPack:
				return "QuestRewards/ggPack_01";
			}
		}
		else
		{
			if (itemTemplate.Type == InventoryItemType.AbilityVfxSwap)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return "QuestRewards/vfxicon";
			}
			if (itemTemplate.Type == InventoryItemType.Experience)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return "QuestRewards/general";
			}
			if (itemTemplate.Type == InventoryItemType.Unlock)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return "QuestRewards/contract";
			}
			if (itemTemplate.Type == InventoryItemType.Conveyance)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return "QuestRewards/general";
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
					for (;;)
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
					for (;;)
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
	}

	public static string GetTypeString(InventoryItemTemplate template, int count)
	{
		return RewardUtils.GetTypeDisplayString(RewardUtils.GetRewardTypeFromInventoryItem(template), count > 1);
	}

	public static Sprite GetItemFg(InventoryItemTemplate template)
	{
		if (template == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetItemFg(InventoryItemTemplate)).MethodHandle;
			}
			return null;
		}
		AbilityData component;
		AbilityData.ActionType actionType;
		if (template.Type == InventoryItemType.Taunt)
		{
			for (;;)
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
		else if (template.Type == InventoryItemType.AbilityVfxSwap)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = (AbilityData.ActionType)template.TypeSpecificData[1];
		}
		else
		{
			if (template.Type == InventoryItemType.Mod)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
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
					for (;;)
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
					for (;;)
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
					for (;;)
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
					for (;;)
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
			return null;
		}
		if (actionType == AbilityData.ActionType.ABILITY_0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return component.m_sprite0;
		}
		if (actionType == AbilityData.ActionType.ABILITY_1)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return component.m_sprite1;
		}
		if (actionType == AbilityData.ActionType.ABILITY_2)
		{
			return component.m_sprite2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_3)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return component.m_sprite3;
		}
		if (actionType == AbilityData.ActionType.ABILITY_4)
		{
			return component.m_sprite4;
		}
		if (actionType == AbilityData.ActionType.ABILITY_5)
		{
			return component.m_sprite5;
		}
		if (actionType == AbilityData.ActionType.ABILITY_6)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return component.m_sprite6;
		}
		return null;
	}

	public static List<InventoryItemTemplate> GetTemplatesFromLootMatrixPack(LootMatrixPack pack)
	{
		List<InventoryItemTemplate> list = new List<InventoryItemTemplate>();
		for (int i = 0; i < pack.BonusMatrixes.Length; i++)
		{
			list.Add(InventoryWideData.Get().GetItemTemplate(pack.BonusMatrixes[i].LootMatrixId));
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(InventoryWideData.GetTemplatesFromLootMatrixPack(LootMatrixPack)).MethodHandle;
		}
		return list;
	}

	[Serializable]
	public class LockboxModel
	{
		public int TemplateId;

		public GameObject ModelPrefab;
	}
}
