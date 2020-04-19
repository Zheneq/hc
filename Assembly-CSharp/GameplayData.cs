using System;
using UnityEngine;

public class GameplayData : MonoBehaviour
{
	private static GameplayData s_instance;

	public GameplayData.DiagonalMovement m_diagonalMovement;

	public GameplayData.MovementMaximumType m_movementMaximumType;

	public GameplayData.AbilityRangeType m_abilityRangeType = GameplayData.AbilityRangeType.BoardDistToBestSquare;

	public bool m_showTextForPowerUps;

	[Header("-- COVER --")]
	public float m_coverProtectionAngle = 90f;

	public float m_coverProtectionDmgMultiplier = 0.5f;

	public float m_coverMinDistance = 1.5f;

	public Shader m_coverShader;

	[Header("-- RESPAWN MOVEMENT --")]
	public int m_recentlySpawnedBonusMovement;

	public int m_recentlyRespawnedBonusMovement;

	public int m_recentlySpawnedDuration;

	public int m_recentlyRespawnedDuration;

	public float m_gravity = -20f;

	[Header("-- ITEMS and CREDITS --")]
	public int m_itemSlots = 6;

	public int m_creditsPerTurn = 0xA;

	public int m_startingCredits = 0x32;

	public int m_creditsPerPlayerKill = 0x3C;

	public float m_creditBonusFractionPerExtraPlayer = 0.5f;

	public bool m_playerBountyCountsParticipation = true;

	public int m_creditsPerMinionKill = 6;

	public bool m_minionBountyCountsParticipation;

	public bool m_participationlessBountiesGoToTeam = true;

	[Space(10f)]
	public int m_capturePointsPerTurn = 0xA;

	[Header("-- BRUSH and INVISIBILITY--")]
	public float m_distanceCanSeeIntoBrush;

	public int m_brushDisruptionTurns = 2;

	[Space(10f)]
	public bool m_unsuppressInvisibilityOnEndOfPhase = true;

	public float m_proximityBasedInvisibilityMinDistance = 1.5f;

	public bool m_blindEnemyBreaksProximityBasedInvisibility;

	[Header("-- ABILITY ALLOWANCE --")]
	public bool m_recallAllowed = true;

	public bool m_recallOnlyWhenOutOfCombat = true;

	public float m_recallIncomingDamageMultiplier = 2f;

	public int[] m_turnsAbilitiesUnlock;

	public int m_turnCatalystsUnlock;

	public bool m_disableAbilitiesOnRespawn = true;

	public Ability.MovementAdjustment m_movementAllowedOnRespawn = Ability.MovementAdjustment.ReducedMovement;

	[Header("-- HP RESOLVE TIMING --")]
	[Tooltip("If overall damage of a phase is fatal for an actor, and that actor should die and miss subsequent queued actions, set this to true.")]
	public bool m_resolveDamageBetweenAbilityPhases;

	[Tooltip("If evasions deal damage (ala Colin's 'Blitz' phase) AND should cause death before damage actions, set this to true.")]
	public bool m_resolveDamageAfterEvasion;

	public bool m_resolveDamageImmediatelyDuringMovement = true;

	[Space(10f)]
	public bool m_keepTechPointsOnRespawn = true;

	public bool m_enableItemGUI = true;

	public bool m_npcsShowTargeters;

	public bool m_showActorAbilityCooldowns = true;

	[Header("-- CAMERA BOUNDS --")]
	public float m_maximumPositionX = 100f;

	public float m_minimumPositionX = -100f;

	public float m_maximumPositionZ = 100f;

	public float m_minimumPositionZ = -100f;

	private void Awake()
	{
		GameplayData.s_instance = this;
	}

	private void OnDestroy()
	{
		GameplayData.s_instance = null;
	}

	public static GameplayData Get()
	{
		return GameplayData.s_instance;
	}

	public enum DiagonalMovement
	{
		Disabled,
		Enabled
	}

	public enum MovementMaximumType
	{
		CannotExceedMax,
		StopAfterExceeding
	}

	public enum AbilityRangeType
	{
		WorldDistToFreePos,
		BoardDistToBestSquare
	}
}
