using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreFreelancerPanel : UIStoreBasePanel
{
	public GridLayoutGroup m_heroListContainer;

	public UIStorePageIndicator m_pageItemPrefab;

	public GridLayoutGroup m_pageListContainer;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	private int m_numPages;

	private int m_currentPage;

	private List<UIStorePageIndicator> m_pageMarkers;

	private List<UIStorePurchaseFreelancerItem> m_freeLancerSlots;

	private List<UIStoreFreelancerPanel.CharacterData> m_visibleFreelancers;

	private void Awake()
	{
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_heroListContainer);
		}
		this.m_freeLancerSlots = new List<UIStorePurchaseFreelancerItem>(this.m_heroListContainer.GetComponentsInChildren<UIStorePurchaseFreelancerItem>(true));
		using (List<UIStorePurchaseFreelancerItem>.Enumerator enumerator = this.m_freeLancerSlots.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIStorePurchaseFreelancerItem uistorePurchaseFreelancerItem = enumerator.Current;
				uistorePurchaseFreelancerItem.m_hitBox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				UIManager.SetGameObjectActive(uistorePurchaseFreelancerItem, false, null);
				StaggerComponent.SetStaggerComponent(uistorePurchaseFreelancerItem.gameObject, true, true);
			}
		}
		this.m_visibleFreelancers = new List<UIStoreFreelancerPanel.CharacterData>();
		this.m_pageMarkers = new List<UIStorePageIndicator>();
		this.m_prevPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnPrevPage);
		this.m_nextPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnNextPage);
	}

	private void Start()
	{
		bool enableHiddenCharacters = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		for (int i = 0; i < GameWideData.Get().m_characterResourceLinks.Length; i++)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().m_characterResourceLinks[i];
			if (characterResourceLink != null)
			{
				if (characterResourceLink.m_characterType != CharacterType.None)
				{
					if (characterResourceLink.m_characterType != CharacterType.PunchingDummy)
					{
						if (!characterResourceLink.m_characterType.IsWillFill() && characterResourceLink.m_characterType != CharacterType.TestFreelancer1)
						{
							if (characterResourceLink.m_characterType != CharacterType.TestFreelancer2)
							{
								if (!enableHiddenCharacters)
								{
									if (characterResourceLink.m_isHidden)
									{
										goto IL_DE;
									}
								}
								UIStoreFreelancerPanel.CharacterData item = new UIStoreFreelancerPanel.CharacterData(characterResourceLink);
								this.m_visibleFreelancers.Add(item);
							}
						}
					}
				}
			}
			IL_DE:;
		}
		List<UIStoreFreelancerPanel.CharacterData> visibleFreelancers = this.m_visibleFreelancers;
		
		visibleFreelancers.Sort(((UIStoreFreelancerPanel.CharacterData a, UIStoreFreelancerPanel.CharacterData b) => a.ResourceLink.GetDisplayName().CompareTo(b.ResourceLink.GetDisplayName())));
		this.m_numPages = Mathf.CeilToInt((float)(this.m_visibleFreelancers.Count / this.m_freeLancerSlots.Count)) + 1;
		for (int j = 0; j < this.m_numPages; j++)
		{
			UIStorePageIndicator uistorePageIndicator = UnityEngine.Object.Instantiate<UIStorePageIndicator>(this.m_pageItemPrefab);
			uistorePageIndicator.transform.SetParent(this.m_pageListContainer.transform);
			uistorePageIndicator.transform.localScale = Vector3.one;
			uistorePageIndicator.transform.localPosition = Vector3.zero;
			uistorePageIndicator.SetSelected(j == 0);
			uistorePageIndicator.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageClicked);
			uistorePageIndicator.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			uistorePageIndicator.SetPageNumber(j + 1);
			this.m_pageMarkers.Add(uistorePageIndicator);
		}
		this.m_nextPage.transform.parent.SetAsLastSibling();
		this.SetCharacterPageList(this.m_currentPage);
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesChange;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesChange;
		}
	}

	private void OnEnable()
	{
		this.SetCharacterPageList(this.m_currentPage);
	}

	private void SetCharacterPageList(int pageNumber)
	{
		int num = pageNumber * this.m_freeLancerSlots.Count;
		for (int i = 0; i < this.m_freeLancerSlots.Count; i++)
		{
			if (num + i < this.m_visibleFreelancers.Count)
			{
				UIStoreFreelancerPanel.CharacterData characterData = this.m_visibleFreelancers[num + i];
				characterData.RefreshProgress(false);
				this.m_freeLancerSlots[i].Setup(characterData.ResourceLink, characterData.CurrentProgress, characterData.TotalProgress);
			}
			else
			{
				this.m_freeLancerSlots[i].Setup(null, 0, 0);
			}
		}
		this.m_currentPage = pageNumber;
		for (int j = 0; j < this.m_pageMarkers.Count; j++)
		{
			this.m_pageMarkers[j].SetSelected(j == this.m_currentPage);
		}
	}

	public void FreeLancerClicked(UIStorePurchaseFreelancerItem clickedHero)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectSkinChoice);
		for (int i = 0; i < this.m_freeLancerSlots.Count; i++)
		{
			if (clickedHero == this.m_freeLancerSlots[i])
			{
				UIStorePanel.Get().OpenFreelancerPage(this.m_freeLancerSlots[i].GetCharLink());
				break;
			}
		}
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		for (int i = 0; i < this.m_pageMarkers.Count; i++)
		{
			if (this.m_pageMarkers[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.SetCharacterPageList(i);
				IL_6E:
				this.FreeLancerClicked(null);
				return;
			}
		}
		goto IL_6E;
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.scrollDelta.y > 0f)
		{
			this.ClickedOnPrevPage(null);
		}
		else if (pointerEventData.scrollDelta.y < 0f)
		{
			this.ClickedOnNextPage(null);
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (this.m_currentPage - 1 < 0)
		{
			return;
		}
		this.m_currentPage--;
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.SetCharacterPageList(this.m_currentPage);
		this.FreeLancerClicked(null);
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (this.m_currentPage + 1 >= this.m_numPages)
		{
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.m_currentPage++;
		this.SetCharacterPageList(this.m_currentPage);
		this.FreeLancerClicked(null);
	}

	protected void UpdatePage(CharacterType characterType = CharacterType.None)
	{
		if (characterType == CharacterType.None)
		{
			List<UIStoreFreelancerPanel.CharacterData> visibleFreelancers = this.m_visibleFreelancers;
			
			visibleFreelancers.ForEach(delegate(UIStoreFreelancerPanel.CharacterData v)
				{
					v.QueueProgressRefresh();
				});
		}
		else
		{
			UIStoreFreelancerPanel.CharacterData characterData = this.m_visibleFreelancers.SingleOrDefault((UIStoreFreelancerPanel.CharacterData v) => v.ResourceLink.m_characterType == characterType);
			if (characterData == null)
			{
				return;
			}
			characterData.QueueProgressRefresh();
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.SetCharacterPageList(this.m_currentPage);
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		this.UpdatePage(newData.CharacterType);
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		this.UpdatePage(CharacterType.None);
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.UpdatePage(CharacterType.None);
	}

	private class CharacterData
	{
		private List<GameBalanceVars.PlayerUnlockable> m_collectionItems;

		private bool m_pendingUpdate;

		public CharacterData(CharacterResourceLink charLink)
		{
			this.ResourceLink = charLink;
			this.m_pendingUpdate = true;
			this.m_collectionItems = new List<GameBalanceVars.PlayerUnlockable>();
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			foreach (GameBalanceVars.PlayerBanner playerBanner in gameBalanceVars.PlayerBanners)
			{
				if (playerBanner.m_relatedCharacter == charLink.m_characterType)
				{
					this.m_collectionItems.Add(playerBanner);
				}
			}
			foreach (GameBalanceVars.PlayerTitle playerTitle in gameBalanceVars.PlayerTitles)
			{
				if (playerTitle.m_relatedCharacter == charLink.m_characterType)
				{
					this.m_collectionItems.Add(playerTitle);
				}
			}
			foreach (GameBalanceVars.TauntUnlockData item in gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).tauntUnlockData)
			{
				this.m_collectionItems.Add(item);
			}
			foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).skinUnlockData)
			{
				foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in skinUnlockData2.patternUnlockData)
				{
					foreach (GameBalanceVars.ColorUnlockData item2 in patternUnlockData2.colorUnlockData)
					{
						this.m_collectionItems.Add(item2);
					}
				}
			}
			foreach (GameBalanceVars.AbilityVfxUnlockData item3 in gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).abilityVfxUnlockData)
			{
				this.m_collectionItems.Add(item3);
			}
			AbilityData component = charLink.ActorDataPrefab.GetComponent<AbilityData>();
			int num2 = 0;
			while (num2 < 0xE)
			{
				List<AbilityMod> availableModsForAbility;
				switch (num2)
				{
				case 0:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability0);
					goto IL_2A5;
				case 1:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability1);
					goto IL_2A5;
				case 2:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability2);
					goto IL_2A5;
				case 3:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability3);
					goto IL_2A5;
				case 4:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability4);
					goto IL_2A5;
				case 5:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability5);
					goto IL_2A5;
				case 6:
					availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability6);
					goto IL_2A5;
				}
				IL_2E8:
				num2++;
				continue;
				IL_2A5:
				for (int num3 = 0; num3 < availableModsForAbility.Count; num3++)
				{
					this.m_collectionItems.Add(availableModsForAbility[num3].GetAbilityModUnlockData(charLink.m_characterType, num2));
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					goto IL_2E8;
				}
			}
		}

		public CharacterResourceLink ResourceLink { get; private set; }

		public int CurrentProgress { get; private set; }

		public int TotalProgress { get; private set; }

		public void RefreshProgress(bool force = false)
		{
			if (!force)
			{
				if (!this.m_pendingUpdate)
				{
					return;
				}
			}
			LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
			bool enableHiddenCharacters = gameplayOverrides.EnableHiddenCharacters;
			int num = 0;
			this.TotalProgress = num;
			this.CurrentProgress = num;
			int i = 0;
			while (i < this.m_collectionItems.Count)
			{
				if (!(this.m_collectionItems[i] is GameBalanceVars.ColorUnlockData))
				{
					goto IL_CE;
				}
				if (gameplayOverrides.IsColorAllowed((CharacterType)this.m_collectionItems[i].Index1, this.m_collectionItems[i].Index2, this.m_collectionItems[i].Index3, this.m_collectionItems[i].ID))
				{
					goto IL_CE;
				}
				IL_25D:
				i++;
				continue;
				IL_CE:
				if (this.m_collectionItems[i].IsOwned())
				{
					this.CurrentProgress++;
					this.TotalProgress++;
					goto IL_25D;
				}
				if (enableHiddenCharacters)
				{
					this.TotalProgress++;
					goto IL_25D;
				}
				if (!this.m_collectionItems[i].m_isHidden)
				{
					if (GameBalanceVarsExtensions.MeetsVisibilityConditions(this.m_collectionItems[i]))
					{
						if (this.m_collectionItems[i] is GameBalanceVars.ColorUnlockData)
						{
							if (GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)this.m_collectionItems[i].Index1).skinUnlockData[this.m_collectionItems[i].Index2].m_isHidden)
							{
							}
							else if (GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)this.m_collectionItems[i].Index1).skinUnlockData[this.m_collectionItems[i].Index2].patternUnlockData[this.m_collectionItems[i].Index3].m_isHidden)
							{
							}
							else
							{
								this.TotalProgress++;
							}
						}
						else
						{
							this.TotalProgress++;
						}
					}
				}
				goto IL_25D;
			}
			this.m_pendingUpdate = false;
		}

		public void QueueProgressRefresh()
		{
			this.m_pendingUpdate = true;
		}
	}
}
