using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixScreen : UIScene
{
	public RectTransform m_container;

	public TextMeshProUGUI m_title;

	public GridLayoutGroup m_lootMatrixBtnGrid;

	public UIInventoryItem m_lootMatrixBtnPrefab;

	public _ButtonSwapSprite m_openBtn;

	public TextMeshProUGUI[] m_openLabels;

	public _ButtonSwapSprite m_getMoreBtn;

	public UIInventoryItem m_lootMatrixButtonPrefab;

	public VerticalLayoutGroup m_accordionRewardGroup;

	public UILockboxRewardItem m_singleRewardItem;

	public _SelectableBtn m_viewContentBtn;

	public _SelectableBtn m_moreInfoBtn;

	public RectTransform m_moreInfoTooltip;

	public LootMatrixThermostat m_thermoStat;

	public const int kDefaultLootMatrix = 515;

	private const float kVfxOpenTime = 0.5f;

	private const float kLockboxSpawnDelay = 0.5f;

	private InventoryItem m_item;

	private InventoryItemTemplate m_template;

	private List<int> m_boxIds;

	private int m_numBoxes;

	private float m_vfxOpenTime;

	private float m_lockboxSpawnTime;

	private Animator m_chestAnimator;

	private CanvasGroup m_accordionRewardCanvasGroup;

	private HorizontalLayoutGroup[] m_accordionRewardRows;

	private List<InventoryItemWithData> m_outputItems;

	private List<UIInventoryItem> m_lockboxButtons;

	private bool m_isOpening;

	private bool m_isLoading;

	private Queue<UILockboxRewardItem> m_pendingDuplicateAnimations;

	private bool m_moreBoxesAvailable;

	private float m_startRowSpacing;

	private float m_endRowSpacing;

	private float m_startColSpacing;

	private float m_endColSpacing;

	private static UILootMatrixScreen s_instance;

	private string m_inventoryItemStr = string.Empty;

	private bool m_throwBoxException;

	public bool IsVisible
	{
		get;
		private set;
	}

	public static UILootMatrixScreen Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LootMatrix;
	}

	public override void Awake()
	{
		s_instance = this;
		m_pendingDuplicateAnimations = new Queue<UILockboxRewardItem>();
		UIInventoryItem[] componentsInChildren = m_lootMatrixBtnGrid.GetComponentsInChildren<UIInventoryItem>(true);
		foreach (UIInventoryItem uIInventoryItem in componentsInChildren)
		{
			UnityEngine.Object.Destroy(uIInventoryItem.gameObject);
		}
		m_lockboxButtons = new List<UIInventoryItem>();
		m_moreInfoBtn.m_ignorePressAnimationCall = true;
		m_viewContentBtn.spriteController.callback = delegate
		{
			UILootMatrixContentViewer.Get().Setup(m_template);
			UILootMatrixContentViewer.Get().SetVisible(true);
		};
		base.Awake();
	}

	public void MoreInfoMouseEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_moreInfoTooltip, true);
	}

	public void MoreInfoMouseExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_moreInfoTooltip, false);
	}

	private void Start()
	{
		m_openBtn.callback = OpenClicked;
		m_getMoreBtn.callback = GetMoreClicked;
		m_moreInfoBtn.spriteController.pointerEnterCallback = MoreInfoMouseEnter;
		m_moreInfoBtn.spriteController.pointerExitCallback = MoreInfoMouseExit;
		UIManager.SetGameObjectActive(m_openBtn.selectableButton, true);
		m_accordionRewardCanvasGroup = m_accordionRewardGroup.GetComponent<CanvasGroup>();
		m_accordionRewardRows = m_accordionRewardGroup.GetComponentsInChildren<HorizontalLayoutGroup>(true);
		SetVisible(false);
		InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(515);
		UIInventoryItem uIInventoryItem = UnityEngine.Object.Instantiate(m_lootMatrixBtnPrefab);
		uIInventoryItem.transform.SetParent(m_lootMatrixBtnGrid.transform);
		uIInventoryItem.transform.localPosition = Vector3.zero;
		uIInventoryItem.transform.localScale = Vector3.one;
		uIInventoryItem.Setup(itemTemplate, null);
		uIInventoryItem.UpdateItemCount(0, false, true, true);
		uIInventoryItem.m_hitbox.callback = ClickLockbox;
		m_lockboxButtons.Add(uIInventoryItem);
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			InventoryComponentUpdated(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent);
		}
		if (m_lockboxButtons.Count > 1)
		{
			SelectLockbox(m_lockboxButtons[1].GetItemTemplate());
		}
		ClientGameManager.Get().OnInventoryComponentUpdated += InventoryComponentUpdated;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnInventoryComponentUpdated -= InventoryComponentUpdated;
			return;
		}
	}

	public void QueueDuplicateAnimation(UILockboxRewardItem item)
	{
		m_pendingDuplicateAnimations.Enqueue(item);
	}

	private void UpdateLockBoxCount(InventoryComponent component)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < component.Items.Count; i++)
		{
			if (dictionary.ContainsKey(component.Items[i].TemplateId))
			{
				dictionary[component.Items[i].TemplateId] += component.Items[i].Count;
			}
			else
			{
				dictionary[component.Items[i].TemplateId] = component.Items[i].Count;
			}
		}
		while (true)
		{
			if (!dictionary.ContainsKey(515))
			{
				dictionary[515] = 0;
			}
			for (int num = m_lockboxButtons.Count - 1; num >= 0; num--)
			{
				if (dictionary.ContainsKey(m_lockboxButtons[num].GetTemplateId()))
				{
					m_lockboxButtons[num].UpdateItemCount(dictionary[m_lockboxButtons[num].GetTemplateId()], false, true);
					dictionary.Remove(m_lockboxButtons[num].GetTemplateId());
				}
				else
				{
					UnityEngine.Object.Destroy(m_lockboxButtons[num].gameObject);
					m_lockboxButtons.RemoveAt(num);
				}
			}
			while (true)
			{
				using (Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<int, int> current = enumerator.Current;
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current.Key);
						if (itemTemplate.Type == InventoryItemType.Lockbox)
						{
							UIInventoryItem uIInventoryItem = UnityEngine.Object.Instantiate(m_lootMatrixBtnPrefab);
							uIInventoryItem.transform.SetParent(m_lootMatrixBtnGrid.transform);
							uIInventoryItem.transform.localPosition = Vector3.zero;
							uIInventoryItem.transform.localScale = Vector3.one;
							uIInventoryItem.Setup(itemTemplate, null);
							uIInventoryItem.UpdateItemCount(current.Value, false, true, true);
							uIInventoryItem.m_hitbox.callback = ClickLockbox;
							m_lockboxButtons.Add(uIInventoryItem);
						}
					}
				}
				UpdateOpenButtonText();
				if (m_template != null)
				{
					while (true)
					{
						UpdateNumberOfLockbox();
						return;
					}
				}
				return;
			}
		}
	}

	private void InventoryComponentUpdated(InventoryComponent component)
	{
		m_inventoryItemStr = string.Join(" | ", component.Items.Select((InventoryItem x) => new StringBuilder().Append(x.Id).Append(",").Append(x.TemplateId).Append("=").Append(x.Count).ToString()).ToArray());
		UpdateLockBoxCount(component);
	}

	public void SetVisible(bool isVisible)
	{
		UIManager.SetGameObjectActive(m_moreInfoTooltip, false);
		IsVisible = isVisible;
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(false);
		if (m_chestAnimator != null)
		{
			UIManager.SetGameObjectActive(m_chestAnimator, false);
		}
		m_vfxOpenTime = -1f;
		m_lockboxSpawnTime = -1f;
		m_openBtn.ResetMouseState();
		m_accordionRewardCanvasGroup.alpha = 0f;
		UIManager.SetGameObjectActive(m_accordionRewardGroup, false);
		UIManager.SetGameObjectActive(m_singleRewardItem, false);
		FrontEndCharacterSelectBackgroundScene.Get().m_lootMatrixModelStage.gameObject.SetActive(isVisible);
		if (UICharacterSelectWorldObjects.Get().IsVisible())
		{
			if (isVisible)
			{
				UICharacterSelectWorldObjects.Get().SetVisible(false);
			}
		}
		if (isVisible)
		{
			UINewUserFlowManager.OnLootMatrixScreenVisible();
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
			if (m_isOpening)
			{
				DoOpenChestAnimationEvent();
			}
			else
			{
				if (m_numBoxes == 0)
				{
					m_template = null;
				}
				SelectLockbox(m_template, true);
			}
			if (AnnouncerSounds.GetAnnouncerSounds() != null)
			{
				AnnouncerSounds.GetAnnouncerSounds().InstantiateLootVOPrefabIfNeeded();
			}
		}
		else
		{
			UILootMatrixPurchaseScreen.Get().SetVisible(false);
			if (UIFrontEnd.Get() != null)
			{
				UIFrontEnd.Get().m_playerPanel.HandlePendingTrustNotifications();
			}
		}
		UIManager.SetGameObjectActive(m_container, isVisible);
	}

	private void ClickLockbox(BaseEventData data)
	{
		if (m_isOpening)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if ((data as PointerEventData).button != 0)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_moreInfoTooltip, false);
			InventoryItemTemplate itemTemplate = (data as PointerEventData).pointerPress.transform.parent.gameObject.GetComponent<UIInventoryItem>().GetItemTemplate();
			SelectLockbox(itemTemplate);
			return;
		}
	}

	public void SelectLockbox(InventoryItemTemplate template, bool forceUpdate = false)
	{
		if (template == null)
		{
			template = InventoryWideData.Get().GetItemTemplate(515);
		}
		if (!forceUpdate)
		{
			if (m_template != null)
			{
				if (template.Index == m_template.Index)
				{
					while (true)
					{
						switch (2)
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
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(false);
		if (m_chestAnimator != null)
		{
			UIManager.SetGameObjectActive(m_chestAnimator, false);
		}
		m_vfxOpenTime = -1f;
		m_lockboxSpawnTime = -1f;
		if (template.TypeSpecificData.Length > 1)
		{
			UIManager.SetGameObjectActive(m_viewContentBtn, template.TypeSpecificData[1] == 1);
		}
		else
		{
			UIManager.SetGameObjectActive(m_viewContentBtn, false);
		}
		UIManager.SetGameObjectActive(m_moreInfoBtn, !m_viewContentBtn.gameObject.activeSelf);
		_SelectableBtn selectableButton = m_getMoreBtn.selectableButton;
		int doActive;
		if (template != null)
		{
			doActive = ((template.Index == 515) ? 1 : 0);
		}
		else
		{
			doActive = 1;
		}
		UIManager.SetGameObjectActive(selectableButton, (byte)doActive != 0);
		m_item = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(template.Index);
		m_template = template;
		if (m_template.Type != InventoryItemType.Lockbox)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					throw new Exception(new StringBuilder().Append("This is not a loot matrix. Template: ").Append(m_template.Index).Append(" -> ").Append(m_template.GetDisplayName()).ToString());
				}
			}
		}
		if (m_chestAnimator != null)
		{
			UnityEngine.Object.Destroy(m_chestAnimator.gameObject);
		}
		GameObject lockboxPrefab = InventoryWideData.Get().GetLockboxPrefab(m_template.Index);
		if (lockboxPrefab == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new Exception(new StringBuilder().Append("Loot matrix ").Append(m_template.Index).Append(" does not have a prefab.").ToString());
				}
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(lockboxPrefab);
		gameObject.transform.SetParent(FrontEndCharacterSelectBackgroundScene.Get().m_chestContainer);
		gameObject.transform.localPosition = Vector3.zero;
		UIManager.SetGameObjectActive(gameObject, false);
		m_chestAnimator = gameObject.GetComponent<Animator>();
		string text = m_template.GetDisplayName();
		if (m_template.AssociatedCharacter != 0)
		{
			text = new StringBuilder().Append("<voffset=0.15em><size=30><sprite=\"CharacterSprites\" index=").Append(2 * (int)m_template.AssociatedCharacter).Append(">\u200b</size></voffset>").Append(text).ToString();
		}
		m_title.text = text;
		m_vfxOpenTime = Time.time + 0.5f;
		m_lockboxSpawnTime = m_vfxOpenTime + 0.5f;
		for (int i = 0; i < m_lockboxButtons.Count; i++)
		{
			if (m_lockboxButtons[i].GetTemplateId() == template.Index)
			{
				m_lockboxButtons[i].m_hitbox.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			}
			else
			{
				m_lockboxButtons[i].m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		while (true)
		{
			UpdateNumberOfLockbox();
			m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, m_item, m_template, m_boxIds);
			UIManager.SetGameObjectActive(m_singleRewardItem, false);
			UIManager.SetGameObjectActive(m_accordionRewardGroup, false);
			m_isLoading = true;
			m_openBtn.selectableButton.SetDisabled(true);
			return;
		}
	}

	private void UpdateNumberOfLockbox()
	{
		List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
		m_boxIds = new List<int>();
		m_numBoxes = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].TemplateId == m_template.Index && items.Count > 0)
			{
				m_boxIds.Add(items[i].Id);
				m_numBoxes += items[i].Count;
			}
		}
		UpdateOpenButtonText();
	}

	private void GetMoreClicked(BaseEventData data)
	{
		UILootMatrixPurchaseScreen.Get().SetVisible(true);
	}

	private void OpenClicked(BaseEventData data)
	{
		if (m_isOpening)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_numBoxes == 0)
		{
			InventoryItemTemplate inventoryItemTemplate = null;
			int num = 0;
			while (true)
			{
				if (num < m_lockboxButtons.Count)
				{
					inventoryItemTemplate = m_lockboxButtons[num].GetItemTemplate();
					if (inventoryItemTemplate.Index != 515)
					{
						break;
					}
					num++;
					continue;
				}
				break;
			}
			SelectLockbox(inventoryItemTemplate);
		}
		else
		{
			UINewUserFlowManager.OnMainLootMatrixOpenClicked();
			UIManager.SetGameObjectActive(m_chestAnimator, true);
			m_openBtn.selectableButton.SetDisabled(true);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxOpenClick);
			m_isOpening = true;
			m_accordionRewardCanvasGroup.alpha = 0f;
			UIManager.SetGameObjectActive(m_accordionRewardGroup, false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(false);
			UIManager.SetGameObjectActive(m_singleRewardItem, false);
			m_item = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItem(m_boxIds[0]);
			ClientGameManager.Get().ConsumeInventoryItem(m_item.Id, false, OpenChestResponse);
		}
	}

	private void OpenChestResponse(ConsumeInventoryItemResponse response)
	{
		if (response.Result != ConsumeInventoryItemResult.Success)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					m_isOpening = false;
					Log.Info("IssueOpeningLootMatrix {0} result={1}", m_boxIds[0], response.Result);
					string title = StringUtil.TR("Error", "Global");
					string description = StringUtil.TR("IssueOpeningLootMatrix", "Global");
					string buttonLabelText = StringUtil.TR("Ok", "Global");
					UIDialogPopupManager.OpenOneButtonDialog(title, description, buttonLabelText, delegate
					{
						if (response.Result == ConsumeInventoryItemResult.ItemNotFound)
						{
							m_throwBoxException = true;
						}
					});
					return;
				}
				}
			}
		}
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		ClientGameManager.Get().RequestUpdateUIState(uiState, playerAccountData.AccountComponent.GetUIState(uiState) + 1, null);
		int num = m_boxIds.Count - 1;
		while (true)
		{
			if (num >= 0)
			{
				if (m_item.Id == m_boxIds[num])
				{
					if (m_item.Count > 1)
					{
						break;
					}
					if (m_item.Count == 1)
					{
						m_boxIds.RemoveAt(num);
						break;
					}
					m_boxIds.RemoveAt(num);
				}
				num--;
				continue;
			}
			break;
		}
		m_numBoxes--;
		m_outputItems = response.OutputItems;
		InventoryItemRarity inventoryItemRarity = InventoryItemRarity.Common;
		using (List<InventoryItemWithData>.Enumerator enumerator = m_outputItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItemWithData current = enumerator.Current;
				InventoryItemRarity rarity = current.Item.GetTemplate().Rarity;
				if (rarity > inventoryItemRarity)
				{
					inventoryItemRarity = rarity;
				}
			}
		}
		switch (inventoryItemRarity)
		{
		case InventoryItemRarity.Common:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(true);
			break;
		case InventoryItemRarity.Uncommon:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(true);
			break;
		case InventoryItemRarity.Rare:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(true);
			break;
		case InventoryItemRarity.Epic:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(true);
			break;
		case InventoryItemRarity.Legendary:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(true);
			break;
		default:
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(true);
			break;
		}
		m_chestAnimator.Play("NanoOpenChest", -1, 0f);
	}

	public void DoOpenChestAnimationEvent()
	{
		bool gotLegendary = false;
		for (int i = 0; i < m_outputItems.Count; i++)
		{
			InventoryItemWithData data = m_outputItems[i];
			InventoryItemTemplate template = data.Item.GetTemplate();
			if (template.Rarity == InventoryItemRarity.Legendary)
			{
				gotLegendary = true;
			}
			if (template.Index != m_template.Index)
			{
				continue;
			}
			m_numBoxes += data.Item.Count;
			if (!m_boxIds.Exists((int x) => x == data.Item.Id))
			{
				m_boxIds.Add(data.Item.Id);
			}
		}
		while (true)
		{
			m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, m_item, m_template, m_boxIds, gotLegendary);
			string text = string.Empty;
			List<InventoryItem> list = new List<InventoryItem>();
			for (int j = 0; j < m_outputItems.Count; j++)
			{
				bool flag = false;
				int num = 0;
				while (true)
				{
					if (num < list.Count)
					{
						if (list[num].TemplateId == m_outputItems[j].Item.TemplateId)
						{
							list[num].Count += m_outputItems[j].Item.Count;
							flag = true;
							break;
						}
						num++;
						continue;
					}
					break;
				}
				if (!flag)
				{
					list.Add(new InventoryItem(m_outputItems[j].Item));
				}
			}
			while (true)
			{
				foreach (InventoryItem item in list)
				{
					text = new StringBuilder().AppendLine(text).Append(" - ").Append(item.GetTemplate().GetDisplayName()).ToString();
					if (item.Count > 1)
					{
						text = new StringBuilder().Append(text).Append(" x").Append(item.Count).ToString();
					}
				}
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = string.Format(StringUtil.TR("LockBoxOpenedReceived", "Global"), m_template.GetDisplayName(), text),
					MessageType = ConsoleMessageType.SystemMessage
				});
				if (m_outputItems.Count == 1)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							m_outputItems[0].Item.GetTemplate().Rarity.PlaySound();
							m_singleRewardItem.Setup(m_outputItems[0].Item, m_outputItems[0].Item.GetTemplate(), m_outputItems[0].IsoGained > 0, m_outputItems[0].IsoGained);
							UIManager.SetGameObjectActive(m_singleRewardItem, true);
							if (m_outputItems[0].IsoGained <= 0)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
									{
										CharacterType characterTypeOfItemTemplate = GetCharacterTypeOfItemTemplate(m_outputItems[0].Item.GetTemplate());
										if (characterTypeOfItemTemplate != 0 && AnnouncerSounds.GetAnnouncerSounds() != null)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													AnnouncerSounds.GetAnnouncerSounds().PlayLootVOForCharacter(characterTypeOfItemTemplate);
													return;
												}
											}
										}
										return;
									}
									}
								}
							}
							return;
						}
					}
				}
				m_accordionRewardCanvasGroup.alpha = 0f;
				UIManager.SetGameObjectActive(m_accordionRewardGroup, true);
				UIManager.SetGameObjectActive(m_accordionRewardRows[0], true);
				for (int k = 1; k < m_accordionRewardRows.Length; k++)
				{
					UIManager.SetGameObjectActive(m_accordionRewardRows[k], false);
				}
				while (true)
				{
					Queue<UILockboxRewardItem> queue = new Queue<UILockboxRewardItem>();
					if (m_outputItems.Count <= 3)
					{
						if (m_outputItems.Count == 3)
						{
							if (m_outputItems[0].Item.GetTemplate().Rarity > m_outputItems[1].Item.GetTemplate().Rarity)
							{
								InventoryItemWithData value = m_outputItems[0];
								m_outputItems[0] = m_outputItems[1];
								m_outputItems[1] = value;
							}
							if (m_outputItems[2].Item.GetTemplate().Rarity > m_outputItems[1].Item.GetTemplate().Rarity)
							{
								InventoryItemWithData value2 = m_outputItems[2];
								m_outputItems[2] = m_outputItems[1];
								m_outputItems[1] = value2;
							}
						}
						m_startRowSpacing = 0f;
						m_endRowSpacing = 0f;
						m_startColSpacing = -700f;
						m_endColSpacing = -120f;
						UILockboxRewardItem[] componentsInChildren = m_accordionRewardRows[0].GetComponentsInChildren<UILockboxRewardItem>(true);
						for (int l = 0; l < componentsInChildren.Length; l++)
						{
							if (l < m_outputItems.Count)
							{
								UIManager.SetGameObjectActive(componentsInChildren[l], true);
								componentsInChildren[l].SetSize(700f);
								queue.Enqueue(componentsInChildren[l]);
							}
							else
							{
								UIManager.SetGameObjectActive(componentsInChildren[l], false);
							}
						}
					}
					else
					{
						if (m_outputItems.Count > 10)
						{
							throw new Exception("More than 10 is not supported yet.");
						}
						m_startRowSpacing = -350f;
						m_endRowSpacing = 0f;
						m_startColSpacing = -350f;
						m_endColSpacing = 0f;
						UIManager.SetGameObjectActive(m_accordionRewardRows[1], true);
						int num2 = Mathf.CeilToInt((float)m_outputItems.Count / 2f);
						UILockboxRewardItem[] componentsInChildren2 = m_accordionRewardRows[0].GetComponentsInChildren<UILockboxRewardItem>(true);
						for (int m = 0; m < componentsInChildren2.Length; m++)
						{
							if (m < num2)
							{
								UIManager.SetGameObjectActive(componentsInChildren2[m], true);
								componentsInChildren2[m].SetSize(350f);
								queue.Enqueue(componentsInChildren2[m]);
							}
							else
							{
								UIManager.SetGameObjectActive(componentsInChildren2[m], false);
							}
						}
						num2 = m_outputItems.Count / 2;
						componentsInChildren2 = m_accordionRewardRows[1].GetComponentsInChildren<UILockboxRewardItem>(true);
						for (int n = 0; n < componentsInChildren2.Length; n++)
						{
							if (n < num2)
							{
								UIManager.SetGameObjectActive(componentsInChildren2[n], true);
								componentsInChildren2[n].SetSize(350f);
								queue.Enqueue(componentsInChildren2[n]);
							}
							else
							{
								UIManager.SetGameObjectActive(componentsInChildren2[n], false);
							}
						}
					}
					InventoryItemRarity inventoryItemRarity = InventoryItemRarity.Common;
					CharacterType characterType = CharacterType.None;
					InventoryItemRarity inventoryItemRarity2 = InventoryItemRarity.Common;
					using (List<InventoryItemWithData>.Enumerator enumerator2 = m_outputItems.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							InventoryItemWithData current2 = enumerator2.Current;
							UILockboxRewardItem uILockboxRewardItem = queue.Dequeue();
							uILockboxRewardItem.Setup(current2.Item, current2.Item.GetTemplate(), current2.IsoGained > 0, current2.IsoGained);
							if (current2.Item.GetTemplate().Rarity > inventoryItemRarity2)
							{
								inventoryItemRarity2 = current2.Item.GetTemplate().Rarity;
							}
							if (current2.IsoGained <= 0)
							{
								InventoryItemTemplate template2 = current2.Item.GetTemplate();
								if (template2 != null)
								{
									if (characterType != 0)
									{
										if (template2.Rarity <= inventoryItemRarity)
										{
											continue;
										}
									}
									CharacterType characterTypeOfItemTemplate2 = GetCharacterTypeOfItemTemplate(template2);
									if (characterTypeOfItemTemplate2 != 0)
									{
										characterType = characterTypeOfItemTemplate2;
										inventoryItemRarity = template2.Rarity;
									}
								}
							}
						}
					}
					inventoryItemRarity2.PlaySound();
					if (characterType != 0)
					{
						if (AnnouncerSounds.GetAnnouncerSounds() != null)
						{
							AnnouncerSounds.GetAnnouncerSounds().PlayLootVOForCharacter(characterType);
						}
					}
					m_accordionRewardGroup.spacing = m_startRowSpacing;
					for (int num3 = 0; num3 < m_accordionRewardRows.Length; num3++)
					{
						m_accordionRewardRows[num3].spacing = m_startColSpacing;
					}
					while (true)
					{
						switch (6)
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

	private CharacterType GetCharacterTypeOfItemTemplate(InventoryItemTemplate template)
	{
		if (template != null && template.TypeSpecificData != null)
		{
			if (template.TypeSpecificData.Length > 0)
			{
				if (template.Type != InventoryItemType.Skin && template.Type != InventoryItemType.Style)
				{
					if (template.Type != InventoryItemType.Taunt)
					{
						goto IL_006b;
					}
				}
				return (CharacterType)template.TypeSpecificData[0];
			}
		}
		goto IL_006b;
		IL_006b:
		return CharacterType.None;
	}

	public void FinishRewardAnimation()
	{
		m_isOpening = false;
		UpdateOpenButtonText();
		UIFrontEnd.Get().m_playerPanel.HandlePendingTrustNotifications();
	}

	private void UpdateOpenButtonText()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		bool flag = false;
		if (clientGameManager != null)
		{
			if (!clientGameManager.HasPurchasedGame)
			{
				InventoryComponent inventoryComponent = clientGameManager.GetPlayerAccountData().InventoryComponent;
				TimeSpan t = new TimeSpan(0, 0, GameBalanceVars.Get().NumSecsToOpenLootMatrix);
				TimeSpan t2 = clientGameManager.UtcNow().Subtract(inventoryComponent.LastLockboxOpenTime);
				flag = (t2 < t);
			}
		}
		int num;
		if (!m_isOpening)
		{
			if (!m_isLoading)
			{
				num = (flag ? 1 : 0);
				goto IL_00a9;
			}
		}
		num = 1;
		goto IL_00a9;
		IL_00a9:
		bool flag2 = (byte)num != 0;
		string text = string.Format(StringUtil.TR("OpenNumber", "Global"), m_numBoxes);
		if (m_numBoxes == 0)
		{
			m_moreBoxesAvailable = false;
			if (m_template != null)
			{
				if (m_template.Index != 515)
				{
					text = StringUtil.TR("Next", "Global");
					m_moreBoxesAvailable = true;
					goto IL_01ce;
				}
			}
			if (clientGameManager != null && clientGameManager.IsPlayerAccountDataAvailable())
			{
				List<InventoryItem> items = clientGameManager.GetPlayerAccountData().InventoryComponent.Items;
				int num2 = 0;
				while (true)
				{
					if (num2 < items.Count)
					{
						if (items[num2].GetTemplate().Type == InventoryItemType.Lockbox)
						{
							text = StringUtil.TR("Next", "Global");
							m_moreBoxesAvailable = true;
							break;
						}
						num2++;
						continue;
					}
					break;
				}
			}
			goto IL_01ce;
		}
		goto IL_01ea;
		IL_01ea:
		m_openBtn.selectableButton.SetDisabled(flag2);
		float num3 = 0f;
		for (int i = 0; i < m_openLabels.Length; i++)
		{
			m_openLabels[i].text = text;
			m_openLabels[i].CalculateLayoutInputHorizontal();
			Vector2 preferredValues = m_openLabels[i].GetPreferredValues();
			if (preferredValues.x > num3)
			{
				Vector2 preferredValues2 = m_openLabels[i].GetPreferredValues();
				num3 = preferredValues2.x;
			}
		}
		m_openBtn.selectableButton.GetComponent<LayoutElement>().preferredWidth = num3 + 150f;
		m_openBtn.selectableButton.spriteController.ResetMouseState();
		return;
		IL_01ce:
		int num4;
		if (!flag2)
		{
			num4 = ((!m_moreBoxesAvailable) ? 1 : 0);
		}
		else
		{
			num4 = 1;
		}
		flag2 = ((byte)num4 != 0);
		goto IL_01ea;
	}

	private void Update()
	{
		if (!IsVisible)
		{
			while (true)
			{
				return;
			}
		}
		if (m_throwBoxException)
		{
			while (true)
			{
				m_throwBoxException = false;
				List<int> boxIds = m_boxIds;
				
				string text = string.Join(", ", boxIds.Select(((int x) => x.ToString())).ToArray());
				throw new Exception(string.Format("IssueOpeningLootMatrix {0} result={1}\nBoxIds:{2}\nInventory: {3}", m_boxIds[0], "ItemNotFound", text, m_inventoryItemStr));
			}
		}
		if (m_vfxOpenTime > 0f)
		{
			if (!FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.activeSelf)
			{
				if (Time.time > m_vfxOpenTime)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxAppear);
					FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.SetActive(true);
					m_vfxOpenTime = -1f;
					goto IL_01d0;
				}
			}
		}
		if (m_lockboxSpawnTime > 0f)
		{
			if (FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.activeSelf)
			{
				if (Time.time > m_lockboxSpawnTime)
				{
					if (m_numBoxes > 0)
					{
						if (m_template.Index == 515)
						{
							UINewUserFlowManager.OnMainLootMatrixDisplayed();
						}
						else
						{
							UINewUserFlowManager.OnSpecialLootMatrixDisplayed();
						}
					}
					UIManager.SetGameObjectActive(m_chestAnimator, true);
					m_lockboxSpawnTime = -1f;
				}
			}
		}
		goto IL_01d0;
		IL_01d0:
		if (m_accordionRewardGroup.IsActive())
		{
			if (m_accordionRewardCanvasGroup.alpha < 1f)
			{
				m_accordionRewardCanvasGroup.alpha += Time.smoothDeltaTime;
			}
			if (m_accordionRewardGroup.spacing < m_endRowSpacing)
			{
				m_accordionRewardGroup.spacing += (m_endRowSpacing - m_startRowSpacing) * Time.smoothDeltaTime;
				if (m_accordionRewardGroup.spacing > m_endRowSpacing)
				{
					m_accordionRewardGroup.spacing = m_endRowSpacing;
				}
			}
			for (int i = 0; i < m_accordionRewardRows.Length; i++)
			{
				if (!(m_accordionRewardRows[i].spacing < m_endColSpacing))
				{
					continue;
				}
				m_accordionRewardRows[i].spacing += (m_endColSpacing - m_startColSpacing) * Time.smoothDeltaTime;
				if (m_accordionRewardRows[i].spacing > m_endColSpacing)
				{
					m_accordionRewardRows[i].spacing = m_endColSpacing;
				}
			}
			if (m_accordionRewardGroup.spacing >= m_endRowSpacing)
			{
				if (m_accordionRewardRows[0].spacing >= m_endColSpacing)
				{
					FinishRewardAnimation();
				}
			}
		}
		if (m_isLoading)
		{
			if (m_chestAnimator.isInitialized)
			{
				m_isLoading = false;
				_SelectableBtn selectableButton = m_openBtn.selectableButton;
				int disabled;
				if (!m_isOpening)
				{
					if (m_numBoxes <= 0)
					{
						disabled = ((m_lockboxButtons.Count <= 1) ? 1 : 0);
					}
					else
					{
						disabled = 0;
					}
				}
				else
				{
					disabled = 1;
				}
				selectableButton.SetDisabled((byte)disabled != 0);
			}
		}
		while (m_pendingDuplicateAnimations.Count > 0)
		{
			m_pendingDuplicateAnimations.Dequeue().PlayDuplicateAnimation();
		}
	}

	public void NotifyGetFocus()
	{
		SetVisible(true);
	}

	public void NotifyLoseFocus()
	{
		SetVisible(false);
	}

	public bool IsOpening()
	{
		return m_isOpening;
	}
}
