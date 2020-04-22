using System;

[Serializable]
public class VictoryCondition
{
	public string m_conditionString;

	public string m_PointName = "Kills@VictoryCondition";

	public PointCondition[] m_conditions_anyMet;

	public PointCondition[] m_conditions_allRequired;

	public PointCondition[] m_conditions_noneAllowed;

	public bool ArePointConditionsMet(int allyPoints, int enemyPoints, bool bTimeLimitExpired, Team team)
	{
		bool flag;
		if (m_conditions_anyMet.Length == 0)
		{
			flag = true;
		}
		else
		{
			flag = false;
			PointCondition[] conditions_anyMet = m_conditions_anyMet;
			int num = 0;
			while (true)
			{
				if (num < conditions_anyMet.Length)
				{
					PointCondition pointCondition = conditions_anyMet[num];
					if (pointCondition.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
					{
						flag = true;
						break;
					}
					num++;
					continue;
				}
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
				break;
			}
		}
		bool flag2;
		if (m_conditions_allRequired.Length == 0)
		{
			flag2 = true;
		}
		else
		{
			flag2 = true;
			PointCondition[] conditions_allRequired = m_conditions_allRequired;
			int num2 = 0;
			while (true)
			{
				if (num2 < conditions_allRequired.Length)
				{
					PointCondition pointCondition2 = conditions_allRequired[num2];
					if (!pointCondition2.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
						flag2 = false;
						break;
					}
					num2++;
					continue;
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
				break;
			}
		}
		bool flag3;
		if (m_conditions_noneAllowed.Length == 0)
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
			flag3 = true;
		}
		else
		{
			flag3 = true;
			PointCondition[] conditions_noneAllowed = m_conditions_noneAllowed;
			int num3 = 0;
			while (true)
			{
				if (num3 < conditions_noneAllowed.Length)
				{
					PointCondition pointCondition3 = conditions_noneAllowed[num3];
					if (pointCondition3.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
						flag3 = false;
						break;
					}
					num3++;
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		int result;
		if (flag)
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
			result = ((flag2 && flag3) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal string GetVictoryLogString(int allyPoints, int enemyPoints, bool bTimeLimitExpired, Team team)
	{
		string text = string.Empty;
		PointCondition[] conditions_allRequired = m_conditions_allRequired;
		foreach (PointCondition pointCondition in conditions_allRequired)
		{
			if (pointCondition == null)
			{
				continue;
			}
			if (text.Length > 0)
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
				text += " and ";
			}
			text += pointCondition.GetVictoryLogString(allyPoints, enemyPoints, bTimeLimitExpired);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			PointCondition[] conditions_anyMet = m_conditions_anyMet;
			int num = 0;
			while (true)
			{
				if (num < conditions_anyMet.Length)
				{
					PointCondition pointCondition2 = conditions_anyMet[num];
					if (pointCondition2 != null && pointCondition2.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
						if (text.Length > 0)
						{
							text += " and ";
						}
						text += pointCondition2.GetVictoryLogString(allyPoints, enemyPoints, bTimeLimitExpired);
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			return text;
		}
	}
}
