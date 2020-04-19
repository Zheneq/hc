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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.CheckAutoDisplay()).MethodHandle;
			}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.DisplayIntroduction(0);
					ClientGameManager.Get().RequestUpdateUIState(this.UIState, 1, null);
				}
				return;
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.PopulateRewards(int[], LayoutGroup)).MethodHandle;
				}
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
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
					if (ClientGameManager.Get().GetPlayerAccountData().InventoryComponent != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						bool flag;
						if (!ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.HasItem(itemList[i]))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Sprite itemFg = InventoryWideData.GetItemFg(itemTemplate);
				for (int j = 0; j < uilockboxRewardItem.m_rewardFgs.Length; j++)
				{
					uilockboxRewardItem.m_rewardFgs[j].sprite = itemFg;
					UIManager.SetGameObjectActive(uilockboxRewardItem.m_rewardFgs[j], itemFg != null, null);
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
		}
	}

	public void SetupGridInfos()
	{
		if (this.GridInfos != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.SetupGridInfos()).MethodHandle;
			}
			for (int i = 0; i < this.GridInfos.Length; i++)
			{
				this.PopulateRewards(this.GridInfos[i].m_InventoryItemIDs, this.GridInfos[i].m_GridLayout);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public virtual void Awake()
	{
		this.currentActivePageIndex = -1;
		if (this.m_pages != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.Awake()).MethodHandle;
			}
			for (int i = 0; i < this.m_pages.Length; i++)
			{
				if (this.m_pages[i].m_backBtn != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_pages[i].m_backBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BackBtnClicked);
				}
				if (this.m_pages[i].m_nextBtn != null)
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
					this.m_pages[i].m_nextBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NextBtnClicked);
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.BackBtnClicked(BaseEventData)).MethodHandle;
			}
		}
	}

	public void NextBtnClicked(BaseEventData data)
	{
		this.currentActivePageIndex++;
		if (this.m_pages != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.NextBtnClicked(BaseEventData)).MethodHandle;
			}
			for (int i = 0; i < this.m_pages.Length; i++)
			{
				if (i == this.currentActivePageIndex)
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
					if (this.m_pages[i].m_backBtn != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			UINewUserFlowManager.OnChapterMoreInfoClosed();
		}
	}

	public virtual void DisplayIntroduction(int pageNum = 0)
	{
		if (this.m_pages != null && this.m_pages.Length > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.DisplayIntroduction(int)).MethodHandle;
			}
			if (pageNum >= this.m_pages.Length)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
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
					for (;;)
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
						for (;;)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(componentsInChildren[i].m_ownedIcon, true, null);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void Update()
	{
		if (-1 < this.currentActivePageIndex && this.currentActivePageIndex < this.m_pages.Length)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIIntroductions.Update()).MethodHandle;
			}
			if (this.m_pages[this.currentActivePageIndex].m_nextBtn != null)
			{
				for (;;)
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.NextBtnClicked(null);
					return;
				}
			}
			if (this.m_pages[this.currentActivePageIndex].m_backBtn != null)
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
