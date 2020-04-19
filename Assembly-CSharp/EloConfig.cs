using System;
using System.Collections.Generic;

[Serializable]
public class EloConfig
{
	public float MatchMakingInitialValue;

	public int NewPlayerHandicapPeriodInGames;

	public int NewPlayerPvPQueueDuration;

	public float PlacementElo;

	public Dictionary<BotDifficulty, float> BotEloValues;

	public EloConfig()
	{
		this.MatchMakingInitialValue = 1000f;
		this.PlacementElo = 1500f;
		this.NewPlayerHandicapPeriodInGames = 5;
		this.NewPlayerPvPQueueDuration = 1;
		this.BotEloValues = new Dictionary<BotDifficulty, float>();
		this.BotEloValues[BotDifficulty.Tutorial] = 1085f;
		this.BotEloValues[BotDifficulty.Stupid] = 1085f;
		this.BotEloValues[BotDifficulty.Easy] = 1420f;
		this.BotEloValues[BotDifficulty.Medium] = 1455f;
		this.BotEloValues[BotDifficulty.Hard] = 1475f;
		this.BotEloValues[BotDifficulty.Expert] = 1485f;
	}

	public EloConfig Clone()
	{
		return (EloConfig)base.MemberwiseClone();
	}

	public float GetHandicappedElo(float elo, int matches, int minPlacementMatches)
	{
		if (matches < minPlacementMatches)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(EloConfig.GetHandicappedElo(float, int, int)).MethodHandle;
			}
			return this.PlacementElo;
		}
		if (matches < this.NewPlayerHandicapPeriodInGames)
		{
			if (elo >= this.MatchMakingInitialValue)
			{
				float num = (float)matches * elo;
				float num2 = (float)(this.NewPlayerHandicapPeriodInGames - matches) * this.MatchMakingInitialValue;
				return (num + num2) / (float)this.NewPlayerHandicapPeriodInGames;
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
		return elo;
	}

	public float GetHandicappedOrHighestElo(float handicappedElo, float accountElo, float characterElo, int matches)
	{
		if (matches >= this.NewPlayerHandicapPeriodInGames)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EloConfig.GetHandicappedOrHighestElo(float, float, float, int)).MethodHandle;
			}
			return Math.Max(accountElo, characterElo);
		}
		return handicappedElo;
	}
}
