using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressOverview : UIPlayerProgressSubPanel
{
	public enum OverviewStat
	{
		TimePlayed,
		MatchesWon,
		WinPercentage,
		NumBadges,
		DamageEfficiency,
		AverageTakedownsPerLife,
		AverageTakedownsPerMatch,
		AverageDeathsPerMatch,
		AverageDamageDonePerTurn,
		AverageSupportDonePerTurn,
		AverageDamageTakenPerTurn
	}

	private class SeasonBucket
	{
		public int Season
		{
			get;
			private set;
		}

		public PersistedStatBucket StatBucket
		{
			get;
			private set;
		}

		public string DisplayString
		{
			get;
			private set;
		}

		public SeasonBucket(SeasonTemplate season, PersistedStatBucket statBucket)
		{
			Season = season.Index;
			StatBucket = statBucket;
			if (Season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason)
			{
				DisplayString = StringUtil.TR("CurrentSeason", "Global");
			}
			else
			{
				DisplayString = season.GetDisplayName();
			}
			DisplayString = DisplayString + ": " + StringUtil.TR_PersistedStatBucketName(statBucket);
		}
	}

	public TextMeshProUGUI m_numTankGames;

	public ImageFilledSloped m_numTankWins;

	public TextMeshProUGUI m_numAssassinGames;

	public ImageFilledSloped m_numAssassinWins;

	public TextMeshProUGUI m_numSupportGames;

	public ImageFilledSloped m_numSupportWins;

	public UIPlayerProgressReward[] m_rewardDisplays;

	public UIPlayerProgressMostPlayedItem[] m_mostUsedHeroDisplays;

	public RectTransform m_rankDisplayContainer;

	public UIPlayerProfileRankDisplay[] m_rankDisplays;

	public UIFreelancerComparisonItem m_freelancerComparisonPrefab;

	public VerticalLayoutGroup m_freelancerComparisonGrid;

	public UIPlayerProgressDropdownBtn m_freelancerComparisonDropdownBtn;

	public UIPlayerProgressDropdownList m_freelancerComparisonDropdown;

	public UIPlayerProgressDropdownBtn m_seasonBucketDropdownBtn;

	public UIPlayerProgressDropdownList m_seasonBucketDropdown;

	private OverviewStat m_overviewStat;

	private List<SeasonBucket> m_seasonBuckets;

	private int m_currentSeasonBucket = -1;

	private List<UIFreelancerComparisonItem> m_freelancerComparisonList;

	private Dictionary<CharacterType, UIFreelancerComparisonItem> m_freelancerComparisonMap;

	private bool m_isDestroyed;

	private void Start()
	{
		m_freelancerComparisonDropdownBtn.m_button.spriteController.callback = OpenFreelancerComparisonDropdown;
		m_seasonBucketDropdownBtn.m_button.spriteController.callback = OpenSeasonBucketDropdown;
		SetupFreelancerRankings();
	}

	private void OnDisable()
	{
		m_freelancerComparisonDropdown.SetVisible(false);
	}

	private void OnDestroy()
	{
		m_isDestroyed = true;
	}

	public void Setup(PersistedAccountData playerData, List<PersistedCharacterData> characterData)
	{
		if (playerData == null)
		{
			return;
		}
		int i;
		while (true)
		{
			if (characterData == null)
			{
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
			i = 0;
			
			using (IEnumerator<PersistedCharacterData> enumerator = characterData.OrderByDescending(((PersistedCharacterData x) => x.ExperienceComponent.Matches)).Take(m_mostUsedHeroDisplays.Length).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PersistedCharacterData current = enumerator.Current;
					if (current.ExperienceComponent.Matches > 0)
					{
						m_mostUsedHeroDisplays[i].Setup(current);
						i++;
					}
				}
			}
			for (; i < m_mostUsedHeroDisplays.Length; i++)
			{
				m_mostUsedHeroDisplays[i].Setup(null);
			}
			while (true)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				using (List<PersistedCharacterData>.Enumerator enumerator2 = characterData.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						PersistedCharacterData current2 = enumerator2.Current;
						if (!current2.CharacterType.IsValidForHumanGameplay())
						{
						}
						else
						{
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(current2.CharacterType);
							if ((bool)characterResourceLink)
							{
								if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
								{
									num2 += current2.ExperienceComponent.Matches;
								}
								else if (characterResourceLink.m_characterRole == CharacterRole.Tank)
								{
									num += current2.ExperienceComponent.Matches;
								}
								else if (characterResourceLink.m_characterRole == CharacterRole.Support)
								{
									num3 += current2.ExperienceComponent.Matches;
								}
							}
						}
					}
				}
				int num4 = num2 + num + num3;
				m_numTankGames.text = num.ToString();
				m_numTankWins.fillAmount = (float)num / (float)num4;
				m_numAssassinGames.text = num2.ToString();
				m_numAssassinWins.fillAmount = (float)num2 / (float)num4;
				m_numSupportGames.text = num3.ToString();
				m_numSupportWins.fillAmount = (float)num3 / (float)num4;
				bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
				int num5 = 0;
				using (List<PersistedCharacterData>.Enumerator enumerator3 = characterData.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						PersistedCharacterData current3 = enumerator3.Current;
						if (!current3.CharacterType.IsValidForHumanGameplay())
						{
						}
						else
						{
							if (!flag)
							{
								if (GameWideData.Get().GetCharacterResourceLink(current3.CharacterType).m_isHidden)
								{
									continue;
								}
							}
							num5 += current3.ExperienceComponent.Level - 1;
						}
					}
				}
				UIManager.SetGameObjectActive(m_rankDisplayContainer, false);
				ClientGameManager.Get().RequestRankedLeaderboardOverview(GameType.Ranked, delegate(RankedLeaderboardOverviewResponse overviewResponse)
				{
					if (overviewResponse.Success)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (!m_isDestroyed)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											UIManager.SetGameObjectActive(m_rankDisplayContainer, true);
											for (i = 0; i < m_rankDisplays.Length; i++)
											{
												m_rankDisplays[i].Setup((UIRankDisplayType)i, overviewResponse.TierInfoPerGroupSize);
											}
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
									}
								}
								return;
							}
						}
					}
				});
				if (m_freelancerComparisonDropdown.Initialize())
				{
					IEnumerator enumerator4 = Enum.GetValues(typeof(OverviewStat)).GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							OverviewStat overviewStat = (OverviewStat)enumerator4.Current;
							m_freelancerComparisonDropdown.AddOption((int)overviewStat, StringUtil.TR("FreelancerOverviewCategory_" + overviewStat, "Global"));
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator4 as IDisposable)) != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									disposable.Dispose();
									goto end_IL_0429;
								}
							}
						}
						end_IL_0429:;
					}
					m_freelancerComparisonDropdown.AddHitbox(m_freelancerComparisonDropdownBtn.m_button.spriteController.gameObject);
					m_freelancerComparisonDropdown.SetSelectCallback(delegate(int overviewStatTypeInt)
					{
						m_overviewStat = (OverviewStat)overviewStatTypeInt;
						SetupBucketDropdown();
						SetupFreelancerRankings();
					});
				}
				if (m_seasonBucketDropdown.Initialize())
				{
					m_seasonBuckets = new List<SeasonBucket>();
					List<SeasonTemplate> list = new List<SeasonTemplate>();
					for (i = 0; i < SeasonWideData.Get().m_seasons.Count; i++)
					{
						if (!SeasonWideData.Get().m_seasons[i].IsTutorial)
						{
							list.Add(SeasonWideData.Get().m_seasons[i]);
						}
					}
					list.Reverse();
					foreach (SeasonTemplate item in list)
					{
						IEnumerator enumerator6 = Enum.GetValues(typeof(PersistedStatBucket)).GetEnumerator();
						try
						{
							while (enumerator6.MoveNext())
							{
								PersistedStatBucket persistedStatBucket = (PersistedStatBucket)enumerator6.Current;
								if (persistedStatBucket.IsTracked())
								{
									SeasonBucket seasonBucket = new SeasonBucket(item, persistedStatBucket);
									m_seasonBucketDropdown.AddOption(m_seasonBuckets.Count, seasonBucket.DisplayString);
									m_seasonBuckets.Add(seasonBucket);
								}
							}
						}
						finally
						{
							IDisposable disposable2;
							if ((disposable2 = (enumerator6 as IDisposable)) != null)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										disposable2.Dispose();
										goto end_IL_05de;
									}
								}
							}
							end_IL_05de:;
						}
					}
					m_seasonBucketDropdown.AddHitbox(m_seasonBucketDropdownBtn.m_button.spriteController.gameObject);
					m_seasonBucketDropdown.SetSelectCallback(delegate(int bucketIndex)
					{
						m_currentSeasonBucket = bucketIndex;
						SetupFreelancerRankings();
					});
					SetupBucketDropdown();
				}
				if (m_freelancerComparisonList != null)
				{
					return;
				}
				while (true)
				{
					m_freelancerComparisonList = new List<UIFreelancerComparisonItem>();
					m_freelancerComparisonList.AddRange(m_freelancerComparisonGrid.GetComponentsInChildren<UIFreelancerComparisonItem>(true));
					m_freelancerComparisonMap = new Dictionary<CharacterType, UIFreelancerComparisonItem>();
					i = 0;
					CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
					foreach (CharacterResourceLink characterResourceLink2 in characterResourceLinks)
					{
						if (!characterResourceLink2.m_characterType.IsValidForHumanGameplay() || !characterResourceLink2.m_allowForPlayers)
						{
							continue;
						}
						if (!flag && characterResourceLink2.m_isHidden)
						{
							continue;
						}
						CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink2.m_characterType);
						if (!characterConfig.AllowForPlayers || characterConfig.IsHidden)
						{
							continue;
						}
						UIFreelancerComparisonItem uIFreelancerComparisonItem;
						if (i < m_freelancerComparisonList.Count)
						{
							uIFreelancerComparisonItem = m_freelancerComparisonList[i];
						}
						else
						{
							uIFreelancerComparisonItem = UnityEngine.Object.Instantiate(m_freelancerComparisonPrefab);
							uIFreelancerComparisonItem.transform.SetParent(m_freelancerComparisonGrid.transform);
							uIFreelancerComparisonItem.transform.localPosition = Vector3.zero;
							uIFreelancerComparisonItem.transform.localScale = Vector3.one;
							m_freelancerComparisonList.Add(uIFreelancerComparisonItem);
						}
						UIManager.SetGameObjectActive(uIFreelancerComparisonItem, true);
						uIFreelancerComparisonItem.Setup(characterResourceLink2);
						m_freelancerComparisonMap.Add(characterResourceLink2.m_characterType, uIFreelancerComparisonItem);
						i++;
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
			}
		}
	}

	private void SetupFreelancerRankings()
	{
		m_freelancerComparisonDropdownBtn.Setup(StringUtil.TR("FreelancerOverviewCategory_" + m_overviewStat, "Global"));
		for (int i = 0; i < m_freelancerComparisonList.Count; i++)
		{
			m_freelancerComparisonList[i].SetupNewStat(m_overviewStat);
		}
		while (true)
		{
			if (m_currentSeasonBucket < 0)
			{
				UIManager.SetGameObjectActive(m_seasonBucketDropdownBtn, false);
			}
			else
			{
				m_seasonBucketDropdownBtn.Setup(m_seasonBuckets[m_currentSeasonBucket].DisplayString);
				UIManager.SetGameObjectActive(m_seasonBucketDropdownBtn, true);
				ClientGameManager clientGameManager = ClientGameManager.Get();
				int season = m_seasonBuckets[m_currentSeasonBucket].Season;
				PersistedStatBucket statBucket = m_seasonBuckets[m_currentSeasonBucket].StatBucket;
				if (m_overviewStat == OverviewStat.NumBadges)
				{
					bool flag = season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
					using (Dictionary<CharacterType, PersistedCharacterData>.ValueCollection.Enumerator enumerator = clientGameManager.GetAllPlayerCharacterData().Values.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PersistedCharacterData current = enumerator.Current;
							if (!m_freelancerComparisonMap.ContainsKey(current.CharacterType))
							{
							}
							else
							{
								Dictionary<PersistedStatBucket, Dictionary<int, int>> dictionary;
								if (flag)
								{
									dictionary = current.ExperienceComponent.BadgesEarned;
								}
								else
								{
									if (!current.ExperienceComponent.BadgesEarnedBySeason.ContainsKey(season))
									{
										continue;
									}
									dictionary = current.ExperienceComponent.BadgesEarnedBySeason[season];
								}
								if (dictionary.ContainsKey(statBucket))
								{
									m_freelancerComparisonMap[current.CharacterType].Adjust(dictionary[statBucket].Values.Sum());
								}
							}
						}
					}
				}
				else
				{
					bool flag2 = season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
					using (Dictionary<CharacterType, PersistedCharacterData>.ValueCollection.Enumerator enumerator2 = clientGameManager.GetAllPlayerCharacterData().Values.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PersistedCharacterData current2 = enumerator2.Current;
							if (!m_freelancerComparisonMap.ContainsKey(current2.CharacterType))
							{
							}
							else
							{
								Dictionary<PersistedStatBucket, PersistedStats> dictionary2;
								if (flag2)
								{
									dictionary2 = current2.ExperienceComponent.PersistedStatsDictionary;
								}
								else
								{
									if (!current2.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(season))
									{
										continue;
									}
									dictionary2 = current2.ExperienceComponent.PersistedStatsDictionaryBySeason[season];
								}
								if (dictionary2.ContainsKey(statBucket))
								{
									if (m_overviewStat == OverviewStat.DamageEfficiency)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].DamageEfficiency);
									}
									else if (m_overviewStat == OverviewStat.AverageTakedownsPerLife)
									{
										PersistedStatEntry copy = dictionary2[statBucket].TotalPlayerAssists.GetCopy();
										copy.NumGamesInSum += dictionary2[statBucket].TotalDeaths.Sum;
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(copy);
									}
									else if (m_overviewStat == OverviewStat.AverageTakedownsPerMatch)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].TotalPlayerAssists);
									}
									else if (m_overviewStat == OverviewStat.AverageDeathsPerMatch)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].TotalDeaths);
									}
									else if (m_overviewStat == OverviewStat.AverageDamageDonePerTurn)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].DamagePerTurn);
									}
									else if (m_overviewStat == OverviewStat.AverageSupportDonePerTurn)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].SupportPerTurn);
									}
									else if (m_overviewStat == OverviewStat.AverageDamageTakenPerTurn)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].DamageTakenPerTurn);
									}
									else if (m_overviewStat == OverviewStat.TimePlayed)
									{
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].SecondsPlayed);
									}
									else
									{
										if (m_overviewStat != OverviewStat.MatchesWon)
										{
											if (m_overviewStat != OverviewStat.WinPercentage)
											{
												continue;
											}
										}
										m_freelancerComparisonMap[current2.CharacterType].CombineStats(dictionary2[statBucket].MatchesWon);
									}
								}
							}
						}
					}
				}
			}
			List<UIFreelancerComparisonItem> freelancerComparisonList = m_freelancerComparisonList;
			
			freelancerComparisonList.Sort(delegate(UIFreelancerComparisonItem x, UIFreelancerComparisonItem y)
				{
					int num = x.GetValue().CompareTo(y.GetValue());
					int result;
					if (num == 0)
					{
						result = y.m_characterName.text.CompareTo(x.m_characterName.text);
					}
					else
					{
						result = num;
					}
					return result;
				});
			float value = m_freelancerComparisonList[m_freelancerComparisonList.Count - 1].GetValue();
			for (int j = 0; j < m_freelancerComparisonList.Count; j++)
			{
				m_freelancerComparisonList[j].transform.SetAsFirstSibling();
				m_freelancerComparisonList[j].SetupDisplay(value);
			}
			return;
		}
	}

	private List<PersistedStats> GetStatsList(PersistedCharacterData charData)
	{
		List<PersistedStats> list = new List<PersistedStats>();
		List<Dictionary<PersistedStatBucket, PersistedStats>> list2 = new List<Dictionary<PersistedStatBucket, PersistedStats>>();
		list2.AddRange(charData.ExperienceComponent.PersistedStatsDictionaryBySeason.Values);
		list2.Add(charData.ExperienceComponent.PersistedStatsDictionary);
		for (int i = 0; i < list2.Count; i++)
		{
			using (Dictionary<PersistedStatBucket, PersistedStats>.Enumerator enumerator = list2[i].GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<PersistedStatBucket, PersistedStats> current = enumerator.Current;
					if (current.Key != PersistedStatBucket.DoNotPersist && current.Key != 0)
					{
						list.Add(current.Value);
					}
				}
			}
		}
		while (true)
		{
			return list;
		}
	}

	private void OpenFreelancerComparisonDropdown(BaseEventData data)
	{
		m_freelancerComparisonDropdown.Toggle();
		m_freelancerComparisonDropdown.HighlightCurrentOption((int)m_overviewStat);
	}

	private void OpenSeasonBucketDropdown(BaseEventData data)
	{
		m_seasonBucketDropdown.Toggle();
		m_seasonBucketDropdown.HighlightCurrentOption(m_currentSeasonBucket);
	}

	private void SetupBucketDropdown()
	{
		bool[] shouldShow = new bool[m_seasonBuckets.Count];
		int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		OverviewStat overviewStat = m_overviewStat;
		if (overviewStat == OverviewStat.NumBadges)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (PersistedCharacterData value in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				foreach (KeyValuePair<PersistedStatBucket, Dictionary<int, int>> item in value.ExperienceComponent.BadgesEarned)
				{
					PersistedStatBucket key = item.Key;
					int num = item.Value.Values.Sum();
					if (num > 0)
					{
						dictionary[activeSeason + key.ToString()] = true;
					}
				}
				using (Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>>.Enumerator enumerator3 = value.ExperienceComponent.BadgesEarnedBySeason.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>> current3 = enumerator3.Current;
						int key2 = current3.Key;
						using (Dictionary<PersistedStatBucket, Dictionary<int, int>>.Enumerator enumerator4 = current3.Value.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								KeyValuePair<PersistedStatBucket, Dictionary<int, int>> current4 = enumerator4.Current;
								PersistedStatBucket key3 = current4.Key;
								int num2 = current4.Value.Values.Sum();
								if (num2 > 0)
								{
									dictionary[key2 + key3.ToString()] = true;
								}
							}
						}
					}
				}
			}
			for (int i = 0; i < m_seasonBuckets.Count; i++)
			{
				int season = m_seasonBuckets[i].Season;
				PersistedStatBucket statBucket = m_seasonBuckets[i].StatBucket;
				shouldShow[i] = dictionary.ContainsKey(season + statBucket.ToString());
			}
		}
		else
		{
			List<PersistedCharacterData> list = new List<PersistedCharacterData>();
			list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
			for (int j = 0; j < m_seasonBuckets.Count; j++)
			{
				int num3 = 0;
				while (true)
				{
					if (num3 < list.Count)
					{
						Dictionary<PersistedStatBucket, PersistedStats> dictionary2;
						if (m_seasonBuckets[j].Season == activeSeason)
						{
							dictionary2 = list[num3].ExperienceComponent.PersistedStatsDictionary;
						}
						else
						{
							if (!list[num3].ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(m_seasonBuckets[j].Season))
							{
								goto IL_03a3;
							}
							dictionary2 = list[num3].ExperienceComponent.PersistedStatsDictionaryBySeason[m_seasonBuckets[j].Season];
						}
						if (dictionary2.ContainsKey(m_seasonBuckets[j].StatBucket))
						{
							shouldShow[j] = true;
							break;
						}
						goto IL_03a3;
					}
					break;
					IL_03a3:
					num3++;
				}
			}
		}
		m_seasonBucketDropdown.CheckOptionDisplayState((int seasonBucketIndex) => shouldShow[seasonBucketIndex]);
		int num4 = 0;
		if (!m_seasonBucketDropdown.IsOptionVisible(m_currentSeasonBucket))
		{
			m_currentSeasonBucket = -1;
		}
		for (int k = 0; k < shouldShow.Length; k++)
		{
			if (!shouldShow[k])
			{
				continue;
			}
			if (m_currentSeasonBucket < 0)
			{
				m_currentSeasonBucket = k;
			}
			num4++;
		}
		while (true)
		{
			m_seasonBucketDropdownBtn.m_button.SetDisabled(num4 < 2);
			return;
		}
	}
}
