using System;
using TMPro;
using UnityEngine;

public class UIGameOverStatWidget : MonoBehaviour, GameOverStatTooltip.IGameOverStatTooltipDataProvider
{
	public enum StatDisplayType
	{
		None,
		GeneralStat,
		FreelancerStat
	}

	public Animator m_Animator;

	public TextMeshProUGUI m_StatName;

	public TextMeshProUGUI m_StatNumber;

	public TextMeshProUGUI m_StatUnit;

	public TextMeshProUGUI m_AverageText;

	public TextMeshProUGUI m_PreviousBestNumber;

	public TextMeshProUGUI m_NewBestText;

	public UITooltipHoverObject m_TooltipObject;

	private int? m_againstAllPercentile;

	private int? m_againstFreelancerPercentile;

	private int? m_againstRolePercentile;

	private int? m_againstPeersPercentile;

	public float? m_medianOfAll;

	public float? m_medianOfFreelancer;

	public float? m_medianOfRole;

	public float? m_medianOfPeers;

	public CharacterType? m_characterType;

	public CharacterRole? m_characterRole;

	private float CurrentStat;

	private double StatAverage;

	private float PreviousRecord;

	private bool HasPersonalAverage = true;

	private string FreelancerStatName;

	private string FreelancerStatDescription;

	public CharacterType? CharacterType => m_characterType;

	public CharacterRole? CharacterRole => m_characterRole;

	public bool HighlightDone
	{
		get;
		private set;
	}

	public StatDisplayType DisplayStatType
	{
		get;
		private set;
	}

	public StatDisplaySettings.StatType GeneralStatType
	{
		get;
		private set;
	}

	public int FreelancerStat
	{
		get;
		private set;
	}

	public IPersistedGameplayStat PreviousStats
	{
		get;
		private set;
	}

	private bool StatLowerIsBetter
	{
		get
		{
			if (DisplayStatType == StatDisplayType.GeneralStat)
			{
				GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(GeneralStatType);
				if (statDescription != null)
				{
					return statDescription.LowerIsBetter;
				}
			}
			return false;
		}
	}

