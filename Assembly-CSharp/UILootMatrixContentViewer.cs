using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixContentViewer : UIScene
{
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

	public UILootMatrixContentViewer.PrefabContainerIndexPair[] LootTablePreviewMatches;

	public List<UILockboxRewardItem> RareItemsListRowOne = new List<UILockboxRewardItem>();

	public List<UILockboxRewardItem> RareItemsListRowTwo = new List<UILockboxRewardItem>();

	public List<UILockboxRewardItem> BonusItems = new List<UILockboxRewardItem>();

	private List<InventoryItemTemplate> LootMatrixItemTemplates;

	private int CurrentPage = -1;

	private const int MAX_PER_ROW = 0xC;

	private static UILootMatrixContentViewer s_instance;

	public static UILootMatrixContentViewer Get()
	{
		return UILootMatrixContentViewer.s_instance;
	}

	public override void Awake()
	{
		UILootMatrixContentViewer.s_instance = this;
		this.SetVisible(false);
		this.m_OKBtn.spriteController.callback = delegate(BaseEventData data)
		{
			this.SetVisible(false);
		};
		this.m_previousPageBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PreviousPageClicked);
		this.m_nextPageBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NextPageClicked);
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LootMatrixContentViewer;
	}

	public void Setup(InventoryItemTemplate Template, bool IsBundle = false)
	{
		this.Setup(new InventoryItemTemplate[]
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixContentViewer.Setup(InventoryItemTemplate[], bool)).MethodHandle;
				}
				if (Templates[i].Type == InventoryItemType.Lockbox)
				{
					list.Add(Templates[i]);
				}
			}
		}
		if (list.Count == 0)
		{
			Log.Error("Opening Loot Matrix Content Viewer with no valid item templates", new object[0]);
			return;
		}
		this.LootMatrixItemTemplates = list;
		UIManager.SetGameObjectActive(this.m_Title, !IsBundle, null);
		UIManager.SetGameObjectActive(this.m_DescriptionOne, !IsBundle, null);
		UIManager.SetGameObjectActive(this.m_DescriptionTwo, !IsBundle, null);
	}

	private void DisplayItems(List<LootTable> Tables)
	{
		if (Tables != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixContentViewer.DisplayItems(List<LootTable>)).MethodHandle;
			}
			if (Tables.Count != 0)
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
				if (Tables[0] != null)
				{
					for (int i = 0; i < this.LootTablePreviewMatches.Length; i++)
					{
						UIManager.SetGameObjectActive(this.LootTablePreviewMatches[i].PreviewObject, this.LootTablePreviewMatches[i].LootTableIndex == Tables[0].Index, null);
					}
					List<int> list = new List<int>();
					for (int j = 0; j < Tables.Count; j++)
					{
						list.AddRange(InventoryWideData.Get().GetAllItemTemplateIDsFromLootTable(Tables[j].Index));
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
					int k = 0;
					int l = 0;
					int m = 0;
					int n = 0;
					while (n < list.Count)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(list[n]);
						UILockboxRewardItem uilockboxRewardItem = null;
						if (InventoryItemRarity.Rare > itemTemplate.Rarity)
						{
							goto IL_1FF;
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
						if (itemTemplate.Rarity > InventoryItemRarity.Epic)
						{
							goto IL_1FF;
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
						if (k < 0xC)
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
							while (k >= this.RareItemsListRowOne.Count)
							{
								UILockboxRewardItem uilockboxRewardItem2 = UnityEngine.Object.Instantiate<UILockboxRewardItem>(this.m_LootMatrixRewardIconPrefab);
								UIManager.ReparentTransform(uilockboxRewardItem2.transform, this.m_RareItemLayoutGroupOne.transform);
								this.RareItemsListRowOne.Add(uilockboxRewardItem2);
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
							uilockboxRewardItem = this.RareItemsListRowOne[k];
							k++;
						}
						else if (l < 0xC)
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
							while (l >= this.RareItemsListRowTwo.Count)
							{
								UILockboxRewardItem uilockboxRewardItem3 = UnityEngine.Object.Instantiate<UILockboxRewardItem>(this.m_LootMatrixRewardIconPrefab);
								UIManager.ReparentTransform(uilockboxRewardItem3.transform, this.m_RareItemLayoutGroupTwo.transform);
								this.RareItemsListRowTwo.Add(uilockboxRewardItem3);
							}
							uilockboxRewardItem = this.RareItemsListRowTwo[l];
							l++;
						}
						IL_291:
						if (uilockboxRewardItem != null)
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
							bool isDuplicate = false;
							if (ClientGameManager.Get() != null)
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
								if (ClientGameManager.Get().GetPlayerAccountData() != null)
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
									if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
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
										bool flag;
										if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(list[n]))
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
											flag = InventoryWideData.IsOwned(itemTemplate);
										}
										else
										{
											flag = true;
										}
										isDuplicate = flag;
									}
								}
							}
							UIManager.SetGameObjectActive(uilockboxRewardItem, true, null);
							uilockboxRewardItem.Setup(new InventoryItem(), itemTemplate, isDuplicate, -1);
							if (uilockboxRewardItem.m_rewardFgs != null)
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
								Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
								for (int num = 0; num < uilockboxRewardItem.m_rewardFgs.Length; num++)
								{
									uilockboxRewardItem.m_rewardFgs[num].sprite = itemFg;
									UIManager.SetGameObjectActive(uilockboxRewardItem.m_rewardFgs[num], itemFg != null, null);
								}
							}
						}
						n++;
						continue;
						IL_1FF:
						if (itemTemplate.Rarity != InventoryItemRarity.Legendary)
						{
							goto IL_291;
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m < 0xC)
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
							while (m >= this.BonusItems.Count)
							{
								UILockboxRewardItem uilockboxRewardItem4 = UnityEngine.Object.Instantiate<UILockboxRewardItem>(this.m_LootMatrixRewardIconPrefab);
								UIManager.ReparentTransform(uilockboxRewardItem4.transform, this.m_BonusItemLayoutGroup.transform);
								this.BonusItems.Add(uilockboxRewardItem4);
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
							uilockboxRewardItem = this.BonusItems[m];
							m++;
							goto IL_291;
						}
						goto IL_291;
					}
					for (int num2 = k; num2 < this.RareItemsListRowOne.Count; num2++)
					{
						UIManager.SetGameObjectActive(this.RareItemsListRowOne[num2], false, null);
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
					for (int num3 = l; num3 < this.RareItemsListRowTwo.Count; num3++)
					{
						UIManager.SetGameObjectActive(this.RareItemsListRowTwo[num3], false, null);
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
					for (int num4 = m; num4 < this.BonusItems.Count; num4++)
					{
						UIManager.SetGameObjectActive(this.BonusItems[num4], false, null);
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
					return;
				}
			}
		}
	}

	private void UpdateButtons()
	{
		bool doActive = true;
		bool doActive2 = true;
		if (this.CurrentPage != -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixContentViewer.UpdateButtons()).MethodHandle;
			}
			if (this.LootMatrixItemTemplates.Count != 0)
			{
				goto IL_3D;
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
		}
		doActive = false;
		doActive2 = false;
		IL_3D:
		if (this.CurrentPage == 0)
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
			doActive = false;
		}
		if (this.CurrentPage == this.LootMatrixItemTemplates.Count - 1)
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
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(this.m_previousPageBtn, doActive, null);
		UIManager.SetGameObjectActive(this.m_nextPageBtn, doActive2, null);
	}

	public void PreviousPageClicked(BaseEventData data)
	{
		this.DisplayPage(this.CurrentPage - 1);
	}

	public void NextPageClicked(BaseEventData data)
	{
		this.DisplayPage(this.CurrentPage + 1);
	}

	public void DisplayPage(int pageIndex)
	{
		int num = Mathf.Clamp(pageIndex, 0, this.LootMatrixItemTemplates.Count);
		if (num < this.LootMatrixItemTemplates.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixContentViewer.DisplayPage(int)).MethodHandle;
			}
			this.CurrentPage = num;
			LootTable lootTable = InventoryWideData.Get().GetLootTable(this.LootMatrixItemTemplates[num].TypeSpecificData[0]);
			this.DisplayItems(new List<LootTable>
			{
				lootTable
			});
			InventoryItem itemByTemplateId = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(this.LootMatrixItemTemplates[num].Index);
			List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
			List<int> list = new List<int>();
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].TemplateId == this.LootMatrixItemTemplates[num].Index)
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
					list.Add(items[i].Id);
				}
			}
			this.m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, itemByTemplateId, this.LootMatrixItemTemplates[num], list, false);
		}
		this.UpdateButtons();
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_Container, visible, null);
		if (visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixContentViewer.SetVisible(bool)).MethodHandle;
			}
			this.DisplayPage(0);
		}
	}

	[Serializable]
	public class PrefabContainerIndexPair
	{
		public int LootTableIndex;

		public RectTransform PreviewObject;
	}
}
