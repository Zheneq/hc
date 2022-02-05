// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// TODO LOW all member variables removed in rogues, some code might have been removed as well
public class ControlPointManager : MonoBehaviour
{
	// added in rogues
	private static ControlPointManager s_instance;

	[Separator("For Randomly Activating Control Point On Start", true)]
	// removed in rogues
	public List<ControlPoint> m_randControlPointsToConsider;
	// removed in rogues
	public int m_numRandControlPointsToActivate;

	[Header("-- Turns Locked Override, ignored if < 0")]
	// removed in rogues
	public int m_turnsLockedOverride = -1;

	[Separator("For Randomly Activating Control Point after X turns of no capturing", true)]
	// removed in rogues
	public RandomControlPointActivateData m_stalemateAlleviateActivations;
	// removed in rogues
	public int m_stalemateAlleviateTurns;

	// added in rogues
#if SERVER
	private void Awake()
	{
		s_instance = this;
	}
#endif

	// added in rogues
#if SERVER
	private void OnDestroy()
	{
		s_instance = null;
	}
#endif

	// added in rogues
#if SERVER
	public static ControlPointManager Get()
	{
		return s_instance;
	}
#endif

	// added in rogues
#if SERVER
	public void OnTurnTick()
	{
		if (NetworkServer.active)
		{
			foreach (ControlPointCoordinator controlPointCoordinator in ControlPointCoordinator.GetCoordinators())
			{
				controlPointCoordinator.OnTurnStart();
			}
		}
	}
#endif
}
