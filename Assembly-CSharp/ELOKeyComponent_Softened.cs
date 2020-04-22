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
		if (m_phase == BinaryModePhaseEnum.PRIMARY)
		{
			while (true)
			{
				switch (6)
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
		if (m_phase == BinaryModePhaseEnum.SECONDARY)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return 'V';
				}
			}
		}
		if (m_phase == BinaryModePhaseEnum.TERTIARY)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 'i';
				}
			}
		}
		return '?';
	}

	public override char GetPhaseChar()
	{
		if (m_phase == BinaryModePhaseEnum.PRIMARY)
		{
			return '0';
		}
		if (m_phase == BinaryModePhaseEnum.SECONDARY)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 'V';
				}
			}
		}
		if (m_phase == BinaryModePhaseEnum.TERTIARY)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return 'i';
				}
			}
		}
		return '?';
	}

	public override string GetPhaseDescription()
	{
		if (m_phase == BinaryModePhaseEnum.PRIMARY)
		{
			return "brute";
		}
		if (m_phase == BinaryModePhaseEnum.SECONDARY)
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
					return "public";
				}
			}
		}
		if (m_phase == BinaryModePhaseEnum.TERTIARY)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return "individual";
				}
			}
		}
		return "error";
	}

	public override void Initialize(BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		m_phase = phase;
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		int phase;
		if (flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC))
		{
			phase = 1;
		}
		else if (flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_INDIVIDUAL))
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
			phase = 2;
		}
		else
		{
			phase = 0;
		}
		m_phase = (BinaryModePhaseEnum)phase;
	}

	public override bool MatchesFlag(MatchmakingQueueConfig.EloKeyFlags flag)
	{
		return flag == MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC || flag == MatchmakingQueueConfig.EloKeyFlags.SOFTENED_INDIVIDUAL;
	}

	public override void InitializePerCharacter(byte groupSize)
	{
	}

	private float GetMaxDeltaFraction(float eloRange, float preMadeGroupRatio, bool won, float currentElo, float averageElo)
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
				while (true)
				{
					switch (2)
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
				num *= (double)preMadeGroupRatio;
			}
		}
		else if (m_phase == BinaryModePhaseEnum.TERTIARY)
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
			float num2 = (!won) ? (averageElo - currentElo) : (currentElo - averageElo);
			if (num2 > 0f)
			{
				double y2 = num2 / 400f;
				num /= Math.Pow(4.0, y2);
			}
		}
		return (float)num;
	}

	public float GetMaxDelta(float eloRange, float preMadeGroupRatio, float placementKFactor, bool won, float currentElo, float averageElo)
	{
		float num = 40f;
		if (placementKFactor > 0f)
		{
			num = placementKFactor;
		}
		else if (m_phase != 0)
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
			num = 20f;
		}
		else
		{
			if (!(currentElo > 2600f))
			{
				if (!(currentElo < 400f))
				{
					if (!(currentElo > 2450f))
					{
						if (!(currentElo < 550f))
						{
							if (!(currentElo > 2300f))
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
								if (!(currentElo < 700f))
								{
									goto IL_00af;
								}
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							num = 20f;
							goto IL_00af;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					num = 10f;
					goto IL_00af;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			num = 5f;
		}
		goto IL_00af;
		IL_010c:
		return num;
		IL_00af:
		float maxDeltaFraction = GetMaxDeltaFraction(eloRange, preMadeGroupRatio, won, currentElo, averageElo);
		if (maxDeltaFraction >= 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (maxDeltaFraction <= 1f)
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
				num *= maxDeltaFraction;
				goto IL_010c;
			}
		}
		Log.Error("Why is account generating in incorrect max K fraction ({0}) for phase ({1})?", maxDeltaFraction, GetPhaseDescription());
		goto IL_010c;
	}
}
