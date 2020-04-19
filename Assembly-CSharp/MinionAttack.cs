using System;

public class MinionAttack : Ability
{
	public int m_damageVsMinions = 0x32;

	public int m_damageVsPlayers = 0x32;

	public int m_damageVsOthers = 0x32;

	private void Start()
	{
		this.m_abilityName = "Minion Cannon";
	}
}
