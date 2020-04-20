using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
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

	public const int kDefaultLootMatrix = 0x203;

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

	public bool IsVisible { get; private set; }

	public static UILootMatrixScreen Get()
	{
		return UILootMatrixScreen.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LootMatrix;
	}

	public override void Awake()
	{
		UILootMatrixScreen.s_instance = this;
		this.m_pendingDuplicateAnimations = new Queue<UILockboxRewardItem>();
		foreach (UIInventoryItem uiinventoryItem in this.m_lootMatrixBtnGrid.GetComponentsInChildren<UIInventoryItem>(true))
		{
			UnityEngine.Object.Destroy(uiinventoryItem.gameObject);
		}
		this.m_lockboxButtons = new List<UIInventoryItem>();
		this.m_moreInfoBtn.m_ignorePressAnimationCall = true;
		this.m_viewContentBtn.spriteController.callback = delegate(BaseEventData data)
		{
			UILootMatrixContentViewer.Get().Setup(this.m_template, false);
			UILootMatrixContentViewer.Get().SetVisible(true);
		};
		base.Awake();
	}

	public void MoreInfoMouseEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_moreInfoTooltip, true, null);
	}

	public void MoreInfoMouseExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_moreInfoTooltip, false, null);
	}

	private void Start()
	{
		this.m_openBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OpenClicked);
		this.m_getMoreBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.GetMoreClicked);
		this.m_moreInfoBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.MoreInfoMouseEnter);
		this.m_moreInfoBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.MoreInfoMouseExit);
		UIManager.SetGameObjectActive(this.m_openBtn.selectableButton, true, null);
		this.m_accordionRewardCanvasGroup = this.m_accordionRewardGroup.GetComponent<CanvasGroup>();
		this.m_accordionRewardRows = this.m_accordionRewardGroup.GetComponentsInChildren<HorizontalLayoutGroup>(true);
		this.SetVisible(false);
		InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(0x203);
		UIInventoryItem uiinventoryItem = UnityEngine.Object.Instantiate<UIInventoryItem>(this.m_lootMatrixBtnPrefab);
		uiinventoryItem.transform.SetParent(this.m_lootMatrixBtnGrid.transform);
		uiinventoryItem.transform.localPosition = Vector3.zero;
		uiinventoryItem.transform.localScale = Vector3.one;
		uiinventoryItem.Setup(itemTemplate, null);
		uiinventoryItem.UpdateItemCount(0, false, true, true);
		uiinventoryItem.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickLockbox);
		this.m_lockboxButtons.Add(uiinventoryItem);
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.InventoryComponentUpdated(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent);
		}
		if (this.m_lockboxButtons.Count > 1)
		{
			this.SelectLockbox(this.m_lockboxButtons[1].GetItemTemplate(), false);
		}
		ClientGameManager.Get().OnInventoryComponentUpdated += this.InventoryComponentUpdated;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnInventoryComponentUpdated -= this.InventoryComponentUpdated;
		}
	}

	public void QueueDuplicateAnimation(UILockboxRewardItem item)
	{
		this.m_pendingDuplicateAnimations.Enqueue(item);
	}

	private void UpdateLockBoxCount(InventoryComponent component)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < component.Items.Count; i++)
		{
			if (dictionary.ContainsKey(component.Items[i].TemplateId))
			{
				Dictionary<int, int> dictionary2;
				int templateId;
				(dictionary2 = dictionary)[templateId = component.Items[i].TemplateId] = dictionary2[templateId] + component.Items[i].Count;
			}
			else
			{
				dictionary[component.Items[i].TemplateId] = component.Items[i].Count;
			}
		}
		if (!dictionary.ContainsKey(0x203))
		{
			dictionary[0x203] = 0;
		}
		for (int j = this.m_lockboxButtons.Count - 1; j >= 0; j--)
		{
			if (dictionary.ContainsKey(this.m_lockboxButtons[j].GetTemplateId()))
			{
				this.m_lockboxButtons[j].UpdateItemCount(dictionary[this.m_lockboxButtons[j].GetTemplateId()], false, true, false);
				dictionary.Remove(this.m_lockboxButtons[j].GetTemplateId());
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_lockboxButtons[j].gameObject);
				this.m_lockboxButtons.RemoveAt(j);
			}
		}
		using (Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, int> keyValuePair = enumerator.Current;
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(keyValuePair.Key);
				if (itemTemplate.Type == InventoryItemType.Lockbox)
				{
					UIInventoryItem uiinventoryItem = UnityEngine.Object.Instantiate<UIInventoryItem>(this.m_lootMatrixBtnPrefab);
					uiinventoryItem.transform.SetParent(this.m_lootMatrixBtnGrid.transform);
					uiinventoryItem.transform.localPosition = Vector3.zero;
					uiinventoryItem.transform.localScale = Vector3.one;
					uiinventoryItem.Setup(itemTemplate, null);
					uiinventoryItem.UpdateItemCount(keyValuePair.Value, false, true, true);
					uiinventoryItem.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickLockbox);
					this.m_lockboxButtons.Add(uiinventoryItem);
				}
			}
		}
		this.UpdateOpenButtonText();
		if (this.m_template != null)
		{
			this.UpdateNumberOfLockbox();
		}
	}

	private void InventoryComponentUpdated(InventoryComponent component)
	{
		this.m_inventoryItemStr = string.Join(" | ", (from x in component.Items
		select string.Concat(new object[]
		{
			x.Id,
			",",
			x.TemplateId,
			"=",
			x.Count
		})).ToArray<string>());
		this.UpdateLockBoxCount(component);
	}

	public void SetVisible(bool isVisible)
	{
		UIManager.SetGameObjectActive(this.m_moreInfoTooltip, false, null);
		this.IsVisible = isVisible;
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(false);
		FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(false);
		if (this.m_chestAnimator != null)
		{
			UIManager.SetGameObjectActive(this.m_chestAnimator, false, null);
		}
		this.m_vfxOpenTime = -1f;
		this.m_lockboxSpawnTime = -1f;
		this.m_openBtn.ResetMouseState();
		this.m_accordionRewardCanvasGroup.alpha = 0f;
		UIManager.SetGameObjectActive(this.m_accordionRewardGroup, false, null);
		UIManager.SetGameObjectActive(this.m_singleRewardItem, false, null);
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
			if (this.m_isOpening)
			{
				this.DoOpenChestAnimationEvent();
			}
			else
			{
				if (this.m_numBoxes == 0)
				{
					this.m_template = null;
				}
				this.SelectLockbox(this.m_template, true);
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
		UIManager.SetGameObjectActive(this.m_container, isVisible, null);
	}

	private void ClickLockbox(BaseEventData data)
	{
		if (this.m_isOpening)
		{
			return;
		}
		if ((data as PointerEventData).button == PointerEventData.InputButton.Left)
		{
			UIManager.SetGameObjectActive(this.m_moreInfoTooltip, false, null);
			InventoryItemTemplate itemTemplate = (data as PointerEventData).pointerPress.transform.parent.gameObject.GetComponent<UIInventoryItem>().GetItemTemplate();
			this.SelectLockbox(itemTemplate, false);
		}
	}

	public void SelectLockbox(InventoryItemTemplate template, bool forceUpdate = false)
	{
		if (template == null)
		{
			template = InventoryWideData.Get().GetItemTemplate(0x203);
		}
		if (!forceUpdate)
		{
			if (this.m_template != null)
			{
				if (template.Index == this.m_template.Index)
				{
					return;
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
		if (this.m_chestAnimator != null)
		{
			UIManager.SetGameObjectActive(this.m_chestAnimator, false, null);
		}
		this.m_vfxOpenTime = -1f;
		this.m_lockboxSpawnTime = -1f;
		if (template.TypeSpecificData.Length > 1)
		{
			UIManager.SetGameObjectActive(this.m_viewContentBtn, template.TypeSpecificData[1] == 1, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_viewContentBtn, false, null);
		}
		UIManager.SetGameObjectActive(this.m_moreInfoBtn, !this.m_viewContentBtn.gameObject.activeSelf, null);
		Component selectableButton = this.m_getMoreBtn.selectableButton;
		bool doActive;
		if (template != null)
		{
			doActive = (template.Index == 0x203);
		}
		else
		{
			doActive = true;
		}
		UIManager.SetGameObjectActive(selectableButton, doActive, null);
		this.m_item = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(template.Index);
		this.m_template = template;
		if (this.m_template.Type != InventoryItemType.Lockbox)
		{
			throw new Exception(string.Concat(new object[]
			{
				"This is not a loot matrix. Template: ",
				this.m_template.Index,
				" -> ",
				this.m_template.GetDisplayName()
			}));
		}
		if (this.m_chestAnimator != null)
		{
			UnityEngine.Object.Destroy(this.m_chestAnimator.gameObject);
		}
		GameObject lockboxPrefab = InventoryWideData.Get().GetLockboxPrefab(this.m_template.Index);
		if (lockboxPrefab == null)
		{
			throw new Exception("Loot matrix " + this.m_template.Index + " does not have a prefab.");
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(lockboxPrefab);
		gameObject.transform.SetParent(FrontEndCharacterSelectBackgroundScene.Get().m_chestContainer);
		gameObject.transform.localPosition = Vector3.zero;
		UIManager.SetGameObjectActive(gameObject, false, null);
		this.m_chestAnimator = gameObject.GetComponent<Animator>();
		string text = this.m_template.GetDisplayName();
		if (this.m_template.AssociatedCharacter != CharacterType.None)
		{
			text = string.Format("<voffset=0.15em><size=30><sprite=\"CharacterSprites\" index={0}>​</size></voffset>{1}", 2 * (int)this.m_template.AssociatedCharacter, text);
		}
		this.m_title.text = text;
		this.m_vfxOpenTime = Time.time + 0.5f;
		this.m_lockboxSpawnTime = this.m_vfxOpenTime + 0.5f;
		for (int i = 0; i < this.m_lockboxButtons.Count; i++)
		{
			if (this.m_lockboxButtons[i].GetTemplateId() == template.Index)
			{
				this.m_lockboxButtons[i].m_hitbox.selectableButton.SetSelected(true, false, string.Empty, string.Empty);
			}
			else
			{
				this.m_lockboxButtons[i].m_hitbox.selectableButton.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		this.UpdateNumberOfLockbox();
		this.m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, this.m_item, this.m_template, this.m_boxIds, false);
		UIManager.SetGameObjectActive(this.m_singleRewardItem, false, null);
		UIManager.SetGameObjectActive(this.m_accordionRewardGroup, false, null);
		this.m_isLoading = true;
		this.m_openBtn.selectableButton.SetDisabled(true);
	}

	private void UpdateNumberOfLockbox()
	{
		List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
		this.m_boxIds = new List<int>();
		this.m_numBoxes = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].TemplateId == this.m_template.Index && items.Count > 0)
			{
				this.m_boxIds.Add(items[i].Id);
				this.m_numBoxes += items[i].Count;
			}
		}
		this.UpdateOpenButtonText();
	}

	private void GetMoreClicked(BaseEventData data)
	{
		UILootMatrixPurchaseScreen.Get().SetVisible(true);
	}

	private void OpenClicked(BaseEventData data)
	{
		if (this.m_isOpening)
		{
			return;
		}
		if (this.m_numBoxes != 0)
		{
			UINewUserFlowManager.OnMainLootMatrixOpenClicked();
			UIManager.SetGameObjectActive(this.m_chestAnimator, true, null);
			this.m_openBtn.selectableButton.SetDisabled(true);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxOpenClick);
			this.m_isOpening = true;
			this.m_accordionRewardCanvasGroup.alpha = 0f;
			UIManager.SetGameObjectActive(this.m_accordionRewardGroup, false, null);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenBase.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenCommon.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenEpic.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenRare.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenLegendary.SetActive(false);
			FrontEndCharacterSelectBackgroundScene.Get().m_vfxOpenUncommon.SetActive(false);
			UIManager.SetGameObjectActive(this.m_singleRewardItem, false, null);
			this.m_item = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItem(this.m_boxIds[0]);
			ClientGameManager.Get().ConsumeInventoryItem(this.m_item.Id, false, new Action<ConsumeInventoryItemResponse>(this.OpenChestResponse));
			return;
		}
		InventoryItemTemplate inventoryItemTemplate = null;
		for (int i = 0; i < this.m_lockboxButtons.Count; i++)
		{
			inventoryItemTemplate = this.m_lockboxButtons[i].GetItemTemplate();
			if (inventoryItemTemplate.Index != 0x203)
			{
				break;
			}
		}
		this.SelectLockbox(inventoryItemTemplate, false);
	}

	private void OpenChestResponse(ConsumeInventoryItemResponse response)
	{
		if (response.Result != ConsumeInventoryItemResult.Success)
		{
			this.m_isOpening = false;
			Log.Info("IssueOpeningLootMatrix {0} result={1}", new object[]
			{
				this.m_boxIds[0],
				response.Result
			});
			string title = StringUtil.TR("Error", "Global");
			string description = StringUtil.TR("IssueOpeningLootMatrix", "Global");
			string buttonLabelText = StringUtil.TR("Ok", "Global");
			UIDialogPopupManager.OpenOneButtonDialog(title, description, buttonLabelText, delegate(UIDialogBox x)
			{
				if (response.Result == ConsumeInventoryItemResult.ItemNotFound)
				{
					this.m_throwBoxException = true;
				}
			}, -1, false);
			return;
		}
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		ClientGameManager.Get().RequestUpdateUIState(uiState, playerAccountData.AccountComponent.GetUIState(uiState) + 1, null);
		for (int i = this.m_boxIds.Count - 1; i >= 0; i--)
		{
			if (this.m_item.Id == this.m_boxIds[i])
			{
				if (this.m_item.Count <= 1)
				{
					if (this.m_item.Count != 1)
					{
						this.m_boxIds.RemoveAt(i);
						continue;
					}
					this.m_boxIds.RemoveAt(i);
				}
				break;
			}
		}

		this.m_numBoxes--;
		this.m_outputItems = response.OutputItems;
		InventoryItemRarity inventoryItemRarity = InventoryItemRarity.Common;
		using (List<InventoryItemWithData>.Enumerator enumerator = this.m_outputItems.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				InventoryItemWithData inventoryItemWithData = enumerator.Current;
				InventoryItemRarity rarity = inventoryItemWithData.Item.GetTemplate().Rarity;
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
		this.m_chestAnimator.Play("NanoOpenChest", -1, 0f);
	}

	public void DoOpenChestAnimationEvent()
	{
		bool gotLegendary = false;
		for (int i = 0; i < this.m_outputItems.Count; i++)
		{
			InventoryItemWithData data = this.m_outputItems[i];
			InventoryItemTemplate template = data.Item.GetTemplate();
			if (template.Rarity == InventoryItemRarity.Legendary)
			{
				gotLegendary = true;
			}
			if (template.Index == this.m_template.Index)
			{
				this.m_numBoxes += data.Item.Count;
				if (!this.m_boxIds.Exists((int x) => x == data.Item.Id))
				{
					this.m_boxIds.Add(data.Item.Id);
				}
			}
		}
		this.m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, this.m_item, this.m_template, this.m_boxIds, gotLegendary);
		string text = string.Empty;
		List<InventoryItem> list = new List<InventoryItem>();
		for (int j = 0; j < this.m_outputItems.Count; j++)
		{
			bool flag = false;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].TemplateId == this.m_outputItems[j].Item.TemplateId)
				{
					list[k].Count += this.m_outputItems[j].Item.Count;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(new InventoryItem(this.m_outputItems[j].Item, 0));
			}
		}
		foreach (InventoryItem inventoryItem in list)
		{
			text = text + Environment.NewLine + " - " + inventoryItem.GetTemplate().GetDisplayName();
			if (inventoryItem.Count > 1)
			{
				text = text + " x" + inventoryItem.Count;
			}
		}
		TextConsole.Get().Write(new TextConsole.Message
		{
			Text = string.Format(StringUtil.TR("LockBoxOpenedReceived", "Global"), this.m_template.GetDisplayName(), text),
			MessageType = ConsoleMessageType.SystemMessage
		}, null);
		if (this.m_outputItems.Count == 1)
		{
			this.m_outputItems[0].Item.GetTemplate().Rarity.PlaySound();
			this.m_singleRewardItem.Setup(this.m_outputItems[0].Item, this.m_outputItems[0].Item.GetTemplate(), this.m_outputItems[0].IsoGained > 0, this.m_outputItems[0].IsoGained);
			UIManager.SetGameObjectActive(this.m_singleRewardItem, true, null);
			if (this.m_outputItems[0].IsoGained <= 0)
			{
				CharacterType characterTypeOfItemTemplate = this.GetCharacterTypeOfItemTemplate(this.m_outputItems[0].Item.GetTemplate());
				if (characterTypeOfItemTemplate != CharacterType.None && AnnouncerSounds.GetAnnouncerSounds() != null)
				{
					AnnouncerSounds.GetAnnouncerSounds().PlayLootVOForCharacter(characterTypeOfItemTemplate);
				}
			}
		}
		else
		{
			this.m_accordionRewardCanvasGroup.alpha = 0f;
			UIManager.SetGameObjectActive(this.m_accordionRewardGroup, true, null);
			UIManager.SetGameObjectActive(this.m_accordionRewardRows[0], true, null);
			for (int l = 1; l < this.m_accordionRewardRows.Length; l++)
			{
				UIManager.SetGameObjectActive(this.m_accordionRewardRows[l], false, null);
			}
			Queue<UILockboxRewardItem> queue = new Queue<UILockboxRewardItem>();
			if (this.m_outputItems.Count <= 3)
			{
				if (this.m_outputItems.Count == 3)
				{
					if (this.m_outputItems[0].Item.GetTemplate().Rarity > this.m_outputItems[1].Item.GetTemplate().Rarity)
					{
						InventoryItemWithData value = this.m_outputItems[0];
						this.m_outputItems[0] = this.m_outputItems[1];
						this.m_outputItems[1] = value;
					}
					if (this.m_outputItems[2].Item.GetTemplate().Rarity > this.m_outputItems[1].Item.GetTemplate().Rarity)
					{
						InventoryItemWithData value2 = this.m_outputItems[2];
						this.m_outputItems[2] = this.m_outputItems[1];
						this.m_outputItems[1] = value2;
					}
				}
				this.m_startRowSpacing = 0f;
				this.m_endRowSpacing = 0f;
				this.m_startColSpacing = -700f;
				this.m_endColSpacing = -120f;
				UILockboxRewardItem[] componentsInChildren = this.m_accordionRewardRows[0].GetComponentsInChildren<UILockboxRewardItem>(true);
				for (int m = 0; m < componentsInChildren.Length; m++)
				{
					if (m < this.m_outputItems.Count)
					{
						UIManager.SetGameObjectActive(componentsInChildren[m], true, null);
						componentsInChildren[m].SetSize(700f);
						queue.Enqueue(componentsInChildren[m]);
					}
					else
					{
						UIManager.SetGameObjectActive(componentsInChildren[m], false, null);
					}
				}
			}
			else
			{
				if (this.m_outputItems.Count > 0xA)
				{
					throw new Exception("More than 10 is not supported yet.");
				}
				this.m_startRowSpacing = -350f;
				this.m_endRowSpacing = 0f;
				this.m_startColSpacing = -350f;
				this.m_endColSpacing = 0f;
				UIManager.SetGameObjectActive(this.m_accordionRewardRows[1], true, null);
				int num = Mathf.CeilToInt((float)this.m_outputItems.Count / 2f);
				UILockboxRewardItem[] componentsInChildren2 = this.m_accordionRewardRows[0].GetComponentsInChildren<UILockboxRewardItem>(true);
				for (int n = 0; n < componentsInChildren2.Length; n++)
				{
					if (n < num)
					{
						UIManager.SetGameObjectActive(componentsInChildren2[n], true, null);
						componentsInChildren2[n].SetSize(350f);
						queue.Enqueue(componentsInChildren2[n]);
					}
					else
					{
						UIManager.SetGameObjectActive(componentsInChildren2[n], false, null);
					}
				}
				num = this.m_outputItems.Count / 2;
				componentsInChildren2 = this.m_accordionRewardRows[1].GetComponentsInChildren<UILockboxRewardItem>(true);
				for (int num2 = 0; num2 < componentsInChildren2.Length; num2++)
				{
					if (num2 < num)
					{
						UIManager.SetGameObjectActive(componentsInChildren2[num2], true, null);
						componentsInChildren2[num2].SetSize(350f);
						queue.Enqueue(componentsInChildren2[num2]);
					}
					else
					{
						UIManager.SetGameObjectActive(componentsInChildren2[num2], false, null);
					}
				}
			}
			InventoryItemRarity inventoryItemRarity = InventoryItemRarity.Common;
			CharacterType characterType = CharacterType.None;
			InventoryItemRarity inventoryItemRarity2 = InventoryItemRarity.Common;
			using (List<InventoryItemWithData>.Enumerator enumerator2 = this.m_outputItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					InventoryItemWithData inventoryItemWithData = enumerator2.Current;
					UILockboxRewardItem uilockboxRewardItem = queue.Dequeue();
					uilockboxRewardItem.Setup(inventoryItemWithData.Item, inventoryItemWithData.Item.GetTemplate(), inventoryItemWithData.IsoGained > 0, inventoryItemWithData.IsoGained);
					if (inventoryItemWithData.Item.GetTemplate().Rarity > inventoryItemRarity2)
					{
						inventoryItemRarity2 = inventoryItemWithData.Item.GetTemplate().Rarity;
					}
					if (inventoryItemWithData.IsoGained <= 0)
					{
						InventoryItemTemplate template2 = inventoryItemWithData.Item.GetTemplate();
						if (template2 != null)
						{
							if (characterType != CharacterType.None)
							{
								if (template2.Rarity <= inventoryItemRarity)
								{
									continue;
								}
							}
							CharacterType characterTypeOfItemTemplate2 = this.GetCharacterTypeOfItemTemplate(template2);
							if (characterTypeOfItemTemplate2 != CharacterType.None)
							{
								characterType = characterTypeOfItemTemplate2;
								inventoryItemRarity = template2.Rarity;
							}
						}
					}
				}
			}
			inventoryItemRarity2.PlaySound();
			if (characterType != CharacterType.None)
			{
				if (AnnouncerSounds.GetAnnouncerSounds() != null)
				{
					AnnouncerSounds.GetAnnouncerSounds().PlayLootVOForCharacter(characterType);
				}
			}
			this.m_accordionRewardGroup.spacing = this.m_startRowSpacing;
			for (int num3 = 0; num3 < this.m_accordionRewardRows.Length; num3++)
			{
				this.m_accordionRewardRows[num3].spacing = this.m_startColSpacing;
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
						return CharacterType.None;
					}
				}
				return (CharacterType)template.TypeSpecificData[0];
			}
		}
		return CharacterType.None;
	}

	public void FinishRewardAnimation()
	{
		this.m_isOpening = false;
		this.UpdateOpenButtonText();
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
		bool flag2;
		if (!this.m_isOpening)
		{
			if (!this.m_isLoading)
			{
				flag2 = flag;
				goto IL_A9;
			}
		}
		flag2 = true;
		IL_A9:
		bool flag3 = flag2;
		string text = string.Format(StringUtil.TR("OpenNumber", "Global"), this.m_numBoxes);
		if (this.m_numBoxes == 0)
		{
			this.m_moreBoxesAvailable = false;
			if (this.m_template != null)
			{
				if (this.m_template.Index != 0x203)
				{
					text = StringUtil.TR("Next", "Global");
					this.m_moreBoxesAvailable = true;
					goto IL_1CE;
				}
			}
			if (clientGameManager != null && clientGameManager.IsPlayerAccountDataAvailable())
			{
				List<InventoryItem> items = clientGameManager.GetPlayerAccountData().InventoryComponent.Items;
				for (int i = 0; i < items.Count; i++)
				{
					if (items[i].GetTemplate().Type == InventoryItemType.Lockbox)
					{
						text = StringUtil.TR("Next", "Global");
						this.m_moreBoxesAvailable = true;
						goto IL_1CE;
					}
				}
			}
			IL_1CE:
			bool flag4;
			if (!flag3)
			{
				flag4 = !this.m_moreBoxesAvailable;
			}
			else
			{
				flag4 = true;
			}
			flag3 = flag4;
		}
		this.m_openBtn.selectableButton.SetDisabled(flag3);
		float num = 0f;
		for (int j = 0; j < this.m_openLabels.Length; j++)
		{
			this.m_openLabels[j].text = text;
			this.m_openLabels[j].CalculateLayoutInputHorizontal();
			if (this.m_openLabels[j].GetPreferredValues().x > num)
			{
				num = this.m_openLabels[j].GetPreferredValues().x;
			}
		}
		this.m_openBtn.selectableButton.GetComponent<LayoutElement>().preferredWidth = num + 150f;
		this.m_openBtn.selectableButton.spriteController.ResetMouseState();
	}

	private void Update()
	{
		if (!this.IsVisible)
		{
			return;
		}
		if (this.m_throwBoxException)
		{
			this.m_throwBoxException = false;
			string separator = ", ";
			IEnumerable<int> boxIds = this.m_boxIds;
			
			string text = string.Join(separator, boxIds.Select(((int x) => x.ToString())).ToArray<string>());
			throw new Exception(string.Format("IssueOpeningLootMatrix {0} result={1}\nBoxIds:{2}\nInventory: {3}", new object[]
			{
				this.m_boxIds[0],
				"ItemNotFound",
				text,
				this.m_inventoryItemStr
			}));
		}
		if (this.m_vfxOpenTime > 0f)
		{
			if (!FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.activeSelf)
			{
				if (Time.time > this.m_vfxOpenTime)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.LockboxAppear);
					FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.SetActive(true);
					this.m_vfxOpenTime = -1f;
					goto IL_1D0;
				}
			}
		}
		if (this.m_lockboxSpawnTime > 0f)
		{
			if (FrontEndCharacterSelectBackgroundScene.Get().m_vfxSpawnIn.activeSelf)
			{
				if (Time.time > this.m_lockboxSpawnTime)
				{
					if (this.m_numBoxes > 0)
					{
						if (this.m_template.Index == 0x203)
						{
							UINewUserFlowManager.OnMainLootMatrixDisplayed();
						}
						else
						{
							UINewUserFlowManager.OnSpecialLootMatrixDisplayed();
						}
					}
					UIManager.SetGameObjectActive(this.m_chestAnimator, true, null);
					this.m_lockboxSpawnTime = -1f;
				}
			}
		}
		IL_1D0:
		if (this.m_accordionRewardGroup.IsActive())
		{
			if (this.m_accordionRewardCanvasGroup.alpha < 1f)
			{
				this.m_accordionRewardCanvasGroup.alpha += Time.smoothDeltaTime;
			}
			if (this.m_accordionRewardGroup.spacing < this.m_endRowSpacing)
			{
				this.m_accordionRewardGroup.spacing += (this.m_endRowSpacing - this.m_startRowSpacing) * Time.smoothDeltaTime;
				if (this.m_accordionRewardGroup.spacing > this.m_endRowSpacing)
				{
					this.m_accordionRewardGroup.spacing = this.m_endRowSpacing;
				}
			}
			for (int i = 0; i < this.m_accordionRewardRows.Length; i++)
			{
				if (this.m_accordionRewardRows[i].spacing < this.m_endColSpacing)
				{
					this.m_accordionRewardRows[i].spacing += (this.m_endColSpacing - this.m_startColSpacing) * Time.smoothDeltaTime;
					if (this.m_accordionRewardRows[i].spacing > this.m_endColSpacing)
					{
						this.m_accordionRewardRows[i].spacing = this.m_endColSpacing;
					}
				}
			}
			if (this.m_accordionRewardGroup.spacing >= this.m_endRowSpacing)
			{
				if (this.m_accordionRewardRows[0].spacing >= this.m_endColSpacing)
				{
					this.FinishRewardAnimation();
				}
			}
		}
		if (this.m_isLoading)
		{
			if (this.m_chestAnimator.isInitialized)
			{
				this.m_isLoading = false;
				_SelectableBtn selectableButton = this.m_openBtn.selectableButton;
				bool disabled;
				if (!this.m_isOpening)
				{
					if (this.m_numBoxes <= 0)
					{
						disabled = (this.m_lockboxButtons.Count <= 1);
					}
					else
					{
						disabled = false;
					}
				}
				else
				{
					disabled = true;
				}
				selectableButton.SetDisabled(disabled);
			}
		}
		while (this.m_pendingDuplicateAnimations.Count > 0)
		{
			this.m_pendingDuplicateAnimations.Dequeue().PlayDuplicateAnimation();
		}
	}

	public void NotifyGetFocus()
	{
		this.SetVisible(true);
	}

	public void NotifyLoseFocus()
	{
		this.SetVisible(false);
	}

	public bool IsOpening()
	{
		return this.m_isOpening;
	}
}
