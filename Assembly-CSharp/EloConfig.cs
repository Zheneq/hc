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
		MatchMakingInitialValue = 1000f;
		PlacementElo = 1500f;
		NewPlayerHandicapPeriodInGames = 5;
		NewPlayerPvPQueueDuration = 1;
		BotEloValues = new Dictionary<BotDifficulty, float>();
		BotEloValues[BotDifficulty.Tutorial] = 1085f;
		BotEloValues[BotDifficulty.Stupid] = 1085f;
		BotEloValues[BotDifficulty.Easy] = 1420f;
		BotEloValues[BotDifficulty.Medium] = 1455f;
		BotEloValues[BotDifficulty.Hard] = 1475f;
		BotEloValues[BotDifficulty.Expert] = 1485f;
	}

	public EloConfig Clone()
	{
		return (EloConfig)MemberwiseClone();
	}

	public float GetHandicappedElo(float elo, int matches, int minPlacementMatches)
	{
		if (matches < minPlacementMatches)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return PlacementElo;
				}
			}
		}
		if (matches < NewPlayerHandicapPeriodInGames)
		{
			if (!(elo < MatchMakingInitialValue))
			{
				float num = (float)matches * elo;
				float num2 = (float)(NewPlayerHandicapPeriodInGames - matches) * MatchMakingInitialValue;
				return (num + num2) / (float)NewPlayerHandicapPeriodInGames;
			}
		}
		return elo;
	}

	public float GetHandicappedOrHighestElo(float handicappedElo, float accountElo, float characterElo, int matches)
	{
		if (matches >= NewPlayerHandicapPeriodInGames)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return Math.Max(accountElo, characterElo);
				}
			}
		}
		return handicappedElo;
	}
}
