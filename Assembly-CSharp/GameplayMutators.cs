using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMutators : MonoBehaviour
{
	private static GameplayMutators s_instance;

	[Separator("Combat Multipliers", true)]
	public float m_energyGainMultiplier = 1f;

	public GameplayMutators.IntervalDef m_energyGainMultInterval;

	[Space(10f)]
	public float m_damageMultiplier = 1f;

	public GameplayMutators.IntervalDef m_damageMultInterval;

	[Space(10f)]
	public float m_healingMultiplier = 1f;

	public GameplayMutators.IntervalDef m_healingMultInterval;

	[Space(10f)]
	public float m_absorbMultiplier = 1f;

	public GameplayMutators.IntervalDef m_absorbMultInterval;

	[Separator("Cooldowns", true)]
	public int m_cooldownSpeedAdjustment;

	public int m_cooldownTimeAdjustment;

	public float m_cooldownMultiplier = 1f;

	[Separator("Powerups", true)]
	public int m_powerupRefreshSpeedAdjustment;

	public int m_powerupDurationAdjustment;

	[Separator("Always On Status", true)]
	public List<GameplayMutators.StatusInterval> m_alwaysOnStatuses;

	[Separator("Status Suppression", true)]
	public List<GameplayMutators.StatusInterval> m_statusSuppression;

	[Separator("Stats", true)]
	public float m_passiveEnergyRegenMultiplier = 1f;

	public float m_passiveHpRegenMultiplier = 1f;

	[Separator("Energized Status Override", true)]
	public bool m_useEnergizedOverride;

	public AbilityModPropertyInt m_energizedEnergyGainMod;

	[Separator("SlowEnergyGain Status Override", true)]
	public bool m_useSlowEnergyGainOverride;

	public AbilityModPropertyInt m_slowEnergyGainEnergyGainMod;

	[Separator("Haste Status Override", true)]
	public bool m_useHasteOverride;

	public int m_hasteHalfMovementAdjustAmount;

	public int m_hasteFullMovementAdjustAmount;

	public float m_hasteMovementMultiplier = 1f;

	[Separator("Slow Status Override", true)]
	public bool m_useSlowOverride;

	public int m_slowHalfMovementAdjustAmount;

	public int m_slowFullMovementAdjustAmount;

	public float m_slowMovementMultiplier = 1f;

	[Separator("Empowered Status Override", true)]
	public bool m_useEmpoweredOverride;

	public AbilityModPropertyInt m_empoweredOutgoingDamageMod;

	public AbilityModPropertyInt m_empoweredOutgoingHealingMod;

	public AbilityModPropertyInt m_empoweredOutgoingAbsorbMod;

	[Separator("Weakened Status Override", true)]
	public bool m_useWeakenedOverride;

	public AbilityModPropertyInt m_weakenedOutgoingDamageMod;

	public AbilityModPropertyInt m_weakenedOutgoingHealingMod;

	public AbilityModPropertyInt m_weakenedOutgoingAbsorbMod;

	[Separator("Armored Status Override", true)]
	public bool m_useArmoredOverride;

	public AbilityModPropertyInt m_armoredIncomingDamageMod;

	[Separator("Vulnerable Status Override", true)]
	public bool m_useVulnerableOverride;

	public float m_vulnerableDamageMultiplier = -1f;

	public int m_vulnerableDamageFlatAdd;

	public static GameplayMutators Get()
	{
		return GameplayMutators.s_instance;
	}

	private void Awake()
	{
		GameplayMutators.s_instance = this;
	}

	private void OnDestroy()
	{
		GameplayMutators.s_instance = null;
	}

	private static bool IsMultiplierActive(GameplayMutators.IntervalDef multInterval)
	{
		if (GameFlowData.Get() != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsMultiplierActive(GameplayMutators.IntervalDef)).MethodHandle;
			}
			if (multInterval != null)
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
				int currentTurn = GameFlowData.Get().CurrentTurn;
				return GameplayMutators.IsActiveInCurrentActionPhase(currentTurn, multInterval.m_onDuration, multInterval.m_restDuration, multInterval.m_startOffset, multInterval.m_delayTillStartOfMovement, GameplayMutators.ActionPhaseCheckMode.Default);
			}
		}
		return true;
	}

	public static float GetEnergyGainMultiplier()
	{
		if (GameplayMutators.s_instance != null && GameplayMutators.IsMultiplierActive(GameplayMutators.s_instance.m_energyGainMultInterval))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetEnergyGainMultiplier()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_energyGainMultiplier;
		}
		return 1f;
	}

	public static float GetDamageMultiplier()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetDamageMultiplier()).MethodHandle;
			}
			if (GameplayMutators.IsMultiplierActive(GameplayMutators.s_instance.m_damageMultInterval))
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
				return GameplayMutators.s_instance.m_damageMultiplier;
			}
		}
		return 1f;
	}

	public static float GetHealingMultiplier()
	{
		if (GameplayMutators.s_instance != null && GameplayMutators.IsMultiplierActive(GameplayMutators.s_instance.m_healingMultInterval))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetHealingMultiplier()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_healingMultiplier;
		}
		return 1f;
	}

	public static float GetAbsorbMultiplier()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetAbsorbMultiplier()).MethodHandle;
			}
			if (GameplayMutators.IsMultiplierActive(GameplayMutators.s_instance.m_absorbMultInterval))
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
				return GameplayMutators.s_instance.m_absorbMultiplier;
			}
		}
		return 1f;
	}

	public static int GetCooldownSpeedAdjustment()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetCooldownSpeedAdjustment()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_cooldownSpeedAdjustment;
		}
		return 0;
	}

	public static int GetCooldownTimeAdjustment()
	{
		if (GameplayMutators.s_instance != null)
		{
			return GameplayMutators.s_instance.m_cooldownTimeAdjustment;
		}
		return 0;
	}

	public static float GetCooldownMultiplier()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetCooldownMultiplier()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_cooldownMultiplier;
		}
		return 1f;
	}

	public static int GetPowerupRefreshSpeedAdjustment()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetPowerupRefreshSpeedAdjustment()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_powerupRefreshSpeedAdjustment;
		}
		return 0;
	}

	public static int GetPowerupDurationAdjustment()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetPowerupDurationAdjustment()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_powerupDurationAdjustment;
		}
		return 0;
	}

	public static bool IsStatusActive(StatusType statusType, int currentTurn, GameplayMutators.ActionPhaseCheckMode phaseCheckMode = GameplayMutators.ActionPhaseCheckMode.Default)
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsStatusActive(StatusType, int, GameplayMutators.ActionPhaseCheckMode)).MethodHandle;
			}
			if (GameplayMutators.s_instance.m_alwaysOnStatuses != null)
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
				for (int i = 0; i < GameplayMutators.s_instance.m_alwaysOnStatuses.Count; i++)
				{
					if (GameplayMutators.s_instance.m_alwaysOnStatuses[i].m_statusType == statusType)
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
						int startOffset = GameplayMutators.s_instance.m_alwaysOnStatuses[i].m_startOffset;
						int duration = GameplayMutators.s_instance.m_alwaysOnStatuses[i].m_duration;
						int interval = GameplayMutators.s_instance.m_alwaysOnStatuses[i].m_interval;
						bool delayTillStartOfMovement = GameplayMutators.s_instance.m_alwaysOnStatuses[i].m_delayTillStartOfMovement;
						return GameplayMutators.IsActiveInCurrentActionPhase(currentTurn, duration, interval, startOffset, delayTillStartOfMovement, phaseCheckMode);
					}
				}
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
		}
		return false;
	}

	public static bool IsStatusSuppressed(StatusType statusType, int currentTurn, GameplayMutators.ActionPhaseCheckMode phaseCheckMode = GameplayMutators.ActionPhaseCheckMode.Default)
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsStatusSuppressed(StatusType, int, GameplayMutators.ActionPhaseCheckMode)).MethodHandle;
			}
			if (GameplayMutators.s_instance.m_statusSuppression != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				List<GameplayMutators.StatusInterval> statusSuppression = GameplayMutators.s_instance.m_statusSuppression;
				for (int i = 0; i < statusSuppression.Count; i++)
				{
					if (statusSuppression[i].m_statusType == statusType)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						GameplayMutators.StatusInterval statusInterval = statusSuppression[i];
						int startOffset = statusInterval.m_startOffset;
						int duration = statusInterval.m_duration;
						int interval = statusInterval.m_interval;
						bool delayTillStartOfMovement = statusInterval.m_delayTillStartOfMovement;
						return GameplayMutators.IsActiveInCurrentActionPhase(currentTurn, duration, interval, startOffset, delayTillStartOfMovement, phaseCheckMode);
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return false;
	}

	public static bool IsActiveInCurrentActionPhase(int currentTurn, int onDuration, int restDuration, int startOffset, bool delayTillMoveStart, GameplayMutators.ActionPhaseCheckMode phaseCheckMode)
	{
		bool flag = false;
		bool flag2 = GameplayMutators.IsActiveInCurrentTurn(currentTurn, onDuration, restDuration, startOffset);
		if (flag2 && phaseCheckMode == GameplayMutators.ActionPhaseCheckMode.Any)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsActiveInCurrentActionPhase(int, int, int, int, bool, GameplayMutators.ActionPhaseCheckMode)).MethodHandle;
			}
			flag = true;
		}
		else if (flag2)
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
			if (delayTillMoveStart)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag3 = GameplayMutators.IsActiveInCurrentTurn(currentTurn - 1, onDuration, restDuration, startOffset);
				if (!GameplayMutators.IsOnStartOfIntervalCycle(currentTurn, onDuration, restDuration, startOffset))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (flag3)
					{
						flag = true;
						goto IL_11D;
					}
				}
				flag = (phaseCheckMode == GameplayMutators.ActionPhaseCheckMode.Movement);
				if (!flag)
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
					if (phaseCheckMode == GameplayMutators.ActionPhaseCheckMode.Default)
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
						ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
						if (currentActionPhase != ActionBufferPhase.Movement)
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
							if (currentActionPhase != ActionBufferPhase.MovementChase)
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
								if (currentActionPhase != ActionBufferPhase.AbilitiesWait && currentActionPhase != ActionBufferPhase.MovementWait)
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
									if (currentActionPhase != ActionBufferPhase.Done)
									{
										goto IL_11D;
									}
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
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
										if (GameFlowData.Get().gameState != GameState.EndingTurn)
										{
											goto IL_11D;
										}
									}
								}
							}
						}
						flag = true;
					}
				}
				IL_11D:;
			}
			else
			{
				flag = true;
			}
		}
		return flag;
	}

	private static bool IsActiveInCurrentTurn(int currentTurn, int onDuration, int restDuration, int startOffset)
	{
		if (startOffset > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsActiveInCurrentTurn(int, int, int, int)).MethodHandle;
			}
			if (currentTurn <= startOffset)
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
				return false;
			}
		}
		if (onDuration <= 0)
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
			return true;
		}
		int num = currentTurn;
		if (startOffset > 0)
		{
			num -= startOffset;
		}
		int num2 = restDuration + onDuration;
		int num3 = (num - 1) % num2;
		if (num3 < onDuration)
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
			return true;
		}
		return false;
	}

	public static bool IsOnStartOfIntervalCycle(int currentTurn, int onDuration, int restDuration, int startOffset)
	{
		if (startOffset > 0 && currentTurn <= startOffset)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.IsOnStartOfIntervalCycle(int, int, int, int)).MethodHandle;
			}
			return false;
		}
		if (onDuration <= 0)
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
			return false;
		}
		int num = currentTurn;
		if (startOffset > 0)
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
			num -= startOffset;
		}
		int num2 = restDuration + onDuration;
		int num3 = (num - 1) % num2;
		return num3 == 0;
	}

	public static float GetPassiveEnergyRegenMultiplier()
	{
		if (GameplayMutators.s_instance != null)
		{
			return GameplayMutators.s_instance.m_passiveEnergyRegenMultiplier;
		}
		return 1f;
	}

	public static float GetPassiveHpRegenMultiplier()
	{
		if (GameplayMutators.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayMutators.GetPassiveHpRegenMultiplier()).MethodHandle;
			}
			return GameplayMutators.s_instance.m_passiveHpRegenMultiplier;
		}
		return 1f;
	}

	[Serializable]
	public struct StatusInterval
	{
		public StatusType m_statusType;

		public int m_duration;

		public int m_interval;

		public int m_startOffset;

		public bool m_delayTillStartOfMovement;

		public string m_activateNotificationTurnBefore;

		public string m_offNotificationTurnBefore;
	}

	[Serializable]
	public class IntervalDef
	{
		public int m_onDuration;

		public int m_restDuration;

		public int m_startOffset;

		public bool m_delayTillStartOfMovement;
	}

	public enum ActionPhaseCheckMode
	{
		Default,
		Abilities,
		Movement,
		Any
	}
}
