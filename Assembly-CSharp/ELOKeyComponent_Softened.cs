using System;
using System.Collections.Generic;

public class ELOKeyComponent_Softened : ELOKeyComponent
{
	public ELOKeyComponent.BinaryModePhaseEnum m_phase;

	private static float c_minRange = 50f;

	private static float c_halfingRange = 100f;

	public override ELOKeyComponent.KeyModeEnum KeyMode
	{
		get
		{
			return ELOKeyComponent.KeyModeEnum.BINARY;
		}
	}

	public override ELOKeyComponent.BinaryModePhaseEnum BinaryModePhase
	{
		get
		{
			return this.m_phase;
		}
	}

	public static uint PhaseWidth
	{
		get
		{
			return 3U;
		}
	}

	public override char GetComponentChar()
	{
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.PRIMARY)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.GetComponentChar()).MethodHandle;
			}
			return '-';
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.SECONDARY)
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
			return 'V';
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.TERTIARY)
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
			return 'i';
		}
		return '?';
	}

	public override char GetPhaseChar()
	{
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.PRIMARY)
		{
			return '0';
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.SECONDARY)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.GetPhaseChar()).MethodHandle;
			}
			return 'V';
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.TERTIARY)
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
			return 'i';
		}
		return '?';
	}

	public override string GetPhaseDescription()
	{
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.PRIMARY)
		{
			return "brute";
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.SECONDARY)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.GetPhaseDescription()).MethodHandle;
			}
			return "public";
		}
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.TERTIARY)
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
			return "individual";
		}
		return "error";
	}

	public override void Initialize(ELOKeyComponent.BinaryModePhaseEnum phase, GameType gameType, bool isCasual)
	{
		this.m_phase = phase;
	}

	public override void Initialize(List<MatchmakingQueueConfig.EloKeyFlags> flags, GameType gameType, bool isCasual)
	{
		ELOKeyComponent.BinaryModePhaseEnum phase;
		if (flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_PUBLIC))
		{
			phase = ELOKeyComponent.BinaryModePhaseEnum.SECONDARY;
		}
		else if (flags.Contains(MatchmakingQueueConfig.EloKeyFlags.SOFTENED_INDIVIDUAL))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.Initialize(List<MatchmakingQueueConfig.EloKeyFlags>, GameType, bool)).MethodHandle;
			}
			phase = ELOKeyComponent.BinaryModePhaseEnum.TERTIARY;
		}
		else
		{
			phase = ELOKeyComponent.BinaryModePhaseEnum.PRIMARY;
		}
		this.m_phase = phase;
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
		if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.SECONDARY)
		{
			if (eloRange > ELOKeyComponent_Softened.c_minRange)
			{
				double y = (double)((eloRange - ELOKeyComponent_Softened.c_minRange) / ELOKeyComponent_Softened.c_halfingRange);
				num /= Math.Pow(2.0, y);
			}
			if (preMadeGroupRatio < 0.25f)
			{
				num *= 0.25;
			}
			else if (preMadeGroupRatio < 1f)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.GetMaxDeltaFraction(float, float, bool, float, float)).MethodHandle;
				}
				num *= (double)preMadeGroupRatio;
			}
		}
		else if (this.m_phase == ELOKeyComponent.BinaryModePhaseEnum.TERTIARY)
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
			float num2 = (!won) ? (averageElo - currentElo) : (currentElo - averageElo);
			if (num2 > 0f)
			{
				double y2 = (double)(num2 / 400f);
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
		else if (this.m_phase != ELOKeyComponent.BinaryModePhaseEnum.PRIMARY)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELOKeyComponent_Softened.GetMaxDelta(float, float, float, bool, float, float)).MethodHandle;
			}
			num = 20f;
		}
		else
		{
			if (currentElo <= 2600f)
			{
				if (currentElo >= 400f)
				{
					if (currentElo <= 2450f)
					{
						if (currentElo >= 550f)
						{
							if (currentElo <= 2300f)
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
								if (currentElo >= 700f)
								{
									goto IL_AF;
								}
								for (;;)
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
							goto IL_AF;
						}
						for (;;)
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
					goto IL_AF;
				}
				for (;;)
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
		IL_AF:
		float maxDeltaFraction = this.GetMaxDeltaFraction(eloRange, preMadeGroupRatio, won, currentElo, averageElo);
		if (maxDeltaFraction >= 0f)
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
			if (maxDeltaFraction <= 1f)
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
				return num * maxDeltaFraction;
			}
		}
		Log.Error("Why is account generating in incorrect max K fraction ({0}) for phase ({1})?", new object[]
		{
			maxDeltaFraction,
			this.GetPhaseDescription()
		});
		return num;
	}
}
