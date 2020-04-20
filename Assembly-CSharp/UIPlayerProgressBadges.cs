using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressBadges : UIPlayerProgressSubPanel
{
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

	private Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, UIPlayerProgressBadges.BadgeSlots> m_badgeRoleSlots;

	public CharacterType CurrentCharacterTypeFilter
	{
		get
		{
			return this.m_characterType;
		}
	}

	private void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		UIPlayerProgressBadges.BadgeSlots.BadgeEntryPrefab = this.m_badgeEntryPrefab;
		this.m_badgeRoleSlots = new Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, UIPlayerProgressBadges.BadgeSlots>();
		this.m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.General] = new UIPlayerProgressBadges.BadgeSlots(this.m_generalLayout);
		this.m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Firepower] = new UIPlayerProgressBadges.BadgeSlots(this.m_firepowerLayout);
		this.m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Frontliner] = new UIPlayerProgressBadges.BadgeSlots(this.m_frontlineLayout);
		this.m_badgeRoleSlots[GameBalanceVars.GameResultBadge.BadgeRole.Support] = new UIPlayerProgressBadges.BadgeSlots(this.m_supportLayout);
		this.m_season = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
	}

	private void Start()
	{
		this.m_freelancerDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(this.m_characterType, delegate(int charTypeInt)
			{
				this.m_characterType = (CharacterType)charTypeInt;
				this.HideOrShowSeasonDropdown();
				this.Setup();
			}, this.m_freelancerDropdownSlot, false, CharacterRole.None);
		};
		this.m_gameModeDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenGameModeDropdown(this.m_gameType, delegate(int gameModeInt)
			{
				this.m_gameType = (PersistedStatBucket)gameModeInt;
				this.HideOrShowSeasonDropdown();
				this.Setup();
			}, this.m_gameModeDropdownSlot);
		};
		this.m_seasonsDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenSeasonsDropdown(this.m_season, delegate(int season)
			{
				this.m_season = season;
				this.Setup();
			}, delegate(int season)
			{
				bool flag = season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				if (flag)
				{
					return true;
				}
				List<PersistedCharacterData> list = new List<PersistedCharacterData>();
				if (this.m_characterType != CharacterType.None)
				{
					list.Add(ClientGameManager.Get().GetPlayerCharacterData(this.m_characterType));
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
				return false;
			}, this.m_seasonsDropdownSlot);
		};
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
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
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		this.HideOrShowSeasonDropdown();
		this.Setup();
	}

	private void HideOrShowSeasonDropdown()
	{
		List<PersistedCharacterData> list = new List<PersistedCharacterData>();
		if (this.m_characterType != CharacterType.None)
		{
			list.Add(ClientGameManager.Get().GetPlayerCharacterData(this.m_characterType));
		}
		else
		{
			list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
		}
		IEnumerable<PersistedCharacterData> source = list;
		
		using (IEnumerator<int> enumerator = source.SelectMany(((PersistedCharacterData x) => x.ExperienceComponent.BadgesEarnedBySeason.Keys)).Distinct<int>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int seasonNumber = enumerator.Current;
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(seasonNumber);
				if (!seasonTemplate.IsTutorial)
				{
					UIManager.SetGameObjectActive(this.m_seasonsDropdownBtn, true, null);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_seasonsDropdownBtn, false, null);
	}

	private void Setup()
	{
		this.Init();
		bool flag = this.m_season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(this.m_season);
		if (seasonTemplate != null)
		{
			if (!seasonTemplate.IsTutorial)
			{
				goto IL_6F;
			}
		}
		this.m_season = -1;
		flag = false;
		IL_6F:
		using (Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, UIPlayerProgressBadges.BadgeSlots>.ValueCollection.Enumerator enumerator = this.m_badgeRoleSlots.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIPlayerProgressBadges.BadgeSlots badgeSlots = enumerator.Current;
				badgeSlots.Reset();
			}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		if (this.m_characterType != CharacterType.None)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_characterType);
			this.GetBadges(playerCharacterData, flag, dictionary);
			this.m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(this.m_characterType), this.m_characterType);
		}
		else
		{
			foreach (PersistedCharacterData persistedCharacterData in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (persistedCharacterData.CharacterType.IsValidForHumanGameplay())
				{
					this.GetBadges(persistedCharacterData, flag, dictionary);
				}
			}
			this.m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), this.m_characterType);
		}
		List<int> list = new List<int>();
		for (int i = 0; i < GameResultBadgeData.Get().BadgeGroups.Length; i++)
		{
			GameResultBadgeData.ConsolidatedBadgeGroup consolidatedBadgeGroup = GameResultBadgeData.Get().BadgeGroups[i];
			UIPlayerProgressBadges.BadgeSlots badgeSlots2 = this.m_badgeRoleSlots[consolidatedBadgeGroup.DisplayCategory];
			UIPlayerProgressBadgeEntry nextSlot = badgeSlots2.GetNextSlot(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			nextSlot.Setup(consolidatedBadgeGroup, i, dictionary);
			UIManager.SetGameObjectActive(nextSlot, true, null);
			list.AddRange(consolidatedBadgeGroup.BadgeIDs);
		}
		GameBalanceVars.GameResultBadge[] gameResultBadges = GameResultBadgeData.Get().GameResultBadges;
		int j = 0;
		while (j < gameResultBadges.Length)
		{
			GameBalanceVars.GameResultBadge gameResultBadge = gameResultBadges[j];
			if (gameResultBadge.DisplayEvenIfConsolidated)
			{
				goto IL_26A;
			}
			if (!list.Contains(gameResultBadge.UniqueBadgeID))
			{
				goto IL_26A;
			}
			IL_2BE:
			j++;
			continue;
			IL_26A:
			UIPlayerProgressBadges.BadgeSlots badgeSlots3 = this.m_badgeRoleSlots[gameResultBadge.Role];
			UIPlayerProgressBadgeEntry nextSlot2 = badgeSlots3.GetNextSlot(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			int count;
			dictionary.TryGetValue(gameResultBadge.UniqueBadgeID, out count);
			nextSlot2.Setup(gameResultBadge, count);
			UIManager.SetGameObjectActive(nextSlot2, true, null);
			goto IL_2BE;
		}
		using (Dictionary<GameBalanceVars.GameResultBadge.BadgeRole, UIPlayerProgressBadges.BadgeSlots>.ValueCollection.Enumerator enumerator3 = this.m_badgeRoleSlots.Values.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				UIPlayerProgressBadges.BadgeSlots badgeSlots4 = enumerator3.Current;
				if (!badgeSlots4.IsVisible())
				{
				}
				else if (badgeSlots4.HasSlotsLeft())
				{
					UIManager.SetGameObjectActive(badgeSlots4.GetNextSlot(new UIEventTriggerUtils.EventDelegate(this.OnScroll)), false, null);
				}
			}
		}
		this.m_gameModeDropdownBtn.Setup(StringUtil.TR_PersistedStatBucketName(this.m_gameType), CharacterType.None);
		string text;
		if (this.m_season < 0)
		{
			text = string.Empty;
		}
		else if (flag)
		{
			text = StringUtil.TR("CurrentSeason", "Global");
		}
		else
		{
			text = SeasonWideData.Get().GetSeasonTemplate(this.m_season).GetDisplayName();
		}
		this.m_seasonsDropdownBtn.Setup(text, CharacterType.None);
	}

	private void GetBadges(PersistedCharacterData charData, bool isCurrentSeason, Dictionary<int, int> existingBadges)
	{
		Dictionary<int, int> dictionary = null;
		if (isCurrentSeason)
		{
			charData.ExperienceComponent.BadgesEarned.TryGetValue(this.m_gameType, out dictionary);
		}
		else if (charData.ExperienceComponent.BadgesEarnedBySeason.ContainsKey(this.m_season))
		{
			charData.ExperienceComponent.BadgesEarnedBySeason[this.m_season].TryGetValue(this.m_gameType, out dictionary);
		}
		if (dictionary != null)
		{
			using (Dictionary<int, int>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> keyValuePair = enumerator.Current;
					int num;
					if (!existingBadges.TryGetValue(keyValuePair.Key, out num))
					{
						num = 0;
					}
					existingBadges[keyValuePair.Key] = num + keyValuePair.Value;
				}
			}
		}
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private class BadgeSlots
	{
		public GridLayoutGroup Layout;

		public List<UIPlayerProgressBadgeEntry> Slots;

		private int m_currentSlot;

		public static UIPlayerProgressBadgeEntry BadgeEntryPrefab;

		public BadgeSlots(GridLayoutGroup layout)
		{
			this.Layout = layout;
			this.Slots = new List<UIPlayerProgressBadgeEntry>();
			this.Slots.AddRange(layout.GetComponentsInChildren<UIPlayerProgressBadgeEntry>(true));
			this.m_currentSlot = 0;
		}

		public void Reset()
		{
			this.m_currentSlot = 0;
			UIManager.SetGameObjectActive(this.Layout, false, null);
		}

		public bool IsVisible()
		{
			return this.Layout.gameObject.activeSelf;
		}

		public UIPlayerProgressBadgeEntry GetNextSlot(UIEventTriggerUtils.EventDelegate scrollDelegate)
		{
			UIManager.SetGameObjectActive(this.Layout, true, null);
			this.m_currentSlot++;
			if (this.m_currentSlot <= this.Slots.Count)
			{
				return this.Slots[this.m_currentSlot - 1];
			}
			UIPlayerProgressBadgeEntry uiplayerProgressBadgeEntry = UnityEngine.Object.Instantiate<UIPlayerProgressBadgeEntry>(UIPlayerProgressBadges.BadgeSlots.BadgeEntryPrefab);
			uiplayerProgressBadgeEntry.transform.SetParent(this.Layout.transform);
			uiplayerProgressBadgeEntry.transform.localPosition = Vector3.zero;
			uiplayerProgressBadgeEntry.transform.localScale = Vector3.one;
			uiplayerProgressBadgeEntry.m_hitbox.RegisterScrollListener(scrollDelegate);
			this.Slots.Add(uiplayerProgressBadgeEntry);
			return uiplayerProgressBadgeEntry;
		}

		public bool HasSlotsLeft()
		{
			return this.m_currentSlot < this.Slots.Count;
		}
	}
}
