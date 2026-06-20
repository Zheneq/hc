using System.Collections.Generic;

public class ELOKeyComponent_Queue : ELOKeyComponent
{
    private GameType m_gameType = GameType.PvP;
    private bool m_isInGeneralMode = true;

    public override KeyModeEnum KeyMode => KeyModeEnum.SPECIFICSvsGENERAL;

    public override BinaryModePhaseEnum BinaryModePhase =>
        m_isInGeneralMode ? BinaryModePhaseEnum.PRIMARY : BinaryModePhaseEnum.SECONDARY;

    public static uint PhaseWidth => 2u;
    public GameType GameType => m_gameType;

    public override char GetComponentChar()
    {
        if (m_isInGeneralMode)
        {
            return '-';
        }

        switch (m_gameType)
        {
            case GameType.PvP:
            case GameType.NewPlayerPvP:
                return 'v';
            case GameType.Ranked:
                return 'r';
            case GameType.Custom:
                return 'm';
            case GameType.Coop:
                return 'c';
            case GameType.Solo:
                return 's';
            case GameType.Casual:
                return 'z';
            default:
                return 'o';
        }
    }

    public override char GetPhaseChar()
    {
        return m_isInGeneralMode ? (char)48 : (char)81;
    }

    public override string GetPhaseDescription()
    {
        return m_isInGeneralMode ? "all" : GameType.ToString();
    }

    public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
    {
        m_isInGeneralMode = phase == BinaryModePhaseEnum.PRIMARY;
        m_gameType = isCasual ? GameType.Casual : gameType;
    }

    public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
    {
        m_isInGeneralMode = !flags.Contains(MatchmakingQueueConfig.EloKeyFlags.QUEUE);
        m_gameType = isCasual ? GameType.Casual : gameType;
    }

    public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
    {
        return flag == MatchmakingQueueConfig.EloKeyFlags.QUEUE;
    }

    public override void InitializePerCharacter(byte groupSize)
    {
    }
}