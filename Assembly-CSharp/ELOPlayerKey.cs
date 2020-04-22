using System;
using System.Collections.Generic;
using System.Linq;

public class ELOPlayerKey
{
	private class EloTracking
	{
		public float accountElo;

		public float characterElo;

		public float weight;

		public int accountMatches;

		public int charMatches;

		public long groupId;

		public Team team = Team.Invalid;
	}

	public enum ACEnum
	{
		ACCOUNT,
		CHARACTER
	}

	private static ELOPlayerKey s_matchmakingKey = new ELOPlayerKey();

	private static ELOPlayerKey s_publicKey = new ELOPlayerKey().Set(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC, GameType.PvP, false);

	private List<ELOKeyComponent> m_components = new List<ELOKeyComponent>();

	private float teamAAccountElo;

	private float teamBAccountElo;

	private float teamACharElo;

	private float teamBCharElo;

	private float teamAWeight;

	private float teamBWeight;

	private float teamAAccountPrediction;

	private float teamBAccountPrediction;

	private int teamAPredictionWeight;

	private int teamBPredictionWeight;

	private float teamACharPrediction;

	private float teamBCharPrediction;

	private List<int> accountPredictionWeights = new List<int>();

	private List<int> charPredictionWeights = new List<int>();

	private Dictionary<long, EloTracking> m_eloTrackings = new Dictionary<long, EloTracking>();

	public static uint MaxIterations => ELOKeyComponent_Coordination.PhaseWidth * ELOKeyComponent_FinalScore.PhaseWidth * ELOKeyComponent_Softened.PhaseWidth * ELOKeyComponent_Queue.PhaseWidth;

	public static ELOPlayerKey MatchmakingEloKey => s_matchmakingKey;

	public static ELOPlayerKey PublicFacingKey => s_publicKey;

	public ELOKeyComponent_Coordination CoordinationComponent => GetComponent(MatchmakingQueueConfig.EloKeyFlags.GROUP) as ELOKeyComponent_Coordination;

	public ELOKeyComponent_FinalScore FinalScoreComponent => GetComponent(MatchmakingQueueConfig.EloKeyFlags.RELATIVE) as ELOKeyComponent_FinalScore;

	public ELOKeyComponent_Softened SoftenedComponent => GetComponent(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC) as ELOKeyComponent_Softened;

	public ELOKeyComponent_Queue QueueComponent => GetComponent(MatchmakingQueueConfig.EloKeyFlags.QUEUE) as ELOKeyComponent_Queue;

	public string KeyText
	{
		get
		{
			string text = string.Empty;
			foreach (ELOKeyComponent component in m_components)
			{
				text += component.GetComponentChar();
			}
			return text;
		}
	}

	public string LogPhaseConcatination
	{
		get
		{
			string text = string.Empty;
			foreach (ELOKeyComponent component in m_components)
			{
				text += component.GetPhaseChar();
			}
			return text;
		}
	}

	private ELOPlayerKey()
	{
		m_components = new List<ELOKeyComponent>();
		m_components.Add(new ELOKeyComponent_Coordination());
		m_components.Add(new ELOKeyComponent_FinalScore());
		m_components.Add(new ELOKeyComponent_Softened());
		m_components.Add(new ELOKeyComponent_Queue());
	}

