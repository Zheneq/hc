using System;

[Serializable]
public class FactionPlayerData
{
	public int FactionID;

	public int TotalXP;

	public void AddXP(int amount)
	{
		TotalXP += amount;
	}

	public int GetXPThroughCurrentLevel(PlayerFactionProgressInfo[] FactionPlayerProgressInfo)
	{
		int num = TotalXP;
		int num2 = 0;
		while (num2 < FactionPlayerProgressInfo.Length && num - FactionPlayerProgressInfo[num2].ExperienceToNextLevel >= 0)
		{
			while (true)
			{
				num -= FactionPlayerProgressInfo[num2].ExperienceToNextLevel;
				num2++;
				goto IL_003e;
			}
			IL_003e:;
		}
		return num;
	}

	public int GetCurrentLevel(PlayerFactionProgressInfo[] FactionPlayerProgressInfo, int adjustedXP = 0)
	{
		int num = 1;
		int num2 = TotalXP + adjustedXP;
		for (int i = 0; i < FactionPlayerProgressInfo.Length && num2 - FactionPlayerProgressInfo[i].ExperienceToNextLevel >= 0; i++)
		{
			num2 -= FactionPlayerProgressInfo[i].ExperienceToNextLevel;
			if (num2 >= 0)
			{
				num++;
			}
		}
		return num;
	}
}
