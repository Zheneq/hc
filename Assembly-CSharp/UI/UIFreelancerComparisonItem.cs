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
		m_characterName.text = charLink.GetDisplayName();
		m_characterIcon.sprite = charLink.GetCharacterSelectIcon();
		Color characterColor = charLink.m_characterColor;
		characterColor.a = 1f;
		m_fillAmount.color = characterColor;
	}

	public void SetupNewStat(UIPlayerProgressOverview.OverviewStat overviewStat)
	{
		m_overviewStat = overviewStat;
		if (m_overviewStat != 0)
		{
			if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.DamageEfficiency)
			{
				if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageDamageDonePerTurn)
				{
					if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageDamageTakenPerTurn)
					{
						if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.AverageSupportDonePerTurn)
						{
							m_value = new PersistedStatEntry();
							return;
						}
					}
				}
			}
		}
		m_value = new PersistedStatFloatEntry();
	}

	public void Adjust(int valueIncrease)
	{
		(m_value as PersistedStatEntry).Adjust(valueIncrease);
	}

	public void Adjust(float valueIncrease)
	{
		(m_value as PersistedStatFloatEntry).Adjust(valueIncrease);
	}

	public void CombineStats(PersistedStatEntry nextValue)
	{
		(m_value as PersistedStatEntry).CombineStats(nextValue);
	}

	public void CombineStats(PersistedStatFloatEntry nextValue)
	{
		(m_value as PersistedStatFloatEntry).CombineStats(nextValue);
	}

	public float GetValue()
	{
		if (m_overviewStat != 0)
		{
			if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.MatchesWon)
			{
				if (m_overviewStat != UIPlayerProgressOverview.OverviewStat.NumBadges)
				{
					return m_value.Average();
				}
			}
		}
		return m_value.GetSum();
	}

	public void SetupDisplay(float maxValue)
	{
		switch (m_overviewStat)
		{
		case UIPlayerProgressOverview.OverviewStat.TimePlayed:
		{
			float num = m_value.GetSum() / 3600f;
			maxValue /= 3600f;
			m_statNumber.text = string.Format(StringUtil.TR("NumHrs", "Global"), StringUtil.GetLocalizedFloat(num, "0.0"));
			Image fillAmount3 = m_fillAmount;
			float fillAmount4;
			if (maxValue > 0f)
			{
				fillAmount4 = num / maxValue;
			}
			else
			{
				fillAmount4 = 0f;
			}
			fillAmount3.fillAmount = fillAmount4;
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.MatchesWon:
		case UIPlayerProgressOverview.OverviewStat.NumBadges:
		{
			float num = m_value.GetSum();
			m_statNumber.text = UIStorePanel.FormatIntToString(Mathf.RoundToInt(num), true);
			Image fillAmount = m_fillAmount;
			float fillAmount2;
			if (maxValue > 0f)
			{
				fillAmount2 = num / maxValue;
			}
			else
			{
				fillAmount2 = 0f;
			}
			fillAmount.fillAmount = fillAmount2;
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.WinPercentage:
		{
			maxValue = 1f;
			float num2 = m_value.GetNumGames();
			float num;
			if (num2 > 0f)
			{
				num = m_value.GetSum() / num2;
			}
			else
			{
				num = 0f;
			}
			m_fillAmount.fillAmount = num;
			m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0%");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.DamageEfficiency:
		{
			float num = m_value.Average();
			m_fillAmount.fillAmount = num;
			m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0%");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerLife:
		case UIPlayerProgressOverview.OverviewStat.AverageTakedownsPerMatch:
		case UIPlayerProgressOverview.OverviewStat.AverageDeathsPerMatch:
		{
			float num = m_value.Average();
			m_fillAmount.fillAmount = num / maxValue;
			m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.00");
			break;
		}
		case UIPlayerProgressOverview.OverviewStat.AverageDamageDonePerTurn:
		case UIPlayerProgressOverview.OverviewStat.AverageSupportDonePerTurn:
		case UIPlayerProgressOverview.OverviewStat.AverageDamageTakenPerTurn:
		{
			float num = m_value.Average();
			m_fillAmount.fillAmount = num / maxValue;
			m_statNumber.text = StringUtil.GetLocalizedFloat(num, "0.0");
			break;
		}
		}
	}
}
