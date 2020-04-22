using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ExperienceComponent : ICloneable
{
	public int Level
	{
		get;
		set;
	}

	public int XPProgressThroughLevel
	{
		get;
		set;
	}

	public int EnteredTutorial
	{
		get;
		set;
	}

	public TutorialVersion TutorialProgress
	{
		get;
		set;
	}

	public int Matches
	{
		get;
		set;
	}

	public int Wins
	{
		get;
		set;
	}

	public int Kills
	{
		get;
		set;
	}

	public int LossStreak
	{
		get;
		set;
	}

	public EloValues EloValues
	{
		get;
		set;
	}

	public DateTime LastWin
	{
		get;
		set;
	}

	public Dictionary<PersistedStatBucket, PersistedStats> PersistedStatsDictionary
	{
		get;
		set;
	}

	public Dictionary<int, Dictionary<PersistedStatBucket, PersistedStats>> PersistedStatsDictionaryBySeason
	{
		get;
		set;
	}

	public Dictionary<GameType, int> MatchesPerGameType
	{
		get;
		set;
	}

	public Dictionary<GameType, int> WinsPerGameType
	{
		get;
		set;
	}

	public Dictionary<PersistedStatBucket, Dictionary<int, int>> BadgesEarned
	{
		get;
		set;
	}

	public Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>> BadgesEarnedBySeason
	{
		get;
		set;
	}

	[JsonIgnore]
	public int VsHumanMatches
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 16; i++)
			{
				GameType gameType = (GameType)i;
				if (!gameType.IsHumanVsHumanGame())
				{
					continue;
				}
				while (true)
				{
					switch (4)
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
				if (MatchesPerGameType.TryGetValue(gameType, out int value))
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
					num += value;
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return num;
			}
		}
	}

	[JsonIgnore]
	public int VsHumanWins
	{
		get
		{
			int num = 0;
			IEnumerator enumerator = Enum.GetValues(typeof(GameType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameType gameType = (GameType)enumerator.Current;
					if (gameType.IsHumanVsHumanGame())
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (WinsPerGameType.TryGetValue(gameType, out int value))
						{
							num += value;
						}
					}
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return num;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							disposable.Dispose();
							goto end_IL_0074;
						}
					}
				}
				end_IL_0074:;
			}
		}
	}

	[JsonIgnore]
	public int Losses => Matches - Wins;

	[JsonIgnore]
	public int XPToNextLevel
	{
		get
		{
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			return gameBalanceVars.CharacterExperienceToLevel(Level);
		}
	}

	public ExperienceComponent()
	{
		Level = 1;
		TutorialProgress = TutorialVersion.None;
		EloValues = new EloValues();
		PersistedStatsDictionary = new Dictionary<PersistedStatBucket, PersistedStats>();
		PersistedStatsDictionaryBySeason = new Dictionary<int, Dictionary<PersistedStatBucket, PersistedStats>>();
		BadgesEarned = new Dictionary<PersistedStatBucket, Dictionary<int, int>>();
		BadgesEarnedBySeason = new Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>>();
		MatchesPerGameType = new Dictionary<GameType, int>(default(GameTypeComparer));
		WinsPerGameType = new Dictionary<GameType, int>(default(GameTypeComparer));
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
