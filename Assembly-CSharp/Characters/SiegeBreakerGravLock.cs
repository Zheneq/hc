public class SiegeBreakerGravLock : Ability
{
	private void Start()
	{
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			m_abilityName = "Grav Lock";
			return;
		}
	}
}
