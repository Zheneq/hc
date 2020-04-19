using System;

public class PowerUp_Speed_Ability : Ability
{
	public int m_duration = 4;

	public ModType m_movementMod = ModType.BonusAdd;

	public float m_movementValue = 5f;

	public ModType m_outgoingDamageMod = ModType.Multiplier;

	public float m_outgoingDamageValue = 1.5f;

	public ModType m_incomingDamageMod = ModType.Multiplier;

	public float m_incomingDamageValue = 0.5f;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp_Speed_Ability.Start()).MethodHandle;
			}
			this.m_abilityName = "ANGER JUICE";
		}
	}
}
