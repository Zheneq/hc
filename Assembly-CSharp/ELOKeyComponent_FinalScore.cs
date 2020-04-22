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

	public override BinaryModePhaseEnum BinaryModePhase
	{
		get
		{
			int result;
			if (m_gameTypeMode == GameTypeMode.ABSOLUTE)
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

	public override char GetComponentChar()
	{
		if (m_gameTypeMode == GameTypeMode.ABSOLUTE)
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
					return '-';
				}
			}
		}
		if (m_gameTypeMode == GameTypeMode.RELATIVE)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return 'R';
				}
			}
		}
		return '?';
	}

	public override char GetPhaseChar()
	{
		return (m_gameTypeMode != 0) ? 'R' : '0';
	}

	public override string GetPhaseDescription()
	{
		object result;
		if (m_gameTypeMode == GameTypeMode.ABSOLUTE)
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
			result = "absolute";
		}
		else
		{
			result = "relative";
		}
		return (string)result;
	}

	public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		int gameTypeMode;
		if (phase == BinaryModePhaseEnum.PRIMARY)
		{
			while (true)
			{
				switch (3)
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
			gameTypeMode = 0;
		}
		else
		{
			gameTypeMode = 1;
		}
		m_gameTypeMode = (GameTypeMode)gameTypeMode;
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		m_gameTypeMode = (flags.Contains(MatchmakingQueueConfig.EloKeyFlags.RELATIVE) ? GameTypeMode.RELATIVE : GameTypeMode.ABSOLUTE);
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 0.5f;
				}
			}
		}
		if (gameResultFraction < 0.5f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			gameResultFraction = 0.5f;
		}
		else if (gameResultFraction > 1f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			gameResultFraction = 1f;
		}
		switch (gameResultAbsolute)
		{
		case GameResult.TeamAWon:
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (team == Team.TeamA)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return (m_gameTypeMode != 0) ? gameResultFraction : 1f;
						}
					}
				}
				if (team == Team.TeamB)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return (m_gameTypeMode != 0) ? (1f - gameResultFraction) : 0f;
						}
					}
				}
				throw new Exception("Unexpected victor");
			}
		case GameResult.TeamBWon:
			if (team == Team.TeamB)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						float result;
						if (m_gameTypeMode == GameTypeMode.ABSOLUTE)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							result = 1f;
						}
						else
						{
							result = gameResultFraction;
						}
						return result;
					}
					}
				}
			}
			if (team == Team.TeamA)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						float result2;
						if (m_gameTypeMode == GameTypeMode.ABSOLUTE)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							result2 = 0f;
						}
						else
						{
							result2 = 1f - gameResultFraction;
						}
						return result2;
					}
					}
				}
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
