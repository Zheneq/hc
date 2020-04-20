using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressStats : UIPlayerProgressSubPanel
{
	public GridLayoutGroup m_freelancerLayout;

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

	public TextMeshProUGUI m_numWins;

	private bool m_initialized;

	private CharacterType m_characterType;

	private CharacterRole m_characterRole;

	private PersistedStatBucket m_gameType = PersistedStatBucket.Deathmatch_Unranked;

	private int m_season = -1;

	private int m_matchesPlayed;

	private int m_matchesWon;

	private UIGameOverStatWidget[] m_freelancerItems;

	private UIGameOverStatWidget[] m_generalItems;

	private UIGameOverStatWidget[] m_firepowerItems;

	private UIGameOverStatWidget[] m_supportItems;

	private UIGameOverStatWidget[] m_frontlineItems;

	private Dictionary<string, CalculateFreelancerStatsResponse> m_percentileInfos = new Dictionary<string, CalculateFreelancerStatsResponse>();

	public bool HasGlobalStatsToCompareTo { get; private set; }

	public bool HasFreelancerStatsToCompareTo { get; private set; }

	public string StatCompareFailure { get; private set; }

	private void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		this.m_freelancerItems = this.m_freelancerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		this.m_generalItems = this.m_generalLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		this.m_firepowerItems = this.m_firepowerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		this.m_supportItems = this.m_supportLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		this.m_frontlineItems = this.m_frontlineLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		this.m_season = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
	}

	private void Awake()
	{
		this.m_freelancerDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(this.m_characterType, delegate(int charTypeInt)
			{
				if (charTypeInt >= 0)
				{
					this.m_characterType = (CharacterType)charTypeInt;
					this.m_characterRole = CharacterRole.None;
				}
				else
				{
					this.m_characterType = CharacterType.None;
					this.m_characterRole = (CharacterRole)(-charTypeInt);
				}
				this.Setup();
			}, this.m_freelancerDropdownSlot, true, this.m_characterRole);
		};
		this.m_gameModeDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenGameModeDropdown(this.m_gameType, delegate(int gameModeInt)
			{
				this.m_gameType = (PersistedStatBucket)gameModeInt;
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
					if (list[i].ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(season))
					{
						return true;
					}
				}
				return false;
			}, this.m_seasonsDropdownSlot);
		};
		this.m_numWins.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uisimpleTooltip = tooltip as UISimpleTooltip;
			if (uisimpleTooltip != null)
			{
				string text = string.Format(StringUtil.TR("MatchesPlayed", "Global"), this.m_matchesPlayed);
				if (this.m_matchesPlayed > 0)
				{
					text = text + Environment.NewLine + string.Format(StringUtil.TR("WinPercentage", "Global"), this.m_matchesWon * 0x64 / this.m_matchesPlayed);
				}
				uisimpleTooltip.Setup(text);
				return true;
			}
			return false;
		}, null);
	}

	private void Start()
	{
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
	}

	private void OnEnable()
	{
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
		
		using (IEnumerator<int> enumerator = source.SelectMany(((PersistedCharacterData x) => x.ExperienceComponent.PersistedStatsDictionaryBySeason.Keys)).Distinct<int>().GetEnumerator())
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
				goto IL_6D;
			}
		}
		this.m_season = -1;
		flag = false;
		IL_6D:
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
		PersistedStats persistedStats;
		if (this.m_characterType != CharacterType.None)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(this.m_characterType);
			this.GetStats(playerCharacterData, flag, out persistedStats);
			this.m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(this.m_characterType), this.m_characterType);
			UIManager.SetGameObjectActive(this.m_freelancerLayout, true, null);
			this.HandleFreelancerRow(persistedStats);
		}
		else if (this.m_characterRole != CharacterRole.None)
		{
			UIManager.SetGameObjectActive(this.m_freelancerLayout, false, null);
			persistedStats = new PersistedStats();
			using (Dictionary<CharacterType, PersistedCharacterData>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().GetAllPlayerCharacterData().Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PersistedCharacterData persistedCharacterData = enumerator.Current;
					if (persistedCharacterData.CharacterType.IsValidForHumanGameplay())
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(persistedCharacterData.CharacterType);
						if (characterResourceLink.m_characterRole == this.m_characterRole)
						{
							PersistedStats persistedStats2;
							this.GetStats(persistedCharacterData, flag, out persistedStats2);
							if (persistedStats2 != null)
							{
								persistedStats.CombineStats(persistedStats2);
							}
						}
					}
				}
			}
			this.m_freelancerDropdownBtn.Setup(StringUtil.TR("CharacterRole_" + this.m_characterRole, "Global"), this.m_characterRole);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_freelancerLayout, false, null);
			persistedStats = new PersistedStats();
			foreach (PersistedCharacterData persistedCharacterData2 in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (persistedCharacterData2.CharacterType.IsValidForHumanGameplay())
				{
					PersistedStats persistedStats3;
					this.GetStats(persistedCharacterData2, flag, out persistedStats3);
					if (persistedStats3 != null)
					{
						persistedStats.CombineStats(persistedStats3);
					}
				}
			}
			this.m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), this.m_characterType);
		}
		if (persistedStats != null)
		{
			this.m_matchesWon = (int)persistedStats.MatchesWon.GetSum();
			this.m_matchesPlayed = persistedStats.MatchesWon.GetNumGames();
		}
		else
		{
			this.m_matchesWon = (this.m_matchesPlayed = 0);
		}
		this.m_numWins.text = string.Format(StringUtil.TR("MatchesWon", "Global"), this.m_matchesWon);
		this.HandleStatRow(this.m_generalItems, StatDisplaySettings.GeneralStats, persistedStats);
		this.HandleStatRow(this.m_firepowerItems, StatDisplaySettings.FirepowerStats, persistedStats);
		this.HandleStatRow(this.m_supportItems, StatDisplaySettings.SupportStats, persistedStats);
		this.HandleStatRow(this.m_frontlineItems, StatDisplaySettings.FrontlinerStats, persistedStats);
		if (this.m_characterType.IsValidForHumanGameplay())
		{
			string percentilesResponseKey = this.GetPercentilesResponseKey();
			if (this.m_percentileInfos.ContainsKey(percentilesResponseKey))
			{
				this.UpdateAllPercentiles(this.m_percentileInfos[percentilesResponseKey]);
			}
			else
			{
				ClientGameManager.Get().CalculateFreelancerStats(this.m_gameType, this.m_characterType, persistedStats, new Action<CalculateFreelancerStatsResponse>(this.UpdateAllPercentiles));
			}
		}
		else
		{
			this.UpdateAllPercentiles(null);
		}
	}

	private string GetPercentilesResponseKey()
	{
		return string.Format("{0}|{1}|{2}", this.m_gameType, this.m_characterType, this.m_season);
	}

	private void UpdateAllPercentiles(CalculateFreelancerStatsResponse response)
	{
		if (response != null)
		{
			if (response.Success)
			{
				bool flag = !response.FreelancerSpecificPercentiles.IsNullOrEmpty<KeyValuePair<int, PercentileInfo>>();
				int i = 0;
				while (i < this.m_freelancerItems.Length)
				{
					if (!flag)
					{
						goto IL_80;
					}
					if (!response.FreelancerSpecificPercentiles.ContainsKey(i))
					{
						goto IL_80;
					}
					this.HasFreelancerStatsToCompareTo = true;
					this.m_freelancerItems[i].UpdatePercentiles(response.FreelancerSpecificPercentiles[i]);
					IL_95:
					i++;
					continue;
					IL_80:
					this.HasFreelancerStatsToCompareTo = false;
					this.m_freelancerItems[i].UpdatePercentiles(null);
					goto IL_95;
				}
				if (!response.GlobalPercentiles.IsNullOrEmpty<KeyValuePair<StatDisplaySettings.StatType, PercentileInfo>>())
				{
					this.HasGlobalStatsToCompareTo = true;
					this.UpdatePercentiles(this.m_generalItems, response.GlobalPercentiles);
					this.UpdatePercentiles(this.m_firepowerItems, response.GlobalPercentiles);
					this.UpdatePercentiles(this.m_supportItems, response.GlobalPercentiles);
					this.UpdatePercentiles(this.m_frontlineItems, response.GlobalPercentiles);
				}
				else
				{
					this.HasGlobalStatsToCompareTo = false;
					this.UpdatePercentiles(this.m_generalItems, null);
					this.UpdatePercentiles(this.m_firepowerItems, null);
					this.UpdatePercentiles(this.m_supportItems, null);
					this.UpdatePercentiles(this.m_frontlineItems, null);
				}
				goto IL_1D9;
			}
		}
		for (int j = 0; j < this.m_freelancerItems.Length; j++)
		{
			this.m_freelancerItems[j].UpdatePercentiles(null);
		}
		this.UpdatePercentiles(this.m_generalItems, null);
		this.UpdatePercentiles(this.m_firepowerItems, null);
		this.UpdatePercentiles(this.m_supportItems, null);
		this.UpdatePercentiles(this.m_frontlineItems, null);
		this.HasGlobalStatsToCompareTo = false;
		this.HasFreelancerStatsToCompareTo = false;
		if (response != null && response.LocalizedFailure != null)
		{
			this.StatCompareFailure = response.LocalizedFailure.ToString();
		}
		IL_1D9:
		if (response != null)
		{
			this.m_percentileInfos[this.GetPercentilesResponseKey()] = response;
		}
	}

	private unsafe void GetStats(PersistedCharacterData charData, bool isCurrentSeason, out PersistedStats stats)
	{
		if (isCurrentSeason)
		{
			charData.ExperienceComponent.PersistedStatsDictionary.TryGetValue(this.m_gameType, out stats);
		}
		else
		{
			if (!charData.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(this.m_season))
			{
				stats = null;
				return;
			}
			charData.ExperienceComponent.PersistedStatsDictionaryBySeason[this.m_season].TryGetValue(this.m_gameType, out stats);
		}
	}

	private void HandleStatRow(UIGameOverStatWidget[] widgets, StatDisplaySettings.StatType[] statTypes, PersistedStats stats)
	{
		int i = 0;
		while (i < statTypes.Length)
		{
			if (i >= widgets.Length)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					goto IL_4D;
				}
			}
			else
			{
				UIManager.SetGameObjectActive(widgets[i], true, null);
				widgets[i].SetupTotalledStat(stats, statTypes[i], this.m_characterType);
				i++;
			}
		}
		IL_4D:
		while (i < widgets.Length)
		{
			UIManager.SetGameObjectActive(widgets[i], false, null);
			i++;
		}
	}

	private void HandleFreelancerRow(PersistedStats stats)
	{
		for (int i = 0; i < this.m_freelancerItems.Length; i++)
		{
			string text = StringUtil.TR_FreelancerStatName(this.m_characterType.ToString(), i);
			string text2 = StringUtil.TR_FreelancerStatDescription(this.m_characterType.ToString(), i);
			if (text.IsNullOrEmpty() && text2.IsNullOrEmpty())
			{
				while (i < this.m_freelancerItems.Length)
				{
					UIManager.SetGameObjectActive(this.m_freelancerItems[i], false, null);
					i++;
				}
				return;
			}
			UIManager.SetGameObjectActive(this.m_freelancerItems[i], true, null);
			AbilityData component = GameWideData.Get().GetCharacterResourceLink(this.m_characterType).ActorDataPrefab.GetComponent<AbilityData>();
			this.m_freelancerItems[i].SetupFreelancerTotalledStats(stats, text, text2, i, component, this.m_characterType);
		}
	}

	private void UpdatePercentiles(UIGameOverStatWidget[] widgets, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		for (int i = 0; i < widgets.Length; i++)
		{
			if (!widgets[i].gameObject.activeSelf)
			{
				return;
			}
			PercentileInfo info = null;
			if (percentiles != null)
			{
				percentiles.TryGetValue(widgets[i].GeneralStatType, out info);
			}
			widgets[i].UpdatePercentiles(info);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return;
		}
	}
}
