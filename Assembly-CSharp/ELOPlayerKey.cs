using System;
using System.Collections.Generic;
using System.Linq;

public class ELOPlayerKey
{
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

	private Dictionary<long, ELOPlayerKey.EloTracking> m_eloTrackings = new Dictionary<long, ELOPlayerKey.EloTracking>();

	private ELOPlayerKey()
	{
		this.m_components = new List<ELOKeyComponent>();
		this.m_components.Add(new ELOKeyComponent_Coordination());
		this.m_components.Add(new ELOKeyComponent_FinalScore());
		this.m_components.Add(new ELOKeyComponent_Softened());
		this.m_components.Add(new ELOKeyComponent_Queue());
	}

	public static uint MaxIterations
	{
		get
		{
			return ELOKeyComponent_Coordination.PhaseWidth * ELOKeyComponent_FinalScore.PhaseWidth * ELOKeyComponent_Softened.PhaseWidth * ELOKeyComponent_Queue.PhaseWidth;
		}
	}

	public static ELOPlayerKey MatchmakingEloKey
	{
		get
		{
			return ELOPlayerKey.s_matchmakingKey;
		}
	}

	public static ELOPlayerKey PublicFacingKey
	{
		get
		{
			return ELOPlayerKey.s_publicKey;
		}
	}

