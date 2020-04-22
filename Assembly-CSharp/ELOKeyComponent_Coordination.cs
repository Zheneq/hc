using System;
using System.Collections.Generic;
using System.Linq;

public class ELOKeyComponent_Coordination : ELOKeyComponent
{
	private bool m_isInGeneralMode = true;

	private byte m_groupSize = 1;

	public override KeyModeEnum KeyMode => KeyModeEnum.SPECIFICSvsGENERAL;

	public override BinaryModePhaseEnum BinaryModePhase
	{
		get
		{
			int result;
			if (m_isInGeneralMode)
			{
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

	public bool InGeneralMode => m_isInGeneralMode;

	public override char GetComponentChar()
	{
		if (m_isInGeneralMode)
		{
			return '-';
		}
		if (m_groupSize == 1)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 'S';
				}
			}
		}
		return $"{m_groupSize}".ToCharArray().ElementAt(0);
	}

	public override char GetPhaseChar()
	{
		int result;
		if (m_isInGeneralMode)
		{
			result = 48;
		}
		else
		{
			result = 71;
		}
		return (char)result;
	}

	public override string GetPhaseDescription()
	{
		if (m_isInGeneralMode)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return "ignore";
				}
			}
		}
		switch (m_groupSize)
		{
		case 1:
			return "solo";
		case 2:
			return "duo";
		case 4:
			return "four player";
		default:
			return $"{m_groupSize} player";
		}
	}

	public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		m_isInGeneralMode = (phase == BinaryModePhaseEnum.PRIMARY);
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		m_isInGeneralMode = !flags.Contains(MatchmakingQueueConfig.EloKeyFlags.GROUP);
	}

	public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		return flag == MatchmakingQueueConfig.EloKeyFlags.GROUP;
	}

	public override void InitializePerCharacter(byte groupSize)
	{
		m_groupSize = groupSize;
		if (groupSize != 0)
		{
			return;
		}
		while (true)
		{
			throw new Exception("Illegal group size");
		}
	}
}
