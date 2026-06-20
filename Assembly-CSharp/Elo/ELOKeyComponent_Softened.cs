using System;
using System.Collections.Generic;

public class ELOKeyComponent_Softened : ELOKeyComponent
{
    public BinaryModePhaseEnum m_phase;

    private static float c_minRange = 50f;
    private static float c_halfingRange = 100f;

    public override KeyModeEnum KeyMode => KeyModeEnum.BINARY;
    public override BinaryModePhaseEnum BinaryModePhase => m_phase;
    public static uint PhaseWidth => 3u;

    public override char GetComponentChar()
    {
        switch (m_phase)
        {
            case BinaryModePhaseEnum.PRIMARY:
                return '-';
            case BinaryModePhaseEnum.SECONDARY:
                return 'V';
            case BinaryModePhaseEnum.TERTIARY:
                return 'i';
            default:
                return '?';
        }
    }

    public override char GetPhaseChar()
    {
        switch (m_phase)
        {
            case BinaryModePhaseEnum.PRIMARY:
                return '0';
            case BinaryModePhaseEnum.SECONDARY:
                return 'V';
            case BinaryModePhaseEnum.TERTIARY:
                return 'i';
            default:
                return '?';
        }
    }

    public override string GetPhaseDescription()
    {
        switch (m_phase)
        {
            case BinaryModePhaseEnum.PRIMARY:
                return "brute";
            case BinaryModePhaseEnum.SECONDARY:
                return "public";
            case BinaryModePhaseEnum.TERTIARY:
                return "individual";
            default:
                return "error";
        }
    }

    public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
    {
        m_phase = phase;
    }

    public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
    {
        m_phase = flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC)
            ? BinaryModePhaseEnum.SECONDARY
            : flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_INDIVIDUAL)
                ? BinaryModePhaseEnum.TERTIARY
                : BinaryModePhaseEnum.PRIMARY;
    }

    public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
    {
        return flag == MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC
               || flag == MatchmakingQueueConfig.EloKeyFlags.SOFTENED_INDIVIDUAL;
    }

    public override void InitializePerCharacter(byte groupSize)
    {
    }

    private float GetMaxDeltaFraction(float eloRange, float preMadeGroupRatio, bool won, float currentElo,
        float averageElo)
    {
        double num = 1.0;
        if (m_phase == BinaryModePhaseEnum.SECONDARY)
        {
            if (eloRange > c_minRange)
            {
                double y = (eloRange - c_minRange) / c_halfingRange;
                num /= Math.Pow(2.0, y);
            }

            if (preMadeGroupRatio < 0.25f)
            {
                num *= 0.25;
            }
            else if (preMadeGroupRatio < 1f)
            {
                num *= preMadeGroupRatio;
            }
        }
        else if (m_phase == BinaryModePhaseEnum.TERTIARY)
        {
            float num2 = won
                ? currentElo - averageElo
                : averageElo - currentElo;
            if (num2 > 0f)
            {
                double y2 = num2 / 400f;
                num /= Math.Pow(4.0, y2);
            }
        }

        return (float)num;
    }

    public float GetMaxDelta(
        float eloRange,
        float preMadeGroupRatio,
        float placementKFactor,
        bool won,
        float currentElo,
        float averageElo)
    {
        float num = 40f;
        if (placementKFactor > 0f)
        {
            num = placementKFactor;
        }
        else if (m_phase != 0)
        {
            num = 20f;
        }
        else if (currentElo > 2600f || currentElo < 400f)
        {
            num = 5f;
        }
        else if (currentElo > 2450f || currentElo < 550f)
        {
            num = 10f;
        }
        else if (currentElo > 2300f || currentElo < 700f)
        {
            num = 20f;
        }
        
        float maxDeltaFraction = GetMaxDeltaFraction(eloRange, preMadeGroupRatio, won, currentElo, averageElo);
        if (maxDeltaFraction >= 0f && maxDeltaFraction <= 1f)
        {
            num *= maxDeltaFraction;
        }
        else
        {
            Log.Error(
                "Why is account generating in incorrect max K fraction ({0}) for phase ({1})?", 
                maxDeltaFraction,
                GetPhaseDescription());
        }

        return num;
    }
}