	public static ELOPlayerKey GeneratePlayerKey(uint iteration, GameType gameType, bool isCasual = false)
	{
		if (isCasual)
		{
			gameType = GameType.Casual;
		}
		Func<uint, ELOKeyComponent.BinaryModePhaseEnum> func = delegate(uint modulo)
		{
			if (modulo == 0U)
			{
				return ELOKeyComponent.BinaryModePhaseEnum.PRIMARY;
			}
			if (modulo == 1U)
			{
				return ELOKeyComponent.BinaryModePhaseEnum.SECONDARY;
			}
			return ELOKeyComponent.BinaryModePhaseEnum.TERTIARY;
		};
		ELOPlayerKey eloplayerKey = new ELOPlayerKey();
		eloplayerKey.CoordinationComponent.Initialize(func(iteration % ELOKeyComponent_Coordination.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Coordination.PhaseWidth;
		eloplayerKey.FinalScoreComponent.Initialize(func(iteration % ELOKeyComponent_FinalScore.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_FinalScore.PhaseWidth;
		eloplayerKey.SoftenedComponent.Initialize(func(iteration % ELOKeyComponent_Softened.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Softened.PhaseWidth;
		eloplayerKey.QueueComponent.Initialize(func(iteration % ELOKeyComponent_Queue.PhaseWidth), gameType, isCasual);
		iteration /= ELOKeyComponent_Queue.PhaseWidth;
		return eloplayerKey;
	}

	public static ELOPlayerKey GenerateGenericKeyForGameType(GameType gt)
	{
		ELOPlayerKey eloplayerKey = new ELOPlayerKey();
		eloplayerKey.Set(MatchmakingQueueConfig.EloKeyFlags.QUEUE, gt, false);
		return eloplayerKey;
	}

	public ELOKeyComponent_Coordination CoordinationComponent
	{
		get
		{
			return this.GetComponent(MatchmakingQueueConfig.EloKeyFlags.GROUP) as ELOKeyComponent_Coordination;
		}
	}

	public ELOKeyComponent_FinalScore FinalScoreComponent
	{
		get
		{
			return this.GetComponent(MatchmakingQueueConfig.EloKeyFlags.RELATIVE) as ELOKeyComponent_FinalScore;
		}
	}

	public ELOKeyComponent_Softened SoftenedComponent
	{
		get
		{
			return this.GetComponent(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC) as ELOKeyComponent_Softened;
		}
	}

	public ELOKeyComponent_Queue QueueComponent
	{
		get
		{
			return this.GetComponent(MatchmakingQueueConfig.EloKeyFlags.QUEUE) as ELOKeyComponent_Queue;
		}
	}

	private ELOKeyComponent GetComponent(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		ELOKeyComponent.BinaryModePhaseEnum binaryModePhaseEnum;
		return this.GetComponentAndPhase(flag, out binaryModePhaseEnum);
	}

	private unsafe ELOKeyComponent GetComponentAndPhase(MatchmakingQueueConfig.EloKeyFlags flag, out ELOKeyComponent.BinaryModePhaseEnum phase)
	{
		using (List<ELOKeyComponent>.Enumerator enumerator = this.m_components.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ELOKeyComponent elokeyComponent = enumerator.Current;
				if (elokeyComponent.MatchesFlag(flag))
				{
					phase = elokeyComponent.BinaryModePhase;
					return elokeyComponent;
				}
			}
		}
		throw new Exception(string.Format("Bad ELO flag {0}", flag));
	}

	private ELOPlayerKey Set(MatchmakingQueueConfig.EloKeyFlags flags, GameType gameType, bool isCasual)
	{
		ELOKeyComponent.BinaryModePhaseEnum phase;
		this.GetComponentAndPhase(flags, out phase).Initialize(phase, gameType, isCasual);
		return this;
	}

	public static ELOPlayerKey FromConfig(GameType gameType, MatchmakingQueueConfig config, bool isCasual = false)
	{
		List<MatchmakingQueueConfig.EloKeyFlags> list;
		if (config != null)
		{
			list = config.MatchmakingElo;
		}
		else
		{
			list = new List<MatchmakingQueueConfig.EloKeyFlags>();
		}
		List<MatchmakingQueueConfig.EloKeyFlags> flags = list;
		ELOPlayerKey eloplayerKey = new ELOPlayerKey();
		using (List<ELOKeyComponent>.Enumerator enumerator = eloplayerKey.m_components.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ELOKeyComponent elokeyComponent = enumerator.Current;
				elokeyComponent.Initialize(flags, gameType, isCasual);
			}
		}
		return eloplayerKey;
	}

	public float GetActualResult(Team team, GameResult gameResultAbsolute, float gameResultFraction)
	{
		return this.FinalScoreComponent.GetActualResult(team, gameResultAbsolute, gameResultFraction);
	}

	private void IncreaseTeamWeight(Team team, long accountId, float accountElo, float charElo, uint weight)
	{
		if (team == Team.TeamA)
		{
			this.teamAAccountElo += accountElo * weight;
			this.teamACharElo += charElo * weight;
			this.teamAWeight += weight;
		}
		else
		{
			if (team != Team.TeamB)
			{
				throw new Exception("Bad team");
			}
			this.teamBAccountElo += accountElo * weight;
			this.teamBCharElo += charElo * weight;
			this.teamBWeight += weight;
		}
		if (!this.m_eloTrackings.ContainsKey(accountId))
		{
			this.m_eloTrackings.Add(accountId, new ELOPlayerKey.EloTracking());
		}
		this.m_eloTrackings[accountId].accountElo += accountElo * weight;
		this.m_eloTrackings[accountId].characterElo += charElo * weight;
		this.m_eloTrackings[accountId].weight += weight;
	}

	private void RegisterPredictionScores(Team team, float accountElo, int accountMatches, float charElo, int charMatches)
	{
		if (team == Team.TeamA)
		{
			this.teamAAccountPrediction += accountElo;
			this.teamACharPrediction += charElo;
			this.teamAPredictionWeight++;
		}
		else
		{
			this.teamBAccountPrediction += accountElo;
			this.teamBCharPrediction += charElo;
			this.teamBPredictionWeight++;
		}
		if (accountMatches >= 0)
		{
			this.accountPredictionWeights.Add(accountMatches);
		}
		if (charMatches >= 0)
		{
			this.charPredictionWeights.Add(charMatches);
		}
	}

	public void InitializeHumanELO(float accountElo, int accountMatches, float charElo, int charMatches, long accountId, long groupId, byte groupSize, Team team)
	{
		this.InitializePerCharacter(groupSize);
		this.IncreaseTeamWeight(team, accountId, accountElo, charElo, 1U);
		this.m_eloTrackings[accountId].accountMatches = accountMatches;
		this.m_eloTrackings[accountId].charMatches = charMatches;
		this.m_eloTrackings[accountId].groupId = groupId;
		this.m_eloTrackings[accountId].team = team;
		this.RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
	}

	public void InitializeHumanELO(EloValues accountEloValues, EloValues charEloValues, long accountId, long groupId, byte groupSize, Team team, string alternateKey = null)
	{
		if (accountEloValues != null && charEloValues != null)
		{
			this.InitializePerCharacter(groupSize);
			float accountElo = 1500f;
			int accountMatches = 0;
			float charElo = 1500f;
			int charMatches = 0;
			string keyText = this.KeyText;
			if (alternateKey != null)
			{
				if (!accountEloValues.Values.ContainsKey(this.KeyText))
				{
					accountEloValues.GetElo(alternateKey, out accountElo, out accountMatches);
					goto IL_80;
				}
			}
			accountEloValues.GetElo(keyText, out accountElo, out accountMatches);
			IL_80:
			if (alternateKey != null)
			{
				if (!charEloValues.Values.ContainsKey(this.KeyText))
				{
					charEloValues.GetElo(alternateKey, out charElo, out charMatches);
					goto IL_CB;
				}
			}
			charEloValues.GetElo(keyText, out charElo, out charMatches);
			IL_CB:
			this.IncreaseTeamWeight(team, accountId, accountElo, charElo, 1U);
			this.m_eloTrackings[accountId].accountMatches = accountMatches;
			this.m_eloTrackings[accountId].charMatches = charMatches;
			this.m_eloTrackings[accountId].groupId = groupId;
			this.m_eloTrackings[accountId].team = team;
			this.RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
			return;
		}
		throw new Exception(string.Format("Broken ExperienceComponent for player {0}", accountId));
	}

	public void InitializeBotELO(Team team, BotDifficulty botDifficulty, Dictionary<BotDifficulty, float> botEloValues)
	{
		this.InitializePerCharacter(1);
		float accountElo = botEloValues[botDifficulty];
		float charElo = botEloValues[botDifficulty];
		int accountMatches = -1;
		int charMatches = -1;
		this.IncreaseTeamWeight(team, 0L, accountElo, charElo, 1U);
		this.RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
	}

	public float GetExpectedResult(Team team, ELOPlayerKey.ACEnum acMode)
	{
		double num = Math.Pow(10.0, this.GetNormalizedTeamElo(Team.TeamA, acMode) / 400.0);
		double num2 = Math.Pow(10.0, this.GetNormalizedTeamElo(Team.TeamB, acMode) / 400.0);
		if (team == Team.TeamA)
		{
			return (float)(num / (num + num2));
		}
		if (team == Team.TeamB)
		{
			return (float)(num2 / (num + num2));
		}
		throw new Exception("unexpected team");
	}

	private double GetNormalizedTeamElo(Team team, ELOPlayerKey.ACEnum acMode)
	{
		if (team == Team.TeamA)
		{
			if (this.teamAWeight <= 0f)
			{
				return 1500.0;
			}
			if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
			{
				return (double)(this.teamAAccountElo / this.teamAWeight);
			}
			return (double)(this.teamACharElo / this.teamAWeight);
		}
		else
		{
			if (team != Team.TeamB)
			{
				throw new Exception("bad team");
			}
			if (this.teamBWeight <= 0f)
			{
				return 1500.0;
			}
			if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
			{
				return (double)(this.teamBAccountElo / this.teamBWeight);
			}
			return (double)(this.teamBCharElo / this.teamBWeight);
		}
	}

	public float GetEloRange(ELOPlayerKey.ACEnum acMode)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (ELOPlayerKey.EloTracking eloTracking in this.m_eloTrackings.Values)
		{
			float num3 = num;
			float num4;
			if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
			{
				num4 = eloTracking.accountElo;
			}
			else
			{
				num4 = eloTracking.characterElo;
			}
			num = num3 + num4;
			num2 += eloTracking.weight;
		}
		if (num2 <= 0f)
		{
			return 1500f;
		}
		float num5 = 0f;
		float num6 = num / num2;
		using (Dictionary<long, ELOPlayerKey.EloTracking>.ValueCollection.Enumerator enumerator2 = this.m_eloTrackings.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ELOPlayerKey.EloTracking eloTracking2 = enumerator2.Current;
				float num7;
				if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
				{
					num7 = eloTracking2.accountElo;
				}
				else
				{
					num7 = eloTracking2.characterElo;
				}
				float num8 = num7 / eloTracking2.weight;
				float num9 = Math.Abs(num8 - num6) * eloTracking2.weight;
				num5 += num9;
			}
		}
		return num5 / num2;
	}

	private float GetAverageElo(ELOPlayerKey.ACEnum acMode)
	{
		float num = this.teamAWeight + this.teamBWeight;
		if (num > 0f)
		{
			float num2;
			if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
			{
				num2 = this.teamAAccountElo + this.teamBAccountElo;
			}
			else
			{
				num2 = this.teamACharElo + this.teamBCharElo;
			}
			float num3 = num2;
			return num3 / num;
		}
		return 1500f;
	}

	private float GetNormalizedPlayerElo(long accountId, ELOPlayerKey.ACEnum acMode)
	{
		ELOPlayerKey.EloTracking eloTracking;
		if (!this.m_eloTrackings.TryGetValue(accountId, out eloTracking))
		{
			throw new Exception(string.Format("failed to find elo for character {0}", accountId));
		}
		float num = (acMode != ELOPlayerKey.ACEnum.ACCOUNT) ? eloTracking.characterElo : eloTracking.accountElo;
		if (eloTracking.weight > 0f)
		{
			return num / eloTracking.weight;
		}
		return num;
	}

	private float GetPreMadeGroupRatio()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		using (Dictionary<long, ELOPlayerKey.EloTracking>.ValueCollection.Enumerator enumerator = this.m_eloTrackings.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ELOPlayerKey.EloTracking eloTracking = enumerator.Current;
				if (eloTracking.team == Team.TeamA)
				{
					num3 += 1f;
					if (eloTracking.groupId != 0L)
					{
						num += 1f;
					}
				}
				else if (eloTracking.team == Team.TeamB)
				{
					num4 += 1f;
					if (eloTracking.groupId != 0L)
					{
						num2 += 1f;
					}
				}
			}
		}
		num = Math.Max(num, 1f);
		num2 = Math.Max(num2, 1f);
		float num5;
		if (num > num2)
		{
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

	private int GetMatchCount(long accountId, ELOPlayerKey.ACEnum acMode)
	{
		ELOPlayerKey.EloTracking eloTracking;
		if (this.m_eloTrackings.TryGetValue(accountId, out eloTracking))
		{
			int result;
			if (acMode == ELOPlayerKey.ACEnum.ACCOUNT)
			{
				result = eloTracking.accountMatches;
			}
			else
			{
				result = eloTracking.charMatches;
			}
			return result;
		}
		return 0x3E8;
	}

	public unsafe void GetKFactor(long accountId, ELOPlayerKey.ACEnum acMode, float eloRange, float placementKFactor, int highKDuration, bool won, out float KFactor, out float MaxDelta)
	{
		int matchCount = this.GetMatchCount(accountId, acMode);
		float normalizedPlayerElo = this.GetNormalizedPlayerElo(accountId, acMode);
		float averageElo = this.GetAverageElo(acMode);
		float num;
		if (matchCount >= highKDuration)
		{
			if (placementKFactor > 0f)
			{
				num = placementKFactor;
				goto IL_52;
			}
		}
		num = 0f;
		IL_52:
		float placementKFactor2 = num;
		float preMadeGroupRatio = this.GetPreMadeGroupRatio();
		MaxDelta = this.SoftenedComponent.GetMaxDelta(eloRange, preMadeGroupRatio, placementKFactor2, won, normalizedPlayerElo, averageElo);
		KFactor = MaxDelta;
		if (this.FinalScoreComponent.IsRelative())
		{
			KFactor *= 3f;
		}
	}

	public void InitializePerCharacter(byte groupSize)
	{
		this.CoordinationComponent.InitializePerCharacter(groupSize);
	}

	public unsafe bool CalculateNewEloValue(long accountId, float actualResult, float expectedResult, float eloRange, float placementKFactor, float placementMaxElo, ELOPlayerKey.ACEnum acEnum, int highKDuration, out float newElo)
	{
		bool result = false;
		bool won = actualResult > 0.5f;
		float num;
		float num2;
		this.GetKFactor(accountId, acEnum, eloRange, placementKFactor, highKDuration, won, out num, out num2);
		if (num > 0f)
		{
			float normalizedPlayerElo = this.GetNormalizedPlayerElo(accountId, acEnum);
			float num3 = num * (actualResult - expectedResult);
			if (num3 > num2)
			{
				num3 = num2;
			}
			else if (num3 < 0f - num2)
			{
				num3 = 0f - num2;
			}
			newElo = normalizedPlayerElo + num3;
			newElo = Math.Max(1f, newElo);
			if (newElo > placementMaxElo)
			{
				if (this.GetMatchCount(accountId, acEnum) < highKDuration)
				{
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

	public unsafe void GeneratePreMatchPrediction(Team winningTeam, ELOPlayerKey.ACEnum acEnum, out float prediction, out int minWeight)
	{
		if (acEnum == ELOPlayerKey.ACEnum.ACCOUNT)
		{
			float num;
			if (this.teamAPredictionWeight > 0)
			{
				num = this.teamAAccountPrediction / (float)this.teamAPredictionWeight;
			}
			else
			{
				num = 1500f;
			}
			float num2 = num;
			double num3 = Math.Pow(10.0, (double)(num2 / 400f));
			float num4;
			if (this.teamBPredictionWeight > 0)
			{
				num4 = this.teamBAccountPrediction / (float)this.teamBPredictionWeight;
			}
			else
			{
				num4 = 1500f;
			}
			float num5 = num4;
			double num6 = Math.Pow(10.0, (double)(num5 / 400f));
			if (winningTeam == Team.TeamA)
			{
				prediction = (float)(num3 / (num3 + num6));
			}
			else
			{
				prediction = (float)(num6 / (num3 + num6));
			}
			int count = this.accountPredictionWeights.Count;
			if (count > 0)
			{
				this.accountPredictionWeights.Sort();
				minWeight = this.accountPredictionWeights.ElementAt(0);
			}
			else
			{
				minWeight = 0;
			}
		}
		else
		{
			float num7;
			if (this.teamAPredictionWeight > 0)
			{
				num7 = this.teamACharPrediction / (float)this.teamAPredictionWeight;
			}
			else
			{
				num7 = 1500f;
			}
			float num8 = num7;
			double num9 = Math.Pow(10.0, (double)(num8 / 400f));
			float num10;
			if (this.teamBPredictionWeight > 0)
			{
				num10 = this.teamBCharPrediction / (float)this.teamBPredictionWeight;
			}
			else
			{
				num10 = 1500f;
			}
			float num11 = num10;
			double num12 = Math.Pow(10.0, (double)(num11 / 400f));
			if (winningTeam == Team.TeamA)
			{
				prediction = (float)(num9 / (num9 + num12));
			}
			else
			{
				prediction = (float)(num12 / (num9 + num12));
			}
			int count2 = this.charPredictionWeights.Count;
			if (count2 > 0)
			{
				this.charPredictionWeights.Sort();
				minWeight = this.charPredictionWeights.ElementAt(0);
			}
			else
			{
				minWeight = 0;
			}
		}
	}

	public string KeyText
	{
		get
		{
			string text = string.Empty;
			foreach (ELOKeyComponent elokeyComponent in this.m_components)
			{
				text += elokeyComponent.GetComponentChar();
			}
			return text;
		}
	}

	public string LogPhaseConcatination
	{
		get
		{
			string text = string.Empty;
			foreach (ELOKeyComponent elokeyComponent in this.m_components)
			{
				text += elokeyComponent.GetPhaseChar();
			}
			return text;
		}
	}

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
}
