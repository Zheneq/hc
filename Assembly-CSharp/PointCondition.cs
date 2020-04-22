using System;

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
			while (true)
			{
				switch (1)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
				goto IL_0118;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag2 = true;
		goto IL_0118;
		IL_0118:
		bool flag3;
		if (m_CTC_conditions != null)
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
			if (m_CTC_conditions.Length != 0)
			{
				if (CollectTheCoins.AreCtcVictoryConditionsMetForTeam(m_CTC_conditions, team))
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
					flag3 = true;
				}
				else
				{
					flag3 = false;
				}
				goto IL_0163;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag3 = true;
		goto IL_0163;
		IL_0163:
		int result;
		if (flag && flag2)
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
			while (true)
			{
				switch (3)
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
			if (whenRelationship == WhenRelationship.OnlyBeforeTurnLimit)
			{
				if (bTimeLimitExpired)
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			text = "ERROR(too soon)";
		}
		else
		{
			text = "Turn Limit Expired";
		}
		string str = "score(" + allyPoints + ")";
		string str2 = (!subtractEnemyPoints) ? (string.Empty + threshold) : ("score(" + enemyPoints + ")+" + threshold);
		string text2 = "ERROR(case)";
		switch (m_pointsMustBe)
		{
		case PointRelationship.GreaterThan:
			text2 = str + ">" + str2;
			break;
		case PointRelationship.LessThan:
			text2 = str + "<" + str2;
			break;
		case PointRelationship.GreaterThanOrEqualTo:
			text2 = str + ">=" + str2;
			break;
		case PointRelationship.LessThanOrEqualTo:
			text2 = str + "<=" + str2;
			break;
		case PointRelationship.EqualTo:
			text2 = str + "=" + str2;
			break;
		case PointRelationship.DontCare:
			text2 = string.Empty;
			break;
		}
		for (int i = 0; i < m_CTF_conditions.Length; i++)
		{
			text2 = text2 + m_CTF_conditions[i].ToString() + " ";
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			string result;
			if (text.Length > 0)
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
				result = text + " and " + text2;
			}
			else
			{
				result = text2;
			}
			return result;
		}
	}
}
