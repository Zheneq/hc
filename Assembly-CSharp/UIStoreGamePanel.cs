using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreGamePanel : UIStoreBasePanel
{
	public RectTransform m_mainPack;

	public RectTransform m_upgradePacks;

	public _SelectableBtn m_buyMainPackBtn;

	public _SelectableBtn m_upgradePacksBtn;

	public _SelectableBtn m_backBtn;

	public TextMeshProUGUI m_mainPackPrice;

	private UIStoreGameItem[] m_gameItemList;

	private void Awake()
	{
		GridLayoutGroup[] componentsInChildren = base.gameObject.GetComponentsInChildren<GridLayoutGroup>(true);
		if (componentsInChildren != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreGamePanel.Awake()).MethodHandle;
			}
			if (componentsInChildren.Length > 0)
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
				if (HitchDetector.Get() != null)
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
					HitchDetector.Get().AddNewLayoutGroup(componentsInChildren[0]);
				}
			}
		}
		this.Setup();
		this.m_buyMainPackBtn.spriteController.callback = delegate(BaseEventData x)
		{
			if (!GameWideData.Get().m_gamePackData.m_gamePacks.IsNullOrEmpty<GamePack>())
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIStoreGamePanel.<Awake>m__0(BaseEventData)).MethodHandle;
				}
				UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
				uipurchaseableItem.m_itemType = PurchaseItemType.Game;
				uipurchaseableItem.m_gamePack = GameWideData.Get().m_gamePackData.m_gamePacks[0];
				UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
				UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
			}
		};
		this.m_upgradePacksBtn.spriteController.callback = delegate(BaseEventData x)
		{
			UIManager.SetGameObjectActive(this.m_mainPack, false, null);
			UIManager.SetGameObjectActive(this.m_upgradePacks, true, null);
		};
		this.m_backBtn.spriteController.callback = delegate(BaseEventData x)
		{
			UIManager.SetGameObjectActive(this.m_mainPack, true, null);
			UIManager.SetGameObjectActive(this.m_upgradePacks, false, null);
		};
	}

	private void OnDestroy()
	{
	}

	private void OnEnable()
	{
		bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
		UIManager.SetGameObjectActive(this.m_mainPack, !hasPurchasedGame, null);
		UIManager.SetGameObjectActive(this.m_upgradePacks, hasPurchasedGame, null);
		UIManager.SetGameObjectActive(this.m_backBtn, !hasPurchasedGame, null);
	}

	public void Setup()
	{
		this.m_gameItemList = this.m_upgradePacks.GetComponentsInChildren<UIStoreGameItem>();
		GamePackData gamePackData = GameWideData.Get().m_gamePackData;
		for (int i = 0; i < this.m_gameItemList.Length; i++)
		{
			if (i + 1 < gamePackData.m_gamePacks.Length)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreGamePanel.Setup()).MethodHandle;
				}
				this.m_gameItemList[i].Setup(gamePackData.m_gamePacks[i + 1]);
			}
			else
			{
				this.m_gameItemList[i].Setup(null);
			}
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
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		if (!GameWideData.Get().m_gamePackData.m_gamePacks.IsNullOrEmpty<GamePack>())
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
			float num;
			float gamePackPrice = CommerceClient.Get().GetGamePackPrice(gamePackData.m_gamePacks[0].ProductCode, accountCurrency, out num);
			this.m_mainPackPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice, accountCurrency);
		}
		else
		{
			this.m_mainPackPrice.text = string.Empty;
		}
	}

	public void PurchaseGame(UIStoreGameItem selectedGameItem)
	{
		if (selectedGameItem != null)
		{
			UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
			uipurchaseableItem.m_itemType = PurchaseItemType.Game;
			uipurchaseableItem.m_gamePack = selectedGameItem.GetGamePackReference();
			UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
		}
	}
}
