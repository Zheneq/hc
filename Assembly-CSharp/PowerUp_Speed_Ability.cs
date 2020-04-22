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
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "ANGER JUICE";
			return;
		}
	}
}
