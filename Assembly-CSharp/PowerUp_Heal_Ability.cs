using System;

public class PowerUp_Heal_Ability : Ability
{
	public int m_healAmount = 0x1E;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp_Heal_Ability.Start()).MethodHandle;
			}
			this.m_abilityName = "Heal PowerUp";
		}
	}
}
