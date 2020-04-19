using System;
using TMPro;
using UnityEngine;

public class UIGameOverStatWidget : MonoBehaviour, GameOverStatTooltip.IGameOverStatTooltipDataProvider
{
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

	public string GetStatName()
	{
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetStatName()).MethodHandle;
			}
			if (!this.FreelancerStatName.IsNullOrEmpty())
			{
				return this.FreelancerStatName;
			}
		}
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return StatDisplaySettings.GetLocalizedName(this.GeneralStatType);
		}
		return "Needs to be authored";
	}

	public string GetStatDescription()
	{
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat && !this.FreelancerStatDescription.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetStatDescription()).MethodHandle;
			}
			return this.FreelancerStatDescription;
		}
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return StatDisplaySettings.GetLocalizedDescription(this.GeneralStatType);
		}
		return "Needs to be authored";
	}

	public float? GetPersonalAverage()
	{
		if (this.HasPersonalAverage)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetPersonalAverage()).MethodHandle;
			}
			return new float?((float)this.StatAverage);
		}
		return null;
	}

	public bool? IsStatLowerBetter()
	{
		bool value;
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.IsStatLowerBetter()).MethodHandle;
			}
			value = GameResultBadgeData.Get().IsStatLowerBetter(this.GeneralStatType);
		}
		else
		{
			value = false;
		}
		return new bool?(value);
	}

	public UIGameOverStatWidget.StatDisplayType GetStatDisplayType()
	{
		return this.DisplayStatType;
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
		return this.m_medianOfFreelancer;
	}

	public float? GetPeerMedian()
	{
		return this.m_medianOfPeers;
	}

	public float? GetRoleMedian()
	{
		return this.m_medianOfRole;
	}

	public float? GetWorldMedian()
	{
		return this.m_medianOfAll;
	}

	public int? GetAgainstAllPercentile()
	{
		return this.m_againstAllPercentile;
	}

	public int? GetAgainstFreelancerPercentile()
	{
		return this.m_againstFreelancerPercentile;
	}

	public int? GetAgainstPeersPercentile()
	{
		return this.m_againstPeersPercentile;
	}

	public int? GetAgainstRolePercentile()
	{
		return this.m_againstRolePercentile;
	}

	public int? GetNumTurns()
	{
		if (AppState.IsInGame())
		{
			ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
			if (playersOriginalActorData != null)
			{
				return new int?(playersOriginalActorData.\u000E().totalPlayerTurns);
			}
		}
		return null;
	}

	public int? GetNumLives()
	{
		if (AppState.IsInGame())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetNumLives()).MethodHandle;
			}
			ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
			if (playersOriginalActorData != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return new int?(playersOriginalActorData.\u000E().totalDeaths + 1);
			}
		}
		return null;
	}

	public float? GetCurrentGameValue()
	{
		if (AppState.IsInGame())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetCurrentGameValue()).MethodHandle;
			}
			if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return new float?(this.CurrentStat);
			}
		}
		return null;
	}

	public GameResultBadgeData.StatDescription.StatUnitType? GetStatUnitType()
	{
		if (AppState.IsInGame())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.GetStatUnitType()).MethodHandle;
			}
			if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(this.GeneralStatType);
				return new GameResultBadgeData.StatDescription.StatUnitType?(statDescription.StatUnit);
			}
		}
		return null;
	}

	public CharacterType? CharacterType
	{
		get
		{
			return this.m_characterType;
		}
	}

	public CharacterRole? CharacterRole
	{
		get
		{
			return this.m_characterRole;
		}
	}

	public void UpdatePercentiles(PercentileInfo info)
	{
		if (info != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.UpdatePercentiles(PercentileInfo)).MethodHandle;
			}
			this.m_againstAllPercentile = info.AgainstAll;
			this.m_againstFreelancerPercentile = info.AgainstSameFreelancer;
			this.m_againstPeersPercentile = info.AgainstPeers;
			this.m_againstRolePercentile = info.AgainstRole;
			this.m_medianOfAll = info.MedianOfAll;
			this.m_medianOfFreelancer = info.MedianOfSameFreelancer;
			this.m_medianOfPeers = info.MedianOfPeers;
			this.m_medianOfRole = info.MedianOfRole;
		}
		else
		{
			this.m_againstAllPercentile = null;
			this.m_againstFreelancerPercentile = null;
			this.m_againstPeersPercentile = null;
			this.m_againstRolePercentile = null;
			this.m_medianOfAll = null;
			this.m_medianOfFreelancer = null;
			this.m_medianOfPeers = null;
			this.m_medianOfRole = null;
		}
	}

	public bool HighlightDone { get; private set; }

	public UIGameOverStatWidget.StatDisplayType DisplayStatType { get; private set; }

	public StatDisplaySettings.StatType GeneralStatType { get; private set; }

	public int FreelancerStat { get; private set; }

	public IPersistedGameplayStat PreviousStats { get; private set; }

	private void Awake()
	{
		this.m_TooltipObject.Setup(TooltipType.GameStatTooltip, delegate(UITooltipBase tooltip)
		{
			GameOverStatTooltip gameOverStatTooltip = tooltip as GameOverStatTooltip;
			if (gameOverStatTooltip != null)
			{
				gameOverStatTooltip.ClearDataProviders();
				gameOverStatTooltip.AddNewDataProvider(this);
				gameOverStatTooltip.Refresh();
			}
			return true;
		}, null);
	}

	public void SetBadgeHighlight(bool doHighlight, bool isOn)
	{
		if (doHighlight)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetBadgeHighlight(bool, bool)).MethodHandle;
			}
			if (isOn)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				base.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(true);
			}
			else
			{
				base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(false);
				base.gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
			}
		}
		else
		{
			base.gameObject.GetComponent<_SelectableBtn>().spriteController.SetAlwaysHoverState(false);
			base.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	private bool StatLowerIsBetter
	{
		get
		{
			if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.get_StatLowerIsBetter()).MethodHandle;
				}
				GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(this.GeneralStatType);
				if (statDescription != null)
				{
					return statDescription.LowerIsBetter;
				}
			}
			return false;
		}
	}

	private void OnEnable()
	{
		if (this.HighlightDone)
		{
			if (this.StatLowerIsBetter)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.OnEnable()).MethodHandle;
				}
				bool flag;
				if (this.PreviousStats != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.PreviousStats.GetNumGames() == 0)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = (this.CurrentStat == this.PreviousRecord);
						goto IL_6B;
					}
				}
				flag = false;
				IL_6B:
				bool flag2 = flag;
				if (this.CurrentStat < this.PreviousRecord || flag2)
				{
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBestIDLE", null, string.Empty, 1, 0f, true, false, null, null);
				}
				else if ((double)this.CurrentStat < this.StatAverage)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemAvgIDLE", null, string.Empty, 1, 0f, true, false, null, null);
				}
				else
				{
					if (0f < this.CurrentStat)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if ((double)this.CurrentStat >= this.StatAverage)
						{
							UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBelowAvgIDLE", null, string.Empty, 1, 0f, true, false, null, null);
							goto IL_161;
						}
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIDLE", null, string.Empty, 1, 0f, true, false, null, null);
				}
				IL_161:;
			}
			else if (this.CurrentStat > this.PreviousRecord)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBestIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else if ((double)this.CurrentStat > this.StatAverage)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemAvgIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else
			{
				if (0f < this.CurrentStat)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if ((double)this.CurrentStat <= this.StatAverage)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBelowAvgIDLE", null, string.Empty, 1, 0f, true, false, null, null);
						return;
					}
				}
				UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIDLE", null, string.Empty, 1, 0f, true, false, null, null);
			}
		}
	}

	public void SetHighlight()
	{
		if (!this.HighlightDone)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetHighlight()).MethodHandle;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.HighlightDone = true;
				if (this.StatLowerIsBetter)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag;
					if (this.PreviousStats != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.PreviousStats.GetNumGames() == 0)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							flag = (this.CurrentStat == this.PreviousRecord);
							goto IL_8C;
						}
					}
					flag = false;
					IL_8C:
					bool flag2 = flag;
					if (this.CurrentStat >= this.PreviousRecord)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag2)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else
						{
							if ((double)this.CurrentStat < this.StatAverage)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemAvgIN", null, string.Empty, 1, 0f, true, false, null, null);
								goto IL_1A1;
							}
							if (0f < this.CurrentStat)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if ((double)this.CurrentStat >= this.StatAverage)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBelowAvgIN", null, string.Empty, 1, 0f, true, false, null, null);
									goto IL_1A1;
								}
							}
							UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1, 0f, true, false, null, null);
							goto IL_1A1;
						}
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBestIN", null, string.Empty, 1, 0f, true, false, null, null);
					IL_1A1:;
				}
				else if (this.CurrentStat > this.PreviousRecord)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBestIN", null, string.Empty, 1, 0f, true, false, null, null);
				}
				else if ((double)this.CurrentStat > this.StatAverage)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemAvgIN", null, string.Empty, 1, 0f, true, false, null, null);
				}
				else
				{
					if (0f < this.CurrentStat)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if ((double)this.CurrentStat <= this.StatAverage)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemBelowAvgIN", null, string.Empty, 1, 0f, true, false, null, null);
							return;
						}
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1, 0f, true, false, null, null);
				}
			}
		}
	}

	public bool BeatAverage()
	{
		return (double)this.CurrentStat > this.StatAverage;
	}

	public bool BeatRecord()
	{
		return this.CurrentStat > this.PreviousRecord;
	}

	public void SetupForAStat(PersistedStats StartValueStats, ActorBehavior actorBehavior, StatDisplaySettings.StatType TypeOfStat)
	{
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.None)
		{
			this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.GeneralStat;
			this.GeneralStatType = TypeOfStat;
			this.PreviousStats = StartValueStats.GetGameplayStat(TypeOfStat);
			GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(TypeOfStat);
			if (this.PreviousStats != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupForAStat(PersistedStats, ActorBehavior, StatDisplaySettings.StatType)).MethodHandle;
				}
				this.StatAverage = (double)this.PreviousStats.Average();
				if (statDescription.LowerIsBetter)
				{
					this.PreviousRecord = this.PreviousStats.GetMin();
				}
				else
				{
					this.PreviousRecord = this.PreviousStats.GetMax();
				}
			}
			else
			{
				this.StatAverage = 0.0;
				this.PreviousRecord = 0f;
			}
			this.m_characterType = actorBehavior.CharacterType;
			this.m_characterRole = actorBehavior.CharacterRole;
			float? stat = actorBehavior.GetStat(TypeOfStat);
			float currentStat;
			if (stat != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				currentStat = stat.Value;
			}
			else
			{
				currentStat = 0f;
			}
			this.CurrentStat = currentStat;
			if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.CurrentStat = UIGameOverScreen.Get().Results.TotalBadgePoints;
			}
			this.FreelancerStatName = null;
			this.FreelancerStatDescription = null;
			GameResultBadgeData.StatDescription statDescription2 = GameResultBadgeData.Get().GetStatDescription(TypeOfStat);
			this.m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription2);
			this.m_StatName.text = StatDisplaySettings.GetLocalizedName(TypeOfStat);
			if (statDescription2.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)(this.CurrentStat * 100f)), "0.#");
				this.m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage * 100.0), "0.#"));
				this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)(this.PreviousRecord * 100f)), "0.#"));
			}
			else
			{
				this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
				this.m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage, 1), "0.#"));
				this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)this.PreviousRecord, 1), "0.#"));
			}
			UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1, 0f, true, false, null, null);
		}
	}

	public void SetupForFreelancerStats(PersistedStats StartValueStats, ActorBehavior actorBehavior, FreelancerStats CurrentGameStats, int FreelancerStatIndex, AbilityData FreelancerAbilityData)
	{
		if (this.DisplayStatType == UIGameOverStatWidget.StatDisplayType.None)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupForFreelancerStats(PersistedStats, ActorBehavior, FreelancerStats, int, AbilityData)).MethodHandle;
			}
			this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.FreelancerStat;
			this.FreelancerStat = FreelancerStatIndex;
			this.FreelancerStatName = null;
			this.FreelancerStatDescription = null;
			this.CurrentStat = (float)CurrentGameStats.GetValueOfStat(FreelancerStatIndex);
			PersistedStatEntry freelancerStat = StartValueStats.GetFreelancerStat(FreelancerStatIndex);
			if (freelancerStat != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				this.StatAverage = Math.Round((double)freelancerStat.Average(), 1);
				this.PreviousRecord = freelancerStat.GetMax();
			}
			else
			{
				this.StatAverage = 0.0;
				this.PreviousRecord = 0f;
			}
			this.m_characterType = actorBehavior.CharacterType;
			this.m_characterRole = actorBehavior.CharacterRole;
			string displayNameOfStat = CurrentGameStats.GetDisplayNameOfStat(FreelancerStatIndex);
			string localizedDescriptionOfStat = CurrentGameStats.GetLocalizedDescriptionOfStat(FreelancerStatIndex);
			if (!displayNameOfStat.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_StatName.text = UIGameOverStatWidget.SubstituteTokens(displayNameOfStat, FreelancerAbilityData);
				this.FreelancerStatName = this.m_StatName.text;
			}
			else
			{
				this.m_StatName.text = string.Format("stat name for freelancer needs to be setup", new object[0]);
			}
			if (!localizedDescriptionOfStat.IsNullOrEmpty())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.FreelancerStatDescription = UIGameOverStatWidget.SubstituteTokens(localizedDescriptionOfStat, FreelancerAbilityData);
			}
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
			this.m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage, 1), "0.#"));
			this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)this.PreviousRecord, 1), "0.#"));
			this.m_StatUnit.text = string.Empty;
			UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, "StatSmallItemZeroIN", null, string.Empty, 1, 0f, true, false, null, null);
		}
	}

	public void SetupTotalledStat(PersistedStats stats, StatDisplaySettings.StatType typeOfStat, CharacterType charType)
	{
		this.FreelancerStatName = null;
		this.FreelancerStatDescription = null;
		this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.GeneralStat;
		this.GeneralStatType = typeOfStat;
		IPersistedGameplayStat persistedGameplayStat = null;
		this.SetupCharAndRole(charType);
		if (stats != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupTotalledStat(PersistedStats, StatDisplaySettings.StatType, global::CharacterType)).MethodHandle;
			}
			persistedGameplayStat = stats.GetGameplayStat(typeOfStat);
		}
		float num;
		if (persistedGameplayStat == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.CurrentStat = 0f;
			this.StatAverage = 0.0;
			this.PreviousRecord = 0f;
			num = 0f;
		}
		else
		{
			this.CurrentStat = persistedGameplayStat.GetSum();
			this.StatAverage = (double)persistedGameplayStat.Average();
			this.PreviousRecord = persistedGameplayStat.GetMax();
			num = persistedGameplayStat.GetMin();
		}
		this.m_StatName.text = StatDisplaySettings.GetLocalizedName(typeOfStat);
		GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(typeOfStat);
		this.m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription);
		UIManager.SetGameObjectActive(this.m_NewBestText, false, null);
		string animToPlay = (this.CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, animToPlay, null, string.Empty, 1, 0f, true, false, null, null);
		if (statDescription.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage * 100.0), "0.#");
			this.m_AverageText.text = string.Format(StringUtil.TR("MinStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)(num * 100f)), "0.#"));
			this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)(this.PreviousRecord * 100f)), "0.#"));
		}
		else if (statDescription.StatUnit != GameResultBadgeData.StatDescription.StatUnitType.None)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage, 1), "0.#");
			this.m_AverageText.text = string.Format(StringUtil.TR("MinStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)num, 1), "0.#"));
			this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)this.PreviousRecord, 1), "0.#"));
		}
		else
		{
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
			this.m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage, 1), "0.#"));
			this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)this.PreviousRecord, 1), "0.#"));
		}
	}

	public void SetupFreelancerTotalledStats(PersistedStats StartValueStats, string name, string description, int FreelancerStatIndex, AbilityData FreelancerAbilityData, CharacterType charType)
	{
		this.FreelancerStatName = name;
		this.FreelancerStatDescription = description;
		this.FreelancerStatName = UIGameOverStatWidget.SubstituteTokens(this.FreelancerStatName, FreelancerAbilityData);
		this.FreelancerStatDescription = UIGameOverStatWidget.SubstituteTokens(this.FreelancerStatDescription, FreelancerAbilityData);
		this.SetupCharAndRole(charType);
		this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.FreelancerStat;
		this.FreelancerStat = FreelancerStatIndex;
		PersistedStatEntry persistedStatEntry = null;
		if (StartValueStats != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupFreelancerTotalledStats(PersistedStats, string, string, int, AbilityData, global::CharacterType)).MethodHandle;
			}
			persistedStatEntry = StartValueStats.GetFreelancerStat(FreelancerStatIndex);
		}
		if (persistedStatEntry != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.StatAverage = Math.Round((double)persistedStatEntry.Average(), 1);
			this.PreviousRecord = persistedStatEntry.GetMax();
			this.CurrentStat = persistedStatEntry.GetSum();
		}
		else
		{
			this.StatAverage = 0.0;
			this.PreviousRecord = 0f;
			this.CurrentStat = 0f;
		}
		this.m_StatName.text = this.FreelancerStatName;
		if (this.m_StatName.text.IsNullOrEmpty())
		{
			this.m_StatName.text = string.Format("stat name for freelancer needs to be setup", new object[0]);
		}
		this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
		this.m_StatUnit.text = string.Empty;
		this.m_AverageText.text = string.Format(StringUtil.TR("AverageStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round(this.StatAverage, 1), "0.#"));
		this.m_PreviousBestNumber.text = string.Format(StringUtil.TR("MaxStat", "Global"), StringUtil.GetLocalizedDouble(Math.Round((double)this.PreviousRecord, 1), "0.#"));
		UIManager.SetGameObjectActive(this.m_NewBestText, false, null);
		string animToPlay = (this.CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, animToPlay, null, string.Empty, 1, 0f, true, false, null, null);
	}

	public void SetupReplayStat(MatchFreelancerStats stats, StatDisplaySettings.StatType typeOfStat, CharacterType charType)
	{
		if (this.DisplayStatType != UIGameOverStatWidget.StatDisplayType.None)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupReplayStat(MatchFreelancerStats, StatDisplaySettings.StatType, global::CharacterType)).MethodHandle;
			}
			return;
		}
		this.FreelancerStatName = null;
		this.FreelancerStatDescription = null;
		this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.GeneralStat;
		this.GeneralStatType = typeOfStat;
		this.SetupCharAndRole(charType);
		float? num = null;
		if (stats != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num = stats.GetStat(typeOfStat);
		}
		if (num != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.CurrentStat = num.Value;
		}
		else
		{
			this.CurrentStat = 0f;
		}
		this.StatAverage = (double)this.CurrentStat;
		this.PreviousRecord = this.CurrentStat;
		this.HasPersonalAverage = false;
		this.m_StatName.text = StatDisplaySettings.GetLocalizedName(typeOfStat);
		GameResultBadgeData.StatDescription statDescription = GameResultBadgeData.Get().GetStatDescription(typeOfStat);
		this.m_StatUnit.text = GameResultBadgeData.StatDescription.GetStatUnit(statDescription);
		if (statDescription.StatUnit == GameResultBadgeData.StatDescription.StatUnitType.Percentage)
		{
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)(this.CurrentStat * 100f)), "0.#");
		}
		else
		{
			this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
		}
		UIManager.SetGameObjectActive(this.m_NewBestText, false, null);
		string animToPlay = (this.CurrentStat != 0f) ? "StatSmallItemBelowAvgIN" : "StatSmallItemZeroIN";
		UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, animToPlay, null, string.Empty, 1, 0f, true, false, null, null);
		this.m_AverageText.text = string.Empty;
		this.m_PreviousBestNumber.text = string.Empty;
	}

	public void SetupReplayFreelancerStat(CharacterType charType, MatchFreelancerStats stats, int statIndex, AbilityData FreelancerAbilityData)
	{
		if (this.DisplayStatType != UIGameOverStatWidget.StatDisplayType.None)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SetupReplayFreelancerStat(global::CharacterType, MatchFreelancerStats, int, AbilityData)).MethodHandle;
			}
			return;
		}
		this.FreelancerStatName = StringUtil.TR_FreelancerStatName(charType.ToString(), statIndex);
		this.FreelancerStatDescription = StringUtil.TR_FreelancerStatDescription(charType.ToString(), statIndex);
		this.FreelancerStatName = UIGameOverStatWidget.SubstituteTokens(this.FreelancerStatName, FreelancerAbilityData);
		this.FreelancerStatDescription = UIGameOverStatWidget.SubstituteTokens(this.FreelancerStatDescription, FreelancerAbilityData);
		this.DisplayStatType = UIGameOverStatWidget.StatDisplayType.FreelancerStat;
		this.FreelancerStat = statIndex;
		this.SetupCharAndRole(charType);
		float? num = null;
		if (stats != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num = stats.GetFreelancerStat(statIndex);
		}
		if (num != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.CurrentStat = num.Value;
		}
		else
		{
			this.CurrentStat = 0f;
		}
		this.StatAverage = (double)this.CurrentStat;
		this.PreviousRecord = this.CurrentStat;
		this.HasPersonalAverage = false;
		this.m_StatName.text = this.FreelancerStatName;
		if (this.m_StatName.text.IsNullOrEmpty())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_StatName.text = string.Format("stat name for freelancer needs to be setup", new object[0]);
		}
		this.m_StatNumber.text = StringUtil.GetLocalizedDouble(Math.Round((double)this.CurrentStat, 1), "0.#");
		this.m_StatUnit.text = string.Empty;
		this.m_AverageText.text = string.Empty;
		this.m_PreviousBestNumber.text = string.Empty;
		UIManager.SetGameObjectActive(this.m_NewBestText, false, null);
		string text;
		if (this.CurrentStat == 0f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			text = "StatSmallItemZeroIN";
		}
		else
		{
			text = "StatSmallItemBelowAvgIN";
		}
		string animToPlay = text;
		UIAnimationEventManager.Get().PlayAnimation(this.m_Animator, animToPlay, null, string.Empty, 1, 0f, true, false, null, null);
	}

	private void SetupCharAndRole(CharacterType charType)
	{
		this.m_characterType = new CharacterType?(charType);
		if (charType.IsValidForHumanGameplay())
		{
			this.m_characterRole = new CharacterRole?(GameWideData.Get().GetCharacterResourceLink(charType).m_characterRole);
		}
		else
		{
			this.m_characterRole = new CharacterRole?(global::CharacterRole.None);
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverStatWidget.SubstituteTokens(string, AbilityData)).MethodHandle;
				}
				string substitute = "<color=#FFC000>" + abilityAtIndex.GetNameString() + "</color>";
				tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, "[ABILITY_" + i + "]", substitute);
			}
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(tooltipNow, null, false);
	}

	public enum StatDisplayType
	{
		None,
		GeneralStat,
		FreelancerStat
	}
}
