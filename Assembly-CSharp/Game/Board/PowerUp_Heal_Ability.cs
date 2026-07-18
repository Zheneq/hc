public class PowerUp_Heal_Ability : Ability
{
	public int m_healAmount = 30;

	private void Start()
	{
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			m_abilityName = "Heal PowerUp";
			return;
		}
	}
}