	public string GetStatName()
	{
		if (DisplayStatType == StatDisplayType.FreelancerStat)
		{
			if (!FreelancerStatName.IsNullOrEmpty())
			{
				return FreelancerStatName;
			}
		}
		if (DisplayStatType == StatDisplayType.GeneralStat)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return StatDisplaySettings.GetLocalizedName(GeneralStatType);
				}
			}
		}
		return "Needs to be authored";
	}

	public string GetStatDescription()
	{
		if (DisplayStatType == StatDisplayType.FreelancerStat && !FreelancerStatDescription.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return FreelancerStatDescription;
				}
			}
		}
		if (DisplayStatType == StatDisplayType.GeneralStat)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return StatDisplaySettings.GetLocalizedDescription(GeneralStatType);
				}
			}
		}
		return "Needs to be authored";
	}

	public float? GetPersonalAverage()
	{
		if (HasPersonalAverage)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return (float)StatAverage;
				}
			}
		}
		return null;
	}

	public bool? IsStatLowerBetter()
	{
		int value;
		if (DisplayStatType == StatDisplayType.GeneralStat)
		{
			value = (GameResultBadgeData.Get().IsStatLowerBetter(GeneralStatType) ? 1 : 0);
		}
		else
		{
			value = 0;
		}
		return (byte)value != 0;
	}

	public StatDisplayType GetStatDisplayType()
	{
		return DisplayStatType;
	}

	public float? GetPreviousSeasonAverage()
	{
		return null;
	}

	public float? GetFriendsAverage()
	{
		return null;
	}

	public float? GetFreelancerMedian()
	{
		return m_medianOfFreelancer;
	}

	public float? GetPeerMedian()
	{
		return m_medianOfPeers;
	}

	public float? GetRoleMedian()
	{
		return m_medianOfRole;
	}

	public float? GetWorldMedian()
	{
		return m_medianOfAll;
	}

	public int? GetAgainstAllPercentile()
	{
		return m_againstAllPercentile;
	}

	public int? GetAgainstFreelancerPercentile()
	{
		return m_againstFreelancerPercentile;
	}

	public int? GetAgainstPeersPercentile()
	{
		return m_againstPeersPercentile;
	}

	public int? GetAgainstRolePercentile()
	{
		return m_againstRolePercentile;
	}

	public int? GetNumTurns()
	{
		if (AppState.IsInGame())
		{
			ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
			if (playersOriginalActorData != null)
			{
				return playersOriginalActorData.GetActorBehavior().totalPlayerTurns;
			}
		}
		return null;
	}

	public int? GetNumLives()
	{
		if (AppState.IsInGame())
		{
			ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
			if (playersOriginalActorData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return playersOriginalActorData.GetActorBehavior().totalDeaths + 1;
					}
				}
			}
		}
		return null;
	}

	public float? GetCurrentGameValue()
	{
		if (AppState.IsInGame())
		{
			if (DisplayStatType == StatDisplayType.GeneralStat)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return CurrentStat;
					}
				}
			}
		}
		return null;
	}

	public GameResultBadgeData.StatDescription.StatUnitType? GetStatUnitType()
	{
		if (AppState.IsInGame())
		{
			if (DisplayStatType == StatDisplayType.GeneralStat)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(GeneralStatType);
						return statDescription.StatUnit;
					}
					}
				}
			}
		}
		return null;
	}

	public void UpdatePercentiles(PercentileInfo info)
	{
		if (info != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					m_againstAllPercentile = info.AgainstAll;
					m_againstFreelancerPercentile = info.AgainstSameFreelancer;
					m_againstPeersPercentile = info.AgainstPeers;
					m_againstRolePercentile = info.AgainstRole;
					m_medianOfAll = info.MedianOfAll;
					m_medianOfFreelancer = info.MedianOfSameFreelancer;
					m_medianOfPeers = info.MedianOfPeers;
					m_medianOfRole = info.MedianOfRole;
					return;
				}
			}
		}
		m_againstAllPercentile = null;
		m_againstFreelancerPercentile = null;
		m_againstPeersPercentile = null;
		m_againstRolePercentile = null;
		m_medianOfAll = null;
		m_medianOfFreelancer = null;
		m_medianOfPeers = null;
		m_medianOfRole = null;
	}

	private void Awake()
	{
		m_TooltipObject.Setup(TooltipType.GameStatTooltip, delegate(UITooltipBase tooltip)
		{
			GameOverStatTooltip gameOverStatTooltip = tooltip as GameOverStatTooltip;
			if (gameOverStatTooltip != null)
			{
				gameOverStatTooltip.ClearDataProviders();
				gameOverStatTooltip.AddNewDataProvider(this);
				gameOverStatTooltip.Refresh();
			}
			return true;
		});
	}

	public void SetBadgeHighlight(bool doHighlight, bool isOn)
	{
		if (doHighlight)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (isOn)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								base.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
								base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(true);
								return;
							}
						}
					}
					base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(false);
					base.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
					return;
				}
			}
		}
		base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(false);
		base.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
	}

	private void OnEnable()
	{
		if (!HighlightDone)
		{
			return;
		}
		if (StatLowerIsBetter)
		{
			while (true)
			{
				int num;
				bool flag;
				switch (1)
				{
				case 0:
					break;
				default:
					{
						if (PreviousStats != null)
						{
							if (PreviousStats.GetNumGames() == 0)
							{
								num = ((CurrentStat == PreviousRecord) ? 1 : 0);
								goto IL_006b;
							}
						}
						num = 0;
						goto IL_006b;
					}
					IL_006b:
					flag = ((byte)num != 0);
					if (CurrentStat < PreviousRecord || flag)
					{
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBestIDLE", null, string.Empty, 1);
					}
					else
					{
						if ((double)CurrentStat < StatAverage)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemAvgIDLE", null, string.Empty, 1);
									return;
								}
							}
						}
						if (0f < CurrentStat)
						{
							if ((double)CurrentStat >= StatAverage)
							{
								UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBelowAvgIDLE", null, string.Empty, 1);
								return;
							}
						}
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIDLE", null, string.Empty, 1);
					}
					return;
				}
			}
		}
		if (CurrentStat > PreviousRecord)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBestIDLE", null, string.Empty, 1);
					return;
				}
			}
		}
		if ((double)CurrentStat > StatAverage)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemAvgIDLE", null, string.Empty, 1);
					return;
				}
			}
		}
		if (0f < CurrentStat)
		{
			if ((double)CurrentStat <= StatAverage)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBelowAvgIDLE", null, string.Empty, 1);
						return;
					}
				}
			}
		}
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIDLE", null, string.Empty, 1);
	}

	public void SetHighlight()
	{
		if (HighlightDone)
		{
			return;
		}
		while (true)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			HighlightDone = true;
			if (StatLowerIsBetter)
			{
				while (true)
				{
					int num;
					bool flag;
					switch (7)
					{
					case 0:
						break;
					default:
						{
							if (PreviousStats != null)
							{
								if (PreviousStats.GetNumGames() == 0)
								{
									num = ((CurrentStat == PreviousRecord) ? 1 : 0);
									goto IL_008c;
								}
							}
							num = 0;
							goto IL_008c;
						}
						IL_008c:
						flag = ((byte)num != 0);
						if (!(CurrentStat < PreviousRecord))
						{
							if (!flag)
							{
								if ((double)CurrentStat < StatAverage)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemAvgIN", null, string.Empty, 1);
											return;
										}
									}
								}
								if (0f < CurrentStat)
								{
									if ((double)CurrentStat >= StatAverage)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBelowAvgIN", null, string.Empty, 1);
												return;
											}
										}
									}
								}
								UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1);
								return;
							}
						}
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBestIN", null, string.Empty, 1);
						return;
					}
				}
			}
			if (CurrentStat > PreviousRecord)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBestIN", null, string.Empty, 1);
						return;
					}
				}
			}
			if ((double)CurrentStat > StatAverage)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemAvgIN", null, string.Empty, 1);
						return;
					}
				}
			}
			if (0f < CurrentStat)
			{
				if ((double)CurrentStat <= StatAverage)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemBelowAvgIN", null, string.Empty, 1);
							return;
						}
					}
				}
			}
			UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1);
			return;
		}
	}

	public bool BeatAverage()
	{
		return (double)CurrentStat > StatAverage;
	}

	public bool BeatRecord()
	{
		return CurrentStat > PreviousRecord;
	}

	public void SetupForAStat(PersistedStats StartValueStats, ActorBehavior actorBehavior, StatDisplaySettings.StatType TypeOfStat)
	{
		if (DisplayStatType != 0)
		{
			return;
		}
		DisplayStatType = StatDisplayType.GeneralStat;
		GeneralStatType = TypeOfStat;
		PreviousStats = StartValueStats.GetGameplayStat(TypeOfStat);
		GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(TypeOfStat);
		if (PreviousStats != null)
		{
			StatAverage = PreviousStats.Average();
			if (statDescription.LowerIsBetter)
			{
				PreviousRecord = PreviousStats.GetMin();
			}
			else
			{
				PreviousRecord = PreviousStats.GetMax();
			}
		}
		else
		{
			StatAverage = 0.0;
			PreviousRecord = 0f;
		}
		m_characterType = actorBehavior.CharacterType;
		m_characterRole = actorBehavior.CharacterRole;
		float? stat = actorBehavior.GetStat(TypeOfStat);
		float currentStat;
		if (stat.HasValue)
		{
			currentStat = stat.Value;
		}
		else
		{
			currentStat = 0f;
		}
		CurrentStat = currentStat;
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
		{
			CurrentStat = UIGameOverScreen.Get().Results.TotalBadgePoints;
		}
		FreelancerStatName = null;
		FreelancerStatDescription = null;
		GameResultBadgeData.StatDescription statDescription2 = GameResultBadgeData.Get().GetStatDescription(TypeOfStat);
		m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription2);
		m_StatName.text = StatDisplaySettings.GetLocalizedName(TypeOfStat);
		if (statDescription2.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
		{
			m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat * 100f), "0.#");
			m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(StatAverage * 100.0), "0.#"));
			m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord * 100f), "0.#"));
		}
		else
		{
			m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
			m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(StatAverage, 1), "0.#"));
			m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord, 1), "0.#"));
		}
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1);
	}

	public void SetupForFreelancerStats(PersistedStats StartValueStats, ActorBehavior actorBehavior, FreelancerStats CurrentGameStats, int FreelancerStatIndex, AbilityData FreelancerAbilityData)
	{
		if (DisplayStatType != 0)
		{
			return;
		}
		while (true)
		{
			DisplayStatType = StatDisplayType.FreelancerStat;
			FreelancerStat = FreelancerStatIndex;
			FreelancerStatName = null;
			FreelancerStatDescription = null;
			CurrentStat = CurrentGameStats.GetValueOfStat(FreelancerStatIndex);
			PersistedStatEntry freelancerStat = StartValueStats.GetFreelancerStat(FreelancerStatIndex);
			if (freelancerStat != null)
			{
				StatAverage = Math.Round(freelancerStat.Average(), 1);
				PreviousRecord = freelancerStat.GetMax();
			}
			else
			{
				StatAverage = 0.0;
				PreviousRecord = 0f;
			}
			m_characterType = actorBehavior.CharacterType;
			m_characterRole = actorBehavior.CharacterRole;
			string displayNameOfStat = CurrentGameStats.GetDisplayNameOfStat(FreelancerStatIndex);
			string localizedDescriptionOfStat = CurrentGameStats.GetLocalizedDescriptionOfStat(FreelancerStatIndex);
			if (!displayNameOfStat.IsNullOrEmpty())
			{
				m_StatName.text = SubstituteTokens(displayNameOfStat, FreelancerAbilityData);
				FreelancerStatName = m_StatName.text;
			}
			else
			{
				m_StatName.text = $"stat name for freelancer needs to be setup";
			}
			if (!localizedDescriptionOfStat.IsNullOrEmpty())
			{
				FreelancerStatDescription = SubstituteTokens(localizedDescriptionOfStat, FreelancerAbilityData);
			}
			m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
			m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(StatAverage, 1), "0.#"));
			m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord, 1), "0.#"));
			m_StatUnit.text = string.Empty;
			UIAnimationEventManager.Get().PlayAnimation(m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1);
			return;
		}
	}

	public void SetupTotalledStat(PersistedStats stats, StatDisplaySettings.StatType typeOfStat, CharacterType charType)
	{
		FreelancerStatName = null;
		FreelancerStatDescription = null;
		DisplayStatType = StatDisplayType.GeneralStat;
		GeneralStatType = typeOfStat;
		IPersistedGameplayStat persistedGameplayStat = null;
		SetupCharAndRole(charType);
		if (stats != null)
		{
			persistedGameplayStat = stats.GetGameplayStat(typeOfStat);
		}
		float num;
		if (persistedGameplayStat == null)
		{
			CurrentStat = 0f;
			StatAverage = 0.0;
			PreviousRecord = 0f;
			num = 0f;
		}
		else
		{
			CurrentStat = persistedGameplayStat.GetSum();
			StatAverage = persistedGameplayStat.Average();
			PreviousRecord = persistedGameplayStat.GetMax();
			num = persistedGameplayStat.GetMin();
		}
		m_StatName.text = StatDisplaySettings.GetLocalizedName(typeOfStat);
		GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(typeOfStat);
		m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription);
		UIManager.SetGameObjectActive(m_NewBestText, false);
		string animToPlay = (CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, animToPlay, null, string.Empty, 1);
		if (statDescription.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(StatAverage * 100.0), "0.#");
					m_AverageText.text = string.Format(StringUtil.TR("MinStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(num * 100f), "0.#"));
					m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord * 100f), "0.#"));
					return;
				}
			}
		}
		if (statDescription.StatUnit != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(StatAverage, 1), "0.#");
					m_AverageText.text = string.Format(StringUtil.TR("MinStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(num, 1), "0.#"));
					m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord, 1), "0.#"));
					return;
				}
			}
		}
		m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
		m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(StatAverage, 1), "0.#"));
		m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord, 1), "0.#"));
	}

	public void SetupFreelancerTotalledStats(PersistedStats StartValueStats, string name, string description, int FreelancerStatIndex, AbilityData FreelancerAbilityData, CharacterType charType)
	{
		FreelancerStatName = name;
		FreelancerStatDescription = description;
		FreelancerStatName = SubstituteTokens(FreelancerStatName, FreelancerAbilityData);
		FreelancerStatDescription = SubstituteTokens(FreelancerStatDescription, FreelancerAbilityData);
		SetupCharAndRole(charType);
		DisplayStatType = StatDisplayType.FreelancerStat;
		FreelancerStat = FreelancerStatIndex;
		PersistedStatEntry persistedStatEntry = null;
		if (StartValueStats != null)
		{
			persistedStatEntry = StartValueStats.GetFreelancerStat(FreelancerStatIndex);
		}
		if (persistedStatEntry != null)
		{
			StatAverage = Math.Round(persistedStatEntry.Average(), 1);
			PreviousRecord = persistedStatEntry.GetMax();
			CurrentStat = persistedStatEntry.GetSum();
		}
		else
		{
			StatAverage = 0.0;
			PreviousRecord = 0f;
			CurrentStat = 0f;
		}
		m_StatName.text = FreelancerStatName;
		if (m_StatName.text.IsNullOrEmpty())
		{
			m_StatName.text = $"stat name for freelancer needs to be setup";
		}
		m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
		m_StatUnit.text = string.Empty;
		m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(StatAverage, 1), "0.#"));
		m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(PreviousRecord, 1), "0.#"));
		UIManager.SetGameObjectActive(m_NewBestText, false);
		string animToPlay = (CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, animToPlay, null, string.Empty, 1);
	}

	public void SetupReplayStat(MatchFreelancerStats stats, StatDisplaySettings.StatType typeOfStat, CharacterType charType)
	{
		if (DisplayStatType != 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		FreelancerStatName = null;
		FreelancerStatDescription = null;
		DisplayStatType = StatDisplayType.GeneralStat;
		GeneralStatType = typeOfStat;
		SetupCharAndRole(charType);
		float? num = null;
		if (stats != null)
		{
			num = stats.GetStat(typeOfStat);
		}
		if (num.HasValue)
		{
			CurrentStat = num.Value;
		}
		else
		{
			CurrentStat = 0f;
		}
		StatAverage = CurrentStat;
		PreviousRecord = CurrentStat;
		HasPersonalAverage = false;
		m_StatName.text = StatDisplaySettings.GetLocalizedName(typeOfStat);
		GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(typeOfStat);
		m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription);
		if (statDescription.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
		{
			m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat * 100f), "0.#");
		}
		else
		{
			m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
		}
		UIManager.SetGameObjectActive(m_NewBestText, false);
		string animToPlay = (CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, animToPlay, null, string.Empty, 1);
		m_AverageText.text = string.Empty;
		m_PreviousBestNumber.text = string.Empty;
	}

	public void SetupReplayFreelancerStat(CharacterType charType, MatchFreelancerStats stats, int statIndex, AbilityData FreelancerAbilityData)
	{
		if (DisplayStatType != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		FreelancerStatName = StringUtil.TR_FreelancerStatName(charType.ToString(), statIndex);
		FreelancerStatDescription = StringUtil.TR_FreelancerStatDescription(charType.ToString(), statIndex);
		FreelancerStatName = SubstituteTokens(FreelancerStatName, FreelancerAbilityData);
		FreelancerStatDescription = SubstituteTokens(FreelancerStatDescription, FreelancerAbilityData);
		DisplayStatType = StatDisplayType.FreelancerStat;
		FreelancerStat = statIndex;
		SetupCharAndRole(charType);
		float? num = null;
		if (stats != null)
		{
			num = stats.GetFreelancerStat(statIndex);
		}
		if (num.HasValue)
		{
			CurrentStat = num.Value;
		}
		else
		{
			CurrentStat = 0f;
		}
		StatAverage = CurrentStat;
		PreviousRecord = CurrentStat;
		HasPersonalAverage = false;
		m_StatName.text = FreelancerStatName;
		if (m_StatName.text.IsNullOrEmpty())
		{
			m_StatName.text = $"stat name for freelancer needs to be setup";
		}
		m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(CurrentStat, 1), "0.#");
		m_StatUnit.text = string.Empty;
		m_AverageText.text = string.Empty;
		m_PreviousBestNumber.text = string.Empty;
		UIManager.SetGameObjectActive(m_NewBestText, false);
		object obj;
		if (CurrentStat == 0f)
		{
			obj = "StatSmallItemZeroIN";
		}
		else
		{
			obj = "StatSmallItemBelowAvgIN";
		}
		string animToPlay = (string)obj;
		UIAnimationEventManager.Get().PlayAnimation(m_Animator, animToPlay, null, string.Empty, 1);
	}

	private void SetupCharAndRole(CharacterType charType)
	{
		m_characterType = charType;
		if (charType.IsValidForHumanGameplay())
		{
			m_characterRole = GameWideData.Get().GetCharacterResourceLink(charType).m_characterRole;
		}
		else
		{
			m_characterRole = global::CharacterRole.None;
		}
	}

	public static string SubstituteTokens(string WidgetDisplayName, AbilityData FreelancerAbilityData)
	{
		string tooltipNow = WidgetDisplayName;
		for (int i = 0; i < 5; i++)
		{
			Ability abilityAtIndex = FreelancerAbilityData.GetAbilityAtIndex(i);
			if (abilityAtIndex != null)
			{
				string substitute = "<color=#FFC000>" + abilityAtIndex.GetNameString() + "</color>";
				tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, "[ABILITY_" + i + "]", substitute);
			}
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(tooltipNow, null);
	}
}
