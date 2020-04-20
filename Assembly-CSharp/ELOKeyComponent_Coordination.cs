using System;
using System.Collections.Generic;
using System.Linq;

public class ELOKeyComponent_Coordination : ELOKeyComponent
{
	private bool m_isInGeneralMode = true;

	private byte m_groupSize = 1;

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

	public override char GetComponentChar()
	{
		if (this.m_isInGeneralMode)
		{
			return '-';
		}
		if (this.m_groupSize == 1)
		{
			return 'S';
		}
		return string.Format("{0}", this.m_groupSize).ToCharArray().ElementAt(0);
	}

	public bool InGeneralMode
	{
		get
		{
			return this.m_isInGeneralMode;
		}
	}

	public override char GetPhaseChar()
	{
		char result;
		if (this.m_isInGeneralMode)
		{
			result = '0';
		}
		else
		{
			result = 'G';
		}
		return result;
	}

	public override string GetPhaseDescription()
	{
		if (this.m_isInGeneralMode)
		{
			return "ignore";
		}
		switch (this.m_groupSize)
		{
		case 1:
			return "solo";
		case 2:
			return "duo";
		case 4:
			return "four player";
		}
		return string.Format("{0} player", this.m_groupSize);
	}

	public override void Initialize(ELOKeyComponent.BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		this.m_isInGeneralMode = (phase == ELOKeyComponent.BinaryModePhaseEnum.PRIMARY);
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		this.m_isInGeneralMode = !flags.Contains(MatchmakingQueueConfig.EloKeyFlags.GROUP);
	}

	public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		return flag == MatchmakingQueueConfig.EloKeyFlags.GROUP;
	}

	public override void InitializePerCharacter(byte groupSize)
	{
		this.m_groupSize = groupSize;
		if (groupSize == 0)
		{
			throw new Exception("Illegal group size");
		}
	}
}
