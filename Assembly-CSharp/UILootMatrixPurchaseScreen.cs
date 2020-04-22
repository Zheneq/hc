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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		UIManager.SetGameObjectActive(m_purchaseContainer, false);
		m_cancelBtn.spriteController.callback = CancelBtnClicked;
	}

	public void PackClicked(UILootMatrixPurchaseButton btn)
	{
		if (!(btn != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
			uIPurchaseableItem.m_itemType = PurchaseItemType.LootMatrixPack;
			uIPurchaseableItem.m_lootMatrixPack = btn.PackRef;
			UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.StorePurchased);
			return;
		}
	}

	private void CancelBtnClicked(BaseEventData data)
	{
		SetVisible(false);
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_purchaseContainer, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Setup();
			return;
		}
	}

	private void Setup()
	{
		List<LootMatrixPack> list = new List<LootMatrixPack>();
		int maxMatrixes = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length;
		for (int i = 0; i < GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks.Length; i++)
		{
			LootMatrixPack lootMatrixPack = GameWideData.Get().m_lootMatrixPackData.m_lootMatrixPacks[i];
			bool flag = lootMatrixPack.IsInEvent();
			if (flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!lootMatrixPack.EventHidden)
				{
					goto IL_0086;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (flag || lootMatrixPack.NonEventHidden)
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			goto IL_0086;
			IL_0086:
			list.Add(lootMatrixPack);
		}
		list.Sort((LootMatrixPack x, LootMatrixPack y) => x.SortOrder * maxMatrixes + x.Index - (y.SortOrder * maxMatrixes + y.Index));
		for (int j = 0; j < DisplayPacks.Length; j++)
		{
			if (j < list.Count)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(DisplayPacks[j], true);
				DisplayPacks[j].Setup(list[j]);
			}
			else
			{
				UIManager.SetGameObjectActive(DisplayPacks[j], false);
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
