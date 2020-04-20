using System;
using System.Collections.Generic;
using System.Globalization;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStatTooltip : UITooltipBase
{
	public TextMeshProUGUI m_StatName;

	public TextMeshProUGUI m_StatDescription;

	public GameOverStatTooltipInfoLine m_StatComparisonPrefab;

	public TextMeshProUGUI m_StatSum;

	public TextMeshProUGUI m_StatNumDividedBy;

	public TextMeshProUGUI m_PercentileLabel;

	public string m_AverageColorHex = "AAAAAA";

	public string m_AboveAverageColorHex = "AA0000";

	public string m_BelowAverageColorHex = "00AA00";

	private List<GameOverStatTooltip.IGameOverStatTooltipDataProvider> m_DataProviders = new List<GameOverStatTooltip.IGameOverStatTooltipDataProvider>();

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
		this.m_DisplayedStatComparers.Add(this.m_StatComparisonPrefab);
	}

	public void ClearDataProviders()
	{
		this.m_DataProviders.Clear();
	}

	public void AddNewDataProvider(GameOverStatTooltip.IGameOverStatTooltipDataProvider provider)
	{
		if (provider != null)
		{
			if (!this.m_DataProviders.Contains(provider))
			{
				this.m_DataProviders.Add(provider);
			}
			this.Refresh();
		}
	}

	private int DisplayPercentile(int? percentile, int displayIndex, string localizedString)
	{
		if (percentile != null)
		{
			while (displayIndex >= this.m_DisplayedStatComparers.Count)
			{
				GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = UnityEngine.Object.Instantiate<GameOverStatTooltipInfoLine>(this.m_StatComparisonPrefab);
				UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
				this.m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
			}
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex], true, null);
			TMP_Text statDisplay = this.m_DisplayedStatComparers[displayIndex].m_StatDisplay;
			string format = "{2}: <color=#{0}>{1}%</color>";
			object arg;
			if (percentile.Value > 0x42)
			{
				arg = this.m_AboveAverageColorHex;
			}
			else
			{
				arg = ((percentile.Value >= 0x22) ? this.m_AverageColorHex : this.m_BelowAverageColorHex);
			}
			statDisplay.text = string.Format(format, arg, percentile.Value.ToString("g", CultureInfo.CurrentCulture), localizedString);
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex].m_Percentile, false, null);
			return displayIndex + 1;
		}
		return displayIndex;
	}

	private string GetColorHex(float AverageValue, float CurrentValue, bool LowerIsBetter)
	{
		if (AverageValue == CurrentValue)
		{
			return this.m_AverageColorHex;
		}
		if (LowerIsBetter)
		{
			if (CurrentValue > AverageValue)
			{
				return this.m_BelowAverageColorHex;
			}
			return this.m_AboveAverageColorHex;
		}
		else
		{
			if (CurrentValue > AverageValue)
			{
				return this.m_AboveAverageColorHex;
			}
			return this.m_BelowAverageColorHex;
		}
	}

	private string GetNumberEnding(int percentile)
	{
		if (!(LocalizationManager.CurrentLanguageCode == "en"))
		{
			return string.Empty;
		}
		int num = percentile % 0xA;
		if (num == 1)
		{
			return "st";
		}
		if (num == 2)
		{
			return "nd";
		}
		if (num == 3)
		{
			return "rd";
		}
		return "th";
	}

	private int DisplayCompareAverage(float? Average, int? Percentile, int displayIndex, string localizedString, bool LowerIsBetter)
	{
		if (Average != null)
		{
			while (displayIndex >= this.m_DisplayedStatComparers.Count)
			{
				GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = UnityEngine.Object.Instantiate<GameOverStatTooltipInfoLine>(this.m_StatComparisonPrefab);
				UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
				this.m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
			}
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex], true, null);
			this.m_DisplayedStatComparers[displayIndex].m_StatDisplay.text = string.Format("{1}: {0}", StringUtil.GetLocalizedFloat(Average.Value, "0.###"), localizedString);
			if (Percentile != null)
			{
				UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex].m_Percentile, true, null);
				this.m_DisplayedStatComparers[displayIndex].m_Percentile.text = string.Format(StringUtil.TR("PercentileTooltip", "Global"), Percentile.Value, this.GetNumberEnding(Percentile.Value));
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex].m_Percentile, false, null);
			}
			return displayIndex + 1;
		}
		return displayIndex;
	}

	private int DisplayNoStatsAvailable(string errorDisplay, int displayIndex)
	{
		if (!errorDisplay.IsNullOrEmpty())
		{
			while (displayIndex >= this.m_DisplayedStatComparers.Count)
			{
				GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = UnityEngine.Object.Instantiate<GameOverStatTooltipInfoLine>(this.m_StatComparisonPrefab);
				UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
				this.m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
			}
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex], true, null);
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[displayIndex].m_Percentile, false, null);
			this.m_DisplayedStatComparers[displayIndex].m_StatDisplay.text = errorDisplay;
			this.m_DisplayedStatComparers[displayIndex].GetComponent<LayoutElement>().preferredHeight = this.m_DisplayedStatComparers[displayIndex].m_StatDisplay.preferredHeight;
			return displayIndex + 1;
		}
		return displayIndex;
	}

	public void Refresh()
	{
		bool flag = false;
		this.PersonalAverage = 0f;
		this.PreviousSeasonAverage = null;
		this.FriendsAverage = null;
		this.SimilarMMRMedian = null;
		this.WorldMedian = null;
		this.FreelancerMedian = null;
		this.RoleMedian = null;
		this.AgainstAllPercentile = null;
		this.AgainstFreelancerPercentile = null;
		this.AgainstPeersPercentile = null;
		this.AgainstRolePercentile = null;
		this.LowerIsBetter = false;
		this.m_characterRole = CharacterRole.None;
		this.m_characterType = CharacterType.None;
		float? num = null;
		GameResultBadgeData.StatDescription.StatUnitType? statUnitType = null;
		int? num2 = null;
		int? num3 = null;
		for (int i = 0; i < this.m_DataProviders.Count; i++)
		{
			if (this.m_DataProviders[i] == null)
			{
				this.m_DataProviders.RemoveAt(i);
				i--;
			}
			else
			{
				if (!this.m_DataProviders[i].GetStatName().IsNullOrEmpty())
				{
					this.StatName = this.m_DataProviders[i].GetStatName();
				}
				if (!this.m_DataProviders[i].GetStatDescription().IsNullOrEmpty())
				{
					this.StatDescription = this.m_DataProviders[i].GetStatDescription();
				}
				if (this.m_DataProviders[i].GetPersonalAverage() != null)
				{
					this.PersonalAverage = this.m_DataProviders[i].GetPersonalAverage().Value;
					flag = true;
				}
				if (this.m_DataProviders[i].GetPreviousSeasonAverage() != null)
				{
					this.PreviousSeasonAverage = new float?(this.m_DataProviders[i].GetPreviousSeasonAverage().Value);
				}
				if (this.m_DataProviders[i].GetFriendsAverage() != null)
				{
					this.FriendsAverage = new float?(this.m_DataProviders[i].GetFriendsAverage().Value);
				}
				if (this.m_DataProviders[i].GetPeerMedian() != null)
				{
					this.SimilarMMRMedian = new float?(this.m_DataProviders[i].GetPeerMedian().Value);
				}
				if (this.m_DataProviders[i].GetFreelancerMedian() != null)
				{
					this.FreelancerMedian = new float?(this.m_DataProviders[i].GetFreelancerMedian().Value);
				}
				if (this.m_DataProviders[i].GetRoleMedian() != null)
				{
					this.RoleMedian = new float?(this.m_DataProviders[i].GetRoleMedian().Value);
				}
				if (this.m_DataProviders[i].GetWorldMedian() != null)
				{
					this.WorldMedian = new float?(this.m_DataProviders[i].GetWorldMedian().Value);
				}
				if (this.m_DataProviders[i].GetAgainstAllPercentile() != null)
				{
					this.AgainstAllPercentile = new int?(this.m_DataProviders[i].GetAgainstAllPercentile().Value);
				}
				if (this.m_DataProviders[i].GetAgainstFreelancerPercentile() != null)
				{
					this.AgainstFreelancerPercentile = new int?(this.m_DataProviders[i].GetAgainstFreelancerPercentile().Value);
				}
				if (this.m_DataProviders[i].GetAgainstPeersPercentile() != null)
				{
					this.AgainstPeersPercentile = new int?(this.m_DataProviders[i].GetAgainstPeersPercentile().Value);
				}
				if (this.m_DataProviders[i].GetAgainstRolePercentile() != null)
				{
					this.AgainstRolePercentile = new int?(this.m_DataProviders[i].GetAgainstRolePercentile().Value);
				}
				if (this.m_DataProviders[i].IsStatLowerBetter() != null)
				{
					this.LowerIsBetter = this.m_DataProviders[i].IsStatLowerBetter().Value;
				}
				if (this.m_DataProviders[i].CharacterRole != null)
				{
					this.m_characterRole = this.m_DataProviders[i].CharacterRole.Value;
				}
				if (this.m_DataProviders[i].CharacterType != null)
				{
					this.m_characterType = this.m_DataProviders[i].CharacterType.Value;
				}
				if (this.m_DataProviders[i].GetCurrentGameValue() != null)
				{
					num = new float?(this.m_DataProviders[i].GetCurrentGameValue().Value);
				}
				if (this.m_DataProviders[i].GetStatUnitType() != null)
				{
					statUnitType = new GameResultBadgeData.StatDescription.StatUnitType?(this.m_DataProviders[i].GetStatUnitType().Value);
				}
				if (this.m_DataProviders[i].GetNumTurns() != null)
				{
					num2 = new int?(this.m_DataProviders[i].GetNumTurns().Value);
				}
				if (this.m_DataProviders[i].GetNumLives() != null)
				{
					num3 = new int?(this.m_DataProviders[i].GetNumLives().Value);
				}
				this.StatDisplayType = this.m_DataProviders[i].GetStatDisplayType();
			}
		}
		bool doActive = false;
		if (statUnitType != null)
		{
			if (num != null)
			{
				if (statUnitType.Value == GameResultBadgeData.StatDescription.StatUnitType.PerTurn)
				{
					if (num2 != null)
					{
						doActive = true;
						this.m_StatSum.text = string.Format(StringUtil.TR("TotalSum", "Global"), num.Value * (float)num2.Value);
						this.m_StatNumDividedBy.text = string.Format(StringUtil.TR("NumberOfTurns", "Global"), num2.Value);
					}
				}
				else if (statUnitType.Value == GameResultBadgeData.StatDescription.StatUnitType.PerLife)
				{
					if (num3 != null)
					{
						doActive = true;
						this.m_StatSum.text = string.Format(StringUtil.TR("TotalSum", "Global"), num.Value * (float)num3.Value);
						this.m_StatNumDividedBy.text = string.Format(StringUtil.TR("NumberOfLives", "Global"), num3.Value);
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_StatSum, doActive, null);
		TMP_Text statName = this.m_StatName;
		string text;
		if (this.StatName.IsNullOrEmpty())
		{
			text = "Needs to be authored";
		}
		else
		{
			text = this.StatName;
		}
		statName.text = text;
		this.m_StatDescription.text = ((!this.StatDescription.IsNullOrEmpty()) ? this.StatDescription : "Needs to be authored");
		int j = 0;
		while (j >= this.m_DisplayedStatComparers.Count)
		{
			GameOverStatTooltipInfoLine gameOverStatTooltipInfoLine = UnityEngine.Object.Instantiate<GameOverStatTooltipInfoLine>(this.m_StatComparisonPrefab);
			UIManager.ReparentTransform(gameOverStatTooltipInfoLine.transform, base.gameObject.transform);
			this.m_DisplayedStatComparers.Add(gameOverStatTooltipInfoLine);
		}
		if (flag)
		{
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[j], true, null);
			this.m_DisplayedStatComparers[j].m_StatDisplay.text = string.Format("{2}: <color=#{0}>{1}</color>", this.m_AverageColorHex, StringUtil.GetLocalizedFloat(this.PersonalAverage, "0.###"), StringUtil.TR("PersonalAverage", "Stats"));
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[j].m_Percentile, false, null);
			j++;
		}
		LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(this.m_characterType);
		LocalizationArg_LocalizationPayload localizationArg_LocalizationPayload = LocalizationArg_LocalizationPayload.Create(this.m_characterRole.GetLocalizedPayload());
		j = this.DisplayCompareAverage(this.WorldMedian, this.AgainstAllPercentile, j, StringUtil.TR("WorldAverage", "Stats"), this.LowerIsBetter);
		j = this.DisplayCompareAverage(this.RoleMedian, this.AgainstRolePercentile, j, LocalizationPayload.Create("RoleMedian", "Stats", new LocalizationArg[]
		{
			localizationArg_LocalizationPayload
		}).ToString(), this.LowerIsBetter);
		j = this.DisplayCompareAverage(this.FreelancerMedian, this.AgainstFreelancerPercentile, j, LocalizationPayload.Create("FreelancerMedian", "Stats", new LocalizationArg[]
		{
			localizationArg_Freelancer
		}).ToString(), this.LowerIsBetter);
		j = this.DisplayCompareAverage(this.SimilarMMRMedian, this.AgainstPeersPercentile, j, StringUtil.TR("SimilarMMRAverage", "Stats"), this.LowerIsBetter);
		j = this.DisplayCompareAverage(this.PreviousSeasonAverage, null, j, StringUtil.TR("PreviousSeasonAverage", "Stats"), this.LowerIsBetter);
		j = this.DisplayCompareAverage(this.FriendsAverage, null, j, StringUtil.TR("FriendAverage", "Stats"), this.LowerIsBetter);
		if (UIPlayerProgressPanel.Get() != null)
		{
			if (this.StatDisplayType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
			{
				if (!UIPlayerProgressPanel.Get().m_statsPanel.HasGlobalStatsToCompareTo)
				{
					j = this.DisplayNoStatsAvailable(UIPlayerProgressPanel.Get().m_statsPanel.StatCompareFailure, j);
					goto IL_C3D;
				}
			}
			if (this.StatDisplayType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
			{
				if (!UIPlayerProgressPanel.Get().m_statsPanel.HasFreelancerStatsToCompareTo)
				{
					j = this.DisplayNoStatsAvailable(UIPlayerProgressPanel.Get().m_statsPanel.StatCompareFailure, j);
				}
			}
		}
		IL_C3D:
		for (int k = j; k < this.m_DisplayedStatComparers.Count; k++)
		{
			UIManager.SetGameObjectActive(this.m_DisplayedStatComparers[k], false, null);
		}
		float num4 = 12f;
		LayoutElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<LayoutElement>();
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			num4 += componentsInChildren[l].preferredHeight;
		}
		(base.gameObject.transform as RectTransform).sizeDelta = new Vector2((base.gameObject.transform as RectTransform).sizeDelta.x, num4);
	}

	public interface IGameOverStatTooltipDataProvider
	{
		string GetStatName();

		string GetStatDescription();

		CharacterRole? CharacterRole { get; }

		CharacterType? CharacterType { get; }

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
}
