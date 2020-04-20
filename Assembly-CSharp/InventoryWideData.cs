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
				tablesChecked.Add(lootTableID);
				using (List<LootTableEntry>.Enumerator enumerator2 = lootTable.Entries.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						LootTableEntry lootTableEntry = enumerator2.Current;
						if (lootTableEntry.Type == LootTableEntryType.InventoryItemTemplate)
						{
							list.Add(lootTableEntry.Index);
						}
						else if (lootTableEntry.Type == LootTableEntryType.LootTable)
						{
							list.AddRange(this.GetAllItemTemplateIDsFromLootTable(lootTableEntry.Index, tablesChecked));
						}
					}
				}
				if (lootTable.FallbackEntry.Type == LootTableEntryType.LootTable)
				{
					list.AddRange(this.GetAllItemTemplateIDsFromLootTable(lootTable.FallbackEntry.Index, tablesChecked));
				}
				else if (lootTable.FallbackEntry.Type == LootTableEntryType.InventoryItemTemplate)
				{
					list.Add(lootTable.FallbackEntry.Index);
				}
				using (List<CheckKarma>.Enumerator enumerator3 = lootTable.CheckKarmas.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						CheckKarma checkKarma = enumerator3.Current;
						if (checkKarma.KarmaRewardEntry.Type == LootTableEntryType.InventoryItemTemplate)
						{
							list.Add(checkKarma.KarmaRewardEntry.Index);
						}
						else if (checkKarma.KarmaRewardEntry.Type == LootTableEntryType.LootTable)
						{
							list.AddRange(this.GetAllItemTemplateIDsFromLootTable(checkKarma.KarmaRewardEntry.Index, tablesChecked));
						}
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
					num = inventoryItemTemplate.Index + 1;
				}
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
					num = lootTable.Index + 1;
				}
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
					num = karmaTemplate.Index + 1;
				}
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
							num2++;
						}
					}
				}
				if (num2 > 1)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				inventoryItemTemplate.Index = num;
				num++;
			}
			flag = (flag || flag2);
		}
		if (flag)
		{
			if (logError)
			{
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
					}
					if (num2 > 1)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					lootTable.Index = num;
					num++;
				}
				flag = (flag || flag2);
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
		}
		if (flag)
		{
			if (logError)
			{
				Debug.LogError("Has bad Karma ID");
			}
		}
	}

	public void CheckAllIndices(bool logErrorOnBadIndex)
	{
		if (Application.isEditor)
		{
			this.CheckInventoryItemTemplateIndexes(logErrorOnBadIndex);
			this.CheckLootTableIndexes(logErrorOnBadIndex);
			this.CheckKarmaTemplateIndexes(logErrorOnBadIndex);
		}
	}

	public InventoryItemTemplate GetItemTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			return null;
		}
		InventoryItemTemplate inventoryItemTemplate = this.m_inventoryItemTemplates.Find((InventoryItemTemplate i) => i.Index == templateId);
		if (inventoryItemTemplate == null)
		{
			throw new Exception(string.Format("Inventory template item list is malformed: template {0} not found.", templateId));
		}
		return inventoryItemTemplate;
	}

	public KarmaTemplate GetKarmaTemplate(int templateId)
	{
		if (templateId <= 0)
		{
			return null;
		}
		KarmaTemplate karmaTemplate = this.m_karmaTemplates.Find((KarmaTemplate i) => i.Index == templateId);
		if (karmaTemplate == null)
		{
			throw new Exception(string.Format("Karma template list is malformed: template {0} not found.", templateId));
		}
		return karmaTemplate;
	}

	public LootTable GetLootTable(int tableId)
	{
		if (tableId <= 0)
		{
			return null;
		}
		LootTable lootTable = this.m_lootTables.Find((LootTable l) => l.Index == tableId);
		if (lootTable == null)
		{
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
				return this.m_lockboxModels[i].ModelPrefab;
			}
		}
		return null;
	}

	public static string TypeDisplayString(InventoryItemTemplate item)
	{
		if (item == null)
		{
			return string.Empty;
		}
		if (item.Type != InventoryItemType.BannerID)
		{
			return item.Type.DisplayString();
		}
		GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(item.TypeSpecificData[0]);
		if (banner == null)
		{
			return item.Type.DisplayString();
		}
		if (banner.m_type == GameBalanceVars.PlayerBanner.BannerType.Background)
		{
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
			return string.Empty;
		}
		if (!itemTemplate.IconPath.IsNullOrEmpty())
		{
			if (!itemTemplate.IconPath.Trim().IsNullOrEmpty())
			{
				return itemTemplate.IconPath;
			}
		}
		if (itemTemplate.Type == InventoryItemType.BannerID)
		{
			GameBalanceVars.PlayerBanner banner = GameBalanceVars.Get().GetBanner(itemTemplate.TypeSpecificData[0]);
			if (banner != null)
			{
				if (!banner.m_iconResourceString.IsNullOrEmpty())
				{
					return banner.m_iconResourceString;
				}
			}
			return "Banners/Background/02_blue";
		}
		if (itemTemplate.Type == InventoryItemType.ChatEmoji)
		{
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Overcon)
		{
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Faction)
		{
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Mod)
		{
			return "QuestRewards/modicon";
		}
		if (itemTemplate.Type == InventoryItemType.Skin)
		{
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
			return "QuestRewards/taunt";
		}
		if (itemTemplate.Type == InventoryItemType.TitleID)
		{
			return "QuestRewards/titleIcon";
		}
		if (itemTemplate.Type == InventoryItemType.Material)
		{
			return "QuestRewards/general";
		}
		if (itemTemplate.Type == InventoryItemType.Lockbox)
		{
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
				return "QuestRewards/vfxicon";
			}
			if (itemTemplate.Type == InventoryItemType.Experience)
			{
				return "QuestRewards/general";
			}
			if (itemTemplate.Type == InventoryItemType.Unlock)
			{
				return "QuestRewards/contract";
			}
			if (itemTemplate.Type == InventoryItemType.Conveyance)
			{
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
					if (!loadingScreenBackground.m_iconPath.IsNullOrEmpty())
					{
						return loadingScreenBackground.m_iconPath;
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
			return null;
		}
		AbilityData component;
		AbilityData.ActionType actionType;
		if (template.Type == InventoryItemType.Taunt)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			int index = template.TypeSpecificData[1];
			actionType = characterResourceLink.m_taunts[index].m_actionForTaunt;
		}
		else if (template.Type == InventoryItemType.AbilityVfxSwap)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink((CharacterType)template.TypeSpecificData[0]);
			component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
			actionType = (AbilityData.ActionType)template.TypeSpecificData[1];
		}
		else
		{
			if (template.Type == InventoryItemType.Mod)
			{
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
					ability = component.m_ability2;
				}
				else if (actionType == AbilityData.ActionType.ABILITY_3)
				{
					ability = component.m_ability3;
				}
				else if (actionType == AbilityData.ActionType.ABILITY_4)
				{
					ability = component.m_ability4;
				}
				else if (actionType == AbilityData.ActionType.ABILITY_5)
				{
					ability = component.m_ability5;
				}
				else if (actionType == AbilityData.ActionType.ABILITY_6)
				{
					ability = component.m_ability6;
				}
				AbilityMod abilityMod = AbilityModHelper.GetAvailableModsForAbility(ability)[template.TypeSpecificData[2]];
				return abilityMod.m_iconSprite;
			}
			return null;
		}
		if (actionType == AbilityData.ActionType.ABILITY_0)
		{
			return component.m_sprite0;
		}
		if (actionType == AbilityData.ActionType.ABILITY_1)
		{
			return component.m_sprite1;
		}
		if (actionType == AbilityData.ActionType.ABILITY_2)
		{
			return component.m_sprite2;
		}
		if (actionType == AbilityData.ActionType.ABILITY_3)
		{
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
		return list;
	}

	[Serializable]
	public class LockboxModel
	{
		public int TemplateId;

		public GameObject ModelPrefab;
	}
}
