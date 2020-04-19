using System;
using UnityEngine;

public class Passive_Exo : Passive
{
	[Header("-- Anim")]
	public int m_anchoredIdleType = 1;

	public int m_unanchoredIdleType;

	[HideInInspector]
	public int m_laserLastCastTurn = -1;

	[HideInInspector]
	public Barrier m_persistingBarrierInstance;

	[HideInInspector]
	public int m_currentConsecutiveSweeps;
}
