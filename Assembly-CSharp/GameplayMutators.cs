using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMutators : MonoBehaviour
{
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

	private static GameplayMutators s_instance;

	[Separator("Combat Multipliers", true)]
	public float m_energyGainMultiplier = 1f;

	public IntervalDef m_energyGainMultInterval;

	[Space(10f)]
	public float m_damageMultiplier = 1f;

	public IntervalDef m_damageMultInterval;

	[Space(10f)]
	public float m_healingMultiplier = 1f;

	public IntervalDef m_healingMultInterval;

	[Space(10f)]
	public float m_absorbMultiplier = 1f;

	public IntervalDef m_absorbMultInterval;

	[Separator("Cooldowns", true)]
	public int m_cooldownSpeedAdjustment;

	public int m_cooldownTimeAdjustment;

	public float m_cooldownMultiplier = 1f;

	[Separator("Powerups", true)]
	public int m_powerupRefreshSpeedAdjustment;

	public int m_powerupDurationAdjustment;

	[Separator("Always On Status", true)]
	public List<StatusInterval> m_alwaysOnStatuses;

	[Separator("Status Suppression", true)]
	public List<StatusInterval> m_statusSuppression;

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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private static bool IsMultiplierActive(IntervalDef multInterval)
	{
		if (GameFlowData.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (multInterval != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						int currentTurn = GameFlowData.Get().CurrentTurn;
						return IsActiveInCurrentActionPhase(currentTurn, multInterval.m_onDuration, multInterval.m_restDuration, multInterval.m_startOffset, multInterval.m_delayTillStartOfMovement, ActionPhaseCheckMode.Default);
					}
					}
				}
			}
		}
		return true;
	}

	public static float GetEnergyGainMultiplier()
	{
		if (s_instance != null && IsMultiplierActive(s_instance.m_energyGainMultInterval))
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
					return s_instance.m_energyGainMultiplier;
				}
			}
		}
		return 1f;
	}

	public static float GetDamageMultiplier()
	{
		if (s_instance != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (IsMultiplierActive(s_instance.m_damageMultInterval))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return s_instance.m_damageMultiplier;
					}
				}
			}
		}
		return 1f;
	}

	public static float GetHealingMultiplier()
	{
		if (s_instance != null && IsMultiplierActive(s_instance.m_healingMultInterval))
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
					return s_instance.m_healingMultiplier;
				}
			}
		}
		return 1f;
	}

	public static float GetAbsorbMultiplier()
	{
		if (s_instance != null)
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
			if (IsMultiplierActive(s_instance.m_absorbMultInterval))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return s_instance.m_absorbMultiplier;
					}
				}
			}
		}
		return 1f;
	}

	public static int GetCooldownSpeedAdjustment()
	{
		if (s_instance != null)
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
					return s_instance.m_cooldownSpeedAdjustment;
				}
			}
		}
		return 0;
	}

	public static int GetCooldownTimeAdjustment()
	{
		if (s_instance != null)
		{
			return s_instance.m_cooldownTimeAdjustment;
		}
		return 0;
	}

	public static float GetCooldownMultiplier()
	{
		if (s_instance != null)
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
					return s_instance.m_cooldownMultiplier;
				}
			}
		}
		return 1f;
	}

	public static int GetPowerupRefreshSpeedAdjustment()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return s_instance.m_powerupRefreshSpeedAdjustment;
				}
			}
		}
		return 0;
	}

	public static int GetPowerupDurationAdjustment()
	{
		if (s_instance != null)
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
					return s_instance.m_powerupDurationAdjustment;
				}
			}
		}
		return 0;
	}

	public static bool IsStatusActive(StatusType statusType, int currentTurn, ActionPhaseCheckMode phaseCheckMode = ActionPhaseCheckMode.Default)
	{
		if (s_instance != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (s_instance.m_alwaysOnStatuses != null)
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
				for (int i = 0; i < s_instance.m_alwaysOnStatuses.Count; i++)
				{
					StatusInterval statusInterval = s_instance.m_alwaysOnStatuses[i];
					if (statusInterval.m_statusType != statusType)
					{
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						StatusInterval statusInterval2 = s_instance.m_alwaysOnStatuses[i];
						int startOffset = statusInterval2.m_startOffset;
						StatusInterval statusInterval3 = s_instance.m_alwaysOnStatuses[i];
						int duration = statusInterval3.m_duration;
						StatusInterval statusInterval4 = s_instance.m_alwaysOnStatuses[i];
						int interval = statusInterval4.m_interval;
						StatusInterval statusInterval5 = s_instance.m_alwaysOnStatuses[i];
						bool delayTillStartOfMovement = statusInterval5.m_delayTillStartOfMovement;
						return IsActiveInCurrentActionPhase(currentTurn, duration, interval, startOffset, delayTillStartOfMovement, phaseCheckMode);
					}
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
		}
		return false;
	}

	public static bool IsStatusSuppressed(StatusType statusType, int currentTurn, ActionPhaseCheckMode phaseCheckMode = ActionPhaseCheckMode.Default)
	{
		if (s_instance != null)
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
			if (s_instance.m_statusSuppression != null)
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
				List<StatusInterval> statusSuppression = s_instance.m_statusSuppression;
				for (int i = 0; i < statusSuppression.Count; i++)
				{
					StatusInterval statusInterval = statusSuppression[i];
					if (statusInterval.m_statusType != statusType)
					{
						continue;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						StatusInterval statusInterval2 = statusSuppression[i];
						int startOffset = statusInterval2.m_startOffset;
						int duration = statusInterval2.m_duration;
						int interval = statusInterval2.m_interval;
						bool delayTillStartOfMovement = statusInterval2.m_delayTillStartOfMovement;
						return IsActiveInCurrentActionPhase(currentTurn, duration, interval, startOffset, delayTillStartOfMovement, phaseCheckMode);
					}
				}
				while (true)
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

	public static bool IsActiveInCurrentActionPhase(int currentTurn, int onDuration, int restDuration, int startOffset, bool delayTillMoveStart, ActionPhaseCheckMode phaseCheckMode)
	{
		bool flag = false;
		bool flag2 = IsActiveInCurrentTurn(currentTurn, onDuration, restDuration, startOffset);
		if (flag2 && phaseCheckMode == ActionPhaseCheckMode.Any)
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
			flag = true;
		}
		else if (flag2)
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
			if (delayTillMoveStart)
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
				bool flag3 = IsActiveInCurrentTurn(currentTurn - 1, onDuration, restDuration, startOffset);
				if (!IsOnStartOfIntervalCycle(currentTurn, onDuration, restDuration, startOffset))
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
					if (flag3)
					{
						flag = true;
						goto IL_0121;
					}
				}
				flag = (phaseCheckMode == ActionPhaseCheckMode.Movement);
				if (!flag)
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
					if (phaseCheckMode == ActionPhaseCheckMode.Default)
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
						ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
						if (currentActionPhase != ActionBufferPhase.Movement)
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
							if (currentActionPhase != ActionBufferPhase.MovementChase)
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
								if (currentActionPhase != ActionBufferPhase.AbilitiesWait && currentActionPhase != ActionBufferPhase.MovementWait)
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
									if (currentActionPhase != ActionBufferPhase.Done)
									{
										goto IL_0121;
									}
									while (true)
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
										while (true)
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
											goto IL_0121;
										}
									}
								}
							}
						}
						flag = true;
					}
				}
			}
			else
			{
				flag = true;
			}
		}
		goto IL_0121;
		IL_0121:
		return flag;
	}

	private static bool IsActiveInCurrentTurn(int currentTurn, int onDuration, int restDuration, int startOffset)
	{
		if (startOffset > 0)
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
			if (currentTurn <= startOffset)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (onDuration <= 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
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
		return false;
	}

	public static bool IsOnStartOfIntervalCycle(int currentTurn, int onDuration, int restDuration, int startOffset)
	{
		if (startOffset > 0 && currentTurn <= startOffset)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (onDuration <= 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		int num = currentTurn;
		if (startOffset > 0)
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
			num -= startOffset;
		}
		int num2 = restDuration + onDuration;
		int num3 = (num - 1) % num2;
		return num3 == 0;
	}

	public static float GetPassiveEnergyRegenMultiplier()
	{
		if (s_instance != null)
		{
			return s_instance.m_passiveEnergyRegenMultiplier;
		}
		return 1f;
	}

	public static float GetPassiveHpRegenMultiplier()
	{
		if (s_instance != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return s_instance.m_passiveHpRegenMultiplier;
				}
			}
		}
		return 1f;
	}
}
