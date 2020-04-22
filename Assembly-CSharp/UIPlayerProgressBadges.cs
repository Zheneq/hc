using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressBadges : UIPlayerProgressSubPanel
{
	private class BadgeSlots
	{
		public GridLayoutGroup Layout;

		public List<UIPlayerProgressBadgeEntry> Slots;

		private int m_currentSlot;

		public static UIPlayerProgressBadgeEntry BadgeEntryPrefab;

		public BadgeSlots(GridLayoutGroup layout)
		{
			Layout = layout;
			Slots = new List<UIPlayerProgressBadgeEntry>();
			Slots.AddRange(layout.GetComponentsInChildren<UIPlayerProgressBadgeEntry>(true));
			m_currentSlot = 0;
		}

		public void Reset()
		{
			m_currentSlot = 0;
			UIManager.SetGameObjectActive(Layout, false);
		}

		public bool IsVisible()
		{
			return Layout.gameObject.activeSelf;
		}

		public UIPlayerProgressBadgeEntry GetNextSlot(UIEventTriggerUtils.EventDelegate scrollDelegate)
		{
			UIManager.SetGameObjectActive(Layout, true);
			m_currentSlot++;
			if (m_currentSlot <= Slots.Count)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return Slots[m_currentSlot - 1];
					}
				}
			}
			UIPlayerProgressBadgeEntry uIPlayerProgressBadgeEntry = Object.Instantiate(BadgeEntryPrefab);
			uIPlayerProgressBadgeEntry.transform.SetParent(Layout.transform);
			uIPlayerProgressBadgeEntry.transform.localPosition = Vector3.zero;
			uIPlayerProgressBadgeEntry.transform.localScale = Vector3.one;
			uIPlayerProgressBadgeEntry.m_hitbox.RegisterScrollListener(scrollDelegate);
			Slots.Add(uIPlayerProgressBadgeEntry);
			return uIPlayerProgressBadgeEntry;
		}

		public bool HasSlotsLeft()
		{
			return m_currentSlot < Slots.Count;
		}
	}

	public GridLayoutGroup m_generalLayout;

	public GridLayoutGroup m_firepowerLayout;

	public GridLayoutGroup m_supportLayout;

	public GridLayoutGroup m_frontlineLayout;

	public UIPlayerProgressDropdownBtn m_freelancerDropdownBtn;

	public UIPlayerProgressDropdownBtn m_gameModeDropdownBtn;

	public UIPlayerProgressDropdownBtn m_seasonsDropdownBtn;

	public RectTransform m_freelancerDropdownSlot;

	public RectTransform m_gameModeDropdownSlot;

	public RectTransform m_seasonsDropdownSlot;

	public ScrollRect m_scrollRect;

	public UIPlayerProgressBadgeEntry m_badgeEntryPrefab;

	private bool m_initialized;

	private CharacterType m_characterType;

	private PersistedStatBucket m_gameType = PersistedStatBucket.Deathmatch_Unranked;

	private int m_season = -1;

	private Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, BadgeSlots> m_badgeRoleSlots;

	public CharacterType CurrentCharacterTypeFilter => m_characterType;

	private void Init()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_initialized = true;
		BadgeSlots.BadgeEntryPrefab = m_badgeEntryPrefab;
		m_badgeRoleSlots = new Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, BadgeSlots>();
		m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.General] = new BadgeSlots(m_generalLayout);
		m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Firepower] = new BadgeSlots(m_firepowerLayout);
		m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Frontliner] = new BadgeSlots(m_frontlineLayout);
		m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Support] = new BadgeSlots(m_supportLayout);
		m_season = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
	}

	private void Start()
	{
		m_freelancerDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(m_characterType, delegate(int charTypeInt)
			{
				m_characterType = (CharacterType)charTypeInt;
				HideOrShowSeasonDropdown();
				Setup();
			}, m_freelancerDropdownSlot, false);
		};
		m_gameModeDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenGameModeDropdown(m_gameType, delegate(int gameModeInt)
			{
				m_gameType = (PersistedStatBucket)gameModeInt;
				HideOrShowSeasonDropdown();
				Setup();
			}, m_gameModeDropdownSlot);
		};
		m_seasonsDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenSeasonsDropdown(m_season, delegate(int season)
			{
				m_season = season;
				Setup();
			}, delegate(int season)
			{
				if (season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				List<PersistedCharacterData> list = new List<PersistedCharacterData>();
				if (m_characterType != 0)
				{
					list.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
				}
				else
				{
					list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
				}
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].ExperienceComponent.BadgesEarnedBySeason.ContainsKey(season))
					{
						return true;
					}
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}, m_seasonsDropdownSlot);
		};
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
	}

	private void OnDisable()
	{
		UIPlayerProgressPanel.Get().HideDropdowns();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		HideOrShowSeasonDropdown();
		Setup();
	}

	private void HideOrShowSeasonDropdown()
	{
		List<PersistedCharacterData> list = new List<PersistedCharacterData>();
		if (m_characterType != 0)
		{
			list.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
		}
		else
		{
			list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
		}
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = ((PersistedCharacterData x) => x.ExperienceComponent.BadgesEarnedBySeason.Keys);
		}
		using (IEnumerator<int> enumerator = list.SelectMany(_003C_003Ef__am_0024cache0).Distinct().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(current);
				if (!seasonTemplate.IsTutorial)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							UIManager.SetGameObjectActive(m_seasonsDropdownBtn, true);
							return;
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_seasonsDropdownBtn, false);
	}

	private void Setup()
	{
		Init();
		bool flag = m_season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(m_season);
		if (seasonTemplate != null)
		{
			if (!seasonTemplate.IsTutorial)
			{
				goto IL_006f;
			}
		}
		m_season = -1;
		flag = false;
		goto IL_006f;
		IL_006f:
		using (Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, BadgeSlots>.ValueCollection.Enumerator enumerator = m_badgeRoleSlots.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BadgeSlots current = enumerator.Current;
				current.Reset();
			}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		if (m_characterType != 0)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_characterType);
			GetBadges(playerCharacterData, flag, dictionary);
			m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(m_characterType), m_characterType);
		}
		else
		{
			foreach (PersistedCharacterData value2 in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (value2.CharacterType.IsValidForHumanGameplay())
				{
					GetBadges(value2, flag, dictionary);
				}
			}
			m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), m_characterType);
		}
		List<int> list = new List<int>();
		for (int i = 0; i < GameResultBadgeData.Get().BadgeGroups.Length; i++)
		{
			GameResultBadgeData.ConsolidatedBadgeGroup consolidatedBadgeGroup = GameResultBadgeData.Get().BadgeGroups[i];
			BadgeSlots badgeSlots = m_badgeRoleSlots[consolidatedBadgeGroup.DisplayCategory];
			UIPlayerProgressBadgeEntry nextSlot = badgeSlots.GetNextSlot(OnScroll);
			nextSlot.Setup(consolidatedBadgeGroup, i, dictionary);
			UIManager.SetGameObjectActive(nextSlot, true);
			list.AddRange(consolidatedBadgeGroup.BadgeIDs);
		}
		while (true)
		{
			GameBalanceVars.GameResultBadge[] gameResultBadges = GameResultBadgeData.Get().GameResultBadges;
			foreach (GameBalanceVars.GameResultBadge gameResultBadge in gameResultBadges)
			{
				if (!gameResultBadge.DisplayEvenIfConsolidated)
				{
					if (list.Contains(gameResultBadge.UniqueBadgeID))
					{
						continue;
					}
				}
				BadgeSlots badgeSlots2 = m_badgeRoleSlots[gameResultBadge.Role];
				UIPlayerProgressBadgeEntry nextSlot2 = badgeSlots2.GetNextSlot(OnScroll);
				dictionary.TryGetValue(gameResultBadge.UniqueBadgeID, out int value);
				nextSlot2.Setup(gameResultBadge, value);
				UIManager.SetGameObjectActive(nextSlot2, true);
			}
			while (true)
			{
				using (Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, BadgeSlots>.ValueCollection.Enumerator enumerator3 = m_badgeRoleSlots.Values.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						BadgeSlots current3 = enumerator3.Current;
						if (!current3.IsVisible())
						{
						}
						else if (current3.HasSlotsLeft())
						{
							UIManager.SetGameObjectActive(current3.GetNextSlot(OnScroll), false);
						}
					}
				}
				m_gameModeDropdownBtn.Setup(StringUtil.TR_PersistedStatBucketName(m_gameType));
				string text = (m_season < 0) ? string.Empty : ((!flag) ? SeasonWideData.Get().GetSeasonTemplate(m_season).GetDisplayName() : StringUtil.TR("CurrentSeason", "Global"));
				m_seasonsDropdownBtn.Setup(text);
				return;
			}
		}
	}

	private void GetBadges(PersistedCharacterData charData, bool isCurrentSeason, Dictionary<int, int> existingBadges)
	{
		Dictionary<int, int> value = null;
		if (isCurrentSeason)
		{
			charData.ExperienceComponent.BadgesEarned.TryGetValue(m_gameType, out value);
		}
		else if (charData.ExperienceComponent.BadgesEarnedBySeason.ContainsKey(m_season))
		{
			charData.ExperienceComponent.BadgesEarnedBySeason[m_season].TryGetValue(m_gameType, out value);
		}
		if (value == null)
		{
			return;
		}
		while (true)
		{
			using (Dictionary<int, int>.Enumerator enumerator = value.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.Current;
					if (!existingBadges.TryGetValue(current.Key, out int value2))
					{
						value2 = 0;
					}
					existingBadges[current.Key] = value2 + current.Value;
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
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}
}
