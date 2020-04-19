using System;

public class SiegeBreakerGravLock : Ability
{
	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SiegeBreakerGravLock.Start()).MethodHandle;
			}
			this.m_abilityName = "Grav Lock";
		}
	}
}
