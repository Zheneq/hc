public class SiegeBreakerBigBang : Ability
{
	public int m_damageAmount = 75;

	public int m_knockbackDistance = 8;

	public float m_radius = 1f;

	public bool m_penetrateLineOfSight = true;

	private void Start()
	{
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			m_abilityName = "Big Bang";
			return;
		}
	}
}
