using UnityEngine;

public class Passive_Sensei : Passive
{
	[Header("-- For Ammo/Orb Generation --")]
	public int m_maxOrbs = 10;

	public int m_orbRegenPerTurn = 1;

	public int m_orbPerEnemyHit;

	public int m_orbPerTurnIfHitEnemy;

	public bool m_gainOrbFromOrbAbility;
}
