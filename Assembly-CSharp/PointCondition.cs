using System;

[Serializable]
public class PointCondition
{
	public PointCondition.PointRelationship m_pointsMustBe = PointCondition.PointRelationship.GreaterThanOrEqualTo;

	public int threshold;

	public bool subtractEnemyPoints;

	public PointCondition.WhenRelationship whenRelationship = PointCondition.WhenRelationship.OnlyAfterTurnLimit;

	public CaptureTheFlag.CTF_VictoryCondition[] m_CTF_conditions;

	public CollectTheCoins.CollectTheCoins_VictoryCondition[] m_CTC_conditions;

	public bool IsConditionMet(int allyPoints, int enemyPoints, bool timeLimitExpired, Team team)
	{
		if (this.whenRelationship == PointCondition.WhenRelationship.OnlyAfterTurnLimit)
		{
			if (!timeLimitExpired)
			{
				return false;
			}
		}
		if (this.whenRelationship == PointCondition.WhenRelationship.OnlyBeforeTurnLimit)
		{
			if (timeLimitExpired)
			{
				return false;
			}
		}
		if (this.subtractEnemyPoints)
		{
			allyPoints -= enemyPoints;
		}
		bool flag;
		switch (this.m_pointsMustBe)
		{
		case PointCondition.PointRelationship.GreaterThan:
			flag = (allyPoints > this.threshold);
			break;
		case PointCondition.PointRelationship.LessThan:
			flag = (allyPoints < this.threshold);
			break;
		case PointCondition.PointRelationship.GreaterThanOrEqualTo:
			flag = (allyPoints >= this.threshold);
			break;
		case PointCondition.PointRelationship.LessThanOrEqualTo:
			flag = (allyPoints <= this.threshold);
			break;
		case PointCondition.PointRelationship.EqualTo:
			flag = (allyPoints == this.threshold);
			break;
		case PointCondition.PointRelationship.DontCare:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		bool flag2;
		if (this.m_CTF_conditions != null)
		{
			if (this.m_CTF_conditions.Length == 0)
			{
			}
			else
			{
				if (CaptureTheFlag.AreCtfVictoryConditionsMetForTeam(this.m_CTF_conditions, team))
				{
					flag2 = true;
					goto IL_118;
				}
				flag2 = false;
				goto IL_118;
			}
		}
		flag2 = true;
		IL_118:
		bool flag3;
		if (this.m_CTC_conditions != null)
		{
			if (this.m_CTC_conditions.Length == 0)
			{
			}
			else
			{
				if (CollectTheCoins.AreCtcVictoryConditionsMetForTeam(this.m_CTC_conditions, team))
				{
					flag3 = true;
					goto IL_163;
				}
				flag3 = false;
				goto IL_163;
			}
		}
		flag3 = true;
		IL_163:
		bool result;
		if (flag && flag2)
		{
			result = flag3;
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal string GetVictoryLogString(int allyPoints, int enemyPoints, bool bTimeLimitExpired)
	{
		string text = string.Empty;
		PointCondition.WhenRelationship whenRelationship = this.whenRelationship;
		if (whenRelationship != PointCondition.WhenRelationship.OnlyAfterTurnLimit)
		{
			if (whenRelationship == PointCondition.WhenRelationship.OnlyBeforeTurnLimit)
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
		string str = "score(" + allyPoints + ")";
		string str2 = (!this.subtractEnemyPoints) ? (string.Empty + this.threshold) : string.Concat(new object[]
		{
			"score(",
			enemyPoints,
			")+",
			this.threshold
		});
		string text2 = "ERROR(case)";
		switch (this.m_pointsMustBe)
		{
		case PointCondition.PointRelationship.GreaterThan:
			text2 = str + ">" + str2;
			break;
		case PointCondition.PointRelationship.LessThan:
			text2 = str + "<" + str2;
			break;
		case PointCondition.PointRelationship.GreaterThanOrEqualTo:
			text2 = str + ">=" + str2;
			break;
		case PointCondition.PointRelationship.LessThanOrEqualTo:
			text2 = str + "<=" + str2;
			break;
		case PointCondition.PointRelationship.EqualTo:
			text2 = str + "=" + str2;
			break;
		case PointCondition.PointRelationship.DontCare:
			text2 = string.Empty;
			break;
		}
		for (int i = 0; i < this.m_CTF_conditions.Length; i++)
		{
			text2 = text2 + this.m_CTF_conditions[i].ToString() + " ";
		}
		string result;
		if (text.Length > 0)
		{
			result = text + " and " + text2;
		}
		else
		{
			result = text2;
		}
		return result;
	}

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
}
