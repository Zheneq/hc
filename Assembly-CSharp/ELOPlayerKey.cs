using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    private static ELOPlayerKey s_publicKey =
        new ELOPlayerKey().Set(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC, GameType.PvP, false);

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

    public static uint MaxIterations
    {
        get
        {
            return ELOKeyComponent_Coordination.PhaseWidth
                   * ELOKeyComponent_FinalScore.PhaseWidth
                   * ELOKeyComponent_Softened.PhaseWidth
                   * ELOKeyComponent_Queue.PhaseWidth;
        }
    }

    public static ELOPlayerKey MatchmakingEloKey
    {
        get { return s_matchmakingKey; }
    }

    public static ELOPlayerKey PublicFacingKey
    {
        get { return s_publicKey; }
    }

    public ELOKeyComponent_Coordination CoordinationComponent
    {
        get { return GetComponent(MatchmakingQueueConfig.EloKeyFlags.GROUP) as ELOKeyComponent_Coordination; }
    }

    public ELOKeyComponent_FinalScore FinalScoreComponent
    {
        get { return GetComponent(MatchmakingQueueConfig.EloKeyFlags.RELATIVE) as ELOKeyComponent_FinalScore; }
    }

    public ELOKeyComponent_Softened SoftenedComponent
    {
        get { return GetComponent(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC) as ELOKeyComponent_Softened; }
    }

    public ELOKeyComponent_Queue QueueComponent
    {
        get { return GetComponent(MatchmakingQueueConfig.EloKeyFlags.QUEUE) as ELOKeyComponent_Queue; }
    }

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
        m_components = new List<ELOKeyComponent>
        {
            new ELOKeyComponent_Coordination(),
            new ELOKeyComponent_FinalScore(),
            new ELOKeyComponent_Softened(),
            new ELOKeyComponent_Queue()
        };
    }

    public static ELOPlayerKey GeneratePlayerKey(uint iteration, GameType gameType, bool isCasual = false)
    {
        if (isCasual)
        {
            gameType = GameType.Casual;
        }

        ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
        eLOPlayerKey.CoordinationComponent.Initialize(Func(iteration % ELOKeyComponent_Coordination.PhaseWidth),
            gameType, isCasual);
        iteration /= ELOKeyComponent_Coordination.PhaseWidth;
        eLOPlayerKey.FinalScoreComponent.Initialize(Func(iteration % ELOKeyComponent_FinalScore.PhaseWidth), gameType,
            isCasual);
        iteration /= ELOKeyComponent_FinalScore.PhaseWidth;
        eLOPlayerKey.SoftenedComponent.Initialize(Func(iteration % ELOKeyComponent_Softened.PhaseWidth), gameType,
            isCasual);
        iteration /= ELOKeyComponent_Softened.PhaseWidth;
        eLOPlayerKey.QueueComponent.Initialize(Func(iteration % ELOKeyComponent_Queue.PhaseWidth), gameType, isCasual);
        iteration /= ELOKeyComponent_Queue.PhaseWidth;
        return eLOPlayerKey;
    }

    private static ELOKeyComponent.BinaryModePhaseEnum Func(uint modulo)
    {
        switch (modulo)
        {
            case 0u:
                return ELOKeyComponent.BinaryModePhaseEnum.PRIMARY;
            case 1u:
                return ELOKeyComponent.BinaryModePhaseEnum.SECONDARY;
            default:
                return ELOKeyComponent.BinaryModePhaseEnum.TERTIARY;
        }
    }

    public static ELOPlayerKey GenerateGenericKeyForGameType(GameType gt)
    {
        ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
        eLOPlayerKey.Set(MatchmakingQueueConfig.EloKeyFlags.QUEUE, gt, false);
        return eLOPlayerKey;
    }

    private ELOKeyComponent GetComponent(MatchmakingQueueConfig.EloKeyFlags flag)
    {
        ELOKeyComponent.BinaryModePhaseEnum foo;
        return GetComponentAndPhase(flag, out foo);
    }

    private ELOKeyComponent GetComponentAndPhase(
        MatchmakingQueueConfig.EloKeyFlags flag,
        out ELOKeyComponent.BinaryModePhaseEnum phase)
    {
        foreach (ELOKeyComponent comp in m_components)
        {
            if (comp.MatchesFlag(flag))
            {
                phase = comp.BinaryModePhase;
                return comp;
            }
        }

        throw new Exception(new StringBuilder().Append("Bad ELO flag ").Append(flag).ToString());
    }

    private ELOPlayerKey Set(MatchmakingQueueConfig.EloKeyFlags flags, GameType gameType, bool isCasual)
    {
        ELOKeyComponent.BinaryModePhaseEnum phase;
        GetComponentAndPhase(flags, out phase)
            .Initialize(phase, gameType, isCasual);
        return this;
    }

    public static ELOPlayerKey FromConfig(GameType gameType, MatchmakingQueueConfig config, bool isCasual = false)
    {
        List<MatchmakingQueueConfig.EloKeyFlags> flags = config != null
            ? config.MatchmakingElo
            : new List<MatchmakingQueueConfig.EloKeyFlags>();
        ELOPlayerKey eLOPlayerKey = new ELOPlayerKey();
        foreach (ELOKeyComponent comp in eLOPlayerKey.m_components)
        {
            comp.Initialize(flags, gameType, isCasual);
        }

        return eLOPlayerKey;
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
                teamAAccountElo += accountElo * weight;
                teamACharElo += charElo * weight;
                teamAWeight += weight;
                break;
            case Team.TeamB:
                teamBAccountElo += accountElo * weight;
                teamBCharElo += charElo * weight;
                teamBWeight += weight;
                break;
            default:
                throw new Exception("Bad team");
        }

        if (!m_eloTrackings.ContainsKey(accountId))
        {
            m_eloTrackings.Add(accountId, new EloTracking());
        }

        m_eloTrackings[accountId].accountElo += accountElo * weight;
        m_eloTrackings[accountId].characterElo += charElo * weight;
        m_eloTrackings[accountId].weight += weight;
    }

    private void RegisterPredictionScores(Team team, float accountElo, int accountMatches, float charElo,
        int charMatches)
    {
        if (team == Team.TeamA)
        {
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
            accountPredictionWeights.Add(accountMatches);
        }

        if (charMatches >= 0)
        {
            charPredictionWeights.Add(charMatches);
        }
    }

    public void InitializeHumanELO(
        float accountElo,
        int accountMatches,
        float charElo,
        int charMatches,
        long accountId,
        long groupId,
        byte groupSize,
        Team team)
    {
        InitializePerCharacter(groupSize);
        IncreaseTeamWeight(team, accountId, accountElo, charElo, 1u);
        m_eloTrackings[accountId].accountMatches = accountMatches;
        m_eloTrackings[accountId].charMatches = charMatches;
        m_eloTrackings[accountId].groupId = groupId;
        m_eloTrackings[accountId].team = team;
        RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
    }

    public void InitializeHumanELO(
        EloValues accountEloValues,
        EloValues charEloValues,
        long accountId,
        long groupId,
        byte groupSize,
        Team team,
        string alternateKey = null)
    {
        if (accountEloValues == null || charEloValues == null)
        {
            throw new Exception(new StringBuilder().Append("Broken ExperienceComponent for player ").Append(accountId).ToString());
        }

        InitializePerCharacter(groupSize);
        float accountElo = 1500f;
        int accountMatches = 0;
        float charElo = 1500f;
        int charMatches = 0;
        string keyText = KeyText;
        if (alternateKey != null && !accountEloValues.Values.ContainsKey(KeyText))
        {
            accountEloValues.GetElo(alternateKey, out accountElo, out accountMatches);
        }
        else
        {
            accountEloValues.GetElo(keyText, out accountElo, out accountMatches);
        }

        if (alternateKey != null && !charEloValues.Values.ContainsKey(KeyText))
        {
            charEloValues.GetElo(alternateKey, out charElo, out charMatches);
        }
        else
        {
            charEloValues.GetElo(keyText, out charElo, out charMatches);
        }

        IncreaseTeamWeight(team, accountId, accountElo, charElo, 1u);
        m_eloTrackings[accountId].accountMatches = accountMatches;
        m_eloTrackings[accountId].charMatches = charMatches;
        m_eloTrackings[accountId].groupId = groupId;
        m_eloTrackings[accountId].team = team;
        RegisterPredictionScores(team, accountElo, accountMatches, charElo, charMatches);
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
                return (float)(num2 / (num + num2));
            default:
                throw new Exception("unexpected team");
        }
    }

    private double GetNormalizedTeamElo(Team team, ACEnum acMode)
    {
        switch (team)
        {
            case Team.TeamA:
                return teamAWeight <= 0f
                    ? 1500.0
                    : acMode == ACEnum.ACCOUNT
                        ? teamAAccountElo / teamAWeight
                        : teamACharElo / teamAWeight;
            case Team.TeamB:
                return teamBWeight <= 0f
                    ? 1500.0
                    : acMode == ACEnum.ACCOUNT
                        ? teamBAccountElo / teamBWeight
                        : teamBCharElo / teamBWeight;
            default:
                throw new Exception("bad team");
        }
    }

    public float GetEloRange(ACEnum acMode)
    {
        float totalElo = 0f;
        float totalWeight = 0f;
        foreach (EloTracking value in m_eloTrackings.Values)
        {
            float elo = acMode == ACEnum.ACCOUNT
                ? value.accountElo
                : value.characterElo;
            totalElo += elo;
            totalWeight += value.weight;
        }

        if (totalWeight <= 0f)
        {
            return 1500f;
        }

        float totalRange = 0f;
        float avgElo = totalElo / totalWeight;
        foreach (EloTracking value in m_eloTrackings.Values)
        {
            float elo = acMode == ACEnum.ACCOUNT
                ? value.accountElo
                : value.characterElo;
            float eloContribution = elo / value.weight;
            float range = Math.Abs(eloContribution - avgElo) * value.weight;
            totalRange += range;
        }

        return totalRange / totalWeight;
    }

    private float GetAverageElo(ACEnum acMode)
    {
        float totalWeight = teamAWeight + teamBWeight;
        if (totalWeight > 0f)
        {
            float totalElo = acMode == ACEnum.ACCOUNT
                ? teamAAccountElo + teamBAccountElo
                : teamACharElo + teamBCharElo;
            return totalElo / totalWeight;
        }

        return 1500f;
    }

    private float GetNormalizedPlayerElo(long accountId, ACEnum acMode)
    {
        EloTracking value;
        if (!m_eloTrackings.TryGetValue(accountId, out value))
        {
            throw new Exception(new StringBuilder().Append("failed to find elo for character ").Append(accountId).ToString());
        }

        float elo = acMode != ACEnum.ACCOUNT
            ? value.characterElo
            : value.accountElo;
        if (value.weight > 0f)
        {
            return elo / value.weight;
        }

        return elo;
    }

    private float GetPreMadeGroupRatio()
    {
        float teamAGroupedNum = 0f;
        float teamBGroupedNum = 0f;
        float teamANum = 0f;
        float teamBNum = 0f;
        foreach (EloTracking value in m_eloTrackings.Values)
        {
            if (value.team == Team.TeamA)
            {
                teamANum += 1f;
                if (value.groupId != 0)
                {
                    teamAGroupedNum += 1f;
                }
            }
            else if (value.team == Team.TeamB)
            {
                teamBNum += 1f;
                if (value.groupId != 0)
                {
                    teamBGroupedNum += 1f;
                }
            }
        }

        teamAGroupedNum = Math.Max(teamAGroupedNum, 1f);
        teamBGroupedNum = Math.Max(teamBGroupedNum, 1f);
        float ratio = teamAGroupedNum > teamBGroupedNum
            ? teamBGroupedNum / teamAGroupedNum
            : teamAGroupedNum / teamBGroupedNum;
        float maxNum = Math.Max(teamANum, teamBNum);
        float maxGroupedNum = Math.Max(teamAGroupedNum, teamBGroupedNum);
        return (maxGroupedNum * ratio + (maxNum - maxGroupedNum)) / maxNum;
    }

    private int GetMatchCount(long accountId, ACEnum acMode)
    {
        EloTracking value;
        if (m_eloTrackings.TryGetValue(accountId, out value))
        {
            return acMode == ACEnum.ACCOUNT
                ? value.accountMatches
                : value.charMatches;
        }

        return 1000;
    }

    public void GetKFactor(
        long accountId,
        ACEnum acMode,
        float eloRange,
        float placementKFactor,
        int highKDuration,
        bool won,
        out float KFactor,
        out float MaxDelta)
    {
        int matchCount = GetMatchCount(accountId, acMode);
        float normalizedPlayerElo = GetNormalizedPlayerElo(accountId, acMode);
        float averageElo = GetAverageElo(acMode);
        float placementKFactor2 = matchCount >= highKDuration && placementKFactor > 0f
            ? placementKFactor
            : 0f;
        float preMadeGroupRatio = GetPreMadeGroupRatio();
        MaxDelta = SoftenedComponent.GetMaxDelta(
            eloRange,
            preMadeGroupRatio,
            placementKFactor2,
            won,
            normalizedPlayerElo,
            averageElo);
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

    public bool CalculateNewEloValue(
        long accountId,
        float actualResult,
        float expectedResult,
        float eloRange,
        float placementKFactor,
        float placementMaxElo,
        ACEnum acEnum,
        int highKDuration,
        out float newElo)
    {
        bool result = false;
        bool won = actualResult > 0.5f;
        float KFactor;
        float MaxDelta;
        GetKFactor(accountId, acEnum, eloRange, placementKFactor, highKDuration, won, out KFactor,
            out MaxDelta);
        if (KFactor > 0f)
        {
            float normalizedPlayerElo = GetNormalizedPlayerElo(accountId, acEnum);
            float num = KFactor * (actualResult - expectedResult);
            if (num > MaxDelta)
            {
                num = MaxDelta;
            }
            else if (num < 0f - MaxDelta)
            {
                num = 0f - MaxDelta;
            }

            newElo = normalizedPlayerElo + num;
            newElo = Math.Max(1f, newElo);
            if (newElo > placementMaxElo && GetMatchCount(accountId, acEnum) < highKDuration)
            {
                newElo = placementMaxElo;
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
            float teamAElo = teamAPredictionWeight > 0
                ? teamAAccountPrediction / teamAPredictionWeight
                : 1500f;
            double qA = Math.Pow(10.0, teamAElo / 400f);
            float teamBElo = teamBPredictionWeight > 0
                ? teamBAccountPrediction / teamBPredictionWeight
                : 1500f;
            double qB = Math.Pow(10.0, teamBElo / 400f);
            prediction = winningTeam == Team.TeamA
                ? (float)(qA / (qA + qB))
                : (float)(qB / (qA + qB));
            if (accountPredictionWeights.Count > 0)
            {
                accountPredictionWeights.Sort();
                minWeight = accountPredictionWeights.ElementAt(0);
            }
            else
            {
                minWeight = 0;
            }
        }
        else
        {
            float teamAElo = teamAPredictionWeight > 0
                ? teamACharPrediction / teamAPredictionWeight
                : 1500f;
            double qA = Math.Pow(10.0, teamAElo / 400f);
            float teamBElo = teamBPredictionWeight > 0
                ? teamBCharPrediction / teamBPredictionWeight
                : 1500f;
            double qB = Math.Pow(10.0, teamBElo / 400f);
            prediction = winningTeam == Team.TeamA
                ? (float)(qA / (qA + qB))
                : (float)(qB / (qA + qB));
            if (charPredictionWeights.Count > 0)
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
}