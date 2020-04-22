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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
					switch (6)
					{
					case 0:
						continue;
					}
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
				while (true)
				{
					switch (5)
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
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
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
						int num;
						if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(itemList[i]))
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
				for (int j = 0; j < uILockboxRewardItem.m_rewardFgs.Length; j++)
				{
					uILockboxRewardItem.m_rewardFgs[j].sprite = itemFg;
					UIManager.SetGameObjectActive(uILockboxRewardItem.m_rewardFgs[j], itemFg != null);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (1)
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
			for (int i = 0; i < m_pages.Length; i++)
			{
				if (m_pages[i].m_backBtn != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_pages[i].m_backBtn.spriteController.callback = BackBtnClicked;
				}
				if (m_pages[i].m_nextBtn != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_pages[i].m_nextBtn.spriteController.callback = NextBtnClicked;
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		currentActivePageIndex++;
		if (m_pages != null)
		{
			while (true)
			{
				switch (5)
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
			for (int i = 0; i < m_pages.Length; i++)
			{
				if (i == currentActivePageIndex)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_pages[i].m_backBtn != null)
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
			switch (4)
			{
			case 0:
				continue;
			}
			UINewUserFlowManager.OnChapterMoreInfoClosed();
			return;
		}
	}

	public virtual void DisplayIntroduction(int pageNum = 0)
	{
		if (m_pages != null && m_pages.Length > 0)
		{
			while (true)
			{
				switch (1)
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
			if (pageNum >= m_pages.Length)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ClientGameManager.Get() != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (ClientGameManager.Get().GetPlayerAccountData() != null)
					{
						while (true)
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
							flag = InventoryWideData.IsOwned(template);
						}
					}
				}
			}
			if (flag && componentsInChildren[i].m_ownedIcon != null)
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_pages[currentActivePageIndex].m_nextBtn != null)
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
				switch (2)
				{
				case 0:
					continue;
				}
				if (InputManager.Get().GetCancelButtonDown())
				{
					BackBtnClicked(null);
				}
				return;
			}
		}
	}
}
