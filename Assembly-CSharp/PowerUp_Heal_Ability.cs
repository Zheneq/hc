using System;

public class PowerUp_Heal_Ability : Ability
{
	public int m_healAmount = 0x1E;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Heal PowerUp";
		}
	}
}
