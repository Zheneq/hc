using System;
using System.Collections.Generic;

public class ELOKeyComponent_FinalScore : ELOKeyComponent
{
    public enum GameTypeMode
    {
        ABSOLUTE,
        RELATIVE
    }

    private GameTypeMode m_gameTypeMode;
    public override KeyModeEnum KeyMode => KeyModeEnum.BINARY;

    public override BinaryModePhaseEnum BinaryModePhase => m_gameTypeMode == GameTypeMode.ABSOLUTE
        ? BinaryModePhaseEnum.PRIMARY
        : BinaryModePhaseEnum.SECONDARY;

    public static uint PhaseWidth => 2u;

    public override char GetComponentChar()
    {
        switch (m_gameTypeMode)
        {
            case GameTypeMode.ABSOLUTE:
                return '-';
            case GameTypeMode.RELATIVE:
                return 'R';
            default:
                return '?';
        }
    }

    public override char GetPhaseChar()
    {
        return m_gameTypeMode == GameTypeMode.ABSOLUTE
            ? '0'
            : 'R';
    }

    public override string GetPhaseDescription()
    {
        return m_gameTypeMode == GameTypeMode.ABSOLUTE
            ? "absolute"
            : "relative";
    }

    public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
    {
        m_gameTypeMode = phase == BinaryModePhaseEnum.PRIMARY
            ? GameTypeMode.ABSOLUTE
            : GameTypeMode.RELATIVE;
    }

    public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
    {
        m_gameTypeMode = flags.Contains(MatchmakingQueueConfig.EloKeyFlags.RELATIVE)
            ? GameTypeMode.RELATIVE
            : GameTypeMode.ABSOLUTE;
    }

    public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
    {
        return flag == MatchmakingQueueConfig.EloKeyFlags.RELATIVE;
    }

    public override void InitializePerCharacter(byte groupSize)
    {
    }

    internal float GetActualResult(Team team, GameResult gameResultAbsolute, float gameResultFraction)
    {
        if (gameResultAbsolute == GameResult.TieGame)
        {
            return 0.5f;
        }

        if (gameResultFraction < 0.5f)
        {
            gameResultFraction = 0.5f;
        }
        else if (gameResultFraction > 1f)
        {
            gameResultFraction = 1f;
        }

        switch (gameResultAbsolute)
        {
            case GameResult.TeamAWon:
                if (team == Team.TeamA)
                {
                    return m_gameTypeMode != 0 ? gameResultFraction : 1f;
                }

                if (team == Team.TeamB)
                {
                    return m_gameTypeMode != 0 ? 1f - gameResultFraction : 0f;
                }

                throw new Exception("Unexpected victor");
            case GameResult.TeamBWon:
                if (team == Team.TeamB)
                {
                    return m_gameTypeMode == GameTypeMode.ABSOLUTE ? 1f : gameResultFraction;
                }

                if (team == Team.TeamA)
                {
                    return m_gameTypeMode == GameTypeMode.ABSOLUTE ? 0f : 1f - gameResultFraction;
                }

                throw new Exception("Unexpected victor");
            default:
                throw new Exception("Unexpected game result");
        }
    }

    public bool IsRelative()
    {
        return m_gameTypeMode == GameTypeMode.RELATIVE;
    }
}