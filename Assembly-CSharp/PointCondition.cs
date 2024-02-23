using System;
using System.Text;

[Serializable]
public class PointCondition
{
	public enum PointRelationship
	{
		GreaterThan,
		LessThan,
		GreaterThanOrEqualTo,
		LessThanOrEqualTo,
		EqualTo,
		DontCare
	}

	public enum WhenRelationship
	{
		AllTheTime,
		OnlyAfterTurnLimit,
		OnlyBeforeTurnLimit
	}

	public PointRelationship m_pointsMustBe = PointRelationship.GreaterThanOrEqualTo;

	public int threshold;

	public bool subtractEnemyPoints;

	public WhenRelationship whenRelationship = WhenRelationship.OnlyAfterTurnLimit;

	public CaptureTheFlag.CTF_VictoryCondition[] m_CTF_conditions;

	public CollectTheCoins.CollectTheCoins_VictoryCondition[] m_CTC_conditions;

	public bool IsConditionMet(int allyPoints, int enemyPoints, bool timeLimitExpired, Team team)
	{
		if (whenRelationship == WhenRelationship.OnlyAfterTurnLimit)
		{
			if (!timeLimitExpired)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (whenRelationship == WhenRelationship.OnlyBeforeTurnLimit)
		{
			if (timeLimitExpired)
			{
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
			}
		}
		if (subtractEnemyPoints)
		{
			allyPoints -= enemyPoints;
		}
		bool flag;
		switch (m_pointsMustBe)
		{
		case PointRelationship.GreaterThan:
			flag = (allyPoints > threshold);
			break;
		case PointRelationship.LessThan:
			flag = (allyPoints < threshold);
			break;
		case PointRelationship.GreaterThanOrEqualTo:
			flag = (allyPoints >= threshold);
			break;
		case PointRelationship.LessThanOrEqualTo:
			flag = (allyPoints <= threshold);
			break;
		case PointRelationship.EqualTo:
			flag = (allyPoints == threshold);
			break;
		case PointRelationship.DontCare:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		bool flag2;
		if (m_CTF_conditions != null)
		{
			if (m_CTF_conditions.Length != 0)
			{
				if (CaptureTheFlag.AreCtfVictoryConditionsMetForTeam(m_CTF_conditions, team))
				{
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
				goto IL_0118;
			}
		}
		flag2 = true;
		goto IL_0118;
		IL_0118:
		bool flag3;
		if (m_CTC_conditions != null)
		{
			if (m_CTC_conditions.Length != 0)
			{
				if (CollectTheCoins.AreCtcVictoryConditionsMetForTeam(m_CTC_conditions, team))
				{
					flag3 = true;
				}
				else
				{
					flag3 = false;
				}
				goto IL_0163;
			}
		}
		flag3 = true;
		goto IL_0163;
		IL_0163:
		int result;
		if (flag && flag2)
		{
			result = (flag3 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal string GetVictoryLogString(int allyPoints, int enemyPoints, bool bTimeLimitExpired)
	{
		string text = string.Empty;
		WhenRelationship whenRelationship = this.whenRelationship;
		if (whenRelationship != WhenRelationship.OnlyAfterTurnLimit)
		{
			if (whenRelationship == WhenRelationship.OnlyBeforeTurnLimit)
			{
				if (bTimeLimitExpired)
				{
					text = "ERROR(too late)";
				}
				else
				{
					text = "Before Turn Limit";
				}
			}
		}
		else if (!bTimeLimitExpired)
		{
			text = "ERROR(too soon)";
		}
		else
		{
			text = "Turn Limit Expired";
		}
		string str = new StringBuilder().Append("score(").Append(allyPoints).Append(")").ToString();
		string str2 = (!subtractEnemyPoints) ? new StringBuilder().Append(string.Empty).Append(threshold).ToString() : new StringBuilder().Append("score(").Append(enemyPoints).Append(")+").Append(threshold).ToString();
		string text2 = "ERROR(case)";
		switch (m_pointsMustBe)
		{
		case PointRelationship.GreaterThan:
			text2 = new StringBuilder().Append(str).Append(">").Append(str2).ToString();
			break;
		case PointRelationship.LessThan:
			text2 = new StringBuilder().Append(str).Append("<").Append(str2).ToString();
			break;
		case PointRelationship.GreaterThanOrEqualTo:
			text2 = new StringBuilder().Append(str).Append(">=").Append(str2).ToString();
			break;
		case PointRelationship.LessThanOrEqualTo:
			text2 = new StringBuilder().Append(str).Append("<=").Append(str2).ToString();
			break;
		case PointRelationship.EqualTo:
			text2 = new StringBuilder().Append(str).Append("=").Append(str2).ToString();
			break;
		case PointRelationship.DontCare:
			text2 = string.Empty;
			break;
		}
		for (int i = 0; i < m_CTF_conditions.Length; i++)
		{
			text2 = new StringBuilder().Append(text2).Append(m_CTF_conditions[i].ToString()).Append(" ").ToString();
		}
		while (true)
		{
			string result;
			if (text.Length > 0)
			{
				result = new StringBuilder().Append(text).Append(" and ").Append(text2).ToString();
			}
			else
			{
				result = text2;
			}
			return result;
		}
	}
}
