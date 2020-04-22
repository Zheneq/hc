using System.Collections.Generic;

public class ELOKeyComponent_Queue : ELOKeyComponent
{
	private GameType m_gameType = GameType.PvP;

	private bool m_isInGeneralMode = true;

	public override KeyModeEnum KeyMode => KeyModeEnum.SPECIFICSvsGENERAL;

	public override BinaryModePhaseEnum BinaryModePhase
	{
		get
		{
			int result;
			if (m_isInGeneralMode)
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
				result = 0;
			}
			else
			{
				result = 1;
			}
			return (BinaryModePhaseEnum)result;
		}
	}

	public static uint PhaseWidth => 2u;

	public GameType GameType => m_gameType;

	public override char GetComponentChar()
	{
		if (m_isInGeneralMode)
		{
			return '-';
		}
		if (m_gameType != GameType.PvP)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_gameType != GameType.NewPlayerPvP)
			{
				if (m_gameType == GameType.Ranked)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return 'r';
						}
					}
				}
				if (m_gameType == GameType.Custom)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return 'm';
						}
					}
				}
				if (m_gameType == GameType.Coop)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return 'c';
						}
					}
				}
				if (m_gameType == GameType.Solo)
				{
					return 's';
				}
				if (m_gameType == GameType.Casual)
				{
					return 'z';
				}
				return 'o';
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
		return 'v';
	}

	public override char GetPhaseChar()
	{
		int result;
		if (m_isInGeneralMode)
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
			result = 48;
		}
		else
		{
			result = 81;
		}
		return (char)result;
	}

	public override string GetPhaseDescription()
	{
		object result;
		if (m_isInGeneralMode)
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
			result = "all";
		}
		else
		{
			result = GameType.ToString();
		}
		return (string)result;
	}

	public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		m_isInGeneralMode = (phase == BinaryModePhaseEnum.PRIMARY);
		m_gameType = ((!isCasual) ? gameType : GameType.Casual);
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		m_isInGeneralMode = !flags.Contains(MatchmakingQueueConfig.EloKeyFlags.QUEUE);
		int gameType2;
		if (isCasual)
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
			gameType2 = 15;
		}
		else
		{
			gameType2 = (int)gameType;
		}
		m_gameType = (GameType)gameType2;
	}

	public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		return flag == MatchmakingQueueConfig.EloKeyFlags.QUEUE;
	}

	public override void InitializePerCharacter(byte groupSize)
	{
	}
}
