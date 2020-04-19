using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFreelancerComparisonItem : MonoBehaviour
{
	public Image m_characterIcon;

	public TextMeshProUGUI m_characterName;

	public TextMeshProUGUI m_statNumber;

	public Image m_fillAmount;

	private IPersistedGameplayStat m_value;

	private UIPlayerProgressOverview.OverviewStat m_overviewStat;

	public void Setup(CharacterResourceLink charLink)
	{
		this.m_characterName.text = charLink.GetDisplayName();
		this.m_characterIcon.sprite = charLink.GetCharacterSelectIcon();
		Color characterColor = charLink.m_characterColor;
		characterColor.a = 1f;
		this.m_fillAmount.color = characterColor;
	}

	public void SetupNewStat(UIPlayerProgressOverview.OverviewStat overviewStat)
	{
		this.m_overviewStat = overviewStat;
		if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.TimePlayed)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFreelancerComparisonItem.SetupNewStat(UIPlayerProgressOverview.OverviewStat)).MethodHandle;
			}
			if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.DamageEfficiency)
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
				if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageDamageDonePerTurn)
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
					if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageDamageTakenPerTurn)
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
						if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageSupportDonePerTurn)
						{
							this.m_value = new PersistedStatEntry();
							return;
						}
					}
				}
			}
		}
		this.m_value = new PersistedStatFloatEntry();
	}

	public void Adjust(int valueIncrease)
	{
		(this.m_value as PersistedStatEntry).Adjust(valueIncrease);
	}

	public void Adjust(float valueIncrease)
	{
		(this.m_value as PersistedStatFloatEntry).Adjust(valueIncrease);
	}

	public void CombineStats(PersistedStatEntry nextValue)
	{
		(this.m_value as PersistedStatEntry).CombineStats(nextValue);
	}

	public void CombineStats(PersistedStatFloatEntry nextValue)
	{
		(this.m_value as PersistedStatFloatEntry).CombineStats(nextValue);
	}

	public float GetValue()
	{
		if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.TimePlayed)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIFreelancerComparisonItem.GetValue()).MethodHandle;
			}
			if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.MatchesWon)
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
				if (this.m_overviewStat != UIPlayerProgressOverview.OverviewStat.NumBadges)
				{
					return this.m_value.Average();
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return this.m_value.GetSum();
	}

	public void SetupDisplay(float maxValue)
	{
		switch (this.m_overviewStat)
		{
		case UIPlayerProgressOverview.OverviewStat.TimePlayed:
		{
			float num = this.m_value.GetSum() / 3600f;
			maxValue /= 3600f;
			this.m_statNumber.text = string.Format(StringUtil.TR("NumHrs", "Global"), StringUtil.GetLocalizedFloat(num, "0.0"));
			Image fillAmount = this.m_fillAmount;
			float fillAmount2;
			if (maxValue > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIFreelancerComparisonItem.SetupDisplay(float)).MethodHandle;
				}
				fillAmount2 = num / maxValue;
			}
			else
			{
				fillAmount2 = 0f;
			}
			fillAmount.fillAmount = fillAmount2;
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.MatchesWon:
		case UIPlayerProgressOverview.OverviewStat.NumBadges:
		{
			float num = this.m_value.GetSum();
			this.m_statNumber.text = UIStorePanel.FormatIntToString(Mathf.RoundToInt(num), true);
			Image fillAmount3 = this.m_fillAmount;
			float fillAmount4;
			if (maxValue > 0f)
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
				fillAmount4 = num / maxValue;
			}
			else
			{
				fillAmount4 = 0f;
			}
			fillAmount3.fillAmount = fillAmount4;
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.WinPercentage:
		{
			maxValue = 1f;
			float num2 = (float)this.m_value.GetNumGames();
			float num;
			if (num2 > 0f)
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
				num = this.m_value.GetSum() / num2;
			}
			else
			{
				num = 0f;
			}
			this.m_fillAmount.fillAmount = num;
			this.m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0%");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.DamageEfficiency:
		{
			float num = this.m_value.Average();
			this.m_fillAmount.fillAmount = num;
			this.m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0%");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerLife:
		case UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerMatch:
		case UIPlayerProgressOverview.OverviewStat.AverageDeathsPerMatch:
		{
			float num = this.m_value.Average();
			this.m_fillAmount.fillAmount = num / maxValue;
			this.m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.00");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.AverageDamageDonePerTurn:
		case UIPlayerProgressOverview.OverviewStat.AverageSupportDonePerTurn:
		case UIPlayerProgressOverview.OverviewStat.AverageDamageTakenPerTurn:
		{
			float num = this.m_value.Average();
			this.m_fillAmount.fillAmount = num / maxValue;
			this.m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0");
			break;
		}
		}
	}
}
