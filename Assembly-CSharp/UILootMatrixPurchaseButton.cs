using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixPurchaseButton : MonoBehaviour
{
	public TextMeshProUGUI m_packCount;

	public Image m_packImage;

	public TextMeshProUGUI m_priceAmount;

	public TextMeshProUGUI m_discountedAmount;

	public _SelectableBtn m_clickBtn;

	public TextMeshProUGUI m_eventText;

	public _SelectableBtn m_viewContentBtn;

	public LootMatrixPack PackRef { get; private set; }

	private void Awake()
	{
		this.m_viewContentBtn.spriteController.callback = delegate(BaseEventData contentData)
		{
			if (this.PackRef != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixPurchaseButton.<Awake>m__0(BaseEventData)).MethodHandle;
				}
				List<InventoryItemTemplate> list = new List<InventoryItemTemplate>();
				if (this.PackRef.IsInEvent())
				{
					for (int i = 0; i < this.PackRef.BonusMatrixes.Length; i++)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(this.PackRef.BonusMatrixes[i].LootMatrixId);
						if (itemTemplate.TypeSpecificData.Length > 1 && itemTemplate.TypeSpecificData[1] == 1)
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
							list.Add(itemTemplate);
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
				}
				if (list.Count > 0)
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
					UILootMatrixContentViewer.Get().Setup(list.ToArray(), false);
					UILootMatrixContentViewer.Get().SetVisible(true);
				}
			}
		};
	}

	public void ButtonClicked(BaseEventData data)
	{
		UILootMatrixPurchaseScreen.Get().PackClicked(this);
		this.m_clickBtn.spriteController.ForceSetPointerEntered(false);
	}

	public void Setup(LootMatrixPack pack)
	{
		bool flag = pack.IsInEvent();
		this.PackRef = pack;
		this.m_clickBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ButtonClicked);
		int num = pack.NumberOfMatrixes;
		if (num == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UILootMatrixPurchaseButton.Setup(LootMatrixPack)).MethodHandle;
			}
			for (int i = 0; i < pack.BonusMatrixes.Length; i++)
			{
				num += pack.BonusMatrixes[i].NumberOfMatrixes;
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
		}
		this.m_packCount.text = num.ToString();
		Image packImage = this.m_packImage;
		Sprite sprite;
		if (flag)
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
			sprite = pack.EventPackSprite;
		}
		else
		{
			sprite = pack.LootMatrixPackSprite;
		}
		packImage.sprite = sprite;
		string accountCurrency = HydrogenConfig.Get().Ticket.AccountCurrency;
		float lootMatrixPackPrice = CommerceClient.Get().GetLootMatrixPackPrice(pack.ProductCode, accountCurrency);
		this.m_priceAmount.text = UIStorePanel.GetLocalizedPriceString(lootMatrixPackPrice, accountCurrency);
		this.m_eventText.text = pack.GetEventText();
		UIManager.SetGameObjectActive(this.m_eventText, flag, null);
		bool flag2 = false;
		if (this.PackRef.IsInEvent())
		{
			for (int j = 0; j < this.PackRef.BonusMatrixes.Length; j++)
			{
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(this.PackRef.BonusMatrixes[j].LootMatrixId);
				if (itemTemplate.TypeSpecificData.Length > 1)
				{
					flag2 = (itemTemplate.TypeSpecificData[1] == 1);
					if (flag2)
					{
						goto IL_191;
					}
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_191:
		UIManager.SetGameObjectActive(this.m_viewContentBtn, flag2, null);
		float price = pack.Prices.GetPrice(accountCurrency);
		if (price == lootMatrixPackPrice)
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
			this.m_discountedAmount.text = string.Empty;
		}
		else
		{
			this.m_discountedAmount.text = UIStorePanel.GetLocalizedPriceString(price, accountCurrency);
		}
	}
}
