// ROGUES
// SERVER
using UnityEngine;

// identical in reactor and rogues
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
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_moveDroneAbility = abilityData.GetAbilityOfType(typeof(TrackerDrone)) as TrackerDrone;
			m_basicAttackAbility = abilityData.GetAbilityOfType(typeof(TrackerHuntingCrossbow)) as TrackerHuntingCrossbow;
			m_escapeAbility = abilityData.GetAbilityOfType(typeof(TrackerFlewTheCoop)) as TrackerFlewTheCoop;
			m_prisonAbility = abilityData.GetAbilityOfType(typeof(TrackerTeslaPrison)) as TrackerTeslaPrison;
		}
	}

	public int GetDamageOnTracked(bool isMovingAbility)
	{
		int damage = m_droneHitDamageAmount;
		if (m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null)
		{
			AbilityMod_TrackerDrone droneMod = m_moveDroneAbility.GetDroneMod();
			damage = droneMod.m_trackedHitDamageMod.GetModifiedValue(m_droneHitDamageAmount);
			if (isMovingAbility && droneMod.m_extraDamageWhenMovingOnTracked > 0)
			{
				damage += droneMod.m_extraDamageWhenMovingOnTracked;
			}
		}
		if (m_escapeAbility != null
			&& m_escapeLastTurnCast > 0
			&& GameFlowData.Get().CurrentTurn - m_escapeLastTurnCast < m_escapeAbility.GetModdedExtraDroneDamageDuration())
		{
			damage += m_escapeAbility.GetModdedExtraDroneDamage();
		}
		return damage;
	}

	public int GetDamageOnUntracked(bool isMovingAbility)
	{
		int damage = m_untrackedDroneHitDamageAmount;
		if (m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null)
		{
			AbilityMod_TrackerDrone droneMod = m_moveDroneAbility.GetDroneMod();
			damage = m_moveDroneAbility.GetDroneMod().m_untrackedHitDamageMod.GetModifiedValue(m_untrackedDroneHitDamageAmount);
			if (isMovingAbility && droneMod.m_extraDamageWhenMovingOnUntracked > 0)
			{
				damage += droneMod.m_extraDamageWhenMovingOnUntracked;
			}
		}
		if (m_escapeAbility != null
			&& m_escapeLastTurnCast > 0
			&& GameFlowData.Get().CurrentTurn - m_escapeLastTurnCast < m_escapeAbility.GetModdedExtraDroneDamageDuration())
		{
			damage += m_escapeAbility.GetModdedExtraDroneUntrackedDamage();
		}
		return damage;
	}

	public bool CanHitInvisibleActors()
	{
		return m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null
			? m_moveDroneAbility.GetDroneMod().m_hitInvisibleTargetsMod.GetModifiedValue(m_hitInvisibleTargets)
			: m_hitInvisibleTargets;
	}

	public bool UseDirectDamageForDrone()
	{
		return m_droneDamageDirect;
	}

	public bool ShouldAddHuntedEffectFromDrone()
	{
		return m_moveDroneAbility != null
			&& m_moveDroneAbility.GetDroneMod() != null
			&& m_basicAttackAbility != null
			&& m_moveDroneAbility.GetDroneMod().m_applyHuntedEffect;
	}

	public StandardActorEffectData GetHuntedEffectData()
	{
		return m_basicAttackAbility?.GetHuntedEffect();
	}

	public StandardEffectInfo GetTrackedHitEffect()
	{
		return m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null
			? m_moveDroneAbility.GetDroneMod().m_trackedHitEffectOverride.GetModifiedValue(m_droneHitEffect)
			: m_droneHitEffect;
	}

	public StandardEffectInfo GetUntrackedHitEffect()
	{
		return m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null
			? m_moveDroneAbility.GetDroneMod().m_untrackedHitEffectOverride.GetModifiedValue(m_untrackedDroneHitEffect)
			: m_untrackedDroneHitEffect;
	}

	public float GetTargeterMaxRangeFromCaster(bool forUlt)
	{
		float range = m_targeterMaxRangeFromCaster;
		if (forUlt && m_prisonAbility != null && m_prisonAbility.GetUltMod() != null)
		{
			range = m_prisonAbility.GetUltMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(range);
		}
		if (!forUlt && m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null)
		{
			range = m_moveDroneAbility.GetDroneMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(range);
		}
		return range;
	}

	public float GetVisionRadius()
	{
		float radius = m_droneVisionRadius;
		if (m_moveDroneAbility != null && m_moveDroneAbility.GetDroneMod() != null)
		{
			radius = m_moveDroneAbility.GetDroneMod().m_droneVisionRadiusMod.GetModifiedValue(m_droneVisionRadius);
		}
		return radius;
	}
}
