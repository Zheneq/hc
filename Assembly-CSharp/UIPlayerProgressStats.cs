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

	public bool HasGlobalStatsToCompareTo
	{
		get;
		private set;
	}

	public bool HasFreelancerStatsToCompareTo
	{
		get;
		private set;
	}

	public string StatCompareFailure
	{
		get;
		private set;
	}

	private void Init()
	{
		if (!m_initialized)
		{
			m_initialized = true;
			m_freelancerItems = m_freelancerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
			m_generalItems = m_generalLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
			m_firepowerItems = m_firepowerLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
			m_supportItems = m_supportLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
			m_frontlineItems = m_frontlineLayout.GetComponentsInChildren<UIGameOverStatWidget>(true);
			m_season = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		}
	}

	private void Awake()
	{
		m_freelancerDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(m_characterType, delegate(int charTypeInt)
			{
				if (charTypeInt >= 0)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_characterType = (CharacterType)charTypeInt;
					m_characterRole = CharacterRole.None;
				}
				else
				{
					m_characterType = CharacterType.None;
					m_characterRole = (CharacterRole)(-charTypeInt);
				}
				Setup();
			}, m_freelancerDropdownSlot, true, m_characterRole);
		};
		m_gameModeDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenGameModeDropdown(m_gameType, delegate(int gameModeInt)
			{
				m_gameType = (PersistedStatBucket)gameModeInt;
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
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return true;
						}
					}
				}
				List<PersistedCharacterData> list = new List<PersistedCharacterData>();
				if (m_characterType != 0)
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
					list.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}, m_seasonsDropdownSlot);
		};
		m_numWins.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
		{
			UISimpleTooltip uISimpleTooltip = tooltip as UISimpleTooltip;
			if (uISimpleTooltip != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						string text = string.Format(StringUtil.TR("MatchesPlayed", "Global"), m_matchesPlayed);
						if (m_matchesPlayed > 0)
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
							text = text + Environment.NewLine + string.Format(StringUtil.TR("WinPercentage", "Global"), m_matchesWon * 100 / m_matchesPlayed);
						}
						uISimpleTooltip.Setup(text);
						return true;
					}
					}
				}
			}
			return false;
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
		List<PersistedCharacterData> list = new List<PersistedCharacterData>();
		if (m_characterType != 0)
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
			list.Add(ClientGameManager.Get().GetPlayerCharacterData(m_characterType));
		}
		else
		{
			list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
		}
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = ((PersistedCharacterData x) => x.ExperienceComponent.PersistedStatsDictionaryBySeason.Keys);
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
						switch (2)
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
		UIManager.SetGameObjectActive(m_seasonsDropdownBtn, false);
	}

	private void Setup()
	{
		Init();
		bool flag = m_season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(m_season);
		if (seasonTemplate != null)
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
			if (!seasonTemplate.IsTutorial)
			{
				goto IL_006d;
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
		}
		m_season = -1;
		flag = false;
		goto IL_006d;
		IL_006d:
		m_gameModeDropdownBtn.Setup(StringUtil.TR_PersistedStatBucketName(m_gameType));
		string text;
		if (m_season < 0)
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
			text = string.Empty;
		}
		else if (flag)
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
			text = StringUtil.TR("CurrentSeason", "Global");
		}
		else
		{
			text = SeasonWideData.Get().GetSeasonTemplate(m_season).GetDisplayName();
		}
		m_seasonsDropdownBtn.Setup(text);
		PersistedStats stats;
		if (m_characterType != 0)
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
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(m_characterType);
			GetStats(playerCharacterData, flag, out stats);
			m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(m_characterType), m_characterType);
			UIManager.SetGameObjectActive(m_freelancerLayout, true);
			HandleFreelancerRow(stats);
		}
		else if (m_characterRole != 0)
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
			UIManager.SetGameObjectActive(m_freelancerLayout, false);
			stats = new PersistedStats();
			using (Dictionary<CharacterType, PersistedCharacterData>.ValueCollection.Enumerator enumerator = ClientGameManager.Get().GetAllPlayerCharacterData().Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PersistedCharacterData current = enumerator.Current;
					if (current.CharacterType.IsValidForHumanGameplay())
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
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(current.CharacterType);
						if (characterResourceLink.m_characterRole == m_characterRole)
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
							GetStats(current, flag, out PersistedStats stats2);
							if (stats2 != null)
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
								stats.CombineStats(stats2);
							}
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
					break;
				}
			}
			m_freelancerDropdownBtn.Setup(StringUtil.TR("CharacterRole_" + m_characterRole, "Global"), m_characterRole);
		}
		else
		{
			UIManager.SetGameObjectActive(m_freelancerLayout, false);
			stats = new PersistedStats();
			foreach (PersistedCharacterData value in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				if (value.CharacterType.IsValidForHumanGameplay())
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
					GetStats(value, flag, out PersistedStats stats3);
					if (stats3 != null)
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
						stats.CombineStats(stats3);
					}
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
			m_matchesWon = (m_matchesPlayed = 0);
		}
		m_numWins.text = string.Format(StringUtil.TR("MatchesWon", "Global"), m_matchesWon);
		HandleStatRow(m_generalItems, StatDisplaySettings.GeneralStats, stats);
		HandleStatRow(m_firepowerItems, StatDisplaySettings.FirepowerStats, stats);
		HandleStatRow(m_supportItems, StatDisplaySettings.SupportStats, stats);
		HandleStatRow(m_frontlineItems, StatDisplaySettings.FrontlinerStats, stats);
		if (m_characterType.IsValidForHumanGameplay())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					string percentilesResponseKey = GetPercentilesResponseKey();
					if (m_percentileInfos.ContainsKey(percentilesResponseKey))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								UpdateAllPercentiles(m_percentileInfos[percentilesResponseKey]);
								return;
							}
						}
					}
					ClientGameManager.Get().CalculateFreelancerStats(m_gameType, m_characterType, stats, UpdateAllPercentiles);
					return;
				}
				}
			}
		}
		UpdateAllPercentiles(null);
	}

	private string GetPercentilesResponseKey()
	{
		return $"{m_gameType}|{m_characterType}|{m_season}";
	}

	private void UpdateAllPercentiles(CalculateFreelancerStatsResponse response)
	{
		if (response != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (response.Success)
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
				bool flag = !response.FreelancerSpecificPercentiles.IsNullOrEmpty();
				for (int i = 0; i < m_freelancerItems.Length; i++)
				{
					if (flag)
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
						if (response.FreelancerSpecificPercentiles.ContainsKey(i))
						{
							HasFreelancerStatsToCompareTo = true;
							m_freelancerItems[i].UpdatePercentiles(response.FreelancerSpecificPercentiles[i]);
							continue;
						}
					}
					HasFreelancerStatsToCompareTo = false;
					m_freelancerItems[i].UpdatePercentiles(null);
				}
				if (!response.GlobalPercentiles.IsNullOrEmpty())
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
				goto IL_01d9;
			}
		}
		for (int j = 0; j < m_freelancerItems.Length; j++)
		{
			m_freelancerItems[j].UpdatePercentiles(null);
		}
		UpdatePercentiles(m_generalItems, null);
		UpdatePercentiles(m_firepowerItems, null);
		UpdatePercentiles(m_supportItems, null);
		UpdatePercentiles(m_frontlineItems, null);
		HasGlobalStatsToCompareTo = false;
		HasFreelancerStatsToCompareTo = false;
		if (response != null && response.LocalizedFailure != null)
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
			StatCompareFailure = response.LocalizedFailure.ToString();
		}
		goto IL_01d9;
		IL_01d9:
		if (response == null)
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
			m_percentileInfos[GetPercentilesResponseKey()] = response;
			return;
		}
	}

	private void GetStats(PersistedCharacterData charData, bool isCurrentSeason, out PersistedStats stats)
	{
		if (isCurrentSeason)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					charData.ExperienceComponent.PersistedStatsDictionary.TryGetValue(m_gameType, out stats);
					return;
				}
			}
		}
		if (!charData.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(m_season))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					stats = null;
					return;
				}
			}
		}
		charData.ExperienceComponent.PersistedStatsDictionaryBySeason[m_season].TryGetValue(m_gameType, out stats);
	}

	private void HandleStatRow(UIGameOverStatWidget[] widgets, StatDisplaySettings.StatType[] statTypes, PersistedStats stats)
	{
		int i;
		for (i = 0; i < statTypes.Length; i++)
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
			if (i < widgets.Length)
			{
				UIManager.SetGameObjectActive(widgets[i], true);
				widgets[i].SetupTotalledStat(stats, statTypes[i], m_characterType);
				continue;
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
			break;
		}
		for (; i < widgets.Length; i++)
		{
			UIManager.SetGameObjectActive(widgets[i], false);
		}
	}

	private void HandleFreelancerRow(PersistedStats stats)
	{
		int i = 0;
		while (true)
		{
			if (i < m_freelancerItems.Length)
			{
				string text = StringUtil.TR_FreelancerStatName(m_characterType.ToString(), i);
				string text2 = StringUtil.TR_FreelancerStatDescription(m_characterType.ToString(), i);
				if (text.IsNullOrEmpty())
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (text2.IsNullOrEmpty())
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
						break;
					}
				}
				UIManager.SetGameObjectActive(m_freelancerItems[i], true);
				AbilityData component = GameWideData.Get().GetCharacterResourceLink(m_characterType).ActorDataPrefab.GetComponent<AbilityData>();
				m_freelancerItems[i].SetupFreelancerTotalledStats(stats, text, text2, i, component, m_characterType);
				i++;
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
			break;
		}
		for (; i < m_freelancerItems.Length; i++)
		{
			UIManager.SetGameObjectActive(m_freelancerItems[i], false);
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

	private void UpdatePercentiles(UIGameOverStatWidget[] widgets, Dictionary<StatDisplaySettings.StatType, PercentileInfo> percentiles)
	{
		for (int i = 0; i < widgets.Length; i++)
		{
			if (!widgets[i].gameObject.activeSelf)
			{
				return;
			}
			PercentileInfo value = null;
			if (percentiles != null)
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
				percentiles.TryGetValue(widgets[i].GeneralStatType, out value);
			}
			widgets[i].UpdatePercentiles(value);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
