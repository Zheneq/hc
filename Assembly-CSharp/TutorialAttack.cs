using System;

public class TutorialAttack : Ability
{
	public float m_width = 1f;

	public float m_distance = 15f;

	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 0xA;

	public int m_knockbackDistance = 3;

	public bool m_knockback = true;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_width, this.m_distance, this.m_penetrateLineOfSight, -1, false, false);
	}
}
