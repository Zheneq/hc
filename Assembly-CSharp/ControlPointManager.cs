using System.Collections.Generic;
using UnityEngine;

public class ControlPointManager : MonoBehaviour
{
	[Separator("For Randomly Activating Control Point On Start", true)]
	public List<ControlPoint> m_randControlPointsToConsider;
	public int m_numRandControlPointsToActivate;

	[Header("-- Turns Locked Override, ignored if < 0")]
	public int m_turnsLockedOverride = -1;

	[Separator("For Randomly Activating Control Point after X turns of no capturing", true)]
	public RandomControlPointActivateData m_stalemateAlleviateActivations;
	public int m_stalemateAlleviateTurns;
}
