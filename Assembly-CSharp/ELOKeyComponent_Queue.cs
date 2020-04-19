using System;
using System.Collections.Generic;

public class ELOKeyComponent_Queue : ELOKeyComponent
{
	private GameType m_gameType = GameType.PvP;

	private bool m_isInGeneralMode = true;

	public override ELOKeyComponent.KeyModeEnum KeyMode
	{
		get
		{
			return ELOKeyComponent.KeyModeEnum.SPECIFICSvsGENERAL;
		}
	}

	public override ELOKeyComponent.BinaryModePhaseEnum BinaryModePhase
	{
		get
		{
			ELOKeyComponent.BinaryModePhaseEnum result;
			if (this.m_isInGeneralMode)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Queue.get_BinaryModePhase()).MethodHandle;
				}
				result = ELOKeyComponent.BinaryModePhaseEnum.PRIMARY;
			}
			else
			{
				result = ELOKeyComponent.BinaryModePhaseEnum.SECONDARY;
			}
			return result;
		}
	}

	public static uint PhaseWidth
	{
		get
		{
			return 2U;
		}
	}

	public GameType GameType
	{
		get
		{
			return this.m_gameType;
		}
	}

	public override char GetComponentChar()
	{
		if (this.m_isInGeneralMode)
		{
			return '-';
		}
		if (this.m_gameType != GameType.PvP)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Queue.GetComponentChar()).MethodHandle;
			}
			if (this.m_gameType == GameType.NewPlayerPvP)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				if (this.m_gameType == GameType.Ranked)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return 'r';
				}
				if (this.m_gameType == GameType.Custom)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return 'm';
				}
				if (this.m_gameType == GameType.Coop)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					return 'c';
				}
				if (this.m_gameType == GameType.Solo)
				{
					return 's';
				}
				if (this.m_gameType == GameType.Casual)
				{
					return 'z';
				}
				return 'o';
			}
		}
		return 'v';
	}

	public override char GetPhaseChar()
	{
		char result;
		if (this.m_isInGeneralMode)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Queue.GetPhaseChar()).MethodHandle;
			}
			result = '0';
		}
		else
		{
			result = 'Q';
		}
		return result;
	}

	public override string GetPhaseDescription()
	{
		string result;
		if (this.m_isInGeneralMode)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Queue.GetPhaseDescription()).MethodHandle;
			}
			result = "all";
		}
		else
		{
			result = this.GameType.ToString();
		}
		return result;
	}

	public override void Initialize(ELOKeyComponent.BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		this.m_isInGeneralMode = (phase == ELOKeyComponent.BinaryModePhaseEnum.PRIMARY);
		this.m_gameType = ((!isCasual) ? gameType : GameType.Casual);
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		this.m_isInGeneralMode = !flags.Contains(MatchmakingQueueConfig.EloKeyFlags.QUEUE);
		GameType gameType2;
		if (isCasual)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Queue.Initialize(List<MatchmakingQueueConfig.EloKeyFlags>, GameType, bool)).MethodHandle;
			}
			gameType2 = GameType.Casual;
		}
		else
		{
			gameType2 = gameType;
		}
		this.m_gameType = gameType2;
	}

	public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		return flag == MatchmakingQueueConfig.EloKeyFlags.QUEUE;
	}

	public override void InitializePerCharacter(byte groupSize)
	{
	}
}
