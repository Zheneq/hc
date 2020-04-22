using I2.Loc;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStatTooltip : UITooltipBase
{
	public interface IGameOverStatTooltipDataProvider
	{
		CharacterRole? CharacterRole
		{
			get;
		}

		CharacterType? CharacterType
		{
			get;
		}

		string GetStatName();

		string GetStatDescription();

		float? GetPersonalAverage();

		float? GetPreviousSeasonAverage();

		float? GetFriendsAverage();

		float? GetFreelancerMedian();

		float? GetPeerMedian();

		float? GetRoleMedian();

		float? GetWorldMedian();

		int? GetAgainstAllPercentile();

		int? GetAgainstFreelancerPercentile();

		int? GetAgainstPeersPercentile();

		int? GetAgainstRolePercentile();

		bool? IsStatLowerBetter();

		float? GetCurrentGameValue();

		GameResultBadgeData.StatDescription.StatUnitType? GetStatUnitType();

		UIGameOverStatWidget.StatDisplayType GetStatDisplayType();

		int? GetNumTurns();

		int? GetNumLives();
	}

	public TextMeshProUGUI m_StatName;

	public TextMeshProUGUI m_StatDescription;

	public GameOverStatTooltipInfoLine m_StatComparisonPrefab;

	public TextMeshProUGUI m_StatSum;

	public TextMeshProUGUI m_StatNumDividedBy;

	public TextMeshProUGUI m_PercentileLabel;

	public string m_AverageColorHex = "AAAAAA";

	public string m_AboveAverageColorHex = "AA0000";

	public string m_BelowAverageColorHex = "00AA00";

	private List<IGameOverStatTooltipDataProvider> m_DataProviders = new List<IGameOverStatTooltipDataProvider>();

	private List<GameOverStatTooltipInfoLine> m_DisplayedStatComparers = new List<GameOverStatTooltipInfoLine>();

	private string StatName;

	private string StatDescription;

	private CharacterRole m_characterRole;

	private CharacterType m_characterType;

	private float PersonalAverage;

	private float? PreviousSeasonAverage;

	private float? FriendsAverage;

	private float? SimilarMMRMedian;

	private float? WorldMedian;

	private float? FreelancerMedian;

	private float? RoleMedian;

	private int? AgainstAllPercentile;

	private int? AgainstRolePercentile;

	private int? AgainstFreelancerPercentile;

	private int? AgainstPeersPercentile;

	private bool LowerIsBetter;

	private UIGameOverStatWidget.StatDisplayType StatDisplayType;

	private void Awake()
	{
		m_DisplayedStatComparers.Add(m_StatComparisonPrefab);
	}

	public void ClearDataProviders()
	{
		m_DataProviders.Clear();
	}

	public void AddNewDataProvider(IGameOverStatTooltipDataProvider provider)
	{
		if (provider == null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_DataProviders.Contains(provider))
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
				m_DataProviders.Add(provider);
			}
			Refresh();
			return;
		}
	}

	private int DisplayPercentile(int? percentile, int displayIndex, string localizedString)
	{
		if (percentile.HasValue)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					while (displayIndex >= m_DisplayedStatComparers.Count)
					{
						GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = Object.Instantiate(m_StatComparisonPrefab);
						UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
						m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex], true);
							TextMeshProUGUI statDisplay = m_DisplayedStatComparers[displayIndex].m_StatDisplay;
							object arg;
							if (percentile.Value <= 66)
							{
								arg = ((percentile.Value >= 34) ? m_AverageColorHex : m_BelowAverageColorHex);
							}
							else
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
								arg = m_AboveAverageColorHex;
							}
							statDisplay.text = string.Format("{2}: <color=#{0}>{1}%</color>", arg, percentile.Value.ToString("g", CultureInfo.CurrentCulture), localizedString);
							UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex].m_Percentile, false);
							return displayIndex + 1;
						}
						}
					}
				}
			}
		}
		return displayIndex;
	}

	private string GetColorHex(float AverageValue, float CurrentValue, bool LowerIsBetter)
	{
		if (AverageValue == CurrentValue)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_AverageColorHex;
				}
			}
		}
		if (LowerIsBetter)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (CurrentValue > AverageValue)
					{
						return m_BelowAverageColorHex;
					}
					return m_AboveAverageColorHex;
				}
			}
		}
		if (CurrentValue > AverageValue)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_AboveAverageColorHex;
				}
			}
		}
		return m_BelowAverageColorHex;
	}

	private string GetNumberEnding(int percentile)
	{
		if (LocalizationManager.CurrentLanguageCode == "en")
		{
			int num = percentile % 10;
			if (num == 1)
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
						return "st";
					}
				}
			}
			if (num == 2)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return "nd";
					}
				}
			}
			if (num == 3)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return "rd";
					}
				}
			}
			return "th";
		}
		return string.Empty;
	}

	private int DisplayCompareAverage(float? Average, int? Percentile, int displayIndex, string localizedString, bool LowerIsBetter)
	{
		if (Average.HasValue)
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
					while (displayIndex >= m_DisplayedStatComparers.Count)
					{
						GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = Object.Instantiate(m_StatComparisonPrefab);
						UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
						m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex], true);
							m_DisplayedStatComparers[displayIndex].m_StatDisplay.text = string.Format("{1}: {0}", StringUtil.GetLocalizedFloat(Average.Value, "0.###"), localizedString);
							if (Percentile.HasValue)
							{
								UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex].m_Percentile, true);
								m_DisplayedStatComparers[displayIndex].m_Percentile.text = string.Format(StringUtil.TR("PercentileTooltip", "Global"), Percentile.Value, GetNumberEnding(Percentile.Value));
							}
							else
							{
								UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex].m_Percentile, false);
							}
							return displayIndex + 1;
						}
					}
				}
			}
		}
		return displayIndex;
	}

	private int DisplayNoStatsAvailable(string errorDisplay, int displayIndex)
	{
		if (!errorDisplay.IsNullOrEmpty())
		{
			while (displayIndex >= m_DisplayedStatComparers.Count)
			{
				GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = Object.Instantiate(m_StatComparisonPrefab);
				UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
				m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
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
				UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex], true);
				UIManager.SetGameObjectActive(m_DisplayedStatComparers[displayIndex].m_Percentile, false);
				m_DisplayedStatComparers[displayIndex].m_StatDisplay.text = errorDisplay;
				m_DisplayedStatComparers[displayIndex].GetComponent<LayoutElement>().preferredHeight = m_DisplayedStatComparers[displayIndex].m_StatDisplay.preferredHeight;
				return displayIndex + 1;
			}
		}
		return displayIndex;
	}

	public void Refresh()
	{
		bool flag = false;
		PersonalAverage = 0f;
		PreviousSeasonAverage = null;
		FriendsAverage = null;
		SimilarMMRMedian = null;
		WorldMedian = null;
		FreelancerMedian = null;
		RoleMedian = null;
		AgainstAllPercentile = null;
		AgainstFreelancerPercentile = null;
		AgainstPeersPercentile = null;
		AgainstRolePercentile = null;
		LowerIsBetter = false;
		m_characterRole = CharacterRole.None;
		m_characterType = CharacterType.None;
		float? num = null;
		GameResultBadgeData.StatDescription.StatUnitType? statUnitType = null;
		int? num2 = null;
		int? num3 = null;
		for (int i = 0; i < m_DataProviders.Count; i++)
		{
			if (m_DataProviders[i] == null)
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
				m_DataProviders.RemoveAt(i);
				i--;
				continue;
			}
			if (!m_DataProviders[i].GetStatName().IsNullOrEmpty())
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
				StatName = m_DataProviders[i].GetStatName();
			}
			if (!m_DataProviders[i].GetStatDescription().IsNullOrEmpty())
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
				StatDescription = m_DataProviders[i].GetStatDescription();
			}
			if (m_DataProviders[i].GetPersonalAverage().HasValue)
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
				PersonalAverage = m_DataProviders[i].GetPersonalAverage().Value;
				flag = true;
			}
			if (m_DataProviders[i].GetPreviousSeasonAverage().HasValue)
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
				PreviousSeasonAverage = m_DataProviders[i].GetPreviousSeasonAverage().Value;
			}
			if (m_DataProviders[i].GetFriendsAverage().HasValue)
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
				FriendsAverage = m_DataProviders[i].GetFriendsAverage().Value;
			}
			if (m_DataProviders[i].GetPeerMedian().HasValue)
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
				SimilarMMRMedian = m_DataProviders[i].GetPeerMedian().Value;
			}
			if (m_DataProviders[i].GetFreelancerMedian().HasValue)
			{
				FreelancerMedian = m_DataProviders[i].GetFreelancerMedian().Value;
			}
			if (m_DataProviders[i].GetRoleMedian().HasValue)
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
				RoleMedian = m_DataProviders[i].GetRoleMedian().Value;
			}
			if (m_DataProviders[i].GetWorldMedian().HasValue)
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
				WorldMedian = m_DataProviders[i].GetWorldMedian().Value;
			}
			if (m_DataProviders[i].GetAgainstAllPercentile().HasValue)
			{
				AgainstAllPercentile = m_DataProviders[i].GetAgainstAllPercentile().Value;
			}
			if (m_DataProviders[i].GetAgainstFreelancerPercentile().HasValue)
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
				AgainstFreelancerPercentile = m_DataProviders[i].GetAgainstFreelancerPercentile().Value;
			}
			if (m_DataProviders[i].GetAgainstPeersPercentile().HasValue)
			{
				AgainstPeersPercentile = m_DataProviders[i].GetAgainstPeersPercentile().Value;
			}
			if (m_DataProviders[i].GetAgainstRolePercentile().HasValue)
			{
				AgainstRolePercentile = m_DataProviders[i].GetAgainstRolePercentile().Value;
			}
			if (m_DataProviders[i].IsStatLowerBetter().HasValue)
			{
				LowerIsBetter = m_DataProviders[i].IsStatLowerBetter().Value;
			}
			if (m_DataProviders[i].CharacterRole.HasValue)
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
				m_characterRole = m_DataProviders[i].CharacterRole.Value;
			}
			if (m_DataProviders[i].CharacterType.HasValue)
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
				m_characterType = m_DataProviders[i].CharacterType.Value;
			}
			if (m_DataProviders[i].GetCurrentGameValue().HasValue)
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
				num = m_DataProviders[i].GetCurrentGameValue().Value;
			}
			if (m_DataProviders[i].GetStatUnitType().HasValue)
			{
				statUnitType = m_DataProviders[i].GetStatUnitType().Value;
			}
			if (m_DataProviders[i].GetNumTurns().HasValue)
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
				num2 = m_DataProviders[i].GetNumTurns().Value;
			}
			if (m_DataProviders[i].GetNumLives().HasValue)
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
				num3 = m_DataProviders[i].GetNumLives().Value;
			}
			StatDisplayType = m_DataProviders[i].GetStatDisplayType();
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			bool doActive = false;
			if (statUnitType.HasValue)
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
				if (num.HasValue)
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
					if (statUnitType.Value == GameResultBadgeData.StatDescription.StatUnitType.PerTurn)
					{
						if (num2.HasValue)
						{
							doActive = true;
							m_StatSum.text = string.Format(StringUtil.TR("TotalSum", "Global"), num.Value * (float)num2.Value);
							m_StatNumDividedBy.text = string.Format(StringUtil.TR("NumberOfTurns", "Global"), num2.Value);
						}
					}
					else if (statUnitType.Value == GameResultBadgeData.StatDescription.StatUnitType.PerLife)
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
						if (num3.HasValue)
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
							doActive = true;
							m_StatSum.text = string.Format(StringUtil.TR("TotalSum", "Global"), num.Value * (float)num3.Value);
							m_StatNumDividedBy.text = string.Format(StringUtil.TR("NumberOfLives", "Global"), num3.Value);
						}
					}
				}
			}
			UIManager.SetGameObjectActive(m_StatSum, doActive);
			TextMeshProUGUI statName = m_StatName;
			object text;
			if (StatName.IsNullOrEmpty())
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
				text = "Needs to be authored";
			}
			else
			{
				text = StatName;
			}
			statName.text = (string)text;
			m_StatDescription.text = ((!StatDescription.IsNullOrEmpty()) ? StatDescription : "Needs to be authored");
			int num4 = 0;
			while (num4 >= m_DisplayedStatComparers.Count)
			{
				GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = Object.Instantiate(m_StatComparisonPrefab);
				UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
				m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (flag)
				{
					UIManager.SetGameObjectActive(m_DisplayedStatComparers[num4], true);
					m_DisplayedStatComparers[num4].m_StatDisplay.text = string.Format("{2}: <color=#{0}>{1}</color>", m_AverageColorHex, StringUtil.GetLocalizedFloat(PersonalAverage, "0.###"), StringUtil.TR("PersonalAverage", "Stats"));
					UIManager.SetGameObjectActive(m_DisplayedStatComparers[num4].m_Percentile, false);
					num4++;
				}
				LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(m_characterType);
				LocalizationArg_LocalizationPayload localizationArg_LocalizationPayload = LocalizationArg_LocalizationPayload.Create(m_characterRole.GetLocalizedPayload());
				num4 = DisplayCompareAverage(WorldMedian, AgainstAllPercentile, num4, StringUtil.TR("WorldAverage", "Stats"), LowerIsBetter);
				num4 = DisplayCompareAverage(RoleMedian, AgainstRolePercentile, num4, LocalizationPayload.Create("RoleMedian", "Stats", localizationArg_LocalizationPayload).ToString(), LowerIsBetter);
				num4 = DisplayCompareAverage(FreelancerMedian, AgainstFreelancerPercentile, num4, LocalizationPayload.Create("FreelancerMedian", "Stats", localizationArg_Freelancer).ToString(), LowerIsBetter);
				num4 = DisplayCompareAverage(SimilarMMRMedian, AgainstPeersPercentile, num4, StringUtil.TR("SimilarMMRAverage", "Stats"), LowerIsBetter);
				num4 = DisplayCompareAverage(PreviousSeasonAverage, null, num4, StringUtil.TR("PreviousSeasonAverage", "Stats"), LowerIsBetter);
				num4 = DisplayCompareAverage(FriendsAverage, null, num4, StringUtil.TR("FriendAverage", "Stats"), LowerIsBetter);
				if (UIPlayerProgressPanel.Get() != null)
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
					if (StatDisplayType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
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
						if (!UIPlayerProgressPanel.Get().m_statsPanel.HasGlobalStatsToCompareTo)
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
							num4 = DisplayNoStatsAvailable(UIPlayerProgressPanel.Get().m_statsPanel.StatCompareFailure, num4);
							goto IL_0c3d;
						}
					}
					if (StatDisplayType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
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
						if (!UIPlayerProgressPanel.Get().m_statsPanel.HasFreelancerStatsToCompareTo)
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
							num4 = DisplayNoStatsAvailable(UIPlayerProgressPanel.Get().m_statsPanel.StatCompareFailure, num4);
						}
					}
				}
				goto IL_0c3d;
				IL_0c3d:
				for (int j = num4; j < m_DisplayedStatComparers.Count; j++)
				{
					UIManager.SetGameObjectActive(m_DisplayedStatComparers[j], false);
				}
				float num5 = 12f;
				LayoutElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<LayoutElement>();
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					num5 += componentsInChildren[k].preferredHeight;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					RectTransform obj = base.gameObject.transform as RectTransform;
					Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
					obj.sizeDelta = new Vector2(sizeDelta.x, num5);
					return;
				}
			}
		}
	}
}
