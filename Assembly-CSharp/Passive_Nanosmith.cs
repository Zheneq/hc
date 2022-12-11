using UnityEngine;

public class Passive_Nanosmith : Passive
{
	[Header("-- Shield Regen")]
	public int m_shieldGainPerTurn;
	public int m_shieldGainLimit = 40;
	[Header("-- Set Shield on Game Start, Limit still applies")]
	public bool m_setShieldOnGameStart = true;
	public bool m_setShieldOnRespawn = true;
	public int m_shieldGainOnGameStart;

	private int m_shieldsApplied;
	private int m_shieldsThatAbsorbedDamage;
}
