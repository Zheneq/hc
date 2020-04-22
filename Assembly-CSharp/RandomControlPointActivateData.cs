using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomControlPointActivateData
{
	public List<ControlPoint> m_candidatesToConsider;

	public int m_numToActivate;

	[Header("-- Turns Locked Override, ignored if < 0")]
	public int m_turnsLockedOverride = -1;

	public bool m_ignoreIfEverActive = true;
}
