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
		if (this.m_conditions_anyMet.Length == 0)
		{
			flag = true;
		}
		else
		{
			flag = false;
			foreach (PointCondition pointCondition in this.m_conditions_anyMet)
			{
				if (pointCondition.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
				{
					flag = true;
					goto IL_4F;
				}
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(VictoryCondition.ArePointConditionsMet(int, int, bool, Team)).MethodHandle;
			}
		}
		IL_4F:
		bool flag2;
		if (this.m_conditions_allRequired.Length == 0)
		{
			flag2 = true;
		}
		else
		{
			flag2 = true;
			foreach (PointCondition pointCondition2 in this.m_conditions_allRequired)
			{
				if (!pointCondition2.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
					flag2 = false;
					goto IL_AA;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		IL_AA:
		bool flag3;
		if (this.m_conditions_noneAllowed.Length == 0)
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
			flag3 = true;
		}
		else
		{
			flag3 = true;
			foreach (PointCondition pointCondition3 in this.m_conditions_noneAllowed)
			{
				if (pointCondition3.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
					flag3 = false;
					goto IL_10F;
				}
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
		IL_10F:
		bool result;
		if (flag)
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
			result = (flag2 && flag3);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal string GetVictoryLogString(int allyPoints, int enemyPoints, bool bTimeLimitExpired, Team team)
	{
		string text = string.Empty;
		foreach (PointCondition pointCondition in this.m_conditions_allRequired)
		{
			if (pointCondition != null)
			{
				if (text.Length > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(VictoryCondition.GetVictoryLogString(int, int, bool, Team)).MethodHandle;
					}
					text += " and ";
				}
				text += pointCondition.GetVictoryLogString(allyPoints, enemyPoints, bTimeLimitExpired);
			}
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
		foreach (PointCondition pointCondition2 in this.m_conditions_anyMet)
		{
			if (pointCondition2 != null && pointCondition2.IsConditionMet(allyPoints, enemyPoints, bTimeLimitExpired, team))
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
				if (text.Length > 0)
				{
					text += " and ";
				}
				text += pointCondition2.GetVictoryLogString(allyPoints, enemyPoints, bTimeLimitExpired);
				return text;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return text;
		}
	}
}
