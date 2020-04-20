using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILootMatrixPurchaseScreen : MonoBehaviour
{
	public _SelectableBtn m_cancelBtn;

	public LayoutGroup m_purchaseButtonContainer;

	public UILootMatrixPurchaseButton[] DisplayPacks;

	public RectTransform m_purchaseContainer;

	private static UILootMatrixPurchaseScreen s_instance;

	public static UILootMatrixPurchaseScreen Get()
	{
		return UILootMatrixPurchaseScreen.s_instance;
	}

	private void Awake()
	{
		UILootMatrixPurchaseScreen.s_instance = this;
		UIManager.SetGameObjectActive(this.m_purchaseContainer, false, null);
		this.m_cancelBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CancelBtnClicked);
	}

	public void PackClicked(UILootMatrixPurchaseButton btn)
	{
		if (btn != null)
		{
			UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
			uipurchaseableItem.m_itemType = PurchaseItemType.LootMatrixPack;
			uipurchaseableItem.m_lootMatrixPack = btn.PackRef;
			UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem, null);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
		}
	}

	private void CancelBtnClicked(BaseEventData data)
	{
		this.SetVisible(false);
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_purchaseContainer, visible, null);
		if (visible)
		{
			this.Setup();
		}
	}

	private void Setup()
	{
		List<LootMatrixPack> list = new List<LootMatrixPack>();
		int maxMatrixes = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length;
		int i = 0;
		while (i < GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length)
		{
			LootMatrixPack lootMatrixPack = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks[i];
			bool flag = lootMatrixPack.IsInEvent();
			if (!flag)
			{
				goto IL_70;
			}
			if (!lootMatrixPack.EventHidden)
			{
				goto IL_86;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				goto IL_70;
			}
			IL_8D:
			i++;
			continue;
			IL_70:
			if (flag || lootMatrixPack.NonEventHidden)
			{
				goto IL_8D;
			}
			IL_86:
			list.Add(lootMatrixPack);
			goto IL_8D;
		}
		list.Sort((LootMatrixPack x, LootMatrixPack y) => x.SortOrder * maxMatrixes + x.Index - (y.SortOrder * maxMatrixes + y.Index));
		for (int j = 0; j < this.DisplayPacks.Length; j++)
		{
			if (j < list.Count)
			{
				UIManager.SetGameObjectActive(this.DisplayPacks[j], true, null);
				this.DisplayPacks[j].Setup(list[j]);
			}
			else
			{
				UIManager.SetGameObjectActive(this.DisplayPacks[j], false, null);
			}
		}
	}
}
