using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIIntroductions : MonoBehaviour
{
	public UILockboxRewardItem m_itemPrefab;

	public AccountComponent.UIStateIdentifier UIState = AccountComponent.UIStateIdentifier.NONE;

	public UIIntroductionPage[] m_pages;

	public UIIntroductions.IntroductionGridInformation[] GridInfos;

	private int currentActivePageIndex;

	public virtual bool AreConditionsMetToAutoDisplay()
	{
		return false;
	}

	public void CheckAutoDisplay()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager == null))
		{
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				bool flag = true;
				PersistedAccountData playerAccountData = clientGameManager.GetPlayerAccountData();
				if (playerAccountData.AccountComponent.GetUIState(this.UIState) == 0)
				{
					flag = false;
				}
				if (!flag && this.AreConditionsMetToAutoDisplay())
				{
					this.DisplayIntroduction(0);
					ClientGameManager.Get().RequestUpdateUIState(this.UIState, 1, null);
				}
				return;
			}
		}
	}

	protected void PopulateRewards(int[] itemList, LayoutGroup gridLayout)
	{
		for (int i = 0; i < itemList.Length; i++)
		{
			UILockboxRewardItem uilockboxRewardItem = UnityEngine.Object.Instantiate<UILockboxRewardItem>(this.m_itemPrefab);
			UIManager.ReparentTransform(uilockboxRewardItem.transform, gridLayout.transform);
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemList[i]);
			bool isDuplicate = false;
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
				{
					if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
					{
						bool flag;
						if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(itemList[i]))
						{
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
			uilockboxRewardItem.Setup(new InventoryItem(), itemTemplate, isDuplicate, -1);
			if (uilockboxRewardItem.m_rewardFgs != null)
			{
				Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
				for (int j = 0; j < uilockboxRewardItem.m_rewardFgs.Length; j++)
				{
					uilockboxRewardItem.m_rewardFgs[j].sprite = itemFg;
					UIManager.SetGameObjectActive(uilockboxRewardItem.m_rewardFgs[j], itemFg != null, null);
				}
			}
		}
	}

	public void SetupGridInfos()
	{
		if (this.GridInfos != null)
		{
			for (int i = 0; i < this.GridInfos.Length; i++)
			{
				this.PopulateRewards(this.GridInfos[i].m_InventoryItemIDs, this.GridInfos[i].m_GridLayout);
			}
		}
	}

	public virtual void Awake()
	{
		this.currentActivePageIndex = -1;
		if (this.m_pages != null)
		{
			for (int i = 0; i < this.m_pages.Length; i++)
			{
				if (this.m_pages[i].m_backBtn != null)
				{
					this.m_pages[i].m_backBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BackBtnClicked);
				}
				if (this.m_pages[i].m_nextBtn != null)
				{
					this.m_pages[i].m_nextBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NextBtnClicked);
				}
			}
		}
		this.CheckAutoDisplay();
		this.SetupGridInfos();
	}

	public void BackBtnClicked(BaseEventData data)
	{
		this.currentActivePageIndex--;
		if (this.m_pages != null)
		{
			for (int i = 0; i < this.m_pages.Length; i++)
			{
				this.m_pages[i].SetVisible(i == this.currentActivePageIndex);
			}
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		this.currentActivePageIndex++;
		if (this.m_pages != null)
		{
			for (int i = 0; i < this.m_pages.Length; i++)
			{
				if (i == this.currentActivePageIndex)
				{
					if (this.m_pages[i].m_backBtn != null)
					{
						UIManager.SetGameObjectActive(this.m_pages[i].m_backBtn, true, null);
					}
					this.m_pages[i].SetVisible(true);
				}
				else
				{
					this.m_pages[i].SetVisible(false);
				}
			}
		}
		if (this.currentActivePageIndex >= this.m_pages.Length)
		{
			UINewUserFlowManager.OnChapterMoreInfoClosed();
		}
	}

	public virtual void DisplayIntroduction(int pageNum = 0)
	{
		if (this.m_pages != null && this.m_pages.Length > 0)
		{
			if (pageNum >= this.m_pages.Length)
			{
				pageNum = this.m_pages.Length - 1;
			}
			this.currentActivePageIndex = pageNum;
			if (this.m_pages[pageNum].m_backBtn != null)
			{
				UIManager.SetGameObjectActive(this.m_pages[pageNum].m_backBtn, false, null);
			}
			this.m_pages[pageNum].SetVisible(true);
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
				UIManager.SetGameObjectActive(componentsInChildren[i].m_ownedIcon, true, null);
			}
		}
	}

	private void Update()
	{
		if (-1 < this.currentActivePageIndex && this.currentActivePageIndex < this.m_pages.Length)
		{
			if (this.m_pages[this.currentActivePageIndex].m_nextBtn != null)
			{
				if (InputManager.Get().GetAcceptButtonDown())
				{
					this.NextBtnClicked(null);
					return;
				}
			}
			if (this.m_pages[this.currentActivePageIndex].m_backBtn != null)
			{
				if (InputManager.Get().GetCancelButtonDown())
				{
					this.BackBtnClicked(null);
				}
			}
		}
	}

	[Serializable]
	public class IntroductionGridInformation
	{
		public string InternalName;

		public LayoutGroup m_GridLayout;

		public int[] m_InventoryItemIDs;
	}
}
