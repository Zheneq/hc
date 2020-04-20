using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ExperienceComponent : ICloneable
{
	public ExperienceComponent()
	{
		this.Level = 1;
		this.TutorialProgress = TutorialVersion.None;
		this.EloValues = new EloValues();
		this.PersistedStatsDictionary = new Dictionary<PersistedStatBucket, PersistedStats>();
		this.PersistedStatsDictionaryBySeason = new Dictionary<int, Dictionary<PersistedStatBucket, PersistedStats>>();
		this.BadgesEarned = new Dictionary<PersistedStatBucket, Dictionary<int, int>>();
		this.BadgesEarnedBySeason = new Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>>();
		this.MatchesPerGameType = new Dictionary<GameType, int>(default(GameTypeComparer));
		this.WinsPerGameType = new Dictionary<GameType, int>(default(GameTypeComparer));
	}

	public int Level { get; set; }

	public int XPProgressThroughLevel { get; set; }

	public int EnteredTutorial { get; set; }

	public TutorialVersion TutorialProgress { get; set; }

	public int Matches { get; set; }

	public int Wins { get; set; }

	public int Kills { get; set; }

	public int LossStreak { get; set; }

	public EloValues EloValues { get; set; }

	public DateTime LastWin { get; set; }

	public Dictionary<PersistedStatBucket, PersistedStats> PersistedStatsDictionary { get; set; }

	public Dictionary<int, Dictionary<PersistedStatBucket, PersistedStats>> PersistedStatsDictionaryBySeason { get; set; }

	public Dictionary<GameType, int> MatchesPerGameType { get; set; }

	public Dictionary<GameType, int> WinsPerGameType { get; set; }

	public Dictionary<PersistedStatBucket, Dictionary<int, int>> BadgesEarned { get; set; }

	public Dictionary<int, Dictionary<PersistedStatBucket, Dictionary<int, int>>> BadgesEarnedBySeason { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	[JsonIgnore]
	public int VsHumanMatches
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 0x10; i++)
			{
				GameType gameType = (GameType)i;
				if (gameType.IsHumanVsHumanGame())
				{
					int num2;
					if (this.MatchesPerGameType.TryGetValue(gameType, out num2))
					{
						num += num2;
					}
				}
			}
			return num;
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
					object obj = enumerator.Current;
					GameType gameType = (GameType)obj;
					if (gameType.IsHumanVsHumanGame())
					{
						int num2;
						if (this.WinsPerGameType.TryGetValue(gameType, out num2))
						{
							num += num2;
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return num;
		}
	}

	[JsonIgnore]
	public int Losses
	{
		get
		{
			return this.Matches - this.Wins;
		}
	}

	[JsonIgnore]
	public int XPToNextLevel
	{
		get
		{
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			return gameBalanceVars.CharacterExperienceToLevel(this.Level);
		}
	}
}
