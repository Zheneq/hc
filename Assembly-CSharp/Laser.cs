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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Laser.Start()).MethodHandle;
			}
			this.m_abilityName = "Laser";
		}
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_width, (float)this.m_distance, this.m_penetrateLineOfSight, -1, false, false);
	}
}
