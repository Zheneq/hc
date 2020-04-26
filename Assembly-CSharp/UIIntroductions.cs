using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIntroductions : MonoBehaviour
{
	[Serializable]
	public class IntroductionGridInformation
	{
		public string InternalName;

		public LayoutGroup m_GridLayout;

		public int[] m_InventoryItemIDs;
	}

	public UILockboxRewardItem m_itemPrefab;

	public AccountComponent.UIStateIdentifier UIState = AccountComponent.UIStateIdentifier.NONE;

	public UIIntroductionPage[] m_pages;

	public IntroductionGridInformation[] GridInfos;

	private int currentActivePageIndex;

	public virtual bool AreConditionsMetToAutoDisplay()
	{
		return false;
	}

	public void CheckAutoDisplay()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			return;
		}
		while (true)
		{
			if (!clientGameManager.IsPlayerAccountDataAvailable())
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
			bool flag = true;
			PersistedAccountData playerAccountData = clientGameManager.GetPlayerAccountData();
			if (playerAccountData.AccountComponent.GetUIState(UIState) == 0)
			{
				flag = false;
			}
			if (!flag && AreConditionsMetToAutoDisplay())
			{
				while (true)
				{
					DisplayIntroduction();
					ClientGameManager.Get().RequestUpdateUIState(UIState, 1, null);
					return;
				}
			}
			return;
		}
	}

	protected void PopulateRewards(int[] itemList, LayoutGroup gridLayout)
	{
		for (int i = 0; i < itemList.Length; i++)
		{
			UILockboxRewardItem uILockboxRewardItem = UnityEngine.Object.Instantiate(m_itemPrefab);
			UIManager.ReparentTransform(uILockboxRewardItem.transform, gridLayout.transform);
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemList[i]);
			bool isDuplicate = false;
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
				{
					if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
					{
						int num;
						if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(itemList[i]))
						{
							num = (InventoryWideData.IsOwned(itemTemplate) ? 1 : 0);
						}
						else
						{
							num = 1;
						}
						isDuplicate = ((byte)num != 0);
					}
				}
			}
			uILockboxRewardItem.Setup(new InventoryItem(), itemTemplate, isDuplicate, -1);
			if (uILockboxRewardItem.m_rewardFgs != null)
			{
				Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
				for (int j = 0; j < uILockboxRewardItem.m_rewardFgs.Length; j++)
				{
					uILockboxRewardItem.m_rewardFgs[j].sprite = itemFg;
					UIManager.SetGameObjectActive(uILockboxRewardItem.m_rewardFgs[j], itemFg != null);
				}
			}
		}
	}

	public void SetupGridInfos()
	{
		if (GridInfos == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < GridInfos.Length; i++)
			{
				PopulateRewards(GridInfos[i].m_InventoryItemIDs, GridInfos[i].m_GridLayout);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public virtual void Awake()
	{
		currentActivePageIndex = -1;
		if (m_pages != null)
		{
			for (int i = 0; i < m_pages.Length; i++)
			{
				if (m_pages[i].m_backBtn != null)
				{
					m_pages[i].m_backBtn.spriteController.callback = BackBtnClicked;
				}
				if (m_pages[i].m_nextBtn != null)
				{
					m_pages[i].m_nextBtn.spriteController.callback = NextBtnClicked;
				}
			}
		}
		CheckAutoDisplay();
		SetupGridInfos();
	}

	public void BackBtnClicked(BaseEventData data)
	{
		currentActivePageIndex--;
		if (m_pages == null)
		{
			return;
		}
		for (int i = 0; i < m_pages.Length; i++)
		{
			m_pages[i].SetVisible(i == currentActivePageIndex);
		}
		while (true)
		{
			return;
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		currentActivePageIndex++;
		if (m_pages != null)
		{
			for (int i = 0; i < m_pages.Length; i++)
			{
				if (i == currentActivePageIndex)
				{
					if (m_pages[i].m_backBtn != null)
					{
						UIManager.SetGameObjectActive(m_pages[i].m_backBtn, true);
					}
					m_pages[i].SetVisible(true);
				}
				else
				{
					m_pages[i].SetVisible(false);
				}
			}
		}
		if (currentActivePageIndex < m_pages.Length)
		{
			return;
		}
		while (true)
		{
			UINewUserFlowManager.OnChapterMoreInfoClosed();
			return;
		}
	}

	public virtual void DisplayIntroduction(int pageNum = 0)
	{
		if (m_pages != null && m_pages.Length > 0)
		{
			if (pageNum >= m_pages.Length)
			{
				pageNum = m_pages.Length - 1;
			}
			currentActivePageIndex = pageNum;
			if (m_pages[pageNum].m_backBtn != null)
			{
				UIManager.SetGameObjectActive(m_pages[pageNum].m_backBtn, false);
			}
			m_pages[pageNum].SetVisible(true);
		}
		UILockboxRewardItem[] componentsInChildren = base.gameObject.GetComponentsInChildren<UILockboxRewardItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			bool flag = false;
			InventoryItemTemplate template = componentsInChildren[i].GetTemplate();
			if (template != null)
			{
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GetPlayerAccountData() != null)
					{
						if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
						{
							flag = InventoryWideData.IsOwned(template);
						}
					}
				}
			}
			if (flag && componentsInChildren[i].m_ownedIcon != null)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i].m_ownedIcon, true);
			}
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

	private void Update()
	{
		if (-1 >= currentActivePageIndex || currentActivePageIndex >= m_pages.Length)
		{
			return;
		}
		while (true)
		{
			if (m_pages[currentActivePageIndex].m_nextBtn != null)
			{
				if (InputManager.Get().GetAcceptButtonDown())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							NextBtnClicked(null);
							return;
						}
					}
				}
			}
			if (!(m_pages[currentActivePageIndex].m_backBtn != null))
			{
				return;
			}
			while (true)
			{
				if (InputManager.Get().GetCancelButtonDown())
				{
					BackBtnClicked(null);
				}
				return;
			}
		}
	}
}
