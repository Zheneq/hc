using UnityEngine;

public class TrackerDroneInfoComponent : MonoBehaviour
{
	[Header("-- Where Drone can be moved to. Used if value > 0")]
	public float m_targeterMaxRangeFromCaster = 10f;

	public float m_targeterMaxRangeFromDrone = -1f;

	[Header("-- For tracked targets ----------------------------")]
	public int m_droneHitDamageAmount = 5;

	public StandardEffectInfo m_droneHitEffect;

	[Header("-- For targets WITHOUT tracked effect -------------")]
	public int m_untrackedDroneHitDamageAmount = 1;

	public StandardEffectInfo m_untrackedDroneHitEffect;

	public bool m_hitUntrackedWhenStationary = true;

	[Header("-- Whether to hit invisible enemies (visibility determined at start of ability/phase)")]
	public bool m_hitInvisibleTargets;

	[Header("-- Whether damage is direct --")]
	public bool m_droneDamageDirect;

	[Header("-- For when drone moves around --")]
	public bool m_droneTravelHitTargets = true;

	public float m_travelTargeterLineRadius = 2f;

	public float m_travelTargeterEndRadius = 2f;

	public VisionProviderInfo.BrushRevealType m_brushRevealType;

	public bool m_targetingIgnoreLos = true;

	[Header("-- For when drone is placed, monitoring an area--")]
	public float m_droneVisionRadius = 2f;

	public AbilityAreaShape m_droneMonitorShape = AbilityAreaShape.Three_x_Three;

	public int m_droneMonitorStartDelay = 1;

	public AbilityPriority m_droneMonitorPhase = AbilityPriority.Combat_Damage;

	[Header("-- For sequences --")]
	public GameObject m_droneMoveSequence;

	public GameObject m_droneUltimateMoveSequence;

	public GameObject m_missileSequencePrefab;

	public GameObject m_droneRadiusSequencePrefab;

	private TrackerDrone m_moveDroneAbility;

	private TrackerHuntingCrossbow m_basicAttackAbility;

	private TrackerFlewTheCoop m_escapeAbility;

	private TrackerTeslaPrison m_prisonAbility;

	public int m_escapeLastTurnCast = -1;

	private void Start()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			m_moveDroneAbility = (component.GetAbilityOfType(typeof(TrackerDrone)) as TrackerDrone);
			m_basicAttackAbility = (component.GetAbilityOfType(typeof(TrackerHuntingCrossbow)) as TrackerHuntingCrossbow);
			m_escapeAbility = (component.GetAbilityOfType(typeof(TrackerFlewTheCoop)) as TrackerFlewTheCoop);
			m_prisonAbility = (component.GetAbilityOfType(typeof(TrackerTeslaPrison)) as TrackerTeslaPrison);
			return;
		}
	}

	public int GetDamageOnTracked(bool isMovingAbility)
	{
		int num = m_droneHitDamageAmount;
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				AbilityMod_TrackerDrone droneMod = m_moveDroneAbility.GetDroneMod();
				num = droneMod.m_trackedHitDamageMod.GetModifiedValue(m_droneHitDamageAmount);
				if (isMovingAbility)
				{
					if (droneMod.m_extraDamageWhenMovingOnTracked > 0)
					{
						num += droneMod.m_extraDamageWhenMovingOnTracked;
					}
				}
			}
		}
		if (m_escapeAbility != null && m_escapeLastTurnCast > 0)
		{
			if (GameFlowData.Get().CurrentTurn - m_escapeLastTurnCast < m_escapeAbility.GetModdedExtraDroneDamageDuration())
			{
				num += m_escapeAbility.GetModdedExtraDroneDamage();
			}
		}
		return num;
	}

	public int GetDamageOnUntracked(bool isMovingAbility)
	{
		int num = m_untrackedDroneHitDamageAmount;
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				AbilityMod_TrackerDrone droneMod = m_moveDroneAbility.GetDroneMod();
				num = m_moveDroneAbility.GetDroneMod().m_untrackedHitDamageMod.GetModifiedValue(m_untrackedDroneHitDamageAmount);
				if (isMovingAbility && droneMod.m_extraDamageWhenMovingOnUntracked > 0)
				{
					num += droneMod.m_extraDamageWhenMovingOnUntracked;
				}
			}
		}
		if (m_escapeAbility != null)
		{
			if (m_escapeLastTurnCast > 0)
			{
				if (GameFlowData.Get().CurrentTurn - m_escapeLastTurnCast < m_escapeAbility.GetModdedExtraDroneDamageDuration())
				{
					num += m_escapeAbility.GetModdedExtraDroneUntrackedDamage();
				}
			}
		}
		return num;
	}

	public bool CanHitInvisibleActors()
	{
		if (m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_moveDroneAbility.GetDroneMod().m_hitInvisibleTargetsMod.GetModifiedValue(m_hitInvisibleTargets);
				}
			}
		}
		return m_hitInvisibleTargets;
	}

	public bool UseDirectDamageForDrone()
	{
		return m_droneDamageDirect;
	}

	public bool ShouldAddHuntedEffectFromDrone()
	{
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				if (m_basicAttackAbility != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return m_moveDroneAbility.GetDroneMod().m_applyHuntedEffect;
						}
					}
				}
			}
		}
		return false;
	}

	public StandardActorEffectData GetHuntedEffectData()
	{
		if (m_basicAttackAbility != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_basicAttackAbility.GetHuntedEffect();
				}
			}
		}
		return null;
	}

	public StandardEffectInfo GetTrackedHitEffect()
	{
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_moveDroneAbility.GetDroneMod().m_trackedHitEffectOverride.GetModifiedValue(m_droneHitEffect);
					}
				}
			}
		}
		return m_droneHitEffect;
	}

	public StandardEffectInfo GetUntrackedHitEffect()
	{
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_moveDroneAbility.GetDroneMod().m_untrackedHitEffectOverride.GetModifiedValue(m_untrackedDroneHitEffect);
					}
				}
			}
		}
		return m_untrackedDroneHitEffect;
	}

	public float GetTargeterMaxRangeFromCaster(bool forUlt)
	{
		float num = m_targeterMaxRangeFromCaster;
		if (forUlt)
		{
			if (m_prisonAbility != null)
			{
				if (m_prisonAbility.GetUltMod() != null)
				{
					num = m_prisonAbility.GetUltMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(num);
				}
			}
		}
		if (!forUlt)
		{
			if (m_moveDroneAbility != null)
			{
				if (m_moveDroneAbility.GetDroneMod() != null)
				{
					num = m_moveDroneAbility.GetDroneMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(num);
				}
			}
		}
		return num;
	}

	public float GetVisionRadius()
	{
		float result = m_droneVisionRadius;
		if (m_moveDroneAbility != null)
		{
			if (m_moveDroneAbility.GetDroneMod() != null)
			{
				result = m_moveDroneAbility.GetDroneMod().m_droneVisionRadiusMod.GetModifiedValue(m_droneVisionRadius);
			}
		}
		return result;
	}
}
