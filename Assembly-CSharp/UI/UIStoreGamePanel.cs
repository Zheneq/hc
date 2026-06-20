using TMPro;
using UnityEngine;
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
			if (componentsInChildren.Length > 0)
			{
				if (HitchDetector.Get() != null)
				{
					HitchDetector.Get().AddNewLayoutGroup(componentsInChildren[0]);
				}
			}
		}
		Setup();
		m_buyMainPackBtn.spriteController.callback = delegate
		{
			if (!GameWideData.Get().m_gamePackData.m_gamePacks.IsNullOrEmpty())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						UIPurchaseableItem item = new UIPurchaseableItem
						{
							m_itemType = PurchaseItemType.Game,
							m_gamePack = GameWideData.Get().m_gamePackData.m_gamePacks[0]
						};
						UIStorePanel.Get().OpenPurchaseDialog(item);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
						return;
					}
					}
				}
			}
		};
		m_upgradePacksBtn.spriteController.callback = delegate
		{
			UIManager.SetGameObjectActive(m_mainPack, false);
			UIManager.SetGameObjectActive(m_upgradePacks, true);
		};
		m_backBtn.spriteController.callback = delegate
		{
			UIManager.SetGameObjectActive(m_mainPack, true);
			UIManager.SetGameObjectActive(m_upgradePacks, false);
		};
	}

	private void OnDestroy()
	{
	}

	private void OnEnable()
	{
		bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
		UIManager.SetGameObjectActive(m_mainPack, !hasPurchasedGame);
		UIManager.SetGameObjectActive(m_upgradePacks, hasPurchasedGame);
		UIManager.SetGameObjectActive(m_backBtn, !hasPurchasedGame);
	}

	public void Setup()
	{
		m_gameItemList = m_upgradePacks.GetComponentsInChildren<UIStoreGameItem>();
		GamePackData gamePackData = GameWideData.Get().m_gamePackData;
		for (int i = 0; i < m_gameItemList.Length; i++)
		{
			if (i + 1 < gamePackData.m_gamePacks.Length)
			{
				m_gameItemList[i].Setup(gamePackData.m_gamePacks[i + 1]);
			}
			else
			{
				m_gameItemList[i].Setup(null);
			}
		}
		while (true)
		{
			HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
			string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
			if (!GameWideData.Get().m_gamePackData.m_gamePacks.IsNullOrEmpty())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						float originalPrice;
						float gamePackPrice = CommerceClient.Get().GetGamePackPrice(gamePackData.m_gamePacks[0].ProductCode, accountCurrency, out originalPrice);
						m_mainPackPrice.text = UIStorePanel.GetLocalizedPriceString(gamePackPrice, accountCurrency);
						return;
					}
					}
				}
			}
			m_mainPackPrice.text = string.Empty;
			return;
		}
	}

	public void PurchaseGame(UIStoreGameItem selectedGameItem)
	{
		if (selectedGameItem != null)
		{
			UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
			uIPurchaseableItem.m_itemType = PurchaseItemType.Game;
			uIPurchaseableItem.m_gamePack = selectedGameItem.GetGamePackReference();
			UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
		}
	}
}
