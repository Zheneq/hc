using System;

public class Laser : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_damageAmount = 0xF;

	public float m_width = 1f;

	public int m_distance = 0xF;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Laser";
		}
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_width, (float)this.m_distance, this.m_penetrateLineOfSight, -1, false, false);
	}
}
