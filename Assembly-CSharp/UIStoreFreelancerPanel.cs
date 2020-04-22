using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStoreFreelancerPanel : UIStoreBasePanel
{
	private class CharacterData
	{
		private List<GameBalanceVars.PlayerUnlockable> m_collectionItems;

		private bool m_pendingUpdate;

		public CharacterResourceLink ResourceLink
		{
			get;
			private set;
		}

		public int CurrentProgress
		{
			get;
			private set;
		}

		public int TotalProgress
		{
			get;
			private set;
		}

		public CharacterData(CharacterResourceLink charLink)
		{
			ResourceLink = charLink;
			m_pendingUpdate = true;
			m_collectionItems = new List<GameBalanceVars.PlayerUnlockable>();
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			GameBalanceVars.PlayerBanner[] playerBanners = gameBalanceVars.PlayerBanners;
			foreach (GameBalanceVars.PlayerBanner playerBanner in playerBanners)
			{
				if (playerBanner.m_relatedCharacter == charLink.m_characterType)
				{
					m_collectionItems.Add(playerBanner);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				GameBalanceVars.PlayerTitle[] playerTitles = gameBalanceVars.PlayerTitles;
				foreach (GameBalanceVars.PlayerTitle playerTitle in playerTitles)
				{
					if (playerTitle.m_relatedCharacter == charLink.m_characterType)
					{
						m_collectionItems.Add(playerTitle);
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					GameBalanceVars.TauntUnlockData[] tauntUnlockData = gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).tauntUnlockData;
					foreach (GameBalanceVars.TauntUnlockData item in tauntUnlockData)
					{
						m_collectionItems.Add(item);
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						GameBalanceVars.SkinUnlockData[] skinUnlockData = gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).skinUnlockData;
						foreach (GameBalanceVars.SkinUnlockData skinUnlockData2 in skinUnlockData)
						{
							GameBalanceVars.PatternUnlockData[] patternUnlockData = skinUnlockData2.patternUnlockData;
							foreach (GameBalanceVars.PatternUnlockData patternUnlockData2 in patternUnlockData)
							{
								GameBalanceVars.ColorUnlockData[] colorUnlockData = patternUnlockData2.colorUnlockData;
								foreach (GameBalanceVars.ColorUnlockData item2 in colorUnlockData)
								{
									m_collectionItems.Add(item2);
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										goto end_IL_0164;
									}
									continue;
									end_IL_0164:
									break;
								}
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									goto end_IL_017c;
								}
								continue;
								end_IL_017c:
								break;
							}
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							GameBalanceVars.AbilityVfxUnlockData[] abilityVfxUnlockData = gameBalanceVars.GetCharacterUnlockData(charLink.m_characterType).abilityVfxUnlockData;
							foreach (GameBalanceVars.AbilityVfxUnlockData item3 in abilityVfxUnlockData)
							{
								m_collectionItems.Add(item3);
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								AbilityData component = charLink.ActorDataPrefab.GetComponent<AbilityData>();
								for (int num2 = 0; num2 < 14; num2++)
								{
									List<AbilityMod> availableModsForAbility;
									switch (num2)
									{
									case 0:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability0);
										break;
									case 1:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability1);
										break;
									case 2:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability2);
										break;
									case 3:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability3);
										break;
									case 4:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability4);
										break;
									case 5:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability5);
										break;
									case 6:
										availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(component.m_ability6);
										break;
									default:
										continue;
									}
									for (int num3 = 0; num3 < availableModsForAbility.Count; num3++)
									{
										m_collectionItems.Add(availableModsForAbility[num3].GetAbilityModUnlockData(charLink.m_characterType, num2));
									}
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								return;
							}
						}
					}
				}
			}
		}

		public void RefreshProgress(bool force = false)
		{
			if (!force)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!m_pendingUpdate)
				{
					return;
				}
			}
			LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
			bool enableHiddenCharacters = gameplayOverrides.EnableHiddenCharacters;
			int num3 = CurrentProgress = (TotalProgress = 0);
			for (int i = 0; i < m_collectionItems.Count; i++)
			{
				if (m_collectionItems[i] is GameBalanceVars.ColorUnlockData)
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
					if (!gameplayOverrides.IsColorAllowed((CharacterType)m_collectionItems[i].Index1, m_collectionItems[i].Index2, m_collectionItems[i].Index3, m_collectionItems[i].ID))
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
						continue;
					}
				}
				if (m_collectionItems[i].IsOwned())
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
					CurrentProgress++;
					TotalProgress++;
				}
				else if (enableHiddenCharacters)
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
					TotalProgress++;
				}
				else
				{
					if (m_collectionItems[i].m_isHidden)
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
					if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(m_collectionItems[i]))
					{
						continue;
					}
					if (m_collectionItems[i] is GameBalanceVars.ColorUnlockData)
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
						if (GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)m_collectionItems[i].Index1).skinUnlockData[m_collectionItems[i].Index2].m_isHidden)
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
						}
						else if (GameBalanceVars.Get().GetCharacterUnlockData((CharacterType)m_collectionItems[i].Index1).skinUnlockData[m_collectionItems[i].Index2].patternUnlockData[m_collectionItems[i].Index3].m_isHidden)
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
						}
						else
						{
							TotalProgress++;
						}
					}
					else
					{
						TotalProgress++;
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				m_pendingUpdate = false;
				return;
			}
		}

		public void QueueProgressRefresh()
		{
			m_pendingUpdate = true;
		}
	}

	public GridLayoutGroup m_heroListContainer;

	public UIStorePageIndicator m_pageItemPrefab;

	public GridLayoutGroup m_pageListContainer;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	private int m_numPages;

	private int m_currentPage;

	private List<UIStorePageIndicator> m_pageMarkers;

	private List<UIStorePurchaseFreelancerItem> m_freeLancerSlots;

	private List<CharacterData> m_visibleFreelancers;

	private void Awake()
	{
		if (HitchDetector.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HitchDetector.Get().AddNewLayoutGroup(m_heroListContainer);
		}
		m_freeLancerSlots = new List<UIStorePurchaseFreelancerItem>(m_heroListContainer.GetComponentsInChildren<UIStorePurchaseFreelancerItem>(true));
		using (List<UIStorePurchaseFreelancerItem>.Enumerator enumerator = m_freeLancerSlots.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIStorePurchaseFreelancerItem current = enumerator.Current;
				current.m_hitBox.RegisterScrollListener(OnScroll);
				UIManager.SetGameObjectActive(current, false);
				StaggerComponent.SetStaggerComponent(current.gameObject, true);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_visibleFreelancers = new List<CharacterData>();
		m_pageMarkers = new List<UIStorePageIndicator>();
		m_prevPage.callback = ClickedOnPrevPage;
		m_nextPage.callback = ClickedOnNextPage;
	}

	private void Start()
	{
		bool enableHiddenCharacters = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		for (int i = 0; i < GameWideData.Get().m_characterResourceLinks.Length; i++)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().m_characterResourceLinks[i];
			if (!(characterResourceLink != null))
			{
				continue;
			}
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
			if (characterResourceLink.m_characterType == CharacterType.None)
			{
				continue;
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
			if (characterResourceLink.m_characterType == CharacterType.PunchingDummy)
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
			if (characterResourceLink.m_characterType.IsWillFill() || characterResourceLink.m_characterType == CharacterType.TestFreelancer1)
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (characterResourceLink.m_characterType == CharacterType.TestFreelancer2)
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
			if (!enableHiddenCharacters)
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
				if (characterResourceLink.m_isHidden)
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			CharacterData item = new CharacterData(characterResourceLink);
			m_visibleFreelancers.Add(item);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			List<CharacterData> visibleFreelancers = m_visibleFreelancers;
			if (_003C_003Ef__am_0024cache0 == null)
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
				_003C_003Ef__am_0024cache0 = ((CharacterData a, CharacterData b) => a.ResourceLink.GetDisplayName().CompareTo(b.ResourceLink.GetDisplayName()));
			}
			visibleFreelancers.Sort(_003C_003Ef__am_0024cache0);
			m_numPages = Mathf.CeilToInt(m_visibleFreelancers.Count / m_freeLancerSlots.Count) + 1;
			for (int j = 0; j < m_numPages; j++)
			{
				UIStorePageIndicator uIStorePageIndicator = Object.Instantiate(m_pageItemPrefab);
				uIStorePageIndicator.transform.SetParent(m_pageListContainer.transform);
				uIStorePageIndicator.transform.localScale = Vector3.one;
				uIStorePageIndicator.transform.localPosition = Vector3.zero;
				uIStorePageIndicator.SetSelected(j == 0);
				uIStorePageIndicator.m_hitbox.callback = PageClicked;
				uIStorePageIndicator.m_hitbox.RegisterScrollListener(OnScroll);
				uIStorePageIndicator.SetPageNumber(j + 1);
				m_pageMarkers.Add(uIStorePageIndicator);
			}
			m_nextPage.transform.parent.SetAsLastSibling();
			SetCharacterPageList(m_currentPage);
			ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
			ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesChange;
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
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
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesChange;
			return;
		}
	}

	private void OnEnable()
	{
		SetCharacterPageList(m_currentPage);
	}

	private void SetCharacterPageList(int pageNumber)
	{
		int num = pageNumber * m_freeLancerSlots.Count;
		for (int i = 0; i < m_freeLancerSlots.Count; i++)
		{
			if (num + i < m_visibleFreelancers.Count)
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
				CharacterData characterData = m_visibleFreelancers[num + i];
				characterData.RefreshProgress();
				m_freeLancerSlots[i].Setup(characterData.ResourceLink, characterData.CurrentProgress, characterData.TotalProgress);
			}
			else
			{
				m_freeLancerSlots[i].Setup(null, 0, 0);
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_currentPage = pageNumber;
			for (int j = 0; j < m_pageMarkers.Count; j++)
			{
				m_pageMarkers[j].SetSelected(j == m_currentPage);
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

	public void FreeLancerClicked(UIStorePurchaseFreelancerItem clickedHero)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectSkinChoice);
		int num = 0;
		while (true)
		{
			if (num < m_freeLancerSlots.Count)
			{
				if (clickedHero == m_freeLancerSlots[num])
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIStorePanel.Get().OpenFreelancerPage(m_freeLancerSlots[num].GetCharLink());
			return;
		}
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		int num = 0;
		while (true)
		{
			if (num < m_pageMarkers.Count)
			{
				if (m_pageMarkers[num].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
				{
					SetCharacterPageList(num);
					break;
				}
				num++;
				continue;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			break;
		}
		FreeLancerClicked(null);
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector2 scrollDelta = pointerEventData.scrollDelta;
		if (scrollDelta.y > 0f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ClickedOnPrevPage(null);
					return;
				}
			}
		}
		Vector2 scrollDelta2 = pointerEventData.scrollDelta;
		if (scrollDelta2.y < 0f)
		{
			ClickedOnNextPage(null);
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (m_currentPage - 1 >= 0)
		{
			m_currentPage--;
			UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
			SetCharacterPageList(m_currentPage);
			FreeLancerClicked(null);
		}
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (m_currentPage + 1 >= m_numPages)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		m_currentPage++;
		SetCharacterPageList(m_currentPage);
		FreeLancerClicked(null);
	}

	protected void UpdatePage(CharacterType characterType = CharacterType.None)
	{
		if (characterType == CharacterType.None)
		{
			List<CharacterData> visibleFreelancers = m_visibleFreelancers;
			if (_003C_003Ef__am_0024cache1 == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_003C_003Ef__am_0024cache1 = delegate(CharacterData v)
				{
					v.QueueProgressRefresh();
				};
			}
			visibleFreelancers.ForEach(_003C_003Ef__am_0024cache1);
		}
		else
		{
			CharacterData characterData = m_visibleFreelancers.SingleOrDefault((CharacterData v) => v.ResourceLink.m_characterType == characterType);
			if (characterData == null)
			{
				return;
			}
			characterData.QueueProgressRefresh();
		}
		if (!base.gameObject.activeInHierarchy)
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
			SetCharacterPageList(m_currentPage);
			return;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		UpdatePage(newData.CharacterType);
	}

	private void OnAccountDataUpdated(PersistedAccountData data)
	{
		UpdatePage();
	}

	private void OnLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		UpdatePage();
	}
}
