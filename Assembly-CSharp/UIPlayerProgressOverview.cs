using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressOverview : UIPlayerProgressSubPanel
{
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

	private UIPlayerProgressOverview.OverviewStat m_overviewStat;

	private List<UIPlayerProgressOverview.SeasonBucket> m_seasonBuckets;

	private int m_currentSeasonBucket = -1;

	private List<UIFreelancerComparisonItem> m_freelancerComparisonList;

	private Dictionary<CharacterType, UIFreelancerComparisonItem> m_freelancerComparisonMap;

	private bool m_isDestroyed;

	private void Start()
	{
		this.m_freelancerComparisonDropdownBtn.m_button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OpenFreelancerComparisonDropdown);
		this.m_seasonBucketDropdownBtn.m_button.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OpenSeasonBucketDropdown);
		this.SetupFreelancerRankings();
	}

	private void OnDisable()
	{
		this.m_freelancerComparisonDropdown.SetVisible(false);
	}

	private void OnDestroy()
	{
		this.m_isDestroyed = true;
	}

	public void Setup(PersistedAccountData playerData, List<PersistedCharacterData> characterData)
	{
		if (playerData != null)
		{
			if (characterData != null)
			{
				int i = 0;
				
				using (IEnumerator<PersistedCharacterData> enumerator = characterData.OrderByDescending(((PersistedCharacterData x) => x.ExperienceComponent.Matches)).Take(this.m_mostUsedHeroDisplays.Length).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PersistedCharacterData persistedCharacterData = enumerator.Current;
						if (persistedCharacterData.ExperienceComponent.Matches > 0)
						{
							this.m_mostUsedHeroDisplays[i].Setup(persistedCharacterData);
							i++;
						}
					}
				}
				while (i < this.m_mostUsedHeroDisplays.Length)
				{
					this.m_mostUsedHeroDisplays[i].Setup(null);
					i++;
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				using (List<PersistedCharacterData>.Enumerator enumerator2 = characterData.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						PersistedCharacterData persistedCharacterData2 = enumerator2.Current;
						if (!persistedCharacterData2.CharacterType.IsValidForHumanGameplay())
						{
						}
						else
						{
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(persistedCharacterData2.CharacterType);
							if (characterResourceLink)
							{
								if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
								{
									num2 += persistedCharacterData2.ExperienceComponent.Matches;
								}
								else if (characterResourceLink.m_characterRole == CharacterRole.Tank)
								{
									num += persistedCharacterData2.ExperienceComponent.Matches;
								}
								else if (characterResourceLink.m_characterRole == CharacterRole.Support)
								{
									num3 += persistedCharacterData2.ExperienceComponent.Matches;
								}
							}
						}
					}
				}
				int num4 = num2 + num + num3;
				this.m_numTankGames.text = num.ToString();
				this.m_numTankWins.fillAmount = (float)num / (float)num4;
				this.m_numAssassinGames.text = num2.ToString();
				this.m_numAssassinWins.fillAmount = (float)num2 / (float)num4;
				this.m_numSupportGames.text = num3.ToString();
				this.m_numSupportWins.fillAmount = (float)num3 / (float)num4;
				bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
				int num5 = 0;
				using (List<PersistedCharacterData>.Enumerator enumerator3 = characterData.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						PersistedCharacterData persistedCharacterData3 = enumerator3.Current;
						if (!persistedCharacterData3.CharacterType.IsValidForHumanGameplay())
						{
						}
						else
						{
							if (!flag)
							{
								if (GameWideData.Get().GetCharacterResourceLink(persistedCharacterData3.CharacterType).m_isHidden)
								{
									continue;
								}
							}
							num5 += persistedCharacterData3.ExperienceComponent.Level - 1;
						}
					}
				}
				UIManager.SetGameObjectActive(this.m_rankDisplayContainer, false, null);
				ClientGameManager.Get().RequestRankedLeaderboardOverview(GameType.Ranked, delegate(RankedLeaderboardOverviewResponse overviewResponse)
				{
					if (overviewResponse.Success)
					{
						if (!this.m_isDestroyed)
						{
							UIManager.SetGameObjectActive(this.m_rankDisplayContainer, true, null);
							for (i = 0; i < this.m_rankDisplays.Length; i++)
							{
								this.m_rankDisplays[i].Setup((UIRankDisplayType)i, overviewResponse.TierInfoPerGroupSize);
							}
						}
					}
				});
				if (this.m_freelancerComparisonDropdown.Initialize())
				{
					IEnumerator enumerator4 = Enum.GetValues(typeof(UIPlayerProgressOverview.OverviewStat)).GetEnumerator();
					try
					{
						while (enumerator4.MoveNext())
						{
							object obj = enumerator4.Current;
							UIPlayerProgressOverview.OverviewStat overviewStat = (UIPlayerProgressOverview.OverviewStat)obj;
							this.m_freelancerComparisonDropdown.AddOption((int)overviewStat, StringUtil.TR("FreelancerOverviewCategory_" + overviewStat, "Global"), CharacterType.None);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator4 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					this.m_freelancerComparisonDropdown.AddHitbox(this.m_freelancerComparisonDropdownBtn.m_button.spriteController.gameObject);
					this.m_freelancerComparisonDropdown.SetSelectCallback(delegate(int overviewStatTypeInt)
					{
						this.m_overviewStat = (UIPlayerProgressOverview.OverviewStat)overviewStatTypeInt;
						this.SetupBucketDropdown();
						this.SetupFreelancerRankings();
					});
				}
				if (this.m_seasonBucketDropdown.Initialize())
				{
					this.m_seasonBuckets = new List<UIPlayerProgressOverview.SeasonBucket>();
					List<SeasonTemplate> list = new List<SeasonTemplate>();
					for (i = 0; i < SeasonWideData.Get().m_seasons.Count; i++)
					{
						if (!SeasonWideData.Get().m_seasons[i].IsTutorial)
						{
							list.Add(SeasonWideData.Get().m_seasons[i]);
						}
					}
					list.Reverse();
					foreach (SeasonTemplate season in list)
					{
						IEnumerator enumerator6 = Enum.GetValues(typeof(PersistedStatBucket)).GetEnumerator();
						try
						{
							while (enumerator6.MoveNext())
							{
								object obj2 = enumerator6.Current;
								PersistedStatBucket persistedStatBucket = (PersistedStatBucket)obj2;
								if (persistedStatBucket.IsTracked())
								{
									UIPlayerProgressOverview.SeasonBucket seasonBucket = new UIPlayerProgressOverview.SeasonBucket(season, persistedStatBucket);
									this.m_seasonBucketDropdown.AddOption(this.m_seasonBuckets.Count, seasonBucket.DisplayString, CharacterType.None);
									this.m_seasonBuckets.Add(seasonBucket);
								}
							}
						}
						finally
						{
							IDisposable disposable2;
							if ((disposable2 = (enumerator6 as IDisposable)) != null)
							{
								disposable2.Dispose();
							}
						}
					}
					this.m_seasonBucketDropdown.AddHitbox(this.m_seasonBucketDropdownBtn.m_button.spriteController.gameObject);
					this.m_seasonBucketDropdown.SetSelectCallback(delegate(int bucketIndex)
					{
						this.m_currentSeasonBucket = bucketIndex;
						this.SetupFreelancerRankings();
					});
					this.SetupBucketDropdown();
				}
				if (this.m_freelancerComparisonList == null)
				{
					this.m_freelancerComparisonList = new List<UIFreelancerComparisonItem>();
					this.m_freelancerComparisonList.AddRange(this.m_freelancerComparisonGrid.GetComponentsInChildren<UIFreelancerComparisonItem>(true));
					this.m_freelancerComparisonMap = new Dictionary<CharacterType, UIFreelancerComparisonItem>();
					i = 0;
					CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
					int j = 0;
					while (j < characterResourceLinks.Length)
					{
						CharacterResourceLink characterResourceLink2 = characterResourceLinks[j];
						if (characterResourceLink2.m_characterType.IsValidForHumanGameplay() && characterResourceLink2.m_allowForPlayers)
						{
							if (!flag && characterResourceLink2.m_isHidden)
							{
							}
							else
							{
								CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink2.m_characterType);
								if (characterConfig.AllowForPlayers && !characterConfig.IsHidden)
								{
									UIFreelancerComparisonItem uifreelancerComparisonItem;
									if (i < this.m_freelancerComparisonList.Count)
									{
										uifreelancerComparisonItem = this.m_freelancerComparisonList[i];
									}
									else
									{
										uifreelancerComparisonItem = UnityEngine.Object.Instantiate<UIFreelancerComparisonItem>(this.m_freelancerComparisonPrefab);
										uifreelancerComparisonItem.transform.SetParent(this.m_freelancerComparisonGrid.transform);
										uifreelancerComparisonItem.transform.localPosition = Vector3.zero;
										uifreelancerComparisonItem.transform.localScale = Vector3.one;
										this.m_freelancerComparisonList.Add(uifreelancerComparisonItem);
									}
									UIManager.SetGameObjectActive(uifreelancerComparisonItem, true, null);
									uifreelancerComparisonItem.Setup(characterResourceLink2);
									this.m_freelancerComparisonMap.Add(characterResourceLink2.m_characterType, uifreelancerComparisonItem);
									i++;
								}
							}
						}
						IL_7F6:
						j++;
						continue;
						goto IL_7F6;
					}
				}
				return;
			}
		}
	}

	private void SetupFreelancerRankings()
	{
		this.m_freelancerComparisonDropdownBtn.Setup(StringUtil.TR("FreelancerOverviewCategory_" + this.m_overviewStat, "Global"), CharacterType.None);
		for (int i = 0; i < this.m_freelancerComparisonList.Count; i++)
		{
			this.m_freelancerComparisonList[i].SetupNewStat(this.m_overviewStat);
		}
		if (this.m_currentSeasonBucket < 0)
		{
			UIManager.SetGameObjectActive(this.m_seasonBucketDropdownBtn, false, null);
		}
		else
		{
			this.m_seasonBucketDropdownBtn.Setup(this.m_seasonBuckets[this.m_currentSeasonBucket].DisplayString, CharacterType.None);
			UIManager.SetGameObjectActive(this.m_seasonBucketDropdownBtn, true, null);
			ClientGameManager clientGameManager = ClientGameManager.Get();
			int season = this.m_seasonBuckets[this.m_currentSeasonBucket].Season;
			PersistedStatBucket statBucket = this.m_seasonBuckets[this.m_currentSeasonBucket].StatBucket;
			if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.NumBadges)
			{
				bool flag = season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				using (Dictionary<CharacterType, PersistedCharacterData>.ValueCollection.Enumerator enumerator = clientGameManager.GetAllPlayerCharacterData().Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PersistedCharacterData persistedCharacterData = enumerator.Current;
						if (!this.m_freelancerComparisonMap.ContainsKey(persistedCharacterData.CharacterType))
						{
						}
						else
						{
							Dictionary<PersistedStatBucket, Dictionary<int, int>> dictionary;
							if (flag)
							{
								dictionary = persistedCharacterData.ExperienceComponent.BadgesEarned;
							}
							else
							{
								if (!persistedCharacterData.ExperienceComponent.BadgesEarnedBySeason.ContainsKey(season))
								{
									continue;
								}
								dictionary = persistedCharacterData.ExperienceComponent.BadgesEarnedBySeason[season];
							}
							if (dictionary.ContainsKey(statBucket))
							{
								this.m_freelancerComparisonMap[persistedCharacterData.CharacterType].Adjust(dictionary[statBucket].Values.Sum());
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
						PersistedCharacterData persistedCharacterData2 = enumerator2.Current;
						if (!this.m_freelancerComparisonMap.ContainsKey(persistedCharacterData2.CharacterType))
						{
						}
						else
						{
							Dictionary<PersistedStatBucket, PersistedStats> dictionary2;
							if (flag2)
							{
								dictionary2 = persistedCharacterData2.ExperienceComponent.PersistedStatsDictionary;
							}
							else
							{
								if (!persistedCharacterData2.ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(season))
								{
									continue;
								}
								dictionary2 = persistedCharacterData2.ExperienceComponent.PersistedStatsDictionaryBySeason[season];
							}
							if (dictionary2.ContainsKey(statBucket))
							{
								if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.DamageEfficiency)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].DamageEfficiency);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerLife)
								{
									PersistedStatEntry copy = dictionary2[statBucket].TotalPlayerAssists.GetCopy();
									copy.NumGamesInSum += dictionary2[statBucket].TotalDeaths.Sum;
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(copy);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerMatch)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].TotalPlayerAssists);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageDeathsPerMatch)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].TotalDeaths);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageDamageDonePerTurn)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].DamagePerTurn);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageSupportDonePerTurn)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].SupportPerTurn);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.AverageDamageTakenPerTurn)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].DamageTakenPerTurn);
								}
								else if (this.m_overviewStat == UIPlayerProgressOverview.OverviewStat.TimePlayed)
								{
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].SecondsPlayed);
								}
								else
								{
									if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.MatchesWon)
									{
										if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.WinPercentage)
										{
											continue;
										}
									}
									this.m_freelancerComparisonMap[persistedCharacterData2.CharacterType].CombineStats(dictionary2[statBucket].MatchesWon);
								}
							}
						}
					}
				}
			}
		}
		List<UIFreelancerComparisonItem> freelancerComparisonList = this.m_freelancerComparisonList;
		
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
		float value = this.m_freelancerComparisonList[this.m_freelancerComparisonList.Count - 1].GetValue();
		for (int j = 0; j < this.m_freelancerComparisonList.Count; j++)
		{
			this.m_freelancerComparisonList[j].transform.SetAsFirstSibling();
			this.m_freelancerComparisonList[j].SetupDisplay(value);
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
					KeyValuePair<PersistedStatBucket, PersistedStats> keyValuePair = enumerator.Current;
					if (keyValuePair.Key != PersistedStatBucket.DoNotPersist && keyValuePair.Key != PersistedStatBucket.None)
					{
						list.Add(keyValuePair.Value);
					}
				}
			}
		}
		return list;
	}

	private void OpenFreelancerComparisonDropdown(BaseEventData data)
	{
		this.m_freelancerComparisonDropdown.Toggle();
		this.m_freelancerComparisonDropdown.HighlightCurrentOption((int)this.m_overviewStat);
	}

	private void OpenSeasonBucketDropdown(BaseEventData data)
	{
		this.m_seasonBucketDropdown.Toggle();
		this.m_seasonBucketDropdown.HighlightCurrentOption(this.m_currentSeasonBucket);
	}

	private void SetupBucketDropdown()
	{
		bool[] shouldShow = new bool[this.m_seasonBuckets.Count];
		int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		UIPlayerProgressOverview.OverviewStat overviewStat = this.m_overviewStat;
		if (overviewStat != UIPlayerProgressOverview.OverviewStat.NumBadges)
		{
			List<PersistedCharacterData> list = new List<PersistedCharacterData>();
			list.AddRange(ClientGameManager.Get().GetAllPlayerCharacterData().Values);
			
			for (int i = 0; i < this.m_seasonBuckets.Count; i++)
			{
				
				for (int j = 0; j < list.Count; j++)
				{
					Dictionary<PersistedStatBucket, PersistedStats> dictionary;
					if (this.m_seasonBuckets[i].Season == activeSeason)
					{
						dictionary = list[j].ExperienceComponent.PersistedStatsDictionary;
						if (dictionary.ContainsKey(this.m_seasonBuckets[i].StatBucket))
						{
							shouldShow[i] = true;
							break;
						}
					}
					else if (list[j].ExperienceComponent.PersistedStatsDictionaryBySeason.ContainsKey(this.m_seasonBuckets[i].Season))
					{
						dictionary = list[j].ExperienceComponent.PersistedStatsDictionaryBySeason[this.m_seasonBuckets[i].Season];
						if (dictionary.ContainsKey(this.m_seasonBuckets[i].StatBucket))
						{
							shouldShow[i] = true;
							break;
						}
					}
				}
			}
		}
		else
		{
			Dictionary<string, bool> dictionary2 = new Dictionary<string, bool>();
			foreach (PersistedCharacterData persistedCharacterData in ClientGameManager.Get().GetAllPlayerCharacterData().Values)
			{
				foreach (KeyValuePair<PersistedStatBucket, Dictionary<int, int>> keyValuePair in persistedCharacterData.ExperienceComponent.BadgesEarned)
				{
					PersistedStatBucket key = keyValuePair.Key;
					int num = keyValuePair.Value.Values.Sum();
					if (num > 0)
					{
						dictionary2[activeSeason + key.ToString()] = true;
					}
				}
				using (Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>>.Enumerator enumerator3 = persistedCharacterData.ExperienceComponent.BadgesEarnedBySeason.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>> keyValuePair2 = enumerator3.Current;
						int key2 = keyValuePair2.Key;
						using (Dictionary<PersistedStatBucket, Dictionary<int, int>>.Enumerator enumerator4 = keyValuePair2.Value.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								KeyValuePair<PersistedStatBucket, Dictionary<int, int>> keyValuePair3 = enumerator4.Current;
								PersistedStatBucket key3 = keyValuePair3.Key;
								int num2 = keyValuePair3.Value.Values.Sum();
								if (num2 > 0)
								{
									dictionary2[key2 + key3.ToString()] = true;
								}
							}
						}
					}
				}
			}
			for (int k = 0; k < this.m_seasonBuckets.Count; k++)
			{
				int season = this.m_seasonBuckets[k].Season;
				PersistedStatBucket statBucket = this.m_seasonBuckets[k].StatBucket;
				shouldShow[k] = dictionary2.ContainsKey(season + statBucket.ToString());
			}
		}
		this.m_seasonBucketDropdown.CheckOptionDisplayState((int seasonBucketIndex) => shouldShow[seasonBucketIndex]);
		int num3 = 0;
		if (!this.m_seasonBucketDropdown.IsOptionVisible(this.m_currentSeasonBucket))
		{
			this.m_currentSeasonBucket = -1;
		}
		for (int l = 0; l < shouldShow.Length; l++)
		{
			if (shouldShow[l])
			{
				if (this.m_currentSeasonBucket < 0)
				{
					this.m_currentSeasonBucket = l;
				}
				num3++;
			}
		}
		this.m_seasonBucketDropdownBtn.m_button.SetDisabled(num3 < 2);
	}

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
		public SeasonBucket(SeasonTemplate season, PersistedStatBucket statBucket)
		{
			this.Season = season.Index;
			this.StatBucket = statBucket;
			if (this.Season == ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason)
			{
				this.DisplayString = StringUtil.TR("CurrentSeason", "Global");
			}
			else
			{
				this.DisplayString = season.GetDisplayName();
			}
			this.DisplayString = this.DisplayString + ": " + StringUtil.TR_PersistedStatBucketName(statBucket);
		}

		public int Season { get; private set; }

		public PersistedStatBucket StatBucket { get; private set; }

		public string DisplayString { get; private set; }
	}
}
