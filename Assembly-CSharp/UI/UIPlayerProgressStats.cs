using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
		if (m_initialized)
		{
			return;
		}
		m_initialized = true;
		m_freelancerItems = m_freelancerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		m_generalItems = m_generalLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		m_firepowerItems = m_firepowerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		m_supportItems = m_supportLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		m_frontlineItems = m_frontlineLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
		m_season = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
	}

	private void Awake()
	{
		m_freelancerDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(
				m_characterType, 
				delegate(int charTypeInt) 
				{
					if (charTypeInt >= 0)
					{
						m_characterType = (CharacterType)charTypeInt;
						m_characterRole = CharacterRole.None;
					}
					else
					{
						m_characterType = CharacterType.None;
						m_characterRole = (CharacterRole)(-charTypeInt);
					}
					Setup();
				},
				m_freelancerDropdownSlot,
				true,
				m_characterRole);
		};
		m_gameModeDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenGameModeDropdown(
				m_gameType, 
				delegate(int gameModeInt)
				{
					m_gameType = (PersistedStatBucket)gameModeInt;
					Setup();
				},
				m_gameModeDropdownSlot);
		};
		m_seasonsDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenSeasonsDropdown(
				m_season, 
				delegate(int season)
				{
					m_season = season;
					Setup();
				}, 
				delegate(int season)
				{
					if (season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason)
					{
						return true;
					}
					List<PersistedCharacterData> characterDatas = new List<PersistedCharacterData>();
					if (m_characterType != CharacterType.None)
					{
						characterDatas.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
					}
					else
					{
						characterDatas.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
					}
					foreach (PersistedCharacterData characterData in characterDatas)
					{
						if (characterData.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(season))
						{
							return true;
						}
					}
					return false;
				},
				m_seasonsDropdownSlot);
		};
		m_numWins.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = tooltip as UISimpleTooltip;
			if (uISimpleTooltip == null)
			{
				return false;
			}
			string text = string.Format(StringUtil.TR("MatchesPlayed", "Global"), m_matchesPlayed);
			if (m_matchesPlayed > 0)
			{
				text += Environment.NewLine + string.Format(StringUtil.TR("WinPercentage", "Global"), m_matchesWon * 100 / m_matchesPlayed);
			}
			uISimpleTooltip.Setup(text);
			return true;
		});
	}

	private void Start()
	{
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
	}

	private void OnEnable()
	{
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
		List<PersistedCharacterData> characterDatas = new List<PersistedCharacterData>();
		if (m_characterType != 0)
		{
			characterDatas.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
		}
		else
		{
			characterDatas.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
		}
		
		foreach (int season in characterDatas.SelectMany(x => x.ExperienceComponent.PersistedStatsDictionaryBySeason.Keys).Distinct())
		{
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(season);
			if (!seasonTemplate.IsTutorial)
			{
				UIManager.SetGameObjectActive(m_seasonsDropdownBtn, true);
				return;
			}
		}
		UIManager.SetGameObjectActive(m_seasonsDropdownBtn, false);
	}

	private void Setup()
	{
		Init();
		bool isCurrentSeason = m_season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(m_season);
		if (seasonTemplate == null || seasonTemplate.IsTutorial)
		{
			m_season = -1;
			isCurrentSeason = false;
		}
		m_gameModeDropdownBtn.Setup(StringUtil.TR_PersistedStatBucketName(m_gameType));
		string seasonName = m_season < 0
			? string.Empty
			: isCurrentSeason
				? StringUtil.TR("CurrentSeason", "Global")
				: SeasonWideData.Get().GetSeasonTemplate(m_season).GetDisplayName();

		m_seasonsDropdownBtn.Setup(seasonName);
		PersistedStats stats;
		if (m_characterType != CharacterType.None)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_characterType);
			GetStats(playerCharacterData, isCurrentSeason, out stats);
			m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(m_characterType), m_characterType);
			UIManager.SetGameObjectActive(m_freelancerLayout, true);
			HandleFreelancerRow(stats);
		}
		else if (m_characterRole != CharacterRole.None)
		{
			UIManager.SetGameObjectActive(m_freelancerLayout, false);
			stats = new PersistedStats();
			foreach (PersistedCharacterData characterData in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (!characterData.CharacterType.IsValidForHumanGameplay())
				{
					continue;
				}
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterData.CharacterType);
				if (characterResourceLink.m_characterRole == m_characterRole)
				{
					GetStats(characterData, isCurrentSeason, out PersistedStats characterStats);
					if (characterStats != null)
					{
						stats.CombineStats(characterStats);
					}
				}
			}
			m_freelancerDropdownBtn.Setup(StringUtil.TR("CharacterRole_" + m_characterRole, "Global"), m_characterRole);
		}
		else
		{
			UIManager.SetGameObjectActive(m_freelancerLayout, false);
			stats = new PersistedStats();
			foreach (PersistedCharacterData characterData in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (!characterData.CharacterType.IsValidForHumanGameplay())
				{
					continue;
				}
				GetStats(characterData, isCurrentSeason, out PersistedStats characterStats);
				if (characterStats != null)
				{
					stats.CombineStats(characterStats);
				}
			}
			m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), m_characterType);
		}
		if (stats != null)
		{
			m_matchesWon = (int)stats.MatchesWon.GetSum();
			m_matchesPlayed = stats.MatchesWon.GetNumGames();
		}
		else
		{
			m_matchesWon = m_matchesPlayed = 0;
		}
		m_numWins.text = string.Format(StringUtil.TR("MatchesWon", "Global"), m_matchesWon);
		HandleStatRow(m_generalItems, StatDisplaySettings.GeneralStats, stats);
		HandleStatRow(m_firepowerItems, StatDisplaySettings.FirepowerStats, stats);
		HandleStatRow(m_supportItems, StatDisplaySettings.SupportStats, stats);
		HandleStatRow(m_frontlineItems, StatDisplaySettings.FrontlinerStats, stats);
		
		if (m_characterType.IsValidForHumanGameplay())
		{
			string percentilesResponseKey = GetPercentilesResponseKey();
			if (m_percentileInfos.ContainsKey(percentilesResponseKey))
			{
				UpdateAllPercentiles(m_percentileInfos[percentilesResponseKey]);
			}
			else
			{
				ClientGameManager.Get().CalculateFreelancerStats(m_gameType, m_characterType, stats, UpdateAllPercentiles);
			}
		}
		else
		{
			UpdateAllPercentiles(null);
		}
	}

	private string GetPercentilesResponseKey()
	{
		return $"{m_gameType}|{m_characterType}|{m_season}";
	}

	private void UpdateAllPercentiles(CalculateFreelancerStatsResponse response)
	{
		if (response != null && response.Success)
		{
			bool hasPercentiles = !response.FreelancerSpecificPercentiles.IsNullOrEmpty();
			for (int i = 0; i < m_freelancerItems.Length; i++)
			{
				if (hasPercentiles && response.FreelancerSpecificPercentiles.ContainsKey(i))
				{
					HasFreelancerStatsToCompareTo = true;
					m_freelancerItems[i].UpdatePercentiles(response.FreelancerSpecificPercentiles[i]);
				}
				else
				{
					HasFreelancerStatsToCompareTo = false;
					m_freelancerItems[i].UpdatePercentiles(null);
				}
			}
			if (!response.GlobalPercentiles.IsNullOrEmpty())
			{
				HasGlobalStatsToCompareTo = true;
				UpdatePercentiles(m_generalItems, response.GlobalPercentiles);
				UpdatePercentiles(m_firepowerItems, response.GlobalPercentiles);
				UpdatePercentiles(m_supportItems, response.GlobalPercentiles);
				UpdatePercentiles(m_frontlineItems, response.GlobalPercentiles);
			}
			else
			{
				HasGlobalStatsToCompareTo = false;
				UpdatePercentiles(m_generalItems, null);
				UpdatePercentiles(m_firepowerItems, null);
				UpdatePercentiles(m_supportItems, null);
				UpdatePercentiles(m_frontlineItems, null);
			}
		}
		else
		{
			foreach (UIGameOverStatWidget statWidget in m_freelancerItems)
			{
				statWidget.UpdatePercentiles(null);
			}
			UpdatePercentiles(m_generalItems, null);
			UpdatePercentiles(m_firepowerItems, null);
			UpdatePercentiles(m_supportItems, null);
			UpdatePercentiles(m_frontlineItems, null);
			HasGlobalStatsToCompareTo = false;
			HasFreelancerStatsToCompareTo = false;
			if (response != null && response.LocalizedFailure != null)
			{
				StatCompareFailure = response.LocalizedFailure.ToString();
			}
		}
		
		if (response != null)
		{
			m_percentileInfos[GetPercentilesResponseKey()] = response;
		}
	}

	private void GetStats(PersistedCharacterData charData, bool isCurrentSeason, out PersistedStats stats)
	{
		if (isCurrentSeason)
		{
			charData.ExperienceComponent.PersistedStatsDictionary.TryGetValue(m_gameType, out stats);
		}
		else if (charData.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(m_season))
		{
			charData.ExperienceComponent.PersistedStatsDictionaryBySeason[m_season].TryGetValue(m_gameType, out stats);
		}
		else
		{
			stats = null;
		}
	}

	private void HandleStatRow(UIGameOverStatWidget[] widgets, StatDisplaySettings.StatType[] statTypes, PersistedStats stats)
	{
		int i;
		for (i = 0; i < statTypes.Length; i++)
		{
			if (i >= widgets.Length)
			{
				break;
			}
			UIManager.SetGameObjectActive(widgets[i], true);
			widgets[i].SetupTotalledStat(stats, statTypes[i], m_characterType);
		}
		for (; i < widgets.Length; i++)
		{
			UIManager.SetGameObjectActive(widgets[i], false);
		}
	}

	private void HandleFreelancerRow(PersistedStats stats)
	{
		int i = 0;
		for (; i < m_freelancerItems.Length; i++)
		{
			string statName = StringUtil.TR_FreelancerStatName(m_characterType.ToString(), i);
			string statDesc = StringUtil.TR_FreelancerStatDescription(m_characterType.ToString(), i);
			if (statName.IsNullOrEmpty() && statDesc.IsNullOrEmpty())
			{
				break;
			}
			UIManager.SetGameObjectActive(m_freelancerItems[i], true);
			AbilityData component = GameWideData.Get().GetCharacterResourceLink(m_characterType).ActorDataPrefab.GetComponent<AbilityData>();
			m_freelancerItems[i].SetupFreelancerTotalledStats(stats, statName, statDesc, i, component, m_characterType);
		}
		for (; i < m_freelancerItems.Length; i++)
		{
			UIManager.SetGameObjectActive(m_freelancerItems[i], false);
		}
	}

	private void UpdatePercentiles(UIGameOverStatWidget[] widgets, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		foreach (UIGameOverStatWidget statWidget in widgets)
		{
			if (!statWidget.gameObject.activeSelf)
			{
				return;
			}
			PercentileInfo value = null;
			if (percentiles != null)
			{
				percentiles.TryGetValue(statWidget.GeneralStatType, out value);
			}
			statWidget.UpdatePercentiles(value);
		}
	}
}
