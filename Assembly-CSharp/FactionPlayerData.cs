using System;

[Serializable]
public class FactionPlayerData
{
	public int FactionID;

	public int TotalXP;

	public void AddXP(int amount)
	{
		this.TotalXP += amount;
	}

	public int GetXPThroughCurrentLevel(PlayerFactionProgressInfo[] FactionPlayerProgressInfo)
	{
		int num = this.TotalXP;
		for (int i = 0; i < FactionPlayerProgressInfo.Length; i++)
		{
			if (num - FactionPlayerProgressInfo[i].ExperienceToNextLevel < 0)
			{
				break;
			}
			num -= FactionPlayerProgressInfo[i].ExperienceToNextLevel;
		}
		return num;
	}

	public int GetCurrentLevel(PlayerFactionProgressInfo[] FactionPlayerProgressInfo, int adjustedXP = 0)
	{
		int num = 1;
		int num2 = this.TotalXP + adjustedXP;
		for (int i = 0; i < FactionPlayerProgressInfo.Length; i++)
		{
			if (num2 - FactionPlayerProgressInfo[i].ExperienceToNextLevel < 0)
			{
				break;
			}
			num2 -= FactionPlayerProgressInfo[i].ExperienceToNextLevel;
			if (num2 >= 0)
			{
				num++;
			}
		}
		return num;
	}
}
