using System;

public static class GameModeUtils
{
	public static bool IsCtfGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		return gameModeEvent != null && GameModeUtils.IsCtfGameModeEventType(gameModeEvent.m_eventType);
	}

	public static bool IsCtcGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		return gameModeEvent != null && GameModeUtils.IsCtcGameModeEventType(gameModeEvent.m_eventType);
	}

	public static bool IsCtfGameModeEventType(GameModeEventType gameModeEventType)
	{
		return gameModeEventType == GameModeEventType.Ctf_FlagPickedUp || gameModeEventType == GameModeEventType.Ctf_FlagDropped || gameModeEventType == GameModeEventType.Ctf_FlagTurnedIn || gameModeEventType == GameModeEventType.Ctf_FlagSentToSpawn;
	}

	public static bool IsCtcGameModeEventType(GameModeEventType gameModeEventType)
	{
		if (gameModeEventType == GameModeEventType.Ctc_CoinPickedUp)
		{
			return true;
		}
		if (gameModeEventType == GameModeEventType.Ctc_CoinsDropped)
		{
			return true;
		}
		if (gameModeEventType == GameModeEventType.Ctc_NonCoinPowerupTouched)
		{
			return true;
		}
		if (gameModeEventType == GameModeEventType.Ctc_CoinPowerupTouched)
		{
			return true;
		}
		return false;
	}
}
