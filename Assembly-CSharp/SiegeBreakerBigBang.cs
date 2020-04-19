using System;

public class SiegeBreakerBigBang : Ability
{
	public int m_damageAmount = 0x4B;

	public int m_knockbackDistance = 8;

	public float m_radius = 1f;

	public bool m_penetrateLineOfSight = true;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SiegeBreakerBigBang.Start()).MethodHandle;
			}
			this.m_abilityName = "Big Bang";
		}
	}
}
