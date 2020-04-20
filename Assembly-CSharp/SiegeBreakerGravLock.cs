using System;

public class SiegeBreakerGravLock : Ability
{
	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Grav Lock";
		}
	}
}
