using System;
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
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			this.m_moveDroneAbility = (component.GetAbilityOfType(typeof(TrackerDrone)) as TrackerDrone);
			this.m_basicAttackAbility = (component.GetAbilityOfType(typeof(TrackerHuntingCrossbow)) as TrackerHuntingCrossbow);
			this.m_escapeAbility = (component.GetAbilityOfType(typeof(TrackerFlewTheCoop)) as TrackerFlewTheCoop);
			this.m_prisonAbility = (component.GetAbilityOfType(typeof(TrackerTeslaPrison)) as TrackerTeslaPrison);
		}
	}

	public int GetDamageOnTracked(bool isMovingAbility)
	{
		int num = this.m_droneHitDamageAmount;
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				AbilityMod_TrackerDrone droneMod = this.m_moveDroneAbility.GetDroneMod();
				num = droneMod.m_trackedHitDamageMod.GetModifiedValue(this.m_droneHitDamageAmount);
				if (isMovingAbility)
				{
					if (droneMod.m_extraDamageWhenMovingOnTracked > 0)
					{
						num += droneMod.m_extraDamageWhenMovingOnTracked;
					}
				}
			}
		}
		if (this.m_escapeAbility != null && this.m_escapeLastTurnCast > 0)
		{
			if (GameFlowData.Get().CurrentTurn - this.m_escapeLastTurnCast < this.m_escapeAbility.GetModdedExtraDroneDamageDuration())
			{
				num += this.m_escapeAbility.GetModdedExtraDroneDamage();
			}
		}
		return num;
	}

	public int GetDamageOnUntracked(bool isMovingAbility)
	{
		int num = this.m_untrackedDroneHitDamageAmount;
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				AbilityMod_TrackerDrone droneMod = this.m_moveDroneAbility.GetDroneMod();
				num = this.m_moveDroneAbility.GetDroneMod().m_untrackedHitDamageMod.GetModifiedValue(this.m_untrackedDroneHitDamageAmount);
				if (isMovingAbility && droneMod.m_extraDamageWhenMovingOnUntracked > 0)
				{
					num += droneMod.m_extraDamageWhenMovingOnUntracked;
				}
			}
		}
		if (this.m_escapeAbility != null)
		{
			if (this.m_escapeLastTurnCast > 0)
			{
				if (GameFlowData.Get().CurrentTurn - this.m_escapeLastTurnCast < this.m_escapeAbility.GetModdedExtraDroneDamageDuration())
				{
					num += this.m_escapeAbility.GetModdedExtraDroneUntrackedDamage();
				}
			}
		}
		return num;
	}

	public bool CanHitInvisibleActors()
	{
		if (this.m_moveDroneAbility != null && this.m_moveDroneAbility.GetDroneMod() != null)
		{
			return this.m_moveDroneAbility.GetDroneMod().m_hitInvisibleTargetsMod.GetModifiedValue(this.m_hitInvisibleTargets);
		}
		return this.m_hitInvisibleTargets;
	}

	public bool UseDirectDamageForDrone()
	{
		return this.m_droneDamageDirect;
	}

	public bool ShouldAddHuntedEffectFromDrone()
	{
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				if (this.m_basicAttackAbility != null)
				{
					return this.m_moveDroneAbility.GetDroneMod().m_applyHuntedEffect;
				}
			}
		}
		return false;
	}

	public StandardActorEffectData GetHuntedEffectData()
	{
		if (this.m_basicAttackAbility != null)
		{
			return this.m_basicAttackAbility.GetHuntedEffect();
		}
		return null;
	}

	public StandardEffectInfo GetTrackedHitEffect()
	{
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				return this.m_moveDroneAbility.GetDroneMod().m_trackedHitEffectOverride.GetModifiedValue(this.m_droneHitEffect);
			}
		}
		return this.m_droneHitEffect;
	}

	public StandardEffectInfo GetUntrackedHitEffect()
	{
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				return this.m_moveDroneAbility.GetDroneMod().m_untrackedHitEffectOverride.GetModifiedValue(this.m_untrackedDroneHitEffect);
			}
		}
		return this.m_untrackedDroneHitEffect;
	}

	public float GetTargeterMaxRangeFromCaster(bool forUlt)
	{
		float num = this.m_targeterMaxRangeFromCaster;
		if (forUlt)
		{
			if (this.m_prisonAbility != null)
			{
				if (this.m_prisonAbility.GetUltMod() != null)
				{
					num = this.m_prisonAbility.GetUltMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(num);
				}
			}
		}
		if (!forUlt)
		{
			if (this.m_moveDroneAbility != null)
			{
				if (this.m_moveDroneAbility.GetDroneMod() != null)
				{
					num = this.m_moveDroneAbility.GetDroneMod().m_droneTargeterMaxRangeFromCasterMod.GetModifiedValue(num);
				}
			}
		}
		return num;
	}

	public float GetVisionRadius()
	{
		float result = this.m_droneVisionRadius;
		if (this.m_moveDroneAbility != null)
		{
			if (this.m_moveDroneAbility.GetDroneMod() != null)
			{
				result = this.m_moveDroneAbility.GetDroneMod().m_droneVisionRadiusMod.GetModifiedValue(this.m_droneVisionRadius);
			}
		}
		return result;
	}
}
