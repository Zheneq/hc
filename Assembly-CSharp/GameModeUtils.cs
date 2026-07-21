public static class GameModeUtils
{
    public static bool IsCtfGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtfGameModeEventType(gameModeEvent.m_eventType);
    }

    public static bool IsCtcGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        return gameModeEvent != null && IsCtcGameModeEventType(gameModeEvent.m_eventType);
    }

    public static bool IsCtfGameModeEventType(GameModeEventType gameModeEventType)
    {
        switch (gameModeEventType)
        {
            case GameModeEventType.Ctf_FlagPickedUp:
            case GameModeEventType.Ctf_FlagDropped:
            case GameModeEventType.Ctf_FlagTurnedIn:
            case GameModeEventType.Ctf_FlagSentToSpawn:
                return true;
            default:
                return false;
        }
    }

    public static bool IsCtcGameModeEventType(GameModeEventType gameModeEventType)
    {
        switch (gameModeEventType)
        {
            case GameModeEventType.Ctc_CoinPickedUp:
            case GameModeEventType.Ctc_CoinsDropped:
            case GameModeEventType.Ctc_NonCoinPowerupTouched:
            case GameModeEventType.Ctc_CoinPowerupTouched:
                return true;
            default:
                return false;
        }
    }
}