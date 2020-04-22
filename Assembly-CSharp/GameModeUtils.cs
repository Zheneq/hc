public static class GameModeUtils
{
	public static bool IsCtfGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		if (gameModeEvent == null)
		{
			return false;
		}
		return IsCtfGameModeEventType(gameModeEvent.m_eventType);
	}

	public static bool IsCtcGameModeEvent(ClientGameModeEvent gameModeEvent)
	{
		if (gameModeEvent == null)
		{
			return false;
		}
		return IsCtcGameModeEventType(gameModeEvent.m_eventType);
	}

	public static bool IsCtfGameModeEventType(GameModeEventType gameModeEventType)
	{
		switch (gameModeEventType)
		{
		case GameModeEventType.Ctf_FlagPickedUp:
			return true;
		case GameModeEventType.Ctf_FlagDropped:
			return true;
		case GameModeEventType.Ctf_FlagTurnedIn:
			return true;
		case GameModeEventType.Ctf_FlagSentToSpawn:
			return true;
		default:
			return false;
		}
	}

	public static bool IsCtcGameModeEventType(GameModeEventType gameModeEventType)
	{
		if (gameModeEventType == GameModeEventType.Ctc_CoinPickedUp)
		{
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
					return true;
				}
			}
		}
		if (gameModeEventType == GameModeEventType.Ctc_CoinsDropped)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (gameModeEventType == GameModeEventType.Ctc_NonCoinPowerupTouched)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (gameModeEventType == GameModeEventType.Ctc_CoinPowerupTouched)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		return false;
	}
}
