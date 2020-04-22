using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixContentViewer : UIScene
{
	[Serializable]
	public class PrefabContainerIndexPair
	{
		public int LootTableIndex;

		public RectTransform PreviewObject;
	}

	public RectTransform m_Container;

	public _SelectableBtn m_OKBtn;

	public TextMeshProUGUI m_Title;

	public TextMeshProUGUI m_DescriptionOne;

	public TextMeshProUGUI m_DescriptionTwo;

	public UILockboxRewardItem m_LootMatrixRewardIconPrefab;

	public LayoutGroup m_RareItemLayoutGroupOne;

	public LayoutGroup m_RareItemLayoutGroupTwo;

	public LayoutGroup m_BonusItemLayoutGroup;

	public LootMatrixThermostat m_thermoStat;

	public _SelectableBtn m_previousPageBtn;

	public _SelectableBtn m_nextPageBtn;

	public PrefabContainerIndexPair[] LootTablePreviewMatches;

	public List<UILockboxRewardItem> RareItemsListRowOne = new List<UILockboxRewardItem>();

	public List<UILockboxRewardItem> RareItemsListRowTwo = new List<UILockboxRewardItem>();

	public List<UILockboxRewardItem> BonusItems = new List<UILockboxRewardItem>();

	private List<InventoryItemTemplate> LootMatrixItemTemplates;

	private int CurrentPage = -1;

	private const int MAX_PER_ROW = 12;

	private static UILootMatrixContentViewer s_instance;

	public static UILootMatrixContentViewer Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		SetVisible(false);
		m_OKBtn.spriteController.callback = delegate
		{
			SetVisible(false);
		};
		m_previousPageBtn.spriteController.callback = PreviousPageClicked;
		m_nextPageBtn.spriteController.callback = NextPageClicked;
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LootMatrixContentViewer;
	}

	public void Setup(InventoryItemTemplate Template, bool IsBundle = false)
	{
		Setup(new InventoryItemTemplate[1]
		{
			Template
		}, IsBundle);
	}

	public void Setup(InventoryItemTemplate[] Templates, bool IsBundle = false)
	{
		List<InventoryItemTemplate> list = new List<InventoryItemTemplate>();
		for (int i = 0; i < Templates.Length; i++)
		{
			if (Templates[i] != null)
			{
				if (Templates[i].Type == InventoryItemType.Lockbox)
				{
					list.Add(Templates[i]);
				}
			}
		}
		if (list.Count == 0)
		{
			Log.Error("Opening Loot Matrix Content Viewer with no valid item templates");
			return;
		}
		LootMatrixItemTemplates = list;
		UIManager.SetGameObjectActive(m_Title, !IsBundle);
		UIManager.SetGameObjectActive(m_DescriptionOne, !IsBundle);
		UIManager.SetGameObjectActive(m_DescriptionTwo, !IsBundle);
	}

	private void DisplayItems(List<LootTable> Tables)
	{
		if (Tables == null)
		{
			return;
		}
		while (true)
		{
			if (Tables.Count == 0)
			{
				return;
			}
			while (true)
			{
				if (Tables[0] == null)
				{
					return;
				}
				for (int i = 0; i < LootTablePreviewMatches.Length; i++)
				{
					UIManager.SetGameObjectActive(LootTablePreviewMatches[i].PreviewObject, LootTablePreviewMatches[i].LootTableIndex == Tables[0].Index);
				}
				List<int> list = new List<int>();
				for (int j = 0; j < Tables.Count; j++)
				{
					list.AddRange(InventoryWideData.Get().GetAllItemTemplateIDsFromLootTable(Tables[j].Index));
				}
				while (true)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					for (int num4 = 0; num4 < list.Count; num4++)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(list[num4]);
						UILockboxRewardItem uILockboxRewardItem = null;
						if (InventoryItemRarity.Rare <= itemTemplate.Rarity)
						{
							if (itemTemplate.Rarity <= InventoryItemRarity.Epic)
							{
								if (num < 12)
								{
									while (num >= RareItemsListRowOne.Count)
									{
										UILockboxRewardItem uILockboxRewardItem2 = UnityEngine.Object.Instantiate(m_LootMatrixRewardIconPrefab);
										UIManager.ReparentTransform(uILockboxRewardItem2.transform, m_RareItemLayoutGroupOne.transform);
										RareItemsListRowOne.Add(uILockboxRewardItem2);
									}
									uILockboxRewardItem = RareItemsListRowOne[num];
									num++;
								}
								else if (num2 < 12)
								{
									while (num2 >= RareItemsListRowTwo.Count)
									{
										UILockboxRewardItem uILockboxRewardItem3 = UnityEngine.Object.Instantiate(m_LootMatrixRewardIconPrefab);
										UIManager.ReparentTransform(uILockboxRewardItem3.transform, m_RareItemLayoutGroupTwo.transform);
										RareItemsListRowTwo.Add(uILockboxRewardItem3);
									}
									uILockboxRewardItem = RareItemsListRowTwo[num2];
									num2++;
								}
								goto IL_0291;
							}
						}
						if (itemTemplate.Rarity == InventoryItemRarity.Legendary)
						{
							if (num3 < 12)
							{
								while (num3 >= BonusItems.Count)
								{
									UILockboxRewardItem uILockboxRewardItem4 = UnityEngine.Object.Instantiate(m_LootMatrixRewardIconPrefab);
									UIManager.ReparentTransform(uILockboxRewardItem4.transform, m_BonusItemLayoutGroup.transform);
									BonusItems.Add(uILockboxRewardItem4);
								}
								uILockboxRewardItem = BonusItems[num3];
								num3++;
							}
						}
						goto IL_0291;
						IL_0291:
						if (uILockboxRewardItem != null)
						{
							bool isDuplicate = false;
							if (ClientGameManager.Get() != null)
							{
								if (ClientGameManager.Get().GetPlayerAccountData() != null)
								{
									if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
									{
										int num5;
										if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(list[num4]))
										{
											num5 = (InventoryWideData.IsOwned(itemTemplate) ? 1 : 0);
										}
										else
										{
											num5 = 1;
										}
										isDuplicate = ((byte)num5 != 0);
									}
								}
							}
							UIManager.SetGameObjectActive(uILockboxRewardItem, true);
							uILockboxRewardItem.Setup(new InventoryItem(), itemTemplate, isDuplicate, -1);
							if (uILockboxRewardItem.m_rewardFgs != null)
							{
								Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
								for (int k = 0; k < uILockboxRewardItem.m_rewardFgs.Length; k++)
								{
									uILockboxRewardItem.m_rewardFgs[k].sprite = itemFg;
									UIManager.SetGameObjectActive(uILockboxRewardItem.m_rewardFgs[k], itemFg != null);
								}
							}
						}
					}
					for (int l = num; l < RareItemsListRowOne.Count; l++)
					{
						UIManager.SetGameObjectActive(RareItemsListRowOne[l], false);
					}
					while (true)
					{
						for (int m = num2; m < RareItemsListRowTwo.Count; m++)
						{
							UIManager.SetGameObjectActive(RareItemsListRowTwo[m], false);
						}
						while (true)
						{
							for (int n = num3; n < BonusItems.Count; n++)
							{
								UIManager.SetGameObjectActive(BonusItems[n], false);
							}
							while (true)
							{
								switch (7)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	private void UpdateButtons()
	{
		bool doActive = true;
		bool doActive2 = true;
		if (CurrentPage != -1)
		{
			if (LootMatrixItemTemplates.Count != 0)
			{
				goto IL_003d;
			}
		}
		doActive = false;
		doActive2 = false;
		goto IL_003d;
		IL_003d:
		if (CurrentPage == 0)
		{
			doActive = false;
		}
		if (CurrentPage == LootMatrixItemTemplates.Count - 1)
		{
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(m_previousPageBtn, doActive);
		UIManager.SetGameObjectActive(m_nextPageBtn, doActive2);
	}

	public void PreviousPageClicked(BaseEventData data)
	{
		DisplayPage(CurrentPage - 1);
	}

	public void NextPageClicked(BaseEventData data)
	{
		DisplayPage(CurrentPage + 1);
	}

	public void DisplayPage(int pageIndex)
	{
		int num = Mathf.Clamp(pageIndex, 0, LootMatrixItemTemplates.Count);
		if (num < LootMatrixItemTemplates.Count)
		{
			CurrentPage = num;
			LootTable lootTable = InventoryWideData.Get().GetLootTable(LootMatrixItemTemplates[num].TypeSpecificData[0]);
			List<LootTable> list = new List<LootTable>();
			list.Add(lootTable);
			DisplayItems(list);
			InventoryItem itemByTemplateId = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(LootMatrixItemTemplates[num].Index);
			List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
			List<int> list2 = new List<int>();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].TemplateId == LootMatrixItemTemplates[num].Index)
				{
					list2.Add(items[i].Id);
				}
			}
			m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, itemByTemplateId, LootMatrixItemTemplates[num], list2);
		}
		UpdateButtons();
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_Container, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			DisplayPage(0);
			return;
		}
	}
}