	public static ELOPlayerKey GeneratePlayerKey(uint iteration, GameType gameType, bool isCasual = false)
	{
		if (isCasual)
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
			gameType = GameType.Casual;
		}
		Func<uint, ELOKeyComponent.BinaryModePhaseEnum> func = delegate(uint modulo)
		{
			switch (modulo)
			{
			case 0u:
				return ELOKeyComponent.BinaryModePhaseEnum.PRIMARY;
			case 1u:
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return ELOKeyComponent.BinaryModePhaseEnum.SECONDARY;
					}
				}
			default:
				return ELOKeyComponent.BinaryModePhaseEnum.TERTIARY;
			}
		};
		ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
		eLOPlayerKey.CoordinationComponent.Initialize(func(iteration % ELOKeyComponent_Coordination.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Coordination.PhaseWidth;
		eLOPlayerKey.FinalScoreComponent.Initialize(func(iteration % ELOKeyComponent_FinalScore.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_FinalScore.PhaseWidth;
		eLOPlayerKey.SoftenedComponent.Initialize(func(iteration % ELOKeyComponent_Softened.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Softened.PhaseWidth;
		eLOPlayerKey.QueueComponent.Initialize(func(iteration % ELOKeyComponent_Queue.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Queue.PhaseWidth;
		return eLOPlayerKey;
	}

	public static ELOPlayerKey GenerateGenericKeyForGameType(GameType gt)
	{
		ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
		eLOPlayerKey.Set(MatchmakingQueueConfig.EloKeyFlags.QUEUE, gt, false);
		return eLOPlayerKey;
	}

	private ELOKeyComponent GetComponent(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		ELOKeyComponent.BinaryModePhaseEnum phase;
		return GetComponentAndPhase(flag, out phase);
	}

	private ELOKeyComponent GetComponentAndPhase(MatchmakingQueueConfig.EloKeyFlags flag, out ELOKeyComponent.BinaryModePhaseEnum phase)
	{
		using (List<ELOKeyComponent>.Enumerator enumerator = m_components.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ELOKeyComponent current = enumerator.Current;
				if (current.MatchesFlag(flag))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							phase = current.BinaryModePhase;
							return current;
						}
					}
				}
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
		throw new Exception($"Bad ELO flag {flag}");
	}

	private ELOPlayerKey Set(MatchmakingQueueConfig.EloKeyFlags flags, GameType gameType, bool isCasual)
	{
		GetComponentAndPhase(flags, out ELOKeyComponent.BinaryModePhaseEnum phase).Initialize(phase, gameType, isCasual);
		return this;
	}

	public static ELOPlayerKey FromConfig(GameType gameType, MatchmakingQueueConfig config, bool isCasual = false)
	{
		List<MatchmakingQueueConfig.EloKeyFlags> list;
		if (config != null)
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
			list = config.MatchmakingElo;
		}
		else
		{
			list = new List<MatchmakingQueueConfig.EloKeyFlags>();
		}
		List<MatchmakingQueueConfig.EloKeyFlags> flags = list;
		ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
		using (List<ELOKeyComponent>.Enumerator enumerator = eLOPlayerKey.m_components.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ELOKeyComponent current = enumerator.Current;
				current.Initialize(flags, gameType, isCasual);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return eLOPlayerKey;
				}
			}
		}
	}

	public float GetActualResult(Team team, GameResult gameResultAbsolute, float gameResultFraction)
	{
		return FinalScoreComponent.GetActualResult(team, gameResultAbsolute, gameResultFraction);
	}

	private void IncreaseTeamWeight(Team team, long accountId, float accountElo, float charElo, uint weight)
	{
		switch (team)
		{
		case Team.TeamA:
			teamAAccountElo += accountElo * (float)weight;
			teamACharElo += charElo * (float)weight;
			teamAWeight += weight;
			break;
		case Team.TeamB:
			teamBAccountElo += accountElo * (float)weight;
			teamBCharElo += charElo * (float)weight;
			teamBWeight += weight;
			break;
		default:
			throw new Exception("Bad team");
		}
		if (!m_eloTrackings.ContainsKey(accountId))
		{
			m_eloTrackings.Add(accountId, new EloTracking());
		}
		m_eloTrackings[accountId].accountElo += accountElo * (float)weight;
		m_eloTrackings[accountId].characterElo += charElo * (float)weight;
		m_eloTrackings[accountId].weight += weight;
	}

	private void RegisterPredictionScores(Team team, float accountElo, int accountMatches, float charElo, int charMatches)
	{
		if (team == Team.TeamA)
		{
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
			teamAAccountPrediction += accountElo;
			teamACharPrediction += charElo;
			teamAPredictionWeight++;
		}
		else
		{
			teamBAccountPrediction += accountElo;
			teamBCharPrediction += charElo;
			teamBPredictionWeight++;
		}
		if (accountMatches >= 0)
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
			accountPredictionWeights.Add(accountMatches);
		}
		if (charMatches < 0)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			charPredictionWeights.Add(charMatches);
			return;
		}
	}

	public void InitializeHumanELO(float accountElo, int accountMatches, float charElo, int charMatches, long accountId, long groupId, byte groupSize, Team team)
	{
		InitializePerCharacter(groupSize);
		IncreaseTeamWeight(team, accountId, accountElo, charElo, 1u);
		m_eloTrackings[accountId].accountMatches = accountMatches;
		m_eloTrackings[accountId].charMatches = charMatches;
		m_eloTrackings[accountId].groupId = groupId;
		m_eloTrackings[accountId].team = team;
		RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
	}

	public void InitializeHumanELO(EloValues accountEloValues, EloValues charEloValues, long accountId, long groupId, byte groupSize, Team team, string alternateKey = null)
	{
		if (accountEloValues != null && charEloValues != null)
		{
			while (true)
			{
				float elo;
				int count;
				float elo2;
				int count2;
				string keyText;
				switch (6)
				{
				case 0:
					break;
				default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						InitializePerCharacter(groupSize);
						elo = 1500f;
						count = 0;
						elo2 = 1500f;
						count2 = 0;
						keyText = KeyText;
						if (alternateKey != null)
						{
							if (!accountEloValues.Values.ContainsKey(KeyText))
							{
								accountEloValues.GetElo(alternateKey, out elo, out count);
								goto IL_0080;
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
						accountEloValues.GetElo(keyText, out elo, out count);
						goto IL_0080;
					}
					IL_0080:
					if (alternateKey != null)
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
						if (!charEloValues.Values.ContainsKey(KeyText))
						{
							charEloValues.GetElo(alternateKey, out elo2, out count2);
							goto IL_00cb;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					charEloValues.GetElo(keyText, out elo2, out count2);
					goto IL_00cb;
					IL_00cb:
					IncreaseTeamWeight(team, accountId, elo, elo2, 1u);
					m_eloTrackings[accountId].accountMatches = count;
					m_eloTrackings[accountId].charMatches = count2;
					m_eloTrackings[accountId].groupId = groupId;
					m_eloTrackings[accountId].team = team;
					RegisterPredictionScores(team, elo, count, elo2, count2);
					return;
				}
			}
		}
		throw new Exception($"Broken ExperienceComponent for player {accountId}");
	}

	public void InitializeBotELO(Team team, BotDifficulty botDifficulty, Dictionary<BotDifficulty, float> botEloValues)
	{
		InitializePerCharacter(1);
		float accountElo = botEloValues[botDifficulty];
		float charElo = botEloValues[botDifficulty];
		int accountMatches = -1;
		int charMatches = -1;
		IncreaseTeamWeight(team, 0L, accountElo, charElo, 1u);
		RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
	}

	public float GetExpectedResult(Team team, ACEnum acMode)
	{
		double num = Math.Pow(10.0, GetNormalizedTeamElo(Team.TeamA, acMode) / 400.0);
		double num2 = Math.Pow(10.0, GetNormalizedTeamElo(Team.TeamB, acMode) / 400.0);
		switch (team)
		{
		case Team.TeamA:
			return (float)(num / (num + num2));
		case Team.TeamB:
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return (float)(num2 / (num + num2));
			}
		default:
			throw new Exception("unexpected team");
		}
	}

	private double GetNormalizedTeamElo(Team team, ACEnum acMode)
	{
		switch (team)
		{
		case Team.TeamA:
			if (teamAWeight > 0f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (acMode == ACEnum.ACCOUNT)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return teamAAccountElo / teamAWeight;
								}
							}
						}
						return teamACharElo / teamAWeight;
					}
				}
			}
			return 1500.0;
		case Team.TeamB:
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (teamBWeight > 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							if (acMode == ACEnum.ACCOUNT)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										return teamBAccountElo / teamBWeight;
									}
								}
							}
							return teamBCharElo / teamBWeight;
						}
					}
				}
				return 1500.0;
			}
		default:
			throw new Exception("bad team");
		}
	}

	public float GetEloRange(ACEnum acMode)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (EloTracking value in m_eloTrackings.Values)
		{
			float num3 = num;
			float num4;
			if (acMode == ACEnum.ACCOUNT)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num4 = value.accountElo;
			}
			else
			{
				num4 = value.characterElo;
			}
			num = num3 + num4;
			num2 += value.weight;
		}
		if (num2 <= 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 1500f;
				}
			}
		}
		float num5 = 0f;
		float num6 = num / num2;
		using (Dictionary<long, EloTracking>.ValueCollection.Enumerator enumerator2 = m_eloTrackings.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EloTracking current2 = enumerator2.Current;
				float num7;
				if (acMode == ACEnum.ACCOUNT)
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
					num7 = current2.accountElo;
				}
				else
				{
					num7 = current2.characterElo;
				}
				float num8 = num7 / current2.weight;
				float num9 = Math.Abs(num8 - num6) * current2.weight;
				num5 += num9;
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
		}
		return num5 / num2;
	}

	private float GetAverageElo(ACEnum acMode)
	{
		float num = teamAWeight + teamBWeight;
		if (num > 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					float num2;
					if (acMode == ACEnum.ACCOUNT)
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
						num2 = teamAAccountElo + teamBAccountElo;
					}
					else
					{
						num2 = teamACharElo + teamBCharElo;
					}
					float num3 = num2;
					return num3 / num;
				}
				}
			}
		}
		return 1500f;
	}

	private float GetNormalizedPlayerElo(long accountId, ACEnum acMode)
	{
		if (m_eloTrackings.TryGetValue(accountId, out EloTracking value))
		{
			float num = (acMode != 0) ? value.characterElo : value.accountElo;
			if (value.weight > 0f)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return num / value.weight;
					}
				}
			}
			return num;
		}
		throw new Exception($"failed to find elo for character {accountId}");
	}

	private float GetPreMadeGroupRatio()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		using (Dictionary<long, EloTracking>.ValueCollection.Enumerator enumerator = m_eloTrackings.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EloTracking current = enumerator.Current;
				if (current.team == Team.TeamA)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num3 += 1f;
					if (current.groupId != 0)
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
						num += 1f;
					}
				}
				else if (current.team == Team.TeamB)
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
					num4 += 1f;
					if (current.groupId != 0)
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
						num2 += 1f;
					}
				}
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
		num = Math.Max(num, 1f);
		num2 = Math.Max(num2, 1f);
		float num5;
		if (num > num2)
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
			num5 = num2 / num;
		}
		else
		{
			num5 = num / num2;
		}
		float num6 = num5;
		float num7 = Math.Max(num3, num4);
		float num8 = Math.Max(num, num2);
		return (num8 * num6 + (num7 - num8)) / num7;
	}

	private int GetMatchCount(long accountId, ACEnum acMode)
	{
		if (m_eloTrackings.TryGetValue(accountId, out EloTracking value))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int result;
					if (acMode == ACEnum.ACCOUNT)
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
						result = value.accountMatches;
					}
					else
					{
						result = value.charMatches;
					}
					return result;
				}
				}
			}
		}
		return 1000;
	}

	public void GetKFactor(long accountId, ACEnum acMode, float eloRange, float placementKFactor, int highKDuration, bool won, out float KFactor, out float MaxDelta)
	{
		int matchCount = GetMatchCount(accountId, acMode);
		float normalizedPlayerElo = GetNormalizedPlayerElo(accountId, acMode);
		float averageElo = GetAverageElo(acMode);
		float num;
		if (matchCount >= highKDuration)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(placementKFactor <= 0f))
			{
				num = placementKFactor;
				goto IL_0052;
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
		num = 0f;
		goto IL_0052;
		IL_0052:
		float placementKFactor2 = num;
		float preMadeGroupRatio = GetPreMadeGroupRatio();
		MaxDelta = SoftenedComponent.GetMaxDelta(eloRange, preMadeGroupRatio, placementKFactor2, won, normalizedPlayerElo, averageElo);
		KFactor = MaxDelta;
		if (FinalScoreComponent.IsRelative())
		{
			KFactor *= 3f;
		}
	}

	public void InitializePerCharacter(byte groupSize)
	{
		CoordinationComponent.InitializePerCharacter(groupSize);
	}

	public bool CalculateNewEloValue(long accountId, float actualResult, float expectedResult, float eloRange, float placementKFactor, float placementMaxElo, ACEnum acEnum, int highKDuration, out float newElo)
	{
		bool result = false;
		bool won = actualResult > 0.5f;
		GetKFactor(accountId, acEnum, eloRange, placementKFactor, highKDuration, won, out float KFactor, out float MaxDelta);
		if (KFactor > 0f)
		{
			float normalizedPlayerElo = GetNormalizedPlayerElo(accountId, acEnum);
			float num = KFactor * (actualResult - expectedResult);
			if (num > MaxDelta)
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
				num = MaxDelta;
			}
			else if (num < 0f - MaxDelta)
			{
				num = 0f - MaxDelta;
			}
			newElo = normalizedPlayerElo + num;
			newElo = Math.Max(1f, newElo);
			if (newElo > placementMaxElo)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GetMatchCount(accountId, acEnum) < highKDuration)
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
					newElo = placementMaxElo;
				}
			}
			result = true;
		}
		else
		{
			newElo = 1500f;
		}
		return result;
	}

	public void GeneratePreMatchPrediction(Team winningTeam, ACEnum acEnum, out float prediction, out int minWeight)
	{
		if (acEnum == ACEnum.ACCOUNT)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					float num;
					if (teamAPredictionWeight > 0)
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
						num = teamAAccountPrediction / (float)teamAPredictionWeight;
					}
					else
					{
						num = 1500f;
					}
					float num2 = num;
					double num3 = Math.Pow(10.0, num2 / 400f);
					float num4;
					if (teamBPredictionWeight > 0)
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
						num4 = teamBAccountPrediction / (float)teamBPredictionWeight;
					}
					else
					{
						num4 = 1500f;
					}
					float num5 = num4;
					double num6 = Math.Pow(10.0, num5 / 400f);
					if (winningTeam == Team.TeamA)
					{
						prediction = (float)(num3 / (num3 + num6));
					}
					else
					{
						prediction = (float)(num6 / (num3 + num6));
					}
					int count = accountPredictionWeights.Count;
					if (count > 0)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								accountPredictionWeights.Sort();
								minWeight = accountPredictionWeights.ElementAt(0);
								return;
							}
						}
					}
					minWeight = 0;
					return;
				}
				}
			}
		}
		float num7;
		if (teamAPredictionWeight > 0)
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
			num7 = teamACharPrediction / (float)teamAPredictionWeight;
		}
		else
		{
			num7 = 1500f;
		}
		float num8 = num7;
		double num9 = Math.Pow(10.0, num8 / 400f);
		float num10;
		if (teamBPredictionWeight > 0)
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
			num10 = teamBCharPrediction / (float)teamBPredictionWeight;
		}
		else
		{
			num10 = 1500f;
		}
		float num11 = num10;
		double num12 = Math.Pow(10.0, num11 / 400f);
		if (winningTeam == Team.TeamA)
		{
			prediction = (float)(num9 / (num9 + num12));
		}
		else
		{
			prediction = (float)(num12 / (num9 + num12));
		}
		int count2 = charPredictionWeights.Count;
		if (count2 > 0)
		{
			charPredictionWeights.Sort();
			minWeight = charPredictionWeights.ElementAt(0);
		}
		else
		{
			minWeight = 0;
		}
	}
}
